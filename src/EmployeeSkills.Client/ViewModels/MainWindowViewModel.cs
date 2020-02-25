using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

using ReactiveUI;

namespace EmployeeSkills.Client.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly List<EmployeeViewModel> _employeesSource;
        private string _searchString;
        private EmployeeViewModel _selectedEmployee;

        public MainWindowViewModel()
        {
            _employeesSource = new List<EmployeeViewModel>()
            {
                new EmployeeViewModel()
                {
                    FullName = "First Employee", 
                    Skills = new ObservableCollection<SkillViewModel>()
                    {
                        new SkillViewModel(){ Level=5, Name = "Skill 1" },
                        new SkillViewModel(){ Level=3, Name = "Skill 2" }
                    }
                },
                new EmployeeViewModel()
                {
                    FullName = "Second Employee",
                    Skills = new ObservableCollection<SkillViewModel>()
                    {
                        new SkillViewModel(){ Level=2, Name = "Skill 1" },
                        new SkillViewModel(){ Level=5, Name = "Skill 2" }
                    }
                },
                new EmployeeViewModel()
                {
                    FullName = "Third Employee",
                    Skills = new ObservableCollection<SkillViewModel>()
                    {
                        new SkillViewModel(){ Level=1, Name = "Skill 1" },
                        new SkillViewModel(){ Level=4, Name = "Skill 2" }
                    }
                }
            };

            AddEmployeeCommand = ReactiveCommand.Create(ExecuteAddEmployee);
            DeleteEmployeeCommand = ReactiveCommand.Create<EmployeeViewModel>(ExecuteDeleteEmployee);
            EditEmployeeCommand = ReactiveCommand.Create<EmployeeViewModel>(ExecuteEditEmployee);
            Employees = new ObservableCollection<EmployeeViewModel>(_employeesSource);
            SelectedEmployee = Employees.First();
        }

        public ReactiveCommand<Unit, Unit> AddEmployeeCommand { get; }
        public ReactiveCommand<EmployeeViewModel, Unit> DeleteEmployeeCommand { get; }
        public ReactiveCommand<EmployeeViewModel, Unit> EditEmployeeCommand { get; }

        public ObservableCollection<EmployeeViewModel> Employees { get; set; }
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
            Employees.Remove(employee);
        }

        private void ExecuteEditEmployee(EmployeeViewModel employeeForEdit)
        {
            employeeForEdit.IsEdit = !employeeForEdit.IsEdit;
            foreach (var employee in Employees.Where(employee => employee != employeeForEdit))
            {
                employee.IsEdit = false;
            }
        }
    }
}
