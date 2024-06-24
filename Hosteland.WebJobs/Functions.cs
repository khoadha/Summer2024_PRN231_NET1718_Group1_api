using Microsoft.Azure.WebJobs;
using Hosteland.Services;

namespace Hosteland.WebJobs {
    public class Functions {

        public Functions()
        {
        }

        // runs every 5 minutes and on startup
        //public async Task MyTimerTriggerOperation([TimerTrigger("0 */1 * * * *", RunOnStartup = true)] TimerInfo timerInfo, CancellationToken cancellationToken) {
        //    await Task.Delay(100, cancellationToken);
        //}
    }
}
