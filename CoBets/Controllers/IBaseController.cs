using System.Collections.Generic;
using System.Threading.Tasks;

namespace Controllers
{
     /**
     * \brief Интерфейс контроллеров
     */
    public interface IBaseController<T>
    {
        /**
         * Добавление элемента в базу данных
         * \param element Экземпляр элемента
         * \return Экземпляр, добавленного элемента -- при успехе,
         * null -- при неудаче
         */
        Task<T?> Add(T element);
        /**
         * Получение информации о всех элементах базы данных
         * \return Список элементов
         */
        Task<List<T>> GetAll();
        /**
         * Удаление элемента из базы данных
         * \param id Идентификатор элемента
         * \return true -- при успешном завершении операции,
         * false -- иначе
         */
        Task<bool> Delete(int id);
        /**
         * Получение информации об элементе по его id
         * \return Экземпляр элемента -- при успехе, null -- иначе
         */
        Task<T?> FindElementById(int id);
        /**
         * Получение информации об элементе по его имени
         * \return Экземпляр элемента -- при успехе, null -- иначе
         */
        Task<T?> FindElementByName(string name);
    }
}
