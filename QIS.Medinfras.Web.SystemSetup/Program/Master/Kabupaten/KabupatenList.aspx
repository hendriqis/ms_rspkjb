<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="KabupatenList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.KabupatenList" %>

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
            openUserControlPopup(url, id, 'Unit Pelayanan Rumah Sakit', 900, 500);
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

        $('.lnkItem a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('ServiceUnitItem', id, 'Item');
        });

        $('.lnkAutoBillItem a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/ServiceUnit/ServiceUnitAutoBillItemEntryCtl.ascx");
            openUserControlPopup(url, id, 'Auto Bill Item', 900, 500);
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
            var KabupatenID = $('#<%=hdnID.ClientID %>').val();
            var filterExpression = "KabupatenID = " + KabuptenID;
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
    <input type="hidden" value="" id="hdnKodeKabupaten" runat="server" />
    
    <input type="hidden" value="" id="hdnCollapseID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnIsBridgingToAplicares" value="" runat="server" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#tempContainerGrdDetail').append($('#containerGrdDetail'));showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" RowStyle-CssClass="trMain">
                            <Columns>
                                <asp:BoundField DataField="KabupatenID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                
                                <asp:BoundField DataField="KodeKabupaten" HeaderText="Kode" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="NamaKabupaten" HeaderText="Nama" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="State" HeaderText="Nama State" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="BPJSReferenceInfo" HeaderText="BPJS Reference" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
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
                <div id="paging"></div>
            </div>
        </div> 
    </div>

    <div id="tempContainerGrdDetail" style="display:none">
        <div id="containerGrdDetail" class="borderBox" style="width: 100%;padding: 10px 5px;">
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpViewDetail" runat="server" Width="100%" ClientInstanceName="cbpViewDetail"
                    ShowLoadingPanel="false" OnCallback="cbpViewDetail_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDetail').show(); }"
                        EndCallback="function(s,e){ $('#containerImgLoadingViewDetail').hide(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto">
                                <asp:GridView ID="grdDetail" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                   
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