using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;

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


    public static IEnumerable<object[]> goodStatesData =>
      new List<object[]>
      {
        new object[] { new List<string>() { "AK" } },
        new object[] { new List<string>() { "AK", "CA" } },
        new object[] { new List<string>() { "GU", "VI", "PU" } },
        new object[] { new List<string>() { "" } },
        new object[] { new List<string>() { null } },
        new object[] { null }
      };

    [Theory]
    [MemberData(nameof(goodStatesData))]
    public void GeographyHavingState(IEnumerable<string> states)
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Filters(filterBuilder =>
          filterBuilder.Geography(geographyBuilder => geographyBuilder.HavingState(states))
        )
        .Build();
      TestPayload(payload);
    }

    public static IEnumerable<object[]> badStatesData =>
      new List<object[]>
      {
        new object[] { new List<string>() { "ZU" } },
        new object[] { new List<string>() { "KH", "LP" } },
        new object[] { new List<string>() { "AQ", "DF", "QA" } }
      };


    [Theory]
    [MemberData(nameof(badStatesData))]
    public void GeographyHavingStateFails(IEnumerable<string> states)
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Filters(filterBuilder =>
          filterBuilder.Geography(geographyBuilder => geographyBuilder.HavingState(states))
        )
        .Build();
      Assert.Throws<ApiException>(() => EssentialsResource.GetOrganization(payload));
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

    public static IEnumerable<object[]> goodZipCodeAndZipRadius =>
      goodZipcodeData.SelectMany(zipCode => goodZipRadius.Select(zipRadius =>
        new object[] { zipCode[0], zipRadius[0] }
      ));

    [Theory]
    [MemberData(nameof(goodZipCodeAndZipRadius))]
    public void GeographyHavingZipcodeAndWithinZipRadius(string zipcode, int zipradius)
    {
      var payload = SearchPayloadBuilder.Create()
       .WithSearchTerms("test")
       .Filters(filterBuilder =>
         filterBuilder.Geography(geographyBuilder => geographyBuilder.HavingZipCode(zipcode)
          .WithinZipRadius(zipradius))
       )
       .Build();
      TestPayload(payload);
    }

    public static IEnumerable<object[]> goodMSAs =>
      new List<object[]>
      {
        new object[] { new List<string>() { "MD - Wilmington, DE-NJ-MD",  } },
        new object[] { new List<string>() { "TX - Tyler", "TX - Beaumont-Port Arthur" } },
        new object[] { new List<string>() { "AR - Memphis, TN-AR-MS", "IN - Louisville, KY-IN", "MD - Wilmington, DE-NJ-MD"  } },
        new object[] { new List<string>() { "ME - Portland" } },
        new object[] { new List<string>() { "" } },
        new object[] { new List<string>() { } },
        new object[] { null },
        //following values contian a mix of valid and invalid msa
        new object[] { new List<string>() { "TX - Tyler", "TX - Not here texas" } },
        new object[] { new List<string>() { "AR - Memphis, TN-AR-MS", "IN - LouisLouis, KY-IN", "MD - Wilmington, DE-NJ-MD"  } }
      };

    [Theory]
    [MemberData(nameof(goodMSAs))]
    public void GeographyHavingMSA(IEnumerable<string> msas)
    {
      var payload = SearchPayloadBuilder.Create()
       .WithSearchTerms("test")
       .Filters(filterBuilder =>
         filterBuilder.Geography(geographyBuilder => geographyBuilder.HavingMSA(msas))
       )
       .Build();
      TestPayload(payload);
    }

    public static IEnumerable<object[]> badMSAs =>
      new List<object[]>
      {
        new object[] { new List<string>() { "MD - Wilmington, DE-MD",  } },
        new object[] { new List<string>() { "here place" } },
        new object[] { new List<string>() { "multiple", "incorrect", "msa" } }
      };

    [Theory]
    [MemberData(nameof(badMSAs))]
    public void GeographyHavingMSAFails(IEnumerable<string> msas)
    {
      var payload = SearchPayloadBuilder.Create()
       .WithSearchTerms("test")
       .Filters(filterBuilder =>
         filterBuilder.Geography(geographyBuilder => geographyBuilder.HavingMSA(msas))
       )
       .Build();
     Assert.Throws<ApiException>(() => EssentialsResource.GetOrganization(payload));
    }

    //public GeographyBuilder HavingCity(IEnumerable<string> cities)


    //public GeographyBuilder HavingCounty(IEnumerable<string> counties)

    [Fact]
    public void PayloadFrom()
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .From(10)
        .Build();
      TestPayload(payload);
    }

    [Fact]
    public void PayloadFromFails()
    {
      Assert.Throws<Exception>(() => SearchPayloadBuilder.Create().WithSearchTerms("test").From(-1).Build());
     
    }

    [Fact]
    public void PayloadTo()
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Size(10)
        .Build();
      TestPayload(payload);
    }

    [Fact]
    public void PayloadToFails()
    {
      Assert.Throws<Exception>(() => SearchPayloadBuilder.Create().WithSearchTerms("test").Size(-1).Build());
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
