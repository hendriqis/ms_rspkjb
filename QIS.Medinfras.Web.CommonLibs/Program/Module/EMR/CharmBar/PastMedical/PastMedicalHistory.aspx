<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true" 
    CodeBehind="PastMedicalHistory.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PastMedicalHistory" EnableViewState="false" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.core.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.widget2.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.mouse.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.draggable.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.droppable.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.effects.core.js")%>'></script>

    <script id="dxis_episodesummaryctl1" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery-ui.js")%>' type='text/javascript'></script>
    <script id="dxis_episodesummaryctl2" src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.easing.1.3.js")%>' type='text/javascript'></script>
    <script id="dxis_episodesummaryctl3" src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.booklet.1.3.1.js")%>' type='text/javascript'></script>
    <script type="text/javascript">
        function onRefreshControl(filterExpression) {
            cbpView.PerformCallback('refresh');
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

            registerEventHandler();
        }
        //#endregion

        function registerEventHandler() {
            $('.trDraggable').each(function () {
                $(this).width($(this).parent().width());
            });
            $('.trDraggable').draggable({
                helper: 'clone',
                drag: function (event, ui) {
                    //insideDropZone = false;
                }
            });

            $('#ulAreaDrop > li').droppable({
                drop: function (event, ui) {
                    //insideDropZone = true;
                    var $tr = ui.helper.clone();
                    var visitID = $tr.find('.keyField').html();
                    if ($('#ulAreaDrop > li .hdnMedicalHistoryVisitID[value=' + visitID + ']').length < 1) {
                        $container = $(this);
                        $container.find('.hdnMedicalHistoryVisitID').val(visitID);
                        $container.find('.tdMedicalHistoryContent').html('');
                        $container.find('.tdMedicalHistoryContent').hide();
                        $container.find('.imgLoadingMedicalHistory').show();
                        var url = ResolveUrl('~/libs/Program/Module/EMR/CharmBar/PastMedical/PastMedicalHistoryContentCtl.ascx');
                        Methods.getHtmlControl(url, visitID, function (result) {
                            $container.find('.imgLoadingMedicalHistory').hide();
                            $container.find('.tdMedicalHistoryContent').show();
                            $container.find('.tdMedicalHistoryContent').html(result.replace(/\VIEWSTATE/g, ''));
                        }, function () {
                            $container.find('.tdMedicalHistoryContent').show();
                            $container.find('.imgLoadingMedicalHistory').hide();
                        });
                    }
                }
            });
        }

        $(function () {
            registerEventHandler();
            $('.lnkMedicalRecordSummary a').live('click', function () {
                var id = $(this).closest('tr').find('.keyField').html();
                var url = ResolveUrl("~/Libs/Controls/PatientPage/EpisodeSummary/EpisodeSummaryCtl.ascx");
                openUserControlPopup(url, id, 'Medical Record - Detail', 1300, 600);
            });
        });

    </script>
    <style type="text/css">
        #ulAreaDrop         { margin: 0; padding: 0; height: 390px; }
        #ulAreaDrop > li    { width:350px; border:1px solid #AAA; height :100%; z-index: 10000; display: inline-block; list-style-type: none; margin-right: 10px; margin-left: 10px; position: relative; }
        .trDraggable        { cursor: pointer; z-index: 19999; }
        .divDropHereInfo    { color: Gray; width:60%;text-align: center; margin-top:100px; }
        #ulAreaDrop > li .imgLoadingMedicalHistory { display: none; position: absolute; left: 44%; top: 44%; } 
    </style>

    <table style="width:100%" cellpadding="0" cellspacing="0">
        <colgroup>
            <col style="width:30%"/>
            <col style="width:10px"/>
        </colgroup>
        <tr style="height:480px">
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:400px">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" RowStyle-CssClass="trDraggable" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="PmHxID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <%=GetLabel("Medical Record History")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div><span><%#: Eval("cfHistoryDateText")%>, <%#: Eval("ServiceUnitName")%></span></div>
                                                    <div><%#: Eval("DepartmentName")%></div>
                                                    <div><%#: Eval("PhysicianName")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
<%--                                            <asp:HyperLinkField HeaderText="Summary" Text="Details" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" ItemStyle-CssClass="lnkMedicalRecordSummary" />--%>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Patient Medical History To Display")%>
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
            <td>&nbsp;</td>
            <td valign="top">
                <ul id="ulAreaDrop">
                    <li class="boxShadow">
                        <img class="imgLoadingMedicalHistory" style="display:none" src='<%= ResolveUrl("~/Libs/Images/Loading.gif")%>' alt="" />
                        <input type="hidden" class="hdnMedicalHistoryVisitID" value="" />
                        <table style="width:100%;height:100%">
                            <tr>
                                <td class="tdMedicalHistoryContent" align="center" valign="top"><div class="divDropHereInfo">Drag and drop medical history here to compare</div></td>
                            </tr>
                        </table>
                    </li>
                    <li class="boxShadow">
                        <img class="imgLoadingMedicalHistory" src='<%= ResolveUrl("~/Libs/Images/Loading.gif")%>' alt="" />
                        <input type="hidden" class="hdnMedicalHistoryVisitID" value="" />
                        <table style="width:100%;height:100%">
                            <tr>
                                <td class="tdMedicalHistoryContent" align="center" valign="top"><div class="divDropHereInfo">Drag and drop medical history here to compare</div></td>
                            </tr>
                        </table>
                    </li>
                </ul>
            </td>
        </tr>
    </table>

    
</asp:Content>

