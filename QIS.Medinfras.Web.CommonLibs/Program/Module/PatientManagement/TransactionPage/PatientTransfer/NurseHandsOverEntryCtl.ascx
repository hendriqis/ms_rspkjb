<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NurseHandsOverEntryCtl.ascx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientPage.NurseHandsOverEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

<script type="text/javascript" id="dxss_nursePatientTransferEntryctl">
    $(function () {
        setDatePicker('<%=txtTransferDate.ClientID %>');
        $('#<%=txtTransferDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

        //#region Left Navigation Panel
        $('#leftPageNavPanel ul li').click(function () {
            $('#leftPageNavPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });

        function showContent(contentID) {
            var i, x, tablinks;
            x = document.getElementsByClassName("divPageNavPanel1Content");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(contentID).style.display = "block";
        }
        //#endregion

        $('#leftPageNavPanel ul li').first().click();
    });

    function onCboTransferTypeValueChanged() {
        cboToHealthcareServiceUnit.PerformCallback();
    }

    function onCboToHealthcareServiceUnitValueChanged()
    {
        cboParamedic2.PerformCallback();
    }

    function onAfterSaveRecordPatientPageEntry(value) {
        cbpView.PerformCallback('refresh');
    }

    $('#<%=btnCopyNote.ClientID %>').live('click', function () {
        var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND GCPatientNoteType IN ('X011^011','X011^012') AND GCParamedicMasterType IN ('X019^002','X019^003')";
        openSearchDialog('planningNote', filterExpression, function (value) {
            $('#<%=hdnPatientVisitNoteID.ClientID %>').val(value);
            onSearchPatientVisitNote(value);
        });
    });

    function onSearchPatientVisitNote(value) {
        var filterExpression = "ID = " + value;
        Methods.getObject('GetPatientVisitNoteList', filterExpression, function (result) {
            if (result != null) {
                var situationText = result.SubjectiveText + "\n" + result.ObjectiveText;
                var assessmentText = result.AssessmentText + "\n" + result.PlanningText;
                var recommendationText = result.InstructionText;
                $('#<%=txtSituationText.ClientID %>').val(situationText);
                $('#<%=txtBackgroundText.ClientID %>').val('');
                $('#<%=txtAssessmentText.ClientID %>').val(assessmentText);
                $('#<%=txtRecommendationText.ClientID %>').val(recommendationText);
            }
            else {
                $('#<%=txtSituationText.ClientID %>').val('');
                $('#<%=txtBackgroundText.ClientID %>').val('');
                $('#<%=txtAssessmentText.ClientID %>').val('');
                $('#<%=txtRecommendationText.ClientID %>').val('');
            }
        });
    }

    function onCboToHealthcareServiceUnitEndCallback(s) {
        hideLoadingPanel();
        var value = cboTransferType.GetValue();
        if (value == 'X076^01') {
            cboToHealthcareServiceUnit.SetValue(cboFromHealthcareServiceUnit.GetValue());
            cboToHealthcareServiceUnit.SetEnabled(false);
        }
        else {
            cboToHealthcareServiceUnit.SetEnabled(true);
        }
    }

    function onCbpToHealthcareServiceUnit(s) {
        hideLoadingPanel();
    }

</script>

<div style="height: 450px; overflow-y: hidden; border: 0px">
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnPatientVisitNoteID" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" id="hdnFromHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnToHealthcareServiceUnitID" runat="server" value="" />
    <table style="width:100%" cellpadding="1" cellspacing="0">
        <colgroup>
            <col style="width:180px"/>
            <col style="width:150px"/>
            <col style="width:80px"/>
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tanggal ")%> - <%=GetLabel("Jam ")%></label></td>
            <td colspan="2">
                <table border="0" cellpadding="1" cellspacing="0">
                    <tr>
                        <td>
                            <asp:TextBox ID="txtTransferDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td><asp:TextBox ID="txtTransferTime" Width="80px" CssClass="time" runat="server"/></td>
                    </tr>
                </table>
            </td>
            <td />
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tipe Transfer")%></label></td>
            <td colspan="2">
                <dxe:ASPxComboBox runat="server" ID="cboTransferType" ClientInstanceName="cboTransferType" Width="100%">
                    <ClientSideEvents ValueChanged="function(s,e) { onCboTransferTypeValueChanged(); }" EndCallback="function (s,e) {onCboTransferTypeEndCallback();}" />
                </dxe:ASPxComboBox>
            </td>
            <td>                
            </td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dari Unit - Perawat")%></label></td>
            <td colspan="2">
                <dxe:ASPxComboBox runat="server" ID="cboFromHealthcareServiceUnit" ClientInstanceName="cboFromHealthcareServiceUnit" Width="300px" />
            </td>
            <td><dxe:ASPxComboBox runat="server" ID="cboParamedic" ClientInstanceName="cboParamedic" Width="98%" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Ke Unit - Perawat")%></label></td>
            <td colspan="2">
                <dxe:ASPxComboBox runat="server" ID="cboToHealthcareServiceUnit" ClientInstanceName="cboToHealthcareServiceUnit" Width="300px" OnCallback="cboToHealthcareServiceUnit_Callback">
                    <ClientSideEvents BeginCallback="function() { showLoadingPanel(); }" EndCallback="function() { onCboToHealthcareServiceUnitEndCallback(); }" ValueChanged="function(s,e) { onCboToHealthcareServiceUnitValueChanged(); }" />
                </dxe:ASPxComboBox>
            </td>
            <td>
                <dxe:ASPxComboBox runat="server" ID="cboParamedic2" ClientInstanceName="cboParamedic2" Width="98%" OnCallback="cboParamedic2_Callback">
                    <ClientSideEvents BeginCallback="function() { showLoadingPanel(); }" EndCallback="function() { hideLoadingPanel(); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td />
            <td colspan="3"style="vertical-align:top">
                <input type="button" id="btnCopyNote" runat="server" class="btnCopyNote w3-btn w3-hover-blue" value="Salin Catatan Terintegrasi"
                    style="width: auto; background-color: Red; color: White;" />
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <table border="0" cellpadding="0" cellspacing="1" style="width:100%">
                    <colgroup>
                        <col style="width: 20%" />
                        <col style="width: 80%" />
                    </colgroup>
                    <tr>
                        <td style="vertical-align:top">
                            <div id="leftPageNavPanel" class="w3-border">
                                <ul>
                                    <li contentID="divPage1" title="Situation" class="w3-hover-red">Situation</li>     
                                    <li contentID="divPage2" title="Background" class="w3-hover-red">Background</li>    
                                    <li contentID="divPage3" title="Assessment" class="w3-hover-red">Assessment</li>                                                
                                    <li contentID="divPage4" title="Recommendation" class="w3-hover-red">Recommendation</li>                                                                                                
                                </ul>     
                            </div> 
                        </td>
                        <td style="vertical-align:top">
                            <div id="divPage1" class="divPageNavPanel1Content w3-animate-left" style="display:none;padding-left:22px"> 
                                <table class="tblEntryContent" style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSituationText" runat="server" Width="100%" TextMode="MultiLine" Rows="12" />
                                        </td>
                                    </tr>                                            
                                </table>
                            </div>
                            <div id="divPage2" class="divPageNavPanel1Content w3-animate-left" style="display:none;padding-left:22px"> 
                                <table class="tblEntryContent" style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtBackgroundText" runat="server" Width="100%" TextMode="MultiLine" Rows="12" />
                                        </td>
                                    </tr>                                            
                                </table>
                            </div>
                            <div id="divPage3" class="divPageNavPanel1Content w3-animate-left" style="display:none;padding-left:22px"> 
                                <table class="tblEntryContent" style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtAssessmentText" runat="server" Width="100%" TextMode="MultiLine" Rows="12" />
                                        </td>
                                    </tr>                                            
                                </table>
                            </div>
                            <div id="divPage4" class="divPageNavPanel1Content w3-animate-left" style="display:none;padding-left:22px"> 
                                <table class="tblEntryContent" style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtRecommendationText" runat="server" Width="100%" TextMode="MultiLine" Rows="12" />
                                        </td>
                                    </tr>                                            
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<div>
    <dxcp:ASPxCallbackPanel ID="cbpTransferType" runat="server" Width="100%" ClientInstanceName="cbpTransferType"
        ShowLoadingPanel="false" OnCallback="cbpTransferType_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpTransferTypeEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
