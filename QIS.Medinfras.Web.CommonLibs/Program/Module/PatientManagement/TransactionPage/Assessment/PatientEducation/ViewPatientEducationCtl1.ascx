<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewPatientEducationCtl1.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientPage.ViewPatientEducationCtl1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_patientEducationEntryctl">
    $(function () {
        $(".rblEducationTypeStatus input").change(function () {
            $txt = $(this).closest('tr').parent().closest('tr').find('.txtFreeText');
            if ($(this).val() == '1')
                $txt.show();
            else
                $txt.hide();
        });
        $(".rblEducationTypeStatus").each(function () {
            $(this).find('input[checked=checked]').change();
        });

        $('#<%=rblIsPatientFamily.ClientID %> input').change(function () {
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
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.chkNursingProblem').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                    $(this).prop('disabled', true);
                });
                $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                    $(this).prop('disabled', true);
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

    function onCbpFormEducationTypeEndCallback(s) {
        hideLoadingPanel();
    }

    function onCbpAssessmentContentEndCallback(s) {
        $('#<%=hdnDivFormLayout.ClientID %>').val(s.cpFormLayout);
        var formContent = s.cpFormLayout;

        $('#<%=divFormContent.ClientID %>').html(
          $('<div/>', {
              html: formContent
          }).html()
        )

        hideLoadingPanel();
    }

    function onCboFormChanged() {

    }

    function onBeforeSaveRecordEntryPopup() {
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

    $('#leftPageNavPanel ul li').first().click();
</script>
<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnDivFormContent" value="" />
    <input type="hidden" runat="server" id="hdnDivFormLayout" value="" />
    <input type="hidden" runat="server" id="hdnFormValues" value="" />
    <input type="hidden" runat="server" id="hdnEducationFormGroup" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 20%" />
            <col style="width: 80%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPageNavPanel" class="w3-border" style="width:150px">
                    <ul>
                        <li contentID="divPage1" contentIndex="1" title="Informasi Umum" class="w3-hover-red">Informasi Umum</li>
                        <li contentID="divPage2" contentIndex="2" title="Materi Edukasi" class="w3-hover-red">Materi Edukasi</li>
                        <li contentID="divPage3" contentIndex="3" title="Evaluas Edukasi" class="w3-hover-red">Evaluasi Edukasi</li>
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
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tanggal ")%> - <%=GetLabel("Jam Edukasi")%></label></td>
                            <td><asp:TextBox ID="txtObservationDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" /></td>
                            <td><asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" ReadOnly="true" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Pemberi Informasi")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtParamedicName" Width="350px" runat="server" Style="text-align:left" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Penerima Informasi")%></label>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblIsPatientFamily" runat="server" RepeatDirection="Horizontal" Enabled="false">
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
                            <td colspan="2">
                                <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                    <colgroup>
                                        <col style="width:140px"/>
                                        <col style="width:85px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSignature2Name" CssClass="txtSignature2Name" runat="server" Width="100%" ReadOnly="true"  />
                                        </td>
                                        <td class="tdLabel" style="padding-left:5px">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Hubungan")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFamilyRelation" CssClass="txtFamilyRelation" runat="server" Width="100%" ReadOnly="true"  />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr> 
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Saksi-1")%></label></td>
                            <td colspan="3">
                                <asp:TextBox ID="txtSignature3Name" CssClass="txtSignature3Name" runat="server" Width="100%" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Saksi-2")%></label></td>
                            <td colspan="3">
                                <asp:TextBox ID="txtSignature4Name" CssClass="txtSignature4Name" runat="server" Width="100%" ReadOnly="true" />
                            </td>
                        </tr>    
                  </table>             
                </div>
                <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width:99%">
                        <colgroup>
                            <col style="width:10px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Topik Edukasi")%></label></td>
                            <td>
                                <asp:TextBox ID="txtPatientEducationGroup" CssClass="txtPatientEducationGroup" runat="server" Width="100%" ReadOnly="true"  />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Form Edukasi")%></label></td>
                            <td>
                                <asp:TextBox ID="txtFormEducationType" CssClass="txtFormEducationType" runat="server" Width="100%" ReadOnly="true"  />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="width:100%">
                                <div id="divFormContent" runat="server" style="height: 450px;overflow-y: auto;"></div>
                            </td>
                        </tr>
                    </table>
                    <div style="width: 99%; height: auto; max-height : 400px; min-height: 300px; border:0px solid #AAA; overflow-y: auto; overflow-x:hidden; padding-left: 10px;">
                        <asp:Repeater ID="rptEducationType" runat="server">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" style="width:98%">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="labelColumn" style="width:250px;vertical-align:top;padding-top:5px">
                                        <input type="hidden" id="hdnGCEducationType" runat="server" value='<%#:Eval("StandardCodeID") %>' />
                                        <%#: Eval("StandardCodeName") %>
                                    </td>
                                    <td>
                                        <div style="padding-left:1px">
                                            <asp:RadioButtonList ID="rblEducationTypeStatus" CssClass="rblEducationTypeStatus" runat="server" RepeatDirection="Horizontal" Enabled="false">
                                                <asp:ListItem Text=" Ya" Value="1"/>
                                                <asp:ListItem Text=" Tidak" Value="0" />
                                            </asp:RadioButtonList>
                                        </div>
                                        <div style="padding-left:5px">
                                            <asp:TextBox ID="txtFreeText" Style="display:none" CssClass="txtFreeText" runat="server" Width="370px" TextMode="Multiline" Rows="2" ReadOnly="true" />
                                        </div>
                                    </td>
                                </tr>                
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                   <table class="tblEntryContent" style="width:99%">
                        <colgroup>
                            <col style="width:10px"/>
                            <col style="width:250px"/>
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tingkat Pemahaman Awal")%></label></td>
                            <td colspan="2">
                                <asp:TextBox ID="txtUnderstandingLevel" CssClass="txtUnderstandingLevel" runat="server" Width="100%" ReadOnly="true"  />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Metode Edukasi")%></label></td>
                            <td colspan="2">
                                <asp:TextBox ID="txtEducationMethod" CssClass="txtEducationMethod" runat="server" Width="100%" ReadOnly="true"  />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Material Edukasi")%></label></td>
                            <td colspan="2">
                                <asp:TextBox ID="txtEducationMaterial" CssClass="txtEducationMaterial" runat="server" Width="100%" ReadOnly="true"  />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Evaluasi Edukasi")%></label></td>
                            <td colspan="2">
                                <asp:TextBox ID="txtEducationEvaluation" CssClass="txtEducationEvaluation" runat="server" Width="100%" ReadOnly="true"  />
                            </td>
                        </tr>
                        <tr style="padding-top: 50px">
                            <td colspan="2" style="width:100%">
                                <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                    <colgroup>
                                        <col style="width:50%" />
                                        <col style="width:50%" />
                                        <col style="width:50%" />
                                        <col style="width:50%" />
                                    </colgroup>
                                    <tr>
                                        <td style="text-align:center ; font-weight: bold">
                                             <%=GetLabel("Edukator")%>
                                        </td>
                                        <td style="text-align:center; font-weight: bold">
                                             <%=GetLabel("Penerima Informasi")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align:top">
                                            <div align="center">
                                                <asp:PlaceHolder ID="plSignature1" runat="server" />
                                            </div>
                                        </td>
                                        <td style="vertical-align:top">
                                                <div align="center">
                                                    <asp:PlaceHolder ID="plSignature2" runat="server" />
                                                </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="tdSaksi1" runat="server" style="text-align:center ; font-weight: bold">
                                             <%=GetLabel("Saksi 1")%>
                                        </td>
                                        <td id="tdSaksi2" runat="server" style="text-align:center; font-weight: bold">
                                             <%=GetLabel("Saksi 2")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align:top">
                                            <div id="divSignature3" runat="server" align="center">
                                                <asp:PlaceHolder ID="plSignature3" runat="server" />
                                            </div>
                                        </td>
                                        <td style="vertical-align:top">
                                                <div id="divSignature4" runat="server" align="center">
                                                    <asp:PlaceHolder ID="plSignature4" runat="server" />
                                                </div>
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


