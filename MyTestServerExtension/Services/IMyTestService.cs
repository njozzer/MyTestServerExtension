using DocsVision.Platform.WebClient;
using MyTestServerExtension.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTestServerExtension.Services
{
    public interface IMyTestService{
        MyTestModel GetName(SessionContext sessionContext,Guid cardId);
        MyTestModel GetMemberSentData(SessionContext sessionContext, Guid cardId, Guid userId);
        MyTestModel ChangeMoneyData(SessionContext sessionContext, Guid cardId, Guid cityId);

        MyTestModel ChangeDayCount(SessionContext sessionContext, Guid cardId, string dateFrom, string dateTo);

        public void InitMyCard(SessionContext sessionContext, Guid cardId);

        public MyTestModel GetTripData(SessionContext sessionContext, Guid cardId);
        public MyTestModel GetTestData(SessionContext sessionContext, Guid cardId);

        public MyTestModel GetTicketData(SessionContext sessionContext, Guid cardId, string dateFrom, string dateTo, Guid cityRef);
    }
}
