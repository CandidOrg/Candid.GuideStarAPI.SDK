using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Candid.GuideStarAPI
{
  public class RestClient : IClient
  {
    /// <summary>
    /// Client to make HTTP requests
    /// </summary>
    public HttpClient HttpClient { get; }

    private readonly SubscriptionKey _subscriptionKey;
    private readonly string _baseUrl;

    public RestClient(SubscriptionKey subscriptionKey, string baseURL)
    {
      _subscriptionKey = subscriptionKey;
      _baseUrl = baseURL;
      HttpClient = DefaultClient();
    }

    private HttpClient DefaultClient()
    {
      return new HttpClient();
    }

    /// <summary>
    /// Make a synchronous request
    /// </summary>
    /// <param name="request">Twilio request</param>
    /// <returns>Twilio response</returns>
    public Response Request(Request request)
    {
      try
      {
        var task = RequestAsync(request);
        task.Wait();
        return task.Result;
      }
      catch (AggregateException ae)
      {
        if (ae.InnerException is ApiException)
          throw ae.InnerException;

        // Combine nested AggregateExceptions
        ae = ae.Flatten();
        throw ae.InnerExceptions[0];
      }
    }

    /// <summary>
    /// Make an asynchronous request
    /// </summary>
    /// <param name="request">Twilio response</param>
    /// <returns>Task that resolves to the response</returns>
    public async Task<Response> RequestAsync(Request request)
    {
      var httpRequest = BuildHttpRequest(request);
      if (!Equals(request.Method, HttpMethod.Get))
      {
        if (request.PostParamsDict?.Any() ?? false)
        {
          httpRequest.Content = new FormUrlEncodedContent(request.PostParamsDict);
        }
        else
        {
          httpRequest.Content = new StringContent(request.PostParams);
          httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
      }

      var httpResponse = await HttpClient.SendAsync(httpRequest).ConfigureAwait(false);
      if (httpResponse == null)
      {
        throw new ApiConnectionException("API Connection Error: No response received.");
      }
      else if (httpResponse.IsSuccessStatusCode)
      {
        try
        {
          var reader = new StreamReader(await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false));
          return new Response(httpResponse.StatusCode, await reader.ReadToEndAsync().ConfigureAwait(false));
        }
        catch (Exception ex)
        {
          throw new ApiException(httpResponse, "Error reading response stream", ex);
        }
      }
      else
        throw new ApiException(httpResponse);
    }

    private HttpRequestMessage BuildHttpRequest(Request request)
    {
      var httpRequest = new HttpRequestMessage(
          new HttpMethod(request.Method.ToString()),
          request.GetUri()
      );

      httpRequest.Headers.Add("Subscription-key", _subscriptionKey.Primary);
      httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      httpRequest.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));

      var libraryVersion = "candid-csharp";
      httpRequest.Headers.TryAddWithoutValidation("User-Agent", libraryVersion);

      return httpRequest;
    }
  }
}
