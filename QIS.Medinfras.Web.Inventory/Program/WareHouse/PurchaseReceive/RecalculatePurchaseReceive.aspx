<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="RecalculatePurchaseReceive.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.RecalculatePurchaseReceive" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });

            //#region Purchase Receive No
            function onGetPurchaseReceiveFilterExpression() {
                var filterExpression = "1=1";
                return filterExpression;
            }

            $('#lblPurchaseReceiveNo.lblLink').live('click', function () {
                openSearchDialog('purchasereceivehd', onGetPurchaseReceiveFilterExpression(), function (value) {
                    onTxtPurchaseReceiveNoChanged(value);
                });
            });

            $('#<%=txtPurchaseReceiveNo.ClientID %>').change(function () {
                onTxtPurchaseReceiveNoChanged($(this).val());
            });

            function onTxtPurchaseReceiveNoChanged(value) {
                var filterExpression = "PurchaseReceiveNo = '" + value + "'";
                Methods.getObject('GetPurchaseReceiveHdList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnPurchaseReceiveID.ClientID %>').val(result.PurchaseReceiveID);
                        $('#<%=txtPurchaseReceiveNo.ClientID %>').val(result.PurchaseReceiveNo);
                    }
                    else {
                        $('#<%=hdnPurchaseReceiveID.ClientID %>').val('');
                        $('#<%=txtPurchaseReceiveNo.ClientID %>').val('');
                    }
                    onRefreshGridView();
                });
            }
            //#endregion

            $('.lnkItem a').live('click', function () {
                var id = $(this).closest('tr').find('.keyField').html();
                var url = ResolveUrl("~/Program/Warehouse/PurchaseReceive/RecalculatePurchaseReceiveDetailCtl.ascx");
                openUserControlPopup(url, id, 'Detail Penerimaan Pembelian', 1300, 550);
            });

        });

        //#region Paging & refresh
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalID);
                cbpView.PerformCallback('refresh');
                intervalID = window.setInterval(function () {
                    onRefreshGridView();
                }, interval);
            }
        }
        //#endregion

        //#region Button
        $btnSave = null;
        $('.btnSave').live('click', function () {
            if ($(this).attr('enabled') != 'false') {
                $tr = $(this).closest('tr');
                var visitID = $tr.find('.keyField').html();
                var diagnose = $tr.find('.txtDiagnose').val();
                var paramedicID = $tr.find('.hdnParamedicID').val();

                var param = visitID + '|' + diagnose + '|' + paramedicID;
                $btnSave = $(this);
                cbpSaveDiagnose.PerformCallback(param);
            }
        });
        //#endregion

        function onCbpSaveDiagnoseEndCallback(s) {
            var result = s.cpResult.split('|');
            if (result[0] == 'success') {
                $tr = $btnSave.closest('tr');
                $btnSave.attr('enabled', 'false');
            }
            else {
                if (result[1] != '')
                    showToast('Save Failed', 'Error Message : ' + result[1]);
                else
                    showToast('Save Failed', '');
            }
            hideLoadingPanel();
        }
    </script>
    <input type="hidden" value="" id="hdnPurchaseReceiveID" runat="server" />
    <div style="padding: 15px">
        <div class="pageTitle">
            <%=GetMenuCaption()%></div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsPatientList">
                        <table class="tblEntryContent" style="width: 60%;">
                            <colgroup>
                                <col style="width: 25%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblPurchaseReceiveNo">
                                        <%=GetLabel("No. BPB")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPurchaseReceiveNo" Width="200px" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                        <%=GetLabel("Halaman Ini Akan")%>
                        <span class="lblLink" id="lblRefresh">[refresh]</span>
                        <%=GetLabel("Setiap")%>
                        <%=GetRefreshGridInterval() %>
                        <%=GetLabel("Menit")%>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="PurchaseReceiveID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="PurchaseReceiveNo" HeaderText="No. Penerimaan" HeaderStyle-Width="200px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfReceivedDateInString" HeaderText="Tgl. Penerimaan" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="cfBusinessPartnerInString" HeaderText="Supplier" HeaderStyle-Width="200px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ProductLineName" HeaderText="Product Line" HeaderStyle-Width="200px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="TransactionAmount" HeaderText="Total POR" DataFormatString="{0:N2}"
                                                HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="TransactionAmountDetail" HeaderText="Total Detail POR" DataFormatString="{0:N2}"
                                                HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="NetTransactionAmount" HeaderText="Total Akhir POR" DataFormatString="{0:N2}"
                                                HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="TransactionStatus" HeaderText="Status POR" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:HyperLinkField HeaderText="Item" Text="Item" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-CssClass="lnkItem" HeaderStyle-Width="120px" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging">
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
