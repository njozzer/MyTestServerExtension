using DocsVision.BackOffice.CardLib.CardDefs;
using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.ObjectModel.Services;
using DocsVision.Platform.Data;
using DocsVision.Platform.Data.Metadata.CardModel;
using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.Utils.Maybe;
using DocsVision.Platform.WebClient;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.CodeAnalysis.Operations;
using MyTestServerExtension.Model;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using static DocsVision.BackOffice.CardLib.CardDefs.RefKinds;
namespace MyTestServerExtension.Services
{
    
    public class MyTestService : IMyTestService
    {
        
        public MyTestModel GetName(SessionContext sessionContext, Guid cardId)
        {
            var card = sessionContext.ObjectContext.GetObject<Document>(cardId)
                ??throw new ArgumentException("Invalid card id",nameof(cardId));
            var content = card.MainInfo["staffSigned"] as string;
            card.MainInfo["reason"] = "awdawd";
            sessionContext.ObjectContext.AcceptChanges();
            return new MyTestModel { content = content };
        }
        public MyTestModel GetMemberSentData(SessionContext sessionContext, Guid cardId, Guid userId)
        {
            var card = sessionContext.ObjectContext.GetObject<Document>(cardId)
                ?? throw new ArgumentException("Invalid card id", nameof(cardId));
            var context = sessionContext.ObjectContext;
            
            StaffEmployee employee = context.GetObject<StaffEmployee>(userId);
            card.MainInfo["memberSent"] = employee.GetObjectId();
            card.MainInfo["memberChief"] = employee.Manager.GetObjectId();
            card.MainInfo["phoneNumber"] = employee.Manager.Phone;
            sessionContext.ObjectContext.AcceptChanges();
            var content = "{\"memberChief\":\"" + employee.Manager.GetObjectId() + "\",\"phoneNumber\":\""+ employee.Manager.Phone + "\"}";
            return new MyTestModel { content = content };
        }

        public MyTestModel ChangeMoneyData(SessionContext sessionContext, Guid cardId, Guid cityId) {
            var card = sessionContext.ObjectContext.GetObject<Document>(cardId)
                ?? throw new ArgumentException("Invalid card id", nameof(cardId));
            var context = sessionContext.ObjectContext;
            var obj = context.GetObject<BaseUniversalItem>(cityId);
            //card.MainInfo["dayCount"];
            var money = (decimal)obj.ItemCard.MainInfo["daySalary"] * (int)card.MainInfo["dayCount"];
            card.MainInfo["money"] = money;
           
            var content =  "{\"money\":\""+money +"\"}" ;
            return new MyTestModel { content = content };
        }
        public MyTestModel ChangeDayCount(SessionContext sessionContext, Guid cardId,string dateFrom,string dateTo)
        {
            var card = sessionContext.ObjectContext.GetObject<Document>(cardId)
                ?? throw new ArgumentException("Invalid card id", nameof(cardId));
            var context = sessionContext.ObjectContext;
            
            var dateFrom_ = DateTime.Parse(dateFrom);
            var dateTo_ = DateTime.Parse(dateTo);
            var dayCount = dateTo_.Subtract(dateFrom_).Days;
            card.MainInfo["dateFrom"] = dateFrom_;
            card.MainInfo["dateTo"] = dateTo_;
            card.MainInfo["dayCount"] = dayCount;
            var id = card.MainInfo["cityRef"].ToString();
            sessionContext.ObjectContext.AcceptChanges();
            
            ChangeMoneyData(sessionContext, cardId, new Guid(id));
            var money = card.MainInfo["money"].ToString();
            var content = "{\"dayCount\":\"" + dayCount + "\",\"money\":\"" + money+ " \"}";
            return new MyTestModel { content = content };
        }

