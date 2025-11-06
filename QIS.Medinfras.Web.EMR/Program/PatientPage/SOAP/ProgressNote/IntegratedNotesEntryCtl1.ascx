<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IntegratedNotesEntryCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.IntegratedNotesEntryCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_IntegratedNotesEntryCtl1">
    $(function () {
        setDatePicker('<%=txtNoteDate.ClientID %>');
        $('#<%=txtNoteDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
        $('#lblPhysicianNoteID').removeClass('lblLink');

        var noteMode = $('#<%=rblNoteMode.ClientID %>').val();

        if (noteMode == "") 
            noteMode = "1";

        if (noteMode != "2")
            $("#<%=txtNoteText.ClientID %>").focus();
        else
            $("#<%=txtSubjectiveText.ClientID %>").focus();

        //#region Text Box event
        $('#<%:txtSubjectiveText.ClientID %>').blur(function () {
            onTxtSubjectiveTextChanged($(this).val());
        });

        $('#<%:txtObjectiveText.ClientID %>').blur(function () {
            onTxtObjectiveTextChanged($(this).val());
        });

        $('#<%:txtAssessmentText.ClientID %>').blur(function () {
            onTxtAssessmentTextChanged($(this).val());
        });

        $('#<%:txtPlanningText.ClientID %>').blur(function () {
            onTxtPlanningTextChanged($(this).val());
        });

        $('#<%:txtInstructionText.ClientID %>').blur(function () {
            onTxtInstructionTextChanged($(this).val());
        });
        //#endregion
    });

    //#region Subjective
    $('#lblSubjective').die('click');
    $('#lblSubjective').live('click', function (evt) {
        var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND GCTextTemplateGroup ='X058^03'";
        openSearchDialog('physicianText', filterExpression, function (value) {
            onTxtSubjectiveTextChanged(value+";");
        });
    });

    function onTxtSubjectiveTextChanged(value) {
        if (value.slice(-1) == ";") {
            var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^03'";
            Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                if (obj != null)
                    if ($('#<%=txtSubjectiveText.ClientID %>').val() != '') {
                        var message = "Are you sure to replace the Subjective Text from your template text ?";
                        displayConfirmationMessageBox("SOAP : AUTO TEXT", message, function (result) {
                            if (result) {
                                $('#<%=txtSubjectiveText.ClientID %>').val(obj.TemplateText);
                            }
                        });
                    }
                    else {
                        $('#<%=txtSubjectiveText.ClientID %>').val(obj.TemplateText);
                    }
            });
        }
    }
    //#endregion

    //#region Objective
    $('#lblObjective').die('click');
    $('#lblObjective').live('click', function (evt) {
        var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND GCTextTemplateGroup ='X058^04' AND IsDeleted = 0";
        openSearchDialog('physicianText', filterExpression, function (value) {
            onTxtObjectiveTextChanged(value+";");
        });
    });

    function onTxtObjectiveTextChanged(value) {
        if (value.slice(-1) == ";") {
            var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^04' AND IsDeleted = 0";
            Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                if (obj != null)
                    if ($('#<%=txtObjectiveText.ClientID %>').val() != '') {
                        var message = "Are you sure to replace the Objective Text from your template text ?";
                        displayConfirmationMessageBox("SOAP : AUTO TEXT", message, function (result) {
                            if (result) {
                                $('#<%=txtObjectiveText.ClientID %>').val(obj.TemplateText);
                            }
                        });
                    }
                    else {
                        $('#<%=txtObjectiveText.ClientID %>').val(obj.TemplateText);
                    }
            });
        }
    }
    //#endregion

    //#region Assessment
    $('#lblAssessment').die('click');
    $('#lblAssessment').live('click', function (evt) {
        var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND GCTextTemplateGroup ='X058^05' AND IsDeleted = 0";
        openSearchDialog('physicianText', filterExpression, function (value) {
            onTxtAssessmentTextChanged(value+";");
        });
    });

    function onTxtAssessmentTextChanged(value) {
        if (value.slice(-1) == ";") {
            var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^05' AND IsDeleted = 0";
            Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                if (obj != null)
                    if ($('#<%=txtAssessmentText.ClientID %>').val() != '') {
                        var message = "Are you sure to replace the Assessment Text from your template text ?";
                        displayConfirmationMessageBox("SOAP : AUTO TEXT", message, function (result) {
                            if (result) {
                                $('#<%=txtAssessmentText.ClientID %>').val(obj.TemplateText);
                            }
                        });
                    }
                    else {
                        $('#<%=txtAssessmentText.ClientID %>').val(obj.TemplateText);
                    }
            });
        }
    }
    //#endregion

    //#region Planning
    $('#lblPlanning').die('click');
    $('#lblPlanning').live('click', function (evt) {
        var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND GCTextTemplateGroup ='X058^06' AND IsDeleted = 0";
        openSearchDialog('physicianText', filterExpression, function (value) {
            onTxtPlanningTextChanged(value+";");
        });
    });

    function onTxtPlanningTextChanged(value) {
        if (value.slice(-1) == ";") {
            var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^06' AND IsDeleted = 0";
            Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                if (obj != null)
                    if ($('#<%=txtPlanningText.ClientID %>').val() != '') {
                        var message = "Are you sure to replace the Planning Text from your template text ?";
                        displayConfirmationMessageBox("SOAP : AUTO TEXT", message, function (result) {
                            if (result) {
                                $('#<%=txtPlanningText.ClientID %>').val(obj.TemplateText);
                            }
                        });
                    }
                    else {
                        $('#<%=txtPlanningText.ClientID %>').val(obj.TemplateText);
                    }
            });
        }
    }
    //#endregion

    //#region Instruction
    $('#lblInstruction').die('click');
    $('#lblInstruction').live('click', function (evt) {
        var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND GCTextTemplateGroup ='X058^07' AND IsDeleted = 0";
        openSearchDialog('physicianText', filterExpression, function (value) {
            onTxtInstructionTextChanged(value+";");
        });
    });

    function onTxtInstructionTextChanged(value) {
        if (value.slice(-1) == ";") {
            var filterExpression = " UserID = " + <%=HttpUtility.HtmlEncode(GetUserID())%> + " AND TemplateCode = '" + value.slice(0, -1) + "' AND GCTextTemplateGroup ='X058^07' AND IsDeleted = 0";
            Methods.getObject('GetvPhysicianTextList', filterExpression, function (obj) {
                if (obj != null)
                    if ($('#<%=txtInstructionText.ClientID %>').val() != '') {
                        var message = "Are you sure to replace the Instruction Text from your template text ?";
                        displayConfirmationMessageBox("SOAP : AUTO TEXT", message, function (result) {
                            if (result) {
                                $('#<%=txtInstructionText.ClientID %>').val(obj.TemplateText);
                            }
                        });
                    }
                    else {
                        $('#<%=txtInstructionText.ClientID %>').val(obj.TemplateText);
                    }
            });
        }
    }
    //#endregion

    $('#btnAddTemplate1.imgLink').live('click', function () {
        if ($('#<%=txtSubjectiveText.ClientID %>').val() != '') {
            displayMessageBox("SAVE TEMPLATE", "Sorry, this feature is not available yet !");
        }           
    });

    $('#<%=btnCopyNote.ClientID %>').live('click', function () {
        var filterExpression = "<%:GetFilterExpression()%>";
        openSearchDialog('planningNote', filterExpression, function (value) {
            $('#<%=hdnPatientVisitNoteID.ClientID %>').val(value);
            onSearchPatientVisitNote(value);
        });
    });

    function onSearchPatientVisitNote(value) {
        var filterExpression = "ID = " + value;
        Methods.getObject('GetPatientVisitNoteList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtSubjectiveText.ClientID %>').val(result.SubjectiveText);
                $('#<%=txtObjectiveText.ClientID %>').val(result.ObjectiveText);
                $('#<%=txtAssessmentText.ClientID %>').val(result.AssessmentText);
                $('#<%=txtPlanningText.ClientID %>').val(result.PlanningText);
                $('#<%=txtInstructionText.ClientID %>').val(result.InstructionText);
            }
            else {
                $('#<%=txtSubjectiveText.ClientID %>').val('');
                $('#<%=txtObjectiveText.ClientID %>').val('');
                $('#<%=txtAssessmentText.ClientID %>').val('');
                $('#<%=txtPlanningText.ClientID %>').val('');
                $('#<%=txtInstructionText.ClientID %>').val('');
            }
        });
    }

    $('#lblLookupTTV.lblLink').live('click', function () {
        var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val();
        openSearchDialog('vvitalsigndt2', filterExpression, function (value) {
            $('#<%=hdnVitalSignID.ClientID %>').val(value);
            onSearchVitalSignValue(value);
        });
    });
    
    function onSearchVitalSignValue(value) {
        var objText = $('#<%=txtObjectiveText.ClientID %>').val();
        var filterExpression = "ID = " + value;
        Methods.getObject('GetvVitalSignDt2List', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtObjectiveText.ClientID %>').val(objText + result.cfVitalSignValue);
            }
            else {
                $('#<%=txtObjectiveText.ClientID %>').val('');
            }
        });
    }

    $('#lblPhysicianNoteID.lblLink').live('click', function () {
        var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND GCPatientNoteType IN ('X011^002','X011^010','X011^011')";
        openSearchDialog('planningNote', filterExpression, function (value) {
            onTxtPlanningNoteChanged(value);
        });
    });

    function onTxtPlanningNoteChanged(value) {
        var filterExpression = "ID = " + value;
        Methods.getObject('GetPatientVisitNoteList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPlanningNoteID.ClientID %>').val(result.ID);
                $('#<%=txtPatientVisitNoteText.ClientID %>').val(result.NoteText);
            }
            else {
                $('#<%=hdnPlanningNoteID.ClientID %>').val('');
                $('#<%=txtPatientVisitNoteText.ClientID %>').val('');
            }
        });
    }

    function onCboPhysicianInstructionSourceChanged(s) {
        if (s.GetValue() != null && s.GetValue().indexOf('^01') > -1) {
            $('#lblPhysicianNoteID').addClass('lblLink');
        }
        else {
            $('#lblPhysicianNoteID').removeClass('lblLink');
            $('#<%=hdnPlanningNoteID.ClientID %>').val('');
            $('#<%=txtPatientVisitNoteText.ClientID %>').val('');
        }
    }

    function onCboPhysicianChanged(s) {
        if (s.GetValue() != null) {
            $('#<%=hdnParamedicID.ClientID %>').val(s.GetValue());
        }
        else
            $('#<%=hdnParamedicID.ClientID %>').val('');
    }

    $('#<%=rblNoteMode.ClientID %> input').change(function () {
        ToggleNoteMode($(this).val());
    });

    function ToggleNoteMode(param) {
        if (param != '2') {
            $('#<%=trSOAPINote.ClientID %>').attr('style', 'display:none');
            $('#<%=tdCopyButton.ClientID %>').attr('style', 'display:none');
            $('#<%=trFreeTextNote.ClientID %>').removeAttr('style');
        }
        else {
            $('#<%=trSOAPINote.ClientID %>').removeAttr('style');
            $('#<%=tdCopyButton.ClientID %>').removeAttr('style');
            $('#<%=trFreeTextNote.ClientID %>').attr('style', 'display:none');
        }
    }
