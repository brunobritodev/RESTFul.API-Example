using RESTFul.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace RESTFul.Api.Commands
{
    public class RegisterUserCommand
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        [Required]
        public string Username { get; set; }
        public int Age { get; set; }

        public string Country { get; set; }

        public User ToEntity()
        {
            return new User()
            {
                Age = Age,
                FirstName = FirstName,
                LastName = LastName,
                Gender = Gender,
                Username = Username,
                Country = Country
            };
        }
    }
}
