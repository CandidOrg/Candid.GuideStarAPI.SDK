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
      var client = GuideStarClient.GetRestClient(request.SubscriptionKey);
      var response = client.Request(request);

      return response.Content;
    }

    protected static async Task<string> GetAsync(Request request)
    {
      var client = GuideStarClient.GetRestClient(request.SubscriptionKey);
      var response = await client.RequestAsync(request);

      return response.Content;
    }

    protected static SubscriptionKey GetSubscriptionKey(Domain domain)
    {
      return GuideStarClient.SubscriptionKeys.ContainsKey(domain)
        ? GuideStarClient.SubscriptionKeys[domain]
        : GuideStarClient.GetDefaultSubscriptionKey();
    }

    protected static Request BuildGetRequest(EIN ein, Domain domain)
    {
      return new Request(
        HttpMethod.Get,
        GetSubscriptionKey(domain),
        domain,
        queryParam: ein.EinString
      );
    }

    protected static Request BuildPostRequest(Domain domain, string payload)
    {
      return new Request(
        HttpMethod.Post,
        GetSubscriptionKey(domain),
        domain,
        postParams: payload
      );
    }
  }
}
