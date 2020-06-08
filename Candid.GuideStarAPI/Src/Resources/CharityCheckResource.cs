using System.Threading.Tasks;

namespace Candid.GuideStarAPI.Resources
{
  public class CharityCheckResource : AbstractResource
  {
    public static string GetCharityCheck(string ein)
    {
      var EIN = new EIN(ein);

      var response = Get(BuildGetRequest(EIN, Domain.CharityCheckV1));

      return response;
    }

    public static async Task<string> GetOrganizationAsync(string ein)
    {
      var EIN = new EIN(ein);

      var response = await GetAsync(BuildGetRequest(EIN, Domain.CharityCheckV1));

      return response;
    }
  }
}
