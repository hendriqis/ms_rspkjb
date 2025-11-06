<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true" 
    CodeBehind="PatientDocumentLibList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PatientDocumentLibList" EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript" id="dxss_episodesummaryctl">
        $(function () {
            charmBarDocumentLoadControl();

            $('#btncharmBarDocumentContainerPrev').click(function () {
                var lstID = $('#<%=hdnRecordID.ClientID %>').val().split('|');
                var idx = $('#<%=hdnDocumentIdx.ClientID %>').val();
                idx--;
                if (idx < 0)
                    idx = lstID.length - 1;
                $('#<%=hdnDocumentIdx.ClientID %>').val(idx);
                charmBarDocumentLoadControl();
            });
            $('#btncharmBarDocumentContainerNext').click(function () {
                var lstID = $('#<%=hdnRecordID.ClientID %>').val().split('|');
                var idx = $('#<%=hdnDocumentIdx.ClientID %>').val();
                idx++;
                if (idx == lstID.length)
                    idx = 0;
                $('#<%=hdnDocumentIdx.ClientID %>').val(idx);
                charmBarDocumentLoadControl();
            });
        });

        function charmBarDocumentLoadControl() {
            if ($('#<%=hdnRecordID.ClientID %>').val() != '') {
                $('#containercharmBarDocumentCtn').html('');
                var lstID = $('#<%=hdnRecordID.ClientID %>').val().split('|');
                var idx = $('#<%=hdnDocumentIdx.ClientID %>').val();
                Methods.getHtmlControl("~/Program/PatientPage/CharmBar/Document/PatientDocumentContentCtl.ascx", lstID[idx], function (result) {
                    $('#containercharmBarDocumentCtn').html(result.replace(/\VIEWSTATE/g, ''));
                });
            }
        }
    </script>
    <input type="hidden" value="" runat="server" id="hdnRecordID" />
    <input type="hidden" value="0" runat="server" id="hdnDocumentIdx" />
    <table style="float: right;margin-top: 0px;">
        <tr>
            <td><img src='<%=ResolveUrl("~/Libs/Images/Icon/prev_record.png") %>' title="Prev" width="25px" alt="" class="imgLink" id="btncharmBarDocumentContainerPrev" style="margin-left: 5px;" /></td>
            <td><img src='<%=ResolveUrl("~/Libs/Images/Icon/next_record.png") %>' title="Next" width="25px" alt="" class="imgLink" id="btncharmBarDocumentContainerNext" style="margin-left: 5px;" /></td>
        </tr>
    </table>
    <h3 class="headerContent" style="padding-left:5px;"><%=GetLabel("Patient Document Information")%></h3>
    <div style="clear:both;width:100%; height:100%; padding:5px;" class="borderBox" id="containercharmBarDocumentCtn">


    </div>
</asp:Content>

