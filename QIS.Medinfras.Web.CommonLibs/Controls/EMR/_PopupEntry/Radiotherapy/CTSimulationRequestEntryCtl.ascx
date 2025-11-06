<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CTSimulationRequestEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.CTSimulationRequestEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_radiotheraphyEntryctl">
    $(function () {
        setDatePicker('<%=txtProgramDate.ClientID %>');
        $('#<%=txtProgramDate.ClientID %>').datepicker('option', 'maxDate', '0');

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

        $('#leftPageNavPanel ul li').first().click();
    });

    function onBeforeSaveRecord() {
        return true;
    }

    function onCboGCScanAreaChanged(s) {
        var value = s.GetValue();
    }

    function onAfterSaveAddRecordEntryPopup(result) {
        if (typeof onRefreshControl == 'function')
            onRefreshControl();

        if (typeof onRefreshCTSimulatorViewGrid == 'function')
            onRefreshCTSimulatorViewGrid();
    }
    function onAfterSaveEditRecordEntryPopup(result) {
        if (typeof onRefreshControl == 'function')
            onRefreshControl();

        if (typeof onRefreshCTSimulatorViewGrid == 'function')
            onRefreshCTSimulatorViewGrid();
    }

</script>
<div style="height: auto">
    <input type="hidden" runat="server" id="hdnPopupMRN" value="" />
    <input type="hidden" runat="server" id="hdnPopupVisitID" value="" />
    <input type="hidden" runat="server" id="hdnPopupMedicalNo" value="" />
    <input type="hidden" runat="server" id="hdnPopupPatientName" value="" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnPopupProgramID" value="" />
    <input type="hidden" runat="server" id="hdnPopupParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnFormLayout" value="" />
    <input type="hidden" runat="server" id="hdnFormValues" value="" />
    <input type="hidden" runat="server" id="hdnCombinationTechnique" value="" />
    <input type="hidden" runat="server" id="hdnCombinationTechniqueCode" value="" />
    <table class="tblEntryContent">
        <colgroup>
            <col style="width: 25%" />
            <col style="width: 75%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPageNavPanel" class="w3-border">
                    <ul>
                        <li contentid="divPage1" title="Permintaan Simulasi" class="w3-hover-red">Permintaan Simulasi</li>
                        <li contentid="divPage2" title="Catatan Tambahan" class="w3-hover-red">Catatan Tambahan</li>
                     </ul>
                </div>
            </td>
            <td style="vertical-align: top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No. RM / Nama Pasien")%></label>                            
                            </td>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtMedicalNo" Width="100px" Enabled="false" runat="server" Style="text-align: center" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPatientName" Width="250px" runat="server" Enabled="false"
                                                Style="text-align: left" />
                                        </td> 
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal Permintaan")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtProgramDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProgramTime" Width="80px" CssClass="time" runat="server"
                                                Style="text-align: center" />
                                        </td> 
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal">
                                    <%=GetLabel("Diagnosis Pasien")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDiagnosisInfo" runat="server" TextMode="MultiLine" Rows="5"
                                    Width="99%" ReadOnly="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal">
                                    <%=GetLabel("Stadium")%></label>
                            </td>   
                            <td>
                                <asp:TextBox ID="txtStagingInfo" Width="100%" runat="server" Enabled="false"
                                    Style="text-align: left" />
                            </td>                         
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Permintaan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboGCSimulationRequestType" ClientInstanceName="cboGCSimulationRequestType"
                                    Width="50%" ToolTip="Jenis Permintaan">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Scan Area")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboGCScanArea" ClientInstanceName="cboGCScanArea"
                                    Width="50%" ToolTip="Scan Area">
                                    <ClientSideEvents ValueChanged="function(s){ onCboGCScanAreaChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Batas Scan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtScanBorder" Width="180px" runat="server"  /> 
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"></td>
                            <td>
                                <asp:RadioButtonList ID="rblIsUsingContrast" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text=" Kontras" Value="1" />
                                    <asp:ListItem Text=" Non Kontras" Value="0"  />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan Khusus")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="15"
                                    Width="100%" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
