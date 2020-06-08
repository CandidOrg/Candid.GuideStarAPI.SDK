using System;

namespace Candid.GuideStarAPI
{
  public class MinMaxBuilder
  {
    protected Min_Max _minMax;

    private MinMaxBuilder() => _minMax = new Min_Max();

    internal static MinMaxBuilder Create() => new MinMaxBuilder();

    public MinMaxBuilder HavingMaximum(int numberEmployees)
    {
      if (numberEmployees >= 0)
      {
        _minMax.max = numberEmployees;
        return this;
      }
      throw new Exception("HavingMaximum must be greater than 0");
    }

    public MinMaxBuilder HavingMinimum(int numberEmployees)
    {
      if (numberEmployees >= 0)
      {
        _minMax.min = numberEmployees;
        return this;
      }
      throw new Exception("HavingMinimum must be greater than 0");
    }

    internal Min_Max Build() => _minMax;
  }
}
