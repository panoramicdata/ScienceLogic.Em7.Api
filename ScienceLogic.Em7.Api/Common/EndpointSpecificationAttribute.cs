using System;
using System.Collections.Generic;

namespace ScienceLogic.Em7.Api.Common
{
	internal class EndpointSpecificationAttribute : Attribute
	{
		public string Path { get; set; }
		public string Verbs { get; set; }

		internal List<Verb> VerbList => Verbs.Split(',').Select(v => (Verb)Enum.Parse(typeof(Verb), v)).ToList();
	}
}