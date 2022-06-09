using System.Text;
using System.Threading.Tasks;
using CoBetsDatabase.Models;
using Controllers;

namespace TelegramBot
{
    /**
     * \brief Класс, отвечающий за действия, связанные с базой данных и запросами бота
     */
    public class DatabaseManager
    {
        private const string PlayersPath = "../../../../Parsers/TeamsParser/players.csv"; 
        private const string TeamsPath = "../../../../Parsers/TeamsParser/teams.csv";
        
        /**
         * Контроллер игроков
         */
        private readonly PlayerController _playerController;
        /**
         * Контроллер команд
         */
        private readonly TeamController _teamController;
        
        /**
         * Конструктор
         * \param playerController Контроллер игроков
         * \param teamController Контроллер команд
         */
        public DatabaseManager(PlayerController playerController, TeamController teamController)
        {
            _playerController = playerController;
            _teamController = teamController;
        }
        
        /**
         * Добавление игроков в базу данных
         * \param PlayersPath Путь к файлу с игроками
         */
        public async Task AddPlayers()
        {
            foreach (string line in System.IO.File.ReadLines(PlayersPath))
            {
                var attributes = line.Split(",");
                var checkPlayerId = int.TryParse(attributes[0], out var playerId);
                var checkTeamId = int.TryParse(attributes[1], out var teamId);
                var name = attributes[2];
                var checkNumber = int.TryParse(attributes[3], out var number);
                var checkPosition = int.TryParse(attributes[4], out var position);

                var player = new Player(
                    checkPlayerId ? playerId : -1,
                    checkTeamId ? teamId : -1,
                    name,
                    checkNumber ? number : -1,
                    checkPosition ? position : -1
                );
                Task.WaitAll(_playerController.Add(player));
            }
        }
        
        /**
         * Добавление команд в базу данных
         * \param TeamsPath Путь к файлу с командами
         */
        public async Task AddTeams()
        {
            foreach (string line in System.IO.File.ReadLines(TeamsPath))
            {
                var attributes = line.Split(",");
                var checkTeamId = int.TryParse(attributes[0], out var teamId);
                var name = attributes[1];
                var team = new Team(
                    checkTeamId ? teamId : -1,
                    name
                );
                Task.WaitAll(_teamController.Add(team));
            }
        }
        
        /**
         * Получение информации о команде по ее названию
         * \param teamName Название команды
         * \return Информация о команде
         */
        public async Task<string> GetTeamInfo(string teamName)
        {
             var sb = new StringBuilder();
             sb.Append(teamName + "\n------------------------------------------------------\n");
             
             var team = await _teamController.FindElementByName(teamName);
             var players = await _playerController.GetPlayersByTeam(team);

             sb.Append("Num \t\t\t Pos \t\t\t Name\n");
             sb.Append("------------------------------------------------------\n");
             
             foreach (var player in players)
             {
                 sb.AppendFormat("{0, -10} {1, -10} {2}\n", player.Number, player.Position, player.Name);
             }

             return sb.ToString();
        }
    }
}
