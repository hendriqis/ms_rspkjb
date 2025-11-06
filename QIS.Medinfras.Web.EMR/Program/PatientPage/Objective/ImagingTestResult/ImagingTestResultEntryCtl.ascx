<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImagingTestResultEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.PatientPage.ImagingTestResultEntryCtl" %>
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

        $('#<%=txtItemName.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtPhotoNumber.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtReferenceNo.ClientID %>').attr('readonly', 'readonly');

        $('#containerEnglish').filter(':visible').hide();

        $('#ulTabLabResult li').click(function () {
            $('#ulTabLabResult li.selected').removeAttr('class');
            $('.containerOrder').filter(':visible').hide();
            $contentID = $(this).attr('contentid');
            $('#' + $contentID).show();
            $(this).addClass('selected');
        });
        //#endregion
    });
    //#region Preview
    $('.btnPreview').click(function () {
        $btnPreview = $(this);
        if ($('#<%:hdnReferenceNo.ClientID %>').val() == "") {
            showToast("ERROR", 'Error Message : ' + "There is no result to be view !");
        }
        else {
            var referenceNo = $('#<%=txtReferenceNo.ClientID %>').val();
            var result = '<%=GetImagingResultImage() %>';
            var resultInfo = result.split('|');
            if (resultInfo[0] == "1") {
                window.open(resultInfo[1], "popupWindow", "width=600, height=600,scrollbars=yes");
            }
            else {
                showToast('Cannot open result preview', 'Error Message : ' + result[1]);
            }
        }
    });
    //#endregion
</script>
<style type="text/css">
    .containerOrder
    {
        border: 1px solid #EAEAEA;
        padding: 0 5px;
    }
</style>
<input type="hidden" id="hdnItemID" value="" runat="server" />
<input type="hidden" id="hdnTransactionID" value="" runat="server" />
<input type="hidden" id="hdnTestOrderID" value="" runat="server" />
<input type="hidden" id="hdnID" value="" runat="server" />
<input type="hidden" id="hdnReferenceNo" value="" runat="server" />
<div style="height: 445px; overflow-y: auto;">
    <table class="tblContentArea">
        <colgroup>
            <col width="60%" />
            <col width="40%" />
        </colgroup>
        <tr valign="top">
            <td>
                <table style="width: 100%">
                    <colgroup>
                        <col style="width: 180px" />
                        <col style="width: 150px" />
                        <col style="width: 120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Test Item")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtItemName" Width="305px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Reference Number")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReferenceNo" Width="100%" runat="server" />
                        </td>
                        <td style="padding-left: 10px">
                            <input type="button" class="btnPreview" value="Display Image" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Photo Number")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhotoNumber" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="display: none">
                <table width="100%">
                    <colgroup>
                        <col width="150px" />
                    </colgroup>
                    <tr>
                        <td>
                            <img src="" runat="server" id="imgPreview" width="150" height="150" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="containerUlTabPage" style="margin-bottom: 3px;">
                    <ul class="ulTabPage" id="ulTabLabResult">
                        <li class="selected" contentid="containerIndonesia">
                            <%=GetLabel("Indonesia") %></li>
                        <li contentid="containerEnglish">
                            <%=GetLabel("English")%></li>
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
            </td>
        </tr>
    </table>
</div>
