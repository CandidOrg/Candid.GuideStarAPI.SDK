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
    public string SubscriptionKey { get; }

    /// <summary>
    /// Query params
    /// </summary>
    public List<KeyValuePair<string, string>> QueryParams { get; private set; }

    /// <summary>
    /// Post params
    /// </summary>
    public List<KeyValuePair<string, string>> PostParams { get; private set; }

    /// <summary>
    /// Create a new Twilio request
    /// </summary>
    /// <param name="method">HTTP Method</param>
    /// <param name="url">Request URL</param>
    public Request(HttpMethod method, string url)
    {
      Method = method;
      Uri = new Uri(url);
      QueryParams = new List<KeyValuePair<string, string>>();
      PostParams = new List<KeyValuePair<string, string>>();
    }

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
        string uri,
        string region = null,
        List<KeyValuePair<string, string>> queryParams = null,
        List<KeyValuePair<string, string>> postParams = null
    )
    {
      Method = method;
      Uri = new Uri("https://apidata.guidestar.org" + uri);

      QueryParams = queryParams ?? new List<KeyValuePair<string, string>>();
      PostParams = postParams ?? new List<KeyValuePair<string, string>>();
    }

    /// <summary>
    /// Construct the request URL
    /// </summary>
    /// <returns>Built URL including query parameters</returns>
    public Uri ConstructUrl()
    {
      return QueryParams.Count > 0 ?
          new Uri(Uri.AbsoluteUri + "?" + EncodeParameters(QueryParams)) :
          new Uri(Uri.AbsoluteUri);
    }

    /// <summary>
    /// Set auth for the request
    /// </summary>
    /// <param name="subscriptionKey">SubscriptionKey</param>
    public void SetAuth(string subscriptionKey)
    {
      SubscriptionKey = subscriptionKey;
    }

    private static string EncodeParameters(IEnumerable<KeyValuePair<string, string>> data)
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
    /// Add query parameter to request
    /// </summary>
    /// <param name="name">name of parameter</param>
    /// <param name="value">value of parameter</param>
    public void AddQueryParam(string name, string value)
    {
      AddParam(QueryParams, name, value);
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

    private static void AddParam(ICollection<KeyValuePair<string, string>> list, string name, string value)
    {
      list.Add(new KeyValuePair<string, string>(name, value));
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
             QueryParams.All(other.QueryParams.Contains) &&
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
               (QueryParams?.GetHashCode() ?? 0) ^
               (PostParams?.GetHashCode() ?? 0);
      }
    }
  }
}