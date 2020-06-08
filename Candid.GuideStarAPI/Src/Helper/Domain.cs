using Candid.GuideStarAPI.Types;

namespace Candid.GuideStarAPI
{
  public sealed class Domain : StringEnum
  {
    private Domain(string value) : base(value) { }
    public Domain() { }
    public static implicit operator Domain(string value)
    {
      return new Domain(value);
    }

    public static readonly Domain PremierV3 = new Domain("premier/v3");
    public static readonly Domain CharityCheckV1 = new Domain("charitycheck/v1");
    public static readonly Domain EssentialsV2 = new Domain("essentials/v2");
  }
}
