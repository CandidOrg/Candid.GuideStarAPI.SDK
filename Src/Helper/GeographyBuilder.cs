using System;
using System.Collections.Generic;
using System.Linq;

namespace Candid.GuideStarAPI
{
  public class GeographyBuilder : IGeographyBuilder
  {
    protected Geography _geography;

    private GeographyBuilder() => _geography = new Geography();

    public static GeographyBuilder Create() => new GeographyBuilder();

    GeographyBuilder HavingState(IEnumerable<string> states)
    {
      _geography.state = states.ToArray();
      return this;
    }
    GeographyBuilder HavingZipCode(string zipCode)
    {
      if (zipCode?.Length == 5)
      {
        _geography.zip = zipCode;
        return this;
      }
      throw new Exception("Zip Codes are 5 characters long");
    }

    GeographyBuilder WithinZipRadius(int zipRadius)
    {
      if (zipRadius > 0)
      {
        _geography.radius = zipRadius;
        return this;
      }
      throw new Exception("Zip Radius must be greater than 0");
    }

    GeographyBuilder HavingMSA(IEnumerable<string> msa)
    {
      _geography.msa = msa.ToArray();
      return this;
    }

    GeographyBuilder HavingCity(IEnumerable<string> cities)
    {
      _geography.city = cities.ToArray();
      return this;
    }

    GeographyBuilder HavingCounty(IEnumerable<string> counties)
    {
      _geography.county = counties.ToArray();
      return this;
    }

    internal Geography Build() => _geography;
  }
}