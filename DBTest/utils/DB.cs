using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DBTest.utils
{
    public class DB
    {
        string connectionString = "Server=localhost;Port=5432;Database=maindb;User Id=postgres;Password=postgres;";
        public DB()
        {
            
        }
        public NpgsqlConnection GetConnection() {
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            return conn;
        }
       
        public Tuple<bool,string?> ifNotExistsUnit(string Name) {
            string query = "select a.\"Name\", a.\"RowID\" from \"dvtable_{7473f07f-11ed-4762-9f1e-7ff10808ddd1}\" a " +
                "where " +
                "a.\"Name\" = @Name";
            var conn = GetConnection();
            var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("Name", Name);
            var reader = cmd.ExecuteReader();
            var isExists = reader.Read();
            var data = "";
            if (isExists) {
                data = reader.GetValue(1).ToString();
            }
            conn.Close();
            return new Tuple<bool, string?> (isExists, data);
        }
        public Tuple<bool, string?> ifNotExistsPosition(string Name)
        {
            string query = "select a.\"Name\", a.\"RowID\" from \"dvtable_{cfdfe60a-21a8-4010-84e9-9d2df348508c}\" a " +
                "where " +
                "a.\"Name\" = @Name";
            var conn = GetConnection();
            var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("Name",Name);
            var reader = cmd.ExecuteReader();
            var isExists = reader.Read();
            var data = "";
            if (isExists)
            {
                data = reader.GetValue(1).ToString();
            }
            conn.Close();
            return new Tuple<bool, string?>(isExists, data);
        }
        
        public Tuple<bool, string?> getCityId(string Name)
        {
            string query = "select a.\"Name\", a.\"RowID\" from \"dvtable_{1b1a44fb-1fb1-4876-83aa-95ad38907e24}\" a " +
                "where " +
                "a.\"Name\" = @Name";
            var conn = GetConnection();
            var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("Name", Name);
            var reader = cmd.ExecuteReader();
            var isExists = reader.Read();
            var data = "";
            if (isExists)
            {
                data = reader.GetValue(1).ToString();
            }
            conn.Close();
            return new Tuple<bool, string?>(isExists, data);
        }
        public Tuple<bool, int> getUserCityId(string cityID)
        {
            string query = "select id from user_cities " +
                "where " +
                "cityid = @cityid";
            Guid cityGUID = Guid.Parse(cityID);
            var conn = GetConnection();
            var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("cityid", NpgsqlTypes.NpgsqlDbType.Uuid, cityGUID);
            var reader = cmd.ExecuteReader();
            var isExists = reader.Read();
            int data = 0;
            if (isExists)
            {
                data = reader.GetInt32(0);
            }
            conn.Close();
            return new Tuple<bool, int>(isExists, data);
        }
        public Tuple<bool, string?> getUserRowID(string[] userArr, string unitID, string position)
        {
            string query = "select a.\"RowID\" from \"dvtable_{dbc8ae9d-c1d2-4d5e-978b-339d22b32482}\" a " +
                "where " +
                "a.\"FirstName\" = @FirstName and " +
                "a.\"MiddleName\" = @MiddleName and " +
                "a.\"LastName\" = @LastName and " +
                "a.\"ParentRowID\" = @ParentRowID and " +
                "a.\"Position\" = @Position";
            var conn = GetConnection();
            var cmd = new NpgsqlCommand(query, conn);
            Guid unitGUID = Guid.Parse(unitID);
            Guid positionGUID = Guid.Parse(position);
            cmd.Parameters.AddWithValue("FirstName", userArr[0]);
            cmd.Parameters.AddWithValue("MiddleName", userArr[1]);
            cmd.Parameters.AddWithValue("LastName", userArr[2]);
            cmd.Parameters.AddWithValue("ParentRowID", NpgsqlTypes.NpgsqlDbType.Uuid, unitGUID);
            cmd.Parameters.AddWithValue("Position", NpgsqlTypes.NpgsqlDbType.Uuid, positionGUID);
            var reader = cmd.ExecuteReader();
            var isExists = reader.Read();
            var data = "";
            if (isExists)
            {
                data = reader.GetValue(0).ToString();
            }
            conn.Close();
            return new Tuple<bool, string?>(isExists, data);
        }
        public Tuple<bool, int> ifUserDataNotExists(string userID, string cityID)
        {
            string query = "select uc.id from user_data " +
                "join user_cities uc " +
                "on (uc.id = user_data.cityid) " +
                "where " +
                "userid = @userid and " +
                "uc.cityid = @cityid" ;
            var conn = GetConnection();
            Guid userGUID = Guid.Parse(userID);
            Guid cityGUID = Guid.Parse(cityID);
            var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("userid", NpgsqlTypes.NpgsqlDbType.Uuid, userGUID);
            cmd.Parameters.AddWithValue("cityid", NpgsqlTypes.NpgsqlDbType.Uuid, cityGUID);
            var reader = cmd.ExecuteReader();
            var isExists = reader.Read();
            int data = 0;
            if (isExists)
            {
                data = reader.GetInt32(0);
            }
            conn.Close();
            return new Tuple<bool, int>(isExists, data);
        }
        public void InsertUnit(string Name,string ParentTreeRowID) {
            string query = "insert into \"dvtable_{7473f07f-11ed-4762-9f1e-7ff10808ddd1}\" " +
                "(\"Name\", \"ParentTreeRowID\", \"Type\", \"SDID\" ) " +
                "values (@Name, @ParentTreeRowID, @Type, @SDID)";
            Guid SDID = Guid.Parse("58a04bb4-154c-44b4-b8c4-ac3e8d4c5b27");
            var conn = GetConnection();
            var cmd = new NpgsqlCommand(query, conn);
            Guid parentGUID = Guid.Parse(ParentTreeRowID);
            cmd.Parameters.AddWithValue("Name", Name);
            cmd.Parameters.AddWithValue("ParentTreeRowID", NpgsqlTypes.NpgsqlDbType.Uuid, parentGUID );
            cmd.Parameters.AddWithValue("Type", 1);
            cmd.Parameters.AddWithValue("SDID", NpgsqlTypes.NpgsqlDbType.Uuid, SDID);
            var reader = cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void InsertPosition(string Name)
        {
            string query = "insert into \"dvtable_{cfdfe60a-21a8-4010-84e9-9d2df348508c}\" " +
                "(\"Name\") " +
                "values (@Name)";
            var conn = GetConnection();
            var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("Name", Name);
            var reader = cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void InsertUser(string[] userArr,string unitID, string position) {
            string query = "insert into \"dvtable_{dbc8ae9d-c1d2-4d5e-978b-339d22b32482}\"" +
                "(\"FirstName\",\"MiddleName\",\"LastName\", \"ParentRowID\",\"SDID\", \"Position\", \"DisplayString\") " +
                "values (@FirstName, @MiddleName, @LastName, @ParentRowID, @SDID, @Position, @DisplayString)";
            Guid SDID = Guid.Parse("58a04bb4-154c-44b4-b8c4-ac3e8d4c5b27");
            var conn = GetConnection();
            var displayString = userArr[2] + "";
            var cmd = new NpgsqlCommand(query, conn);
            Guid parentGUID = Guid.Parse(unitID);
            Guid positionID = Guid.Parse(position);
            cmd.Parameters.AddWithValue("FirstName", userArr[0]);
            cmd.Parameters.AddWithValue("MiddleName", userArr[1]);
            cmd.Parameters.AddWithValue("LastName", userArr[2]);
            cmd.Parameters.AddWithValue("ParentRowID", NpgsqlTypes.NpgsqlDbType.Uuid, parentGUID);
            cmd.Parameters.AddWithValue("SDID", NpgsqlTypes.NpgsqlDbType.Uuid, SDID);
            cmd.Parameters.AddWithValue("Position", NpgsqlTypes.NpgsqlDbType.Uuid, positionID);
            cmd.Parameters.AddWithValue("DisplayString", "");
            var reader = cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void InsertCities() { 
            
        }
        public void InsertUserCities(string userID, int citiesid) {
            string query = "insert into user_data " +
                "(userid,cityid) " +
                "values (@userid, @cityid)";
            var conn = GetConnection();
            Console.WriteLine(citiesid);
            Guid userGUID = Guid.Parse(userID);
            var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("userid", NpgsqlTypes.NpgsqlDbType.Uuid, userGUID);
            cmd.Parameters.AddWithValue("cityid", NpgsqlTypes.NpgsqlDbType.Integer, citiesid);
            var reader = cmd.ExecuteNonQuery();
            conn.Close();
        }
        
    }
}
