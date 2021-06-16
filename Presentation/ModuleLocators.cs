using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Code.Presentation
{
    public class ModuleLocators : IModule
    {
        #region private properties        
        /// <summary>        
        /// Instance of IRegionManager        
        /// </summary>        
        private IRegionManager _regionManager;

        #endregion

        #region Constructor        
        /// <summary>        
        /// parameterized constructor initializes IRegionManager        
        /// </summary>        
        /// <param name="regionManager"></param>        
        public ModuleLocators(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
        #endregion

        #region Interface methods        
        /// <summary>        
        /// Initializes Welcome page of your application.        
        /// </summary>        
        /// <param name="containerProvider"></param>        
        public void OnInitialized(IContainerProvider containerProvider)
        {
           // _regionManager.RegisterViewWithRegion("Shell", typeof(ModuleLocators));  //ModuleLocators is added for testing purpose,     
            _regionManager.RegisterViewWithRegion("Shell", typeof(View.WelcomePageView));                                                       //later we'll replace it with WelcomePageView      
        }
       
        /// <summary>        
        /// RegisterTypes used to register modules        
        /// </summary>        
        /// <param name="containerRegistry"></param>        
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
        #endregion
    }
}
