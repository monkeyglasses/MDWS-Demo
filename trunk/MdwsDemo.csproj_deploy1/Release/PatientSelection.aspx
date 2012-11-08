<%@ Page Title="Patient Selection Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="PatientSelection.aspx.cs" Inherits="MdwsDemo.PatientSelection" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <asp:Label ID="labelMessage" ForeColor="Red" Font-Bold="true" Font-Size="Larger" runat="server" />
    <div>
        <p>
            Search for a patient. The "match" function supports searching by SSN, name, last initial + last 4 SSN. For example:
        </p>
        <ul>
            <li>123456789</li>
            <li>SMITH,JOHN</li>
            <li>A6789</li>
        </ul>
        <br />
        
        Search: <asp:TextBox ID="textboxPatientSearch" runat="server" />&nbsp;<asp:Button ID="buttonMatch" runat="server" Text="Find Patient" OnClick="matchPatient" />
        
        <br /><br />

        <asp:ListBox ID="listboxPatientMatchResults" runat="server" OnSelectedIndexChanged="showPatientProperties" AutoPostBack="true" />
        <div>
            DFN: <asp:Label ID="labelDfn" runat="server" /><br />
            DOB: <asp:Label ID="labelDob" runat="server" />
        </div>

        <br />
        <asp:Button ID="buttonSelectPatient" Text="Select Patient" runat="server" OnClick="selectPatient" />


        <asp:Panel ID="panelNavigation" Visible="false" runat="server">
            <p>Now that you've successfully logged in and selected a patient, what would you like to look at?</p>

            <ul>
                <li><a href="Meds.aspx">Meds</a></li>
            </ul>
        </asp:Panel>
    </div>

</asp:Content>
