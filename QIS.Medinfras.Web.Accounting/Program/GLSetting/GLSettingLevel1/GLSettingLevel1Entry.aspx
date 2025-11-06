<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="GLSettingLevel1Entry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.GLSettingLevel1Entry" %>
    
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region Parameter
            $('#lblSDParameter.lblLink').click(function () {
                var filterExpression = "<%:SearchDialogFilterExpression %>";
                openSearchDialog('<%=SearchDialogType %>', filterExpression, function (value) {
                    $('#<%=txtSDParameterCode.ClientID %>').val(value);
                    onTxtParameterCodeChanged(value);
                });
            });

            $('#<%=txtSDParameterCode.ClientID %>').change(function () {
                onTxtParameterCodeChanged($(this).val());
            });

            function onTxtParameterCodeChanged(value) {
                var sdFilterExpression = "<%:SearchDialogFilterExpression %>";
                var filterExpression = "<%:SearchDialogCodeField %> = '" + value + "'";
                if (sdFilterExpression != '')
                    filterExpression += " AND " + filterExpression;
                Methods.getObject('<%=SearchDialogMethodName %>', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSDParameterID.ClientID %>').val(result['<%=SearchDialogIDField %>']);
                        $('#<%=txtSDParameterName.ClientID %>').val(result['<%=SearchDialogNameField %>']);
                    }
                    else {
                        $('#<%=hdnSDParameterID.ClientID %>').val('');
                        $('#<%=txtSDParameterCode.ClientID %>').val('');
                        $('#<%=txtSDParameterName.ClientID %>').val('');
                    }
                });
            }

            cbmParameterValue.SetText($('#<%=hdnCbmParameterText.ClientID %>').val());
            //#endregion
        }

        //#region Multi Check Combo Box
        var textSeparator = "|";
        function updateText() {
            var selectedItems = cbmParameterValueListItem.GetSelectedItems();
            cbmParameterValue.SetText(getSelectedItemsText(selectedItems));
        }
        function synchronizeListBoxValues(dropDown, args) {
            var paramValue = $('#<%=hdnCbmParameterText.ClientID %>').val();
            cbmParameterValueListItem.UnselectAll();
            if (paramValue != "") {
                dropDown.SetText(paramValue);
            }
            var texts = dropDown.GetText().split(textSeparator);
            var values = getValuesByTexts(texts);
            cbmParameterValueListItem.SelectValues(values);
            updateText(); // for remove non-existing texts
        }
        function syncNewText() {
            var texts = cbmParameterValue.GetText().split(textSeparator);
            var values = getValuesByTexts(texts);
            cbmParameterValueListItem.SelectValues(values);
            cbmParameterValue.HideDropDown();
        }
        function getSelectedItemsText(items) {
            var texts = [];
            for (var i = 0; i < items.length; i++)
                texts.push(items[i].text);
            return texts.join(textSeparator);
        }
        function getValuesByTexts(texts) {
            var actualValues = [];
            var item;
            for (var i = 0; i < texts.length; i++) {
                item = cbmParameterValueListItem.FindItemByText(texts[i]);
                if (item != null) {
                    actualValues.push(item.value);
                }
            }
            $('#<%=hdnCbmParameterValue.ClientID %>').val(actualValues);
            return actualValues;
        }
        //#endregion
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnCbmParameterText" runat="server" value="" />
    <input type="hidden" id="hdnCbmParameterValue" runat="server" value="" />
    <input type="hidden" id="hdnListText" runat="server" value="" />
    <input type="hidden" id="hdnListValue" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent">
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Parameter Code")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtParameterCode" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Parameter Name")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtParameterName" Width="600px" runat="server" />
                        </td>
                    </tr>
                    <tr class="tdLabel" style="display: none" id="trCboParameterValue" runat="server">
                        <td class="tdLabel">
                            <label class="lblMandatory" id="lblParameterValue">
                                <%=GetLabel("Parameter Value")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboParameterValue" runat="server" Width="600px" />
                        </td>
                    </tr>
                    <tr style="display: none" id="trCbmParameterValue" runat="server">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%:GetLabel("Parameter Value")%></label>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col style="width: 3px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <dxe:ASPxDropDownEdit ClientInstanceName="cbmParameterValue" ID="cbmParameterValue"
                                            Width="285px" runat="server" AnimationType="None">
                                            <DropDownWindowStyle BackColor="#EDEDED" />
                                            <DropDownWindowTemplate>
                                                <dxe:ASPxListBox Width="100%" ID="cbmParameterValueListItem" ClientInstanceName="cbmParameterValueListItem"
                                                    SelectionMode="CheckColumn" runat="server" Height="200" EnableSelectAll="true">
                                                    <Border BorderStyle="None" />
                                                    <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
                                                    <ClientSideEvents SelectedIndexChanged="updateText" Init="updateText" />
                                                </dxe:ASPxListBox>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="padding: 4px">
                                                            <dxe:ASPxButton ID="ASPxButton1" AutoPostBack="False" runat="server" Text="Simpan Pilihan"
                                                                Style="float: right">
                                                                <ClientSideEvents Click="function(s, e){ syncNewText(); cbmParameterValue.HideDropDown(); }" />
                                                            </dxe:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </DropDownWindowTemplate>
                                            <ClientSideEvents TextChanged="synchronizeListBoxValues" DropDown="synchronizeListBoxValues"
                                                CloseUp="function(s, e) {s.ShowDropDown(); }" />
                                        </dxe:ASPxDropDownEdit>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trTxtParameterValue" runat="server">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Parameter Value")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtParameterValue" Width="600px" runat="server" />
                        </td>
                    </tr>
                    <tr id="trSDParameterValue" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblSDParameter">
                                <%=GetLabel("Parameter Value")%></label>
                        </td>
                        <td colspan="2">
                            <input type="hidden" id="hdnSDParameterID" runat="server" value="" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtSDParameterCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSDParameterName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px">
                            <label>
                                <%=GetLabel("Notes")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNotes" Width="600px" runat="server" TextMode="MultiLine" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
