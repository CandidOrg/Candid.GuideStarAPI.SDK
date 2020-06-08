using System;

namespace Candid.GuideStarAPI
{
  public sealed class SubscriptionKeys
  {
    public readonly SubscriptionKey EssentialsKey;
    public readonly SubscriptionKey PremierKey;
    public readonly SubscriptionKey CharityCheckKey;

    internal bool IsEmpty()
    {
      return EssentialsKey == null && PremierKey == null && CharityCheckKey == null;
    }
  }

  public sealed class SubscriptionKey
  {
    public readonly string SubscriptionString;
    private int SubscriptionLength = 32;
    public SubscriptionKey(string key)
    {
      if (key?.Length != SubscriptionLength)
      {
        throw new Exception($"Entered SubscriptionKey of incorrect length. Keys should be {SubscriptionLength} characters long");
      }

      SubscriptionString = key;
    }
  }
}