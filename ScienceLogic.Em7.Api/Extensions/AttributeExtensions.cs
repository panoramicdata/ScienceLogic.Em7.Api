using System;
using System.Collections.Generic;
using System.Linq;
using ScienceLogic.Em7.Api.Common;

namespace ScienceLogic.Em7.Api.Extensions
{
	internal static class AttributeExtensions
	{
		public static List<Verb> GetVerbs<T>() where T : IdentifiedItem
		{
			var dnAttribute = typeof(T).GetCustomAttributes(
				typeof(EndpointSpecificationAttribute), true
			).FirstOrDefault() as EndpointSpecificationAttribute;
			return dnAttribute?.VerbList;
		}

		public static string GetPath<T>()
		{
			if (!(typeof(T).GetCustomAttributes(
				typeof(EndpointSpecificationAttribute), true
			).FirstOrDefault() is EndpointSpecificationAttribute dnAttribute))
			{
				throw new Exception($"No Path attribute set for class {typeof(T).Name}");
			}
			var nameSpaceLastText = typeof(T).Namespace.Split('.').Last().ToLowerInvariant();
			return $"{nameSpaceLastText}/{dnAttribute.Path}";
		}
	}
}