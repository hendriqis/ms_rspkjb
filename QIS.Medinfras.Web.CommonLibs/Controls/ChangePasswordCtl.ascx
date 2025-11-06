<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePasswordCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.ChangePasswordCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

<script type="text/javascript" id="dxss_changepasswordctl">
    $('#btnSaveChangePassword').click(function (evt) {
        var newPass = $('#<%=txtNewPassword.ClientID %>').val();
        var confPass = $('#<%=txtConfirmPassword.ClientID %>').val();
        if (newPass == confPass) {
            cbpChangePasswordProcess.PerformCallback('save');
        }
        else {
            displayErrorMessageBox('Change password failed', 'Error Message : Password baru berbeda dengan konfirmasi password');
        }
    });

    function displayErrorMessageBox(title, message, fn) {
        functionHandlerCloseToast = fn;
        $('#messageBoxError .messageTitle').html('ERROR : ' + title);
        $('#messageBoxError .messageBody').html(message);
        $('#messageBoxError').show();
    }

    function onCbpChangePasswordProcessEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'fail') {
            displayErrorMessageBox('Change password failed', 'Error Message : ' + param[1]);
        }
        else {

            displayErrorMessageBox("Success", "Change password success and it will be affected on next login");
            pcRightPanelContent.Hide();
        }
        hideLoadingPanel();
    }
</script>

<fieldset id="fsChangePassword" style="margin:0"> 
    <table class="tblEntryContent" style="width:100%">
        <colgroup>
            <col style="width:145px"/>
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Password Lama")%></label></td>
            <td><asp:TextBox ID="txtOldPassword" CssClass="required" TextMode="Password" Width="100%" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Password Baru")%></label></td>
            <td><asp:TextBox ID="txtNewPassword" CssClass="required" TextMode="Password" Width="100%" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Konfirmasi Password")%></label></td>
            <td><asp:TextBox ID="txtConfirmPassword" CssClass="confirmpasswordchangepassword required" TextMode="Password" Width="100%" runat="server" /></td>
        </tr>
    </table>
</fieldset>
<div style="width:100%;text-align:center">
    <table style="margin-left: auto; margin-right: auto; margin-top: 10px;">
        <tr>
            <td><input type="button" value='<%= GetLabel("Simpan")%>' style="width:70px" id="btnSaveChangePassword" class="w3-btn w3-hover-blue" /></td>
            <%--<td><input type="button" value='<%= GetLabel("Tutup")%>' style="width:70px" onclick="pcRightPanelContent.Hide();" class="w3-btn w3-hover-blue"/></td>--%>
        </tr>
    </table>
</div>
<div id="messageBoxError" class="w3-modal">
    <div class="w3-modal-content w3-animate-zoom">
        <header class="w3-container w3-red"> 
      <span onclick="document.getElementById('messageBoxError').style.display='none'" 
      class="w3-button w3-display-topright">&times;</span>
      <h2 class="messageTitle"></h2>
    </header>
        <div class="w3-container">
            <p>
                <div class="messageBody" class="messageBody" style="min-height: 125px; max-height: 250px;
                    overflow: auto">
                </div>
            </p>
        </div>
        <footer class="w3-container w3-red">
      <p style="text-align:center"><input class="w3-btn w3-black w3-round-xxlarge w3-hover-blue" style="width:100px" onclick="document.getElementById('messageBoxError').style.display='none'" value="OK" /></p>
    </footer>
    </div>
</div>

<dxcp:ASPxCallbackPanel ID="cbpChangePasswordProcess" runat="server" Width="100%" ClientInstanceName="cbpChangePasswordProcess"
    ShowLoadingPanel="false" OnCallback="cbpChangePasswordProcess_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
        EndCallback="function(s,e){ onCbpChangePasswordProcessEndCallback(s); }" />
</dxcp:ASPxCallbackPanel>

