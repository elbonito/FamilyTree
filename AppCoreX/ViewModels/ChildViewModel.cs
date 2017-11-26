using System;
using System.Collections.Generic;
using System.Text;
using AppCoreX.Models;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace AppCoreX.ViewModels
{
    public class ChildViewModel : MvxViewModel<Child, Child>
    {
        private readonly IMvxNavigationService _navigationService;

        private Child _child;

        public Child Child
        {
            get => _child;
            set => SetProperty(ref _child, value);
        }


        public ChildViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Prepare(Child child)
        {
            Child = child;
            if(Child==null)Child=new Child(){Pets = new List<Pet>()};
        }
    }
}
