<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true"
    CodeBehind="ServiceUnitList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ServiceUnitList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
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

        $('.lnkHealthcare a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/ServiceUnit/ServiceUnitHealthcareEntryCtl.ascx");
            openUserControlPopup(url, id, 'Unit Pelayanan Rumah Sakit', 1200, 500);
        });

        $('#<%=grdView.ClientID %> tr:gt(0) td.tdExpand').live('click', function () {
            $tr = $(this).parent();
            $trDetail = $(this).parent().next();
            if ($trDetail.attr('class') != 'trDetail') {
                $trCollapse = $('.trDetail');

                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
                $newTr = $("<tr><td></td><td colspan='6'></td></tr>").attr('class', 'trDetail');
                $newTr.insertAfter($tr);
                $newTr.find('td').last().append($('#containerGrdDetail'));

                if ($trCollapse != null) {
                    $trCollapse.prev().find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                    $trCollapse.remove();
                }

                $('#<%=grdDetail.ClientID %> tr:gt(0)').remove();
                $('#<%=hdnCollapseID.ClientID %>').val($tr.find('.keyField').html());
                cbpViewDetail.PerformCallback();
            }
            else {
                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                $('#tempContainerGrdDetail').append($('#containerGrdDetail'));

                $trDetail.remove();
            }
        });

        $('.lnkSubKlinik a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/ServiceUnit/ServiceUnitHealthcareSpecialtyCtl.ascx");
            openUserControlPopup(url, id, 'Sub Klinik', 900, 500);
        });

        $('.lnkItem a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('ServiceUnitItem', id, 'Item');
        });

        $('.lnkVitalSign a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('ServiceUnitVitalSign', id, 'Tanda Vital');
        });

        $('.lnkAutoBillItem a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/ServiceUnit/ServiceUnitAutoBillItemEntryCtl.ascx");
            openUserControlPopup(url, id, 'Auto Bill Item', 1200, 500);
        });

        $('.lnkAutoBillItemParamedic a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/ServiceUnit/ServiceUnitAutoBillItemParamedicEntryCtl.ascx");
            openUserControlPopup(url, id, 'Auto Bill Item Paramedic', 1200, 500);
        });

        $('.lnkAutoBillItemDaily a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/ServiceUnit/ServiceUnitAutoBillItemDailyEntryCtl.ascx");
            openUserControlPopup(url, id, 'Auto Bill Item Daily', 1200, 500);
        });

        $('.lnkRoom a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/ServiceUnit/ServiceUnitRoomEntryCtl.ascx");
            openUserControlPopup(url, id, 'Kamar', 900, 500);
        });

        $('.lnkVisitType a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/ServiceUnit/ServiceUnitVisitTypeEntryCtl.ascx");
            openUserControlPopup(url, id, 'Jenis Kunjungan', 900, 500);
        });

        $('.lnkHealthcareProfessional a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('ServiceUnitParamedic', id, 'Healthcare Professional');
        });

        function onAfterDeleteClickSuccess() {
            var isBridgingAplicares = $('#<%=hdnIsBridgingToAplicares.ClientID %>').val();
            if (isBridgingAplicares == "1") {
                UpdateRoomAplicares();
            }
        }

        //#region APLICARES

        function UpdateRoomAplicares() {
            var serviceUnitID = $('#<%=hdnID.ClientID %>').val();
            var filterExpression = "ServiceUnitID = " + serviceUnitID;
            Methods.getListObject('GetvServiceUnitAplicaresList', filterExpression, function (result) {
                for (i = 0; i < result.length; i++) {
                    var kodeKelas = result[i].AplicaresClassCode;
                    var kodeRuang = result[i].ServiceUnitCode;

                    var hsuID = result[i].HealthcareServiceUnitID;
                    var classID = result[i].ClassID;

                    AplicaresService.deleteClassRoom(kodeKelas, kodeRuang, function (resultRoom) {
                        if (resultRoom != null) {
                            try {
                                AplicaresService.updateStatusDeleteFromAplicares(hsuID, classID, function (resultUpdate) {
                                    if (resultUpdate != null) {
                                        try {
                                            var resultUpdate = resultUpdate.split('|');
                                            if (resultUpdate[0] == "1") {
                                                showToast('INFORMATION', "SUCCESS");

                                                cbpView.PerformCallback('refresh');
                                            }
                                            else {
                                                showToast('FAILED', resultUpdate[2]);
                                            }
                                        } catch (error) {
                                            showToast('FAILED', error);
                                        }
                                    }
                                });
                            } catch (err) {
                                showToast('FAILED', err);
                            }
                        }
                    });
                }
            });
        }

        //#endregion
        
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnCollapseID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnIsBridgingToAplicares" value="" runat="server" />
    <input type="hidden" id="hdnIsBridgingToMedinfrasMobileApps" value="" runat="server" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#tempContainerGrdDetail').append($('#containerGrdDetail'));showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" RowStyle-CssClass="trMain">
                            <Columns>
                                <asp:BoundField DataField="ServiceUnitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField ItemStyle-Width="20px" ItemStyle-CssClass="tdExpand" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <img class="imgExpand imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>'
                                            alt='' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ServiceUnitCode" HeaderText="Kode" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ServiceUnitName" HeaderText="Nama" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ShortName" HeaderText="Nama Singkat" HeaderStyle-Width="200px"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ServiceInterval" HeaderText="Lama Pelayanan" HeaderStyle-Width="100px"
                                    ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                <asp:HyperLinkField HeaderText="Rumah Sakit" HeaderStyle-Width="120px" Text="Setting"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkHealthcare" HeaderStyle-HorizontalAlign="Center" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Data Tidak Tersedia")%>
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
    <div id="tempContainerGrdDetail" style="display: none">
        <div id="containerGrdDetail" class="borderBox" style="width: 100%; padding: 10px 5px;">
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpViewDetail" runat="server" Width="100%" ClientInstanceName="cbpViewDetail"
                    ShowLoadingPanel="false" OnCallback="cbpViewDetail_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDetail').show(); }"
                        EndCallback="function(s,e){ $('#containerImgLoadingViewDetail').hide(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto">
                                <asp:GridView ID="grdDetail" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="HealthcareServiceUnitID" HeaderStyle-CssClass="keyField"
                                            ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="HealthcareName" HeaderText="Nama Rumah Sakit" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ServiceInterval" HeaderText="Lama Pelayanan" HeaderStyle-Width="80px"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                        <asp:HyperLinkField HeaderText="Sub Klinik" Text="Sub Klinik" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-CssClass="lnkSubKlinik" HeaderStyle-Width="100px" />
                                        <asp:HyperLinkField HeaderText="Item" Text="Item" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-CssClass="lnkItem" HeaderStyle-Width="100px" />
                                        <asp:HyperLinkField HeaderText="Auto Bill Item" Text="Auto Bill Item" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-CssClass="lnkAutoBillItem" HeaderStyle-Width="120px" />
                                        <asp:HyperLinkField HeaderText="Auto Bill Item Paramedic" Text="Auto Bill Item Paramedic" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-CssClass="lnkAutoBillItemParamedic" HeaderStyle-Width="120px" />
                                        <asp:HyperLinkField HeaderText="Auto Bill Daily" Text="Auto Bill Daily" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-CssClass="lnkAutoBillItemDaily" HeaderStyle-Width="120px" />
                                        <asp:HyperLinkField HeaderText="Jenis Kunjungan" Text="Jenis Kunjungan" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-CssClass="lnkVisitType" HeaderStyle-Width="120px" />
                                        <asp:HyperLinkField HeaderText="Tanda Vital" Text="Tanda Vital" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-CssClass="lnkVitalSign" HeaderStyle-Width="120px" />
                                        <asp:HyperLinkField HeaderText="Printer" Text="Printer" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-CssClass="lnkPrinter" HeaderStyle-Width="120px" Visible="false" />
                                        <asp:HyperLinkField HeaderText="Dokter / Tenaga Medis" Text="Dokter/ Tenaga Medis"
                                            ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkHealthcareProfessional"
                                            HeaderStyle-Width="150px" />
                                        <asp:HyperLinkField HeaderText="Kamar" Text="Kamar" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-CssClass="lnkRoom" HeaderStyle-Width="100px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewDetail">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
