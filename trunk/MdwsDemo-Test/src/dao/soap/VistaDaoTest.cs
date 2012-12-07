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

        #region Patient Identity

        #region SSN MATCH

        [Test]
        [ExpectedException(typeof(ApplicationException), ExpectedMessage = "No patients returned for that search...")] // exception is thrown by DAO
        public void testMatchSsnKnownNonExistent()
        {
            _vistaDao.connect("901");
            UserTO user = _vistaDao.login("04VEHU", "VEHU04");

            IList<PatientTO> result = _vistaDao.match("123456789");

            Assert.Fail("Previous call should have thrown exception");
        }

        [Test]
        public void testMatchSsnKnownUnique()
        {
            _vistaDao.connect("901");
            UserTO user = _vistaDao.login("04VEHU", "VEHU04");

            IList<PatientTO> result = _vistaDao.match("666330008");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
            Assert.IsFalse(String.IsNullOrEmpty(result[0].ssn), "The match by SSN call should return the patient's SSN!");
            Assert.IsFalse(String.IsNullOrEmpty(result[0].name), "The match by SSN call should return the patient's name!");
            Assert.IsFalse(String.IsNullOrEmpty(result[0].dob), "The match by SSN call should return the patient's date of birth!");
            Assert.IsFalse(String.IsNullOrEmpty(result[0].localPid), "The match by SSN call should return the patient's local ID!");
            Assert.IsTrue(String.Equals(result[0].ssn, "*SENSITIVE*"), "Note - sometimes the match call will return the sensitivity string based on various business rules");
        }

        [Test]
        public void testMatchSsnKnownDuplicate()
        {
            _vistaDao.connect("901");
            UserTO user = _vistaDao.login("04VEHU", "VEHU04");

            Assert.Fail("Need to stage Vista with duplicate SSN");
            IList<PatientTO> result = _vistaDao.match("dupe SSN");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 1);
        }

        #endregion

        #region NAME MATCH

        [Test]
        public void testMatchNameExact()
        {
            string expectedName = "BCMA,EIGHT";
            string expectedPid = "100022";
            string expectedSsn = "666330008";

            _vistaDao.connect("901");
            UserTO user = _vistaDao.login("04VEHU", "VEHU04");

            IList<PatientTO> result = _vistaDao.match(expectedName);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 1, "Matches by name usually return more than one result - need to filter/search");

            PatientTO locatedRecord = null;

            foreach (PatientTO patient in result)
            {
                if (String.Equals(patient.localPid, expectedPid))
                {
                    locatedRecord = patient;
                    break;
                }
            }

            Assert.IsNotNull(locatedRecord, "Expected to find a patient with known identifier for SSN");
            Assert.IsTrue(String.IsNullOrEmpty(locatedRecord.ssn), "The match by name call does not return SSN");
            Assert.IsTrue(String.IsNullOrEmpty(locatedRecord.dob), "The match by name call does not return date of birth");
            Assert.IsFalse(String.IsNullOrEmpty(locatedRecord.name), "The match by name call should return name!");
            Assert.IsFalse(String.IsNullOrEmpty(locatedRecord.localPid), "The match by name call should return the patient ID!");

            // now - need to select patient and ensure localPid, SSN and name are all as expected
            PatientTO selectedPatient = _vistaDao.select(expectedPid);

            Assert.IsTrue(String.Equals(selectedPatient.name, expectedName));
            Assert.IsTrue(String.Equals(selectedPatient.ssn, expectedSsn));
            Assert.IsTrue(String.Equals(selectedPatient.localPid, expectedPid));
        }

        [Test]
        public void testMatchLastName()
        {
            string lastName = "BCMA";
            string expectedName = "BCMA,TWENTY-PATIENT";
            string expectedPid = "100035";
            string expectedSsn = "666330020";
            string expectedDob = "19350407";

            _vistaDao.connect("901");
            UserTO user = _vistaDao.login("04VEHU", "VEHU04");

            IList<PatientTO> result = _vistaDao.match(lastName); // passing just the last name

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 1, "Matches by name usually return more than one result - need to filter/search");

            PatientTO locatedRecord = null;

            foreach (PatientTO patient in result)
            {
                if (String.Equals(patient.localPid, expectedPid))
                {
                    locatedRecord = patient;
                    break;
                }
            }

            // the match RPC will return, at most, 44 results - we can't always be certain those results contain
            // the patient for whom we are searching - if the patient is not in the collection then we can loop
            // the match call using the last patient's name as the start point for the search (Vista traverses the 
            // file by the name) and quit when we no longer are looking at the correct last names (if the patient wasn't found)
            while (locatedRecord == null)
            {
                if (!(result[result.Count - 1].name.Contains(lastName))) // check the last result's name in the collection
                {
                    Assert.Fail("Looked through all the last names {0} but didn't find the expected patient!");
                }
                result = _vistaDao.match(result[result.Count - 1].name); // make sure to start the next search at the last name from previous' search

                foreach (PatientTO patient in result)
                {
                    if (String.Equals(patient.localPid, expectedPid))
                    {
                        locatedRecord = patient;
                        break;
                    }
                }
            }

            Assert.IsNotNull(locatedRecord, "Expected to find a patient with known identifier for SSN");
            Assert.IsTrue(String.IsNullOrEmpty(locatedRecord.ssn), "The match by name call does not return SSN");
            Assert.IsTrue(String.IsNullOrEmpty(locatedRecord.dob), "The match by name call does not return date of birth");
            Assert.IsFalse(String.IsNullOrEmpty(locatedRecord.name), "The match by name call should return name!");
            Assert.IsFalse(String.IsNullOrEmpty(locatedRecord.localPid), "The match by name call should return the patient ID!");

            // now - need to select patient and ensure localPid, SSN and name are all as expected
            PatientTO selectedPatient = _vistaDao.select(expectedPid);

            Assert.IsTrue(String.Equals(selectedPatient.name, expectedName));
            Assert.IsTrue(String.Equals(selectedPatient.ssn, expectedSsn));
            Assert.IsTrue(String.Equals(selectedPatient.localPid, expectedPid));
            Assert.IsTrue(String.Equals(selectedPatient.dob, expectedDob));
        }

        #endregion

        #endregion
    }
}
