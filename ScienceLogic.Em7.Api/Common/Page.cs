using System;
using System.Collections.Generic;
using System.Text;

namespace ScienceLogic.Em7.Api.Common;

public class Page<T> where T : IdentifiedItem
{
	public List<T> Items { get; set; } = [];

	public uint Skip { get; set; }

	public uint Take { get; set; }

	public uint TotalCount { get; set; }
}
