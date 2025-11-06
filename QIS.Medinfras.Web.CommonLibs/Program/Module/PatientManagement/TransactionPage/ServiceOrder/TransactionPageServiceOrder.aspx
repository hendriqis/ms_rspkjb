<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master"
    AutoEventWireup="true" CodeBehind="TransactionPageServiceOrder.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransactionPageServiceOrder" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />
    <input type="hidden" value="" id="hdnCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnVoid" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        function onAfterSaveAddRecordEntryPopup(param) {
            var serviceOrderID = $('#<%=hdnServiceOrderID.ClientID %>').val();
            if (serviceOrderID == '' || serviceOrderID == '0')
                onAfterAddRecordAddRowCount();
            onLoadObject(param);
        }

        function onAfterSaveRecordDtSuccess(serviceOrderID) {
            if ($('#<%=hdnServiceOrderID.ClientID %>').val() == '0') {
                $('#<%=hdnServiceOrderID.ClientID %>').val(serviceOrderID);
                var filterExpression = 'ServiceOrderID = ' + serviceOrderID;
                Methods.getObject('GetServiceOrderHdList', filterExpression, function (result) {
                    $('#<%=txtServiceOrderNo.ClientID %>').val(result.ServiceOrderNo).trigger('change');
                });

                onAfterCustomSaveSuccess();
            }
        }

        function onLoad() {
            setCustomToolbarVisibility();

            setDatePicker('<%=txtServiceOrderDate.ClientID %>');

            //#region Transaction No
            $('#lblServiceOrderNo.lblLink').click(function () {
                var filterExpression = "<%:GetFilterExpressionServiceOrderHd() %>";
                openSearchDialog('serviceorderhd', filterExpression, function (value) {
                    $('#<%=txtServiceOrderNo.ClientID %>').val(value);
                    onTxtServiceOrderNoChanged(value);
                });
            });

            $('#<%=txtServiceOrderNo.ClientID %>').change(function () {
                onTxtServiceOrderNoChanged($(this).val());
            });

            function onTxtServiceOrderNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            //#region Physician
            $('#<%=lblPhysician.ClientID %>.lblLink').live('click', function () {
                var filterExpression = "";
                var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
                if (serviceUnitID != "" && serviceUnitID != "0")
                    filterExpression += "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + serviceUnitID + ")";
                if (filterExpression != "") filterExpression += " AND ";
                filterExpression += "IsDeleted = 0";
                openSearchDialog('paramedic', filterExpression, function (value) {
                    $('#<%=txtPhysicianCode.ClientID %>').val(value);
                    onTxtPhysicianCodeChanged(value);
                });
            });

            $('#<%=txtPhysicianCode.ClientID %>').change(function () {
                onTxtPhysicianCodeChanged($(this).val());
            });

            function onTxtPhysicianCodeChanged(value) {
                var filterExpression = "ParamedicCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                        $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                    }
                    else {
                        $('#<%=hdnPhysicianID.ClientID %>').val('');
                        $('#<%=txtPhysicianCode.ClientID %>').val('');
                        $('#<%=txtPhysicianName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Service Unit
            function getServiceUnitFilterFilterExpression() {
                var filterExpression = "IsUsingRegistration = 1 AND HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + $('#<%=hdnDepartmentIDTo.ClientID %>').val() + "'";
                return filterExpression;
            }
            $('#<%=lblSrvceUnit.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('serviceunitperhealthcare', getServiceUnitFilterFilterExpression(), function (value) {
                    $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                    onTxtServiceUnitCodeChanged(value);
                });
            });

            $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
                onTxtServiceUnitCodeChanged($(this).val());
            });

            function onTxtServiceUnitCodeChanged(value) {
                var filterExpression = getServiceUnitFilterFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
                Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                        $('#<%=hdnPhysicianID.ClientID %>').val('');
                        $('#<%=txtPhysicianCode.ClientID %>').val('');
                        $('#<%=txtPhysicianName.ClientID %>').val('');
                    }
                    else {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                        $('#<%=txtServiceUnitCode.ClientID %>').val('');
                        $('#<%=txtServiceUnitName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Item
            function getItemMasterFilterExpression() {
                var serviceOrderID = $('#<%=hdnServiceOrderID.ClientID %>').val();
                var filterExpression = '';
                if (serviceOrderID != '')
                    filterExpression = 'ItemID IN (SELECT ItemID FROM vServiceUnitItem WHERE HealthcareServiceUnitID = ' + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + ') AND ItemID NOT IN (SELECT ItemID FROM ServiceOrderDt WHERE ServiceOrderID = ' + $('#<%=hdnServiceOrderID.ClientID %>').val() + ' AND IsDeleted = 0) AND IsDeleted = 0';
                else
                    filterExpression = 'ItemID IN (SELECT ItemID FROM vServiceUnitItem WHERE HealthcareServiceUnitID = ' + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + ') AND IsDeleted = 0';

                filterExpression += " AND GCItemStatus != 'X181^999'";
                return filterExpression;
            }

            $('#lblItem.lblLink').click(function () {
                openSearchDialog('item', getItemMasterFilterExpression(), function (value) {
                    $('#<%=txtItemCode.ClientID %>').val(value);
                    onTxtItemCodeChanged(value);
                });
            });

            $('#<%=txtItemCode.ClientID %>').change(function () {
                onTxtItemCodeChanged($(this).val());
            });

            function onTxtItemCodeChanged(value) {
                var filterExpression = getItemMasterFilterExpression() + " AND ItemCode = '" + value + "'";
                Methods.getObject('GetItemMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                        $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                        $('#<%=hdnGCItemUnit.ClientID %>').val(result.GCItemUnit);
                    }
                    else {
                        $('#<%=hdnItemID.ClientID %>').val('');
                        $('#<%=txtItemCode.ClientID %>').val('');
                        $('#<%=txtItemName.ClientID %>').val('');
                        $('#<%=hdnGCItemUnit.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('#lblAddData').click(function (evt) {
                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    $('#<%=lblSrvceUnit.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=lblPhysician.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtPhysicianCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtServiceUnitCode.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=hdnEntryID.ClientID %>').val('');
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                    $('#<%=txtRemarks.ClientID %>').val('');
                    $('#containerEntry').show();
                }
            });

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup'))
                    cbpProcess.PerformCallback('save');
            });
        }


        $('#<%=grdView.ClientID %> .imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($row);
                    $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });

        $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
            $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
            $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
            $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
            $('#containerEntry').show();
        });

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    var serviceOrderID = s.cpServiceOrderID;
                    onAfterSaveRecordDtSuccess(serviceOrderID);
                    $('#containerEntry').hide();
                    cbpView.PerformCallback('refresh');
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else
                    cbpView.PerformCallback('refresh');
            }
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            if ($('#<%=txtServiceOrderNo.ClientID %>').val() == '')
                onAfterAddRecordAddRowCount();
            onLoadObject(param);
        }

        function setCustomToolbarVisibility() {
            var isVoid = $('#<%:hdnIsAllowVoid.ClientID %>').val();

            if (isVoid == 1) {
                if (getIsAdd()) {
                    $('#<%=btnVoid.ClientID %>').hide();
                }
                else {
                    if ($('#<%:hdnGCTransactionStatus.ClientID %>').val() == Constant.TransactionStatus.OPEN) {
                        $('#<%=btnVoid.ClientID %>').show();
                    } else {
                        $('#<%=btnVoid.ClientID %>').hide();
                    }
                }
            } else {
                $('#<%=btnVoid.ClientID %>').hide();
            }
        }

        $('#<%=btnVoid.ClientID %>').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                //                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/TransactionPage/Charges/ChargesVoidCtl.ascx');
                //                var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                //                var transactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();
                //                var id = registrationID + '|' + transactionID;
                //                openUserControlPopup(url, id, 'Void Transaction', 400, 230);
                showDeleteConfirmation(function (data) {
                    var param = 'void;' + data.GCDeleteReason + ';' + data.Reason;
                    onCustomButtonClick(param);
                });
            }
        });

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var ServiceOrderID = $('#<%=hdnServiceOrderID.ClientID %>').val();
            var TransactionStatus = $('#<%:hdnGCTransactionStatus.ClientID %>').val();
            var Department = $('#<%:hdnDepartmentIDTo.ClientID %>').val();
            if (ServiceOrderID != null && ServiceOrderID != "" && ServiceOrderID != "0") {
                if (code == 'PM-00531' || code == 'PM-00533' || code == 'PM-00542' || code == 'PM-00544' || code == 'PM-00554' || code == 'PM-00560' || code == 'PM-00561') {
                    if (TransactionStatus == Constant.TransactionStatus.OPEN) {
                        errMessage.text = 'Transaksi harus dipropose terlebih dahulu sebelum proses bisa dilakukan.';
                        return false;
                    } else {
                        filterExpression.text = 'ServiceOrderID = ' + ServiceOrderID;
                        return true;
                    }
                }
                else {
                    errMessage.text = 'Transaksi harus dipropose terlebih dahulu sebelum proses bisa dilakukan.';
                    return false;
                }
            }
            else {
                errMessage.text = 'Pilih nomor order terlebih dahulu.';
                return false;
            }

        }

        function onAfterCustomClickSuccess(type) {
            onRefreshControl();
        }

    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnServiceOrderID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentIDTo" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowVoid" runat="server" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblServiceOrderNo">
                                    <%=GetLabel("No Order")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtServiceOrderNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Referensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceNo" Width="150px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                                -
                                <%=GetLabel("Jam") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtServiceOrderDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtServiceOrderTime" Width="80px" CssClass="time" runat="server"
                                                Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label>
                                    <%=GetLabel("Catatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" Width="400px" TextMode="MultiLine" Height="90px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 180px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory" runat="server" id="Label1">
                                    <%=GetLabel("Dari Dokter / Paramedis")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboParamedicFrom" Width="100%">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr runat="server" id="trServiceUnit">
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblSrvceUnit">
                                    <%=GetLabel("Unit Pelayanan")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtServiceUnitName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" runat="server" id="lblPhysician">
                                    <%=GetLabel("Dokter / Paramedis")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtPhysicianCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPhysicianName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                        <div class="pageTitle">
                            <%=GetLabel("Entry")%></div>
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
                                                <col style="width: 100px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink lblMandatory" id="lblItem">
                                                        <%=GetLabel("Pelayanan")%></label>
                                                </td>
                                                <td colspan="2">
                                                    <input type="hidden" value="" id="hdnItemID" runat="server" />
                                                    <input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
                                                    <table cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 120px" />
                                                            <col style="width: 3px" />
                                                            <col style="width: 250px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtItemCode" Width="100%" runat="server" />
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
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Keterangan")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRemarks" Width="500px" runat="server" TextMode="MultiLine" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td colspan="2">
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
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <img class="imgEdit <%# IsEditable.ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                                    title='<%=GetLabel("Edit")%>' src='<%# IsEditable.ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                    alt="" />
                                                            </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <img class="imgDelete <%# IsEditable.ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                                    title='<%=GetLabel("Delete")%>' src='<%# IsEditable.ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                    alt="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                    <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                    <input type="hidden" value="<%#:Eval("GCServiceOrderStatus") %>" bindingfield="Remarks" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemName1" HeaderText="Pelayanan" HeaderStyle-Width="300px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ServiceOrderStatus" HeaderText="Status" HeaderStyle-Width="200px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div style="width: 100%; text-align: center">
                        <span class="lblLink" id="lblAddData" style="<%=IsEditable.ToString() == "False" ? "style='display:none'": "" %>">
                            <%= GetLabel("Add Data")%></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
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
                                                <tr id="trProposedBy" style="display: none" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Dipropose Oleh") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divProposedBy">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trProposedDate" style="display: none" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Dipropose Pada") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divProposedDate">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trVoidBy" style="display: none" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Divoid Oleh") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divVoidBy">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trVoidDate" style="display: none" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Divoid Pada") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divVoidDate">
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
                                                <tr id="trVoidReason" style="display: none" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Alasan Batal")%>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divVoidReason">
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
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
