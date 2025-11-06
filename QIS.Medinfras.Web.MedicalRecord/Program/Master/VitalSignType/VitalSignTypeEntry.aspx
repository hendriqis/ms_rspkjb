<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="VitalSignTypeEntry.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.VitalSignTypeEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() 
        {
            $('#<%=txtDisplayColorPicker.ClientID %>').colorPicker();
            $('#<%=txtDisplayColorPicker.ClientID %>').change(function () {
                $('#<%=txtDisplayColor.ClientID %>').val($(this).val());
            });

            $('#<%=txtDisplayColor.ClientID %>').change(function () {
                $('#<%=txtDisplayColorPicker.ClientID %>').val($(this).val());
                $('#<%=txtDisplayColorPicker.ClientID %>').change();
            });

            $('#lblValueCode.lblLink').click(function () {
                openSearchDialog('standardcodeheader', '', function (value) {
                    $('#<%=txtValueCode.ClientID %>').val(value);
                    onTxtValueCodeChanged(value);
                });
            });

            $('#<%=txtValueCode.ClientID %>').change(function () {
                onTxtValueCodeChanged($(this).val());
            });

            function onTxtValueCodeChanged(value) {
                var filterExpression = "StandardCodeID = '" + value + "'";
                Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtValueCodeName.ClientID %>').val(result.StandardCodeName);
                    else {
                        $('#<%=txtValueCode.ClientID %>').val('');
                        $('#<%=txtValueCodeName.ClientID %>').val('');
                    }
                });
            }
        }

        function onCboValueTypeValueChanged(s) {
            if (s.GetValue() != 'X103^002') {
                $('#lblValueCode').attr('class', 'lblDisabled');
                $('#<%=txtValueCode.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtValueCode.ClientID %>').val('');
                $('#<%=txtValueCodeName.ClientID %>').val('');
            }
            else {
                $('#lblValueCode').attr('class', 'lblLink');
                $('#<%=txtValueCode.ClientID %>').removeAttr('readonly');
            }
        }
    </script>
    
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Vital Sign Entry")%></div>--%>
    <table class="tblContentArea" style="width:100%">
        <colgroup>
            <col style="width:40%"/>
            <col style="width:40%"/>
            <col style="width:20%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:30%"/>
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Tanda Vital")%></label></td>
                        <td><asp:TextBox ID="txtVitalSignCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Tanda Vital")%></label></td>
                        <td><asp:TextBox ID="txtVitalSignName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Singkat")%></label></td>
                        <td><asp:TextBox ID="txtShortName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Inisial")%></label></td>
                        <td><asp:TextBox ID="txtInitial" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("FHIR Reference")%></label></td>
                        <td>
                            <table border="0" cellpadding="1" cellspacing="0">
                                <colgroup>
                                    <col style="width:120px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtFHIRReferenceCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFHIRReferenceName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tipe Nilai")%></label></td>
                        <td>
                            <dxe:ASPxComboBox ID="cboValueType" Width="300px" runat="server">
                                <ClientSideEvents Init="function(s,e){ onCboValueTypeValueChanged(s); }" 
                                    ValueChanged="function(s,e) { onCboValueTypeValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>                            
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblValueCode"><%=GetLabel("Kode Nilai")%></label></td>
                        <td>
                            <input type="hidden" value="" runat="server" id="hdnValueCodeID" />
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:100px"/>
                                    <col style="width:3px"/>
                                    <col />
                                <col/>
                                </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtValueCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtValueCodeName" Width="100%" runat="server" /></td>
                                    </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Unit Nilai")%></label></td>
                        <td><asp:TextBox ID="txtValueUnit" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Unit FHIR")%></label></td>
                        <td><asp:TextBox ID="txtFHIRReferenceUnit" Width="100px" runat="server" /></td>
                    </tr>
                </table>
            </td>
            <td>
                <table>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Warna")%></label></td>
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
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Urutan Tanda Vital")%></label></td>
                        <td><asp:TextBox ID="txtVitalSignOrder" CssClass="number" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Format Nilai")%></label></td>
                        <td><asp:TextBox ID="txtValueMask" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nilai Grafik Minimal")%></label></td>
                        <td><asp:TextBox ID="txtChartMinValue" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nilai Grafik Maximal")%></label></td>
                        <td><asp:TextBox ID="txtChartMaxValue" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nilai Normal Minimal")%></label></td>
                        <td><asp:TextBox ID="txtMinNormalValue" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nilai Normal Maximal")%></label></td>
                        <td><asp:TextBox ID="txtMaxNormalValue" Width="300px" runat="server" /></td>
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

