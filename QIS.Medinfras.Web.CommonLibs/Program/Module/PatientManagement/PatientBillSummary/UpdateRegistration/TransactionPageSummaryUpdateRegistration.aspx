<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="TransactionPageSummaryUpdateRegistration.aspx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransactionPageSummaryUpdateRegistration" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcessUpdateRegistration" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Simpan")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=btnProcessUpdateRegistration.ClientID %>').click(function () {
                if (IsValid(null, 'fsEditReg', 'mpEditReg'))
                    onCustomButtonClick('update');
            });

            //#region Physician
            function onGetPatientVisitParamedicFilterExpression() {
                var filterExpression = "<%:OnGetParamedicFilterExpression() %>";
                return filterExpression;
            }

            $('#lblPatientVisitPhysician.lblLink').click(function () {
                openSearchDialog('paramedic', onGetPatientVisitParamedicFilterExpression(), function (value) {
                    $('#<%=txtPhysicianCode.ClientID %>').val(value);
                    onTxtPatientVisitPhysicianCodeChanged(value);
                });
            });

            $('#<%=txtPhysicianCode.ClientID %>').change(function () {
                onTxtPatientVisitPhysicianCodeChanged($(this).val());
            });

            function onTxtPatientVisitPhysicianCodeChanged(value) {
                var filterExpression = onGetPatientVisitParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
                Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                    if (result != null) {
                        cboRegistrationEditSpecialty.SetValue(result.SpecialtyID);
                        $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                        $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                    }
                    else {
                        cboRegistrationEditSpecialty.SetValue('');
                        $('#<%=hdnPhysicianID.ClientID %>').val('');
                        $('#<%=txtPhysicianCode.ClientID %>').val('');
                        $('#<%=txtPhysicianName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region referral
            function getReferralDescriptionFilterExpression() {
                var filterExpression = "GCReferrerGroup = '" + cboReferral.GetValue() + "' AND IsDeleted = 0";
                return filterExpression;
            }

            function getReferralParamedicFilterExpression() {
                var filterExpression = "GCParamedicMasterType = '" + Constant.ParamedicType.Physician + "'";
                return filterExpression;
            }

            $('#<%:lblReferralDescription.ClientID %>.lblLink').live('click', function () {
                var referral = cboReferral.GetValue();
                if (referral == Constant.ReferrerGroup.DOKTERRS) {
                    openSearchDialog('referrerparamedic', getReferralParamedicFilterExpression(), function (value) {
                        $('#<%:txtReferralDescriptionCode.ClientID %>').val(value);
                        onTxtReferralDescriptionCodeChanged(value);
                    });
                } else {
                    openSearchDialog('referrer', getReferralDescriptionFilterExpression(), function (value) {
                        $('#<%:txtReferralDescriptionCode.ClientID %>').val(value);
                        onTxtReferralDescriptionCodeChanged(value);
                    });
                }
            });

            $('#<%:txtReferralDescriptionCode.ClientID %>').live('change', function () {
                onTxtReferralDescriptionCodeChanged($(this).val());
            });

            function onTxtReferralDescriptionCodeChanged(value) {
                var filterExpression = "";
                var referral = cboReferral.GetValue();
                if (referral == Constant.ReferrerGroup.DOKTERRS) {
                    filterExpression = getReferralParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
                    Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%:hdnReferrerParamedicID.ClientID %>').val(result.ParamedicID);
                            $('#<%:txtReferralDescriptionName.ClientID %>').val(result.ParamedicName);
                        }
                        else {
                            $('#<%:hdnReferrerParamedicID.ClientID %>').val('');
                            $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                            $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                        }
                    });
                } else {
                    filterExpression = getReferralDescriptionFilterExpression() + " AND CommCode = '" + value + "'";
                    Methods.getObject('GetvReferrerList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%:hdnReferrerID.ClientID %>').val(result.BusinessPartnerID);
                            $('#<%:txtReferralDescriptionName.ClientID %>').val(result.BusinessPartnerName);
                        }
                        else {
                            $('#<%:hdnReferrerID.ClientID %>').val('');
                            $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                            $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                        }
                    });
                }
            }
            //#endregion
        });

        function onAfterCustomClickSuccess(type) {
            var filterExpression = 'RegistrationID = ' + $('#<%=hdnRegistrationID.ClientID %>').val();
            Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                setLblParamedicName(result.ParamedicName);
                setLblSpecialtyName(result.SpecialtyName);
            });

        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <fieldset id="fsEditReg">
        <table class="tblContentArea" style="width: 600px">
            <colgroup>
                <col style="width: 25%" />
                <col style="width: 75%" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("No Registrasi")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtRegistrationID" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("No RM")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMRN" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Klinik")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtServiceUnit" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblLink lblMandatory" id="lblPatientVisitPhysician">
                        <%=GetLabel("Dokter / Paramedis")%></label>
                </td>
                <td>
                    <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 30%" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtPhysicianCode" CssClass="required" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Spesialisasi")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboRegistrationEditSpecialty" ClientInstanceName="cboRegistrationEditSpecialty"
                        Width="100%" runat="server" />
                </td>
            </tr>
            <tr id="trChargeClass" runat="server">
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Kelas Tagihan")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboChargeClassID" ClientInstanceName="cboChargeClassID" Width="100%"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%:GetLabel("Rujukan")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboReferral" ClientInstanceName="cboReferral" Width="100%"
                        runat="server">
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblLink" runat="server" id="lblReferralDescription">
                        <%:GetLabel("Deskripsi Rujukan")%></label>
                </td>
                <td>
                    <input type="hidden" id="hdnReferrerID" value="" runat="server" />
                    <input type="hidden" id="hdnReferrerParamedicID" value="" runat="server" />
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 30%" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtReferralDescriptionCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferralDescriptionName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%:GetLabel("No. Rujukan")%></label>
                </td>
                <td>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 30%" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtReferralNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
