<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="DataMigrationConfigurationColumnMapEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Tools.DataMigrationConfigurationColumnMapEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onCboInputTypeValueChanged(value) {
            $('.trType:visible').hide();
            switch (value) {
                case "2": $('#<%=trComboBox.ClientID %>').show(); break;
                case "3": $('#<%=trCheckBox.ClientID %>').show(); break;
                case "4": $('#<%=trDateEdit.ClientID %>').show(); break;
                case "5": $('#<%=trSearchDialog.ClientID %>').show(); break;
                case "6": $('#<%=trCode.ClientID %>').show(); break;
            }
        }

        function onLoad() {
            $('#<%=chkIsVisible.ClientID %>').change(function () {
                var isChecked = $(this).is(":checked");
                if (isChecked)
                    $('#<%=trColumnDescription.ClientID %>').show();
                else {
                    $('#<%=trColumnDescription.ClientID %>').hide();
                    cboInputType.SetValue('1');
                    onCboInputTypeValueChanged('1');
                }
            });

            $('#<%=chkIsVisible.ClientID %>').change();
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnHeaderID" runat="server" value="" />
    <input type="hidden" id="hdnGridColumn" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Data Migration Column Map")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:35%"/>
                        <col style="width:65%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Table Name")%></label></td>
                        <td><asp:TextBox ID="txtTableName" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Link Column")%></label></td>
                        <td><asp:TextBox ID="txtLinkColumn" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Column Name")%></label></td>
                        <td><asp:TextBox ID="txtColumnName" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Visible")%></label></td>
                        <td><asp:CheckBox ID="chkIsVisible" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trColumnDescription" runat="server">
            <td>
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:35%"/>
                        <col style="width:65%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Column Caption")%></label></td>
                        <td><asp:TextBox ID="txtColumnCaption" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("From Column")%></label></td>
                        <td><asp:TextBox ID="txtFromColumn" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Default Value")%></label></td>
                        <td><asp:TextBox ID="txtDefaultValue" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Input Type")%></label></td>
                        <td>
                            <dxe:ASPxComboBox ID="cboInputType" Width="100%" ClientInstanceName="cboInputType" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e){
                                    onCboInputTypeValueChanged(s.GetValue());
                                }" />
                                <Items>
                                    <dxe:ListEditItem Text="Text Box" Selected="true" Value="1" />
                                    <dxe:ListEditItem Text="Combo Box" Value="2" />
                                    <dxe:ListEditItem Text="Check Box" Value="3" />
                                    <dxe:ListEditItem Text="Date Edit" Value="4" />
                                    <dxe:ListEditItem Text="Search Dialog" Value="5" />
                                    <dxe:ListEditItem Text="Code" Value="6" />
                                </Items>
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trComboBox" runat="server" style="display:none" class="trType">
            <td>
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:35%"/>
                        <col style="width:65%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Method Name")%></label></td>
                        <td><asp:TextBox ID="txtMethodName" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Value Field")%></label></td>
                        <td><asp:TextBox ID="txtValueField" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Text Field")%></label></td>
                        <td><asp:TextBox ID="txtTextField" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Filter Expression")%></label></td>
                        <td><asp:TextBox ID="txtFilterExpression" runat="server" Width="100%" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trCheckBox" runat="server" style="display:none" class="trType">
            <td>
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:35%"/>
                        <col style="width:65%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Value Checked")%></label></td>
                        <td><asp:TextBox ID="txtValueChecked" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Value Unchecked")%></label></td>
                        <td><asp:TextBox ID="txtValueUnchecked" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Other Value")%></label></td>
                        <td><asp:CheckBox ID="chkOtherValue" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trDateEdit" runat="server" style="display:none" class="trType">
            <td>
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:35%"/>
                        <col style="width:65%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Format Date")%></label></td>
                        <td><asp:TextBox ID="txtFormatDate" runat="server" Width="100%" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trSearchDialog" runat="server" style="display:none" class="trType">
            <td>
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:35%"/>
                        <col style="width:65%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Search Dialog Type")%></label></td>
                        <td><asp:TextBox ID="txtSearchDialogType" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Method Name")%></label></td>
                        <td><asp:TextBox ID="txtSearchDialogMethodName" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Filter Expression")%></label></td>
                        <td><asp:TextBox ID="txtSearchDialogFilterExpression" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("ID Field")%></label></td>
                        <td><asp:TextBox ID="txtSearchDialogIDField" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Code Field")%></label></td>
                        <td><asp:TextBox ID="txtSearchDialogCodeField" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Name Field")%></label></td>
                        <td><asp:TextBox ID="txtSearchDialogNameField" runat="server" Width="100%" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trCode" runat="server" style="display:none" class="trType">
            <td>
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:35%"/>
                        <col style="width:65%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("ID Column")%></label></td>
                        <td><asp:TextBox ID="txtIDColumn" runat="server" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Format Code")%></label></td>
                        <td><asp:TextBox ID="txtFormatCode" runat="server" Width="100%" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
