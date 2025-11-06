<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPBase.Master"
    AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Login" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPBase" runat="server">
    <style>
        .dxpcCloseButton
        {
            display: none;
        }
    </style>
    <script type="text/javascript">

        var loginData = '';
        $(function () {
            IsResetCheckin();

            $('.imgOpenModule.enabled').live('click', function () {
                if ($('#<%=pnlUserLoginInformation.ClientID %>').is(":visible")) {
                    var link = $(this).attr('link');
                    var moduleID = $(this).attr('moduleid');
                    if (loginData == '') {
                        cbpProcess.PerformCallback('getdata|' + link + '|' + moduleID);
                    }
                    else {
                        openModule(link, moduleID);
                        //                        maximize();
                    }
                }
            });

            $('#btnLogin').click(function (evt) {
                if (IsValid(evt, 'fsLogin', 'mpLogin'))
                    cbpProcess.PerformCallback('login');
                return false;
            });



        });

        //        function maximize() {
        //            window.moveTo(0, 0);
        //            window.resizeTo(screen.availWidth, screen.availHeight);
        //        }

        var listWindow = [];
        function openModule(link, moduleID) {
            showLoadingPanel();
            cbpProcess.PerformCallback('openModule|' + link + "|" + moduleID);
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
            $('#<%=lblUserLoginInfo.ClientID %>').html(userName);
            cbpSelectUserRole.PerformCallback();
        }

        $('#<%=ddlHealthcare.ClientID %>').live('change', function () {
            cbpRptModule.PerformCallback($('#<%=ddlHealthcare.ClientID %>').val());
        });

        function onCbpSelectUserRoleEndCallback() {
            cbpRptModule.PerformCallback($('#<%=ddlHealthcare.ClientID %>').val());
        }
        function onCbpProcess(s) {
            if (s.cpParam == 'login') {
                var result = s.cpResult.split('|');
                if (result[0] == 'success') {
                    loginData = s.cpLoginData;
                    onLoginSuccess(result[1]);
                    IsResetCheckin();
                    IsCashier();

                }
                else {
                    var messageBody = result[1];
                    displayErrorMessageBox('USER LOGIN', messageBody);
                    hideLoadingPanel();
                }
            }
            else if (s.cpParam == 'reset') {
                var result = s.cpResult.split('|');
                if (result[0] == 'success') {
                    IsResetCheckin();
                    ShowSnackbarNotification(result[1]);
                    window.location.reload(true);
                }
                else {
                    var messageBody = result[1];
                    ShowSnackbarError('RESET PASSWORD ' + messageBody);

                }
                hideLoadingPanel();
            }
            else if (s.cpParam == 'selectCashierGroup') {
                $('#divSelectCashierGroup').hide();
                hideLoadingPanel();
            }
            else if (s.cpParam == 'refresh') {
                IsResetCheckin();
            }
            else if (s.cpParam == 'openModule') {
                if (s.cpLink != "") {
                    loginData = s.cpLoginData;
                    var url = ResolveUrl(s.cpLink);
                    var windowID = 'Map' + s.cpModuleID;
                    var win = window.open("", windowID, 'status=1,toolbar=0,menubar=0,resizable=1,scrollbars=1,fullscreen');
                    if (win) {
                        //win.moveTo(0, 0);
                        win.focus();
                        listWindow.push(win);
                        //win.resizeTo(screen.availWidth, screen.availHeight);

                        var mapForm = document.createElement("form");
                        mapForm.target = windowID;
                        mapForm.method = "POST";
                        mapForm.action = url;
                        var mapInput = document.createElement("input");
                        mapInput.type = "hidden";
                        mapInput.name = "id";

                        mapInput.value = loginData + '|' + $('#<%=ddlHealthcare.ClientID %>').val() + '|1';
                        mapForm.appendChild(mapInput);

                        document.body.appendChild(mapForm);

                        mapForm.submit();

                        $(mapForm).remove();

                    } else {
                        var messageBody = "Konfigurasi atau opsi <b>Allow Popups</b> di browser harus diaktifkan untuk penggunaan aplikasi Medinfras.";
                        displayMessageBox('MEDINFRAS', messageBody);
                    }
                    hideLoadingPanel();
                }
                else {
                    loginData = s.cpLoginData;
                    openModule(s.cpLink, s.cpModuleID);
                }
            }
            else {
                var messageBody = "Harap lakukan logout/login ulang kembali.";
                displayMessageBox('MEDINFRAS', messageBody);
                hideLoadingPanel();
            }
        }
        function checkConfirmPasswordChangePassword(value, element) {
            return value == $('#<%=txtNewPassword.ClientID %>').val();
        }

        jQuery.validator.addMethod("confirmpasswordchangepassword", checkConfirmPasswordChangePassword, "");

        $('#btnSaveChangePassword').live('click', function (evt) {
            if (IsValid(evt, 'fsChangePassword', 'mpChangePassword')) {
                if ($('#<%=txtNewPassword.ClientID %>').val() == $('#<%=txtConfirmPassword.ClientID %>').val()) {
                    $('#loading').show();
                    cbpProcess.PerformCallback('reset');

                } else {
                    $('#loading').hide();
                    ShowSnackbarError('password baru dan konfirm password tidak sesuai.');
                }

            } else {
                $('#loading').hide();
                ShowSnackbarError('Harap isi untuk semua field yang tersedia');
            }

            return false;
        });

        function IsResetCheckin() {
            var isResetPassword = $('#<%=hdnIsResetPassword.ClientID %>').val();
            if (isResetPassword == "1") {
                $('#changePassword').show();
            } else {
                $('#changePassword').hide();
            }
        }

        function IsCashier() {
            var isCashier = $('#<%=hdnIsCashier.ClientID %>').val();
            if (isCashier == "1") {
                $('#divSelectCashierGroup').show();
            } else {
                $('#divSelectCashierGroup').hide();
            }
        }

        $('#btnSelectCashierGroup').live('click', function (evt) {
            cbpProcess.PerformCallback('selectCashierGroup');
            return true;
        });

         
    </script>
    <style type="text/css">
        body
        {
            background-image: url('<%=GetBackgroundImage()%>');
        }
    </style>
    <div style="float: right; margin: 5px 10px 0 0;">
        <img src='<%=ResolveUrl("~/Libs/Images/qislogos.png")%>' alt="" /></div>
    <div class="loginBg borderBox">
        <img src='<%=ResolveUrl("~/Libs/Images/medinfras_logo.png")%>' alt="" /></div>
    <div id="loginContainerLoginInfo" class="loginContainerLoginInfo" runat="server">
        <center>
            <fieldset id="fsLogin">
                <table cellpadding="2">
                    <tr>
                        <td>
                            <%=GetLabel("User ID")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserName" Width="200px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=GetLabel("Password")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPassword" TextMode="Password" Width="200px" runat="server" />
                        </td>
                        <td style="text-align: left">
                            <input type="submit" value="Log In" id="btnLogin" class="btnNormal" style="height: 30px" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </center>
    </div>
    <div runat="server" id="pnlUserLoginInformation" class="loginContainerLoginInfo borderBox pnlUserLoginInformation">
        <div class="borderBox" style="float: right; font-size: 0.8em;">
            <dxcp:ASPxCallbackPanel ID="cbpSelectUserRole" runat="server" Width="100%" ClientInstanceName="cbpSelectUserRole"
                ShowLoadingPanel="false" OnCallback="cbpSelectUserRole_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpSelectUserRoleEndCallback(); }" />
                <PanelCollection>
                    <dx:PanelContent ID="pnlSelectUserRole" runat="server">
                        <table style="margin-top: 10px">
                            <tr>
                                <td align="right" colspan="2">
                                    <%=GetLabel("Rumah Sakit")%>
                                    <asp:DropDownList ID="ddlHealthcare" runat="server" Width="350px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2" style="font-size:smaller">
                                    <%=GetLabel("Licensed to : ")%><%=GetHealthcareNameAndPatchVersionInfo() %>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: right;">
                                    <div runat="server" id="pnlInvalidApplicationLicense" style="display: none">
                                        <span style="color: Red">
                                            <%=GetLabel("Invalid Application License. For license information, please mail to ")%><a
                                                style="color: Blue" href="mailto:support@medinfras.com">support@medinfras.com</a></span>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
        </div>
        <div style="margin-top: 10px;">
            <%=GetLabel("Selamat Datang")%>, <span runat="server" id="lblUserLoginInfo"></span>
            <br />
            <asp:LinkButton ID="lnkLogout" CssClass="lnkLogout" Text="[Logout]" OnClick="lnkLogout_Click"
                OnClientClick="onLnkLogoutClientClick();" runat="server" />
        </div>
    </div>
    <div class="loginBg borderBox" style="padding: 1% 0; width: 100%; height: 500px">
        <table style="width: 100%;">
            <colgroup>
                <col style="width: 46%" />
            </colgroup>
            <tr>
                <td valign="top" style="text-align: center;">
                    <div id="divHealthcareInfo" style="width: 580px; position: relative; margin-left: 20px;
                        height: 172px; display: block" runat="server">
                        <img src='<%=ResolveUrl("~/Libs/Images/healthcarelogo.png")%>' style="position: absolute;
                            z-index: 2; left: 10px; top: 0px;" alt="" />
                    </div>
                </td>
                <td valign="top">
                    <div style="text-align: left; width: 100%; padding-left: 0px;">
                        <dxcp:ASPxCallbackPanel ID="cbpRptModule" runat="server" Width="100%" ClientInstanceName="cbpRptModule"
                            ShowLoadingPanel="false" OnCallback="cbpRptModule_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Repeater ID="rptModule" runat="server">
                                        <HeaderTemplate>
                                            <ul id="loginUlListModule">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li class="<%#: Eval("CssClass")%>">
                                                <img class="imgOpenModule <%#: Eval("CssClass")%>" src='<%# Eval("ImageUrl")%>' alt=""
                                                    link='<%#: Eval("Link")%>' moduleid='<%#: Eval("ModuleID")%>' />
                                                <div class="<%#: Eval("CssClass")%>">
                                                    <%#: Eval("ModuleName")%></div>
                                            </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
											<li class="enabled" style="display:none">
                                                <img class="enabled" src='/medinfras/rsmd/SystemSetup/Libs/Images/Module/report.png' alt=""
                                                    onclick="goreport(); return false;" />
                                                <div class="enabled">
                                                    REPORT</div>
											</li>	
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
                ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){onCbpProcess(s)}" />
                <PanelCollection>
                    <dx:PanelContent>
                        <asp:HiddenField ID="hdnIsResetPassword" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnIsCashier" runat="server" Value="0" />
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
        </div>
    </div>
    <!-- region Layout SetPassword -->
    <div id="changePassword" class="w3-modal">
        <div class="w3-modal-content w3-card-4 w3-animate-zoom" style="max-width: 600px">
            <div class="w3-card-4">
                <div class="w3-container w3-blue">
                    <h2>
                        Reset Password</h2>
                </div>
                <div class="w3-container">
                    <fieldset id="fsChangePassword" style="margin: 0">
                        <p>
                            <label class="w3-text">
                                <b>Password Lama</b></label>
                            <asp:TextBox ID="txtOldPassword" TextMode="Password" runat="server" CssClass="w3-input w3-border w3-sand"
                                Width="90%" />
                        </p>
                        <p>
                            <label class="w3-text">
                                <b>Password Baru</b></label>
                            <asp:TextBox ID="txtNewPassword" TextMode="Password" runat="server" CssClass="w3-input w3-border w3-sand"
                                Width="90%" />
                        </p>
                        <p>
                            <label class="w3-text">
                                <b>Konfirm Password</b></label>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="w3-input w3-border w3-sand"
                                Width="90%" />
                        </p>
                        <p id="loading" style="display: none;">
                            Loading.....</p>
                        <p>
                            <input type="button" value="Kirim" class="w3-btn w3-blue" id="btnSaveChangePassword" />
                        </p>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
    <!-- endregion  -->
    <!-- region Layout SelectCashierGroup -->
    <div id="divSelectCashierGroup" class="w3-modal">
        <div class="w3-modal-content w3-card-4 w3-animate-zoom" style="max-width: 600px">
            <div class="w3-card-4">
                <div class="w3-container w3-blue">
                    <h2>
                        Select Cashier Group</h2>
                </div>
                <div class="w3-container">
                    <fieldset id="fsSelectCashierGroup" style="margin: 0">
                        <p>
                            <dxe:ASPxComboBox ID="cboCashierGroup" ClientInstanceName="cboCashierGroup" Width="100%"
                                runat="server" />
                        </p>
                        <p>
                            <input type="button" value="Simpan" class="w3-btn w3-blue" id="btnSelectCashierGroup" />
                        </p>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
    <!-- endregion  -->
    <input type="hidden" id="hdnPatchVersion" runat="server" />
    <div id="loginFooter" class="loginBg borderBox">
        <div>
            2013 © PT. Quantum Infra Solusindo</div>
    </div>
	<script>
	    function goreport() {
	        var str = document.body.innerHTML;
	        var patt = new RegExp("userid=[0-9]*&username=([0-9]|[a-z]|[A-Z])*");
	        var res = patt.exec(str)[0];
	        $.post("https://" + location.host + "/bimasm/account/LoginByPass", res,
				function (d) {
				    if (d.STATUS == "OK") {
				        window.open("https://" + location.host + "/bimasm/report");
				    } else {
				        alert("MOHON MAAF..! ANDA TIDAK MEMPUNYAI AKSES KE MODUL REPORT");
				    }
				}, "json");
	    }
	</script>	
</asp:Content>
