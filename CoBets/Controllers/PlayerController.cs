using System.Collections.Generic;
using System.Threading.Tasks;
using CoBetsDatabase.Models;
using CoBetsDatabase.Repositories.Interfaces;

namespace Controllers
{
    /**
     * \brief Контроллер игроков
     */
    public class PlayerController : IBaseController<Player>
    {
        /**
         * Репозиторий игроков
         */
        private readonly IPlayerRepository _playerRepository;
        
        /**
         * Конструктор
         * \param playerRepository Репозиторий игроков
         */
        public PlayerController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }
        
        /**
         * Добавление игрока в базу данных
         * \param element Экземпляр игрока
         * \return Экземпляр, добавленного игрока -- при успехе,
         * null -- при неудаче
         */
        public Task<Player?> Add(Player element)
        {
            return _playerRepository.Add(element);
        }
        
        /**
         * Получение информации о всех игроках базы данных
         * \return Список игроков
         */
        public Task<List<Player>> GetAll()
        {
            return _playerRepository.GetAll();
        }
        
        /**
         * Удаление игрока из базы данных
         * \param id Идентификатор игрока
         * \return true -- при успешном завершении операции,
         * false -- иначе
         */
        public Task<bool> Delete(int id)
        {
            return _playerRepository.Delete(id);
        }
        
        /**
         * Получение информации об игроке по его id
         * \return Экземпляр игрока -- при успехе, null -- иначе
         */
        public Task<Player?> FindElementById(int id)
        {
            return _playerRepository.FindElementById(id);
        }
        
        /**
         * Получение информации об игроке по его имени
         * \return Экземпляр игрока -- при успехе, null -- иначе
         */
        public Task<Player?> FindElementByName(string name)
        {
            return _playerRepository.FindElementByName(name);
        }
        
        /**
         * Получение информации об игроке по его команде
         * \return Экземпляр игрока -- при успехе, null -- иначе
         */
        public Task<List<Player>> GetPlayersByTeam(Team team)
        {
            return _playerRepository.GetPlayersByTeam(team);
        }
    }
}
