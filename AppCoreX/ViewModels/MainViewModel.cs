﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
        private const string EndpointUri = "https://appx.documents.azure.com:443/";
        private const string PrimaryKey = "VuRzczyKnaVI28KlDs8yTvjrEah7b6tgFay5Qb7WG14TyFpgUlkcC3R2848Bj5PGauEav5Zaa1Njkm1mdd3KsA==";
        private DocumentClient client;
        private string _hello;

        public MainViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public string Hello
        {
            get => _hello;
            set => SetProperty(ref _hello, value);
        }

        public IMvxCommand AddFamilyCommand =>new MvxCommand(async () =>
        {
           await _navigationService.Navigate<CreateFamilyViewModel>();
        }); 

        public override async void Prepare()
        {
            Hello = "On Prepare";
            try
            {
               // await GetStartedDemo();
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Debug.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Debug.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
            finally
            {
                Debug.WriteLine("End of demo");

            }
        }

        private async Task GetStartedDemo()
        {
            client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
            //create a Azure Cosmos DB database
            await client.CreateDatabaseIfNotExistsAsync(new Database { Id = "FamilyDB_oa" });
            //Create a Collection
            await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("FamilyDB_oa"), new DocumentCollection { Id = "FamilyCollection_oa" });
            //add  insert two documents, one each for the Andersen Family and the Wakefield Family 
            Family andersenFamily = new Family
            {
                Id = "Andersen.1",
                LastName = "Andersen",
                Parents = new ObservableCollection<Parent>
                    {
                new Parent { FirstName = "Thomas" },
                new Parent { FirstName = "Mary Kay" }
                    },
                Children = new List<Child>
                {
                new Child
                {
                        FirstName = "Henriette Thaulow",
                        Gender = "female",
                        Grade = 5,
                        Pets = new List<Pet>
                        {
                                new Pet { GivenName = "Fluffy" }
                        }
                }
                    },
                Address = new Address { State = "WA", County = "King", City = "Seattle" },
                IsRegistered = true
            };

            await CreateFamilyDocumentIfNotExists("FamilyDB_oa", "FamilyCollection_oa", andersenFamily);

            Family wakefieldFamily = new Family
            {
                Id = "Wakefield.7",
                LastName = "Wakefield",
                Parents = new ObservableCollection<Parent>
                    {
                new Parent { FamilyName = "Wakefield", FirstName = "Robin" },
                new Parent { FamilyName = "Miller", FirstName = "Ben" }
                    },
                Children = new  List<Child>
                    {
                new Child
                {
                        FamilyName = "Merriam",
                        FirstName = "Jesse",
                        Gender = "female",
                        Grade = 8,
                        Pets = new List<Pet>
                        {
                                new Pet { GivenName = "Goofy" },
                                new Pet { GivenName = "Shadow" }
                        }
                },
                new Child
                {
                        FamilyName = "Miller",
                        FirstName = "Lisa",
                        Gender = "female",
                        Grade = 1
                }
                    },
                Address = new Address { State = "NY", County = "Manhattan", City = "NY" },
                IsRegistered = false
            };

            await CreateFamilyDocumentIfNotExists("FamilyDB_oa", "FamilyCollection_oa", wakefieldFamily);


        }
        private async Task CreateFamilyDocumentIfNotExists(string databaseName, string collectionName, Family family)
        {
            try
            {
                await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, family.Id));
                // this.WriteToConsoleAndPromptToContinue("Found {0}", family.Id);
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), family);
                    //zvithis.WriteToConsoleAndPromptToContinue("Created Family {0}", family.Id);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}