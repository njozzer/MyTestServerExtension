using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTest.utils
{
    public class Utils
    {
        string fileName;
        string path;
        public Utils(string fileName, string path)
        {
            this.fileName = fileName;
            this.path = path;
        }
        public void createUnitIfNotExists(string[] unitArr, DB db) {
            var tuple_ = db.ifNotExistsUnit(unitArr[2]);
            if (!tuple_.Item1)
            {
                var parentTuple = db.ifNotExistsUnit(unitArr[1]);
                db.InsertUnit(unitArr[2], parentTuple.Item2);
            }
            
        }
        public void createPositionIfNotExists(string position, DB db)
        {
            var tuple_ = db.ifNotExistsPosition(position);
            if (!tuple_.Item1)
            {
                db.InsertPosition(position);
            }

        }
        public void createUser(string[] userArr, string unitID, string position, DB db) {
            
            var tuple_ = db.getUserRowID(userArr, unitID, position);
            if (!tuple_.Item1)
            {
                db.InsertUser(userArr, unitID, position);
                
            }
        }
        public void createUserData(string userID,uint citiesid, DB db)
        {

            var tuple_ = db.ifUserDataNotExists(userID,citiesid);
            if (!tuple_.Item1)
            {
                db.InsertUserCities(userID, citiesid);

            }
        }
        public uint getCitiesID(string[] citiesArr)
        {
            uint id = 1;
            var i = 2;
            foreach (var c in citiesArr)
            {
               
                var tmp = c.Split(":")[1];
                
                if (tmp =="да")
                {
                    id += (uint)1 << i;
                }
                else
                {

                }
                i--;
            }
            return id;
        }
        public void doAction(string line, DB db) {
            string? unitID;
            string? positionID;
            string? userID;
            var arr = line.Split(",");
            var unitArr = arr[0].Split("\\");
            var nameArr = arr[1].Split(" ");
            var positionName = arr[2];
            var citiesArr = arr[3].Split(" ");

            //var citiesArr = tmp.Split(" ");

            createUnitIfNotExists(unitArr, db);
            createPositionIfNotExists(positionName, db);

            unitID = db.ifNotExistsUnit(unitArr[2]).Item2;
            Console.WriteLine(unitID);
            positionID = db.ifNotExistsPosition(positionName).Item2;
            Console.WriteLine(positionID);
            createUser(nameArr, unitID, positionID, db);
            userID = db.getUserRowID(nameArr, unitID, positionID).Item2;
            Console.WriteLine(userID);
            createUserData(userID, getCitiesID(citiesArr), db);
        }
        public void readFile()
        {
            var db = new DB();
            var reader = new StreamReader(path + fileName);
            string? line;
                        
            while ((line = reader.ReadLine()) != null) {
                doAction(line, db);

            }
        }
    }
}
