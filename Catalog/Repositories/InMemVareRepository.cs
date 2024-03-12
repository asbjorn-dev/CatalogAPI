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

        // NOTE: Bruger FromResult og CompletedTask osv fordi metoderne herinde ikke indebærer nogen asynkron opgave. Det her var test af hardcoded data jeg startede allerførst med.
        public async Task<IEnumerable<Vare>> GetVareAsync() 
        {
            return await Task.FromResult(Varene);
        }

        public async Task<Vare> GetEnkeltVareAsync(Guid id) 
        {
            var varen = Varene.Where(vare => vare.Id == id).SingleOrDefault();
            // Uden singleordefault finder den en hel collection vi skal bare bruge 1 udfra id fra parameter ellers returner den null hvis der ikke er nogle
            return await Task.FromResult(varen);
        }

        public async Task CreateVareAsync(Vare vare)
        {
            Varene.Add(vare);
            // Lav en task som allerede er færdig og return den betyder CompletedTask
            await Task.CompletedTask;
        }

        public async Task UpdateVareAsync(Vare vare)
        {
            var index = Varene.FindIndex(existingVare => existingVare.Id == vare.Id);
            // nu replacer vi i listen "Varene" det index det rigtige sted med den nye vare
            Varene[index] = vare;
            await Task.CompletedTask;
        }

        public async Task DeleteVareAsync(Guid id)
        {
            var index = Varene.FindIndex(existingVare => existingVare.Id == id);
            Varene.RemoveAt(index);
            await Task.CompletedTask;
        }
    }
}