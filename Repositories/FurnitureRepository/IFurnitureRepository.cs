using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.FurnitureRepository
{
    public interface IFurnitureRepository
    {
        Task<List<Furniture>> GetFurnitures();
        Task<Furniture> AddFurniture(Furniture furniture);
        Task<bool> SaveAsync();

    }
}
