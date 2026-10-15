using System.Net;
using ScienceLogic.Em7.Api.Exceptions;
using Xunit;

namespace ScienceLogic.Em7.Api.Test;

public sealed class ApiExceptionTests
{
	[Theory]
	[InlineData(HttpStatusCode.BadRequest, "bad request")]
	[InlineData(HttpStatusCode.NotFound, "not found")]
	[InlineData(HttpStatusCode.InternalServerError, "boom")]
	public void ConstructorStoresStatusCodeAndMessage(HttpStatusCode status, string message)
	{
		var exception = new ApiException(status, message);

		Assert.Equal(status, exception.HttpStatusCode);
		Assert.Equal(message, exception.Message);
	}

	[Fact]
	public void ConstructorWithInnerExceptionKeepsIt()
	{
		Exception inner = new InvalidOperationException("root");
		var exception = new ApiException(HttpStatusCode.ServiceUnavailable, "wrapped", inner);

		Assert.Same(inner, exception.InnerException);
		Assert.Equal(HttpStatusCode.ServiceUnavailable, exception.HttpStatusCode);
	}
}
