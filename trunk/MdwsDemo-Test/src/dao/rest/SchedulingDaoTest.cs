using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MdwsDemo.scheduling;

namespace MdwsDemo.dao.rest
{
    [TestFixture]
    public class SchedulingDaoTest
    {
        SchedulingDao _dao;
        string _accessCode = "5821FOB";
        string _verifyCode = "6446FDB~";

        [TestFixtureSetUp]
        public void testFixtureSetUp()
        {
            _dao = new SchedulingDao(new Uri("http://mdws.vistaehr.com/mdws2/SchedulingSvc.svc"));
        }

        [TestFixtureTearDown]
        public void testFixtureTearDown()
        {
            _dao.disconnect();
        }

        void connectAndLogin()
        {
            _dao.connectAndLogin("100", _accessCode, _verifyCode);
        }

        [Test]
        public void testGetVhaRequest()
        {
            string jsonResponse = _dao.makeRequest("/getVHA");
            Assert.IsFalse(String.IsNullOrEmpty(jsonResponse));
        }

        [Test]
        public void testConnectAndLogin()
        {
            UserTO result = _dao.connectAndLogin("901", "1programmer", "programmer1");
            Assert.IsNotNull(result);
            Assert.IsNull(result.fault);
        }

        [Test]
        public void testGetAppointmentTypes()
        {
            connectAndLogin(); // setup connection
            IList<AppointmentTypeTO> result = _dao.getAppointmentTypes("A");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        public void testGetClinics()
        {
            connectAndLogin(); // setup connection
            IList<HospitalLocationTO> result = _dao.getClinics("A");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        public void testGetClinicSchedulingDetails()
        {
            connectAndLogin(); // setup connection
            IList<HospitalLocationTO> clinics = _dao.getClinics("A");
            Assert.IsNotNull(clinics);
            Assert.IsTrue(clinics.Count > 0);

            // get details for each of the clinics above
            foreach (HospitalLocationTO clinic in clinics)
            {
                HospitalLocationTO clinicWithDetails = _dao.getClinicSchedulingDetails(clinic.id);

                Assert.IsNull(clinicWithDetails.fault);
                // The commented out assertions are due to the test database not consistently having these fields
                // one of the clinics in our test Vista doesn't have a display start time specified - artifact of test database...
                //Assert.IsFalse(String.IsNullOrEmpty(clinicWithDetails.appointmentLength), "Should receive appt length");
                //Assert.IsFalse(String.IsNullOrEmpty(clinicWithDetails.availability), "Should receive availability info");
                // one of the clinics in our test Vista doesn't have a display start time specified - artifact of test database...
                //Assert.IsFalse(String.IsNullOrEmpty(clinicWithDetails.clinicDisplayStartTime), "Should receive scheduling start time");
                //Assert.IsFalse(String.IsNullOrEmpty(clinicWithDetails.displayIncrements), "Should receive display increments");
                Assert.IsFalse(String.IsNullOrEmpty(clinicWithDetails.name), "Should receive display name");
                Assert.IsFalse(String.IsNullOrEmpty(clinicWithDetails.type), "Should receive location type (e.g. 'C' for clinic)");
            }
        }

        [Test]
        public void testMakeAppointment()
        {
            string pid = "10110";

            connectAndLogin();
            // user is now authenticated
            IList<AppointmentTypeTO> apptTypes = _dao.getAppointmentTypes("A");
            // we now have the appointment types we can display to the user
            IList<HospitalLocationTO> clinics = _dao.getClinics("A"); // get clinics starting at 'A'
            // we now have a list of clinics we can allow the user to select one
            HospitalLocationTO clinicWithDetails = _dao.getClinicSchedulingDetails(clinics[0].id);
            // we now have the details for the selected clinic and can show those to the user so a timeslot can be selected and we can pass the correct params to makeAppointment

            // now ready to make appt - we choose the first appt type and clinic in the returns from above - we also choose to make an appt as soon as the 
            // clinic opens - the appt length is the length specified for the clinic
            AppointmentTO scheduledAppt = _dao.makeAppointment(pid, apptTypes[0].id, clinics[0].id, "3121206.080000", clinicWithDetails.appointmentLength);

            Assert.IsNull(scheduledAppt.fault);
        }
    }
}
