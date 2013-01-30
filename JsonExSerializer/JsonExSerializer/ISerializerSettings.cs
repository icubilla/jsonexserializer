﻿using System;
using System.Collections.Generic;
using JsonExSerializer.Framework.ExpressionHandlers;
using JsonExSerializer.MetaData;
using System.Collections;
using JsonExSerializer.Framework.Parsing;
using JsonExSerializer.Collections;
using JsonExSerializer.TypeConversion;
namespace JsonExSerializer
{
    public interface ISerializerSettings
    {
        #region "Options"
        bool IsCompact { get; set; }
        bool OutputTypeInformation { get; set; }
        void SetJsonStrictOptions();
        ReferenceOption ReferenceWritingType { get; set; }
        IgnoredPropertyOption IgnoredPropertyAction { get; set; }

        /// <summary>
        /// Controls the action taken during deserialization when a property is specified in the Json Text,
        /// but does not exist on the class or object.
        /// </summary>
        MissingPropertyOptions MissingPropertyAction { get; set; }
        #endregion

        #region "Parsing"
        bool IsReferenceableType(Type objectType);
        ExpressionHandlerCollection ExpressionHandlers { get; set; }
        IList<IParsingStage> ParsingStages { get; }
        #endregion

        #region "Default Values"
        DefaultValueCollection DefaultValues { get; set; }
        DefaultValueOption DefaultValueSetting { get; set; }
        DefaultValueOption GetEffectiveDefaultValueSetting();
        #endregion

        IDictionary Parameters { get; }

        #region Metadata
        List<CollectionHandler> CollectionHandlers { get; }
        void RegisterCollectionHandler(CollectionHandler handler);
        TypeAliasCollection TypeAliases { get; set; }
        ITypeSettings Types { get; set; }
        #endregion

    }
}
