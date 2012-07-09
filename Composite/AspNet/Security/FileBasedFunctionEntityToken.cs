﻿using System;
using Composite.C1Console.Security;

namespace Composite.AspNet.Security
{
    /// <summary>    
    /// </summary>
    /// <exclude />
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)] 
	[SecurityAncestorProvider(typeof(StandardFunctionSecurityAncestorProvider))]
	public class FileBasedFunctionEntityToken : EntityToken
	{
		private readonly string _id;
		public override string Id
		{
			get { return _id; }
		}

        private readonly string _source;
		public override string Source
		{
			get { return _source; }
		}

		public override string Type
		{
			get { return String.Empty; }
		}

        /// <summary>
        /// Gets the name of the function provider.
        /// </summary>
        /// <value>
        /// The name of the function provider.
        /// </value>
        public string FunctionProviderName
        {
            get { return Source; }
        }

        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        /// <value>
        /// The name of the function.
        /// </value>
        public string FunctionName
        {
            get { return Id; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileBasedFunctionEntityToken"/> class.
        /// </summary>
        /// <param name="providerName">Name of the fuction provider.</param>
        /// <param name="functionFullName">Full name of the function.</param>
		public FileBasedFunctionEntityToken(string functionProviderName, string functionFullName)
		{
            _source = functionProviderName;
            _id = functionFullName;
		}


		public override string Serialize()
		{
			return DoSerialize();
		}


        /// <exclude />
		public static EntityToken Deserialize(string serializedEntityToken)
		{
			string type;
			string source;
			string id;

			DoDeserialize(serializedEntityToken, out type, out source, out id);

			return new FileBasedFunctionEntityToken(source, id);
		}
	}
}
