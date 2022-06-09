using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace TelegramBot
{
    /**
     * \brief Класс, отвечающий за формирования сообщений
     */
    public class BotMessages
    {
        private const string MatchesFile = "../../../../Parsers/CoefsParser/coefs.csv";
        private const string DateFormatFrom = "yyyy-MM-ddTHH:mm:ssZ";
        private const string DateFormatTo = "dd.MM HH:mm";
        
        /**
         * Мэнеджер базы данных
         */
        private readonly DatabaseManager _databaseManager;
        /**
         * Список матчей
         */
        private readonly List<string?> _matches;
        /**
         * Индекс матча
         */
        private int _matchIdx;
        
        /**
         * Конструктор
         * \param databaseManager Мэнеджер базы данных
         */
        public BotMessages(DatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;

            using (var reader = new StreamReader(MatchesFile))
            {
                _matches = new List<string?>();
                _matchIdx = -1;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    _matches.Add(line);
                }
            }
        }
        
        /**
         * Формирование списка матчей
         * \return matchList Список матчей
         */
        public List<string> FormMatchList()
        {
            var matchList = new List<string>();
            foreach (var match in _matches)
            {
                var values = match!.Split(',');

                var time = ParseTime(values[0]);
                matchList.Add(time + " || " +
                              values[1] + "  &  " + values[2]);
            }

            return matchList;
        }
        
        /**
         * Формирование времени
         * \return Отформатированное время
         */
        private string ParseTime(string value)
        {
            var time = DateTime.ParseExact(value, DateFormatFrom, CultureInfo.InvariantCulture);
            return time.ToString(DateFormatTo);
        }
        
        /**
         * Формирование информации о командах
         * \param idxStr Индекс строки
         * \return matchInfo Информация о матче
         */
        public async Task<List<string>> FormTeamsInfo(string idxStr)
        {
            var idx = int.Parse(idxStr);
            _matchIdx = idx;
            var match = _matches[_matchIdx];

            var values = match!.Split(',');
            var team1 = values[1].Replace(" ", "-");
            var team2 = values[2].Replace(" ", "-");

            var teamInfo1 = await _databaseManager.GetTeamInfo(team1);
            var teamInfo2 = await _databaseManager.GetTeamInfo(team2);
            
            var matchInfo = new List<string>
            {
                teamInfo1,
                teamInfo2
            };
            
            return matchInfo;
        }
        
        /**
         * Формирование информации о ставках и коэффициентах 
         * \return coefsInfo Информация о ставках и коэффициентах
         */
        public List<string>? FormBetsInfo(string amount)
        {
            var match = _matches[_matchIdx];
            var values = match!.Split(',');
            
            var betNameStr1 = values[3]; 
            var coefStr1 = values[4];
            
            var betNameStr2 = values[5]; 
            var coefStr2 = values[6];

            var sum = ParseStringToDouble(amount);
            var coef1 = ParseStringToDouble(coefStr1, true);
            var coef2 = ParseStringToDouble(coefStr2, true);

            var betManager = new BetManager(coef1, coef2, sum);
            if (!betManager.IsFork())
            {
                return null;
            }
            
            var betsInfo = FormBetsInfoList(betManager, betNameStr1, betNameStr2);
            return betsInfo;
        }
        
        private double ParseStringToDouble(string value, bool isCoef = false)
        {
            if (isCoef)
            {
                value = value.Replace("' ", "");
            }
            double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var dValue);
            return dValue;
        }

        private List<string> FormBetsInfoList(BetManager betManager, string betNameStr1, string betNameStr2)
        {
            betManager.CalculateBets();
            
            var bet1 = betManager.Bet1;
            var gain1 = betManager.GetPotentialGainBet1();
            
            var bet2 = betManager.Bet2;
            var gain2 = betManager.GetPotentialGainBet2();

            var betInfo1 = FormBetInfoString(betNameStr1, bet1, gain1);
            var betInfo2 = FormBetInfoString(betNameStr2, bet2, gain2);

            var betsInfoList = new List<string>
            {
                betInfo1,
                betInfo2
            };
            
            return betsInfoList;
        }

        private string FormBetInfoString(string betName, double bet, double gain)
        {
            var coefsInfo = betName + "\n" +
                             "Bet:  " + bet.ToString("C") + "\n" +
                             "Gain: " + gain.ToString("C");
            return coefsInfo;
        }
    }
}
