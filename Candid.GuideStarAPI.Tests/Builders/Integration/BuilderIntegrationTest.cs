using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.Json;
using Candid.GuideStarAPI.Resources;
using Candid.GuideStarApiTest;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Candid.GuideStarAPI.Tests.Builders.Integration
{
  public class BuilderIntegrationTest
  {
    private readonly IConfiguration _config;
    private static string ESSENTIALS_KEY;

    public BuilderIntegrationTest()
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

    public static IEnumerable<object[]> goodFinancialValuesAndStatesData =>
    FinacialBuilderTests.goodFinancialValues.SelectMany(finVal =>
      GeographyBuilderTests.goodStatesData.Select(states =>
        new object[] { finVal[0], states[0] }
      )
    );

    [Theory]
    [MemberData(nameof(goodFinancialValuesAndStatesData))]
    public void Financials990AssetsMaximum(long value, IEnumerable<string> states)
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Filters(filterBuilder =>
          {
            filterBuilder.Financials(finBuilder =>
              finBuilder.Form990Assets(assets => assets.HavingMaximum(value))
            );

            filterBuilder.Geography(geographyBuilder => geographyBuilder.HavingState(states));

            filterBuilder.Organization(organizationBuilder =>
              organizationBuilder.FormTypes(auditBuilder => auditBuilder.Only990tRequired())
            );
          }
        )
        .Build();
      //TestPayload(payload);
    }
  }
}
