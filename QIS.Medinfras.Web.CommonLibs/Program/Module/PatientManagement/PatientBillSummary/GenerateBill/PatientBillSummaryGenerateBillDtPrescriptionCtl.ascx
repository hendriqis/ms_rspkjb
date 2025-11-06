<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientBillSummaryGenerateBillDtPrescriptionCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryGenerateBillDtPrescriptionCtl" %>

<table class="tblContentArea">
    <colgroup>
        <col style="width:50%"/>
        <col style="width:50%"/>
    </colgroup>
    <tr>
        <td style="padding:5px;vertical-align:top">
            <table class="tblEntryContent" style="width:100%">
                <colgroup>
                    <col style="width:30%"/>
                    <col/>
                </colgroup>
                <tr>
                    <td class="tdLabel"><label><%=GetLabel("No Bukti")%></label></td>
                    <td><asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" /></td>
                </tr>
                <tr>
                    <td class="tdLabel"><%=GetLabel("Tanggal") %> - <%=GetLabel("Jam") %></td>
                    <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="padding-right: 1px;width:145px"><asp:TextBox ID="txtTransactionDate" ReadOnly="true" Width="120px" CssClass="datepicker" runat="server" /></td>
                            <td style="width:5px">&nbsp;</td>
                            <td><asp:TextBox ID="txtTransactionTime" Width="80px" CssClass="time" runat="server" ReadOnly="true" Style="text-align:center" /></td>
                        </tr>
                    </table>
                </td>
                </tr>
            </table>
        </td>
        <td style="padding:5px;vertical-align:top">
            <table class="tblEntryContent" style="width:100%">
                <colgroup>
                    <col style="width:30%"/>
                    <col/>
                </colgroup>
                <tr>
                    <td class="tdLabel"><label id="Label1" class="lblNormal" runat="server"><%=GetLabel("No Referensi")%></label></td>
                    <td><asp:TextBox ID="txtReferenceNo" Width="100%" ReadOnly="true" runat="server" /></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<div class="containerUlTabPage" style="height:300px;background-color:White; overflow-y:scroll;margin-bottom: 10px;">
    <asp:ListView ID="lvwPrescription" runat="server">
        <EmptyDataTemplate>
            <table id="tblView" runat="server" class="grdNormal" cellspacing="0" rules="all" >
                <tr>  
                    <th rowspan="2">
                        <div><%=GetLabel("Generic")%> - <%=GetLabel("Product")%> - <%=GetLabel("Strength")%> - <%=GetLabel("Form")%></div>
                        <div><div style="color:Blue;width:35px;float:left;"><%=GetLabel("DOSE")%></div> <%=GetLabel("Dose")%> - <%=GetLabel("Route")%> - <%=GetLabel("DosingFrequency")%></div>
                    </th>
                    <th rowspan="2" align="right" style="padding:3px; width: 80px;">
                        <div><%=GetLabel("Harga Satuan")%></div>
                    </th>
                    <th rowspan="2" align="right" style="padding:3px; width: 80px;">
                        <div><%=GetLabel("Jumlah")%></div>
                    </th>
                    <th rowspan="2" align="left" style="padding:3px; width: 80px;">
                        <div><%=GetLabel("Kelas Tagihan")%></div>
                    </th>   
                    <th colspan="3" align="center"><%=GetLabel("Total")%></th>
                </tr>
                <tr>
                    <th style="width:120px">
                        <div style="text-align:right;padding-right:3px"><%=GetLabel("Instansi")%></div>
                    </th>
                    <th style="width:120px">
                        <div style="text-align:right;padding-right:3px"><%=GetLabel("Pasien")%></div>
                    </th>
                    <th style="width:120px">
                        <div style="text-align:right;padding-right:3px"><%=GetLabel("Total")%></div>
                    </th>
                </tr>
                <tr class="trEmpty">
                    <td colspan="20">
                        <%=GetLabel("No Data To Display") %>
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>                                
            <table id="tblView" runat="server" class="grdService grdNormal" cellspacing="0" rules="all" >
                <tr>  
                    <th rowspan="2">
                        <div><%=GetLabel("Generic")%> - <%=GetLabel("Product")%> - <%=GetLabel("Strength")%> - <%=GetLabel("Form")%></div>
                        <div><div style="color:Blue;width:35px;float:left;"><%=GetLabel("DOSE")%></div> <%=GetLabel("Dose")%> - <%=GetLabel("Route")%> - <%=GetLabel("DosingFrequency")%></div>
                    </th>
                    <th rowspan="2" align="right" style="padding:3px; width: 80px;">
                        <div><%=GetLabel("Harga Satuan")%></div>
                    </th>
                    <th rowspan="2" align="right" style="padding:3px; width: 80px;">
                        <div><%=GetLabel("Jumlah")%></div>
                    </th>
                    <th rowspan="2" align="left" style="padding:3px; width: 80px;">
                        <div><%=GetLabel("Kelas Tagihan")%></div>
                    </th>   
                    <th colspan="3" align="center"><%=GetLabel("Total")%></th>
                </tr>
                <tr>
                    <th style="width:120px">
                        <div style="text-align:right;padding-right:3px"><%=GetLabel("Instansi")%></div>
                    </th>
                    <th style="width:120px">
                        <div style="text-align:right;padding-right:3px"><%=GetLabel("Pasien")%></div>
                    </th>
                    <th style="width:120px">
                        <div style="text-align:right;padding-right:3px"><%=GetLabel("Total")%></div>
                    </th>
                </tr>
                <tr runat="server" id="itemPlaceholder" ></tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <div style="padding:3px">
                        <div><%#: Eval("InformationLine1")%></div>
                        <div><div style="color:Blue;width:35px;float:left;"><%=GetLabel("DOSE")%></div> <%#: Eval("NumberOfDosage")%> <%#: Eval("DosingUnit")%> - <%#: Eval("Route")%> - <%#: Eval("cfDoseFrequency")%></div>
                    </div>
                </td>
                <td style="padding:3px;" align="right">
                    <div><%#: Eval("Tariff", "{0:N}")%></div>
                </td>
                <td style="padding:3px;" align="right">
                    <div><%#: Eval("TakenQty")%> <%#: Eval("ItemUnit")%></div>
                </td>
                <td style="padding:3px;">
                    <div><%#: Eval("ChargeClassName")%></div>
                </td>
                <td>
                    <div style="padding:3px;text-align:right;">
                        <div><%#: Eval("PayerAmount", "{0:N}")%></div>                                                   
                    </div>
                </td>
                <td>
                    <div style="padding:3px;text-align:right;">
                        <div><%#: Eval("PatientAmount", "{0:N}")%></div>                                                   
                    </div>
                </td>
                <td>
                    <div style="padding:3px;text-align:right;">
                        <div><%#: Eval("LineAmount", "{0:N}")%></div>                                                   
                    </div>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</div> 
