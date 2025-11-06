<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master" AutoEventWireup="true" 
    CodeBehind="InfoPatientTransaction.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.InfoPatientTransaction" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx" TagName="ToolbarCtl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPPatientPageNavigationPane" runat="server">
     <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" /> 
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />  
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="plhEntry" runat="server">
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 100%" />
            </colgroup>
            <tr>
                <td style="padding-top:5px; padding-bottom:5px"><h4><%=GetLabel("RESEP")%></h4></td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdViewOrderFarmasi" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                        <Columns>
                            <asp:BoundField DataField="PrescriptionOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                            <asp:BoundField DataField="PrescriptionDateInString" HeaderText="Tanggal Order" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                            <asp:BoundField DataField="PrescriptionTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                            <asp:BoundField DataField="PrescriptionOrderNo" HeaderText="No. Order" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" />
                            <asp:BoundField DataField="DispensaryServiceUnitName" HeaderText="Lokasi Farmasi" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="TransactionStatus" HeaderText="Status" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
                        </Columns>
                        <EmptyDataTemplate>
                            <%=GetLabel("Tidak ada transaksi order resep yang outstanding/pending")%>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="padding-top:5px; padding-bottom:5px"><h4><%=GetLabel("RETUR RESEP")%></h4></td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdViewOrderReturResep" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                        <Columns>
                            <asp:BoundField DataField="PrescriptionReturnOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                            <asp:BoundField DataField="OrderDateInString" HeaderText="Tanggal Order" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                            <asp:BoundField DataField="OrderTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                            <asp:BoundField DataField="PrescriptionReturnOrderNo" HeaderText="No. Order" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" />
                            <asp:BoundField DataField="LocationName" HeaderText="Lokasi Farmasi" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
                        </Columns>
                        <EmptyDataTemplate>
                            <%=GetLabel("Tidak ada transaksi order retur resep yang outstanding/pending")%>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td><h4><%=GetLabel("ORDER PENUNJANG MEDIS")%></h4></td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdViewOrderPenunjang" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                        <Columns>
                            <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                            <asp:BoundField DataField="TestOrderDateInString" HeaderText="Tanggal Order" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                            <asp:BoundField DataField="TestOrderTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                            <asp:BoundField DataField="TestOrderNo" HeaderText="No. Order" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" />
                            <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
                        </Columns>
                        <EmptyDataTemplate>
                            <%=GetLabel("Tidak ada transaksi order pemeriksaan penunjang yang outstanding/pending")%>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td><h4><%=GetLabel("ORDER PELAYANAN RAWAT JALAN/DARURAT")%></h4></td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdServiceOrder" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                        <Columns>
                            <asp:BoundField DataField="ServiceOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                            <asp:BoundField DataField="ServiceOrderDateInString" HeaderText="Tanggal Order" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                            <asp:BoundField DataField="ServiceOrderTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                            <asp:BoundField DataField="ServiceOrderNo" HeaderText="No. Order" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" />
                            <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  />
                            <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
                        </Columns>
                        <EmptyDataTemplate>
                            <%=GetLabel("Tidak ada transaksi order pelayanan yang outstanding/pending")%>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr style="display:none">
                <td><h4><%=GetLabel("Charges")%></h4></td>
            </tr>
            <tr style="display:none">
                <td>
                    <asp:GridView ID="grdViewCharges" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                        <Columns>
                            <asp:BoundField DataField="TransactionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                            <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"/>
                            <asp:BoundField DataField="TransactionNo" HeaderText="No. Transaksi" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"/>
                            <asp:BoundField DataField="TransactionDateInString" HeaderText="Tanggal Transaksi" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="TransactionTime" HeaderText="Waktu Transaksi" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="TotalPatientAmount" HeaderText="Pasien Total" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                            <asp:BoundField DataField="TotalPayerAmount" HeaderText="Instansi Total" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                            <asp:BoundField DataField="TotalAmount" HeaderText="Jumlah Total" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                            <asp:BoundField DataField="TransactionStatus" HeaderText="Status" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
                        </Columns>
                        <EmptyDataTemplate>
                            <%=GetLabel("No Data To Display")%>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr style="display:none">
                <td><h4><%=GetLabel("Billing")%></h4></td>
            </tr>
            <tr style="display:none">
                <td>
                    <asp:GridView ID="grdViewBilling" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                        <Columns>
                            <asp:BoundField DataField="PatientBillingID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                            <asp:BoundField DataField="PatientBillingNo" HeaderText="No. Billing" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"/>
                            <asp:BoundField DataField="BillingDateInString" HeaderText="Tanggal Bill" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="BillingTime" HeaderText="Jam Bill" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="TotalPatientAmount" HeaderText="Total Pasien" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                            <asp:BoundField DataField="TotalPayerAmount" HeaderText="Total Instansi" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                            <asp:BoundField DataField="TotalAmount" HeaderText="Total" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                        </Columns>
                        <EmptyDataTemplate>
                            <%=GetLabel("No Data To Display")%>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
