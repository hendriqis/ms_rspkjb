<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="CustomerContractList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.CustomerContractList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"`
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
        $('#lblCustomer.lblLink').live('click', function () {
            openSearchDialog('businesspartners', '', function (value) {
                $('#<%=txtCustomerCode.ClientID %>').val(value);
                onTxtCustomerCodeChanged(value);
            });
        });

        $('#<%=txtCustomerCode.ClientID %>').live('change', function () {
            onTxtCustomerCodeChanged($(this).val());
        });

        function onTxtCustomerCodeChanged(value) {
            var filterExpression = "BusinessPartnerCode = '" + value + "'";
            Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnCustomerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%=txtCustomerName.ClientID %>').val(result.BusinessPartnerName);
                }
                else {
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
                $newTr = $("<tr><td></td><td colspan='5'></td></tr>").attr('class', 'trDetail');
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
            openMatrixControl('ContractCoverage', id, 'Coverage Type - ' + $('#<%=txtCustomerName.ClientID %>').val());
        });

        $('.lnkContractSummary a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/CustomerContract/CustomerContractSummaryViewCtl.ascx");
            openUserControlPopup(url, id, 'Contract Summary', 700, 600);
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
            openMatrixControl('ContractCoverageMember', param, 'Coverage Type Member - ' + $('#<%=txtCustomerName.ClientID %>').val());
        });
        
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnExpandID" runat="server" />
    <table>
        <colgroup>
            <col style="width:100px"/>
            <col style="width:500px"/>
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblLink" id="lblCustomer"><%=GetLabel("Customer")%></label></td>
            <td>
                <input type="hidden" value="" runat="server" id="hdnCustomerID" />
                <table style="width:100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width:30%"/>
                        <col style="width:3px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td><asp:TextBox ID="txtCustomerCode" Width="100%" runat="server" /></td>
                        <td>&nbsp;</td>
                        <td><asp:TextBox ID="txtCustomerName" ReadOnly="true" Width="100%" runat="server" /></td>
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
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ContractID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                 <asp:TemplateField ItemStyle-Width="20px" ItemStyle-CssClass="tdExpand" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <img class="imgExpand imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>' alt='' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ContractNo" HeaderText="Contract No" />
                                <asp:BoundField DataField="StartDateInString" ItemStyle-HorizontalAlign="Center" HeaderText="Start Date" HeaderStyle-Width="150px" />
                                <asp:BoundField DataField="EndDateInString" ItemStyle-HorizontalAlign="Center" HeaderText="End Date" HeaderStyle-Width="150px" />
                                <asp:HyperLinkField HeaderText="Contract Summary" Text="Contract Summary" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkContractSummary" HeaderStyle-Width="150px" />
                                <asp:HyperLinkField HeaderText="Coverage Type" Text="Coverage Type" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkCoverageType" HeaderStyle-Width="150px" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
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
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpViewDetail" runat="server" Width="100%" ClientInstanceName="cbpViewDetail"
                    ShowLoadingPanel="false" OnCallback="cbpViewDetail_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDetail').show(); }"
                        EndCallback="function(s,e){ $('#containerImgLoadingViewDetail').hide(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto">
                                <asp:GridView ID="grdDetail" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="CoverageTypeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="CoverageTypeCode" HeaderText="Coverage Type Code" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="CoverageTypeName" HeaderText="Coverage Type Name" />
                                        <asp:HyperLinkField HeaderText="Member" Text="Member" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkCoverageMember" HeaderStyle-Width="120px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
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