</script>
<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnPlanningNoteID" value="" />
    <input type="hidden" value="" id="hdnPatientVisitNoteID" runat="server" />
    <input type="hidden" value="" id="hdnVitalSignID" runat="server" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnUserLoginParamedicID" value="0" />
    <input type="hidden" runat="server" id="hdnIsAllowFreeTextMode" value="0" />
    <input type="hidden" runat="server" id="hdnFilterParameter" value="0" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table class="tblEntryContent" style="width: 100%">
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
                                <%=GetLabel("Jam ")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoteDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoteTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Dokter")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboParamedicID" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="Label1">
                                <%=GetLabel("Mode")%></label>
                        </td>
                        <td colspan="2">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblNoteMode" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text=" Free Text" Value="1" Selected="True" />
                                            <asp:ListItem Text=" SOAPI" Value="2" />
                                        </asp:RadioButtonList>
                                    </td>
                                    <td id="tdCopyButton" runat="server" style="vertical-align: top;">
                                        <input type="button" id="btnCopyNote" runat="server" value="Salin SOAP" class="btnCopyNote w3-btn w3-hover-blue"
                                            style="width: 100px; background-color: Red; color: White;" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trFreeTextNote" runat="server">
                        <td colspan="3">
                            <table border="0" cellpadding="0" cellspacing="1" style="width: 100%">
                                <col style="width: 180px" />
                                <col />
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <label class="lblNormal">
                                            <%=GetLabel("Catatan Dokter") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNoteText" runat="server" Width="100%" TextMode="Multiline" Height="150px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="margin-bottom: 10px; vertical-align: top">
                                        <label class="lblNormal">
                                            <%=GetLabel("Instruksi")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInstructionText2" Width="100%" runat="server" TextMode="MultiLine"
                                            Rows="2" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trSOAPINote" style="display: none" runat="server">
                        <td colspan="3">
                            <table border="0" cellpadding="0" cellspacing="1">
                                <colgroup>
                                    <col style="width: 195px" />
                                    <col style="width: 150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="font-weight: bold; padding-top: 5px; vertical-align: top;">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <td class="tdLabel" valign="top" style="width: 120px">
                                                <label class="lblMandatory lblLink" id="lblSubjective" title="Pilih Template Text : Subjective">
                                                    <%=GetLabel("Subjective")%></label>
                                            </td>
                                            <td style="display: none">
                                                <img class="imgLink" id="btnAddTemplate1" title='<%=GetLabel("Add to My Template")%>'
                                                    src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" />
                                            </td>
                                        </table>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtSubjectiveText" runat="server" Width="648px" TextMode="MultiLine"
                                            Rows="4" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="font-weight: bold; margin-bottom: 10px; vertical-align: top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <td class="tdLabel" valign="top" style="width: 150px">
                                                <label class="lblMandatory lblLink" id="lblObjective" title="Pilih Template Text : Objective">
                                                    <%=GetLabel("Objective")%></label>
                                                <br />
                                                <label class="lblLink" id="lblLookupTTV">
                                                    <%=GetLabel("Lookup Tanda Vital")%></label>
                                            </td>
                                            <td style="display: none">
                                                <img class="imgLink" id="btnAddTemplate2" title='<%=GetLabel("Add to My Template")%>'
                                                    src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" />
                                            </td>
                                        </table>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtObjectiveText" Width="648px" runat="server" TextMode="MultiLine"
                                            Rows="4" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="font-weight: bold; margin-bottom: 10px; vertical-align: top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <td class="tdLabel" valign="top" style="width: 120px">
                                                <label class="lblMandatory lblLink" id="lblAssessment" title="Pilih Template Text : Assessment">
                                                    <%=GetLabel("Assessment")%></label>
                                            </td>
                                            <td style="display: none">
                                                <img class="imgLink" id="btnAddTemplate3" title='<%=GetLabel("Add to My Template")%>'
                                                    src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" />
                                            </td>
                                        </table>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAssessmentText" Width="648px" runat="server" TextMode="MultiLine"
                                            Rows="4" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="font-weight: bold; margin-bottom: 10px; vertical-align: top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <td class="tdLabel" valign="top" style="width: 120px">
                                                <label class="lblMandatory lblLink" id="lblPlanning" title="Pilih Template Text : Planning">
                                                    <%=GetLabel("Planning")%></label>
                                            </td>
                                            <td style="display: none">
                                                <img class="imgLink" id="btnAddTemplate4" title='<%=GetLabel("Add to My Template")%>'
                                                    src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" />
                                            </td>
                                        </table>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtPlanningText" Width="648px" runat="server" TextMode="MultiLine"
                                            Rows="4" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" style="font-weight: bold; margin-bottom: 10px; vertical-align: top">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <td class="tdLabel" valign="top" style="width: 120px">
                                                <label class="lblMandatory lblLink" id="lblInstruction" title="Pilih Template Text : Instruction">
                                                    <%=GetLabel("Instruksi")%></label>
                                            </td>
                                            <td style="display: none">
                                                <img class="imgLink" id="btnAddTemplate5" title='<%=GetLabel("Add to My Template")%>'
                                                    src='<%= ResolveUrl("~/Libs/Images/button/vitalsign.png")%>' alt="" />
                                            </td>
                                        </table>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtInstructionText" Width="648px" runat="server" TextMode="MultiLine"
                                            Rows="4" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trRMOPhysician1" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Cara Pemberian Instruksi")%></label>
                        </td>
                        <td colspan="2">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <colgroup>
                                    <col style="width: 200px" />
                                    <col style="width: 100px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboPhysicianInstructionSource" ClientInstanceName="cboPhysicianInstructionSource"
                                            runat="server" Width="300px">
                                            <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPhysicianInstructionSourceChanged(s); }"
                                                Init="function(s,e){ onCboPhysicianInstructionSourceChanged(s); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsWritten" runat="server" Checked="false" />
                                        <%:GetLabel("Tulis")%>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsReadback" runat="server" Checked="false" />
                                        <%:GetLabel("Baca")%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trRMOPhysician2" runat="server">
                        <td class="tdLabel" style="vertical-align: top">
                            <label class="lblLink" id="lblPhysicianNoteID">
                                <%=GetLabel("Instruksi Dokter")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPatientVisitNoteText" Width="100%" Height="50px" runat="server"
                                TextMode="MultiLine" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr id="trRMOPhysician3" runat="server">
                        <td class="tdLabel">
                            <asp:CheckBox ID="chkIsNeedConfirmation" runat="server" Checked="false" />
                            <%:GetLabel("Perlu Konfirmasi Dokter")%>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboPhysician" ClientInstanceName="cboPhysician" runat="server"
                                Width="300px">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPhysicianChanged(s); }"
                                    Init="function(s,e){ onCboPhysicianChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr id="trRMOPhysician4" runat="server">
                        <td>
                            <asp:CheckBox ID="chkIsNeedNotification" runat="server" Checked="false" />
                            <%:GetLabel("Kirim Notifikasi Ke Unit")%>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" runat="server"
                                Width="300px">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr id="trRMOPhysician5" runat="server">
                        <td colspan="3">
                            <asp:CheckBox ID="chkIsRMOHandsover" runat="server" Checked="false" />
                            <%:GetLabel("Catatan Hand Over Dokter Jaga")%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
