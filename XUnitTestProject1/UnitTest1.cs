using System;
using Candid.GuideStarAPI;
using Xunit;

namespace Candid.GuideStarAPITest
{
  public class GuideStarClientTest
  {
    private static string CHARITY_CHECK_KEY = "f393e46952b54efc93b40166828b2804";
    private static string CHARITY_CHECK_PDF_KEY = "4e1ba3f796fd45dda8443cf00d11c5ec";
    private static string ESSENTIALS_KEY = "53e4525092684ecc95e6b62361ffcce7";
    private static string PREMIER_KEY = "0866b5c31c444dd1b950c27db7859fa1";
    private static string GUIDESTAR_EIN = "";

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
