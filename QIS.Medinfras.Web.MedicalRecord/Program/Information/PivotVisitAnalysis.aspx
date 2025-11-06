<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPList.master" AutoEventWireup="true" 
CodeBehind="PivotVisitAnalysis.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.PivotVisitAnalysis" %>

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
                $('#<%=hdnSelectedMonth.ClientID %>').val(cboMonth.GetValue());
                $('#<%=hdnSelectedYear.ClientID %>').val(cboYear.GetValue());
                cbpView.PerformCallback('refresh');
            });
        });

        function onCbpViewEndCallback(s) {
            $('#divPivot').show();
            hideLoadingPanel();
        }
    </script>
    <table>
        <tr id="trPeriode" runat="server">
            <td class="tdLabel">
                <label class="tdLabel">
                <%=GetLabel("Periode")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboYear" Width="120px" ClientInstanceName="cboYear" runat="server" />
                <input type="hidden" id="hdnSelectedYear" runat="server" />
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboMonth" Width="120px" ClientInstanceName="cboMonth" runat="server" />
                <input type="hidden" value="" id="hdnSelectedMonth" runat="server" />
            </td>            
        </tr>
   </table>
    
    <div id="divPivot" style="display: none">
        <uc1:PivotOptionCtl ID="PivotOptionCtl1" runat="server" />
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 450px;">
                        <dx:ASPxPivotGrid ID="pvView" runat="server" ClientIDMode="AutoID" Width="100%" DataSourceID="odsPivotVisitAnalysis">
                            <Fields>                 
                                <dx:PivotGridField Area="RowArea" AreaIndex="0" FieldName="AgeGroup" Caption="Kelompok Usia"
                                    ID="ageGroup" ValueStyle-Wrap="False">
                                    <CustomTotals>
                                        <dx:PivotGridCustomTotal SummaryType="Average" />
                                        <dx:PivotGridCustomTotal />
                                        <dx:PivotGridCustomTotal SummaryType="Min" />
                                        <dx:PivotGridCustomTotal SummaryType="Max" />
                                    </CustomTotals>
                                </dx:PivotGridField>    
                                <dx:PivotGridField Area="DataArea" AreaIndex="0" FieldName="RegistrationID" Caption="Registration"
                                    ID="registrationID" SummaryType="Count" />
                                <dx:PivotGridField Area="ColumnArea" AreaIndex="1" FieldName="cfIsNewPatient" Caption="Jenis Kunjungan"
                                    ID="statusNewPatient" />
                                <dx:PivotGridField Area="FilterArea" AreaIndex="3" FieldName="DepartmentID" Caption="Department"
                                    ID="departmentName" />
                                <dx:PivotGridField Area="ColumnArea" AreaIndex="2" FieldName="Gender" Caption="Gender"
                                    ID="gender" />
                            </Fields>
                            <OptionsPager RowsPerPage="15" />
                            <OptionsView ShowHorizontalScrollBar="True" />
                        </dx:ASPxPivotGrid>
                        <asp:HiddenField ID="hdnFilterExpression1" runat="server" Value="" />
                        <asp:ObjectDataSource ID="odsPivotVisitAnalysis" runat="server" SelectMethod="GetvRegistrationAgeGroupList"
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
    <input type="hidden" value="DiagnosePivot" id="hdnFileName" runat="server" />
</asp:Content>

