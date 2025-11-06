<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeMedicationSignaCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.ChangeMedicationSignaCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div class="toolbarArea">
    <ul>
        <li id="btnMPChangeProcess">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbProcess.png")%>' alt="" /><div>
                <%=GetLabel("Process")%></div>
        </li>
    </ul>
</div>
<script type="text/javascript" id="dxss_ChangeMedicationSignaCtl">
    $(function () {
        setDatePicker('<%=txtChangeInstructionDate.ClientID %>');
        setDatePicker('<%=txtDefaultStartDate.ClientID %>');
        $('#<%=txtChangeInstructionDate.ClientID %>').datepicker('option', 'minDate', '0');
        $('#<%=txtDefaultStartDate.ClientID %>').datepicker('option', 'minDate', '0');

        $('#lblChangeInstructionText.lblLink').live('click', function () {
            RefreshChangeInstructionText();
        });

        SetDefaultChangeStartDate();
    });

    function RefreshChangeInstructionText() {
        $('#<%=hdnGeneratedChangeText.ClientID %>').val('');
        if (getSelectedChangeCheckbox()) {
            var text = $('#<%=hdnGeneratedChangeText.ClientID %>').val();
            $('#<%=txtChangeInstructionText.ClientID %>').val(text);
        }
    }

    function SetDefaultChangeStartDate() {
        var startDate = new Date();
        var endDate = Methods.getDatePickerDate($('#<%=txtDefaultStartDate.ClientID %>').val());
        startDate.setDate(endDate.getDate());
        var startDateText = Methods.dateToDatePickerFormat(startDate);

        $('.txtChangeStartDate').each(function () {
            setDatePickerElement($(this));
            $(this).datepicker('option', 'minDate', '0');
            $(this).val(startDateText);
        });
    }

    $('#chkChangeSelectAll').die('change');
    $('#chkChangeSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsProcessItemChange').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });

        if (isChecked) {
            $('.txtChangeStartDate').each(function () {
                $(this).val($('#<%=txtDefaultStartDate.ClientID %>').val());
            });
        }

        RefreshChangeInstructionText();
    });

    $('.chkIsProcessItemChange input').live('change', function () {
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

    $('#<%=txtChangeInstructionDate.ClientID %>').change(function (evt) {
        onRefreshGrid();
    });

    function getSelectedChangeCheckbox() {
        var tempSelectedID = "";
        var tempSelectedStartDate = "";
        var tempSelectedDuration = "";
        var tempSelectedTime1 = "";
        var tempSelectedTime2 = "";
        var tempSelectedTime3 = "";
        var tempSelectedTime4 = "";
        var tempSelectedTime5 = "";
        var tempSelectedTime6 = "";
        var title = "Perubahan Aturan Pemberian: ";
        var generatedText = "";

        $('.grdPrescriptionOrderDt3 .chkIsProcessItemChange input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var id = $(this).closest('tr').find('.keyField').html();
            var itemName = $tr.find('.lblItemName').html();
            var startDate = $tr.find('.txtChangeStartDate').val();
            var frequency = $tr.find('.txtFrequency').val();
            var numberOfDosage = $tr.find('.txtNumberOfDosage').val();
            var itemUnit = $tr.find('.lblItemUnit').html();

            var text = itemName + ", Mulai Tanggal: " + startDate + ", Aturan Pemberian : " + frequency + " x " + numberOfDosage + " " + itemUnit;

            if (generatedText != "")
                generatedText += "\n";
            else
                generatedText += (title + "\n\n");

            generatedText += text;
        });
        if (generatedText != "") {
            $('#<%=hdnGeneratedChangeText.ClientID %>').val(generatedText)
            return true;
        }
        else return false;
    }

    $('#btnMPChangeProcess').click(function () {
        if (!getSelectedChangeCheckbox()) {
            displayErrorMessageBox("Ubah Aturan Pemberian", 'Error Message : ' + "Item yang akan diproses untuk pembuatan instruksi ubah aturan pemberian belum dipilih.");
        }
        else {
            var message = $('#<%=hdnGeneratedChangeText.ClientID %>').val();
            displayConfirmationMessageBox("Extend Durasi Pemberian : Catatan Instruksi", message, function (result) {
                if (result) {
                    cbpPopupChangeProcess.PerformCallback('process');
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
                displayErrorMessageBox("Ubah Aturan Pemberian", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
            }
            else {
                pcRightPanelContent.Hide();
                if (typeof onRefreshList == 'function') {
                    onRefreshList();
                    displayMessageBox("Ubah Aturan Pemberian", param[2]);
                }
            }
        }
    }

    function onCbpViewChangeEndCallback(s) {
        hideLoadingPanel();
    }

    function onRefreshGridChange() {
        cbpViewChange.PerformCallback('refresh');
    }

    function oncboChangePhysicianChanged(s) {
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
<input type="hidden" runat="server" id="hdnGeneratedChangeText" value="" />
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
                                <dxe:ASPxComboBox ID="cboChangePhysician" ClientInstanceName="cboChangePhysician" runat="server" Width="250px" >
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ oncboChangePhysicianChanged(s); }" Init="function(s,e){ oncboChangePhysicianChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Instruksi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChangeInstructionDate" runat="server" Width="120px" CssClass="datepicker" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtChangeInstructionTime" Width="80px" CssClass="time" runat="server"
                                    Style="text-align: center" />
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Mulai Tanggal Berubah")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDefaultStartDate" runat="server" Width="120px" CssClass="datepicker" />
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
                                     <dxcp:ASPxCallbackPanel ID="cbpViewChange" runat="server" Width="100%" ClientInstanceName="cbpViewChange"
                                        ShowLoadingPanel="false" OnCallback="cbpViewChange_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewChangeEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelChangeContent1" runat="server">
                                                <asp:Panel runat="server" ID="pnlChangeView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                                    position: relative; font-size: 0.95em;">
                                                    <asp:ListView runat="server" ID="lvwChangeView" OnItemDataBound="lvwChangeView_ItemDataBound">
                                                        <EmptyDataTemplate>
                                                            <table id="tblView" runat="server" class="grdPrescriptionOrderDt3 grdSelected" cellspacing="0" rules="all">
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
                                                            <table id="tblView" runat="server" class="grdPrescriptionOrderDt3 grdSelected" cellspacing="0" rules="all">
                                                                <tr>
                                                                    <th rowspan="2" class="keyField">&nbsp;</th>
                                                                    <th rowspan="2" class="hiddenColumn" >&nbsp;</th>
                                                                    <th rowspan="2" align="center" style="width:30px">
                                                                        <input id="chkChangeSelectAll" type="checkbox" />
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
                                                                            <%=GetLabel("Mulai Tanggal")%></div>
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
                                                                <td align="center" style="width:30px"><asp:CheckBox ID="chkIsProcessItemChange" runat="server" CssClass="chkIsProcessItemChange" Checked="true" /></td>
                                                                <td class="tdItemName"><label class="lblItemName"><%#: Eval("DrugName")%></label></td>
                                                                <td align="right" style="width: 40px;">
                                                                    <input type="text" class="txtFrequency number min" min = "1" validationgroup="mpDrugEntry"  value="<%#:Eval("Frequency") %>" style="width:100%"/>
                                                                </td>
                                                                <td align="left" style="width: 40px;text-align:left"><div><%#: Eval("DosingFrequency")%></div></td>
                                                                <td align="right">
                                                                    <input type="text" class="txtNumberOfDosage number" value="<%#:Eval("cfNumberOfDosage") %>" style="width:100%"/>
                                                                </td>
                                                                <td align="left">
                                                                    <label class="lblItemUnit"> <%#: Eval("DosingUnit")%></label>
                                                                </td>
                                                                <td class="tdRoute" style="width:80px"><label class="lblRoute"><%#: Eval("Route")%></label></td>
                                                                <td style="width:110px"><asp:TextBox ID="txtChangeStartDate" Width="80px" runat="server" CssClass="txtChangeStartDate datepicker" /></td>
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
                        <dxcp:ASPxCallbackPanel ID="cbpPopupChangeProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupChangeProcess"
                            ShowLoadingPanel="false" OnCallback="cbpPopupChangeProcess_Callback">
                            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" style="vertical-align:top">
                    <div style="padding-bottom: 10px"><label id ="lblChangeInstructionText" class="lblNormal lblLink"><%=GetLabel("Catatan Instruksi (Click to Refresh)")%></label></div>
                    <div><asp:TextBox ID="txtChangeInstructionText" Width="100%" runat="server" TextMode="MultiLine" Height="120px" /></div>
                </td>
            </tr>
        </table>
    </div>
</div>