<table style="width:100%" cellpadding="0" cellspacing="0">
    <colgroup>
        <col style="width:15%"/>
        <col style="width:35%"/>
        <col style="width:15%"/>
        <col style="width:35%"/>
    </colgroup>
    <tr>
        <td><div class="lblComponent" style="text-align: left; padding-left: 5px;"><%=GetLabel("Grand Total") %> : </div></td>
        <td style="text-align:right;padding-right: 10px;">
            Rp. <asp:TextBox ID="txtTotal" ReadOnly="true" CssClass="number" runat="server" Width="200px" />
        </td>
    </tr>
    <tr>
        <td><div class="lblComponent" style="text-align: left; padding-left: 5px;"><%=GetLabel("Grand Total Instansi") %> : </div></td>
        <td style="text-align:right;padding-right: 10px;">
            Rp. <asp:TextBox ID="txtTotalPayer" ReadOnly="true" CssClass="number" runat="server" Width="200px" />
        </td>
        <td><div class="lblComponent" style="text-align: left; padding-left: 5px;"><%=GetLabel("Grand Total Pasien") %>  : </div></td>
        <td style="text-align:right;padding-right: 10px;">
            Rp. <asp:TextBox ID="txtTotalPatient" ReadOnly="true" CssClass="number" runat="server" Width="200px" />
        </td>
    </tr>
</table>