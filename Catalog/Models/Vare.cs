using System;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;

namespace Catalog.Models
{
    // Prøver records fordi - godt med immutable objekter, "with expressions" support og value equality support jeg vil prøve af i dette proj
    public record Vare
    {
    // init-only properties - efter man skaber entitet så kan man ikke modify den
        public Guid Id {get; init;}
        public string Name {get; init;}
        public decimal Price {get; init;}
        public DateTimeOffset CreatedDate {get; init;}
    }
}