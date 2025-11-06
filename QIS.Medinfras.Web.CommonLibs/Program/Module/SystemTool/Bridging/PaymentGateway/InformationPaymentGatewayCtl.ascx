<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InformationPaymentGatewayCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.InformationPaymentGatewayCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_sendADTNotificationCtl">
    $(function () {
        $('#leftPanel ul li').click(function () {
            $('#leftPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');
            showContent(contentID);
        });

        $('#leftPanel ul li').first().click();
    });
    function showContent(contentID) {
        var i, x, tablinks;
        x = document.getElementsByClassName("divContent");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        document.getElementById(contentID).style.display = "block";
    }

    $('.lblLink.btnCheck').click(function () {
        var rowID = $(this).parent().parent().attr('id');
        $('#<%=hdnReferenceNo.ClientID %>').val(rowID);
        cbpCheckPaymentGatewayView.PerformCallback('getstatus');
    });

    function onCbpPaymentVirtualHistoryViewEndCallback(s) { }

    function onCbpCheckPaymentGatewayViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'getstatus') {

            if (param[1] == 'fail') {
                displayMessageBox('FAILED', param[2]);

            } else {
                displayMessageBox('SUCCESS', 'Pembayaran berhasil diproses');
                SnackbarNotify('PAYMENT SUCCESS');
                pcRightPanelContent.Hide();
                var PaymentNo = $('#<%=hdnPaymentNo.ClientID %>').val(param[3]);
                if (PaymentNo != "") {
                    onAfterVirtualPayment(PaymentNo);
                }

            }
        }
        hideLoadingPanel();
    }

     


</script>
<style type="text/css">
    #leftPanel          { border:1px solid #6E6E6E; width:100%;height:100%; position: relative; }    
    #leftPanel > ul       { margin:0; padding:2px; border-bottom:1px groove black; }
    #leftPanel > ul > li    { list-style-type: none; font-size: 15px; display:list-item; border: 1px solid #fdf5e6!important; padding: 5px 8px; cursor: pointer; background-color:#87CEEB!important; }
    #leftPanel > ul > li.selected { background-color: #ff5722!important; color: White; }   
    .divContent { padding-left: 3px; min-height:490px;} 
</style>
<div style="width:100%;">

