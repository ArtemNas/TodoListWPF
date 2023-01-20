using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TodoListWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Хранит в себе список всех задач
        /// </summary>
        private List<Task> allTasks = new List<Task>();

        /// <summary>
        /// Отфильтровать коллекцию с задачами в соответствием с выбранной вкладкой
        /// </summary>
        private void FilterTasksCollection()
        {
            ObservableCollection<Task> filteredTasks = new ObservableCollection<Task>();

            switch (TaskTab.SelectedIndex)
            {
                case 0:
                    // выбрана вкладка Все задачи
                    filteredTasks = new ObservableCollection<Task>(allTasks.OrderByDescending(x => x.CreatedDate));
                    AllTasksList.ItemsSource = filteredTasks;
                    AllTasksNoneTB.Visibility = filteredTasks.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case 1:
                    // выбрана вкладка Сегодня задачи
                    filteredTasks = new ObservableCollection<Task>(allTasks.Where(x => x.DateTime.HasValue && x.DateTime.Value.Date == DateTime.Now.Date).OrderByDescending(x => x.CreatedDate)); ;
                    TodayTasksList.ItemsSource = filteredTasks;
                    TodayTasksNoneTB.Visibility = filteredTasks.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case 2:
                    // выбрана вкладка Важные задачи
                    filteredTasks = new ObservableCollection<Task>(allTasks.Where(x => x.IsImportant).OrderByDescending(x => x.CreatedDate));
                    ImportantTasksList.ItemsSource = filteredTasks;
                    ImportantTasksNoneTB.Visibility = filteredTasks.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case 3:
                    // выбрана вкладка Просроченые задачи
                    filteredTasks = new ObservableCollection<Task>(allTasks.Where(x => !x.IsCompleted && x.DateTime.HasValue && x.DateTime.Value < DateTime.Now).OrderByDescending(x => x.CreatedDate));
                    ExpiredTasksList.ItemsSource = filteredTasks;
                    ExpiredTasksNoneTB.Visibility = filteredTasks.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case 4:
                    // выбрана вкладка Выполнено задачи
                    filteredTasks = new ObservableCollection<Task>(allTasks.Where(x => x.IsCompleted).OrderByDescending(x => x.CreatedDate));
                    DoneTasksList.ItemsSource = filteredTasks;
                    DoneTasksNoneTB.Visibility = filteredTasks.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
                    break;
            }
        }


        public MainWindow()
        {
            InitializeComponent();

            // если данные удалось успешно загрузить, то заполняем список задач из файла
            if (DataManager.LoadData())
            {
                allTasks = DataManager.Tasks;
            }
            else
            {
                MessageBox.Show("Не удалось загрузить задачи из XML файла.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool isLoaded;

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
        }

        /// <summary>
        /// Событие выбора вкладки
        /// </summary>
        private void TaskTab_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(!isLoaded) return;

            // при первичной загрузке индекс выбранной вкладки может сбрасываться на -1, в таком случае проставляем 0 - первую вкладку
            if (TaskTab.SelectedIndex == -1)
            {
                TaskTab.SelectedIndex = 0;
                return;
            }

            FilterTasksCollection();
        }

        private void AddTaskButton_OnClick(object sender, RoutedEventArgs e)
        {
            // открываем диалоговое окно с добавлением задачи
            AddNewTaskWindow addNewTaskWindow = new AddNewTaskWindow();

            var dialogResult = addNewTaskWindow.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                allTasks.Add(addNewTaskWindow.Task);
                FilterTasksCollection();
                SaveData();
            }
        }

        /// <summary>
        /// Сохранение данных в файл
        /// </summary>
        private void SaveData()
        {
            if (!DataManager.UpdateTasks(allTasks))
            {
                MessageBox.Show("Не удалось сохранить данные в XML файл.", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработка нажатия на кнопку редактирования
        /// </summary>
        private void EditTask_Click(object sender, ExecutedRoutedEventArgs e)
        {
            // преобразуем параметр команды к типу Task
            Task currentTask = e.Parameter as Task;
            if (currentTask != null)
            {
                // открываем диалоговое окно с редактированием задачи
                AddNewTaskWindow editTaskWindow = new AddNewTaskWindow(currentTask);

                var dialogResult = editTaskWindow.ShowDialog();
                if (dialogResult.HasValue && dialogResult.Value)
                {
                    currentTask.Name = editTaskWindow.Task.Name;
                    currentTask.Description = editTaskWindow.Task.Description;
                    currentTask.IsImportant = editTaskWindow.Task.IsImportant;
                    currentTask.DateTime = editTaskWindow.Task.DateTime;
                    currentTask.CreatedDate = editTaskWindow.Task.CreatedDate;

                    SaveData();

                    FilterTasksCollection();
                }
            }
        }

        /// <summary>
        /// Обработка нажатия на кнопку удаления
        /// </summary>
        private void DeleteTask_Click(object sender, ExecutedRoutedEventArgs e)
        {
            // преобразуем параметр команды к типу Task
            Task currentTask = e.Parameter as Task;
            if (currentTask != null)
            {
                // удаляем задачу из общего списка и сохраняем результат
                allTasks.Remove(currentTask);
                FilterTasksCollection();
                SaveData();
            }
        }

        /// <summary>
        /// Обработка нажатия на кнопку просмотра информации о задаче
        /// </summary>
        private void ShowDescription_Click(object sender, ExecutedRoutedEventArgs e)
        {
            // преобразуем параметр команды к типу Task
            Task currentTask = e.Parameter as Task;
            if (currentTask != null)
            {
                MessageBox.Show(currentTask.Description, $"Описание задачи {currentTask.Name}");
            }
        }
    }
}
