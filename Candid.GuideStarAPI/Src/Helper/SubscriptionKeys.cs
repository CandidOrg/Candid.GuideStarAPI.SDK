using System;
using System.Collections.Generic;

namespace Candid.GuideStarAPI
{
  public sealed class SubscriptionKeys : Dictionary<Domain, SubscriptionKey>
  {
    internal SubscriptionKeys()
    {
    }

    public new void Add(Domain domain, SubscriptionKey key)
    {
      if (ContainsKey(domain))
        throw new Exception($"Subscription list already contains value for domain '${domain.ToString()}'");
      else
        base.Add(domain, key);
    }

    public void Add(Domain domain, string primary, string secondary = null)
    {
      Add(domain, new SubscriptionKey(primary, secondary));
    }

    public bool IsEmpty()
    {
      return (Count == 0);
    }
  }

  public sealed class SubscriptionKey
  {
    private const int SubscriptionLength = 32;

    public SubscriptionKey(string primaryKey, string secondaryKey = null)
    {
      if ((primaryKey?.Length != SubscriptionLength) ||
        (!string.IsNullOrEmpty(secondaryKey) && secondaryKey?.Length != SubscriptionLength))
      {
        throw new Exception($"Entered SubscriptionKey of incorrect length. Keys should be {SubscriptionLength} characters long");
      }

      Primary = primaryKey;
      Secondary = secondaryKey;
    }

    public string Primary { get; private set; }
    public string Secondary { get; private set; }
  }
}