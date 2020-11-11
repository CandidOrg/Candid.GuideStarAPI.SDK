using Candid.GuideStarAPI.Resources;
using Candid.GuideStarApiTest;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Candid.GuideStarAPI.Tests.Resources
{
  [Collection("API Tests Collection")]
  public class PremierResourceTests
  {
    private readonly IConfiguration _config;
    private static string PREMIER_KEY;
    public PremierResourceTests()
    {
      _config = ConfigLoader.InitConfiguration();
      PREMIER_KEY = _config["Keys:PREMIER_KEY"];

      SetSubscriptionKeys();
    }
    private static void SetSubscriptionKeys()
    {
      if (!string.IsNullOrEmpty(PREMIER_KEY))
        GuideStarClient.SubscriptionKeys.Add(Domain.PremierV3, PREMIER_KEY);
    }

    [Fact]
    public void GetOrganization_Works()
    {
      var charitycheck = PremierResource.GetOrganization("13-1837418");
      var result = JsonDocument.Parse(charitycheck);
      result.RootElement.TryGetProperty("code", out var response);
      Assert.True(response.TryGetInt32(out int code));
      Assert.True(code == 200);

      Assert.NotNull(charitycheck);
      Assert.Contains("charity", charitycheck);
    }

    [Fact]
    public async void GetOrganizationAsync_Works()
    {
      var charitycheck = await PremierResource.GetOrganizationAsync("13-1837418");
      var result = JsonDocument.Parse(charitycheck);
      result.RootElement.TryGetProperty("code", out var response);
      Assert.True(response.TryGetInt32(out int code));
      Assert.True(code == 200);

      Assert.NotNull(charitycheck);
      Assert.Contains("charity", charitycheck);
    }
  }
}
