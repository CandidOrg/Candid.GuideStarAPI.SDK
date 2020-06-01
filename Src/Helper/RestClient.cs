using System;
using System.IO;
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

    private readonly SubscriptionKeys _subscriptionKeys;
    private readonly SubscriptionKey _subscriptionKey;
    private readonly string _baseUrl;

    public RestClient(SubscriptionKey subscriptionKey, string baseURL)
    {
      _subscriptionKey = subscriptionKey;
      _baseUrl = baseURL;
      HttpClient = DefaultClient();
    }

    public RestClient(SubscriptionKeys keys, HttpClient httpClient = null)
    {
      _subscriptionKeys = keys;
      HttpClient = httpClient ?? DefaultClient();
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
        httpRequest.Content = new FormUrlEncodedContent(request.PostParams);
      }

      var httpResponse = await HttpClient.SendAsync(httpRequest).ConfigureAwait(false);
      var reader = new StreamReader(await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false));

      // Create and return a new Response. Keep a reference to the last
      // response for debugging, but don't return it as it may be shared
      // among threads.
      var response = new Response(httpResponse.StatusCode, await reader.ReadToEndAsync().ConfigureAwait(false));
      return response;
    }

    private static Response ProcessResponse(Response response)
    {
      if (response == null)
      {
        //TODO: add ApiConnectionException
        throw new Exception("Connection Error: No response received.");
      }

      if (response.StatusCode >= HttpStatusCode.OK && response.StatusCode < HttpStatusCode.Ambiguous)
      {
        return response;
      }

      // Deserialize and throw exception
      //TODO: add RestException
      Exception restException = null;
      try
      {
        restException = new Exception(response.Content);
      }
      //TODO: add JsonReaderException
      catch (Exception) { /* Allow null check below to handle */ }

      if (restException == null)
      {
        //TODO: add ApiException
        throw new Exception($"Api Error: {response.StatusCode} - {(response.Content ?? "[no content]")}");
      }

      throw new Exception(
          restException.Message ?? "Unable to make request, " + response.StatusCode
      );
    }

    private HttpRequestMessage BuildHttpRequest(Request request)
    {
      var httpRequest = new HttpRequestMessage(
          new HttpMethod(request.Method.ToString()),
          request.GetUri()
      );

      httpRequest.Headers.Add("Subscription-key", _subscriptionKey.SubscriptionString);
      httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      httpRequest.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));

      var libraryVersion = "candid-csharp";
      httpRequest.Headers.TryAddWithoutValidation("User-Agent", libraryVersion);

      return httpRequest;
    }
  }
}
