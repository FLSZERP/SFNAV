using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using SFAPP.SFDC;
using System.Configuration;
using System.Data;

namespace SFAPP
{
    public class SF
    {
        string userName = ConfigurationManager.AppSettings["UserID"].ToString();
        string password = ConfigurationManager.AppSettings["Password"].ToString();
        SforceService SfdcBinding = null;
        LoginResult CurrentLoginResult = null;
        DescribeSObjectResult describeSObject = null;
        DescribeSObjectResult SFObj = null;
        public SF()
        {
            SfdcBinding = new SforceService();
            try
            {
                CurrentLoginResult = SfdcBinding.login(userName, password);
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                //This is likley to be caused by bad username or password
                SfdcBinding = null;
                Logging.WriteBuglog(e);
                throw e;
            }
            catch (Exception e)
            {
                //This is something else, probably comminication
                SfdcBinding = null;
                Logging.WriteBuglog(e);
                throw e;
            }
            //Change the binding to the new endpoint
            SfdcBinding.Url = CurrentLoginResult.serverUrl;

            //Create a new session header object and set the session id to that returned by the login
            SfdcBinding.SessionHeaderValue = new SessionHeader();
            SfdcBinding.SessionHeaderValue.sessionId = CurrentLoginResult.sessionId;
        }

        public QueryResult QuerySFFromContact(string where)
        {           
            QueryResult queryResult = null;
            String SOQL = "SELECT ";
            DescribeSObjectResult[] describeSObjectResults =
                        SfdcBinding.describeSObjects(
                        new string[] { "Contact" });
            SFObj = SfdcBinding.describeSObject("Contact");
            Field[] fields = SFObj.fields;
            foreach (Field field in fields)
            {
                SOQL += field.name + ",";
            }
            SOQL = SOQL.Substring(0, SOQL.Length - 1);

            SOQL += " from Contact";
            if (where!="")
            {
                SOQL += " where " + where;
            }
            try
            {
                queryResult = SfdcBinding.query(SOQL);
            }
            catch(Exception e)
            {
                Logging.WriteBuglog(e);
                throw e;
            }
            return queryResult;
        }
        internal QueryResult QuerySFFromAccount(string where)
        {
            QueryResult queryResult = null;
            String SOQL = "SELECT ";
            DescribeSObjectResult[] describeSObjectResults =
                        SfdcBinding.describeSObjects(
                        new string[] { "Account" });
            SFObj = SfdcBinding.describeSObject("Account");
            Field[] fields = SFObj.fields;
            foreach (Field field in fields)
            {
                SOQL += field.name + ",";
            }
            SOQL = SOQL.Substring(0, SOQL.Length - 1);

            SOQL += " from Account";
            if (where != "")
            {
                SOQL += " where " + where;
            }
            try
            {
                queryResult = SfdcBinding.query(SOQL);
            }
            catch (Exception e)
            {
                Logging.WriteBuglog(e);
                throw e;
            }
            return queryResult;
        }
        public QueryResult QuerySFFromUser(string where)
        {
            QueryResult queryResult = null;
            String SOQL = "SELECT Alias,FirstName,LastName";
            SOQL += " from User";
            if (where != "")
            {
                SOQL += " where " + where;
            }
            try
            {
                queryResult = SfdcBinding.query(SOQL);
            }
            catch(Exception e)
            {
                Logging.WriteBuglog(e);
                throw e;
            }
            return queryResult;
        }
        public DataTable GetContact()
        {
            QueryResult queryResult = null;
            String SOQL = "SELECT ";
            SFObj = SfdcBinding.describeSObject("contact");
            Field[] fields = SFObj.fields;
            foreach (Field field in fields)
            {
                SOQL += field.name + ",";
            }
            SOQL = SOQL.Substring(0, SOQL.Length - 1);
            SOQL += " from contact";

            queryResult = SfdcBinding.query(SOQL);
            DataTable dt = new DataTable("Contact");
            foreach (Field field in fields)
            {
                SOQL += field.name + ",";
                dt.Columns.Add(new DataColumn(field.name));
            }

            for (int i = 0; i < queryResult.records.Count(); i++)
            {
                Contact contact = (Contact)queryResult.records[i];

                DataRow dr = dt.NewRow();
                foreach (Field field in fields)
                {

                }

                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// InitContactToSF (from NAV)
        /// </summary>
        /// <param name="contacts">Salesforce.Contact</param>
        /// <returns></returns>
        public bool InsertContactToSF(Contact contacts,out string result)
        {
            bool Inserted = false;
            SaveResult[] saveResults = null;
            try
            {
                saveResults = SfdcBinding.create(new sObject[] { contacts });
            }
            catch(Exception e)
            {
                Logging.WriteBuglog(e);
                throw e;
            }
            if (saveResults[0].success)
            {
                //string Id = "";
                result = saveResults[0].id;
                Inserted = true;
            }
            else
            {
                //string result = "";
                result = saveResults[0].errors[0].message;
                Inserted = false;
            }
                       
            return Inserted;
        }
        public string InsertUserToSF(User users)
        {
            SaveResult[] saveResults = SfdcBinding.create(new sObject[] { users });
            string result = "";
            if (saveResults[0].success)
            {
                //string Id = "";
                result = saveResults[0].id;
            }
            else
            {
                //string result = "";
                result = saveResults[0].errors[0].message;
            }
            return result;
        }



        internal bool InsertAccountToSF(Account SFAccount, out string result)
        {
            bool Inserted = false;
            SaveResult[] saveResults = null;
            try
            {
                saveResults = SfdcBinding.create(new sObject[] { SFAccount });
            }
            catch (Exception e)
            {
                Logging.WriteBuglog(e);
                throw e;
            }
            if (saveResults[0].success)
            {
                //string Id = "";
                result = saveResults[0].id;
                Inserted = true;
            }
            else
            {
                //string result = "";
                result = saveResults[0].errors[0].message;
                Inserted = false;
            }

            return Inserted;
        }

        
    }
}
