using System;
using System.Text.Json;
using Candid.GuideStarAPI.Resources;
using Candid.GuideStarApiTest;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Candid.GuideStarAPI.Tests
{
  public class GuideStarClientTest
  {
    private static readonly IConfiguration _config = ConfigLoader.InitConfiguration();
    private static string CHARITY_CHECK_KEY;
    private static string ESSENTIALS_KEY;
    private static string PREMIER_KEY;

    /// <summary>
    /// This happens before every test
    /// </summary>
    public GuideStarClientTest()
    {
      CHARITY_CHECK_KEY = _config["Keys:CHARITY_CHECK_KEY"];
      ESSENTIALS_KEY = _config["Keys:ESSENTIALS_KEY"];
      PREMIER_KEY = _config["Keys:PREMIER_KEY"];

      SetSubscriptionKeys();
    }

    private static void SetSubscriptionKeys()
    {
      if (!string.IsNullOrEmpty(CHARITY_CHECK_KEY))
        GuideStarClient.SubscriptionKeys.Add(Domain.CharityCheckV1, CHARITY_CHECK_KEY);
      if (!string.IsNullOrEmpty(ESSENTIALS_KEY))
        GuideStarClient.SubscriptionKeys.Add(Domain.EssentialsV2, ESSENTIALS_KEY);
      if (!string.IsNullOrEmpty(PREMIER_KEY))
        GuideStarClient.SubscriptionKeys.Add(Domain.PremierV3, PREMIER_KEY);
      if (!string.IsNullOrEmpty(ESSENTIALS_KEY))
        GuideStarClient.SubscriptionKeys.Add(Domain.Lookup, ESSENTIALS_KEY);
    }

    [Fact]
    public void GuideStarApiClientInitWorks()
    {
      GuideStarClient.SetDefaultSubscriptionKey(PREMIER_KEY);

      Assert.NotNull(GuideStarClient.GetDefaultSubscriptionKey());
      Assert.NotNull(GuideStarClient.GetDefaultSubscriptionKey().Primary);
    }

    [Fact]
    public void GuideStarApiClientInitThrowsException()
    {
      Assert.Throws<Exception>(() => GuideStarClient.SetDefaultSubscriptionKey(""));
      Assert.Throws<Exception>(() => GuideStarClient.SetDefaultSubscriptionKey(defaultSubscriptionKey: null));
      Assert.Throws<Exception>(() => GuideStarClient.SetDefaultSubscriptionKey(PREMIER_KEY + " "));
    }

    [Fact]
    public void GuideStarApiClientGetRestClientWorks()
    {
      Assert.NotNull(GuideStarClient.GetRestClient());
    }

    [Fact]
    public void GuideStarPremierResourceWorks()
    {
      var premier = PremierResource.GetOrganization("13-1837418");

      var result = JsonDocument.Parse(premier);
      result.RootElement.TryGetProperty("code", out var response);
      Assert.True(response.TryGetInt32(out int code));
      Assert.True(code == 200);

      Assert.NotNull(premier);
    }

    [Fact]
    public void GuideStarCharityCheckResourceWorks()
    {
      var charitycheck = CharityCheckResource.GetOrganization("13-1837418");
      var result = JsonDocument.Parse(charitycheck);
      result.RootElement.TryGetProperty("code", out var response);
      Assert.True(response.TryGetInt32(out int code));
      Assert.True(code == 200);
    }

    [Fact]
    public void GuideStarReInitWorks()
    {
      _ = PremierResource.GetOrganization("13-1837418");
      var charitycheck = CharityCheckResource.GetOrganization("13-1837418");

      var result = JsonDocument.Parse(charitycheck);
      result.RootElement.TryGetProperty("code", out var response);
      Assert.True(response.TryGetInt32(out int code));
      Assert.True(code == 200);

      Assert.NotNull(charitycheck);
      Assert.Contains("charity", charitycheck);
    }

    [Fact]
    public void GuideStarEssentialsCheckResourceWorks()
    {
      // return guidestar as search result.  expecting 200 result
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("guidestar")
        .Filters(
          filterBuilder => filterBuilder
          .Organization(
              organizationBuilder => organizationBuilder.IsOnBMF(true)
                    .SpecificExclusions(
                        seBuilder => seBuilder.ExcludeDefunctOrMergedOrganizations()
                                              .ExcludeRevokedOrganizations()
                      )
                  )
          .Geography(
              g => g.HavingCounty(new string[] { "James City" })
          )
      ).Build();
      var essentials = EssentialsResource.GetOrganization(payload);
      var result = JsonDocument.Parse(essentials);
      result.RootElement.TryGetProperty("code", out var response);
      Assert.True(response.TryGetInt32(out int code));
      Assert.True(code == 200);

      Assert.NotNull(essentials);
    }

    [Fact]
    public void GuideStarClient_SubscriptionKeys_Works()
    {
      GuideStarClient.SubscriptionKeys.Clear();
      SetSubscriptionKeys();
      Assert.NotEmpty(GuideStarClient.SubscriptionKeys);
    }

    [Fact]
    public void GuideStarClient_BadSubscriptionKey_Expect401()
    {
      GuideStarClient.SubscriptionKeys[Domain.PremierV3] = new SubscriptionKey("01234567890123456789012345678901"); // 32

      try
      {
        var premier = PremierResource.GetOrganization("13-1837418");
        Assert.True(false); // Fail - shouldn't have worked
      }
      catch (ApiException ex)
      {
        Assert.NotNull(ex.Response);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, ex.Response.StatusCode);
      }
      catch (Exception)
      {
        Assert.True(false); // Fail - wrong exception type
      }
    }

    [Fact]
    public void GuideStarEssentialsLookupWorks()
    {
      var lookup = EssentialsResource.GetLookup();

      Assert.NotNull(lookup);

      var result = JsonDocument.Parse(lookup);
      result.RootElement.TryGetProperty("data", out var data);
      Assert.True(result.RootElement.TryGetProperty("code", out var code));
      Assert.True(code.GetInt32() == 200);

      Assert.True(data.GetArrayLength() > 0);
    }
  }
}
