# Candid.GuideStarAPI.SDK

This library provides simplified access to the [GuideStar APIs](https://apiportal.guidestar.org/).

## API Getting Started Guide

[Here](https://candidorg-my.sharepoint.com/:w:/g/personal/alison_jannette_candid_org/EQ1mxwFp-RpCuG_TlyrhbckBBRPwYaFQqaen5Ja9Hu01WA?e=WFXCM2) 
is our getting started guide. This will help you get an account setup.

## SDK Setup

At the most simple level, calling an API requires initializing the system with an API key, then calling
the correct method from the resource you are interested in.

The majority of our resources use an EIN(Employer Identification Number) as a key for documents.

``` csharp
 GuideStarClient.Init(PREMIER_KEY);
 var premier = PremierResource.GetOrganization(EIN);
```

The above code will return the premier JSON document of EIN as a string.

``` csharp
 GuideStarClient.Init(PREMIER_KEY);
 var premier = await PremierResource.GetOrganizationAsync(EIN);
 ```

All request methods have and asynchronous and synchronous version.

Our Essentials resource takes a search payload as input and then behaves similarly to our other resources.
The return is a JSON collection of organization info that matches the payload parameters.

``` csharp
var payload = SearchPayloadBuilder.Create()
  .WithSearchTerms("guidestar")
  .Filters(
    filterBuilder => filterBuilder
    .Organization(
        organizationBuilder => organizationBuilder.IsOnBMF(true)
              .SpecificExclusions(
                  seBuilder => seBuilder.ExcludeDefunctOrMergedOrganizations()
                                        .ExcludeRevokedOrganizations()
                )
            )
    .Geography(
        g => g.HavingCounty(new string[] { "James City" })
    )
).Build();

GuideStarClient.Init(ESSENTIALS_KEY);
var essentials = EssentialsResource.GetOrganization(payload);
```

## SDK Demonstration Repo

We've created a [demonstration website](https://github.com/CandidOrg/APIDemoCore) in ASP.NET Core MVC
to show example usage of the SDK