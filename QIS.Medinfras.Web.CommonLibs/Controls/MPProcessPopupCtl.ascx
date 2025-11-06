<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MPProcessPopupCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.MPProcessPopupCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

    <input type="hidden" runat="server" id="hdnIsAdd" value="1" />
    <div class="toolbarArea">
        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
            <tr>
                <td style="width:50%" id="tdToolbarButton">
                    <ul>
                        <li runat="server" id="btnMPEntryPopupProcess">
                            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
                                <%=GetLabel("Process")%></div>
                        </li>
                    </ul>
                </td>
                <td style="width:50%; text-align: right; padding-left:5px; padding-right: 5px">
                    <div style="font-size: 1.4em">
                        <%=HttpUtility.HtmlEncode(GetPopupTitle())%></div>
                </td>
            </tr>
        </table>
    </div>
    <div style="padding:5px 0;">  
        <script type="text/javascript" id="dxss_mpentrypopupctl">
            window.setEntryPopupIsAdd = function (isAdd) {
                if (isAdd)
                    $('#<%=hdnIsAdd.ClientID %>').val('1');
                else
                    $('#<%=hdnIsAdd.ClientID %>').val('0');
            }
            window.getEntryPopupIsAdd = function () {
                return ($('#<%=hdnIsAdd.ClientID %>').val() == '1');
            }
            
            window.onRefreshCtlControl = function() {
                cbpMPEntryPopupContent.PerformCallback('refresh');
            }

            function addToolbarButton(htmlElement) {
                $("#tdToolbarButton ul").append(htmlElement);
            }

            $(function () {
                $('#<%=btnMPEntryPopupProcess.ClientID %>').show();
                $('#<%=btnMPEntryPopupProcess.ClientID %>').click(function (evt) {
                    var errMessage = { text: "" };
                    var isAllowProcess = true;
                    if (typeof onBeforeProcess != 'undefined')
                        isAllowProcess = onBeforeProcess(errMessage);
                    if (isAllowProcess) {
                        if (IsValid(evt, 'fsMPEntryPopup', 'mpEntryPopup')) {
                            cbpMPEntryPopupProcess.PerformCallback('process');
                            if (errMessage.text != '')
                                displayErrorMessageBox('PROCESS', errMessage.text);
                        }
                    }
                    else if (errMessage.text != '')
                        displayErrorMessageBox('PROCESS', errMessage.text);
                });
            });
        </script>
        <dxcp:ASPxCallbackPanel ID="cbpMPEntryPopupContent" runat="server" Width="100%" ClientInstanceName="cbpMPEntryPopupContent"
            ShowLoadingPanel="false" OnCallback="cbpMPEntryPopupContent_Callback">
            <ClientSideEvents BeginCallback="function(s,e){
                showLoadingPanel();
            }" EndCallback="function(s,e){
                setEntryPopupIsAdd(true);
                hideLoadingPanel();
            }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server"> 
                    <fieldset id="fsMPEntryPopup">  
                        <asp:Panel ID="pnlEntryPopup" runat="server" />  
                    </fieldset>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>

        <dxcp:ASPxCallbackPanel ID="cbpMPEntryPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpMPEntryPopupProcess"
            ShowLoadingPanel="false" OnCallback="cbpMPEntryPopupProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e){
                showLoadingPanel();
            }" EndCallback="function(s,e){
                var result = s.cpResult.split('|');
                if(result[0] == 'process'){
                    if(result[1] == 'success'){
                        var param = s.cpRetval;
                         if(typeof onAfterProcessPopupEntry != 'undefined') {
                             onAfterProcessPopupEntry(param);
                         }
                        
                        if(typeof onGetEntryPopupReturnValue != 'undefined' && typeof onAfterSaveRightPanelContent != 'undefined')
                            onAfterSaveRightPanelContent($('#hdnRightPanelContentCode').val(), onGetEntryPopupReturnValue(), false);
                        
                        pcRightPanelContent.Hide();
                    }
                    else {
                        if(result[2] != '')
                            displayErrorMessageBox('PROCESS', result[2]);
                        else
                            displayErrorMessageBox('PROCESS', result[2]);

                        pcRightPanelContent.Hide();
                    }
                }
                else
                {
                    pcRightPanelContent.Hide();
                }
                hideLoadingPanel();
            }" />
        </dxcp:ASPxCallbackPanel>
    </div>

