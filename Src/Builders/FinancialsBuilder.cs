using System;

namespace Candid.GuideStarAPI
{
  public class FinancialsBuilder
  {
    protected Financials _financials;
    
    private FinancialsBuilder() => _financials = new Financials();

    internal static FinancialsBuilder Create() => new FinancialsBuilder();

    public FinancialsBuilder Form990Assets(Action<MinMaxBuilder> action)
    {
      var _f990Assets = MinMaxBuilder.Create();
      action(_f990Assets);
      _financials.total_assets = _f990Assets.Build();
      return this;
    }

    public FinancialsBuilder Form990Expenses(Action<MinMaxBuilder> action)
    {
      var _f990Expenses = MinMaxBuilder.Create();
      action(_f990Expenses);
      _financials.total_expenses = _f990Expenses.Build();
      return this;
    }

    public FinancialsBuilder Form990Revenue(Action<MinMaxBuilder> action)
    {
      var _f990Revenue = MinMaxBuilder.Create();
      action(_f990Revenue);
      _financials.total_revenue = _f990Revenue.Build();
      return this;
    }

    internal Financials Build() => _financials;
  }
}
