<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImagingTestResultDetailCtl.ascx.cs" 
Inherits="QIS.Medinfras.Web.Imaging.Program.ImagingTestResultDetailCtl" %>


<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $(function () {
        //#region ServiceUnit

        function getTemplateTextExpression() {
            var filterExpression = "GCTemplateGroup = '<%=GCTemplateGroup %>' AND IsDeleted = 0";
            return filterExpression;
        }

        $('#lblTestResult.lblLink').click(function () {
            openSearchDialog('templatetext', getTemplateTextExpression(), function (value) {
                var filterExpression = getTemplateTextExpression() + " AND TemplateCode = '" + value + "'";
                Methods.getObjectValue('GetTemplateTextList', filterExpression, "TemplateContent", function (result) {
                    tinyMCE.get('<%=txtTestResult.ClientID %>').execCommand('mceSetContent', false, result);
                    //tinyMCE.triggerSave();
                });
            });
        });

        setHtmlEditor();
    });   
</script>
<input type="hidden" id="hdnItemID" value="" runat="server" />
<input type="hidden" id="hdnID" value="" runat="server" />
<div class="pageTitle"><%=GetLabel("Work List : Imaging Test Result Detail")%></div>
<div style="height:445px;overflow-y:scroll;">
    <div class="pageTitle">
    </div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:49%"/>
            <col style="width:3px"/>
            <col style="width:49%"/>
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Border Line")%></label></td>
            <td>
            <dxe:ASPxComboBox ID="cboBorderLine" ClientInstanceName="cboBorderLine" Width="350px" runat="server"/>
            </td>        
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Photo Number :")%></label></td>
            <td><asp:TextBox ID="txtPhotoNumber" Width="120px" runat="server" /></td>      
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("File Name :")%></label></td>
            <td><asp:TextBox ID="txtFileName" Width="350px" runat="server" /></td>      
        </tr>
        <tr>
            <td colspan="2" class="tdLabel"><label class="lblNormal lblLink" id="lblTestResult"><%=GetLabel("Test Result")%></label></td>
        </tr>
        <tr>
            <td colspan="2"><asp:TextBox TextMode="MultiLine" Width="100%" Height="300px" ID="txtTestResult" runat="server" CssClass="htmlEditor" /></td>
        </tr>
    </table>
</div>

