using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using VoiceTexterBot;
using VoiceTexterBot.Controllers;

internal class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.Unicode;
 
        // Объект, отвечающий за постоянный жизненный цикл приложения
        var host = new HostBuilder()
            .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
            .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
            .Build(); // Собираем
 
        Console.WriteLine("Сервис запущен");
        // Запускаем сервис
        await host.RunAsync();
        Console.WriteLine("Сервис остановлен");
    }

    static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<TextMessageController>();
        
        // Регистрируем объект TelegramBotClient c токеном подключения
        services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("6776734156:AAFGlUBuMBSU8Cdlf77hJNHm_SpF6nJ7LaM"));
        // Регистрируем постоянно активный сервис бота
        services.AddHostedService<Bot>();
    }
}