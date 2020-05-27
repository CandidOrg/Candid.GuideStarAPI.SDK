using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Candid.GuideStarAPI
{
  public class GuideStarClient
  {
    // We store our subscription key and base url in the Web.config
    private static string _subscriptionKey;
    private static readonly string _baseUrl = "https://apidata.guidestar.org";

    // Init a static HttpClient with our base url to make calls with
    public static HttpClient client = new HttpClient
    {
      BaseAddress = new Uri(_baseUrl)
    };

    /// <summary>
    /// Initializes GuideStarClient. Required call for GuideStarClient to work.
    /// </summary>
    /// <param name="subscriptionKey">Your API subscription key</param>
    public static void Init(string subscriptionKey)
    {
      if (string.IsNullOrWhiteSpace(subscriptionKey))
      {
        throw new ArgumentException("SubscriptionKey cannot be null or whitespace");
      }
      _subscriptionKey = subscriptionKey;

      //set required header
      if (client.DefaultRequestHeaders.Contains("Subscription-key"))
      {
        client.DefaultRequestHeaders.Remove("Subscription-key");
      }
      client.DefaultRequestHeaders.Add("Subscription-key", subscriptionKey);
    }

    public static IClient GetClient() 
    {

    }

    // Using generics to do GET calls and return JSON deserialized as a model
    public static async Task<T> Get<T>(string endpoint, ICollection<KeyValuePair<String, String>> query = null)
    {
      var responseAsJson = await GetJson(endpoint, query);
      var responseAsModel = JsonConvert.DeserializeObject<T>(responseAsJson);
      // Return that response object for controllers to manipulate
      return responseAsModel;
    }

    public static async Task<T> Post<T>(string endpoint, object request)
    {
      var responseAsJson = await PostJson(endpoint, request);
      
      var responseAsModel = JsonConvert.DeserializeObject<T>(responseAsJson);

      return responseAsModel;
    }

    // If we want just the JSON string, we'll use this request and skip the deserialization process
    public static async Task<string> GetJson(string endpoint, ICollection<KeyValuePair<String, String>> query = null)
    {
      if (query != null)
      {
        foreach (KeyValuePair<string, string> param in query)
        {
          endpoint += "?" + param.Key + "=" + param.Value;
        }
      }

      var response = await client.GetAsync(endpoint);
      var responseAsJson = await response.Content.ReadAsStringAsync();

      return responseAsJson;
    }

    public static async Task<string> PostJson(string endpoint, object request)
    {
      var jsonRequest = JsonConvert.SerializeObject(request);
      var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

      //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
      //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

      var response = await client.PostAsync(endpoint, stringContent);
      var responseAsJson = await response.Content.ReadAsStringAsync();

      return responseAsJson;
    }
  }
}
