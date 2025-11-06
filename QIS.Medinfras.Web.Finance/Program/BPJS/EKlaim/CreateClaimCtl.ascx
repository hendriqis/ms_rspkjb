<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateClaimCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.CreateClaimCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_createclaimctl">
    $(function () {
        setDatePicker('<%=txtDOBCtl.ClientID %>');
        $('#<%=txtDOBCtl.ClientID %>').datepicker('option', 'maxDate', '0');
    });

    //#region MRN
    $('#<%:lblMRN.ClientID %>.lblLink').live('click', function () {
        var filterExpression = "MRN IN (SELECT MRN FROM Registration WHERE GCRegistrationStatus != 'X020^006' AND RegistrationID IN (SELECT RegistrationID FROM RegistrationBPJS WHERE NoSEP IS NOT NULL)) AND IsDeleted = 0";
        openSearchDialog($('#<%:hdnPatientSearchDialogType.ClientID %>').val(), filterExpression, function (value) {
            if (value != null) {
                $('#<%:txtMedicalNoCtl.ClientID %>').val(value);
                ontxtMedicalNoCtlChanged(value);
            }
        });
    });

    $('#<%:txtMedicalNoCtl.ClientID %>').live('change', function () {
        ontxtMedicalNoCtlChanged($(this).val());
    });

    function ontxtMedicalNoCtlChanged(value) {
        var filterExpression = "MedicalNo = '" + value + "' AND IsDeleted = 0";
        Methods.getObject('GetPatientList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:txtNoPesertaCtl.ClientID %>').val(result.NHSRegistrationNo);
                $('#<%:txtPatientNameCtl.ClientID %>').val(result.FullName);
                $('#<%:txtDOBCtl.ClientID %>').val(result.cfDOBInDatePicker);
                cboGenderCtl.SetValue(result.GCGender);
            } else {
                alert("No Rekam Medis tidak tersedia.");
            }
        });
    }
    //#endregion

    //#region SEP
    $('#<%:lblSEP.ClientID %>.lblLink').live('click', function () {
        var medicalNo = $('#<%=txtMedicalNoCtl.ClientID %>').val();
        var noPeserta = $('#<%=txtNoPesertaCtl.ClientID %>').val();
        if (medicalNo != "") {
            var filterExpression = "MedicalNo = '" + medicalNo + "'";
            openSearchDialog('registrationbpjs', filterExpression, function (value) {
                if (value != null) {
                    $('#<%:txtNoSEPCtl.ClientID %>').val(value);
                }
            });
        }
        else if (noPeserta != "") {
            var filterExpression = "NoPeserta = '" + noPeserta + "'";
            openSearchDialog('registrationbpjs', filterExpression, function (value) {
                if (value != null) {
                    $('#<%:txtNoSEPCtl.ClientID %>').val(value);
                }
            });
        }
        else {
            alert("Pilih no Rekam Medis / no Kartu terlebih dahulu.");
        }
    });
    //#endregion

    $('#<%=btnCreateClaim.ClientID %>').live('click', function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            var isBridgingToEKlaim = $('#<%=hdnIsBridgingToEKlaim.ClientID %>').val();
            if (isBridgingToEKlaim == "1") {
                CreateNewClaim();
            }
        }
    });

    //#region Create New Claim
    function CreateNewClaim() {
        var nomor_kartu = $('#<%:txtNoPesertaCtl.ClientID %>').val();
        var nomor_sep = $('#<%:txtNoSEPCtl.ClientID %>').val();
        var nomor_rm = $('#<%:txtMedicalNoCtl.ClientID %>').val();
        var nama_pasien = $('#<%:txtPatientNameCtl.ClientID %>').val();

        var paramDOB = $('#<%:txtDOBCtl.ClientID %>').val();
        var tgl_lahir = paramDOB.substring(10, 6) + "-" + paramDOB.substring(5, 3) + "-" + paramDOB.substring(2, 0) + " 00:00:00";

        var paramGender = cboGenderCtl.GetValue();
        var gender = "";

        if (paramGender == "0003^M") {
            gender = "1";
        } else {
            gender = "2";
        }

        EKlaimService.newClaim(nomor_kartu, nomor_sep, nomor_rm, nama_pasien, tgl_lahir, gender, function (result) {
            if (result != null) {
                showToast('INFORMATION', result);
            }
        });

    }
    //#endregion

</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnCreateClaim" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Process")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <input type="hidden" id="hdnIsBridgingToEKlaim" value="" runat="server" />
    <input type="hidden" id="hdnPatientSearchDialogType" value="patient1" runat="server" />
    <fieldset id="fsTrxPopup" style="margin: 0">
        <table class="tblEntryContent">
            <colgroup>
                <col style="width: 150px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblLink lblMandatory" runat="server" id="lblMRN">
                        <%:GetLabel("No Rekam Medis")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMedicalNoCtl" Width="120px" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("No Kartu")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtNoPesertaCtl" Width="150px" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblLink lblMandatory" runat="server" id="lblSEP">
                        <%=GetLabel("No SEP")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtNoSEPCtl" Width="150px" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Nama Pasien")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtPatientNameCtl" Width="350px" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Tanggal Lahir")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtDOBCtl" Width="120px" runat="server" CssClass="datepicker" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Jenis Kelamin")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboGenderCtl" ClientInstanceName="cboGenderCtl" Width="150px" runat="server">
                    </dxe:ASPxComboBox>
                </td>
            </tr>
        </table>
    </fieldset>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
