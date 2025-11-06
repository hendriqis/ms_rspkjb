<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ObstetricHistoryEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ObstetricHistoryEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_obstetricHistoryEntryctl">
    $('#<%=txtPregnancyNo.ClientID %>').focus();

    if ($('#<%:chkIsOtherComplication.ClientID %>').is(':checked')) {
        $('#<%:trOtherComplication.ClientID %>').removeAttr('style');
    }
    else {
        $('#<%:trOtherComplication.ClientID %>').attr('style', 'display:none');
    }

    $('#<%:chkIsOtherComplication.ClientID %>').live('change', function () {
        $chkIsOtherComplication = $('#<%:chkIsOtherComplication.ClientID %>');
        if ($(this).is(':checked')) {
            $('#<%:trOtherComplication.ClientID %>').removeAttr('style');
        }
        else {
            $('#<%:trOtherComplication.ClientID %>').attr('style', 'display:none');

        }
    });
</script>
<div style="height: 450px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnIsNutrition" value="" />
    <table>
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td>
                <table style="width: 100%" id="tblEntryContent">
                    <colgroup>
                        <col style="width: 170px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kehamilan Ke-")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPregnancyNo" Width="60px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Usia Kehamilan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPregnancyDuration" Width="60px" runat="server" /><%=GetLabel(" minggu")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Jenis Persalinan")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboBirthMethod" ClientInstanceName="cboBirthMethod"
                                Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Jenis Caesar")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboCaesarMethod" ClientInstanceName="cboCaesarMethod"
                                Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kondisi Persalinan")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboBornCondition" ClientInstanceName="cboBornCondition"
                                Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Komplikasi Persalinan")%></label>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="width: 33.33%">
                                        <asp:CheckBox ID="chkIsAntepartumBleeding" runat="server" Text=" Pendarahan sebelum"
                                            Checked="false" />
                                    </td>
                                    <td style="width: 33.33%">
                                        <asp:CheckBox ID="chkIsPostpartumBleeding" runat="server" Text=" Pendarahan sesudah"
                                            Checked="false" />
                                    </td>
                                    <td style="width: 33.33%">
                                        <asp:CheckBox ID="chkIsPreclampsia" runat="server" Text=" Pre Eclampsia" Checked="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="width: 33.33%">
                                        <asp:CheckBox ID="chkIsEclampsia" runat="server" Text=" Eclampsia" Checked="false" />
                                    </td>
                                    <td style="width: 33.33%">
                                        <asp:CheckBox ID="chkIsHasInfectious" runat="server" Text=" Infeksi" Checked="false" />
                                    </td>
                                    <td style="width: 33.33%">
                                        <asp:CheckBox ID="chkIsOtherComplication" runat="server" Text=" Lain-lain" Checked="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trOtherComplication" style="display: none" runat="server">
                        <td>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtOtherComplicationRemarks" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Komplikasi Non Obstetri")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboNonObstetri" ClientInstanceName="cboNonObstetri" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Penolong Persalinan")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboParamedicType" ClientInstanceName="cboParamedicType"
                                Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Jenis Kelamin")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboSex" ClientInstanceName="cboSex" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Berat Badan Lahir (BBL)")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtWeight" Width="80px" runat="server" /><%=GetLabel(" gram")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Panjang Badan Lahir (PBL)")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLength" Width="80px" runat="server" /><%=GetLabel(" cm")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan Tambahan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="3">
                            <asp:CheckBox ID="chkIsIbuNifas" runat="server" Text="Ibu Nifas mendapat vitamin A"
                                Checked="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
