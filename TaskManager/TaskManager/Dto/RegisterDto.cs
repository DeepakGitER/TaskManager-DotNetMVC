﻿using System.ComponentModel.DataAnnotations;

namespace TaskManager.Dto
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

}
