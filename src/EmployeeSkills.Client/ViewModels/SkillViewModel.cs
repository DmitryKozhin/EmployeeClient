using System;
using System.Reactive;

using ReactiveUI;

namespace EmployeeSkills.Client.ViewModels
{
    public class SkillViewModel : ViewModelBase
    {
        private byte _level;
        private string _name;

        public Guid Id { get; set; }
        public string Name
        {
            get => _name;
            set
            {
                this.RaiseAndSetIfChanged(ref _name, value);
                EditType = EditType.Update;
            }
        }

        public byte Level
        {
            get => _level;
            set
            {
                this.RaiseAndSetIfChanged(ref _level, value);
                EditType = EditType.Update;
            }
        }

        public EditType EditType { get; set; }
    }
}