<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MPEntryPopupCtl2.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.MPEntryPopupCtl2" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<input type="hidden" runat="server" id="hdnIsAdd" value="1" />
<div class="toolbarArea">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
        <tr>
            <td style="width: 50%" id="tdToolbarButton">
                <ul>
                    <li style="display: none" runat="server" id="btnMPEntryPopupNew">
                        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnew.png")%>' alt="" /><div>
                            <%=GetLabel("New")%></div>
                    </li>
                    <li style="display: none" runat="server" id="btnMPEntryPopupSave">
                        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
                            <%=GetLabel("Save")%></div>
                    </li>
<%--                    <li style="display: none" runat="server" id="btnMPEntryPopupProposed">
                        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><div>
                            <%=GetLabel("Proposed")%></div>
                    </li>--%>
                    <li style="display: none" runat="server" id="btnMPEntryPopupVoid">
                        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
                            <%=GetLabel("Void")%></div>
                    </li>
                </ul>
            </td>
            <td style="width: 50%; text-align: right; padding-left: 5px; padding-right: 5px">
                <div style="font-size: 1.4em">
                    <%=HttpUtility.HtmlEncode(GetPopupTitle())%></div>
            </td>
        </tr>
    </table>
</div>
<div style="padding: 5px 0;">
    <script type="text/javascript" id="dxss_mpentrypopupctl2">
        window.setEntryPopupIsAdd = function (isAdd) {
            if (isAdd) {
                $('#<%=hdnIsAdd.ClientID %>').val('1');
            }
            else {
                $('#<%=hdnIsAdd.ClientID %>').val('0');
            }
        }
        window.getEntryPopupIsAdd = function () {
            return ($('#<%=hdnIsAdd.ClientID %>').val() == '1');
        }

        window.onRefreshCtlControl = function () {
            cbpMPEntryPopupContent.PerformCallback('refresh');
        }

        function addToolbarButton(htmlElement) {
            $("#tdToolbarButton ul").append(htmlElement);
        }

        function SetCustomVisibilityControl(IsAdd, IsEditable) {
            if (IsEditable == '1') {
                $('#<%=btnMPEntryPopupSave.ClientID %>').removeAttr('style');
                $('#<%=btnMPEntryPopupVoid.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%=btnMPEntryPopupSave.ClientID %>').attr('style', 'display:none');
                $('#<%=btnMPEntryPopupVoid.ClientID %>').attr('style', 'display:none');
            }
        }

        function SetCustomVisibilityControlOnNew() {
            $('#<%=btnMPEntryPopupSave.ClientID %>').removeAttr('style');
        }

        $(function () {
            $('#<%=btnMPEntryPopupSave.ClientID %>').show();
            $('#<%=btnMPEntryPopupNew.ClientID %>').show();
            $('#<%=btnMPEntryPopupVoid.ClientID %>').hide();

            $('#<%=btnMPEntryPopupNew.ClientID %>').click(function () {
                $('#<%=btnMPEntryPopupSave.ClientID %>').removeAttr('style');
                $('#<%=btnMPEntryPopupVoid.ClientID %>').attr('style', 'display:none');
                cbpMPEntryPopupContent.PerformCallback('new');
            });
            $('#<%=btnMPEntryPopupVoid.ClientID %>').click(function () {
                showToastConfirmation('Are You Sure Want To Void?', function (result) {
                    if (result) cbpMPEntryPopupProcess.PerformCallback('void');
                });
            });
            $('#<%=btnMPEntryPopupSave.ClientID %>').click(function (evt) {
                var errMessage = { text: "" };
                var isAllowSave = true;
                if (typeof onBeforeSaveRecord != 'undefined')
                    isAllowSave = onBeforeSaveRecord(errMessage);
                if (isAllowSave) {
                    if (IsValid(evt, 'fsMPEntryPopup', 'mpEntryPopup'))
                        cbpMPEntryPopupProcess.PerformCallback('save');
                }
            });
        });
    </script>
    <dxcp:ASPxCallbackPanel ID="cbpMPEntryPopupContent" runat="server" Width="100%" ClientInstanceName="cbpMPEntryPopupContent"
        ShowLoadingPanel="false" OnCallback="cbpMPEntryPopupContent_Callback">
        <ClientSideEvents BeginCallback="function(s,e){
                showLoadingPanel();
            }" EndCallback="function(s,e){
                setEntryPopupIsAdd(true);
                SetCustomVisibilityControlOnNew();
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
                        var param = s.cpRetval;                        
                        if(result[0] == 'saveadd' && typeof onAfterSaveAddRecordEntryPopup != 'undefined')
                            onAfterSaveAddRecordEntryPopup(param);
                        if(result[0] == 'saveedit' && typeof onAfterSaveEditRecordEntryPopup != 'undefined')
                            onAfterSaveEditRecordEntryPopup(param);

                        SetCustomVisibilityControl(1, 1);
                        var isAdd = false;
                        if(result[0] == 'saveadd')
                            isAdd = true;

                        if(result[2] != '')
                            showToast('Save Failed', 'Error Message : ' + result[2]);
                    }
                    else
                        if(result[2] != '')
                            showToast('Save Failed', 'Error Message : ' + result[2]);
                        else
                            showToast('Save Failed', '');
                }
                else if(result[0] == 'propose') {
                    if(result[1] == 'success') {
                        var param = s.cpRetval;
                        SetCustomVisibilityControl(0, 0);
                        onAfterProposedRecordEntryPopup(param);
                    }
                    else {
                        if(result[2] != '') {
                            showToast('Proposed Failed', 'Error Message : ' + result[2]);
                        }
                        else {
                            showToast('Proposed Failed', '');
                        }
                   }
                }
                else if(result[0] == 'void') {
                    if(result[1] == 'success') {
                        var param = s.cpRetval;
                        SetCustomVisibilityControl(0, 0);
                        onAfterVoidRecordEntryPopup(param);
                    }
                    else {
                        if(result[2] != '') {
                            showToast('Proposed Failed', 'Error Message : ' + result[2]);
                        }
                        else {
                            showToast('Proposed Failed', '');
                        }
                   }
                }
                hideLoadingPanel();
            }" />
    </dxcp:ASPxCallbackPanel>
</div>
