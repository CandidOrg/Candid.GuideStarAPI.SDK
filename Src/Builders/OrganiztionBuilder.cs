using Candid.GuideStarAPI.Src.Builders;
using Candid.GuideStarAPI.Src.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Candid.GuideStarAPI
{
  class OrganizationBuilder : IOrganizationBuilder
  {
    protected Organization _organization;
    protected AffiliationTypeBuilder _affiliationTypeBuilder;
    protected SpecificExclusionBuilder _specificExclusionBuilder;
    protected NumberOfEmployeesBuilder _numberOfEmployeesBuilder;

    private OrganizationBuilder() => _organization = new Organization();
    
    internal static OrganizationBuilder Create() => new OrganizationBuilder();

    public IOrganizationBuilder HavingFoundationCode(IEnumerable<string> foundationCode)
    {
      _organization.foundation_codes = foundationCode.ToArray();
      return this;
    }

    public IOrganizationBuilder HavingNTEEMajorCode(IEnumerable<string> nteeMajorCode)
    {
      _organization.ntee_major_codes = nteeMajorCode.ToArray();
      return this;
    }

    public IOrganizationBuilder HavingNTEEMinorCode(IEnumerable<string> nteeMinorCode)
    {
      _organization.ntee_minor_codes = nteeMinorCode.ToArray();
      return this;
    }

    public IOrganizationBuilder HavingProfileLevel(IEnumerable<string> level)
    {
      _organization.profile_levels = level.ToArray();
      return this;
    }

    public IOrganizationBuilder HavingSubsectionCode(IEnumerable<string> subsectionCode)
    {
      _organization.subsection_codes = subsectionCode.ToArray();
      return this;
    }

    public IOrganizationBuilder IsOnBMF(bool bmfStatus)
    {
      _organization.bmf_status = bmfStatus;
      return this;
    }

    public IOrganizationBuilder IsPub78Verified(bool pubStatus)
    {
      _organization.pub78_verified = pubStatus;
      return this;
    }

    public IAffiliationTypeBuilder AffiliationType()
    {
      _affiliationTypeBuilder = AffiliationTypeBuilder.Create();
      return _affiliationTypeBuilder;
    }

    public ISpecificExclusionBuilder SpecificExclusions()
    {
      _specificExclusionBuilder = SpecificExclusionBuilder.Create();
      return _specificExclusionBuilder;
    }

    public INumberOfEmployeesBuilder NumberOfEmployees()
    {
      _numberOfEmployeesBuilder = NumberOfEmployeesBuilder.Create();
      return _numberOfEmployeesBuilder;
    }

    internal Organization Build() {
      _organization.affiliation_type = _affiliationTypeBuilder.Build();
      _organization.specific_exclusions = _specificExclusionBuilder.Build();
      //_organization.audits = _auditBuilder.Build();
      return _organization; 
    }

    
  }
}
