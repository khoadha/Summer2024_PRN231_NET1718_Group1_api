using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.GlobalRateRepository
{
    public interface IGlobalRateRepository
    {
        Task<List<GlobalRate>> GetRates();
        Task<GlobalRate> GetRateById(int id);
        Task<GlobalRate> GetNewestRateAsync();

        Task<GlobalRate> AddRate(GlobalRate RoomCategory);
        Task<bool> SaveAsync();

    }
}
