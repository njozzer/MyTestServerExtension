using Autofac;
using DocsVision.BackOffice.CardLib.CardDefs;
using DocsVision.Layout.WebClient.Services;
using DocsVision.WebClient.Extensibility;
using DocsVision.WebClient.Helpers;
using DocsVision.WebClientLibrary.ObjectModel.Services.EntityLifeCycle;
using Microsoft.Extensions.DependencyInjection;
using MyTestServerExtension.CardLifeCycle;
using MyTestServerExtension.DataGridPlugins;
using MyTestServerExtension.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Resources;


namespace MyTestServerExtension
{

    public class MyServerExtension : WebClientExtension
    {
       
        public MyServerExtension()
            : base()
        {

        }
        public override string ExtensionName => "ServerExtension";

        public override Version ExtensionVersion => new Version(1, 0, 0);


        public override void InitializeServiceCollection(IServiceCollection services)
        {
            services.AddSingleton<IMyTestService, MyTestService>();
            services.AddSingleton<IDataGridControlPlugin, MyDataPlugin>();
            services.Decorate<ICardLifeCycleEx>((original, serviceProvider) => {
                var typeId = original.CardTypeId;
                if (typeId == CardDocument.ID)
                {
                    var feature1Service = serviceProvider.GetRequiredService<IMyTestService>();
                    return new TestCardLifeCycle(original, feature1Service);
                }
                return original;
            });
           
            
        }


    }
}