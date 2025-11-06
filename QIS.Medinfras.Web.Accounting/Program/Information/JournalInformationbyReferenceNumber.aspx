<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="JournalInformationbyReferenceNumber.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.JournalInformationbyReferenceNumber" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1.Export, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid.Export" TagPrefix="dx" %>
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
    <li id="btnVerified" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Verifikasi")%></div>
    </li>
    <li id="btnUnverified" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Batal Verifikasi")%></div>
    </li>
    <li id="btnReportPageRAWExport" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrawexcel.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("RAW Excel")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            cbpTotal.PerformCallback('refresh');
        });

        $('#<%=btnVerified.ClientID %>').live('click', function () {
            showToastConfirmation('Apakah anda yakin akan melakukan VERIFIKASI terhadap detail jurnal terpilih ?', function (result) {
                if (result) {
                    $('#<%=hdnSelectedTransactionDtID.ClientID %>').val("");
                    getSelectedVerified();
                    if ($('#<%=hdnSelectedTransactionDtID.ClientID %>').val() != "") {
                        onCustomButtonClick('verified');
                    } else {
                        showToast('Warning', 'Silakan pilih detail terlebih dahulu.');
                    }
                }
            });
        });

        $('#<%=btnUnverified.ClientID %>').live('click', function () {
            showToastConfirmation('Apakah anda yakin akan melakukan BATAL VERIFIKASI terhadap detail jurnal terpilih ?', function (result) {
                if (result) {
                    $('#<%=hdnSelectedTransactionDtID.ClientID %>').val("");
                    getSelectedVerified();
                    if ($('#<%=hdnSelectedTransactionDtID.ClientID %>').val() != "") {
                        onCustomButtonClick('unverified');
                    } else {
                        showToast('Warning', 'Silakan pilih detail terlebih dahulu.');
                    }
                }
            });
        });

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.lvwView .chkIsSelected').each(function () {
                $chk = $(this).find('input');
                $chk.prop('checked', isChecked);
                $chk.change();
            });
        });

        function getSelectedVerified() {
            var lstSelectedTransactionDtID = $('#<%=hdnSelectedTransactionDtID.ClientID %>').val().split(',');
            $('.lvwView .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').val();
                    var idx = lstSelectedTransactionDtID.indexOf(key);
                    if (idx < 0) {
                        lstSelectedTransactionDtID.push(key);
                    }
                    else {
                    }
                }
                else {
                    var $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').html();
                    var idx = lstSelectedTransactionDtID.indexOf(key);
                    if (idx > -1) {
                        lstSelectedTransactionDtID.splice(idx, 1);
                    }
                }
            });

            $('#<%=hdnSelectedTransactionDtID.ClientID %>').val(lstSelectedTransactionDtID.join(','));
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        function onAfterCustomClickSuccess(type, retval) {
            cbpView.PerformCallback('refresh');
        }

        $('#<%=btnReportPageRAWExport.ClientID %>').live('click', function (evt) {
            __doPostBack('<%=btnRAWExport.UniqueID%>', '');
        });

    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionText" runat="server" />
    <input type="hidden" value="" id="hdnSelectedTransactionDtID" runat="server" />
    <table width="100%">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top" align="left">
                <table>
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 300px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("No. Referensi")%></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtReferenceNo" Width="430px" />
                        </td>
                    </tr>
                     <tr>
                            <td class="tdLabel">
                                <label class="tdLabel">
                                    <%=GetLabel("Status Transaksi")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboTransactionStatus" ClientInstanceName="cboTransactionStatus" Width="40%" runat="server" />
                                <input type="hidden" id="hdnDataSource" runat="server" />
                            </td>
                        </tr>
                </table>
            </td>
            <td style="vertical-align: top" align="right">
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
        <tr>
            <td colspan="2">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em;">
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 35px" align="center">
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </th>
                                                <th style="width: 130px" align="left">
                                                    <%=GetLabel("No / Tgl Jurnal")%>
                                                </th>
                                                <th style="width: 180px" align="left">
                                                    <%=GetLabel("COA")%>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("Segment")%>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("No. Referensi")%>
                                                </th>
                                                <th align="left">
                                                    <%=GetLabel("Keterangan")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Status Verifikasi")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Status Transaksi")%>
                                                </th>
                                                <th style="width: 35px" align="center">
                                                    <%=GetLabel("Posisi")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Debit")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Kredit")%>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="20">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="lvwView grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 35px" align="center">
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </th>
                                                <th style="width: 130px" align="left">
                                                    <%=GetLabel("No / Tgl Jurnal")%>
                                                </th>
                                                <th style="width: 180px" align="left">
                                                    <%=GetLabel("COA")%>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("Segment")%>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("No. Referensi")%>
                                                </th>
                                                <th align="left">
                                                    <%=GetLabel("Keterangan")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Status Verifikasi")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Status Transaksi")%>
                                                </th>
                                                <th style="width: 35px" align="center">
                                                    <%=GetLabel("Posisi")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Debit")%>
                                                </th>
                                                <th style="width: 120px" align="right">
                                                    <%=GetLabel("Kredit")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("TransactionDtID")%>' />
                                            </td>
                                            <td>
                                                <div style="font-weight: bold">
                                                    <%#:Eval("JournalNo") %></div>
                                                <div style="font-size: smaller">
                                                    <%#:Eval("cfJournalDateInString") %></div>
                                            </td>
                                            <td>
                                                <div style="font-weight: bold">
                                                    <%#:Eval("GLAccountNo") %></div>
                                                <div style="font-size: smaller">
                                                    <%#:Eval("GLAccountName") %></div>
                                            </td>
                                            <td>
                                                <div style="font-size: smaller">
                                                    DP:
                                                    <%#:Eval("DepartmentID") %></div>
                                                <div style="font-size: smaller">
                                                    SU:
                                                    <%#:Eval("ServiceUnitName") %></div>
                                                <div style="font-size: smaller">
                                                    RC:
                                                    <%#:Eval("RevenueCostCenterName") %></div>
                                                <div style="font-size: smaller">
                                                    CG:
                                                    <%#:Eval("CustomerGroupName") %></div>
                                                <div style="font-size: smaller">
                                                    BP:
                                                    <%#:Eval("BusinessPartnerName") %></div>
                                            </td>
                                            <td>
                                                <%#:Eval("ReferenceNo") %>
                                            </td>
                                            <td>
                                                <div style="font-size: smaller">
                                                    <b>Hd:</b>
                                                    <%#:Eval("Remarks") %></div>
                                                <div style="font-size: smaller">
                                                    <b>Dt:</b>
                                                    <%#:Eval("RemarksDetail") %></div>
                                            </td>
                                            <td>
                                                <div style="font-weight: bold">
                                                    <%#:Eval("cfIsVerifiedInText") %></div>
                                                <div style="font-size: smaller; font-style: italic">
                                                    <%#:Eval("LastVerifiedByName") %></div>
                                                <div style="font-size: smaller">
                                                    <%#:Eval("cfLastVerifiedDateInString") %></div>
                                            </td>
                                            <td align="center">
                                                <%#:Eval("TransactionStatus") %>
                                            </td>
                                            <td align="center">
                                                <%#:Eval("Position") %>
                                            </td>
                                            <td align="right">
                                                <%#:Eval("cfDebitAmountInString") %>
                                            </td>
                                            <td align="right">
                                                <%#:Eval("cfCreditAmountInString") %>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
            </td>
        </tr>
    </table>
    <div style="display: none;">
        <asp:Button ID="btnRAWExport" Visible="true" runat="server" OnClick="btnRAWExport_Click"
            Text="RAW Export" UseSubmitBehavior="false" OnClientClick="return true;" />
    </div>
</asp:Content>
