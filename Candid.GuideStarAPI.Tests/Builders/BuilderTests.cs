using System;
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
      //TestPayload(payload);
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

    public static IEnumerable<object[]> goodZipcodeData =>
      new List<object[]>
      {
        new object[] { "90210" },
        new object[] { "10001" },
        new object[] { "80014" }
      };

    [Theory]
    [MemberData(nameof(goodZipcodeData))]
    public void GeographyZipCode(string zipcode)
    {
      var payload = SearchPayloadBuilder.Create()
       .WithSearchTerms("test")
       .Filters(filterBuilder =>
         filterBuilder.Geography(geographyBuilder => geographyBuilder.HavingZipCode(zipcode))
       )
       .Build();
      TestPayload(payload);
    }

    public static IEnumerable<object[]> badZipcodeData =>
      new List<object[]>
      {
        new object[] { "902101" },
        new object[] { "9021" },
        new object[] { "" },
        new object[] { null }
      };

    [Theory]
    [MemberData(nameof(badZipcodeData))]
    public void GeographyZipCodeFails(string zipcode)
    {
      Assert.Throws<ArgumentOutOfRangeException>(() => SearchPayloadBuilder.Create()
       .WithSearchTerms("test")
       .Filters(filterBuilder =>
         filterBuilder.Geography(geographyBuilder => geographyBuilder.HavingZipCode(zipcode))
       )
       .Build());
    }

    public static IEnumerable<object[]> goodZipRadius =>
      new List<object[]>
      {
        new object[] { 0 },
        new object[] { 10 },
        new object[] { 50 }
      };

    [Theory]
    [MemberData(nameof(goodZipRadius))]
    public void GeographyWithinZipRadius(int zipRadius)
    {
      var payload = SearchPayloadBuilder.Create()
       .WithSearchTerms("test")
       .Filters(filterBuilder =>
         filterBuilder.Geography(geographyBuilder => geographyBuilder.WithinZipRadius(zipRadius))
       )
       .Build();
      TestPayload(payload);
    }

    public static IEnumerable<object[]> badZipRadius =>
     new List<object[]>
     {
        new object[] { -1 },
        new object[] { 51}
     };

    [Theory]
    [MemberData(nameof(badZipRadius))]
    public void GeographyWithinZipRadiusFails(int zipRadius)
    {
      Assert.Throws<ArgumentOutOfRangeException>(() => SearchPayloadBuilder.Create()
       .WithSearchTerms("test")
       .Filters(filterBuilder =>
         filterBuilder.Geography(geographyBuilder => geographyBuilder.WithinZipRadius(zipRadius))
       )
       .Build());
    }

    [Fact]
    public void PayloadFrom()
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .From(10)
        .Build();
      //TestPayload(payload);
    }

    [Fact]
    public void PayloadFromFails()
    {
      Assert.Throws<Exception>(() => SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .From(-1)
        .Build());
    }

    [Fact]
    public void PayloadTo()
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Size(10)
        .Build();
      //TestPayload(payload);
    }

    [Fact]
    public void PayloadToFails()
    {
      Assert.Throws<Exception>(() => SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Size(-1)
        .Build());
    }

    public static IEnumerable<object[]> goodAffils =>
      new List<object[]>
      {
        new object[] {true, true, false, false},
        new object[] {false, false, true, false},
        new object[] {false, true, false, false},
        new object[] {false, false, false, true}
      };

    [Theory]
    [MemberData(nameof(goodAffils))]
    public void Affiliations(bool headquarters, bool parent, bool subordinate, bool independent)
    {
      var payload = SearchPayloadBuilder.Create().Build();

      //Independent orgs should have no other flags in this category
      if (independent)
      {
        var payloadind = SearchPayloadBuilder.Create()
         .WithSearchTerms("test")
         .Filters(FilterBuilder =>
           FilterBuilder.Organization(OrganizationBuilder =>
             OrganizationBuilder.AffiliationType(AffiliationTypeBuilder => AffiliationTypeBuilder.OnlyIndependent())))
         .Build();
        payload = payloadind;
      }

      //Subordinate orgs should have no other flags in this category
      if (subordinate)
      {
        var payloadsub = SearchPayloadBuilder.Create()
         .WithSearchTerms("test")
         .Filters(FilterBuilder =>
           FilterBuilder.Organization(OrganizationBuilder =>
            OrganizationBuilder.AffiliationType(AffiliationTypeBuilder => AffiliationTypeBuilder.OnlySubordinate())))
         .Build();
        payload = payloadsub;
      }

      //This logic is assuming that all headquarters should be parents but not all parents are headquarters
      if (parent)
      {
        if (headquarters)
        {
          var payloadhq = SearchPayloadBuilder.Create()
            .WithSearchTerms("test")
            .Filters(FilterBuilder =>
            FilterBuilder.Organization(OrganizationBuilder =>
              OrganizationBuilder.AffiliationType(AffiliationTypeBuilder => AffiliationTypeBuilder.OnlyParents())));

          payloadhq.Filters(FilterBuilder =>
            FilterBuilder.Organization(OrganizationBuilder =>
              OrganizationBuilder.AffiliationType(AffiliationTypeBuilder => AffiliationTypeBuilder.OnlyHeadquarters())));

          payload = payloadhq.Build();
        }
        else
        {
          var payloadpar = SearchPayloadBuilder.Create()
            .WithSearchTerms("test")
            .Filters(FilterBuilder =>
              FilterBuilder.Organization(OrganizationBuilder =>
                OrganizationBuilder.AffiliationType(AffiliationTypeBuilder => AffiliationTypeBuilder.OnlyParents())))
            .Build();
          payload = payloadpar;
        }
      }

      //TestPayload(payload);
    }
  }
}
