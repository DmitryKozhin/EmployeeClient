using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using EmployeeSkills.Client.Models;
using EmployeeSkills.Client.ViewModels;

using RestSharp;

namespace EmployeeSkills.Client.Services
{
    public class EmployeesService
    {
        private readonly IRestClient _restClient;

        public EmployeesService(ApplicationConfig config)
        {
            _restClient = new RestClient(config.ServerUrl);
        }

        public async Task DeleteEmployees(IEnumerable<long> deleteIds)
        {
            foreach (var deleteId in deleteIds)
            {
                var request = new RestRequest(deleteId.ToString(), Method.DELETE);
                var result = await _restClient.ExecuteAsync<Employee>(request);
                if (!result.IsSuccessful)
                    throw new InvalidOperationException();
            }
        }

        public async Task<IEnumerable<EmployeeViewModel>> PullEmployees()
        {
            var request = new RestRequest(Method.GET);
            var employees = await _restClient.ExecuteAsync<List<Employee>>(request);
            if (!employees.IsSuccessful)
                throw new InvalidOperationException(employees.ErrorMessage);

            return CreateFromModel(employees.Data);
        }

        public async Task PushChanges(IEnumerable<EmployeeViewModel> employeeViewModels)
        {
            var employees = employeeViewModels.Where(t => t.EditType != default).Select(CreateFromVm).ToList();
            var forCreate = employees.Where(t => t.Id == default);
            var forUpdate = employees.Except(forCreate);

            foreach (var employeeForCreate in forCreate)
            {
                var request = new RestRequest(Method.POST);
                request.AddJsonBody(employeeForCreate);
                var result = await _restClient.ExecuteAsync<Employee>(request);
                if (!result.IsSuccessful)
                    throw new InvalidOperationException(result.ErrorMessage);
            }

            foreach (var employeeForUpdate in forUpdate)
            {
                var request = new RestRequest(employeeForUpdate.Id.ToString(), Method.PUT);
                request.AddJsonBody(employeeForUpdate);
                var result = await _restClient.ExecuteAsync<Employee>(request);
                if (!result.IsSuccessful)
                    throw new InvalidOperationException(result.ErrorMessage);
            }
        }

        private static IEnumerable<EmployeeViewModel> CreateFromModel(List<Employee> employees)
        {
            return employees.Select(employee => new EmployeeViewModel(employee.Id, employee.Name,
                new ObservableCollection<SkillViewModel>(employee.Skills.Select(skill =>
                    new SkillViewModel(skill.Id, skill.Name, skill.Level)))));
        }

        private static Employee CreateFromVm(EmployeeViewModel employeeViewModel)
        {
            return new Employee
            {
                Id = employeeViewModel.Id,
                Name = employeeViewModel.FullName,
                Skills = employeeViewModel.Skills.Select(CreateFromVm).ToList()
            };
        }

        private static Skill CreateFromVm(SkillViewModel skillViewModel)
        {
            return new Skill
            {
                Id = skillViewModel.Id,
                Level = skillViewModel.Level,
                Name = skillViewModel.Name
            };
        }
    }
}