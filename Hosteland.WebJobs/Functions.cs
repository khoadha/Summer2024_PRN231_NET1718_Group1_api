using Hosteland.WebJobs.Services.OrderService;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Hosteland.WebJobs {
    public class Functions {

        private readonly IOrderService _orderService;
        private readonly ILogger<Functions> _logger;

        public Functions(IOrderService orderService, ILogger<Functions> logger) {
            _orderService = orderService;
            _logger = logger;
        }

        public async Task Trigger(
            [TimerTrigger("0 * * * * *", RunOnStartup = false, UseMonitor = false)]
            TimerInfo timerInfo,
            CancellationToken cancellationToken) {
            await _orderService.TriggerMonthlyBill(cancellationToken);
        }
    }
}
