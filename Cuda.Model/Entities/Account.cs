using System;
using System.ComponentModel.DataAnnotations.Schema;
using Cuda.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Cuda.Model.Entities
{
    public class Account : IdentityUser<Guid>, IEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}".Trim();

        public string ProfileImageUrl { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}