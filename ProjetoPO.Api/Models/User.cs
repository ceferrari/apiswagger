using System;

namespace ProjetoPO.Api.Models
{
    public class BaseUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class User : BaseUser
    {
        public Guid Id { get; set; }
    }
}
