﻿using System.ComponentModel.DataAnnotations;

namespace TDV.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}