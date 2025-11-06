<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BillHistory.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.BillHistory" %>

<asp:ListView ID="lvwView" runat="server">
    <EmptyDataTemplate>
        <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0" rules="all" >
            <tr>  
                <th rowspan="2" align="left">
                    <div style="padding:3px;float:left; width: 180px;">
                        <div><%= GetLabel("No. Transaksi")%></div>
                        <div><%= GetLabel("Tanggal")%></div>                                                 
                    </div>
                </th>
                <th colspan="2"><%=GetLabel("Administrasi")%></th>
                <th colspan="2"><%=GetLabel("Services")%></th>
                <th rowspan="2">
                    <div><%=GetLabel("Status") %></div>
                </th>   
            </tr>
            <tr>
                <th style="width:100px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("Instansi")%>
                    </div>
                </th>
                <th style="width:100px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("Pasien")%>
                    </div>
                </th>
                <th style="width:100px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("Instansi")%>
                    </div>
                </th>
                <th style="width:100px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("Pasien")%>
                    </div>
                </th>
            </tr>    
            <tr class="trEmpty">
                <td colspan="8">
                    <%=GetLabel("No Data To Display") %>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>                                
        <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0" rules="all" >
            <tr>  
                <th rowspan="2" align="left">
                    <div style="padding:3px;float:left; width: 180px;">
                        <div><%= GetLabel("No. Transaksi")%></div>
                        <div><%= GetLabel("Tanggal")%></div>                                                 
                    </div>
                </th>
                <th colspan="2"><%=GetLabel("Administrasi")%></th>
                <th colspan="2"><%=GetLabel("Services")%></th>
                <th rowspan="2">
                    <div><%=GetLabel("Status") %></div>
                </th>
            </tr>
            <tr>
                <th style="width:100px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("Instansi")%>
                    </div>
                </th>
                <th style="width:100px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("Pasien")%>
                    </div>
                </th>
                <th style="width:100px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("Instansi")%>
                    </div>
                </th>
                <th style="width:100px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("Pasien")%>
                    </div>
                </th>
            </tr>
            <tr runat="server" id="itemPlaceholder" ></tr>
            <tr class="trFooter">  
                <td>
                    <div style="text-align:right;padding:0px 3px">
                        <%=GetLabel("Total")%>
                    </div>
                </td>
                <td>
                    <div style="text-align:right;padding:0px 3px" id="tdTotalAdminPayer" runat="server">0.00</div>
                </td>
                <td>
                    <div style="text-align:right;padding:0px 3px" id="tdTotalAdminPatient" runat="server">0.00</div>
                </td>
                <td>
                    <div style="text-align:right;padding-right:3px" id="tdTotalServicePayer" runat="server">0.00</div>
                </td>
                <td>
                    <div style="text-align:right;padding-right:3px" id="tdTotalServicePatient" runat="server">0.00</div>
                </td>
                <td colspan="2">&nbsp;</td>
            </tr>
        </table>
    </LayoutTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <div style="padding:3px;float:left; width: 180px;">
                    <b><%#: Eval("PatientBillingNo")%></b>
                    <div><%#: Eval("BillingDateInString")%> <%#: Eval("BillingTime")%></div>                                                    
                </div>
            </td>
            <td>
                <div style="padding:3px;text-align:right;">
                    <div><%#: Eval("AdministrationFeeAmount", "{0:N}")%></div>                                                   
                </div>
            </td>
            <td>
                <div style="padding:3px;text-align:right;">
                    <div><%#: Eval("PatientAdminFeeAmount", "{0:N}")%></div>                                                   
                </div>
            </td>
            <td>
                <div style="padding:3px;text-align:right;">
                    <div><%#: Eval("ServiceFeeAmount", "{0:N}")%></div>                                                   
                </div>
            </td>
            <td>
                <div style="padding:3px;text-align:right;">
                    <div><%#: Eval("PatientServiceFeeAmount", "{0:N}")%></div>                                                   
                </div>
            </td>
            <td>
                <div style="padding:3px;"><%#: Eval("TransactionStatus")%></div>
            </td>
        </tr>
    </ItemTemplate>
</asp:ListView>
