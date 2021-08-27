using GrpcClientFactoryDemo.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcClientFactoryDemo.Controller
{
    [Route("/")]
    public class TestController : ControllerBase
    {

        private readonly TableSchemaQueryService _tableSchemaQueryService;

        private readonly ColumnSchemaQueryService _columnSchemaQueryService;

        public TestController(TableSchemaQueryService tableSchemaQueryService,
            ColumnSchemaQueryService columnSchemaQueryService)
        {
            _tableSchemaQueryService = tableSchemaQueryService;
            _columnSchemaQueryService = columnSchemaQueryService;
        }

        public string Index()
        {
            return _tableSchemaQueryService.GetTableSchemaById("6d28b87e-d4e4-4253-822d-f952a90fa3be");
        }

        [Route("TestWithAuth")]
        public string TestWithAuth()
        {
            return _columnSchemaQueryService.GetColumnSchemaListByTableId("6d28b87e-d4e4-4253-822d-f952a90fa3be");
        }
    }
}
