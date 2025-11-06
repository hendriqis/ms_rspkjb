<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="PrintLaboratoryResult1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PrintLaboratoryResult1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtFromDate.ClientID %>');
            setDatePicker('<%=txtToDate.ClientID %>');

            $('#<%=txtFromDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            $('#<%=txtToDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            $('#<%=btnRefresh.ClientID %>').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    onRefreshGrid();
                }
            });
        });

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGrid();
        }, interval);

        $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live('click', function () {
            var detail = $(this).find('.keyField').html();
            var mrn = $(this).find('.hiddenColumn').html();
            $('#<%=hdnID.ClientID %>').val(detail);
            $('#<%=hdnMRN.ClientID %>').val(mrn);
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#divErrorDetail').html('');
            $('#divErrorDetail').append(convert(detail));
        });

        $('#<%=grdView.ClientID %> .imgViewHasil.imgLink').live('click', function () {
            var transactionID = $(this).attr("transactionID");
            var transactionNo = $(this).attr("transactionNo");
            cbpAPICall.PerformCallback(transactionID + "|" + transactionNo);
        });

        $('#<%=txtFromDate.ClientID %>').live('change', function () {
            var start = $('#<%=txtFromDate.ClientID %>').val();
            var end = $('#<%=txtToDate.ClientID %>').val();

            $('#<%=txtFromDate.ClientID %>').val(validateDateFromTo(start, end));
        });

        $('#<%=txtToDate.ClientID %>').live('change', function () {
            var start = $('#<%=txtFromDate.ClientID %>').val();
            var end = $('#<%=txtToDate.ClientID %>').val();

            $('#<%=txtToDate.ClientID %>').val(validateDateToFrom(start, end));
        });

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrid();
            }, 0);
        }

        var convert = function (convert) {
            return $('<span />', { html: convert }).text();
        }

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

        function onRefreshGrid() {
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpView.PerformCallback('refresh');
            intervalID = window.setInterval(function () {
                onRefreshGridView();
            }, interval);
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


        $('.lblTransactionNo').live('click', function () {
            var transactionID = $(this).closest('tr').find('.keyField').html();
            var id = transactionID;
            var url = ResolveUrl("~/Libs/Program/Module/SystemTool/PrintTestResult/LIS/ViewLaboratoryTestDetailCtl1.ascx");
            openUserControlPopup(url, id, 'Detail Transaksi', 550, 500);
        });

        function onCbpAPICallEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                window.open("data:application/pdf;base64," + param[1], "popupWindow", "width=600, height=600,scrollbars=yes");
            }
            else {
                displayErrorMessageBox('Cetak Ulang Hasil', 'Error Message : ' + param[1]);
            }
        }
    </script>
    <input type="hidden" value="" id="hdnModuleID" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnIsBridgingToPGxTest" runat="server" value="0" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <div style="position: relative;">
        <table tyle="width:60%;">
            <colgroup>
                <col style="width: 120px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal Transaksi")%></label>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:TextBox runat="server" CssClass="datepicker" ID="txtFromDate" Width="120px" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="datepicker" ID="txtToDate" Width="120px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Quick Filter")%></label>
                </td>
                <td>
                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                        Width="378px" Watermark="Search">
                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                        <IntellisenseHints>
                            <qis:QISIntellisenseHint Text="Nama" FieldName="PatientName" />
                            <qis:QISIntellisenseHint Text="No. RM" FieldName="MedicalNo" />
                            <qis:QISIntellisenseHint Text="No. Transaksi" FieldName="TransactionNo" />
                        </IntellisenseHints>
                    </qis:QISIntellisenseTextBox>
                </td>
            </tr>
        </table>
        <div style="padding: 10px 0 10px 3px; font-size: 0.95em">
            <%=GetLabel("Halaman ini akan auto refresh")%>
            <%=GetLabel("setiap ")%>
            <%=GetRefreshGridInterval() %>
            <%=GetLabel("menit ")%>
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="TransactionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="MRN" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" />
                                <asp:BoundField DataField="TransactionNo" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn transactionNo" />
                                <asp:BoundField DataField="cfTransactionDate" HeaderText="Tanggal" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="90px" />
                                <asp:BoundField DataField="TransactionTime" HeaderText="Jam" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="50px" />
                                <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                    HeaderText="No. Transaksi">
                                    <ItemTemplate>
                                        <label class="lblTransactionNo lblLink">
                                            <%#:Eval("TransactionNo")%></label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="MedicalNo" HeaderText="No. RM" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="80px" />
                                <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ReferenceNo" HeaderText="No. Referensi LIS" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Status Hasil" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align='center'>
                                                    <img class="imgViewHasil <%# Eval("IsResultExist").ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                        title='<%=GetLabel("View Hasil dari LIS (PDF)")%>' src='<%# Eval("IsResultExist").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/view_disabled.png") : ResolveUrl("~/Libs/Images/Button/view.png")%>'
                                                        alt="" transactionID="<%#:Eval("TransactionID")%>" transactionNo="<%#:Eval("TransactionNo")%>" />
                                                    <br>
                                                    <asp:Label ID="lblStatusHasil" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <div <%# (Eval("cfShowReportButton").ToString() != "True") ? "Style='display:none'":"" %>>
                                            <input type="button" class="btnPharmacogenomic w3-btn w3-hover-blue" value="REPORT"
                                                style="background-color: Green; color: White; width: 100px" mrn="<%#:Eval("MRN")%>"
                                                transactionid="<%#:Eval("TransactionID")%>" transactionno="<%#:Eval("TransactionNo")%>"
                                                sampleid="<%#:Eval("cfSampleID")%>" /></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada transaksi pemeriksaan laboratorium di periode ini")%>
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
    <dxcp:ASPxCallbackPanel ID="cbpAPICall" runat="server" Width="100%" ClientInstanceName="cbpAPICall"
        ShowLoadingPanel="false" OnCallback="cbpAPICall_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpAPICallEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
