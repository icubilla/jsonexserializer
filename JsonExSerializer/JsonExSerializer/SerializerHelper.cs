using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace JsonExSerializer
{
    public class SerializerHelper
    {
        private Type _serializedType;
        private SerializationContext _context;
        private TextWriter _writer;
        private const string indentLevel = "   ";

        internal SerializerHelper(Type t, SerializationContext context, TextWriter writer)
        {
            _serializedType = t;
            _context = context;
            _writer = writer;
        }

        public void Serialize(object o)
        {
            string indent = "";
            if (o != null && _context.OutputTypeComment)
            {
                _writer.WriteLine("/*");
                _writer.WriteLine("  Created by JsonExSerializer");
                _writer.WriteLine("  Assembly: " + o.GetType().Assembly.ToString());
                _writer.WriteLine("  Type: " + o.GetType().FullName);
                _writer.WriteLine("*/");
            }
            if (o != null && _context.OutputTypeInformation && o.GetType() != _serializedType)
            {
                WriteCast(o.GetType());
            }
            Serialize(o, indent);
            
        }

        public void Serialize(object o, string indent)
        {
            if (o == null)
            {
                _writer.Write("null");
            }
            else
            {
                switch (Type.GetTypeCode(o.GetType()))
                {
                    case TypeCode.Char:
                        WriteChar((char)o, indent);
                        break;
                    case TypeCode.String:
                        WriteString((string)o, indent);
                        break;
                    case TypeCode.Double:
                        WriteDouble((double) o, indent);
                        break;
                    case TypeCode.Single:
                        WriteFloat((float)o, indent);
                        break;
                    case TypeCode.Object:
                        if (o is ICollection || o.GetType().IsArray)
                        {
                            SerializeCollection(o, indent);
                        }
                        else
                        {
                            SerializeObject(o, indent);
                        }
                        break;
                    case TypeCode.DateTime:
                        WriteDateTime((DateTime)o, indent);
                        break;
                    case TypeCode.Boolean:
                        WriteBoolean((bool)o, indent);
                        break;
                    case TypeCode.Byte:
                        WriteByte((byte)o, indent);
                        break;
                    case TypeCode.DBNull:
                        WriteDBNull((DBNull)o, indent);
                        break;
                    case TypeCode.Empty:
                        throw new Exception("Unsupported value (Empty): " + o);
                    case TypeCode.Int16:
                        WriteInt16((short)o, indent);
                        break;
                    case TypeCode.Int32:
                        WriteInt32((int)o, indent);
                        break;
                    case TypeCode.Int64:
                        WriteInt64((long)o, indent);
                        break;
                    case TypeCode.SByte:
                        WriteSByte((sbyte)o, indent);
                        break;

                    case TypeCode.UInt16:
                        WriteUInt16((ushort)o, indent);
                        break;
                    case TypeCode.UInt32:
                        WriteUInt32((uint)o, indent);
                        break;
                    case TypeCode.UInt64:
                        WriteUInt64((ulong)o, indent);
                        break;
                    case TypeCode.Decimal:
                        WriteDecimal((decimal)o, indent);
                        break;
                    default:
                        _writer.Write(Convert.ToString(o));
                        break;
                }
            }
        }

        private void SerializeEnum(object o, string indent)
        {
            _writer.Write(Enum.Format(o.GetType(), o, "d"));
        }

        private void SerializeObject(object o, string indent)
        {
            TypeHandler handler = TypeHandler.GetHandler(o.GetType());
            
            bool addComma = false;
            _writer.Write('{');
            // indent if not in compact mode
            string subindent = "";
            if (!_context.IsCompact)
            {
                _writer.Write('\n');
                subindent = indent + indentLevel;
            }
            foreach (TypeHandlerProperty prop in handler.Properties)
            {
                if (addComma)
                {
                    _writer.Write(", ");
                    if (!_context.IsCompact) _writer.Write(Environment.NewLine);
                }
                _writer.Write(subindent);
                Serialize(prop.Name, subindent);
                _writer.Write(":");
                object value = prop.GetValue(o);
                if (value != null && _context.OutputTypeInformation && value.GetType() != prop.PropertyType)
                {
                    WriteCast(value.GetType());
                }
                Serialize(value, subindent);
                addComma = true;
            }
            if (!_context.IsCompact)
            {
                _writer.Write(Environment.NewLine);
                _writer.Write(indent);
            }
            _writer.Write('}');
        }

        private void SerializeCollection(object o, string indent)
        {
            TypeHandler handler = TypeHandler.GetHandler(o.GetType());

            bool outputTypeInfo = _context.OutputTypeInformation;
            Type elemType = handler.GetElementType();
            

            bool addComma = false;
            _writer.Write('[');
            // indent if not in compact mode
            string subindent = "";
            if (!_context.IsCompact)
            {
                _writer.Write('\n');
                subindent = indent + indentLevel;
            }
            foreach (object value in (IEnumerable) o)
            {
                if (addComma)
                {
                    _writer.Write(", ");
                    if (!_context.IsCompact) _writer.Write(Environment.NewLine);
                }
                _writer.Write(subindent);
                if (outputTypeInfo && value.GetType() != elemType)
                {
                    WriteCast(value.GetType());
                }
                Serialize(value, subindent);
                addComma = true;
            }
            if (!_context.IsCompact)
            {
                _writer.Write(Environment.NewLine);
                _writer.Write(indent);
            }
            _writer.Write(']');
        }

        protected void WriteDateTime(DateTime value, string indent)
        {
            _writer.Write(value.ToString());
        }
        protected void WriteBoolean(bool value, string indent)
        {
            _writer.Write(value);
        }
        protected void WriteByte(byte value, string indent)
        {
            _writer.Write(value);
        }
        protected void WriteDBNull(DBNull value, string indent)
        {
            _writer.Write(value);
        }
        protected void WriteInt16(short value, string indent)
        {
            _writer.Write(value);
        }
        protected void WriteInt32(int value, string indent)
        {
            _writer.Write(value);
        }
        protected void WriteInt64(long value, string indent)
        {
            _writer.Write(value);
        }
        protected void WriteSByte(sbyte value, string indent)
        {
            _writer.Write(value);
        }
        protected void WriteUInt16(ushort value, string indent)
        {
            _writer.Write(value);
        }
        protected void WriteUInt32(uint value, string indent)
        {
            _writer.Write(value);
        }
        protected void WriteUInt64(ulong value, string indent)
        {
            _writer.Write(value);
        }
        protected void WriteDecimal(decimal value, string indent)
        {
            _writer.Write(value);
        }

        protected void WriteDouble(double value, string indent)
        {
            _writer.Write(value.ToString("R"));
        }

        protected void WriteFloat(float value, string indent)
        {
            _writer.Write(value.ToString("R"));
        }

        protected void WriteChar(char value, string indent)
        {
            _writer.Write('"');
            _writer.Write(EscapeString(value.ToString()));
            _writer.Write('"');
        }

        protected void WriteString(string value, string indent)
        {
            _writer.Write('"');
            _writer.Write(EscapeString(value));
            _writer.Write('"');
        }

        private void WriteCast(Type t)
        {
            if (t != typeof(string)) {
                _writer.Write('(');
                //TODO: Write simple type name for primitive types 
                // such as "int" instead of "System.Int32"
                WriteTypeInfo(t);
                _writer.Write(')');
            }
        }

        /// <summary>
        /// Writes out the type for an object in regular C# code syntax
        /// </summary>
        /// <param name="t">the type to write</param>
        private void WriteTypeInfo(Type t)
        {
            string alias = _context.GetTypeAlias(t);
            if (alias != null) {
                _writer.Write(alias);
            }
            else if (t.IsArray)
            {
                WriteTypeInfo(t.GetElementType());
                _writer.Write("[]");
            }
            else if (t.IsGenericType)
            {
                string genericName = _context.GetTypeAlias(t.GetGenericTypeDefinition());
                if (genericName == null)
                {
                    genericName = t.GetGenericTypeDefinition().FullName;
                    genericName = genericName.Substring(0, genericName.LastIndexOf('`'));
                }
                _writer.Write(genericName);
                _writer.Write('<');
                bool writeComma = false;
                foreach (Type genArgType in t.GetGenericArguments())
                {
                    if (writeComma)
                        _writer.Write(',');
                    writeComma = true;
                    WriteTypeInfo(genArgType);
                }
                _writer.Write('>');
            }
            else
            {
                _writer.Write(t.FullName);
            }
        }

        private string EscapeString(string s)
        {
            return s.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\t", "\\t").Replace("\"", "\\\"");
        }

    }
}
