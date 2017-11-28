using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AppCoreX.Models;

namespace AppCoreX.Interface
{
    public interface IComosDBService
    {
        Task CreateFamilyDocumentIfNotExists(Family family);
    }
}
