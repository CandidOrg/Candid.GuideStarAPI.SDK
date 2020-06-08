using System;

namespace Candid.GuideStarAPI
{
  public sealed class EIN
  {
    public readonly string EinString;
    private int EinLength = 10;
    public EIN(string ein)
    {
      if (ein?.Length != EinLength)
      {
        throw new Exception($"Entered SubscriptionKey of incorrect length. Keys should be {EinLength} characters long");
      }

      EinString = ein;
    }
  }
}
