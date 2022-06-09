using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;

namespace TelegramBot
{
    /**
     * \brief Основной класс бота
     */
    public class Bot : IHostedService
    {
        private readonly TelegramBotClient _client;
        private readonly CancellationTokenSource _tokenSrc;
        private readonly DatabaseManager _databaseManager;
        private readonly Handlers _handlers;
        private readonly ILogger _logger;

        public Bot(ILogger<Bot> logger, string token, DatabaseManager databaseManager, Handlers handlers)
        {
            _client = new TelegramBotClient(token);
            _tokenSrc = new CancellationTokenSource();
            _databaseManager = databaseManager;
            _handlers = handlers;
            _logger = logger;
        }

        private void FillDatabase()
        {
            _logger.LogInformation("Start filling teams");
            Task.WaitAll(_databaseManager.AddTeams());
            _logger.LogInformation("Teams table is filled");
            
            _logger.LogInformation("Start filling players");
            Task.Run(_databaseManager.AddPlayers);
            _logger.LogInformation("Players table is filled");

        }
        
        /**
         * Запуск бота
         */
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting bot...");
            return Task.Run(Run, cancellationToken);
        }
        
        private void Run()
        {
            var receiverOptions = new ReceiverOptions {AllowedUpdates = { }};
            
            _client.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, _tokenSrc.Token);
            _logger.LogInformation("Bot has been started.");
        }
        
        /**
         * Остановка бота
         */
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _tokenSrc.Cancel();
            _logger.LogInformation("Stopping bot...");
            return Task.CompletedTask;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            //return;
            if (update.Type == UpdateType.Message &&
                update.Message!.Type == MessageType.Text)
            {
                await _handlers.Message(botClient, cancellationToken, update.Message);
            }
            else if (update.Type == UpdateType.CallbackQuery &&
                update.CallbackQuery?.Message!.Type == MessageType.Text)
            {
                await _handlers.CallbackQuery(botClient, cancellationToken, update.CallbackQuery);
            }
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError("Error: " + exception.Message);
            return Task.CompletedTask;
        }
    }
}
