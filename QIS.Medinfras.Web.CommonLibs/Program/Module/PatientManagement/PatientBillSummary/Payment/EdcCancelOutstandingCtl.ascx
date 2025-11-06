<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EdcCancelOutstandingCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.EdcCancelOutstandingCtl" %>
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

    $('.btnCancel').click(function () {
        var rowID = $(this).parent().parent().attr('id');
        $('#<%=hdnID.ClientID %>').val(rowID);
        cbpEdcPaymentOutstanding.PerformCallback('cancel');
    });

    function onCbpEdcPaymentOutstandingViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'cancel') {

            if (param[1] == 'fail') {
                displayMessageBox('FAILED', param[2]);

            } else {
                displayMessageBox('SUCCESS', 'Proses Pembayaran EDC berhasil dibatalkan');
                window.location.reload(true);

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
    <input type="hidden" runat="server" id="hdnRegID" value="" />
    <input type="hidden" runat="server" id="hdnID" value="" />
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
                       <li contentID="divPage2" title="History Pembayaran" class="w3-hover-red">Outstanding Process EDC</li>
                     
                    </ul>     
                </div> 
            </td>
             <td style="vertical-align:top; padding-left: 5px;">
                <div id="divPage2" class="w3-border divContent w3-animate-left" style="display:none">
                
               
                    <dxcp:ASPxCallbackPanel ID="cbpEdcPaymentOutstanding" runat="server" Width="100%" ClientInstanceName="cbpEdcPaymentOutstanding"
                    ShowLoadingPanel="false" OnCallback="cbpEdcPaymentOutstanding_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpEdcPaymentOutstandingViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                        <asp:panel runat="server" id="Panel1" style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                 <asp:gridview    id="gridEdcPaymentView" runat="server" cssclass="grdView notAllowSelect" autogeneratecolumns="false"
                                     showheaderwhenempty="true" emptydatarowstyle-cssclass="trEmpty" OnRowDataBound="cbpEdcPaymentOutstanding_RowDataBound">
                                   <Columns>
                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:BoundField DataField="RegistrationNo" HeaderText="Nomor Registrasi"  />
                                    <asp:BoundField DataField="FullName" HeaderText="Nama"  />
                                    <asp:BoundField DataField="IsFinish" HeaderText="Status" ItemStyle-HorizontalAlign="Right" />
                                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Left" HeaderText=""
                                                HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <div id="<%#: Eval("ID") %>">
                                                        <div style="padding: 3px; text-align: center">
                                                            <input type="button" class="btnCancel" value="Batal Proses" />
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
   

</div>
   
