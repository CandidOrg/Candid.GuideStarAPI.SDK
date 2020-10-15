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
      // Only do this once
      if (!GuideStarClient.SubscriptionKeys.IsEmpty())
        return;
      if (!string.IsNullOrEmpty(ESSENTIALS_KEY))
        GuideStarClient.SubscriptionKeys.Add(Domain.EssentialsV2, ESSENTIALS_KEY);
    }

    /// <summary>
    /// This test ensures that Audits can be passed in and valid results return from the API.
    /// </summary>
    [Fact]
    public void GuideStarAuditBuilderWorks()
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
      var essentials = EssentialsResource.GetOrganization(payload);
      var result = JsonDocument.Parse(essentials);
      result.RootElement.TryGetProperty("code", out var response);
      Assert.True(response.TryGetInt32(out int code));
      Assert.True(code == 200);

      Assert.NotNull(essentials);
    }

    [Fact]
    public void GuideStarSortbyRelevenceAscendingWorks()
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Sort(sortBuilder => 
          sortBuilder.SortBy(SortOptions.Relevance)
          .SortByAscending()
        )
        .Build();
      var essentials = EssentialsResource.GetOrganization(payload);
      var result = JsonDocument.Parse(essentials);
      result.RootElement.TryGetProperty("code", out var response);
      Assert.True(response.TryGetInt32(out int code));
      Assert.True(code == 200);

      Assert.NotNull(essentials);
    }

    [Fact]
    public void GuideStarSortbyRelevenceDescendingWorks()
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Sort(sortBuilder =>
          sortBuilder.SortBy(SortOptions.Relevance)
          .SortByDescending()
        )
        .Build();
      var essentials = EssentialsResource.GetOrganization(payload);
      var result = JsonDocument.Parse(essentials);
      result.RootElement.TryGetProperty("code", out var response);
      Assert.True(response.TryGetInt32(out int code));
      Assert.True(code == 200);

      Assert.NotNull(essentials);
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
  //Candid.GuideStarAPI.Sort
  //Candid.GuideStarAPI.Specific_Exclusions
  //Candid.GuideStarAPI.ApiConnectionException
  //Candid.GuideStarAPI.Audits


}
