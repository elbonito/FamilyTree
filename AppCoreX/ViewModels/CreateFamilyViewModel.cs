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
    public class CreateFamilyViewModel : MvxViewModel<Family>
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IComosDBService _comosDbService;
        private readonly IUserDialogs _userDialogs;
        private Family _family;

        public CreateFamilyViewModel(IMvxNavigationService navigationService, IComosDBService comosDbService, IUserDialogs dialogs)
        {
            _userDialogs = dialogs;
            _navigationService = navigationService;
            _comosDbService = comosDbService;
        }
        private string _familyName;

        public string FamilyName
        {
            get => _familyName;
            set
            {
                SetProperty(ref _familyName, value);
                Family.LastName = value;
            }
        }

        public Family Family
        {
            get => _family;
            set => SetProperty(ref _family, value);
        }
        private ObservableCollection<Parent> _parents;

        public ObservableCollection<Parent> Parents
        {
            get => _parents;
            set => SetProperty(ref _parents, value);
        }

        private Parent _selectParent;

        public Parent SelectedParent
        {
            get => _selectParent;
            set => SetProperty(ref _selectParent, value);
        }

        private int _selectedChildIndex;

        public int SelectedChildIndex
        {
            get => _selectedChildIndex;
            set => SetProperty(ref _selectedChildIndex, value);
        }
        public IMvxCommand AddParent => new MvxCommand(async () =>
          {
              var newParent = await _navigationService.Navigate<ParentViewModel, Parent, Parent>(new Parent { FamilyName = FamilyName });
              if (newParent == null) return;
              //add parent to list
              Family.Parents.Add(newParent);
          });
        public IMvxCommand EditParentCommand => new MvxCommand(async () =>
          {
              //todo when the parentviewmodel closes tempParent remains, so instead of remove i should replace by also storing the
              //index of the current selected item.
              if (SelectedParent == null) return;
              var tempParent = SelectedParent;
              Family.Parents.Remove(SelectedParent);
              var editedParent = await _navigationService.Navigate<ParentViewModel, Parent, Parent>(tempParent);
              if (editedParent == null) return;
              Family.Parents.Add(editedParent);
          });

        public IMvxCommand AddChildCommand => new MvxCommand(async () =>
        {
            var newChild = await _navigationService.Navigate<ChildViewModel, Child, Child>(new Child { FamilyName = FamilyName });
            if (newChild == null) return;
            //add parent to list
            Family.Children.Add(newChild);
        });
        public IMvxCommand EditChildMvxCommand => new MvxCommand(async () =>
         {
             if (SelectedChildIndex < 0) return;
             var child = Family.Children[SelectedChildIndex];
             var editedChild = await _navigationService.Navigate<ChildViewModel, Child, Child>(child);
             if (editedChild == null) return;
             Family.Children[SelectedChildIndex] = editedChild;
         });
        public IMvxCommand DeleteChildMvxCommand => new MvxCommand(() =>
        {
            if (SelectedChildIndex < 0) return;
            Family.Children.RemoveAt(SelectedChildIndex);
        });

        public IMvxCommand<Child> SelectedChildCommand => new MvxCommand<Child>((child) =>
        {
            //Family.Parents.Remove(parent);

        });

        public IMvxCommand SaveFamilyMvxCommand => new MvxCommand(async () =>
          {
              //check to make sure values are not null
              if (string.IsNullOrEmpty(Family.LastName) || string.IsNullOrEmpty(Family.Address.City) ||
                  string.IsNullOrEmpty(Family.Address.County) || string.IsNullOrEmpty(Family.Address.State))
              {
                  await _userDialogs.AlertAsync("Family Name, County, City, State Cannot be Empty", "Attention Required Family");
                  return;
              }

              if (Family.Parents.Count == 0 || Family.Parents.Equals(null))
              {
                  await _userDialogs.AlertAsync("Must have at least one parent", "Attention Required Family");
                  return;
              }
              //create id for family instead of lastname plus number im using guid
              Family.Id = Guid.NewGuid().ToString();
              await _comosDbService.CreateFamilyDocumentIfNotExists(Family);
              await _navigationService.Close(this);

          });
        public IMvxCommand CancelFamilyMvxCommand => new MvxCommand(() =>
        {
            _navigationService.Close(this);
        });
        public IMvxCommand EditFamilyMvxCommand => new MvxCommand(async () =>
        {
            if (string.IsNullOrEmpty(Family.Id) || Family == null) return;
            await _comosDbService.ReplaceFamilyDocument(Family);
            await _navigationService.Close(this);

        });
        public IMvxCommand DeleteFamilyMvxCommand => new MvxCommand(async () =>
        {
            if (string.IsNullOrEmpty(Family.Id) || Family == null) return;
            await _comosDbService.DeleteFamilyDocument(Family.Id);
            await _navigationService.Close(this);

        });

        private bool _canEdit;

        public bool CanEdit
        {
            get => _canEdit;
            set => SetProperty(ref _canEdit, value);
        }

        public override void Prepare()
        {
            Parents = new ObservableCollection<Parent>();
            Family = new Family
            {
                Parents = new ObservableCollection<Parent>(),
                Children = new ObservableCollection<Child>(),
                Address = new Address()

            };
        }

        public override void Prepare(Family family)
        {
            Family = family;
            FamilyName = family.LastName;
            CanEdit = !string.IsNullOrEmpty(Family.Id);
        }
    }
}
