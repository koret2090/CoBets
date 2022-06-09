using System.Collections.Generic;
using System.Threading.Tasks;
using CoBetsDatabase.Models;

namespace CoBetsDatabase.Repositories.Interfaces
{
    /**
     * \brief Интерфейс репозитория игроков
     */
    public interface IPlayerRepository : ICrudRepository<Player>
    {
        /**
         * Получение информации об игроке по его команде
         * \return Экземпляр игрока -- при успехе, null -- иначе
         */
        Task<List<Player>> GetPlayersByTeam(Team team);
    }
}