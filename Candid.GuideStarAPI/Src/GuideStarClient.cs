using System;

namespace Candid.GuideStarAPI
{
  public static class GuideStarClient
  {
    private static SubscriptionKey _defaultSubscriptionKey = null;

    /// <summary>
    /// Initializes GuideStarClient with default subscription key
    /// Recommended use is to set all subscription keys with GuideStarClient.SubscriptionKeys.Add()
    /// </summary>
    /// <param name="defaultSubscriptionKey">Your API subscription key</param>
    public static void SetDefaultSubscriptionKey(string defaultSubscriptionKey)
    {
      _defaultSubscriptionKey = new SubscriptionKey(defaultSubscriptionKey);
    }

    public static SubscriptionKeys SubscriptionKeys { get; } = new SubscriptionKeys();

    /// <summary>
    /// Get the rest client
    /// </summary>
    /// <returns>The rest client</returns>
    public static RestClient GetRestClient(SubscriptionKey subscriptionKey = null)
    {
      return new RestClient(subscriptionKey ?? _defaultSubscriptionKey);
    }

    public static SubscriptionKey GetDefaultSubscriptionKey()
    {
      return _defaultSubscriptionKey;
    }
  }
}
