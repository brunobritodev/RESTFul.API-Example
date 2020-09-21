using RESTFul.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace RESTFul.Api.Commands
{
    public class UpdateApplicantCommand
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }

        public void Update(Applicant actual)
        {
            actual.FirstName = FirstName;
            actual.LastName = LastName;
            actual.Gender = Gender;
            actual.Age = Age;
            actual.Country = Country;
        }
    }

    public class TransferApplicantCommand
    {
        public string Username { get; set; }
        public int Company { get; set; }
    }

}