using System;
using System.Net.Http;

namespace Candid.GuideStarAPI
{
  public class ApiException : Exception
  {
    public HttpResponseMessage Response { get; }

    internal ApiException(HttpResponseMessage response, string message = null, Exception innerException = null) 
      : base(message ?? response?.ReasonPhrase, innerException)
    {
      Response = response;
    }
  }
}
