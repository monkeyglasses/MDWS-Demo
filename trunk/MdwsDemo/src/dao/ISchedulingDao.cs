using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MdwsDemo.scheduling;

namespace MdwsDemo.dao
{
    public interface ISchedulingDao
    {
        UserTO connectAndLogin(string sitecode, string username, string password);
        void disconnect();
        IList<AppointmentTypeTO> getAppointmentTypes(string target);
        IList<HospitalLocationTO> getClinics(string target);
        HospitalLocationTO getClinicSchedulingDetails(string clinicId);
        IList<PatientTO> getPatientsByClinic(string clinicId, string startDate, string stopDate);
        RegionArray getSites();
        //AppointmentTO makeAppointment(string pid, string apptType, string clinicId, string apptTimestamp, string apptLength);
        AppointmentTO makeAppointment(string pid, string clinicId, string apptTimestamp, string category, string subCategory, string apptLength, string apptType);

    }
}