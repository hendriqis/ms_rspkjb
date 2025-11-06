<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="PiutangInformation.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.PiutangInformation" %>

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
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Business Partners
        function onGetPayerFilterExpression() {
            var filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#lblPayer.lblLink').live('click', function () {
            openSearchDialog('payer', onGetPayerFilterExpression(), function (value) {
                $('#<%=txtPayerCode.ClientID %>').val(value);
                onTxtPayerCodeChanged(value);
            });
        });

        $('#<%=txtPayerCode.ClientID %>').live('change', function () {
            onTxtPayerCodeChanged($(this).val());
        });

        function onTxtPayerCodeChanged(value) {
            var filterExpression = onGetPayerFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
            Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPayerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%=txtPayerCode.ClientID %>').val(result.BusinessPartnerCode);
                    $('#<%=txtPayerName.ClientID %>').val(result.BusinessPartnerName);
                }
                else {
                    $('#<%=hdnPayerID.ClientID %>').val('');
                    $('#<%=txtPayerCode.ClientID %>').val('');
                    $('#<%=txtPayerName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onCboPeriodeTypeValueChanged(s) {
            var periodeType = cboPeriodeType.GetValue();
            if (periodeType == "1") {
                $('#<%:trStatusInvoice.ClientID %>').attr('style', 'display:none');
            }
            else {
                $('#<%:trStatusInvoice.ClientID %>').removeAttr('style');
            }
        }

        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpView.PerformCallback('refresh');
        });

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                cbpView.PerformCallback('refresh');
            }, 0);
        }

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

    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <div>
        <table class="tblContentArea" style="width: 100%">
            <colgroup>
                <col style="width: 70%" />
                <col style="width: 30%" />
            </colgroup>
            <tr>
                <td>
                    <table>
                        <colgroup>
                            <col style="width: 100px" />
                            <col style="width: 150px" />
                            <col style="width: 350px" />
                            <col style="width: 100px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <b>
                                        <%=GetLabel("Periode")%></b></label>
                            </td>
                            <td colspan="3">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboPeriodeType" ClientInstanceName="cboPeriodeType" Width="100%"
                                                runat="server">
                                                <ClientSideEvents ValueChanged="function(s,e){ onCboPeriodeTypeValueChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
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
                                <label class="lblNormal lblLink" id="lblPayer">
                                    <%=GetLabel("Instansi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnPayerID" runat="server" value="" />
                                <asp:TextBox ID="txtPayerCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtPayerName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr id="trStatusInvoice" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Filter Invoice")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboStatusInvoice" Width="100%" ClientInstanceName="cboStatusInvoice"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Quick Filter")%></label>
                            </td>
                            <td colspan="2">
                                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                    Width="100%" Watermark="Search">
                                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                    <IntellisenseHints>
                                        <qis:QISIntellisenseHint Text="RegistrationNo" FieldName="RegistrationNo" />
                                        <qis:QISIntellisenseHint Text="PatientName" FieldName="PatientName" />
                                        <qis:QISIntellisenseHint Text="PaymentNo" FieldName="PaymentNo" />
                                        <qis:QISIntellisenseHint Text="ARInvoiceNo" FieldName="ARInvoiceNo" />
                                        <qis:QISIntellisenseHint Text="ARReceivingNo" FieldName="ARReceivingNo" />
                                    </IntellisenseHints>
                                </qis:QISIntellisenseTextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <td colspan="2">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:panelcontent id="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="PaymentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("Instansi")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding: 3px">
                                                    <div>
                                                        <%#: Eval("BusinessPartnerName")%>
                                                        (<%#: Eval("BusinessPartnerCode")%>)</div>
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
                                                            <%#: Eval("cfRegistrationDateInString")%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                            <%=GetLabel("Department")%>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <%#: Eval("DepartmentID")%>
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
                                                            <%#: Eval("ServiceUnitName")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("No. Pengakuan Piutang")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding: 3px">
                                                    <div>
                                                        <%#: Eval("PaymentNo")%><br />
                                                        <%#: Eval("cfPaymentDateInString")%></div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("No. Invoice")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding: 3px">
                                                    <div>
                                                        <%#: Eval("ARInvoiceNo")%><br />
                                                        <%#: Eval("cfInvoiceDate")%></div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("No. Penerimaan Piutang")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding: 3px">
                                                    <div>
                                                        <%#: Eval("ARReceivingNo")%></div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                            <HeaderTemplate>
                                                <%=GetLabel("Nilai Piutang")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding: 3px">
                                                    <div>
                                                        <%#: Eval("cfPaymentAmount")%><br />
                                                    </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:panelcontent>
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
    </div>
</asp:Content>
