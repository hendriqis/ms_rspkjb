<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExtendOrderDurationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ExtendOrderDurationCtl" %>
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
        setDatePicker('<%=txtDefaultStartDate.ClientID %>');
        $('#<%=txtDefaultStartDate.ClientID %>').datepicker('option', 'minDate', '0');

        SetDefaultStartDate();

        $('#lblPhysicianNoteID').removeClass('lblLink');

        registerCollapseExpandHandler();
    });

    function SetDefaultStartDate() {
        var startDate = new Date();
        var endDate = Methods.getDatePickerDate($('#<%=txtDefaultStartDate.ClientID %>').val());
        startDate.setDate(endDate.getDate() + 1);
        var startDateText = Methods.dateToDatePickerFormat(startDate);

        $('.txtStartDate').each(function () {
            setDatePickerElement($(this));
            $(this).datepicker('option', 'minDate', '0');
            $(this).val(startDateText);
        });
    }

    $('#lblPhysicianNoteID.lblLink').live('click', function () {
        var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND GCPatientNoteType IN ('X011^010','X011^011','X011^012') AND IsNeedNotification = 1";
        openSearchDialog('planningNote', filterExpression, function (value) {
            onTxtPlanningNoteChanged(value);
        });
    });

    function onTxtPlanningNoteChanged(value) {
        var filterExpression = "ID = " + value;
        Methods.getObject('GetPatientVisitNoteList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPlanningNoteID.ClientID %>').val(result.ID);
                $('#<%=txtPatientVisitNoteText.ClientID %>').val(result.NoteText);
            }
            else {
                $('#<%=hdnPlanningNoteID.ClientID %>').val('');
                $('#<%=txtPatientVisitNoteText.ClientID %>').val('');
            }
        });
    }

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsProcessItem').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });

        if (isChecked) {
            $('.txtDuration').each(function () {
                $(this).val($('#<%=txtDefaultDuration.ClientID %>').val());
            });
        }
    });

    $('.chkIsProcessItem input').live('change', function () {
        var $cell = $(this).closest("td");
        var $tr = $cell.closest('tr');
        var isChecked = $(this).is(":checked");
        
        if ($(this).is(':checked')) {
            $cell.addClass('highlight');
            $tr.find('.txtDuration').val($('#<%=txtDefaultDuration.ClientID %>').val());
        }
        else {
            $cell.removeClass('highlight');
        }
    });

    $('#<%=txtDefaultStartDate.ClientID %>').change(function (evt) {
        onRefreshGrid();
    });

    function getSelectedCheckbox() {
        var tempSelectedID = "";
        var tempSelectedStartDate = "";
        var tempSelectedDuration = "";
        var tempSelectedTime1 = "";
        var tempSelectedTime2 = "";
        var tempSelectedTime3 = "";
        var tempSelectedTime4 = "";
        var tempSelectedTime5 = "";
        var tempSelectedTime6 = "";

        $('.grdPrescriptionOrderDt .chkIsProcessItem input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var id = $(this).closest('tr').find('.keyField').html();
            var startDate = $tr.find('.txtStartDate').val();
            var duration = $tr.find('.txtDuration').val();
            var time1 = $tr.find('.txtMedicationTime1').val();
            var time2 = $tr.find('.txtMedicationTime2').val();
            var time3 = $tr.find('.txtMedicationTime3').val();
            var time4 = $tr.find('.txtMedicationTime4').val();
            var time5 = $tr.find('.txtMedicationTime5').val();
            var time6 = $tr.find('.txtMedicationTime6').val();

            if (tempSelectedID != "") {
                tempSelectedID += ",";
                tempSelectedStartDate += ",";
                tempSelectedDuration += ",";
                tempSelectedTime1 += ",";
                tempSelectedTime2 += ",";
                tempSelectedTime3 += ",";
                tempSelectedTime4 += ",";
                tempSelectedTime5 += ",";
                tempSelectedTime6 += ",";
            }
            tempSelectedID += id;
            tempSelectedStartDate += startDate;
            tempSelectedDuration += duration;
            tempSelectedTime1 += time1;
            tempSelectedTime2 += time2;
            tempSelectedTime3 += time3;
            tempSelectedTime4 += time4;
            tempSelectedTime5 += time5;
            tempSelectedTime6 += time6;
        });
        if (tempSelectedID != "") {
            $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
            $('#<%=hdnSelectedStartDate.ClientID %>').val(tempSelectedStartDate);
            $('#<%=hdnSelectedDuration.ClientID %>').val(tempSelectedDuration);
            $('#<%=hdnSelectedTime1.ClientID %>').val(tempSelectedTime1);
            $('#<%=hdnSelectedTime2.ClientID %>').val(tempSelectedTime2);
            $('#<%=hdnSelectedTime3.ClientID %>').val(tempSelectedTime3);
            $('#<%=hdnSelectedTime4.ClientID %>').val(tempSelectedTime4);
            $('#<%=hdnSelectedTime5.ClientID %>').val(tempSelectedTime5);
            $('#<%=hdnSelectedTime6.ClientID %>').val(tempSelectedTime6);

            return true;
        }
        else return false;
    }

    $('#btnMPEntryProcess').click(function () {
        if (!getSelectedCheckbox()) {
            displayErrorMessageBox("Extend Durasi Pemberian", 'Error Message : ' + "Please select the item to be process !");
        }
        else {
            var message = "Process the Extend Medication Order ?";
            displayConfirmationMessageBox("Extend Durasi Pemberian",message, function (result) {
                if (result) cbpPopupProcess.PerformCallback('process');
            });
        }
    });

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        var retval = s.cpRetval.split('|');
        if (param[0] == 'process') {
            if (param[1] == '0') {
                displayErrorMessageBox("Extend Durasi Pemberian", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
            }
            else {
                pcRightPanelContent.Hide();
                if (typeof onRefreshList == 'function') {
                    onRefreshList();
                    displayMessageBox("Extend Durasi Pemberian", param[2]);
                }
            }
        }
    }

    function onCboPhysicianInstructionSourceChanged(s) {
        if (s.GetValue() != null && s.GetValue().indexOf('^01') > -1) {
            $('#lblPhysicianNoteID').addClass('lblLink');
        }
        else {
            $('#lblPhysicianNoteID').removeClass('lblLink');
            $('#<%=hdnPlanningNoteID.ClientID %>').val('');
            $('#<%=txtPatientVisitNoteText.ClientID %>').val('');

        }
    }

    function oncbpExtendViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onRefreshGrid() {
        cbpExtendView.PerformCallback('refresh');
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
<div>
    <div>
        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
            <colgroup>
                <col style="width:40%" />
                <col style="width:60%" />
            </colgroup>
            <tr>
                <td style="vertical-align:top">
                    <table width="100%">
                        <colgroup>
                            <col width="200px" />
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Terakhir Pemberian")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDefaultStartDate" runat="server" Width="120px" CssClass="datepicker" />
                            </td>
                            <td />
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblNormal">
                                    <%=GetLabel("Penambahan Durasi")%></label>
                            </td>
                            <td style="vertical-align:top">
                                <asp:TextBox ID="txtDefaultDuration" runat="server" Width="60px" Text="1" CssClass="number" /> &nbsp; <%=GetLabel("hari")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan Integrasi")%></label>
                            </td>
                            <td style="vertical-align:top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                    <tr>
                                        <td><asp:CheckBox ID="chkIsGenerateCPPT" runat="server" Text=" CPPT" ToolTip="Generate CPPT" Checked="false" /></td>
                                        <td><asp:CheckBox ID="chkIsGenerateJournal" runat="server" Text=" Jurnal Farmasi" ToolTip="Generate Jurnal" Checked="true" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align:top">
                    <h4 class="h4collapsed">
                        <%=GetLabel("Instruksi Penambahan Durasi")%></h4>
                    <div class="containerTblEntryContent containerEntryPanel1">
                        <div style="position: relative;"> 
                            <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                <colgroup>
                                    <col width="200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Cara Pemberian Instruksi")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboPhysicianInstructionSource" ClientInstanceName="cboPhysicianInstructionSource"
                                        runat="server" Width="250px" >
                                            <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPhysicianInstructionSourceChanged(s); }" Init="function(s,e){ onCboPhysicianInstructionSourceChanged(s); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="vertical-align:top">
                                        <label class="lblLink" id="lblPhysicianNoteID">
                                            <%=GetLabel("Instruksi Perawat/Dokter")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPatientVisitNoteText" Width="100%" Height="100px" runat="server" TextMode="MultiLine" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="vertical-align:top">
                                        <label class="lblNormal"><%=GetLabel("Catatan Farmasi")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPharmacyNoteText" Width="100%" runat="server" TextMode="MultiLine" />
                                    </td>
                                </tr>
                                <tr>
                                    <td />
                                    <td>
                                       <asp:CheckBox ID="chkIsNeedConfirmation" runat="server" Checked = "false" Text=" perlu konfirmasi" /> 
                                    </td>
                                </tr>
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
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div style="height:400px; overflow:scroll;overflow-x: hidden;">
        <table class="tblContentArea" width="100%">
            <tr>
                <td>
                     <dxcp:ASPxCallbackPanel ID="cbpExtendView" runat="server" Width="100%" ClientInstanceName="cbpExtendView"
                        ShowLoadingPanel="false" OnCallback="cbpExtendView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpExtendViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwExtendView" OnItemDataBound="lvwExtendView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdPrescriptionOrderDt grdSelected" cellspacing="0" rules="all">
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
                                            <table id="tblView" runat="server" class="grdPrescriptionOrderDt grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th rowspan="2" class="keyField">&nbsp;</th>
                                                    <th rowspan="2" class="hiddenColumn" >&nbsp;</th>
                                                    <th rowspan="2" align="center" style="width:30px">
                                                        <input id="chkSelectAll" type="checkbox" />
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
                                                    <th colspan="6" align="center" style="width:160px">
                                                        <div>
                                                            <%=GetLabel("Waktu Pemberian") %></div>
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
                                                    <th style="width: 40px; text-align:center">
                                                        <div>
                                                            <%=GetLabel("1") %></div>
                                                    </th>
                                                    <th style="width: 40px; text-align:center">
                                                        <div>
                                                            <%=GetLabel("2") %></div>
                                                    </th>
                                                    <th style="width: 40px; text-align:center">
                                                        <div>
                                                            <%=GetLabel("3") %></div>
                                                    </th>
                                                    <th style="width: 40px; text-align:center">
                                                        <div>
                                                            <%=GetLabel("4") %></div>
                                                    </th>
                                                    <th style="width: 40px; text-align:center">
                                                        <div>
                                                            <%=GetLabel("5") %></div>
                                                    </th>
                                                    <th style="width: 40px; text-align:center">
                                                        <div>
                                                            <%=GetLabel("6") %></div>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField"><%#: Eval("PrescriptionOrderDetailID")%></td>
                                                <td align="center" style="width:30px"><asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsProcessItem" /></td>
                                                <td class="tdItemName">
                                                    <div>
                                                        <label class="lblItemName"><%#: Eval("DrugName")%></label>
                                                    </div>
                                                    <div style="color:blue;font-style:italic">
                                                        <label class="lblParamedicName"><%#: Eval("ParamedicName")%></label>
                                                    </div>
                                                </td>
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
                                                <td style="width: 40px;"><input type="text" class="txtMedicationTime1" CssClass="time" value="<%#:Eval("Sequence1Time") %>" style="width:60px; text-align:center" /></td>
                                                <td style="width: 40px;"><input type="text" class="txtMedicationTime2" CssClass="time" value="<%#:Eval("Sequence2Time") %>" style="width:60px; text-align:center" /></td>
                                                <td style="width: 40px;"><input type="text" class="txtMedicationTime3" CssClass="time" value="<%#:Eval("Sequence3Time") %>" style="width:60px; text-align:center" /></td>
                                                <td style="width: 40px;"><input type="text" class="txtMedicationTime4" CssClass="time" value="<%#:Eval("Sequence4Time") %>" style="width:60px; text-align:center" /></td>
                                                <td style="width: 40px;"><input type="text" class="txtMedicationTime5" CssClass="time" value="<%#:Eval("Sequence5Time") %>" style="width:60px; text-align:center" /></td>
                                                <td style="width: 40px;"><input type="text" class="txtMedicationTime6" CssClass="time" value="<%#:Eval("Sequence6Time") %>" style="width:60px; text-align:center" /></td>
                                                <td style="width:110px"><asp:TextBox ID="txtStartDate" Width="80px" runat="server" CssClass="txtStartDate datepicker" /></td>
                                                <td style="width:60px"><asp:TextBox ID="txtDuration" Width="60px" runat="server" CssClass="txtDuration number"/></td>
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
</div>
