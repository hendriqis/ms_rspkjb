<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="MenuEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.MenuEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
    
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setCRUDModeEnabled();

            $('#<%=chkRead.ClientID %>').change(function () {
                setCRUDModeEnabled();
            });

            //#region Parent
            function getParentFilterExpression() {
                var filterExpression = "ModuleID = '" + cboModule.GetValue() + "' AND IsHeader = 1";
                return filterExpression;
            }

            $('#lblParent.lblLink').click(function () {
                openSearchDialog('menu', getParentFilterExpression(), function (value) {
                    $('#<%=txtParentCode.ClientID %>').val(value);
                    onTxtParentCodeChanged(value);
                });
            });

            $('#<%=txtParentCode.ClientID %>').change(function () {
                onTxtParentCodeChanged($(this).val());
            });

            function onTxtParentCodeChanged(value) {
                var filterExpression = getParentFilterExpression() + " AND MenuCode = '" + value + "'";
                Methods.getObject('GetMenuMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnParentID.ClientID %>').val(result.MenuID);
                        $('#<%=txtParentName.ClientID %>').val(result.MenuCaption);
                    }
                    else {
                        $('#<%=hdnParentID.ClientID %>').val('');
                        $('#<%=txtParentCode.ClientID %>').val('');
                        $('#<%=txtParentName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }

        function setCRUDModeEnabled() {
            var isEnabled = $('#<%=chkRead.ClientID %>').is(":checked");
            if (!isEnabled) {
                $('#<%=chkCreate.ClientID %>').prop('checked', false);
                $('#<%=chkUpdate.ClientID %>').prop('checked', false);
                $('#<%=chkDelete.ClientID %>').prop('checked', false);
                $('#<%=chkPrint.ClientID %>').prop('checked', false);
                $('#<%=chkApprove.ClientID %>').prop('checked', false);
                $('#<%=chkVoid.ClientID %>').prop('checked', false);
                $('#<%=chkVoidWithReason.ClientID %>').prop('checked', false);

                $('#<%=chkCreate.ClientID %>').attr("disabled", true);
                $('#<%=chkUpdate.ClientID %>').attr("disabled", true);
                $('#<%=chkDelete.ClientID %>').attr("disabled", true);
                $('#<%=chkPrint.ClientID %>').attr("disabled", true);
                $('#<%=chkApprove.ClientID %>').attr("disabled", true);
                $('#<%=chkVoid.ClientID %>').attr("disabled", true);
                $('#<%=chkVoidWithReason.ClientID %>').attr("disabled", true);
            }
            else {
                $('#<%=chkCreate.ClientID %>').removeAttr("disabled");
                $('#<%=chkUpdate.ClientID %>').removeAttr("disabled");
                $('#<%=chkDelete.ClientID %>').removeAttr("disabled");
                $('#<%=chkPrint.ClientID %>').removeAttr("disabled");
                $('#<%=chkApprove.ClientID %>').removeAttr("disabled");
                $('#<%=chkVoid.ClientID %>').removeAttr("disabled");
                $('#<%=chkVoidWithReason.ClientID %>').removeAttr("disabled");
            }
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Menu")%></div>--%>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:40%"/>
            <col style="width:33%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:30%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Menu Code")%></label></td>
                        <td><asp:TextBox ID="txtMenuCode" Width="200px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Module")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboModule" ClientInstanceName="cboModule" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Menu Caption")%></label></td>
                        <td><asp:TextBox ID="txtMenuCaption" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Help Link For List")%></label></td>
                        <td><asp:TextBox ID="txtHelpLinkForList" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Help Link For Entry")%></label></td>
                        <td><asp:TextBox ID="txtHelpLinkForEntry" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Menu Tooltip")%></label></td>
                        <td><asp:TextBox ID="txtMenuToolTip" Width="100%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                    </tr>
                </table>
            </td>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:30%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblParent"><%=GetLabel("Parent")%></label></td>
                        <td>
                            <input type="hidden" value="" runat="server" id="hdnParentID" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtParentCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtParentName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Menu Url")%></label></td>
                        <td><asp:TextBox ID="txtMenuUrl" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Image Url")%></label></td>
                        <td><asp:TextBox ID="txtImageUrl" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Menu Level")%></label></td>
                        <td><asp:TextBox ID="txtMenuLevel" CssClass="required number" Width="150px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Menu Index")%></label></td>
                        <td><asp:TextBox ID="txtMenuIndex" CssClass="required number" Width="150px" runat="server" /></td>
                    </tr>
                </table>
            </td>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <tr><td><asp:CheckBox ID="chkIsHeader" runat="server" /> <%=GetLabel("Header")%></td></tr>
                    <tr><td><asp:CheckBox ID="chkIsShowInPullDownMenu" runat="server" /> <%=GetLabel("Show In Pull Down Menu")%></td></tr>
                    <tr><td><asp:CheckBox ID="chkIsVisible" runat="server" /> <%=GetLabel("Visible")%></td></tr>
                    <tr><td><asp:CheckBox ID="chkIsBeginGroup" runat="server" /> <%=GetLabel("Is Begin Group")%></td></tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding:5px;vertical-align:top" colspan="3">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:160px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("CRUD Mode")%></label></td>
                        <td valign="top">
                            <table>
                                <colgroup>
                                    <col style="width:100px"/>
                                    <col style="width:100px"/>
                                    <col style="width:100px"/>
                                    <col style="width:150px"/>
                                </colgroup>
                                <tr>
                                    <td><asp:CheckBox ID="chkCreate" runat="server" style="text-align: left" /> <%=GetLabel("Create")%></td>
                                    <td><asp:CheckBox ID="chkRead" runat="server" style="text-align: left" /> <%=GetLabel("Read")%></td>
                                    <td><asp:CheckBox ID="chkUpdate" runat="server" style="text-align: left" /> <%=GetLabel("Update")%></td>
                                    <td><asp:CheckBox ID="chkDelete" runat="server" style="text-align: left" /> <%=GetLabel("Delete")%></td>
                                </tr>
                                <tr>
                                    <td><asp:CheckBox ID="chkPrint" runat="server" style="text-align: left" /> <%=GetLabel("Print")%></td>
                                    <td><asp:CheckBox ID="chkApprove" runat="server" style="text-align: left" /> <%=GetLabel("Approve")%></td>
                                    <td><asp:CheckBox ID="chkVoid" runat="server" style="text-align: left" /> <%=GetLabel("Void")%></td>
                                    <td><asp:CheckBox ID="chkVoidWithReason" runat="server" style="text-align: left" /> <%=GetLabel("Void With Reason")%></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
