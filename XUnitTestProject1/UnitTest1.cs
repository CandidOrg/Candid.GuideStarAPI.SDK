using Candid.GuideStarAPI;
using Candid.GuideStarApiTest;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;

namespace Candid.GuideStarAPITest
{
  public class GuideStarClientTest
  {
    private IConfiguration _config;
    private static string CHARITY_CHECK_KEY;
    private static string CHARITY_CHECK_PDF_KEY;
    private static string ESSENTIALS_KEY;
    private static string PREMIER_KEY;
    private static string GUIDESTAR_EIN = "";

    public GuideStarClientTest()
    {
      _config = ConfigLoader.InitConfiguration();
      CHARITY_CHECK_KEY = _config["Keys:CHARITY_CHECK_KEY"];
      CHARITY_CHECK_PDF_KEY = _config["Keys:CHARITY_CHECK_PDF_KEY"];
      ESSENTIALS_KEY = _config["Keys:ESSENTIALS_KEY"];
      PREMIER_KEY = _config["Keys:PREMIER_KEY"];
    }
    [Fact]
    public void GuideStarApiClientInitWorks()
    {
      GuideStarClient.Init(PREMIER_KEY);

      Assert.NotNull(GuideStarClient.GetSubscriptionKey());
      Assert.NotNull(GuideStarClient.GetSubscriptionKey().SubscriptionString);
    }

    [Fact]
    public void GuideStarApiClientInitThrowsException()
    {
      Assert.Throws<Exception>(() => GuideStarClient.Init(""));
      Assert.Throws<Exception>(() => GuideStarClient.Init(subscriptionKey: null));
      Assert.Throws<Exception>(() => GuideStarClient.Init(PREMIER_KEY + " "));
    }

    [Fact]
    public void GuideStarApiClientGetRestClientWorks()
    {
      GuideStarClient.Init(PREMIER_KEY);

      Assert.NotNull(GuideStarClient.GetRestClient());
    }

    [Fact]
    public void GuideStarPremierResourceWorks()
    {
      GuideStarClient.Init(PREMIER_KEY);
      var premier = PremierResource.GetOrganization("13-1837418");

      Assert.NotNull(premier);
    }

    [Fact]
    public void GuideStarCharityCheckResourceWorks()
    {
      GuideStarClient.Init(CHARITY_CHECK_KEY);
      var charitycheck = CharityCheckResource.GetCharityCheck("13-1837418");

      Assert.NotNull(charitycheck);
    }
  }
}
