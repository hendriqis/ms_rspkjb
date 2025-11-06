<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientInformationRealizationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientInformationRealizationCtl" %>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" id="hdnHSUID" value="" runat="server" />
    <input type="hidden" id="hdnDeptID" value="" runat="server" />
    <div class="pageTitle">
        <%=GetLabel("Detail Informasi Realisasi")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
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
                        <asp:BoundField DataField="OrderNo" HeaderText="No. Resep" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="cfOrderDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="OrderTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Realisasi" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada Resep yang outstanding")%>
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
                        <asp:BoundField DataField="OrderNo" HeaderText="No. Retur Resep" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="cfOrderDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="OrderTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Realisasi" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada Retur Resep yang outstanding")%>
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
                        <asp:BoundField DataField="OrderNo" HeaderText="No. Order" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="cfOrderDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="OrderTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Realisasi" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
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
                        <asp:BoundField DataField="OrderNo" HeaderText="No. Order" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="cfOrderDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="OrderTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Realisasi" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
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
                <asp:GridView ID="grdViewOtherDiagnosticO" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="OrderNo" HeaderText="No. Order" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="cfOrderDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="OrderTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Realisasi" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
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
                        <asp:BoundField DataField="OrderNo" HeaderText="No. Order" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                        <asp:BoundField DataField="cfOrderDateInString" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                        <asp:BoundField DataField="OrderTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Realisasi" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="left" HeaderStyle-Width="150px" />
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("No Data To Display")%>
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
