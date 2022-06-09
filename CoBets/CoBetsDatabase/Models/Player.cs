using System.ComponentModel.DataAnnotations;

namespace CoBetsDatabase.Models
{
    /**
     * \brief Класс Игрока
     *
     * Служит для реализации модели игрока 
     */
    public class Player
    {
        [Key]
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public int Position { get; set; }
        
        public Team Team { get; set; }
        
        /**
         * Конструктор
         * \param id Идентификатор игрока
         * \param teamId Идентификатор команды
         * \param name Имя игрока
         * \param position Позиция игрока в команде
         */
        public Player(int id, int teamId, string name, int number, int position)
        {
            Id = id;
            TeamId = teamId;
            Name = name;
            Number = number;
            Position = position;
        }
    }
}
