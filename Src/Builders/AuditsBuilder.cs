namespace Candid.GuideStarAPI
{
  class AuditBuilder : IAuditBuilder
  {
    protected Audits _audits;

    private AuditBuilder() => _audits = new Audits();

    internal static AuditBuilder Create() => new AuditBuilder();

    public IAuditBuilder HavingA133Audit()
    {
      _audits.a_133_audit_performed = true;
      return this;
    }

    internal Audits Build() => _audits;
  }
}
