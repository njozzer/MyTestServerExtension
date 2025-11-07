using DocsVision.BackOffice.ObjectModel;
using DocsVision.Platform.WebClient;
using DocsVision.WebClientLibrary.ObjectModel.Services.EntityLifeCycle;
using DocsVision.WebClientLibrary.ObjectModel.Services.EntityLifeCycle.Options;
using MyTestServerExtension.Helpers;
using MyTestServerExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTestServerExtension.CardLifeCycle
{

    public class TestCardLifeCycle : ICardLifeCycleEx
    {
        
        public TestCardLifeCycle(ICardLifeCycleEx baseLifeCycle,IMyTestService myTestService) { 
            this.baseLifeCycle = baseLifeCycle;
            this.myTestService = myTestService;

        }  
        protected ICardLifeCycleEx baseLifeCycle {  get;  }
        protected IMyTestService myTestService { get; }
        public Guid CardTypeId => baseLifeCycle.CardTypeId;

        public Guid Create(SessionContext sessionContext, CardCreateLifeCycleOptions options)
        {
            var cardId = baseLifeCycle.Create(sessionContext, options);
            if (options.CardKindId == Const.cardId) {
                myTestService.InitMyCard(sessionContext,cardId);
            }
            return cardId;
        }

        public bool CanDelete(SessionContext sessionContext, CardDeleteLifeCycleOptions options, out string message)
            => baseLifeCycle.CanDelete(sessionContext, options, out message);

        public string GetDigest(SessionContext sessionContext, CardDigestLifeCycleOptions options)
            => baseLifeCycle.GetDigest(sessionContext,options);

        public void OnDelete(SessionContext sessionContext, CardDeleteLifeCycleOptions options)
            => baseLifeCycle.OnDelete(sessionContext,options);

        public void OnSave(SessionContext sessionContext, CardSaveLifeCycleOptions options)
        {

            var doc = sessionContext.ObjectContext.GetObject<Document>(options.CardId);
            if (doc.SystemInfo.CardKind.GetObjectId() == Const.cardId)
            {

            }
            baseLifeCycle.OnSave(sessionContext, options);
        }


        public bool Validate(SessionContext sessionContext, CardValidateLifeCycleOptions options, out List<ValidationResult> validationResults)
            => baseLifeCycle.Validate(sessionContext,options,out validationResults);
        
    }

}
