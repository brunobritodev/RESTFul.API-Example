using Bogus;
using RESTFul.Api.Service;
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
        public string Uniquename { get; set; }

        public Company() { }

        public Company(string name, List<Applicant> applicants)
        {
            Applicants = applicants;
            Name = name;
            Uniquename = name.Urlize();
        }

        public static Faker<Company> Get(int applicants)
        {
            return new Faker<Company>().CustomInstantiator(f =>
                new Company(f.Company.CompanyName(), Applicant.Get().Generate(applicants)));
        }
    }
}