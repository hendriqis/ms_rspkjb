<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPList.master"
    AutoEventWireup="true" CodeBehind="PivotPatientChargesDt.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.PivotPatientChargesDt" %>

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
                                <dx:PivotGridField Area="ColumnArea" AreaIndex="0" FieldName="TransactionDate" ID="fieldEventDate0"
                                    Caption="Year" GroupInterval="DateYear" />
                                <dx:PivotGridField Area="ColumnArea" AreaIndex="1" FieldName="TransactionDate" ID="fieldEventDate1"
                                    Caption="Month" GroupInterval="DateMonth" />
                                <dx:PivotGridField Area="FilterArea" AreaIndex="2" FieldName="DepartmentID" Caption="Department"
                                    ID="departmentID" />
                                <dx:PivotGridField Area="DataArea" AreaIndex="3" FieldName="LineAmount" Caption="Biaya"
                                    ID="lineAmount" SummaryType="Sum" CellFormat-FormatString="{0:N}" />
                                <dx:PivotGridField Area="DataArea" AreaIndex="3" FieldName="ItemCode" Caption="Jumlah"
                                    ID="itemCode" SummaryType="Count" />
                                <dx:PivotGridField Area="FilterArea" AreaIndex="4" FieldName="ItemName1" Caption="Nama Item"
                                    ID="itemName" />
                                <dx:PivotGridField Area="FilterArea" AreaIndex="5" FieldName="ItemType" Caption="Tipe Item"
                                    ID="itemType" />
                            </Fields>
                            <OptionsPager RowsPerPage="20" />
                            <OptionsView ShowHorizontalScrollBar="True" />
                        </dx:ASPxPivotGrid>
                        <asp:HiddenField ID="hdnFilterExpression1" runat="server" Value="" />
                        <asp:ObjectDataSource ID="odsPivotRegistration" runat="server" SelectMethod="GetvPatientChargesDtPivotList"
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
    <input type="hidden" value="PatienChargesPivot" id="hdnFileName" runat="server" />
</asp:Content>
