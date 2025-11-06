<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="EstimatedCharges.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.EstimatedCharges" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                $('#lblQuickPicks').show();
            }
            else {
                $('#lblQuickPicks').hide();
            }

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });

            if ($('#<%=hdnTransactionID.ClientID %>').val() == "" || $('#<%=hdnTransactionID.ClientID %>').val() == "0") {
                setDatePicker('<%=txtTransactionDate.ClientID %>');
            }
        }

        $(function () {
            //#region RegistraionNo
            function GetRegistrationFilter() {
                var filterExpression = "GCRegistrationStatus IN ('" + Constant.RegistrationStatus.CHECKED_IN + "','" + Constant.RegistrationStatus.RECEIVING_TREATMENT + "') AND DepartmentID = '" + Constant.Facility.INPATIENT + "'";
                return filterExpression;
            }

            $('#<%:lblRegNo.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('registration5', GetRegistrationFilter(), function (value) {
                    $('#<%=txtRegistrationNo.ClientID %>').val(value);
                    onTxtRegistrationNoChanged(value);
                });
            });

            $('#<%=txtRegistrationNo.ClientID %>').live('change', function () {
                onTxtRegistrationNoChanged($(this).val());
            });

            function onTxtRegistrationNoChanged(value) {
                var filterExpression = GetRegistrationFilter() + " AND RegistrationNo = '" + value + "'";
                Methods.getObject('GetvRegistration12List', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=hdnRegistrationID.ClientID %>').val(result.RegistrationID);
                        $('#<%=txtMedicalNo.ClientID %>').val(result.MedicalNo);
                        $('#<%=txtPatientName.ClientID %>').val(result.PatientName);
                        $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                    }
                    else {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                        $('#<%=hdnRegistrationID.ClientID %>').val('');
                        $('#<%=txtRegistrationNo.ClientID %>').val('');
                        $('#<%=txtMedicalNo.ClientID %>').val('');
                        $('#<%=txtPatientName.ClientID %>').val('');
                        $('#<%=txtServiceUnitName.ClientID %>').val('');
                    }
                });
                onCboCustomerTypeChanged();
            }
            //#endregion

            //#region TransactionNo
            $('#lblTransactionNo.lblLink').live('click', function () {
                var filterExpression = "TransactionNo != ''";
                openSearchDialog('estimatedcharges', filterExpression, function (value) {
                    $('#<%=txtTransactionNo.ClientID %>').val(value);
                    onTxtTransactionNoChanged(value);
                });
            });

            function onTxtTransactionNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            //#region AddData
            $('#lblQuickPicks').live('click', function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    var regID = $('#<%=hdnRegistrationID.ClientID %>').val();
                    var regNo = $('#<%=txtRegistrationNo.ClientID %>').val();
                    var custType = cboCustomerType.GetValue();
                    var classID = cboClass.GetValue();
                    var businessParnterID = $('#<%=hdnBusinessPartnerID.ClientID %>').val();
                    var coverageTypeID = $('#<%=hdnCoverageTypeID.ClientID %>').val();
                    var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
                    var transactionDate = $('#<%=txtTransactionDate.ClientID %>').val();
                    var transactionDate = $('#<%=txtTransactionDate.ClientID %>').val();
                    var hsuID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
                    var transactionNo = $('#<%=txtTransactionNo.ClientID %>').val();

                    var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/Transaction/EstimatedChargesQuickPicksCtl.ascx');
                    var width = 1200;
                    var id = transactionID + '|' + custType + '|' + businessParnterID + '|' + regID + '|' + classID + '|' + coverageTypeID + '|' + transactionDate + '|' + hsuID + '|' + transactionNo;
                    if (regNo != '') {
                        if (custType != 'X004^999') {
                            if (businessParnterID != '' && coverageTypeID != '') {
                                openUserControlPopup(url, id, 'Quick Picks', width, 600);
                            }
                            else {
                                displayErrorMessageBox('Silahkan Coba Lagi', "Silahkan pilih Instansi dan Tipe Jaminan terlebih dahulu.");
                            }
                        }
                        else {
                            openUserControlPopup(url, id, 'Quick Picks', width, 600);
                        }
                    }
                    else {
                        displayErrorMessageBox('Silahkan Coba Lagi', "Silahkan pilih Registrasi terlebih dahulu.");
                    }
                }
            });
            //#endregion

            //#region Business Partner
            function getBusinessPartnerFilterExpression() {
                var FilterExpression = "GCCustomerType = '" + cboCustomerType.GetValue() + "' AND IsDeleted=0";
                return FilterExpression;
            }

            $('#<%:lblBusinessPartner.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('payer', getBusinessPartnerFilterExpression(), function (value) {
                    $('#<%=txtBusinessPartnerCode.ClientID %>').val(value);
                    onTxtBusinessPartnerCodeChanged(value);
                })
            });

            $('#<%=txtBusinessPartnerCode.ClientID %>').live('change', function () {
                onTxtBusinessPartnerCodeChanged($(this).val());
            });

            function onTxtBusinessPartnerCodeChanged(value) {
                var filterExpression = getBusinessPartnerFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
                Methods.getObject('GetvCustomerList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnBusinessPartnerID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%=txtBusinessPartnerName.ClientID %>').val(result.BusinessPartnerName);
                    }
                    else {
                        $('#<%=hdnBusinessPartnerID.ClientID %>').val('');
                        $('#<%=txtBusinessPartnerCode.ClientID %>').val('');
                        $('#<%=txtBusinessPartnerName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Coverage Type
            function getCoverageTypeFilterExpression() {
                if ($('#<%=hdnBusinessPartnerID.ClientID %>').val() == "") {
                    var FilterExpression = "BusinessPartnerID = 0";
                    return FilterExpression;
                }
                else {
                    var FilterExpression = "BusinessPartnerID = " + $('#<%=hdnBusinessPartnerID.ClientID %>').val();
                    return FilterExpression;
                }
            }

            $('#<%:lblCoverageType.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('customercoverage', getCoverageTypeFilterExpression(), function (value) {
                    $('#<%=txtCoverageTypeCode.ClientID %>').val(value);
                    onTxtCoverageTypeCodeChanged(value);
                })
            });

            $('#<%=txtCoverageTypeCode.ClientID %>').live('change', function () {
                onTxtCoverageTypeCodeChanged($(this).val());
            });

            function onTxtCoverageTypeCodeChanged(value) {
                var filterExpression = getCoverageTypeFilterExpression() + " AND CoverageTypeCode = '" + value + "'";
                Methods.getObject('GetvContractCoverageList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                        $('#<%=txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                    }
                    else {
                        $('#<%=hdnCoverageTypeID.ClientID %>').val('');
                        $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                        $('#<%=txtCoverageTypeName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        });

        //#region edit and delete
        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($row);
                    $('#<%=hdnID.ClientID %>').val(entity.ID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });

        $('.imgEdit.imgLink').die('click');
        $('.imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnID.ClientID %>').val(entity.ID);
            $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
            $('#<%=txtQuantity.ClientID %>').val(entity.Qty);

            $('#containerEntry').show();
        });

        $('#<%=txtQuantity.ClientID %>').live('change', function () {
            var value = $(this).val();
            $(this).val(checkMinusDecimalOK(value)).trigger('changeValue');
        });

        $('#btnCancel').die('click');
        $('#btnCancel').live('click', function () {
            $('#containerEntry').hide();
        });

        $('#btnSave').die('click');
        $('#btnSave').live('click', function (evt) {            
            cbpProcess.PerformCallback('save');
        });
        //#endregion

        //#region Paging    
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');

            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                var totalPurchase = parseFloat(param[2]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail') {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }
                else {
                    $('#containerEntry').hide();
                    cbpView.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail') {
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                }
                else {
                    $('#containerEntry').hide();
                    cbpView.PerformCallback('refresh');
                }
            }
            $('#containerEntry').hide();
        }

        function onCboCustomerTypeChanged() {
            var custType = cboCustomerType.GetValue();
            if (custType == 'X004^999') {
                var filterExpression = "GCCustomerType = 'X004^999' AND IsDeleted = 0";
                Methods.getObject('GetvCustomerList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnBusinessPartnerID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%=txtBusinessPartnerCode.ClientID %>').val(result.BusinessPartnerCode);
                        $('#<%=txtBusinessPartnerName.ClientID %>').val(result.BusinessPartnerName);
                        $('#<%=hdnCoverageTypeID.ClientID %>').val('');
                        $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                        $('#<%=txtCoverageTypeName.ClientID %>').val('');
                    }
                    else {
                        $('#<%=hdnBusinessPartnerID.ClientID %>').val('');
                        $('#<%=txtBusinessPartnerCode.ClientID %>').val('');
                        $('#<%=txtBusinessPartnerName.ClientID %>').val('');
                        $('#<%=hdnCoverageTypeID.ClientID %>').val('');
                        $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                        $('#<%=txtCoverageTypeName.ClientID %>').val('');
                    }
                });
            }
            else {
                $('#<%=hdnBusinessPartnerID.ClientID %>').val('');
                $('#<%=txtBusinessPartnerCode.ClientID %>').val('');
                $('#<%=txtBusinessPartnerName.ClientID %>').val('');
                $('#<%=hdnCoverageTypeID.ClientID %>').val('');
                $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                $('#<%=txtCoverageTypeName.ClientID %>').val('');
            }
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            onLoadObject(param);
        }
    </script>
    <div style="padding: 10px;">
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" id="hdnTransactionID" runat="server" value="" />
        <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
        <input type="hidden" id="hdnOutPatientID" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" />
        <input type="hidden" id="hdnFilterItem" runat="server" />
        <input type="hidden" id="hdnGCItemType" runat="server" />
        <input type="hidden" value="" id="hdnPageCount" runat="server" />
        <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblTransactionNo" class="lblLink">
                                    <%=GetLabel("No. Transaksi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransactionNo" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <input type="hidden" id="hdnRegistrationID" runat="server" />
                                <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" />
                                <label id="lblRegNo" class="lblLink lblMandatory" runat="server">
                                    <%=GetLabel("No. Registrasi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRegistrationNo" Width="100%" CssClass="required" ValidationGroup="mpEntry"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Pasien")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtMedicalNo" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPatientName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Unit Pelayanan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr style='display: none'>
                            <td class="tdLabel">
                                <%=GetLabel("Jenis Pasien")%>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblJnsPasien" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow" ClientIDMode="Static">
                                    <asp:ListItem Text="RJ" Value="RJ" />
                                    <asp:ListItem Text="RI" Value="RI" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                            </td>
                            <td style="padding-right: 1px; width: 145px">
                                <asp:TextBox ID="txtTransactionDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Kelas Tagihan") %>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboClass" ClientInstanceName="cboClass" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblPenjamin" class="lblMandatory" runat="server">
                                    <%=GetLabel("Pembayar")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboCustomerType" ClientInstanceName="cboCustomerType" Width="100%"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboCustomerTypeChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal lblLink" id="lblBusinessPartner" runat="server">
                                    <%=GetLabel("Instansi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnBusinessPartnerID" runat="server" value="" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtBusinessPartnerCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBusinessPartnerName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal lblLink" id="lblCoverageType" runat="server">
                                    <%=GetLabel("Tipe Jaminan")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnCoverageTypeID" runat="server" value="" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtCoverageTypeCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCoverageTypeName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div id="containerEntry" style="margin-top: 4px; display: none;">
            <div class="pageTitle">
                <%=GetLabel("Edit")%></div>
            <fieldset id="fsTrxPopup" style="margin: 0">
                <input type="hidden" value="" id="hdnEntryID" runat="server" />
                <table style="width: 100%" class="tblEntryDetail">
                    <colgroup>
                        <col style="width: 50%" />
                    </colgroup>
                    <tr>
                        <td valign="top">
                            <table style="width: 100%">
                                <colgroup>
                                    <col style="width: 150px" />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Item")%></label>
                                    </td>
                                    <td colspan="2">
                                        <table cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 120px" />
                                                <col style="width: 3px" />
                                                <col style="width: 250px" />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtItemCode" ReadOnly="true" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Jumlah")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQuantity" Width="120px" CssClass="number" Min="0" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnSave" value='<%= GetLabel("Save")%>' />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnCancel" value='<%= GetLabel("Cancel")%>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div>
            <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                            position: relative; font-size: 0.95em;">
                            <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                OnRowDataBound="grdView_RowDataBound" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                EmptyDataRowStyle-CssClass="trEmpty">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:TemplateField HeaderStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <img class="imgEdit <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>" title='<%=GetLabel("Edit")%>'
                                                            src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                            alt="" />
                                                    </td>
                                                    <td style="width: 1px">
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <img class="imgDelete <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>" title='<%=GetLabel("Delete")%>'
                                                            src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                            alt="" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                            <input type="hidden" value="<%#:Eval("EstimatedChargesHdID") %>" bindingfield="EstimatedChargesHdID" />
                                            <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                            <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                            <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                            <input type="hidden" value="<%#:Eval("Qty") %>" bindingfield="Qty" />
                                            <input type="hidden" value="<%#:Eval("Tariff") %>" bindingfield="Tariff" />
                                            <input type="hidden" value="<%#:Eval("PatientAmount") %>" bindingfield="PatientAmount" />
                                            <input type="hidden" value="<%#:Eval("PayerAmount") %>" bindingfield="PayerAmount" />
                                            <input type="hidden" value="<%#:Eval("LineAmount") %>" bindingfield="LineAmount" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nama Barang" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="250px">
                                        <ItemTemplate>
                                            <div style="padding: 1px">
                                                <div>
                                                    <span style="font-style: normal; font-weight: bold">
                                                        <%#: Eval("ItemName1")%></div>
                                                <div>
                                                    <span style="font-style: italic;">
                                                        <%#: Eval("ItemCode")%></span></div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Qty" HeaderText="Quantity" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Right"
                                        ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                    <asp:BoundField DataField="Tariff" HeaderText="Harga / Satuan" HeaderStyle-Width="120px"
                                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                    <asp:BoundField DataField="PayerAmount" HeaderText="Instansi" HeaderStyle-Width="120px"
                                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                    <asp:BoundField DataField="PatientAmount" HeaderText="Pasien" HeaderStyle-Width="120px"
                                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                    <asp:BoundField DataField="LineAmount" HeaderText="Total" HeaderStyle-Width="120px"
                                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                    <asp:TemplateField HeaderText="Petugas" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="250px">
                                        <ItemTemplate>
                                            <div style="padding: 1px">
                                                <div>
                                                    <span style="font-style: normal;">
                                                        <%#: Eval("CreatedByFullName")%><br>
                                                        <span style="font-style: normal;">
                                                            <%#: Eval("cfCreatedDateInString")%></span></div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <%=GetLabel("Tidak ada data")%>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
        </div>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging">
                </div>
            </div>
        </div>
        <div style="width: 100%; text-align: center">
            <span class="lblLink" id="lblQuickPicks" style="margin-right: 200px;">
                <%= GetLabel("Quick Picks")%></span>
        </div>
    </div>
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
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
