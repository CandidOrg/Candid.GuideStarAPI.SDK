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
        [Fact]
        public void HavingFoundationCode_Works()
        {
            List<string> foundationCodes = new List<string>();
            foundationCodes.Add("17");
            var payload = SearchPayloadBuilder.Create()
            .Filters(
              filterBuilder => filterBuilder
              .Organization(
                  organizationBuilder =>
                    organizationBuilder.HavingFoundationCode(foundationCodes)
              )
          ).Build();
            TestPayload(payload);
        }
        [Fact]
        public void HavingNTEEMajorCode_Works()
        {
            List<string> foundationCodes = new List<string>();
            foundationCodes.Add("A Arts, Culture, and Humanities");
            foundationCodes.Add("B Educational Institutions");
            var payload = SearchPayloadBuilder.Create()
            .Filters(
              filterBuilder => filterBuilder
              .Organization(
                  organizationBuilder =>
                    organizationBuilder.HavingNTEEMajorCode(foundationCodes)
              )
          ).Build();
            TestPayload(payload);
        }
        [Fact]
        public void HavingNTEEMinorCode_Works()
        {
            List<string> foundationCodes = new List<string>();
            foundationCodes.Add("B01 Alliance/Advocacy Organizations - 4807");
            var payload = SearchPayloadBuilder.Create()
            .Filters(
              filterBuilder => filterBuilder
              .Organization(
                  organizationBuilder =>
                    organizationBuilder.HavingNTEEMinorCode(foundationCodes)
              )
          ).Build();
            TestPayload(payload);
        }
        [Fact]
        public void HavingProfileLevel_Works()
        {
            List<string> foundationCodes = new List<string>();
            foundationCodes.Add("Platinum");
            foundationCodes.Add("Silver");
            var payload = SearchPayloadBuilder.Create()
            .Filters(
              filterBuilder => filterBuilder
              .Organization(
                  organizationBuilder =>
                    organizationBuilder.HavingProfileLevel(foundationCodes)
              )
          ).Build();
            TestPayload(payload);
        }
        [Fact]
        public void HavingSubsectionCode_Works()
        {
            List<string> foundationCodes = new List<string>();
            foundationCodes.Add("501(c)(3) Public Charity");
            var payload = SearchPayloadBuilder.Create()
            .Filters(
              filterBuilder => filterBuilder
              .Organization(
                  organizationBuilder =>
                    organizationBuilder.HavingSubsectionCode(foundationCodes)
              )
          ).Build();
            TestPayload(payload);
        }
        public static IEnumerable<object[]> boolParameters =>
    new List<object[]>
    {
        new object[] {  true},
        new object[] { false},
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
            TestPayload(payload);
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
            TestPayload(payload);
        }
        [Fact]
        public void AffiliationType_Works()
        {
            Action<AffiliationTypeBuilder> action = (AffiliationTypeBuilder) => {
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
            TestPayload(payload);
        }
        [Fact]
        public void SpecificExclusions_Works()
        {
            Action<SpecificExclusionBuilder> action = (SpecificExclusionBuilder) => {
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
            TestPayload(payload);
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
            TestPayload(payload);
        }
        [Fact]
        public void FormTypes_Works()
        {
            Action<FormTypeBuilder> action = (FormTypeBuilder) => {
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
            TestPayload(payload);
        }
        [Fact]
        public void Audits_Works()
        {
            Action<AuditBuilder> action = (AffiliationTypeBuilder) => {
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
            TestPayload(payload);
        }

    }
}
