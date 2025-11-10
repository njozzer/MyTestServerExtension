using DocsVision.Platform.WebClient;
using DocsVision.Platform.WebClient.Models;
using DocsVision.Platform.WebClient.Models.Generic;
using Microsoft.AspNetCore.Mvc;
using MyTestServerExtension.Model;
using MyTestServerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTestServerExtension.Controllers
{
    public class MyTestController: ControllerBase{
        private readonly ICurrentObjectContextProvider contextProvider;
        private readonly IMyTestService testService;
        public MyTestController(ICurrentObjectContextProvider contextProvider, IMyTestService testService)
        {
            this.contextProvider = contextProvider;
            this.testService = testService;
        }
        
        [HttpPost]
        public CommonResponse<MyTestModel> GetName([FromBody] MyTestRequestModel model) { 
            var sessionContext = contextProvider.GetOrCreateCurrentSessionContext(); 
            var res = testService.GetName(sessionContext,model.DocumentId);
            return CommonResponse.CreateSuccess(res);
        }

        [HttpPost]
        public CommonResponse<MyTestModel> GetMemberSentData([FromBody] MyTestMemberRequestModel model)
        {
            var sessionContext = contextProvider.GetOrCreateCurrentSessionContext();
            var res = testService.GetMemberSentData(sessionContext, model.DocumentId, model.id);
            return CommonResponse.CreateSuccess(res);
        }

        [HttpPost]
        public CommonResponse<MyTestModel> ChangeMoneyData([FromBody] MyTestMemberRequestModel model)
        {
            var sessionContext = contextProvider.GetOrCreateCurrentSessionContext();
            var res = testService.ChangeMoneyData(sessionContext, model.DocumentId,model.id);
            return CommonResponse.CreateSuccess(res);
        }

        [HttpPost]
        public CommonResponse<MyTestModel> ChangeDayCount([FromBody] MyTestDateRequsetModel model)
        {
            var sessionContext = contextProvider.GetOrCreateCurrentSessionContext();
            var res = testService.ChangeDayCount(sessionContext, model.DocumentId, model.dateFrom, model.dateTo);
            return CommonResponse.CreateSuccess(res);
        }
        /*
        [HttpPost]
        public CommonResponse InitMyCard([FromBody] MyTestRequestModel model)
        {
            var sessionContext = contextProvider.GetOrCreateCurrentSessionContext();
            testService.InitMyCard(sessionContext, model.DocumentId);
            return CommonResponse.CreateSuccess("Success");
        }*/

        [HttpPost]
        public CommonResponse<MyTestModel> GetTripData([FromBody] MyTestDateRequsetModel model)
        {
            var sessionContext = contextProvider.GetOrCreateCurrentSessionContext();
            var res = testService.GetTripData(sessionContext, model.DocumentId);
            
            return CommonResponse.CreateSuccess(res);
        }

        [HttpPost]
        public CommonResponse<MyTestModel> GetTestData([FromBody] MyTestDateRequsetModel model)
        {
            var sessionContext = contextProvider.GetOrCreateCurrentSessionContext();
            var res = testService.GetTestData(sessionContext, model.DocumentId);

            return CommonResponse.CreateSuccess(res);
        }

        
    }
}
