using Catalog.Dtos;
using Catalog.Models;

namespace Catalog
{
    public static class Extensions {
        // AsDto tager en instans af Vare som input og returner en ny instans af VareDto som er vores DTO
        // en DTO er et objekt der bærer data mellem processer
        // vi gør det her fordi vi ikke vil eksponere hele domænemodel for klienten (kig schema i swagger)
        public static VareDto AsDto(this Vare vare)
        {
            return new VareDto{
                Id = vare.Id,
                Name = vare.Name,
                Price = vare.Price,
                CreatedDate = vare.CreatedDate
            };
        }
    }
}