using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{
    public enum InputStages
    {
        /**
         * Начальное состояние
         */
        Start,
        /**
         * Состояние вывода информации о команде
         */
        TeamsInfoOutput,
        /**
         * Состояние ожидания ввода суммы
         */
        AmountInfoOutput,
        /**
         * Состояние вывода суммы
         */
        BetsInfoOutput
    }
    
    public class Handlers
    {
        private const string StartString = "/start";
        private const string MatchString = "List of matches for the next month";
        private const string BetBtnText = "Bet";
        private const string BetBtnCallback = "betButton";

        private const string AmountText = "Choose amount to bet:";
        private const string NotForkMatch = "The selected match can`t be settled because it is not a fork!\n" +
                                       "Please, select another.";
        
        private const double LowBorder = 100;
        private const double HighBorder = 50000;
        
        private const string Amount1 = "500";
        private const string Amount2 = "1000";
        private const string Amount3 = "5000";
        
        private readonly List<string> _amounts;

        private InputStages Stage { get; set; }

        private readonly BotMessages _messages;
        private readonly BotMenu _menu;

        public Handlers(BotMessages messages, BotMenu menu)
        {
            _messages = messages;
            _menu = menu;
            _amounts = new List<string>() { Amount1, Amount2, Amount3 };
        }

        private bool IsCorrectAmount(string amountStr)
        {
            var isNumber = double.TryParse(amountStr, NumberStyles.Any, CultureInfo.InvariantCulture, 
                    out var amount);
            
            if (isNumber)
            {
                return LowBorder <= amount && amount <= HighBorder;
            }
            return false;
        }

        public async Task Message(ITelegramBotClient botClient, CancellationToken cancellationToken, 
            Message message)
        {
            if (message.Text == StartString)
            {
                Stage = InputStages.Start; 
                await SendReply(botClient, cancellationToken, message.Chat.Id);
            }
            else if (IsCorrectAmount(message.Text))
            {
                Stage = InputStages.BetsInfoOutput;
                await SendReply(botClient, cancellationToken, message.Chat.Id, message.Text);
            }
        }
        
        public async Task CallbackQuery(ITelegramBotClient botClient, CancellationToken cancellationToken, 
            CallbackQuery callback)
        {
            if (callback.Data != BetBtnCallback)
            {
                Stage = InputStages.TeamsInfoOutput;
                await SendReply(botClient, cancellationToken, callback.Message!.Chat.Id, callback.Data);
            }
            else
            {
                Stage = InputStages.AmountInfoOutput;
                await SendReply(botClient, cancellationToken, callback.Message!.Chat.Id);
            }
        }

        private async Task SendReply(ITelegramBotClient botClient, CancellationToken cancellationToken,
            long chatId, string data = null)
        {
            switch (Stage)
            {
                case InputStages.Start:
                    await SendMatchList(botClient, cancellationToken, chatId);
                    break;
                case InputStages.TeamsInfoOutput:
                    await SendTeamsInfo(botClient, cancellationToken, chatId, data);
                    break;
                case InputStages.AmountInfoOutput:
                    await SendAmountInfo(botClient, cancellationToken, chatId);
                    break;
                case InputStages.BetsInfoOutput:
                    await SendBetsInfo(botClient, cancellationToken, chatId, data);
                    break;
            }
        }
        
        private async Task SendMessage(ITelegramBotClient botClient, CancellationToken cancellationToken,
            long chatId, string message, IReplyMarkup markup = null)
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: message,
                replyMarkup: markup,
                cancellationToken: cancellationToken);
        }

        private async Task SendMatchList(ITelegramBotClient botClient, CancellationToken cancellationToken, 
            long chatId)
        {
            var matches = _messages.FormMatchList();
            var menu = _menu.FormMatchListMenu(matches);

            await SendMessage(botClient, cancellationToken, chatId, MatchString, menu);
        }

        private async Task SendTeamsInfo(ITelegramBotClient botClient, CancellationToken cancellationToken,
            long chatId, string data)
        {
            var teamsInfo = await _messages.FormTeamsInfo(data);
            var menu = _menu.FormCoefsInfoMenu(BetBtnText, BetBtnCallback);
            
            await SendMessage(botClient, cancellationToken, chatId, teamsInfo[0]);
            await SendMessage(botClient, cancellationToken, chatId, teamsInfo[1], menu);
        }
        
        private async Task SendAmountInfo(ITelegramBotClient botClient, CancellationToken cancellationToken,
            long chatId)
        {
            var menu = _menu.FormAmountInfoMenu(_amounts);
            await SendMessage(botClient, cancellationToken, chatId, AmountText, menu);
        }
        
        private async Task SendBetsInfo(ITelegramBotClient botClient, CancellationToken cancellationToken,
            long chatId, string data)
        {
            var coefsInfo = _messages.FormBetsInfo(data);
            var menu = _menu.RemoveAmountBtnsMenu();
            
            if (coefsInfo == null)
            {
                await SendMessage(botClient, cancellationToken, chatId, NotForkMatch, menu);
            }
            else
            {
                await SendMessage(botClient, cancellationToken, chatId, coefsInfo[0], menu);
                await SendMessage(botClient, cancellationToken, chatId, coefsInfo[1]);
            }
        }
    }
}
