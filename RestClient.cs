using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Candid.GuideStarAPI
{
  class RestClient : IClient
  {
    /// <summary>
    /// Client to make HTTP requests
    /// </summary>
    public HttpClient HttpClient { get; }

    private readonly string _subscriptionKey;

    public RestClient(string subscriptionKey, HttpClient httpClient = null)
    {
      _subscriptionKey = subscriptionKey;
      HttpClient = httpClient ?? DefaultClient();
    }

    private HttpClient DefaultClient()
    {
      return new HttpClient();
    }

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

    public Task<Response> RequestAsync(Request request)
    {
      request.SetAuth(_subscriptionKey);
      Response response;
      try
      {
        response = await HttpClient.MakeRequestAsync(request);
      }
      catch (Exception clientException)
      {
        throw new ApiConnectionException(
            "Connection Error: " + request.Method + request.ConstructUrl(),
            clientException
        );
      }
      return ProcessResponse(response);
    }

    private static Response ProcessResponse(Response response)
    {
      if (response == null)
      {
        throw new ApiConnectionException("Connection Error: No response received.");
      }

      if (response.StatusCode >= HttpStatusCode.OK && response.StatusCode < HttpStatusCode.Ambiguous)
      {
        return response;
      }

      // Deserialize and throw exception
      RestException restException = null;
      try
      {
        restException = RestException.FromJson(response.Content);
      }
      catch (JsonReaderException) { /* Allow null check below to handle */ }

      if (restException == null)
      {
        throw new ApiException("Api Error: " + response.StatusCode + " - " + (response.Content ?? "[no content]"));
      }

      throw new ApiException(
          restException.Code,
          (int)response.StatusCode,
          restException.Message ?? "Unable to make request, " + response.StatusCode,
          restException.MoreInfo,
          restException.Details
      );
    }

    private HttpRequestMessage BuildHttpRequest(Request request)
    {
      var httpRequest = new HttpRequestMessage(
          new HttpMethod(request.Method.ToString()),
          request.ConstructUrl()
      );

      httpRequest.Headers.Add("Subscription-key", _subscriptionKey);
      httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      httpRequest.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));

      var libraryVersion = "candid-csharp";
      httpRequest.Headers.TryAddWithoutValidation("User-Agent", libraryVersion);

      return httpRequest;
    }
  }
}
