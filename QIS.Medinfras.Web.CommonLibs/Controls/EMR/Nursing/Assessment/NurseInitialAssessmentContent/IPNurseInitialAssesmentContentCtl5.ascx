<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IPNurseInitialAssesmentContentCtl5.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.IPNurseInitialAssesmentContentCtl5" %>

<script type="text/javascript" id="dxss_nurseInitialAssessmentctl5">
    $(function () {
        if ($('#<%=hdnSocialHistoryValue.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnSocialHistoryValue.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent2.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent2.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent2.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent2.ClientID %>').find('.chkNursingProblem').each(function () {
                    if ($(this).attr('controlID') == temp[0] && $(this).attr('nursingProblemCode') == temp[1]) {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent2.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent2.ClientID %>').find('.txtNursingProblem').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
            }
        }
    });
</script>

<input type="hidden" runat="server" id="hdnSocialHistoryLayout" value="" />
<input type="hidden" runat="server" id="hdnSocialHistoryValue" value="" />
<h4 class="w3-blue">
    <%=GetLabel(" Pengkajian Psikososial Spiritual dan Kultural")%></h4>
<div class="containerTblEntryContent">
    <table class="tblContentArea">
        <tr>
            <td>
                <div id="divFormContent2" runat="server" style="height: 450px;overflow-y: scroll;"></div>
            </td>
        </tr>
    </table>
</div>

