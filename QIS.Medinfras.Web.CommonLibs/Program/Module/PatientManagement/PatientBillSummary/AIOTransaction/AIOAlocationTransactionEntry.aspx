<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master"
    AutoEventWireup="true" CodeBehind="AIOAlocationTransactionEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.AIOAlocationTransactionEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnLink" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Link")%></div>
    </li>
    <li id="btnUnlink" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Un-link")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback('refresh');
        }

        $('#<%=btnLink.ClientID %>').live('click', function () {
            var balanceTariffSelected = $('#<%=hdnSelectedBalanceTariffDtID.ClientID %>').val();
            if (balanceTariffSelected != null && balanceTariffSelected != "") {
                if ($('.chkIsSelected input:checked').length < 1)
                    showToast('Warning', '<%=GetErrorMsgSelectTransactionFirst() %>');
                else {
                    getCheckedMember();
                    onCustomButtonClick('linkaio');
                }
            } else {
                showToast('Warning', 'Harap pilih Balance Tariff terlebih dahulu.');
            }
        });

        $('#<%=btnUnlink.ClientID %>').live('click', function () {
            var balanceTariffSelected = $('#<%=hdnSelectedBalanceTariffDtID.ClientID %>').val();
            if (balanceTariffSelected != null && balanceTariffSelected != "") {
                if ($('.chkIsSelected input:checked').length < 1)
                    showToast('Warning', '<%=GetErrorMsgSelectTransactionFirst() %>');
                else {
                    getCheckedMember();
                    onCustomButtonClick('unlinkaio');
                }
            } else {
                showToast('Warning', 'Harap pilih Balance Tariff terlebih dahulu.');
            }
        });

        function getCheckedMember() {
            var lstTransactionDtID = $('#<%=hdnSelectedTransactionDtID.ClientID %>').val().split(',');
            $('.chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var $tr = $(this).closest('tr');
                    var keyField = $tr.find('.keyField').val();
                    var idx = lstTransactionDtID.indexOf(keyField);
                    if (idx < 0) {
                        lstTransactionDtID.push(keyField);
                    }
                    else {
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').val();
                    var idx = lstTransactionDtID.indexOf(key);
                    if (idx > -1) {
                        lstTransactionDtID.splice(idx, 1);
                    }
                }
            });

            $('#<%=hdnSelectedTransactionDtID.ClientID %>').val(lstTransactionDtID.join(','));
        }

        $('.chkSelectAll input').die('change');
        $('.chkSelectAll input').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        //#region Balance Tariff
        function getBalanceTariffFilterExpression() {
            var filterExpression = "IsBalanceTariff = 1 AND IsDeleted = 0 AND VisitID = " + $('#<%=hdnVisitID.ClientID %>').val();
            return filterExpression;
        }

        $('#lblBalanceTariff.lblLink').live('click', function () {
            openSearchDialog('consultvisititempackagebalance', getBalanceTariffFilterExpression(), function (value) {
                $('#<%=txtBalanceTariffItemCode.ClientID %>').val(value);
                ontxtBalanceTariffItemCodeChanged(value);
            });
        });

        $('#<%=txtBalanceTariffItemCode.ClientID %>').live('change', function () {
            ontxtBalanceTariffItemCodeChanged($(this).val());
        });

        function ontxtBalanceTariffItemCodeChanged(value) {
            var filterExpression = getBalanceTariffFilterExpression() + " AND DtItemCode = '" + value + "'";
            Methods.getObject('GetvConsultVisitItemPackageBalanceList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnSelectedBalanceTariffDtID.ClientID %>').val(result.DtID);
                    $('#<%=txtBalanceTariffItemCode.ClientID %>').val(result.DtItemCode);
                    $('#<%=txtBalanceTariffItemName.ClientID %>').val(result.DtItemName1);
                }
                else {
                    $('#<%=hdnSelectedBalanceTariffDtID.ClientID %>').val('');
                    $('#<%=txtBalanceTariffItemCode.ClientID %>').val('');
                    $('#<%=txtBalanceTariffItemName.ClientID %>').val('');
                }
            });
        }
        //#endregion
    
    </script>
    <input type="hidden" value="" id="hdnSelectedBalanceTariffDtID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedTransactionDtID" runat="server" />
    <div style="height: 500px; overflow-y: auto;">
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td>
                    <table>
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 150px" />
                            <col style="width: 300px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <label class="lblLink lblMandatory" id="lblBalanceTariff">
                                    <%=GetLabel("Balance Tariff")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBalanceTariffItemCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtBalanceTariffItemName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="padding: 5px; min-height: 150px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { hideLoadingPanel(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <table class="tblView grdNormal notAllowSelect" cellspacing="0" rules="all">
                                            <tr>
                                                <th style="width: 40px">
                                                    <div style="padding: 3px">
                                                        <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" style="display:none" />
                                                    </div>
                                                </th>
                                                <th style="width: 180px" align="left">
                                                    <div style="padding: 3px; float: left">
                                                        <div>
                                                            <%= GetLabel("Jenis Item")%></div>
                                                    </div>
                                                </th>
                                                <th align="left">
                                                    <div style="padding: 3px; float: left">
                                                        <div>
                                                            <%= GetLabel("Nama Item")%></div>
                                                    </div>
                                                </th>
                                                <th style="width: 180px" align="left">
                                                    <div style="padding: 3px; float: left">
                                                        <div>
                                                            <%= GetLabel("No. Transaksi")%></div>
                                                    </div>
                                                </th>
                                                <th style="width: 180px" align="left">
                                                    <div style="padding: 3px; float: left">
                                                        <div>
                                                            <%= GetLabel("Unit Transaksi")%></div>
                                                    </div>
                                                </th>
                                                <th style="width: 170px" align="right">
                                                    <div style="padding: 3px">
                                                        <%=GetLabel("Total")%>
                                                    </div>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%= GetLabel("Status Transaksi")%></div>
                                                    </div>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%= GetLabel("Status Link")%></div>
                                                    </div>
                                                </th>
                                            </tr>
                                            <asp:ListView ID="lvwView" runat="server" OnItemDataBound="lvwView_ItemDataBound">
                                                <EmptyDataTemplate>
                                                    <tr class="trEmpty">
                                                        <td colspan="15">
                                                            <%=GetLabel("---tidak ada data---") %>
                                                        </td>
                                                    </tr>
                                                </EmptyDataTemplate>
                                                <ItemTemplate>
                                                    <tr style="<%#: Eval("MovementID").ToString() != "" && Eval("MovementID").ToString() != "0" ? "background-color: #84ffda " : ""%>">
                                                        <td align="center">
                                                            <div style="padding: 3px">
                                                                <asp:CheckBox ID="chkIsSelected" CssClass="chkIsSelected" runat="server" />
                                                                <input type="hidden" class="keyField" value="<%#: Eval("ID")%>" />
                                                                <input type="hidden" class="TransactionID" value="<%#: Eval("TransactionID")%>" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: left;">
                                                                <%#: Eval("ItemType")%></div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: left; font-weight: bold">
                                                                <%#: Eval("ItemName1")%></div>
                                                            <div style="padding: 3px; text-align: left; font-size: smaller; font-style: italic">
                                                                <%#: Eval("ItemCode")%></div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: left; font-weight: bold">
                                                                <%#: Eval("TransactionNo")%></div>
                                                            <div style="padding: 3px; text-align: left;">
                                                                <%#: Eval("cfTransactionDateInString")%><%=GetLabel(" | ")%><%#: Eval("TransactionTime")%></div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: left;">
                                                                <%#: Eval("ChargesServiceUnitName")%></div>
                                                            <div style="padding: 3px; text-align: left; font-size: smaller; font-style: italic">
                                                                <%#: Eval("ChargesDepartmentID")%></div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("cfLineAmountInString")%></div>
                                                            </div>
                                                        </td>
                                                        <td style="<%#: Eval("GCTransactionStatus").ToString() == "X121^001" ? "background-color:#FFE4E1" : ""%>">
                                                            <div style="padding: 3px; text-align: center;">
                                                                <%#: Eval("TransactionStatus")%></div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: center;">
                                                                <div>
                                                                    <%#: Eval("MovementID").ToString() != "" && Eval("MovementID").ToString() != "0" ? "V" : "X"%></div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </table>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingService">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
