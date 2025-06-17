
using System;
using System.Threading.Tasks;
using metabot;
using metabot.Services;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var token = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN_SIRI");
        var connectionString = Environment.GetEnvironmentVariable("CONN_STRING");

        if (connectionString != null)
        {
            var dbcontext = new AppDbContext(connectionString);

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Please enter the TELEGRAM_BOT_TOKEN environment variable");
                return;
            }
        
            var contextProvider = new ChatContextProvider();
            
            var bot = new BotService(token, dbcontext, contextProvider);
            await bot.StartAsync();
        }
    }
}