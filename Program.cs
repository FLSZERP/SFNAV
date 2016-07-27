using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFAPP.SFDC;
namespace SFAPP
{
    class Program
    {
        static void Main(string[] args)
        {

            MainFunction mf = new MainFunction();
            Console.WriteLine("Salesforce & NAV synchronization");
            Console.WriteLine();
            Console.WriteLine("Select one option");
            Console.WriteLine("1. Get records from Salesforce");
            Console.WriteLine("2. Get records from NAV");
            Console.WriteLine("3. Insert Account to Salesforce");
            Console.WriteLine("4. Insert Contact to Salesforce");
            Console.WriteLine("5. Sync from Salesforce to NAV");
            Console.WriteLine();
            switch (Console.ReadKey().KeyChar)
            {
                case ('1'):                   
                    mf.InsertToSFContactFromSFService();
                    Console.ReadLine();
                    break;
                case ('2'):
                    mf.InitNAVContact();
                    Console.ReadLine();
                    break;
                case ('3'):
                    mf.InsertAccountToSF();
                    Console.ReadLine();
                    break;
                case ('4'):
                    mf.InsertContactToSF();
                    Console.WriteLine("Done");
                    Console.ReadLine();
                    break;
                case ('5'):                    
                    Console.WriteLine("\nAccounts synchronization");
                    mf.SyncAccountToNAV();
                    Console.WriteLine("Contacts synchronization");
                    mf.SyncContactToNAV();
                    Console.WriteLine("Done");
                    Console.ReadLine();
                    break;
            }
            

        }
        
    }
}
