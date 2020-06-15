using System.Net.Http;
using System.Threading.Tasks;

namespace Candid.GuideStarAPI.Resources
{
  /// <summary>
  /// Parent class for all resources
  /// </summary>
  public abstract class AbstractResource
  {
    /// <summary>
    /// A synchronous document retrieval request
    /// </summary>
    /// <param name="request">Resource specific request</param>
    /// <returns>JSON document</returns>
    protected static string Get(Request request)
    {
      var client = GuideStarClient.GetRestClient();
      var response = client.Request(request);

      return response.Content;
    }

    protected static async Task<string> GetAsync(Request request)
    {
      var client = GuideStarClient.GetRestClient();
      var response = await client.RequestAsync(request);

      return response.Content;
    }

    protected static Request BuildGetRequest(EIN ein, Domain domain)
    {
      return new Request(
        HttpMethod.Get,
        GuideStarClient.GetSubscriptionKey(),
        domain,
        queryParam: ein.EinString
      );
    }
  }
}
