using System.Collections.Generic;
using System.Threading.Tasks;
using CoBetsDatabase.Models;
using CoBetsDatabase.Repositories.Interfaces;

namespace Controllers
{
    /**
     * \brief Контроллер команд
     */
    public class TeamController : IBaseController<Team>
    {
        /**
         * Репозиторий команды
         */
        private readonly ITeamRepository _teamRepository;
        
        /**
         * Конструктор
         * \param database Репозиторий команды
         */
        public TeamController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }
        
        /**
         * Добавление команды в базу данных
         * \param element Экземпляр команды
         * \return Экземпляр, добавленной команды -- при успехе,
         * null -- при неудаче
         */
        public Task<Team?> Add(Team element)
        {
            return _teamRepository.Add(element);
        }
        
        /**
         * Получение информации о всех командах базы данных
         * \return Список команд
         */
        public Task<List<Team>> GetAll()
        {
            return _teamRepository.GetAll();
        }
        
        /**
         * Удаление команды из базы данных
         * \param id Идентификатор команды
         * \return true -- при успешном завершении операции,
         * false -- иначе
         */
        public Task<bool> Delete(int id)
        {
            return _teamRepository.Delete(id);
        }
        
        /**
         * Получение информации о команде по ее id
         * \return Экземпляр команды -- при успехе, null -- иначе
         */
        public Task<Team?> FindElementById(int id)
        {
            return _teamRepository.FindElementById(id);
        }
        
        /**
         * Получение информации о команде по ее названию
         * \return Экземпляр команды -- при успехе, null -- иначе
         */
        public Task<Team?> FindElementByName(string name)
        {
            return _teamRepository.FindElementByName(name);
        }
    }
}
