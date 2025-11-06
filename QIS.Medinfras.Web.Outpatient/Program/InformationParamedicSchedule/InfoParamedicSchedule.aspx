<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master"
    AutoEventWireup="true" CodeBehind="InfoParamedicSchedule.aspx.cs" Inherits="QIS.Medinfras.Web.Outpatient.Program.InfoParamedicSchedule" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript" id="dxss_patientvisitctl">
        //#region datepicker
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpInfoParamedicScheduleDateView, 'paging');

            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') == 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                    cbpViewDetail.PerformCallback();
                }
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setDatePicker('<%=txtPSSchduleDate.ClientID %>');
            $('#<%=txtPSSchduleDate.ClientID %>').datepicker('option', 'maxDate', '1000');
        });
        //#endregion

        $('#<%=txtPSSchduleDate.ClientID %>').live('change', function (evt) {
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

        //#region tab
        $(function () {
            $('#ulTabLabResult li').click(function () {
                $("#p1").removeAttr("class").hide();
                $('#ulTabLabResult li.selected').removeAttr('class');
                $('.containerInfo').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
        });
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <div class="containerUlTabPage" style="margin-bottom: 3px;">
        <ul class="ulTabPage" id="ulTabLabResult">
            <li contentid="containerPerKlinik" class="selected">
                <%=GetLabel("Jadwal Per Klinik")%></li>
            <li contentid="containerPerTanggal">
                <%=GetLabel("Jadwal Per Tanggal")%></li>
        </ul>
    </div>
    <div id="containerPerKlinik" class="containerInfo">
        <style type="text/css">
            .tdEmptySchedule
            {
                background-color: #AAA;
            }
        </style>
        <div style="padding: 15px">
            <div class="pageTitle">
                <%=GetLabel("Jadwal Dokter Per Klinik")%></div>
            <table class="tblContentArea" style="width: 100%">
                <tr>
                    <td style="width: 100px">
                        <label>
                            <%=GetLabel("Klinik") %></label>
                    </td>
                    <td>
                        <dxe:ASPxComboBox ID="cboInfoParamedicScheduleServiceUnit" ClientInstanceName="cboInfoParamedicScheduleServiceUnit"
                            Width="200px" runat="server">
                            <ClientSideEvents ValueChanged="function(s,e) { onCboInfoParamedicScheduleServiceUnitValueChanged(s); }" />
                        </dxe:ASPxComboBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <input type="hidden" id="hdnParamedicID" runat="server" />
                        <div style="position: relative;">
                            <dxcp:ASPxCallbackPanel ID="cbpInfoParamedicScheduleView" runat="server" Width="100%"
                                ClientInstanceName="cbpInfoParamedicScheduleView" ShowLoadingPanel="false" OnCallback="cbpInfoParamedicScheduleView_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpInfoParamedicScheduleViewEndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                        <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 350px;
                                            font-size: 1.1em;">
                                            <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                                <EmptyDataTemplate>
                                                    <table id="tblView" runat="server" class="grdSelected" cellspacing="0" rules="all">
                                                        <tr>
                                                            <th>
                                                                <%=GetLabel("Nama Dokter")%>
                                                            </th>
                                                            <th style="width: 110px" align="center">
                                                                <%=GetLabel("Senin")%>
                                                            </th>
                                                            <th style="width: 110px" align="center">
                                                                <%=GetLabel("Selasa")%>
                                                            </th>
                                                            <th style="width: 110px" align="center">
                                                                <%=GetLabel("Rabu")%>
                                                            </th>
                                                            <th style="width: 110px" align="center">
                                                                <%=GetLabel("Kamis")%>
                                                            </th>
                                                            <th style="width: 110px" align="center">
                                                                <%=GetLabel("Jumat")%>
                                                            </th>
                                                            <th style="width: 110px" align="center">
                                                                <%=GetLabel("Sabtu")%>
                                                            </th>
                                                            <th style="width: 110px" align="center">
                                                                <%=GetLabel("Minggu")%>
                                                            </th>
                                                        </tr>
                                                        <tr class="trEmpty">
                                                            <td colspan="8">
                                                                <%=GetLabel("No Data To Display")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="tblView" runat="server" class="grdNormal" cellspacing="0" rules="all">
                                                        <tr>
                                                            <th>
                                                                <%=GetLabel("Nama Dokter")%>
                                                            </th>
                                                            <th style="width: 110px" align="center">
                                                                <%=GetLabel("Senin")%>
                                                            </th>
                                                            <th style="width: 110px" align="center">
                                                                <%=GetLabel("Selasa")%>
                                                            </th>
                                                            <th style="width: 110px" align="center">
                                                                <%=GetLabel("Rabu")%>
                                                            </th>
                                                            <th style="width: 110px" align="center">
                                                                <%=GetLabel("Kamis")%>
                                                            </th>
                                                            <th style="width: 110px" align="center">
                                                                <%=GetLabel("Jumat")%>
                                                            </th>
                                                            <th style="width: 110px" align="center">
                                                                <%=GetLabel("Sabtu")%>
                                                            </th>
                                                            <th style="width: 110px" align="center">
                                                                <%=GetLabel("Minggu")%>
                                                            </th>
                                                        </tr>
                                                        <tr runat="server" id="itemPlaceholder">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <%#: Eval("ParamedicName")%>
                                                        </td>
                                                        <td align="center" id="tdCol1" runat="server">
                                                        </td>
                                                        <td align="center" id="tdCol2" runat="server">
                                                        </td>
                                                        <td align="center" id="tdCol3" runat="server">
                                                        </td>
                                                        <td align="center" id="tdCol4" runat="server">
                                                        </td>
                                                        <td align="center" id="tdCol5" runat="server">
                                                        </td>
                                                        <td align="center" id="tdCol6" runat="server">
                                                        </td>
                                                        <td align="center" id="tdCol7" runat="server">
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>
                            <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                            </div>
                            <div class="containerPaging">
                                <div class="wrapperPaging">
                                    <div id="pagingInfoParamedicScheduleView">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="containerPerTanggal" class="containerInfo" style="display: none">
        <style type="text/css">
            .tdEmptyScheduleDate
            {
                background-color: #AAA;
            }
        </style>
        <div style="padding: 15px">
            <div class="pageTitle">
                <%=GetLabel("Jadwal Dokter Per Tanggal")%></div>
            <table class="tblContentArea" style="width: 100%">
                <tr>
                    <td style="width: 100px">
                        <label>
                            <%=GetLabel("Tanggal Praktek") %></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPSSchduleDate" Width="120px" runat="server" CssClass="datepicker" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table style="width: 100%">
                        <colgroup>
                            <col style="width: 40%" />
                            <col style="width: 60%" />
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
                                                        <asp:BoundField DataField="ServiceUnitCode" HeaderText="Kode" HeaderStyle-Width="100px"
                                                            HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Nama" HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="ShortName" HeaderText="Nama Singkat" HeaderStyle-Width="200px"
                                                            HeaderStyle-HorizontalAlign="Left" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Tidak ada transaksi permintaan barang")%>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingView2">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
<%--                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingInfoParamedicScheduleView2">
                                        </div>
                                    </div>
                                </div>--%>
                            </td>
                            <td style="vertical-align: top">
                                <div style="position: relative;">
                                    <dxcp:ASPxCallbackPanel ID="cbpViewDetail" runat="server" Width="100%" ClientInstanceName="cbpViewDetail"
                                        OnCallback="cbpViewDetail_Callback" ShowLoadingPanel="false">
                                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDetail').show(); }"
                                            EndCallback="function(s,e){ $('#containerImgLoadingViewDetail').hide(); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent3" runat="server">
                                                <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGridProcessList">
                                                    <asp:GridView ID="grdDetail" runat="server" CssClass="grdSelected grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="HealthcareServiceUnitID" HeaderStyle-CssClass="keyField"
                                                                ItemStyle-CssClass="keyField" />
                                                            <asp:BoundField DataField="ParamedicName" HeaderText="Nama Dokter" HeaderStyle-Width="300px"
                                                                HeaderStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="LeaveInfo" HeaderText="Informasi Cuti" HeaderStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="RoomName" HeaderText="Ruang" HeaderStyle-Width="100px"
                                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="Slot" HeaderText="Sesi" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center"
                                                                HeaderStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="StartTime" HeaderText="Jam Mulai" HeaderStyle-Width="50px"
                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="EndTime" HeaderText="Jam Selesai" HeaderStyle-Width="50px"
                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="CountSlot" HeaderText="Target" HeaderStyle-Width="70px"
                                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="CountAP" HeaderText="Perjanjian" HeaderStyle-Width="70px"
                                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="CountAP_Complete" HeaderText="Datang" HeaderStyle-Width="70px"
                                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="CountAP_UnComplete" HeaderText="Belum Datang" HeaderStyle-Width="70px"
                                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                                            <asp:BoundField DataField="CountAP_Void" HeaderText="Batal" HeaderStyle-Width="70px"
                                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Tidak ada informasi Detail Permintaan Barang")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDetail">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
<%--                                    <div class="containerPaging">
                                        <div class="wrapperPaging">
                                            <div id="pagingInfoParamedicScheduleView3">
                                            </div>
                                        </div>
                                    </div>--%>
                                </div>
                            </td>
                        </tr>
                    </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
