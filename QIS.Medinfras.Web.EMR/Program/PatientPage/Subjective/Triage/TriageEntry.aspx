<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master"
    AutoEventWireup="true" CodeBehind="TriageEntry.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.TriageEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientStatusSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erpatientstatus1">
        $(function () {
            setDatePicker('<%=txtServiceDate.ClientID %>');
            $('#<%=txtServiceDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            cboVisitType.SetFocus();

            $('#<%=btnPatientStatusSave.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientStatus', 'mpPatientStatus'))
                    onCustomButtonClick('save');
            });
        });
        
        function onAfterCustomClickSuccess(type, retval) {
        }

        function onCboVisitReasonValueChanged() {
            if (cboVisitReason.GetValue() == Constant.VisitReason.OTHER)
                $('#<%=txtVisitNotes.ClientID %>').removeAttr('readonly');
            else
                $('#<%=txtVisitNotes.ClientID %>').attr('readonly', 'readonly');
        }

        function onCboReferralValueChanged(s) {
            $('#<%:hdnReferrerID.ClientID %>').val('');
            $('#<%:hdnReferrerParamedicID.ClientID %>').val('');
            $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
            $('#<%:txtReferralDescriptionName.ClientID %>').val('');
            if (cboReferral.GetValue() != '' && cboReferral.GetValue() != null) {
                $('#<%:lblReferralDescription.ClientID %>').attr('class', 'lblLink');
                $('#<%:txtReferralDescriptionCode.ClientID %>').removeAttr('readonly');
            }
            else {
                $('#<%:lblReferralDescription.ClientID %>').attr('class', 'lblDisabled');
                $('#<%:txtReferralDescriptionCode.ClientID %>').attr('readonly', 'readonly');
            }
        }

        //#region Referral Description
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
                filterExpression = getReferralDescriptionFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
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
    </script>
    <div>
        <input type="hidden" runat="server" id="hdnID" value="" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0minute" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1minute" value="00" />
        <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
        <fieldset id="mpPatientStatus">
            <table class="tblContentArea">
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <table class="tblEntryContent" style="width:50%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Tanggal dan Waktu")%></label>
                                </td>
                                <td colspan="2">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtServiceDate" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                            <td style="padding-left:5px">
                                                <asp:TextBox ID="txtServiceTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   <label class="lblMandatory">
                                    <%=GetLabel("Jenis Kunjungan") %></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox runat="server" ID="cboVisitType" ClientInstanceName="cboVisitType" Width="100%" />
                                </td>
                            </tr>     
                            <tr>
                                <td>
                                   <label class="lblMandatory">
                                    <%=GetLabel("Triage") %></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox runat="server" ID="cboTriage" ClientInstanceName="cboTriage" Width="100%" />
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
                                <ClientSideEvents ValueChanged="function(s){ onCboReferralValueChanged(s); }" />
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
                                    <col style="width: 80px" />
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
                                        <asp:TextBox ID="txtReferralDescriptionName" Width="100%" runat="server" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Alasan Kunjungan")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboVisitReason" ClientInstanceName="cboVisitReason" Width="100%"
                                        runat="server">
                                        <ClientSideEvents Init="function(s,e){ onCboVisitReasonValueChanged(s); }" ValueChanged="function(s,e){ onCboVisitReasonValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>    
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" id="lblOtherVisitNotesLabel" runat="server">
                                    </label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVisitNotes" Width="100%" runat="server" />
                                </td>
                            </tr>  
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Keadaan Datang")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboAdmissionCondition" ClientInstanceName="cboAdmissionCondition"
                                        Width="100%" runat="server" />
                                </td>
                            </tr>     
                            <tr>
                                <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                    <%=GetLabel("Lokasi Kejadian") %> <br />
                                    <%=GetLabel("Kasus di luar penyakit") %>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtEmergencyCase" runat="server" Width="100%" TextMode="Multiline"
                                        Rows="5" />
                                </td>
                            </tr>                                                                                                                                              
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>