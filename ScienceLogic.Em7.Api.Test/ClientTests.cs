using System.Net;
using System.Net.Http.Headers;
using System.Text;
using ScienceLogic.Em7.Api.Common;
using ScienceLogic.Em7.Api.Exceptions;
using Xunit;

namespace ScienceLogic.Em7.Api.Test;

public sealed class ClientTests
{
	[Fact]
	public async Task GetPageUsesBasicAuthenticationAndPagination()
	{
		HttpRequestMessage? capturedRequest = null;
		var handler = new StubHandler(request =>
		{
			capturedRequest = request;
			return new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent("[{\"ppGuid\":\"device-1\"}]", Encoding.UTF8, "application/json")
			};
		});
		using var client = new Client("em7.example.com", "user", "secret", handler);

		var page = await client.GetPage(new SkipTakeQuery<TestItem>("devices", 10, 25), TestContext.Current.CancellationToken);

		Assert.Equal((uint)10, page.Skip);
		Assert.Equal((uint)25, page.Take);
		Assert.Equal("device-1", Assert.Single(page.Items).PpGuid);
		Assert.Equal(new Uri("https://em7.example.com/api/devices?offset=10&limit=25"), capturedRequest!.RequestUri);
		Assert.Equal(new AuthenticationHeaderValue("Basic", "dXNlcjpzZWNyZXQ="), capturedRequest.Headers.Authorization);
	}

	[Fact]
	public async Task GetThrowsApiExceptionForAnUnsuccessfulResponse()
	{
		var handler = new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.NotFound)
		{
			Content = new StringContent("missing")
		});
		using var client = new Client("https://em7.example.com", "user", "secret", handler);

		var exception = await Assert.ThrowsAsync<ApiException>(() => client.Get(new GetQuery<TestItem>("devices/42"), TestContext.Current.CancellationToken));

		Assert.Equal(HttpStatusCode.NotFound, exception.HttpStatusCode);
		Assert.Contains("missing", exception.Message);
	}

	private sealed class TestItem : IdentifiedItem;

	private sealed class StubHandler(Func<HttpRequestMessage, HttpResponseMessage> responseFactory) : HttpMessageHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			return Task.FromResult(responseFactory(request));
		}
	}
}
