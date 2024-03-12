using System.ComponentModel.DataAnnotations;

namespace Catalog.Dtos 
{
    public record CreateVareDto 
    {
        // laver data annotations for at undgå post med null i name f.eks. m.m.
        [Required]
        public string Name {get; init;}
        [Required]
        [Range(1, 1000000)]
        public decimal Price {get; init;}
    }
}