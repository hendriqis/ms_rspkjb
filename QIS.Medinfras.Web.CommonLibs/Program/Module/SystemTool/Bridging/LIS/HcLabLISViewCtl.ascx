<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HcLabLISViewCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.HcLabLISViewCtl" %>
<div runat="server" id="divJsonViewer" style="overflow-x: hidden;
    overflow-y: auto; height: 400px">
    <asp:TextBox ID="txtResult" Width="100%" runat="server" TextMode="MultiLine" Height="100%"
        ReadOnly="true" />
</div>