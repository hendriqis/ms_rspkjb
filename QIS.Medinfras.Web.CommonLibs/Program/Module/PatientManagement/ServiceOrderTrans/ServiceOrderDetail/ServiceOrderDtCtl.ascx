<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceOrderDtCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ServiceOrderDtCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_itemlaboratoryfractionentryctl">
    setDatePicker('<%=txtRealizationDate.ClientID %>');
    $('#<%=txtRealizationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

    $('#btnServiceOrderApprove').click(function (evt) {
        if (IsValid(evt, 'fsMPPopupEntry', 'mpEntry')) {
            var lstServiceOrderDtID = '';
            var lstServicePartnerID = '';
            var lstIsCito = '';
            $('#<%=grdView.ClientID %> .chkIsSelected input:checked').each(function () {
                $tr = $(this).closest('tr');
                var ServiceOrderDtID = $tr.find('.keyField').html();
                var ServicePartnerID = $tr.find('.hdnServicePartnerID').val();
                var isCito = 0;
                if ($tr.find('.chkIsCITO input').is(":checked")) {
                    isCito = 1;
                };
                //if (ServicePartnerID == "") ServicePartnerID = "0";
                if (lstServiceOrderDtID != '') {
                    lstServiceOrderDtID += ',';
                    lstServicePartnerID += ',';
                    lstIsCito += ',';
                }
                lstServiceOrderDtID += ServiceOrderDtID;
                lstServicePartnerID += ServicePartnerID;
                lstIsCito += isCito;
            });
            $('#<%=hdnListServiceOrderDtID.ClientID %>').val(lstServiceOrderDtID);
            $('#<%=hdnListServicePartnerID.ClientID %>').val(lstServicePartnerID);
            $('#<%=hdnListIsCito.ClientID %>').val(lstIsCito);
            cbpEntryPopupView.PerformCallback('approve');
        }
    });

    $('#btnServiceOrderDecline').click(function (evt) {
        if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
            var lstServiceOrderDtID = '';
            var lstServicePartnerID = '';
            var lstIsCito = '';
            $('#<%=grdView.ClientID %> .chkIsSelected input:checked').each(function () {
                $tr = $(this).closest('tr');
                var ServiceOrderDtID = $tr.find('.keyField').html();
                var ServicePartnerID = $tr.find('.hdnServicePartnerID').val();
                var isCito = 0;
                if ($tr.find('.chkIsCITO input').is(":checked")) {
                    isCito = 1;
                };
                //if (ServicePartnerID == "") ServicePartnerID = "0";
                if (lstServiceOrderDtID != '') {
                    lstServiceOrderDtID += ',';
                    lstServicePartnerID += ',';
                    lstIsCito += ',';
                }
                lstServiceOrderDtID += ServiceOrderDtID;
                lstServicePartnerID += ServicePartnerID;
                lstIsCito += isCito;
            });
            $('#<%=hdnListServiceOrderDtID.ClientID %>').val(lstServiceOrderDtID);
            $('#<%=hdnListServicePartnerID.ClientID %>').val(lstServicePartnerID);
            $('#<%=hdnListIsCito.ClientID %>').val(lstIsCito);
            cbpEntryPopupView.PerformCallback('decline');
        }
    });

    $('#btnServiceOrderClose').click(function (evt) {
        if ($('#<%=hdnTransactionID.ClientID %>').val() == '' || $('#<%=hdnTransactionID.ClientID %>').val() == '0') {
            showLoadingPanel();
            document.location = document.referrer;
        }
        else {
            pcRightPanelContent.Hide();
        }
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
                if (s.cpRetval != "") onLoadObject(s.cpRetval);
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

    //#region Service Partner
    $td = null;
    $('.lblServicePartner.lblLink').live('click', function () {
        $td = $(this).parent();
        openSearchDialog('Servicepartner', 'IsDeleted = 0', function (value) {
            onTxtServicePartnerChanged(value);
        });
    });

    function onTxtServicePartnerChanged(value) {
        var filterExpression = "BusinessPartnerCode = '" + value + "'";
        Methods.getObject('GetvServicePartnerList', filterExpression, function (result) {
            if (result != null) {
                $td.find('.hdnServicePartnerID').val(result.BusinessPartnerID);
                if (result.ShortName != "")
                    $td.find('.lblServicePartner').html(result.ShortName);
                else
                    $td.find('.lblServicePartner').html(result.BusinessPartnerName);
            }
            else {
                $td.find('.hdnServicePartnerID').val('0');
                $td.find('.lblServicePartner').html('');
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
<div style="height: 500px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnIsAllowVoid" runat="server" value="" />
    <input type="hidden" id="hdnLinkedChargesID" value="" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
    <input type="hidden" id="hdnVisitID" value="" runat="server" />
    <input type="hidden" id="hdnServiceOrderID" value="" runat="server" />
    <input type="hidden" id="hdnOrderDate" value="" runat="server" />
    <input type="hidden" id="hdnClassID" value="" runat="server" />
    <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnHSUImagingID" value="" runat="server" />
    <input type="hidden" id="hdnHSULaboratoryID" value="" runat="server" />
    <input type="hidden" id="hdnDefaultParamedicID" value="" runat="server" />
    <input type="hidden" id="Hidden1" value="" runat="server" />
    <input type="hidden" id="hdnListServiceOrderDtID" runat="server" />
    <input type="hidden" id="hdnListServicePartnerID" runat="server" />
    <input type="hidden" id="hdnListIsCito" runat="server" />
    <input type="hidden" id="hdnIsEndingAmountRoundingTo100" runat="server" value="0" />
    <input type="hidden" id="hdnIsEndingAmountRoundingTo1" runat="server" value="0" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <fieldset id="fsMPPopupEntry">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 130px" />
                            <col style="width: 170px" />
                            <col style="width: 70px" />
                            <col style="width: 250px" />
                            <col />
                        </colgroup>
                        <tr id="trTransactionNo" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblServiceOrderTransactionNo">
                                    <%=GetLabel("No. Order")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransactionNo" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr id="trDiorderOleh" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblUserOrder">
                                    <%=GetLabel("Diorder Oleh")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderUser" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblDokter">
                                    <%=GetLabel("Dokter")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtOrderPhysicianName" runat="server" Width="100%" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr id="trOrderDateTime" runat="server">
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                                /
                                <%=GetLabel("Jam Order") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtServiceOrderDate" runat="server" Style="text-align: center" ReadOnly="true" />
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtServiceOrderTime" CssClass="time" runat="server" Width="100px" Style="text-align: center"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label>
                                    <%=GetLabel("Catatan")%></label>
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtNotes" ReadOnly="true" Width="100%" TextMode="MultiLine" Height="90px"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr id="trTransactionDateTime" runat="server">
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                                /
                                <%=GetLabel("Jam Realisasi") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRealizationDate" runat="server" CssClass="datepicker" Style="text-align: center" />
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtRealizationTime" CssClass="time" runat="server" Width="100px" Style="text-align: center" />
                            </td>
                        </tr>
                        <tr id="trTransactionParamedic" runat="server">
                            <td colspan="6">
                                <table>
                                    <col style="width: 150px" />
                                    <col style="width: 120px" />
                                    <col />
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblLink lblMandatory" id="lblServiceOrderPhysician">
                                                <%=GetLabel("Dokter Pelaksana")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPhysicianCode" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPhysicianName" Width="300px" ReadOnly="true" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </fieldset>
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
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <input id="chkSelectAll" type="checkbox" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <div>
                                                    <%=GetLabel("Pelayanan") %></div>
                                                <div style="color: Blue">
                                                    <%=GetLabel("Diagnosa") %></div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("ItemName1")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Remarks" HeaderText="Catatan" HeaderStyle-Width="200px"
                                            HeaderStyle-VerticalAlign="Top" />
                                        <asp:BoundField DataField="ServiceOrderStatus" HeaderText="Status" HeaderStyle-Width="100px"
                                            HeaderStyle-VerticalAlign="Top" />
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
