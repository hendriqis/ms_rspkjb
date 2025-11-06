<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="ItemDrugList.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.ItemDrugList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView2();
            grd.init('grdDrug', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');
        });
        
        $(function () {
            /*$('.grdDrug > tbody > tr:not(:first-child, :nth-child(2), .trDetail)').live('click', function () {
                $('.grdDrug tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('.grdDrug tr:eq(2)').click();*/

//            $('#ulTabGrdDetail li').click(function () {
//                $('#ulTabGrdDetail li.selected').removeAttr('class');
//                $('.containerGrdDt').filter(':visible').hide();
//                $contentID = $(this).attr('contentid');
//                $('#' + $contentID).show();
//                $(this).addClass('selected');
//            });
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
            $('#containerImgLoadingView').hide();
            $('#<%=hdnID.ClientID %>').val('');
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('.grdDrug tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            $('.grdDrug tr:eq(2)').click();
        }
        //#endregion

        $('.tdItemDrugDetail a').live('click', function () {
            var type = $(this).attr('type');
            if (type != "0") {
                var id = $(this).closest('tr').find('.keyField').html() + '|' + $(this).attr('type');
                var url = ResolveUrl("~/Program/Master/ItemDrug/ItemDrugDetailCtl.ascx");
                var headerText = $(this).html();
                openUserControlPopup(url, id, headerText, 600, 500);
            } else {
                var id = $(this).closest('tr').find('.keyField').html();
                var url = ResolveUrl("~/Program/Master/ItemDrug/ItemDrugInfoEditCtl.ascx");
                var headerText = $(this).html();
                openUserControlPopup(url, id, headerText, 800, 550);
            }
        });

//        $('.grdDrug td.tdExpand').live('click', function () {
//            $tr = $(this).parent();
//            $trDetail = $(this).parent().next();
//            $('#containerDetail2').hide();
//            if ($trDetail.attr('class') != 'trDetail') {
//                $('#ulTabGrdDetail li:eq(0)').click();

//                $trCollapse = $('.trDetail');

//                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
//                $newTr = $("<tr><td></td><td colspan='20'></td></tr>").attr('class', 'trDetail');
//                $newTr.insertAfter($tr);
//                $newTr.find('td').last().append($('#containerGrdDetail'));

//                if ($trCollapse != null) {
//                    $trCollapse.prev().find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
//                    $trCollapse.remove();
//                }

//                $('.grdDetail1 tr:gt(0)').remove();
//                $('.grdDetail2 tr:gt(0)').remove();
//                $('#<%=hdnExpandID.ClientID %>').val($tr.find('.keyField').html());
//                cbpViewDetail1.PerformCallback('refresh');
//            }
//            else {
//                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
//                $('#tempContainerGrdDetail').append($('#containerGrdDetail'));

//                $('.grdDetail1 tr:gt(0)').remove();


//                $trDetail.remove();
//            }
//        });

        function onGetEntryPopupReturnValue() {
            return '';
        }

        function onAfterSaveRightPanelContent(code, value) {
            if ($('#containerDetail1').is(":visible"))
                cbpViewDetail1.PerformCallback();
        }

//        $('.lnkEditItemPlanning').live('click', function () {
//            var itemPlanningID = $(this).closest('tr').find('.keyField').html();
//            var url = ResolveUrl("~/Program/Master/ItemDrug/ItemProductPlanningEntryCtl.ascx");
//            openUserControlPopup(url, itemPlanningID, 'Item Planning', 600, 500);
//        });
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnExpandID" runat="server" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:ListView runat="server" ID="lvwView">
                            <EmptyDataTemplate>
                                <table id="tblView" runat="server" class="grdProductLine grdSelected" cellspacing="0" rules="all" >
                                    <tr>  
                                        <th class="keyField" rowspan="2">&nbsp;</th>
                                        <th rowspan="2"><%=GetLabel("Item Information")%></th>                       
                                        <th colspan="8"><%=GetLabel("Drug Information")%></th>                       
                                        <th colspan="2"><%=GetLabel("Inventory")%></th>                     
                                    </tr>
                                    <tr>
                                        <th style="width:90px"><%=GetLabel("Master")%></th>
                                        <th style="width:90px"><%=GetLabel("Contains")%></th>
                                        <th style="width:90px"><%=GetLabel("Indications")%></th> 
                                        <th style="width:90px"><%=GetLabel("Dosage")%></th> 
                                        <th style="width:100px"><%=GetLabel("Administration")%></th>  
                                        <th style="width:100px"><%=GetLabel("Contradiction")%></th>  
                                        <th style="width:110px"><%=GetLabel("Special Precautions")%></th>  
                                        <th style="width:100px"><%=GetLabel("Adverse Reaction")%></th>  
                                        <th style="width:80px"><%=GetLabel("Item Unit")%></th>  
                                        <th style="width:100px"><%=GetLabel("Alternate Unit")%></th>  
                                    </tr>
                                    <tr class="trEmpty">
                                        <td colspan="10">
                                            <%=GetLabel("No Data To Display")%>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <table id="tblView" runat="server" class="grdDrug grdSelected" cellspacing="0" rules="all" >
                                    <tr>  
                                        <th class="keyField" rowspan="2">&nbsp;</th>
                                        <th rowspan="2"><%=GetLabel("Item Information")%></th>                       
                                        <th colspan="8"><%=GetLabel("Drug Information")%></th>                       
                                        <th colspan="2"><%=GetLabel("Inventory")%></th>                     
                                    </tr>
                                    <tr>
                                        <th style="width:90px"><%=GetLabel("Master")%></th>
                                        <th style="width:90px"><%=GetLabel("Contains")%></th>
                                        <th style="width:90px"><%=GetLabel("Indications")%></th> 
                                        <th style="width:90px"><%=GetLabel("Dosage")%></th> 
                                        <th style="width:100px"><%=GetLabel("Administration")%></th>  
                                        <th style="width:100px"><%=GetLabel("Contradiction")%></th>  
                                        <th style="width:110px"><%=GetLabel("Special Precautions")%></th>  
                                        <th style="width:100px"><%=GetLabel("Adverse Reaction")%></th>  
                                        <th style="width:80px"><%=GetLabel("Item Unit")%></th>  
                                        <th style="width:100px"><%=GetLabel("Alternate Unit")%></th>  
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder" ></tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="keyField"><%#: Eval("ItemID")%></td>
                                    <td>
                                        <%#: Eval("ItemName1")%>
                                        <table cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:100px"/>
                                                <col style="width:8px"/>
                                                <col style="width:300px"/>
                                            </colgroup>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <div style="font-size:0.9em;font-style:italic"><%=GetLabel("MIMS Class")%></div>
                                                    <div style="font-size:0.9em;font-style:italic"><%=GetLabel("Generic Name")%></div>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td valign="top">
                                                    <div style="font-size:0.9em;color:#0066FF;"><%#: Eval("MIMSClassName")%></div>
                                                    <div style="font-size:0.9em;color:#000000;font-weight:bold"><%#: Eval("GenericName")%></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="center" class="tdItemDrugDetail"><a type="0"><%=GetLabel("Master")%></a></td>
                                    <td align="center" class="tdItemDrugDetail"><a type="1"><%=GetLabel("Contains")%></a></td>
                                    <td align="center" class="tdItemDrugDetail"><a type="2"><%=GetLabel("Indications")%></a></td>
                                    <td align="center" class="tdItemDrugDetail"><a type="3"><%=GetLabel("Dosage")%></a></td>
                                    <td align="center" class="tdItemDrugDetail"><a type="4"><%=GetLabel("Administration")%></a></td>
                                    <td align="center" class="tdItemDrugDetail"><a type="5"><%=GetLabel("Contradiction")%></a></td>
                                    <td align="center" class="tdItemDrugDetail"><a type="6"><%=GetLabel("Special Precautions")%></a></td>
                                    <td align="center" class="tdItemDrugDetail"><a type="7"><%=GetLabel("Adverse Reaction")%></a></td>
                                    <td align="center"><%#: Eval("ItemUnit")%></td>
                                    <td align="center" class="tdItemDrugDetail"><a type="10"><%=GetLabel("Alternate Unit")%></a></td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
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
</asp:Content>