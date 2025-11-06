<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="JournalList.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.JournalList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnApprove" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><div>
            <%=GetLabel("Approve")%></div>
    </li>
    <li id="btnVoid" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png")%>' alt="" /><div>
            <%=GetLabel("Void")%></div>
    </li>
    <li id="btnReOpen" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Re-Open")%></div>
    </li>
    <li id="btnTemp" runat="server" crudmode="R" style="height: 53px"></li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        function onRefreshControl(filterExpression) {
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            setDatePicker('<%=txtFromJournalDate.ClientID %>');
            $('#<%=txtFromJournalDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtToJournalDate.ClientID %>');
            $('#<%=txtToJournalDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $("#<%=txtFromJournalDate.ClientID%>").change(function () {
                cbpView.PerformCallback('refresh');
            });

            $("#<%=txtToJournalDate.ClientID%>").change(function () {
                cbpView.PerformCallback('refresh');
            });

            $('#<%=btnApprove.ClientID %>').click(function () {
                onCustomButtonClick('approve');
            });

            $('#<%=btnVoid.ClientID %>').click(function () {
                var url = ResolveUrl('~/Program/Journal/JournalEntryVoidCtl.ascx');
                var transactionID = $('#<%=hdnID.ClientID %>').val();
                var id = transactionID;
                openUserControlPopup(url, id, 'Void Journal', 400, 230);
            });

            $('#<%=btnReOpen.ClientID %>').click(function () {
                onCustomButtonClick('reopen');
            });
        });

        function onAfterCustomClickSuccess() {
            cbpView.PerformCallback('refresh');
        }

        $('.lnkDetail a').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.keyField').html();
            var url = ResolveUrl("~/Program/Journal/JournalListDtCtl.ascx");
            openUserControlPopup(url, id, 'Detail', 1200, 500);
        });

        function onCboTransactionStatusValueChanged(s) {
            var value = s.GetValue();
            $('#<%=hdnTransactionStatus.ClientID %>').val(value);
            if (value == Constant.TransactionStatus.OPEN) {
                $('#<%=btnApprove.ClientID %>').removeAttr('style');
                $('#<%=btnVoid.ClientID %>').removeAttr('style');
                $('#<%=btnReOpen.ClientID %>').attr('style', 'display:none');
            } else if (value == Constant.TransactionStatus.APPROVED) {
                $('#<%=btnApprove.ClientID %>').attr('style', 'display:none');
                $('#<%=btnVoid.ClientID %>').attr('style', 'display:none');
                $('#<%=btnReOpen.ClientID %>').removeAttr('style');
            } else if (value == Constant.TransactionStatus.PROCESSED) {
                $('#<%=btnApprove.ClientID %>').attr('style', 'display:none');
                $('#<%=btnVoid.ClientID %>').attr('style', 'display:none');
                $('#<%=btnReOpen.ClientID %>').attr('style', 'display:none');
            } else if (value == Constant.TransactionStatus.CLOSED) {
                $('#<%=btnApprove.ClientID %>').attr('style', 'display:none');
                $('#<%=btnVoid.ClientID %>').attr('style', 'display:none');
                $('#<%=btnReOpen.ClientID %>').attr('style', 'display:none');
            } else {
                $('#<%=btnApprove.ClientID %>').attr('style', 'display:none');
                $('#<%=btnVoid.ClientID %>').attr('style', 'display:none');
                $('#<%=btnReOpen.ClientID %>').attr('style', 'display:none');
            }

            cbpView.PerformCallback('refresh');
        }

        function onCboGCJournalGroupEndCallBack(s) {
            onCboGCJournalGroupValueChanged(s);
        }

        function onCboGCJournalGroupValueChanged(s) {
            var value = s.GetValue();
            $('#<%=hdnJournalGroup.ClientID %>').val(value);
            $('#<%=hdnTransactionCode.ClientID %>').val('');
            $('#<%=txtTransactionCode.ClientID %>').val('');
            $('#<%=txtTransactionName.ClientID %>').val('');
            cbpView.PerformCallback('refresh');
        }

        //#region Transaction Type
        $('#lblTransactionType.lblLink').live("click", function () {
            var group = $('#<%=hdnJournalGroup.ClientID %>').val();
            if (group == "X188^001") {
                filterExpression = "TransactionCode BETWEEN '7200' AND '7220'";
            } else if (group == "X188^002") {
                filterExpression = "TransactionCode BETWEEN '7221' AND '7240'";
            } else if (group == "X188^003") {
                filterExpression = "TransactionCode BETWEEN '7241' AND '7260'";
            } else if (group == "X188^004") {
                filterExpression = "TransactionCode BETWEEN '7261' AND '7270'";
            } else if (group == "X188^005") {
                filterExpression = "TransactionCode BETWEEN '7271' AND '7280'";
            } else {
                var status = $('#<%=hdnTransactionStatus.ClientID %>').val();
                if (status == "X121^004") {
                    filterExpression = "TransactionCode = '7299'";
                } else if (status == "X121^005" || status == "X121^999") {
                    filterExpression = "TransactionCode BETWEEN '7281' AND '7299'";
                } else {
                    filterExpression = "TransactionCode BETWEEN '7281' AND '7288'";
                }
            }

            openSearchDialog('transactiontype', filterExpression, function (value) {
                $('#<%=txtTransactionCode.ClientID %>').val(value);
                onTxtTransactionCodeChanged(value);
            });
        });

        $('#<%=txtTransactionCode.ClientID %>').live("change", function () {
            onTxtTransactionCodeChanged($(this).val());
        });

        function onTxtTransactionCodeChanged(value) {
            var filterExpression = "TransactionCode = '" + value + "'";
            Methods.getObject('GetTransactionTypeList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnIsFilterTransactionCode.ClientID %>').val("1");
                    $('#<%=hdnTransactionCode.ClientID %>').val(result.TransactionCode);
                    $('#<%=txtTransactionName.ClientID %>').val(result.TransactionName);
                }
                else {
                    $('#<%=hdnTransactionCode.ClientID %>').val('');
                    $('#<%=txtTransactionCode.ClientID %>').val('');
                    $('#<%=txtTransactionName.ClientID %>').val('');
                }
            });
            cbpView.PerformCallback('refresh');
        }
        //#endregion 

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGridView();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpView.PerformCallback('refresh');
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var transactionID = $('#<%=hdnID.ClientID %>').val();

            if (transactionID == '' || transactionID == '0') {
                errMessage.text = 'Pilih Jurnal Terlebih Dahulu!';
                return false;
            }
            else {
                var status = $('#<%=grdView.ClientID %> tr.selected .hdnGCTransactionStatus').val();
                if (status == "<%=GetGCTransactionStatusOpen() %>") {
                    errMessage.text = 'Jurnal Belum di Approve';
                    return false;
                } else {
                    filterExpression.text = 'GLTransactionID = ' + transactionID;
                    return true;
                }
            }
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnTransactionStatus" runat="server" value="" />
    <input type="hidden" id="hdnJournalGroup" runat="server" value="" />
    <input type="hidden" id="hdnIsFilterTransactionCode" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" runat="server" value="" />
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <table cellpadding="0" cellspacing="0">
        <colgroup>
            <col style="width: 150px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Tanggal")%></label>
            </td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 145px" />
                        <col style="width: 5px" />
                        <col style="width: 145px" />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtFromJournalDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            &nbsp;<%=GetLabel("s/d") %>&nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtToJournalDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel" style="padding-top: 3px">
                <label>
                    <%=GetLabel("Status")%></label>
            </td>
            <td style="padding-top: 3px">
                <dxe:ASPxComboBox ID="cboTransactionStatus" ClientInstanceName="cboTransactionStatus"
                    Width="300px" runat="server">
                    <ClientSideEvents ValueChanged="function(s,e){ onCboTransactionStatusValueChanged(s); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr id="trGroupDt" runat="server">
            <td class="tdLabel" style="padding-top: 3px">
                <label>
                    <%=GetLabel("Grup Jurnal") %></label>
            </td>
            <td style="padding-top: 3px">
                <dxe:ASPxComboBox ID="cboGCJournalGroup" ClientInstanceName="cboGCJournalGroup" Width="300px"
                    runat="server" OnCallback="cboGCJournalGroup_Callback" >
                    <ClientSideEvents ValueChanged="function(s,e){ onCboGCJournalGroupValueChanged(s); }" EndCallback="function(s,e){ onCboGCJournalGroupEndCallBack(s); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td class="tdLabel" style="padding-top: 3px">
                <label class="lblLink" id="lblTransactionType">
                    <%=GetLabel("Sumber Data")%></label>
            </td>
            <td style="padding-top: 3px; width: 700px">
                <input type="hidden" id="hdnTransactionCode" value="" runat="server" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtTransactionCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionName" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel" style="padding-top: 3px">
                <label>
                    <%=GetLabel("Quick Filter")%></label>
            </td>
            <td style="padding-top: 3px">
                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                    Width="300px" Watermark="Search">
                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                    <IntellisenseHints>
                        <qis:QISIntellisenseHint Text="JournalNo" FieldName="JournalNo" />
                        <qis:QISIntellisenseHint Text="Remarks" FieldName="Remarks" />
                    </IntellisenseHints>
                </qis:QISIntellisenseTextBox>
            </td>
        </tr>
    </table>
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="GLTransactionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="JournalNo" HeaderText="No Jurnal" HeaderStyle-Width="150px" />
                                <asp:BoundField DataField="JournalDateInString" ItemStyle-HorizontalAlign="Center"
                                    HeaderText="Tanggal" HeaderStyle-Width="150px" />
                                <asp:BoundField DataField="Remarks" HeaderText="Catatan" />
                                <asp:BoundField ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataField="DebitAmount" DataFormatString="{0:N}" HeaderText="Debet" HeaderStyle-Width="150px"/>
                                <asp:BoundField ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataField="CreditAmount" DataFormatString="{0:N}" HeaderText="Kredit" HeaderStyle-Width="150px"/>
                                <asp:BoundField DataField="TransactionStatus" HeaderText="Status" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="100px" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkDetail"
                                    HeaderText="Detail" HeaderStyle-Width="150px">
                                    <ItemTemplate>
                                        <input type="hidden" class="hdnGCTransactionStatus" value='<%#:Eval("GCTransactionStatus") %>'>
                                        <a>Detail</a>
                                    </ItemTemplate>
                                </asp:TemplateField>
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
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
