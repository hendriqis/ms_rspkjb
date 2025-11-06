<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPList.master"
    AutoEventWireup="true" CodeBehind="PhysicianSchedule.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.PhysicianSchedule" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:content id="Content1" contentplaceholderid="plhList" runat="server">
    <script type="text/javascript">
        function onRefreshGrdView() {
            if (IsValid(null, 'fsPhysicianSchedule', 'mpPhysicianSchedule'))
                cbpView.PerformCallback('refresh');
        }

        //#region Physician
        function GetPhysicianFilter() {
            var filterExpression = "ParamedicID IN (SELECT ParamedicID FROM vServiceUnitParamedic WHERE DepartmentID = 'MCU')";
            return filterExpression;
        }

        $('#lblPhysician.lblLink').live('click', function () {
            openSearchDialog('paramedic', GetPhysicianFilter(), function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtParamedicCodeChanged(value);
            });
        });

        $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtParamedicCodeChanged($(this).val());
        });

        function onTxtParamedicCodeChanged(value) {
            var filterExpression = "ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%=hdnPhysicianID.ClientID %>').val('');
                    $('#<%=txtPhysicianCode.ClientID %>').val('');
                    $('#<%=txtPhysicianName.ClientID %>').val('');
                }
                cbpView.PerformCallback('refresh');
            });
        }
        //#endregion

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        $('.lnkVisitType a').live('click', function () {
            var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
            var healthcareServiceUnitID = $(this).closest('tr').find('.keyField').html();
            var id = paramedicID + '|' + healthcareServiceUnitID;
            var url = ResolveUrl("~/Program/Master/ParamedicSchedule/PSVisitTypeEntryCtl.ascx");
            openUserControlPopup(url, id, 'Jenis Kunjungan', 900, 500);
        });

        $('.lnkSchedule a').live('click', function () {
            var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
            var healthcareServiceUnitID = $(this).closest('tr').find('.keyField').html();
            var id = paramedicID + '|' + healthcareServiceUnitID;
            var url = ResolveUrl("~/Program/Master/ParamedicSchedule/PSScheduleDayEntryCtl.ascx");
            openUserControlPopup(url, id, 'Jadwal Dokter', 1300, 500);
        });

        $('.lnkLeaveSchedule a').live('click', function () {
            var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
            var healthcareServiceUnitID = $(this).closest('tr').find('.keyField').html();
            var id = paramedicID + '|' + healthcareServiceUnitID;
            var url = ResolveUrl("~/Program/Master/ParamedicSchedule/PSleaveScheduleEntryCtl.ascx");
            openUserControlPopup(url, id, 'Jadwal Cuti', 900, 500);
        });
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <fieldset id="fsPhysicianSchedule">
        <table>
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 500px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblLink" id="lblPhysician">
                        <%=GetLabel("Dokter / Paramedis")%></label>
                </td>
                <td>
                    <input type="hidden" value="" runat="server" id="hdnPhysicianID" />
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 120px" />
                            <col style="width: 3px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:textbox id="txtPhysicianCode" width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:textbox id="txtPhysicianName" readonly="true" width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </fieldset>
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:panel runat="server" id="pnlView" cssclass="pnlContainerGrid">
                        <asp:gridview id="grdView" runat="server" cssclass="grdSelected" autogeneratecolumns="false"
                            showheaderwhenempty="true" emptydatarowstyle-cssclass="trEmpty">
                            <columns>
                                    <asp:BoundField DataField="HealthcareServiceUnitID" HeaderStyle-CssClass="keyField"
                                        ItemStyle-CssClass="keyField" />
                                    <asp:BoundField DataField="HealthcareName" HeaderText="Rumah Sakit" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="350px" />
                                    <asp:BoundField DataField="ServiceUnitName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Unit Pelayanan"/>
                                    <asp:HyperLinkField ItemStyle-HorizontalAlign="Center" HeaderText="Jenis Kunjungan" Text="Setting"
                                        ItemStyle-CssClass="lnkVisitType" HeaderStyle-Width="150px" />
                                    <asp:HyperLinkField ItemStyle-HorizontalAlign="Center" HeaderText="Jadwal" Text="Setting"
                                        ItemStyle-CssClass="lnkSchedule" HeaderStyle-Width="150px" />
                                    <asp:HyperLinkField ItemStyle-HorizontalAlign="Center" HeaderText="Jadwal Cuti" Text="Setting"
                                        ItemStyle-CssClass="lnkLeaveSchedule" HeaderStyle-Width="150px" />
                                </columns>
                            <emptydatatemplate>
                                    <%=GetLabel("No Data To Display")%>
                                </emptydatatemplate>
                        </asp:gridview>
                    </asp:panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging">
                </div>
            </div>
        </div>
    </div>
</asp:content>
