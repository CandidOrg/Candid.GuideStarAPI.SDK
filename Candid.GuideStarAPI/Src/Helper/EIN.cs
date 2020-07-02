using System;
using System.Text.RegularExpressions;

namespace Candid.GuideStarAPI
{
  public sealed class EIN
  {
    public readonly string EinString;
    private static Regex ein_regex = new Regex(@"^\d{2}-\d{7}$");
    private static Regex numeric_regex = new Regex(@"^(\d{2})(\d{7})$");

    public EIN(string ein)
    {
      ein = ein?.Trim();

      if (string.IsNullOrEmpty(ein))
        throw new ArgumentNullException("EIN cannot be null");
      else if (numeric_regex.IsMatch(ein))
      {
        var parts = numeric_regex.Match(ein);
        ein = string.Format("{0}-{1}", parts.Groups[1].Value, parts.Groups[2].Value);
      }
      else if (!ein_regex.IsMatch(ein))
        throw new ArgumentException("Bad EIN format. Should be in ##-####### format.");

      EinString = ein;
    }
  }
}
