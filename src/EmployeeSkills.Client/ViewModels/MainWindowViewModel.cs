using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using EmployeeSkills.Client.Models;

using ReactiveUI;

namespace EmployeeSkills.Client.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly List<EmployeeViewModel> _employeesSource;
        private string _searchString;
        private EmployeeViewModel _selectedEmployee;
        private ObservableCollection<EmployeeViewModel> _employees;

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
            Employees = new ObservableCollection<EmployeeViewModel>(_employeesSource);
            SelectedEmployee = Employees.First();
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
    }
}
