using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UI.Code.Model;
using UI.Code.View.Dialog;

namespace UI.Code.ViewModel
{
    public class LogInfoViewModel : BaseViewModel, INavigationAware
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }

      

      

        public LogInfoViewModel(IDialogService dialogService)
        {
            Title = App.LogUser.FullName;

            //  Title = "Project Detail";


            //  SubmitCommand = new DelegateCommand(async () => await Submit());
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Title = App.LogUser.FullName;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }
    }
}
