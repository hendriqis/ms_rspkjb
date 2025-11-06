<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPList.master"
    AutoEventWireup="true" CodeBehind="PivotPatientChargesDt.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PivotPatientChargesDt" %>

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
<%@ Register Src="~/Libs/Program/Information/PivotOptionCtl.ascx" TagName="PivotOptionCtl"
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

            //#region Item Group
            function onGetItemGroupFilterExpression() {
                var filterExpression = "GCItemType = '" + $('#<%=hdnGCItemType.ClientID %>').val() + "' AND IsDeleted = 0";
                return filterExpression;
            }

            $('#lblItemGroup.lblLink').click(function () {
                openSearchDialog('itemgroup', onGetItemGroupFilterExpression(), function (value) {
                    $('#<%=txtItemGroupCode.ClientID %>').val(value);
                    onTxtItemGroupCodeChanged(value);
                });
            });

            $('#<%=txtItemGroupCode.ClientID %>').change(function () {
                onTxtItemGroupCodeChanged($(this).val());
            });

            function onTxtItemGroupCodeChanged(value) {
                var filterExpression = onGetItemGroupFilterExpression() + " AND ItemGroupCode = '" + value + "'";
                Methods.getObject('GetItemGroupMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnItemGroupID.ClientID %>').val(result.ItemGroupID);
                        $('#<%=txtItemGroupName.ClientID %>').val(result.ItemGroupName1);
                    }
                    else {
                        $('#<%=hdnItemGroupID.ClientID %>').val('');
                        $('#<%=txtItemGroupCode.ClientID %>').val('');
                        $('#<%=txtItemGroupName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
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
    <input type="hidden" id="hdnGCItemType" runat="server" />
    <uc2:PeriodeCtl ID="ctlPeriode" runat="server" />
    <table>
        <colgroup>
            <col style="width: 100px" />
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Asal Pasien")%></label></td>
            <td><dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" Width="100%" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblLink" id="lblItemGroup"><%=GetLabel("Kelompok Item")%></label></td>
            <td>
                <input type="hidden" id="hdnItemGroupID" value="" runat="server" />
                <table style="width:100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width:100px"/>
                        <col style="width:3px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td><asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" /></td>
                        <td>&nbsp;</td>
                        <td><asp:TextBox ID="txtItemGroupName" Width="100%" runat="server" ReadOnly="true"/></td>
                    </tr>
                </table>
            </td>
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
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 540px;">
                        <dx:ASPxPivotGrid ID="pvView" runat="server" ClientIDMode="AutoID" Width="100%" DataSourceID="odsPivotRegistration">
                            <Fields>
                                <dx:PivotGridField Area="RowArea" AreaIndex="0" FieldName="ItemName1" Caption="Jenis Pemeriksaan"
                                    ID="itemName">
                                    <CustomTotals>
                                        <dx:PivotGridCustomTotal SummaryType="Count" />
                                    </CustomTotals>
                                </dx:PivotGridField>
                                <dx:PivotGridField Area="ColumnArea" AreaIndex="0" FieldName="DepartmentName" ID="fieldEventDate0"
                                    Caption="Asal Pasien" />
                                <dx:PivotGridField Area="ColumnArea" AreaIndex="1" FieldName="ClassName" ID="fieldEventDate1"
                                    Caption="Kelas" />
                                <dx:PivotGridField Area="DataArea" AreaIndex="1" FieldName="ItemCode" Caption="Jumlah"
                                    ID="itemCode" SummaryType="Count" />
                                <dx:PivotGridField Area="DataArea" AreaIndex="2" FieldName="Tariff" Caption="Harga"
                                    ID="tariff" SummaryType="Average" Options-ShowGrandTotal="False"  Options-ShowTotals="False" CellFormat-FormatType="Numeric" CellFormat-FormatString="n2" />
                                <dx:PivotGridField Area="DataArea" AreaIndex="3" FieldName="LineAmount" Caption="Total"
                                    ID="lineAmount" SummaryType="Sum" CellFormat-FormatType="Numeric" CellFormat-FormatString="n2" />
                            </Fields>
                            <OptionsPager RowsPerPage="10" />
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
