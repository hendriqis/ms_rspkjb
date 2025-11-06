<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master"
    AutoEventWireup="true" CodeBehind="BridgingINACBGs.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.BridgingINACBGs" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtFromRegistrationDate.ClientID %>');
            $('#<%=txtFromRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtToRegistrationDate.ClientID %>');
            $('#<%=txtToRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtFromRegistrationDate.ClientID %>').change(function (evt) {
                onRefreshGridView();
            });

            $('#<%=txtToRegistrationDate.ClientID %>').change(function (evt) {
                onRefreshGridView();
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });

            //#region Service Unit
            function getHealthcareServiceUnitFilterExpression() {
                var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + cboPatientFrom.GetValue() + "'"; ;
                return filterExpression;
            }

            $('#lblServiceUnit.lblLink').click(function () {
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
                    onRefreshGridView();
                });
            }
            //#endregion
        });

        function onCboPatientFromValueChanged() {
            $('#<%=hdnServiceUnitID.ClientID %>').val('');
            $('#<%=txtServiceUnitCode.ClientID %>').val('');
            $('#<%=txtServiceUnitName.ClientID %>').val('');
            onRefreshGridView();
        }

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                refreshGrdRegisteredPatient();
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                    onRefreshGridView();
            }, 0);
        }

        function onCboResultTypeValueChanged() {
            onRefreshGridView();
        }
    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <div style="padding: 15px">
        <input type="hidden" id="hdnVisitID" runat="server" value="" />
        <input type="hidden" id="hdnSelectedMember" runat="server" />
        <div class="pageTitle">
            Bridging to INACBGs</div>
        <%--<div class="pageTitle"><%=GetMenuCaption()%></div>--%>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 50%">
                        <colgroup>
                            <col style="width: 30%" />
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
                                        <col style="width: 3px" />
                                        <col style="width: 145px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtFromRegistrationDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("s/d") %>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToRegistrationDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Asal Pasien")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPatientFrom" ClientInstanceName="cboPatientFrom" runat="server"
                                    Width="100%">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboPatientFromValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblNormal" id="lblServiceUnit">
                                    <%=GetLabel("Unit Pelayanan")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="99%" runat="server" />
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
                                    Width="435px" Watermark="Search">
                                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                    <IntellisenseHints>
                                        <qis:QISIntellisenseHint Text="No Pendaftaran" FieldName="RegistrationNo" />
                                        <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                        <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                    </IntellisenseHints>
                                </qis:QISIntellisenseTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Status Pendaftaran")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboRegistrationStatus" ClientInstanceName="cboRegistrationStatus"
                                    Width="150px" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onRefreshGridView(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tampilan Hasil")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboResultType" ClientInstanceName="cboResultType" Width="150px"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboResultTypeValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="position: relative;" id="divView">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel ID="pnlView" CssClass="pnlContainerGrid" runat="server">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="PaymentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-Width="50px">
                                                    <HeaderTemplate>
                                                        <input type="checkbox" id="chkCheckAll" style="text-align: center;" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="PaymentNo" HeaderText="No Piutang" HeaderStyle-Width="140px"
                                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="PaymentDateInString" HeaderText="Tanggal Piutang" HeaderStyle-Width="120px"
                                                    ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="RegistrationNo" HeaderText="No Registrasi" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="PaymentDateInString" HeaderText="Tanggal Piutang" HeaderStyle-Width="130px"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="TotalPaymentAmount" HeaderText="Total Piutang" HeaderStyle-HorizontalAlign="Right"
                                                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="200px" />
                                                <asp:TemplateField HeaderStyle-Width="150px" HeaderText="No Referensi" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <table style="width: 90%" cellpadding="0" cellspacing="0">
                                                            <colgroup>
                                                                <col style="width: 50px" />
                                                                <col style="width: 5px" />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <input type="text" class="txtReferenceNoPrefix" style="width: 100%;" />
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <input type="text" class="txtReferenceNo" style="width: 100%;" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Data Tidak Tersedia")%>
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
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
