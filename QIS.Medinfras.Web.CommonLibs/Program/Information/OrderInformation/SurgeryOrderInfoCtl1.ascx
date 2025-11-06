<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SurgeryOrderInfoCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SurgeryOrderInfoCtl1" %>
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
<script type="text/javascript" id="dxss_SurgeryOrderEntryCtl1">
    $(function () {
        $('#<%=rblIsEmergency.ClientID %> input').change(function () {
            if ($(this).val() == "1") {
                $('#<%=chkIsCITO.ClientID %>').prop('checked', true);
            }
            else {
                $('#<%=chkIsCITO.ClientID %>').prop('checked', false);
            }
        });

        $('#<%=chkIsNextVisit.ClientID %>').change(function () {
            var isChecked = $(this).is(":checked");
            if (isChecked) {
                $('#<%=trNextVisit.ClientID %>').removeAttr("style");
            }
            else {
                $('#<%=trNextVisit.ClientID %>').attr("style", "display:none");
            }
        });

        $('#<%=grdProcedureGroupView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdProcedureGroupView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnOrderDtProcedureGroupID.ClientID %>').val($(this).find('.keyField').html());
        });
        $('#<%=grdProcedureGroupView.ClientID %> tr:eq(1)').click();

        $('#<%=grdParamedicTeamView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdParamedicTeamView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnOrderDtParamedicTeamID.ClientID %>').val($(this).find('.keyField').html());
        });
        $('#<%=grdParamedicTeamView.ClientID %> tr:eq(1)').click();

        //#region Room

        function getRoomFilterExpression() {
            var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            var filterExpression = '';

            if (serviceUnitID != '') {
                filterExpression = "HealthcareServiceUnitID = " + serviceUnitID;
            }

            if (filterExpression != '') {
                filterExpression += " AND ";
            }
            filterExpression += "IsDeleted = 0";

            return filterExpression;
        }

        $('#<%:lblRoom.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('serviceunitroom', getRoomFilterExpression(), function (value) {
                $('#<%:txtRoomCode.ClientID %>').val(value);
                onTxtRoomCodeChanged(value);
            });
        });

        $('#<%:txtRoomCode.ClientID %>').live('change', function () {
            onTxtRoomCodeChanged($(this).val());
        });

        function onTxtRoomCodeChanged(value) {
            var filterExpression = getRoomFilterExpression() + " AND RoomCode = '" + value + "'";
            getRoom(filterExpression);
        }

        function getRoom(_filterExpression) {
            var filterExpression = _filterExpression;
            if (filterExpression == '') filterExpression = getRoomFilterExpression();
            var serviceUnitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
            if (serviceUnitID != "") {
                Methods.getListObject('GetvServiceUnitRoomList', filterExpression, function (result) {
                    if (result.length == 1) {
                        $('#<%:hdnRoomID.ClientID %>').val(result[0].RoomID);
                        $('#<%:txtRoomName.ClientID %>').val(result[0].RoomName);
                        $('#<%:txtRoomCode.ClientID %>').val(result[0].RoomCode);
                    }
                    else {
                        $('#<%:hdnRoomID.ClientID %>').val('');
                        $('#<%:txtRoomCode.ClientID %>').val('');
                        $('#<%:txtRoomName.ClientID %>').val('');
                    }
                });
            } else {
                $('#<%:hdnRoomID.ClientID %>').val('');
                $('#<%:txtRoomCode.ClientID %>').val('');
                $('#<%:txtRoomName.ClientID %>').val('');
            }
        }
        //#endregion

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

        $('#<%=rblIsHasInfectiousDisease.ClientID %> input').change(function () {
            if ($(this).val() == "1") {
                $('#<%=trInfectiousInfo.ClientID %>').removeAttr("style");
            }
            else {
                $('#<%=trInfectiousInfo.ClientID %>').attr("style", "display:none");
            }
        });

        $('#<%=rblIsHasComorbidities.ClientID %> input').change(function () {
            if ($(this).val() == "1") {
                $('#<%=trComorbiditiesInfo.ClientID %>').removeAttr("style");
            }
            else {
                $('#<%=trComorbiditiesInfo.ClientID %>').attr("style", "display:none");
            }
        });

        $('#leftPageNavPanel ul li').first().click();
    });
    //#region Procedure Group

    function onCbpProcedureGroupViewEndCallback(s) {
        var param = s.cpResult.split('|');

        if (param[0] == 'refresh') {
            $('#<%=grdProcedureGroupView.ClientID %> tr:eq(1)').click();
        }
        else
            $('#<%=grdProcedureGroupView.ClientID %> tr:eq(1)').click();
    }

    //#endregion

    //#region ParamedicTeam

    function oncbpParamedicTeamViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            $('#<%=grdParamedicTeamView.ClientID %> tr:eq(1)').click();
        }
        else
            $('#<%=grdParamedicTeamView.ClientID %> tr:eq(1)').click();
    }
    //#endregion

    function onCboGCInfectiousDiseaseChanged(s) {
        var cboGCInfectiousDisease = s.GetValue();

        if (cboGCInfectiousDisease != Constant.InfectiousDisease.OTHERS) {
            $('#<%=txtOtherInfectiousDisease.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtOtherInfectiousDisease.ClientID %>').val('');
        }
        else {
            $('#<%=txtOtherInfectiousDisease.ClientID %>').removeAttr('readonly');
        }
    }

    function onCboGCComorbiditiesChanged(s) {
        var cboGCComorbidities = s.GetValue();

        if (cboGCComorbidities != Constant.Comorbidities.OTHERS) {
            $('#<%=txtOtherComorbidities.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtOtherComorbidities.ClientID %>').val('');
        }
        else {
            $('#<%=txtOtherComorbidities.ClientID %>').removeAttr('readonly');
        }
    }

    function onGetScheduleFilterExpression() {
        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
        var scheduleDate = $('#<%=txtScheduleDate.ClientID %>').val();
        var scheduleDateInDatePicker = Methods.getDatePickerDate(scheduleDate);
        var scheduleDateFormatString = Methods.dateToString(scheduleDateInDatePicker);
        var filterExpression = "VisitID = " + visitID + " AND ScheduleDate = '" + scheduleDateFormatString + "' AND GCScheduleStatus != 'X449^03' AND IsDeleted = 0";
        return filterExpression;
    }
