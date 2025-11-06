<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true" 
    CodeBehind="BroadcastMessageList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.BroadcastMessageList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
 
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li runat="server" id="btnTransferOrder">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#leftPanel ul li').click(function () {
                $('#leftPanel ul li.selected').removeClass('selected');
                $(this).addClass('selected');
                var contentID = $(this).attr('contentID');
                showContent(contentID);
            });

            $('#leftPanel ul li').first().click();

            var pageCount = parseInt('<%=pageCount %>');
            $(function () {
                setPaging($("#pagingout"), pageCount, function (page) {
                    cbpOutboxView.PerformCallback('changepage|' + page);
                });
            });
            $(function () {
                setPaging($("#paginginbox"), pageCount, function (page) {
                    CbpInboxView.PerformCallback('changepage|' + page);
                });
            });
            
            /*signal R
            var bc = $.connection.BroadcastMessage;
            bc.client.addMessage = function (message) {
            ///// $('#listMessages').append('<li>' + message + '</li>');
            };*/

            $("#btnSend").live('click', function () {

                //// var message = $('#<%=txtMessage.ClientID %>').val();
                ///// sendNotify();
                cbpOutboxView.PerformCallback('save');
            });



        });
        
        window.onbeforeunload = function () {
            $.connection.hub.stop();
        };
        function showContent(contentID) {
            var i, x, tablinks;
            x = document.getElementsByClassName("divContent");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(contentID).style.display = "block";
        }
        function sendNotify() {
            var message = $('#<%=hdnjsonSendNotification.ClientID %>').val();
            SendNotification(message); //file di Function_signalR.js
 

        }

        
        function onCbpOutboxViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail') {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                } else {
                    sendNotify();
                    showToast('Success');
                }
            } else if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    
                setPaging($("#paging"), pageCount, function (page) {
                    cbpOutboxView.PerformCallback('changepage|' + page);
                });
            }
            hideLoadingPanel();
         }
         function onCbpInboxViewEndCallback(s) {
             var param = s.cpResult.split('|');
             if (param[0] == 'refresh') {
                 var pageCount = parseInt(param[1]);
                 if (pageCount > 0)
                     setPaging($("#paginginbox"), pageCount, function (page) {
                        cbpInboxView.PerformCallback('changepage|' + page);
                     });
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
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchOrder" runat="server" />
  <input type="hidden" value="" id="hdnParam" runat="server" />
  <input type="hidden" value="" id="hdnFilterExpression" runat="server" />
  <input type="hidden" value="" id="hdnQuickTextOrder" runat="server" />
  <input type="hidden" value="0" id="hdnIsUsingUDD" runat="server" />
  <input type="hidden" value="" id="hdnUnit" runat="server" />
  <input type="hidden" value="" id="hdnPageTitle" runat="server" />
  <table border="0" cellpadding="0" cellspacing="0" style="width:100%;">
    <colgroup>
      <col style="width:300px" />
      <col />
    </colgroup>
    <tr>
      <td colspan="2">
        <div id="lblTitle" runat="server" class="w3-blue-grey w3-xxlarge" style="text-align:center; text-shadow:1px 1px 0 #444"><%=GetLabel("INFORMASI KONTAK PESAN")%> </div>
      </td>
    </tr>
    <tr>
      <td style="vertical-align:top">
        <div id="leftPanel" class="w3-border">
          <ul>
            <li contentID="divPage1" title="Proses Pembayaran" class="w3-hover-red">Buat Pesan</li>
            <li contentID="divPage2" title="Proses Pembayaran" class="w3-hover-red">Inbox</li>
            <li contentID="divPage3" title="Proses Pembayaran" class="w3-hover-red">Outbox</li>
          </ul>
        </div>
      </td>
      <td style="vertical-align:top; padding-left: 5px;">
        <div id="divPage1" class="w3-border divContent w3-animate-left">
          <table class="tblContentArea">
            <colgroup>
              <col style="width: 100%" />
            </colgroup>
            <tr>
              <td>
                <fieldset id="fsEntryPopup" style="margin: 0" >
                  <div class="w3-container">
                    <div class="w3-card-4">
                      <div class="w3-container w3-blue">
                        <h2>Kirim Pesan</h2>
                      </div>
                      <div class="w3-container">
                        <label>Subject</label>
                        <asp:TextBox CssClass="w3-input" ID="txtSubject" runat="server" Width="70%"></asp:TextBox>
                        <label>User Only</label>
                        <asp:CheckBox ID="chkIsOnlyUser" runat="server" CssClass="w3-input" />
                        <label>kepada</label>
                        <dxe:ASPxComboBox CssClass="w3-input" ID="cboUserGroup" ClientInstanceName="cboUserGroup" Width="100%" runat="server"></dxe:ASPxComboBox>
                        <label>Pesan</label>
                        <asp:TextBox CssClass="w3-input" Width="50%" ID="txtMessage" runat="server" TextMode="MultiLine" Columns="10" />
                        <div style="padding:10px;">
                          <input type="button" class="w3-btn w3-blue" id="btnSend" value="Kirim" />
                          <br />
                          <ul id="listMessages">
                          </ul>
                        </div>
                      </div>
                    </div>
                  </div>
                </fieldset>
              </td>
            </tr>
          </table>
        </div>
        <div id="divPage2" class="w3-border divContent w3-animate-left" style="display:none">
          <table class="tblContentArea">
            <colgroup>
              <col style="width: 100%" />
            </colgroup>
            <tr>
              <td colspan="2">
                <h4 style="text-align: center">Inbox</h4>
                <div class="w3-container">
                  <div class="w3-card-4">
                    <dxcp:ASPxCallbackPanel ID="cbpInboxView" runat="server" Width="100%" ClientInstanceName="cbpInboxView" ShowLoadingPanel="false" OnCallback="cbpInboxView_Callback">
                      <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpInboxViewEndCallback(s); }" />
                      <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                        <asp:HiddenField ID="hdnPageCountInbox" runat="server" />
                          <asp:panel runat="server" id="Panel1" style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                            <asp:gridview id="gridViewInbox" runat="server" cssclass="grdView notAllowSelect" autogeneratecolumns="false" showheaderwhenempty="true" emptydatarowstyle-cssclass="trEmpty">
                              <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="FromFullname" HeaderText="Dari" />
                                <asp:BoundField DataField="SubjectMessage" HeaderText="Subject" />
                                <asp:BoundField DataField="CreatedDate" HeaderText="Tanggal Kirim" />
                              </Columns>
                            </asp:gridview>
                          </asp:panel>
                        </dx:PanelContent>
                      </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                     <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paginginbox">
                            </div>
                        </div>
                    </div>
                  </div>
                </div>
              </td>
            </tr>
          </table>
        </div>
        <div id="divPage3" class="w3-border divContent w3-animate-left" style="display:none">
        <table class="tblContentArea">
          <colgroup>
            <col style="width: 100%" />
          </colgroup>
          <tr>
            <td><h4 style="text-align: center">Outbox</h4></td>
          </tr>
          <tr>
            <td>
                <div class="w3-container">
                  <div class="w3-card-4">
                    <dxcp:ASPxCallbackPanel ID="cbpOutboxView" runat="server" Width="100%" ClientInstanceName="cbpOutboxView" ShowLoadingPanel="false" OnCallback="cbpOutboxView_Callback">
                      <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpOutboxViewEndCallback(s); }" />
                      <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                          <asp:panel runat="server" id="Panel2" style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:HiddenField ID="hdnjsonSendNotification" runat="server" />
                            <asp:gridview id="gridOutboxView" runat="server" cssclass="grdView notAllowSelect" autogeneratecolumns="false" showheaderwhenempty="true" emptydatarowstyle-cssclass="trEmpty">
                              <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                 <asp:BoundField DataField="ToFullname" HeaderText="Kepada" />
                                <asp:BoundField DataField="SubjectMessage" HeaderText="Subject" />
                                <asp:BoundField DataField="CreatedDate" HeaderText="Tanggal Kirim" />
                              </Columns>
                            </asp:gridview>
                          </asp:panel>
                        </dx:PanelContent>
                      </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                  <div class="imgLoadingGrdView" id="containerImgLoadingPatientFamily">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingout">
                        </div>
                    </div>
                </div>
                  </div>
                </div>
            </td>
          </tr>
        </table>
       </div>
      </td>
    </tr>
  </table>
</asp:Content>
