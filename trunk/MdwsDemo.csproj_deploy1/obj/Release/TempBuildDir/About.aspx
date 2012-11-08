<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="MdwsDemo.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        About
    </h2>
    <p>
        The awesome folks at the VA wrote this nifty little demo to illustrate how easy it actually is to use MDWS for data. It consists of 
        several web pages showing how to leverage MDWS for common tasks such as:
    </p>

    <ul>
        <li>Leveraging Vista A/V codes for user authentication/authorization</li>
        <li>Searching for and selecting a patient</li>
        <li>Retrieving patient data and displaying it in a GUI</li>
        <li>Writing a progress note</li>
        <li>Writing unit tests for your application</li>
        <li>Exercising MDWS via unit testing</li>
    </ul>
</asp:Content>
