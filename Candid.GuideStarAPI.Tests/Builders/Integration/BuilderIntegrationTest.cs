using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.Json;
using Candid.GuideStarAPI.Resources;
using Candid.GuideStarApiTest;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Candid.GuideStarAPI.Tests.Builders.Integration
{
  public class BuilderIntegrationTest
  {
    private readonly IConfiguration _config;
    private static string ESSENTIALS_KEY;

    public BuilderIntegrationTest()
    {
      _config = ConfigLoader.InitConfiguration();
      ESSENTIALS_KEY = _config["Keys:ESSENTIALS_KEY"];

      SetSubscriptionKeys();
    }

    private static void SetSubscriptionKeys()
    {
      if (!string.IsNullOrEmpty(ESSENTIALS_KEY))
        GuideStarClient.SubscriptionKeys.Add(Domain.EssentialsV2, ESSENTIALS_KEY);
    }

    public static IEnumerable<object[]> goodFinancialValuesAndStatesData =>
    FinacialBuilderTests.goodFinancialValues.SelectMany(finVal =>
      GeographyBuilderTests.goodStatesData.Select(states =>
        new object[] { finVal[0], states[0] }
      )
    );

    [Theory]
    [MemberData(nameof(goodFinancialValuesAndStatesData))]
    public void Financials990AssetsMaximum(long value, IEnumerable<string> states)
    {
      var payload = SearchPayloadBuilder.Create()
        .WithSearchTerms("test")
        .Filters(filterBuilder =>
          {
            filterBuilder.Financials(finBuilder =>
              finBuilder.Form990Assets(assets => assets.HavingMaximum(value))
            );

            filterBuilder.Geography(geographyBuilder => geographyBuilder.HavingState(states));

            filterBuilder.Organization(organizationBuilder =>
              organizationBuilder.FormTypes(auditBuilder => auditBuilder.Only990tRequired())
            );
          }
        )
        .Build();
    }

    [Fact]
    public void BuilderAllExplicit()
    {
      const string searchterms = "";
      const int from = 1;
      const int size = 25;
      const string sortby = "organization_name";
      string[] state = { "ny" };
      const string zip = "";
      const int radius = 50;
      string[] msa = { "" };
      string[] city = { "" };
      string[] county = { "" };
      string[] profilelevel = { "Platinum", "Bronze" };
      string[] nteemajor = { "" };
      string[] nteeminor = { "" };
      string[] subsection = { "" };
      string[] foundation = { "" };
      const bool bmfstatus = true;
      const bool pub78verified = true;
      const int minemployees = 0;
      const int maxemployees = 1000000000;
      const int minrevenue = 0;
      const int maxrevenue = 1000000000;
      const int minexpenses = 0;
      const int maxexpesnes = 1000000000;
      const int minassets = 0;
      const int maxassets = 100000000;

      var payload = SearchPayloadBuilder.Create()
        //search terms is a string used as a keyword search
        .WithSearchTerms(searchterms)
        //from is an integer to specify which record to start at for pagination
        .From(from)
        //size is an integer to specify how many records to return
        .Size(size)
        .Sort(SortBuilder =>
          {
            //sortby is a string to specify how to sort the records
            SortBuilder.SortBy(sortby);
            //sortbyascending is used to sort the records alphabectically. The sort is set to true when this is present in the form.
            SortBuilder.SortByAscending();
            //sortbydescending is used to sort the records reverse alphabectically. The sort is set to true when this is present in the form.
            //SortBuilder.SortByDescending();
          }
        )
        .Filters(filterBuilder =>
          {
            filterBuilder.Geography(geographyBuilder =>
              {
                //state is a string array to filter on multiple states
                geographyBuilder.HavingState(state);
                //zip is a string to filter on 1 zip code
                //geographyBuilder.HavingZipCode(zip);
                //radius is an integer to include up to 50 miles from the zip
                geographyBuilder.WithinZipRadius(radius);
                //msa is an string array to filter on multiple msa codes
                geographyBuilder.HavingMSA(msa);
                //city is a string array to filter on multiple cities
                geographyBuilder.HavingCity(city);
                //county is a string array to filter on multiple counties
                geographyBuilder.HavingCounty(county);
              }
            );
            filterBuilder.Organization(OrganizationBuilder =>
              {
                //profilelevel is a string array to filter on the seal of transparencies
                OrganizationBuilder.HavingProfileLevel(profilelevel);
                //nteemajor is a string array to filter on major ntee codes
                OrganizationBuilder.HavingNTEEMajorCode(nteemajor);
                //nteeminor is a string array to filter on minor ntee codes
                OrganizationBuilder.HavingNTEEMinorCode(nteeminor);
                //subsection is a string array to filer on subsection codes
                OrganizationBuilder.HavingSubsectionCode(subsection);
                //foundation is a string array to filer on foundation codes
                OrganizationBuilder.HavingFoundationCode(foundation);
                //bmfstatus is a boolean to filter on bmf status
                OrganizationBuilder.IsOnBMF(bmfstatus);
                //pub78verified is a boolean to filter on verification on pub 78
                OrganizationBuilder.IsPub78Verified(pub78verified);
                OrganizationBuilder.AffiliationType(AffiliationTypeBuilder =>
                  {
                    //only parents is used to filter parent organizations. The search is set to true when this is present in the form.  
                    //AffiliationTypeBuilder.OnlyParents(); 
                    //only subordinate is used to filter subordinate organizations. The search is set to true when this is present in the form.
                    //AffiliationTypeBuilder.OnlySubordinate(); 
                    //only independent is used to filter independent organizations. The search is set to true when this is present in the form.
                    //AffiliationTypeBuilder.OnlyIndependent(); 
                    //only headquarters is used to filter headquarter organizations. The search is set to true when this is present in the form.
                    //AffiliationTypeBuilder.OnlyHeadquarters(); 
                  }
                );
                OrganizationBuilder.SpecificExclusions(SpecificExclusionBuilder =>
                  {
                    //exclude revoked organizations is used to filter out any revoked organizations. The search is set to true when this is present in the form.
                    SpecificExclusionBuilder.ExcludeRevokedOrganizations();
                    //exclude defunct or merged organizations is used to filter out any defunct or merged organizations. the search is set to true when this is present in the form.
                    SpecificExclusionBuilder.ExcludeDefunctOrMergedOrganizations();
                  }
                );
                OrganizationBuilder.NumberOfEmployees(MinMaxBuilder =>
                  {
                    //maxemployees is an integer to include organizations with the number of employees less than the specified amount
                    MinMaxBuilder.HavingMaximum(maxemployees);
                    //minemployees is an integer to include organizations with the number of employees more than the specified amount
                    MinMaxBuilder.HavingMinimum(minemployees);
                  }
                );
                OrganizationBuilder.FormTypes(FormTypeBuilder =>
                  {
                    //only f990 is used to filter organizations who filed a form 990. The search is set to true when this is present in the form.
                    FormTypeBuilder.OnlyF990();
                    //only f990pf is used to filter organizations who filed a form 990-pf. The search is set to true when this is present in the form.
                    //FormTypeBuilder.OnlyF990PF(); 
                    //only f990t required is used to filter organizations who are required to filed a form 990-t. The search is set to true when this is present in the form.
                    //FormTypeBuilder.Only990tRequired(); 
                  }
                );
                OrganizationBuilder.Audits(AuditBuilder =>
                  {
                    // having a133 audit is used to filter organizations who have completed an a133 audit. The search is set to true when this is present in the form.
                    AuditBuilder.HavingA133Audit();
                  }
                );
              }
            );
            filterBuilder.Financials(finBuilder =>
              {
                finBuilder.Form990Assets(assets =>
                  {
                    //maxassets is an integer to include organizations with assets less than the specified amount
                    assets.HavingMaximum(maxassets);
                    //minassets is an integer to include organizations with assets more than the specified amount
                    assets.HavingMinimum(minassets);
                  }
                );
                finBuilder.Form990Expenses(expenses =>
                  {
                    //maxexpenses is an integer to include organizations with expenses less than the specified amount
                    expenses.HavingMaximum(maxexpesnes);
                    //minexpenses is an integer to include organizations with expenses more than the specified amount
                    expenses.HavingMinimum(minexpenses);
                  }
                );
                finBuilder.Form990Revenue(revenue =>
                  {
                    //maxrevenue is an integer to include organizations with revenue less than the specified amount
                    revenue.HavingMaximum(maxrevenue);
                    //minrevenue is an integer to include organizations with revenue more than the specified amount
                    revenue.HavingMinimum(minrevenue);
                  }
                );
              }
            );
          }
        )
        .Build();

      GuideStarClient.SubscriptionKeys.Add(Domain.EssentialsV2, ESSENTIALS_KEY);
      var essentials = EssentialsResource.GetOrganization(payload);
    }
  }
}
