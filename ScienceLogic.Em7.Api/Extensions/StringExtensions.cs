using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ScienceLogic.Em7.Api.Extensions
{
	internal static class StringExtensions
	{
		public static string Base64Encode(this string plainText) => Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));


		internal static string FormatJson(this string json) => JsonConvert.SerializeObject((dynamic)JsonConvert.DeserializeObject(json), Formatting.Indented);
	}
}

