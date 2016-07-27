using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFAPP.SFDC;
using SFAPP.SystemServiceRef;
using SFAPP.NAVContactPageRef;
using SFAPP.CompanyInfoRef;
using SFAPP.NAVContactRelPageRef;
namespace SFAPP
{
    class MainFunction
    {
        SF sf = new SF();
        static string comp1 = "Fiducia Limited - BAK";
        static string baseURL = "http://192.168.20.242:1003/Live01/WS/"; 

        //NAV Contact Service 
        string contactPageURL = baseURL + Uri.EscapeDataString(comp1) + "/Page/newcontact";
        newcontact_Service contactService = new newcontact_Service();

        //NAV Company Information Service
        string comPanyInfoPageURL = baseURL + Uri.EscapeDataString(comp1) + "/Page/CompanyInformation";
        CompanyInformation_Service companyInfoService = new CompanyInformation_Service();

        //NAV Contact Relation Service
        string contactRelPageURL = baseURL + Uri.EscapeDataString(comp1) + "/Page/ContactRelation";
        ContactRelation_Service contactRelService = new ContactRelation_Service();

        public MainFunction()
        {            
            contactService.Url = contactPageURL;
            contactService.UseDefaultCredentials = true;
 
            companyInfoService.Url = comPanyInfoPageURL;
            companyInfoService.UseDefaultCredentials = true;

            contactRelService.Url = contactRelPageURL;
            contactRelService.UseDefaultCredentials = true;
        }
        public void InsertAccountToSF()
        {
            Console.WriteLine("");
            newcontact_Filter filter1 = new newcontact_Filter();
            newcontact_Filter filter2 = new newcontact_Filter();
            newcontact_Filter filter3 = new newcontact_Filter();
            newcontact_Filter filter4 = new newcontact_Filter();
            newcontact_Filter filter5 = new newcontact_Filter();
            newcontact_Filter filter6 = new newcontact_Filter();

            filter1.Field = newcontact_Fields.Name_3;
            filter1.Criteria = "<>''";
            filter1.Field = newcontact_Fields.No;
            filter1.Criteria = "CT000001..CT000050";
            filter2.Field = newcontact_Fields.Obsolete_Contact;
            filter2.Criteria = "false";
            filter3.Field = newcontact_Fields.Type;
            filter3.Criteria = "Company";
            try
            {
                newcontact_Filter[] filters = new newcontact_Filter[] { filter1, filter2, filter3 };
                newcontact[] contacts = GetContactFromNAV(filters);
                Console.WriteLine("\n{0} records detected on NAV, currDate: {1}", contacts.Count(),DateTime.Now);
                Logging.Writelog("-----------------------------------------------------");
                Logging.Writelog("Insert to SF Account Begin, " + contacts.Count().ToString() + " records detected on NAV");
                int InsertCount = 0;
                foreach (newcontact contact1 in contacts)
                {
                    if (ValidateContactOnSF(contact1) == false)
                    {
                        Console.WriteLine("{0} has already exists in SF Account", contact1.No);
                        Logging.Writelog(contact1.No + " has already exists in SF");
                        continue;
                    }
                    //Transfer Account fields begin
                    Account SFAccount = new Account();
                    SFAccount.Name = contact1.Name_3;
                    SFAccount.Website = contact1.Home_Page;
                    SFAccount.Contact_No__c = contact1.No;
                    SFAccount.Phone = contact1.SF_Phone_No;
                    SFAccount.Description = contact1.g_txtBusDesc + contact1.g_txtHisImpDev + contact1.g_txtMainCust + contact1.g_txtMainComp;
                    SFAccount.BillingCountry = contact1.Country_Region_Code;
                    SFAccount.BillingCity = contact1.City;
                    SFAccount.BillingPostalCode = contact1.Post_Code;
                    SFAccount.BillingStreet = contact1.Address_3;
                    //Transfer Account fields end
                    string result = "";
                    if (sf.InsertAccountToSF(SFAccount, out result) == true)
                    {
                        InsertCount += 1;
                        Console.WriteLine("Account {0} inserted, Id on SF is {1}", contact1.No, result);
                        Logging.Writelog(contact1.No + " inserted, Id on SF is " + result);
                    }
                    else
                    {
                        Console.WriteLine("Account {0} can not insert to SF, error result: {1}", contact1.No, result);
                        Logging.Writelog(result);
                    }               

                }
                if (InsertCount > 0)
                {
                    Console.WriteLine("{0} records inserted", InsertCount);
                    DateTime dt = DateTime.Now;
                    SetLastSyncDateTimeInNAV(dt.AddSeconds(5));
                }
                else Console.WriteLine("Nothing insert");
                Logging.Writelog("Insert to SF Account END, " + InsertCount.ToString() + " records inserted");
                Logging.Writelog("-----------------------------------------------------");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Logging.WriteBuglog(e);
                throw e;
            }
        }

