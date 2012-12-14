using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using MdwsDemo.scheduling;

namespace MdwsDemo.dao.rest
{
    /// <summary>
    /// REST SchedulingDao
    /// 
    /// This DAO leverages the the SOAP objects we created in our SchedulingSvc proxy. The open source Newtonsoft JSON library is used for serialization
    /// </summary>
    public class SchedulingDao : ISchedulingDao
    {
        Uri _schedulingEndpoint;
        string _cookie;

        public string Cookie { get { return _cookie; } }

        public SchedulingDao(Uri schedulingServiceUri)
        {
            _schedulingEndpoint = schedulingServiceUri;
        }

        public UserTO connectAndLogin(string sitecode, string username, string password)
        {
            string connectResponse = makeRequest("/connect/" + sitecode);
            string loginResponse = makeRequest(String.Format("/login?username={0}&password={1}&permission={2}&token={3}", username, password, "", _cookie));
            return JsonConvert.DeserializeObject<UserTO>(loginResponse);
        }

        public void disconnect()
        {
            if (String.IsNullOrEmpty(_cookie))
            {
                throw new ArgumentException("No connections!");
            }
            makeRequest("/disconnect?token=" + _cookie);
            _cookie = null;
        }

        public RegionArray getSites()
        {
            string getVhaResponse = makeRequest("/getVHA");
            return JsonConvert.DeserializeObject<RegionArray>(getVhaResponse);
        }

        public IList<HospitalLocationTO> getClinics(string target)
        {
            string getClinicsResponse = makeRequest(String.Format("/clinics/{0}?token={1}", target, _cookie));
            TaggedHospitalLocationArray result = JsonConvert.DeserializeObject<TaggedHospitalLocationArray>(getClinicsResponse);
            IList<HospitalLocationTO> list = new List<HospitalLocationTO>();
            foreach (HospitalLocationTO loc in result.locations)
            {
                list.Add(loc);
            }
            return list;
        }

        public IList<AppointmentTypeTO> getAppointmentTypes(string target)
        {
            string appointmentTypesResponse = makeRequest(String.Format("/appointmentTypes/{0}?token={1}", target, _cookie));
            AppointmentTypeArray result = JsonConvert.DeserializeObject<AppointmentTypeArray>(appointmentTypesResponse);
            IList<AppointmentTypeTO> list = new List<AppointmentTypeTO>();
            foreach (AppointmentTypeTO type in result.appointmentTypes)
            {
                list.Add(type);
            }
            return list;
        }

        public HospitalLocationTO getClinicSchedulingDetails(string clinicId)
        {
            string getSchedulingDetailsResponse = makeRequest(String.Format("/clinicSchedulingDetails/{0}?token={1}", clinicId, _cookie));
            return JsonConvert.DeserializeObject<HospitalLocationTO>(getSchedulingDetailsResponse);
        }

        public IList<PatientTO> getPatientsByClinic(string clinicId, string startDate, string stopDate)
        {
            throw new NotImplementedException();
        }

        public AppointmentTO makeAppointment(string pid, string clinicId, string apptTimestamp, string category, string subCategory, string apptLength, string apptType)
        {
            selectPatient(pid); 
            // notice the POST!
            string makeAppointmentResponse = makeRequest("POST", String.Format("/appointment?clinicId={0}&appointmentType={1}&appointmentTimestamp={2}&appointmentLength={3}&token={4}", clinicId, apptType, apptTimestamp, apptLength, _cookie));
            return JsonConvert.DeserializeObject<AppointmentTO>(makeAppointmentResponse);
        }

        public PatientTO selectPatient(string pid)
        {
            string selectResponse = makeRequest(String.Format("/select/{0}?token={1}", pid, _cookie));
            return JsonConvert.DeserializeObject<PatientTO>(selectResponse);
        }

        #region HTTP Request

        internal string makeRequest(string request)
        {
            return makeRequest("GET", request);
        }

        internal string makeRequest(string method, string request)
        {
            WebRequest client = WebRequest.Create(_schedulingEndpoint.ToString() + request);
            client.ContentType = "";
            client.Method = method;
            //Stream data = client.getr.GetRequestStream();
            WebResponse response = client.GetResponse();

            HttpStatusCode status = ((HttpWebResponse)response).StatusCode;

            if (null != response.Headers.Get("token")) // only the first call that shoud start a session will return a cookie - e.g. "connect" returns a cookie
            {
                _cookie = response.Headers.Get("token"); // this is the session ID header for REST services
            }
            string description = ((HttpWebResponse)response).StatusDescription;
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string responseString = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return responseString;
        }

        #endregion



    }
}