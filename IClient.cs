using System.Threading.Tasks;

namespace Candid.GuideStarAPI
{
  public interface IClient
  {
    Response Request(Request request);

    Task<Response> RequestAsync(Request request);
  }
}