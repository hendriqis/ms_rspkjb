<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegistrationVoidCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.RegistrationVoidCtl" %>
    
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_patientregistrationvoidctl">
    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnVoidRegistration.ClientID %>').click(function (evt) {
        var filterRegLinked = "LinkedRegistrationID = " + $('#<%=hdnRegID.ClientID %>').val() + " AND GCRegistrationStatus != '" + Constant.RegistrationStatus.CANCELLED + "'";
        Methods.getObject('GetRegistrationList', filterRegLinked, function (result) {
            if (result != null) {
                displayErrorMessageBox('ERROR', "Maaf, registrasi ini tidak dapat dibatalkan karena sudah digunakan sebagai Registrasi Asal di nomor " + result.RegistrationNo);
            } else {
                if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
                    cbpPatientRegistrationVoid.PerformCallback('void');
                }
            }
        });
    });

    function onVoidPatientRegistrationSuccess() {
        var deptID = $('#<%=hdnDeptID.ClientID %>').val();

        if (deptID == Constant.Facility.INPATIENT) {
            var isBridgingAplicares = $('#<%=hdnIsBridgingToAplicares.ClientID %>').val();
            if (isBridgingAplicares == "1") {
                UpdateRoomAplicares();
            }
        }

        pcRightPanelContent.Hide();
        onRefreshControl();
    }

    function onCboVoidReasonChanged() {
        if (cboVoidReason.GetValue() != 'X129^999')
            $('#<%=trReason.ClientID %>').attr('style', 'display:none');
        else
            $('#<%=trReason.ClientID %>').removeAttr('style');
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnVoidRegistration" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear:both"/>
            <div><%=GetLabel("Process")%></div>
        </li>
    </ul>
</div>

<div style="padding:10px;">
    <fieldset id="fsTrxPopup" style="margin:0"> 
        <input type="hidden" id="hdnIsBridgingToAplicares" runat="server" />                          
        <input type="hidden" id="hdnRegID" runat="server" value="" />   
        <input type="hidden" id="hdnVisitID" runat="server" value="" />
        <input type="hidden" id="hdnDeptID" runat="server" value="" />
        <input type="hidden" id="hdnParam" runat="server" value="" />    
        <input type="hidden" id="hdnIsBridgingToGateway" runat="server" value="" />    
        <input type="hidden" id="hdnProviderGatewayService" runat="server" value="" /> 
        <input type="hidden" id="hdnMenggunakanValidasiChiefComplaint" runat="server" value="" />
        <input type="hidden" id="hdnIsVoidRegistrationDeleteLinkedReg" runat="server" value="" />
        <input type="hidden" id="hdnIsBridgingToIPTV" runat="server" value="" />
        <input type="hidden" id="hdnIsBridgingToMobileJKN" value="0" runat="server" />
        <input type="hidden" id="hdnIsBridgingToSistemAntrian" value="0" runat="server" />
        <input type="hidden" id="hdnIsInpatientUsingConfirmation" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width:250px"/>
                <col style="width:200px"/>
            </colgroup>
            <tr>
                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Void Reason")%></label></td>
                <td>
                    <dxe:ASPxComboBox runat="server" ID="cboVoidReason" ClientInstanceName="cboVoidReason" Width="200px">
                        <ClientSideEvents ValueChanged="function(s,e){ onCboVoidReasonChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr id="trReason" runat="server" style="display:none">
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Reason")%></label></td>
                <td><asp:TextBox ID="txtReason" Width="200px" runat="server" /></td>
            </tr>
        </table>
    
        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div> 

        <dxcp:ASPxCallbackPanel ID="cbpPatientRegistrationVoid" runat="server" Width="100%" ClientInstanceName="cbpPatientRegistrationVoid"
            ShowLoadingPanel="false" OnCallback="cbpPatientRegistrationVoid_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" 
                EndCallback="function(s,e){
                    var result = s.cpResult.split('|');
                    if(result[0] == 'fail')
                        showToast('Save Failed', result[1]);
                    else
                        onVoidPatientRegistrationSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>

