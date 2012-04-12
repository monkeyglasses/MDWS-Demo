using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MdwsDemo.dao.soap;
using MdwsDemo.mdws;
using MdwsDemo.domain;

namespace MdwsDemo
{
    public partial class Meds : System.Web.UI.Page
    {
        MySession _mySession;

        protected void Page_Load(object sender, EventArgs e)
        {
#if DEBUG
            if (!Page.IsPostBack && Session["MySession"] == null)
            {
                _mySession = new MySession();
                _mySession.User = new UserTO() { DUZ = "20005", name = "VEHU,FOUR", SSN = "666548835", siteId = "901" };
                _mySession.VistaDao = new dao.soap.VistaDao();

                if (_mySession.Patient == null)
                {
                    _mySession.VistaDao.visit(_mySession.VisitorPassword, _mySession.User.siteId, _mySession.User.siteId, _mySession.User.name, _mySession.User.DUZ, _mySession.User.SSN, "");
                    _mySession.Patient = _mySession.VistaDao.select("3");
                    _mySession.VistaDao.disconnect();
                }

                Session["MySession"] = _mySession;
            }
#endif
            _mySession = (Session["MySession"] as MySession);
            if (_mySession == null || _mySession.User == null)
            {
                Response.Redirect("Login.aspx", true);
            }

            if (_mySession.Patient == null)
            {
                Response.Redirect("PatientSelection.aspx");
            }

            _mySession.VistaDao.visit(_mySession.VisitorPassword, _mySession.User.siteId, _mySession.User.siteId, _mySession.User.name, _mySession.User.DUZ, _mySession.User.SSN, "");
            _mySession.Patient = _mySession.VistaDao.select(_mySession.Patient.localPid);
            IList<MedicationTO> results = _mySession.VistaDao.getAllMeds();
            _mySession.VistaDao.disconnect();

            labelPatientName.Text = _mySession.Patient.name;
            datagridMeds.DataSource = results;
            datagridMeds.DataBind();
        }
    }
}