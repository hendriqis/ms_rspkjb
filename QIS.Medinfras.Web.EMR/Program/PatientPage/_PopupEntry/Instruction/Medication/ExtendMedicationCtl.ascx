<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExtendMedicationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.ExtendMedicationCtl" %>
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
        setDatePicker('<%=txtExtendInstructionDate.ClientID %>');
        $('#<%=txtExtendInstructionDate.ClientID %>').datepicker('option', 'minDate', '0');

        $('#lblExtendInstructionText.lblLink').live('click', function () {
            RefreshExtendInstructionText();
        });

        SetDefaultExtendStartDate();
    });

    function RefreshExtendInstructionText() {
        $('#<%=hdnGeneratedTextExtend.ClientID %>').val('');
        if (getExtendSelectedCheckbox()) {
            var text = $('#<%=hdnGeneratedTextExtend.ClientID %>').val();
            $('#<%=txtExtendInstructionText.ClientID %>').val(text);
        }
    }

    function SetDefaultExtendStartDate() {
        var startDate = new Date();
        var endDate = Methods.getDatePickerDate($('#<%=txtExtendInstructionDate.ClientID %>').val());
        startDate.setDate(endDate.getDate() + 1);
        var startDateText = Methods.dateToDatePickerFormat(startDate);

        $('.txtExtendStartDate').each(function () {
            setDatePickerElement($(this));
            $(this).datepicker('option', 'minDate', '0');
            $(this).val(startDateText);
        });
    }

    $('#chkExtendSelectAll').die('change');
    $('#chkExtendSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsProcessItemExtend').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });

        if (isChecked) {
            $('.txtExtendDuration').each(function () {
                $(this).val($('#<%=txtDefaultExtendDuration.ClientID %>').val());
            });
        }

        RefreshInstructionText();
    });

    $('.chkIsProcessItemExtend input').live('change', function () {
        var $cell = $(this).closest("td");
        var $tr = $cell.closest('tr');
        var isChecked = $(this).is(":checked");
        
        if ($(this).is(':checked')) {
            $cell.addClass('highlight');
            $tr.find('.txtExtendDuration').val($('#<%=txtDefaultExtendDuration.ClientID %>').val());
        }
        else {
            $cell.removeClass('highlight');
        }
    });

    $('#<%=txtExtendInstructionDate.ClientID %>').change(function (evt) {
        onRefreshExtendGrid();
    });

    function getExtendSelectedCheckbox() {
        var tempSelectedID = "";
        var tempSelectedStartDate = "";
        var tempSelectedDuration = "";
        var tempSelectedTime1 = "";
        var tempSelectedTime2 = "";
        var tempSelectedTime3 = "";
        var tempSelectedTime4 = "";
        var tempSelectedTime5 = "";
        var tempSelectedTime6 = "";
        var title = "Penambahan Durasi Pengobatan: ";
        var generatedText = "";

        $('.grdPrescriptionOrderDt2 .chkIsProcessItemExtend input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var id = $(this).closest('tr').find('.keyField').html();
            var itemName = $tr.find('.lblItemName').html();
            var startDate = $tr.find('.txtExtendStartDate').val();
            var duration = $tr.find('.txtExtendDuration').val();

            var text = itemName + ", Mulai Tanggal: " + startDate + ", Durasi: " + duration + " hari";

            if (generatedText != "")
                generatedText += "\n";
            else
                generatedText += (title + "\n\n");

            generatedText += text;
        });
        if (generatedText != "") {
            $('#<%=hdnGeneratedTextExtend.ClientID %>').val(generatedText)
            return true;
        }
        else return false;
    }

    $('#btnMPEntryProcess').click(function () {
        if (!getExtendSelectedCheckbox()) {
            displayErrorMessageBox("Penambahan Durasi Pemberian", 'Error Message : ' + "Item yang ditambahkan durasi pengobatan belum dipilih.");
        }
        else {
            var extendInstruction = $('#<%=txtExtendInstructionText.ClientID %>').val();
            var message = $('#<%=hdnGeneratedTextExtend.ClientID %>').val();
            if (extendInstruction != '') {
                displayConfirmationMessageBox("Penambahan Durasi Pemberian : Catatan Instruksi", message, function (result) {
                    if (result) {
                        cbpPopupProcessExtend.PerformCallback('process');
                    }
                });
            } else {
                displayErrorMessageBox("Penambahan Durasi Pemberian", 'Error Message : ' + "Catatan Instruksi harus diisi.");
            }
        }
    });


    function onCbpPopupProcesExtendEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        var retval = s.cpRetval.split('|');
        if (param[0] == 'process') {
            if (param[1] == '0') {
                displayErrorMessageBox("Penambahan Durasi Pemberian", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
            }
            else {
                pcRightPanelContent.Hide();
                if (typeof onRefreshList == 'function') {
                    onRefreshList();
                    displayMessageBox("Penambahan Durasi Pemberian", param[2]);
                }
            }
        }
    }

    function onCbpExtendViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onRefreshExtendGrid() {
        cbpViewExtend.PerformCallback('refresh');
    }

    function onCboExtendPhysicianChanged(s) {
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
<input type="hidden" runat="server" id="hdnGeneratedTextExtend" value="" />
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
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboExtendPhysician" ClientInstanceName="cboExtendPhysician" runat="server" Width="100%" >
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboExtendPhysicianChanged(s); }" Init="function(s,e){ onCboExtendPhysicianChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                             <td></td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Instruksi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtExtendInstructionDate" runat="server" Width="120px" CssClass="datepicker" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtExtendInstructionTime" Width="80px" CssClass="time" runat="server"
                                    Style="text-align: center" />
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblNormal">
                                    <%=GetLabel("Penambahan Durasi")%></label>
                            </td>
                            <td style="vertical-align:top">
                                <asp:TextBox ID="txtDefaultExtendDuration" runat="server" Width="60px" Text="1" CssClass="number" /> &nbsp; <%=GetLabel("hari")%>
                            </td>
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
                                     <dxcp:ASPxCallbackPanel ID="cbpViewExtend" runat="server" Width="100%" ClientInstanceName="cbpViewExtend"
                                        ShowLoadingPanel="false" OnCallback="cbpViewExtend_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewExtendEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelExtendContent1" runat="server">
                                                <asp:Panel runat="server" ID="pnlExtendView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                                    position: relative; font-size: 0.95em;">
                                                    <asp:ListView runat="server" ID="lvwExtendView" OnItemDataBound="lvwExtendView_ItemDataBound">
                                                        <EmptyDataTemplate>
                                                            <table id="tblExtendView" runat="server" class="grdPrescriptionOrderDt2 grdSelected" cellspacing="0" rules="all">
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
                                                            <table id="tblExtendView" runat="server" class="grdPrescriptionOrderDt2 grdSelected" cellspacing="0" rules="all">
                                                                <tr>
                                                                    <th rowspan="2" class="keyField">&nbsp;</th>
                                                                    <th rowspan="2" class="hiddenColumn" >&nbsp;</th>
                                                                    <th rowspan="2" align="center" style="width:30px">
                                                                        <input id="chkExtendSelectAll" type="checkbox" />
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
                                                                            <%=GetLabel("Mulai Extend")%></div>
                                                                    </th>
                                                                    <th rowspan="2" align="right" style="width:60px"><%=GetLabel("Duration")%></th>
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
                                                                <td align="center" style="width:30px"><asp:CheckBox ID="chkIsProcessItemExtend" runat="server" CssClass="chkIsProcessItemExtend" Checked="true" /></td>
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
                                                                <td style="width:110px"><asp:TextBox ID="txtExtendStartDate" Width="80px" runat="server" CssClass="txtExtendStartDate datepicker" /></td>
                                                                <td style="width:60px"><asp:TextBox ID="txtExtendDuration" Width="60px" runat="server" CssClass="txtExtendDuration number"/></td>
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
                        <dxcp:ASPxCallbackPanel ID="cbpPopupProcessExtend" runat="server" Width="100%" ClientInstanceName="cbpPopupProcessExtend"
                            ShowLoadingPanel="false" OnCallback="cbpPopupProcessExtend_Callback">
                            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesExtendEndCallback(s); }" />
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" style="vertical-align:top">
                    <div style="padding-bottom: 10px"><label id ="lblExtendInstructionText" class="lblNormal lblLink"><%=GetLabel("Catatan Instruksi (Click to Generate)")%></label></div>
                    <div><asp:TextBox ID="txtExtendInstructionText" Width="100%" runat="server" TextMode="MultiLine" Height="120px" /></div>
                </td>
            </tr>
        </table>
    </div>
</div>
