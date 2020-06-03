namespace Candid.GuideStarAPI
{
  internal class FormTypeBuilder : IFormTypeBuilder
  {
    protected Form_Types _formType;

    private FormTypeBuilder() => _formType = new Form_Types();

    internal static FormTypeBuilder Create() => new FormTypeBuilder();

    public IFormTypeBuilder Only990tRequired()
    {
      _formType.required_to_file_990t = true;
      return this;
    }

    public IFormTypeBuilder OnlyF990()
    {
      _formType.f990 = true;
      return this;
    }

    public IFormTypeBuilder OnlyF990PF()
    {
      _formType.f990pf = true;
      return this;
    }

    internal Form_Types Build() => _formType;
  }
}
