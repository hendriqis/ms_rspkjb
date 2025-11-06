<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientBillSummaryPayReceiptDtCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryPayReceiptDtCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
 
<script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
<script type="text/javascript" id="dxss_paymentreceiptdtctl">
    $(function () {
        var total = $('#<%=hdnTotalPayment.ClientID %>').val();
        $('#tdTotalAll').html(total);
    });
</script>

<input type="hidden" runat="server" value="" id="hdnTotalPayment" />
<table class="tblContentArea" style="width:100%">
    <tr>
        <td>
            <div style="padding: 5px; height: 400px; overflow-y:scroll">
                <asp:ListView ID="lvwView" runat="server">
                        <EmptyDataTemplate>
                            <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0" rules="all" >
                                <tr>  
                                    <th align="left">
                                        <div style="padding:3px; text-align:left">
                                            <div><%= GetLabel("No Pembayaran")%></div>
                                            <div><%= GetLabel("Tanggal")%></div>                                                    
                                        </div>
                                    </th>
                                    <th>
                                        <div style="padding:3px; text-align:center">
                                            <div><%= GetLabel("Dibuat Oleh")%></div>                                                      
                                        </div>
                                    </th>
                                    <th>
                                        <div style="text-align:right">
                                            <%=GetLabel("Total Bayar")%>
                                        </div>
                                    </th>
                                    </tr>
                                    <tr class="trEmpty">
                                    <td colspan="4">
                                        <%=GetLabel("No Data To Display") %>
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <LayoutTemplate>                                
                            <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0" rules="all" >
                                <tr>  
                                    <th align="left">
                                        <div style="padding:3px; text-align:left">
                                            <div><%= GetLabel("No Pembayaran")%></div>
                                            <div><%= GetLabel("Tanggal")%></div>                                                    
                                        </div>
                                    </th>
                                    <th>
                                        <div style="padding:3px; text-align:center">
                                            <div><%= GetLabel("Dibuat Oleh")%></div>                                                      
                                        </div>
                                    </th>
                                    <th>
                                        <div style="text-align:right">
                                            <%=GetLabel("Total Bayar")%>
                                        </div>
                                    </th>
                                </tr>
                                <tr runat="server" id="itemPlaceholder" ></tr>
                                <tr class="trFooter">  
                                    <td>&nbsp;</td>
                                    <td>
                                        <div style="text-align:right;padding:3px">
                                            <%=GetLabel("Total")%>
                                        </div>
                                    </td>
                                    <td>
                                        <div style="text-align:right;padding:3px" id="tdTotalAll">
                                            <%=GetLabel("Total")%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                <td>
                                    <div style="padding:3px;float:left;">
                                        <%#: Eval("PaymentNo")%></a>
                                        <div><%#: Eval("PaymentDateInString")%></div>                                                    
                                    </div>
                                </td>
                                <td>
                                    <div style="padding:3px;">
                                        <div><%#: Eval("LastUpdatedByUserName")%></div>                                                    
                                    </div>
                                </td>
                                <td>
                                    <div style="padding:3px;text-align:right;">
                                        <input type="hidden" class="hdnPaymentAmount" value='<%#: Eval("ReceiptAmount")%>' />
                                        <div><%#: Eval("ReceiptAmount", "{0:N}")%></div>                                                   
                                    </div>   
                                </td>
                                </tr>
                            </ItemTemplate>
                    </asp:ListView>    
            </div>
        </td>
    </tr>
</table>
