using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TodoListWPF
{
    /// <summary>
    /// Команды для кнопок
    /// </summary>
    public class DataCommands
    {
        public static RoutedCommand Delete { get; set; }
        public static RoutedCommand Edit { get; set; }
        public static RoutedCommand ShowDescription { get; set; }

        static DataCommands()
        {
            Edit = new RoutedCommand(nameof(Edit), typeof(DataCommands));
            Delete = new RoutedCommand(nameof(Delete), typeof(DataCommands));
            ShowDescription = new RoutedCommand(nameof(ShowDescription), typeof(DataCommands));
        }
    }
}
