using System;
using System.Collections.Generic;
using Catalog.Models;

namespace Catalog.Repositories 
{
    public interface IInMemVareRepository
    {
        Vare GetEnkeltVare(Guid id);
        IEnumerable<Vare> GetVare();
        void CreateVare(Vare vare);
        void UpdateVare(Vare vare);
        void DeleteVare(Guid id);
    }
}