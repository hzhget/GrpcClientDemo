using Common;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcClientDemo.Extensions
{
    public static class GrpcExtension
    {

        public static ErrorInfo GetErrorInfoFromException(this RpcException ex)
        {
            var info = ex.Trailers.Get(ErrorInfo.Descriptor.FullName.ToLower() + Metadata.BinaryHeaderSuffix);
            if (info != null)
            {
                return ErrorInfo.Parser.ParseFrom(info.ValueBytes);

            }
            return null;
        }
    }
}
