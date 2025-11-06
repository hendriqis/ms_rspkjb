<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionPatientInformationDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransactionPatientInformationDetailCtl" %>

<div style="height: 440px; overflow-y: auto">
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <div class="pageTitle">
        <%=GetLabel("Detail Registrasi")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No Registrasi")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRegistrationNo" ReadOnly="true" Width="150px" runat="server" />
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
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td><h4><%=GetLabel("Item Transaksi yang Belum Diposting")%></h4></td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grdViewChargesNotPosting" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="cfTransactionDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                        <asp:BoundField DataField="ChargesServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left"/>
                        <asp:BoundField DataField="TransactionNo" HeaderText="No. Transaksi" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" />
                        <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px"/>
                        <asp:BoundField DataField="cfQty" HeaderText="Qty" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px"/>
                        <asp:BoundField DataField="LineAmount" HeaderText="Harga" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px"/>
                        <asp:BoundField DataField="TransactionStatus" HeaderText="Status Transaksi" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada data")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td><h4><%=GetLabel("Order Pemeriksaan ke Penunjang Medis")%></h4></td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grdViewTestOrder" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Tujuan Order" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="TestOrderDateInString" HeaderText="Tanggal Order" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                        <asp:BoundField DataField="TestOrderTime" HeaderText="Jam Order" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                        <asp:BoundField DataField="TestOrderNo" HeaderText="No. Order" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"/>
                        <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status Order" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="170px"/>
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada data")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td><h4><%=GetLabel("Order Pemeriksaan ke Pelayanan Rawat Jalan / Rawat Darurat")%></h4></td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grdViewServiceOrder" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Tujuan Order" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="ServiceOrderDateInString" HeaderText="Tanggal Order" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                        <asp:BoundField DataField="ServiceOrderTime" HeaderText="Jam Order" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                        <asp:BoundField DataField="ServiceOrderNo" HeaderText="No. Order" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"/>
                        <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status Order" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="170px"/>
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada data")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td><h4><%=GetLabel("Seluruh Transaksi Pasien")%></h4></td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grdViewCharges" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="TransactionDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"/>
                        <asp:BoundField DataField="TransactionTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px"/>
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="TransactionNo" HeaderText="No. Transaksi" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"/>
                        <asp:BoundField DataField="TotalPatientAmount" HeaderText="Total Pasien" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" HeaderStyle-Width="125px" />
                        <asp:BoundField DataField="TotalPayerAmount" HeaderText="Total Instansi" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" HeaderStyle-Width="125px" />
                        <asp:BoundField DataField="TotalAmount" HeaderText="Jumlah Total" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" HeaderStyle-Width="125px" />
                        <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status Transaksi" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px"/>
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada data")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td><h4><%=GetLabel("Transaksi Pasien yang Belum Diproses Menjadi Tagihan")%></h4></td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grdViewChargesNotBilling" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="TransactionDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"/>
                        <asp:BoundField DataField="TransactionTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px"/>
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="TransactionNo" HeaderText="No. Transaksi" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"/>
                        <asp:BoundField DataField="TotalPatientAmount" HeaderText="Total Pasien" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" HeaderStyle-Width="125px" />
                        <asp:BoundField DataField="TotalPayerAmount" HeaderText="Total Instansi" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" HeaderStyle-Width="125px" />
                        <asp:BoundField DataField="TotalAmount" HeaderText="Jumlah Total" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" HeaderStyle-Width="125px" />
                        <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status Transaksi" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px"/>
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada data")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td><h4><%=GetLabel("Tagihan Pasien yang Belum Dibayar")%></h4></td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grdViewChargesNotPayment" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="BillingDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"/>
                        <asp:BoundField DataField="BillingTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px"/>
                        <asp:BoundField DataField="PatientBillingNo" HeaderText="No. Tagihan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="TotalPatientAmount" HeaderText="Total Pasien" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="125px"  />
                        <asp:BoundField DataField="TotalPayerAmount" HeaderText="Total Instansi" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="125px"  />
                        <asp:BoundField DataField="TotalAmount" HeaderText="Jumlah Total" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="125px"  />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada data")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
    </table>
</div>
