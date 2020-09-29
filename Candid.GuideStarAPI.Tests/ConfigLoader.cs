using Microsoft.Extensions.Configuration;

namespace Candid.GuideStarApiTest
{
  public static class ConfigLoader
  {
    public static IConfiguration InitConfiguration()
    {
      //This will load appsettings.test.json first, 
      //then override with values from User Secrets, if they exists.
      var config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.test.json")
          .AddUserSecrets<Keys>()
          .Build();
      return config;
    }
  }
}
