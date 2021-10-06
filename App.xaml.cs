using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UI.Code.Model;
using UI.Code.Services;
using UI.Code.Style;
using Prism;
using Prism.Unity;
using UI.Code.View.Dialog;
using Unity;
using UI.Code.ViewModel;
using Prism.Ioc;
using System.ComponentModel;
using UI.Code.View;
using UI.Code.View.Navigation;
using UI.Code.Controls;
using Prism.Services.Dialogs;
using Prism.Mvvm;
using Prism.Regions;
using CommonServiceLocator;

namespace UI.Code
{

public enum Skin { Dark, Light }
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    ///  public enum Skin { Red, Blue }
    public partial class App : PrismApplication
    {
        //public static string AppUrl = "http://localhost:1169/";

        //public static string AppUrl = "http://xoricwebapi-prod.us-east-1.elasticbeanstalk.com/";
        public static string AppUrl = "http://api.deckinspectors.com/v2/";
        public static readonly Guid UserID = new Guid("5C60F693-BEF5-E011-A485-80EE7300C695");
        public static User LogUser = null;
        public static string ProjectID = string.Empty;
        public static string AppTempProjectID = string.Empty;
        public static Project MoveProject { get; set; }
        public static bool IsInvasive { get; set; }
        public App()
        {
            
        }
       
        protected override Window CreateShell()
        {
       //     IUnityContainer con = ServiceLocator.Current.GetInstance<IUnityContainer>();
            //con.Register<IRegionManager, RegionManager>();
            // Prism.Ioc.ContainerLocator.Container
            var w = Container.Resolve<Shell>();
            return w;
        }
        public static string Role { get; set; }
        public static string LogUserName { get; set; }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            IsInvasive = false;
            ViewModelLocationProvider.Register<ProjectsPageView, ProjectsPageViewModel>();
            var container = new UnityContainer();
            //  ServiceLocator.SetLocatorProvider(() => Prism.Ioc.ContainerLocator.Container.Resolve());
           
           // containerRegistry.Register<IRegionManager, RegionManager>();
            containerRegistry.RegisterDialog<NotificationDialog, NotificationDialogViewModel>("NotificationDialog");
            containerRegistry.Register<IUserService, UserService>();
            containerRegistry.Register<IProjectService, ProjectService>();
            containerRegistry.Register<IProjectLocation, ProjectLocationService>();
            containerRegistry.Register<IProjectBuilding, ProjectBuildingDataStore>();

            containerRegistry.Register<IBuildingApartment, BuildingApartmentDataStore>();
            containerRegistry.Register<IBuildingLocation, BuildingLocationDataStore>();

            containerRegistry.Register<IProjectCommonLocationImages, ProjectCommonLocationImagesService>();
            containerRegistry.Register<IBuildingApartmentImages, BuildingApartmentImagesDataStore>();
            containerRegistry.Register<IBuildingCommonLocationImages, BuildingCommonLocationImagesDataStore>();


            // containerRegistry.Register<IDialogService, DialogService>();
            containerRegistry.Register<IProjectCommonLocationImages, ProjectCommonLocationImagesService>();
            containerRegistry.Register<IVisualFormVisualProjectLocationDataStore, VisualProjectLocationService>();
            containerRegistry.Register<IVisualProjectLocationPhotoDataStore, VisualProjectLocationPhotoDataStore>();
            containerRegistry.Register<ITreeService, TreeService>();


            containerRegistry.Register<IVisualFormVisualProjectLocationDataStore, VisualProjectLocationService>();
            containerRegistry.Register<IVisualProjectLocationPhotoDataStore, VisualProjectLocationPhotoDataStore>();


            containerRegistry.Register<IVisualFormBuildingLocationDataStore, VisualFormBuildingLocationDataStore>();
            containerRegistry.Register<IVisualBuildingLocationPhotoDataStore, VisualBuildingLocationPhotoDataStore>();

            containerRegistry.Register<IVisualFormApartmentDataStore, VisualFormApartmentDataStore>();
            containerRegistry.Register<IVisualApartmentLocationPhotoDataStore, VisualApartmentLocationPhotoDataStore>();


