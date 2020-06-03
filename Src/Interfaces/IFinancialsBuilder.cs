using System;

namespace Candid.GuideStarAPI
{
  public interface IFinancialsBuilder
  {
    IFinancialsBuilder Form990Revenue(Action<IMinMaxBuilder> action);
    IFinancialsBuilder Form990Expenses(Action<IMinMaxBuilder> action);
    IFinancialsBuilder Form990Assets(Action<IMinMaxBuilder> action);
  }
}
