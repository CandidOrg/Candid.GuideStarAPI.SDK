using Newtonsoft.Json;
using System.Collections.Generic;

namespace Candid.GuideStarAPI
{
  public class SearchPayload
  {
    public string search_terms { get; set; }
    public int? from { get; set; }
    public int? size { get; set; }
    public Sort sort { get; set; }
    public Filters filters { get; set; }

    public Dictionary<string, object> ToDictionary()
    {
      // remove null fields
      return JsonConvert.DeserializeObject<Dictionary<string, object>>(
        JsonConvert.SerializeObject(
          this, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));

    }

    public string ToJson()
    {
      // remove null fields
      return JsonConvert.SerializeObject(
          this, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
    }
  }

  public class Sort
  {
    public string sort_by { get; set; }
    public bool? ascending { get; set; }
  }

  public class Filters
  {
    public Geography geography { get; set; }
    public Organization organization { get; set; }
    public Financials financials { get; set; }
  }

  public class Geography
  {
    public string[] state { get; set; }
    public string zip { get; set; }
    public int? radius { get; set; }
    public string[] msa { get; set; }
    public string[] city { get; set; }
    public string[] county { get; set; }
  }

  public class Organization
  {
    public string[] profile_levels { get; set; }
    public string[] ntee_major_codes { get; set; }
    public string[] ntee_minor_codes { get; set; }
    public string[] subsection_codes { get; set; }
    public string[] foundation_codes { get; set; }
    public bool? bmf_status { get; set; }
    public bool? pub78_verified { get; set; }
    public Affiliation_Type affiliation_type { get; set; }
    public Specific_Exclusions specific_exclusions { get; set; }
    public Min_Max number_of_employees_range { get; set; }
    public Form_Types form_types { get; set; }
    public Audits audits { get; set; }
  }

  public class Affiliation_Type
  {
    public bool? parent { get; set; }
    public bool? subordinate { get; set; }
    public bool? independent { get; set; }
    public bool? headquarter { get; set; }
  }

  public class Specific_Exclusions
  {
    public bool? exclude_revoked_organizations { get; set; }
    public bool? exclude_defunct_or_merged_organizations { get; set; }
  }

  public class Form_Types
  {
    public bool? f990 { get; set; }
    public bool? f990pf { get; set; }
    public bool? required_to_file_990t { get; set; }
  }

  public class Audits
  {
    public bool? a_133_audit_performed { get; set; }
  }

  public class Financials
  {
    public Min_Max total_revenue { get; set; }
    public Min_Max total_expenses { get; set; }
    public Min_Max total_assets { get; set; }
  }

  public class Min_Max
  {
    public int? min { get; set; }
    public int? max { get; set; }
  }
}
