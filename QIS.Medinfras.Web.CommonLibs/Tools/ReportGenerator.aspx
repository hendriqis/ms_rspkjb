<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportGenerator.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.ReportGenerator" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnGenerateReport" runat="server" Text="Generate Report" OnClick="btnGenerateReport_Click" /><br />
        <asp:Button ID="btnGenerateBaseReport" runat="server" Text="Generate Base Report" OnClick="btnGenerateBaseReport_Click" />
    </div>
    </form>
</body>
</html>
