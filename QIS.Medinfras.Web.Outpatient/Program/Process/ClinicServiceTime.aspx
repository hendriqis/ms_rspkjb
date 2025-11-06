<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="ClinicServiceTime.aspx.cs" Inherits="QIS.Medinfras.Web.Outpatient.Program.ClinicServiceTime" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript" id="dxss_patientvisitctl">
        //#region datepicker
        $(function () {
            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                cbpViewDetail.PerformCallback();
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });
        //#endregion

        $('#<%=btnRefresh.ClientID %>').click(function (evt) {
            if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
                cbpInfoParamedicScheduleDateView.PerformCallback('refresh');
                cbpViewDetail.PerformCallback('refresh');
            }
        });

        $('#<%=txtScheduleDate.ClientID %>').live('change', function (evt) {
            cbpInfoParamedicScheduleDateView.PerformCallback('refresh');
        });

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#pagingInfoParamedicScheduleView"), pageCount, function (page) {
                cbpInfoParamedicScheduleView.PerformCallback('changepage|' + page);
            });
        });

        function onCboInfoParamedicScheduleServiceUnitValueChanged(s) {
            cbpInfoParamedicScheduleView.PerformCallback('refresh');
        }

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

        function onCboInfoParamedicScheduleDateServiceUnitValueChanged(s) {
            cbpInfoParamedicScheduleDateView.PerformCallback('refresh');
        }

        function onCbpInfoParamedicScheduleDateViewEndCallback(s) {
            hideLoadingPanel();
        }
        //#endregion

        //#region Event Handler

        $('.imgStart.imgLink').die('click');
        $('.imgStart.imgLink').live('click', function () {
            $tr = $(this).closest('tr').parent().closest('tr');
            var paramedicID = $tr.find('.hdnParamedicID').val();
            var healthcareServiceUnitID = $tr.find('.hdnHealthcareServiceUnitID').val();
            var departmentID = $tr.find('.hdnDepartmentID').val();
            var tempDate = $tr.find('.hdnTempDate').val();
            var operationalTimeID = $tr.find('.hdnOperationalTimeID').val();
            var id = paramedicID + '|' + healthcareServiceUnitID + '|' + departmentID + '|' + tempDate + '|' + operationalTimeID;
            cbpViewDetail.PerformCallback('start|' + id);
        });

        $('.imgPause.imgLink').die('click');
        $('.imgPause.imgLink').live('click', function () {
            $tr = $(this).closest('tr').parent().closest('tr');
            var paramedicID = $tr.find('.hdnParamedicID').val();
            var healthcareServiceUnitID = $tr.find('.hdnHealthcareServiceUnitID').val();
            var departmentID = $tr.find('.hdnDepartmentID').val();
            var tempDate = $tr.find('.hdnTempDate').val();
            var operationalTimeID = $tr.find('.hdnOperationalTimeID').val();
            var id = paramedicID + '|' + healthcareServiceUnitID + '|' + departmentID + '|' + tempDate + '|' + operationalTimeID;
            var url = ResolveUrl("~/Program/Process/ClinicPauseCtl.ascx");
            openUserControlPopup(url, id, 'Tutup Sementara', 500, 150);
        });

        $('.imgStop.imgLink').die('click');
        $('.imgStop.imgLink').live('click', function () {
            $tr = $(this).closest('tr').parent().closest('tr');
            var paramedicID = $tr.find('.hdnParamedicID').val();
            var healthcareServiceUnitID = $tr.find('.hdnHealthcareServiceUnitID').val();
            var departmentID = $tr.find('.hdnDepartmentID').val();
            var tempDate = $tr.find('.hdnTempDate').val();
            var operationalTimeID = $tr.find('.hdnOperationalTimeID').val();
            var id = paramedicID + '|' + healthcareServiceUnitID + '|' + departmentID + '|' + tempDate + '|' + operationalTimeID;
            cbpViewDetail.PerformCallback('stop|' + id);
        });

        function onAfterSaveEditRecordEntryPopup() {
            cbpViewDetail.PerformCallback('refresh');
        }

        function onAfterPopupControlClosing() {
            cbpViewDetail.PerformCallback('refresh');
        }
        //#endregion

    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnIsBridgingToGateway" runat="server" />
    <input type="hidden" value="" id="hdnProviderGatewayService" runat="server" />
    <div id="containerPerTanggal" class="containerInfo">
        <style type="text/css">
            .tdEmptyScheduleDate
            {
                background-color: #AAA;
            }
        </style>
        <table style="width: 100%">
            <tr>
                <td style="width: 100px">
                    <label>
                        <%=GetLabel("Tanggal ") %></label>
                </td>
                <td>
                    <asp:TextBox ID="txtScheduleDate" Width="120px" runat="server" CssClass="datepicker"
                        ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table style="width: 100%">
                        <colgroup>
                            <col style="width: 25%" />
                            <col style="width: 75%" />
                        </colgroup>
                        <tr>
                            <td style="vertical-align: top">
                                <dxcp:ASPxCallbackPanel ID="cbpInfoParamedicScheduleDateView" runat="server" Width="100%"
                                    ClientInstanceName="cbpInfoParamedicScheduleDateView" ShowLoadingPanel="false"
                                    OnCallback="cbpInfoParamedicScheduleDateView_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpInfoParamedicScheduleDateViewEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent2" runat="server">
                                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridProcessList">
                                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ServiceUnitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <div>
                                                                    <%=GetLabel("Klinik")%></div>
                                                                <div>
                                                                    <%=GetLabel("Kode")%></div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <div>
                                                                                <%#: Eval("ServiceUnitName")%></div>
                                                                            <div>
                                                                                <%#: Eval("ServiceUnitCode")%></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Tidak ada unit pelayanan yang tersedia")%>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingView2">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </td>
                            <td style="vertical-align: top">
                                <div style="position: relative;">
                                    <dxcp:ASPxCallbackPanel ID="cbpViewDetail" runat="server" Width="100%" ClientInstanceName="cbpViewDetail"
                                        OnCallback="cbpViewDetail_Callback" ShowLoadingPanel="false">
                                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDetail').show(); }"
                                            EndCallback="function(s,e){ $('#containerImgLoadingViewDetail').hide(); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent4" runat="server">
                                                <asp:Panel runat="server" ID="PanelDt" CssClass="pnlContainerGridProcessList">
                                                    <asp:ListView runat="server" ID="lvwView">
                                                        <EmptyDataTemplate>
                                                            <table id="tblView" runat="server" class="grdScheduleDt grdSelected" cellspacing="0"
                                                                rules="all">
                                                                <tr>
                                                                    <th class="keyField" rowspan="2">
                                                                        &nbsp;
                                                                    </th>
                                                                    <th rowspan="2" align="left">
                                                                        <%=GetLabel("Dokter")%>
                                                                    </th>
                                                                    <th rowspan="2" style="width: 60px">
                                                                        <%=GetLabel("Ruang")%>
                                                                    </th>
                                                                    <th rowspan="2" style="width: 60px">
                                                                        <%=GetLabel("Sesi Praktek")%>
                                                                    </th>
                                                                    <th colspan="2">
                                                                        <%=GetLabel("WAKTU PELAYANAN")%>
                                                                    </th>
                                                                    <th colspan="4" style="width: 60px">
                                                                        <%=GetLabel("STATUS PASIEN")%>
                                                                    </th>
                                                                    <th rowspan="2">
                                                                        <%=GetLabel("STATUS KLINIK")%>
                                                                    </th>
                                                                    <th rowspan="2" style="width: 60px">
                                                                        <%=GetLabel("ACTION")%>
                                                                    </th>
                                                                </tr>
                                                                <tr>
                                                                    <th style="width: 50px" align="center">
                                                                        <%=GetLabel("Mulai")%>
                                                                    </th>
                                                                    <th style="width: 50px" align="center">
                                                                        <%=GetLabel("Selesai")%>
                                                                    </th>
                                                                    <th style="width: 40px" align="right">
                                                                        <%=GetLabel("APP")%>
                                                                    </th>
                                                                    <th style="width: 40px" align="right">
                                                                        <%=GetLabel("REG")%>
                                                                    </th>
                                                                    <th style="width: 40px" align="right">
                                                                        <%=GetLabel("OPEN")%>
                                                                    </th>
                                                                    <th style="width: 40px" align="right">
                                                                        <%=GetLabel("VOID")%>
                                                                    </th>
                                                                </tr>
                                                                <tr class="trEmpty">
                                                                    <td colspan="25">
                                                                        <%=GetLabel("Tidak ada informasi Jadwal Dokter")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </EmptyDataTemplate>
                                                        <LayoutTemplate>
                                                            <table id="tblView" runat="server" class="grdScheduleDt grdSelected" cellspacing="0"
                                                                rules="all">
                                                                <tr>
                                                                    <th class="keyField" rowspan="2">
                                                                        &nbsp;
                                                                    </th>
                                                                    <th rowspan="2" align="left">
                                                                        <%=GetLabel("Dokter")%>
                                                                    </th>
                                                                    <th rowspan="2" style="width: 60px">
                                                                        <%=GetLabel("Ruang")%>
                                                                    </th>
                                                                    <th rowspan="2" style="width: 60px">
                                                                        <%=GetLabel("Sesi Praktek")%>
                                                                    </th>
                                                                    <th colspan="2">
                                                                        <%=GetLabel("WAKTU PELAYANAN")%>
                                                                    </th>
                                                                    <th colspan="4" style="width: 60px">
                                                                        <%=GetLabel("STATUS PASIEN")%>
                                                                    </th>
                                                                    <th rowspan="2" style="width: 60px">
                                                                        <%=GetLabel("STATUS KLINIK")%>
                                                                    </th>
                                                                    <th rowspan="2" style="width: 60px">
                                                                        <%=GetLabel("ACTION")%>
                                                                    </th>
                                                                </tr>
                                                                <tr>
                                                                    <th style="width: 50px" align="center">
                                                                        <%=GetLabel("Mulai")%>
                                                                    </th>
                                                                    <th style="width: 50px" align="center">
                                                                        <%=GetLabel("Selesai")%>
                                                                    </th>
                                                                    <th style="width: 40px" align="right">
                                                                        <%=GetLabel("APP")%>
                                                                    </th>
                                                                    <th style="width: 40px" align="right">
                                                                        <%=GetLabel("REG")%>
                                                                    </th>
                                                                    <th style="width: 40px" align="right">
                                                                        <%=GetLabel("OPEN")%>
                                                                    </th>
                                                                    <th style="width: 40px" align="right">
                                                                        <%=GetLabel("VOID")%>
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="keyField">
                                                                    <%#: Eval("ParamedicID")%>
                                                                </td>
                                                                <td style="display:none">
                                                                    <input type="hidden" value='<%#:Eval("ParamedicID") %>' class="hdnParamedicID" />
                                                                    <input type="hidden" value='<%#:Eval("HealthcareServiceUnitID") %>' class="hdnHealthcareServiceUnitID" />
                                                                    <input type="hidden" value='<%#:Eval("DepartmentID") %>' class="hdnDepartmentID" />
                                                                    <input type="hidden" value='<%#:Eval("TempDate") %>' class="hdnTempDate" />
                                                                    <input type="hidden" value='<%#:Eval("OperationalTimeID") %>' class="hdnOperationalTimeID" />
                                                                    <input type="hidden" value='<%#:Eval("RoomID") %>' class="hdnRoomID" />
                                                                    <input type="hidden" value='<%#:Eval("GCClinicStatus") %>' class="hdnGCClinicStatus" id="hdnGCClinicStatus" runat="server"/>
                                                                    <input type="hidden" value='<%#:Eval("ClinicStatus") %>' class="hdnClinicStatus" />
                                                                </td>
                                                                <td>
                                                                    <img class="imgPhysicianImage" src='<%# Eval("cfPhysicianImageUrl")%>' alt="" height="55px"
                                                                        width="50px" style="float: left; margin-right: 10px;" />
                                                                    <div>
                                                                        <label class="lblDetail">
                                                                            <%#: Eval("ParamedicName")%></label></div>
                                                                </td>
                                                                <td>
                                                                    <%#: Eval("RoomName")%>
                                                                </td>
                                                                <td align="right">
                                                                    <%#: Eval("Slot")%>
                                                                </td>
                                                                <td align="center">
                                                                    <%#: Eval("StartTime")%>
                                                                </td>
                                                                <td align="center">
                                                                    <%#: Eval("EndTime")%>
                                                                </td>
                                                                <td align="right">
                                                                    <%#: Eval("CountAP")%>
                                                                </td>
                                                                <td align="right">
                                                                    <%#: Eval("CountAP_Complete")%>
                                                                </td>
                                                                <td align="right">
                                                                    <%#: Eval("CountAP_UnComplete")%>
                                                                </td>
                                                                <td align="right">
                                                                    <%#: Eval("CountAP_Void")%>
                                                                </td>
                                                                <td align="right">
                                                                    <%#: Eval("ClinicStatus")%>
                                                                </td>
                                                                <td align="center">
                                                                    <table cellpadding="1" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <img class="imgStart imgLink" id="imgStart" title='<%=GetLabel("Start")%>' src='<%# ResolveUrl("~/Libs/Images/Status/start.png")%>'
                                                                                    alt="" style="float: left" width="24px" height="24px" />
                                                                            </td>
                                                                            <td style="width: 1px">
                                                                                <img class="imgPause imgLink" id="imgPause" title='<%=GetLabel("Pause")%>' src='<%# ResolveUrl("~/Libs/Images/Status/pause.png")%>'
                                                                                    alt="" style="float: left" width="24px" height="24px" />
                                                                            </td>
                                                                            <td>
                                                                                <img class="imgStop imgLink" id="imgStop" title='<%=GetLabel("Stop")%>' src='<%# ResolveUrl("~/Libs/Images/Status/stop_service.png")%>'
                                                                                    alt="" style="float: left" width="24px" height="24px" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:ListView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDetail">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
