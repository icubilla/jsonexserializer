using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace JsonExSerializer.Collections
{

    /// <summary>
    /// Collection handler for a non-generic System.Collections.Stack
    /// class.
    /// </summary>
    public class GenericStackHandler : CollectionHandler
    {
        public override bool IsCollection(Type collectionType)
        {
            return collectionType.IsGenericType && typeof(Stack<>).IsAssignableFrom(collectionType.GetGenericTypeDefinition());
        }

        public override ICollectionBuilder ConstructBuilder(Type collectionType, int itemCount)
        {
            Type itemType = GetItemType(collectionType);
            return (ICollectionBuilder) Activator.CreateInstance(typeof(GenericStackBuilder<>).MakeGenericType(itemType));
        }

        public override ICollectionBuilder ConstructBuilder(object collection)
        {
            Type itemType = GetItemType(collection.GetType());
            return (ICollectionBuilder)Activator.CreateInstance(typeof(GenericStackBuilder<>).MakeGenericType(itemType), collection);
        }

        public override Type GetItemType(Type CollectionType)
        {
            return CollectionType.GetGenericArguments()[0];
        }

        public override System.Collections.IEnumerable GetEnumerable(object collection)
        {            
            object[] items = new object[((ICollection)collection).Count];
            ((ICollection)collection).CopyTo(items, 0);
            Array.Reverse(items);
            return items;
        }
    }
}
