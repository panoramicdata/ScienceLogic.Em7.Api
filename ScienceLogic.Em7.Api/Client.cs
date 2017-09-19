using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ScienceLogic.Em7.Api.Extensions;
using ScienceLogic.Em7.Api.Exceptions;
using ScienceLogic.Em7.Api.Common;
using Newtonsoft.Json;

namespace ScienceLogic.Em7.Api
{
	public class Client : IDisposable
	{
		private readonly string _server;
		private readonly string _release;
		private readonly string _apiVersion;
		private readonly string _companyId;
		private readonly string _publicKey;
		private readonly HttpClient _httpClient;

		public Client(string server, string release, string apiVersion, string companyId, string publicKey, string privateKey, string appId)
		{
			_server = server;
			_release = release;
			_apiVersion = apiVersion;
			_companyId = companyId;
			_publicKey = publicKey;
			_httpClient = new HttpClient
			{
				BaseAddress = BaseAddress,
				DefaultRequestHeaders =
				{
					Authorization = new AuthenticationHeaderValue("Basic", $"{CompositeUserName}:{privateKey}".Base64Encode()),
				}
			};
			_httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.connectwise.com+json; version=3.0.0");
			_httpClient.DefaultRequestHeaders.Add("Cookie", $"cw-app-id={appId}");

		}

		private Uri BaseAddress => new Uri($"https://{_server}/{_release}/apis/{_apiVersion}/");

		private string CompositeUserName => $"{_companyId}+{_publicKey}";

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_httpClient.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public async Task<Page<T>> GetPage<T>(SkipTakeQuery<T> query) where T : IdentifiedItem
		{
			var response = await _httpClient.GetAsync(query.SubUri);
			if (response.StatusCode != HttpStatusCode.OK)
			{
				throw new ApiException(response.StatusCode, $"{query.SubUri} resulted in response:\n{await response.Content.ReadAsStringAsync()}");
			}
			var responseMessage = await response.Content.ReadAsStringAsync();
			try
			{
				var items = Deserialize<List<T>>(responseMessage);
				return new Page<T>
				{
					Skip = query.Skip,
					Take = query.Take,
					Items = items
				};
			}
			catch (JsonSerializationException exception)
			{
				throw new ApiException(response.StatusCode, $"{exception.Message} when converting the following to List<{typeof(T).Name}>:\n\n{responseMessage.FormatJson()}");
			}
		}

		private static T Deserialize<T>(string responseMessage)
		{
			return JsonConvert.DeserializeObject<T>(
				responseMessage,
				new JsonSerializerSettings
				{
#if DEBUG
					MissingMemberHandling = MissingMemberHandling.Error,
					// ContractResolver = new RequireObjectPropertiesContractResolver(),
#endif
					TypeNameHandling = TypeNameHandling.Auto,
					// Converters = converters
				});
		}

		public async Task<T> Get<T>(GetQuery<T> query) where T : IdentifiedItem
			=> await Result<T>(query.SubUri);

		public async Task<List<T>> Get<T>(UnpagedQuery<T> query) where T : IdentifiedItem
			=> await Result<List<T>>(query.SubUri);

		public async Task<T> Get<T>() where T : UnidentifiedItem
			=> await Result<T>(AttributeExtensions.GetPath<T>());

		private async Task<T> Result<T>(string requestUri)
		{
			var response = await _httpClient.GetAsync(requestUri);
			if (response.StatusCode != HttpStatusCode.OK)
			{
				throw new ApiException(response.StatusCode, $"{requestUri} resulted in response:\n{await response.Content.ReadAsStringAsync()}");
			}
			var responseMessage = await response.Content.ReadAsStringAsync();
			try
			{
				return Deserialize<T>(responseMessage);
			}
			catch (JsonSerializationException exception)
			{
				throw new ApiException(response.StatusCode, $"{exception.Message} when converting the following to {typeof(T).Name}:\n\n{responseMessage.FormatJson()}");
			}
		}
	}
}

