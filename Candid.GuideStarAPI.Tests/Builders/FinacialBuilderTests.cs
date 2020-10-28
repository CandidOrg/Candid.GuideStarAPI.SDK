using System;
using System.Collections.Generic;
using System.Text.Json;
using Candid.GuideStarAPI.Resources;
using Candid.GuideStarApiTest;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Candid.GuideStarAPI.Tests.Builders
{
  [Collection("API Tests Collection")]
  public class FinacialBuilderTests
  {
    private readonly IConfiguration _config;
    private static string ESSENTIALS_KEY;

    public FinacialBuilderTests()
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

    public static IEnumerable<object[]> goodFinancialValues =>
      new List<object[]>
      {
        new object[] { 0 },
        new object[] { 0L },
        new object[] { int.MaxValue },
        new object[] { long.MaxValue },
        new object[] { 100_000_000 },
        new object[] { 1_000_000_000 },
        new object[] { 100_000_000L },
        new object[] { 1_000_000_000L },
        new object[] { 10_000_000_000L },
      };

    [Theory]
    [MemberData(nameof(goodFinancialValues))]
    public void Financials990AssetsMaximum(long value)
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Filters(filterBuilder =>
          filterBuilder.Financials(finBuilder =>
            finBuilder.Form990Assets(assets => assets.HavingMaximum(value))
          )
        )
        .Build();
      //TestPayload(payload);
    }

    [Theory]
    [MemberData(nameof(goodFinancialValues))]
    public void Financials990AssetsMinimum(long value)
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Filters(filterBuilder =>
          filterBuilder.Financials(finBuilder =>
            finBuilder.Form990Assets(assets => assets.HavingMinimum(value))
          )
        )
        .Build();
      //TestPayload(payload);
    }

    [Theory]
    [MemberData(nameof(goodFinancialValues))]
    public void Financials990ExpensesMaximum(long value)
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Filters(filterBuilder =>
          filterBuilder.Financials(finBuilder =>
            finBuilder.Form990Expenses(expenses => expenses.HavingMaximum(value))
          )
        )
        .Build();
      //TestPayload(payload);
    }

    [Theory]
    [MemberData(nameof(goodFinancialValues))]
    public void Financials990ExpensesMinimum(long value)
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Filters(filterBuilder =>
          filterBuilder.Financials(finBuilder =>
            finBuilder.Form990Expenses(expenses => expenses.HavingMinimum(value))
          )
        )
        .Build();
      //TestPayload(payload);
    }

    [Theory]
    [MemberData(nameof(goodFinancialValues))]
    public void Financials990RevenueMaximum(long value)
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Filters(filterBuilder =>
          filterBuilder.Financials(finBuilder =>
            finBuilder.Form990Revenue(assets => assets.HavingMaximum(value))
          )
        )
        .Build();
      //TestPayload(payload);
    }

    [Theory]
    [MemberData(nameof(goodFinancialValues))]
    public void Financials990RevenueMinimum(long value)
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Filters(filterBuilder =>
          filterBuilder.Financials(finBuilder =>
            finBuilder.Form990Revenue(assets => assets.HavingMinimum(value))
          )
        )
        .Build();
      //TestPayload(payload);
    }

    public static IEnumerable<object[]> badFinancialValues =>
      new List<object[]>
      {
        new object[] { int.MinValue },
        new object[] { long.MinValue },
        new object[] { -1 },
      };


    [Theory]
    [MemberData(nameof(badFinancialValues))]
    public void Financials990HavingMaximum_Fails(long value)
    {
      Assert.Throws<ArgumentOutOfRangeException>(() => 
        SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Filters(filterBuilder =>
          filterBuilder.Financials(finBuilder =>
            finBuilder.Form990Assets(assets => assets.HavingMaximum(value))
          )
        )
        .Build());
    }

    [Theory]
    [MemberData(nameof(badFinancialValues))]
    public void Financials990HavingMinimum_Fails(long value)
    {
      Assert.Throws<ArgumentOutOfRangeException>(() =>
        SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Filters(filterBuilder =>
          filterBuilder.Financials(finBuilder =>
            finBuilder.Form990Assets(assets => assets.HavingMinimum(value))
          )
        )
        .Build());
    }

    [Theory]
    [MemberData(nameof(badFinancialValues))]
    public void Financials990ExpensesMaximum_Fails(long value)
    {
      Assert.Throws<ArgumentOutOfRangeException>(() =>
        SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Filters(filterBuilder =>
          filterBuilder.Financials(finBuilder =>
            finBuilder.Form990Expenses(assets => assets.HavingMaximum(value))
          )
        )
        .Build());
    }

    [Theory]
    [MemberData(nameof(badFinancialValues))]
    public void Financials990ExpensesMinimum_Fails(long value)
    {
      Assert.Throws<ArgumentOutOfRangeException>(() =>
        SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Filters(filterBuilder =>
          filterBuilder.Financials(finBuilder =>
            finBuilder.Form990Expenses(assets => assets.HavingMinimum(value))
          )
        )
        .Build());
    }

    [Theory]
    [MemberData(nameof(badFinancialValues))]
    public void Financials990RevenueMaximum_Fails(long value)
    {
      Assert.Throws<ArgumentOutOfRangeException>(() =>
        SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Filters(filterBuilder =>
          filterBuilder.Financials(finBuilder =>
            finBuilder.Form990Revenue(assets => assets.HavingMaximum(value))
          )
        )
        .Build());
    }

    [Theory]
    [MemberData(nameof(badFinancialValues))]
    public void Financials990RevenueMinimum_Fails(long value)
    {
      Assert.Throws<ArgumentOutOfRangeException>(() =>
        SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Filters(filterBuilder =>
          filterBuilder.Financials(finBuilder =>
            finBuilder.Form990Assets(assets => assets.HavingMinimum(value))
          )
        )
        .Build());
    }
  }
}
