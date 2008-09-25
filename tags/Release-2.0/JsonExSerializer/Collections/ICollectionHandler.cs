/*
 * Copyright (c) 2007, Ted Elliott
 * Code licensed under the New BSD License:
 * http://code.google.com/p/jsonexserializer/wiki/License
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace JsonExSerializer.Collections
{
    public interface ICollectionHandler
    {
        /// <summary>
        /// Checks to see if the collection type is handled by this handler.
        /// </summary>
        /// <param name="collectionType">the type to check</param>
        /// <returns>true if this handler can process the collection type</returns>
        bool IsCollection(Type collectionType);

        /// <summary>
        /// Constructs a collection builder for the given collection type.  The
        /// type must be supported by this handler.
        /// </summary>
        /// <param name="collectionType">the type to construct a builder for</param>
        /// <returns>a collection builder</returns>
        ICollectionBuilder ConstructBuilder(Type collectionType, int itemCount);

        /// <summary>
        /// Constructs a collection builder to update an existing collection.  The
        /// type must be supported by this handler.
        /// </summary>
        /// <param name="collection">an existing istance of the collection class to be populated</param>
        /// <param name="collectionType">the type to construct a builder for</param>
        /// <returns>a collection builder</returns>
        ICollectionBuilder ConstructBuilder(object collection);


        /// <summary>
        /// Gets the type of items that this collection type holds
        /// </summary>
        /// <param name="CollectionType">the type of the collection</param>
        /// <returns>the item type</returns>
        Type GetItemType(Type CollectionType);

        /// <summary>
        /// Gets the enumerable property of the collection.  This is normally
        /// just the collection itself, but can be implemented to provide a custom enumerable
        /// </summary>
        /// <param name="collection">the collection</param>
        /// <returns>an IEnumerable object</returns>
        IEnumerable GetEnumerable(object collection);
    }
}