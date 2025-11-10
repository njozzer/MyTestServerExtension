using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

using System.DirectoryServices;

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
        public void createUserData(string userID,string cityId, DB db)
        {

            var tuple_ = db.ifUserDataNotExists(userID, cityId);
            if (!tuple_.Item1)
            {
                var tuple_2 = db.getUserCityId(cityId);
                db.InsertUserCities(userID, tuple_2.Item2);

            }
        }
        
        public static void CreateADUser(string domainController, string containerOU, string username, string password, string firstName, string lastName)
        {
            
            try
            {
                // Connect to the specified OU in Active Directory
                using (DirectoryEntry ouEntry = new DirectoryEntry($"LDAP://{domainController}/{containerOU}"))
                {
                    // Create a new user entry
                    using (DirectoryEntry newUser = ouEntry.Children.Add($"CN={firstName} {lastName}", "user"))
                    {
                        // Set mandatory properties
                        newUser.Properties["sAMAccountName"].Value = username;
                        newUser.Properties["givenName"].Value = firstName;
                        newUser.Properties["sn"].Value = lastName;
                        newUser.Properties["displayName"].Value = $"{firstName} {lastName}";
                        newUser.Properties["userPrincipalName"].Value = $"{username}@{domainController.Split('.')[0]}.local"; // Adjust domain part as needed

                        // Commit changes to create the user object
                        newUser.CommitChanges();

                        // Set the password (must be done after committing the user object)
                        newUser.Invoke("SetPassword", new object[] { password });

                        // Enable the user account
                        newUser.Properties["userAccountControl"].Value = 0x200; // ADS_UF_NORMAL_ACCOUNT
                        newUser.CommitChanges();

                        Console.WriteLine($"User '{username}' created successfully in Active Directory.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user: {ex.Message}");
            }
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
            foreach(var city in citiesArr)
            {
                var cityData = city.Split(":");
                if (cityData[1] == "да")
                {
                    var cityId = db.getCityId(cityData[0]);
                    Console.WriteLine(cityId.Item2);
                    createUserData(userID, cityId.Item2, db);
                }
            }
            

        }
        public void test() {
            string domainController = "engineer.school.local";
            string containerOU = "OU=Users,DC=engineer.school,DC=local";
            string username = "nedopekin_ey";
            string password = "P@ssw0rd";
            string firstName = "New";
            string lastName = "User";
            CreateADUser(domainController,containerOU,username,password,firstName,lastName);
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
