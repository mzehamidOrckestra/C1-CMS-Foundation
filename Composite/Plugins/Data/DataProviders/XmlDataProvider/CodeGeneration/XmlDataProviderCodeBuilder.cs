﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Composite.Core.Extensions;
using Composite.Core.Linq;
using Composite.Core.Serialization.CodeGeneration;
using Composite.Core.Types;
using Composite.Data;
using Composite.Data.DynamicTypes;
using Composite.Data.ProcessControlled;
using Composite.Plugins.Data.DataProviders.XmlDataProvider.Foundation;


namespace Composite.Plugins.Data.DataProviders.XmlDataProvider.CodeGeneration
{
    internal class XmlDataProviderCodeBuilder
    {
        private readonly CodeGenerationBuilder _codeGenerationBuilder;
        private readonly string _namespaceName;


        public XmlDataProviderCodeBuilder(string providerName, CodeGenerationBuilder codeGenerationBuilder)
        {
            _codeGenerationBuilder = codeGenerationBuilder;

            _namespaceName = NamesCreator.MakeNamespaceName(providerName);

            AddCodeNamespaces();
        }



        internal void AddDataType(DataTypeDescriptor dataTypeDescriptor)
        {
            Type interfaceType = DataTypeTypesManager.GetDataType(dataTypeDescriptor);
            if (interfaceType == null) return;

            XmlProviderCodeGenerator codeGenerator = new XmlProviderCodeGenerator(dataTypeDescriptor, _namespaceName);
            IEnumerable<CodeTypeDeclaration> codeTypeDeclarations = codeGenerator.CreateCodeDOMs();
            codeTypeDeclarations.ForEach(f => _codeGenerationBuilder.AddType(_namespaceName, f));

            // Property serializer for entity tokens and more
            Dictionary<string, Type> serializerProperties = dataTypeDescriptor.KeyFields.ToDictionary(f => f.Name, f => f.InstanceType);
            PropertySerializerTypeCodeGenerator.AddPropertySerializerTypeCode(_codeGenerationBuilder, codeGenerator.DataIdClassFullName, serializerProperties);
            
            _codeGenerationBuilder.AddReference(interfaceType.Assembly);
        }



        private void AddCodeNamespaces()
        {
            _codeGenerationBuilder.AddReference(typeof(XElement).Assembly);
            _codeGenerationBuilder.AddReference(typeof(Exception).Assembly);
            _codeGenerationBuilder.AddReference(typeof(IQueryable).Assembly);
            _codeGenerationBuilder.AddReference(typeof(XName).Assembly);
            _codeGenerationBuilder.AddReference(typeof(XmlReader).Assembly);
            _codeGenerationBuilder.AddReference(typeof(EditorBrowsableAttribute).Assembly);
            _codeGenerationBuilder.AddReference(typeof(ExpressionCreator).Assembly);
            _codeGenerationBuilder.AddReference(typeof(ExtendedNullable<>).Assembly);
            _codeGenerationBuilder.AddReference(typeof(DataSourceId).Assembly);
            _codeGenerationBuilder.AddReference(typeof(IProcessControlled).Assembly);
            _codeGenerationBuilder.AddReference(typeof(XmlDataProvider).Assembly);
        }
    }
}
