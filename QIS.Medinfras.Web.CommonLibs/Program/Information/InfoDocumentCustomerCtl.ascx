<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoDocumentCustomerCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Information.InfoDocumentCustomerCtl" %>

<script type="text/javascript" id="dxss_episodesummaryctl">
    $(function () {
        infoDocumentCustomerLoadControl();

        $('#btnContainerPrev').click(function () {
            var lstID = $('#<%=hdnContractID.ClientID %>').val().split('|');
            var idx = $('#<%=hdnCustomerDocumentIdx.ClientID %>').val();
            idx--;
            if (idx < 0)
                idx = lstID.length - 1;
            $('#<%=hdnCustomerDocumentIdx.ClientID %>').val(idx);
            infoDocumentCustomerLoadControl();
        });
        $('#btnContainerNext').click(function () {
            var lstID = $('#<%=hdnContractID.ClientID %>').val().split('|');
            var idx = $('#<%=hdnCustomerDocumentIdx.ClientID %>').val();
            idx++;
            if (idx == lstID.length)
                idx = 0;
            $('#<%=hdnCustomerDocumentIdx.ClientID %>').val(idx);
            infoDocumentCustomerLoadControl();
        });
    });

    function infoDocumentCustomerLoadControl() {
        if ($('#<%=hdnContractID.ClientID %>').val() != '') {
            $('#containerInfoDocumentContent').html('');
            var lstID = $('#<%=hdnContractID.ClientID %>').val().split('|');
            var idx = $('#<%=hdnCustomerDocumentIdx.ClientID %>').val();
            Methods.getHtmlControl("~/libs/Program/Information/InfoDocumentCustomerContentCtl.ascx", lstID[idx], function (result) {
                $('#containerInfoDocumentContent').html(result);
            });
        }
    }
</script>
<input type="hidden" value="0" runat="server" id="hdnPayerID" />
<input type="hidden" value="0" runat="server" id="hdnContractID" />
<input type="hidden" value="0" runat="server" id="hdnCustomerDocumentIdx" />
<table style="float: right;margin-top: 0px;">
    <tr>
        <td><img src='<%=ResolveUrl("~/Libs/Images/Icon/prev_record.png") %>' title="Prev" width="25px" alt="" class="imgLink" id="btnContainerPrev" style="margin-left: 5px;" /></td>
        <td><img src='<%=ResolveUrl("~/Libs/Images/Icon/next_record.png") %>' title="Next" width="25px" alt="" class="imgLink" id="btnContainerNext" style="margin-left: 5px;" /></td>
    </tr>
</table>
<h3 class="headerContent" style="padding-left:5px;"><%=GetLabel("Information Customer Documents")%></h3>
<div style="clear:both;width:100%; height:100%; padding:5px;" class="borderBox" id="containerInfoDocumentContent">
</div>