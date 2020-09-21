using AspNetCore.IQueryable.Extensions.Attributes;
using AspNetCore.IQueryable.Extensions.Filter;
using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;
using Microsoft.AspNetCore.Mvc;
using RESTFul.Api.Models;

namespace RESTFul.Api.ViewModels
{
    public class ApplicantSearch : IQuerySort, IQueryPaging
    {

        [FromQuery(Name = "lives_in")]
        public string Country { get; set; }

        [QueryOperator(Operator = WhereOperator.GreaterThanOrEqualTo, HasName = "Age"), FromQuery(Name = "older_than")]
        public int? OlderThan { get; set; }

        [QueryOperator(Operator = WhereOperator.LessThanOrEqualTo, HasName = "Age"), FromQuery(Name = "younger_than")]
        public int? YoungerThan { get; set; }

        [QueryOperator(Operator = WhereOperator.Equals), FromQuery(Name = "status")]
        public Status? Status { get; set; }

        public int? Offset { get; set; }
        public int? Limit { get; set; } = 10;
        public string Sort { get; set; }
    }

}
