using System;
using System.Collections.Generic;

namespace Candid.GuideStarAPI
{
  public interface IOrganizationBuilder
  {
    IOrganizationBuilder HavingProfileLevel(IEnumerable<string> level);
    IOrganizationBuilder HavingNTEEMajorCode(IEnumerable<string> nteeMajorCode);
    IOrganizationBuilder HavingNTEEMinorCode(IEnumerable<string> nteeMinorCode);
    IOrganizationBuilder HavingSubsectionCode(IEnumerable<string> subsectionCode);
    IOrganizationBuilder HavingFoundationCode(IEnumerable<string> foundationCode);
    IOrganizationBuilder IsOnBMF(bool bmfStatus);
    IOrganizationBuilder IsPub78Verified(bool pubStatus);
    IOrganizationBuilder AffiliationType(Action<IAffiliationTypeBuilder> action);
    IOrganizationBuilder SpecificExclusions(Action<ISpecificExclusionBuilder> action);
    IOrganizationBuilder NumberOfEmployees(Action<IMinMaxBuilder> action);
    IOrganizationBuilder FormTypes(Action<IFormTypeBuilder> action);
    IOrganizationBuilder Audits(Action<IAuditBuilder> action);
  }
}
