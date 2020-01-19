using RESTFul.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace RESTFul.Api.Commands
{
    public class UpdateUserCommand
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }

        public void Update(User actual)
        {
            actual.FirstName = FirstName;
            actual.LastName = LastName;
            actual.Gender = Gender;
            actual.Age = Age;
            actual.Country = Country;
        }
    }
}