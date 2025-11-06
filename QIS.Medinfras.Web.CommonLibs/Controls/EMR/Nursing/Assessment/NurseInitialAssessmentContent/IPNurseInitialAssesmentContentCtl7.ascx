<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IPNurseInitialAssesmentContentCtl7.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.IPNurseInitialAssesmentContentCtl7" %>

<script type="text/javascript" id="dxss_nurseInitialAssessmentctl7">
    $(function () {
        if ($('#<%=hdnDischargePlanningValue.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnDischargePlanningValue.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent4.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent4.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent4.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent4.ClientID %>').find('.chkNursingProblem').each(function () {
                    if ($(this).attr('controlID') == temp[0] && $(this).attr('nursingProblemCode') == temp[1]) {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent4.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
                $('#<%=divFormContent4.ClientID %>').find('.txtNursingProblem').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
                });
            }
        }
    });
</script>

<input type="hidden" runat="server" id="hdnDischargePlanningLayout" value="" />
<input type="hidden" runat="server" id="hdnDischargePlanningValue" value="" />
<h4 class="w3-blue">
    <%=GetLabel("Perencanaan Pemulangan Pasien")%></h4>
<div class="containerTblEntryContent">
    <table class="tblContentArea">
        <tr>
            <td>
                <div id="divFormContent4" runat="server" style="height: 450px;overflow-y: scroll;"></div>
            </td>
        </tr>
    </table>
</div>

