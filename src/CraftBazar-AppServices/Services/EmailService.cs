public class EmailService
{
    public Task SendEmailAsync(string to, string subject, string body)
    {
        Console.WriteLine($"[DummyEmail] Email sent to: {to}");
        Console.WriteLine($"[DummyEmail] Subject: {subject}");
        Console.WriteLine($"[DummyEmail] Body: {body}");
        return Task.CompletedTask;
    }
}