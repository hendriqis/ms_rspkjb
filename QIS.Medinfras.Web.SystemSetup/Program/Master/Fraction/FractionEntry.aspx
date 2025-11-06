<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="FractionEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.FractionEntry" %>

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

            //#region Parent
            $('#lblParentID.lblLink').click(function () {
                openSearchDialog('fraction', 'IsHeader = 1 AND IsDeleted = 0', function (value) {
                    $('#<%=txtParentCode.ClientID %>').val(value);
                    onTxtParentCodeChanged(value);
                });
            });

            $('#<%=txtParentCode.ClientID %>').change(function () {
                onTxtParentCodeChanged($(this).val());
            });

            function onTxtParentCodeChanged(value) {
                var filterExpression = "FractionCode = '" + value + "' AND IsHeader = 1 AND IsDeleted = 0";
                Methods.getObject('GetFractionList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnParentID.ClientID %>').val(result.FractionID);
                        $('#<%=txtParentName.ClientID %>').val(result.FractionName1);
                    }
                    else {
                        $('#<%=hdnParentID.ClientID %>').val('');
                        $('#<%=txtParentCode.ClientID %>').val('');
                        $('#<%=txtParentName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Specimen
            $('#lblSpecimen.lblLink').click(function () {
                openSearchDialog('specimen', 'IsDeleted = 0', function (value) {
                    $('#<%=txtSpecimenCode.ClientID %>').val(value);
                    onTxtSpecimenCodeChanged(value);
                });
            });

            $('#<%=txtSpecimenCode.ClientID %>').change(function () {
                onTxtSpecimenCodeChanged($(this).val());
            });

            function onTxtSpecimenCodeChanged(value) {
                var filterExpression = "SpecimenCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetSpecimenList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSpecimenID.ClientID %>').val(result.SpecimenID);
                        $('#<%=txtSpecimenName.ClientID %>').val(result.SpecimenName);
                    }
                    else {
                        $('#<%=hdnSpecimenID.ClientID %>').val('');
                        $('#<%=txtSpecimenCode.ClientID %>').val('');
                        $('#<%=txtSpecimenName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }
    </script>
    <input type="hidden" id="hdnFractionID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Fraction")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:30%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Fraction Code")%></label></td>
                        <td><asp:TextBox ID="txtFractionCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Fraction Name 1")%></label></td>
                        <td><asp:TextBox ID="txtFractionName1" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Fraction Name 2")%></label></td>
                        <td><asp:TextBox ID="txtFractionName2" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblParentID"><%=GetLabel("Parent")%></label></td>
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
                        <td class="tdLabel"><label class="lblLink lblMandatory" id="lblSpecimen"><%=GetLabel("Specimen")%></label></td>
                        <td>
                            <input type="hidden" value="" runat="server" id="hdnSpecimenID" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:30%"/>
                                    <col style="width:3px"/>
                                    <col/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtSpecimenCode" Width="100%" runat="server" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox ID="txtSpecimenName" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Metric Unit")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboGCMetricUnit" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("International Unit")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboGCInternationalUnit" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Lab Test Result Type")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboGCLabTestResultType" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Conversion Factor")%></label></td>
                        <td><asp:TextBox ID="txtConversionFactor" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td><asp:CheckBox ID="chkIsHeader" Width="300px" runat="server" Text="Is Header" /></td>
                    </tr>
                </table>
            </td>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:30%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Display Order")%></label></td>
                        <td><asp:TextBox ID="txtDisplayOrder" CssClass="number" Width="80px" runat="server" /></td>
                    </tr>
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
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Chart Min Value")%></label></td>
                        <td><asp:TextBox ID="txtChartMinValue" CssClass="number" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Chart Max Value")%></label></td>
                        <td><asp:TextBox ID="txtChartMaxValue" CssClass="number" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td><asp:CheckBox ID="chkIsDisplayInChart" Width="300px" runat="server" Text="Is Display In Chart" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
