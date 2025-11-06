<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master" AutoEventWireup="true"
    CodeBehind="TestOrderList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.TestOrderList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbRefresh.png")%>' alt="" /><div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="testOrderList_script">
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
                $tr = $(this).closest('tr').parent().closest('tr');
                var entity = rowToObject($tr);
                var visitID = entity.VisitID;
                var schDate = entity.cfScheduledDateInString2;
                var testOrderID = $tr.find('.keyField').html();
                var hsuID = entity.HealthcareServiceUnitID;
                var hsuIDOK = $('#<%=hdnOperatingRoomID.ClientID %>').val();
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());

                var isAlreadyHaveOKOrder = 0;
                if (hsuID == hsuIDOK) {
                    var filterExpression = "VisitID = " + visitID + " AND ScheduledDate = '" + schDate + "' AND GCTransactionStatus NOT IN ('X121^999','X121^005') AND GCOrderStatus NOT IN ('X126^006','X121^005') AND TestOrderID != '" + testOrderID + "' AND HealthcareServiceUnitID = '" + hsuIDOK + "'";
                    Methods.getObject('GetTestOrderHdList', filterExpression, function (resultCheck) {
                        if (resultCheck != null) {
                            isAlreadyHaveOKOrder = 1;
                        }
                    });
                }

                if (isAlreadyHaveOKOrder) {
                    displayConfirmationMessageBox('SEND ORDER :', 'Masih ada outstanding order kamar operasi, <b>lanjutkan proses pembuatan order lagi?', function (result) {
                        if (result) {
                            $('#<%:hdnIsOutstandingOrder.ClientID %>').val("0");
                            onCustomButtonClick('Propose');
                        }
                    });
                }
                else {
                    displayConfirmationMessageBox('SEND ORDER :', 'Kirim order pemeriksaan ke penunjang ?', function (result) {
                        if (result) {
                            $('#<%:hdnIsOutstandingOrder.ClientID %>').val("0");
                            onCustomButtonClick('Propose');
                        }
                    });
                }
            });
            //#endregion

            //#region Reopen
            $('.btnReopen').die('click');
            $('.btnReopen').live('click', function () {
                $tr = $(this).closest('tr').parent().closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());

                displayConfirmationMessageBox('REOPEN ORDER :', 'Reopen order pemeriksaan penunjang ?', function (result) {
                    if (result) {
                        onCustomButtonClick('ReOpen');
                        $('#<%:hdnIsOutstandingOrder.ClientID %>').val("1");
                    }
                });
            });
            //#endregion
        });

        $('#<%=btnRefresh.ClientID %>').click(function (evt) {
            onRefreshControl();
        });

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
            cbpViewDt.PerformCallback('refresh');
        }

        function onAfterCustomClickSuccess() {
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
                    cbpViewDt.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onBeforeBackToListPage() {
            if ($('#<%:hdnIsOutstandingOrder.ClientID %>').val() == "1") {
                var line1 = "Masih ada order pemeriksaan yang belum dikirim ke Penunjang, Silahkan dikirim order terlebih dahulu dengan meng-klik tombol <b>Send Order</b>";
                var line2 = "<br />Jika masih mengalami kendala, silahkan klik tombol <b>Refresh</b>";
                var messageBody = line1 + line2;
                displayMessageBox('SEND ORDER', messageBody);
            }
            else {
                backToPatientList();
            }
        }
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnOperatingRoomID" runat="server" value="" />
    <input type="hidden" id="hdnIsOutstandingOrder" runat="server" value="0" />
    <input type="hidden" id="hdnIsBridgingToMedinfrasV1" runat="server" value="0" />
    <input type="hidden" id="hdnCenterBackConsumerAPI" runat="server" value="0" />
    <input type="hidden" id="hdnIsUsingMultiVisitScheduleOrder" runat="server" value="" />
    <table style="width: 100%">
        <colgroup>
            <col style="width: 30%" />
            <col style="width: 70%" />
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
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="TestOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div>
                                                        <%=GetLabel("Tanggal ") %>
                                                        -
                                                        <%=GetLabel("Jam Order") %>, <span style="color: blue">
                                                            <%=GetLabel("Diorder Oleh") %></span></div>
                                                    <div style="font-weight: bold">
                                                        <%=GetLabel("Unit Pelayanan") %>,
                                                        <%=GetLabel("Test Order No.") %>, <span style="font-style: italic">
                                                            <%=GetLabel("No. Transaksi") %></span></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <div>
                                                                    <%#: Eval("TestOrderDateTimeInString")%>, <span style="color: blue">
                                                                        <%#: Eval("ParamedicName") %></span></div>
                                                                <div style="font-weight: bold">
                                                                    <%#: Eval("ServiceUnitName")%>,
                                                                    <%#: Eval("TestOrderNo")%>, <span style="font-style: italic">
                                                                        <%#: Eval("ChargesNo")%></span></div>
                                                            </td>
                                                            <td align="right" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"" %>>
                                                                <div>
                                                                    <input type="button" class="btnPropose w3-btn w3-hover-blue" value="SEND ORDER" style="background-color: Red;
                                                                        color: White; width: 100px;" /></div>
                                                            </td>
                                                            <td align="right" <%# Eval("IsAllowReopen").ToString() == "False" ? "Style='display:none'":"Style='margin-right:10px'; padding-right: 2px" %>>
                                                                <div>
                                                                    <input type="button" class="btnReopen w3-btn w3-hover-blue" value="REOPEN" style="background-color: Green;
                                                                        color: White; width: 100px" /></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <input type="hidden" value="<%#:Eval("VisitID") %>" bindingfield="VisitID" />
                                                    <input type="hidden" value="<%#:Eval("HealthcareServiceUnitID") %>" bindingfield="HealthcareServiceUnitID" />
                                                    <input type="hidden" value="<%#:Eval("cfScheduledDateInString2") %>" bindingfield="cfScheduledDateInString2" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi order Penunjang untuk pasien ini")%>
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
                                    <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div style="font-weight: bold">
                                                        <%=GetLabel("Pemeriksaan/Pelayanan") %></div>
                                                    <div>
                                                        <span style="color: Blue; font-style: italic">
                                                            <%=GetLabel("Dokter Pelaksana") %></span></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="font-weight: bold">
                                                        <%#: Eval("ItemName1") %></div>
                                                    <div>
                                                        <span style="color: Blue; font-style: italic">
                                                            <%#: Eval("RealizationParamedicName")%></span></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Qty")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <%#: Eval("ItemQty")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="280px" Visible="false">
                                                <HeaderTemplate>
                                                    <div style="font-weight: bold">
                                                        <%=GetLabel("STATUS") %></div>
                                                    <div>
                                                        <span>
                                                            <%=GetLabel("Remarks") %></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <span>
                                                            <%#: Eval("TestOrderStatus")%></span></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi order penunjang untuk pasien ini") %>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingDt">
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
