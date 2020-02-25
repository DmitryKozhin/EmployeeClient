using ReactiveUI;

namespace EmployeeSkills.Client.ViewModels
{
    public class SkillViewModel : ViewModelBase
    {
        private byte _level;
        private string _name;

        public SkillViewModel(int id = default, string name = "", byte level = default)
        {
            Id = id;
            _name = name;
            _level = level;
        }

        public int Id { get; set; }

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
    }
}