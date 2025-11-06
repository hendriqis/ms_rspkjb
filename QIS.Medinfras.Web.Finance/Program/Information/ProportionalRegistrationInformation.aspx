<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="ProportionalRegistrationInformation.aspx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ProportionalRegistrationInformation" %>

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

        //#region Service Unit
        function getHealthcareServiceUnitFilterExpression() {
            var filterExpression = "IsUsingRegistration = 1 AND HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + cboDepartment.GetValue() + "' AND IsDeleted = 0";
            return filterExpression;
        }

        $('#lblServiceUnit.lblLink').live('click', function () {
            openSearchDialog('serviceunitperhealthcare', getHealthcareServiceUnitFilterExpression(), function (value) {
                $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                onTxtServiceUnitCodeChanged(value);
            });
        });

        $('#<%=txtServiceUnitCode.ClientID %>').change(function () {
            onTxtServiceUnitCodeChanged($(this).val());
        });

        function onTxtServiceUnitCodeChanged(value) {
            var filterExpression = getHealthcareServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
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

        function onRefreshGrdReg() {
            $('#<%=hdnFilterExpressionQuickSearchReg.ClientID %>').val(txtSearchViewReg.GenerateFilterExpression());
            cbpView.PerformCallback('refresh');
        }

        $('#<%=chkIsPreviousEpisodePatient.ClientID %>').die();
        $('#<%=chkIsPreviousEpisodePatient.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtPeriodFrom.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtPeriodTo.ClientID %>').attr('readonly', 'readonly');
            }
            else {
                $('#<%=txtPeriodFrom.ClientID %>').removeAttr('readonly');
                $('#<%=txtPeriodTo.ClientID %>').removeAttr('readonly');
            }
            onRefreshGrdReg();
        });

        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            onRefreshGrdReg();
        });

        function onTxtSearchViewRegSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrdReg();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
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

        $('.lblTransaction.lblLink').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var regNo = $(this).closest('tr').find('.registrationNo').html();
            var medicalNo = $(this).closest('tr').find('.medicalNo').html();
            var patientName = $(this).closest('tr').find('.patientName').html();
            var param = id + '|' + regNo + '|' + medicalNo + '|' + patientName;
            var title = 'Detail Transaksi ' + regNo + '(' + medicalNo + ') ' + patientName;
            var url = ResolveUrl("~/Program/Information/ProportionalRegistrationInformationTransactionDetailCtl.ascx");
            openUserControlPopup(url, param, title, 1200, 300);
        });

        $('#<%=txtPeriodFrom.ClientID %>').live('change', function () {
            var start = $('#<%=txtPeriodFrom.ClientID %>').val();
            var end = $('#<%=txtPeriodTo.ClientID %>').val();

            $('#<%=txtPeriodFrom.ClientID %>').val(validateDateFromTo(start, end));
        });

        $('#<%=txtPeriodTo.ClientID %>').live('change', function () {
            var start = $('#<%=txtPeriodFrom.ClientID %>').val();
            var end = $('#<%=txtPeriodTo.ClientID %>').val();

            $('#<%=txtPeriodTo.ClientID %>').val(validateDateToFrom(start, end));
        });

        function onCboDepartmentValueChanged(s) {
            $('#<%=hdnServiceUnitID.ClientID %>').val('');
            $('#<%=txtServiceUnitCode.ClientID %>').val('');
            $('#<%=txtServiceUnitName.ClientID %>').val('');
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchReg" runat="server" />
    <table class="tblContentArea" style="width: 100%">
        <colgroup>
            <col style="width: 70%" />
            <col style="width: 30%" />
        </colgroup>
        <tr>
            <td>
                <table>
                    <colgroup>
                        <col style="width: 130px" />
                        <col style="width: 250px" />
                        <col style="width: 350px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Registrasi")%></label>
                        </td>
                        <td colspan="3">
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
                                                    <asp:CheckBox ID="chkIsPreviousEpisodePatient" runat="server" Checked="false" Text="Abaikan Tanggal" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Department")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" Width="100%"
                                runat="server">
                            <clientsideevents valuechanged="function(s){ onCboDepartmentValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <colgroup>
                        <col style="width: 130px" />
                        <col style="width: 100px" />
                        <col style="width: 350px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <label class="lblNormal lblLink" id="lblServiceUnit">
                                <%=GetLabel("Unit Perawatan")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnServiceUnitID" runat="server" value="" />
                            <asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <table>
                    <colgroup>
                        <col style="width: 130px" />
                        <col style="width: 460px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Quick Filter")%></label>
                        </td>
                        <td>
                            <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchViewReg"
                                ID="txtSearchViewReg" Width="100%" Watermark="Search">
                                <ClientSideEvents SearchClick="function(s){ onTxtSearchViewRegSearchClick(s); }" />
                                <IntellisenseHints>
                                    <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                    <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                    <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
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
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                            position: relative; font-size: 0.95em;">
                            <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                <Columns>
                                    <asp:BoundField DataField="RegistrationID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:BoundField DataField="RegistrationNo" HeaderStyle-CssClass="hiddenColumn registrationNo"
                                        ItemStyle-CssClass="hiddenColumn registrationNo" />
                                    <asp:BoundField DataField="MedicalNo" HeaderStyle-CssClass="hiddenColumn medicalNo"
                                        ItemStyle-CssClass="hiddenColumn medicalNo" />
                                    <asp:BoundField DataField="PatientName" HeaderStyle-CssClass="hiddenColumn patientName"
                                        ItemStyle-CssClass="hiddenColumn patientName" />
                                    <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="5%">
                                        <HeaderTemplate>
                                            <div style="text-align: left; padding-left: 3px">
                                                <%=GetLabel("No Registrasi")%>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="padding: 3px">
                                                <div>
                                                    <%#: Eval("RegistrationNo")%><BR>
                                                    <b>Status : <%#: Eval("RegistrationStatus")%></b></div>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="3%">
                                        <HeaderTemplate>
                                            <div style="text-align: center">
                                                <%=GetLabel("Tanggal Registrasi")%>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="text-align: center">
                                                <div>
                                                    <%#: Eval("cfRegistrationDateInString")%></div>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="10%">
                                        <HeaderTemplate>
                                            <div style="text-align: left; padding-left: 3px">
                                                <%=GetLabel("Unit Pelayanan")%>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="padding: 3px">
                                                <div>
                                                    (<%#: Eval("DepartmentID")%>)
                                                    <%#: Eval("ServiceUnitName")%></div>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="15%">
                                        <HeaderTemplate>
                                            <div style="text-align: left; padding-left: 3px">
                                                <%=GetLabel("Data Pasien")%>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="padding: 3px">
                                                <div>
                                                    (<%#: Eval("MedicalNo")%>)
                                                    <%#: Eval("PatientName")%></div>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="10%">
                                        <HeaderTemplate>
                                            <div style="text-align: left; padding-left: 3px">
                                                <%=GetLabel("Penjamin Bayar")%>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="padding: 3px">
                                                <div>
                                                    <%#: Eval("BusinessPartnerName")%>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="5%">
                                        <HeaderTemplate>
                                            <div style="text-align: center">
                                                <%=GetLabel("Detail Transaksi")%>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="text-align: center">
                                                <div>
                                                    <label class="lblTransaction lblLink" runat="server" id="lblTransaction">
                                                        Detail Transaksi
                                                    </label>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top"></ItemStyle>
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
    </table>
    </div>
</asp:Content>
