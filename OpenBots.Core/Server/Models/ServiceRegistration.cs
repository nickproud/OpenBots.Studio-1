using System;

namespace OpenBots.Core.Server.Models
{
    public class ServiceRegistration : NamedEntity
    {
        public string Environment { get; set; }

        public int Version { get; set; }

        public ServiceProtocol Protocol { get; set; }

        public Uri ServiceBaseUri { get; set; }

        public Uri HealthCheckUri { get; set; }

        public Uri OpenAPIUri { get; set; }

        public Uri SwaggerUri { get; set; }

        public string Description { get; set; }

        public string ClientSDKNugetPackageID { get; set; }

        public bool IsCurrentlyUnderMaintenance { get; set; }

        public DateTime? MaintenanceEndsOnUTC { get; set; }

        public string ServiceTag { get; set; }

        public enum ServiceProtocol : int
        {
            Unknown = 0,
            HTTPS = 1,
            GRPC = 2
        }
    }
}
