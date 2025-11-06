<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="CustomerContractEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.CustomerContractEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtStartDate.ClientID %>');
            setDatePicker('<%=txtEndDate.ClientID %>');

            $('#<%=chkIsControlClassCare.ClientID %>').change(function () {
                var isChecked = $(this).is(":checked");
                cboClassCare.SetEnabled(isChecked);
            });

            $('#<%=chkIsControlClassCare.ClientID %>').change();
        }

        function onBeforeGoToListPage(mapForm) {
            mapForm.appendChild(createInputHiddenPost("customerID", $('#<%=hdnCustomerID.ClientID %>').val()));
        }

        $('#<%=txtStartDate.ClientID %>').live('change', function () {
            var start = $('#<%=txtStartDate.ClientID %>').val();
            var end = $('#<%=txtEndDate.ClientID %>').val();

            $('#<%=txtStartDate.ClientID %>').val(validateDateFromTo(start, end));
        });

        $('#<%=txtEndDate.ClientID %>').live('change', function () {
            var start = $('#<%=txtStartDate.ClientID %>').val();
            var end = $('#<%=txtEndDate.ClientID %>').val();

            $('#<%=txtEndDate.ClientID %>').val(validateDateToFrom(start, end));
        });

        $('#<%:txtServiceUnitPrice.ClientID %>').live('change', function () {
            var value = $(this).val();
            $(this).val(checkMinusDecimalOK(value)).trigger('changeValue');
        });

        $('#<%:txtDrugSuppliesUnitPrice.ClientID %>').live('change', function () {
            var value = $(this).val();
            $(this).val(checkMinusDecimalOK(value)).trigger('changeValue');
        });

        $('#<%:txtLogisticUnitPrice.ClientID %>').live('change', function () {
            var value = $(this).val();
            $(this).val(checkMinusDecimalOK(value)).trigger('changeValue');
        });

        $('#<%:chkAdministrationPercentage.ClientID %>').live('change', function () {
            var value = $('#<%:txtAdministrationAmount.ClientID %>').val();
            var token = ",";
            var newToken = "";
            value = value.split(token).join(newToken);
            var isPercentage = $('#<%:chkAdministrationPercentage.ClientID %>').is(':checked');

            if (isPercentage) {
                if (value > 100) {
                    $('#<%:txtAdministrationAmount.ClientID %>').val('0').trigger('changeValue');
                }
            }
        });

        $('#<%:txtAdministrationAmount.ClientID %>').live('change', function () {
            var value = $('#<%:txtAdministrationAmount.ClientID %>').val();
            var isPercentage = $('#<%:chkAdministrationPercentage.ClientID %>').is(':checked');

            if (isNaN(value)) {
                $('#<%:txtAdministrationAmount.ClientID %>').val('0').trigger('changeValue');
            }
            else {
                if (value < 0) {
                    $('#<%:txtAdministrationAmount.ClientID %>').val('0').trigger('changeValue');
                }
            }

            if (isPercentage) {
                if (value > 100) {
                    $('#<%:txtAdministrationAmount.ClientID %>').val('0').trigger('changeValue');
                }
            }
        });

        $('#<%:chkPatientAdminPercentage.ClientID %>').live('change', function () {
            var value = $('#<%:txtPatientAdmAmount.ClientID %>').val();
            var token = ",";
            var newToken = "";
            value = value.split(token).join(newToken);
            var isPercentage = $('#<%:chkPatientAdminPercentage.ClientID %>').is(':checked');

            if (isPercentage) {
                if (value > 100) {
                    $('#<%:txtPatientAdmAmount.ClientID %>').val('0').trigger('changeValue');
                }
            }
        });

        $('#<%:txtPatientAdmAmount.ClientID %>').live('change', function () {
            var value = $('#<%:txtPatientAdmAmount.ClientID %>').val();
            var isPercentage = $('#<%:chkPatientAdminPercentage.ClientID %>').is(':checked');

            if (isNaN(value)) {
                $('#<%:txtPatientAdmAmount.ClientID %>').val('0').trigger('changeValue');
            }
            else {
                if (value < 0) {
                    $('#<%:txtPatientAdmAmount.ClientID %>').val('0').trigger('changeValue');
                }
            }

            if (isPercentage) {
                if (value > 100) {
                    $('#<%:txtPatientAdmAmount.ClientID %>').val('0').trigger('changeValue');
                }
            }
        });

        $('#<%:chkServiceChargePercentage.ClientID %>').live('change', function () {
            var value = $('#<%:txtServiceChargeAmount.ClientID %>').val();
            var token = ",";
            var newToken = "";
            value = value.split(token).join(newToken);
            var isPercentage = $('#<%:chkServiceChargePercentage.ClientID %>').is(':checked');

            if (isPercentage) {
                if (value > 100) {
                    $('#<%:txtServiceChargeAmount.ClientID %>').val('0').trigger('changeValue');
                }
            }
        });

        $('#<%:txtServiceChargeAmount.ClientID %>').live('change', function () {
            var value = $('#<%:txtServiceChargeAmount.ClientID %>').val();
            var isPercentage = $('#<%:chkServiceChargePercentage.ClientID %>').is(':checked');

            if (isNaN(value)) {
                $('#<%:txtServiceChargeAmount.ClientID %>').val('0').trigger('changeValue');
            }
            else {
                if (value < 0) {
                    $('#<%:txtServiceChargeAmount.ClientID %>').val('0').trigger('changeValue');
                }
            }

            if (isPercentage) {
                if (value > 100) {
                    $('#<%:txtServiceChargeAmount.ClientID %>').val('0').trigger('changeValue');
                }
            }
        });

        $('#<%:chkPatientServicePercentage.ClientID %>').live('change', function () {
            var value = $('#<%:txtPatientServiceAmount.ClientID %>').val();
            var token = ",";
            var newToken = "";
            value = value.split(token).join(newToken);
            var isPercentage = $('#<%:chkPatientServicePercentage.ClientID %>').is(':checked');

            if (isPercentage) {
                if (value > 100) {
                    $('#<%:txtPatientServiceAmount.ClientID %>').val('0').trigger('changeValue');
                }
            }
        });

        $('#<%:txtPatientServiceAmount.ClientID %>').live('change', function () {
            var value = $('#<%:txtPatientServiceAmount.ClientID %>').val();
            var isPercentage = $('#<%:chkPatientServicePercentage.ClientID %>').is(':checked');

            if (isNaN(value)) {
                $('#<%:txtPatientServiceAmount.ClientID %>').val('0').trigger('changeValue');
            }
            else {
                if (value < 0) {
                    $('#<%:txtPatientServiceAmount.ClientID %>').val('0').trigger('changeValue');
                }
            }

            if (isPercentage) {
                if (value > 100) {
                    $('#<%:txtPatientServiceAmount.ClientID %>').val('0').trigger('changeValue');
                }
            }
        });

        $('#<%:txtMinAdministrationAmount.ClientID %>').live('change', function () {
            var value = $(this).val();
            var maxAmount = $('#<%:txtMaxAdministrationAmount.ClientID %>').val();
            var token = ",";
            var token1 = ".00";
            var newToken = "";
            value = parseFloat(value.split(token).join(newToken));
            maxAmount = maxAmount.split(token).join(newToken);
            maxAmount = parseFloat(maxAmount.split(token1).join(newToken));

            if (value > maxAmount) {
                $('#<%:txtMinAdministrationAmount.ClientID %>').val('0').trigger('changeValue');
            }
        });

        $('#<%:txtMaxAdministrationAmount.ClientID %>').live('change', function () {
            var value = $(this).val();
            var minAmount = $('#<%:txtMinAdministrationAmount.ClientID %>').val();

            var token = ",";
            var token1 = ".00";
            var newToken = "";
            value = parseFloat(value.split(token).join(newToken));
            minAmount = minAmount.split(token).join(newToken);
            minAmount = parseFloat(minAmount.split(token1).join(newToken));

            if (value < minAmount) {
                $('#<%:txtMaxAdministrationAmount.ClientID %>').val(minAmount).trigger('changeValue');
            }
        });

        $('#<%:txtMinPatientAdmAmount.ClientID %>').live('change', function () {
            var value = $(this).val();
            var maxAmount = $('#<%:txtMaxPatientAdmAmount.ClientID %>').val();
            var token = ",";
            var token1 = ".00";
            var newToken = "";
            value = parseFloat(value.split(token).join(newToken));
            maxAmount = maxAmount.split(token).join(newToken);
            maxAmount = parseFloat(maxAmount.split(token1).join(newToken));

            if (value > maxAmount) {
                $('#<%:txtMinPatientAdmAmount.ClientID %>').val('0').trigger('changeValue');
            }
        });

        $('#<%:txtMaxPatientAdmAmount.ClientID %>').live('change', function () {
            var value = $(this).val();
            var minAmount = $('#<%:txtMinPatientAdmAmount.ClientID %>').val();

            var token = ",";
            var token1 = ".00";
            var newToken = "";
            value = parseFloat(value.split(token).join(newToken));
            minAmount = minAmount.split(token).join(newToken);
            minAmount = parseFloat(minAmount.split(token1).join(newToken));

            if (value < minAmount) {
                $('#<%:txtMaxPatientAdmAmount.ClientID %>').val(minAmount).trigger('changeValue');
            }
        });

        $('#<%:txtMinServiceChargeAmount.ClientID %>').live('change', function () {
            var value = $(this).val();
            var maxAmount = $('#<%:txtMaxServiceChargeAmount.ClientID %>').val();
            var token = ",";
            var token1 = ".00";
            var newToken = "";
            value = parseFloat(value.split(token).join(newToken));
            maxAmount = maxAmount.split(token).join(newToken);
            maxAmount = parseFloat(maxAmount.split(token1).join(newToken));

            if (value > maxAmount) {
                $('#<%:txtMinServiceChargeAmount.ClientID %>').val('0').trigger('changeValue');
            }
        });

        $('#<%:txtMaxServiceChargeAmount.ClientID %>').live('change', function () {
            var value = $(this).val();
            var minAmount = $('#<%:txtMinServiceChargeAmount.ClientID %>').val();

            var token = ",";
            var token1 = ".00";
            var newToken = "";
            value = parseFloat(value.split(token).join(newToken));
            minAmount = minAmount.split(token).join(newToken);
            minAmount = parseFloat(minAmount.split(token1).join(newToken));

            if (value < minAmount) {
                $('#<%:txtMaxServiceChargeAmount.ClientID %>').val(minAmount).trigger('changeValue');
            }
        });

        $('#<%:txtMinPatientServiceAmount.ClientID %>').live('change', function () {
            var value = $(this).val();
            var maxAmount = $('#<%:txtMaxPatientServiceAmount.ClientID %>').val();
            var token = ",";
            var token1 = ".00";
            var newToken = "";
            value = parseFloat(value.split(token).join(newToken));
            maxAmount = maxAmount.split(token).join(newToken);
            maxAmount = parseFloat(maxAmount.split(token1).join(newToken));

            if (value > maxAmount) {
                $('#<%:txtMinPatientServiceAmount.ClientID %>').val('0').trigger('changeValue');
            }
        });

        $('#<%:txtMaxPatientServiceAmount.ClientID %>').live('change', function () {
            var value = $(this).val();
            var minAmount = $('#<%:txtMinPatientServiceAmount.ClientID %>').val();

            var token = ",";
            var token1 = ".00";
            var newToken = "";
            value = parseFloat(value.split(token).join(newToken));
            minAmount = minAmount.split(token).join(newToken);
            minAmount = parseFloat(minAmount.split(token1).join(newToken));

            if (value < minAmount) {
                $('#<%:txtMaxPatientServiceAmount.ClientID %>').val(minAmount).trigger('changeValue');
            }
        });

    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnCustomerID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("No Kontrak")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtContractNo" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal Mulai")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtStartDate" CssClass="datepicker" Width="120px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal Akhir")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEndDate" CssClass="datepicker" Width="120px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsControlCoverageLimit" runat="server" />
                            <%=GetLabel("Kontrol Limit Jaminan")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsGenericDrugOnly" runat="server" />
                            <%=GetLabel("Hanya Obat Generic")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsControlClassCare" runat="server" />
                            <%=GetLabel("Kontrol Jatah Kelas")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Jatah Kelas")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboClassCare" Width="200px" ClientInstanceName="cboClassCare"
                                runat="server" />
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
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <table>
                    <tr>
                        <td>
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
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <%=GetLabel("Ringkasan Kontrak") %><br />
                <asp:TextBox TextMode="MultiLine" Width="100%" Height="300px" ID="txtContractSummary"
                    runat="server" CssClass="htmlEditor" />
            </td>
        </tr>
    </table>
</asp:Content>
