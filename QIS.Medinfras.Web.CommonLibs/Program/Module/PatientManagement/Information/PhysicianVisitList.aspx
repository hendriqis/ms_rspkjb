<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="PhysicianVisitList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PhysicianVisitList" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erevaluationnotes">
        $(function () {
            setDatePicker('<%=txtFromVisitDate.ClientID %>');
            $('#<%=txtFromVisitDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            setDatePicker('<%=txtToVisitDate.ClientID %>');
            $('#<%=txtToVisitDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        function onCboPhysicianListChanged(s) {
            if (s.GetValue() != null) {
                $('#<%=hdnParamedicID.ClientID %>').val(s.GetValue());
            }
            else
                $('#<%=hdnParamedicID.ClientID %>').val('');
            cbpView.PerformCallback('refresh');
        }

        $('#<%=txtFromVisitDate.ClientID %>').live('change', function () {
            cbpView.PerformCallback('refresh');
        });

        $('#<%=txtToVisitDate.ClientID %>').live('change', function () {
            cbpView.PerformCallback('refresh');
        });

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

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
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnVisitID" runat="server" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <div style="position: relative;">
        <table class="tblEntryContent" style="width: 100%">
            <colgroup>
                <col width="200px" />
            </colgroup>         
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Dokter ") %></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboPhysicianList" ClientInstanceName="cboPhysicianList" Width="350px"
                        runat="server">
                        <ClientSideEvents ValueChanged="function(s,e) { onCboPhysicianListChanged(s); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr id="trDate">
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal Kunjungan") %></label>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:TextBox ID="txtFromVisitDate" Width="110px" CssClass="datepicker" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtToVisitDate" Width="110px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty" ShowHeader="true">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="cfNoteDate" HeaderText="Date" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NoteTime" HeaderText="Time" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ParamedicName" HeaderText="Physician Name" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada catatan dokter untuk pasien ini") %>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
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
</asp:Content>
