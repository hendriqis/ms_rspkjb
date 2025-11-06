<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceOrderDtCtlX.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ServiceOrderDtCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_itemlaboratoryfractionentryctl">
    setDatePicker('<%=txtServiceOrderDate.ClientID %>');
    $('#btnServiceOrderApprove').click(function (evt) {
        if (IsValid(evt, 'fsMPPopupEntry', 'mpEntry')) {
            if ($('#<%=grdView.ClientID %> .chkIsSelected:visible input:checked').length < 1) {
                showToast('Warning', 'Please Select Confirmed Item First');
            }
            else {
                var param = '';
                $('#<%=grdView.ClientID %> .chkIsSelected input:checked').each(function () {
                    var itemRequestHdID = $(this).closest('tr').find('.keyField').html();
                    if (param != '')
                        param += ',';
                    param += itemRequestHdID;
                });
                $('#<%=hdnParam.ClientID %>').val(param);
                cbpEntryPopupView.PerformCallback('approve');
            }
        }
    });

    $('#btnServiceOrderDecline').click(function (evt) {
        if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
            if ($('#<%=grdView.ClientID %> .chkIsSelected input:checked').length < 1) {
                showToast('Warning', 'Please Select Cancel Item First');
            }
            else {
                var param = '';
                $('#<%=grdView.ClientID %> .chkIsSelected input:checked').each(function () {
                    var itemRequestHdID = $(this).closest('tr').find('.keyField').html();
                    if (param != '')
                        param += ',';
                    param += itemRequestHdID;
                });
                $('#<%=hdnParam.ClientID %>').val(param);
                cbpEntryPopupView.PerformCallback('decline');
            }
        }
    });

    $('#btnServiceOrderClose').click(function (evt) {
        cbpEntryPopupView.PerformCallback('close');
    });


    function onCbpEntryPopupViewEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'approve') {
            if (param[1] == 'fail')
                showToast('Approve Failed', 'Error Message : ' + param[2]);
            else {
                if ($('#<%=hdnTransactionID.ClientID %>').val() == '')
                    onAfterAddRecordAddRowCount();
                onLoadObject(s.cpRetval);
                pcRightPanelContent.Hide();
            }
        }
        else if (param[0] == 'decline') {
            if (param[1] == 'fail')
                showToast('Cancel Failed', 'Error Message : ' + param[2]);
        }
        else
            pcRightPanelContent.Hide();
    }

    function getServiceOrderTransactionFilterExpression() {
        var filterExpression = "<%:GetServiceOrderTransactionFilterExpression() %>";
        return filterExpression;
    }

    $('#lblServiceOrderTransactionNo.lblLink').click(function () {
        openSearchDialog('patientchargeshd', getServiceOrderTransactionFilterExpression(), function (value) {
            $('#<%=txtTransactionNo.ClientID %>').val(value);
            onTxtServiceOrderTransactionNoChanged(value);
        });
    });

    $('#<%=txtTransactionNo.ClientID %>').live('change', function () {
        onTxtServiceOrderTransactionNoChanged($(this).val());
    });

    function onTxtServiceOrderTransactionNoChanged(value) {
        var filterExpression = getServiceOrderTransactionFilterExpression() + " AND TransactionNo = '" + value + "'";
        Methods.getObject('GetPatientChargesHdList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnTransactionID.ClientID %>').val(result.TransactionID);
                $('#<%=txtServiceOrderDate.ClientID %>').val(result.TransactionDateInDatePickerFormat);
                $('#<%=txtServiceOrderTime.ClientID %>').val(result.TransactionTime);
            }
            else {
                $('#<%=hdnTransactionID.ClientID %>').val('');
                $('#<%=txtTransactionNo.ClientID %>').val('');
                $('#<%=txtServiceOrderDate.ClientID %>').val('');
                $('#<%=txtServiceOrderTime.ClientID %>').val('');
            }
        });
    }

    //#region Physician
    $('#lblServiceOrderPhysician.lblLink').live('click', function () {
        openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
            $('#<%=txtPhysicianCode.ClientID %>').val(value);
            onTxtServiceOrderPhysicianCodeChanged(value);
        });
    });

    $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
        onTxtServiceOrderPhysicianCodeChanged($(this).val());
    });

    function onTxtServiceOrderPhysicianCodeChanged(value) {
        var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
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

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });

    function onCboVoidReasonValueChanged(s) {
        if (s.GetValue() == 'X129^999')
            $('#<%=txtVoidReason.ClientID %>').show();
        else
            $('#<%=txtVoidReason.ClientID %>').hide();
    }
