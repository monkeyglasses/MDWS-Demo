using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MdwsDemo.domain;

namespace MdwsDemo.dao.rest
{
    [TestFixture]
    public class CrudDaoTest
    {
        CrudDao _dao;

        [TestFixtureSetUp]
        public void testFixtureSetUp()
        {
            _dao = new CrudDao("http://localhost:8734/CrudSvc/");
        }

        #region Read

        [Test]
        public void testGet()
        {
            VistaRecordTO result = _dao.read("901", "8994.5", "35,");

            Assert.IsNull(result.fault);
            Assert.IsNotNull(result.fields);
            foreach (VistaFieldTO field in result.fields)
            {
                System.Console.WriteLine(field.number + " - " + field.value);
            }
        }

        [Test]
        public void testGetSubfile()
        {
            VistaRecordTO result = _dao.read("901", "8994.51", "1,13,");

            Assert.IsNull(result.fault);
            Assert.IsNotNull(result.fields);
            foreach (VistaFieldTO field in result.fields)
            {
                System.Console.WriteLine(field.number + " - " + field.value);
            }
        }

        #endregion

        #region Create

        [Test]
        public void testCreate()
        {
            Dictionary<String, String> fieldsAndValues = new Dictionary<string, string>();
            fieldsAndValues.Add(".01", "MVWS MEDICAL DOMAIN WEB SERVICES2");
            fieldsAndValues.Add(".02", "11916"); // pointer to option file - DVBA CAPRI GUI in my test system
            fieldsAndValues.Add(".03", "batter up");

            String recordIen = _dao.create("901", "8994.5", "", fieldsAndValues).text;

            Assert.IsFalse(String.IsNullOrEmpty(recordIen));
            Int32 trash = 0;
            Assert.IsTrue(Int32.TryParse(recordIen, out trash));
        }

        #endregion

        #region Update

        [Test]
        public void testUpdate()
        {
            Dictionary<String, String> fieldsAndValues = new Dictionary<string, string>();
            fieldsAndValues.Add(".01", "MVWS MEDICAL DOMAIN WEB SERVICES2");
            fieldsAndValues.Add(".02", "11916"); // pointer to option file - DVBA CAPRI GUI in my test system
            fieldsAndValues.Add(".03", "Take the red pill");

            String okResponse = _dao.update("901", "8994.5", "37,", fieldsAndValues).text;

            Assert.IsTrue(String.Equals("OK", okResponse));
        }

        #endregion

        #region Delete

        [Test]
        public void testDelete()
        {
            String okResponse = _dao.delete("901", "8994.5", "35,").text;

            Assert.IsTrue(String.Equals("OK", okResponse));
        }

        #endregion
    }
}
