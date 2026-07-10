namespace ScienceLogic.Em7.Api.Common;

public abstract class Query<T>
{
	protected Query(string subUri)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(subUri);
		SubUri = subUri;
	}

	public string SubUri { get; }
}

public sealed class GetQuery<T>(string subUri) : Query<T>(subUri) where T : IdentifiedItem;

public sealed class UnpagedQuery<T>(string subUri) : Query<T>(subUri) where T : IdentifiedItem;

public sealed class SkipTakeQuery<T>(string subUri, uint skip, uint take) : Query<T>($"{subUri}?offset={skip}&limit={take}") where T : IdentifiedItem
{
	public uint Skip { get; } = skip;
	public uint Take { get; } = take;
}

public abstract class UnidentifiedItem;
