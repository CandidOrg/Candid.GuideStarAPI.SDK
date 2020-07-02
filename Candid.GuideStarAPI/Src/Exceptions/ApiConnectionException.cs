namespace Candid.GuideStarAPI
{
  public class ApiConnectionException : ApiException
  {
    internal ApiConnectionException(string message) : base(null, message) { }
  }
}
