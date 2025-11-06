<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeReferralManualCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ChangeReferralManualCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ChangeReferralManualCtl">
    $(function () {
    });

    function onCboReferralValueChanged(s) {
        if (getIsAdd()) {
            if (cboReferral.GetValue() != '' && cboReferral.GetValue() != null) {
                $('#<%:lblReferralDescription.ClientID %>').attr('class', 'lblLink');
                $('#<%:txtReferralDescriptionCode.ClientID %>').removeAttr('readonly');
                $('#<%:txtReferralNo.ClientID %>').removeAttr('readonly');
            }
            else {
                $('#<%:lblReferralDescription.ClientID %>').attr('class', 'lblDisabled');
                $('#<%:txtReferralDescriptionCode.ClientID %>').attr('readonly', 'readonly');
                $('#<%:txtReferralNo.ClientID %>').attr('readonly', 'readonly');
                $('#<%:hdnReferrerID.ClientID %>').val('');
                $('#<%:hdnReferrerParamedicID.ClientID %>').val('');
                $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                $('#<%:txtReferralNo.ClientID %>').val('');

            }
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
        var referralid = $('#<%:hdnReferrerID.ClientID %>').val();
        var paramedicid = $('#<%:hdnReferrerParamedicID.ClientID %>').val();
        if (referral == Constant.ReferrerGroup.DOKTERRS) {
            filterExpression = getReferralParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {

                if (result != null) {
                    
                    $('#<%:hdnReferrerParamedicID.ClientID %>').val(result.ParamedicID); ///
                    $('#<%:txtReferralDescriptionName.ClientID %>').val(result.ParamedicName);

                    $('#<%:hdnReferrerID.ClientID %>').val('');
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

                    $('#<%:hdnReferrerParamedicID.ClientID %>').val('');
                   
                }
                else {
              
                    $('#<%:hdnReferrerID.ClientID %>').val('');
                    $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                    $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                }
               
            }
            );
            
        }
    }
    //#endregion
    function onGetEntryPopupReturnValue() {
        var result = $('#<%=txtRegistrationNo.ClientID %>').val();
        return result;
    }
</script>
<div style="height: 250px; overflow-y: auto;">
    <input type="hidden" runat="server" id="hdnRegistrationIDCtl" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 150px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No Registrasi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtRegistrationNo" ReadOnly="true" Width="150px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Pasien")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPatient" ReadOnly="true" Width="90%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%:GetLabel("Rujukan")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboReferral" ClientInstanceName="cboReferral" Width="90%"
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
                <table style="width: 90%" cellpadding="0" cellspacing="0">
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
                            <asp:TextBox ID="txtReferralDescriptionName" Width="100%" runat="server" />
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
                <asp:TextBox ID="txtReferralNo" Width="90%" runat="server" />
            </td>
        </tr>
    </table>
</div>
