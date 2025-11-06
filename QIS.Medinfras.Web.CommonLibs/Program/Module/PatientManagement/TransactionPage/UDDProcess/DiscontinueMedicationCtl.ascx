<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiscontinueMedicationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.DiscontinueMedicationCtl" %>
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
    
    .druglist { font-weight: bold;}
</style>
<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {
        setDatePicker('<%=txtDefaultDiscontinueDate.ClientID %>');
        $('#<%=txtDefaultDiscontinueDate.ClientID %>').datepicker('option', 'minDate', '0');

        $('.txtStartDate').each(function () {
            setDatePickerElement($(this));
            $(this).datepicker('option', 'minDate', '0');
        });

        $('#lblPhysicianNoteID').removeClass('lblLink');

        registerCollapseExpandHandler();
    });

    $('#lblPhysicianNoteID.lblLink').live('click', function () {
        var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND (GCPatientNoteType IN ('X011^010','X011^011') OR (GCPatientNoteType = 'X011^012' AND IsNeedNotification = 1))";
        openSearchDialog('planningNote', filterExpression, function (value) {
            onTxtPlanningNoteChanged(value);
        });
    });

    function onTxtPlanningNoteChanged(value) {
        var filterExpression = "ID = " + value;
        Methods.getObject('GetvPatientVisitNoteList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPlanningNoteID.ClientID %>').val(result.ID);
                $('#<%=txtPatientVisitNoteText.ClientID %>').val(result.cfNoteSOAPI);
            }
            else {
                $('#<%=hdnPlanningNoteID.ClientID %>').val('');
                $('#<%=txtPatientVisitNoteText.ClientID %>').val('');
            }
        });
    }

    $('#chkSelectAll2').die('change');
    $('#chkSelectAll2').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsProcessItem2').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });

        if (isChecked) {
            $('.txtStartDate').each(function () {
                $(this).val($('#<%=txtDefaultDiscontinueDate.ClientID %>').val());
            });

            $('.cboDiscontinueReason').each(function () {
                $(this).val(cboDefaultGCDiscontinueReason.GetValue());
            });

            $('.txtOtherReason').each(function () {
                $(this).val($('#<%=txtDefaultOtherDiscontinueReason.ClientID %>').val());
            });
        }
    });

    $('.chkIsProcessItem2 input').live('change', function () {
        var $cell = $(this).closest("td");
        var $tr = $cell.closest('tr');
        var isChecked = $(this).is(":checked");
        if ($(this).is(':checked')) {
            $cell.addClass('highlight');
            $tr.find('.txtStartDate').val($('#<%=txtDefaultDiscontinueDate.ClientID %>').val());
        }
        else {
            $cell.removeClass('highlight');
        }
    });
    function getSelectedCheckbox() {
        var tempSelectedID = "";
        var tempSelectedPmID = "";
        var tempSelectedDate = "";
        var tempSelectedDrugName = "";
        var tempSelectedReasonType = "";
        var tempSelectedReasonRemark = "";

        $('.grdPrescriptionOrderDt .chkIsProcessItem2 input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var id = $(this).closest('tr').find('.keyField').html();
            var pmid = $(this).closest('tr').find('.hiddenColumn').html();
            var date = $tr.find('.txtStartDate').val();
            //var drugName = $(this).closest('tr').find('.tdItemName').html();
            var reasonType = $tr.find('.cboDiscontinueReason').val();
            var reasonRemark = $tr.find('.txtOtherReason').val();

            if (tempSelectedID != "") {
                tempSelectedID += ",";
                tempSelectedPmID += ",";
                tempSelectedDate += ",";
                tempSelectedReasonType += ",";
                tempSelectedReasonRemark += ",";
                //tempSelectedDrugName += "<br />";
            }
            tempSelectedID += id;
            tempSelectedPmID += pmid;
            tempSelectedDate += date;
            tempSelectedReasonType += reasonType;
            tempSelectedReasonRemark += reasonRemark;
            //tempSelectedDrugName += drugName;

        });

        if (tempSelectedID != "") {
            $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
            $('#<%=hdnSelectedPmID.ClientID %>').val(tempSelectedPmID);
            $('#<%=hdnSelectedDate.ClientID %>').val(tempSelectedDate);
            $('#<%=hdnSelectedReasonType.ClientID %>').val(tempSelectedReasonType);
            $('#<%=hdnSelectedReasonRemark.ClientID %>').val(tempSelectedReasonRemark);
            $('#<%=hdnSelectedDrugName.ClientID %>').val(tempSelectedDrugName);

            return true;
        }
        else return false;
    }

    $('#btnMPEntryProcess').click(function () {
        if (!getSelectedCheckbox()) {
            displayErrorMessageBox("Stop/Discontinue Pemberian", "Item Obat yang akan di-stop belum dipilih.");
        }
        else {
            var message = "Are you sure to discontinue the medication order ? <br/> <p style='font-weight:bold'>" + $('#<%=hdnSelectedDrugName.ClientID %>').val() + "</p> <br/>";
            //var message = "Are you sure to discontinue the medication order ?";
            displayConfirmationMessageBox("Stop/Discontinue Pemberian",message, function (result) {
                if (result) cbpPopupProcessDiscontinue.PerformCallback('process');
            });
        }
    });

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

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        var retval = s.cpRetval.split('|');
        if (param[0] == 'process') {
            if (param[1] == '0') {
                displayErrorMessageBox("Stop/Discontinue Pemberian", "<span style='color:red'>" + param[2] + "</span>");
            }
            else {
                displayMessageBox("Stop/Discontinue Pemberian", param[2]);
                pcRightPanelContent.Hide();
                if (typeof onRefreshDetailList == 'function')
                    onRefreshDetailList();
            }
        }
    }

    function oncbpDiscontinueViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onRefreshGrid() {
        cbpDiscontinueView.PerformCallback('refresh');
    }

    function onCboDiscountReasonChanged(s) {
        $txt = $('#<%=txtDefaultOtherDiscontinueReason.ClientID %>');
        if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1) {
            $txt.show();
            $txt.focus();
        }
        else
            $txt.hide();
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
<input type="hidden" runat="server" id="hdnSelectedDate" value="" />
<input type="hidden" runat="server" id="hdnSelectedReasonType" value="" />
<input type="hidden" runat="server" id="hdnSelectedReasonRemark" value="" />
<input type="hidden" runat="server" id="hdnSelectedDrugName" value="" />
<input type="hidden" runat="server" id="hdnSelectedPmID" value="" />
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
                            <col width="180px" />
                            <col  />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Dihentikan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDefaultDiscontinueDate" runat="server" Width="140px" CssClass="datepicker" />
                            </td>
                            <td></td>
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
                        <%=GetLabel("Instruksi Stop/Discontinue Pemberian")%></h4>
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
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Alasan Dihentikan")%></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboDefaultGCDiscontinueReason" ClientInstanceName="cboDefaultGCDiscontinueReason"
                                                            runat="server" Width="250px">
                                                            <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboDiscountReasonChanged(s); }"
                                                                Init="function(s,e){ onCboDiscountReasonChanged(s); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDefaultOtherDiscontinueReason" runat="server" Width="100%" Text="" />
                                                    </td>
                                                </td>
                                            </tr>
                                        </table>
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
                </td>
            </tr>
            <tr>
                <td>
                     <dxcp:ASPxCallbackPanel ID="cbpDiscontinueView" runat="server" Width="100%" ClientInstanceName="cbpDiscontinueView"
                        ShowLoadingPanel="false" OnCallback="cbpDiscontinueView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpDiscontinueViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelDiscontinueContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwDiscontinueView" OnItemDataBound="lvwDiscontinueView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblDiscontinueView" runat="server" class="grdPrescriptionOrderDt grdSelected" cellspacing="0" rules="all">
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
                                            <table id="tblDiscontinueView" runat="server" class="grdPrescriptionOrderDt grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th class="keyField">&nbsp;</th>
                                                    <th class="hiddenColumn" >&nbsp;</th>
                                                    <th align="center" style="width:30px">
                                                        <input id="chkSelectAll2" type="checkbox" />
                                                    </th>
                                                    <th align="left">
                                                        <div>
                                                            <%=GetLabel("Nama Obat")%>
                                                        <div>
                                                        <div>
                                                            <%=GetLabel("Diorder Oleh")%>
                                                        </div>
                                                    </th>
                                                    <th align="center" style="width:80px"><%=GetLabel("Mulai Pemberian")%></th>
                                                    <th align="center" style="width:110px"><%=GetLabel("Tanggal Dihentikan")%></th>
                                                    <th align="left" style="padding: 3px; width: 200px;">
                                                        <div>
                                                            <%=GetLabel("Alasan Dihentikan")%></div>
                                                    </th>
                                                    <th align="left" style="width:200px"><%=GetLabel("Keterangan Lain-lain")%></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField"><%#: Eval("PrescriptionOrderDetailID")%></td>
                                                <td class="hiddenColumn"><%#: Eval("PastMedicationID")%></td>
                                                <td align="center" style="width:30px"><asp:CheckBox ID="chkIsProcessItem2" runat="server" CssClass="chkIsProcessItem2" /></td>
                                                <td class="tdItemName">
                                                    <div>
                                                        <%#: Eval("DrugName")%>
                                                    </div>
                                                    <div style="color:blue;font-style:italic">
                                                        <%#: Eval("ParamedicName")%>
                                                    </div>
                                                </td>
                                                <td align="center" class="tdStartDate"><%#: Eval("cfStartDate")%></td>
                                                <td style="width:110px"><asp:TextBox ID="txtStartDate" Width="80px" runat="server" CssClass="txtStartDate datepicker" /></td>
                                                <td style="width:200px">
                                                    <asp:DropDownList runat="server" ID="cboDiscontinueReason" CssClass="cboDiscontinueReason" Width="100%"></asp:DropDownList>
                                                </td>
                                                <td style="width:200px"><input type="text" class="txtOtherReason" value="" style="width:100%"/></td>
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
        <dxcp:ASPxCallbackPanel ID="cbpPopupProcessDiscontinue" runat="server" Width="100%" ClientInstanceName="cbpPopupProcessDiscontinue"
            ShowLoadingPanel="false" OnCallback="cbpPopupProcessDiscontinue_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</div>
