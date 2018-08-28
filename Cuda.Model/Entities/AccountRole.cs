using Microsoft.AspNetCore.Identity;
using System;

namespace Cuda.Model.Entities
{
    public class AccountRole : IdentityUserRole<Guid>
    {
        public Account Account { get; set; }

        public ApplicationRole ApplicationRole { get; set; }
    }
}