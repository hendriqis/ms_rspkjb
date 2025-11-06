<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryBodyDiagramContainerCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.EpisodeSummaryBodyDiagramContainerCtl" %>

<script type="text/javascript" id="dxss_episodesummaryctl">
    $(function () {
        episodeSummaryBodyDiagramLoadControl();

        $('#btnBodyDiagramContainerPrev').click(function () {
            var lstID = $('#<%=hdnIDBodyDiagram.ClientID %>').val().split('|');
            var idx = $('#<%=hdnBodyDiagramIdx.ClientID %>').val();
            idx--;
            if (idx < 0)
                idx = lstID.length - 1;
            $('#<%=hdnBodyDiagramIdx.ClientID %>').val(idx);
            episodeSummaryBodyDiagramLoadControl();
        });
        $('#btnBodyDiagramContainerNext').click(function () {
            var lstID = $('#<%=hdnIDBodyDiagram.ClientID %>').val().split('|');
            var idx = $('#<%=hdnBodyDiagramIdx.ClientID %>').val();
            idx++;
            if (idx == lstID.length)
                idx = 0;
            $('#<%=hdnBodyDiagramIdx.ClientID %>').val(idx);
            episodeSummaryBodyDiagramLoadControl();
        });
    });

    function episodeSummaryBodyDiagramLoadControl() {
        if ($('#<%=hdnIDBodyDiagram.ClientID %>').val() != '') {
            $('#containerEpisodeSummaryBodyDiagramCtn').html('');
            var lstID = $('#<%=hdnIDBodyDiagram.ClientID %>').val().split('|');
            var idx = $('#<%=hdnBodyDiagramIdx.ClientID %>').val();
            Methods.getHtmlControl("~/Program/PatientPage/Summary/EpisodeSummary/EpisodeSummaryBodyDiagramContentCtl.ascx", lstID[idx], function (result) {
                $('#containerEpisodeSummaryBodyDiagramCtn').html(result);
            });
        }
    }
</script>
<input type="hidden" value="" runat="server" id="hdnIDBodyDiagram" />
<input type="hidden" value="0" runat="server" id="hdnBodyDiagramIdx" />
<table style="float: right;margin-top: 0px;">
    <tr>
        <td><img src='<%=ResolveUrl("~/Libs/Images/Icon/prev_record.png") %>' title="Prev" width="25px" alt="" class="imgLink" id="btnBodyDiagramContainerPrev" style="margin-left: 5px;" /></td>
        <td><img src='<%=ResolveUrl("~/Libs/Images/Icon/next_record.png") %>' title="Next" width="25px" alt="" class="imgLink" id="btnBodyDiagramContainerNext" style="margin-left: 5px;" /></td>
    </tr>
</table>
<h3 class="headerContent" style="padding-left:5px;"><%=GetLabel("Body Diagram Information")%></h3>
<div style="clear:both;width:100%; height:100%; padding:5px;" class="borderBox" id="containerEpisodeSummaryBodyDiagramCtn">


</div>