using DocsVision.Platform.Data;
using DocsVision.Platform.StorageServer;
using DocsVision.Platform.StorageServer.Extensibility;
using System.Text.Json;

namespace MyTestPlatformExtension
{
    public class Row {
        public string Id { get; set; }
        public string dateFrom { get; set; }
        public string cityRef { get; set; }
        public string reason { get; set; }
        public string state { get; set; }
        public Row(string id, string dateFrom,string cityRef,string reason,string state)
        {
            this.Id = id;
            this.dateFrom = dateFrom;
            this.cityRef = cityRef;
            this.reason = reason;
            this.state = state;
        }
    }
    public class SE: StorageServerExtension
    {
        public SE() { 
            
        }
        [ExtensionMethod]
        public string getTrips(Guid employeeId) {
            DbRequest.DataLayer.Connection.CreateCommand();
            using (var cmd = base.DbRequest.DataLayer.Connection.CreateCommand("getTripData", System.Data.CommandType.StoredProcedure))
            {
                cmd.AddParameter("EmployeeId", System.Data.DbType.Guid, System.Data.ParameterDirection.Input, 0, employeeId);
                var dbr = cmd.ExecuteReader();
                var res = "[";
                if (dbr.Read()) {
                    res += "{";
                    res += "\"t1\":\"" + dbr["t1"]+"\",";
                    res += "\"t2\":\"" + dbr["t2"] + "\",";
                    res += "\"t3\":\"" + dbr["t3"] + "\",";
                    res += "\"t4\":\"" + dbr["t4"] + "\",";
                    res += "\"t5\":\"" + dbr["t5"] + "\"";
                    res += "}";
                    /*var row = new Row((string)dbr["t1"], (string)dbr["t2"], (string)dbr["t3"], (string)dbr["t4"], (string)dbr["t5"]);
                    res += JsonSerializer.Serialize(row);*/
                }
                
                while (dbr.Read())
                {
                    res += ",";
                    res += "{";
                    res += "\"t1\":\"" + dbr["t1"] + "\",";
                    res += "\"t2\":\"" + dbr["t2"] + "\",";
                    res += "\"t3\":\"" + dbr["t3"] + "\",";
                    res += "\"t4\":\"" + dbr["t4"] + "\",";
                    res += "\"t5\":\"" + dbr["t5"] + "\"";
                    res += "}";
                    /*
                    var row = new Row((string)dbr["t1"], (string)dbr["t2"], (string)dbr["t3"], (string)dbr["t4"], (string)dbr["t5"]);
                    res += JsonSerializer.Serialize(row);*/


                }
                res+= "]";
                return res;

            }  
            
        }
    }
}