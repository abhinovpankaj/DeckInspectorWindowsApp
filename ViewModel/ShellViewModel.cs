using CommonServiceLocator;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UI.Code.ViewModel
{
    public class ShellViewModel
    {
        public DelegateCommand UsersCommand { get; }
        public DelegateCommand ProjectsCommand { get; }

        public DelegateCommand OnLoadedCommand { get; }
        IRegionManager RegionManger
        {
            get
            {
                return (IRegionManager)Prism.Ioc.ContainerLocator.Container.Resolve(typeof(IRegionManager));
            }
        }
        public ShellViewModel()
        {
            // RegionManager.SetRegionManager(/*content control for region manager*/, RegionManger);
            UsersCommand = new DelegateCommand(GoToUsers);
            ProjectsCommand = new DelegateCommand(GoToProjects);

            OnLoadedCommand = new DelegateCommand(OnLoaded);
        }
        public void OnLoaded()
        {
            // RegionManger.RequestNavigate("MainRegion", "Login");
            RegionManger.RequestNavigate("MainRegion", "Login");
        }
        private void GoToUsers()
        {
            RegionManger.RequestNavigate("MainRegion", "Users");
        }
        private void GoToProjects()
        {
            RegionManger.RequestNavigate("MainRegion", "Projects");
        }
    }
}
