using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AppCoreX.Helpers;
using AppCoreX.Interface;
using AppCoreX.Models;
using AppCoreX.ViewModels;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace AppCore.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IComosDBService _comosDbService;
        private readonly IAppSettings _appSettings;

        private string _hello;

        public MainViewModel(IMvxNavigationService navigationService, IComosDBService comosDbService, IAppSettings appSettings)
        {
            _comosDbService = comosDbService;
            _navigationService = navigationService;
            _appSettings = appSettings;
        }

        public string Hello
        {
            get => _hello;
            set => SetProperty(ref _hello, value);

        }

        public IMvxCommand AddFamilyCommand => new MvxCommand(async () =>
         {
             await _navigationService.Navigate<CreateFamilyViewModel>();
         });
        public IMvxCommand SearchMvxCommand => new MvxCommand(async () =>
          {
              await _navigationService.Navigate<SearchFamilyViewModel>();
          });
        public IMvxCommand DatabaseMvxCommand => new MvxCommand(async () =>
        {
            await _navigationService.Navigate<DatabaseManagementViewModel>();
        });
        public override void Prepare()
        {
            Hello = "On Prepare";
            //if need to generate run class
            //var generate = new TutorialCode();
            //generate.Run();
            //check saved settings
            //set defaultdatabase
            TextConstants.DatabaseName = TextConstants.DefaultDatabaseName;
            TextConstants.CollectionName = TextConstants.DefaultCollectionName;

            if (string.IsNullOrEmpty(_appSettings.DatabaseName) ||
                string.IsNullOrEmpty(_appSettings.DocumentCollectionName)) return;
            TextConstants.DatabaseName = _appSettings.DatabaseName;
            TextConstants.CollectionName = _appSettings.DocumentCollectionName;

        }

    }

}
