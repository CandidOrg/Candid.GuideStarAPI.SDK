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
      var essentials = EssentialsResource.GetOrganization(payload);
      var result = JsonDocument.Parse(essentials);
      result.RootElement.TryGetProperty("code", out var response);
      Assert.True(response.TryGetInt32(out int code));
      Assert.True(code == 200);

      Assert.NotNull(essentials);
    }

    [Fact]
    public void SortbyRelevenceAscending()
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
    public void SortbyRelevenceDescending()
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

    [Fact]
    public void SortbyOrganizationNameAscending()
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Sort(sortBuilder =>
          sortBuilder.SortBy(SortOptions.OrganizationName)
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
    public void SortbyOrganizationNameDescending()
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Sort(sortBuilder =>
          sortBuilder.SortBy(SortOptions.OrganizationName)
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

    [Fact]
    public void SortbyBmfAssetsAscending()
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Sort(sortBuilder =>
          sortBuilder.SortBy(SortOptions.BmfAssets)
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
    public void SortbyBmfAssetsDescending()
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Sort(sortBuilder =>
          sortBuilder.SortBy(SortOptions.BmfAssets)
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

    [Fact]
    public void SortbyBmfGrossReceiptsAscending()
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Sort(sortBuilder =>
          sortBuilder.SortBy(SortOptions.BmfGrossReceipts)
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
    public void SortbyBmfGrossReceiptsDescending()
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Sort(sortBuilder =>
          sortBuilder.SortBy(SortOptions.BmfGrossReceipts)
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

    //Candid.GuideStarAPI.Geography
    [Fact]
    public void 
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
