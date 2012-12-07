using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MdwsDemo.scheduling;

namespace MdwsDemo.dao.soap
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
            VistaDao dao = new VistaDao();
            dao.addDataSource("100", "Dewdrop", "172.19.100.199", "9100", "1");
            _dao = new SchedulingDao(dao.EmrSvcSession); // copy session from VistaDao
        }

        [TestFixtureTearDown]
        public void testFixtureTearDown()
        {
            _dao.disconnect();
        }

        void login100()
        {
            _dao.connectAndLogin("100", _accessCode, _verifyCode);
        }

        /// <summary>
        /// This test illustrates the simple concepts of authenticating your user to a site
        /// </summary>
        [Test]
        public void testConnectAndLogin()
        {
            RegionArray siteTable = _dao.getSites();
            SiteTO selectedSite = null; // placeholder
            // we already know our site code but, just for completeness, showing walking through the region array here
            // the VA organizes itself logically and geographically by regions and sites. "regions" is  roughly what 
            // we term internally as VISNs. There are 23 VISNs in VA - Ann Arbor is in VISN 11. Since MDWS uses this organization
            // for other things, we modified the terminology slightly
            // This first foreach loop goes through all the regions in the site table - in our test environment we only have one region
            foreach (RegionTO region in siteTable.regions)
            {
                if (region.sites != null && region.sites.sites != null && region.sites.sites.Length > 0)
                {
                    foreach (SiteTO site in region.sites.sites)
                    {
                        if (site.sitecode == "100") // we chose a really difficult way of finding our site!
                        {
                            selectedSite = site;
                            break;
                        }
                    }
                }
                if (selectedSite != null)
                {
                    break;
                }
            }
            UserTO user = _dao.connectAndLogin(selectedSite.sitecode, _accessCode, _verifyCode);

            Assert.IsNull(user.fault);
        }

        [Test]
        public void testGetClinics()
        {
            login100();

            IList<HospitalLocationTO> clinics = _dao.getClinics("A"); // get clinics starting at 'A'

            Assert.IsNotNull(clinics);
            Assert.IsTrue(clinics.Count > 0);
            foreach (HospitalLocationTO clinic in clinics)
            {
                Assert.IsFalse(String.IsNullOrEmpty(clinic.id), "All clinics need an ID");
                Assert.IsFalse(String.IsNullOrEmpty(clinic.name), "All clinics need a name");
            }
        }

        [Test]
        public void testGetClinicSchedulingDetails()
        {
            login100();

            IList<HospitalLocationTO> clinics = _dao.getClinics("A"); // get clinics starting at 'A'

            // get details for each of the clinics above
            foreach (HospitalLocationTO clinic in clinics)
            {
                HospitalLocationTO clinicWithDetails = _dao.getClinicSchedulingDetails(clinic.id);

                Assert.IsNull(clinicWithDetails.fault);
                Assert.IsFalse(String.IsNullOrEmpty(clinicWithDetails.appointmentLength), "Should receive appt length");
                Assert.IsFalse(String.IsNullOrEmpty(clinicWithDetails.availability), "Should receive availability info");
                // one of the clinics in our test Vista doesn't have a display start time specified - artifact of test database...
                //Assert.IsFalse(String.IsNullOrEmpty(clinicWithDetails.clinicDisplayStartTime), "Should receive scheduling start time");
                Assert.IsFalse(String.IsNullOrEmpty(clinicWithDetails.displayIncrements), "Should receive display increments");
                Assert.IsFalse(String.IsNullOrEmpty(clinicWithDetails.name), "Should receive display name");
                Assert.IsFalse(String.IsNullOrEmpty(clinicWithDetails.type), "Should receive location type (e.g. 'C' for clinic)");
            }
        }

        [Test]
        public void testGetAppointmentTypes()
        {
            login100();

            IList<AppointmentTypeTO> apptTypes = _dao.getAppointmentTypes("A");

            foreach (AppointmentTypeTO apptType in apptTypes)
            {
                Assert.IsTrue(apptType.active, "All types should be active if returned through this call");
                Assert.IsFalse(String.IsNullOrEmpty(apptType.id), "Should always received ID");
                Assert.IsFalse(String.IsNullOrEmpty(apptType.name), "Should always receive name");
            }
        }

        [Test]
        public void testMakeAppointment()
        {
            string pid = "91";

            login100();
            // user is now authenticated
            IList<AppointmentTypeTO> apptTypes = _dao.getAppointmentTypes("A");
            // we now have the appointment types we can display to the user
            IList<HospitalLocationTO> clinics = _dao.getClinics("A"); // get clinics starting at 'A'
            // we now have a list of clinics we can allow the user to select one
            HospitalLocationTO clinicWithDetails = _dao.getClinicSchedulingDetails(clinics[0].id);
            // we now have the details for the selected clinic and can show those to the user so a timeslot can be selected and we can pass the correct params to makeAppointment

            // now ready to make appt - we choose the first appt type and clinic in the returns from above - we also choose to make an appt as soon as the 
            // clinic opens - the appt length is the length specified for the clinic
            AppointmentTO scheduledAppt = _dao.makeAppointment(pid, apptTypes[0].id, clinics[0].id, clinicWithDetails.clinicDisplayStartTime, clinicWithDetails.appointmentLength);

            Assert.IsNull(scheduledAppt.fault);
        }
    }
}
