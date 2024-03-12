using System;
using System.Collections.Generic;
using Catalog.Models;
using System.Threading.Tasks;

namespace Catalog.Repositories 
{
    public interface IVareRepository
    {
        Task<Vare> GetEnkeltVareAsync(Guid id);
        Task<IEnumerable<Vare>> GetVareAsync();
        Task CreateVareAsync(Vare vare);
        Task UpdateVareAsync(Vare vare);
        Task DeleteVareAsync(Guid id);
    }
}