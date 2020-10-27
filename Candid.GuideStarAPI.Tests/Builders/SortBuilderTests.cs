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
    public class SortBuilderTests
    {
        private readonly IConfiguration _config;
        private static string CHARITY_CHECK_KEY;
        private static string ESSENTIALS_KEY;
        private static string PREMIER_KEY;

        public SortBuilderTests()
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
                      sortBuilder.SortByAscending();
                  else
                      sortBuilder.SortByDescending();
              }
              )
              .Build();
            TestPayload(payload);
        }
    }
}
