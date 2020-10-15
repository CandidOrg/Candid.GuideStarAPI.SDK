using Candid.GuideStarAPI.Types;

namespace Candid.GuideStarAPI
{
  public class SortBuilder
  {
    protected Sort _sort;

    private SortBuilder()
    {
      _sort = new Sort();
    }

    internal static SortBuilder Create() => new SortBuilder();

    public SortBuilder SortBy(SortOptions sort)
    {
      _sort.sort_by = sort;
      return this;
    }

    public SortBuilder SortByAscending()
    {
      _sort.ascending = true;
      return this;
    }

    public SortBuilder SortByDescending()
    {
      _sort.ascending = false;
      return this;
    }

    public Sort Build() => _sort;
  }

  public class SortOptions : StringEnum
  {
    private SortOptions(string value) : base(value) { }

    public static implicit operator SortOptions(string value)
    {
      return new SortOptions(value);
    }

    //relecance is null since empty string is not a valid JSON value
    //from: https://github.com/dotnet/runtime/issues/34310
    public static readonly SortOptions Relevance;
    public static readonly SortOptions OrganizationName = "Organization_name";
    public static readonly SortOptions BmfGrossReceipts = "bmf_gross_receipts";
    public static readonly SortOptions BmfAssets = "bmf_assets";
  }
}
