<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master"
    AutoEventWireup="true" CodeBehind="PsychiatryStatusEntry.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PsychiatryStatusEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientStatusSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erpatientstatus">
        $(function () {
            setDatePicker('<%=txtStatusDate.ClientID %>');
            $('#<%=txtStatusDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=btnPatientStatusSave.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientStatus', 'mpPatientStatus')) {
                    onCustomButtonClick('save');
                }
            });

            $('#<%=txtGeneralAppearance.ClientID %>').focus();
        });

        function onAfterCustomClickSuccess(type, retval) {
            if ($('#<%=hdnID.ClientID %>').val() == '')
                $('#<%=hdnID.ClientID %>').val(retval);
        }

        $('#<%=btnPatientStatusSave.ClientID %>').click(function () {
            if (IsValid(null, 'fsPatientStatus', 'mpPatientStatus'))
                onCustomButtonClick('save');
        });


        function onCboSpeechContactChanged(s) {
            $txt = $('#<%=txtSpeechContactText.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboConsciousnessChanged(s) {
            $txt = $('#<%=txtConsciousness.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboTimeOrientationChanged(s) {
            $txt = $('#<%=txtTimeOrientation.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboPlaceOrientationChanged(s) {
            $txt = $('#<%=txtPlaceOrientation.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboPersonOrientationChanged(s) {
            $txt = $('#<%=txtPersonOrientation.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboMoodyLevelChanged(s) {
            $txt = $('#<%=txtMoodyLevel.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboThoughtProcessFormChanged(s) {
            $txt = $('#<%=txtThoughtProcessForm.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboThoughtProcessFlowChanged(s) {
            $txt = $('#<%=txtThoughtProcessFlow.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboThoughtProcessContentChanged(s) {
            $txt = $('#<%=txtThoughtProcessContent.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboPerceptionChanged(s) {
            $txt = $('#<%=txtPerception.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboVolitionChanged(s) {
            $txt = $('#<%=txtVolition.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }

        function onCboPsychomotorChanged(s) {
            $txt = $('#<%=txtPsychomotor.ClientID %>');
            if (s.GetValue() != null && s.GetValue().indexOf('^999') > -1)
                $txt.show();
            else
                $txt.hide();
        }
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
                <colgroup>
                    <col style="width: 50%" />
                    <col style="width: 50%" />
                </colgroup>
                <tr>
                    <td style="padding: 5px; vertical-align: top;">
                        <table class="tblEntryContent" style="width: 100%">
                            <tr>
                                <td class="tdLabel" valign="top" style="width: 140px" >
                                    <label class="lblMandatory">
                                        <%=GetLabel("Date and Time")%></label>
                                </td>
                                <td colspan="2">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtStatusDate" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                            <td style="padding-left:5px">
                                                <asp:TextBox ID="txtStatusTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("General Appearance")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtGeneralAppearance" runat="server" TextMode="MultiLine" Rows="16"
                                        Width="100%" />
                                </td>
                            </tr>                
                        </table>
                    </td>
                    <td valign="top">
                        <table id="tblHPI" style="width: 100%" runat="server">
                            <tr><td colspan="3">&nbsp;<br /></td></tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Speech Contact")%></label>
                                </td>
                                <td style="width:140px" >
                                    <dxe:ASPxComboBox runat="server" ID="cboSpeechContact" ClientInstanceName="cboSpeechContact" Width="150px">
                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboSpeechContactChanged(s); }" Init="function(s,e){ onCboSpeechContactChanged(s); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSpeechContactText" CssClass="txtChief" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Consciousness")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboConsciousness" ClientInstanceName="cboConsciousness"
                                        Width="150px">
                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboConsciousnessChanged(s); }"
                                            Init="function(s,e){ onCboConsciousnessChanged(s); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConsciousness" CssClass="txtChief" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Orientation - Time")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboTimeOrientation" ClientInstanceName="cboTimeOrientation"
                                        Width="150px">
                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboTimeOrientationChanged(s); }"
                                            Init="function(s,e){ onCboTimeOrientationChanged(s); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTimeOrientation" CssClass="txtChief" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Orientation - Place")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboPlaceOrientation" ClientInstanceName="cboPlaceOrientation"
                                        Width="150px">
                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPlaceOrientationChanged(s); }"
                                            Init="function(s,e){ onCboPlaceOrientationChanged(s); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPlaceOrientation" CssClass="txtChief" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Orientation - Person")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboPersonOrientation" ClientInstanceName="cboPersonOrientation" Width="150px">
                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPersonOrientationChanged(s); }" Init="function(s,e){ onCboPersonOrientationChanged(s); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPersonOrientation" CssClass="txtChief" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Moody Level")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboMoodyLevel" ClientInstanceName="cboMoodyLevel"
                                        Width="150px">
                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboMoodyLevelChanged(s); }"
                                            Init="function(s,e){ onCboMoodyLevelChanged(s); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMoodyLevel" CssClass="txtChief" Width="100%" runat="server" />
                                </td>
                            </tr>  
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Thought Process - Form")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboThoughtProcessForm" ClientInstanceName="cboThoughtProcessForm"
                                        Width="150px">
                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboThoughtProcessFormChanged(s); }"
                                            Init="function(s,e){ onCboThoughtProcessFormChanged(s); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtThoughtProcessForm" CssClass="txtChief" Width="100%" runat="server" />
                                </td>
                            </tr>  
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Thought Process - Flow")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboThoughtProcessFlow" ClientInstanceName="cboThoughtProcessFlow"
                                        Width="150px">
                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboThoughtProcessFlowChanged(s); }"
                                            Init="function(s,e){ onCboThoughtProcessFlowChanged(s); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtThoughtProcessFlow" CssClass="txtChief" Width="100%" runat="server" />
                                </td>
                            </tr>  
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Thought Process - Content")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboThoughtProcessContent" ClientInstanceName="cboThoughtProcessContent"
                                        Width="150px">
                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboThoughtProcessContentChanged(s); }"
                                            Init="function(s,e){ onCboThoughtProcessContentChanged(s); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtThoughtProcessContent" CssClass="txtChief" Width="100%" runat="server" />
                                </td>
                            </tr>  
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Perception")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboPerception" ClientInstanceName="cboPerception"
                                        Width="150px">
                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPerceptionChanged(s); }"
                                            Init="function(s,e){ onCboPerceptionChanged(s); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPerception" CssClass="txtChief" Width="100%" runat="server" />
                                </td>
                            </tr>  
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Volition")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboVolition" ClientInstanceName="cboVolition"
                                        Width="150px">
                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboVolitionChanged(s); }"
                                            Init="function(s,e){ onCboVolitionChanged(s); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVolition" CssClass="txtChief" Width="100%" runat="server" />
                                </td>
                            </tr>  
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Psychomotor")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboPsychomotor" ClientInstanceName="cboPsychomotor"
                                        Width="150px">
                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPsychomotorChanged(s); }"
                                            Init="function(s,e){ onCboPsychomotorChanged(s); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPsychomotor" CssClass="txtChief" Width="100%" runat="server" />
                                </td>
                            </tr> 
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>