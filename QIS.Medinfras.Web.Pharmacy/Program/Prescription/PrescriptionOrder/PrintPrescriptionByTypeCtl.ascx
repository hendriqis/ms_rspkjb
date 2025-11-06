<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintPrescriptionByTypeCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.PrintPrescriptionByTypeCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_PrintPrescriptionByTypeCtl">
    $('#btnPrintCtl').click(function (evt) {
        var transactionID = $('#<%=hdnTransactionIDCtl.ClientID %>').val();
        var printType = cboPrintType.GetValue();
        var filterExpression = transactionID;
        if (printType == 1) {
            filterExpression += "~";
        }
        openReportViewer("PH-00057", filterExpression);
    });
</script>
<style type="text/css">
    .highlight
    {
        background-color: #FE5D15;
        color: White;
    }
</style>
<input type="hidden" id="hdnTransactionIDCtl" runat="server" />
<div style="height: 100px; overflow-y: auto">
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table>
                    <colgroup>
                        <col width="115px" />
                        <col width="100px" />
                        <col width="115px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Resep")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTransactionNo" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="lblNormal">
                                <%=GetLabel("Pilihan Cetak")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboPrintType" ClientInstanceName="cboPrintType" Width="200px"
                                runat="server">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <input type="button" id="btnPrintCtl" value='<%= GetLabel("PRINT")%>' />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
