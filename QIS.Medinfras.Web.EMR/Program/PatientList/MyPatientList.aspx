<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="MyPatientList.aspx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.MyPatientList" %>

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

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#pagingInfoParamedicScheduleView"), pageCount, function (page) {
                cbpInfoParamedicScheduleView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpInfoParamedicScheduleDateViewEndCallback(s) {
            hideLoadingPanel();
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
                                                        <asp:BoundField DataField="HealthcareServiceUnitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <div>
                                                                    <%=GetLabel("Unit Pelayanan ")%></div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <div>
                                                                                <%#: Eval("ServiceUnitName")%></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="70px">
                                                            <HeaderTemplate>
                                                                <div>
                                                                    <%=GetLabel("Jumlah Pasien")%></div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <div>
                                                                                <%#: Eval("NoOfPatients")%></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Tidak ada informasi unit pelayanan")%>
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
                                                                    <th align="left" style="width: 20%">
                                                                        <%=GetLabel("Ruang")%>
                                                                    </th>
                                                                    <th style="width: 20%">
                                                                        <%=GetLabel("Tempat Tidur")%>
                                                                    </th>
                                                                    <th style="width: 20%">
                                                                        <%=GetLabel("No. RM")%>
                                                                    </th>
                                                                    <th style="width: 20%">
                                                                        <%=GetLabel("Nama Pasien")%>
                                                                    </th>
                                                                    <th style="width: 20%">
                                                                        <%=GetLabel("Peranan")%>
                                                                    </th>
                                                                </tr>
                                                                <tr class="trEmpty">
                                                                    <td colspan="25">
                                                                        <%=GetLabel("Tidak ada informasi pasien dirawat")%>
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
                                                                    <th style="width: 5%">
                                                                        <%=GetLabel("Ruang")%>
                                                                    </th>
                                                                    <th style="width: 5%">
                                                                        <%=GetLabel("Tempat Tidur")%>
                                                                    </th>
                                                                    <th style="width: 10%">
                                                                        <%=GetLabel("No. RM")%>
                                                                    </th>
                                                                    <th style="width: 50%" align="left">
                                                                        <%=GetLabel("Nama Pasien")%>
                                                                    </th>
                                                                    <th style="width: 10%" align="left">
                                                                        <%=GetLabel("Peranan")%>
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="keyField">
                                                                    <%#: Eval("ParamedicTeamPhysicianID")%>
                                                                </td>
                                                                <td style="display: none">
                                                                    <input type="hidden" value='<%#:Eval("AppointmentParamedicID") %>' class="hdnAppointmentParamedicID" />
                                                                    <input type="hidden" value='<%#:Eval("RegistrationParamedicID") %>' class="hdnRegistrationParamedicID" />
                                                                    <input type="hidden" value='<%#:Eval("ParamedicTeamPhysicianID") %>' class="hdnParamedicTeamPhysicianID" />
                                                                    <input type="hidden" value='<%#:Eval("HealthcareServiceUnitID") %>' class="hdnHealthcareServiceUnitID" />
                                                                    <input type="hidden" value='<%#:Eval("DepartmentID") %>' class="hdnDepartmentID" />
                                                                    <input type="hidden" value='<%#:Eval("RoomID") %>' class="hdnRoomID" />
                                                                    <input type="hidden" value='<%#:Eval("BedID") %>' class="hdnBedID" />
                                                                </td>
                                                                <td align="center">
                                                                    <%#: Eval("RoomName")%>
                                                                </td>
                                                                <td align="center">
                                                                    <%#: Eval("BedCode")%>
                                                                </td>
                                                                <td align="center">
                                                                    <%#: Eval("RegistrationPatientMedicalNo")%>
                                                                </td>
                                                                <td align="left">
                                                                    <%#: Eval("RegistrationPatientName")%>
                                                                </td>
                                                                <td align="left">
                                                                    <%#: Eval("ParamedicRole")%>
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
