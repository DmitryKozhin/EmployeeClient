using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;

using Avalonia.Threading;

using EmployeeSkills.Client.Services;

using Newtonsoft.Json;

using ReactiveUI;

namespace EmployeeSkills.Client.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly EmployeesService _employeesService;
        private List<EmployeeViewModel> _employeesSource;
        private string _searchString;
        private EmployeeViewModel _selectedEmployee;
        private readonly List<long> _deletedEmployees;
        private ObservableCollection<EmployeeViewModel> _employees;
        private bool _isLoading;

        public MainWindowViewModel()
        {
            AddEmployeeCommand = ReactiveCommand.Create(ExecuteAddEmployee);
            SaveCommand = ReactiveCommand.Create(ExecuteSave);
            DeleteEmployeeCommand = ReactiveCommand.Create<EmployeeViewModel>(ExecuteDeleteEmployee);
            EditEmployeeCommand = ReactiveCommand.Create<EmployeeViewModel>(ExecuteEditEmployee);

            Employees = new ObservableCollection<EmployeeViewModel>();
            _deletedEmployees = new List<long>();

            var confAsString = File.ReadAllText("config.json");
            var appConfig = JsonConvert.DeserializeObject<ApplicationConfig>(confAsString);
            _employeesService = new EmployeesService(appConfig);
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                try
                {
                    IsLoading = true;
                    var employees = await _employeesService.PullEmployees();
                    _employeesSource = employees.ToList();
                    Employees = new ObservableCollection<EmployeeViewModel>(_employeesSource);
                    SelectedEmployee = _employeesSource.FirstOrDefault();
                }
                catch (Exception e)
                {
                    var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Error", e.Message);
                    await messageBox.Show();
                }
            }).ContinueWith(task => IsLoading = false);
        }

        public ReactiveCommand<Unit, Unit> AddEmployeeCommand { get; }
        public ReactiveCommand<EmployeeViewModel, Unit> DeleteEmployeeCommand { get; }
        public ReactiveCommand<EmployeeViewModel, Unit> EditEmployeeCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }

        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }

        public ObservableCollection<EmployeeViewModel> Employees
        {
            get => _employees;
            set => this.RaiseAndSetIfChanged(ref _employees, value);
        }

        public EmployeeViewModel SelectedEmployee
        {
            get => _selectedEmployee;
            set => this.RaiseAndSetIfChanged(ref _selectedEmployee, value);
        }

        public string SearchString
        {
            get => _searchString;
            set
            {
                _searchString = value;
                if (string.IsNullOrEmpty(_searchString))
                {
                    Employees = new ObservableCollection<EmployeeViewModel>(_employeesSource);
                    return;
                }

                var filteredEmployees = _employeesSource.Where(t => 
                    t.FullName.ToUpper().Contains(_searchString.ToUpper())).ToList();

                Employees = new ObservableCollection<EmployeeViewModel>(filteredEmployees);
            }
        }

        private void ExecuteAddEmployee()
        {
            var newEmployee = new EmployeeViewModel()
            {
                EditType = EditType.Create,
            };

            Employees.Add(newEmployee);
        }

        private void ExecuteDeleteEmployee(EmployeeViewModel employee)
        {
            if (employee.Id != default)
                _deletedEmployees.Add(employee.Id);

            Employees.Remove(employee);
        }

        private void ExecuteEditEmployee(EmployeeViewModel employeeForEdit)
        {
            if (employeeForEdit.IsEdit)
                employeeForEdit.ApplyChanges();

            employeeForEdit.IsEdit = !employeeForEdit.IsEdit;
            foreach (var employee in Employees.Where(employee => employee != employeeForEdit))
                employee.DiscardChanges();
        }

        private async void ExecuteSave()
        {
            try
            {
                await _employeesService.DeleteEmployees(_deletedEmployees);
                await _employeesService.PushChanges(Employees);
            }
            catch (Exception e)
            {
                var messageBox = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow("Error", e.Message);
                await messageBox.Show();
            }
        }
    }
}
