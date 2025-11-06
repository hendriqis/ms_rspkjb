<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageList.master" AutoEventWireup="true" 
    CodeBehind="NursingProcessList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.NursingProcessList1" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbRefresh.png")%>' alt="" /><div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $btnPropose = null;
        $btnReopen = null;
        $(function () {
            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                    cbpViewDt.PerformCallback('refresh');
                }
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            //#region Propose
            $('.btnPropose').die('click');
            $('.btnPropose').live('click', function () {
                $btnPropose = $(this);

                $tr = $(this).closest('tr').parent().closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());

                displayConfirmationMessageBox('SEND ORDER : FARMASI', 'Kirim order resep ke farmasi ?', function (result) {
                    if (result) {
                        onCustomButtonClick('Propose');
                        $('#<%:hdnIsOutstandingOrder.ClientID %>').val("0");
                    }
                });
            });
            //#endregion

            //#region Reopen
            $('.btnReopen').die('click');
            $('.btnReopen').live('click', function () {
                $btnReopen = $(this);

                $tr = $(this).closest('tr').parent().closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());

                displayConfirmationMessageBox('SEND ORDER : FARMASI', 'Reopen order resep ke farmasi ?', function (result) {
                    if (result) {
                        onCustomButtonClick('ReOpen');
                        $('#<%:hdnIsOutstandingOrder.ClientID %>').val("1");
                    }
                });
            });
            //#endregion

            $('.imgEdit.imgLink').die('click');
            $('.imgEdit.imgLink').live('click', function () {
                $tr = $(this).closest('tr').parent().closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                cbpMPListProcess.PerformCallback('edit');
            });

            $('.imgDelete.imgLink').die('click');
            $('.imgDelete.imgLink').live('click', function () {
                $tr = $(this).closest('tr').parent().closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                displayConfirmationMessageBox('ORDER FARMASI', 'Lanjutkan proses hapus order farmasi ?', function (result) {
                    if (result) cbpMPListProcess.PerformCallback('delete');
                });
            });

        });

        $('#<%=btnRefresh.ClientID %>').click(function (evt) {
            onRefreshControl();
        });

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
            cbpViewDt.PerformCallback('refresh');
        }

        function onAfterCustomClickSuccess() {
//            if ($btnPropose != null) {
//                $btnPropose.hide();
//            }
//            if ($btnReopen != null) {
//                $btnReopen.hide();
            //            }
            onRefreshControl();
        }
        
        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
            cbpViewDt.PerformCallback('refresh');
        }
        
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

        //#region Paging Dt
        function onCbpViewDtEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onBeforeBackToListPage() {
            if ($('#<%:hdnIsOutstandingOrder.ClientID %>').val() == "1") {
                var line1 = "Masih ada order resep yang belum dikirim ke Farmasi, Silahkan dikirim order terlebih dahulu dengan meng-klik tombol <b>Send Order</b>";
                var line2 = "<br />Jika masih mengalami kendala, silahkan klik tombol <b>Refresh</b>";
                var messageBody = line1 + line2;
                displayMessageBox('ORDER RESEP', messageBody);
            }
            else {
                backToPatientList();
            }
        }
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnIsOutstandingOrder" runat="server" value="0" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <table style="width:100%">
        <colgroup>
            <col style="width:30%"/>
            <col style="width:70%"/>
        </colgroup>
        <tr>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" 
                                    EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="TransactionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
<%--                                                               <img class="imgEdit imgLink" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>
                                                                    title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                    alt="" style="float: left" />
--%>                                                              </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td>
    <%--                                                            <img class="imgDelete imgLink" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>
                                                                    src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                    alt="" style="float: left" />
    --%>                                                        </td>
                                                            <td style="width: 1px">
                                                                &nbsp;
                                                            </td>
                                                            <td>
        <%--                                                        <img class="imgProceed" <%# Eval("IsProceed").ToString() == "False" ? "Style='display:none'":"" %>
                                                                    src='<%# ResolveUrl("~/Libs/Images/Button/lock.png")%>'
                                                                    alt="" style="float: left" title="Sudah diproses" />
        --%>                                                    </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>   
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div><%=GetLabel("Tanggal - Waktu Asuhan")%>, <%=GetLabel("No. Asuhan")%></div>
                                                    <div style="width:250px;float:left"><%=GetLabel("Perawat")%></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <div><%#: Eval("TransactionDateTimeInString")%>, <%#: Eval("TransactionNo")%> </div>
                                                                <div style="width:250px;float:left"><%#: Eval("ParamedicName") %></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada proses asuhan keperawatan untuk pasien ini")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging"></div>
                        </div>
                    </div> 
                </div>
            </td>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                        ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                            EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="NursingItemText"  HeaderText = "Deskripsi" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada proses asuhan keperawatan untuk pasien ini")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt" >
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingDt"></div>
                        </div>
                    </div> 
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
