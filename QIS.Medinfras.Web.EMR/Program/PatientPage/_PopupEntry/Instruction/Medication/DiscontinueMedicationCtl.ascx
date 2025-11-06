<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiscontinueMedicationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.DiscontinueMedicationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div class="toolbarArea">
    <ul>
        <li id="btnMPEntryProcess">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbProcess.png")%>' alt="" /><div>
                <%=GetLabel("Process")%></div>
        </li>
    </ul>
</div>
<style type="text/css">       
    .highlight    {  background-color:#FE5D15; color: White; }
</style>
<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {
        setDatePicker('<%=txtInstructionDate.ClientID %>');
        $('#<%=txtInstructionDate.ClientID %>').datepicker('option', 'minDate', '0');

        $('#lblInstructionText.lblLink').live('click', function () {
            RefreshInstructionText();
        });

        SetDefaultStartDate();
    });

    function RefreshInstructionText() {
        $('#<%=hdnGeneratedText.ClientID %>').val('');
        if (getSelectedCheckbox()) {
            var text = $('#<%=hdnGeneratedText.ClientID %>').val();
            $('#<%=txtInstructionText.ClientID %>').val(text);
        }
    }

    function SetDefaultStartDate() {
        var startDate = new Date();
        var endDate = Methods.getDatePickerDate($('#<%=txtInstructionDate.ClientID %>').val());
        startDate.setDate(endDate.getDate() + 1);
        var startDateText = Methods.dateToDatePickerFormat(startDate);

        $('.txtStartDate').each(function () {
            setDatePickerElement($(this));
            $(this).datepicker('option', 'minDate', '0');
            $(this).val(startDateText);
        });
    }

    $('#chkDiscontinueSelectAll').die('change');
    $('#chkDiscontinueSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsProcessItemDiscontinue').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });

        RefreshInstructionText();
    });

    $('.chkIsProcessItemDiscontinue input').live('change', function () {
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

    $('#<%=txtInstructionDate.ClientID %>').change(function (evt) {
        onRefreshGrid();
    });

    function getSelectedCheckbox() {
        var tempSelectedID = "";
        var tempSelectedStartDate = "";
        var tempSelectedTime1 = "";
        var tempSelectedTime2 = "";
        var tempSelectedTime3 = "";
        var tempSelectedTime4 = "";
        var tempSelectedTime5 = "";
        var tempSelectedTime6 = "";
        var title = "Discontinue/Stop Pengobatan: ";
        var generatedText = $('#<%=hdnGeneratedText.ClientID %>').val();

        $('.grdPrescriptionOrderDt1 .chkIsProcessItemDiscontinue input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var id = $(this).closest('tr').find('.keyField').html();
            var itemName = $tr.find('.lblItemName').html();
            var startDate = $tr.find('.txtStartDate').val();

            var text = itemName + ", Mulai Tanggal: " + startDate;

            if (generatedText != "")
                generatedText += "\n";
            else
                generatedText += (title + "\n\n");

            generatedText += text;
        });
        if (generatedText != "") {
            $('#<%=hdnGeneratedText.ClientID %>').val(generatedText)
            return true;
        }
        else return false;
    }

    $('#btnMPEntryProcess').click(function () {
        if (!getSelectedCheckbox()) {
            displayErrorMessageBox("Discontinue/Stop Pengobatan", 'Error Message : ' + "Belum ada item yang dipilih");
        }
        else {
            var message = $('#<%=hdnGeneratedText.ClientID %>').val();
            displayConfirmationMessageBox("Discontinue/Stop Pengobatan : Catatan Instruksi", message, function (result) {
                if (result) {
                    cbpPopupProcess.PerformCallback('process');
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
                displayErrorMessageBox("Discontinue/Stop Pengobatan", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
            }
            else {
                pcRightPanelContent.Hide();
                if (typeof onRefreshList == 'function') {
                    onRefreshList();
                    displayMessageBox("Discontinue/Stop Pengobatan", param[2]);
                }
            }
        }
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onRefreshGrid() {
        cbpView.PerformCallback('refresh');
    }

    function onCboPhysicianChanged(s) {
        if (s.GetValue() != null) {
            $('#<%=hdnParamedicID.ClientID %>').val(s.GetValue());
        }
        else
            $('#<%=hdnParamedicID.ClientID %>').val('');
    }
</script>
<input type="hidden" runat="server" id="hdnVisitID" value="" />
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnSelectedTime1" value="" />
<input type="hidden" runat="server" id="hdnSelectedTime2" value="" />
<input type="hidden" runat="server" id="hdnSelectedTime3" value="" />
<input type="hidden" runat="server" id="hdnSelectedTime4" value="" />
<input type="hidden" runat="server" id="hdnSelectedTime5" value="" />
<input type="hidden" runat="server" id="hdnSelectedTime6" value="" />
<input type="hidden" runat="server" id="hdnSelectedStartDate" value="" />
<input type="hidden" runat="server" id="hdnSelectedDuration" value="" />
<input type="hidden" runat="server" id="hdnDispensaryServiceUnitID" value="" />
<input type="hidden" runat="server" id="hdnLocationID" value="" />
<input type="hidden" runat="server" id="hdnPlanningNoteID" value="" />
<input type="hidden" runat="server" id="hdnParamedicID" value="" />
<input type="hidden" runat="server" id="hdnGeneratedText" value="" />
<div>
    <div>
        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
            <colgroup>
                <col />
            </colgroup>
            <tr>
                <td style="vertical-align:top">
                    <table width="100%">
                        <colgroup>
                            <col width="200px" />
                            <col width="145px" />
                            <col width="80px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Dokter")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPhysician" ClientInstanceName="cboPhysician" runat="server" Width="250px" >
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPhysicianChanged(s); }" Init="function(s,e){ onCboPhysicianChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Instruksi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInstructionDate" runat="server" Width="120px" CssClass="datepicker" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtInstructionTime" Width="80px" CssClass="time" runat="server"
                                    Style="text-align: center" />
                            </td>
                            <td />
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="vertical-align : top">
                    <div style="height:250px; overflow:scroll;overflow-x: hidden;">
                        <table class="tblContentArea" width="100%">
                            <tr>
                                <td>
                                     <dxcp:ASPxCallbackPanel ID="cbpDiscontinueView" runat="server" Width="100%" ClientInstanceName="cbpDiscontinueView"
                                        ShowLoadingPanel="false" OnCallback="cbpDiscontinueView_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpDiscontinueViewEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelDiscontinueContent1" runat="server">
                                                <asp:Panel runat="server" ID="pnlDiscontinueView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                                    position: relative; font-size: 0.95em;">
                                                    <asp:ListView runat="server" ID="lvwDiscontinueView" OnItemDataBound="lvwDiscontinueView_ItemDataBound">
                                                        <EmptyDataTemplate>
                                                            <table id="tblView" runat="server" class="grdPrescriptionOrderDt1 grdSelected" cellspacing="0" rules="all">
                                                                <tr>
                                                                    <th class="keyField" rowspan="2">&nbsp;</th>
                                                                    <th  align="left"><%=GetLabel("Drug Name")%></th>
                                                                </tr>
                                                                <tr class="trEmpty">
                                                                    <td colspan="2">
                                                                        <%=GetLabel("Tidak ada data penjadwalan obat")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </EmptyDataTemplate>
                                                        <LayoutTemplate>
                                                            <table id="tblView" runat="server" class="grdPrescriptionOrderDt1 grdSelected" cellspacing="0" rules="all">
                                                                <tr>
                                                                    <th rowspan="2" class="keyField">&nbsp;</th>
                                                                    <th rowspan="2" class="hiddenColumn" >&nbsp;</th>
                                                                    <th rowspan="2" align="center" style="width:30px">
                                                                        <input id="chkDiscontinueSelectAll" type="checkbox" />
                                                                    </th>
                                                                    <th rowspan="2" align="left">
                                                                        <div>
                                                                            <%=GetLabel("Nama Obat")%>
                                                                        <div>
                                                                    </th>
                                                                    <th colspan="5" align="center" style="width:200px">
                                                                        <div>
                                                                            <%=GetLabel("Aturan Pemberian")%></div>
                                                                    </th>
                                                                    <th rowspan="2" align="center" style="padding: 3px; width: 120px;">
                                                                        <div>
                                                                            <%=GetLabel("Mulai Stop")%></div>
                                                                    </th>
                                                                </tr>
                                                                <tr>
                                                                    <th style="width: 40px;">
                                                                        <div style="text-align:right">
                                                                            <%=GetLabel("Frekuensi") %></div>
                                                                    </th>
                                                                    <th style="width: 40px;text-align:left">
                                                                        <div>
                                                                            <%=GetLabel("Periode") %></div>
                                                                    </th>
                                                                    <th style="width: 40px;">
                                                                        <div style="text-align:right">
                                                                            <%=GetLabel("Dosis") %></div>
                                                                    </th>
                                                                    <th style="width: 50px;">
                                                                        <div style="text-align:left">
                                                                            <%=GetLabel("Satuan") %></div>
                                                                    </th>
                                                                    <th style="width: 80px;">
                                                                        <div style="text-align:left">
                                                                            <%=GetLabel("Rute") %></div>
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="keyField"><%#: Eval("PrescriptionOrderDetailID")%></td>
                                                                <td align="center" style="width:30px"><asp:CheckBox ID="chkIsProcessItemDiscontinue" runat="server" CssClass="chkIsProcessItemDiscontinue" Checked="true" /></td>
                                                                <td class="tdItemName"><label class="lblItemName"><%#: Eval("DrugName")%></label></td>
                                                                <td align="right" style="width: 40px;">
                                                                    <input type="text" class="txtFrequency number min" min = "1" validationgroup="mpDrugEntry"  value="<%#:Eval("Frequency") %>" style="width:100%" readonly="readonly"/>
                                                                </td>
                                                                <td align="left" style="width: 40px;text-align:left"><div><%#: Eval("DosingFrequency")%></div></td>
                                                                <td align="right">
                                                                    <input type="text" class="txtNumberOfDosage number" value="<%#:Eval("cfNumberOfDosage") %>" style="width:100%" readonly="readonly"/>
                                                                </td>
                                                                <td align="left">
                                                                    <div> <%#: Eval("DosingUnit")%></div>
                                                                </td>
                                                                <td class="tdRoute" style="width:80px"><label class="lblRoute"><%#: Eval("Route")%></label></td>
                                                                <td style="width:110px"><asp:TextBox ID="txtStartDate" Width="80px" runat="server" CssClass="txtStartDate datepicker" /></td>
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
                        <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
                            ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
                            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" style="vertical-align:top">
                    <div style="padding-bottom: 10px"><label id ="lblInstructionText" class="lblNormal lblLink"><%=GetLabel("Catatan Instruksi (Click to Refresh)")%></label></div>
                    <div><asp:TextBox ID="txtInstructionText" Width="100%" runat="server" TextMode="MultiLine" Height="120px" /></div>
                </td>
            </tr>
        </table>
    </div>
</div>
