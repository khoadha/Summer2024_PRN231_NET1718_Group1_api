using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class GlobalRateDTO
    {
        public float? Deposit { get; set; }
        public float? Refund { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    
    public class AddGlobalRateDTO
    {
        public float? Deposit { get; set; }
        public float? Refund { get; set; }
    }
}
