<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionPatientInformationDetailCtl2.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransactionPatientInformationDetailCtl2" %>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnParamCode" value="" runat="server" />
    <div class="pageTitle">
        <%=GetLabel("Detail Informasi")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No Registrasi")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRegistraionNo" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Pasien")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPatientName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id='trheaderFPrescription' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Resep Selesai")%></h4>
            </td>
        </tr>
        <tr id='trdetailFPrescription' runat="server">
            <td>
                <asp:GridView ID="grdViewPrescriptionF" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="PrescriptionOrderNo" HeaderText="No. Resep" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="PrescriptionDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="PrescriptionTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
                        <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada Resep yang sudah selesai")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderOPrescription' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Resep Outstanding")%></h4>
            </td>
        </tr>
        <tr id='trdetailOPrescription' runat="server">
            <td>
                <asp:GridView ID="grdViewPrescriptionO" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="PrescriptionOrderNo" HeaderText="No. Resep" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="PrescriptionDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="PrescriptionTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
                        <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada Resep yang outstanding")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderFPrescriptionR' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Retur Resep Selesai")%></h4>
            </td>
        </tr>
        <tr id='trdetailFPrescriptionR' runat="server">
            <td>
                <asp:GridView ID="grdViewPrescriptionRF" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="PrescriptionReturnOrderNo" HeaderText="No. Retur Resep"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                        <asp:BoundField DataField="TransactionNo" HeaderText="No. Resep" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="OrderDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                        <asp:BoundField DataField="OrderTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada Retur Resep yang sudah selesai")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderOPrescriptionR' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Retur Resep Outstanding")%></h4>
            </td>
        </tr>
        <tr id='trdetailOPrescriptionR' runat="server">
            <td>
                <asp:GridView ID="grdViewPrescriptionRO" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="PrescriptionReturnOrderNo" HeaderText="No. Retur Resep"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                        <asp:BoundField DataField="TransactionNo" HeaderText="No. Transaksi" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="OrderDateInString" HeaderText="Tanggal Order" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                        <asp:BoundField DataField="OrderTime" HeaderText="Jam Order" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada Retur Resep yang outstanding")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderFLaboratory' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Order Pemeriksaan Laboratorium sudah diproses")%></h4>
            </td>
        </tr>
        <tr id='trdetailFLaboratory' runat="server">
            <td>
                <asp:GridView ID="grdViewLaboratoryF" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="TestOrderNo" HeaderText="No. Order" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="TestOrderDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="TestOrderTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
                        <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("No Data To Display")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderOLaboratory' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Order Pemeriksaan Laboratorium outstanding")%></h4>
            </td>
        </tr>
        <tr id='trdetailOLaboratory' runat="server">
            <td>
                <asp:GridView ID="grdViewLaboratoryO" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="TestOrderNo" HeaderText="No. Order" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="TestOrderDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="TestOrderTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
                        <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("No Data To Display")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderFImaging' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Order Pemeriksaan Radiology sudah diproses")%></h4>
            </td>
        </tr>
        <tr id='trdetailFImaging' runat="server">
            <td>
                <asp:GridView ID="grdViewImagingF" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="TestOrderNo" HeaderText="No. Order" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="TestOrderDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="TestOrderTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
                        <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("No Data To Display")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderOImaging' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Order Pemeriksaan Radiology outstanding")%></h4>
            </td>
        </tr>
        <tr id='trdetailOImaging' runat="server">
            <td>
                <asp:GridView ID="grdViewImagingO" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="TestOrderNo" HeaderText="No. Order" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="TestOrderDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="TestOrderTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
                        <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("No Data To Display")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderFOtherDiagnostic' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Order Pemeriksaan Lain sudah diproses")%></h4>
            </td>
        </tr>
        <tr id='trdetailFOtherDiagnostic' runat="server">
            <td>
                <asp:GridView ID="grdViewOtherDiagnosticF" runat="server" CssClass="grdSelected"
                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="TestOrderNo" HeaderText="No. Order" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="TestOrderDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="TestOrderTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
                        <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("No Data To Display")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderOOtherDiagnostic' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Order Pemeriksaan Lain outstanding")%></h4>
            </td>
        </tr>
        <tr id='trdetailOOtherDiagnostic' runat="server">
            <td>
                <asp:GridView ID="grdViewOtherDiagnosticO" runat="server" CssClass="grdSelected"
                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="TestOrderNo" HeaderText="No. Order" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="TestOrderDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="TestOrderTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
                        <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("No Data To Display")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderFService' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Order Service sudah diproses")%></h4>
            </td>
        </tr>
        <tr id='trdetailFService' runat="server">
            <td>
                <asp:GridView ID="grdViewServiceF" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="ServiceOrderNo" HeaderText="No. Order" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="ServiceOrderDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="ServiceOrderTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
                        <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("No Data To Display")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderOService' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Order Service outstanding")%></h4>
            </td>
        </tr>
        <tr id='trdetailOService' runat="server">
            <td>
                <asp:GridView ID="grdViewServiceO" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="ServiceOrderNo" HeaderText="No. Order" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="ServiceOrderDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="ServiceOrderTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
                        <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("No Data To Display")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderFAllTransaction' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Transaksi Pelayanan Pasien yang sudah diproses menjadi tagihan")%></h4>
            </td>
        </tr>
        <tr id='trdetailFAllTransaction' runat="server">
            <td>
                <asp:GridView ID="grdViewAllTransactionF" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="TransactionNo" HeaderText="No. Transaksi" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                        <asp:BoundField DataField="TransactionDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="TransactionTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="170px" />
                        <asp:BoundField DataField="TransactionStatus" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                        <asp:BoundField DataField="TotalPatientAmount" HeaderText="Total Pasien" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="60px" />
                        <asp:BoundField DataField="TotalPayerAmount" HeaderText="Total Instansi" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="60px" />
                        <asp:BoundField DataField="TotalAmount" HeaderText="Jumlah Total" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="60px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("No Data To Display")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderOAllTransaction' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Transaksi Pelayanan Pasien yang belum diproses menjadi tagihan")%></h4>
            </td>
        </tr>
        <tr id='trdetailOAllTransaction' runat="server">
            <td>
                <asp:GridView ID="grdViewAllTransactionO" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="TransactionNo" HeaderText="No. Transaksi" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                        <asp:BoundField DataField="TransactionDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="TransactionTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="170px" />
                        <asp:BoundField DataField="TransactionStatus" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                        <asp:BoundField DataField="TotalPatientAmount" HeaderText="Total Pasien" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="60px" />
                        <asp:BoundField DataField="TotalPayerAmount" HeaderText="Total Instansi" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="60px" />
                        <asp:BoundField DataField="TotalAmount" HeaderText="Jumlah Total" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="60px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("No Data To Display")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderPObat' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Obat posting")%></h4>
            </td>
        </tr>
        <tr id='trdetailPObat' runat="server">
            <td>
                <asp:GridView ID="grdViewObatP" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="TransactionNo" HeaderText="No. Transaksi" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="cfTransactionDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="TransactionTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ChargesServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
                        <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                        <asp:BoundField DataField="cfQty" HeaderText="Qty" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" />
                        <asp:BoundField DataField="LineAmount" HeaderText="Harga" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada Obat yang sudah diposting")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderOPObat' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Obat outstanding posting")%></h4>
            </td>
        </tr>
        <tr id='trdetailOPObat' runat="server">
            <td>
                <asp:GridView ID="grdViewObatOP" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="TransactionNo" HeaderText="No. Transaksi" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="cfTransactionDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="TransactionTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ChargesServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
                        <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                        <asp:BoundField DataField="cfQty" HeaderText="Qty" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" />
                        <asp:BoundField DataField="LineAmount" HeaderText="Harga" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada Obat yang outstanding posting")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderPAlkes' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Alkes posting")%></h4>
            </td>
        </tr>
        <tr id='trdetailPAlkes' runat="server">
            <td>
                <asp:GridView ID="grdViewAlkesP" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="TransactionNo" HeaderText="No. Transaksi" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="cfTransactionDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="TransactionTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ChargesServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
                        <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                        <asp:BoundField DataField="cfQty" HeaderText="Qty" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" />
                        <asp:BoundField DataField="LineAmount" HeaderText="Harga" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada Alkes yang sudah diposting")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderOPAlkes' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Alkes outstanding posting")%></h4>
            </td>
        </tr>
        <tr id='trdetailOPAlkes' runat="server">
            <td>
                <asp:GridView ID="grdViewAlkesOP" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="TransactionNo" HeaderText="No. Transaksi" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="cfTransactionDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="TransactionTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ChargesServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
                        <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                        <asp:BoundField DataField="cfQty" HeaderText="Qty" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" />
                        <asp:BoundField DataField="LineAmount" HeaderText="Harga" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada Alkes yang outstanding posting")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderPBarangUmum' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Barang Umum posting")%></h4>
            </td>
        </tr>
        <tr id='trdetailPBarangUmum' runat="server">
            <td>
                <asp:GridView ID="grdViewBarangUmumP" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="TransactionNo" HeaderText="No. Transaksi" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="cfTransactionDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="TransactionTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ChargesServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
                        <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                        <asp:BoundField DataField="cfQty" HeaderText="Qty" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" />
                        <asp:BoundField DataField="LineAmount" HeaderText="Harga" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada Barang Umum yang sudah diposting")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderOPBarangUmum' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Barang Umum outstanding posting")%></h4>
            </td>
        </tr>
        <tr id='trdetailOPBarangUmum' runat="server">
            <td>
                <asp:GridView ID="grdViewBarangUmumOP" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="TransactionNo" HeaderText="No. Transaksi" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="cfTransactionDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="TransactionTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ChargesServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
                        <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                        <asp:BoundField DataField="cfQty" HeaderText="Qty" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" />
                        <asp:BoundField DataField="LineAmount" HeaderText="Harga" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada Barang Umum yang outstanding posting")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderFBilling' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Tagihan Pasien yang sudah dibayar")%></h4>
            </td>
        </tr>
        <tr id='trdetailFBilling' runat="server">
            <td>
                <asp:GridView ID="grdViewBillingF" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="PatientBillingNo" HeaderText="No. Tagihan" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                        <asp:BoundField DataField="BillingDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="BillingTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="TransactionStatus" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                        <asp:BoundField DataField="TotalPatientAmount" HeaderText="Total Pasien" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="60px" />
                        <asp:BoundField DataField="TotalPayerAmount" HeaderText="Total Instansi" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="60px" />
                        <asp:BoundField DataField="TotalAmount" HeaderText="Jumlah Total" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="60px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada Tagihan Pasien yang sudah dibayar")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr id='trheaderOBilling' runat="server">
            <td>
                <h4>
                    <%=GetLabel("Tagihan Pasien yang belum dibayar")%></h4>
            </td>
        </tr>
        <tr id='trdetailOBilling' runat="server">
            <td>
                <asp:GridView ID="grdViewBillingO" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="PatientBillingNo" HeaderText="No. Tagihan" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                        <asp:BoundField DataField="BillingDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="BillingTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="TransactionStatus" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                        <asp:BoundField DataField="TotalPatientAmount" HeaderText="Total Pasien" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="60px" />
                        <asp:BoundField DataField="TotalPayerAmount" HeaderText="Total Instansi" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="60px" />
                        <asp:BoundField DataField="TotalAmount" HeaderText="Jumlah Total" HeaderStyle-HorizontalAlign="Right"
                            ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="60px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada Tagihan Pasien yang belum dibayar")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
</div>
