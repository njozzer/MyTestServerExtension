using DocsVision.BackOffice.ObjectModel;
using DocsVision.Layout.WebClient.Models;
using DocsVision.Layout.WebClient.Models.TableData;
using DocsVision.Layout.WebClient.Services;
using DocsVision.Platform.ObjectManager;
using DocsVision.Platform.WebClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTestServerExtension.DataGridPlugins
{
    internal class MyDataPlugin : IDataGridControlPlugin
    {
        public string Name => "GetDataPlugin";

        public TableModel GetTableData(SessionContext sessionContext, List<ParamModel> parameters)
        {
            TableModel tableModel = new TableModel();
            string rowNumColumn = "id";
            string dateColumn = "dateFrom";
            string cityColumn = "city";
            string reasonColumn = "reason";
            string statusColumn = "status";
            tableModel.Columns.Add(new ColumnModel() { 
                Id = rowNumColumn,
                Name = "№",
                Type = DocsVision.WebClient.Models.Grid.ColumnType.UniqueId
            });
            tableModel.Columns.Add(new ColumnModel()
            {
                Id = dateColumn,
                Name = "Дата выезда",
                Type = DocsVision.WebClient.Models.Grid.ColumnType.DateTime
            });
            tableModel.Columns.Add(new ColumnModel()
            {
                Id = cityColumn,
                Name = "Город",
                Type = DocsVision.WebClient.Models.Grid.ColumnType.String
            });
            tableModel.Columns.Add(new ColumnModel()
            {
                Id = reasonColumn,
                Name = "Основание для поездки",
                Type = DocsVision.WebClient.Models.Grid.ColumnType.String
            });
            tableModel.Columns.Add(new ColumnModel()
            {
                Id = statusColumn,
                Name = "Статус заявки",
                Type = DocsVision.WebClient.Models.Grid.ColumnType.String
            });
            ExtensionManager extensionManager = sessionContext.Session.ExtensionManager;
            ExtensionMethod getTrips = extensionManager.GetExtensionMethod("MyExtension", "getTrips");
            getTrips.Parameters.AddNew("employeeId", ParameterValueType.Guid, sessionContext.UserInfo.EmployeeId);
            var res = (string)getTrips.Execute() + "";
            var arr = JArray.Parse(res);
            foreach( var item in arr)
            {
                var t = "В процессе";
                if (item["t5"].ToString() == "Закрыто")
                {
                    t = "Завершена";
                }
                tableModel.Rows.Add(new RowModel()
                {
                    Id = "1",
                    EntityId = "2",
                    Cells = new List<CellModel>()
                {
                    new CellModel() {
                        ColumnId = rowNumColumn,
                        Value = item["t1"]
                    },
                    new CellModel() {
                        ColumnId = dateColumn,
                        Value = item["t2"]
                    },
                    new CellModel() {
                        ColumnId = cityColumn,
                        Value = item["t3"]
                    }
                    ,
                    new CellModel() {
                        ColumnId = reasonColumn,
                        Value = item["t4"]
                    }
                    ,
                    new CellModel() {
                        ColumnId = statusColumn,
                        Value = t
                    }
                }
                });
                
            }
            
            return tableModel;
        }
    }
}
