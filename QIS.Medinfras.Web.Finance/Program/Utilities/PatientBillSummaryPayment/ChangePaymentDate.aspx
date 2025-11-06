<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="ChangePaymentDate.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ChangePaymentDate" %>

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
            setDatePicker('<%=txtDateFrom.ClientID %>');
            setDatePicker('<%=txtDateTo.ClientID %>');
            $('#<%:txtDateTo.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
        });

        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        $('.lblDetail').die('click');
        $('.lblDetail').live('click', function () {
            $tr = $(this).closest('tr');
            var paymentID = $tr.find('.hiddenColumn').html();
            var url = ResolveUrl("~/Program/Utilities/PatientBillSummaryPayment/ChangePaymentDateCtl.ascx");
            openUserControlPopup(url, paymentID, 'Detail Pembayaran', 800, 450);
        });

        //#region Paging
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
                if (pageCount > 0)
                    $('.grdView tr:eq(2)').click();
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('.grdView tr:eq(2)').click();
        }
        //#endregion

        function onRefreshGrid() {
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpView.PerformCallback('refresh');
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrid();
            }, 0);
        }

        function onAfterPopupControlClosing() {
            cbpView.PerformCallback('refresh');
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <table width="100%">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top">
                <table class="tblEntryContent" style="width: 650px;">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 600px" />
                    </colgroup>
                    <tr>
                        <td>
                            <label>
                                <%=GetLabel("Tanggal") %></label>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateFrom" Width="120px" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateTo" Width="120px" />
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
                                Width="400px" Watermark="Search">
                                <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                <IntellisenseHints>
                                    <qis:QISIntellisenseHint Text="No Pembayaran" FieldName="PaymentNo" />
                                    <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                    <qis:QISIntellisenseHint Text="No Rekam Medis" FieldName="MedicalNo" />
                                    <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                </IntellisenseHints>
                            </qis:QISIntellisenseTextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="vertical-align: top">
                <table id="tblInfoWarning" runat="server">
                    <tr>
                        <td style="vertical-align: top" class="blink-alert">
                            <img height="80" src='<%= ResolveUrl("~/Libs/Images/Warning.png")%>' alt='' class="blink-alert" />
                        </td>
                        <td style="vertical-align: middle">
                            <label class="lblWarning" runat="server">
                                <%=GetLabel("Hati-hati !!!") %></label>
                            <br />
                            <label runat="server" style="font-style:italic; color:Red">
                                <%=GetLabel("Perubahan tanggal & jam pembayaran ini akan menyebabkan :") %></label>
                            <br />
                            <label runat="server" style="font-style:italic; color:Red">
                                <%=GetLabel("(1) Tanggal dalam nomor bayar akan berbeda dgn tanggal bayarnya.") %></label>
                            <br />
                            <label runat="server" style="font-style:italic; color:Red">
                                <%=GetLabel("(2) Jika sudah ada cetakan terbentuk, akan menyebabkan data di sistem dan cetakan menjadi berbeda.") %></label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="PaymentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="PaymentID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" />
                                        <asp:TemplateField HeaderText="No Pembayaran" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="150px">
                                            <ItemTemplate>
                                                <label class="lblNormal" style="font-weight: bold">
                                                    <%#:Eval("PaymentNo")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tanggal / Jam Pembayaran" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                                            <ItemTemplate>
                                                <label class="lblNormal">
                                                    <%#:Eval("PaymentDateInString")%></label>
                                                <br />
                                                <label class="lblNormal">
                                                    <%#:Eval("PaymentTime")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PaymentType" HeaderText="Jenis Pembayaran" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="170px" />
                                        <asp:TemplateField HeaderText="Informasi Pendaftaran" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <label class="lblNormal" style="font-weight: bold">
                                                    <%#:Eval("RegistrationNo")%></label>
                                                <br />
                                                <label class="lblNormal" style="font-style: italic">
                                                    <%#:Eval("DepartmentID")%></label>
                                                <br />
                                                <label class="lblNormal">
                                                    <%#:Eval("ServiceUnitName")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Informasi Pasien" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <label class="lblNormal">
                                                    <%#:Eval("PatientName")%></label>
                                                <br />
                                                <label class="lblNormal" style="font-style: italic">
                                                    <%#:Eval("MedicalNo")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Pembayaran" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="150px">
                                            <ItemTemplate>
                                                <label class="lblNormal">
                                                    <%#:Eval("TotalPaymentAmount", "{0:N2}")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-Width="60px">
                                            <ItemTemplate>
                                                <label class="lblLink lblDetail">
                                                    <%=GetLabel("UBAH") %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada informasi saldo persediaan di lokasi ini")%>
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
            </td>
        </tr>
    </table>
</asp:Content>
