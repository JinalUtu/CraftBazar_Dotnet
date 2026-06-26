using Hangfire;

namespace CraftBazar_Controller.Jobs;

public class RegistrationEmailJob
{
    public Task SendRegistrationEmail(string email, DateTime? registrationTime = null)
    {
        // How to stop the recurring background jobs
        if (DateTime.UtcNow > registrationTime?.AddMinutes(2))
        {
            RecurringJob.RemoveIfExists("test-recurring-job");
            return Task.CompletedTask;
        }
        Console.WriteLine($"Registration email sent to: {email}");
        Console.WriteLine("Subject: Welcome to CraftBazar");
        Console.WriteLine("Body: Thank you for registering. This is a dummy email sent by Hangfire.");
        return Task.CompletedTask;
    }

    public Task TestFailure()
    {
        throw new Exception("Test Exception");
    }
}
