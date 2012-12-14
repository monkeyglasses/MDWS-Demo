using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MdwsDemo.scheduling;
using MdwsDemo.dao.soap;
using MdwsDemo.utils;
using MdwsDemo.domain;

namespace MdwsDemo.web.mhv
{
    public partial class Calendar : System.Web.UI.Page
    {
        DateTime _selectedDate;
        IList<TimeSlot> _slots;
        SchedulingDao _dao;
        string _siteCode = "100";
        string _accessCode = "9952FRN";
        string _verifyCode = "MQGS)874";
        string _patient = "51";
        string _ssn = "222334444";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (calendarSelectDate.SelectedDate == null || calendarSelectDate.SelectedDate.Year == 1)
                {
                    calendarSelectDate.SelectedDate = DateTime.Today;
                    _selectedDate = DateTime.Today;
                }
                else
                {
                    _selectedDate = calendarSelectDate.SelectedDate;
                }

                _dao = new SchedulingDao();
                UserTO user = _dao.connectAndLogin(_siteCode, _accessCode, _verifyCode);
                //_dao.connect("901");
                //_dao.login("1programmer", "programmer1", "");
                HospitalLocationTO clinic = _dao.getClinicSchedulingDetails("12");
                PatientTO selectedPatient =_dao.selectPatient(_patient);
                labelPatientName.Text = selectedPatient.name;
                _dao.disconnect();
                // just do this once at launch
                _slots = SchedulingUtils.getItemsFromAvailabilityString
                    (clinic.clinicDisplayStartTime, Convert.ToInt32(clinic.appointmentLength), Convert.ToInt32(clinic.displayIncrements), clinic.availability);
                Session.Add("SLOTS", _slots);
                populateDropdown(_slots);
            }

            _slots = Session["SLOTS"] as IList<TimeSlot>;
        }

        protected void Click_SelectDate(object sender, EventArgs e)
        {
            _selectedDate = calendarSelectDate.SelectedDate;
            populateDropdown(_slots);
        }

        protected void Click_MakeAppointment(object sender, EventArgs e)
        {
            TimeSlot selectedSlot =
                (Session["FILTERED_SLOTS"] as IList<TimeSlot>)[dropdownAvailableTimes.SelectedIndex];

            if (!selectedSlot.Available)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Please choose an available slot only');", true);
                return;
            }

            string dateString = "3" + selectedSlot.Start.ToString("yyMMdd.HHmmss");

            try
            {
                //esb.appointmentResponse response = new SchedulingDao().makeAppointmentEsb(dateString, "30", "12", _patient, _ssn);

                _dao = new SchedulingDao();
                UserTO user = _dao.connectAndLogin(_siteCode, _accessCode, _verifyCode);
                AppointmentTO result = _dao.makeAppointment(_patient, "12", dateString, "N", "", "30", "9");
                _dao.disconnect();

                if (result.fault == null)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Your appointment is set for " +
                        selectedSlot.Start.ToString("MM/dd") + " at " + selectedSlot.Start.ToString("HH:mm") +
                        ". Good job.');", true);
                }
            }
            catch (Exception exc)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + exc.Message + "')", true);
            }
        }

        private void populateDropdown(IList<TimeSlot> slots)
        {
            IList<TimeSlot> filtered = new List<TimeSlot>();
            foreach (TimeSlot slot in slots)
            {
                if (slot.Start.Year == _selectedDate.Year &&
                    slot.Start.Month == _selectedDate.Month &&
                    slot.Start.Day == _selectedDate.Day)
                {
                    filtered.Add(slot);
                }
            }
            dropdownAvailableTimes.DataSource = filtered;
            dropdownAvailableTimes.DataTextField = "Text";
            dropdownAvailableTimes.DataValueField = "Start";
            dropdownAvailableTimes.DataBind();

            Session["FILTERED_SLOTS"] = filtered;
        }
    }
}