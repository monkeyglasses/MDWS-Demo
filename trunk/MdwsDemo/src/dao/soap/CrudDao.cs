using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MdwsDemo.crud;
using MdwsDemo.utils;

namespace MdwsDemo.dao.soap
{
    public class CrudDao
    {
        crud.QuerySvc _svc;

        public CrudDao()
        {
            _svc = new crud.QuerySvc();
            _svc.CookieContainer = new System.Net.CookieContainer();
        }

        public Dictionary<String, String> read(String iens, String fields, String vistaFile)
        {
            Dictionary<String, String> result = new Dictionary<string, string>();
            StringDictionaryTO sd = _svc.read(iens, fields, vistaFile);
            if (sd.fault != null)
            {
                throw new Exception(sd.fault.message);
            }

            for (int i = 0; i < sd.keys.Length; i++)
            {
                result.Add(sd.keys[i], sd.values[i]); // key/values are at same index
            }
            return result;
        }

        public String create(Dictionary<String, String> fieldsAndValues, String vistaFile, String iens)
        {
            // sample serialized Dictionary<String, String>: [{\"Key\":\"Key01\",\"Value\":\"Value01\"},{\"Key\":\"Key02\",\"Value\":\"Value02\"},{\"Key\":\"Key03\",\"Value\":\"Value03\"}]
            string dictString = JsonUtils.Serialize<Dictionary<String, String>>(fieldsAndValues); // to make the API simple, we serialize our dictionary to a string. JSON serialization is widely available in all major programming languages. It would also be trivial to implement your own serializer that passed the format MDWS wants
            TextTO result = _svc.create(dictString, vistaFile, iens);
            if (result.fault != null)
            {
                throw new Exception(result.fault.message);
            }
            return result.text; // IEN of new record
        }

        public void update(Dictionary<String, String> fieldsAndValues, String vistaFile, String iens)
        {
            // sample serialized Dictionary<String, String>: [{\"Key\":\"Key01\",\"Value\":\"Value01\"},{\"Key\":\"Key02\",\"Value\":\"Value02\"},{\"Key\":\"Key03\",\"Value\":\"Value03\"}]
            string dictString = JsonUtils.Serialize<Dictionary<String, String>>(fieldsAndValues); // to make the API simple, we serialize our dictionary to a string. JSON serialization is widely available in all major programming languages. It would also be trivial to implement your own serializer that passed the format MDWS wants
            TextTO result = _svc.update(dictString, vistaFile, iens);
            if (result.fault != null)
            {
                throw new Exception(result.fault.message);
            }
            // success!
            //result.text = "OK"
        }

        public void delete(String vistaFile, String iens)
        {
            TextTO result = _svc.delete(iens, vistaFile);
            if (result.fault != null)
            {
                throw new Exception(result.fault.message);
            }
            // success!
            //result.text = "OK"
        }

        #region Connect and Login

        public string connect(string sitecode)
        {
            DataSourceArray result = _svc.connect(sitecode);
            if (result.fault != null)
            {
                throw new ApplicationException(result.fault.message);
            }
            return result.items[0].welcomeMessage;
        }

        public UserTO login(string accessCode, string verifyCode)
        {
            UserTO result = _svc.login(accessCode, verifyCode, "OR CPRS GUI CHART");
            if (result.fault != null)
            {
                throw new ApplicationException(result.fault.message);
            }
            return result;
        }

        public void disconnect()
        {
            if (_svc != null)
            {
                try
                {
                    _svc.disconnect();
                }
                catch (Exception)
                {
                    // we don't ever want to care about this function
                }
            }
        }

        #endregion
    }
}