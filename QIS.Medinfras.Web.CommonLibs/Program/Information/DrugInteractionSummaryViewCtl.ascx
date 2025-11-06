<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DrugInteractionSummaryViewCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.DrugInteractionSummaryViewCtl" %>
<style>
    .viewClass
    {
        width: 700px;
        overflow: auto;
        height: 400px;
        overflow-y: auto;
    }
</style>
<input type="hidden" id="hdnID" value="" runat="server" />
<iframe style="width: 680px; height: 400px; border: 0" src="<%= ResolveUrl("~/Libs/Scripts/MIMS/Mims.html")%>"
    title=""></iframe>
