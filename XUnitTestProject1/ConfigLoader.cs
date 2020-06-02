using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Candid.GuideStarApiTest
{
  public static class ConfigLoader
  {
    public static IConfiguration InitConfiguration()
    {
      var config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.test.json")
          .AddUserSecrets("ec841c6b-7acc-49ff-8b88-8339a5a1225a")
          .Build();
      return config;
    }
  }
}
