using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Acr.UserDialogs;
using AppCoreX.Interface;
using AppCoreX.Models;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace AppCoreX.ViewModels
{
    public class SearchFamilyViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IComosDBService _comosDbService;
        private readonly IUserDialogs _userDialogs;
        private string _searchInputText;

        public string SearchInputText
        {
            get => _searchInputText;
            set => SetProperty(ref _searchInputText, value);
        }

        private Family _selectedFamily;

        public Family SelectedFamily
        {
            get { return _selectedFamily; }
            set { SetProperty(ref _selectedFamily, value); }
        }
        
        private ObservableCollection<Family> _families;

        public ObservableCollection<Family> Families
        {
            get => _families;
            set => SetProperty(ref _families, value);
        }
        public IMvxCommand SearchTextMvxCommand => new MvxCommand(async () =>
         {
             if (string.IsNullOrEmpty(SearchInputText))
             {
                 await _userDialogs.AlertAsync("Enter LastName to Search", "Error");
                 return;
             }
             Families = await _comosDbService.ExecuteSimpleQuery(SearchInputText);
             if (Families.Count == 0) await _userDialogs.AlertAsync("no Families found", "Search Result");

             SearchInputText = null;

         });
        public IMvxCommand ViewFamilyMvxCommand => new MvxCommand(async () =>
        {
            if (SelectedFamily != null) await _navigationService.Navigate<CreateFamilyViewModel,Family>(SelectedFamily);
           SelectedFamily = null;
            Families = new ObservableCollection<Family>();
        });
        public SearchFamilyViewModel(IMvxNavigationService navigationService, IComosDBService comosDbService, IUserDialogs userDialogs)
        {
            _navigationService = navigationService;
            _comosDbService = comosDbService;
            _userDialogs = userDialogs;
        }


    }
}
