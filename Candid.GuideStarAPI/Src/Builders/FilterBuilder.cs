using System;

namespace Candid.GuideStarAPI
{
    public class FilterBuilder
    {
        protected Filters _filter;

        private FilterBuilder() => _filter = new Filters();
        internal static FilterBuilder Create() => new FilterBuilder();

        public FilterBuilder Geography(Action<GeographyBuilder> action)
        {
            var _geoBuilder = GeographyBuilder.Create();
            action(_geoBuilder);
            _filter.geography = _geoBuilder.Build();
            return this;
        }

        public FilterBuilder Organization(Action<OrganizationBuilder> action)
        {
            var _orgBuilder = OrganizationBuilder.Create();
            action(_orgBuilder);
            _filter.organization = _orgBuilder.Build();
            return this;
        }

        public FilterBuilder Financials(Action<FinancialsBuilder> action)
        {
            var _finBuilder = FinancialsBuilder.Create();
            action(_finBuilder);
            _filter.financials = _finBuilder.Build();
            return this;
        }

        internal Filters Build() => _filter;
    }
}
