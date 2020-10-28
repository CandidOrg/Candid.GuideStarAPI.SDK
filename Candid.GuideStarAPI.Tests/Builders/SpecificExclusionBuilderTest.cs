using Candid.GuideStarAPI.Resources;
using Candid.GuideStarApiTest;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Candid.GuideStarAPI.Tests.Builders
{
  [Collection("API Tests Collection")]
  public class SpecificExclusionBuilderTest
  {
    private readonly IConfiguration _config;
    private static string CHARITY_CHECK_KEY;
    private static string ESSENTIALS_KEY;
    private static string PREMIER_KEY;

    public SpecificExclusionBuilderTest()
    {
      _config = ConfigLoader.InitConfiguration();
      CHARITY_CHECK_KEY = _config["Keys:CHARITY_CHECK_KEY"];
      ESSENTIALS_KEY = _config["Keys:ESSENTIALS_KEY"];
      PREMIER_KEY = _config["Keys:PREMIER_KEY"];

      SetSubscriptionKeys();
    }

    private static void SetSubscriptionKeys()
    {
      // Only do this once
      if (!GuideStarClient.SubscriptionKeys.IsEmpty())
        return;
      if (!string.IsNullOrEmpty(CHARITY_CHECK_KEY))
        GuideStarClient.SubscriptionKeys.Add(Domain.CharityCheckV1, CHARITY_CHECK_KEY);
      if (!string.IsNullOrEmpty(ESSENTIALS_KEY))
        GuideStarClient.SubscriptionKeys.Add(Domain.EssentialsV2, ESSENTIALS_KEY);
      if (!string.IsNullOrEmpty(PREMIER_KEY))
        GuideStarClient.SubscriptionKeys.Add(Domain.PremierV3, PREMIER_KEY);
      if (!string.IsNullOrEmpty(ESSENTIALS_KEY))
        GuideStarClient.SubscriptionKeys.Add(Domain.Lookup, ESSENTIALS_KEY);
    }
    private static void TestPayload(SearchPayload payload)
    {
      var essentials = EssentialsResource.GetOrganization(payload);
      var result = JsonDocument.Parse(essentials);
      result.RootElement.TryGetProperty("code", out var response);
      Assert.True(response.TryGetInt32(out int code));
      Assert.True(code == 200);

      Assert.NotNull(essentials);
    }
    [Fact]
    public void ExcludeDefunctOrMergedOrganizations_Works()
    {
      var payload = SearchPayloadBuilder.Create()
     .Filters(
       filterBuilder => filterBuilder
       .Organization(
           organizationBuilder =>
             organizationBuilder.SpecificExclusions(auditBuilder => auditBuilder.ExcludeDefunctOrMergedOrganizations())
       )
   ).Build();

      //TestPayload(payload);
    }

    [Fact]
    public void ExcludeRevokedOrganizations_Works()
    {

      var payload = SearchPayloadBuilder.Create()
     .Filters(
       filterBuilder => filterBuilder
       .Organization(
           organizationBuilder =>
             organizationBuilder.SpecificExclusions(auditBuilder => auditBuilder.ExcludeRevokedOrganizations())
       )
   ).Build();
      //TestPayload(payload);
    }
  }
}
