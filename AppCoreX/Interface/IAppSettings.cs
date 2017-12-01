using System;
using System.Collections.Generic;
using System.Text;

namespace AppCoreX.Interface
{
    public interface IAppSettings
    {
        string DatabaseName { get; set; }
        string DocumentCollectionName { get; set; }
    }
}
