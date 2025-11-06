<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="ServiceUnitEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ServiceUnitEntry" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" id="dxss_patiententryctl">
        function onLoad() {
            //#region BPJS Reference Poli
            $('#lblvKlaimPoli.lblLink').click(function () {
                openSearchDialog('vklaimpoli', '', function (value) {
                    $('#<%=txtVKlaimPoliCode.ClientID %>').val(value);
                    ontxtvKlaimPoliCodeChanged(value);
                });
            });

            $('#<%=txtVKlaimPoliCode.ClientID %>').change(function () {
                ontxtvKlaimPoliCodeChanged($(this).val());
            });

            function ontxtvKlaimPoliCodeChanged(value) {
                var filterExpression = "BPJSCode = '" + value + "'";
                Methods.getObject('GetvBPJSReferencePoliList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtVKlaimPoliCode.ClientID %>').val(result.BPJSCode);
                        $('#<%=txtVKlaimPoliName.ClientID %>').val(result.BPJSName);
                    }
                    else {
                        $('#<%=txtVKlaimPoliCode.ClientID %>').val('');
                        $('#<%=txtVKlaimPoliName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Inhealth Reference Poli
            $('#lblInhealthReference.lblLink').click(function () {
                var filterExpression = "Others LIKE '%True%'";
                openSearchDialog('vinhealthreferencepoli', filterExpression, function (value) {
                    $('#<%=txtInhealthKodePoli.ClientID %>').val(value);
                    onTxtInhealthKodePoliChanged(value);
                });
            });

            $('#<%=txtInhealthKodePoli.ClientID %>').change(function () {
                onTxtInhealthKodePoliChanged($(this).val());
            });

            function onTxtInhealthKodePoliChanged(value) {
                var filterExpression = "Others = '" + value + "'";
                Methods.getObject('GetvInhealthReferencePoliList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtInhealthKodePoli.ClientID %>').val(result.ObjectCode);
                        $('#<%=txtInhealthNamaPoli.ClientID %>').val(result.ObjectName);
                    }
                    else {
                        $('#<%=txtInhealthKodePoli.ClientID %>').val('');
                        $('#<%=txtInhealthNamaPoli.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }

        $('#btnGetIHS').live('click', function () {
            var serviceUnitCode = $('#<%=txtServiceUnitCode.ClientID %>').val();
            var serviceUnitName = $('#<%=txtServiceUnitName.ClientID %>').val();
            var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
            try {
                IHSService.createIHSLocationID(serviceUnitCode, serviceUnitName, departmentID, function (result) {
                    GetIHSDataHandler(result);
                });
            }
            catch (err) {
                displayErrorMessageBox("Integrasi SATUSEHAT", err.Message);
            }
        });

        function GetIHSDataHandler(result) {
            try {
                var resultInfo = result.split('|');
                if (resultInfo[0] == "1") {
                    $('#<%=txtIHSLocationID.ClientID %>').val(resultInfo[1]);
                }
                else {
                    $('#<%=txtIHSLocationID.ClientID %>').val('');
                    displayErrorMessageBox('Integrasi SatuSehat', resultInfo[2]);
                }
            }
            catch (err) {
                displayErrorMessageBox('Integrasi SATUSEHAT', 'Error Message : ' + err.Description);
            }
        }

    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnDepartmentID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitICUID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitPICUID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitNICUID" runat="server" value="" />
    <input type="hidden" id="hdnIsBridgingToMedinfrasMobileApps" runat="server" value="" />
    <input type="hidden" id="hdnIsAllowMultiVisitSchedule" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 40%" />
            <col style="width: 60%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtServiceUnitCode" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtServiceUnitName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama 2")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtServiceUnitName2" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Singkat")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtShortName" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblvKlaimPoli">
                                <%=GetLabel("Referensi V-Klaim")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtVKlaimPoliCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVKlaimPoliName" Enabled="false" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblInhealthReference">
                                <%=GetLabel("Referensi Inhealth")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtInhealthKodePoli" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInhealthNamaPoli" Enabled="false" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Lama Pelayanan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtServiceInterval" Width="150px" CssClass="number" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" title="Kode Lokasi/Organisasi di Platform SATUSEHAT Kemenkes">
                                <%=GetLabel("IHS ID / No. SatuSEHAT") %>
                            </label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtIHSLocationID" Width="100%" runat="server" />
                        </td>
                        <td style="padding-left: 5px">
                            <input type="button" id="btnGetIHS" value="Get IHS Location ID" style="width: 100%;" class="btnGetIHS w3-btn1 w3-hover-blue" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No Akunt GL Segment")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtGLAccountSegmentNo" Width="150px" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <div>
                    <asp:CheckBox ID="chkIsUsingJobOrder" runat="server" Text=" Order Pemeriksaan" />
                </div>
                <div>
                    <asp:CheckBox ID="chkIsAllowRegistration" runat="server" Text=" Pendaftaran Pasien" />
                </div>
                <div>
                    <asp:CheckBox ID="chkIsHasPrescription" runat="server" Text=" Pelayanan Order Farmasi (Resep)" />
                </div>
                <div>
                    <asp:CheckBox ID="chkIsLaboratoryUnit" runat="server" Text=" Unit Laboratorium" />
                </div>
                <div>
                    <asp:CheckBox ID="chkIsNutritionUnit" runat="server" Text=" Unit Gizi" />
                </div>
                <div>
                    <asp:CheckBox ID="chkIsIntensiveUnit" runat="server" Text=" Unit Perawatan Intensif" />
                </div>
                <div>
                    <asp:CheckBox ID="chkIsHasDiagnosticResult" runat="server" Text=" Mempunyai Hasil Penunjang" />
                </div>
                <div>
                    <asp:CheckBox ID="chkIsAllowMultiVisitSchedule" runat="server" Text=" Penjadwalan Multikunjungan" />
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
