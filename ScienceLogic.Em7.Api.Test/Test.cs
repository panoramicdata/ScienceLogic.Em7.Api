using System.IO;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace ScienceLogic.Em7.Api.Test
{
	public abstract class Test
	{
		protected ITestOutputHelper Output { get; }

		protected Test(ITestOutputHelper testOutputHelper)
		{
			Output = testOutputHelper;

			// Read credentials
			var credentials = LoadCredentials("Credentials.json");

			Client = new Client(credentials.Server, credentials.Username, credentials.Password);
		}

		protected Client Client { get; }

		private Credentials LoadCredentials(string credentialsFileName)
		{
			using (var r = new StreamReader(credentialsFileName))
			{
				var json = r.ReadToEnd();
				return JsonConvert.DeserializeObject<Credentials>(json);
			}
		}
	}
}