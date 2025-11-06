<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewProcedureReportCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ViewProcedureReportCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_ViewProcedureReportCtl">
    $(function () {
        $('#<%=grdParamedicTeamView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdParamedicTeamView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnOrderDtParamedicTeamID.ClientID %>').val($(this).find('.keyField').html());
        });
        $('#<%=grdParamedicTeamView.ClientID %> tr:eq(1)').click();


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

        $('#lblPhysicianNoteID').removeClass('lblLink');

    });

    $('#<%=chkIsHasProcedureComplication.ClientID %>').die('change');
    $('#<%=chkIsHasProcedureComplication.ClientID %>').live('change', function () {
        var isChecked = $(this).is(":checked");

        if ($(this).is(':checked')) {
            $('#<%=chkIsHasProcedureHemorrhage.ClientID %>').removeAttr("disabled");
            $('#<%=txtProcedurePainIndex.ClientID %>').removeAttr("disabled");
            $('#<%=txtAnesthesiaComplicationRemarks.ClientID %>').removeAttr("disabled");
            $('#<%=txtProcedureComplicationRemarks.ClientID %>').removeAttr("disabled");
        }
        else {
            $('#<%=chkIsHasProcedureHemorrhage.ClientID %>').attr("disabled", "disabled");
            $('#<%=chkIsHasProcedureHemorrhage.ClientID %>').prop("checked", false);
            cboGCHemorrhage1.SetEnabled(false);
            cboGCHemorrhage1.SetValue("");
            $('#<%=txtProcedurePainIndex.ClientID %>').attr("disabled", "disabled");
            $('#<%=txtProcedurePainIndex.ClientID %>').val('');
            $('#<%=txtAnesthesiaComplicationRemarks.ClientID %>').attr("disabled", "disabled");
            $('#<%=txtAnesthesiaComplicationRemarks.ClientID %>').val('');
            $('#<%=txtProcedureComplicationRemarks.ClientID %>').attr("disabled", "disabled");
            $('#<%=txtProcedureComplicationRemarks.ClientID %>').val('');
        }
    });

    $('#<%=chkIsHasProcedureHemorrhage.ClientID %>').die('change');
    $('#<%=chkIsHasProcedureHemorrhage.ClientID %>').live('change', function () {
        var isChecked = $(this).is(":checked");
        if ($(this).is(':checked')) {
            cboGCHemorrhage1.SetEnabled(true);
        }
        else {
            cboGCHemorrhage1.SetEnabled(false);
            cboGCHemorrhage1.SetValue("");
        }
    });

    $('#<%=chkIsHasApplicatorReleaseComplication.ClientID %>').die('change');
    $('#<%=chkIsHasApplicatorReleaseComplication.ClientID %>').live('change', function () {
        var isChecked = $(this).is(":checked");

        if ($(this).is(':checked')) {
            $('#<%=chkIsHasApplicatorReleaseHemorrhage.ClientID %>').removeAttr("disabled");
            $('#<%=txtPainIndex.ClientID %>').removeAttr("disabled");
            $('#<%=txtApplicatorReleaseComplicationRemarks.ClientID %>').removeAttr("disabled");
        }
        else {
            $('#<%=chkIsHasApplicatorReleaseHemorrhage.ClientID %>').attr("disabled", "disabled");
            $('#<%=chkIsHasApplicatorReleaseHemorrhage.ClientID %>').prop("checked", false);
            cboGCApplicatorReleaseHemorrhage.SetEnabled(false);
            cboGCApplicatorReleaseHemorrhage.SetValue("");
            $('#<%=txtPainIndex.ClientID %>').attr("disabled", "disabled");
            $('#<%=txtPainIndex.ClientID %>').val('');
            $('#<%=txtApplicatorReleaseComplicationRemarks.ClientID %>').attr("disabled", "disabled");
            $('#<%=txtApplicatorReleaseComplicationRemarks.ClientID %>').val('');
        }
    });

    $('#<%=chkIsHasApplicatorReleaseHemorrhage.ClientID %>').die('change');
    $('#<%=chkIsHasApplicatorReleaseHemorrhage.ClientID %>').live('change', function () {
        var isChecked = $(this).is(":checked");
        if ($(this).is(':checked')) {
            cboGCApplicatorReleaseHemorrhage.SetEnabled(true);
        }
        else {
            cboGCApplicatorReleaseHemorrhage.SetEnabled(false);
            cboGCApplicatorReleaseHemorrhage.SetValue("");
        }
    });


    //#region ParamedicTeam

    var pageCount2 = parseInt('<%=gridParamedicTeamPageCount %>');
    $(function () {
        setPaging($("#paramedicTeamPaging"), pageCount2, function (page) {
            cbpParamedicTeamView.PerformCallback('changepage|' + page);
        });
    });

    function oncbpParamedicTeamViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdParamedicTeamView.ClientID %> tr:eq(1)').click();

            setPaging($("#paramedicPaging"), pageCount2, function (page) {
                cbpParamedicTeamView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdParamedicTeamView.ClientID %> tr:eq(1)').click();
    }
    //#endregion

</script>

<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnProgramID" value="" />
    <input type="hidden" runat="server" id="hdnVisitDepartmentID" value="" />
    <input type="hidden" runat="server" id="hdnPopupVisitID" value="" />
    <input type="hidden" runat="server" id="hdnPopupMRN" value="" />
    <input type="hidden" runat="server" id="hdnPopupMedicalNo" value="" />
    <input type="hidden" runat="server" id="hdnPopupTestOrderID" value="" />
    <input type="hidden" runat="server" id="hdnPopupParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnPopupParamedicTeamID" value="" />
    <input type="hidden" runat="server" id="hdnPopupPatientName" value="" />
    <input type="hidden" runat="server" id="hdnTotalFraction" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowFreeTextMode" value="0" />
    <input type="hidden" runat="server" id="hdnOrderDtParamedicTeamID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 20%" />
            <col style="width: 80%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPageNavPanel" class="w3-border">
                    <ul>
                        <li contentid="divPage1" title="Jadwal Kamar Tindakan" class="w3-hover-red">Informasi Tindakan</li>
                        <li contentid="divPage2" title="Prosedur Tindakan" class="w3-hover-red">Prosedur Tindakan</li>
                        <li contentid="divPage3" title="Team Pelaksana" class="w3-hover-red">Team Pelaksana</li>
                        <li contentid="divPage4" title="Komplikasi Saat Tindakan" class="w3-hover-red">Komplikasi Saat Tindakan</li>
                        <li contentid="divPage5" title="Komplikasi Saat Pelepasan Aplikator" class="w3-hover-red">Komplikasi Saat Pelepasan Aplikator</li>
                    </ul>
                </div>
            </td>
            <td style="vertical-align: top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 170px" />
                            <col style="width: 150px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal">
                                    <%=GetLabel("Diagnosis Pasien")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtDiagnosisInfo" runat="server" TextMode="MultiLine" Rows="3"
                                    Width="100%" ReadOnly="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblNormal">
                                    <%=GetLabel("Fraksi Ke-")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFractionNo" Width="60px" CssClass="number" runat="server" ReadOnly="True"/>
                            </td>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblNormal">
                                    <%=GetLabel("Jumlah Fraksi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalFraction" Width="60px" CssClass="number" runat="server" ReadOnly="True"/>
                            </td>
                        </tr> 
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam Laporan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReportDate" Width="120px" runat="server" ReadOnly="True" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtReportTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" ReadOnly="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam Tindakan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtStartDate" Width="120px" runat="server" ReadOnly="True" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtStartTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"
                                    MaxLength="5" ReadOnly="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam Selesai")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEndDate" Width="120px" runat="server" ReadOnly="True" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtEndTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"
                                    MaxLength="5" ReadOnly="True" />
                            </td>
                        </tr> 
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Lama Tindakan") %>
                                </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDuration" Width="60px" CssClass="number" runat="server" ReadOnly="True" />
                                menit
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Pembuat Laporan")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboParamedicID" Width="100%" runat="server" Enabled="False" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Aplikator")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox runat="server" ID="cboGCApplicatorType" ClientInstanceName="cboGCApplicatorType"
                                    Width="100%" ToolTip="Aplikator Brakiterapi" Enabled="False">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Panjang Intra Uterine")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox runat="server" ID="cboGCIntrauterineLength" ClientInstanceName="cboGCIntrauterineLength"
                                    Width="100%" ToolTip="Panjang Intra Uterine" Enabled="False">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Sudut Intra Uterine")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox runat="server" ID="cboGCIntrauterineCorner" ClientInstanceName="cboGCIntrauterineCorner"
                                    Width="100%" ToolTip="Sudut Intra Uterine" Enabled="False">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Diameter Ovoid/Silinder")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox runat="server" ID="cboGCCylinder" ClientInstanceName="cboGCCylinder"
                                    Width="100%" ToolTip="Diameter Ovoid/Silinder" Enabled="False">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblNormal">
                                    <%=GetLabel("Kedalaman Jarum")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNeddleDepth" Width="60px" CssClass="number" runat="server" ReadOnly="True"/> cm
                            </td>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblNormal">
                                    <%=GetLabel("Jumlah Jarum")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalNeddle" Width="60px" CssClass="number" runat="server" Enabled="True"/> 
                            </td>
                        </tr> 
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblNormal">
                                    <%=GetLabel("Lokasi Jarum")%></label>
                            </td>
                            <td colspan="3">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsNeedleLocation1" Width="60px" runat="server" Text=" 1" Enabled="False" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsNeedleLocation3" Width="60px" runat="server" Text=" 3" Enabled="False" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsNeedleLocation5" Width="60px" runat="server" Text=" 5" Enabled="False" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsNeedleLocation7" Width="60px" runat="server" Text=" 7" Enabled="False" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsNeedleLocation9" Width="60px" runat="server" Text=" 9" Enabled="False" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsNeedleLocation11" Width="60px" runat="server" Text=" 9" Enabled="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsNeedleLocation2" Width="60px" runat="server" Text=" 2" Enabled="False" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsNeedleLocation4" Width="60px" runat="server" Text=" 4" Enabled="False" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsNeedleLocation6" Width="60px" runat="server" Text=" 6" Enabled="False" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsNeedleLocation8" Width="60px" runat="server" Text=" 8" Enabled="False" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsNeedleLocation10" Width="60px" runat="server" Text=" 10" Enabled="False" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsNeedleLocation12" Width="60px" runat="server" Text=" 12" Enabled="False" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblNormal">
                                    <%=GetLabel("Dosis Preskripsi Brakiterapi")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtTotalDosage" Width="60px" CssClass="number" runat="server" ReadOnly="True"/> Gy
                            </td>
                        </tr> 
                        <tr>
                            <td class="tdLabel" style="vertical-align:top; padding-top: 3px">
                                <label class="lblNormal">
                                    <%=GetLabel("Limitasi Dosis Pada OAR")%></label>
                            </td>
                            <td colspan="3">
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <label class="lblNormal">
                                                <%=GetLabel("Bladder")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBladderDosageLimitation" Width="60px" CssClass="number" runat="server" ReadOnly="True"/> Gy
                                        </td>
                                        <td style="padding-left: 10px">
                                            <label class="lblNormal">
                                                <%=GetLabel("Sigmoid")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSigmoidDosageLimitation" Width="60px" CssClass="number" runat="server" ReadOnly="True"/> Gy
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label class="lblNormal">
                                                <%=GetLabel("Rectum")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRectumDosageLimitation" Width="60px" CssClass="number" runat="server" ReadOnly="True"/> Gy
                                        </td>
                                        <td style="padding-left: 10px">
                                            <label class="lblNormal">
                                                <%=GetLabel("Bowel")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBowelDosageLimitation" Width="60px" CssClass="number" runat="server" ReadOnly="True"/> Gy
                                        </td>
                                    </tr>
                                </table>
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
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Prosedur Tindakan") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtProcedureRemarks" runat="server" Width="99%" TextMode="Multiline" Height="450px" ReadOnly="True" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <div style="position: relative;">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                            <tr>
                                <td>
                                    <dxcp:ASPxCallbackPanel ID="cbpParamedicTeamView" runat="server" Width="100%" ClientInstanceName="cbpParamedicTeamView"
                                        ShowLoadingPanel="false" OnCallback="cbpParamedicTeamView_Callback">
                                        <ClientSideEvents EndCallback="function(s,e){ oncbpParamedicTeamViewEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent3" runat="server">
                                                <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGrid" Style="height: 300px">
                                                    <asp:GridView ID="grdParamedicTeamView" runat="server" CssClass="grdSelected grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                <ItemTemplate>
                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                    <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                                    <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                                                    <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                                                    <input type="hidden" value="<%#:Eval("GCParamedicRole") %>" bindingfield="GCParamedicRole" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="ParamedicName" HeaderText="Dokter/Tenaga Medis" HeaderStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="ParamedicRole" HeaderText="Peranan" HeaderStyle-Width="150px"
                                                                HeaderStyle-HorizontalAlign="Left" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Tidak ada data team pelaksana untuk order ini")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="containerPaging">
                                        <div class="wrapperPaging">
                                            <div id="paramedicPaging">
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="divPage4" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chkIsHasProcedureComplication" Width="100%" runat="server" Text=" Terjadi Komplikasi pada saat tindakan" Enabled="False" /> 
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <asp:CheckBox ID="chkIsHasProcedureHemorrhage" Width="100%" runat="server" Text=" Terjadi Perdarahan" Enabled="false" /> 
                            </td>
                            <td colspan="2">
                                <dxe:AspxComboBox runat="server" id="cboGCHemorrhage1" ClientInstanceName="cboGCHemorrhage1"
                                    width="100%" tooltip="Kondisi Perdarahan" ClientEnabled="False">
                                </dxe:AspxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblNormal">
                                    <%=GetLabel("Skala Nyeri (1-10)")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtProcedurePainIndex" Width="60px" CssClass="number" runat="server" Enabled="False"/> 
                            </td>
                        </tr> 
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan Komplikasi Anestesi") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAnesthesiaComplicationRemarks" runat="server" Width="99%" TextMode="Multiline" Height="150px" ReadOnly="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan Komplikasi Tindakan") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtProcedureComplicationRemarks" runat="server" Width="99%" TextMode="Multiline" Height="150px" ReadOnly="True" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage5" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chkIsHasApplicatorReleaseComplication" Width="100%" runat="server" Text=" Terjadi Komplikasi pada saat pelepasan aplikator" Enabled="False" /> 
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <asp:CheckBox ID="chkIsHasApplicatorReleaseHemorrhage" Width="100%" runat="server" Text=" Terjadi Perdarahan" Enabled="False" /> 
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox runat="server" ID="cboGCApplicatorReleaseHemorrhage" ClientInstanceName="cboGCApplicatorReleaseHemorrhage"
                                    Width="100%" ToolTip="Kondisi Perdarahan" ClientEnabled="False">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblNormal">
                                    <%=GetLabel("Skala Nyeri (1-10)")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtPainIndex" Width="60px" CssClass="number" runat="server" ReadOnly="True"/> 
                            </td>
                        </tr> 
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan Komplikasi Pelepasan Aplikator") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtApplicatorReleaseComplicationRemarks" runat="server" Width="99%" TextMode="Multiline" Height="250px" ReadOnly="True" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
<div style="display: none">
</div>

