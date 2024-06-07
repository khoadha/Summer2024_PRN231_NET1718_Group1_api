using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RoomRepository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _context;

        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Room>> GetRoom()
        {
            var list = await _context.Rooms
                .Include(r => r.Category)
                .Include(r => r.RoomImages) 
                .Include(r => r.RoomFurniture)
                .ThenInclude(rf => rf.Furniture)
                .ToListAsync();
            return list;
        }

        public async Task<Room> AddRoom(Room room, List<RoomImage> images)
        {

            try
            {
                room.IsAvailable = true;
                room.RoomImages = images;

                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding the room.", ex);
            }

            return room;
        }

        public async Task<Room> EditRoom(Room room)
        {
            var existingRoom = await _context.Rooms.FindAsync(room.Id);
            if (existingRoom != null)
            {                
                _context.Rooms.Update(room);
                await _context.SaveChangesAsync();
            }
            return existingRoom;
        }

        public async Task<bool> DeleteRoom(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Room> FindRoomById(int id)
        {
            return await _context.Rooms
            .Include(r => r.Category)
                .Include(r => r.RoomImages)
                .Include(r => r.RoomFurniture)
                .ThenInclude(rf => rf.Furniture)
                 .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Room>> SearchRoom(string query)
        {
            var listProducts = await _context.Rooms
                .Include(r => r.Category)
                .Include(r => r.RoomImages)
                .Include(r => r.RoomFurniture)
                .Where(r => r.Name.Contains(query))
                .ToListAsync();
            return listProducts;
        }
    }
}
