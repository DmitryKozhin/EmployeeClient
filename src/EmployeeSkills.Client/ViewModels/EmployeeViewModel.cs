using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive;

using ReactiveUI;

namespace EmployeeSkills.Client.ViewModels
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class EmployeeViewModel : ViewModelBase
    {
        private static readonly string EDIT_BUTTON_IMAGE_PATH = $@"{BASE_PATH}\Assets\edit.png";
        private static readonly string OK_BUTTON_IMAGE_PATH = $@"{BASE_PATH}\Assets\done.png";

        private string _fullName;
        private bool _isEdit;
        private string _editButtonImagePath;

        public EmployeeViewModel()
        {
            AddSkillCommand = ReactiveCommand.Create(ExecuteAddSkill);
            DeleteSkillCommand = ReactiveCommand.Create<SkillViewModel>(ExecuteDeleteSkill);
            Skills = new ObservableCollection<SkillViewModel>();
            EditButtonImagePath = EDIT_BUTTON_IMAGE_PATH;
        }

        public ReactiveCommand<Unit, Unit> AddSkillCommand { get; }
        public ReactiveCommand<SkillViewModel, Unit> DeleteSkillCommand { get; }

        public string EditButtonImagePath
        {
            get => _editButtonImagePath;
            set => this.RaiseAndSetIfChanged(ref _editButtonImagePath, value);
        }

        public EditType EditType { get; set; }

        public string FullName
        {
            get => _fullName;
            set => this.RaiseAndSetIfChanged(ref _fullName, value);
        }

        public long Id { get; }

        public bool IsEdit
        {
            get => _isEdit;
            set
            {
                this.RaiseAndSetIfChanged(ref _isEdit, value);
                EditButtonImagePath = value ? OK_BUTTON_IMAGE_PATH : EDIT_BUTTON_IMAGE_PATH;
            }
        }

        public ObservableCollection<SkillViewModel> Skills { get; set; }

        private void ExecuteAddSkill()
        {
            var lastSkill = Skills.LastOrDefault();
            if (lastSkill != null && lastSkill.EditType != EditType.Update)
                return;

            var newSkill = new SkillViewModel
            {
                Level = 0,
                Name = string.Empty,
                EditType = EditType.Create
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

        private void NewSkillOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            EditType = EditType.Update;
        }
    }
}