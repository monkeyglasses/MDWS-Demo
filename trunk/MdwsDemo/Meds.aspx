<%@ Page Title="Meds Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Meds.aspx.cs" Inherits="MdwsDemo.Meds" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <p>
        Patient Name: <asp:Label ID="labelPatientName" runat="server" />
    </p>

    <asp:DataGrid ID="datagridMeds" runat="server" />

    <asp:Label ID="labelErrorMessage" runat="server" ForeColor="Red" Font-Bold="true" />
</asp:Content>
