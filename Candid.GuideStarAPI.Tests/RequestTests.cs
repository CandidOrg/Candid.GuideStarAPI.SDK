using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Candid.GuideStarAPI.Tests
{
  [Collection("API Tests Collection")]
  public class RequestTests
  {
    [Fact]
    public void Construct_GetNoParams_ExpectAbsoluteURI()
    {
      var key = new SubscriptionKey("01234567890123456789012345678901");
      var request = new Request(HttpMethod.Get, key, Domain.EssentialsV2);
      Assert.True(request.GetUri().IsAbsoluteUri);
    }

    [Fact]
    public void Construct_GetWithParams_ExpectParams()
    {
      var key = new SubscriptionKey("01234567890123456789012345678901");
      var expected_value = @"ParamValue";
      var request = new Request(HttpMethod.Get, key, Domain.EssentialsV2, queryParam: expected_value);
      Assert.Equal(expected_value, request.QueryParam);
    }

    [Fact]
    public void Construct_Post_ExpectPostParams()
    {
      var key = new SubscriptionKey("01234567890123456789012345678901");

      // Set up parameters
      var parms = new Dictionary<string, string>();
      parms.Add("one", "two");
      parms.Add("three", "four");

      var request = new Request(HttpMethod.Post, key, Domain.EssentialsV2, null, parms);
      var expected_value = "one=two&three=four";

      Assert.NotEmpty(request.PostParamsDict);
      Assert.Equal(expected_value, Encoding.UTF8.GetString(request.EncodePostParams()));
    }

    [Fact]
    public void AddPostParam_KeyValue_ExpectInDictionary()
    {
      var key = new SubscriptionKey("01234567890123456789012345678901");
      var request = new Request(HttpMethod.Post, key, Domain.EssentialsV2);

      request.AddPostParam("key", "value");

      Assert.True(request.PostParamsDict.ContainsKey("key"));
      Assert.Equal("value", request.PostParamsDict["key"]);
    }

    [Fact]
    public void Equals_AreEqual_ExpectTrue()
    {
      var key = new SubscriptionKey("01234567890123456789012345678901");
      var req1 = new Request(HttpMethod.Get, key, Domain.EssentialsV2, "PostParams", "QueryParams");
      var req2 = new Request(HttpMethod.Get, key, Domain.EssentialsV2, "PostParams", "QueryParams");

      Assert.Equal(req1, req2);
    }

    [Fact]
    public void Equals_NotEqual_ExpectFalse()
    {
      var key = new SubscriptionKey("01234567890123456789012345678901");
      var req1 = new Request(HttpMethod.Get, key, Domain.EssentialsV2, "PostParams", "QueryParams");
      var req2 = new Request(HttpMethod.Get, key, Domain.EssentialsV2, "PostParams2", "QueryParams2");

      Assert.NotEqual(req1, req2);
    }

    [Fact]
    public void GetHashCode_KnownValues_ExpectEqual()
    {
      var key = new SubscriptionKey("01234567890123456789012345678901");
      var request = new Request(HttpMethod.Get, key, Domain.EssentialsV2, "PostParams", "QueryParams");

      var expected_result = HttpMethod.Get.GetHashCode()
                            ^ request.GetUri().GetHashCode()
                            ^ "PostParams".GetHashCode()
                            ^ "QueryParams".GetHashCode();

      Assert.Equal(expected_result, request.GetHashCode());
    }
  }
}
