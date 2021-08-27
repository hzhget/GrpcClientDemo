using Common;
using Database;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcClientDemo.Extensions;
using System;
using System.Threading.Tasks;
using static Database.ColumnSchemaQuery;
using static Database.TableMutation;
using static Database.TableRelationshipSchemaQuery;
using static Database.TableSchemaQuery;

namespace GrpcClientDemo
{

    class Program
    {
        private static readonly string Token = "eyJhbGciOiJIUzUxMiJ9.eyJleHAiOjE2MzA0ODMzNTQsIlVTRVJfSUQiOiJhZG1pbiIsIkNMSUVOVF9JRCI6ImxvY2FsaG9zdCIsIlJPTEVfSUQiOltdfQ.0eqUwKy4JnsKxeJKJ_0gQBM0TJaVS8W1KJl6KYRm6lB9L-dx6Dy7orZEoGeDMkbcUimQRPYk0eTk3507HA87_A";


        static void Main(string[] args)
        {
            try
            {
                using var channel = GrpcChannel.ForAddress("http://localhost:50000");
                //using var channel = CreateAuthenticatedChannel("http://localhost:50000", token);
                //TestTableRelationship(channel);
                GetTableSchemaById(channel);
                Console.WriteLine("---------------------------------");
                GetColumnSchemaListByTableId(channel);
                Console.WriteLine("---------------------------------");
                InsertDataToTable(channel);

            }
            catch (RpcException ex)
            {
                ErrorInfo info = ex.GetErrorInfoFromException();
                Console.WriteLine(info);
            }
        }


        // 根据ID获取TableScheam
        private static void GetTableSchemaById(GrpcChannel channel)
        {
            var client = new TableSchemaQueryClient(channel);

            var request = new GetTableSchemaRequest();

            request.Id = "6d28b87e-d4e4-4253-822d-f952a90fa3be";

            var response = client.GetByID(request);

            Console.WriteLine("Table Name = [{0}]", response.Name);

            Console.WriteLine("Table CreatorID = [{0}]", response.CreatorID);

        }

        // 根据TableID获取ColumnSchema
        private static void GetColumnSchemaListByTableId(GrpcChannel channel)
        {
            var client = new ColumnSchemaQueryClient(channel);

            var request = new ListColumnSchemaRequest();

            request.TableSchemaID = "6d28b87e-d4e4-4253-822d-f952a90fa3be";

            var response = client.List(request);

            foreach (var col in response.Data)
            {
                Console.WriteLine("Column Name = [{0}]", col.Name);

            }
        }

        private static void InsertDataToTable(GrpcChannel channel)
        {
            var client = new TableMutationClient(channel);



            InsertRowsRequest insertRowsRequest = new InsertRowsRequest();

            insertRowsRequest.Name = "apple";

            insertRowsRequest.CreatorID = "033e9080-0599-474a-8a3f-7c846e226bfb";

            var row = new InsertRowsRequest.Types.Row();

            var cell = new InsertRowsRequest.Types.Cell();

            cell.ColumnName = "name";

            cell.Value = "test name";

            row.Cells.Add(cell);

            var cell2 = new InsertRowsRequest.Types.Cell();

            cell2.ColumnName = "description";

            cell2.Value = "test  description";

            row.Cells.Add(cell2);

            insertRowsRequest.Rows.Add(row);

            var response = client.InsertRows(insertRowsRequest);

            Console.WriteLine("Table Rows = {0}",string.Join("\n",response.Rows));

        }


        // 方法1： 添加token
        private static void TestTableRelationship(GrpcChannel channel)
        {
            var client = new TableRelationshipSchemaQueryClient(channel);
            var request = new ListRelationshipRequest();
            request.TableSchemaID = "6b9df5e3-8055-4700-89fc-1be78a0781e7";
            request.Relation = "<";

            var headers = new Metadata();

            headers.Add("Authorization", $"Bearer {Token}");

            var response = client.ListRelationship(request, headers);

            foreach (var r in response.Data)
            {
                Console.WriteLine(r);
            }
        }

        // 方法2： 添加token
        private static GrpcChannel CreateAuthenticatedChannel(string address, string token)
        {
            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(token))
                {
                    metadata.Add("Authorization", $"Bearer {token}");
                }
                return Task.CompletedTask;
            });

            // SslCredentials is used here because this channel is using TLS.
            // CallCredentials can't be used with ChannelCredentials.Insecure on non-TLS channels.
            var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
            });
            return channel;
        }


    }
}