</script>
<div style="height: 440px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnLinkedChargesID" value="" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnVisitID" value="" runat="server" />
    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
    <input type="hidden" id="hdnServiceOrderID" value="" runat="server" />
    <input type="hidden" id="hdnOrderDate" value="" runat="server" />
    <input type="hidden" id="hdnClassID" value="" runat="server" />
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnHSUImagingID" value="" runat="server" />
    <input type="hidden" id="hdnHSULaboratoryID" value="" runat="server" />
    <input type="hidden" id="hdnIsHealthcareServiceUnitHasParamedic" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <fieldset id="fsMPPopupEntry">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 160px" />
                            <col />
                        </colgroup>
                        <tr id="trTransactionNo" runat="server">
                            <td class="tdLabel">
                                <label class="lblLink" id="lblServiceOrderTransactionNo">
                                    <%=GetLabel("No Order")%></label>
                            </td>
                            <td colspan="2">
                                <input type="hidden" id="hdnTransactionID" runat="server" value="" />
                                <asp:TextBox ID="txtTransactionNo" ReadOnly="true" Width="150px" runat="server" />
                            </td>
                        </tr>
                        <tr id="trTransactionDateTime" runat="server">
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                                /
                                <%=GetLabel("Jam") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtServiceOrderDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 3px">
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
                        <tr id="trTransactionParamedic" runat="server">
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" id="lblServiceOrderPhysician">
                                    <%=GetLabel("Dokter / Paramedis")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnPhysicianID" runat="server" value="" />
                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtPhysicianCode" Width="100%" runat="server" />
                                        </td>
                                        <td />
                                        <td>
                                            <asp:TextBox ID="txtPhysicianName" Width="100%" ReadOnly="true" runat="server" />
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
                                <asp:TextBox ID="txtNotes" ReadOnly="true" Width="100%" TextMode="MultiLine" Height="90px"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <input type="hidden" value="" id="hdnParam" runat="server" />
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField">
                                            <HeaderStyle CssClass="keyField"></HeaderStyle>
                                            <ItemStyle CssClass="keyField"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <input id="chkSelectAll" type="checkbox" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="40px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemCode" HeaderText="Kode Item" HeaderStyle-Width="100px">
                                            <HeaderStyle Width="100px"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-Width="200px">
                                            <HeaderStyle Width="200px"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Remarks" HeaderText="Catatan" HeaderStyle-Width="200px">
                                            <HeaderStyle Width="200px"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ServiceOrderStatus" HeaderText="Status" HeaderStyle-Width="100px">
                                            <HeaderStyle Width="100px"></HeaderStyle>
                                        </asp:BoundField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="trEmpty"></EmptyDataRowStyle>
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
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right; padding-top: 10px">
        <table id="tblApproveDecline" runat="server" width="100%">
            <tr>
                <td style="width: 80px;">
                    <input type="button" id="btnServiceOrderApprove" value='<%= GetLabel("Process")%>'
                        style="width: 100px" />
                </td>
                <td>
                    <table id="tblVoidReason" runat="server">
                        <tr>
                            <td style="width: 80px;">
                                <input type="button" id="btnServiceOrderDecline" value='<%= GetLabel("Void")%>' style="width: 100px" />
                            </td>
                            <td class="tdLabel">
                                <%=GetLabel("Alasan Batal") %>
                            </td>
                            <td style="padding-left: 10x">
                                <dxe:ASPxComboBox ID="cboVoidReason" runat="server" Width="200px">
                                    <ClientSideEvents Init="function(s,e){ onCboVoidReasonValueChanged(s); }" ValueChanged="function(s,e){ onCboVoidReasonValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVoidReason" runat="server" Width="150px" Style="display: none" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 80px;">
                    <input type="button" value='<%= GetLabel("Close")%>' id="btnServiceOrderClose" style="width: 100px" />
                </td>
            </tr>
        </table>
    </div>
</div>
