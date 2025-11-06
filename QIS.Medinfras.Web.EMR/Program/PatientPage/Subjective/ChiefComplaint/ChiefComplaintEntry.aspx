<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master"
    AutoEventWireup="true" CodeBehind="ChiefComplaintEntry.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.ChiefComplaintEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientStatusSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erpatientstatus">
        $(function () {
            setDatePicker('<%=txtServiceDate.ClientID %>');
            $('#<%=txtServiceDate.ClientID %>').datepicker('option', 'maxDate', '0');

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
                                        <%=GetLabel("Chief Complaint")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtChiefComplaint" runat="server" TextMode="MultiLine" Rows="16"
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
                        </table>
                    </td>
                    <td valign="top">
                        <table id="tblHPI" style="width: 100%" runat="server">
                            <tr>
                                <td colspan="3">
                                    &nbsp;<br />
                                </td>
                            </tr>
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
                                <td style="width: 150px">
                                    <dxe:ASPxComboBox runat="server" ID="cboOnset" ClientInstanceName="cboOnset" Width="150px">
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
                                                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Last Saved on")%></label></td> 
                                                                <td>
                                                                    <label id="lblLastUpdatedDate" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Last Saved by")%></label></td> 
                                                                <td>
                                                                    <label id="lblLastUpdatedBy"  runat="server"  />
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
                </tr>
            </table>
        </fieldset>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
