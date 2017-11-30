using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Acr.UserDialogs;
using AppCoreX.Interface;
using Microsoft.Azure.Documents;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace AppCoreX.ViewModels
{
    public class DatabaseManagementViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IComosDBService _comosDbService;

        private string _documentName;

        public string DocumentName
        {
            get => _documentName;
            set => SetProperty(ref _documentName, value);
        }

        private string _databaseName;

        public string DatabaseName
        {
            get => _databaseName;
            set => SetProperty(ref _databaseName, value);
        }

        private ObservableCollection<Database> _databases;

        public ObservableCollection<Database> Databases
        {
            get => _databases;
            set => SetProperty(ref _databases, value);
        }
        private ObservableCollection<DocumentCollection> _documentCollections;

        public ObservableCollection<DocumentCollection> DocumentCollections
        {
            get => _documentCollections;
            set => SetProperty(ref _documentCollections, value);
        }

        private Database _selecteDatabase;

        public Database SelectedDatabase
        {
            get => _selecteDatabase;
            set
            {
                SetProperty(ref _selecteDatabase, value);
                GetCollectionList();
            }

        }

        private DocumentCollection _selectedDocumentCollection;

        public DocumentCollection SelectedDocumentCollection
        {
            get => _selectedDocumentCollection;
            set => SetProperty(ref _selectedDocumentCollection, value);
        }


        private async void GetCollectionList()
        {
            if (SelectedDatabase == null) return;
            var colresult = await _comosDbService.GetDocumentCollectionsListAsync(SelectedDatabase.Id);
            DocumentCollections = new ObservableCollection<DocumentCollection>(colresult);
        }

        public IMvxCommand AddCollectionMvxCommand => new MvxCommand(async () =>
        {
            if (string.IsNullOrEmpty(DocumentName) || SelectedDatabase == null) return;
            await _comosDbService.CreateDocumentCollectionAsync(SelectedDatabase.Id, DocumentName);
            DocumentCollections = null;
            GetCollectionList();
            DocumentName = null;

        });
        public IMvxCommand DeleteCollectionMvxCommand => new MvxCommand(async () =>
        {
            if (SelectedDocumentCollection==null || SelectedDatabase == null) return;
            await _comosDbService.DeleteDocumentCollectionAsync(SelectedDocumentCollection.SelfLink);
            DocumentCollections = null;
            GetCollectionList();
            DocumentName = null;

        });
        public IMvxCommand AddDatabaseMvxCommand => new MvxCommand(async () =>
        {
            if (string.IsNullOrEmpty(DatabaseName)) return;
            await _comosDbService.CreateDatabaseAsync(DatabaseName);
            DocumentCollections = null;
            RetrieveDatabases();
            DatabaseName = null;
        });

        public IMvxCommand DeleteDatabaseMvxCommand => new MvxCommand(async () =>
        {
            if (SelectedDatabase == null) return;
            await _comosDbService.DeleteDatabaseAsync(SelectedDatabase.Id);
            DocumentCollections = null;
            RetrieveDatabases();
            SelectedDatabase = null;

        });



        public DatabaseManagementViewModel(IMvxNavigationService navigationService, IUserDialogs userDialogs, IComosDBService comosDbService)
        {
            _navigationService = navigationService;
            _userDialogs = userDialogs;
            _comosDbService = comosDbService;
        }

        public override void Prepare()
        {
            RetrieveDatabases();
        }

        public async void RetrieveDatabases()
        {
            var result = await _comosDbService.GetDatabaseListAsync();
            Databases = new ObservableCollection<Database>(result);
        }
    }
}
