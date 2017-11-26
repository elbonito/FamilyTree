using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using AppCoreX.Models;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace AppCoreX.ViewModels
{
    public class ParentViewModel : MvxViewModel<Parent, Parent>
    {
        private readonly IMvxNavigationService _navigationService;
        private Parent _parent;

        public Parent Parent
        {
            get => _parent;
            set => SetProperty(ref _parent, value);
        }


        public IMvxCommand SaveMvxCommand => new MvxCommand(async () =>
        {
            //add validation
            if (string.IsNullOrEmpty(Parent.FamilyName) || string.IsNullOrEmpty(Parent.FirstName)) return;
            await _navigationService.Close(this, Parent);
        });
        public IMvxCommand CancelMvxCommand => new MvxCommand(async () =>
        {
            await _navigationService.Close(this);
        });

        public ParentViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Prepare(Parent parent)
        {
            Parent = parent;
            if (parent == null) Parent = new Parent();
        }
    }
}
