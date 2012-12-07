using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MdwsDemo.mdws;

namespace MdwsDemo.dao.soap
{
    public class VistaDao
    {
        EmrSvc _emrSvc;

        public VistaDao()
        {
            _emrSvc = new EmrSvc();
            _emrSvc.CookieContainer = new System.Net.CookieContainer();
        }

        public System.Net.CookieContainer EmrSvcSession
        {
            get { return _emrSvc.CookieContainer; }
        }

        public RegionArray getSitesFile()
        {
            RegionArray regions = _emrSvc.getVHA();
            if (regions.fault != null)
            {
                throw new ApplicationException(regions.fault.message);
            }
            foreach (RegionTO visn in regions.regions)
            {
                visn.name = "VISN " + visn.id + " - " + visn.name; // makes for a user friendly dropdown
            }
            return regions;
        }

        public SiteTO addDataSource(string sitecode, string siteName, string hostname, string brokerPort, string visn)
        {
            SiteTO result = _emrSvc.addDataSource(sitecode, siteName, hostname, brokerPort, "HIS", "VISTA", visn);
            if (result.fault != null)
            {
                throw new ApplicationException(result.fault.message);
            }
            return result;
        }

        public string connect(string sitecode)
        {
            DataSourceArray result = _emrSvc.connect(sitecode);
            if (result.fault != null)
            {
                throw new ApplicationException(result.fault.message);
            }
            return result.items[0].welcomeMessage;
        }

        public UserTO login(string accessCode, string verifyCode)
        {
            UserTO result = _emrSvc.login(accessCode, verifyCode, "OR CPRS GUI CHART");
            if (result.fault != null)
            {
                throw new ApplicationException(result.fault.message);
            }
            return result;
        }

        public void visit(string securityPhrase, string visitSite, string userSite, 
            string userName, string duz, string ssn, string permissionString)
        {
            TaggedTextArray result = _emrSvc.visit(securityPhrase, visitSite, userSite, userName, duz, ssn, permissionString);
            if (result.fault != null)
            {
                throw new ApplicationException(result.fault.message);
            }
            if (result.results != null && result.results.Length > 0)
            {
                foreach (TaggedText tt in result.results)
                {
                    if (tt != null && tt.fault != null)
                    {
                        throw new ApplicationException(tt.fault.message);
                    }
                }
            }
        }

        public PatientTO select(string dfn)
        {
            PatientTO result = _emrSvc.select(dfn);
            if (result.fault != null)
            {
                throw new ApplicationException(result.fault.message);
            }
            return result;
        }

        public void setupMultiSiteQuery(string securityPhrase)
        {
            SiteArray result = _emrSvc.setupMultiSiteQuery(securityPhrase);
            if (result.fault != null)
            {
                throw new ApplicationException(result.fault.message);
            }
        }

        public IList<MedicationTO> getAllMeds()
        {
            TaggedMedicationArrays allMeds = _emrSvc.getAllMeds();
            IList<MedicationTO> result = new List<MedicationTO>();

            if (allMeds.fault != null)
            {
                throw new ApplicationException(allMeds.fault.message);
            }
            foreach (TaggedMedicationArray tma in allMeds.arrays)
            {
                if (tma != null && tma.fault != null)
                {
                    throw new ApplicationException(tma.fault.message);
                }
                if (tma == null || tma.count == 0 || tma.meds == null)
                {
                    continue;
                }
                foreach (MedicationTO med in tma.meds)
                {
                    result.Add(med);
                }
            }
            return result;
        }

        public void disconnect()
        {
            if (_emrSvc != null)
            {
                try
                {
                    _emrSvc.disconnect();
                }
                catch (Exception)
                {
                    // we don't ever want to care about this function
                }
            }
        }

        public IList<PatientTO> match(string target)
        {
            TaggedPatientArrays tpas = _emrSvc.match(target);

            if (tpas == null || tpas.arrays == null || tpas.arrays.Length == 0)
            {
                throw new ApplicationException("Nothing returned!");
            }
            if (tpas.fault != null)
            {
                throw new ApplicationException(tpas.fault.message);
            }
            if (tpas.arrays[0] == null || tpas.arrays[0].count == 0 || tpas.arrays[0].patients == null || tpas.arrays[0].patients.Length == 0)
            {
                throw new ApplicationException("No patients returned for that search...");
            }

            IList<PatientTO> patients = new List<PatientTO>();

            foreach (PatientTO patient in tpas.arrays[0].patients)
            {
                patients.Add(patient);
            }

            return patients;
        }
    }
}