        public void InitMyCard(SessionContext sessionContext, Guid cardId)
        {
            var card = sessionContext.ObjectContext.GetObject<Document>(cardId);
            
            var author = card.MainInfo.Author;
            var manager = author.Manager;
            var staffSvc = sessionContext.ObjectContext.GetService<IStaffService>();
            var group = staffSvc.GetGroup(new Guid("{5103C1D3-69AA-4CA7-AD70-CCFD0B4220E6}"));
            var groupStaff = staffSvc.GetGroupEmployees(group);
            StaffEmployee tmp = null;
            foreach (var item in groupStaff)
            {
                if (item.Status.ToString() == "Active")
                {
                    tmp = item; break;
                }
            }
            StaffEmployee tmpSigned = author.Manager;
            /*if (author.Unit.Manager.GetObjectId() == manager.GetObjectId()) {
                tmpSigned = ;
            }
            else
            {
                tmpSigned = author.Unit.Manager;
            }*/
            card.MainInfo["memberIssued"] = tmp.GetObjectId();
            card.MainInfo["memberSigned"] = tmpSigned.GetObjectId();
            card.MainInfo["memberSent"] = author.GetObjectId();
            card.MainInfo["memberChief"] = manager.GetObjectId();
            card.MainInfo["phoneNumber"] = manager.Phone;

            sessionContext.ObjectContext.SaveObject(card);
        }
        public MyTestModel GetTestData(SessionContext sessionContext, Guid cardId) {
            var card = sessionContext.ObjectContext.GetObject<Document>(cardId);
            ExtensionManager extensionManager = sessionContext.Session.ExtensionManager;
            ExtensionMethod getRole = extensionManager.GetExtensionMethod("MyExtension", "GetRole");
            var cityGUID = Guid.Parse(card.MainInfo["cityRef"].ToString());
            getRole.Parameters.AddNew("employeeId", ParameterValueType.Guid, sessionContext.UserInfo.EmployeeId);
            getRole.Parameters.AddNew("cityId", ParameterValueType.Guid, cityGUID);
            getRole.Parameters.AddNew("cardId", ParameterValueType.Guid, cardId);
            var res = (string)getRole.Execute() + "";
            
            return new MyTestModel { content =  res };
        }
        public MyTestModel GetTripData(SessionContext sessionContext, Guid cardId) {
            ExtensionManager extensionManager = sessionContext.Session.ExtensionManager;
            ExtensionMethod getTrips = extensionManager.GetExtensionMethod("MyExtension", "getTrips");
            getTrips.Parameters.AddNew("employeeId", ParameterValueType.Guid, sessionContext.UserInfo.EmployeeId);
            
            return new MyTestModel { content = (string)getTrips.Execute()+"" };
        }

        public MyTestModel GetTicketData(SessionContext sessionContext, Guid cardId, string dateFrom, string dateTo, Guid cityRef)
        {
            var origin = "LED";
            var obj = sessionContext.ObjectContext.GetObject<BaseUniversalItem>(cityRef);
            var destination = obj.ItemCard.MainInfo["cityCode"];
            var token = "b165d8c4be5500d4da61df5067fd34ad";
            var departure_at = dateFrom;
            var return_at = dateTo;
            var direct = "true";
            var limit = "10";
            using HttpClient client = new HttpClient();

            string url = "https://api.travelpayouts.com/aviasales/v3/prices_for_dates?" +
                "origin=" + origin + "&" +
                "destination=" + destination + "&" +
                "departure_at=" + departure_at + "&" +
                "return_at=" + return_at + "&" +
                "unique=false&" +
                "sorting=price&" +
                "direct=" + direct + "&" +
                "currency=rub&" +
                "limit=" + limit + "&" +
                "page=1&" +
                "one_way=true&" +
                "token=" + token;

            HttpResponseMessage response = client.GetAsync(url).Result;
            var responseBody = response.Content.ReadAsStream();
            using (StreamReader reader = new StreamReader(responseBody, Encoding.UTF8))

            {
                string content = reader.ReadToEnd();
                
                var res = content;
                return new MyTestModel { content = res };
            }
               
        }
        
    }
}
