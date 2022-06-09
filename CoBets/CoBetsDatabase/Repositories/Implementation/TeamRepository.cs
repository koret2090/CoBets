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
     * \brief Класс-репозиторий для команд
     *
     * Служит для реализации действий, связанных с командами и базой данных
     */
    public class TeamRepository : ITeamRepository, IDisposable
    {
        /**
         * Контекст базы данных
         */
        private readonly CoBetsDbContext _database;
        
        /**
         * Конструктор
         * \param database Контекст базы данных
         */
        public TeamRepository(CoBetsDbContext database)
        {
            _database = database;
        }
        
        /**
         * Добавление команды в базу данных
         * \param element Экземпляр команды
         * \return Экземпляр, добавленной команды -- при успехе,
         * null -- при неудаче
         */
        public async Task<Team?> Add(Team element)
        {
            try
            {
                var addedTeam = await _database.Teams.AddAsync(element);
                await _database.SaveChangesAsync();
                return addedTeam.Entity;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        
        /**
         * Получение информации о всех командах базы данных
         * \return Список команд
         */
        public async Task<List<Team>> GetAll()
        {
            return await _database.Teams.OrderBy(t => t.Id).ToListAsync();
        }
        
        /**
         * Удаление команды из базы данных
         * \param id Идентификатор команды
         * \return true -- при успешном завершении операции,
         * false -- иначе
         */
        public async Task<bool> Delete(int id)
        {
            try
            {
                var team = await _database.Teams.FirstOrDefaultAsync(t => t.Id == id);
                if (team != null)
                {
                    _database.Teams.Remove(team);
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
         * Получение информации о команде по ее id
         * \return Экземпляр команды -- при успехе, null -- иначе
         */
        public async Task<Team?> FindElementById(int id)
        {
            var team = await _database.Teams.FindAsync(id); 
            return team;
        }
        
        /**
         * Получение информации о команде по ее названию
         * \return Экземпляр команды -- при успехе, null -- иначе
         */
        public async Task<Team?> FindElementByName(string name)
        {
            return await _database.Teams.Where(team => team.Name == name).FirstOrDefaultAsync();
        }

        public async void Dispose()
        {
            await _database.DisposeAsync();
        }
    }
}