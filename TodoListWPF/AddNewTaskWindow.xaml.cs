using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace TodoListWPF
{
    /// <summary>
    /// Interaction logic for AddNewTaskWindow.xaml
    /// </summary>
    public partial class AddNewTaskWindow : Window
    {
        /// <summary>
        /// Задача для редактирования/добавления
        /// </summary>
        public Task Task { get; private set; } = new Task();

        public AddNewTaskWindow(Task task = null)
        {
            InitializeComponent();

            Title = task == null ? "Добавление задачи" : "Редактирование задачи";

            if (task != null)
            {
                // т.к. в параметре передается ссылка на объект, а у нас есть возможсноть отменить редактирование, то создаем копию данного объекта
                CreateTaskCopy(task);

                NameTextBox.Text = task.Name;
                DescTextBox.Text = task.Description;
                ImportantCheckBox.IsChecked = ImportantCheckBox.IsChecked;
                DatePicker.SelectedDate = task.DateTime;
                TimeText.Text = task.DateTime?.ToString("HH:mm") ?? string.Empty;
            }
        }

        private void CreateTaskCopy(Task task)
        {
            Task = new Task
            {
                Name = task.Name,
                Description = task.Description,
                DateTime = task.DateTime,
                IsCompleted = task.IsCompleted,
                IsImportant = task.IsImportant
            };
        }


        /// <summary>
        /// Обработчик нажатия на кнопку сохранения
        /// </summary>
        private void Save_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameTextBox.Text))
            {
                MessageBox.Show("Название задачи не заполнено!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Task.Name = NameTextBox.Text;
            Task.Description = DescTextBox.Text;
            Task.DateTime = DatePicker.SelectedDate;

            // пытаемся преобразовать текст из строки с временем в формат TimeSpan и если получилось, то прибавляем это время к выбранной дате
            if (Task.DateTime.HasValue && !string.IsNullOrEmpty(TimeText.Text) && TimeSpan.TryParseExact(TimeText.Text, "hh\\:mm", null, out TimeSpan time))
            {
                Task.DateTime = Task.DateTime.Value.Date.AddMilliseconds(time.TotalMilliseconds);
            }

            Task.IsImportant = ImportantCheckBox.IsChecked ?? false;
            Task.CreatedDate = DateTime.Now;

            DialogResult = true;
        }
    }
}
