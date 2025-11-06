<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionPatientInformationDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inpatient.Program.TransactionPatientInformationDetailCtl" %>

<div style="height: 440px; overflow-y: auto">
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <div class="pageTitle">
        <%=GetLabel("Detil Registrasi")%></div>
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
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td><h4><%=GetLabel("Order Penunjang Medis")%></h4></td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Service Unit Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="left"/>
                        <asp:BoundField DataField="TestOrderNo" HeaderText="Test Order No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="TestOrderDateInString" HeaderText="Test Order Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="TestOrderTime" HeaderText="Test Order Time" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
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
        <tr>
            <td><h4><%=GetLabel("Charges")%></h4></td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grdView2" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
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
        <tr>
            <td><h4><%=GetLabel("Billing")%></h4></td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grdView3" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="PatientBillingID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                        <asp:BoundField DataField="PatientBillingNo" HeaderText="No. Billing" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="BillingDate" HeaderText="Billing Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="BillingTime" HeaderText="Billing Time" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="TotalPatientAmount" HeaderText="Total Patient Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                        <asp:BoundField DataField="TotalPayerAmount" HeaderText="Total Payer Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                        <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("No Data To Display")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
    </table>
</div>
