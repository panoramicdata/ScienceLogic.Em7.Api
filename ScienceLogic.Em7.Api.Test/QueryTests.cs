using ScienceLogic.Em7.Api.Common;
using Xunit;

namespace ScienceLogic.Em7.Api.Test;

public sealed class QueryTests
{
	private sealed class Device : IdentifiedItem;

	[Fact]
	public void GetQueryStoresSubUriAndResultType()
	{
		var query = new GetQuery<Device>("devices");

		Assert.Equal("devices", query.SubUri);
		Assert.Equal(typeof(Device), query.ResultType);
	}

	[Theory]
	[InlineData("")]
	[InlineData("   ")]
	public void GetQueryRejectsAnEmptySubUri(string subUri)
	{
		Assert.Throws<ArgumentException>(() => new GetQuery<Device>(subUri));
	}

	[Fact]
	public void GetQueryRejectsANullSubUri()
	{
		Assert.Throws<ArgumentNullException>(() => new GetQuery<Device>(null!));
	}

	[Fact]
	public void UnpagedQueryStoresSubUriAndResultType()
	{
		var query = new UnpagedQuery<Device>("devices");

		Assert.Equal("devices", query.SubUri);
		Assert.Equal(typeof(Device), query.ResultType);
	}

	[Fact]
	public void SkipTakeQueryBuildsTheOffsetAndLimitSubUri()
	{
		var query = new SkipTakeQuery<Device>("devices", 10, 25);

		Assert.Equal("devices?offset=10&limit=25", query.SubUri);
		Assert.Equal(10u, query.Skip);
		Assert.Equal(25u, query.Take);
	}
}
