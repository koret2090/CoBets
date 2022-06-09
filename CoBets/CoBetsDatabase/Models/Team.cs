using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoBetsDatabase.Models
{
    /**
     * \brief Класс Команды
     *
     * Служит для реализации модели команды 
     */
    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        
        public List<Player> Players { get; set; }
        
        /**
         * Конструктор
         * \param id Идентификатор команды
         * \param name Название команды
         */
        public Team(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
