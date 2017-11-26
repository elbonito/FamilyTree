using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace AppCore.ViewModels
{
    public class MainViewModel:MvxViewModel
    {

        private const string EndpointUri = "https://appx.documents.azure.com:443/";
        private const string PrimaryKey = "VuRzczyKnaVI28KlDs8yTvjrEah7b6tgFay5Qb7WG14TyFpgUlkcC3R2848Bj5PGauEav5Zaa1Njkm1mdd3KsA==";

        private string _hello;

        public string Hello
        {
            get => _hello;
            set => SetProperty(ref _hello, value);
        }

        public override void Prepare()
        {
            Hello = "On Prepare";
        }
    }
}
