namespace ScienceLogic.Em7.Api.Common;

public class Reference<T>
{
	public Type ItemType => typeof(T);
	public string? Uri { get; set; }
	public string? Description { get; set; }
}
