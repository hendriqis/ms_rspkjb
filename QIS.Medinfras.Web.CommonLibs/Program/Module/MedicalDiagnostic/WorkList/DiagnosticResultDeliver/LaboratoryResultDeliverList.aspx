<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="LaboratoryResultDeliverList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.LaboratoryResultDeliverList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
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
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtResultDateFrom.ClientID %>');
            setDatePicker('<%=txtResultDateTo.ClientID %>');

            $('#<%=txtResultDateFrom.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            $('#<%=txtResultDateTo.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
        });

        $('.imgResultDt.imgResultDt').die('click');
        $('.imgResultDt.imgResultDt').live('click', function (evt) {
            var selectedID = $(this).closest('tr').find('.keyField').html();

            var paramResult = selectedID;
            var url = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/WorkList/DiagnosticResultDeliver/LaboratoryResultDtInfoCtl.ascx");
            openUserControlPopup(url, paramResult, 'Detail Hasil Pemeriksaan', 1200, 400);
        });

        $('.imgDelivered.imgDelivered').die('click');
        $('.imgDelivered.imgDelivered').live('click', function (evt) {
            var selectedID = $(this).closest('tr').find('.keyField').html();

            var paramResult = selectedID;
            var url = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/WorkList/DiagnosticResultDeliver/LaboratoryResultDeliverListDtCtl.ascx");
            openUserControlPopup(url, paramResult, 'Serah Terima Hasil Pemeriksaan', 1200, 400);
        });

        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        function oncboVisitDepartmentValueChanged(evt) {
            $('#<%=hdnVisitHealthcareServiceUnitID.ClientID %>').val('');
            $('#<%=txtVisitServiceUnitCode.ClientID %>').val('');
            $('#<%=txtVisitServiceUnitName.ClientID %>').val('');
            onRefreshGrid();
        }

        //#region Service Unit
        $('#lblVisitServiceUnit.lblLink').live('click', function () {
            var filterExpression = '';
            var oDepartmentID = cboVisitDepartment.GetValue();
            if (oDepartmentID != '') {
                filterExpression = "IsUsingRegistration = 1 AND DepartmentID = '" + oDepartmentID + "' AND IsDeleted = 0";
                openSearchDialog('serviceunitperhealthcare', filterExpression, function (value) {
                    $('#<%=txtVisitServiceUnitCode.ClientID %>').val(value);
                    ontxtVisitServiceUnitCodeChanged(value);
                });
            } else {
                alert("Harap pilih asal department terlebih dahulu.");
            }
        });

        $('#<%=txtVisitServiceUnitCode.ClientID %>').live('change', function () {
            ontxtVisitServiceUnitCodeChanged($(this).val());
        });

        function ontxtVisitServiceUnitCodeChanged(value) {
            var filterExpression = "ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnVisitHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=txtVisitServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                }
                else {
                    $('#<%=hdnVisitHealthcareServiceUnitID.ClientID %>').val('');
                    $('#<%=txtVisitServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtVisitServiceUnitName.ClientID %>').val('');
                }
                onRefreshGrid();
            });
        }
        //#endregion

        //#region Paging
        var pageCountAvailable = parseInt('<%=PageCount %>');
        $(function () {
            setPagingDetailItem(pageCountAvailable);
        });

        function setPagingDetailItem(pageCount) {
            if (pageCount > 0)
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, 8);
        }

        function onCbpViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPagingDetailItem(pageCount);
            }
            else {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            }
            hideLoadingPanel();
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
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }

    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <table class="tblContentArea" style="width: 100%">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table>
                    <colgroup>
                        <col style="width: 200px" />
                        <col style="width: 500px" />
                        <col />
                    </colgroup>
                    <tr id="trContractDate" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Hasil Dari - Sampai")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtResultDateFrom" CssClass="datepicker" Width="120px" runat="server" /><%=GetLabel(" s/d ")%>
                            <asp:TextBox ID="txtResultDateTo" CssClass="datepicker" Width="120px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Asal Department")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboVisitDepartment" ClientInstanceName="cboVisitDepartment"
                                Width="350px" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e) { oncboVisitDepartmentValueChanged(e); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblVisitServiceUnit">
                                <%=GetLabel("Asal Unit Pelayanan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnVisitHealthcareServiceUnitID" runat="server" value="" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtVisitServiceUnitCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVisitServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
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
                                Width="350px" Watermark="Search">
                                <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                <IntellisenseHints>
                                    <qis:QISIntellisenseHint Text="PatientName" FieldName="PatientName" />
                                    <qis:QISIntellisenseHint Text="MedicalNo" FieldName="MedicalNo" />
                                    <qis:QISIntellisenseHint Text="RegistrationNo" FieldName="RegistrationNo" />
                                </IntellisenseHints>
                            </qis:QISIntellisenseTextBox>
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
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <input type="hidden" id="hdnFilterExpression" value="" runat="server" />
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" HeaderText="Informasi Hasil">
                                            <ItemTemplate>
                                                <label style="font-size: x-small; font-style: italic;">
                                                    <%=GetLabel("Tgl/Jam Hasil : ")%>
                                                </label>
                                                <label style="font-size: small;">
                                                    <%#:Eval("cfResultDateInString") %><%=GetLabel(" | ")%><%#:Eval("ResultTime") %>
                                                </label>
                                                <br />
                                                <label style="font-size: x-small; font-style: italic;">
                                                    <%=GetLabel("Dibuat Oleh : ")%>
                                                </label>
                                                <label style="font-size: small;">
                                                    <%#:Eval("CreatedByName") %>
                                                </label>
                                                <br />
                                                <label style="font-size: x-small; font-style: italic;">
                                                    <%=GetLabel("Dibuat Pada : ")%>
                                                </label>
                                                <label style="font-size: small;">
                                                    <%#:Eval("cfCreatedDateInString") %>
                                                </label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" HeaderText="Informasi Transaksi & Order">
                                            <ItemTemplate>
                                                <label style="font-size: x-small; font-style: italic;">
                                                    <%=GetLabel("No. Transaksi : ")%>
                                                </label>
                                                <label style="font-size: medium; font-style: italic; font-weight: bold">
                                                    <%#:Eval("ChargesTransactionNo") %>
                                                </label>
                                                <br />
                                                <label style="font-size: x-small; font-style: italic;">
                                                    <%=GetLabel("Tgl/Jam Transaksi : ")%>
                                                </label>
                                                <label style="font-size: small;">
                                                    <%#:Eval("cfChargesTransactionDateInString") %><%=GetLabel(" | ")%><%#:Eval("ChargesTransactionTime") %>
                                                </label>
                                                <br />
                                                <label style="font-size: x-small; font-style: italic;">
                                                    <%=GetLabel("No. Order : ")%>
                                                </label>
                                                <label style="font-size: small; font-style: italic; font-weight: bold">
                                                    <%#:Eval("TestOrderNo") %>
                                                </label>
                                                <br />
                                                <label style="font-size: x-small; font-style: italic;">
                                                    <%=GetLabel("Tgl/Jam Order : ")%>
                                                </label>
                                                <label style="font-size: small;">
                                                    <%#:Eval("cfTestOrderDateInString") %><%=GetLabel(" | ")%><%#:Eval("TestOrderTime") %>
                                                </label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Left" HeaderText="Informasi Kunjungan">
                                            <ItemTemplate>
                                                <label style="font-size: x-small; font-style: italic;">
                                                    <%=GetLabel("Pasien : ")%>
                                                </label>
                                                <label style="font-size: medium; font-style: italic; font-weight: bold">
                                                    <%#:Eval("cfPatientInfo") %>
                                                </label>
                                                <br />
                                                <label style="font-size: x-small; font-style: italic;">
                                                    <%=GetLabel("No. Registrasi : ")%>
                                                </label>
                                                <label style="font-size: small; font-style: italic; font-weight: bold">
                                                    <%#:Eval("RegistrationNo") %>
                                                </label>
                                                <br />
                                                <label style="font-size: x-small; font-style: italic;">
                                                    <%=GetLabel("Tgl/Jam Registrasi : ")%>
                                                </label>
                                                <label style="font-size: small;">
                                                    <%#:Eval("cfRegistrationDateInString") %><%=GetLabel(" | ")%><%#:Eval("RegistrationTime") %>
                                                </label>
                                                <br />
                                                <label style="font-size: x-small; font-style: italic;">
                                                    <%=GetLabel("Asal Registrasi : ")%>
                                                </label>
                                                <label style="font-size: small;">
                                                    <%#:Eval("VisitDepartmentID") %><%=GetLabel(" | ")%><%#:Eval("VisitServiceUnitName") %>
                                                </label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" HeaderText="Detail Hasil">
                                            <ItemTemplate>
                                                <div style="text-align: center">
                                                    <img class="imgResultDt <%#"imgResultDt"%>" title='<%=GetLabel("Detail Hasil")%>'
                                                        src='<%# ResolveUrl("~/Libs/Images/Button/list.png")%>' alt="" style="margin-right: 2px" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" HeaderText="Proses Hasil">
                                            <ItemTemplate>
                                                <div style="text-align: center">
                                                    <img class="imgDelivered <%#"imgDelivered"%>" title='<%=GetLabel("Proses Serah Terima Hasil")%>'
                                                        src='<%# ResolveUrl("~/Libs/Images/Button/link.png")%>' alt="" style="margin-right: 2px" />
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
