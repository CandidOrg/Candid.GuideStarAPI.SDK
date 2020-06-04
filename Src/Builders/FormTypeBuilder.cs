namespace Candid.GuideStarAPI
{
  public class FormTypeBuilder
  {
    protected Form_Types _formType;

    private FormTypeBuilder() => _formType = new Form_Types();

    internal static FormTypeBuilder Create() => new FormTypeBuilder();

    public FormTypeBuilder Only990tRequired()
    {
      _formType.required_to_file_990t = true;
      return this;
    }

    public FormTypeBuilder OnlyF990()
    {
      _formType.f990 = true;
      return this;
    }

    public FormTypeBuilder OnlyF990PF()
    {
      _formType.f990pf = true;
      return this;
    }

    internal Form_Types Build() => _formType;
  }
}
