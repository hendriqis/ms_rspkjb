<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPBase.Master" AutoEventWireup="true" CodeBehind="ReportViewerDirectPrint.aspx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ReportViewerDirectPrint" %>

<%@ Register assembly="DevExpress.XtraReports.v11.1.Web, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPBase" runat="server">
    <div style="display:none">
        <dx:ReportViewer ID="ReportViewer1" runat="server" AutoSize="True"  >
            <ClientSideEvents PageLoad="function(s,e){ s.Print(); }" />
        </dx:ReportViewer>
    </div>
    <input type="hidden" id="hdnParam" runat="server" value="" />
</asp:Content>
