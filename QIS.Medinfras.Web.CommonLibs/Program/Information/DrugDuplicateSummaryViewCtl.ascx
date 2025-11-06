<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DrugDuplicateSummaryViewCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.DrugDuplicateSummaryViewCtl" %>
<style>
.viewClass 
{
   width:700px;
   overflow:auto;
   height:400px;
   overflow-y: auto;
}
</style>

<input type="hidden" id="hdnID" value="" runat="server" />
<div runat="server" id="divDuplicate" class="viewClass" style="width:700px;overflow:auto;height:350px;">
</div>