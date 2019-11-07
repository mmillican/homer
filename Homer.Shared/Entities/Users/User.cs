﻿using System.ComponentModel.DataAnnotations;

namespace Homer.Shared.Entities.Users
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, MaxLength(100)]
        public string EmailAddress { get; set; }
    }
}
