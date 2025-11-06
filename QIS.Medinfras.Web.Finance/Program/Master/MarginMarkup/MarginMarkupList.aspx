<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master"
    AutoEventWireup="true" CodeBehind="MarginMarkupList.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.MarginMarkupList" %>

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

        $('#<%=grdView.ClientID %> tr:gt(0) td.tdExpand').live('click', function () {
            $tr = $(this).parent();
            $trDetail = $(this).parent().next();
            if ($trDetail.attr('class') != 'trDetail') {
                $trCollapse = $('.trDetail');

                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
                $newTr = $("<tr><td></td><td colspan='3'></td></tr>").attr('class', 'trDetail');
                $newTr.insertAfter($tr);
                $newTr.find('td').last().append($('#containerGrdDetail'));

                if ($trCollapse != null) {
                    $trCollapse.prev().find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                    $trCollapse.remove();
                }

                $('#<%=grdDetail.ClientID %> tr:gt(0)').remove();
                $('#<%=hdnExpandID.ClientID %>').val($tr.find('.keyField').html());
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                cbpViewDetailMarkup.PerformCallback();
            }
            else {
                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                $('#tempContainerGrdDetail').append($('#containerGrdDetail'));

                $trDetail.remove();
            }
        });

        $('#lblEntryPopupAddData').live('click', function () {
            var sequenceNo = "0";
            var markupID = $('#<%=hdnID.ClientID %>').val();
            var filter = markupID + "|" + sequenceNo;
            var url = ResolveUrl("~/Program/Master/MarginMarkup/MarginMarkupDtEntryCtl.ascx");
            openUserControlPopup(url, filter, 'Margin Markup Detail', 600, 300);
        });

        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function () {
            if (confirm("Are You Sure Want To Delete This Data?")) {
                $row = $(this).closest('tr');
                var sequenceNo = $row.find('.hdnSequenceNoList').val();
                $('#<%=hdnSequenceNo.ClientID %>').val(sequenceNo);

                cbpViewDetailMarkup.PerformCallback('delete');
            }
        });

        $('.imgEdit.imgLink').die('click');
        $('.imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var sequenceNo = $row.find('.hdnSequenceNoList').val();
            var markupID = $('#<%=hdnID.ClientID %>').val();
            var filter = markupID + "|" + sequenceNo;
            var url = ResolveUrl("~/Program/Master/MarginMarkup/MarginMarkupDtEntryCtl.ascx");
            openUserControlPopup(url, filter, 'Margin Markup Detail', 600, 300);
        });

        function onAfterSaveAddRecordEntryPopup() {
            cbpViewDetailMarkup.PerformCallback('refresh');
        }

        function onAfterSaveEditRecordEntryPopup() {
            cbpViewDetailMarkup.PerformCallback('refresh');
        }

        function onCbpViewDetailMarkupEndCallback(s) {
            cbpViewDetailMarkup.PerformCallback('refresh');
        }

        $('.lnkMarginMarkupDtClass').live('click', function () {
            $row = $(this).closest('tr');
            var sequenceNo = $row.find('.hdnSequenceNoList').val();
            var markupID = $('#<%=hdnID.ClientID %>').val();
            var filter = markupID + "|" + sequenceNo;
            var url = ResolveUrl("~/Program/Master/MarginMarkup/MarginMarkupDtClassEntryCtl.ascx");
            openUserControlPopup(url, filter, 'Margin per Kelas', 600, 500);
        });

    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnSequenceNo" runat="server" />
    <input type="hidden" value="" id="hdnExpandID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="MarkupID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField ItemStyle-Width="20px" ItemStyle-CssClass="tdExpand" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <img class="imgExpand imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>'
                                            alt='' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="MarkupCode" HeaderText="Kode Markup" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="MarkupName" HeaderText="Nama Markup" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:CheckBoxField DataField="IsMarkupInPercentage" HeaderText="%" HeaderStyle-Width="50px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <%--<asp:HyperLinkField HeaderText="Range" Text="Setup" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkMarginMarkupDt" HeaderStyle-Width="120px" />--%>
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
                <dxcp:ASPxCallbackPanel ID="cbpViewDetailMarkup" runat="server" Width="100%" ClientInstanceName="cbpViewDetailMarkup"
                    ShowLoadingPanel="false" OnCallback="cbpViewDetailMarkup_Callback" EndCallback="function(s,e){ onCbpViewDetailMarkupEndCallback(s); }">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDetail').show(); }"
                        EndCallback="function(s,e){ $('#containerImgLoadingViewDetail').hide(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto">
                                <asp:GridView ID="grdDetail" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                                
                                                <input type="hidden" class="hdnSequenceNoList" value="<%#: Eval("SequenceNo")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="SequenceNo" HeaderText="Sequence No" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="cfStartingValueInString" HeaderText="Starting Value" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="cfEndingValueInString" HeaderText="Ending Value" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="cfMarkupAmountInString" HeaderText="Markup Amount" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="cfMarkupAmount2InString" HeaderText="Markup Amount 2" ItemStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="120px" />
                                        <asp:HyperLinkField HeaderText="Detail Kelas" Text="Setup" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkMarginMarkupDtClass"
                                            HeaderStyle-Width="100px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewDetail">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
