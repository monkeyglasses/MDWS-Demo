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
    public partial class Login : System.Web.UI.Page
    {
        MySession _mySession;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _mySession = new MySession();

                _mySession.VistaDao = new VistaDao();
                _mySession.SitesFile = _mySession.VistaDao.getSitesFile();
                dropDownVisn.DataSource = _mySession.SitesFile.regions;
                dropDownVisn.DataTextField = "name";
                dropDownVisn.DataValueField = "id";
                dropDownVisn.DataBind();

                dropDownVisn.SelectedValue = _mySession.SitesFile.regions[0].id; // set the selected value for the selectVisn function
                selectVisn(dropDownVisn, e);

                Session["MySession"] = _mySession;
                return;
            }

            _mySession = (MySession)Session["MySession"];
        }

        protected void LoginClick(object sender, EventArgs e)
        {
            string accessCode = textBoxAccessCode.Text;
            string verifyCode = textBoxVerifyCode.Text;

            try
            {
                _mySession.VistaDao.connect(_mySession.SelectedSite);
                UserTO user = _mySession.VistaDao.login(accessCode, verifyCode);
                if (user.fault != null) // invalid login
                {
                    labelMessage.Text = user.fault.message;
                }
                else
                {
                    _mySession.VistaDao.disconnect(); // Why are we disconnecting?? We're done with the connection until we need more data. We can visit for subsequent calls
                    _mySession.User = user;
                    Session["MySession"] = _mySession;
                    Response.Redirect("PatientSelection.aspx", true);
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                // ugh... MS... this exception is thrown by the Response.Redirect function - we just catch it and continue normally
            }
            catch (Exception exc)
            {
                labelMessage.Text = exc.Message;
            }
        }

        protected void selectVisn(Object sender, EventArgs e)
        {
            string visnId = (sender as DropDownList).SelectedValue;
            RegionTO selectedRegion = null;
            foreach(RegionTO region in _mySession.SitesFile.regions)
            {
                if (String.Equals(region.id, visnId))
                {
                    selectedRegion = region;
                    break;
                }
            }
            dropDownSite.DataSource = selectedRegion.sites.sites;
            dropDownSite.DataTextField = "name";
            dropDownSite.DataValueField = "sitecode";
            dropDownSite.DataBind();
            dropDownSite.Items.Insert(0, "== Select Your Site ==");
        }

        protected void selectSite(Object sender, EventArgs e)
        {
            string selectedSite = (sender as DropDownList).SelectedValue;
            if (String.IsNullOrEmpty(selectedSite) || String.Equals(selectedSite, "== Select Your Site =="))
            {
                return;
            }
            _mySession.SelectedSite = selectedSite;
            string welcomeMsg = _mySession.VistaDao.connect(_mySession.SelectedSite);
            welcomeMsg = welcomeMsg.Replace("\n", "<br />");
            literalWelcomeMsg.Text = "<p>" + welcomeMsg + "</p>";
            _mySession.VistaDao.disconnect(); // we're disconnecting to keep session management as simple as possible
        }

        protected void DownloadSource(Object sender, EventArgs e)
        {
            try
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(@"C:\inetpub\wwwroot\MdwsDemo\MdwsDemo.7z");
                System.IO.FileStream file = System.IO.File.Open(@"C:\inetpub\wwwroot\MdwsDemo\MdwsDemo.7z", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                byte[] bytes = new byte[fileInfo.Length];
                int read = file.Read(bytes, 0, (int)fileInfo.Length);
                file.Close();
                file.Dispose();

                if (bytes == null || bytes.Length == 0)
                {
                    labelMessage.Text = "Awww, snap. Doesn't look like we were able to get that for you. Complain and try back later...";
                }
                else
                {
                    Response.AddHeader("Content-Disposition", "attachment;filename=MdwsDemo.7z");
                    Response.ContentType = "application/octet-stream";
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception exc)
            {
                labelMessage.Text = "Awww, snap. I couldn't get that for you. Maybe this will help figure out why:</p>" + exc.Message + "</p>";
            }
        }
    }
}