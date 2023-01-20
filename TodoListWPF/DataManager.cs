using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TodoListWPF
{
    /// <summary>
    /// Работа с хранилищем данных
    /// </summary>
    public static class DataManager
    {
        // путь к папке, где храниться xml с данными
        private static readonly string dataFolder = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));

        // название файла
        private const string TASKS_FILE_NAME = "tasks.xml";

        // полный путь к файлу
        private static string tasksFilePath;

        public static List<Task> Tasks { get; private set; } = new List<Task>();

        /// <summary>
        /// Загрузка даных из файла
        /// </summary>
        public static bool LoadData()
        {
            try
            {
                // формируем полный путь к файлу
                tasksFilePath = Path.Combine(dataFolder, TASKS_FILE_NAME);

                LoadTasks();

                return true;
            }
            catch (Exception e)
            {
                // произошла ошибка при считывании данных из xml
                return false;
            }
        }

        /// <summary>
        /// Загрузка информации о задачах
        /// </summary>
        private static void LoadTasks()
        {
            if (!File.Exists(tasksFilePath)) return;

            DataContractSerializer dcs = new DataContractSerializer(Tasks.GetType());
            using (FileStream fs = new FileStream(tasksFilePath, FileMode.Open))
            {
                using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas()))
                {
                    Tasks = (List<Task>)dcs.ReadObject(reader);
                }
            }
        }

        /// <summary>
        /// Обновление файла с задачами
        /// </summary>
        public static bool UpdateTasks(List<Task> tasks)
        {
            try
            {
                Tasks = tasks;

                SerializeTasks();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Сериализация списка задач в файл XML
        /// </summary>
        private static void SerializeTasks()
        {
            DataContractSerializer dcs = new DataContractSerializer(Tasks.GetType());

            using (Stream stream = new FileStream(tasksFilePath, FileMode.Create, FileAccess.Write))
            {
                using (XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8))
                {
                    writer.WriteStartDocument();
                    dcs.WriteObject(writer, Tasks);
                }
            }
        }
    }
}
