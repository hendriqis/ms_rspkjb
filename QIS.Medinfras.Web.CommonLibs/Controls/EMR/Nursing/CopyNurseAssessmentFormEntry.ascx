<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CopyNurseAssessmentFormEntry.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.CopyNurseAssessmentFormEntry" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_copynurseassessmentformentryctl">
    setDatePicker('<%=txtObservationDate.ClientID %>');
    $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', '0');

    function openLink(evt, animName) {
        var i, x, tablinks;
        x = document.getElementsByClassName("city");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        tablinks = document.getElementsByClassName("tablink");
        for (i = 0; i < x.length; i++) {
            tablinks[i].className = tablinks[i].className.replace(" w3-red", "");
        }
        document.getElementById(animName).style.display = "block";
        evt.currentTarget.className += " w3-red";
    }

    $(function () {
        if ($('#<%=hdnFormValuesCopyCtl.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnFormValuesCopyCtl.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
                $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1")
                        $(this).prop('checked', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1")
                        $(this).prop('checked', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
                $('#<%=divFormContent.ClientID %>').find('.chkNursingProblem').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.txtNursingProblem').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
            }
        }
    });

    function onBeforeSaveRecordEntryPopup() {
        var values = getFormValues();
        return true;
    }

    function getFormValues() {
        var controlValues = '';
        $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            controlValues += $(this).attr('controlID') + '=' + $(this).val();
        });
        $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            controlValues += $(this).attr('controlID') + '=' + $(this).val();
        });
        $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });
        $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });

        $('#<%=hdnFormValuesCopyCtl.ClientID %>').val(controlValues);

        return controlValues;
    }

    //#region Propose
    $('.btnTest').die('click');
    $('.btnTest').live('click', function () {
        alert(getFormValues());
    });
    //#endregion
</script>
<style type="text/css">
    
</style>
<div style="height: auto">
    <input type="hidden" runat="server" id="hdnIDCopyCtl" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnFormType" value="" />
    <input type="hidden" runat="server" id="hdnFormLayoutCopyCtl" value="" />
    <input type="hidden" runat="server" id="hdnFormValuesCopyCtl" value="" />
    <table class="tblEntryContent" style="width: 100%">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 150px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Medical No")%></label>
            </td>
            <td class="tdLabel">
                <asp:TextBox ID="txtMedicalNo" Width="120px" runat="server" ReadOnly />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Nama Pasien")%></label>
            </td>
            <td class="tdLabel">
                <asp:TextBox ID="txtPatientName" Width="450px" runat="server" ReadOnly />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Tanggal Lahir")%></label>
            </td>
            <td class="tdLabel">
                <asp:TextBox ID="txtDateOfBirth" Width="180px" runat="server" ReadOnly />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("No. Registrasi")%></label>
            </td>
            <td class="tdLabel">
                <asp:TextBox ID="txtRegistrationNo" Width="180px" runat="server" ReadOnly />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Tanggal ")%>
                    -
                    <%=GetLabel("Jam ")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server" />
                <asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server"
                    Style="text-align: center" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("PPA")%></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox ID="cboParamedicID" Width="350px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
            </td>
            <td colspan="2">
                <asp:CheckBox ID="chkIsInitialAssessment" runat="server" Text=" Bagian Pengkajian Awal Pasien" />
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <div id="divFormContent" runat="server" style="height: 300px; overflow-y: auto;">
                </div>
            </td>
        </tr>
        <tr id="trToddlerNutritionProblem" runat="server">
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Masalah Gizi Balita")%></label>
            </td>
            <td colspan="2">
                <dxe:ASPxComboBox ID="cboToddlerNutritionProblemCopyCtl" Width="450px" runat="server" />
            </td>
        </tr>
    </table>
</div>
