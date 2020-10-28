using System.Collections.Generic;
using System.Text.Json;
using Candid.GuideStarAPI.Resources;
using Candid.GuideStarApiTest;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Candid.GuideStarAPI.Tests
{
  public class BuilderTests
  {
    private readonly IConfiguration _config;
    private static string ESSENTIALS_KEY;

    public BuilderTests()
    {
      _config = ConfigLoader.InitConfiguration();
      ESSENTIALS_KEY = _config["Keys:ESSENTIALS_KEY"];

      SetSubscriptionKeys();
    }

    private static void SetSubscriptionKeys()
    {
      if (!string.IsNullOrEmpty(ESSENTIALS_KEY))
        GuideStarClient.SubscriptionKeys.Add(Domain.EssentialsV2, ESSENTIALS_KEY);
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

    /// <summary>
    /// This test ensures that Audits can be passed in and valid results return from the API.
    /// </summary>
    [Fact]
    public void AuditBuilder()
    {
      // return guidestar as search result.  expecting 200 result
      var payload = SearchPayloadBuilder.Create()
        .Filters(
          filterBuilder => filterBuilder
          .Organization(
              organizationBuilder =>
                organizationBuilder.Audits(auditBuilder => auditBuilder.HavingA133Audit())
          )
      ).Build();
      TestPayload(payload);
    }

    public static IEnumerable<object[]> SortPatameters =>
      new List<object[]>
      {
        new object[] { SortOptions.Relevance, true},
        new object[] { SortOptions.Relevance, false},
        new object[] { SortOptions.OrganizationName, true},
        new object[] { SortOptions.OrganizationName, false},
        new object[] { SortOptions.BmfGrossReceipts, true},
        new object[] { SortOptions.BmfGrossReceipts, false},
        new object[] { SortOptions.BmfAssets, true},
        new object[] { SortOptions.BmfAssets, false}
      };

    [Theory]
    [MemberData(nameof(SortPatameters))]
    public void SortBuilderTheory(SortOptions sort, bool sortAscending)
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Sort(sortBuilder =>
        {
          sortBuilder.SortBy(sort);
          if (sortAscending)
            sortBuilder.SortByDescending();
          else
            sortBuilder.SortByDescending();
        }
        )
        .Build();
      TestPayload(payload);
    }

  }

  //Candid.GuideStarAPI.Response
  //Candid.GuideStarAPI.Geography
  //Candid.GuideStarAPI.ApiException
  //Candid.GuideStarAPI.Affiliation_Type
  //Candid.GuideStarAPI.Filters
  //Candid.GuideStarAPI.Financials
  //Candid.GuideStarAPI.Form_Types
  //Candid.GuideStarAPI.Min_Max
  //Candid.GuideStarAPI.Specific_Exclusions
  //Candid.GuideStarAPI.ApiConnectionException
  //Candid.GuideStarAPI.Audits


}
