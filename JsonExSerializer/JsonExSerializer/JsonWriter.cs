using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace JsonExSerializer
{
    public abstract class JsonWriter : IJsonWriter
    {
        protected TextWriter _writer;

        protected enum OpType
        {
            CtorStart,
            CtorEnd,
            ObjStart,
            ObjEnd,
            ArrStart,
            ArrEnd,
            OpKey,
            OpValue,
            OpCast
        }

        protected short indentLevel = 0;
        protected short indentSize = 4;
        private IState _currentState;

        public JsonWriter(TextWriter writer, bool indent) {
            this._writer = writer;
            _currentState = new InitialState(this);
            if (!indent)
                indentSize = 0;
        }

        private void PreWrite(OpType operation) {
            _currentState.PreWrite(operation);
            return;
        }

        protected void WriteIndent()
        {
            if (indentSize > 0)
                _writer.Write("".PadRight(indentSize * indentLevel));
        }

        protected void WriteLineBreak()
        {
            if (indentSize > 0)
                _writer.Write("\r\n");
        }
        public IJsonWriter ConstructorStart(Type constructorType)
        {
            PreWrite(OpType.CtorStart);
            _writer.Write("new ");
            WriteTypeInfo(constructorType);
            _writer.Write("(");
            return this;
        }

        public IJsonWriter ConstructorEnd()
        {
            PreWrite(OpType.CtorEnd);
            _writer.Write(")");
            return this;
        }

        public IJsonWriter ObjectStart()
        {
            PreWrite(OpType.ObjStart);
            _writer.Write('{');
            return this;
        }

        public IJsonWriter Key(string key)
        {
            PreWrite(OpType.OpKey);
            WriteQuotedString(key);
            _writer.Write(':');
            return this;
        }

        public IJsonWriter ObjectEnd()
        {
            PreWrite(OpType.ObjEnd);
            _writer.Write('}');
            return this;
        }

        public IJsonWriter ArrayStart()
        {
            PreWrite(OpType.ArrStart);
            _writer.Write('[');
            return this;
        }

        public IJsonWriter ArrayEnd()
        {
            PreWrite(OpType.ArrEnd);
            _writer.Write(']');
            return this;
        }

        public IJsonWriter Value(bool value)
        {
            PreWrite(OpType.OpValue);
            _writer.Write(value);
            return this;
        }

        public IJsonWriter Value(long value)
        {
            PreWrite(OpType.OpValue);
            _writer.Write(value);
            return this;
        }

        public IJsonWriter Value(double value)
        {
            PreWrite(OpType.OpValue);
            _writer.Write(value.ToString("R"));
            return this;
        }

        public IJsonWriter Value(float value)
        {
            PreWrite(OpType.OpValue);
            _writer.Write(value.ToString("R"));
            return this;
        }

        public IJsonWriter QuotedValue(string value)
        {
            PreWrite(OpType.OpValue);
            WriteQuotedString(value);
            return this;
        }

        protected void WriteQuotedString(string value)
        {
            _writer.Write('"');
            _writer.Write(EscapeString(value));
            _writer.Write('"');
        }

        private static string EscapeString(string s)
        {
            return s.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\t", "\\t").Replace("\"", "\\\"");
        }

        /// <summary>
        /// Writes out the type for an object in regular C# code syntax
        /// </summary>
        /// <param name="t">the type to write</param>
        protected abstract void WriteTypeInfo(Type t);

        public IJsonWriter Cast(Type castedType)
        {
            if (castedType != typeof(string))
            {
                PreWrite(OpType.OpCast);
                _writer.Write('(');
                WriteTypeInfo(castedType);
                _writer.Write(')');
            }
            return this;
        }

        public abstract IJsonWriter WriteObject(object value);


        public void Dispose()
        {
            _writer.Dispose();
        }

        private interface IState
        {
            JsonWriter Outer { get; set; }

            /// <summary>
            /// Reference to the previous state, this should be
            /// set when a new state is created
            /// </summary>
            IState PreviousState { get; set; }

            /// <summary>
            /// Called before a write operation occurs
            /// </summary>
            /// <param name="operation"></param>
            void PreWrite(OpType operation);

            /// <summary>
            /// Called when control is returned back to a prior state. 
            /// The current state implementing the transition should pass itself
            /// as the "other" state.
            /// </summary>
            /// <param name="otherState">the state that is returning control back to the previous state</param>
            /// <param name="operation">the current operation that is causing control to return</param>
            void ReturnFrom(IState otherState, OpType operation);
        }

        /// <summary>
        /// Base class for states, implements helper functions for transitions
        /// </summary>
        private class StateBase : IState
        {
            protected IState _previousState;
            protected JsonWriter _outer;
            protected bool needComma;

            public StateBase(JsonWriter outer) {
                this._outer = outer;
                needComma = false;
            }

            public StateBase() : this(null) {
            }

            public virtual JsonWriter Outer {
                get { return _outer; }
                set { _outer = value; }
            }

            public virtual IState PreviousState
            {
                get { return _previousState; }
                set { _previousState = value; }
            }

            public virtual void PreWrite(OpType operation)
            {
                switch (operation)
                {
                    case OpType.ArrStart:
                        NewState(new ArrayState());
                        break;
                    case OpType.CtorStart:
                        NewState(new CtorState());
                        break;
                    case OpType.ObjStart:
                        NewState(new ObjectState());
                        break;
                    case OpType.OpCast:
                        Current(operation);
                        needComma = false;
                        break;
                    default:
                        InvalidState(operation);
                        break;
                }
            }

            public virtual void ReturnFrom(IState otherState, OpType operation)
            {

            }

            protected void InvalidState(OpType operation)
            {
                throw new InvalidOperationException(string.Format("Invalid operation {0} for current state {1}", operation, this.GetType().Name));
            }

            protected virtual void ReturnToPrevious(OpType operation) {
                Outer.indentLevel--;
                if (!(this is KeyState) && operation != OpType.OpValue)
                {
                    Outer.WriteLineBreak();
                    Outer.WriteIndent();
                }

                Outer._currentState = PreviousState;
                if (PreviousState == null)
                    throw new InvalidOperationException("Attempt to return to previous state when there is no previous state");

                PreviousState.ReturnFrom(this, operation);
            }

            protected virtual void NewState(IState newState) {
                if (needComma == true)
                {
                    Outer._writer.Write(", ");
                }
                if (!(this is InitialState) && !(this is KeyState))
                {
                    Outer.WriteLineBreak();
                    Outer.WriteIndent();
                }
                if (!(this is KeyState))
                    Outer.indentLevel++;

                newState.PreviousState = this;
                newState.Outer = Outer;
                Outer._currentState = newState;
            }

            protected virtual void Current(OpType operation)
            {
                if (needComma == true)
                {
                    Outer._writer.Write(", ");
                }
                Outer.WriteLineBreak();
                Outer.WriteIndent();
                needComma = true;
            }
        }

        private class InitialState : StateBase
        {
            public InitialState(JsonWriter outer) : base(outer) {
            }


            public override void PreWrite(OpType operation)
            {
                switch (operation)
                {
                    case OpType.ArrStart:
                        NewState(new ArrayState());
                        break;
                    case OpType.CtorStart:
                        NewState(new CtorState());
                        break;
                    case OpType.ObjStart:
                        NewState(new ObjectState());
                        break;
                    case OpType.OpCast:
                        // do nothing
                        break;
                    case OpType.OpValue:
                        NewState(new DoneState());
                        break;
                    default:
                        InvalidState(operation);
                        break;
                }
            }

            public override void ReturnFrom(IState otherState, OpType operation)
            {
                // only one expression can be written then we're done
                // so never return to initial state
                Outer._currentState = new DoneState();
                Outer._currentState.Outer = Outer;
            }
        }

        private class ArrayState : StateBase {

            public override void PreWrite(OpType operation)
            {
                switch (operation)
                {
                    case OpType.ArrStart:
                    case OpType.CtorStart:
                    case OpType.ObjStart:
                    case OpType.OpCast:
                        base.PreWrite(operation);
                        break;
                    case OpType.OpValue:
                        Current(operation);
                        break;
                    case OpType.ArrEnd:
                        ReturnToPrevious(operation);
                        break;
                    default:
                        InvalidState(operation);
                        break;
                }
            }
        }

        private class CtorState : StateBase {
            public override void PreWrite(OpType operation)
            {
                switch (operation)
                {
                    case OpType.CtorEnd:
                        ReturnToPrevious(operation);
                        break;
                    default:
                        InvalidState(operation);
                        break;
                }
            }
        }

        private class ObjectState : StateBase {
            public override void PreWrite(OpType operation)
            {
                switch (operation)
                {
                    case OpType.ObjEnd:
                        ReturnToPrevious(operation);
                        break;
                    case OpType.OpKey:
                        NewState(new KeyState());
                        needComma = true;
                        break;
                    default:
                        InvalidState(operation);
                        break;
                }
            }
        }

        private class DoneState : StateBase {
            public override void PreWrite(OpType operation)
            {
                InvalidState(operation);
            }
        }

        private class KeyState : StateBase {
            public override void PreWrite(OpType operation)
            {
                switch (operation)
                {
                    case OpType.OpValue:
                        ReturnToPrevious(operation);
                        break;
                    default:
                        base.PreWrite(operation);
                        break;
                }
            }
            public override void ReturnFrom(IState otherState, OpType operation)
            {
                Outer._currentState = PreviousState;
            }
        }
    }
}
