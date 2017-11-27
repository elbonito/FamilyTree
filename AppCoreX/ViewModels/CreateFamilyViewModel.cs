using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using AppCoreX.Models;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace AppCoreX.ViewModels
{
    public class CreateFamilyViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private Family _family;

        public CreateFamilyViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
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
              var newParent = await _navigationService.Navigate<ParentViewModel, Parent, Parent>(null);
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
            var newChild = await _navigationService.Navigate<ChildViewModel, Child, Child>(null);
            if (newChild == null) return;
            //add parent to list
            Family.Children.Add(newChild);
        });
        public IMvxCommand EditChildMvxCommand =>new MvxCommand(async () =>
        {
            if(SelectedChildIndex<0)return;
            var child = Family.Children[SelectedChildIndex];
            var editedChild = await _navigationService.Navigate<ChildViewModel,Child, Child>(child);
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

        public IMvxCommand SaveFamilyMvxCommand=>new MvxCommand(() =>
        {
            //check to make sure values are not null
            if(string.IsNullOrEmpty(Family.LastName)||string.IsNullOrEmpty(Family.Address.City)||string.IsNullOrEmpty(Family.Address.County)||string.IsNullOrEmpty(Family.Address.State))
                return;
            if (Family.Parents.Count == 0 || Family.Parents.Equals(null)) return;
            

        });
        public IMvxCommand CancelFamilyMvxCommand => new MvxCommand(() =>
        {
            _navigationService.Close(this);
        });

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
    }
}
