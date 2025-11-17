using System.IO;
using System.Text;
using System.Text.Json.Nodes;
using DocsVision.BackOffice.CardLib.CardDefs;
using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.ObjectModel.Services;
using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.ObjectModel;
using DocsVision.Platform.ObjectModel.Search;
using DocsVision.Workflow.Gates;
using DocumentFormat.OpenXml.Office.CustomUI;
namespace IntroductionToSDK {

	internal class Program {
		public static void Main(string[] args) {
			var serverURL = System.Configuration.ConfigurationManager.AppSettings["DVUrl"];
			var username = System.Configuration.ConfigurationManager.AppSettings["Username"];
			var password = System.Configuration.ConfigurationManager.AppSettings["Password"];

			var sessionManager = SessionManager.CreateInstance();
			sessionManager.Connect(serverURL, String.Empty, username, password);

			UserSession? session = null;
			try {
				session = sessionManager.CreateSession();
				var context = CreateContext(session);
				
				mineLogic(session, context);
				Console.WriteLine("Press any key to continue...");
				Console.ReadKey();
			} finally {
				session?.Close();
			}
		}

		public static ObjectContext CreateContext(UserSession session) {
			return DocsVision.BackOffice.ObjectModel.ContextFactory.CreateContext(session);
		}

		static void ChangeCardState(ObjectContext context, Document card, string targetState) {
			IStateService stateSvc = context.GetService<IStateService>();
			var branch = stateSvc.FindLineBranchesByStartState(card.SystemInfo.State)
				.FirstOrDefault(s => s.EndState.DefaultName == targetState);
			stateSvc.ChangeState(card, branch);
		}
		public static String getUser(int id) {
			switch (id) {
				case 0: return "DVAdmin";
				case 1: return "mihaylov_sa";
				case 2: return "ivanov_as";
				case 3: return "kolesnikova_sn";
				case 4: return "lebedev_ip";
				case 5: return "petrova_sa";
				case 6: return "samoylov_pn";
				case 7: return "semenov_km";
				case 8: return "kuznetsov_vi";
				case 9: return "ivanov_ii";
			}
			return "";
		}
		public static void createCard(UserSession session, ObjectContext context,KindsCardKind kind,int i) {
			var randomizer = new Random();
			var docSvc = context.GetService<IDocumentService>();
			var staffSvc = context.GetService<IStaffService>();
			
			var citiesSVc = context.GetService<IBaseUniversalService>();
			
			var citiesId = new Guid("{caef1c8d-e401-4efa-831e-aa30cc942c3e}");
			var obj = context.GetObject<BaseUniversal>(citiesId);
			var cities = obj.ItemTypes[2].Items;
			
			var card = docSvc.CreateDocument(null, kind);
			card.MainInfo.Author = staffSvc.GetCurrentEmployee();
			card.MainInfo["dateCreation"] = DateTime.Now;
			card.MainInfo["namePreview"] = "TestName" + i;
			
			card.MainInfo["money"] = randomizer.NextDouble()*10000;
			card.MainInfo["reason"] = "TestReason" + i;
			card.MainInfo["listData"] = i%2;
			var dayFrom = new DateTime();
			
			dayFrom = dayFrom.AddYears(2024);
			dayFrom = dayFrom.AddDays(randomizer.Next(1,200));
			var dayTo = new DateTime(dayFrom.Ticks);
			dayTo = dayTo.AddDays(randomizer.Next(1,20));
			card.MainInfo["dateFrom"] = dayFrom.ToUniversalTime();
			card.MainInfo["dateTo"] = dayTo.ToUniversalTime();
			
			card.MainInfo["dayCount"] = dayTo.Subtract(dayFrom).TotalDays;
			var staffSent = staffSvc.FindEmpoyeeByAccountName("ENGINEER\\" + getUser(2));
			var staffIssued = staffSvc.FindEmpoyeeByAccountName("ENGINEER\\" + getUser(0));
			var staffSigned = staffSvc.FindEmpoyeeByAccountName("ENGINEER\\" + getUser(1));
			card.MainInfo["memberIssued"] = staffIssued.GetObjectId();
			card.MainInfo["memberSigned"] = staffSigned.GetObjectId();
			card.MainInfo["memberSent"] = staffSent.GetObjectId();
			
			card.MainInfo["memberChief"] = staffSent.Manager.GetObjectId();
			card.MainInfo["cityRef"] = cities[i%3].GetObjectId();

			card.MainInfo["phoneNumber"] = staffSent.Manager.Phone;
			card.MainInfo["organization"] = staffSent.Unit.GetObjectId();
			
			context.AcceptChanges();
			docSvc.AddMainFile(card, "d:\\programming\\vm\\asd.txt");

			if (i % 2 == 0) {
				ChangeCardState(context, card, "На согласовании");
				context.AcceptChanges();
				if (i % 4 == 0) {
					ChangeCardState(context, card, "На оформлении");
					context.AcceptChanges();
					if (i % 5 == 0) {
						ChangeCardState(context, card, "Закрыто");
						context.AcceptChanges();
					}
				}
			}
		}
		public static void Test(UserSession session, ObjectContext context) {
			var staffSvc = context.GetService<IStaffService>();
			
			//var citiesSVc = context.GetService<IBaseUniversalService>();
			var id = new Guid("08bc91c8-e56f-473d-8db3-e5f05aa9b7d1");
			StaffEmployee employee = context.GetObject<StaffEmployee>(id);
			employee.InactiveStatus = StaffEmployeeInactiveStatus.BusinessTrip;
			
			// citiesId = new Guid("4c5f3354-e27b-4053-9ba1-9c1213059163");
			//var obj = context.GetObject<BaseUniversalItem>(citiesId);
			//Console.WriteLine(obj.ItemCard.MainInfo["daySalary"].ToString());
		}
		public static void mineLogic(UserSession session, ObjectContext context) {
			//Console.WriteLine($"Session: {session.Id}");
			/*var docSvc = context.GetService<IDocumentService>();
			
			var cardKindId = new Guid("{B118D1EA-E477-4150-ADB7-4720A2FBD6AE}");
			var cardKind = context.GetObject<KindsCardKind>(cardKindId);

			var citiesId = new Guid("{caef1c8d-e401-4efa-831e-aa30cc942c3e}");
			var obj = context.GetObject<BaseUniversal>(citiesId);
			*/
			/*
			var id = new Guid("c4f4823e-345b-4f87-86a5-4beded1257d5");
			var card = context.GetObject<Document>(id);
			
			var kindSvc = context.GetService<IKindService>();
			Console.WriteLine(cardKind.GetObjectId());
			Console.WriteLine(card.SystemInfo.CardKind.GetObjectId());*/

			/*for(int i = 0;i< 5; i++) {
			createCard(session, context, cardKind,0);
			}*/
			//var docSvc = context.GetService<IDocumentService>();
			/*var roleModelService = context.GetService<IRoleModelService>();
			
			var staffSvc = context.GetService<IStaffService>();
			var group = staffSvc.GetGroup(new Guid("{5103C1D3-69AA-4CA7-AD70-CCFD0B4220E6}"));
			var groupStaff = staffSvc.GetGroupEmployees(group);
			foreach (var item in groupStaff) {
				Console.WriteLine(item.Status.ToString()=="Active");
				Console.WriteLine(item.FullName);
			}*/
			var origin = "LED";
			var destination = "MOW";
			var token = "b165d8c4be5500d4da61df5067fd34ad";
			var departure_at= "2025-11-19";
			var return_at = "2025-11-25";
			var direct = "true";
			var limit = "10";
			using HttpClient client = new HttpClient();
			
			string url = "https://api.travelpayouts.com/aviasales/v3/prices_for_dates?" +
				"origin=" + origin + "&" +
				"destination=" + destination + "&" +
				"departure_at="+ departure_at +"&" +
				"return_at="+ return_at +"&" +
				"unique=false&" +
				"sorting=price&" +
				"direct="+direct+"&" +
				"currency=rub&" +
				"limit="+limit+"&" +
				"page=1&" +
				"one_way=true&" +
				"token="+token;

			HttpResponseMessage response = client.GetAsync(url).Result;
			var responseBody = response.Content.ReadAsStream();
			using(StreamReader reader = new StreamReader(responseBody, Encoding.UTF8))

			{
				string content = reader.ReadToEnd();
				var js = JsonObject.Parse(content);

				Console.WriteLine(js["data"][0]); // Output: Hello from MemoryStream!
			}
			
			//Test(session, context);
			//var document = context.GetObject<Document>(new Guid("6221a6cd-b4d7-49a2-af98-54415cb2e999"));
			//Console.WriteLine(document.SystemInfo.State);
		}
		public static void SomeLogic(UserSession session, ObjectContext context) {
			Console.WriteLine($"Session: {session.Id}");

			var document = context.GetObject<Document>(new Guid("d544efd9-d4e8-4cea-8763-3dc7a9a60728"));
			var docName = document.MainInfo.Name;
			var author = document.MainInfo.Registrar;
			Console.WriteLine($"Card name: {document.MainInfo.Name}, author: {author.DisplayString}");

			//var officeMemoKindId = new Guid("{2E3456C8-F1B4-4A82-908A-560A8AF6E7DF}");
			//var officeMemoKind = context.GetObject<KindsCardKind>(officeMemoKindId);

			var officeMemoKind = context.FindObject<KindsCardKind>(
				new QueryObject(
					KindsCardKind.NameProperty.Name, "Служебная записка"));

			var requestTypeId = new Guid("{12A19587-C6C0-477F-9811-EFEBAB3FBBE3}");
			var requestType = context.GetObject<BaseUniversalItem>(requestTypeId);

			var docSvc = context.GetService<IDocumentService>();
			var staffSvc = context.GetService<IStaffService>();
			var officeMemo = docSvc.CreateDocument(null, officeMemoKind);
			officeMemo.MainInfo.Author = staffSvc.GetCurrentEmployee();
			officeMemo.MainInfo.Registrar = staffSvc.GetCurrentEmployee();
			officeMemo.MainInfo[CardDocument.MainInfo.RegDate] = DateTime.Now;
			officeMemo.MainInfo.Name = "Card created from code";
			officeMemo.MainInfo.Item = requestType;

			var approvers = (IList<BaseCardSectionRow>)officeMemo.GetSection(CardDocument.Approvers.ID);
			var approverRow1 = new BaseCardSectionRow();
			approverRow1[CardDocument.Approvers.Approver] = staffSvc.GetCurrentEmployee().GetObjectId();
			approvers.Add(approverRow1);
			var approverRow2 = new BaseCardSectionRow();
			approverRow2[CardDocument.Approvers.Approver] = staffSvc.FindEmpoyeeByAccountName("ENGINEER\\DVAdmin")?.GetObjectId();
			approvers.Add(approverRow2);

			context.AcceptChanges();

			ChangeCardState(context, officeMemo, "Is approving");
			context.AcceptChanges();

			Console.WriteLine($"New card id: {officeMemo.GetObjectId()}");
		}
	}
}
