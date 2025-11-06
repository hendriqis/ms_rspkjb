<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoParamedicScheduleCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.InfoParamedicScheduleCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>


<script type="text/javascript" id="dxss_patientvisitctl">
    function onCboInfoParamedicScheduleServiceUnitValueChanged(s) {
        cbpInfoParamedicScheduleView.PerformCallback('refresh');
        cbpViewTodaySchedule.PerformCallback();
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingInfoParamedicScheduleView"), pageCount, function (page) {
            cbpInfoParamedicScheduleView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpInfoParamedicScheduleViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);

            setPaging($("#pagingInfoParamedicScheduleView"), pageCount, function (page) {
                cbpInfoParamedicScheduleView.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion

    //#region tab
    $(function () {
        $('#ulTabLabResult li').click(function () {
            $('#ulTabLabResult li.selected').removeAttr('class');
            $('.containerInfo').filter(':visible').hide();
            $contentID = $(this).attr('contentid');
            $('#' + $contentID).show();
            $(this).addClass('selected');

        });
    });
    //#endregion
</script>
<style type="text/css">
    .tdEmptySchedule            { background-color: #AAA; }
</style>
<table>
    <tr>
        <td style="width:100px"><label><%=GetLabel("Klinik") %></label></td>
        <td>
            <dxe:ASPxComboBox ID="cboInfoParamedicScheduleServiceUnit" ClientInstanceName="cboInfoParamedicScheduleServiceUnit" Width="200px" runat="server">
                <ClientSideEvents ValueChanged="function(s,e) { onCboInfoParamedicScheduleServiceUnitValueChanged(s); }" />
            </dxe:ASPxComboBox>
        </td>
    </tr>
</table>
<input type="hidden" id="hdnParamedicID" runat="server" />
<div style="position: relative;">
    <div class="containerUlTabPage" style="margin-bottom: 3px;">
        <ul class="ulTabPage" id="ulTabLabResult">
            <li contentid="containerRoutineSchedule" class="selected">
                <%=GetLabel("Jadwal Rutin Dokter")%></li>
            <li contentid="containerTodaySchedule" id="patientCall" runat="server">
                <%=GetLabel("Jadwal Dokter Hari Ini")%></li>
        </ul>
    </div>
    <div id="containerRoutineSchedule" class="containerInfo">
        <dxcp:ASPxCallbackPanel ID="cbpInfoParamedicScheduleView" runat="server" Width="100%" ClientInstanceName="cbpInfoParamedicScheduleView"
            ShowLoadingPanel="false" OnCallback="cbpInfoParamedicScheduleView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpInfoParamedicScheduleViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:350px;font-size:1.1em;">
                        <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                            <EmptyDataTemplate>
                                <table id="tblView" runat="server" class="grdSelected" cellspacing="0" rules="all" >
                                    <tr>
                                        <th><%=GetLabel("Nama Dokter")%></th>
                                        <th style="width:110px" align="center"><%=GetLabel("Senin")%></th>
                                        <th style="width:110px" align="center"><%=GetLabel("Selasa")%></th>
                                        <th style="width:110px" align="center"><%=GetLabel("Rabu")%></th>
                                        <th style="width:110px" align="center"><%=GetLabel("Kamis")%></th>
                                        <th style="width:110px" align="center"><%=GetLabel("Jumat")%></th>
                                        <th style="width:110px" align="center"><%=GetLabel("Sabtu")%></th>
                                        <th style="width:110px" align="center"><%=GetLabel("Minggu")%></th>
                                    </tr>
                                    <tr class="trEmpty">
                                        <td colspan="8">
                                            <%=GetLabel("No Data To Display")%>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <table id="tblView" runat="server" class="grdNormal" cellspacing="0" rules="all" >
                                    <tr>
                                        <th><%=GetLabel("Nama Dokter")%></th>
                                        <th style="width:110px" align="center"><%=GetLabel("Senin")%></th>
                                        <th style="width:110px" align="center"><%=GetLabel("Selasa")%></th>
                                        <th style="width:110px" align="center"><%=GetLabel("Rabu")%></th>
                                        <th style="width:110px" align="center"><%=GetLabel("Kamis")%></th>
                                        <th style="width:110px" align="center"><%=GetLabel("Jumat")%></th>
                                        <th style="width:110px" align="center"><%=GetLabel("Sabtu")%></th>
                                        <th style="width:110px" align="center"><%=GetLabel("Minggu")%></th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder" ></tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td><%#: Eval("ParamedicName")%></td>
                                    <td align="center" id="tdCol1" runat="server"></td>
                                    <td align="center" id="tdCol2" runat="server"></td>
                                    <td align="center" id="tdCol3" runat="server"></td>
                                    <td align="center" id="tdCol4" runat="server"></td>
                                    <td align="center" id="tdCol5" runat="server"></td>
                                    <td align="center" id="tdCol6" runat="server"></td>
                                    <td align="center" id="tdCol7" runat="server"></td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </div>
    <div id="containerTodaySchedule" class="containerInfo" style="display:none">
        <dxcp:ASPxCallbackPanel ID="cbpViewTodaySchedule" runat="server" Width="100%" ClientInstanceName="cbpViewTodaySchedule"
            ShowLoadingPanel="false" OnCallback="cbpViewTodaySchedule_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpViewTodayScheduleEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent2" runat="server">
                    <asp:panel runat="server" id="Panel1" cssclass="pnlContainerGrid">
                        <asp:gridview id="grdView" runat="server" cssclass="grdSelected" autogeneratecolumns="false"
                            showheaderwhenempty="true" emptydatarowstyle-cssclass="trEmpty">
                            <columns>
                                    <asp:BoundField DataField="ParamedicName" HeaderText="Nama Dokter" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="250px" />
                                    <asp:BoundField DataField="OperationalTimeName" HeaderText="Jam Praktek" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                    <asp:BoundField DataField="RoomName" HeaderText="Ruang" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                    <asp:BoundField DataField="TotalMaximumAppointment" HeaderText="Maksimum Kuota" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                    <asp:BoundField DataField="TotalAppointment" HeaderText="Perjanjian" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                    <asp:BoundField DataField="TotalAppointmentCancel" HeaderText="Batal Perjanjian" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                    <asp:BoundField DataField="TotalAppointmentRegistered" HeaderText="Sudah Registrasi" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                </columns>
                            <emptydatatemplate>
                                    <%=GetLabel("No Data To Display")%>
                                </emptydatatemplate>
                        </asp:gridview>
                    </asp:panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </div>
    <div class="imgLoadingGrdView" id="containerImgLoadingView" >
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div>
    <div class="containerPaging">
        <div class="wrapperPaging">
            <div id="pagingInfoParamedicScheduleView"></div>
        </div>
    </div> 
</div>

