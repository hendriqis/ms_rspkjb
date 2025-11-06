<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true"
    CodeBehind="CustomerContractList.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.CustomerContractList" %>

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

        //#region Customer
        function onGetCustomerFilterExpression() {
            return "<%=OnGetCustomerFilterExpression() %>";
        }

        $('#lblCustomer.lblLink').live('click', function () {
            openSearchDialog('businesspartners', onGetCustomerFilterExpression(), function (value) {
                $('#<%=txtCustomerCode.ClientID %>').val(value);
                onTxtCustomerCodeChanged(value);
            });
        });

        $('#<%=txtCustomerCode.ClientID %>').live('change', function () {
            onTxtCustomerCodeChanged($(this).val());
        });

        function onTxtCustomerCodeChanged(value) {
            var filterExpression = onGetCustomerFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
            Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnCustomerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%=txtCustomerName.ClientID %>').val(result.BusinessPartnerName);
                }
                else {
                    $('#<%=hdnID.ClientID %>').val('');
                    $('#<%=hdnCustomerID.ClientID %>').val('');
                    $('#<%=txtCustomerCode.ClientID %>').val('');
                    $('#<%=txtCustomerName.ClientID %>').val('');
                }
                cbpView.PerformCallback('refresh');
            });
        }
        //#endregion

        $('#<%=grdView.ClientID %> tr:gt(0) td.tdExpand').live('click', function () {
            $tr = $(this).parent();
            $trDetail = $(this).parent().next();
            if ($trDetail.attr('class') != 'trDetail') {
                $trCollapse = $('.trDetail');

                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
                $newTr = $("<tr><td></td><td colspan='7'></td></tr>").attr('class', 'trDetail');
                $newTr.insertAfter($tr);
                $newTr.find('td').last().append($('#containerGrdDetail'));

                if ($trCollapse != null) {
                    $trCollapse.prev().find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                    $trCollapse.remove();
                }

                $('#<%=grdDetail.ClientID %> tr:gt(0)').remove();
                $('#<%=hdnExpandID.ClientID %>').val($tr.find('.keyField').html());
                cbpViewDetail.PerformCallback();
            }
            else {
                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                $('#tempContainerGrdDetail').append($('#containerGrdDetail'));

                $trDetail.remove();
            }
        });

        $('.lnkCoverageType a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('ContractCoverage', id, 'Skema Penjaminan - ' + $('#<%=txtCustomerName.ClientID %>').val());
        });

        $('.lnkContractDocument a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/CustomerContract/ContractDocumentCtl.ascx");
            openUserControlPopup(url, id, 'Dokumen Kontrak', 700, 600);
        });

        $('.lnkContractSummary a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Libs/Program/Information/CustomerContractSummaryViewCtl.ascx");
            openUserControlPopup(url, id, 'Ringkasan Kontrak', 700, 600);
        });

        function onAfterSaveMatrixCtl(type) {
            if (type == 'ContractCoverage') {
                cbpView.PerformCallback('refresh');
            }
        }

        $('.lnkCoverageMember a').live('click', function () {
            var contractID = $('#<%=hdnExpandID.ClientID %>').val();
            var coverageTypeID = $(this).closest('tr').find('.keyField').html();
            var param = contractID + '|' + coverageTypeID;
            openMatrixControl('ContractCoverageMember', param, 'Anggota Tipe Jaminan - ' + $('#<%=txtCustomerName.ClientID %>').val());
        });

        $('.lnkDocumentNote a').live('click', function () {
            var id = 'CO' + '|' + $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Libs/Program/Controls/DocumentNotesCtl.ascx");
            openUserControlPopup(url, id, 'Catatan Kontrak', 700, 600);
        });

    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnExpandID" runat="server" />
    <table>
        <colgroup>
            <col style="width: 100px" />
            <col style="width: 500px" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblCustomer">
                    <%=GetLabel("Instansi")%></label>
            </td>
            <td>
                <input type="hidden" value="" runat="server" id="hdnCustomerID" />
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtCustomerCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtCustomerName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#tempContainerGrdDetail').append($('#containerGrdDetail')); $('#hdnID').val(''); showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <ClientSideEvents BeginCallback="function(s,e){ $(&#39;#tempContainerGrdDetail&#39;).append($(&#39;#containerGrdDetail&#39;)); $(&#39;#hdnID&#39;).val(&#39;&#39;); showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }"></ClientSideEvents>
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ContractID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField">
                                    <HeaderStyle CssClass="keyField"></HeaderStyle>
                                    <ItemStyle CssClass="keyField"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField ItemStyle-Width="20px" ItemStyle-CssClass="tdExpand" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <img class="imgExpand imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>'
                                            alt='' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" CssClass="tdExpand" Width="20px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ContractNo" HeaderText="No Kontrak" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="StartDateInString" ItemStyle-HorizontalAlign="Center"
                                    HeaderText="Tanggal Mulai" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="EndDateInString" ItemStyle-HorizontalAlign="Center" HeaderText="Tanggal Akhir"
                                    HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderText="Ringkasan Kontrak" Text="Ringkasan Kontrak" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-CssClass="lnkContractSummary" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" CssClass="lnkContractSummary"></ItemStyle>
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="Skema Penjaminan" Text="Skema Penjaminan" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-CssClass="lnkCoverageType" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" CssClass="lnkCoverageType"></ItemStyle>
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="Dokumen Kontrak" Text="Dokumen" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-CssClass="lnkContractDocument" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" CssClass="lnkContractDocument"></ItemStyle>
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="Catatan Kontrak" Text="Catatan" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-CssClass="lnkDocumentNote" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" CssClass="lnkDocumentNote"></ItemStyle>
                                </asp:HyperLinkField>
                            </Columns>
                            <EmptyDataRowStyle CssClass="trEmpty"></EmptyDataRowStyle>
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
                    <ClientSideEvents BeginCallback="function(s,e){ $(&#39;#containerImgLoadingViewDetail&#39;).show(); }"
                        EndCallback="function(s,e){ $(&#39;#containerImgLoadingViewDetail&#39;).hide(); }">
                    </ClientSideEvents>
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto">
                                <asp:GridView ID="grdDetail" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="CoverageTypeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField">
                                            <HeaderStyle CssClass="keyField"></HeaderStyle>
                                            <ItemStyle CssClass="keyField"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CoverageTypeCode" HeaderText="Kode Jenis Jaminan" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Left" Width="200px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CoverageTypeName" HeaderText="Nama Jenis Jaminan" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:HyperLinkField HeaderText="Anggota" Text="Anggota" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-CssClass="lnkCoverageMember" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Width="250px"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" CssClass="lnkCoverageMember"></ItemStyle>
                                        </asp:HyperLinkField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="trEmpty"></EmptyDataRowStyle>
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
