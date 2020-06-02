using System;
using System.Collections.Generic;
using System.Text;

namespace Candid.GuideStarAPI.Src.Interfaces
{
  interface INumberOfEmployeesBuilder
  {
    INumberOfEmployeesBuilder HavingMinimum(int numberEmployees);
    INumberOfEmployeesBuilder HavingMaximum(int numberEmployees);
  }
}
