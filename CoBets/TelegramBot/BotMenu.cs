using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{
    /**
     * \brief Класс, отвечающий за различные меню
     */
    public class BotMenu
    {
        /**
         * Формирование списка матчей
         * \param matches Список матчей
         * \return menu Меню с выбором матчей
         */
        public InlineKeyboardMarkup FormMatchListMenu(IReadOnlyList<string> matches)
        {
            var btns = new List<InlineKeyboardButton[]>();
            for (var i = 0; i < matches.Count; i++)
            {
                var curBtn = InlineKeyboardButton.WithCallbackData(matches[i], i.ToString());
                btns.Add(new[] {curBtn});
            }
            
            var menu = new InlineKeyboardMarkup(btns);
            return menu;
        }
        
        public ReplyKeyboardMarkup FormAmountInfoMenu(List<string> btnsText)
        {
            var btns = new List<KeyboardButton[]>();
            foreach (var btn in btnsText)
            {
                var curBtn = new KeyboardButton(btn);
                btns.Add(new[] {curBtn});
            }
            
            var menu = new ReplyKeyboardMarkup(btns);
            return menu;
        }

        public ReplyKeyboardRemove RemoveAmountBtnsMenu()
        {
            var menu = new ReplyKeyboardRemove();
            return menu;
        }
        
        /**
         * Формирование списка матчей
         * \param matches Список матчей
         * \return menu Меню с выбором матчей
         */
        public InlineKeyboardMarkup FormCoefsInfoMenu(string btnText, string btnCallback)
        {
            var curBtn = InlineKeyboardButton.WithCallbackData(btnText, btnCallback);
            var menu = new InlineKeyboardMarkup(curBtn);

            return menu;
        }
    }
}
