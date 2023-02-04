using Gazel;
using Gazel.Scheduling;

namespace Inventiv.Sample.Module.Todo.Jobs;

public class ReminderJob : IScheduledJob // Job classes should implement this interface to be registered as sheduled jobs
{
    private const int DEFAULT_BATCH_COUNT = 100;

    private readonly IModuleContext _context;

    // Like all public classes, you can inject any service here
    public ReminderJob(IModuleContext context)
    {
        _context = context;

        BatchCount = DEFAULT_BATCH_COUNT;
    }

    // Properties are initialized automatically from web.config when setter is public.
    public int BatchCount { get; set; }

    // Execute method is called when the job is triggered
    public void Execute()
    {
        // This job fetches tasks as with a TOP X query
        // For batch jobs it is a good practice to handle only allowed amount of records at one execution.
        // When you register this job to run every 10 seconds from 07:00 to 08:00, there will be 2 hours = 3600 seconds = 360 executions every day
        // If batch count is 100 this means this job will work correctly as long as the volume is less than 36000 tasks to remind every day.
        foreach (var task in _context.Query<TaskCards>().TakeBy(BatchCount, _context.System.Today.AddDays(1).ToDateTime(), false))
        {
            task.RemindUser();
        }
    }
}
