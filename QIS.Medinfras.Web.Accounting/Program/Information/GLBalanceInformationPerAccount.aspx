<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="GLBalanceInformationPerAccount.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.GLBalanceInformationPerAccount" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            //#region GL Account 
            $('#lblGLAccount.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtGLAccountCode.ClientID %>').val(value);
                    onTxtGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtGLAccountCode.ClientID %>').change(function () {
                onTxtGLAccountCodeChanged($(this).val());
            });

            function onTxtGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnGLAccountID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtGLAccountName.ClientID %>').val(result.GLAccountName);

                        $('#<%=hdnSubLedgerID.ClientID %>').val(result.SubLedgerID);
                        $('#<%=hdnSearchDialogTypeName.ClientID %>').val(result.SearchDialogTypeName);
                        $('#<%=hdnFilterExpression.ClientID %>').val(result.FilterExpression);
                        $('#<%=hdnIDFieldName.ClientID %>').val(result.IDFieldName);
                        $('#<%=hdnCodeFieldName.ClientID %>').val(result.CodeFieldName);
                        $('#<%=hdnDisplayFieldName.ClientID %>').val(result.DisplayFieldName);
                        $('#<%=hdnMethodName.ClientID %>').val(result.MethodName);
                        onSubLedgerIDChanged();
                    }
                    else {
                        $('#<%=hdnGLAccountID.ClientID %>').val('');
                        $('#<%=txtGLAccountCode.ClientID %>').val('');
                        $('#<%=txtGLAccountName.ClientID %>').val('');

                        $('#<%=hdnSubLedgerID.ClientID %>').val('');
                        $('#<%=hdnSearchDialogTypeName.ClientID %>').val('');
                        $('#<%=hdnFilterExpression.ClientID %>').val('');
                        $('#<%=hdnIDFieldName.ClientID %>').val('');
                        $('#<%=hdnCodeFieldName.ClientID %>').val('');
                        $('#<%=hdnDisplayFieldName.ClientID %>').val('');
                        $('#<%=hdnMethodName.ClientID %>').val('');
                    }

                    $('#<%=hdnSubLedgerDtID.ClientID %>').val('');
                    $('#<%=txtSubLedgerDtCode.ClientID %>').val('');
                    $('#<%=txtSubLedgerDtName.ClientID %>').val('');
                });
            }

            function onSubLedgerIDChanged() {
                if ($('#<%=hdnSubLedgerID.ClientID %>').val() == '0' || $('#<%=hdnSubLedgerID.ClientID %>').val() == '') {
                    $('#<%=lblSubLedgerDt.ClientID %>').attr('class', 'lblDisabled');
                    $('#<%=txtSubLedgerDtCode.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=lblSubLedgerDt.ClientID %>').attr('class', 'lblLink');
                    $('#<%=txtSubLedgerDtCode.ClientID %>').removeAttr('readonly');
                }
            }
            //#endregion

            //#region Sub Ledger 
            function onGetSubLedgerDtFilterExpression() {
                var filterExpression = $('#<%=hdnFilterExpression.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnSubLedgerID.ClientID %>').val());
                return filterExpression;
            }

            $('#<%=lblSubLedgerDt.ClientID %>').click(function () {
                if ($('#<%=hdnSearchDialogTypeName.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnSearchDialogTypeName.ClientID %>').val(), onGetSubLedgerDtFilterExpression(), function (value) {
                        $('#<%=txtSubLedgerDtCode.ClientID %>').val(value);
                        onTxtSubLedgerDtCodeChanged(value);
                    });
                }
            });

            $('#<%=txtSubLedgerDtCode.ClientID %>').change(function () {
                onTxtSubLedgerDtCodeChanged($(this).val());
            });

            function onTxtSubLedgerDtCodeChanged(value) {
                if ($('#<%=hdnSearchDialogTypeName.ClientID %>').val() != '') {
                    var filterExpression = onGetSubLedgerDtFilterExpression() + " AND " + $('#<%=hdnCodeFieldName.ClientID %>').val() + " = '" + value + "'";
                    Methods.getObject($('#<%=hdnMethodName.ClientID %>').val(), filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnSubLedgerDtID.ClientID %>').val(result[$('#<%=hdnIDFieldName.ClientID %>').val()]);
                            $('#<%=txtSubLedgerDtName.ClientID %>').val(result[$('#<%=hdnDisplayFieldName.ClientID %>').val()]);
                        }
                        else {
                            $('#<%=hdnSubLedgerDtID.ClientID %>').val('');
                            $('#<%=txtSubLedgerDtCode.ClientID %>').val('');
                            $('#<%=txtSubLedgerDtName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion

            //#region Department
            function onGetDepartmentFilterExpression() {
                var filterExpression = "GLAccountNoSegment IS NOT NULL AND IsActive = 1";
                return filterExpression;
            }

            $('#lblDepartment.lblLink').live('click', function () {
                openSearchDialog('departmentakunt', onGetDepartmentFilterExpression(), function (value) {
                    $('#<%=txtDepartmentID.ClientID %>').val(value);
                    ontxtDepartmentIDChanged(value);
                });
            });

            $('#<%=txtDepartmentID.ClientID %>').live('change', function () {
                var param = $('#<%=txtDepartmentID.ClientID %>').val();
                ontxtDepartmentIDChanged(param);
            });

            function ontxtDepartmentIDChanged(value) {
                var filterExpression = onGetDepartmentFilterExpression() + " AND DepartmentID = '" + value + "'";
                Methods.getObject('GetDepartmentList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnDepartmentID.ClientID %>').val(result.DepartmentID);
                        $('#<%=txtDepartmentName.ClientID %>').val(result.DepartmentName);
                    }
                    else {
                        $('#<%=hdnDepartmentID.ClientID %>').val('');
                        $('#<%=txtDepartmentID.ClientID %>').val('');
                        $('#<%=txtDepartmentName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Service Unit
            function onGetServiceUnitFilterExpression() {
                var filterExpression = "GLAccountNoSegment IS NOT NULL AND IsDeleted = 0";
                return filterExpression;
            }

            $('#lblServiceUnit.lblLink').live('click', function () {
                openSearchDialog('serviceunitakunt', onGetServiceUnitFilterExpression(), function (value) {
                    $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                    ontxtServiceUnitCodeChanged(value);
                });
            });

            $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
                var param = $('#<%=txtServiceUnitCode.ClientID %>').val();
                ontxtServiceUnitCodeChanged(param);
            });

            function ontxtServiceUnitCodeChanged(value) {
                var filterExpression = onGetServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
                Methods.getObject('GetServiceUnitMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnServiceUnitID.ClientID %>').val(result.ServiceUnitID);
                        $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                    }
                    else {
                        $('#<%=hdnServiceUnitID.ClientID %>').val('');
                        $('#<%=txtServiceUnitCode.ClientID %>').val('');
                        $('#<%=txtServiceUnitName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Business Partner
            function onGetBusinessPartnerFilterExpression() {
                var filterExpression = "GLAccountNoSegment IS NOT NULL AND IsDeleted = 0";
                return filterExpression;
            }

            $('#lblBusinessPartner.lblLink').live('click', function () {
                openSearchDialog('businesspartnersakun', onGetBusinessPartnerFilterExpression(), function (value) {
                    $('#<%=txtBusinessPartnerCode.ClientID %>').val(value);
                    ontxtBusinessPartnerCodeChanged(value);
                });
            });

            $('#<%=txtBusinessPartnerCode.ClientID %>').live('change', function () {
                var param = $('#<%=txtBusinessPartnerCode.ClientID %>').val();
                ontxtBusinessPartnerCodeChanged(param);
            });

            function ontxtBusinessPartnerCodeChanged(value) {
                var filterExpression = onGetBusinessPartnerFilterExpression() + " AND BusinessPartnerCode = '" + $('#<%=txtBusinessPartnerCode.ClientID %>').val() + "'";
                Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
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
        });

        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            cbpTotal.PerformCallback('refresh');
        });

        function onGetGLAccountFilterExpression() {
            var filterExpression = "IsHeader = 0 AND IsDeleted = 0";
            return filterExpression;
        }

        function onCboHealthcareValueChanged(s) {
            var value = s.GetValue();
            $('#<%=hdnHealthcare.ClientID %>').val(value);
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }   
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <div>
        <table width="100%">
            <colgroup>
                <col style="width: 5%" />
                <col style="width: 5px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel" colspan="2">
                    <label class="tdLabel">
                        <%=GetLabel("Periode")%></label>
                </td>
                <td>
                    <table style="width: 60%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col width="150px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboYear" ClientInstanceName="cboYear" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboMonth" ClientInstanceName="cboMonth" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="padding-left: 10px">
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col width="60px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel" style="text-align: right;">
                                            <label class="tdLabel">
                                                <%=GetLabel("Status")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboStatus" Width="100%" ClientInstanceName="cboStatus" runat="server" />
                                            <input type="hidden" id="hdnDataSource" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" colspan="2">
                    <label class="lblNormal" id="lblHealthcare">
                        <%=GetLabel("Healthcare")%></label>
                </td>
                <td>
                    <table style="width: 60%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboHealthcare" ClientInstanceName="cboHealthcare" Width="100%"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboHealthcareValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                                <input type="hidden" id="hdnHealthcare" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" colspan="2">
                    <label class="lblLink" id="lblGLAccount">
                        <%=GetLabel("COA")%></label>
                </td>
                <td>
                    <input type="hidden" id="hdnGLAccountID" runat="server" />
                    <input type="hidden" id="hdnSubLedgerID" runat="server" />
                    <input type="hidden" id="hdnSearchDialogTypeName" runat="server" />
                    <input type="hidden" id="hdnIDFieldName" runat="server" />
                    <input type="hidden" id="hdnCodeFieldName" runat="server" />
                    <input type="hidden" id="hdnDisplayFieldName" runat="server" />
                    <input type="hidden" id="hdnMethodName" runat="server" />
                    <input type="hidden" id="hdnFilterExpression" runat="server" />
                    <table style="width: 60%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="txtGLAccountCode" Width="100%" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtGLAccountName" Width="100%" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="display: none;">
                <td class="tdLabel" colspan="2">
                    <label class="lblDisabled" runat="server" id="lblSubLedgerDt">
                        <%=GetLabel("Sub Akun")%></label>
                </td>
                <td>
                    <input type="hidden" id="hdnSubLedgerDtID" runat="server" />
                    <table style="width: 60%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="txtSubLedgerDtCode" Width="100%" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSubLedgerDtName" Width="100%" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" colspan="2">
                    <label class="lblLink" id="lblDepartment">
                        <%=GetLabel("Department")%></label>
                </td>
                <td>
                    <input type="hidden" id="hdnDepartmentID" runat="server" />
                    <table style="width: 60%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="txtDepartmentID" Width="100%" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDepartmentName" ReadOnly="true" Width="100%" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" colspan="2">
                    <label class="lblLink" id="lblServiceUnit">
                        <%=GetLabel("Service Unit")%></label>
                </td>
                <td>
                    <input type="hidden" id="hdnServiceUnitID" runat="server" />
                    <table style="width: 60%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="txtServiceUnitCode" Width="100%" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtServiceUnitName" ReadOnly="true" Width="100%" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" colspan="2">
                    <label class="lblLink" id="lblBusinessPartner">
                        <%=GetLabel("Business Partner")%></label>
                </td>
                <td>
                    <input type="hidden" id="hdnBusinessPartnerID" runat="server" />
                    <table style="width: 60%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="txtBusinessPartnerCode" Width="100%" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBusinessPartnerName" ReadOnly="true" Width="100%" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <div style="height: 400px; overflow-y: auto;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                        position: relative; font-size: 0.95em;">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="TransactionDtID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:BoundField DataField="JournalNo" ItemStyle-HorizontalAlign="Left" HeaderText="No. Voucher"
                                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
                                                <asp:BoundField DataField="JournalDateInString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Tanggal" HeaderStyle-Width="100px" />
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="180px">
                                                    <HeaderTemplate>
                                                        <%=GetLabel("Segment") %></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div style="font-size: 12px;">
                                                            DP:
                                                            <%#:Eval("DepartmentID") %></div>
                                                        <div style="font-size: 12px;">
                                                            SU:
                                                            <%#:Eval("ServiceUnitName") %></div>
                                                        <div style="font-size: 12px;">
                                                            RC:
                                                            <%#:Eval("RevenueCostCenterName") %></div>
                                                        <div style="font-size: 12px;">
                                                            CG:
                                                            <%#:Eval("CustomerGroupName") %></div>
                                                        <div style="font-size: 12px;">
                                                            BP:
                                                            <%#:Eval("BusinessPartnerName") %></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Remarks" ItemStyle-HorizontalAlign="Left" HeaderText="Catatan" />
                                                <asp:BoundField DataField="BalanceBEGIN" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}"
                                                    HeaderStyle-Width="100px" HeaderText="Saldo Awal" HeaderStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="DEBITAmount" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}"
                                                    HeaderStyle-Width="100px" HeaderText="Debet" HeaderStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="CREDITAmount" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}"
                                                    HeaderStyle-Width="100px" HeaderText="Kredit" HeaderStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="BalanceEND" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}"
                                                    HeaderStyle-Width="100px" HeaderText="Saldo Akhir" HeaderStyle-HorizontalAlign="Right" />
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
                    </div>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="3">
                    <dxcp:ASPxCallbackPanel ID="cbpTotal" runat="server" Width="100%" ClientInstanceName="cbpTotal"
                        ShowLoadingPanel="false" OnCallback="cbpTotal_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ cbpView.PerformCallback('refresh'); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <table>
                                    <colgroup>
                                        <col style="width: 120px;" />
                                        <col style="width: 200px;" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <%=GetLabel("Total Debet") %>
                                        </td>
                                        <td>
                                            <asp:TextBox Width="100%" runat="server" ReadOnly="true" CssClass="number" ID="txtTotalBalanceDEBIT" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%=GetLabel("Total Kredit") %>
                                        </td>
                                        <td>
                                            <asp:TextBox Width="100%" runat="server" ReadOnly="true" CssClass="number" ID="txtTotalBalanceCREDIT" />
                                        </td>
                                    </tr>
                                </table>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
