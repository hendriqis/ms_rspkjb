<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConsentFormCtl1.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientPage.ConsentFormCtl1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_patientEducationEntryctl">
    setDatePicker('<%=txtObservationDate.ClientID %>');
    $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

    $(function () {

        $('#<%=rblIsPatientFamily.ClientID %> input').die('change');
        $('#<%=rblIsPatientFamily.ClientID %> input').live('change', function () {
            if ($(this).val() == "1") {
                $('#<%=trFamilyInfo.ClientID %>').removeAttr("style");
            }
            else {
                $('#<%=trFamilyInfo.ClientID %>').attr("style", "display:none");
            }
        });

        if ($('#<%=hdnFormValues.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnFormValues.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
                $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.chkNursingProblem').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
            }
        }
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

    function oncbpConsentFormContentEndCallback(s) {
        $('#<%=hdnDivFormLayout.ClientID %>').val(s.cpFormLayout);
        var formContent = s.cpFormLayout;

        $('#<%=divFormContent.ClientID %>').html(
          $('<div/>', {
              html: formContent
          }).html()
        )

        hideLoadingPanel();
    }

    function onBeforeSaveRecord(errMessage) {
        var patientEducationForm = $('#<%=hdnPatientEducationForm.ClientID %>').val();
        var values = getFormValues();

        return true;
    }

    function getFormValues() {
        var controlValues = '';
        $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            controlValues += $(this).attr('controlID') + '=' + $(this).val();
        });
        $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            controlValues += $(this).attr('controlID') + '=' + $(this).val();
        });
        $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });
        $('#<%=divFormContent.ClientID %>').find('.chkNursingProblem').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });
        $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });
        $('#<%=hdnFormValues.ClientID %>').val(controlValues);

        return controlValues;
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
                        <li contentID="divPage1" contentIndex="1" title="Informasi Umum" class="w3-hover-red">Pemberian Informasi</li>
                        <li contentID="divPage2" contentIndex="3" title="Konfirmasi Informasi" class="w3-hover-red">Konfirmasi Informasi</li>
                    </ul>
                </div>
            </td>
            <td style="padding:5px;vertical-align:top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                   <table class="tblEntryContent" style="width:99%">
                        <colgroup>
                            <col style="width:180px"/>
                            <col style="width:150px"/>
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tanggal ")%> - <%=GetLabel("Jam")%></label></td>
                            <td><asp:TextBox ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                            <td><asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Pemberi Informasi")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboParamedicID" Width="350px" runat="server" />
                            </td>
                        </tr>

                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kelompok")%></label></td>
                            <td colspan="2">
                                <dxe:ASPxComboBox runat="server" ID="cboGCConsentFormGroup" ClientInstanceName="cboGCConsentFormGroup"
                                    Width="100%">
                                    <ClientSideEvents SelectedIndexChanged="function (s,e) {cbpConsentFormType.PerformCallback();}" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Informed Consent")%></label></td>
                            <td colspan="2">
                                <dxcp:ASPxCallbackPanel ID="cbpConsentFormType" runat="server" Width="100%" ClientInstanceName="cbpConsentFormType"
                                    ShowLoadingPanel="false" OnCallback="cbpFormConsentType_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { oncbpConsentFormTypeEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                            <dxe:ASPxComboBox runat="server" ID="cboGCConsentFormType" ClientInstanceName="cboGCConsentFormType"
                                                Width="100%">
                                                <ClientSideEvents SelectedIndexChanged = "function(s,e) { cbpConsentFormContent.PerformCallback();}" />
                                            </dxe:ASPxComboBox>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="width:100%">
                                <dxcp:ASPxCallbackPanel ID="cbpConsentFormContent" runat="server" Width="100%" ClientInstanceName="cbpConsentFormContent"
                                    ShowLoadingPanel="false" OnCallback="cbpConsentFormContent_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { oncbpConsentFormContentEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent7" runat="server">
                                            <table class="tblContentArea" border="0">
                                                <tr>
                                                    <td>
                                                        <div id="divFormContent" runat="server" style="height: 450px;overflow-y: auto;"></div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                            </td>
                        </tr>
                  </table>             
                </div>
                <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                   <table class="tblEntryContent" style="width:99%">
                        <colgroup>
                            <col style="width:180px"/>
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Penerima Informasi")%></label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblIsPatientFamily" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Pasien" Value="0" Selected="True" />
                                    <asp:ListItem Text="Keluarga / Lain-lain" Value="1"  />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr id="trFamilyInfo" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama Penerima")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                    <colgroup>
                                        <col style="width:140px"/>
                                        <col style="width:85px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSignature2Name" CssClass="txtSignature2Name" runat="server" Width="100%"  />
                                        </td>
                                        <td class="tdLabel" style="padding-left:5px">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Hubungan")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox runat="server" ID="cboFamilyRelation" ClientInstanceName="cboFamilyRelation"
                                                Width="99%" ToolTip = "Hubungan dengan Pasien" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>    
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Saksi-1")%></label></td>
                            <td>
                                <asp:TextBox ID="txtSignature3Name" CssClass="txtSignature3Name" runat="server" Width="100%"  />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Saksi-2")%></label></td>
                            <td>
                                <asp:TextBox ID="txtSignature4Name" CssClass="txtSignature4Name" runat="server" Width="100%"  />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col style="width:400px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Sudah membaca dan mengerti informasi yang disampaikan")%></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rblIsReadAndUnderstand" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text=" Ya" Value="1" />
                                                <asp:ListItem Text=" Tidak" Value="0"  />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col style="width:400px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Persetujuan terhadap tindakan/informasi yang disampaikan")%></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rblIsApproveOrReject" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text=" Ya" Value="1" />
                                                <asp:ListItem Text=" Tidak" Value="0"  />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                   </table>
                </div>
            </td>
        </tr>
    </table>
</div>


