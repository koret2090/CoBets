namespace TelegramBot
{
    /**
     * \brief Класс, отвечающий за расчёты, связанные со ставками.
     *
     * Служит для проверки возможности сущестсвования "вилки",
     * расчёта суммы ставок на исходы по известным коеффициентам матча. 
     */
    public class BetManager
    {
        /**
         * Коэффициент на первую команду в матче
         */
        private double Coef1 { get; }
        /**
         * Коэффициент на вторую команду в матче
         */
        private double Coef2 { get; }
        /**
         * Доступная сумма для ставок
         */
        private double BetSum { get; }
        /**
         * Ставка на первую команду в матче
         */
        public double Bet1 { get; private set; }
        /**
         * Ставка на вторую команду в матче
         */
        public  double Bet2 { get; private set; }
        
        /**
         * Конструктор
         * \param Coef1 Коэффициент на первую команду в матче
         * \param Coef2 Коэффициент на вторую команду в матче
         * \param BetSum Доступная сумма для ставок
         */
        public BetManager(double Coef1, double Coef2, double BetSum)
        {
            this.Coef1 = Coef1;
            this.Coef2 = Coef2;
            this.BetSum = BetSum;
        }
        
        /**
         * Проверка является ли ситуация "вилкой"
         * \return true -- если ситуация является вилкой, false -- иначе
         */
        public bool IsFork()
        {
            return 1 / Coef1 + 1 / Coef2 < 1;
        }
        
        /**
         * Высчитывает суммы ставок на первую и вторую команду
         */
        public void CalculateBets()
        {
            double coefFork = (1 / Coef1 + 1 / Coef2);
            Bet1 = 1 / Coef1 / coefFork * BetSum;
            Bet2 = 1 / Coef2 / coefFork * BetSum;
        }
        
        /**
         * Высчитывает потенциальный выигрыш денег при победе первой команды
         * \return Потенциальный выигрыш денег при победе первой команды
         */
        public double GetPotentialGainBet1()
        {
            return Coef1 * Bet1;
        }
        
        /**
         * Высчитывает потенциальный выигрыш денег при победе второй команды
         * \return Потенциальный выигрыш денег при победе второй команды
         */
        public double GetPotentialGainBet2()
        {
            return Coef2 * Bet2;
        }
    }
}
