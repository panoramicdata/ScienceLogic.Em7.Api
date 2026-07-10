using System;
using System.Collections.Generic;

namespace ScienceLogic.Em7.Api.Common;

internal class EndpointSpecificationAttribute : Attribute
{
	public string? Path { get; set; }
	public string? Verbs { get; set; }

	internal List<Verb> VerbList => (Verbs ?? string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries).Select(v => Enum.Parse<Verb>(v)).ToList();
}
