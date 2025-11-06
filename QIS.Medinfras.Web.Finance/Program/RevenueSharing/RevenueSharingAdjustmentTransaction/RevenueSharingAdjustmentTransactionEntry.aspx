<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/ParamedicPage/MPBaseParamedicPageTrx.master"
    AutoEventWireup="true" CodeBehind="RevenueSharingAdjustmentTransactionEntry.aspx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingAdjustmentTransactionEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Program/RevenueSharing/RevenueSharingToolbarCtl.ascx" TagName="ToolbarCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnApprove" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><div>
            <%=GetLabel("Approve")%></div>
    </li>
    <li id="btnReopen" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Re-open")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setCustomToolbarVisibility();

            var gcTransactionStatus = $('#<%=hdnGCTransactionStatus.ClientID %>').val();
            if (gcTransactionStatus == "" || gcTransactionStatus == "X121^001") {
                setDatePicker('<%=txtRSAdjustmentDate.ClientID %>');
                setDatePicker('<%=txtTransactionDate.ClientID %>');
            }

            $('#btnEntryPopupCancel').live('click', function () {
                $('#containerPopupEntryData').hide();
            });

            $('#btnEntryPopupSave').live('click', function () {
                if (IsValid(null, 'fsEntryPopup', 'mpEntryPopup'))
                    cbpEntryPopupView.PerformCallback('save');
                return false;
            });
        }

        function setCustomToolbarVisibility() {
            var rsAdjustmentID = $('#<%=hdnRSAdjustmentID.ClientID %>').val();
            var gcTransactionStatus = $('#<%=hdnGCTransactionStatus.ClientID %>').val();
            var isAllowApprove = $('#<%=hdnIsAllowApprove.ClientID %>').val();

            if (rsAdjustmentID != "" && rsAdjustmentID != "0") {
                if (gcTransactionStatus == "X121^001") {
                    if (isAllowApprove == "1") {
                        $('#<%=btnApprove.ClientID %>').show();
                    }
                    else {
                        $('#<%=btnApprove.ClientID %>').hide();
                    }
                    $('#<%=btnReopen.ClientID %>').hide();
                } else if (gcTransactionStatus == "X121^003") {
                    $('#<%=btnApprove.ClientID %>').hide();
                    $('#<%=btnReopen.ClientID %>').show();
                } else {
                    $('#<%=btnApprove.ClientID %>').hide();
                    $('#<%=btnReopen.ClientID %>').hide();
                }
            }
            else {
                $('#<%=btnApprove.ClientID %>').hide();
                $('#<%=btnReopen.ClientID %>').hide();
            }
        }

        $('#<%=btnApprove.ClientID %>').live('click', function () {
            onCustomButtonClick('approve');
        });

        $('#<%=btnReopen.ClientID %>').live('click', function () {
            onCustomButtonClick('reopen');
        });

        function onAfterCustomClickSuccess(type, retval) {
            onLoadObject(retval);
        }

        //#region RS Adjustment No
        $('#lblRSAdjustmentNo.lblLink').live('click', function () {
            var filterExpression = "<%=GetFilterExpression() %>";
            openSearchDialog('transrevenuesharingadjustmenthd', filterExpression, function (value) {
                $('#<%=txtRSAdjustmentNo.ClientID %>').val(value);
                ontxtRSAdjustmentNoChanged(value);
            });
        });

        $('#<%=txtRSAdjustmentNo.ClientID %>').live('change', function () {
            ontxtRSAdjustmentNoChanged($(this).val());
        });

        function ontxtRSAdjustmentNoChanged(value) {
            onLoadObject(value);
        };
        //#endregion

        $('#<%=rblAdjustment.ClientID%>').live('change', function () {
            var AdjustmentGroup = $('#<%=rblAdjustment.ClientID %> input:checked').val();
            var adjGroupPlus = "<%=OnGetAdjustmentGroupPlus() %>";

            if (AdjustmentGroup == adjGroupPlus) {
                $('#trAdjustmentTypeAdd').show();
                $('#trAdjustmentTypeMin').hide();

                $('#lblEntryARInvoiceParamedic').hide();
            }
            else {
                $('#trAdjustmentTypeAdd').hide();
                $('#trAdjustmentTypeMin').show();

                $('#lblEntryARInvoiceParamedic').show();
            }

            cbpView.PerformCallback();
        });

        //#region Detail Process
        $('#lblEntryPopupAddData').live('click', function (evt) {
            var gcTransactionStatus = $('#<%=hdnGCTransactionStatus.ClientID %>').val();
            if (gcTransactionStatus == "X121^001" || gcTransactionStatus == "") {
                if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
                    $('#<%=hdnID.ClientID %>').val('');
                    $('#<%=txtAdjustmentAmount.ClientID %>').val('0').trigger('changeValue');
                    $('#<%=txtAdjustmentAmountBruto.ClientID %>').val('0').trigger('changeValue');
                    cboAdjustmentTypeAdd.SetValue('');
                    cboAdjustmentTypeMin.SetValue('');
                    $('#<%=rblAdjustment.ClientID%>').change();
                    $('#<%=chkRevenueSharingFee.ClientID %>').prop('checked', false);
                    $('#<%=txtRemarks.ClientID %>').val('');
                    $('#<%=hdnRevenueSharingID.ClientID %>').val('');
                    $('#<%=txtRevenueSharingCode.ClientID %>').val('');
                    $('#<%=txtRevenueSharingName.ClientID %>').val('');
                    $('#<%=txtRegistrationNo.ClientID %>').val('');
                    $('#<%=txtRegistrationDate.ClientID %>').val('');
                    $('#<%=txtDischargeDate.ClientID %>').val('');
                    $('#<%=txtReceiptNo.ClientID %>').val('');
                    $('#<%=txtInvoiceNo.ClientID %>').val('');
                    $('#<%=txtReferenceNo.ClientID %>').val('');
                    $('#<%=txtBusinessPartnerName.ClientID %>').val('');
                    $('#<%=txtMedicalNo.ClientID %>').val('');
                    $('#<%=txtPatientName.ClientID %>').val('');
                    $('#<%=txtTransactionNo.ClientID %>').val('');
                    $('#<%=txtTransactionDate.ClientID %>').val('');
                    $('#<%=txtItemName1.ClientID %>').val('');
                    $('#<%=txtChargedQty.ClientID %>').val('0');

                    $('#containerPopupEntryData').show();
                }
            } else {
                showToast('Failed', 'Transaksi tidak dapat diubah. Harap refresh halaman ini.');
            }
        });

        $('.imgDelete').live('click', function () {
            $tr = $(this).closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($tr);
                    $('#<%=hdnID.ClientID %>').val(entity.ID);
                    cbpEntryPopupView.PerformCallback('delete');
                }
            });
        });

        $('.imgEdit').live('click', function () {
            $tr = $(this).closest('tr');
            var entity = rowToObject($tr);

            var adjustmentGroup = $('#<%=rblAdjustment.ClientID %> input:checked').val();
            var adjGroupPlus = "<%=OnGetAdjustmentGroupPlus() %>";

            $('#<%=hdnID.ClientID %>').val(entity.ID);
            $('#containerPopupEntryData').show();

            if (adjustmentGroup == adjGroupPlus) {
                $('#trAdjustmentTypeAdd').show();
                $('#trAdjustmentTypeMin').hide();
                cboAdjustmentTypeAdd.SetValue(entity.GCRSAdjustmentType);
            }
            else {
                $('#trAdjustmentTypeAdd').hide();
                $('#trAdjustmentTypeMin').show();
                cboAdjustmentTypeMin.SetValue(entity.GCRSAdjustmentType);
            }
            $('#<%=txtAdjustmentAmountBruto.ClientID %>').val(entity.AdjustmentAmountBRUTO).trigger('changeValue');
            $('#<%=txtAdjustmentAmount.ClientID %>').val(entity.AdjustmentAmount).trigger('changeValue');
            if (entity.IsTaxed == 'True') {
                $('#<%=chkRevenueSharingFee.ClientID %>').prop('checked', true);
            }
            else {
                $('#<%=chkRevenueSharingFee.ClientID %>').prop('checked', false);
            }
            $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
            $('#<%=hdnRevenueSharingID.ClientID %>').val(entity.RevenueSharingID);
            $('#<%=txtRevenueSharingCode.ClientID %>').val(entity.RevenueSharingCode);
            $('#<%=txtRevenueSharingName.ClientID %>').val(entity.RevenueSharingName);
            $('#<%=txtRegistrationNo.ClientID %>').val(entity.RegistrationNo);
            $('#<%=txtRegistrationDate.ClientID %>').val(entity.RegistrationDate);
            $('#<%=txtDischargeDate.ClientID %>').val(entity.DischargeDate);
            $('#<%=txtReceiptNo.ClientID %>').val(entity.ReceiptNo);
            $('#<%=txtInvoiceNo.ClientID %>').val(entity.InvoiceNo);
            $('#<%=txtReferenceNo.ClientID %>').val(entity.ReferenceNo);
            $('#<%=txtBusinessPartnerName.ClientID %>').val(entity.BusinessPartnerName);
            $('#<%=txtMedicalNo.ClientID %>').val(entity.MedicalNo);
            $('#<%=txtPatientName.ClientID %>').val(entity.PatientName);
            $('#<%=txtTransactionNo.ClientID %>').val(entity.TransactionNo);
            $('#<%=txtTransactionDate.ClientID %>').val(entity.cfTransactionDateInDatePickerFormat);
            $('#<%=txtItemName1.ClientID %>').val(entity.ItemName1);
            $('#<%=txtChargedQty.ClientID %>').val(entity.ChargedQty).trigger('changeValue');
        });

        //#region RevenueSharing
        $('#<%=lblRevenueSharingDt.ClientID %>').live('click', function () {
            var filterExpression = "IsDeleted = 0";
            openSearchDialog('revenuesharing', filterExpression, function (value) {
                $('#<%=txtRevenueSharingCode.ClientID %>').val(value);
                ontxtRevenueSharingCodeChanged(value);
            });
        });

        $('#<%=txtRevenueSharingCode.ClientID %>').live('change', function () {
            ontxtRevenueSharingCodeChanged($(this).val());
        });

        function ontxtRevenueSharingCodeChanged(value) {
            var filterExpression = "IsDeleted = 0 AND RevenueSharingCode = '" + value + "'";
            Methods.getObject('GetRevenueSharingHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                    $('#<%=txtRevenueSharingName.ClientID %>').val(result.RevenueSharingName);
                }
                else {
                    $('#<%=hdnRevenueSharingID.ClientID %>').val('');
                    $('#<%=txtRevenueSharingCode.ClientID %>').val('');
                    $('#<%=txtRevenueSharingName.ClientID %>').val('');
                }
            });
        };
        //#endregion

        //#endregion

        function onCbpEntryPopupViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    $('#containerPopupEntryData').hide();
                    if ($('#<%=txtRSAdjustmentNo.ClientID %>').val() == "") {
                        ontxtRSAdjustmentNoChanged(param[2]);
                    } else {
                        ontxtRSAdjustmentNoChanged($('#<%=txtRSAdjustmentNo.ClientID %>').val());
                    }
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    ontxtRSAdjustmentNoChanged($('#<%=txtRSAdjustmentNo.ClientID %>').val());
            }

            setCustomToolbarVisibility();
            hideLoadingPanel();
        };

    </script>
    <div>
        <input type="hidden" id="hdnPageTitle" runat="server" />
        <input type="hidden" id="hdnParamedicID" runat="server" />
        <input type="hidden" id="hdnIsEditable" runat="server" />
        <input type="hidden" id="hdnIsAllowApprove" runat="server" />
        <input type="hidden" id="hdnRSAdjustmentID" runat="server" />
        <input type="hidden" id="hdnRSAdjustmentNo" runat="server" />
        <input type="hidden" id="hdnGCTransactionStatus" runat="server" />
        <fieldset id="fsEntryPopup" style="margin: 0">
            <table class="tblContentArea" style="width: 100%">
                <colgroup>
                    <col style="width: 50%" />
                    <col style="width: 50%" />
                </colgroup>
                <tr>
                    <td>
                        <table>
                            <colgroup>
                                <col style="width: 150px" />
                            </colgroup>
                            <tr>
                                <td>
                                    <label id="lblRSAdjustmentNo" class="lblLink">
                                        <%=GetLabel("Nomor Penyesuaian")%></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtRSAdjustmentNo" Width="150px" Style="text-align: center" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal Penyesuaian")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRSAdjustmentDate" Width="120px" CssClass="datepicker" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px;">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="rblAdjustment" RepeatDirection="Horizontal" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="right" valign="top">
                        <table>
                            <colgroup>
                                <col style="width: 150px" />
                            </colgroup>
                            <tr>
                                <td style="width: 150px;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Total Adjustment Bruto") %></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtTotalAdjustmentAmountBruto" Width="250px" ReadOnly="true"
                                        Style="text-align: right" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Total Adjustment") %></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtTotalAdjustmentAmount" Width="250px" ReadOnly="true"
                                        Style="text-align: right" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                            <input type="hidden" id="hdnID" runat="server" value="" />
                            <div class="pageTitle">
                                <%=GetLabel("Tambah atau Edit Detail")%></div>
                            <table class="tblEntryDetail" style="width: 100%">
                                <colgroup>
                                    <col style="width: 40%" />
                                    <col style="width: 60%" />
                                </colgroup>
                                <tr>
                                    <td valign="top" align="left">
                                        <table>
                                            <colgroup>
                                                <col style="width: 170px" />
                                                <col />
                                            </colgroup>
                                            <tr style="display: none" id="trAdjustmentTypeAdd">
                                                <td>
                                                    <label class="lblMandatory" id="lblAdjustmentTypeAdd">
                                                        <%=GetLabel("Jenis Penambahan") %></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ClientInstanceName="cboAdjustmentTypeAdd" ID="cboAdjustmentTypeAdd"
                                                        Width="200px" />
                                                </td>
                                            </tr>
                                            <tr style="display: none" id="trAdjustmentTypeMin">
                                                <td>
                                                    <label class="lblMandatory" id="lblAdjustmentTypeMin">
                                                        <%=GetLabel("Jenis Pengurangan") %></label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ClientInstanceName="cboAdjustmentTypeMin" ID="cboAdjustmentTypeMin"
                                                        Width="200px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkRevenueSharingFee" runat="server" /><%:GetLabel(" Diperhitungkan Pajak")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblMandatory" id="lblAdjustmentAmountBruto">
                                                        <%=GetLabel("Adjustment Amount Bruto") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAdjustmentAmountBruto" CssClass="txtCurrency"
                                                        Width="200px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblMandatory" id="lblAdjustmentAmount">
                                                        <%=GetLabel("Adjustment Amount") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtAdjustmentAmount" CssClass="txtCurrency" Width="200px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblMandatory" id="lblRemarks">
                                                        <%=GetLabel("Remark") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtRemarks" TextMode="MultiLine" Rows="2" Width="350px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' />&nbsp<input
                                                        type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Cancel")%>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top" align="left">
                                        <table>
                                            <colgroup>
                                                <col style="width: 200px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <input type="hidden" id="hdnRevenueSharingID" runat="server" />
                                                    <label class="lblLink lblRevenueSharingDt" id="lblRevenueSharingDt" runat="server">
                                                        <%=GetLabel("RevenueSharing") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtRevenueSharingCode" Width="150px" />
                                                    <asp:TextBox runat="server" ID="txtRevenueSharingName" Width="300px" ReadOnly="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("RegNo | RegDate | DischDate") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtRegistrationNo" Width="150px" />
                                                    <asp:TextBox runat="server" ID="txtRegistrationDate" ToolTip="yyyyMMdd" Width="150px" />
                                                    <asp:TextBox runat="server" ID="txtDischargeDate" ToolTip="yyyyMMdd" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("ReceiptNo") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtReceiptNo" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("InvoiceNo") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtInvoiceNo" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("ReferenceNo") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtReferenceNo" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("BusinessPartnerName") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtBusinessPartnerName" Width="320px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Patient") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtMedicalNo" ToolTip="xx-xx-xx-xx" Width="150px" />
                                                    <asp:TextBox runat="server" ID="txtPatientName" Width="350px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("TransNo | TransDate") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtTransactionNo" Width="150px" />
                                                    <asp:TextBox runat="server" ID="txtTransactionDate" ToolTip="dd-MM-yyyy" CssClass="datepicker"
                                                        Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel("ItemName1 | ChargedQty") %></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtItemName1" Width="320px" />
                                                    <asp:TextBox runat="server" ID="txtChargedQty" CssClass="txtCurrency" Width="150px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr id="trCbpView">
                    <td colspan="2">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <input type="hidden" bindingfield="ID" value="<%#: Eval("ID") %>" />
                                                        <input type="hidden" bindingfield="GCRSAdjustmentType" value="<%#: Eval("GCRSAdjustmentType")%>" />
                                                        <input type="hidden" bindingfield="AdjustmentAmountBRUTO" value="<%#: Eval("AdjustmentAmountBRUTO")%>" />
                                                        <input type="hidden" bindingfield="AdjustmentAmount" value="<%#: Eval("AdjustmentAmount")%>" />
                                                        <input type="hidden" bindingfield="IsTaxed" value="<%#: Eval("IsTaxed")%>" />
                                                        <input type="hidden" bindingfield="Remarks" value="<%#: Eval("Remarks")%>" />
                                                        <input type="hidden" bindingfield="RevenueSharingID" value="<%#: Eval("RevenueSharingID")%>" />
                                                        <input type="hidden" bindingfield="RevenueSharingCode" value="<%#: Eval("RevenueSharingCode")%>" />
                                                        <input type="hidden" bindingfield="RevenueSharingName" value="<%#: Eval("RevenueSharingName")%>" />
                                                        <input type="hidden" bindingfield="RegistrationNo" value="<%#: Eval("RegistrationNo")%>" />
                                                        <input type="hidden" bindingfield="RegistrationDate" value="<%#: Eval("RegistrationDate")%>" />
                                                        <input type="hidden" bindingfield="DischargeDate" value="<%#: Eval("DischargeDate")%>" />
                                                        <input type="hidden" bindingfield="ReceiptNo" value="<%#: Eval("ReceiptNo")%>" />
                                                        <input type="hidden" bindingfield="InvoiceNo" value="<%#: Eval("InvoiceNo")%>" />
                                                        <input type="hidden" bindingfield="ReferenceNo" value="<%#: Eval("ReferenceNo")%>" />
                                                        <input type="hidden" bindingfield="BusinessPartnerName" value="<%#: Eval("BusinessPartnerName")%>" />
                                                        <input type="hidden" bindingfield="MedicalNo" value="<%#: Eval("MedicalNo")%>" />
                                                        <input type="hidden" bindingfield="PatientName" value="<%#: Eval("PatientName")%>" />
                                                        <input type="hidden" bindingfield="TransactionNo" value="<%#: Eval("TransactionNo")%>" />
                                                        <input type="hidden" bindingfield="cfTransactionDateInDatePickerFormat" value="<%#: Eval("cfTransactionDateInDatePickerFormat")%>" />
                                                        <input type="hidden" bindingfield="ItemName1" value="<%#: Eval("ItemName1")%>" />
                                                        <input type="hidden" bindingfield="ChargedQty" value="<%#: Eval("ChargedQty")%>" />
                                                        <img class="imgEdit <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>" src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                            alt="" style="float: left; margin-left: 7px" />
                                                        <img class="imgDelete <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>" src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                            alt="" style="float: left; margin-left: 7px" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:BoundField DataField="RSAdjustmentType" HeaderText="Adjustment Type" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="300px" />
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <div style="height: 100px; overflow-y: auto">
                                                            <table style="vertical-align: top">
                                                                <colgroup>
                                                                    <col style="width: 150px" />
                                                                    <col style="width: 10px" />
                                                                </colgroup>
                                                                <tr>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("Remark") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <b>
                                                                            <label class="lblNormal">
                                                                                <%#: Eval("Remarks")%></label></b>
                                                                    </td>
                                                                </tr>
                                                                <tr style='<%# Eval("RevenueSharingCode").ToString() == "" ? "display:none": "" %>'>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("RevenueSharing") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: small">
                                                                            <%=GetLabel("[") %><%#: Eval("RevenueSharingCode")%><%=GetLabel("] ") %>
                                                                            <%#: Eval("RevenueSharingName")%></label>
                                                                    </td>
                                                                </tr>
                                                                <tr style='<%# Eval("RegistrationNo").ToString() == "" ? "display:none": "" %>'>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("RegNo | RegDate | DischDate") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: small">
                                                                            <%#: Eval("RegistrationNo")%><%=GetLabel(" | ") %><%#: Eval("RegistrationDate")%><%=GetLabel(" | ") %><%#: Eval("DischargeDate")%></label>
                                                                    </td>
                                                                </tr>
                                                                <tr style='<%# Eval("ReceiptNo").ToString() == "" ? "display:none": "" %>'>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("ReceiptNo") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: small">
                                                                            <%#: Eval("ReceiptNo")%></label>
                                                                    </td>
                                                                </tr>
                                                                <tr style='<%# Eval("InvoiceNo").ToString() == "" ? "display:none": "" %>'>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("InvoiceNo") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: small">
                                                                            <%#: Eval("InvoiceNo")%></label>
                                                                    </td>
                                                                </tr>
                                                                <tr style='<%# Eval("ReferenceNo").ToString() == "" ? "display:none": "" %>'>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("ReferenceNo") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: small">
                                                                            <%#: Eval("ReferenceNo")%></label>
                                                                    </td>
                                                                </tr>
                                                                <tr style='<%# Eval("BusinessPartnerName").ToString() == "" ? "display:none": "" %>'>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("BusinessPartnerName") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: small">
                                                                            <%#: Eval("BusinessPartnerName")%></label>
                                                                    </td>
                                                                </tr>
                                                                <tr style='<%# Eval("PatientName").ToString() == "" ? "display:none": "" %>'>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("Patient") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: small">
                                                                            <%=GetLabel("[") %><%#: Eval("MedicalNo")%><%=GetLabel("] ") %><%#: Eval("PatientName")%></label>
                                                                    </td>
                                                                </tr>
                                                                <tr style='<%# Eval("TransactionNo").ToString() == "" ? "display:none": "" %>'>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("TransactionNo | TransactionDate") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: small">
                                                                            <%#: Eval("TransactionNo")%><%=GetLabel(" | ") %><%#: Eval("cfTransactionDateInString")%></label>
                                                                    </td>
                                                                </tr>
                                                                <tr style='<%# Eval("ItemName1").ToString() == "" ? "display:none": "" %>'>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel("ItemName1 | ChargedQty") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: x-small; font-style: italic">
                                                                            <%=GetLabel(":") %></label>
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblNormal" style="font-size: small">
                                                                            <%#: Eval("ItemName1")%><%=GetLabel(" | ") %><%#: Eval("cfChargedQtyInString")%></label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="AdjustmentAmountBRUTO" HeaderStyle-HorizontalAlign="Right"
                                                    ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="200px" DataFormatString="{0:N2}"
                                                    HeaderText="Adjustment Amount Bruto" />
                                                <asp:BoundField DataField="AdjustmentAmount" HeaderStyle-HorizontalAlign="Right"
                                                    ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="200px" DataFormatString="{0:N2}"
                                                    HeaderText="Adjustment Amount" />
                                                <asp:BoundField DataField="RSSummaryNo" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" HeaderText="No. Rekap JasMed" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("No Data To Display")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                            <span class="lblLink" id="lblEntryPopupAddData" style="margin-right: 100px;">
                                <%= GetLabel("Tambah Data")%></span>
                        </div>
                        <dxcp:ASPxCallbackPanel runat="server" ID="cbpEntryPopupView" ClientInstanceName="cbpEntryPopupView"
                            OnCallback="cbpEntryPopupView_Callback" ShowLoadingPanel="false">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpEntryPopupViewEndCallback(s); }" />
                        </dxcp:ASPxCallbackPanel>
                    </td>
                </tr>
            </table>
            <div>
                <table width="100%">
                    <tr>
                        <td>
                            <div style="width: 600px;">
                                <div class="pageTitle" style="text-align: center">
                                    <%=GetLabel("Informasi")%></div>
                                <div style="background-color: #EAEAEA;">
                                    <table width="600px" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col width="150px" />
                                            <col width="30px" />
                                        </colgroup>
                                        <tr>
                                            <td align="left">
                                                <%=GetLabel("Dibuat Oleh") %>
                                            </td>
                                            <td align="center">
                                                :
                                            </td>
                                            <td>
                                                <div runat="server" id="divCreatedBy">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <%=GetLabel("Dibuat Pada") %>
                                            </td>
                                            <td align="center">
                                                :
                                            </td>
                                            <td>
                                                <div runat="server" id="divCreatedDate">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trApprovedBy" runat="server">
                                            <td align="left">
                                                <%=GetLabel("Approved Oleh") %>
                                            </td>
                                            <td align="center">
                                                :
                                            </td>
                                            <td>
                                                <div runat="server" id="divApprovedBy">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="trApprovedDate" runat="server">
                                            <td align="left">
                                                <%=GetLabel("Approved Pada")%>
                                            </td>
                                            <td align="center">
                                                :
                                            </td>
                                            <td>
                                                <div runat="server" id="divApprovedDate">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <%=GetLabel("Terakhir Diubah Oleh") %>
                                            </td>
                                            <td align="center">
                                                :
                                            </td>
                                            <td>
                                                <div runat="server" id="divLastUpdatedBy">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <%=GetLabel("Terakhir Diubah Pada")%>
                                            </td>
                                            <td align="center">
                                                :
                                            </td>
                                            <td>
                                                <div runat="server" id="divLastUpdatedDate">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
    </div>
</asp:Content>
