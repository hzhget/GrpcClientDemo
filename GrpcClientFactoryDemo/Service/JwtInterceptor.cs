using Grpc.Core;
using Grpc.Core.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcClientFactoryDemo.Service
{
    public class JwtInterceptor : Interceptor
    {
        private static readonly string Token = "eyJhbGciOiJIUzUxMiJ9.eyJleHAiOjE2MzA0ODMzNTQsIlVTRVJfSUQiOiJhZG1pbiIsIkNMSUVOVF9JRCI6ImxvY2FsaG9zdCIsIlJPTEVfSUQiOltdfQ.0eqUwKy4JnsKxeJKJ_0gQBM0TJaVS8W1KJl6KYRm6lB9L-dx6Dy7orZEoGeDMkbcUimQRPYk0eTk3507HA87_A";

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            Metadata meta = new Metadata();

            meta.Add("Authorization", $"Bearer {Token}");

            var newContext = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, context.Options.WithHeaders(meta));
            
            return base.AsyncUnaryCall(request, newContext, continuation);
        }

        public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            Metadata meta = new Metadata();

            meta.Add("Authorization", $"Bearer {Token}");

            var newContext = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, context.Options.WithHeaders(meta));
           
            return base.BlockingUnaryCall(request, newContext, continuation);
        }

    }
}
