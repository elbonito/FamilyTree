using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace AppConsole
{
    class Program
    {
        // ADD THIS PART TO YOUR CODE
        private const string EndpointUri = "https://appx.documents.azure.com:443/";
        private const string PrimaryKey = "VuRzczyKnaVI28KlDs8yTvjrEah7b6tgFay5Qb7WG14TyFpgUlkcC3R2848Bj5PGauEav5Zaa1Njkm1mdd3KsA==";
        private DocumentClient client;
        static void Main(string[] args)
        {
            // ADD THIS PART TO YOUR CODE
            try
            {
                Program p = new Program();
                p.GetStartedDemo().Wait();
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit.");
                Console.ReadKey();
            }
        }

        // ADD THIS PART TO YOUR CODE
        private async Task GetStartedDemo()
        {
            this.client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
            // This will create a database named FamilyDB
            await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = "FamilyDB_oa" });
            //A collection is a container of JSON documents and associated JavaScript application logic.
            await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("FamilyDB_oa"), new DocumentCollection { Id = "FamilyCollection_oa" });
        }
        // ADD THIS PART TO YOUR CODE
        private void WriteToConsoleAndPromptToContinue(string format, params object[] args)
        {
            Console.WriteLine(format, args);
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }
    }
}
