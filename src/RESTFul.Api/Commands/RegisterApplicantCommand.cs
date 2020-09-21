using RESTFul.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace RESTFul.Api.Commands
{
    public class RegisterApplicantCommand
    {
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        [Required]
        public string Username { get; set; }
        public int Age { get; set; }

        public string Country { get; set; }

        public Applicant ToEntity()
        {
            return new Applicant()
            {
                Age = Age,
                CompanyId = CompanyId,
                FirstName = FirstName,
                LastName = LastName,
                Gender = Gender,
                Username = Username,
                Country = Country
            };
        }
    }
}
