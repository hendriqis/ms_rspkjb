<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ChangePatientBlackListStatus.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ChangePatientBlackListStatus" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnChangePatientBlackListStatusSave" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('#<%=txtOtherBlackListReason.ClientID %>').hide();
            $('#<%=cboGCBlacklistReason.ClientID %>').hide();

            $('#<%=btnChangePatientBlackListStatusSave.ClientID %>').click(function () {
                var mrn = $('#<%=txtMRN.ClientID %>').val();
                if (mrn == '')
                    showToast('Warning', '<%=GetErrorMsgSelectMedicalNoFirst() %>');
                else
                    onCustomButtonClick('changepatientblacklist');
            });

            //#region No RM
            $('#<%=lblMRN.ClientID %>.lblLink').click(function () {
                openSearchDialog('patient', '', function (value) {
                    $('#<%=txtMRN.ClientID %>').val(value);
                    onTxtMRNChanged(value);
                });
            });
            $('#<%=txtMRN.ClientID %>').change(function () {
                onTxtMRNChanged($(this).val());
            });

            $('#<%=chkIsBlackList.ClientID %>').change(function () {
                if ($('#<%=chkIsBlackList.ClientID %>').is(':checked')) {
                    $('#<%=cboGCBlacklistReason.ClientID %>').show();
                    cboGCBlacklistReason.SetText('');
                } else {
                    $('#<%=cboGCBlacklistReason.ClientID %>').hide();
                    $('#<%=txtOtherBlackListReason.ClientID %>').hide();
                    $('#<%=txtOtherBlackListReason.ClientID %>').val('');
                }
            });

            function onTxtMRNChanged(value) {
                var filterExpression = "MedicalNo = '" + value + "'";
                Methods.getObject('GetvPatientList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnMRN.ClientID %>').val(result.MRN);
                        $('#<%=txtPatientName.ClientID %>').val(result.PatientName);
                        $('#<%=txtAddress.ClientID %>').val(result.HomeAddress);
                        $('#<%=txtTelephone.ClientID %>').val(result.cfPhoneNo);
                        $('#<%=txtMobilePhoneNo.ClientID %>').val(result.cfMobilePhoneNo);
                        $('#<%=chkIsBlackList.ClientID %>').prop('checked', result.IsBlacklist);

                        if (result.IsBlacklist == false) {
                            $('#<%=cboGCBlacklistReason.ClientID %>').hide();
                            cboGCBlacklistReason.SetText('');
                            $('#<%=txtOtherBlackListReason.ClientID %>').hide();
                            $('#<%=txtOtherBlackListReason.ClientID %>').val('');
                        } else {
                            $('#<%=cboGCBlacklistReason.ClientID %>').show();
                            cboGCBlacklistReason.SetValue(result.GCBlacklistReason);

                            var defaultOther = Constant.PatientBlackListReason.OTHER;
                            if (result.GCBlacklistReason == defaultOther) {
                                $('#<%=txtOtherBlackListReason.ClientID %>').show();
                                $('#<%=txtOtherBlackListReason.ClientID %>').val(result.OtherBlacklistReason);
                            } else {
                                $('#<%=txtOtherBlackListReason.ClientID %>').hide();
                                $('#<%=txtOtherBlackListReason.ClientID %>').val('');
                            }
                        }
                    }
                    else {
                        $('#<%=hdnMRN.ClientID %>').val('');
                        $('#<%=txtMRN.ClientID %>').val('');
                        $('#<%=txtPatientName.ClientID %>').val('');
                        $('#<%=txtAddress.ClientID %>').val('');
                        $('#<%=txtTelephone.ClientID %>').val('');
                        $('#<%=txtMobilePhoneNo.ClientID %>').val('');
                        $('#<%=chkIsBlackList.ClientID %>').prop('checked', false);
                        $('#<%=cboGCBlacklistReason.ClientID %>').hide();
                        cboGCBlacklistReason.SetText('');
                        $('#<%=txtOtherBlackListReason.ClientID %>').hide();
                        $('#<%=txtOtherBlackListReason.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }

        function onAfterCustomClickSuccess(type) {
            showToast('Save Success', '');

            $('#<%=hdnMRN.ClientID %>').val('');
            $('#<%=txtMRN.ClientID %>').val('');
            $('#<%=txtPatientName.ClientID %>').val('');
            $('#<%=txtAddress.ClientID %>').val('');
            $('#<%=txtTelephone.ClientID %>').val('');
            $('#<%=txtMobilePhoneNo.ClientID %>').val('');
            $('#<%=chkIsBlackList.ClientID %>').prop('checked', false);
            cboGCBlacklistReason.SetText('');
            $('#<%=txtOtherBlackListReason.ClientID %>').hide();
            $('#<%=txtOtherBlackListReason.ClientID %>').val('');
        }

        function onCboGCBlacklistReasonValueChanged() {
            var defaultOther = Constant.PatientBlackListReason.OTHER;
            if (cboGCBlacklistReason.GetValue() == defaultOther) {
                $('#<%=txtOtherBlackListReason.ClientID %>').show();
            }
            else {
                $('#<%=txtOtherBlackListReason.ClientID %>').hide();
                $('#<%=txtOtherBlackListReason.ClientID %>').val('');
            }
        }
    </script>
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <%--<div class="pageTitle"><%=GetMenuCaption()%></div>--%>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td colspan="2">
                <h4>
                    <%=GetLabel("Data Pasien")%></h4>
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" runat="server" id="lblMRN">
                                <%=GetLabel("No. RM")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMRN" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Pasien")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatientName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Alamat")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Telepon")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTelephone" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Ponsel")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMobilePhoneNo" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Status")%></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsBlackList" runat="server" /><%=GetLabel("Blacklist")%>
                        </td>
                        <td style="display: none">
                            <asp:CheckBox ID="chkIsPSE" runat="server" /><%=GetLabel("PSE")%>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboGCBlacklistReason" Width="200px" ClientInstanceName="cboGCBlacklistReason"
                                runat="server">
                                <ClientSideEvents ValueChanged="function(){ onCboGCBlacklistReasonValueChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtOtherBlackListReason" Width="100%" runat="server" TextMode="MultiLine"
                                Rows="1" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