        /// <summary>
        /// Insert Contact to Salesforce from NAV, please set filters & fields needed on this function
        /// </summary>
        public void InsertContactToSF()
        {
            Console.WriteLine("");
            newcontact_Filter filter1 = new newcontact_Filter();
            newcontact_Filter filter2 = new newcontact_Filter();
            newcontact_Filter filter3 = new newcontact_Filter();
            newcontact_Filter filter4 = new newcontact_Filter();
            newcontact_Filter filter5 = new newcontact_Filter();
            newcontact_Filter filter6 = new newcontact_Filter();

            filter1.Field = newcontact_Fields.Name_3;
            filter1.Criteria = "<>''";
            filter1.Field = newcontact_Fields.No;
            filter1.Criteria = "CT000001..CT000200";
            filter2.Field = newcontact_Fields.Obsolete_Contact;
            filter2.Criteria = "false";
            filter3.Field = newcontact_Fields.Type;
            filter3.Criteria = "Person";
            try
            {
                newcontact_Filter[] filters = new newcontact_Filter[] { filter1, filter2, filter3 };
                newcontact[] contacts = GetContactFromNAV(filters);
                Console.WriteLine("\n{0} records detected on NAV", contacts.Count());
                Logging.Writelog("-----------------------------------------------------");
                Logging.Writelog("Insert to SF Begin, " + contacts.Count().ToString() + " records detected on NAV");
                int InsertCount = 0;
                foreach (newcontact contact1 in contacts)
                {
                    if (ValidateContactOnSF(contact1) == false)
                    {
                        Console.WriteLine("{0} has already exists in SF", contact1.No);
                        Logging.Writelog(contact1.No+" has already exists in SF");
                        continue;
                    }
                    Contact SFcontact = new Contact();
                    SFcontact.ContactNo__c = contact1.No;
                    SFcontact.FirstName = contact1.First_Name;
                    SFcontact.LastName = contact1.Surname;
                    SFcontact.Phone = contact1.Phone_No;
                    SFcontact.Birthdate = contact1.Birthday_DM;
                    SFcontact.Description = contact1.Type.ToString();
                    SFcontact.Title = contact1.Job_Title;
                    SFcontact.MailingCountry = contact1.Country_Region_Code;
                    SFcontact.MailingCity = contact1.City;
                    SFcontact.MailingPostalCode = contact1.Post_Code;
                    SFcontact.MailingStreet = contact1.Address_3;

                    if (Validator.IsEmail(contact1.E_Mail) == true) SFcontact.Email = contact1.E_Mail;
                    string result = "";
                    if (sf.InsertContactToSF(SFcontact, out result) == true)
                    {
                        InsertCount += 1;
                        Console.WriteLine("{0} inserted, Id on SF is {1}", contact1.No, result);
                        Logging.Writelog(contact1.No + " inserted, Id on SF is " + result);
                    }
                    else
                    {
                        Console.WriteLine("{0} can not insert to SF, error result: {1}", contact1.No, result);
                        Logging.Writelog(result);
                    }


                }
                if (InsertCount > 0) 
                { 
                    Console.WriteLine("{0} records inserted", InsertCount);
                    DateTime dt = DateTime.Now;
                    SetLastSyncDateTimeInNAV(dt.AddSeconds(5));                    
                }
                else Console.WriteLine("Nothing insert");
                Logging.Writelog("Insert to SF END, " + InsertCount.ToString() + " records inserted1");
                Logging.Writelog("-----------------------------------------------------");
                Console.ReadLine();
            }
            catch(Exception e)
            {
                Logging.WriteBuglog(e);
                throw e;
            }

        }

