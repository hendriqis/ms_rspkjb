<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EKlaimEntryClaimFinal.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.EKlaimEntryClaimFinal" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_claimfinal">
    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnFinalClaim.ClientID %>').click(function (evt) {
        var isBridgingToEKlaim = $('#<%=hdnIsBridgingToEKlaim.ClientID %>').val();
        if (isBridgingToEKlaim == "1") {
            ClaimFinal();
        } else {
            showToast('INFORMATION', 'Status bridging dengan e-klaim sedang nonaktif.');
        }
    });

    function ClaimFinal() {
        var nomor_sep = $('#<%:hdnSEPNo.ClientID %>').val();
        var coder_nik = $('#<%=hdnCoderNIK.ClientID %>').val();

        EKlaimService.claimFinal(nomor_sep, coder_nik, function (result) {
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
                            <%=GetLabel("PROCESS")%></div>
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
            <td style="vertical-align:middle">
                <img height="40" src='<%= ResolveUrl("~/Libs/Images/Warning.png")%>' alt='' />
            </td>
        </tr>
    </table>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnIsBridgingToEKlaim" value="" runat="server" />
        <input type="hidden" id="hdnParam" runat="server" value="" />
        <input type="hidden" id="hdnSEPNo" runat="server" value="" />
        <input type="hidden" id="hdnCoderNIK" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 200px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("No SEP")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtSEPNo" Width="180px" runat="server" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Coder NIK")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtCoderNIK" Width="250px" runat="server" ReadOnly="true" />
                </td>
            </tr>
        </table>
    </fieldset>
</div>
