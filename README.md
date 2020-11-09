# Candid.GuideStarAPI.SDK

This library provides simplified access to the [GuideStar APIs](https://apiportal.guidestar.org/).

## API Getting Started Guide

[Here](https://candid.my.salesforce.com/sfc/p/3h000001QSAr/a/3h000000TnBo/uQQygqyylOe2y81D9tCEgMJKCuHekZZ5aXDcrDoAvhQ) 
is our getting started guide. This will help you get your API Account and Subscription keys which are necessary to use the SDK.

## Installation

GuideStar SDK is available on [NuGet](https://www.nuget.org/packages/Candid.GuideStarAPI/):

    PM> Install-Package Candid.GuideStarAPI

or if using .NET Core:

    dotnet add package Candid.GuideStarAPI

## SDK Setup

At the most simple level, calling an API requires initializing the system with an API key, then calling
the correct method from the resource you are interested in.

The majority of our resources use an EIN (Employer Identification Number) as a key for documents.

### To use Premier

The following code will return the Premier JSON document of an EIN as a string.

``` csharp
 GuideStarClient.SubscriptionKeys.Add(Domain.PremierV3, PREMIER_KEY);
 var premier = PremierResource.GetOrganization(EIN);
```

All request methods have an asynchronous and synchronous version.

``` csharp
 GuideStarClient.SubscriptionKeys.Add(Domain.PremierV3, PREMIER_KEY);
 var premier = await PremierResource.GetOrganizationAsync(EIN);
 ```
All Premier responses are defined in our API documentation found [here](https://apiportal.guidestar.org/docs/services/microservices.api.profile/operations/5e9857f563b518201fe624b3?).

### To use Charity Check

Similarly, the following code will return the Charity Check JSON document of an EIN as a string.

``` csharp
GuideStarClient.SubscriptionKeys.Add(Domain.CharityCheckV1, CHARITYCHECK_KEY);
var charitycheck = CharityCheckResource.GetOrganization(EIN);
```
All Charity Check responses are defined in the API documentation found [here](https://apiportal.guidestar.org/docs/services/microservices.api.charitycheck/operations/59f794e7259f000a2cde9b04).

### To use Essentials

The Essentials resource takes a search payload as input and then behaves similarly to our other resources. For all realtive search filters allowed in the payload, please refer to the code example below and refer to the API documentation found [here](https://apiportal.guidestar.org/docs/services/microservices.api.search/operations/5c471f485d8d8981bbd8bc9f?).

``` csharp
string searchterms = "";
int from = 1;
int size = 25;
string sortby = "organization_name";
string[] state = { "ny" };
string zip = "";
int radius = 50;
string[] msa = { "" };
string[] city = { "" };
string[] county = { "" };
string[] profilelevel = { "Platinum", "Bronze" };
string[] nteemajor = { "" };
string[] nteeminor = { "" };
string[] subsection = { "" };
string[] foundation = { "" };
bool bmfstatus = true;
bool pub78verified = true;
int minemployees = 0;
int maxemployees = 1000000000;
int minrevenue = 0;
int maxrevenue = 1000000000;
int minexpenses = 0;
int maxexpesnes = 1000000000;
int minassets = 0;
int maxassets = 100000000;

string ESSENTIALS_KEY = "Your key here";

var payload = SearchPayloadBuilder.Create()
.WithSearchTerms(searchterms) //search terms is a string used as a keyword search
.From(from) //from is an integer to specify which record to start at for pagination
.Size(size) //size is an integer to specify how many records to return
.Sort(SortBuilder =>
{
    SortBuilder.SortBy(sortby); //sortby is a string to specify how to sort the records
    SortBuilder.SortByAscending(); //sortbyascending is used to sort the records alphabectically. The sort is set to true when this is present in the form.
    //SortBuilder.SortByDescending(); //sortbydescending is used to sort the records reverse alphabectically. The sort is set to true when this is present in the form.
}
)
.Filters(filterBuilder =>
{
    filterBuilder.Geography(geographyBuilder =>
    {
        geographyBuilder.HavingState(state); //state is a string array to filter on multiple states
        //geographyBuilder.HavingZipCode(zip); //zip is a string to filter on 1 zip code 
        geographyBuilder.WithinZipRadius(radius); //radius is an integer to include up to 50 miles from the zip
        geographyBuilder.HavingMSA(msa); //msa is an string array to filter on multiple msa codes
        geographyBuilder.HavingCity(city); //city is a string array to filter on multiple cities
        geographyBuilder.HavingCounty(county); //county is a string array to filter on multiple counties
    }
    );
    filterBuilder.Organization(OrganizationBuilder =>
    {
        OrganizationBuilder.HavingProfileLevel(profilelevel); //profilelevel is a string array to filter on the seal of transparency level
        OrganizationBuilder.HavingNTEEMajorCode(nteemajor); //nteemajor is a string array to filter on major ntee codes
        OrganizationBuilder.HavingNTEEMinorCode(nteeminor); //nteeminor is a string array to filter on minor ntee codes
        OrganizationBuilder.HavingSubsectionCode(subsection); //subsection is a string array to filer on subsection codes
        OrganizationBuilder.HavingFoundationCode(foundation); //foundation is a string array to filer on foundation codes
        OrganizationBuilder.IsOnBMF(bmfstatus); //bmfstatus is a boolean to filter on bmf status
        OrganizationBuilder.IsPub78Verified(pub78verified); //pub78verified is a boolean to filter on verification on pub 78
        OrganizationBuilder.AffiliationType(AffiliationTypeBuilder =>
        {
            //AffiliationTypeBuilder.OnlyParents(); //only parents is used to filter parent organizations. The search is set to true when this is present in the form.  
            //AffiliationTypeBuilder.OnlySubordinate(); //only subordinate is used to filter subordinate organizations. The search is set to true when this is present in the form.
            //AffiliationTypeBuilder.OnlyIndependent(); //only independent is used to filter independent organizations. The search is set to true when this is present in the form.
            //AffiliationTypeBuilder.OnlyHeadquarters(); //only headquarters is used to filter headquarter organizations. The search is set to true when this is present in the form.
        }
        );
        OrganizationBuilder.SpecificExclusions(SpecificExclusionBuilder =>
        {
            SpecificExclusionBuilder.ExcludeRevokedOrganizations(); //exclude revoked organizations is used to filter out any revoked organizations. The search is set to true when this is present in the form.
            SpecificExclusionBuilder.ExcludeDefunctOrMergedOrganizations(); //exclude defunct or merged organizations is used to filter out any defunct or merged organizations. the search is set to true when this is present in the form.
        }
        );
        OrganizationBuilder.NumberOfEmployees(MinMaxBuilder =>
        {
            MinMaxBuilder.HavingMaximum(maxemployees); //maxemployees is an integer to include organizations with the number of employees less than the specified amount
            MinMaxBuilder.HavingMinimum(minemployees); //minemployees is an integer to include organizations with the number of employees more than the specified amount
        }
        );
        OrganizationBuilder.FormTypes(FormTypeBuilder =>
        {
            FormTypeBuilder.OnlyF990(); //only f990 is used to filter organizations who filed a form 990. The search is set to true when this is present in the form.
            //FormTypeBuilder.OnlyF990PF(); //only f990pf is used to filter organizations who filed a form 990-pf. The search is set to true when this is present in the form.
            //FormTypeBuilder.Only990tRequired(); //only f990t required is used to filter organizations who are required to filed a form 990-t. The search is set to true when this is present in the form.
        }
        );
        OrganizationBuilder.Audits(AuditBuilder =>
        {
            AuditBuilder.HavingA133Audit(); // having a133 audit is used to filter organizations who have completed an a133 audit. The search is set to true when this is present in the form.
        }
        );
    }
    );
    filterBuilder.Financials(finBuilder =>
    {
        finBuilder.Form990Assets(assets =>
        {
            assets.HavingMaximum(maxassets); //maxassets is an integer to include organizations with assets less than the specified amount
            assets.HavingMinimum(minassets); //minassets is an integer to include organizations with assets more than the specified amount
        }
        );
        finBuilder.Form990Expenses(expenses =>
        {
            expenses.HavingMaximum(maxexpesnes); //maxexpenses is an integer to include organizations with expenses less than the specified amount
            expenses.HavingMinimum(minexpenses); //minexpenses is an integer to include organizations with expenses more than the specified amount
        }
        );
        finBuilder.Form990Revenue(revenue =>
        { 
            revenue.HavingMaximum(maxrevenue); //maxrevenue is an integer to include organizations with revenue less than the specified amount
            revenue.HavingMinimum(minrevenue); //minrevenue is an integer to include organizations with revenue more than the specified amount
        }
        );
    }
    );
}
)
.Build();

GuideStarClient.SubscriptionKeys.Add(Domain.EssentialsV2, ESSENTIALS_KEY);
var essentials = EssentialsResource.GetOrganization(payload);
```
The return is a JSON collection of organization info that matches the payload parameters. All Essentials responses are defined in the API Documention found [here](https://apiportal.guidestar.org/docs/services/microservices.api.search/operations/5c471f485d8d8981bbd8bc9f?).


## SDK Demonstration Repo

There is a [demonstration website](https://github.com/CandidOrg/APIDemoCore) in ASP.NET Core MVC to show example usage of the SDK.
