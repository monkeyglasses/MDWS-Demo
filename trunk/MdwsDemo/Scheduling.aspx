<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Scheduling.aspx.cs" Inherits="MdwsDemo.Scheduling" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Please select your clinic:<br />
        <asp:ListBox ID="listboxClinics" runat="server" Rows="8" />

        <asp:Calendar ID="Calendar1" runat="server">
            
        </asp:Calendar>


    </div>
    </form>
</body>
</html>
