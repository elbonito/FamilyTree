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

        public IMvxCommand<Child> SelectedChildCommand => new MvxCommand<Child>(async (child) =>
        {
            //Family.Parents.Remove(parent);

        });

        public override void Prepare()
        {
            Parents = new ObservableCollection<Parent>();
            Family = new Family
            {
                Parents = new ObservableCollection<Parent>(),
                Children = new List<Child>(),
                Address = new Address()

            };
        }
    }
}
