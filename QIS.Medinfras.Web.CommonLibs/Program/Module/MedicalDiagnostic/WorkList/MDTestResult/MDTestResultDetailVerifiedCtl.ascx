<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MDTestResultDetailVerifiedCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.MDTestResultDetailVerifiedCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $(function () {
        //#region Service Unit
        function getTemplateTextExpression() {
            var filterExpression = "GCTemplateGroup = '<%=GCTemplateGroup %>' AND IsDeleted = 0";
            return filterExpression;
        }

        $('#<%=txtItemInfo.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtBorderLine.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtPhotoNumber.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtFileName.ClientID %>').attr('readonly', 'readonly');

        $('#containerEnglish').filter(':visible').hide();

        $('#ulTabLabResult li').click(function () {
            $('#ulTabLabResult li.selected').removeAttr('class');
            $('.containerOrder').filter(':visible').hide();
            $contentID = $(this).attr('contentid');
            $('#' + $contentID).show();
            $(this).addClass('selected');
        });

        //#endregion

        $('.btnViewer').live('click', function () {
            var isBriding = $('#<%=hdnIsRisBRIDGING.ClientID %>').val();
            if (isBriding == "1") {
                if ($('#<%=hdnRISVendor.ClientID %>').val() == "X081^07") {
                    var medicalNo = $('#<%=hdnMedicalNo.ClientID %>').val().replaceAll('-', '');
                    var viewerUrl = $('#<%=hdnViewerUrl.ClientID %>').val() + medicalNo;
                    window.open(viewerUrl, "popupWindow", "width=600, height=600,scrollbars=yes");
                }
            } else {
                var viewerUrl = $('#<%=hdnViewerUrl.ClientID %>').val();
                window.open(viewerUrl, "popupWindow", "width=600, height=600,scrollbars=yes");
            }
        });
    });
       
</script>
<style type="text/css">
    .containerOrder         { border: 1px solid #EAEAEA; padding: 0 5px; height:350px }
</style>
<input type="hidden" id="hdnItemID" value="" runat="server" />
<input type="hidden" id="hdnID" value="" runat="server" />
<input type="hidden" id="hdnMedicalNo" value="" runat="server" />
<input type="hidden" id="hdnViewerUrl" value="" runat="server" />
<input type="hidden" id="hdnRISVendor" value="" runat="server" />
<input type="hidden" id="hdnIsRisBRIDGING" value="" runat="server" />
<div style="height:500px;overflow-y:auto;">
    <table class="tblContentArea">
        <colgroup>
            <col style="width:150px"/>
            <col style="width:150px"/>
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Pemeriksaan")%></label></td>
            <td><asp:TextBox ID="txtItemInfo" Width="350px" runat="server" /></td>      
            <td>
                <input type="button" id="btnViewer" runat="server" class="btnViewer" value="View Image" style="height: 25px; width: 100px; background-color: Red; color: White;" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No. Photo")%></label></td>
            <td><asp:TextBox ID="txtPhotoNumber" Width="350px" runat="server" /></td>      
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama File")%></label></td>
            <td><asp:TextBox ID="txtFileName" Width="350px" runat="server" /></td>      
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Result Border Line")%></label></td>
            <td><asp:TextBox ID="txtBorderLine" Width="350px" runat="server" /></td> 
        </tr>
    </table>
    <div class="containerUlTabPage" style="margin-bottom:3px;">
           <ul class="ulTabPage" id="ulTabLabResult">
                <li class="selected" contentid="containerIndonesia"><%=GetLabel("Indonesia") %></li>
                <li contentid="containerEnglish"><%=GetLabel("English")%></li>
           </ul>
    </div>

   <div id="containerIndonesia" class="containerOrder">
        <div id="contentIndonesia" runat="server">
        </div>
   </div>

   <div id="containerEnglish" class="containerOrder">
        <div id="contentEnglish" runat="server">
        </div>
   </div>

</div>

