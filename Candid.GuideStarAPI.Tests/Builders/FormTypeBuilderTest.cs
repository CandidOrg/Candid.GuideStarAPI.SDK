using System;
using System.Text.Json;
using Candid.GuideStarAPI;
using Candid.GuideStarAPI.Resources;
using Candid.GuideStarApiTest;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Candid.GuideStarAPI.Tests.Builders
{
  [Collection("API Tests Collection")]
  public class FormTypeBuilderTest
  {
    private readonly IConfiguration _config;
    private static string ESSENTIALS_KEY;

    public FormTypeBuilderTest()
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
    public void Only990tRequired()
    {
      var payload = SearchPayloadBuilder.Create()
        .Filters(
          filterBuilder => filterBuilder
          .Organization(
              organizationBuilder =>
                organizationBuilder.FormTypes(auditBuilder => auditBuilder.Only990tRequired())
          )
      ).Build();
      //TestPayload(payload);
    }

    [Fact]
    public void OnlyF990()
    {
      var payload = SearchPayloadBuilder.Create()
        .Filters(
          filterBuilder => filterBuilder
          .Organization(
              organizationBuilder =>
                organizationBuilder.FormTypes(auditBuilder => auditBuilder.OnlyF990())
          )
      ).Build();
      //TestPayload(payload);
    }

    [Fact]
    public void OnlyF990PF()
    {
      var payload = SearchPayloadBuilder.Create()
        .Filters(
          filterBuilder => filterBuilder
          .Organization(
              organizationBuilder =>
                organizationBuilder.FormTypes(auditBuilder => auditBuilder.OnlyF990PF())
          )
      ).Build();
      //TestPayload(payload);
    }
  }
}
