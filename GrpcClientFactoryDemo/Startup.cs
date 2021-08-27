using Grpc.Core;
using GrpcClientFactoryDemo.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using static Database.ColumnSchemaQuery;
using static Database.TableSchemaQuery;

namespace GrpcClientFactoryDemo
{
    public class Startup
    {
        private static readonly string Token = "eyJhbGciOiJIUzUxMiJ9.eyJleHAiOjE2MzA0ODMzNTQsIlVTRVJfSUQiOiJhZG1pbiIsIkNMSUVOVF9JRCI6ImxvY2FsaG9zdCIsIlJPTEVfSUQiOltdfQ.0eqUwKy4JnsKxeJKJ_0gQBM0TJaVS8W1KJl6KYRm6lB9L-dx6Dy7orZEoGeDMkbcUimQRPYk0eTk3507HA87_A";
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped(typeof(TableSchemaQueryService));

            services.AddScoped(typeof(ColumnSchemaQueryService));


            services.AddGrpcClient<TableSchemaQueryClient>(o =>
            {
                o.Address = new Uri("http://localhost:50000");
            }).AddInterceptor(() => new JwtInterceptor())
                ;

            services.AddGrpcClient<ColumnSchemaQueryClient>(nameof(ColumnSchemaQueryClient) + "Authenticated",
                o =>
            {
                o.Address = new Uri("http://localhost:50000");
            }).ConfigureChannel(c => {
                //var credentials = CallCredentials.FromInterceptor((context, metadata) =>
                //{
                //    metadata.Add("Authorization", $"Bearer {Token}");
                //    return Task.CompletedTask;
                //});

                //c.Credentials = ChannelCredentials.Create(new SslCredentials(), credentials);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
