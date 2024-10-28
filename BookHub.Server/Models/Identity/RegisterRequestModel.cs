﻿namespace BookHub.Server.Models.Identity
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterRequestModel
    {
        [Required]
        public string Username { get; init; } = null!;

        [Required]
        public string Email { get; init; } = null!;

        [Required]
        public string Password { get; init; } = null!;
    }
}
