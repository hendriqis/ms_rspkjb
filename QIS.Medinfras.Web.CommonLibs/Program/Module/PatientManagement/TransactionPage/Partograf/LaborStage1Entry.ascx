<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LaborStage1Entry.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.LaborStage1Entry" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_LaborStage1Entry">
    setDatePicker('<%=txtLaborStageDate.ClientID %>');
    $('#<%=txtLaborStageDate.ClientID %>').datepicker('option', 'maxDate', '0');

    $(function () {
        $('#<%=divFormContent.ClientID %>').html($('#<%=hdnDivHTML.ClientID %>').val());
        if ($('#<%=hdnFormValues.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnFormValues.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
                $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.chkNursingProblem').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
            }
        }
    });

    function onBeforeSaveRecord() {
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
        $('#<%=divFormContent.ClientID %>').find('.chkNursingProblem').each(function () {
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
        $('#<%=hdnFormValues.ClientID %>').val(controlValues);

        return controlValues;
    }

    function onAfterSaveAddRecordEntryPopup(result) {
        if (typeof onRefreshLaborStageGrid == 'function')
            onRefreshLaborStageGrid();
    }
    function onAfterSaveEditRecordEntryPopup(result) {
        if (typeof onRefreshLaborStageGrid == 'function')
            onRefreshLaborStageGrid();
    }
</script>
<div style="height: auto">
    <input type="hidden" runat="server" id="hdnAntenatalRecordID" value="" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnStage" value="1" />
    <input type="hidden" runat="server" id="hdnDivHTML" value="" />
    <input type="hidden" runat="server" id="hdnFormValues" value="" />

    <table class="tblEntryContent">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal ")%>
                                -
                                <%=GetLabel("Jam ")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLaborStageDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtLaborStageTime" Width="80px" CssClass="time" runat="server"
                                Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Bidan")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboParamedicID" Width="350px" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="tblContentArea" border="0">
        <tr>
            <td>
                <div id="divFormContent" runat="server" style="height: 450px;overflow-y: auto;"></div>
            </td>
        </tr>
    </table>
</div>
