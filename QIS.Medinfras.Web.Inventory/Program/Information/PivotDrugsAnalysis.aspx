<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master"
    AutoEventWireup="true" CodeBehind="PivotDrugsAnalysis.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.PivotDrugsAnalysis" %>

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

<%@ Register Src="~/Program/Information/PivotOptionCtl.ascx" TagName="PivotOptionCtl" TagPrefix="uc1" %>
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

            function onGetFilterExpressionItemProduct() {
                return "<%=OnGetFilterExpressionItemProduct() %>";
            }

            $('#lblGroupItem.lblLink').click(function () {
                openSearchDialog('itemgroup', onGetFilterExpressionItemProduct(), function (value) {
                    $('#<%=txtItemGroupCode.ClientID %>').val(value);
                    onTxtItemCodeChanged(value);
                });
            });

            $('#<%=txtItemGroupCode.ClientID %>').change(function () {
                onTxtItemCodeChanged($(this).val());
            });

            function onTxtItemCodeChanged(value) {
                var filterExpression = onGetFilterExpressionItemProduct() + " AND ItemGroupCode = '" + value + "'";
                Methods.getObject('GetItemGroupMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnItemGroupID.ClientID %>').val(result.ItemGroupID);
                        $('#<%=txtItemGroupName.ClientID %>').val(result.ItemGroupName1);
                    }
                    else {
                        $('#<%=hdnItemGroupID.ClientID %>').val('');
                        $('#<%=txtItemGroupName.ClientID %>').val('');
                        $('#<%=txtItemGroupCode.ClientID %>').val('');
                    }
                });
            }
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
    <input type="hidden" value="" id="hdnHealthcareID" runat="server" />
    <uc2:PeriodeCtl ID="ctlPeriode" runat="server" />
    <table>
        <colgroup>
            <col width="100px" />
            <col width="400px" />
        </colgroup>
        <tr>
            <td></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblLink" id="lblGroupItem"><%=GetLabel("Kelompok Item")%></label></td>
            <td>
                <input type="hidden" id="hdnItemGroupID" runat="server" value="" />
                <table style="width:100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width:100px"/>
                        <col style="width:3px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td><asp:TextBox ID="txtItemGroupCode" CssClass="required" Width="100%" runat="server" /></td>
                        <td>&nbsp;</td>
                        <td><asp:TextBox ID="txtItemGroupName" ReadOnly="true" Width="100%" runat="server" /></td>
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
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 500px;">
                        <dx:ASPxPivotGrid ID="pvView" runat="server" ClientIDMode="AutoID" Width="100%" DataSourceID="odsPivotItemBalance">
                            <OptionsPager RowsPerPage="10" />
                            <Fields>
                                <dx:PivotGridField Area="RowArea" AreaIndex="1" FieldName="ItemName1" Caption="Item" ID="pvtItemName1"/>
                                <dx:PivotGridField Area="RowArea" AreaIndex="2" FieldName="ItemUnit" Caption="Satuan" ID="PivotItemUnit"/>
                                <dx:PivotGridField Area="ColumnArea" AreaIndex="1" FieldName="LocationName" Caption="Lokasi" ID="PivotLocationName"/>
                                <dx:PivotGridField Area="DataArea" AreaIndex="1" FieldName="QuantityBegin" Caption="Stok Awal" ID="PivotQuantityBegin" CellFormat-FormatString="{0:N2}" CellFormat-FormatType="Custom" />
                                <dx:PivotGridField Area="DataArea" AreaIndex="2" FieldName="Mutasi" Caption="Mutasi" ID="PivotMutasi" CellFormat-FormatString="{0:N2}" CellFormat-FormatType="Custom" />
                                <dx:PivotGridField Area="DataArea" AreaIndex="3" FieldName="Keluar" Caption="Keluar" ID="PivotKeluar" CellFormat-FormatString="{0:N2}" CellFormat-FormatType="Custom" />
                                <dx:PivotGridField Area="DataArea" AreaIndex="4" FieldName="AverageOut" Caption="Pemakaian Rata-Rata" ID="PivotAverageOut" CellFormat-FormatString="{0:N2}" CellFormat-FormatType="Custom" />
                                <dx:PivotGridField Area="DataArea" AreaIndex="5" FieldName="QuantityEnd" Caption="Stok Akhir" ID="PivotQuantityEnd" CellFormat-FormatString="{0:N2}" CellFormat-FormatType="Custom" />
                                <dx:PivotGridField Area="FilterArea" AreaIndex="0" FieldName="UnitPrice" Caption="HNA" ID="hna" CellFormat-FormatString="{0:N2}" />
                                <dx:PivotGridField Area="FilterArea" AreaIndex="1" FieldName="PurchaseUnit" Caption="Satuan Besar" ID="purchaseUnit" Visible="False" />
                                <dx:PivotGridField Area="FilterArea" AreaIndex="2" FieldName="ConversionFactor" Caption="Konversi" ID="conversion" Visible="False"/>
                            </Fields>
                            <OptionsPager RowsPerPage="20" />
                            <OptionsView ShowHorizontalScrollBar="True" />
                        </dx:ASPxPivotGrid>
                        <input type="hidden" value="19000101" id="hdnFromDate" runat="server" />
                        <input type="hidden" value="19000101" id="hdnToDate" runat="server" />
                        <input type="hidden" value="1 = 0" id="hdnAdditionalFilterExpression" runat="server" />
                        <asp:ObjectDataSource ID="odsPivotItemBalance" runat="server" SelectMethod="GetPivotItemBalancePerPeriodeList"
                            TypeName="QIS.Medinfras.Data.Service.BusinessLayer">
                            <SelectParameters>
                                <asp:ControlParameter Name="HealthcareID" ControlID="hdnHealthcareID" Type="String" PropertyName="Value" />
                                <asp:ControlParameter Name="FromDate" ControlID="hdnFromDate" Type="String" PropertyName="Value" />
                                <asp:ControlParameter Name="ToDate" ControlID="hdnToDate" Type="String" PropertyName="Value" />
                                <asp:ControlParameter Name="AdditionalFilterExpression" ControlID="hdnAdditionalFilterExpression" Type="String" PropertyName="Value" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </div>
    <input type="hidden" value="ItemBalancePivot" id="hdnFileName" runat="server" />
</asp:Content>
