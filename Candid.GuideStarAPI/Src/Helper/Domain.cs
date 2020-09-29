using Candid.GuideStarAPI.Types;

namespace Candid.GuideStarAPI
{
  public sealed class Domain : StringEnum
  {
    private Domain(string value) : base(value) { }

    public static implicit operator Domain(string value)
    {
      return new Domain(value);
    }

    public static readonly Domain PremierV3 = "premier/v3";
    public static readonly Domain CharityCheckV1 = "charitycheck/v1";
    public static readonly Domain EssentialsV2 = "essentials/v2";
    public static readonly Domain Lookup = "essentials/lookup";
  }
}
