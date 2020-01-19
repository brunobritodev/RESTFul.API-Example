using System.Collections.Generic;
using System.Diagnostics;

namespace RESTFul.Api.Models
{
    [DebuggerDisplay("{FullName}")]
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{LastName}, {FirstName}";
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Username { get; set; }
        public bool Active { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
        public string Country { get; set; }
    }
}