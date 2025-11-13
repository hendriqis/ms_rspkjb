<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="UserManagementEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.UserManagementEntry" %>
    
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function checkConfirmPassword(value, element) {
            return value == $('#<%=txtPassword.ClientID %>').val();
        }
        function checkConfirmMobilePIN(value, element) {
            return value == $('#<%=txtMobilePIN.ClientID %>').val();
        }

        jQuery.validator.addMethod("confirmpassword", checkConfirmPassword, "");
        jQuery.validator.addMethod("confirmmobilepin", checkConfirmMobilePIN, "");

        function onLoad() {
            //#region CopyUser
            $('#lblCopyFromUser.lblLink').click(function () {
                var filterExpression = "IsDeleted = 0 AND UserID != 1";
                openSearchDialog('user', filterExpression, function (value) {
                    $('#<%=txtCopyUserName.ClientID %>').val(value);
                    onTxtCopyUserNameChanged(value);
                });
            });

            $('#<%=txtCopyUserName.ClientID %>').change(function () {
                onTxtCopyUserNameChanged($(this).val());
            });

            function onTxtCopyUserNameChanged(value) {
                var filterExpression = "UserID ='" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvUserList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnCopyUserID.ClientID %>').val(result.UserID);
                        $('#<%=txtCopyUserName.ClientID %>').val(result.UserName);
                        $('#<%=txtCopyUserFullName.ClientID %>').val(result.FullName);
                    }
                    else {
                        $('#<%=hdnCopyUserID.ClientID %>').val('');
                        $('#<%=txtCopyUserName.ClientID %>').val('');
                        $('#<%=txtCopyUserFullName.ClientID %>').val('');
                    }

                }); 
            }

            //#region Paramedic
            $('#lblParamedic.lblLink').click(function () {
                var filterExpression = "";
                openSearchDialog('paramedic', filterExpression, function (value) {
                    $('#<%=txtParamedicCode.ClientID %>').val(value);
                    onTxtParamedicCodeChanged(value);
                });
            });

            $('#<%=txtParamedicCode.ClientID %>').change(function () {
                onTxtParamedicCodeChanged($(this).val());
            });

            function onTxtParamedicCodeChanged(value) {
                var filterExpression = "ParamedicCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnParamedicID.ClientID %>').val(result.ParamedicID);
                        $('#<%=txtParamedicName.ClientID %>').val(result.ParamedicName);
                    }
                    else {
                        $('#<%=hdnParamedicID.ClientID %>').val('');
                        $('#<%=txtParamedicCode.ClientID %>').val('');
                        $('#<%=txtParamedicName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Employee
            function getEmployeeFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }
            $('#<%:lblEmployee.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('employeeNew', getEmployeeFilterExpression(), function (value) {
                    var param = value.split('|');
                    $('#<%:hdnEmployeeID.ClientID %>').val(param[0]);
                    $('#<%:txtEmployeeCode.ClientID %>').val(param[1]);
                    $('#<%:txtEmployeeName.ClientID %>').val(param[2]);
                });
            });

            $('#<%:txtEmployeeCode.ClientID %>').change(function () {
                onTxtEmployeeCodeChanged($(this).val());
            });

            function onTxtEmployeeCodeChanged(value) {
                var filterExpression = getEmployeeFilterExpression() + " AND EmployeeCode = '" + value + "'";
                Methods.getObject('GetEmployeeList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%:hdnEmployeeID.ClientID %>').val(result.EmployeeID);
                        $('#<%:txtEmployeeName.ClientID %>').val(result.FullName);
                    }
                    else {
                        $('#<%:hdnEmployeeID.ClientID %>').val('');
                        $('#<%:txtEmployeeCode.ClientID %>').val('');
                        $('#<%:txtEmployeeName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <h4><%=GetLabel("Informasi Umum")%></h4>
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:25%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Pengguna")%></label></td>
                        <td><asp:TextBox ID="txtUserName" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Lengkap")%></label></td>
                        <td><asp:TextBox ID="txtFullName" Width="100%" runat="server" /></td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Email")%></label></td>
                        <td><asp:TextBox ID="txtEmail" Width="100%" CssClass="email" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kata Sandi")%></label></td>
                        <td><asp:TextBox ID="txtPassword" TextMode="Password" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Konfirmasi Kata Sandi")%></label></td>
                        <td><asp:TextBox ID="txtConfirmPassword" TextMode="Password" CssClass="confirmpassword" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label ><%=GetLabel("Wajib Reset Password")%></label></td>
                        <td><asp:CheckBox ID="chkIsResetPassword" runat="server" /></td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("PIN Ponsel")%></label></td>
                        <td><asp:TextBox ID="txtMobilePIN" Width="300px" runat="server" /></td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Konfirmasi PIN Ponsel")%></label></td>
                        <td><asp:TextBox ID="txtConfirmMobilePIN" CssClass="confirmmobilepin" Width="300px" runat="server" /></td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Pertanyaan Keamanan")%></label></td>
                        <td><asp:TextBox ID="txtSecurityQuestion" Width="100%" runat="server" /></td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Jawaban Keamanan")%></label></td>
                        <td><asp:TextBox ID="txtSecurityAnswer" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" runat="server" id="lblEmployee">
                                <%:GetLabel("Pegawai")%></label>
                        </td>
                        <td>
                            <input type="hidden" runat="server" id="hdnEmployeeID" value="" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtEmployeeCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmployeeName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblParamedic"><%=GetLabel("Paramedis")%></label></td>
                        <td>
                            <input type="hidden" id="hdnParamedicID" runat="server" value="" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:100px"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtParamedicCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtParamedicName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label ><%=GetLabel("User Kasir")%></label></td>
                        <td><asp:CheckBox ID="chkIsCashier" runat="server" /></td>
                    </tr>
                     <tr>
                        <td class="tdLabel"><label ><%=GetLabel("NIK (EKLAIM)")%></label></td>
                       <td><asp:TextBox ID="txtSSN" Width="100%" runat="server" /></td>
                    </tr>
                </table>
            </td>
            <td style="padding:5px;vertical-align:top">
                <asp:Panel ID="pnlCustomAttribute" runat="server">
                    <h4><%=GetLabel("Atribut")%></h4>
                    <asp:Repeater ID="rptCustomAttribute" runat="server">
                        <HeaderTemplate>
                            <table class="tblEntryContent" style="width:100%">
                                <colgroup>
                                    <col style="width:30%"/>
                                </colgroup>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%#: Eval("Value") %></label></td>
                                <td>
                                    <input type="hidden" value='<%#: Eval("Code") %>' runat="server" id="hdnTagFieldCode" />
                                    <asp:TextBox ID="txtTagField" Width="300px" runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </asp:Panel>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="padding:5px; vertical-align:top">
                <div id="divCopyUser" runat="server" visible = "false">
                    <h4><%=GetLabel("Copy From User")%></h4>
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:30%"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblLink" id="lblCopyFromUser"><%=GetLabel("Copy From User")%></label></td>
                            <td>
                                <input type="hidden" id = "hdnCopyUserID" runat="server" value="" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%" />
                                        <col style="width:3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtCopyUserName" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtCopyUserFullName" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
