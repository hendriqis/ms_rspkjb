<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EKlaimEntrySendClaimPerDate.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.EKlaimEntrySendClaimPerDate" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_sendclaimpersep">
    $(function () {
        hideLoadingPanel();

        setDatePicker('<%=txtParameterDateFrom.ClientID %>');
        $('#<%=txtParameterDateFrom.ClientID %>').datepicker('option', 'maxDate', '0');

        setDatePicker('<%=txtParameterDateTo.ClientID %>');
        $('#<%=txtParameterDateTo.ClientID %>').datepicker('option', 'maxDate', '0');
    });

    $('#<%=btnFinalClaim.ClientID %>').click(function (evt) {
        var isBridgingToEKlaim = $('#<%=hdnIsBridgingToEKlaim.ClientID %>').val();
        if (isBridgingToEKlaim == "1") {
            SendClaimPerSEP();
        } else {
            showToast('INFORMATION', 'Status bridging dengan e-klaim sedang nonaktif.');
        }
    });

    function SendClaimPerSEP() {
        var nomor_sep = $('#<%:hdnSEPNo.ClientID %>').val();

        EKlaimService.sendClaim(nomor_sep, function (result) {
            if (result != null) {
                showToast('INFORMATION', result);
            }
        });
    }
</script>
<div class="toolbarArea">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <colgroup>
            <col width="40%" />
            <col width="60%" />
        </colgroup>
        <tr>
            <td>
                <ul id="ulMPListToolbar" runat="server">
                    <li id="btnFinalClaim" runat="server">
                        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
                        <div>
                            <%=GetLabel("SEND")%></div>
                    </li>
                </ul>
            </td>
            <td style="text-align: right; padding-right: 5px">
                <div style="font-size: larger; color: Red">
                    <b>
                        <%=GetLabel("Data yang sudah diproses,")%></b>
                    <br />
                    <b>
                        <%=GetLabel("tidak bisa dikembalikan lagi.")%></b></div>
            </td>
            <td style="vertical-align: middle">
                <img height="40" src='<%= ResolveUrl("~/Libs/Images/Warning.png")%>' alt='' />
            </td>
        </tr>
    </table>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnIsBridgingToEKlaim" value="" runat="server" />
        <input type="hidden" id="hdnParam" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 400px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtParameterDateFrom" Width="120px" CssClass="datepicker" runat="server" />
                    <label class="lblNormal">
                        <%=GetLabel("s/d")%></label>
                    <asp:TextBox ID="txtParameterDateTo" Width="120px" CssClass="datepicker" runat="server" />
                </td>
            </tr>
        </table>
    </fieldset>
</div>
