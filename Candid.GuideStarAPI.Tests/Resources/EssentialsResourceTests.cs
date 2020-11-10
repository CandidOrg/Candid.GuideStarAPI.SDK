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
  public class EssentialsResourceTests
  {
    private readonly IConfiguration _config;
    private static string ESSENTIALS_KEY;

    public EssentialsResourceTests()
    {
      _config = ConfigLoader.InitConfiguration();
      ESSENTIALS_KEY = _config["Keys:ESSENTIALS_KEY"];

      SetSubscriptionKeys();
    }

    private static void SetSubscriptionKeys()
    {
      if (!string.IsNullOrEmpty(ESSENTIALS_KEY))
      {
        GuideStarClient.SubscriptionKeys.Add(Domain.EssentialsV2, ESSENTIALS_KEY);
        GuideStarClient.SubscriptionKeys.Add(Domain.Lookup, ESSENTIALS_KEY);
      }
    }

    [Fact]
    public void GetOrganization_Works()
    {
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
    public async void GetOrganizationAsync_Works()
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
      var essentials = await EssentialsResource.GetOrganizationAsync(payload);
      var result = JsonDocument.Parse(essentials);
      result.RootElement.TryGetProperty("code", out var response);
      Assert.True(response.TryGetInt32(out int code));
      Assert.True(code == 200);

      Assert.NotNull(essentials);
    }
  }
}
