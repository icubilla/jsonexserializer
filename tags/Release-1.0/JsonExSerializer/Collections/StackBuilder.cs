using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace JsonExSerializer.Collections
{

    /// <summary>
    /// Builder for a non-generic System.Collections.Stack
    /// class.
    /// </summary>
    public class StackBuilder : ICollectionBuilder
    {
        protected object _stack;

        public StackBuilder(Type stackType)
        {
            _stack = Activator.CreateInstance(stackType);
        }
        #region ICollectionBuilder Members

        public virtual void Add(object item)
        {
            ((Stack)_stack).Push(item);
        }

        public virtual object GetResult()
        {
            return _stack;
        }

        #endregion
    }
}