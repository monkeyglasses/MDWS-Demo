<%@ Page Title="Login Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="MdwsDemo.Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

        <p><asp:Label ID="labelMessage" ForeColor="Red" Font-Bold="true" Font-Size="Larger" runat="server" /></p>


        Select Your VISN: <asp:DropDownList ID="dropDownVisn" AutoPostBack="true" OnSelectedIndexChanged="selectVisn" runat="server" />
        <br /><br />
        <asp:Label ID="labelSiteText" Text="Select Your Site: " runat="server" />
        <asp:DropDownList ID="dropDownSite" AutoPostBack="true" OnSelectedIndexChanged="selectSite" runat="server" />
        <br /><br />

        <small><asp:Literal ID="literalWelcomeMsg" runat="server" /></small>

        Access Code: <asp:TextBox ID="textBoxAccessCode" runat="server" />
        <br /><br />
        Verify Code: <asp:TextBox ID="textBoxVerifyCode" runat="server" />
        <br /><br />
        <asp:Button ID="buttonLogin" OnClick="LoginClick" Text="Login" runat="server" />


        <br /><br /><br /><br /><br />
        <asp:Button ID="buttonDownloadSource" Text="Like This? Download Source!" OnClick="DownloadSource" runat="server" />

        <p>
        Need some same A/V codes? <a href="http://sandbox.vainnovation.us/groups/innovationvista/wiki/60417/Access_and_Verify_Codes_for_VISN_0__Innovation_VistA.html">Click here!</a>
        </p>
</asp:Content>
