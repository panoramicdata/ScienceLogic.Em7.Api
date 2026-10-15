using ScienceLogic.Em7.Api.Common;
using Xunit;

namespace ScienceLogic.Em7.Api.Test;

public sealed class ReferenceTests
{
	[Fact]
	public void ItemTypeIsTheGenericParameter()
	{
		var reference = new Reference<string>();

		Assert.Equal(typeof(string), reference.ItemType);
	}

	[Fact]
	public void UriAndDescriptionRoundTrip()
	{
		var reference = new Reference<IdentifiedItem> { Uri = "https://em7.example.com/api/devices/1", Description = "A device" };

		Assert.Equal("https://em7.example.com/api/devices/1", reference.Uri);
		Assert.Equal("A device", reference.Description);
	}

	[Fact]
	public void UriAndDescriptionDefaultToNull()
	{
		var reference = new Reference<IdentifiedItem>();

		Assert.Null(reference.Uri);
		Assert.Null(reference.Description);
	}
}
