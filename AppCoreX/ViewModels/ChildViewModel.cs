using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Acr.UserDialogs;
using AppCoreX.Helpers;
using AppCoreX.Models;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace AppCoreX.ViewModels
{
    public class ChildViewModel : MvxViewModel<Child, Child>
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;

        private Child _child;

        public Child Child
        {
            get => _child;
            set => SetProperty(ref _child, value);
        }

        private Pet _pet;

        public Pet Pet
        {
            get => _pet;
            set => SetProperty(ref _pet, value);
        }


        private string _petName;

        public string PetName
        {
            get => _petName;
            set => SetProperty(ref _petName, value);
        }

        private int _selectedIndex;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                SetProperty(ref _selectedIndex, value);
                if(value>=0)
                PetName = Child.Pets[value].GivenName;
            } 
        }

        public IMvxCommand AddPetMvxCommand => new MvxCommand(() =>
        {
            if (string.IsNullOrEmpty(PetName)) return;
            _userDialogs.ShowLoading(TextConstants.AddingMessage);
            Child.Pets.Add(Pet = new Pet{ GivenName = PetName });
            PetName = null;
            _userDialogs.HideLoading();

        });
        public IMvxCommand EditPetMvxCommand => new MvxCommand(() =>
        {
            if (SelectedIndex.Equals(null)) return;
            if (string.IsNullOrEmpty(PetName)) return;
            _userDialogs.ShowLoading(TextConstants.EditingMessage);
            Child.Pets[SelectedIndex] = new Pet{GivenName = PetName};
            PetName = null;
            SelectedIndex = -1;
            _userDialogs.HideLoading();
        });

        public IMvxCommand DeletePetMvxCommand => new MvxCommand(() =>
        {
            if (SelectedIndex.Equals(null)) return;
            _userDialogs.ShowLoading(TextConstants.DeletingMessage);
            Child.Pets.RemoveAt(SelectedIndex);
            PetName = null;
            _userDialogs.HideLoading();

        });

        public IMvxCommand SaveMvxCommand => new MvxCommand(async () =>
        {
            await _navigationService.Close(this,Child);
        });
        public IMvxCommand CancelMvxCommand => new MvxCommand(async () =>
        {
            await _navigationService.Close(this, null);
        });

        public ChildViewModel(IMvxNavigationService navigationService, IUserDialogs userDialogs)
        {
            //check if pet has been instantiated
            if (Pet == null) Pet = new Pet();
            _navigationService = navigationService;
            _userDialogs = userDialogs;
        }

        public override void Prepare(Child child)
        {
            Child = child ?? new Child() { Pets = new ObservableCollection<Pet>() };
            Child.Pets = child?.Pets ?? new ObservableCollection<Pet>();
        }
    }
}
