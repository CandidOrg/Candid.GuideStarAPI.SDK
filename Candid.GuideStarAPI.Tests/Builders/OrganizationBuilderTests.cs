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
  public class OrganizationBuilderTests
  {
    private readonly IConfiguration _config;
    private static string CHARITY_CHECK_KEY;
    private static string ESSENTIALS_KEY;
    private static string PREMIER_KEY;

    public OrganizationBuilderTests()
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
    public static IEnumerable<object[]> foundationCodesGood =>
new List<object[]>
{
            new object[] { new List<string> { "17" } },
};

    [Theory]
    [MemberData(nameof(foundationCodesGood))]
    public void HavingFoundationCode_Works(List<string> foundationCodes)
    {
      var payload = SearchPayloadBuilder.Create()
      .Filters(
        filterBuilder => filterBuilder
        .Organization(
            organizationBuilder =>
              organizationBuilder.HavingFoundationCode(foundationCodes)
        )
    ).Build();
      //TestPayload(payload);
    }
    public static IEnumerable<object[]> foundationCodesBad =>
new List<object[]>
{
            new object[] { new List<string> { " " } },
            new object[] { new List<string> { null } },
};

    [Theory]
    [MemberData(nameof(foundationCodesBad))]
    public void HavingFoundationCode_Fails(List<string> foundationCodes)
    {
      var payload = SearchPayloadBuilder.Create()
      .Filters(
        filterBuilder => filterBuilder
        .Organization(
            organizationBuilder =>
              organizationBuilder.HavingFoundationCode(foundationCodes)
        )
    ).Build();
      Assert.Throws<ApiException>(() => EssentialsResource.GetOrganization(payload));
    }
    public static IEnumerable<object[]> NTEEMajorCodesGood =>
  new List<object[]>
  {
            new object[] { new List<string> { "A Arts, Culture, and Humanities" } },
            new object[] { new List<string> { "B Educational Institutions" } },
  };

    [Theory]
    [MemberData(nameof(NTEEMajorCodesGood))]
    public void HavingNTEEMajorCode_Works(List<string> NTEEMajorCodes)
    {

      var payload = SearchPayloadBuilder.Create()
      .Filters(
        filterBuilder => filterBuilder
        .Organization(
            organizationBuilder =>
              organizationBuilder.HavingNTEEMajorCode(NTEEMajorCodes)
        )
    ).Build();
      //TestPayload(payload);
    }
    public static IEnumerable<object[]> NTEEMajorCodesBad =>
 new List<object[]>
 {
            new object[] { new List<string> { " " } },
            new object[] { new List<string> { null } },
 };
    [Theory]
    [MemberData(nameof(NTEEMajorCodesBad))]
    public void HavingNTEEMajorCode_Fails(List<string> NTEEMajorCodes)
    {
      var payload = SearchPayloadBuilder.Create()
      .Filters(
        filterBuilder => filterBuilder
        .Organization(
            organizationBuilder =>
              organizationBuilder.HavingNTEEMajorCode(NTEEMajorCodes)
        )
    ).Build();
      Assert.Throws<ApiException>(() => EssentialsResource.GetOrganization(payload));
    }
    public static IEnumerable<object[]> nteeMinorCodesGood =>
   new List<object[]>
   {
            new object[] { new List<string> { "B01 Alliance/Advocacy Organizations" } },
   };

    [Theory]
    [MemberData(nameof(nteeMinorCodesGood))]
    public void HavingNTEEMinorCode_Works(List<string> nteeMinorCodes)
    {
      var payload = SearchPayloadBuilder.Create()
      .Filters(
        filterBuilder => filterBuilder
        .Organization(
            organizationBuilder =>
              organizationBuilder.HavingNTEEMinorCode(nteeMinorCodes)
        )
    ).Build();
      //TestPayload(payload);
    }
    public static IEnumerable<object[]> nteeMinorCodesBad =>
   new List<object[]>
   {
            new object[] { new List<string> { "bad ntee" } },
            new object[] { new List<string> { " " } },
            new object[] { new List<string> { null } },
   };

    [Theory]
    [MemberData(nameof(nteeMinorCodesBad))]
    public void HavingNTEEMinorCode_Fails(List<string> nteeMinorCodes)
    {
      var payload = SearchPayloadBuilder.Create()
      .Filters(
        filterBuilder => filterBuilder
        .Organization(
            organizationBuilder =>
              organizationBuilder.HavingNTEEMinorCode(nteeMinorCodes)
        )
    ).Build();
      Assert.Throws<ApiException>(() => EssentialsResource.GetOrganization(payload));
    }
    public static IEnumerable<object[]> profileLevels =>
   new List<object[]>
   {
            new object[] { new List<string> { "Platinum" } },
            new object[] { new List<string> { "Platinum", "Silver" } },
   };

    [Theory]
    [MemberData(nameof(profileLevels))]
    public void HavingProfileLevel_Works(List<string> profileLevels)
    {
      var payload = SearchPayloadBuilder.Create()
      .Filters(
        filterBuilder => filterBuilder
        .Organization(
            organizationBuilder =>
              organizationBuilder.HavingProfileLevel(profileLevels)
        )
    ).Build();
      //TestPayload(payload);
    }
    public static IEnumerable<object[]> profileLevelsBad =>
   new List<object[]>
   {
            new object[] { new List<string> { " " } },
            new object[] { new List<string> { null } },
   };

    [Theory]
    [MemberData(nameof(profileLevelsBad))]
    public void HavingProfileLevel_Fails(List<string> profileLevels)
    {
      var payload = SearchPayloadBuilder.Create()
      .Filters(
        filterBuilder => filterBuilder
        .Organization(
            organizationBuilder =>
              organizationBuilder.HavingProfileLevel(profileLevels)
        )
    ).Build();
      Assert.Throws<ApiException>(() => EssentialsResource.GetOrganization(payload));
    }
    public static IEnumerable<object[]> subsectionsGood =>
    new List<object[]>
    {
            new object[] { new List<string> { "501(c)(3) Public Charity", "501(c)(3) Private Non-Operating Foundation" } },
            new object[] { new List<string> { "501(c)(3) Private Non-Operating Foundation" } },
    };

    [Theory]
    [MemberData(nameof(subsectionsGood))]
    public void HavingSubsectionCode_Works(List<string> subsections)
    {
      var payload = SearchPayloadBuilder.Create()
      .Filters(
        filterBuilder => filterBuilder
        .Organization(
            organizationBuilder =>
              organizationBuilder.HavingSubsectionCode(subsections)
        )
    ).Build();
      //TestPayload(payload);
    }
    public static IEnumerable<object[]> subsectionsBad =>
    new List<object[]>
    {
            new object[] { new List<string> { "4014" } },
            new object[] { new List<string> { "303" } },
            new object[] { new List<string> { " " } },
    };

    [Theory]
    [MemberData(nameof(subsectionsBad))]
    public void HavingSubsectionCode_Fails(List<string> subsections)
    {
      var payload = SearchPayloadBuilder.Create()
      .Filters(
        filterBuilder => filterBuilder
        .Organization(
            organizationBuilder =>
              organizationBuilder.HavingSubsectionCode(subsections)
        )
    ).Build();
      Assert.Throws<ApiException>(() => EssentialsResource.GetOrganization(payload));
    }

    public static IEnumerable<object[]> boolParameters =>
new List<object[]>
{
        new object[] {  true},
        new object[] { false},
        new object[] { null},
};

    [Theory]
    [MemberData(nameof(boolParameters))]
    public void IsOnBMF_Works(bool bmfStatus)
    {
      var payload = SearchPayloadBuilder.Create()
      .Filters(
        filterBuilder => filterBuilder
        .Organization(
            organizationBuilder =>
              organizationBuilder.IsOnBMF(bmfStatus)
        )
    ).Build();
      //TestPayload(payload);
    }

    [Theory]
    [MemberData(nameof(boolParameters))]
    public void IsPub78Verified_Works(bool IsPub78Verified)
    {
      var payload = SearchPayloadBuilder.Create()
      .Filters(
        filterBuilder => filterBuilder
        .Organization(
            organizationBuilder =>
              organizationBuilder.IsPub78Verified(IsPub78Verified)
        )
    ).Build();
      //TestPayload(payload);
    }
    [Fact]
    public void AffiliationType_Works()
    {
      Action<AffiliationTypeBuilder> action = (AffiliationTypeBuilder) =>
      {
        AffiliationTypeBuilder.OnlyParents();
      };
      var payload = SearchPayloadBuilder.Create()
      .Filters(
        filterBuilder => filterBuilder
        .Organization(
            organizationBuilder =>
              organizationBuilder.AffiliationType(action)
        )
    ).Build();
      //TestPayload(payload);
    }
    [Fact]
    public void SpecificExclusions_Works()
    {
      Action<SpecificExclusionBuilder> action = (SpecificExclusionBuilder) =>
      {
        SpecificExclusionBuilder.ExcludeDefunctOrMergedOrganizations();
      };
      var payload = SearchPayloadBuilder.Create()
      .Filters(
        filterBuilder => filterBuilder
        .Organization(
            organizationBuilder =>
              organizationBuilder.SpecificExclusions(action)
        )
    ).Build();
      //TestPayload(payload);
    }
    [Fact]
    public void NumberOfEmployees_Works()
    {
      Action<MinMaxBuilder> action = (MinMaxBuilder) =>
      {
        MinMaxBuilder.HavingMinimum(1);
      };
      var payload = SearchPayloadBuilder.Create()
      .Filters(
        filterBuilder => filterBuilder
        .Organization(
            organizationBuilder =>
              organizationBuilder.NumberOfEmployees(action)
        )
    ).Build();
      //TestPayload(payload);
    }
    [Fact]
    public void FormTypes_Works()
    {
      Action<FormTypeBuilder> action = (FormTypeBuilder) =>
      {
        FormTypeBuilder.Only990tRequired();
      };
      var payload = SearchPayloadBuilder.Create()
      .Filters(
        filterBuilder => filterBuilder
        .Organization(
            organizationBuilder =>
              organizationBuilder.FormTypes(action)
        )
    ).Build();
      //TestPayload(payload);
    }
    [Fact]
    public void Audits_Works()
    {
      Action<AuditBuilder> action = (AffiliationTypeBuilder) =>
      {
        AffiliationTypeBuilder.HavingA133Audit();
      };
      var payload = SearchPayloadBuilder.Create()
      .Filters(
        filterBuilder => filterBuilder
        .Organization(
            organizationBuilder =>
              organizationBuilder.Audits(action)
        )
    ).Build();
      //TestPayload(payload);
    }

  }
}
