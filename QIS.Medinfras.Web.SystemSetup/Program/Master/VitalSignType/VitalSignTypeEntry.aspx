<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="VitalSignTypeEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.VitalSignTypeEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('#<%=txtDisplayColorPicker.ClientID %>').colorPicker();
            $('#<%=txtDisplayColorPicker.ClientID %>').change(function () {
                $('#<%=txtDisplayColor.ClientID %>').val($(this).val());
            });
            $('#<%=txtDisplayColor.ClientID %>').change(function () {
                $('#<%=txtDisplayColorPicker.ClientID %>').val($(this).val());
                $('#<%=txtDisplayColorPicker.ClientID %>').change();
            });

            //#region Value Code
            $('#lblValueCode.lblLink').click(function () {
                openSearchDialog('standardcodeheader', '', function (value) {
                    $('#<%=txtValueCodeCode.ClientID %>').val(value);
                    onTxtValueCodeCodeChanged(value);
                });
            });

            $('#<%=txtValueCodeCode.ClientID %>').change(function () {
                onTxtValueCodeCodeChanged($(this).val());
            });

            function onTxtValueCodeCodeChanged(value) {
                var filterExpression = "StandardCodeID = '" + value + "'";
                Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtValueCodeName.ClientID %>').val(result.StandardCodeName);
                    else {
                        $('#<%=txtValueCodeCode.ClientID %>').val('');
                        $('#<%=txtValueCodeName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }

        function onCboValueTypeValueChanged(s) {
            if (s.GetValue() != 'X103^002') {
                $('#lblValueCode').attr('class', 'lblDisabled');
                $('#<%=txtValueCodeCode.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtValueCodeCode.ClientID %>').val('');
                $('#<%=txtValueCodeName.ClientID %>').val('');
            }
            else {
                $('#lblValueCode').attr('class', 'lblLink');
                $('#<%=txtValueCodeCode.ClientID %>').removeAttr('readonly');
            }
        }
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Location")%></div>
    <table class="tblContentArea" style="width:100%">
        <colgroup>
            <col style="width:35%"/>
            <col style="width:35%"/>
            <col style="width:30%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:30%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Vital Sign Code")%></label></td>
                        <td><asp:TextBox ID="txtVitalSignCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Vital Sign Name")%></label></td>
                        <td><asp:TextBox ID="txtVitalSignName" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Short Name")%></label></td>
                        <td><asp:TextBox ID="txtShortName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Initial")%></label></td>
                        <td><asp:TextBox ID="txtVitalSignLabel" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Value Type")%></label></td>
                        <td>
                            <dxe:ASPxComboBox ID="cboValueType" Width="100%" runat="server">
                                <ClientSideEvents Init="function(s,e){ onCboValueTypeValueChanged(s); }" 
                                    ValueChanged="function(s,e) { onCboValueTypeValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblValueCode"><%=GetLabel("Value Code")%></label></td>
                        <td>
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtValueCodeCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtValueCodeName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Value Unit")%></label></td>
                        <td><asp:TextBox ID="txtValueUnit" Width="300px" runat="server" /></td>
                    </tr>
                </table>
            </td>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:30%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Display Color")%></label></td>
                        <td>
                            <table cellpadding="0" cellspacing="0"> 
                                <tr>
                                    <td><asp:TextBox ID="txtDisplayColor" CssClass="colorpicker" Width="100px" runat="server" /></td>
                                    <td style="padding-left:5px"><asp:TextBox ID="txtDisplayColorPicker" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Vital Sign Order")%></label></td>
                        <td><asp:TextBox ID="txtVitalSignOrder" CssClass="number" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Value Mask")%></label></td>
                        <td><asp:TextBox ID="txtValueMask" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Chart Min Value")%></label></td>
                        <td><asp:TextBox ID="txtChartMinValue" CssClass="number" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Chart Max Value")%></label></td>
                        <td><asp:TextBox ID="txtChartMaxValue" CssClass="number" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Min Normal Value")%></label></td>
                        <td><asp:TextBox ID="txtMinNormalValue" CssClass="number" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Max Normal Value")%></label></td>
                        <td><asp:TextBox ID="txtMaxNormalValue" CssClass="number" Width="300px" runat="server" /></td>
                    </tr>
                </table>
            </td>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <tr><td><asp:CheckBox ID="chkIsDisplayInBanner" runat="server" /> <%=GetLabel("Display In Banner")%></td></tr>
                    <tr><td><asp:CheckBox ID="chkIsDisplayInChart" runat="server" /> <%=GetLabel("Display In Chart")%></td></tr>
                    <tr><td><asp:CheckBox ID="chkIsSpecialIndicator" runat="server" /> <%=GetLabel("Special Indicator")%></td></tr>
                    <tr><td><asp:CheckBox ID="chkIsNumericValue" runat="server" /> <%=GetLabel("Is Numeric Value")%></td></tr>
                    <tr><td><asp:CheckBox ID="chkIsUsedBySystem" runat="server" /> <%=GetLabel("Used By System")%></td></tr>
                    <tr><td><asp:CheckBox ID="chkIsAutoGenerated" runat="server" /> <%=GetLabel("Auto Generated")%></td></tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
