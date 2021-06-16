using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Code.Model;
using UI.Code.Services;
using Unity;

namespace UI.Code.ViewModel
{
    public class WelcomePageViewModel : BaseViewModel
    {
   
        public WelcomePageViewModel()
        {

            Load();
        }
        public async Task Load()
        {
           
          //  int t= (await userService.GetItemsAsync()).ToList().Count;
        }
        #region Properties      
        /// <summary>      
        /// This string property will have default text for demo purpose.    
        /// </summary>      
        private string _imGoodByeText = "This is binded from WelcomePageViewModel, Thank you for being part of this Blog!";
        /// <summary>      
        /// This string property will be binded with Textblock on view       
        /// </summary>      
        public string ImGoodByeText
        {
            get { return _imGoodByeText; }
            set { _imGoodByeText = value; }
        }
        #endregion
    }
}
