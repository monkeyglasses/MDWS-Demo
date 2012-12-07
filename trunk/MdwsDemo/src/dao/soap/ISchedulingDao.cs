using System;
namespace MdwsDemo.src.dao.soap
{
    interface ISchedulingDao
    {
        global::MdwsDemo.scheduling.UserTO connectAndLogin(string sitecode, string username, string password);
        void disconnect();
        global::System.Collections.Generic.IList<global::MdwsDemo.scheduling.AppointmentTypeTO> getAppointmentTypes(string target);
        global::System.Collections.Generic.IList<global::MdwsDemo.scheduling.HospitalLocationTO> getClinics(string target);
        global::MdwsDemo.scheduling.HospitalLocationTO getClinicSchedulingDetails(string clinicId);
        global::System.Collections.Generic.IList<global::MdwsDemo.scheduling.PatientTO> getPatientsByClinic(string clinicId, string startDate, string stopDate);
        global::MdwsDemo.scheduling.RegionArray getSites();
        global::MdwsDemo.scheduling.AppointmentTO makeAppointment(string pid, string apptType, string clinicId, string apptTimestamp, string apptLength);
    }
}
