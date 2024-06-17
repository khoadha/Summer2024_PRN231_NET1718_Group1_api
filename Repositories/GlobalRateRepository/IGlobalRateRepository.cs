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
        Task<List<GlobalRate>> GetRateForOrder();
        Task<GlobalRate> UpdateRate(GlobalRate RoomCategory);
        Task<bool> SaveAsync();

    }
}
