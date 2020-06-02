using Candid.GuideStarAPI.Src.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Candid.GuideStarAPI.Src.Builders
{
  class NumberOfEmployeesBuilder : INumberOfEmployeesBuilder
  {
    protected Number_Of_Employees_Range _numEmployees;

    private NumberOfEmployeesBuilder() => _numEmployees = new Number_Of_Employees_Range();

    internal static NumberOfEmployeesBuilder Create() => new NumberOfEmployeesBuilder();

    public INumberOfEmployeesBuilder HavingMaximum(int numberEmployees)
    {
      if (numberEmployees > 0)
      {
        _numEmployees.max = numberEmployees;
        return this;
      }
      throw new Exception("HavingMaximum must be greater than 0");
    }

    public INumberOfEmployeesBuilder HavingMinimum(int numberEmployees)
    {
      if (numberEmployees > 0)
      {
        _numEmployees.min = numberEmployees;
        return this;
      }
      throw new Exception("HavingMinimum must be greater than 0");
    }

    internal Number_Of_Employees_Range Build() => _numEmployees;
  }
}
