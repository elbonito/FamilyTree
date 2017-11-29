using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using AppCoreX.Models;

namespace AppCoreX.Interface
{
    public interface IComosDBService
    {
        Task CreateFamilyDocumentIfNotExists(Family family);
        Task<ObservableCollection<Family>> ExecuteSimpleQuery(string lastName);
        Task ReplaceFamilyDocument(Family family);
        Task DeleteFamilyDocument(string documentName);
    }
}
