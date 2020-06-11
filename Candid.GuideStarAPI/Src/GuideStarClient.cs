using System;

namespace Candid.GuideStarAPI
{
  public class GuideStarClient
  {
    private static SubscriptionKeys _subscriptionKeys;
    private static SubscriptionKey _subscriptionKey;
    private static readonly string _baseUrl = "https://apidata.guidestar.org";

    /// <summary>
    /// Initializes GuideStarClient. Required call for GuideStarClient to work.
    /// </summary>
    /// <param name="subscriptionKey">Your API subscription key</param>
    public static void Init(SubscriptionKeys subscriptionKeys)
    {
      if (subscriptionKeys.IsEmpty())
      {
        throw new ArgumentException("SubscriptionKey cannot be null or whitespace");
      }
      _subscriptionKeys = subscriptionKeys;
    }

    /// <summary>
    /// Initializes GuideStarClient. Required call for GuideStarClient to work.
    /// </summary>
    /// <param name="subscriptionKey">Your API subscription key</param>
    public static void Init(string subscriptionKey)
    {
      _subscriptionKey = new SubscriptionKey(subscriptionKey);
    }

    /// <summary>
    /// Get the rest client
    /// </summary>
    /// <returns>The rest client</returns>
    public static RestClient GetRestClient()
    {
      if (_subscriptionKeys?.IsEmpty() ?? false)
      {
        //TODO: Add authentication exception
        throw new Exception(
            "TwilioRestClient was used before AccountSid and AuthToken were set, please call TwilioClient.init()"
        );
      }

      return new RestClient(_subscriptionKey, _baseUrl);
    }

    public static SubscriptionKey GetSubscriptionKey()
    {
      return _subscriptionKey;
    }
  }
}
