using BusinessObjects.Entities;
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
            var list = await _context.Rooms.ToListAsync();
            return list;
        }

        public async Task<Room> AddRoom(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<Room> EditRoom(Room room)
        {
            var existingRoom = await _context.Rooms.FindAsync(room.Id);
            if (existingRoom != null)
            {
                existingRoom.Name = room.Name;
                existingRoom.RoomSize = room.RoomSize;
                existingRoom.CostPerDay = room.CostPerDay;
                existingRoom.Location = room.Location;
                existingRoom.CategoryId = room.CategoryId;
                existingRoom.IsAvailable = room.IsAvailable;
                //  RoomFurniture and RoomImages chua add

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
                .Include(r => r.RoomFurniture)
                    .ThenInclude(rf => rf.Furniture)
                .Include(r => r.RoomImages)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
