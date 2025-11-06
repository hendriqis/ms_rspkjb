<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientAllergyEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inpatient.Program.PatientAllergyEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientallergyctl">
    $('#<%=txtAllergenName.ClientID %>').focus();
    setDatePicker('<%=txtObservationDate.ClientID %>');
    $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

    function oncboAllergenTypeValueChanged(s) {
        var value = s.GetValue();
        if (value == '0127^DA') {
            $('#<%:trAllergenName.ClientID %>').attr('style', 'display:none');
            $('#<%:trDrugGenericName.ClientID %>').removeAttr('style');
        }
        else {
            $('#<%:trDrugGenericName.ClientID %>').attr('style', 'display:none');
            $('#<%:trAllergenName.ClientID %>').removeAttr('style');
        }
    }

    //#region Drug Generic Name
    $('#lblDrugGenericName.lblLink').click(function () {
        openSearchDialog('genericname', 'IsDeleted = 0', function (value) {
            ontxtDrugGenericNameChanged(value);
        });
    });

    function ontxtDrugGenericNameChanged(value) {
        var filterExpression = "GenericID = '" + value + "' AND IsDeleted = 0";
        Methods.getObject('GetvGenericNameList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtDrugGenericName.ClientID %>').val(result.DrugGenericName);
            }
            else {
                $('#<%=txtDrugGenericName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onBeforeSaveRecordEntryPopup(errMessage) {
        var value = cboAllergenType.GetValue();
        if (value == '0127^DA') {
            if ($('#<%=txtDrugGenericName.ClientID %>').val() == '') {
                errMessage.Text = "Harap Isi Nama Alergi Terlebih Dahulu";
                displayErrorMessageBox('SAVE', errMessage.Text);
                return false;
            }
            else {
                return true;
            }
        }
        else {
            if ($('#<%=txtAllergenName.ClientID %>').val() == '') {
                errMessage.Text = "Harap Isi Nama Alergi Terlebih Dahulu";
                displayErrorMessageBox('SAVE', errMessage.Text);
                return false;
            }
            else {
                return true;
            }
        }
    }
</script>
<div style="height: 300px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table style="width: 460px" class="tblEntryContent">
                    <colgroup>
                        <col style="width: 100%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal Log")%></label>
                        </td>
                        <td colspan="5">
                            <asp:TextBox runat="server" ID="txtObservationDate" CssClass="datepicker" Width="130px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tipe Alergi")%></label>
                        </td>
                        <td colspan="5">
                            <dxe:ASPxComboBox runat="server" ID="cboAllergenType" ClientInstanceName="cboAllergenType"
                                Width="300px">
                                <ClientSideEvents ValueChanged="function(s,e){ oncboAllergenTypeValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr id="trAllergenName" runat="server">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Alergi")%></label>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtAllergenName" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr id="trDrugGenericName" runat="server" style="display:none">
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblDrugGenericName">
                                <%=GetLabel("Nama Alergi")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDrugGenericName" Width="300px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Sumber Informasi")%></label>
                        </td>
                        <td colspan="5">
                            <dxe:ASPxComboBox runat="server" ID="cboFindingSource" ClientInstanceName="cboFindingSource"
                                Width="200px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Alergi Sejak")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboYear" ClientInstanceName="cboYear" Width="100%" />
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboMonth" ClientInstanceName="cboMonth" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tingkat Alergi")%></label>
                        </td>
                        <td colspan="5">
                            <dxe:ASPxComboBox runat="server" ID="cboSeverity" ClientInstanceName="cboSeverity"
                                Width="300px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Reaksi Alergi")%></label>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtReaction" Width="300px" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
