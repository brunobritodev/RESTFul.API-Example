using Bogus;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RESTFul.Api.Models
{
    public class Company
    {
        public string Name { get; set; }
        public int Id { get; set; }
        [JsonIgnore]
        public ICollection<Applicant> Applicants { get; set; }

        public Company() { }


        public static Faker<Company> Get(int applicants)
        {
            return new Faker<Company>()
                .RuleFor(c => c.Name, f => f.Company.CompanyName())
                .RuleFor(c => c.Applicants, (f, company) => Applicant.Get().Generate(applicants));
        }
    }
}