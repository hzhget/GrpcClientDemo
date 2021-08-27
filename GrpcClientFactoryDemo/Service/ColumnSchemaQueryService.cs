using Database;
using Grpc.Net.ClientFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Database.ColumnSchemaQuery;

namespace GrpcClientFactoryDemo.Service
{
    public class ColumnSchemaQueryService
    {
        private readonly ColumnSchemaQueryClient _client;

        public ColumnSchemaQueryService(GrpcClientFactory grpcClientFactory)
        {
            _client = grpcClientFactory.CreateClient<ColumnSchemaQueryClient>(nameof(ColumnSchemaQueryClient) + "Authenticated");

        }

        public string GetColumnSchemaListByTableId(string TableId)
        {

            var request = new ListColumnSchemaRequest();

            request.TableSchemaID = TableId;

            var response = _client.List(request);

            foreach (var col in response.Data)
            {
                Console.WriteLine("Column Name = [{0}]", col.Name);

            }
            return response.Data[0].Name;
        }
    }
}
