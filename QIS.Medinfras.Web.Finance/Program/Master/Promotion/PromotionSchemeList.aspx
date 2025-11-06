<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="PromotionSchemeList.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.PromotionSchemeList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView);


            $('#ulTabGrdDetail li').click(function () {
                $('#ulTabGrdDetail li.selected').removeAttr('class');
                $('.containerGrdDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
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
            $('#containerDetail2').hide();
            if ($trDetail.attr('class') != 'trDetail') {
                $('#ulTabGrdDetail li:eq(0)').click();

                $trCollapse = $('.trDetail');

                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
                $newTr = $("<tr><td></td><td colspan='6'></td></tr>").attr('class', 'trDetail');
                $newTr.insertAfter($tr);
                $newTr.find('td').last().append($('#containerGrdDetail'));

                if ($trCollapse != null) {
                    $trCollapse.prev().find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                    $trCollapse.remove();
                }

                $('.grdDetail1 tr:gt(0)').remove();
                $('#<%=hdnExpandID.ClientID %>').val($tr.find('.keyField').html());
                cbpViewDetail1.PerformCallback('refresh');
            }
            else {
                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                $('#tempContainerGrdDetail').append($('#containerGrdDetail'));

                $('.grdDetail1 tr:gt(0)').remove();

                $trDetail.remove();
            }
        });

        $('.lnkDepartment a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var type = $(this).closest('tr').find('.gcPromotionType').html();
            if (type == 'X415^001') {
                var url = ResolveUrl("~/Program/Master/Promotion/PromotionSchemeDepartmentEntryCtl.ascx");
                openUserControlPopup(url, id, 'Skema Promo : Instalasi', 1000, 500);
            }
            else {
                displayMessageBox("SKEMA PROMO", "Konfigurasi ini hanya berlaku untuk Skema Promo dengan Harga dan Diskon Detail.");
            }
        });

        $('.lnkItemGroup a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var type = $(this).closest('tr').find('.gcPromotionType').html();
            if (type == 'X415^001') {
                var url = ResolveUrl("~/Program/Master/Promotion/PromotionSchemeItemGroupEntryCtl.ascx");
                openUserControlPopup(url, id, 'Skema Promo : Kelompok Item', 1000, 520);
            }
            else {
                displayMessageBox("SKEMA PROMO", "Konfigurasi ini hanya berlaku untuk Skema Promo dengan Harga dan Diskon Detail.");
            }
        });

        $('.lnkItem a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var type = $(this).closest('tr').find('.gcPromotionType').html();
            if (type == 'X415^001') {
                var url = ResolveUrl("~/Program/Master/Promotion/PromotionSchemeItemEntryCtl.ascx");
                openUserControlPopup(url, id, 'Item', 1000, 520);
            }
            else {
                displayMessageBox("SKEMA PROMO", "Konfigurasi ini hanya berlaku untuk Skema Promo dengan Harga dan Diskon Detail.");
            }
        });

        $('.lnkItemGift a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var type = $(this).closest('tr').find('.gcPromotionType').html();
            if (type == 'X415^002') {
                var url = ResolveUrl("~/Program/Master/Promotion/PromotionSchemeItemFreeGiftEntryCtl.ascx");
                openUserControlPopup(url, id, 'Item Free Gift', 1000, 520);
            }
            else {
                displayMessageBox("SKEMA PROMO", "Konfigurasi ini hanya berlaku untuk Skema Promo dengan Harga dan Diskon Global.");
            }
        });

        $('.lnkHealthcare a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('HealthcarePromotionScheme', id, 'Skema Promo - Healthcare');
        });

        $('.lnkItemGroupMapping a').live('click', function () {
            var type = $(this).closest('tr').find('.gcPromotionType').html();
            var id = $(this).closest('tr').find('.keyField').html();
            if (type == 'X415^002') {
                openMatrixControl('PromotionSchemeItemGroupMapping', id, 'Skema Promo - Kelompok Item (Minimum Transaksi)');
            }
            else {
                displayMessageBox("SKEMA PROMO", "Konfigurasi ini hanya berlaku untuk Skema Promo dengan Harga dan Diskon Global.");
            }
        });

        $('.lnkServiceUnit a').live('click', function () {
            var promotionSchemeID = $('#<%=hdnExpandID.ClientID %>').val();
            var departmentID = $(this).closest('tr').find('.keyField').html();
            var param = promotionSchemeID + '|' + departmentID;
            var url = ResolveUrl("~/Program/Master/Promotion/PromotionSchemeDepartmentServiceUnitEntryCtl.ascx");
            openUserControlPopup(url, param, 'Skema Promo - Unit Pelayanan', 1000, 500);
        });
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnExpandID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#tempContainerGrdDetail').append($('#containerGrdDetail'));showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="overflow-y: scroll">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="PromotionSchemeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="GCPromotionType" HeaderStyle-CssClass="hiddenColumn gcPromotionType" ItemStyle-CssClass="hiddenColumn gcPromotionType" />
                                <asp:TemplateField ItemStyle-Width="20px" ItemStyle-CssClass="tdExpand" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <img class="imgExpand imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>' alt='' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PromotionSchemeCode" HeaderText="Kode Promo" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="PromotionSchemeName" HeaderText="Nama Promo" HeaderStyle-HorizontalAlign="Left" />
                                <asp:HyperLinkField HeaderText="Instalasi" Text="Instalasi" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-CssClass="lnkDepartment" />
                                <asp:HyperLinkField HeaderText="Kelompok Item" Text="Kelompok Item" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" ItemStyle-CssClass="lnkItemGroup" />
                                <asp:HyperLinkField HeaderText="Kelompok Item (Minimum Transaksi)" Text="Kelompok Item (Minimum Transaksi)" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkItemGroupMapping" HeaderStyle-Width="250px" />
                                <%--<asp:HyperLinkField HeaderText="Item Free Gift" Text="Item" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-CssClass="lnkItemGift" />--%>
                                <asp:HyperLinkField HeaderText="Item" Text="Item" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-CssClass="lnkItem" />
                                <asp:HyperLinkField HeaderText="Rumah Sakit" Text="Rumah Sakit" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkHealthcare" HeaderStyle-Width="100px" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Data Tidak Tersedia")%>
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

    <div id="tempContainerGrdDetail" style="display:none">
        <div id="containerGrdDetail" class="borderBox" style="width: 100%;padding: 10px 5px;">
            <div class="containerUlTabPage">
                <ul class="ulTabPage" id="ulTabGrdDetail">
                    <li class="selected" contentid="containerDetail1"><%=GetLabel("Instalasi") %></li>
                </ul>
            </div>
            <div style="position: relative;">
                <div id="containerDetail1" class="containerGrdDt">
                    <dxcp:ASPxCallbackPanel ID="cbpViewDetail1" runat="server" Width="100%" ClientInstanceName="cbpViewDetail1"
                        ShowLoadingPanel="false" OnCallback="cbpViewDetail1_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDetail1').show(); }"
                            EndCallback="function(s,e){ $('#containerImgLoadingViewDetail1').hide(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto">
                                    <asp:ListView runat="server" ID="lvwDetail1">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect grdDetail1" cellspacing="0" rules="all" >
                                                <tr>
                                                    <th style="width:250px" rowspan="2"  align="left"><%=GetLabel("Instalasi")%></th>
                                                    <th colspan="4"><%=GetLabel("Tindakan / Pelayanan")%></th>
                                                    <th colspan="4"><%=GetLabel("Obat dan Persediaan Medis")%></th>
                                                    <th colspan="4"><%=GetLabel("Barang Umum")%></th>  
                                                </tr>
                                                <tr>  
                                                    <th style="width:120px" align="right"><%=GetLabel("Harga Khusus")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th> 
                                                    
                                                    <th style="width:120px" align="right"><%=GetLabel("Harga Khusus")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                    
                                                    <th style="width:120px" align="right"><%=GetLabel("Harga Khusus")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="15">
                                                        <%=GetLabel("Konfigurasi skema promo tidak tersedia")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                                <tr>
                                                    <th style="width:250px" rowspan="2" align="left"><%=GetLabel("Instalasi")%></th>
                                                    <th style="width:150px" rowspan="2" align="left"><%=GetLabel("Formula Jasa Medis")%></th>
                                                    <th colspan="2"><%=GetLabel("Tindakan / Pelayanan")%></th>
                                                    <th colspan="2"><%=GetLabel("Obat dan Persediaan Medis")%></th>
                                                    <th colspan="2"><%=GetLabel("Barang Umum")%></th>    
                                                    <th style="width:100px" rowspan="2"><%=GetLabel("Unit Pelayanan")%></th>
                                                </tr>
                                                <tr>  
                                                    <th style="width:120px" align="right"><%=GetLabel("Harga Khusus")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                 
                                                    <th style="width:120px" align="right"><%=GetLabel("Harga Khusus")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                    
                                                    <th style="width:120px" align="right"><%=GetLabel("Harga Khusus")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder" ></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField"><%#: Eval("DepartmentID")%></td>
                                                <td><%#: Eval("DepartmentName")%></td>     
                                                <td><%#: Eval("RevenueSharingName")%></td>         

                                                <td align="right"><%#: Eval("DisplayTariff1")%></td>
                                                <td align="right"><%#: Eval("DisplayDiscountAmount1")%></td>

                                                <td align="right"><%#: Eval("DisplayTariff2")%></td>
                                                <td align="right"><%#: Eval("DisplayDiscountAmount2")%></td>

                                                <td align="right"><%#: Eval("DisplayTariff3")%></td>
                                                <td align="right"><%#: Eval("DisplayDiscountAmount3")%></td>

                                                <td align="center" class="lnkServiceUnit"><a><%=GetLabel("Unit Pelayanan")%></a></td>                                                 
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel> 
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDetail1">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>   
                </div> 
            </div>
        </div>
    </div>
</asp:Content>