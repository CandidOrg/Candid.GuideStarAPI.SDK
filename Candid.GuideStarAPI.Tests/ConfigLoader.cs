using Microsoft.Extensions.Configuration;

namespace Candid.GuideStarApiTest
{
  public static class ConfigLoader
  {
    public static IConfiguration InitConfiguration()
    {
      var config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.test.json")
          .AddUserSecrets<Keys>()
          .Build();
      return config;
    }
  }
}