</script>
<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnTestOrderID" value="" />
    <input type="hidden" runat="server" id="hdnPlanningNoteID" value="" />
    <input type="hidden" value="" id="hdnPatientVisitNoteID" runat="server" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicTeamID" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowFreeTextMode" value="0" />
    <input type="hidden" value="1" id="hdnProcedureGroupProcessMode" runat="server" />
    <input type="hidden" value="1" id="hdnParamedicTeamProcessMode" runat="server" />
    <input type="hidden" runat="server" id="hdnOrderDtProcedureGroupID" value="" />
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
                        <li contentid="divPage1" title="Jadwal Kamar Operasi" class="w3-hover-red">
                            Jadwal Operasi</li>
                        <li contentid="divPage2" title="Jenis Operasi" class="w3-hover-red">
                            Jenis Operasi</li>
                        <li contentid="divPage3" title="Team Pelaksana" class="w3-hover-red">
                            Team Pelaksana</li>
                        <li contentid="divPage4" title="Permohonan Khusus" class="w3-hover-red">
                            Permohonan Khusus</li>
                    </ul>
                </div>
            </td>
            <td style="vertical-align: top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 180px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam Order")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderDate" Width="120px" CssClass="datepicker" runat="server" Enabled="false" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Pasien")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtPatientName" Width="100%" runat="server" Enabled="False" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No. RM")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMedicalNo" Width="100%" runat="server" Enabled="False" />
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <label class="lblNormal">
                                                <%=GetLabel("No. Registrasi")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRegistrationNo" Width="100%" runat="server" Enabled="False" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No. Order")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtOrderNo" Width="100%" runat="server" Enabled="False" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Dokter")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboParamedicID" Width="100%" runat="server" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label runat="server">
                                    <%=GetLabel("Estimasi Lama Operasi") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEstimatedDuration" Width="80px" CssClass="number" runat="server" Enabled="false" />
                                menit
                            </td>
                            <td style="padding-left: 5px">
                                <asp:CheckBox ID="chkIsUsedRequestTime" Width="180px" runat="server" Text=" Permintaan Jam Khusus" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Jadwal Operasi") %>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblIsEmergency" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table" Enabled="false">
                                    <asp:ListItem Text=" Emergency" Value="1" />
                                    <asp:ListItem Text=" Elektif" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam Rencana")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtScheduleDate" Width="120px" CssClass="datepicker" runat="server" Enabled="false" />
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="1">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtScheduleTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" Enabled="false" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsNextVisit" Width="200px" runat="server" Text=" Kunjungan berikutnya" Enabled="false" />
                                        </td>
                                        <td style="display: none">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trNextVisit" runat="server" style="display: none">
                            <td class="tdLabel">
                                <%=GetLabel("Jenis Kunjungan Berikutnya") %>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblNextVisitType" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table" Enabled="false">
                                    <asp:ListItem Text=" ODS" Value="1" />
                                    <asp:ListItem Text=" Rawat Inap" Value="2" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblRoom">
                                    <%:GetLabel("Ruang Operasi")%></label>
                            </td>
                            <td colspan="2">
                                <input type="hidden" id="hdnRoomID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtRoomCode" Width="100%" runat="server" Enabled="false" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td colspan="">
                                            <asp:TextBox ID="txtRoomName" Width="100%" runat="server" Enabled="False" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Riwayat Penyakit Infeksi") %></td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblIsHasInfectiousDisease" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Enabled="false">
                                    <asp:ListItem Text=" Ya" Value="1" />
                                    <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>    
                        </tr>  
                        <tr id="trInfectiousInfo" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tipe Penyakit Infeksi")%></label>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                    <colgroup>
                                        <col style="width:250px"/>
                                        <col style="width:80px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <dxe:ASPxComboBox runat="server" ID="cboGCInfectiousDisease" ClientInstanceName="cboGCInfectiousDisease"
                                                Width="99%" ToolTip = "Tipe Penyakit Infeksi" Enabled="false">
                                                <ClientSideEvents ValueChanged="function(s){ onCboGCInfectiousDiseaseChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td class="tdLabel" style="padding-left:5px">
                                            <label class="lblNormal">
                                                <%=GetLabel("Lain-lain")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOtherInfectiousDisease" CssClass="txtOtherInfectiousDisease" runat="server" Width="100%" ReadOnly  />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>  
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Memiliki Komorbid") %></td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblIsHasComorbidities" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Enabled="false">
                                    <asp:ListItem Text=" Ya" Value="1" />
                                    <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>    
                        </tr>  
                         <tr id="trComorbiditiesInfo" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tipe Komorbid")%></label>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                                    <colgroup>
                                        <col style="width:250px"/>
                                        <col style="width:80px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <dxe:ASPxComboBox runat="server" ID="cboGCComorbidities" ClientInstanceName="cboGCComorbidities"
                                                Width="99%" ToolTip = "Tipe Komorbid" Enabled="false">
                                                    <ClientSideEvents ValueChanged="function(s){ onCboGCComorbiditiesChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td class="tdLabel" style="padding-left:5px">
                                            <label class="lblNormal">
                                                <%=GetLabel("Lain-lain")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOtherComorbidities" CssClass="txtOtherComorbidities" runat="server" Width="100%" ReadOnly />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>  
                    </table>
                </div>
                <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
                        <colgroup>
                            <col style="width:250px"/>
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Sumber Informasi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtProcedureGroupSource" Width="100%" runat="server" Enabled="False" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="position: relative;">
                                    <dxcp:ASPxCallbackPanel ID="cbpProcedureGroupView" runat="server" Width="100%" ClientInstanceName="cbpProcedureGroupView"
                                        ShowLoadingPanel="false" OnCallback="cbpProcedureGroupView_Callback">
                                        <ClientSideEvents EndCallback="function(s,e){ onCbpProcedureGroupViewEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent6" runat="server">
                                                <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 300px">
                                                    <asp:GridView ID="grdProcedureGroupView" runat="server" CssClass="grdSelected grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                <ItemTemplate>
                                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                    <input type="hidden" value="<%#:Eval("ProcedureGroupID") %>" bindingfield="ProcedureGroupID" />
                                                                    <input type="hidden" value="<%#:Eval("ProcedureGroupCode") %>" bindingfield="ProcedureGroupCode" />
                                                                    <input type="hidden" value="<%#:Eval("ProcedureGroupName") %>" bindingfield="ProcedureGroupName" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                <HeaderTemplate>
                                                                    <%=GetLabel("Jenis Operasi")%>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div>
                                                                        <%#: Eval("ProcedureGroupName")%></div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Kategori Operasi" DataField="SurgeryClassification" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Belum ada informasi jenis operasi untuk pasien ini") %>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <div style="position: relative;">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
                        <colgroup>
                            <col style="width:250px"/>
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Sumber Informasi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtParamedicTeamSource" Width="100%" runat="server" Enabled="False" />
                            </td>
                        </tr>
                            <tr>
                                <td colspan="2">
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
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsUsingSpecificItem" Width="180px" runat="server" Text=" Penggunaan Alat Tertentu" Enabled="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsCITO" Width="100px" runat="server" Text=" CITO" Enabled="False" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan Order") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" Width="99%" TextMode="Multiline" Height="250px" Enabled="false" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
