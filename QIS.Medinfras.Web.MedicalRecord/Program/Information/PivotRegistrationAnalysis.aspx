<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master"
    AutoEventWireup="true" CodeBehind="PivotRegistrationAnalysis.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.PivotRegistrationAnalysis" %>

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
<%@ Register Src="~/Program/Information/PivotOptionCtl.ascx" TagName="PivotOptionCtl"
    TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Controls/PeriodeCtl.ascx" TagName="PeriodeCtl" TagPrefix="uc2" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnGenerate" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Generate")%>
        </div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=btnGenerate.ClientID %>').click(function () {
                showLoadingPanel();

                cbpView.PerformCallback('refresh');
            });
        });

        function onCboPeriodeValueChanged(s) {
            $tr = $(s.GetInputElement()).closest('tr').parent().closest('tr');
            var isNumberVisible = false;
            var isCustomPeriodVisible = false;
            if ((s.GetValue() > 'X106^049' && s.GetValue() < 'X106^054')
                || (s.GetValue() > 'X106^009' && s.GetValue() < 'X106^014')) {
                isNumberVisible = true;
                isCustomPeriodVisible = false;
            }
            else if (s.GetValue() == 'X106^090') {
                isNumberVisible = false;
                isCustomPeriodVisible = true;
            }
            else {
                isNumberVisible = false;
                isCustomPeriodVisible = false;
            }

            $txtValueNum = $tr.find('.txtValueNum');
            $tdCustomDate = $tr.find('.tdCustomDate');
            if (isNumberVisible)
                $txtValueNum.show();
            else
                $txtValueNum.hide();

            if (isCustomPeriodVisible) {
                $tdCustomDate.show();
                setDatePickerElement($tr.find('.txtValueDateFrom'));
                setDatePickerElement($tr.find('.txtValueDateTo'));
            }
            else
                $tdCustomDate.hide();
        }

        function onCbpViewEndCallback(s) {
            $('#divPivot').show();
            hideLoadingPanel();
        }
    </script>
    <uc2:PeriodeCtl ID="ctlPeriode" runat="server" />
    <table>
        <colgroup>
            <col style="width: 100px" />
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Template")%></label></td>
            <td><dxe:ASPxComboBox ID="cboLayoutTemplate" ClientInstanceName="cboLayoutTemplate" Width="100%" runat="server" /></td>
        </tr>
    </table>
    <hr />
    <div id="divPivot" style="display: none">
        <uc1:PivotOptionCtl ID="ctlPivotOption" runat="server" />
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 450px;">
                        <dx:ASPxPivotGrid ID="pvView" runat="server" ClientIDMode="AutoID" Width="100%" DataSourceID="odsPivotRegistration">
                            <Fields>
                                <dx:PivotGridField Area="RowArea" AreaIndex="0" FieldName="ServiceUnitName" Caption="Unit Pelayanan"
                                    ID="ServiceUnit">
                                    <CustomTotals>
                                        <dx:PivotGridCustomTotal SummaryType="Count" />
                                    </CustomTotals>
                                </dx:PivotGridField>
                                <dx:PivotGridField Area="FilterArea" AreaIndex="0" FieldName="VisitDate" ID="registrationDate"
                                    Caption="Tanggal" GroupInterval="DateDay" />
                                <dx:PivotGridField Area="FilterArea" AreaIndex="1" FieldName="VisitDate" ID="registrationWeek"
                                    Caption="Minggu" GroupInterval="DateWeekOfMonth" />
                                <dx:PivotGridField Area="FilterArea" AreaIndex="3" FieldName="ParamedicName" Caption="Tenaga Medis"
                                    ID="paramedicName" />
                                <dx:PivotGridField Area="FilterArea" AreaIndex="4" FieldName="SpecialtyName" Caption="Spesialisasi"
                                    ID="specialtyName" />
                                <dx:PivotGridField Area="FilterArea" AreaIndex="5" FieldName="ClassName" Caption="Kelas"
                                    ID="className" />
                                <dx:PivotGridField Area="FilterArea" AreaIndex="6" FieldName="CustomerType" Caption="Jenis Pembayar"
                                    ID="customerType" />
                                <dx:PivotGridField Area="FilterArea" AreaIndex="7" FieldName="BusinessPartnerName"
                                    Caption="Pembayar" ID="businessPartnerName" />
                                <dx:PivotGridField Area="FilterArea" AreaIndex="8" FieldName="VisitTypeName" Caption="Tipe Kunjungan"
                                    ID="visittypenameID" />
                                <dx:PivotGridField Area="FilterArea" AreaIndex="9" FieldName="Triage" Caption="Triage"
                                    ID="dischargeCondition" />
                                <dx:PivotGridField Area="FilterArea" AreaIndex="10" FieldName="DischargeCondition"
                                    Caption="Kondisi Keluar" ID="triage" />
                                <dx:PivotGridField Area="FilterArea" AreaIndex="10" FieldName="RegistrationStatus"
                                    Caption="Status Pendaftaran" ID="registrationStatus" />
                                <dx:PivotGridField Area="FilterArea" AreaIndex="11" FieldName="Sex" Caption="Jenis Kelamin"
                                    ID="sexID" />
                                <dx:PivotGridField Area="FilterArea" AreaIndex="5" FieldName="IsCancel" Caption="Batal Kunjungan"
                                    ID="isCancelVisit" HeaderStyle-HorizontalAlign="Right" />
                                <dx:PivotGridField Area="RowArea" AreaIndex="0" FieldName="DepartmentName" Caption="Departemen"
                                    ID="departmentName" TotalsVisibility="AutomaticTotals" />
                                <dx:PivotGridField Area="ColumnArea" AreaIndex="0" FieldName="VisitDate" ID="registrationYear"
                                    Caption="Tahun" GroupInterval="DateYear" />
                                <dx:PivotGridField Area="ColumnArea" AreaIndex="1" FieldName="VisitDate" ID="registrationMonth"
                                    Caption="Bulan" GroupInterval="DateMonth" />
                                <dx:PivotGridField Area="DataArea" AreaIndex="0" FieldName="IsNotCancel" Caption="Jumlah"
                                    ID="visitID" HeaderStyle-HorizontalAlign="Right" />
                                <dx:PivotGridField Area="DataArea" AreaIndex="1" FieldName="IsNewPatient" Caption="Pasien Baru"
                                    ID="isNewPatient" SummaryType="Sum" HeaderStyle-HorizontalAlign="Right" />
                                <dx:PivotGridField Area="DataArea" AreaIndex="2" FieldName="IsOldPatient" Caption="Pasien Lama"
                                    ID="isOldPatient" SummaryType="Sum" HeaderStyle-HorizontalAlign="Right" />
                            </Fields>
                            <OptionsPager RowsPerPage="20" />
                            <OptionsView ShowHorizontalScrollBar="True" />
                        </dx:ASPxPivotGrid>
                        <asp:HiddenField ID="hdnFilterExpression1" runat="server" Value="1 = 0" />
                        <asp:ObjectDataSource ID="odsPivotRegistration" runat="server" SelectMethod="GetvPivotRegistrationList"
                            TypeName="QIS.Medinfras.Data.Service.BusinessLayer">
                            <SelectParameters>
                                <asp:ControlParameter Name="filterExpression" ControlID="hdnFilterExpression1" Type="String"
                                    PropertyName="Value" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </div>
    <input type="hidden" value="RegistrasiPivot" id="hdnFileName" runat="server" />
</asp:Content>
