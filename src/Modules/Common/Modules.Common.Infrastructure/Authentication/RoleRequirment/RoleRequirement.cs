using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Common.Infrastructure.Authentication.RoleRequirment
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; init; }
        public RoleRequirement(string Role)
        {
            this.Role = Role;
        }
    }
}
