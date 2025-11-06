<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="ClassEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ClassEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('#<%=chkIsBPJSClass.ClientID %>').change(function () {
                if ($(this).is(":checked")) {
                    $('#<%=trBPJSClassCodeName.ClientID %>').css('display', '');
                    $('#<%=trBPJSClassType.ClientID %>').css('display', '');
                    $('#<%=trBPJSKelasNaik.ClientID %>').css('display', '');
                }
                else {
                    $('#<%=trBPJSClassCodeName.ClientID %>').css('display', 'none');
                    $('#<%=trBPJSClassType.ClientID %>').css('display', 'none');
                    $('#<%=trBPJSKelasNaik.ClientID %>').css('display', 'none');
                    $('#<%=txtBPJSCode.ClientID %>').val('');
                    $('#<%=txtBPJSName.ClientID %>').val('');
                }
            });

            $('#lblVClaim.lblLink').click(function () {
                openSearchDialog('vclaimKelasPerawatan', '', function (value) {
                    $('#<%=txtBPJSCode.ClientID %>').val(value);
                    ontxtBPJSCodeChanged(value);
                });
            });

            $('#<%=txtBPJSCode.ClientID %>').change(function () {
                ontxtBPJSCodeChanged($(this).val());
            });

            function ontxtBPJSCodeChanged(value) {
                var filterExpression = "BPJSCode = '" + value + "'";
                Methods.getObject('GetvBPJSReferenceClassCareList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtBPJSName.ClientID %>').val(result.BPJSName);
                    else {
                        $('#<%=txtBPJSCode.ClientID %>').val('');
                        $('#<%=txtBPJSName.ClientID %>').val('');
                    }
                });
            }
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 55%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 50%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Kelas")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtClassCode" Width="120px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Kelas")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtClassName" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Singkat")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtShortName" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kelas RL Standar")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboGCClassRL" runat="server" Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kelas Priority")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtClassPriority" CssClass="number" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kenaikan Harga Pelayanan (%)")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMarginPercentage1" Width="120px" CssClass="number" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kenaikan Harga Peralatan dan Bahan Medis (%)")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMarginPercentage2" Width="120px" CssClass="number" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kenaikan Harga Barang Umum (%)")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMarginPercentage3" Width="120px" CssClass="number" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nilai Minimum Deposit")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDepositAmount" Width="120px" CssClass="number" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kelas Rawat Inap")%></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsInPatientClass" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kelas Tagihan")%></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsUsedInChargeClass" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kelas BPJS")%></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsBPJSClass" runat="server" />
                        </td>
                    </tr>
                    <tr style="display: none" id="trBPJSClassCodeName" runat="server">
                        <td class="tdLabel">
                            <label class="lblLink" id="lblVClaim">
                                <%=GetLabel("Referensi VClaim")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBPJSCode" Width="150px" runat="server" />
                            <asp:TextBox ID="txtBPJSName" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr style="display: none" id="trBPJSClassType" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal" id="Label1">
                                <%=GetLabel("Tipe Kelas BPJS")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboBPJSClassType" Width="120px" ClientInstanceName="cboYear"
                                runat="server" />
                        </td>
                    </tr>
                    <tr style="display: none" id="trBPJSKelasNaik" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal" id="Label2">
                                <%=GetLabel("Kelas Naik BPJS")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboBPJSKelasNaik" Width="120px" ClientInstanceName="cboYear" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kelas INACBGs")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboINACBGClass" runat="server" Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kode Kelas Aplicares")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKodeKelasApplicares" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Kelas Aplicares")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNamaKelasApplicares" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kode Kelas Inhealth")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInhealthClassCode" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Kelas Inhealth")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInhealthClassName" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsControlUnitPrice" runat="server" />
                            <%=GetLabel("Kontrol Harga")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Harga Pelayanan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtServiceUnitPrice" CssClass="txtCurrency" Width="200px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Harga Obat/Alkes")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDrugSuppliesUnitPrice" CssClass="txtCurrency" Width="200px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Harga Barang Umum")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLogisticUnitPrice" CssClass="txtCurrency" Width="200px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Pengali INA Ditempati")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPengaliINADitempati" CssClass="txtCurrency" Width="200px" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <table class="grdNormal" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 80px" />
                        <col style="width: 15px" />
                        <col style="width: 90px" />
                        <col style="width: 90px" />
                        <col style="width: 90px" />
                    </colgroup>
                    <tr>
                        <th rowspan="2" colspan="2" align="left">
                            <%=GetLabel("JENIS BIAYA")%>
                        </th>
                        <th colspan="2">
                            <%=GetLabel("NILAI")%>
                        </th>
                        <th colspan="2">
                            <%=GetLabel("JUMLAH")%>
                        </th>
                    </tr>
                    <tr>
                        <th>
                            <%=GetLabel("%")%>
                        </th>
                        <th>
                            <%=GetLabel("Biaya")%>
                        </th>
                        <th align="right">
                            <%=GetLabel("Minimum")%>
                        </th>
                        <th align="right">
                            <%=GetLabel("Maximum")%>
                        </th>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            <%=GetLabel("Administrasi")%>
                        </td>
                        <td>
                            <%=GetLabel("Instansi")%>
                        </td>
                        <td align="center">
                            <asp:CheckBox ID="chkAdministrationPercentage" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtAdministrationAmount" CssClass="txtCurrency" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtMinAdministrationAmount" CssClass="txtCurrency" Width="100%"
                                runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtMaxAdministrationAmount" CssClass="txtCurrency" Width="100%"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=GetLabel("Pasien")%>
                        </td>
                        <td align="center">
                            <asp:CheckBox ID="chkPatientAdminPercentage" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatientAdmAmount" CssClass="txtCurrency" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtMinPatientAdmAmount" CssClass="txtCurrency" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtMaxPatientAdmAmount" CssClass="txtCurrency" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            <%=GetLabel("Service")%>
                        </td>
                        <td>
                            <%=GetLabel("Instansi")%>
                        </td>
                        <td align="center">
                            <asp:CheckBox ID="chkServiceChargePercentage" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtServiceChargeAmount" CssClass="txtCurrency" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtMinServiceChargeAmount" CssClass="txtCurrency" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtMaxServiceChargeAmount" CssClass="txtCurrency" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=GetLabel("Pasien")%>
                        </td>
                        <td align="center">
                            <asp:CheckBox ID="chkPatientServicePercentage" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatientServiceAmount" CssClass="txtCurrency" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtMinPatientServiceAmount" CssClass="txtCurrency" Width="100%"
                                runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtMaxPatientServiceAmount" CssClass="txtCurrency" Width="100%"
                                runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
