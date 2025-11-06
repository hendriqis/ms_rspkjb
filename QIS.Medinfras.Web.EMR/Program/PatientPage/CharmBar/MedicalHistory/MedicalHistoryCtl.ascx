<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicalHistoryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.MedicalHistoryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script id='dxis_medicalhistoryctl10' src='<%= ResolveUrl("~/Libs/Scripts/jquery/jquery-1.7.min.js")%>' type='text/javascript'></script>
<script id='dxis_medicalhistoryctl11' src='<%= ResolveUrl("~/Libs/Scripts/jquery/jquery.paginate.js")%>' type='text/javascript'></script>
<script id="dxis_medicalhistoryctl1" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.core.js")%>'></script>
<script id="dxis_medicalhistoryctl2" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.widget2.js")%>'></script>
<script id="dxis_medicalhistoryctl3" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.mouse.js")%>'></script>
<script id="dxis_medicalhistoryctl4" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.draggable.js")%>'></script>
<script id="dxis_medicalhistoryctl5" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.droppable.js")%>'></script>
<script id="dxis_medicalhistoryctl6" type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.effects.core.js")%>'></script>
<script id='dxis_medicalhistoryctl7' src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery-ui.js")%>' type='text/javascript'></script>
<script id='dxis_medicalhistoryctl8' src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.easing.1.3.js")%>' type='text/javascript'></script>
<script id='dxis_medicalhistoryctl9' src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.booklet.1.3.1.js")%>' type='text/javascript'></script>
<script type="text/javascript">
    function onRefreshControl(filterExpression) {
        cbpView.PerformCallback('refresh');
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#paging"), pageCount, function (page) {
            loadMedicalHistoryGridContent(page);
        });
        loadMedicalHistoryGridContent(1);
    });

    function loadMedicalHistoryGridContent(page) {
        showLoadingPanel();
        var url = ResolveUrl('~/Program/PatientPage/CharmBar/MedicalHistory/MedicalHistoryGridContentCtl.ascx');
        Methods.getHtmlControl(url, page, function (result) {
            $('#<%=pnlView.ClientID %>').html(result.replace(/\VIEWSTATE/g, ''));
            onCharmbarMedicalHistoryEndCallback();
        }, function () {
            onCharmbarMedicalHistoryEndCallback();
        });
    }

    function onCharmbarMedicalHistoryEndCallback(s) {
        hideLoadingPanel();
        registerEventHandler();
        $('.lnkMedicalRecordSummary a').click(function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/PatientPage/Information/EpisodeSummary/EpisodeSummaryCtl.ascx");
            openUserControlPopupTopCharmBar(url, id, 'Medical Record - Detail', 1300, 600);
        });
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
                    var url = ResolveUrl('~/Program/PatientPage/HealthRecord/MedicalHistory/MedicalHistoryContentCtl.ascx');
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
    });

</script>
<style type="text/css">
    #ulAreaDrop         { margin: 0; padding: 0; height: 390px; }
    #ulAreaDrop > li    { width:250px; border:1px solid #AAA; height :100%; z-index: 10000; display: inline-block; list-style-type: none; margin-right: 10px; margin-left: 10px; position: relative; }
    .trDraggable        { cursor: pointer; z-index: 19999; }
    .divDropHereInfo    { color: Gray; width:60%;text-align: center; margin-top:100px; }
    #ulAreaDrop > li .imgLoadingMedicalHistory { display: none; position: absolute; left: 44%; top: 44%; } 
</style>

<table style="width:100%" cellpadding="0" cellspacing="0">
    <colgroup>
        <col style="width:36%"/>
        <col style="width:10px"/>
    </colgroup>
    <tr style="height:480px">
        <td valign="top">
            <div style="position: relative;">
                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:300px"></asp:Panel>
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
                            <td class="tdMedicalHistoryContent" align="center" valign="top"><div class="divDropHereInfo">TEST</div></td>
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
