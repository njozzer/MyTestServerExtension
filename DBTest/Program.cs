using DBTest.utils;

using Npgsql;



namespace MyTestServerExtension
{
    class Program {
        public static void Main(string[] args) {
            
            var file = new Utils("DATA.csv","c:\\Users\\njozzer\\source\\repos\\MyTestServerExtension\\");
            file.readFile();
            
        }
        
    }
}