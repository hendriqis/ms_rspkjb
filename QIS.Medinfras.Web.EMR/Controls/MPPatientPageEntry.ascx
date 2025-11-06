<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MPPatientPageEntry.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Controls.MPPatientPageEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<input type="hidden" runat="server" id="hdnIsAdd" value="1" />
<div class="toolbarArea">
    <ul>
        <li style="display:none" id="btnMPEntryPopupNew"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnew.png")%>' alt="" /><div><%=GetLabel("New")%></div></li>
        <li style="display:none" id="btnMPEntryPopupSave"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div><%=GetLabel("Save")%></div></li>
    </ul>
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

        $(function () {
            $('#btnMPEntryPopupSave').show();
            if ($('#<%=hdnIsAdd.ClientID %>').val() == '1') {
                //$('#btnMPEntryPopupNew').show();
            }

            $('#btnMPEntryPopupNew').click(function () {
                cbpMPEntryPopupContent.PerformCallback('new');
            });
            $('#btnMPEntryPopupSave').click(function (evt) {
                var errMessage = { text: "" };
                var isAllowSave = true;
                if (typeof onBeforeSaveRecordEntryPopup != 'undefined') {
                    isAllowSave = onBeforeSaveRecordEntryPopup(errMessage);
                }
                if (isAllowSave) {
                    if (IsValid(evt, 'fsMPEntryPopup', 'mpEntryPopup'))
                        cbpMPEntryPopupProcess.PerformCallback('save');
                }
                /*else {
                    if (errMessage.text == '')
                        alert('Cannot Save This Record');
                    else
                        alert('Cannot Save This Record. ' + errMessage.text);
                }*/
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
            if(result[0] == 'saveadd' || result[0] == 'saveedit'){
                if(result[1] == 'success'){
                    if (typeof onAfterSaveRecordPatientPageEntry == 'function'){
                        onAfterSaveRecordPatientPageEntry(s.cpRetval);
                    }
                    if(typeof onRefreshControl != 'undefined')
                        onRefreshControl('');
                    pcRightPanelContent.Hide();
                }
                else
                    if(result[2] != '')
                        showToast('Save Failed', 'Error Message : ' + result[2]);
                    else                        
                        showToast('Save Failed', '');
            }
            hideLoadingPanel();
        }" />
    </dxcp:ASPxCallbackPanel>
</div>


