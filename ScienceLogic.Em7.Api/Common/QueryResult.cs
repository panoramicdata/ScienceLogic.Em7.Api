using System.Collections.Generic;

namespace ScienceLogic.Em7.Api.Common
{
	public class QueryResult
	{
		public SearchSpec SearchSpec { get; set; }
		public int Total_Matched { get; set; }
		public int Total_Returned { get; set; }
		public List<Reference<IdentifiedItem>> Result_Set { get; set; }
	}
}