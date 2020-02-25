using System.Collections.Generic;
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
        private string _tempName;

        public EmployeeViewModel(long id = default, string fullName = "", ObservableCollection<SkillViewModel> skills = null)
        {
            AddSkillCommand = ReactiveCommand.Create(ExecuteAddSkill);
            DeleteSkillCommand = ReactiveCommand.Create<SkillViewModel>(ExecuteDeleteSkill);
            Skills = skills ?? new ObservableCollection<SkillViewModel>();
            Id = id;
            _fullName = fullName;
            EditButtonImagePath = EDIT_BUTTON_IMAGE_PATH;

            foreach (var skillViewModel in Skills)
                skillViewModel.PropertyChanged += SkillViewModelOnPropertyChanged;
        }

        private void SkillViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
            => EditType = EditType.Update;

        public ReactiveCommand<Unit, Unit> AddSkillCommand { get; }
        public ReactiveCommand<SkillViewModel, Unit> DeleteSkillCommand { get; }

        public string EditButtonImagePath
        {
            get => _editButtonImagePath;
            set => this.RaiseAndSetIfChanged(ref _editButtonImagePath, value);
        }

        public string FullName
        {
            get => _fullName;
            set
            {
                this.RaiseAndSetIfChanged(ref _fullName, value);
                EditType = EditType.Update;
            }
        }

        public string TempName
        {
            get => string.IsNullOrEmpty(_tempName) ? FullName : _tempName;
            set => this.RaiseAndSetIfChanged(ref _tempName, value);
        }

        public long Id { get; set; }

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

            Skills.Add(newSkill);
            EditType = EditType.Update;
        }

        private void ExecuteDeleteSkill(SkillViewModel skill)
        {
            Skills.Remove(skill);
            skill.PropertyChanged -= SkillViewModelOnPropertyChanged;
            EditType = EditType.Update;
        }

        public void DiscardChanges()
        {
            IsEdit = false;
            TempName = FullName;
        }

        public void ApplyChanges()
            => FullName = TempName;
    }
}