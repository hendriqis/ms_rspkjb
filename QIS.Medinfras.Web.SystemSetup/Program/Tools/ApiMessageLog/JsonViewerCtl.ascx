<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JsonViewerCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.JsonViewerCtl" %>
<input type="hidden" id="hdnContractID" value="" runat="server" />
<input type="hidden" id="hdnPayerID" value="" runat="server" />
<div runat="server" id="divJsonViewer" style="overflow-x: hidden; width: 1000px;
    overflow-y: auto; height: 400px">
    <asp:TextBox ID="txtUserName" Width="100%" runat="server" TextMode="MultiLine" Height="100%"
        ReadOnly="true" />
</div>
