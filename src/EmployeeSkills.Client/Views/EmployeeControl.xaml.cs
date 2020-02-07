using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace EmployeeSkills.Client.Views
{
    public class EmployeeControl : UserControl
    {
        public EmployeeControl()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
