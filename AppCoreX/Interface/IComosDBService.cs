using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using AppCoreX.Models;
using Microsoft.Azure.Documents;

namespace AppCoreX.Interface
{
    public interface IComosDBService
    {

        Task<ObservableCollection<Family>> ExecuteSimpleQuery(string lastName);
        Task ReplaceFamilyDocumentAsync(Family family);
        Task DeleteFamilyDocumentAsync(string documentName);
        Task<bool> DeleteDatabaseAsync(string databaseName);
        Task<bool> CreateDatabaseAsync(string databaseName);
        Task<bool> CreateDocumentCollectionAsync(string databaseName, string collectionName);
        Task<bool> DeleteDocumentCollectionAsync(string collectionSelfLink);
        Task<bool> CreateFamilyDocumentIfNotExistsAsync(Family family);

        Task<List<Database>> GetDatabaseListAsync();
        Task<List<DocumentCollection>> GetDocumentCollectionsListAsync(string databaseId);

    }
}
