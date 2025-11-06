<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InformasiKontrakInstansiCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.InformasiKontrakInstansiCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<style type="text/css">
    #rightPanel
    {
        border: 1px solid #6E6E6E;
        width: 100%;
        height: 100%;
        position: relative;
    }
    #rightPanel > ul
    {
        margin: 0;
        padding: 2px;
        border-bottom: 1px groove black;
    }
    #rightPanel > ul > li
    {
        list-style-type: none;
        font-size: 12px;
        display: inline-block;
        border: 1px solid #848484;
        padding: 5px 8px;
        cursor: pointer;
    }
    #rightPanel > ul > li.selected
    {
        background-color: #DB2C12;
        color: White;
    }
</style>
<script type="text/javascript" id="dxss_ContractInfoctl">
    $(function () {
        $('#rightPanel ul li').live('click', function () {
            $('#rightPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contractID = $('#<%=hdnContractID.ClientID %>').val();
            var payerID = $('#<%=hdnPayerID.ClientID %>').val();
            var id = payerID + "|" + contractID;
            var url = $(this).attr('url');
            $('#containerLeftPanelContent').html('');
            if (url != '#') {
                $('#divLeftPanelContentLoading').show();
                Methods.getHtmlControl(ResolveUrl(url), id, function (result) {
                    $('#divLeftPanelContentLoading').hide();
                    $('#containerLeftPanelContent').html(result.replace(/\VIEWSTATE/g, ''));
                }, function () {
                    $('#divLeftPanelContentLoading').hide();
                });
            }
        });
        $('#rightPanel ul li').first().click();
    });
</script>
<input type="hidden" id="hdnRegistrationID" value="" runat="server" />
<input type="hidden" id="hdnVisitID" value="" runat="server" />
<input type="hidden" id="hdnContractID" value="" runat="server" />
<input type="hidden" id="hdnPayerID" value="" runat="server" />
<table style="width: 100%" cellpadding="0" cellspacing="0">
    <colgroup>
        <col />
        <col style="width: 10px" />
        <col style="width: 36%" />
    </colgroup>
    <tr style="height: 300px" runat="server">
        <td style="height: 550px; width: 100%;vertical-align: top">
            <div id="rightPanel">
                <ul>
                    <li url="~/Libs/Program/Information/ContractSummaryViewCtl.ascx" title="Ringkasan Kontrak">
                        Summaries</li>
                    <li url="~/Libs/Program/Information/InfoDocumentCustomerCtl.ascx" title="Dokumen Kontrak Rekanan">
                        Document</li>
                    <li url="~/Libs/Program/Information/CustomerContractNotesViewCtl.ascx" title="Catatan Kontrak">
                        Notes</li>
                </ul>
                <div id="containerLeftPanelContent">
                </div>
                <div id="divLeftPanelContentLoading" style="position: absolute; top: 30%; left: 48%;
                    display: none">
                    <div style="margin: 0 auto">
                        <img src="<%= ResolveUrl("~/Libs/Images/Loading.gif")%>" alt="" />
                    </div>
                </div>
            </div>
        </td>
    </tr>
</table>
