using System.Collections.Generic;
using ScienceLogic.Em7.Api.Common;

namespace ScienceLogic.Em7.Api.DynamicApp
{
	public class DynamicApp : NamedIdentifiedItem
	{
		public string Version { get; set; }
		public string State { get; set; }
		public string Date_Create { get; set; }
		public string Date_Edit { get; set; }
		public string User_Edit { get; set; }
		public string Poll { get; set; }
		public string Disabled_Rollup { get; set; }
		public string Retention { get; set; }
		public object Norm_Retention { get; set; }
		public string Descr { get; set; }
		public string Poll_Alert { get; set; }
		public string Context { get; set; }
		public string Ipp_Flag { get; set; }
		public string Comp_Dev { get; set; 

		public string Class_Type { get; set; }
		public string Cache_Results { get; set; }
		public string Description { get; set; }
		public string Null_Row_Option { get; set; }
		public string Null_Col_Option { get; set; }
		public object Dashboard_Id { get; set; }
		public string Maxdevice_Results { get; set; }
		public Reference<List<CollectionObject>> Collection_Objects { get; set; }
		public Reference<List<CollectionObject>> Presentation_Objects { get; set; }
	}
}