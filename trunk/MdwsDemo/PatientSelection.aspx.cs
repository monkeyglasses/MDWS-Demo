using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MdwsDemo.domain;
using MdwsDemo.mdws;
using System.Configuration;

namespace MdwsDemo
{
    public partial class PatientSelection : System.Web.UI.Page
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
                Session["MySession"] = _mySession;
            }
#endif
            _mySession = (Session["MySession"] as MySession);
            if (_mySession == null || _mySession.User == null)
            {
                Response.Redirect("Login.aspx", true);
            }

            if (!IsPostBack)
            {
                labelMessage.Text = "Congratulations on your successful login, " + _mySession.User.name + ". You're so smart. You can search for a patient below.";
            }
        }

        protected void matchPatient(object sender, EventArgs e)
        {
            try
            {
                // remember we disconnected after logging in... need to visit for more data
                _mySession.VistaDao.visit(_mySession.VisitorPassword, _mySession.User.siteId, _mySession.User.siteId, _mySession.User.name, _mySession.User.DUZ, _mySession.User.SSN, "");
                IList<PatientTO> matchResult = _mySession.VistaDao.match(textboxPatientSearch.Text);
                _mySession.TempHolder = matchResult;
                _mySession.VistaDao.disconnect();
                Session["MySession"] = _mySession;
                listboxPatientMatchResults.DataSource = matchResult;
                listboxPatientMatchResults.DataTextField = "name";
                listboxPatientMatchResults.DataValueField = "localPid";
                listboxPatientMatchResults.DataBind();
            }
            catch (Exception exc)
            {
                labelMessage.Text = exc.Message;
            }
        }

        protected void showPatientProperties(object sender, EventArgs e)
        {
            try
            {
                string patientDfn = listboxPatientMatchResults.SelectedItem.Value;
                IList<PatientTO> tempPatients = _mySession.TempHolder as IList<PatientTO>;

                PatientTO selectedPatient = null;
                foreach (PatientTO p in tempPatients)
                {
                    if (p.localPid == patientDfn)
                    {
                        selectedPatient = p;
                        break;
                    }
                }

                labelDfn.Text = patientDfn;
                labelDob.Text = selectedPatient.dob;
            }
            catch (Exception exc)
            {
                labelMessage.Text = exc.Message;
            }
        }

        protected void selectPatient(object sender, EventArgs e)
        {
            try
            {
                string selectedDfn = listboxPatientMatchResults.SelectedItem.Value;

                IList<PatientTO> tempPatients = _mySession.TempHolder as IList<PatientTO>;
                PatientTO selectedPatient = null;
                foreach (PatientTO p in tempPatients)
                {
                    if (p.localPid == selectedDfn)
                    {
                        selectedPatient = p;
                        break;
                    }
                }

                _mySession.Patient = selectedPatient;
                labelMessage.Text = "You have selected " + selectedPatient.name + ". All subsequent calls would normally operate operate on this patient";
                panelNavigation.Visible = true; // show nav panel now that we've selected a patient
            }
            catch (Exception exc)
            {
                labelMessage.Text = exc.Message;
            }
        }

    }
}