        /// <summary>
        /// Compare contact no. in SF, do not insert if contact no. has exists
        /// </summary>
        /// <param name="contact1">NAV contact entity</param>
        /// <returns>bool</returns>
        private bool ValidateContactOnSF(newcontact contact1)
        {
            QueryResult queryResult = sf.QuerySFFromContact("ContactNo__c='" + contact1.No + "'");
            if (contact1 != null)
            {
                if (queryResult.records != null)
                {

                    return false;
                }
                else return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// Sync contacts between SF & NAV, please set fields needed
        /// </summary>
        public void SyncContactToNAV()
        {
            //Get Last Synchronization datetime in FL
            DateTime dt = GetLastSyncDateTimeFromNAV();
            DateTime dt2 = DateTime.Now.ToLocalTime();
          
            QueryResult queryResult = null;
            try
            {
                if (Validator.IsDateTime(dt.ToString()) == true)
                {
                    queryResult = sf.QuerySFFromContact(" LastModifiedDate>=" + dt.ToString("yyyy-MM-ddTHH:mm:ss.000Z") + " and LastModifiedDate<=" +
                                                        dt2.ToString("yyyy-MM-ddTHH:mm:ss.000Z") + " and ContactNo__c like '%CT%' order by ContactNo__c");

                }
                                                       
                int processCount = 0;
                if (queryResult.records != null)
                {
                    Console.WriteLine("\nRecords date filter: " + GetChinaDateTime(dt).ToString("yyyy-MM-dd HH:mm:ss") + " to " + dt2.ToString("yyyy-MM-dd HH:mm:ss"));
                    for (int i = 0; i < queryResult.records.Count(); i++)
                    {
                        Contact contact = (Contact)queryResult.records[i];
                        newcontact navcontact = contactService.Read(contact.ContactNo__c);
                        if (navcontact != null)
                        {
                            navcontact.First_Name = contact.FirstName;
                            navcontact.Surname = contact.LastName;
                            navcontact.E_Mail = contact.Email;
                            navcontact.Name_3 = contact.Name;
                            navcontact.Phone_No = contact.Phone;
                            if (contact.Birthdate!=null) navcontact.Birthday_DM = (DateTime)contact.Birthdate;
                            navcontact.Job_Title = contact.Title;
                            navcontact.Country_Region_Code = contact.MailingCountry;
                            navcontact.City = contact.MailingCity;
                            navcontact.Post_Code = contact.MailingPostalCode;
                            navcontact.Address_3 = contact.MailingStreet;                          
                            contactService.Update(ref navcontact);
                            processCount += 1;
                            Console.WriteLine(contact.ContactNo__c + " updated");
                        }
                        else
                        {
                            Console.WriteLine("No record find on NAV");
                        }
                    }
                    if (processCount > 0) SetLastSyncDateTimeInNAV(dt2.AddSeconds(-30));
                }
                //else Console.WriteLine("\nNothing detected from SF with date filter{1} to {2}", GetChinaDateTime(dt).ToString("yyyy-MM-dd HH:mm:ss"), dt2.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            catch(Exception e)
            {
                Logging.WriteBuglog(e);
                throw e;
            }
            Console.ReadLine();
        }
        /// <summary>
        /// Sync contacts between SF & NAV, please set fields needed
        /// </summary>
        public void SyncAccountToNAV()
        {
            //Get Last Synchronization datetime in FL
            DateTime dt = GetLastSyncDateTimeFromNAV();
            DateTime dt2 = DateTime.Now.ToLocalTime();

            QueryResult queryResult = null;
            try
            {
                if (Validator.IsDateTime(dt.ToString()) == true)
                {
                    queryResult = sf.QuerySFFromAccount(" LastModifiedDate>=" + dt.ToString("yyyy-MM-ddTHH:mm:ss.000Z") + " and LastModifiedDate<=" +
                                                        dt2.ToString("yyyy-MM-ddTHH:mm:ss.000Z") + " and Contact_No__c like '%CT%' order by Contact_No__c");

                }

                int processCount = 0;
                if (queryResult.records != null)
                {
                    Console.WriteLine("\nRecords of SF Account date filter: " + GetChinaDateTime(dt).ToString("yyyy-MM-dd HH:mm:ss") + " to " + dt2.ToString("yyyy-MM-dd HH:mm:ss"));
                    for (int i = 0; i < queryResult.records.Count(); i++)
                    {
                        Account account = (Account)queryResult.records[i];
                        newcontact navcontact = contactService.Read(account.Contact_No__c);
                        if (navcontact != null)
                        {
                            navcontact.Name_3 = account.Name;
                            navcontact.SF_Phone_No = account.Phone;
                            navcontact.Country_Region_Code = account.BillingCountry;
                            navcontact.City = account.BillingCity;
                            navcontact.Post_Code = account.BillingPostalCode;
                            navcontact.Address_3 = account.BillingStreet;
                            contactService.Update(ref navcontact);
                            processCount += 1;
                            Console.WriteLine("Contact No.: "+account.Contact_No__c + " updated");
                        }
                        else
                        {
                            Console.WriteLine("No record find on NAV");
                        }
                    }
                    if (processCount > 0) SetLastSyncDateTimeInNAV(dt2.AddSeconds(-30));
                }
                //else Console.WriteLine("\nNothing detected from SF with date filter{1} to {2}", GetChinaDateTime(dt).ToString("yyyy-MM-dd HH:mm:ss"), dt2.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            catch (Exception e)
            {
                Logging.WriteBuglog(e);
                throw e;
            }
        }
        public DateTime GetLastSyncDateTimeFromNAV()
        {
            DateTime dt = new DateTime();        
            CompanyInformation_Filter filter1 = new CompanyInformation_Filter();
            filter1.Field = CompanyInformation_Fields.Name;
            filter1.Criteria = comp1;
            CompanyInformation_Filter[] filters = new CompanyInformation_Filter[] { filter1 };
            CompanyInformation companyInfo = companyInfoService.ReadMultiple(filters, null, 0)[0];
            if (companyInfo != null)
            {
                dt = companyInfo.Last_Sync_Datetime;                           
            }
            return dt;
        }
        public DateTime GetChinaDateTime(DateTime dt)
        {
            DateTime dt1 = new DateTime();
            try
            {
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
                dt1 = TimeZoneInfo.ConvertTimeFromUtc(dt, cstZone);
                //Console.WriteLine("\nThe date and time are {0} {1}.",
                //                  dt,
                //                  cstZone.IsDaylightSavingTime(dt) ?
                //                          cstZone.DaylightName : cstZone.StandardName);
            }
            catch (TimeZoneNotFoundException ex)
            {
                //Console.WriteLine("\nThe registry does not define the Central Standard Time zone.");
                Logging.WriteBuglog(ex);
            }
            catch (InvalidTimeZoneException ex)
            {
                //Console.WriteLine("\nRegistry data on the Central Standard Time zone has been corrupted.");
                Logging.WriteBuglog(ex);

            }
            return dt1;
        }
        public void SetLastSyncDateTimeInNAV(DateTime lastSyncDate)
        {        
            CompanyInformation_Filter filter1 = new CompanyInformation_Filter();
            filter1.Field = CompanyInformation_Fields.Name;
            filter1.Criteria = comp1;
            CompanyInformation_Filter[] filters = new CompanyInformation_Filter[] { filter1 };
            CompanyInformation companyInfo = companyInfoService.ReadMultiple(filters, null, 0)[0];
            if (companyInfo != null)
            {
                companyInfo.Last_Sync_Datetime = lastSyncDate;
                companyInfoService.Update(ref companyInfo);
                Console.WriteLine("\nSet last Sync. date successfully on NAV, value is : {0}", lastSyncDate);
                
            }

        }
        public void InsertToSFContactFromSFService()
        {
            QueryResult queryResult = sf.QuerySFFromContact("");
            for (int i = 0; i < queryResult.records.Count(); i++)
            {
                Contact contact = (Contact)queryResult.records[i];
                Console.WriteLine("Name: {0}  Email: {1}", contact.Name,contact.Email);
               
            }
            Console.ReadLine();

        }
        public void InitNAVContact()
        {          
            newcontact_Filter filter1 = new newcontact_Filter();
            filter1.Field = newcontact_Fields.Name;
            filter1.Criteria = "Alan*";
            newcontact_Filter[] filters = new newcontact_Filter[] { filter1 };
            newcontact[] contacts = contactService.ReadMultiple(filters, null, 0);
            
            foreach (newcontact contact1 in contacts)
            {
                Console.WriteLine(contact1.No);
            }
            Console.ReadLine(); 
        }
        public newcontact[] GetContactFromNAV(newcontact_Filter[] filters)
        {                  
            return contactService.ReadMultiple(filters, null, 0);
        }

        
        
        private User ValidateUserOnSF(newcontact newcontact)
        {
            User user = new User();
            QueryResult queryResult = sf.QuerySFFromUser("Alias='Alan Hu'");// + newcontact.Creation_User + "'");
            if (queryResult.records != null)
            {
                 user = (User)queryResult.records[0];
            }
            else
            {
                user.FirstName = "Alan";
                user.LastName = "Hu";
                Console.WriteLine(sf.InsertUserToSF(user));
            }
            return user;

        }
       
    }
}
