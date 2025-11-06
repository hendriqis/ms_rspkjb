<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="PhysicianVisitPatientInformation.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PhysicianVisitPatientInformation" %>

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
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                    cbpViewDetail.PerformCallback();
                }
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });
        //#endregion

        $('#<%=btnRefresh.ClientID %>').click(function (evt) {
            if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
                if ($('#<%:hdnPhysicianID.ClientID %>').val() == "") {
                    showToast("ERROR", 'Error Message : ' + "Dokter harus dipilih terlebih dahulu !");
                }
                else {
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();
                    cbpInfoParamedicScheduleDateView.PerformCallback('refresh');
                    cbpViewDetail.PerformCallback('refresh');
                }
            }
        });

        //#region Physician
        function onGetPhysicianFilterExpression() {
            var filterExpression = "";

            filterExpression = "ParamedicID IN (SELECT DISTINCT ParamedicID FROM ConsultVisit WHERE GCVisitStatus IN ('X020^002','X020^003') AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vHealthcareServiceUnit WHERE DepartmentID = 'INPATIENT'))";
            return filterExpression;
        }
        $('#lblPhysician.lblLink').live('click', function () {
            openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtPhysicianCodeChanged(value);
            });
        });

        $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPhysicianCodeChanged($(this).val());
        });

        function onTxtPhysicianCodeChanged(value) {
            var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%=txtPhysicianCode.ClientID %>').val('');
                    $('#<%=hdnPhysicianID.ClientID %>').val('');
                    $('#<%=txtPhysicianName.ClientID %>').val('');
                }
            });
            cbpInfoParamedicScheduleDateView.PerformCallback('refresh');
            cbpViewDetail.PerformCallback('refresh');
        }
        //#endregion

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#pagingInfoParamedicScheduleView"), pageCount, function (page) {
                cbpInfoParamedicScheduleView.PerformCallback('changepage|' + page);
            });
        });

        function onCboInfoParamedicScheduleServiceUnitValueChanged(s) {
            cbpInfoParamedicScheduleDateView.PerformCallback('refresh');
        }

        function onCbpInfoParamedicScheduleViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);

                setPaging($("#pagingInfoParamedicScheduleView"), pageCount, function (page) {
                    cbpInfoParamedicScheduleView.PerformCallback('changepage|' + page);
                });

                $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                    if ($(this).attr('class') != 'selected') {
                        $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                        $(this).addClass('selected');
                        $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                        cbpViewDetail.PerformCallback();
                    }
                });
                $('#<%=grdView.ClientID %> tr:eq(1)').click();

            }
        }

        function onCboInfoParamedicScheduleDateServiceUnitValueChanged(s) {
            cbpInfoParamedicScheduleDateView.PerformCallback('refresh');
        }

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
                <td style="width: 100px">
                    <label>
                        <%=GetLabel("Departemen") %></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboInfoParamedicScheduleServiceUnit" ClientInstanceName="cboInfoParamedicScheduleServiceUnit"
                        Width="15%" Enabled="false" runat="server">
                        <ClientSideEvents ValueChanged="function(s,e) { onCboInfoParamedicScheduleServiceUnitValueChanged(s); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" style="width: 150px">
                    <label class="lblLink" id="lblPhysician">
                        <%=GetLabel("Dokter / Tenaga Medis")%></label>
                </td>
                <td>
                    <input type="hidden" id="hdnPhysicianID" runat="server" value="" />
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 10%" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtPhysicianCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="20%" runat="server" />
                            </td>
                        </tr>
                    </table>
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
                                                        <asp:BoundField DataField="HealthcareServiceUnitID" HeaderStyle-CssClass="keyField"
                                                            ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <div>
                                                                    <%=GetLabel("Unit Pelayanan")%></div>
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
                                                    </Columns>
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
                                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
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
                                                                    <th style="width: 50%">
                                                                        <%=GetLabel("Nama Pasien")%>
                                                                    </th>
                                                                    <th style="width: 10%">
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
                                                                <td align="center">
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
