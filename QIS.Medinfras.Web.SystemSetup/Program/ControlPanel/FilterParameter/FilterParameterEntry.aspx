<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="FilterParameterEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.FilterParameterEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
    
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onCboFilterParameterTypeValueChanged(val) {
            if (val == Constant.FilterParameterType.SEARCH_DIALOG) {
                $('#<%=trSearchDialog.ClientID %>').removeAttr('style');
                $('#<%=trFieldName.ClientID %>').removeAttr('style');
                $('#<%=trComboBox.ClientID %>').attr('style', 'display:none');
                $('#<%=trSelectAll.ClientID %>').removeAttr('style');
                $('#<%=trCustomComboBox.ClientID %>').attr('style', 'display:none');
                $('#<%=trYearComboBox.ClientID %>').attr('style', 'display:none');
            }
            else if (val == Constant.FilterParameterType.CUSTOM_COMBO_BOX) {
                $('#<%=trCustomComboBox.ClientID %>').removeAttr('style');
                $('#<%=trYearComboBox.ClientID %>').attr('style', 'display:none');
                $('#<%=trComboBox.ClientID %>').attr('style', 'display:none');
                $('#<%=trSearchDialog.ClientID %>').attr('style', 'display:none');
                $('#<%=trFieldName.ClientID %>').removeAttr('style');
                $('#<%=txtCssClass.ClientID %>').removeAttr('style');         
            }
            else if (val == Constant.FilterParameterType.YEAR_COMBO_BOX) {
                $('#<%=trYearComboBox.ClientID %>').removeAttr('style');
                $('#<%=trCustomComboBox.ClientID %>').attr('style', 'display:none');
                $('#<%=trSearchDialog.ClientID %>').attr('style', 'display:none');
                $('#<%=trComboBox.ClientID %>').attr('style', 'display:none');
                $('#<%=trFieldName.ClientID %>').removeAttr('style');
                $('#<%=txtCssClass.ClientID %>').removeAttr('style');          
            }
            else {
                $('#<%=trCustomComboBox.ClientID %>').attr('style', 'display:none');
                $('#<%=trSearchDialog.ClientID %>').attr('style', 'display:none');
                $('#<%=trYearComboBox.ClientID %>').attr('style', 'display:none');
                if (val == Constant.FilterParameterType.COMBO_BOX || val == Constant.FilterParameterType.CHECK_LIST)
                    $('#<%=trComboBox.ClientID %>').removeAttr('style');
                else
                    $('#<%=trComboBox.ClientID %>').attr('style', 'display:none');

                if (val == Constant.FilterParameterType.TEXT_BOX)
                    $('#<%=trTextBox.ClientID %>').removeAttr('style');
                else
                    $('#<%=trTextBox.ClientID %>').attr('style', 'display:none');

                if (val == Constant.FilterParameterType.COMBO_BOX)
                    $('#<%=trSelectAll.ClientID %>').removeAttr('style');
                else
                    $('#<%=trSelectAll.ClientID %>').attr('style', 'display:none');

                if (val != Constant.FilterParameterType.FREE_TEXT)
                    $('#<%=trFieldName.ClientID %>').removeAttr('style');
                else
                    $('#<%=trFieldName.ClientID %>').attr('style', 'display:none');
            }
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Filter Parameter")%></div>--%>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:20%"/>
                        <col style="width:65%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Filter Parameter Code")%></label></td>
                        <td><asp:TextBox ID="txtFilterParameterCode" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Filter Parameter Name")%></label></td>
                        <td><asp:TextBox ID="txtFilterParameterName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Filter Parameter Caption")%></label></td>
                        <td><asp:TextBox ID="txtFilterParameterCaption" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Filter Parameter Type")%></label></td>
                        <td>
                            <dxe:ASPxComboBox ID="cboFilterParameterType" Width="300px" runat="server">
                                <ClientSideEvents Init="function(s,e){ onCboFilterParameterTypeValueChanged(s.GetValue()); }"
                                    ValueChanged="function(s,e){ onCboFilterParameterTypeValueChanged(s.GetValue()); }" />
                            </dxe:ASPxComboBox>                        
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr id="trFieldName" runat="server" style="display:none" class="trType">
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:20%"/>
                        <col style="width:65%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Field Name")%></label></td>
                        <td><asp:TextBox ID="txtFieldName" Width="300px" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr id="trSelectAll" runat="server" style="display:none" class="trType">
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:20%"/>
                        <col style="width:65%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Allow Select All")%></label></td>
                        <td><asp:CheckBox ID="chkIsAllowSelectAll" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        
        <tr id="trSearchDialog" runat="server" style="display:none" class="trType">
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:20%"/>
                        <col style="width:65%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Search Dialog Type")%></label></td>
                        <td><asp:TextBox ID="txtSearchDialogType" runat="server" Width="300px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Method Name")%></label></td>
                        <td><asp:TextBox ID="txtSearchDialogMethodName" runat="server" Width="300px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Filter Expression")%></label></td>
                        <td><asp:TextBox ID="txtSearchDialogFilterExpression" runat="server" Width="300px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("ID Field")%></label></td>
                        <td><asp:TextBox ID="txtSearchDialogIDField" runat="server" Width="300px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Code Field")%></label></td>
                        <td><asp:TextBox ID="txtSearchDialogCodeField" runat="server" Width="300px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Name Field")%></label></td>
                        <td><asp:TextBox ID="txtSearchDialogNameField" runat="server" Width="300px" /></td>
                    </tr>
                </table>
            </td>
        </tr>
            
        <tr id="trComboBox" runat="server" style="display:none" class="trType">
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:20%"/>
                        <col style="width:65%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Method Name")%></label></td>
                        <td><asp:TextBox ID="txtMethodName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Filter Expression")%></label></td>
                        <td><asp:TextBox ID="txtFilterExpression" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Value Field Name")%></label></td>
                        <td><asp:TextBox ID="txtValueFieldName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Text Field Name")%></label></td>
                        <td><asp:TextBox ID="txtTextFieldName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Client Instance Name")%></label></td>
                        <td><asp:TextBox ID="txtClientInstanceName" Width="300px" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr id="trCustomComboBox" runat="server" style="display:none" class="trType">
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:20%"/>
                        <col style="width:65%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("List Text ( | separated)")%></label></td>
                        <td><asp:TextBox ID="txtListText" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("List Value ( | separated)")%></label></td>
                        <td><asp:TextBox ID="txtListValue" Width="300px" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr id="trYearComboBox" runat="server" style="display:none" class="trType">
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:20%"/>
                        <col style="width:65%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("- n Year(s)")%></label></td>
                        <td><asp:TextBox ID="txtMinusNYear" CssClass="number" Width="120px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("+ n Year(s)")%></label></td>
                        <td><asp:TextBox ID="txtPlusNYear" CssClass="number" Width="120px" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr id="trTextBox" runat="server" style="display:none" class="trType">
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:35%"/>
                        <col style="width:65%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Css Class")%></label></td>
                        <td><asp:TextBox ID="txtCssClass" Width="300px" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr id="trDefaultValue" runat="server" class="trType">
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:20%"/>
                        <col style="width:65%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Default Value")%></label></td>
                        <td><asp:TextBox ID="txtDefaultValue" Width="300px" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
