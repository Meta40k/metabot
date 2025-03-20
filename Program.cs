using metabot;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var token = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN_SIRI");

        if (string.IsNullOrEmpty(token))
        {
            Console.WriteLine("Please enter the TELEGRAM_BOT_TOKEN environment variable");
            return;
        }
        
        
        var bot = new BotService(token);
        await bot.StartAsync();
    }
}