namespace Candid.GuideStarAPI
{
  public interface IMinMaxBuilder
  {
    IMinMaxBuilder HavingMinimum(int value);
    IMinMaxBuilder HavingMaximum(int value);
  }
}