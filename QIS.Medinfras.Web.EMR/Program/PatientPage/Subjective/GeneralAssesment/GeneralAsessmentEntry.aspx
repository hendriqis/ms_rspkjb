<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master"
    AutoEventWireup="true" CodeBehind="GeneralAsessmentEntry.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.GeneralAsessmentEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>

<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientStatusSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script id="dxis_assessment1" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.core.js")%>' type='text/javascript'></script>
    <script id="dxis_assessment2" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.widget2.js")%>' type='text/javascript'></script>
    <script id="dxis_assessment3" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.accordion.js")%>' type='text/javascript'></script>

    <style type="text/css">
        #listSummary div.containerUl { background-color:transparent; height:250px}
        #listSummary div.containerUl ul  { margin:0; padding:0; margin-left:25px; }
        #listSummary div.containerUl ul:not(:first-child) { margin-top: 10px; }
        #listSummary div.containerUl ul li  { list-style-type:none; font-size: 12px; padding-bottom:5px; }
        #listSummary div.containerUl ul li span  { color:#3E3EE3; }
        #listSummary div.containerUl a        { font-size:12px; color:#3E93E3; cursor: pointer; float: right; margin-right: 20px; }
        #listSummary div.containerUl a:hover  { text-decoration: underline; }
        #listSummary div.headerHistoryContent  { text-align: left; border-bottom:1px groove black; font-size: 13px; margin:0; padding:0; margin-bottom: 5px; }
                      
        #tblEpisodeSummary tr td  { border: 1px solid #6E6E6E; vertical-align:top; }
        #tblEpisodeSummary tr td h5  { background-color: #73BE32; border-bottom:1px groove black; font-weight:bold; font-size: 16px; margin:0; padding:0; }
        #tblEpisodeSummary tr td div.containerUl { height:190px; overflow-y: scroll; }
        #tblEpisodeSummary tr td ul  { margin:0; padding:0; margin-left:25px; }
        #tblEpisodeSummary tr td ul:not(:first-child) { margin-top: 10px; }
        #tblEpisodeSummary tr td ul li  { list-style-type: circle; font-size: 13px; }
        #tblEpisodeSummary tr td ul li span  { color:#3E3EE3; }
        #tblEpisodeSummary tr td a        { font-size:11px; color:#3E93E3; cursor: pointer; float: right; margin-right: 20px; }
        #tblEpisodeSummary tr td a:hover  { text-decoration: underline; }
        
        #rightPanel          { border:1px solid #6E6E6E; width:100%;height:100%; position: relative; }    
        #rightPanel > ul       { margin:0; padding:2px; border-bottom:1px groove black; }
        #rightPanel > ul > li    { list-style-type: none; font-size: 13px; display: inline-block; border: 1px solid #848484; padding: 5px 8px; cursor: pointer; background-color: #95a5a6; }
        #rightPanel > ul > li.selected { background-color: #f39c12; color: White; }
    </style>

    <script type="text/javascript" id="dxss_erpatientstatus">
        $(function () {

            $('#listSummary').show();
            $('#listSummary').accordion({ heightStyle: 'content' });

            setDatePicker('<%=txtServiceDate.ClientID %>');
            $('#<%=txtServiceDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=btnPatientStatusSave.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientStatus', 'mpPatientStatus')) {
                    onCustomButtonClick('save');
                }
            });

            $('#<%=txtChiefComplaint.ClientID %>').keydown(function () {
                $('#<%=hdnIsChanged.ClientID %>').val('1');
            });

            $('#<%=txtChiefComplaint.ClientID %>').focus();

        });

        function onBeforeChangePage() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                var message = "Your record is not saved yet, Do you want to save ?";
                showToastConfirmation(message, function (result) {
                    if (result) {
                        onCustomButtonClick('save');
                        $('#<%=hdnIsChanged.ClientID %>').val('0');
                    }
                });
            }
            else {
                gotoNextPage();
            }
        }
        function onBeforeBackToListPage() {
            if ($('#<%=hdnIsSaved.ClientID %>').val() == '0' && $('#<%=hdnIsChanged.ClientID %>').val() == '1') {
                PromptUserToSave();
            }
            else {
                backToPatientList();
            }
        }

        function PromptUserToSave() {
            var message = "Your record is not saved yet, Do you want to save ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    onCustomButtonClick('save');
                    $('#<%=hdnIsChanged.ClientID %>').val('0');
                }
            });
        }

        function onAfterCustomClickSuccess(type, retval) {
            if ($('#<%=hdnID.ClientID %>').val() == '')
                $('#<%=hdnID.ClientID %>').val(retval);
        }

        $('#<%=btnPatientStatusSave.ClientID %>').click(function () {
            if (IsValid(null, 'fsPatientStatus', 'mpPatientStatus')) {
                onCustomButtonClick('save');
                $('#<%=hdnIsChanged.ClientID %>').val('0');
            }
        });

        function onCboOnsetChanged(s) {
            //            $('#<%=hdnIsChanged.ClientID %>').val('1');
            $txt = $('#<%=txtOnset.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboProvocationChanged(s) {
            //            $('#<%=hdnIsChanged.ClientID %>').val('1');
            $txt = $('#<%=txtProvocation.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboQualityChanged(s) {
            //            $('#<%=hdnIsChanged.ClientID %>').val('1');
            $txt = $('#<%=txtQuality.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboSeverityChanged(s) {
            //            $('#<%=hdnIsChanged.ClientID %>').val('1');
            $txt = $('#<%=txtSeverity.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboTimeChanged(s) {
            //            $('#<%=hdnIsChanged.ClientID %>').val('1');
            $txt = $('#<%=txtTime.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboRelievedByChanged(s) {
            //            $('#<%=hdnIsChanged.ClientID %>').val('1');
            $txt = $('#<%=txtRelievedBy.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }
    </script>
    <div>
        <input type="hidden" runat="server" id="hdnID" value="" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0minute" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1minute" value="00" />
        <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
        <input type="hidden" runat="server" id="hdnIsSaved" value="0" />
        <input type="hidden" runat="server" id="hdnIsChanged" value="0" />
        <fieldset id="mpPatientStatus">
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 60%" />
                    <col style="width: 40%" />
                </colgroup>
                <tr>
                    <td style="padding: 5px; vertical-align: top;">
                        <table class="tblEntryContent" style="width: 100%">
                            <tr>
                                <td class="tdLabel" valign="top" style="width: 130px">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Date and Time")%></label>
                                </td>
                                <td colspan="2">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtServiceDate" Width="120px" CssClass="datepicker" runat="server" />
                                                </td>
                                                <td style="padding-left: 5px">
                                                    <asp:TextBox ID="txtServiceTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                              </tr>
                              <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Subjective")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtChiefComplaint" runat="server" TextMode="MultiLine" Rows="8"
                                        Width="100%" />
                                </td>
                              </tr>                              
                              <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Objective")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtObjective" runat="server" TextMode="MultiLine" Rows="8"
                                        Width="100%" />
                                </td>
                              </tr>
                              <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Assessment")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtAssessment" runat="server" TextMode="MultiLine" Rows="8"
                                        Width="100%" />
                                </td>
                              </tr>
                              <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Planning")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtPlanning" runat="server" TextMode="MultiLine" Rows="8"
                                        Width="100%" />
                                </td>
                              </tr>
                              <tr>
                                <td class="tdLabel">
                                    &nbsp;
                                </td>
                                <td colspan="2">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <td style="width: 50%">
                                            <asp:CheckBox ID="chkAutoAnamnesis" runat="server" Text="Autoanamnesis" Checked="false" />
                                        </td>
                                        <td style="width: 50%">
                                            <asp:CheckBox ID="chkAlloAnamnesis" runat="server" Text="Alloanamnesis / Heteroanamnesis"
                                                Checked="false" />
                                        </td>
                                    </table>
                                </td>
                              </tr>
                              <tr id="trRecordStatus">
                                <td colspan="3">
                                    <table id="tblRecordStatus" runat="server">
                                        <tr>
                                            <td style="vertical-align: bottom;">
                                                <br />
                                                <br />
                                                <br />
                                                <div style="border: 1px solid gray; padding: 2px 2px 5px 5px;">
                                                    <h4 style="background-color: transparent; color: blue; font-weight: bold">
                                                        <%=GetLabel("RECORD STATUS :")%></h4>
                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col width="120px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td class="tdLabel">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Last Saved on")%></label>
                                                            </td>
                                                            <td>
                                                                <label id="lblLastUpdatedDate" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel">
                                                                <label class="lblNormal">
                                                                    <%=GetLabel("Last Saved by")%></label>
                                                            </td>
                                                            <td>
                                                                <label id="lblLastUpdatedBy" runat="server" />
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
                    </td>
                    <td valign="top">
                        <div class="accordion" id="listSummary" style="text-align: left;">
                            <a style="font-size: 14px;"><%= GetLabel("HISTORY OF PRESENT ILLNESS")%></a>
                            <div class="containerUl">
                                <table id="tblHPI" style="width: 98%" runat="server">
                                    <tr>
                                        <td class="tdLabel" style="width: 120px">
                                            <label class="lblNormal">
                                                <%=GetLabel("Location")%></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtLocation" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Onset")%></label>
                                        </td>
                                        <td style="width: 120px">
                                            <dxe:ASPxComboBox runat="server" ID="cboOnset" ClientInstanceName="cboOnset" Width="100%">
                                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboOnsetChanged(s); }" Init="function(s,e){ onCboOnsetChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOnset" CssClass="txtChief" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Provocation")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox runat="server" ID="cboProvocation" ClientInstanceName="cboProvocation"
                                                Width="150px">
                                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboProvocationChanged(s); }"
                                                    Init="function(s,e){ onCboProvocationChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProvocation" CssClass="txtChief" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Quality")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox runat="server" ID="cboQuality" ClientInstanceName="cboQuality"
                                                Width="150px">
                                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboQualityChanged(s); }"
                                                    Init="function(s,e){ onCboQualityChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtQuality" CssClass="txtChief" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Severity")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox runat="server" ID="cboSeverity" ClientInstanceName="cboSeverity"
                                                Width="150px">
                                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboSeverityChanged(s); }"
                                                    Init="function(s,e){ onCboSeverityChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSeverity" CssClass="txtChief" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Time")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox runat="server" ID="cboTime" ClientInstanceName="cboTime" Width="150px">
                                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboTimeChanged(s); }" Init="function(s,e){ onCboTimeChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTime" CssClass="txtChief" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Relieved By")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox runat="server" ID="cboRelievedBy" ClientInstanceName="cboRelievedBy"
                                                Width="150px">
                                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboRelievedByChanged(s); }"
                                                    Init="function(s,e){ onCboRelievedByChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRelievedBy" CssClass="txtChief" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <a style="font-size: 14px;"><%= GetLabel("REVIEW OF SYSTEM")%></a>
                            <div class="containerUl">
                            </div>
                            <a style="font-size: 14px;"><%= GetLabel("ASSESSMENT")%></a>
                            <div class="containerUl">
                            </div>
                            <a style="font-size: 14px;"><%= GetLabel("PLANNING : LABORATORY")%></a>
                            <div class="containerUl">
                            </div>
                            <a style="font-size: 14px;"><%= GetLabel("PLANNING : IMAGING")%></a>
                            <div class="containerUl">
                            </div>
                            <a style="font-size: 14px;"><%= GetLabel("PLANNING : MEDICATION")%></a>
                            <div class="containerUl">
                            </div>
                            <a style="font-size: 14px;"><%= GetLabel("DISCHARGE AND FOLLOW-UP VISIT")%></a>
                            <div class="containerUl">
                                <div class="divContentTitle"><%= GetLabel("Follow-up Visit")%></div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>

        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>      
    </div>
</asp:Content>
