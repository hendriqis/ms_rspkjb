<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="DetailPiutangInformation.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.DetailPiutangInformation" %>

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
    <li id="btnRefresh" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><div>
            <%=GetLabel("Refresh")%></div>
    </li>
    <li id="btnDownload" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdownload.png")%>' alt="" /><div>
            <%=GetLabel("Download")%></div>
    </li>
    <li id="btnDownloadV2" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdownload.png")%>' alt="" /><div>
            <%=GetLabel("Download V2")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtPeriodFrom.ClientID %>');
            setDatePicker('<%=txtPeriodTo.ClientID %>');
        });

        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        $('#<%=btnDownload.ClientID %>').die('click');
        $('#<%=btnDownload.ClientID %>').live('click', function () {
            onCustomButtonClick('download');
        });

        $('#<%=btnDownloadV2.ClientID %>').die('click');
        $('#<%=btnDownloadV2.ClientID %>').live('click', function () {
            onCustomButtonClick('downloadv2');
        });

        function downloadDocument(stringparam) {
            var fileName = $('#<%=hdnFileName.ClientID %>').val() + '.CSV';
            var link = document.createElement("a");
            link.href = 'data:text/csv,' + encodeURIComponent(stringparam);
            link.download = fileName;
            link.click();
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGridView();
            }, 0);
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        function onAfterCustomClickSuccess(type, retval) {
            if (type == "download") {
                if (retval != "Download Failed") {
                    downloadDocument(retval);
                } else {
                    displayErrorMessageBox('Download Failed', 'Tidak ada data.');
                }
            } else if (type == "downloadv2") {
                if (retval != "Download Failed") {
                    downloadDocument(retval);
                } else {
                    displayErrorMessageBox('Download Failed', 'Tidak ada data.');
                }
            }
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnFileName" runat="server" value="" />
    <table class="tblContentArea" style="width: 100%">
        <colgroup>
            <col style="width: 70%" />
            <col style="width: 30%" />
        </colgroup>
        <tr>
            <td>
                <table>
                    <colgroup>
                        <col style="width: 110px" />
                        <col style="width: 150px" />
                        <col style="width: 350px" />
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <b>
                                    <%=GetLabel("Periode Invoice")%></b></label>
                        </td>
                        <td colspan="2">
                            <table width="100%" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" Width="120px" ID="txtPeriodFrom" CssClass="datepicker" />
                                                </td>
                                                <td style="width: 30px; text-align: center">
                                                    s/d
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" Width="120px" ID="txtPeriodTo" CssClass="datepicker" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <label class="lblNormal">
                                <b>
                                    <%=GetLabel("No. Invoice")%></b></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" Width="292px" ID="txtARInvoiceNo" />
                        </td>
                    </tr>
                </table>
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
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ARInvoiceNo" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("No. Invoice")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding: 3px">
                                                    <div>
                                                        <%#: Eval("ARInvoiceNo")%><br>
                                                        <%#: Eval("cfARInvoiceDate")%></div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("Penjamin Bayar")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding: 3px">
                                                    <div>
                                                        <%#: Eval("BusinessPartnerName")%><br>
                                                        <%#: Eval("BusinessPartnerName2")%></div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("No. Pembayaran")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding: 3px">
                                                    <div>
                                                        <%#: Eval("PaymentNo")%><br>
                                                        <%#: Eval("cfPaymentDate")%></div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("Data Registrasi")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    (<%#: Eval("MedicalNo")%>)
                                                    <%#: Eval("PatientName") %></div>
                                                <table cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 170px" />
                                                        <col style="width: 10px" />
                                                        <col style="width: 250px" />
                                                        <col style="width: 50px" />
                                                        <col style="width: 10px" />
                                                        <col style="width: 120px" />
                                                        <col style="width: 10px" />
                                                        <col style="width: 10px" />
                                                        <col style="width: 120px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                            <%=GetLabel("No. Registrasi")%>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <%#: Eval("RegistrationNo")%>
                                                        </td>
                                                        <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                            <%=GetLabel("Tanggal")%>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <%#: Eval("cfRegistrationDate")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                            <%=GetLabel("Unit Pelayanan")%>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <%#: Eval("VisitServiceUnitName")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("Item")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding: 3px">
                                                    <div>
                                                        (<%#: Eval("ItemCode")%>)
                                                        <%#: Eval("ItemName1")%><br>
                                                        <%#: Eval("ChargedQuantity")%>
                                                        <%#: Eval("ItemUnit")%>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="110px" HeaderStyle-HorizontalAlign="Right"
                                            ItemStyle-HorizontalAlign="Right">
                                            <HeaderTemplate>
                                                <%=GetLabel("Amount")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding: 3px">
                                                    <div>
                                                        Patient :
                                                        <%#: Eval("cfPatientAmount")%><br>
                                                        Payer :
                                                        <%#: Eval("cfPayerAmount")%>
                                                    </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="110px" HeaderStyle-HorizontalAlign="Right"
                                            ItemStyle-HorizontalAlign="Right">
                                            <HeaderTemplate>
                                                <%=GetLabel("Corporate Amount")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding: 3px">
                                                    <div>
                                                        C1 : <%#: Eval("cfTotalCorporateAmount1")%><br>
                                                        C2 : <%#: Eval("cfTotalCorporateAmount2")%>
                                                    </div>
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
            </td>
        </tr>
    </table>
</asp:Content>
