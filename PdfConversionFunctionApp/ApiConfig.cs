using System;
using System.Collections.Generic;
using System.Text;

namespace PdfConversionFunctionApp
{
    public class ApiConfig
    {
        public string Endpoint { get; set; }
        public string GrantType { get; set; }
        public string Scope { get; set; }
        public string Resource { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string GraphEndpoint { get; set; }
        public string SiteId { get; set; }
    }
}
