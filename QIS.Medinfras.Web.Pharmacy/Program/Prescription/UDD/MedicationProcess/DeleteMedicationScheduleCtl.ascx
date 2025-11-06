<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeleteMedicationScheduleCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.DeleteMedicationScheduleCtl" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div class="toolbarArea">
    <ul>
        <li id="btnDelete">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdelete.png")%>' alt="" /><div>
                <%=GetLabel("Delete")%></div>
        </li>
    </ul>
</div>
<style type="text/css">
    .highlight
    {
        background-color: #FE5D15;
        color: White;
    }
</style>
<script type="text/javascript" id="dxss_DeleteMedicationScheduleCtl">

    function onTxtSearchViewSearchClick(s) {
        setTimeout(function () {
            s.SetBlur();
            onRefreshGrid();
        }, 0);
    }

    $('#chkDiscontinueSelectAll').die('change');
    $('#chkDiscontinueSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsProcessItemDeleted').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });

    $('.chkIsProcessItemDeleted input').live('change', function () {
        var $cell = $(this).closest("td");
        var $tr = $cell.closest('tr');
        var isChecked = $(this).is(":checked");

        if ($(this).is(':checked')) {
            $cell.addClass('highlight');
        }
        else {
            $cell.removeClass('highlight');
        }
    });

    function getSelectedCheckbox() {
        var tempSelectedID = "";

        var title = "Delete medication schedule : ";

        $('.grdPrescriptionOrderDt1 .chkIsProcessItemDeleted input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var id = $(this).closest('tr').find('.keyField').html();

            if (tempSelectedID != "")
                tempSelectedID += ",";

            tempSelectedID += id;
        });

        if (tempSelectedID != "") {
            $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
            return true;
        }
        else {
            return false;
        }
    }

    $('#btnDelete').live('click', function () {
        var deleteReason = $('#<%=txtDeleteReason.ClientID %>').val();
        if (deleteReason == "") {
            displayErrorMessageBox("Hapus Jadwal Pemberian", 'Error Message : ' + "Harap isi alasan dihapus terlebih dahulu");
        }
        else if (!getSelectedCheckbox()) {
            displayErrorMessageBox("Hapus Jadwal Pemberian", 'Error Message : ' + "Belum ada item yang dipilih");
        }
        else {
            displayConfirmationMessageBox("Hapus Jadwal Pemberian : ", "Lanjutkan proses hapus jadwal pemberian ?", function (result) {
                if (result) {
                    cbpDeleteScheduleProcess.PerformCallback('process');
                }
            });
        }
    });


    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        var retval = s.cpRetval.split('|');
        if (param[0] == 'process') {
            if (param[1] == '0') {
                displayErrorMessageBox("Hapus Jadwal Pemberian", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
            }
            else {
                pcRightPanelContent.Hide();
                if (typeof onRefreshList == 'function') {
                    onRefreshList();
                    displayMessageBox("Hapus Jadwal Pemberian", param[2]);
                }
            }
        }
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onRefreshGrid() {
        $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
        getSelectedCheckbox();
        cbpDeleteScheduleView.PerformCallback('refresh');
    }
</script>
<input type="hidden" runat="server" id="hdnVisitID" value="" />
<input type="hidden" runat="server" id="hdnPrescriptionOrderDtID" value="" />
<input type="hidden" runat="server" id="hdnPastMedicationID" value="" />
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnFilterExpressionQuickSearch" value="" />
<div>
    <div>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; padding-top: 10px;
            padding-bottom: 10px">
            <colgroup>
                <col width="150px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Alasan Dihapus")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtDeleteReason" runat="server" Width="75%" Text="" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr style="margin: 0 0 0 0;" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Quick Search")%></label>
                </td>
                <td>
                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                        Width="75%" Watermark="Search">
                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                        <IntellisenseHints>
                            <qis:QISIntellisenseHint Text="PrescriptionOrderNo" FieldName="PrescriptionOrderNo" />
                        </IntellisenseHints>
                    </qis:QISIntellisenseTextBox>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top" colspan="2">
                    <div style="height: 400px; overflow: scroll; overflow-x: hidden;">
                        <table class="tblContentArea" width="100%">
                            <tr>
                                <td>
                                    <dxcp:ASPxCallbackPanel ID="cbpDeleteScheduleView" runat="server" Width="100%" ClientInstanceName="cbpDeleteScheduleView"
                                        ShowLoadingPanel="false" OnCallback="cbpDeleteScheduleView_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelDiscontinueContent1" runat="server">
                                                <asp:Panel runat="server" ID="pnlDeleteScheduleView" Style="width: 100%; margin-left: auto;
                                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                                    <asp:ListView runat="server" ID="lvwDeleteScheduleView">
                                                        <EmptyDataTemplate>
                                                            <table id="tblView" runat="server" class="grdPrescriptionOrderDt1 grdSelected" cellspacing="0"
                                                                rules="all">
                                                                <tr>
                                                                    <th class="keyField" rowspan="2">
                                                                        &nbsp;
                                                                    </th>
                                                                    <th align="left" style="width: 150px">
                                                                        <div>
                                                                            <%=GetLabel("Prescription Order No")%>
                                                                            <div>
                                                                    </th>
                                                                    <th align="left">
                                                                        <div>
                                                                            <%=GetLabel("Drug Name")%>
                                                                            <div>
                                                                    </th>
                                                                    <th align="center" style="width: 120px; text-align: center">
                                                                        <div>
                                                                            <%=GetLabel("Medication Date")%>
                                                                            <div>
                                                                    </th>
                                                                    <th align="center" style="width: 120px; text-align: center">
                                                                        <div>
                                                                            <%=GetLabel("Medication Time")%>
                                                                            <div>
                                                                    </th>
                                                                    <th align="center" style="width: 80px; text-align: center">
                                                                        <div>
                                                                            <%=GetLabel("Sequence")%>
                                                                            <div>
                                                                    </th>
                                                                    <th align="center" style="width: 120px; text-align: center">
                                                                        <div>
                                                                            <%=GetLabel("Status")%></div>
                                                                    </th>
                                                                    <th align="center" style="width: 80px; text-align: center">
                                                                        <div>
                                                                            <%=GetLabel("Is UDD")%>
                                                                            <div>
                                                                    </th>
                                                                </tr>
                                                                <tr class="trEmpty">
                                                                    <td colspan="15">
                                                                        <%=GetLabel("No data to display")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </EmptyDataTemplate>
                                                        <LayoutTemplate>
                                                            <table id="tblView" runat="server" class="grdPrescriptionOrderDt1 grdSelected" cellspacing="0"
                                                                rules="all">
                                                                <tr>
                                                                    <th class="keyField">
                                                                        &nbsp;
                                                                    </th>
                                                                    <th align="center" style="width: 30px">
                                                                        <input id="chkDiscontinueSelectAll" type="checkbox" />
                                                                    </th>
                                                                    <th align="left" style="width: 150px">
                                                                        <div>
                                                                            <%=GetLabel("Prescription Order No")%>
                                                                            <div>
                                                                    </th>
                                                                    <th align="left">
                                                                        <div>
                                                                            <%=GetLabel("Drug Name")%>
                                                                            <div>
                                                                    </th>
                                                                    <th align="center" style="width: 120px; text-align: center">
                                                                        <div>
                                                                            <%=GetLabel("Medication Date")%>
                                                                            <div>
                                                                    </th>
                                                                    <th align="center" style="width: 120px; text-align: center">
                                                                        <div>
                                                                            <%=GetLabel("Medication Time")%>
                                                                            <div>
                                                                    </th>
                                                                    <th align="center" style="width: 80px; text-align: center">
                                                                        <div>
                                                                            <%=GetLabel("Sequence")%>
                                                                            <div>
                                                                    </th>
                                                                    <th align="center" style="width: 120px; text-align: center">
                                                                        <div>
                                                                            <%=GetLabel("Status")%></div>
                                                                    </th>
                                                                    <th align="center" style="width: 80px; text-align: center">
                                                                        <div>
                                                                            <%=GetLabel("Is UDD")%>
                                                                            <div>
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="keyField">
                                                                    <%#: Eval("ID")%>
                                                                </td>
                                                                <td align="center" style="width: 30px">
                                                                    <asp:CheckBox ID="chkIsProcessItemDeleted" runat="server" CssClass="chkIsProcessItemDeleted" />
                                                                </td>
                                                                <td>
                                                                    <label>
                                                                        <%#: Eval("PrescriptionOrderNo")%></label>
                                                                </td>
                                                                <td>
                                                                    <label>
                                                                        <%#: Eval("DrugName")%></label>
                                                                </td>
                                                                <td class="tdMedicationDate" align="center">
                                                                    <label>
                                                                        <%#: Eval("MedicationDateInString")%></label>
                                                                </td>
                                                                <td class="tdMedicationTime" align="center">
                                                                    <label>
                                                                        <%#: Eval("cfMedicationTime")%></label>
                                                                </td>
                                                                <td class="tdSequenceNo" align="center">
                                                                    <label>
                                                                        <%#: Eval("SequenceNo")%></label>
                                                                </td>
                                                                <td class="tdMedicationStatus" align="center">
                                                                    <label>
                                                                        <%#: Eval("MedicationStatus")%></label>
                                                                </td>
                                                                <td align="center">
                                                                    <label>
                                                                        <%#: Eval("IsUsingUDD")%></label>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:ListView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                </td>
                            </tr>
                        </table>
                        <dxcp:ASPxCallbackPanel ID="cbpDeleteScheduleProcess" runat="server" Width="100%"
                            ClientInstanceName="cbpDeleteScheduleProcess" ShowLoadingPanel="false" OnCallback="cbpDeleteScheduleProcess_Callback">
                            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>
