using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBots.Core.Server.Models
{
    public class ServerInfo
    {
        public Guid PersonId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public bool ForcedPasswordChange { get; set; }
        public bool IsUserConsentRequired { get; set; }
        public bool IsJoinOrgRequestPending { get; set; }
        public List<OrganizationListing> MyOrganizations { get; set; }

        public class OrganizationListing
        {
            public Guid? Id { get; set; }
            public string Name { get; set; }
            public bool? IsAdministrator { get; set; }
            public Guid? TenantKey { get; set; }
        }
    }
}
