using System.Collections.Generic;
using Catalog.Models;
using Microsoft.AspNetCore.Http.Features;

namespace Catalog.Repositories
{
    public class InMemVareRepository : IVareRepository
    {

        // readonly da det ikke skal ændre listen efter construct af repo objekt
        private readonly List<Vare> Varene = new List<Vare>()
        {
            new Vare { Id= Guid.NewGuid(), Name = "Cykel", Price = 599, CreatedDate = DateTimeOffset.UtcNow},
            new Vare { Id= Guid.NewGuid(), Name = "Bil", Price = 10000, CreatedDate = DateTimeOffset.UtcNow},
            new Vare { Id= Guid.NewGuid(), Name = "Båd", Price = 30000, CreatedDate = DateTimeOffset.UtcNow}
        };

        public IEnumerable<Vare> GetVare() 
        {
            return Varene;
        }

        public Vare GetEnkeltVare(Guid id) 
        {
            return Varene.Where(vare => vare.Id == id).SingleOrDefault();
            // Uden singleordefault finder den en hel collection vi skal bare bruge 1 udfra id fra parameter ellers returner den null hvis der ikke er nogle
        }

        public void CreateVare(Vare vare)
        {
            Varene.Add(vare);
        }

        public void UpdateVare(Vare vare)
        {
            var index = Varene.FindIndex(existingVare => existingVare.Id == vare.Id);
            // nu replacer vi i listen "Varene" det index det rigtige sted med den nye vare
            Varene[index] = vare;
        }

        public void DeleteVare(Guid id)
        {
            var index = Varene.FindIndex(existingVare => existingVare.Id == id);
            Varene.RemoveAt(index);
        }
    }
}