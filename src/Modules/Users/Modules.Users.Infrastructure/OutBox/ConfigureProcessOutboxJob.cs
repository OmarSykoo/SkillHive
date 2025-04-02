using Microsoft.Extensions.Options;
using Quartz;

namespace Modules.Users.Infrastructure.OutBox;

public class ConfigureProcessOutboxJob(IOptions<OutBoxOptions> outboxOptions) : IConfigureOptions<QuartzOptions>
{
    private readonly OutBoxOptions _outboxOptions = outboxOptions.Value;
    public void Configure(QuartzOptions options)
    {
        string jobName = typeof(ProcessOutboxJob).FullName!;
        var jobKey = new JobKey(jobName);

        options
        .AddJob<ProcessOutboxJob>(configure => configure.WithIdentity(jobKey))
        .AddTrigger(configure =>
            configure.ForJob(jobKey)
            .WithSimpleSchedule(
                schedule => schedule.WithIntervalInSeconds(_outboxOptions.TimeSpanInSeconds)
                                    .RepeatForever()
            )
        );
    }

}
