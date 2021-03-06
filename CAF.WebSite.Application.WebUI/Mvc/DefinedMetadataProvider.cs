﻿using CAF.Infrastructure.Core;
using CAF.Infrastructure.Core.Exceptions;
using CAF.WebSite.Application.WebUI.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace CAF.WebSite.Application.WebUI
{
    /// <summary>
    /// 自定义
    /// This MetadataProvider adds some functionality on top of the default DataAnnotationsModelMetadataProvider.
    /// It adds custom attributes (implementing IModelAttribute) to the AdditionalValues property of the model's metadata
    /// so that it can be retrieved later.
    /// </summary>
    public class DefinedMetadataProvider : CachedDataAnnotationsModelMetadataProvider
    {

        protected override CachedDataAnnotationsModelMetadata CreateMetadataPrototype(IEnumerable<Attribute> attributes, Type containerType, Type modelType, string propertyName)
        {
            var metadata = base.CreateMetadataPrototype(attributes, containerType, modelType, propertyName);
            var attrs = attributes.OfType<IModelAttribute>().ToList();

            foreach (var attr in attrs)
            {
                if (metadata.AdditionalValues.ContainsKey(attr.Name))
                {
                    throw new WorkException("There is already an attribute with the name of '{0}' on this model.".FormatCurrent(attr.Name));
                }
                metadata.AdditionalValues.Add(attr.Name, attr);
            }

            return metadata;
        }

        protected override CachedDataAnnotationsModelMetadata CreateMetadataFromPrototype(CachedDataAnnotationsModelMetadata prototype, Func<object> modelAccessor)
        {
            var result = base.CreateMetadataFromPrototype(prototype, modelAccessor);

            if (prototype.AdditionalValues.Count > 0)
            {
                var attrs = prototype.AdditionalValues.Where(x => x.Value is IModelAttribute).ToArray();
                if (attrs.Any())
                {
                    result.AdditionalValues.AddRange(attrs);
                }
            }

            return result;
        }
    }
}