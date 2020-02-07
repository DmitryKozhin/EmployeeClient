using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;

using ReactiveUI;

namespace EmployeeSkills.Client.ViewModels
{
    public class EmployeeViewModel : ViewModelBase
    {
        public EmployeeViewModel()
        {
            AddSkill = ReactiveCommand.Create(ExecuteAddSkill);
            DeleteSkill = ReactiveCommand.Create<SkillViewModel>(ExecuteDeleteSkill);
            Skills = new ObservableCollection<SkillViewModel>();
        }

        public Guid Id { get; set; }
        public string FullName { get; set; }
        public ObservableCollection<SkillViewModel> Skills { get; set; }

        public ReactiveCommand<Unit, Unit> AddSkill { get; }
        public ReactiveCommand<SkillViewModel, Unit> DeleteSkill { get; }

        public EditType EditType { get; private set; }

        private void NewSkillOnPropertyChanged(object sender, PropertyChangedEventArgs e)
            => EditType = EditType.Update;

        private void ExecuteAddSkill()
        {
            var lastSkill = Skills.LastOrDefault();
            if (lastSkill != null && lastSkill.EditType != EditType.Update)
                return;

            var newSkill = new SkillViewModel()
            {
                Level = 0,
                Name = string.Empty,
                EditType = EditType.Create,
            };

            newSkill.PropertyChanged += NewSkillOnPropertyChanged;
            Skills.Add(newSkill);
            EditType = EditType.Update;
        }

        private void ExecuteDeleteSkill(SkillViewModel skill)
        {
            skill.PropertyChanged -= NewSkillOnPropertyChanged;
            Skills.Remove(skill);
            EditType = EditType.Update;
        }
    }
}