            containerRegistry.RegisterForNavigation<ShellViewModel, Shell>("Shell");
            containerRegistry.RegisterForNavigation<LoginPageView>("Login");
            containerRegistry.RegisterForNavigation<UsersPageView,UsersPageViewModel>("Users");
            containerRegistry.RegisterForNavigation<ProjectsPageView>("Projects");
            containerRegistry.RegisterForNavigation<AdminNavigation>("AdminNav");
            containerRegistry.RegisterForNavigation<UserNavigation>("UserNav");
            containerRegistry.RegisterForNavigation<ProjectAddOrEdit, ProjectAddOrEditViewModel>("ProjectAddOrEdit");
            containerRegistry.RegisterForNavigation<ReportView>("Report");
            containerRegistry.RegisterForNavigation<LocationDetail>("LocationDetail");
            containerRegistry.RegisterForNavigation<DummyView>("DummyView");
            containerRegistry.RegisterForNavigation<LogInfo,LogInfoViewModel>("LogInfo");
            containerRegistry.RegisterForNavigation<UserAddOrEdit, UserAddOrEditViewModel>("UserAddEdit");

            // containerRegistry.RegisterForNavigation<TreeViewPage, ProjectsPageViewModel>("Tree");
            containerRegistry.RegisterForNavigation<UCFilePageView, UCFilePageViewModel>("FileUC");
            containerRegistry.RegisterForNavigation<FilePageView, FilePageViewModel>("File");
            containerRegistry.RegisterForNavigation<ProjectPageView,ProjectViewModel>("Project");
            containerRegistry.RegisterForNavigation<AssigeProjectView, AssigeProjectViewModel>("Assign");
            containerRegistry.RegisterForNavigation<BuildingPageView, BuildingViewModel>("Building");
            containerRegistry.RegisterForNavigation<DetailViewProjectLocationImage, DetailViewProjectLocationImageViewModel>("DetailLocation");
            containerRegistry.RegisterForNavigation<DetailViewBuildingLocationImage, DetailViewBuildingLocationImageViewModel>("DetailBuildingLocation");
            containerRegistry.RegisterForNavigation<DetailViewBuildingApartmentImage, DetailViewBuildingApartmentImageViewModel>("DetailApartment");

            containerRegistry.RegisterForNavigation<ProjectLocationAddOrEdit, ProjectLocationAddOrEditViewModel>("ProjectLocationAddEdit");
            containerRegistry.RegisterForNavigation<BuildingAddOrEdit, BuildingAddOrEditViewModel>("BuildingAddOrEdit");
            containerRegistry.RegisterForNavigation<BuildingLocationAddOrEdit, BuildingLocationAddOrEditViewModel>("BuildingLocationAddOrEdit");
            containerRegistry.RegisterForNavigation<BuildingApartmentAddOrEdit, BuildingAparmentEditViewModel>("BuildingApartmentAddOrEdit");
            containerRegistry.RegisterForNavigation<VisualProjectLocationView, VisualProjectLocationViewModel>("VisualProjectLocation");
            containerRegistry.RegisterForNavigation<VisualBuildingLocationView, VisualBuildingLocationViewModel>("VisualBuildingLocation");
            containerRegistry.RegisterForNavigation<VisualApartmentView, VisualApartmentViewModel>("VisualApartmentView");


            containerRegistry.RegisterForNavigation<InvasiveVisualProjectLocationView, VisualProjectLocationViewModel>("InvasiveVisualProjectLocationView");
            containerRegistry.RegisterForNavigation<InvasiveVisualBuildingLocationView, VisualBuildingLocationViewModel>("InvasiveVisualBuildingLocationView");
            containerRegistry.RegisterForNavigation<InvasiveVisualApartmentView, VisualApartmentViewModel>("InvasiveVisualApartmentView");

        }



        //protected override void OnStartup(StartupEventArgs e)
        //{

        //    //container.RegisterType<IShoppingCartService, ShoppingCartService>();
        //    base.OnStartup(e);
        //    IUnityContainer container = new UnityContainer();
        //    container.RegisterType<IUserService, UserService>();
        //    container.RegisterType<IProjectService, ProjectService>();
        //    container.RegisterType<IProjectLocation, ProjectLocationService>();
        //    container.RegisterType<IProjectCommonLocationImages, ProjectCommonLocationImagesService>();
        //    BootStrapper bootStrapper = new BootStrapper();
        //    bootStrapper.Run();
        //}
        public static Skin Skin { get; set; } = Skin.Light;
        //public void ChangeSkin(Skin newSkin)
        //{
        //    Skin = newSkin;

        //    foreach (ResourceDictionary dict in Resources.MergedDictionaries)
        //    {

        //        if (dict is SkinResourceDictionary skinDict)
        //            skinDict.UpdateSource();
        //        else
        //            dict.Source = dict.Source;
        //    }
        //}
        // (App.Current as App).ChangeSkin(Skin.Blue);
      //  RefreshNiceSquare();
    }
}
