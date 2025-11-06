<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SendPaymentGatewayCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SendPaymentGatewayCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_sendADTNotificationCtl">
    var intervalChekced;
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
    function onCbpPaymentGatewayViewEndCallback(s) {
        var param = s.cpResult.split('|');

        if (param[0] == 'process') {

            $('#<%=hdnIsSend.ClientID %>').val(param[3]);

            if (param[3] == "1") {
                $('#<%=btnGetStatusPayment.ClientID %>').show();
                $('#<%=btnSend.ClientID %>').hide();
            } else {
                $('#<%=btnGetStatusPayment.ClientID %>').hide();
                $('#<%=btnSend.ClientID %>').show();
            }
            
            if (param[1] == 'fail') {
                displayMessageBox('FAILED', param[2]);
            }
            else {
                $('#<%=hdnUrl.ClientID %>').val(param[4]);
                displayMessageBox('Success', param[2]);

                ///check payment
                intervalChekced = setInterval(function () {
                    cbpCheckPaymentGatewayView.PerformCallback('getstatus');
                }, 2 * 60000); // 2 menit

            }
        }
        else if (param[0] == 'getstatus') {

            if (param[1] == 'fail') {
                displayMessageBox('FAILED', param[2]);

                if (param[2] == "PENDING") {
                    ///check payment
                    intervalChekced = setInterval(function () {
                        cbpCheckPaymentGatewayView.PerformCallback('getstatus');
                    }, 2 * 60000); // 2 menit
                }

            } else {
                displayMessageBox('SUCCESS', 'Pembayaran berhasil diproses');
                //                SnackbarNotify('PAYMENT SUCCESS');
                ShowSnackbarNotification('PAYMENT SUCCESS');
                pcRightPanelContent.Hide();
                var PaymentNo = $('#<%=hdnPaymentNo.ClientID %>').val(param[3]);
                if (PaymentNo != "") {
                    onAfterVirtualPayment(PaymentNo);
                }
                /////////cbpMPEntryContent.PerformCallback('next');
                /////cbpMPEntryContent.PerformCallback('load');
            }
        }
        hideLoadingPanel();
    }

    $('#<%=btnSend.ClientID %>').click(function () {
        if ($('#<%=hdnPaymentChannel.ClientID %>').val() != "") {
            cbpPaymentGatewayView.PerformCallback('process');
        }
        else {
            displayMessageBox('WARNING', 'Payment Channel belum di set untuk virtual payment ini');
        }
    });

    $('#<%=btnGetStatusPayment.ClientID %>').click(function () {
       cbpPaymentGatewayView.PerformCallback('getstatus');
    });

    

    function startCheck() {
        interval = setInterval(function () {
            cbpCheckPaymentGatewayView.PerformCallback('getstatus');
        }, 2 * 60000);
    }
    function onCbpCheckPaymentGatewayViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'getstatus') {
            console.log(param[2] + "\n");
            if (param[1] != 'fail') {
                clearInterval(intervalChekced);
                intervalChekced = 0;
                SnackbarNotify('PAYMENT SUCCESS');
                pcRightPanelContent.Hide();
                /////// cbpMPEntryContent.PerformCallback('next');
                ////cbpMPEntryContent.PerformCallback('load');
                var PaymentNo = $('#<%=hdnPaymentNo.ClientID %>').val(param[3]);
                if (PaymentNo != "") {
                    onAfterVirtualPayment(PaymentNo);
                }
              
            }
        }
      
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
    <input type="hidden" id="hdnMessageType" value="" runat="server" />
    <input type="hidden" id="hdnTransactionID" value="" runat="server" />
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnPatientName" value="" runat="server" />
    <input type="hidden" id="hdnEmailAddress" value="" runat="server" />
      
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
               <div id="lblTitle" runat="server" class="w3-blue-grey w3-xxlarge" style="text-align:center; text-shadow:1px 1px 0 #444"><%=GetLabel("PEMBAYARAN DENGAN VIRTUAL PAYMENT")%></div>
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top">
                <div id="leftPanel" class="w3-border">
                    <ul>
                        <li contentID="divPage1" title="Proses Pembayaran" class="w3-hover-red">Proses Pembayaran</li>
                    </ul>     
                </div> 
            </td>
             <td style="vertical-align:top; padding-left: 5px;">
               <div id="divPage1" class="w3-border divContent w3-animate-left" style="display:none"> 
                <table class="tblContentArea">
                    <colgroup>
                        <col style="width: 100%" />
                    </colgroup>
                    <tr>
                    <td>
                        <fieldset id="fsEntryPopup" style="margin: 0">
                            <table class="tblEntryContent" cellpadding="0" cellspacing="1">
                                <tr>
                                     <td colspan="2"><h4 style="text-align: center">Informasi Pembayaran</h4></td>
                                </tr>
                                 <tr>
                                    <td>Nomor Tagihan</td>
                                    <td><asp:TextBox  ID="txtBillingNo" runat="server" Width="95%" /></td>
                                </tr>
                                <tr>
                                    <td>Total Tagihan</td>
                                    <td><asp:TextBox  ID="txtTotalPayment" runat="server"  Width="95%" /></td>
                                </tr> 
                                <tr>
                                    <td>Metode Pembayaran</td>
                                    <td><asp:TextBox  ID="txtMethodPayment" runat="server"  Width="95%" /></td>
                                </tr> 
                                <tr>
                                    <td>Informasi Bank</td>
                                    <td><asp:TextBox  ID="txtBankName" runat="server"  Width="95%" /></td>
                                </tr> 
                                <tr>
                                    <td>Provider</td>
                                    <td><dxe:ASPxComboBox ID="cboProviderMethod" ClientInstanceName="cboProviderMethod" Width="50%" runat="server" /></td>
                                </tr>
                                <tr id="trWarning" runat="server" style="display:none">
                                    <td></td>
                                    <td><label  id="lblWarning" runat="server" style="color:Red; font-weight:bold;"></label></td>
                                </tr>
                                <tr id="trphone" runat="server" style="display:none;">
                                    <td>Nomor OVO</td>
                                    <td><asp:TextBox  ID="txtPhoneNo" runat="server"  Width="95%" /></td>
                                </tr> 
                                <tr>
                                    <td> </td>
                                    <td>
                                        <input type="button" value="Kirim" id="btnSend" runat="server" class="btnView w3-btn w3-hover-blue"  />
                                        <input type="button" value="Cek Status Pembayaran" id="btnGetStatusPayment" style="display:none" runat="server" class="btnView w3-btn w3-hover-blue"  />
                                    </td>
                                </tr> 
                            </table>
                        </fieldset>
                    </td>
                </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpPaymentGatewayView" runat="server" Width="100%" ClientInstanceName="cbpPaymentGatewayView"
                    ShowLoadingPanel="false" OnCallback="cbpPaymentGatewayView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpPaymentGatewayViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                             <div align="center" id="divResponse" runat="server" style="display:none;">
                                      <a href="" id="link" runat="server" target="_blank" ></a>
                                      <br />
                                        <asp:Image ID="QRIS_Logo" runat="server" Width="150" style="display:none;"/><br />
                                        <asp:Image ID="qrcodeURL" runat="server" Width="250" Height="250" /><br />
                                        <label ID="QRIS_NMID" runat="server" style="display:none;"></label>
                                      
                                </div>
                                 <asp:panel runat="server" id="pnlEntryPopupGrdView" style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                               <input type="hidden" id="hdnUrl" value="" runat="server" />
                                <asp:gridview Visible="false" id="grdView" runat="server" cssclass="grdView notAllowSelect" autogeneratecolumns="false"
                                     showheaderwhenempty="true" emptydatarowstyle-cssclass="trEmpty">
                                   </asp:gridview>
                                   </asp:panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
               

               <dxcp:ASPxCallbackPanel ID="cbpCheckPaymentGatewayView" runat="server" Width="100%" ClientInstanceName="cbpCheckPaymentGatewayView"
                    ShowLoadingPanel="false" OnCallback="cbpCheckPaymentGatewayView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){  }" EndCallback="function(s,e){ onCbpCheckPaymentGatewayViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                        <asp:HiddenField ID="hdnPaymentNo" runat="server" Value="" />
                               <asp:panel runat="server" id="Panel1" style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:gridview Visible="false" id="grdCheckPaymentView" runat="server" cssclass="grdView notAllowSelect" autogeneratecolumns="false"
                                     showheaderwhenempty="true" emptydatarowstyle-cssclass="trEmpty">
                                   </asp:gridview>
                                   </asp:panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
               
            </td>
         </tr>

    </table>
</div>
   
