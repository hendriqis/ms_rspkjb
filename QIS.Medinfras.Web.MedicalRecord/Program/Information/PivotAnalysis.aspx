<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
CodeBehind="PivotAnalysis.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.PivotAnalysis" %>

<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    namespace="DevExpress.Web.ASPxPivotGrid" tagprefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v11.1.Export, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    namespace="DevExpress.Web.ASPxPivotGrid.Export" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <style type="text/css">
        .dxpgControl tr td                          { background-color: #FFFFFF; }
        .dxpgControl tr td tr:first-child td        { background-color: #9CC525; }        
    </style>

        <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <strong><%=GetLabel("Export to")%>:</strong>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboListExportFormat" runat="server" Style="vertical-align: middle"
                                SelectedIndex="0" ValueType="System.String" Width="61px">
                                <Items>
                                    <dxe:ListEditItem Text="Pdf" Value="0" />
                                    <dxe:ListEditItem Text="Excel" Value="1" />
                                </Items>
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <asp:Button ID="btnSavePivot" OnClick="btnSavePivot_Click" runat="server" Text="Save" />
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="3" cellspacing="0">
                    <tr>
                        <td rowspan="5" valign="top" style="width: 106px">
                            <strong><%=GetLabel("Export Options")%>: </strong>
                        </td>
                        <td style="width: 350px">
                            <asp:CheckBox ID="chkPrintHeadersOnEveryPage" runat="server" Text="Print headers on every page" /><br />
                            <asp:CheckBox ID="chkPrintFilterHeaders" runat="server" Text="Print filter headers" Checked="True" /><br />
                            <asp:CheckBox ID="chkPrintColumnHeaders" runat="server" Text="Print column headers" Checked="True" /><br />
                            <asp:CheckBox ID="chkPrintRowHeaders" runat="server" Text="Print row headers" Checked="True" /><br />
                            <asp:CheckBox ID="checkPrintDataHeaders" runat="server" Text="Print data headers" Checked="True" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <dx:ASPxPivotGrid ID="pvRegistraition" runat="server" ClientIDMode="AutoID" Width="100%" DataSourceID="odsPivotRegistration" >
        <Fields>
            <dx:PivotGridField Area="RowArea" AreaIndex="0" FieldName="ServiceUnitName" Caption="Unit Pelayanan"
                ID="ServiceUnit">
                <CustomTotals>
                    <dx:PivotGridCustomTotal SummaryType="Count" />
                </CustomTotals>
            </dx:PivotGridField>
            <dx:PivotGridField Area="ColumnArea" AreaIndex="0" FieldName="VisitDate" ID="fieldEventDate0"
                Caption="Year" GroupInterval="DateYear" />
            <dx:PivotGridField Area="ColumnArea" AreaIndex="1" FieldName="VisitDate" ID="fieldEventDate1"
                Caption="Month" GroupInterval="DateMonth"  />
            <dx:PivotGridField Area="FilterArea" AreaIndex="2" FieldName="Sex" Caption="Sex"
                ID="sexID" />         
            <dx:PivotGridField Area="DataArea"  AreaIndex="0" FieldName="VisitID" Caption="Visit"
                ID="visitID" SummaryType="Count"  />   
            <dx:PivotGridField Area="FilterArea" AreaIndex="3" FieldName="DepartmentID" Caption="Department"
                ID="departmentID" />   
            <dx:PivotGridField Area="FilterArea" AreaIndex="4" FieldName="VisitTypeName" Caption="VisitType"
                ID="visittypenameID" />
        </Fields>
        <OptionsPager RowsPerPage="20" />
        <OptionsView ShowHorizontalScrollBar="True" />
    </dx:ASPxPivotGrid>    
    <asp:ObjectDataSource ID="odsPivotRegistration" runat="server" 
        SelectMethod="GetvConsultVisitList" 
        TypeName="QIS.Medinfras.Data.Service.BusinessLayer">
        <SelectParameters>
            <asp:Parameter Name="filterExpression" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <dx:ASPxPivotGridExporter ID="ASPxPivotGridExporter1" runat="server" ASPxPivotGridID="pvRegistraition"
        Visible="False" />
</asp:Content>
