using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListWPF
{

    /// <summary>
    /// Объект, хранящий в себе информацию о задаче
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Название задачи
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание задачи
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дата создания задачи
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Дата и время в которое необходимо напомнить о задаче
        /// </summary>
        public DateTime? DateTime { get; set; }

        /// <summary>
        /// Флаг того, что задача выполнена
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Флаг того, что задача важная
        /// </summary>
        public bool IsImportant { get; set; }
    }
}
