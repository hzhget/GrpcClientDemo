using Database;
using System;
using static Database.TableSchemaQuery;

namespace GrpcClientFactoryDemo.Service
{
    public class TableSchemaQueryService
    {
        private readonly TableSchemaQueryClient _client;

        public TableSchemaQueryService(TableSchemaQueryClient client)
        {
            _client = client;
        }

        public string GetTableSchemaById(string TableId)
        {
            var request = new GetTableSchemaRequest();

            request.Id = TableId;

            var response = _client.GetByID(request);

            Console.WriteLine("Table Name = [{0}]", response.Name);

            Console.WriteLine("Table CreatorID = [{0}]", response.CreatorID);

            return response.Name;

        }

    }
}
