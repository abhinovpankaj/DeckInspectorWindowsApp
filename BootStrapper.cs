using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UI.Code.Controls;
using UI.Code.View;
using UI.Code.View.Dialog;
using UI.Code.View.Navigation;
using UI.Code.ViewModel;

namespace UI.Code
{
    [Obsolete]
    public class BootStrapper : UnityBootstrapper
    {
        #region Overridden Methods      
        /// <summary>      
        /// Entry point to the application      
        /// </summary>      
        /// <param name="runWithDefaultConfiguration"></param>      
        public override void Run(bool runWithDefaultConfiguration)
        {
            base.Run(runWithDefaultConfiguration);
        }


        /// <summary>      
        /// Initializes shell.xaml      
        /// </summary>      
        /// <returns></returns>      
        protected override DependencyObject CreateShell()
        {
            return Container.TryResolve<Shell>();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.RegisterTypeForNavigation<ShellViewModel,Shell>("Shell");
            Container.RegisterTypeForNavigation<LoginPageView>("Login");
            Container.RegisterTypeForNavigation<UserPageView>("Users");
            Container.RegisterTypeForNavigation<ProjectsPageView>("Projects");
            Container.RegisterTypeForNavigation<AdminNavigation>("AdminNav");
            Container.RegisterTypeForNavigation<ProjectDetailPageView>("ProjectDetail");
            Container.RegisterTypeForNavigation<ReportView>("Report");
            Container.RegisterTypeForNavigation<LocationDetail>("LocationDetail");
            Container.RegisterTypeForNavigation<DummyView>("DummyView");
           // Container.RegisterDialog<NotificationDialog, NotificationDialogViewModel>();

        }
        /// <summary>      
        /// loads the Shell.xaml      
        /// </summary>      
        protected override void InitializeShell()
        {
            //var login = new LoginWindow();
            //var loginVM = new LoginPageViewModel();

            //loginVM.LoginCompleted += (sender, args) =>
            //{
            //    Application.Current.MainWindow.Show();
            //    login.Close();
            //};
            //login.DataContext = loginVM;
            //login.ShowDialog();
            //App.Current.MainWindow = (Window)Shell;
            App.Current.MainWindow.Show();
        }
        

        /// <summary>      
        /// Add view(module) from other assemblies and begins with modularity      
        /// </summary>      
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            Type ModuleLocatorType = typeof(Presentation.ModuleLocators);
            ModuleCatalog.AddModule(new Prism.Modularity.ModuleInfo
            {
                ModuleName = ModuleLocatorType.Name,
                ModuleType = ModuleLocatorType.AssemblyQualifiedName
            });
        }
        #endregion
    }
}
