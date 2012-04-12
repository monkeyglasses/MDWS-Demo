using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MdwsDemo.dao.soap;
using MdwsDemo.mdws;

namespace MdwsDemo.domain
{
    public class MySession
    {
        public string VisitorPassword = "This is a sample BSE pass phrase";
        public string SelectedSite { get; set; }
        public VistaDao VistaDao { get; set; }
        public UserTO User { get; set; }
        public PatientTO Patient { get; set; }
        public RegionArray SitesFile { get; set; }
        /// <summary>
        /// Use for storing things between calls
        /// </summary>
        public object TempHolder { get; set; }
    }
}