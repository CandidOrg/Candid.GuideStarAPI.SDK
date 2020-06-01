using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Candid.GuideStarAPI
{
  public class Request
  {
    /// <summary>
    /// HTTP Method
    /// </summary>
    public HttpMethod Method { get; }

    private Uri Uri;

    /// <summary>
    /// Endpoint Subscription Key
    /// </summary>
    public readonly SubscriptionKey SubscriptionKey;

    /// <summary>
    /// Query param
    /// </summary>
    public string QueryParam { get; private set; }

    /// <summary>
    /// Post params
    /// </summary>
    public IDictionary<string, string> PostParams { get; private set; }

    /// <summary>
    /// Create a new Twilio request
    /// </summary>
    /// <param name="method">HTTP method</param>
    /// <param name="uri">Request URI</param>
    /// <param name="region">Twilio region</param>
    /// <param name="queryParams">Query parameters</param>
    /// <param name="postParams">Post data</param>
    public Request(
        HttpMethod method,
        SubscriptionKey subscriptionKey,
        Domain uri,
        string queryParam = null,
        IDictionary<string, string> postParams = null
    )
    {
      Method = method;
      SubscriptionKey = subscriptionKey;
      QueryParam = queryParam ?? string.Empty;
      PostParams = postParams ?? new Dictionary<string, string>();

      Uri = ConstructUrl(new Uri("https://apidata.guidestar.org/" + uri), QueryParam);
    }

    /// <summary>
    /// Construct the request URL
    /// </summary>
    /// <returns>Built URL including query parameters</returns>
    public Uri ConstructUrl(Uri uri, string queryParam)
    {
      return new Uri($"{uri.AbsoluteUri}/{queryParam}");
    }

    public Uri GetUri()
    {
      return Uri;
    }

    private static string EncodeParameters(IDictionary<string, string> data)
    {
      var result = "";
      var first = true;
      foreach (var pair in data)
      {
        if (first)
        {
          first = false;
        }
        else
        {
          result += "&";
        }

        result += WebUtility.UrlEncode(pair.Key) + "=" + WebUtility.UrlEncode(pair.Value);
      }

      return result;
    }

    /// <summary>
    /// Encode POST data for transfer
    /// </summary>
    /// <returns>Encoded byte array</returns>
    public byte[] EncodePostParams()
    {
      return Encoding.UTF8.GetBytes(EncodeParameters(PostParams));
    }

    /// <summary>
    /// Add a parameter to the request payload
    /// </summary>
    /// <param name="name">name of parameter</param>
    /// <param name="value">value of parameter</param>
    public void AddPostParam(string name, string value)
    {
      AddParam(PostParams, name, value);
    }

    private static void AddParam(IDictionary<string, string> dict, string name, string value)
    {
      dict.Add(name, value);
    }

    /// <summary>
    /// Compare request
    /// </summary>
    /// <param name="obj">object to compare to</param>
    /// <returns>true if requests are equal; false otherwise</returns>
    public override bool Equals(object obj)
    {
      if (obj == null)
      {
        return false;
      }

      if (ReferenceEquals(this, obj))
      {
        return true;
      }
      if (obj.GetType() != typeof(Request))
      {
        return false;
      }

      var other = (Request)obj;
      return Method.Equals(other.Method) &&
             Uri.Equals(other.Uri) &&
             QueryParam.Equals(other.QueryParam) &&
             PostParams.All(other.PostParams.Contains);
    }

    /// <summary>
    /// Generate hash code for request
    /// </summary>
    /// <returns>generated hash code</returns>
    public override int GetHashCode()
    {
      unchecked
      {
        return (Method?.GetHashCode() ?? 0) ^
               (Uri?.GetHashCode() ?? 0) ^
               (QueryParam?.GetHashCode() ?? 0) ^
               (PostParams?.GetHashCode() ?? 0);
      }
    }
  }
}