<input type="hidden" id="hdnQMIND" value="" runat="server" />
<input type="hidden" id="hdnReferenceNo" value="" runat="server" />
    <input type="hidden" id="hdnMessageType" value="" runat="server" />
    <input type="hidden" id="hdnTransactionID" value="" runat="server" />
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnPatientName" value="" runat="server" />
    <input type="hidden" id="hdnEmailAddress" value="" runat="server" />
     <input type="hidden" id="hdnBillingNo" value="" runat="server" />
    <input type="hidden" id="hdnVisitID" value="" runat="server" />
    <input type="hidden" value="" id="hdnTestOrderID" runat="server" />
    <input type="hidden" id="hdnIsSend" value="" runat="server" />
    <input type="hidden" id="hdnGCChasier" value="" runat="server" />
    <input type="hidden" id="hdnGCShift" value="" runat="server" />
    <input type="hidden" id="hdnPaymentMethod" value="" runat="server" />
    <input type="hidden" id="hdnBankCode" value="" runat="server" />
    <input type="hidden" id="hdnPaymentChannel" value="" runat="server" />
    <input type="hidden" id="hdnPaymentChannnelID" value="" runat="server" />
     <input type="hidden" id="hdnBillingID" value="" runat="server" />
      <input type="hidden" id="hdnstrBillingNo" value="" runat="server" />
     <table border="0" cellpadding="0" cellspacing="0" style="width:100%;">
        <colgroup>
            <col style="width:300px" />
            <col />
        </colgroup>
        <tr>
            <td colspan="2">
               <div id="lblTitle" runat="server" class="w3-blue-grey w3-xxlarge" style="text-align:center; text-shadow:1px 1px 0 #444">Informasi History Virtual Payment</div>
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top">
                <div id="leftPanel" class="w3-border">
                    <ul>
                       <li contentID="divPage2" title="History Pembayaran" class="w3-hover-red">History Pembayaran</li>
                     <%--   
                        <li contentID="divPage2" title="History Pembayaran" class="w3-hover-red">History Pembayaran</li>
                     --%>   
                    </ul>     
                </div> 
            </td>
             <td style="vertical-align:top; padding-left: 5px;">
                <div id="divPage2" class="w3-border divContent w3-animate-left" style="display:none">
                
                <div>
                    <table>
                        <tr>
                            <td>No. Registrasi</td>
                            <td><asp:TextBox ID="txtRegistrationNo" runat="server" ReadOnly="true" style="width:350px" ></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Nama Pasien</td>
                            <td><asp:TextBox ID="txtPatientName" runat="server" ReadOnly="true" style="width:350px" ></asp:TextBox></td>
                        </tr>
                    </table>
                </div>
                 <dxcp:ASPxCallbackPanel ID="cbpPaymentVirtualHistory" runat="server" Width="100%" ClientInstanceName="cbpPaymentVirtualHistory"
                    ShowLoadingPanel="false" OnCallback="cbpPaymentVirtualHistory_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpPaymentVirtualHistoryViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                        <asp:panel runat="server" id="Panel1" style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:gridview    id="gridPaymentVirtualView" runat="server" cssclass="grdView notAllowSelect" autogeneratecolumns="false"
                                     showheaderwhenempty="true" emptydatarowstyle-cssclass="trEmpty" OnRowDataBound="cbpPaymentVirtualHistory_RowDataBound">
                                   <Columns>
                                    <asp:BoundField DataField="ReferenceNo" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:BoundField DataField="ReferenceNo" HeaderText="No Reference"  />
                                    <asp:BoundField DataField="BillingNo" HeaderText="No Tagihan"  />
                                    <asp:BoundField DataField="PaymentAmount" HeaderText="Total" ItemStyle-HorizontalAlign="Right" />
                                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Left" HeaderText="Bank"
                                                HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBank" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderText="Status"
                                                HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderText="Remarks"
                                                HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                  <%--  <asp:Label ID="lblRemarks" runat="server" />--%>
                                                    <asp:HyperLink ID="link" runat="server" Target="_blank"></asp:HyperLink>
                                                    
                                                    
                                                        <%--<img src="<%# ResolveUrl("~/Libs/Images/QRIS-Logo.png") %>" width="40" height="40" />--%>
                                                        <asp:PlaceHolder ID="plQrisLogo" runat="server" Visible="false"  />
                                                        <asp:PlaceHolder ID="plBarCode" runat="server" Visible="false"  />
                                                        <asp:Label id="lblQMIND" runat="server" Visible="false"></asp:Label>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                      
                                      <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderText=""
                                                HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                 <div id="<%#: Eval("ReferenceNo") %>">
                                                         <div style="padding: 3px; text-align: center">
                                                           <label class="lblLink btnCheck w3-btn w3-red">Cek Status Pembayaran</label>
                                                        </div>
                                                        </div>   
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                </Columns>
                                   </asp:gridview>
                                   </asp:panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
               </div> 
            </td>
         </tr>

    </table>
    <dxcp:ASPxCallbackPanel ID="cbpCheckPaymentGatewayView" runat="server" Width="100%" ClientInstanceName="cbpCheckPaymentGatewayView"
                    ShowLoadingPanel="false" OnCallback="cbpCheckPaymentGatewayView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpCheckPaymentGatewayViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:HiddenField ID="hdnPaymentNo" runat="server" Value="" />
                            <asp:panel runat="server" id="Panel2" style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:gridview Visible="false" id="grdCheckPaymentView" runat="server" cssclass="grdView notAllowSelect" autogeneratecolumns="false"
                                     showheaderwhenempty="true" emptydatarowstyle-cssclass="trEmpty">
                                   </asp:gridview>
                                   </asp:panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
              
</div>
   
