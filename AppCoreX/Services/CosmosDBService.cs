using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AppCoreX.Interface;
using AppCoreX.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace AppCoreX.Services
{
    public class CosmosDBService : IComosDBService
    {
        private const string DatabaseName = "FamilyDB_oa";
        private const string CollectionName = "FamilyCollection_oa";
        private const string EndpointUri = "https://appx.documents.azure.com:443/";
        private const string PrimaryKey = "VuRzczyKnaVI28KlDs8yTvjrEah7b6tgFay5Qb7WG14TyFpgUlkcC3R2848Bj5PGauEav5Zaa1Njkm1mdd3KsA==";
        private readonly DocumentClient _client;

        public CosmosDBService()
        {
            _client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
        }

        public async Task CreateDBAndCollection()
        {
            //create a Azure Cosmos DB database
            await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseName });
            //Create a Collection
            await _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DatabaseName), new DocumentCollection { Id = CollectionName });
            //add  insert two documents, one each for the Andersen Family and the Wakefield Family 
        }

        public async Task CreateFamilyDocumentIfNotExists(Family family)
        {
            try
            {
                // await  CreateDBAndCollection();
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

            try
            {
                await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseName, CollectionName, family.Id));
                Debug.WriteLine("fount {0}", family.Id);
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), family);
                    Debug.WriteLine("Created Family {0}", family.Id);
                }
                else
                {
                    throw;
                }
            }
        }

        public Task<ObservableCollection<Family>> ExecuteSimpleQuery(string lastName)
        {

            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Here we find the Andersen family via its LastName
            IQueryable<Family> familyQuery = _client.CreateDocumentQuery<Family>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), queryOptions)
                .Where(f => f.LastName == lastName);

            // The query is executed synchronously here, but can also be executed asynchronously via the IDocumentQuery<T> interface
            Debug.WriteLine("Running LINQ query...");
            foreach (Family family in familyQuery)
            {
                Debug.WriteLine("\tRead {0}", family);
            }
            return Task.FromResult(new ObservableCollection<Family>(familyQuery.ToList()));

        }

        public async Task ReplaceFamilyDocument(Family family)
        {
            try
            {
                await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseName, CollectionName, family.Id), family);
                Debug.WriteLine($"Replaced Family {0}", family.LastName);
            }
            catch (DocumentClientException de)
            {
                throw;
            }
        }
        //documentName is the document Id
        public async Task DeleteFamilyDocument(string documentName)
        {
            try
            {
                await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseName, CollectionName, documentName));
                Debug.WriteLine($"Deleted Family {0}", documentName);
            }
            catch (DocumentClientException de)
            {
                throw;
            }
        }
    }
}
