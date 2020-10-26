# Candid.GuideStarAPI.SDK

This library provides simplified access to the [GuideStar APIs](https://apiportal.guidestar.org/).

## API Getting Started Guide

[Here](https://candid.my.salesforce.com/sfc/p/3h000001QSAr/a/3h000000TnBo/uQQygqyylOe2y81D9tCEgMJKCuHekZZ5aXDcrDoAvhQ) 
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

All request methods have an asynchronous and synchronous version.

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

GuideStarClient.SubscriptionKeys.Add(Domain.EssentialsV2, ESSENTIALS_KEY);
var essentials = EssentialsResource.GetOrganization(payload);
```

## SDK Demonstration Repo

We've created a [demonstration website](https://github.com/CandidOrg/APIDemoCore) in ASP.NET Core MVC
to show example usage of the SDK
