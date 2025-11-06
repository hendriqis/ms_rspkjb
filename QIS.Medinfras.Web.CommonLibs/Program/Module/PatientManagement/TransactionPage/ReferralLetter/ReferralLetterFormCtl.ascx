<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReferralLetterFormCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientPage.ReferralLetterFormCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_ReferralLetterFormCtl">
    setDatePicker('<%=txtObservationDate.ClientID %>');
    $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

    $(function () {
    });

    //#region Left Navigation Panel
    $('#leftPageNavPanel ul li').click(function () {
        $('#leftPageNavPanel ul li.selected').removeClass('selected');
        $(this).addClass('selected');
        var contentID = $(this).attr('contentID');

        showContent(contentID);
    });

    function showContent(contentID) {
        var i, x, tablinks;
        x = document.getElementsByClassName("divPageNavPanelContent");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        document.getElementById(contentID).style.display = "block";
    }
    //#endregion

    function oncbpConsentFormTypeEndCallback(s) {
        hideLoadingPanel();
    }

    function onBeforeSaveRecord(errMessage) {
        return true;
    }

    function onAfterSaveAddRecordEntryPopup(result) {
        if (typeof onRefreshControl == 'function')
            onRefreshControl();
    }

    function onAfterSaveEditRecordEntryPopup(result) {
        if (typeof onRefreshControl == 'function')
            onRefreshControl();
    }

    $('#leftPageNavPanel ul li').first().click();
</script>
<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnDivFormContent" value="" />
    <input type="hidden" runat="server" id="hdnDivFormLayout" value="" />
    <input type="hidden" runat="server" id="hdnFormValues" value="" />
    <input type="hidden" runat="server" id="hdnConsentFormGroup" value="" />
    <input type="hidden" runat="server" id="hdnExtraFieldCount" value="" />
    <input type="hidden" runat="server" id="hdnPatientEducationForm" value="" />
    <input type="hidden" runat="server" id="hdnExistingSignature" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 20%" />
            <col style="width: 80%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPageNavPanel" class="w3-border">
                    <ul>
                        <li contentid="divPage1" contentindex="1" title="Informasi Umum" class="w3-hover-red">
                            Informasi Umum</li>
                        <li contentid="divPage2" contentindex="3" title="Konfirmasi Informasi" class="w3-hover-red">
                            Extra Field</li>
                    </ul>
                </div>
            </td>
            <td style="padding: 5px; vertical-align: top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 99%">
                        <colgroup>
                            <col style="width: 180px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server"
                                    Style="text-align: center" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Pembuat")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboParamedicID" Width="350px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Surat Rujukan")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox runat="server" ID="cboGCConsentFormGroup" ClientInstanceName="cboGCConsentFormGroup"
                                    Width="100%">
                                    <ClientSideEvents SelectedIndexChanged="function (s,e) {cbpConsentFormType.PerformCallback();}" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 99%">
                        <colgroup>
                            <col style="width: 180px" />
                            <col />
                        </colgroup>
                        <tr id="trExtraField1" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField1" CssClass="txtField1" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trExtraField2" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField2" CssClass="txtField2" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trExtraField3" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField3" CssClass="txtField3" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trExtraField4" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField4" CssClass="txtField4" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trExtraField5" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField5" CssClass="txtField5" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trExtraField6" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label6" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField6" CssClass="txtField6" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trExtraField7" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label7" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField7" CssClass="txtField7" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trExtraField8" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label8" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField8" CssClass="txtField8" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trExtraField9" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label9" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField9" CssClass="txtField9" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trExtraField10" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label10" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField10" CssClass="txtField10" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trExtraField11" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label11" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField11" CssClass="txtField11" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trExtraField12" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label12" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField12" CssClass="txtField12" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trExtraField13" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label13" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField13" CssClass="txtField13" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trExtraField14" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label14" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField14" CssClass="txtField14" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trExtraField15" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label15" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField15" CssClass="txtField15" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trExtraField16" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label16" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField16" CssClass="txtField16" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trExtraField17" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label17" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField17" CssClass="txtField17" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trExtraField18" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label18" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField18" CssClass="txtField18" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trExtraField19" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label19" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField19" CssClass="txtField19" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr id="trExtraField20" runat="server" style="display: none">
                            <td class="tdLabel">
                                <asp:Label ID="Label20" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtField20" CssClass="txtField20" runat="server" Width="100%" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
