using Microsoft.Extensions.Logging;
using Quartz;
using System.Diagnostics;

namespace CleanKit.Net.Application
{
    public abstract class BackgroundJob : IJob
    {
        protected readonly ILogger Logger;

        protected BackgroundJob(ILogger logger)
        {
            Logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //Setting up the stopwatch
            var sp = new Stopwatch();
            sp.Start();

            try
            {
                Logger.LogDebug("Started background job at {@DateTimeUtc}", DateTime.UtcNow);
                await ExecuteAsync(context);
                Logger.LogDebug(
                    "Completed background job at {@DateTimeUtc} in {@ElapsedMilliseconds}ms",
                    DateTime.UtcNow,
                    sp.ElapsedMilliseconds
                );
            }
            catch (Exception exception)
            {
                Logger.LogError(
                    "{@Type} occurred at {@DateTimeUtc} in {@ElapsedMilliseconds}ms: {@Message}\n{@StackTrace}",
                    exception.GetType().Name,
                    DateTime.UtcNow,
                    sp.ElapsedMilliseconds,
                    exception.Message,
                    exception.StackTrace
                );
            }
        }

        public virtual Task ExecuteAsync(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
