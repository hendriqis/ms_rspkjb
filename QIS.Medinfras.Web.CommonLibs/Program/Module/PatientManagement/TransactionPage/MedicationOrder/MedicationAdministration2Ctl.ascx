<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicationAdministration2Ctl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.MedicationAdministration2Ctl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<style type="text/css">       
    .highlight    {  background-color:#FE5D15; color: White; }
    
    .druglist { font-weight: bold;}
</style>
<script type="text/javascript" id="dxss_medicationAdministration2Ctl">
    $(function () {

        setDatePicker('<%=txtMedicationDate.ClientID %>');
        $('#<%=txtMedicationDate.ClientID %>').datepicker('option', 'maxDate', 0);

        $('#<%=chkUseDefaultTime.ClientID %>').change(function () {
            var isChecked = $(this).is(":checked");
            if (isChecked)
                $('#<%=txtMedicationTime.ClientID %>').removeAttr("disabled");
            else
                $('#<%=txtMedicationTime.ClientID %>').attr("disabled", "disabled");
        });

        $('#<%=txtMedicationDate.ClientID %>').die('change');
        $('#<%=txtMedicationDate.ClientID %>').live('change', function (evt) {
            onRefreshGrid();
        });

        $('#<%=rblIsPatientFamily.ClientID %> input').change(function () {
            if ($(this).val() == "1") {
                $('#<%=trFamilyInfo.ClientID %>').removeAttr("style");
            }
            else {
                $('#<%=trFamilyInfo.ClientID %>').attr("style", "display:none");
            }
        });
    });

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsProcessItem').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });

        if (isChecked) {
            var isApplyDefaultTime = $('#<%=chkUseDefaultTime.ClientID %>').is(":checked");
            if (isApplyDefaultTime) {
                $('.txtTime').each(function () {
                    $(this).val($('#<%=txtMedicationTime.ClientID %>').val());
                });
            }

            $('.cboMedicationStatus').each(function () {
                $(this).val(cboDefaultMedicationStatus.GetValue());
            });

            $('.txtOtherReason').each(function () {
                $(this).val($('#<%=txtDefaultOtherMedicationStatus.ClientID %>').val());
            });
        }
    });

    $('.chkIsProcessItem input').live('change', function () {
        var $cell = $(this).closest("td");
        var $tr = $cell.closest('tr');
        var isChecked = $(this).is(":checked");
        if ($(this).is(':checked')) {
            $cell.addClass('highlight');
            var isApplyDefaultTime = $('#<%=chkUseDefaultTime.ClientID %>').is(":checked");
            if (isApplyDefaultTime) {
                $tr.find('.txtTime').val($('#<%=txtMedicationTime.ClientID %>').val());
            }
        }
        else {
            $cell.removeClass('highlight');
        }
    });
    function getSelectedCheckbox() {
        var tempSelectedID = "";
        var tempSelectedTime = "";
        var tempSelectedDrugName = "";
        var tempSelectedStatus = "";
        var tempOtherStatus = "";

        $('.grdPrescriptionOrderDt .chkIsProcessItem input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var id = $(this).closest('tr').find('.keyField').html();
            var date = $tr.find('.txtTime').val();
            //var drugName = $(this).closest('tr').find('.tdItemName').html();
            var reasonType = $tr.find('.cboMedicationStatus').val();
            var reasonRemark = $tr.find('.txtOtherReason').val();

            if (tempSelectedID != "") {
                tempSelectedID += ",";
                tempSelectedTime += ",";
                tempSelectedStatus += ",";
                tempOtherStatus += ",";
                //tempSelectedDrugName += "<br />";
            }
            tempSelectedID += id;
            tempSelectedTime += date;
            tempSelectedStatus += reasonType;
            tempOtherStatus += reasonRemark;
            //tempSelectedDrugName += drugName;

        });

        if (tempSelectedID != "") {
            $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
            $('#<%=hdnSelectedDate.ClientID %>').val(tempSelectedTime);
            $('#<%=hdnSelectedStatus.ClientID %>').val(tempSelectedStatus);
            $('#<%=hdnSelectedOtherStatus.ClientID %>').val(tempOtherStatus);
            $('#<%=hdnSelectedDrugName.ClientID %>').val(tempSelectedDrugName);

            return true;
        }
        else return false;
    }

    $('#btnMPEntryProcess').click(function () {
        if (!getSelectedCheckbox()) {
            displayErrorMessageBox("Medication Administration","Belum ada item obat yang dipilih untuk diproses !");
        }
        else {
            var message = "Lanjutkan proses update Catatan Pemberian Obat ke Pasien ?";
            displayConfirmationMessageBox("Medication Administration", message, function (result) {
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
                displayErrorMessageBox("Medication Administration", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
            }
            else {
                displayMessageBox('Medication Administration', param[2]);
                pcRightPanelContent.Hide();
                if (typeof onRefreshDetailList == 'function')
                    onRefreshDetailList();
            }
        }
    }

    function oncbpMedicalChartProcessViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onRefreshGrid() {
        cbpMedicalChartProcessView.PerformCallback('refresh');
    }

    function onCboDefaultMedicationStatusChanged(s) {
        $txt = $('#<%=txtDefaultOtherMedicationStatus.ClientID %>');
        if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1) {
            $txt.show();
            $txt.focus();
        }
        else
            $txt.hide();
    }

    function onRefreshGrid() {
        cbpMedicalChartProcessView.PerformCallback('refresh');
    }
</script>
<div class="toolbarArea processButton">
    <ul>
        <li id="btnMPEntryProcess">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbProcess.png")%>' alt="" /><div>
                <%=GetLabel("Process")%></div>
        </li>
    </ul>
</div>
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnSelectedDate" value="" />
<input type="hidden" runat="server" id="hdnSelectedStatus" value="" />
<input type="hidden" runat="server" id="hdnSelectedOtherStatus" value="" />
<input type="hidden" runat="server" id="hdnSelectedDrugName" value="" />
<input type="hidden" runat="server" id="hdnDispensaryServiceUnitID" value="" />
<input type="hidden" runat="server" id="hdnLocationID" value="" />
<input type="hidden" runat="server" id="hdnSelectedMRN" value="" />
<input type="hidden" runat="server" id="hdnRecordIDList" value="" />
<div>
    <table border="0" cellpadding="1" cellspacing="0">
        <colgroup>
            <col style="width:45%" />
            <col style="width:55%" />
        </colgroup>
        <tr>
           <td style="vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:150px"/>
                        <col style="width:150px"/>
                        <col style="width:100px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td colspan="4"><h4><%=GetLabel("INFORMASI PASIEN :")%></h4></td>                        
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel"><label class="lblNormal" runat="server" id="Label1"><%=GetLabel("Scan Barcode Gelang")%></label></td>
                        <td colspan="3"><asp:TextBox ID="txtBarcodeEntry1" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal" runat="server" id="lblMRN1"><%=GetLabel("No.RM")%></label></td>
                        <td><asp:TextBox ID="txtMRN1" Width="120px" runat="server" ReadOnly="true" /></td>
                        <td />
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Pasien")%></label></td>
                        <td colspan="3"><asp:TextBox ID="txtPatientName1" Width="100%" runat="server" ReadOnly="true" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Panggilan")%></label></td>
                        <td><asp:TextBox ID="txtPreferredName1" Width="100%" runat="server" ReadOnly="true" /></td>
                    </tr>
                        <td class="tdLabel" style="text-align:left; padding-right:5px"><label class="lblNormal"><%=GetLabel("Jenis Kelamin")%></label></td>
                        <td><asp:TextBox ID="txtGender1" Width="100%" runat="server" ReadOnly="true" /></td><tr>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tempat Lahir")%></label></td>
                        <td><asp:TextBox ID="txtBirthPlace1" Width="100%" runat="server" ReadOnly="true"/></td>
                        <td class="tdLabel" style="text-align:right; padding-right:5px"><label class="lblNormal"><%=GetLabel("Tanggal Lahir")%></label></td>
                        <td><asp:TextBox ID="txtDOB1" Width="100%" runat="server" ReadOnly="true" CssClass="datepicker"/></td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Alamat")%></label></td>
                        <td colspan="3"><asp:TextBox ID="txtAddress1" Width="100%" runat="server" TextMode="MultiLine" Rows="5" ReadOnly="true" /></td>
                    </tr>
                </table>
           </td>
           <td style="vertical-align:top">
                <table class="tblEntryContent">
                    <colgroup>
                        <col width="220px" />
                        <col width="150px" />
                        <col width="150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td colspan="4"><h4><%=GetLabel("INFORMASI PEMBERIAN OBAT :")%></h4></td>                        
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Pemberian")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMedicationDate" runat="server" Width="100px" CssClass="datepicker" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Sequence Pemberian")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboSequence" ClientInstanceName="cboSequence"
                            runat="server" Width="100%" >
                                <ClientSideEvents ValueChanged="function(s,e) { onRefreshGrid(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td />
                    </tr>
                    <tr>
                        <td colspan="4" style="font-weight:bold">Default Value untuk seluruh item (Checked All Item) :</td>                        
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Default Waktu Pemberian")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtMedicationTime" runat="server" Width="60px" Text="00:00" CssClass="time" Enabled="false"/>
                            <asp:CheckBox ID="chkUseDefaultTime" runat="server" Text=" Gunakan Waktu Default" Checked="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Medication Status")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboDefaultMedicationStatus" ClientInstanceName="cboDefaultMedicationStatus"
                            runat="server" Width="100%" >
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Other Status")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDefaultOtherMedicationStatus" runat="server" Width="100%" Text="" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Perawat")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboParamedic" ClientInstanceName="cboParamedic"
                            runat="server" Width="100%" >
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <asp:CheckBox ID="chkIsNeedConfirmation" runat="server" Checked = "false" /> <%:GetLabel("Konfirmasi Perawat")%>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboParamedic2" ClientInstanceName="cboParamedic2" runat="server" Width="100%" >
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Penerima Informasi")%></label>
                        </td>
                        <td colspan="3">
                            <asp:RadioButtonList ID="rblIsPatientFamily" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Pasien" Value="0" Selected="True" />
                                <asp:ListItem Text="Keluarga / Lain-lain" Value="1"  />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr id="trFamilyInfo" runat="server" style="display: none">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Penerima")%></label>
                        </td>
                        <td colspan="3">
                            <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                <colgroup>
                                    <col style="width:150px"/>
                                    <col style="width:100px"/>
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtPatientFamilyName" CssClass="txtPatientFamilyName" runat="server" Width="100%"  />
                                    </td>
                                    <td class="tdLabel" style="padding-left:5px">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Hubungan")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox runat="server" ID="cboFamilyRelation" ClientInstanceName="cboFamilyRelation"
                                            Width="99%" ToolTip = "Hubungan dengan Pasien" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>  
                </table>
           </td>
        </tr>
    </table>
</div>

<div id="divMedication">
    <div style="height:400px; overflow:scroll;overflow-x: hidden;">
        <table class="tblContentArea" width="100%">
            <tr>
                <td>
                     <dxcp:ASPxCallbackPanel ID="cbpMedicalChartProcessView" runat="server" Width="100%" ClientInstanceName="cbpMedicalChartProcessView"
                        ShowLoadingPanel="false" OnCallback="cbpMedicalChartProcessView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpMedicalChartProcessViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdPurchaseRequest grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th class="keyField" rowspan="2">&nbsp;</th>
                                                    <th  align="left"><%=GetLabel("Drug Name")%></th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="2" style="font-size:11pt;color:red; font-weight:bold">
                                                        <%=GetLabel("Tidak ada informasi obat untuk pasien ini")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdPrescriptionOrderDt grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th class="keyField">&nbsp;</th>
                                                    <th class="hiddenColumn" >&nbsp;</th>
                                                    <th align="center" style="width:30px">
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </th>
                                                    <th align="left">
                                                        <div>
                                                            <%=GetLabel("Drug Name")%>
                                                        <div>
                                                    </th>
                                                    <th align="center" style="width:60px"><%=GetLabel("Time")%></th>
                                                    <th align="left" style="padding: 3px; width: 200px;">
                                                        <div>
                                                            <%=GetLabel("Medication Status")%></div>
                                                    </th>
                                                    <th align="left" style="width:200px"><%=GetLabel("Other Status")%></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField"><%#: Eval("ID")%></td>
                                                <td align="center" style="width:30px"><asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsProcessItem" /></td>
                                                <td class="tdItemName"><%#: Eval("DrugName")%></td>
                                                <td style="width:60px"><asp:TextBox ID="txtTime" Width="60px" runat="server" CssClass="txtTime datepicker" /></td>
                                                <td style="width:200px">
                                                    <asp:DropDownList runat="server" ID="cboMedicationStatus" CssClass="cboMedicationStatus" Width="100%"></asp:DropDownList>
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
        <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
            ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</div>
