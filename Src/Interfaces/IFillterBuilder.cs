using System;

namespace Candid.GuideStarAPI
{
  public interface IFilterBuilder
  {
    IFilterBuilder Geography(Action<IGeographyBuilder> action);
    IFilterBuilder Organization(Action<IOrganizationBuilder> action);
  }
}