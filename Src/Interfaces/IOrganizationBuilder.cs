using Candid.GuideStarAPI.Src.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace Candid.GuideStarAPI
{
  interface IOrganizationBuilder
  {
    IOrganizationBuilder HavingProfileLevel(IEnumerable<string> level);
    IOrganizationBuilder HavingNTEEMajorCode(IEnumerable<string> nteeMajorCode);
    IOrganizationBuilder HavingNTEEMinorCode(IEnumerable<string> nteeMinorCode);
    IOrganizationBuilder HavingSubsectionCode(IEnumerable<string> subsectionCode);
    IOrganizationBuilder HavingFoundationCode(IEnumerable<string> foundationCode);
    IOrganizationBuilder IsOnBMF(bool bmfStatus);
    IOrganizationBuilder IsPub78Verified(bool pubStatus);
    IAffiliationTypeBuilder AffiliationType();
    ISpecificExclusionBuilder SpecificExclusions();
    INumberOfEmployeesBuilder NumberOfEmployees();
    //IFormTypeBuilder FormTypes()
    //IAuditBuilder Audits();
  }
}
