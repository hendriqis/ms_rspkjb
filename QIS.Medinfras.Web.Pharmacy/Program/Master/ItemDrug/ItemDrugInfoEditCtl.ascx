<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemDrugInfoEditCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.ItemDrugInfoEditCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_itemdruginfoctl">

    $('#<%=chkIsHasRestrictionInformation.ClientID %>').live('change', function () {
        if ($(this).is(":checked")) {
            $('#<%=trRestrictionInformation.ClientID %>').removeAttr('style');
        }
        else {
            $('#<%=trRestrictionInformation.ClientID %>').attr('style', 'display:none');
        }
    });

    //#region MIMS Class
    $('#lblMIMSClass.lblLink').click(function () {
        openSearchDialog('mimsclass', 'IsDeleted = 0', function (value) {
            $('#<%=txtMIMSClassCode.ClientID %>').val(value);
            onTxtMIMSClassCodeChanged(value);
        });
    });

    $('#<%=txtMIMSClassCode.ClientID %>').change(function () {
        onTxtMIMSClassCodeChanged($(this).val());
    });

    function onTxtMIMSClassCodeChanged(value) {
        var filterExpression = "MIMSClassCode = '" + value + "'";
        Methods.getObject('GetMIMSClassList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnMIMSClassID.ClientID %>').val(result.MIMSClassID);
                $('#<%=txtMIMSClassName.ClientID %>').val(result.MIMSClassName);
            }
            else {
                $('#<%=hdnMIMSClassID.ClientID %>').val('');
                $('#<%=txtMIMSClassCode.ClientID %>').val('');
                $('#<%=txtMIMSClassName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region ATC Class
    $('#lblATCClass.lblLink').click(function () {
        openSearchDialog('atcclass', 'IsDeleted = 0', function (value) {
            $('#<%=txtATCClassCode.ClientID %>').val(value);
            onTxtATCClassCodeChanged(value);
        });
    });

    $('#<%=txtATCClassCode.ClientID %>').change(function () {
        onTxtATCClassCodeChanged($(this).val());
    });

    function onTxtATCClassCodeChanged(value) {
        var filterExpression = "ATCClassCode = '" + value + "'";
        Methods.getObject('GetATCClassificationList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnATCClassID.ClientID %>').val(result.ATCClassID);
                $('#<%=txtATCClassName.ClientID %>').val(result.ATCClassName);
            }
            else {
                $('#<%=hdnATCClassID.ClientID %>').val('');
                $('#<%=txtATCClassCode.ClientID %>').val('');
                $('#<%=txtATCClassName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Default Signa
    $('#lblSigna.lblLink').click(function () {
        openSearchDialog('signa', 'IsDeleted = 0', function (value) {
            $('#<%=txtSignaLabel.ClientID %>').val(value);
            txtSignaLabelChanged(value);
        });
    });

    $('#<%=txtSignaLabel.ClientID %>').change(function () {
        txtSignaLabelChanged($(this).val());
    });

    function txtSignaLabelChanged(value) {
        var filterExpression = "IsDeleted = 0 AND SignaLabel = '" + value + "'";
        Methods.getObject('GetvSignaList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnDefaultSignaID.ClientID %>').val(result.SignaID);
                $('#<%=txtSignaName1.ClientID %>').val(result.SignaName1);
            }
            else {
                $('#<%=hdnDefaultSignaID.ClientID %>').val('');
                $('#<%=txtSignaLabel.ClientID %>').val('');
                $('#<%=txtSignaName1.ClientID %>').val('');
            }
        });
    }
    //#endregion
</script>
<input type="hidden" id="hdnItemID" runat="server" value="" />
<div style="height: 450px; overflow-y: auto">
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <fieldset id="fsEntryPopup" style="margin: 0">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Generik")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtGenericName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kadar")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col style="width: 30%" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDose" CssClass="number required" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboDose" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Bentuk/Kemasan")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 130px" />
                                        <col style="width: 3px" />
                                        <col style="width: 130px" />
                                        <col style="width: 3px" />
                                        <col style="width: 130px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboDrugForm" runat="server" Width="100%" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblMIMSClass">
                                    <%=GetLabel("Klasifikasi MIMS")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnMIMSClassID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtMIMSClassCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMIMSClassName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblATCClass">
                                    <%=GetLabel("Klasifikasi ATC")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnATCClassID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtATCClassCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtATCClassName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Klasifikasi Obat")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboDrugClassification" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kategori Pasien Hamil")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPregnancyCategory" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Coenam Rule")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboCoenamRule" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Medication Routes")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboMedicationRoute" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblLink" id="lblSigna">
                                    <%=GetLabel("Default Signa")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnDefaultSignaID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 31%" />
                                        <col style="width: 62%" />
                                        <col style="width: 10%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSignaLabel" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSignaName1" Width="100%" runat="server" ReadOnly="true" TabIndex="999" />
                                        </td>
                                        <td>
                                            &nbsp;
                                            <img height="20" src='<%= ResolveUrl("~/Libs/Images/Warning.png")%>' alt='' title='Default Signa akan tampil sebagai default dalam pembuatan Transaksi Resep Farmasi' />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("StatusVEN")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboStatusVEN" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Fungsi Obat")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPurposeOfMedication" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Spesial Instruction")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMedicationAdministration" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <colgroup>
                                        <col width="33%" />
                                        <col width="33%" />
                                        <col width="33%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsFormularium" runat="server" /><%=GetLabel("Formularium")%>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsBPJSFormularium" runat="server" /><%=GetLabel("Formularium BPJS")%>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsEmployeeFormularium" runat="server" /><%=GetLabel("Formularium Karyawan")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsGeneric" runat="server" /><%=GetLabel("Obat Generik")%>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsPrecursor" runat="server" /><%=GetLabel("Precursor")%>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkISHAM" runat="server" /><%=GetLabel("High Alert Medication")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsLASA" runat="server" /><%=GetLabel("Look Alike Sound Alike")%>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsExternalMedication" runat="server" /><%=GetLabel("Obat Luar")%>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsChronic" runat="server" /><%=GetLabel("Obat Kronis")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsOOT" runat="server" /><%=GetLabel("Obat-obat Tertentu")%>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsInjection" runat="server" /><%=GetLabel("Obat Injeksi")%>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsHasRestrictionInformation" runat="server" /><%=GetLabel("Restriksi Obat")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsFORNAS" runat="server" /><%=GetLabel("Formularium Nasional")%>
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chkIsControlExpired" Text="Kontrol Expired" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="display: none" id="trRestrictionInformation" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Informasi Restriksi Obat")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRestrictionInformation" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Cara Penyimpanan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtStorageRemarks" Width="100%" runat="server" TextMode="MultiLine"
                                    Rows="4" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
</div>
