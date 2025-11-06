<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BodyDiagramListCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.BodyDiagramListCtl" %>
<script type="text/javascript" id="dxss_episodesummaryctl">
    $(function () {
        charmBarBodyDiagramLoadControl();

        $('#btnCharmBarBodyDiagramContainerPrev').click(function () {
            var lstID = $('#<%=hdnIDBodyDiagram.ClientID %>').val().split('|');
            var idx = $('#<%=hdnBodyDiagramIdx.ClientID %>').val();
            idx--;
            if (idx < 0)
                idx = lstID.length - 1;
            $('#<%=hdnBodyDiagramIdx.ClientID %>').val(idx);
            episodeSummaryBodyDiagramLoadControl();
        });
        $('#btnCharmBarBodyDiagramContainerNext').click(function () {
            var lstID = $('#<%=hdnIDBodyDiagram.ClientID %>').val().split('|');
            var idx = $('#<%=hdnBodyDiagramIdx.ClientID %>').val();
            idx++;
            if (idx == lstID.length)
                idx = 0;
            $('#<%=hdnBodyDiagramIdx.ClientID %>').val(idx);
            charmBarBodyDiagramLoadControl();
        });
    });

    function charmBarBodyDiagramLoadControl() {
        if ($('#<%=hdnIDBodyDiagram.ClientID %>').val() != '') {
            $('#containerCharmBarBodyDiagramCtn').html('');
            var lstID = $('#<%=hdnIDBodyDiagram.ClientID %>').val().split('|');
            var idx = $('#<%=hdnBodyDiagramIdx.ClientID %>').val();
            Methods.getHtmlControl("~/Program/PatientPage/CharmBar/BodyDiagram/BodyDiagramContentCtl.ascx", lstID[idx], function (result) {
                $('#containerCharmBarBodyDiagramCtn').html(result.replace(/\VIEWSTATE/g, ''));
            });
        }
    }
</script>
<input type="hidden" value="" runat="server" id="hdnIDBodyDiagram" />
<input type="hidden" value="0" runat="server" id="hdnBodyDiagramIdx" />
<table style="float: right;margin-top: 0px;">
    <tr>
        <td><img src='<%=ResolveUrl("~/Libs/Images/Icon/prev_record.png") %>' title="Prev" width="25px" alt="" class="imgLink" id="btnCharmBarBodyDiagramContainerPrev" style="margin-left: 5px;" /></td>
        <td><img src='<%=ResolveUrl("~/Libs/Images/Icon/next_record.png") %>' title="Next" width="25px" alt="" class="imgLink" id="btnCharmBarBodyDiagramContainerNext" style="margin-left: 5px;" /></td>
    </tr>
</table>
<h3 class="headerContent" style="padding-left:5px;"><%=GetLabel("Body Diagram Information")%></h3>
<div style="clear:both;width:100%; height:100%; padding:5px;" class="grow borderBox" id="containerCharmBarBodyDiagramCtn">


</div>