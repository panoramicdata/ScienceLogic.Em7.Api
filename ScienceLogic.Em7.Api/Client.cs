using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ScienceLogic.Em7.Api.Common;
using ScienceLogic.Em7.Api.Exceptions;
using ScienceLogic.Em7.Api.Extensions;

namespace ScienceLogic.Em7.Api;

public sealed class Client : IDisposable
{
	private static readonly JsonSerializerOptions SerializerOptions = new() { PropertyNameCaseInsensitive = true };
	private readonly HttpClient _httpClient;

	public Client(string server, string username, string password)
		: this(server, username, password, new HttpClientHandler())
	{
	}

	public Client(string server, string username, string password, HttpMessageHandler handler)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(server);
		ArgumentException.ThrowIfNullOrWhiteSpace(username);
		ArgumentNullException.ThrowIfNull(password);
		ArgumentNullException.ThrowIfNull(handler);

		var authority = server.Contains("://", StringComparison.Ordinal) ? server : $"https://{server}";
		_httpClient = new HttpClient(handler)
		{
			BaseAddress = new Uri($"{authority.TrimEnd('/')}/api/")
		};
		var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
		_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	}

	public async Task<Page<T>> GetPage<T>(SkipTakeQuery<T> query, CancellationToken cancellationToken = default)
		where T : IdentifiedItem
	{
		ArgumentNullException.ThrowIfNull(query);
		var items = await GetResult<List<T>>(query.SubUri, cancellationToken).ConfigureAwait(false);
		return new Page<T> { Skip = query.Skip, Take = query.Take, Items = items };
	}

	public Task<T> Get<T>(GetQuery<T> query, CancellationToken cancellationToken = default) where T : IdentifiedItem
	{
		ArgumentNullException.ThrowIfNull(query);
		return GetResult<T>(query.SubUri, cancellationToken);
	}

	public Task<List<T>> Get<T>(UnpagedQuery<T> query, CancellationToken cancellationToken = default) where T : IdentifiedItem
	{
		ArgumentNullException.ThrowIfNull(query);
		return GetResult<List<T>>(query.SubUri, cancellationToken);
	}

	public Task<T> Get<T>(CancellationToken cancellationToken = default) where T : UnidentifiedItem
		=> GetResult<T>(AttributeExtensions.GetPath<T>(), cancellationToken);

	public void Dispose() => _httpClient.Dispose();

	private async Task<T> GetResult<T>(string requestUri, CancellationToken cancellationToken)
	{
		using var response = await _httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
		var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
		if (response.StatusCode != HttpStatusCode.OK)
		{
			throw new ApiException(response.StatusCode, $"{requestUri} resulted in response:\n{content}");
		}

		try
		{
			return JsonSerializer.Deserialize<T>(content, SerializerOptions)
				?? throw new JsonException($"The response could not be converted to {typeof(T).Name}.");
		}
		catch (JsonException exception)
		{
			throw new ApiException(response.StatusCode, $"{exception.Message} when converting the response to {typeof(T).Name}.", exception);
		}
	}
}
