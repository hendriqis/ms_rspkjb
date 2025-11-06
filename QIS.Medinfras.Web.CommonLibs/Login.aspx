<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPBase.Master" AutoEventWireup="true" 
    CodeBehind="Login.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Login" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPBase" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#btnLogin').click(function (evt) {
                if (IsValid(evt, 'fsLogin', 'mpLogin'))
                    cbpProcess.PerformCallback('login');
                return false;
            });

            var moduleID = $('#<%=hdnModuleID.ClientID %>').val();
            $('#lblEnter').click(function () {
                var link = '~/Libs/Program/RemoteLogon.aspx';
                if (loginData == '')
                    cbpProcess.PerformCallback('getdata|' + link + '|' + moduleID);
                else
                    openModule(link, moduleID);
            });
        });

        var loginData = '';
        var listWindow = [];
        function openModule(link, moduleID) {
            showLoadingPanel();
            var url = ResolveUrl(link);
            var windowID = 'Map' + moduleID;
            var win = window.open("", windowID, 'status=1,toolbar=0,menubar=0,resizable=1,location=0,scrollbars=1,fullscreen');
            if (win) {
                win.moveTo(0, 0);
                win.focus();
                listWindow.push(win);
                win.resizeTo(screen.width, screen.height - 20);

                var mapForm = document.createElement("form");
                mapForm.target = windowID;
                mapForm.method = "POST";
                mapForm.action = url;

                var mapInput = document.createElement("input");
                mapInput.type = "hidden";
                mapInput.name = "id";
                mapInput.value = loginData + '|' + $('#<%=hdnHealthcareID.ClientID %>').val() + '|1';
                mapForm.appendChild(mapInput);

                document.body.appendChild(mapForm);

                mapForm.submit();

                $(mapForm).remove();

            } else {
                showToast('Warning', 'You must allow popups for this map to work.');
            }
            hideLoadingPanel();
        }

        function onLnkLogoutClientClick() {
            for (var i = 0; i < listWindow.length; ++i) {
                if (!listWindow[i].closed) {
                    listWindow[i].close();
                }
            }
            listWindow = [];
            __doPostBack('<%=lnkLogout.UniqueID%>', '');
        }

        function onLoginSuccess(userName) {
            $('#<%=loginContainerLoginInfo.ClientID %>').hide();
            $('#<%=pnlUserLoginInformation.ClientID %>').show();
            $('#<%=pnlOpenWindowPopup.ClientID %>').show();
            $('#<%=lblUserLoginInfo.ClientID %>').html(userName);
            hideLoadingPanel();
        }
    </script>
    <input type="hidden" id="hdnModuleID" runat="server" />
    <input type="hidden" id="hdnHealthcareID" runat="server" />
    <div class="loginBg borderBox">Medinfras: <%=moduleName%></div>
    <div id="loginContainerLoginInfo" class="loginContainerLoginInfo" runat="server">
        <center>
            <fieldset id="fsLogin">     
                <table cellpadding="2">
                    <tr>
                        <td><%=GetLabel("User ID")%></td>
                        <td><asp:TextBox ID="txtUserName" Width="200px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><%=GetLabel("Password")%></td>
                        <td><asp:TextBox ID="txtPassword" TextMode="Password" Width="200px" runat="server" /></td>
                        <td><input type="submit" value="Log In" id="btnLogin" /></td>
                    </tr>
                </table>
            </fieldset>
        </center>
    </div>
    <div runat="server" id="pnlUserLoginInformation" class="loginContainerLoginInfo borderBox pnlUserLoginInformation">
        <div style="margin-top: 10px;">
            <%=GetLabel("Selamat Datang")%>, <span runat="server" id="lblUserLoginInfo"></span><br />
            <asp:LinkButton ID="lnkLogout" CssClass="lnkLogout" Text="[Logout]" OnClick="lnkLogout_Click" OnClientClick="onLnkLogoutClientClick();" runat="server" />
        </div>
    </div> 
    <div class="loginBg borderBox" style="height:330px">
        <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
            ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" 
                EndCallback="function(s,e){
                    if(s.cpParam == 'login'){
                        var result = s.cpResult.split('|');
                        if(result[0] == 'success'){
                            loginData = s.cpLoginData;
                            onLoginSuccess(result[1]);
                        }
                        else {
                            showToast('Login Failed', 'Error Message : ' + result[1]);
                            hideLoadingPanel();
                        }
                    }
                    else {
                        loginData = s.cpLoginData;
                        openModule(s.cpLink, s.cpModuleID);
                    }
                }" />
        </dxcp:ASPxCallbackPanel>
    </div>
    
    <center id="pnlOpenWindowPopup" runat="server">
        <a style="font-size: 30px" class="lblLink" id="lblEnter">[Enter]</a>
    </center>
</asp:Content>
