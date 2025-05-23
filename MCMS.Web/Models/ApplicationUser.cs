﻿using Microsoft.AspNetCore.Identity;

namespace MCMS.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
