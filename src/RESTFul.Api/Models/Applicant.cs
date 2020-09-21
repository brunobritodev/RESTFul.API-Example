using Bogus;
using Bogus.DataSets;
using System;
using System.Diagnostics;

namespace RESTFul.Api.Models
{
    [DebuggerDisplay("{FullName}")]
    public class Applicant
    {
        public Applicant() { }
        public Applicant(Applicant applicant, int companyId)
        {
            CompanyId = companyId;
            Age = applicant.Age;
            Country = applicant.Country;
            FirstName = applicant.FirstName;
            Gender = applicant.Gender;
            LastName = applicant.LastName;
            Username = applicant.Username;
            Active = true;
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{LastName}, {FirstName}";
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Username { get; set; }
        public bool Active { get; set; }
        public string Country { get; set; }
        public Status Status { get; set; }
        public DateTime DateOfApply { get; set; }
        public Company Company { get; set; }

        public static Faker<Applicant> Get()
        {
            return new Faker<Applicant>()
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.FullName, f => f.Person.FullName)
                .RuleFor(u => u.Gender, f => f.PickRandom<Name.Gender>().ToString())
                .RuleFor(u => u.Age, f => f.Random.Int(18, 60))
                .RuleFor(u => u.Username, f => f.Person.UserName)
                .RuleFor(u => u.DateOfApply, f => f.Date.Past())
                .RuleFor(u => u.Active, true)
                .RuleFor(u => u.Status, Status.WaitingReview)
                .RuleFor(u => u.Country, f => f.Address.Country());
        }

        public void Delete()
        {
            Active = false;
        }

        public void Approve()
        {
            Status = Status.Approved;
        }

        public void Decline()
        {
            Status = Status.Declined;
        }
    }
}