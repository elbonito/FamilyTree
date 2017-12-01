using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AppCoreX.Helpers;
using AppCoreX.Interface;
using AppCoreX.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace AppCoreX.Services
{
    public class CosmosDBService : IComosDBService
    {
        private readonly DocumentClient _client;

        public CosmosDBService()
        {
            _client = new DocumentClient(new Uri(TextConstants.EndpointUri), TextConstants.PrimaryKey);
        }

        public Task<ObservableCollection<Family>> ExecuteSimpleQuery(string lastName)
        {

            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Here we find the Andersen family via its LastName
            IQueryable<Family> familyQuery = _client.CreateDocumentQuery<Family>(
                    UriFactory.CreateDocumentCollectionUri(TextConstants.DatabaseName, TextConstants.CollectionName), queryOptions)
                .Where(f => f.LastName == lastName);

            // The query is executed synchronously here, but can also be executed asynchronously via the IDocumentQuery<T> interface
            Debug.WriteLine("Running LINQ query...");
            foreach (Family family in familyQuery)
            {
                Debug.WriteLine("\tRead {0}", family);
            }
            return Task.FromResult(new ObservableCollection<Family>(familyQuery.ToList()));

        }

        public async Task ReplaceFamilyDocumentAsync(Family family)
        {
            try
            {
                await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(TextConstants.DatabaseName, TextConstants.CollectionName, family.Id), family);
                Debug.WriteLine($"Replaced Family {0}", family.LastName);
            }
            catch (DocumentClientException de)
            {
                throw;
            }
        }
        //documentName is the document Id
        public async Task DeleteFamilyDocumentAsync(string documentName)
        {
            try
            {
                await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(TextConstants.DatabaseName, TextConstants.CollectionName, documentName));
                Debug.WriteLine($"Deleted Family {0}", documentName);
            }
            catch (DocumentClientException de)
            {
                throw;
            }
        }

        public async Task<bool> DeleteDatabaseAsync(string databaseName)
        {
            // Clean up/delete the database
            try
            {
                var resultResponse = await _client.DeleteDatabaseAsync(UriFactory.CreateDatabaseUri(databaseName));
                if (resultResponse.StatusCode.Equals(HttpStatusCode.OK)) return true;
            }
            catch (DocumentClientException de)
            {
                Debug.WriteLine(de.Message);
            }
            return false;
        }

        public async Task<bool> CreateDatabaseAsync(string databaseName)
        {
            try
            {
                //create a Azure Cosmos DB database
                var resultResponse = await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseName });
                if (resultResponse.StatusCode.Equals(HttpStatusCode.OK)) return true;
            }
            catch (DocumentClientException de)
            {
                Debug.WriteLine(de.Message);
            }
            return false;
        }

        public async Task<bool> CreateDocumentCollectionAsync(string databaseName, string collectionName)
        {
            try
            {
                //create a Azure Cosmos DB database
                var resultResponse = await _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(databaseName), new DocumentCollection { Id = collectionName });
                if (resultResponse.StatusCode.Equals(HttpStatusCode.OK)) return true;
            }
            catch (DocumentClientException de)
            {
                Debug.WriteLine(de.Message);
            }
            return false;
        }

        public async Task<bool> DeleteDocumentCollectionAsync(string collectionSelfLink)
        {
            try
            {
                var resultResponse = await _client.DeleteDocumentCollectionAsync(collectionSelfLink);
                if (resultResponse.StatusCode.Equals(HttpStatusCode.OK)) return true;
            }
            catch (DocumentClientException e)
            {
                Debug.WriteLine(e.Message);

            }
            return false;
        }

        public async Task<bool> CreateFamilyDocumentIfNotExistsAsync(Family family)
        {
            try
            {
                await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(TextConstants.DatabaseName, TextConstants.CollectionName, family.Id));
                Debug.WriteLine($"found {0}", family.Id);
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(TextConstants.DatabaseName, TextConstants.CollectionName), family);
                    Debug.WriteLine($"Created Family {0}", family.Id);
                    return true;
                }
                Debug.WriteLine(de.Message);
            }
            return false;
        }

        public async Task<List<Database>> GetDatabaseListAsync()
        {
            try
            {
                var result = await _client.ReadDatabaseFeedAsync();
                foreach (var database in result)
                {
                    Debug.WriteLine($"id {database.Id}");
                    Debug.WriteLine($"collectionLinks {database.CollectionsLink}");
                }
                return result.ToList();
            }
            catch (DocumentClientException e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }

        public async Task<List<DocumentCollection>> GetDocumentCollectionsListAsync(string databseId)
        {
            try
            {
                var response = await _client.ReadDocumentCollectionFeedAsync(UriFactory.CreateDatabaseUri(databseId));
                foreach (var documentCollection in response)
                {
                    Debug.WriteLine($"id {0}", documentCollection.Id);
                }
                return response.ToList();
            }
            catch (DocumentClientException e)
            {
                Debug.WriteLine(e.Message);

            }
            return null;
        }
    }
}
