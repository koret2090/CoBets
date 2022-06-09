using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoBetsDatabase.Models;
using CoBetsDatabase.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoBetsDatabase.Repositories.Implementation
{
    /**
     * \brief Класс-репозиторий для игроков
     *
     * Служит для реализации действий, связанных с игроками и базой данных
     */
    public class PlayerRepository : IPlayerRepository, IDisposable
    {
        /**
         * Контекст базы данных
         */
        private readonly CoBetsDbContext _database;
        
        /**
         * Конструктор
         * \param database Контекст базы данных
         */
        public PlayerRepository(CoBetsDbContext database)
        {
            _database = database;
        }
        
        /**
         * Добавление игрока в базу данных
         * \param element Экземпляр игрока
         * \return Экземпляр, добавленного игрока -- при успехе,
         * null -- при неудаче
         */
        public async Task<Player?> Add(Player element)
        {
            try
            {
                var addedPlayer = await _database.Players.AddAsync(element);
                await _database.SaveChangesAsync();
                return addedPlayer.Entity;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        
        /**
         * Получение информации о всех игроках базы данных
         * \return Список игроков
         */
        public async Task<List<Player>> GetAll()
        {
            return await _database.Players.OrderBy(p => p.Id).ToListAsync();
        }
        
        /**
         * Удаление игрока из базы данных
         * \param id Идентификатор игрока
         * \return true -- при успешном завершении операции,
         * false -- иначе
         */
        public async Task<bool> Delete(int id)
        {
            try
            {
                var player = await _database.Players.FirstOrDefaultAsync(p => p.Id == id);
                if (player != null)
                {
                    _database.Players.Remove(player);
                    await _database.SaveChangesAsync();
                    return true;        
                }

                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        /**
         * Получение информации об игроке по его id
         * \return Экземпляр игрока -- при успехе, null -- иначе
         */
        public async Task<Player?> FindElementById(int id)
        {
            return await _database.Players.FindAsync(id); 
        }
        
        /**
         * Получение информации об игроке по его имени
         * \return Экземпляр игрока -- при успехе, null -- иначе
         */
        public async Task<Player?> FindElementByName(string name)
        {
            return await _database.Players.Where(player => player.Name == name).FirstOrDefaultAsync();
        }
        
        /**
         * Получение информации об игроке по его команде
         * \return Экземпляр игрока -- при успехе, null -- иначе
         */
        public async Task<List<Player>> GetPlayersByTeam(Team team)
        {
            return await _database.Players.Where(needed => needed.Team.Name == team.Name).ToListAsync();
        }

        public async void Dispose()
        {
            await _database.DisposeAsync();
        }
    }
}