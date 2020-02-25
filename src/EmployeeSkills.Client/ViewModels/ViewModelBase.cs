using System.IO;

using ReactiveUI;

namespace EmployeeSkills.Client.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        protected static readonly string BASE_PATH = Directory.GetCurrentDirectory();
        private static readonly string DELETE_BUTTON_IMAGE_PATH = $@"{BASE_PATH}\Assets\delete.png";
        public string DeleteButtonImagePath => DELETE_BUTTON_IMAGE_PATH;
    }
}
