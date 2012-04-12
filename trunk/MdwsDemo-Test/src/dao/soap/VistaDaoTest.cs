using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MdwsDemo.mdws;
using MdwsDemo.dao.soap;

namespace MdwsDemo_Test.dao.soap
{
    [TestFixture]
    public class VistaDaoTest
    {
        VistaDao _vistaDao;

        [SetUp]
        public void SetUp()
        {
            _vistaDao = new VistaDao();
        }

        [TearDown]
        public void TearDown()
        {
            if (_vistaDao != null)
            {
                _vistaDao.disconnect();
            }
        }

        [Test]
        public void testConnect()
        {
            _vistaDao.connect("901"); // CPM test site
        }

        [Test]
        public void testLogin()
        {
            _vistaDao.connect("901");
            UserTO user = _vistaDao.login("04VEHU", "VEHU04");

            Assert.IsNotNull(user);
            Assert.IsNull(user.fault);
            Assert.IsTrue(String.Equals(user.name, "VEHU,FOUR"));
            Assert.IsTrue(String.Equals(user.SSN, "666548835"));
            Assert.IsTrue(String.Equals(user.DUZ, "20005"));
        }

        [Test]
        public void testSelect()
        {
            _vistaDao.connect("901");
            UserTO user = _vistaDao.login("04VEHU", "VEHU04");

            PatientTO result = _vistaDao.select("100022");
            Assert.IsTrue(String.Equals(result.name, "BCMA,EIGHT"));
            Assert.IsTrue(String.Equals(result.ssn, "666330008"));
            Assert.IsTrue(String.Equals(result.localPid, "100022"));
        }

        [Test]
        public void testSetupMultiSiteQuery()
        {
            _vistaDao.connect("901");
            UserTO user = _vistaDao.login("04VEHU", "VEHU04");
            PatientTO result = _vistaDao.select("3"); // need patient with ICN

            // this doesn't really do anything interesting since the patient only has records at 
            // this one test system. in production, MDWS would connect to the patient's other
            // sites, select the same patient there
            _vistaDao.setupMultiSiteQuery("Every good boy deserves fudge"); // make believe BSE security phrase
        }

        [Test]
        public void testGetAllMeds()
        {
            _vistaDao.connect("901");
            UserTO user = _vistaDao.login("04VEHU", "VEHU04");
            PatientTO result = _vistaDao.select("3");
            _vistaDao.setupMultiSiteQuery("Every good boy deserves fudge");

            IList<MedicationTO> results =  _vistaDao.getAllMeds();
            Assert.IsTrue(results.Count > 0, "Expected to see some meds"); 
            Assert.IsTrue(String.Equals(results[0].name, "ASPIRIN TAB,EC"));
        }

        [Test]
        public void testVisit()
        {
            _vistaDao.visit("Every good boy deserves fudge", "901", "901", "VEHU,FOUR", "2005", "666548835", "OR CPRS GUI CHART");
        }
    }
}
