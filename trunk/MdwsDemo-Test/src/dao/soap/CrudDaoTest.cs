using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MdwsDemo.dao.soap
{
    [TestFixture]
    public class CrudDaoTest
    {
        CrudDao _dao;

        [TestFixtureSetUp]
        public void testFixtureSetUp()
        {
            _dao = new CrudDao();
            _dao.connect("101");
            _dao.login("1programmer", "programmer1");
        }

        [TestFixtureTearDown]
        public void testFixtureTearDown()
        {
            _dao.disconnect();
        }

        [Test]
        public void testGetPatientRecord()
        {
            Dictionary<String, String> result = _dao.read("100108,", "*", "2"); 
            Assert.IsTrue(result.ContainsKey(".01")); // all files should have the .01 field populated
            Assert.IsFalse(result.ContainsKey(".1412")); // we know for this patient in our test database that this field does not contain data so the API doesn't fetch the field - i.e. the API only returns keys for fields that have non-null values (.1412 is CONFIDENTIAL STREET)
        }

        [Test]
        public void testGetRecord()
        {
            Dictionary<String, String> result = _dao.read("13,", ".01;.02;.03", "8994.5"); // read three fields from remote application file - record IEN ends with comma, fields separated by semicolon, file is by number (8994.5 = REMOTE APPLICATION)
            Assert.IsTrue(result.ContainsKey(".01"));
            Assert.IsTrue(result.ContainsKey(".02"));
            Assert.IsTrue(result.ContainsKey(".03"));
        }

        [Test]
        public void testGetSubfileRecord()
        {
            // read all fields (denoted by asterisk) from remote application file's CALLBACKTYPE subfile
            // record IEN follows FM convention of N-level,N-1level,...,topLevel,
            // file is by number (8994.51 = CALLBACKTYPE subfile of REMOTE APPLICATION file)
            Dictionary<String, String> result = _dao.read("1,13,", "*", "8994.51"); 
            // we know this subfile contains these 4 fields
            Assert.IsTrue(result.ContainsKey(".01"));
            Assert.IsTrue(result.ContainsKey(".02"));
            Assert.IsTrue(result.ContainsKey(".03"));
            Assert.IsTrue(result.ContainsKey(".04"));
        }

        [Test]
        public void testCreateAndReadRecord()
        {
            Dictionary<String, String> fieldsAndValues = new Dictionary<string, string>();
            fieldsAndValues.Add(".01", "JOELS NEW REMOTE APPLICATION");
            fieldsAndValues.Add(".02", "11916"); // pointer to OPTION file though FM API does NOT enforce foreign key constraint! set with caution
            fieldsAndValues.Add(".03", "MyHashedPhrase123");

            String vistaFile = "8994.5";

            String ien = _dao.create(fieldsAndValues, vistaFile, ""); // we can leave IENS blank because this is a top level file record!

            Int32 trash = 0;
            Assert.IsTrue(Int32.TryParse(ien, out trash), "IENS in this file are numeric so expect this value to be");
            Assert.IsFalse(String.Equals(ien, "0"));

            // we've proved read works so let's get the record to verify
            Dictionary<String, String> result = _dao.read(ien + ",", "*", "8994.5"); // we add a comma to the IEN received from the create API to stay consistent with FM conventions
            // we know this file contains these 3 fields
            Assert.IsTrue(String.Equals(fieldsAndValues[".01"], result[".01"]));
            Assert.IsTrue(String.Equals(fieldsAndValues[".02"], result[".02"]));
            Assert.IsTrue(String.Equals(fieldsAndValues[".03"], result[".03"]));
        }

        [Test]
        public void testCreateAndReadSubfileRecord()
        {
            Dictionary<String, String> fieldsAndValues = new Dictionary<string, string>();
            fieldsAndValues.Add(".01", "JOELS NEWER REMOTE APPLICATION");
            fieldsAndValues.Add(".02", "11916"); // pointer to OPTION file though FM API does NOT enforce foreign key constraint! set with caution
            fieldsAndValues.Add(".03", "MyHashedPhrase456");

            String parentIen = _dao.create(fieldsAndValues, "8994.5", ""); // we can leave IENS blank because this is a top level file record!

            // great - we've created parent file, now let's create subfile
            fieldsAndValues = new Dictionary<string, string>();
            fieldsAndValues.Add(".01", "H"); // callback type - see file docs
            fieldsAndValues.Add(".02", "8080"); // callback port - see file docs
            fieldsAndValues.Add(".03", "127.0.0.1");
            fieldsAndValues.Add(".04", "MyWebPage.aspx");

            String subFileIen = _dao.create(fieldsAndValues, "8994.51", parentIen + ","); // we set IENS to parent IEN

            Int32 trash = 0;
            Assert.IsTrue(Int32.TryParse(subFileIen, out trash), "IENS in this file are numeric so expect this value to be");
            Assert.IsFalse(String.Equals(subFileIen, "0"));

            // we've proved read works so let's get the record to verify
            Dictionary<String, String> result = _dao.read(subFileIen + "," + parentIen + ",", "*", "8994.51"); // we concatenate the IENS this way to stay consistent with FM conventions
            // we know this subfile contains these 4 fields
            Assert.IsTrue(String.Equals(fieldsAndValues[".01"], result[".01"]));
            Assert.IsTrue(String.Equals(fieldsAndValues[".02"], result[".02"]));
            Assert.IsTrue(String.Equals(fieldsAndValues[".03"], result[".03"]));
            Assert.IsTrue(String.Equals(fieldsAndValues[".04"], result[".04"]));
        }

        [Test]
        public void testUpdate()
        {
            String knownIens = "16";
            // update works mostly the same way as create
            Dictionary<String, String> fieldsAndValues = new Dictionary<string, string>();
            fieldsAndValues.Add(".01", "CHANGING MY REMOTE APPLICATION NAME");
            fieldsAndValues.Add(".02", "11916"); // pointer to OPTION file though FM API does NOT enforce foreign key constraint! set with caution
            fieldsAndValues.Add(".03", "SuperSecret");

            _dao.update(fieldsAndValues, "8994.5", knownIens); // we must have IENS because we're updating

            // update should throw an exception if update was unsuccessful but let's get record to verify anyways
            Dictionary<String, String> result = _dao.read(knownIens, "*", "8994.5"); // we add a comma to the IEN received from the create API to stay consistent with FM conventions
            // we know this file contains these 3 fields
            Assert.IsTrue(String.Equals(fieldsAndValues[".01"], result[".01"]));
            Assert.IsTrue(String.Equals(fieldsAndValues[".02"], result[".02"]));
            Assert.IsTrue(String.Equals(fieldsAndValues[".03"], result[".03"]));
        }

        [Test]
        public void testUpdateSubfile()
        {
            String knownSubfileIens = "1,16,";
            // update works mostly the same way as create
            Dictionary<String, String> fieldsAndValues = new Dictionary<string, string>();
            fieldsAndValues.Add(".01", "H"); // callback type - see file docs
            fieldsAndValues.Add(".02", "1234"); // callback port - see file docs
            fieldsAndValues.Add(".03", "127.0.0.2");
            fieldsAndValues.Add(".04", "MyNewWebPage.aspx");

            _dao.update(fieldsAndValues, "8994.51", knownSubfileIens); // we must have IENS because we're updating

            // update should throw an exception if update was unsuccessful but let's get record to verify anyways
            Dictionary<String, String> result = _dao.read(knownSubfileIens, "*", "8994.51"); // we add a comma to the IEN received from the create API to stay consistent with FM conventions
            // we know this file contains these 4 fields
            Assert.IsTrue(String.Equals(fieldsAndValues[".01"], result[".01"]));
            Assert.IsTrue(String.Equals(fieldsAndValues[".02"], result[".02"]));
            Assert.IsTrue(String.Equals(fieldsAndValues[".03"], result[".03"]));
            Assert.IsTrue(String.Equals(fieldsAndValues[".04"], result[".04"]));
        }

        [Test]
        public void testDelete()
        {
            // let's test creating and then deleting a record
            Dictionary<String, String> fieldsAndValues = new Dictionary<string, string>();
            fieldsAndValues.Add(".01", "JOELS NEW REMOTE APPLICATION");
            fieldsAndValues.Add(".02", "11916"); // pointer to OPTION file though FM API does NOT enforce foreign key constraint! set with caution
            fieldsAndValues.Add(".03", "MyHashedPhrase123");

            String vistaFile = "8994.5";

            String ien = _dao.create(fieldsAndValues, vistaFile, ""); // we can leave IENS blank because this is a top level file record!

            // before we delete, let's prove record exists
            // we've proved read works so let's get the record to verify
            Dictionary<String, String> result = _dao.read(ien + ",", "*", vistaFile); // we add a comma to the IEN received from the create API to stay consistent with FM conventions
            // we know this file contains these 3 fields
            Assert.IsTrue(String.Equals(fieldsAndValues[".01"], result[".01"]));
            Assert.IsTrue(String.Equals(fieldsAndValues[".02"], result[".02"]));
            Assert.IsTrue(String.Equals(fieldsAndValues[".03"], result[".03"]));

            _dao.delete(vistaFile, ien + ",");

            // now let's try and get the same record again to show it's gone
            try
            {
                result = _dao.read(ien + ",", "*", vistaFile); // we add a comma to the IEN received from the create API to stay consistent with FM conventions
                Assert.Fail("If we reached this point our test failed");
            }
            catch (Exception)
            {
                // cool! our DAO should throw an exception because it received a fault the record doesn't exist
            }
        }
    }
}
