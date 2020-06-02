using System;
using System.Collections.Generic;
using System.Linq;

namespace Candid.GuideStarAPI
{
  public class GeographyBuilder : IGeographyBuilder
  {
    protected Geography _geography;

    private GeographyBuilder() => _geography = new Geography();

    internal static GeographyBuilder Create() => new GeographyBuilder();

    public IGeographyBuilder HavingState(IEnumerable<string> states)
    {
      _geography.state = states.ToArray();
      return this;
    }
    public IGeographyBuilder HavingZipCode(string zipCode)
    {
      if (zipCode?.Length == 5)
      {
        _geography.zip = zipCode;
        return this;
      }
      throw new ArgumentOutOfRangeException("Zip Codes are 5 characters long");
    }

    public IGeographyBuilder WithinZipRadius(int zipRadius)
    {
      if (zipRadius > 0)
      {
        _geography.radius = zipRadius;
        return this;
      }
      throw new ArgumentOutOfRangeException("Zip Radius must be greater than 0");
    }

    public IGeographyBuilder HavingMSA(IEnumerable<string> msa)
    {
      _geography.msa = msa.ToArray();
      return this;
    }

    public IGeographyBuilder HavingCity(IEnumerable<string> cities)
    {
      _geography.city = cities.ToArray();
      return this;
    }

    public IGeographyBuilder HavingCounty(IEnumerable<string> counties)
    {
      _geography.county = counties.ToArray();
      return this;
    }

    internal Geography Build() => _geography;
  }
}