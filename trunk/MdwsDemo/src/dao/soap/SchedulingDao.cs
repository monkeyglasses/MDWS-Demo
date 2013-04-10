using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MdwsDemo.scheduling;

namespace MdwsDemo.dao.soap
{
    public class SchedulingDao : ISchedulingDao
    {
        SchedulingSvc _svc;

        public SchedulingDao()
        {
            _svc = new SchedulingSvc();
            _svc.CookieContainer = new System.Net.CookieContainer();
        }

        /// <summary>
        /// This constructor can be used when the need to share sessions among facades is enountered. For example, calling addDataSource
        /// on EmrSvc and then switching to use calls on this facade.
        /// </summary>
        /// <param name="emrSvcCookieContainer">The CookieContainer from the EmrSvc facade (could be from other facades also)</param>
        public SchedulingDao(System.Net.CookieContainer emrSvcCookieContainer)
        {
            _svc = new SchedulingSvc();
            _svc.CookieContainer = emrSvcCookieContainer;
        }

        /// <summary>
        /// Disconnect all Vista connections - never throw an error
        /// </summary>
        public void disconnect()
        {
            try
            {
                _svc.disconnect();
            }
            catch (Exception) { }
        }

        public RegionArray getSites()
        {
            RegionArray result = _svc.getVHA();
            if (result.fault != null)
            {
                throw new ApplicationException(result.fault.message);
            }
            return result;
        }

        /// <summary>
        /// Connect to a Vista and authenticate the user
        /// </summary>
        /// <param name="sitecode">User's site ID (getVHA returns a list of site IDs)</param>
        /// <param name="username">User's access code</param>
        /// <param name="password">User's verify code</param>
        /// <returns>UserTO</returns>
        public UserTO connectAndLogin(string sitecode, string username, string password)
        {
            DataSourceArray connectResult = _svc.connect(sitecode);
            if (connectResult.fault != null)
            {
                throw new ApplicationException(connectResult.fault.message);
            }
            UserTO user = _svc.login(username, password, "");
            if (user.fault != null)
            {
                throw new ApplicationException(user.fault.message);
            }
            return user;
        }

        /// <summary>
        /// Get clinics starting search at target name
        /// </summary>
        /// <param name="target">The alphabetical starting point for the clinic list</param>
        /// <returns>IList of HospitalLocationTO</returns>
        public IList<HospitalLocationTO> getClinics(string target)
        {
            TaggedHospitalLocationArray clinics = _svc.getClinics(target);
            if (clinics.fault != null)
            {
                throw new ApplicationException(clinics.fault.message);
            }
            // generic collections are easier to work with - just copying over to list
            IList<HospitalLocationTO> result = new List<HospitalLocationTO>();
            foreach (HospitalLocationTO loc in clinics.locations)
            {
                result.Add(loc);
            }
            return result;
        }

        /// <summary>
        /// Returns the object that contains detailed info (scheduling start time, appointment length, availability info, etc) for a clinic's schedule
        /// </summary>
        /// <param name="clinicId">Clinic's ID</param>
        /// <returns>HospitalLocationTO</returns>
        public HospitalLocationTO getClinicSchedulingDetails(string clinicId)
        {
            HospitalLocationTO result = _svc.getClinicSchedulingDetails(clinicId);
            if (result.fault != null)
            {
                throw new ApplicationException(result.fault.message);
            }
            return result;
        }

        /// <summary>
        /// Returns a list of the valid appointment types that can be scheduled. The list begins at the alphabetical search point of target
        /// </summary>
        /// <param name="target">The alphabetical start point for the appointment type search</param>
        /// <returns>IList of AppointmentTypeTO</returns>
        public IList<AppointmentTypeTO> getAppointmentTypes(string target)
        {
            AppointmentTypeArray apptTypes = _svc.getAppointmentTypes(target);
            if (apptTypes.fault != null)
            {
                throw new ApplicationException(apptTypes.fault.message);
            }
            IList<AppointmentTypeTO> result = new List<AppointmentTypeTO>();
            foreach (AppointmentTypeTO apptType in apptTypes.appointmentTypes)
            {
                result.Add(apptType);
            }
            return result;
        }

        /// <summary>
        /// Retrieve patients with a scheduled appointment in a clinic. Notice: data structure may be modified to accomodate appointment information
        /// </summary>
        /// <param name="clinicId">Clinic's ID</param>
        /// <param name="startDate">Beginning date</param>
        /// <param name="stopDate">Stop date</param>
        /// <returns></returns>
        public IList<PatientTO> getPatientsByClinic(string clinicId, string startDate, string stopDate)
        {
            PatientArray result = _svc.getPatientsByClinic(clinicId, startDate, stopDate);
            if (result.fault != null)
            {
                throw new ApplicationException(result.fault.message);
            }
            IList<PatientTO> patients = new List<PatientTO>();
            foreach (PatientTO patient in result.patients)
            {
                patients.Add(patient);
            }
            return patients;
        }

        public AppointmentTO makeAppointment(string pid, string clinicId, string apptTimestamp, string category, string subCategory, string apptLength, string apptType)
        {
            selectPatient(pid);
            AppointmentTO scheduledAppt = _svc.makeAppointment(clinicId, apptTimestamp, category, subCategory, apptLength, apptType);
            if (scheduledAppt.fault != null)
            {
                throw new ApplicationException("Unable to make appointment: " + scheduledAppt.fault.message);
            }
            return scheduledAppt;
        }

        // This call was implemented as part of a demo - the service is not currently available on the internet
        public esb.appointmentResponse makeAppointmentEsb(string appointmentDate, string appointmentLength, string clinicIen, string patientIen, string patientSsn)
        {
            esb.AppointmentWorkflowService svc = new esb.AppointmentWorkflowService();
            esb.appointmentResponse response = svc.makeAppointmentWorkflow(new esb.appointment()
            {
                appointmentDate = appointmentDate,
                appointmentLength = appointmentLength,
                clinicIen = clinicIen,
                patientIen = patientIen,
                patientSsn = patientSsn
            });
            return response;
        }

        internal PatientTO selectPatient(string pid)
        {
            return _svc.select(pid);
        }
    }
}