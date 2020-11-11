using System;
using System.Collections.Concurrent;

namespace Candid.GuideStarAPI
{
  public sealed class SubscriptionKeys : ConcurrentDictionary<Domain, SubscriptionKey>
  {
    internal SubscriptionKeys()
    {
    }

    /// <summary>
    /// Add a subscription key for a given domain.
    /// Implements upsert behavior, that is it will add a value if it doesn't exist and update
    /// an existing value.
    /// Null valued domain and keys will cause an ArgumentNullException
    /// </summary>
    /// <param name="domain">Domain that a value should be associated with</param>
    /// <param name="key">API key for domain value</param>
    public void Add(Domain domain, SubscriptionKey key)
    {
      if (domain is null)
      {
        throw new ArgumentNullException(nameof(domain));
      }
      if (key is null)
      {
        throw new ArgumentNullException(nameof(key));
      }

      base[domain] = key;
    }

    public void Add(Domain domain, string primary, string secondary = null)
    {
      Add(domain, new SubscriptionKey(primary, secondary));
    }

    public new bool IsEmpty()
    {
      return Count == 0;
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

    public string Primary { get; }
    public string Secondary { get; }
  }
}