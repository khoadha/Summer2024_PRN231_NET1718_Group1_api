using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.FurnitureRepository
{
    public class FurnitureRepository : IFurnitureRepository
    {
        private readonly AppDbContext _context;

        public FurnitureRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Furniture> AddFurniture(Furniture furniture)
        {
            try
            {
                await _context.Furniture.AddAsync(furniture);
                await SaveAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return furniture;
        }

        public async Task<List<Furniture>> GetFurnitures()
        {
            List<Furniture> result = new List<Furniture>();
            try
            {
                result = await _context.Furniture.ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<int> GetFurnituresTotal()
        {
            var result = 0;
            try
            {
                result = await _context.Furniture.CountAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
    }
}
