using Hosteland.Services.OrderService;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace Hosteland.WebJobs {
    public class Functions {

        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;

        public Functions(IOrderService orderService, IConfiguration configuration)
        {
            _configuration = configuration;
            _orderService = orderService;
        }

        public async Task MyTimerTriggerOperation([TimerTrigger("%schedule%", RunOnStartup = false)] TimerInfo timerInfo, CancellationToken cancellationToken) {
            await _orderService.TriggerMonthlyBill(cancellationToken);
            Console.WriteLine(timerInfo.ScheduleStatus);
        }
    }
}
