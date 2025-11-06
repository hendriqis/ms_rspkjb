<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RevenueSharingSummaryEntryAdjTransCtlX.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingSummaryEntryAdjTransCtlX" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_RevenueSharingSummaryEntryAdjTransCtlX">
    $(function () {
    });

    //#region RS Adjustment No
    $('#lblRSAdjustmentNo.lblLink').live('click', function () {
        var filterExpression = "<%=GetFilterExpression() %>";
        openSearchDialog('transrevenuesharingadjustmenthd', filterExpression, function (value) {

            var filterExpression = "RSAdjustmentNo = '" + value + "'";
            Methods.getObject('GetTransRevenueSharingAdjustmentHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnRSAdjustmentID.ClientID %>').val(result.RSAdjustmentID);
                    $('#<%=hdnRSAdjustmentNo.ClientID %>').val(result.RSAdjustmentNo);
                    $('#<%=txtRSAdjustmentNo.ClientID %>').val(result.RSAdjustmentNo);
                    $('#<%=txtRSAdjustmentDate.ClientID %>').val(result.cfRSAdjustmentDateInStringDateFormat);
                    $('#<%=txtTotalAdjustmentAmount.ClientID %>').val(result.cfTotalAdjustmentAmountInString);
                }
                else {
                    $('#<%=hdnRSAdjustmentNo.ClientID %>').val('');
                    $('#<%=txtRSAdjustmentNo.ClientID %>').val('');
                    $('#<%=txtRSAdjustmentDate.ClientID %>').val('');
                    $('#<%=txtTotalAdjustmentAmount.ClientID %>').val('');
                }
            });

            cbpViewAdjCtl.PerformCallback();
        });
    });
    //#endregion

    $('#<%=rblAdjustmentCtl.ClientID%>').live('change', function () {
        $('#<%=hdnAdjustmentGroup.ClientID %>').val($('#<%=rblAdjustmentCtl.ClientID %> input:checked').val());
        cbpViewAdjCtl.PerformCallback();
    });

    function oncbpViewAdjCtlEndCallback(s) {
        hideLoadingPanel();
    }
</script>
<div id="containerPopup">
    <input type="hidden" id="hdnRSSummaryIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnRSSummaryMaxAmountCtl" runat="server" value="" />
    <input type="hidden" id="hdnIsUsedAdjustmentAmountBRUTO" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td align="left" valign="top">
                <table>
                    <colgroup>
                        <col style="width: 150px" />
                    </colgroup>
                    <tr>
                        <td>
                            <input type="hidden" id="hdnRSAdjustmentID" runat="server" />
                            <input type="hidden" id="hdnRSAdjustmentNo" runat="server" />
                            <input type="hidden" id="hdnAdjustmentGroup" runat="server" />
                            <label id="lblRSAdjustmentNo" class="lblLink">
                                <%=GetLabel("Nomor Penyesuaian")%></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtRSAdjustmentNo" Width="150px" ReadOnly="true"
                                Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Penyesuaian")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRSAdjustmentDate" Width="150px" runat="server" ReadOnly="true"
                                Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="lblNormal">
                                <%=GetLabel("Total Penyesuaian") %></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtTotalAdjustmentAmount" Width="150px" ReadOnly="true"
                                Style="text-align: right; color: Purple" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:RadioButtonList runat="server" ID="rblAdjustmentCtl" RepeatDirection="Horizontal" />
                        </td>
                    </tr>
                </table>
            </td>
            <td align="left" valign="top">
                <table>
                    <colgroup>
                        <col style="width: 200px" />
                    </colgroup>
                    <tr>
                        <td>
                            <label class="lblNormal">
                                <%=GetLabel("Nomor Rekap")%></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtRSSummaryNoCtl" Width="150px" ReadOnly="true"
                                Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="lblNormal">
                                <%=GetLabel("Total Rekap") %></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtRSSummaryAmountCtl" Width="150px" ReadOnly="true"
                                Style="text-align: right" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="lblNormal">
                                <%=GetLabel("Total Penyesuaian Maksimal") %></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaxTotalAdjustmentAmountCtl" Width="150px" ReadOnly="true"
                                Style="text-align: right; color: Red" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trcbpViewAdjCtl">
            <td colspan="2">
                <div style="height: 350px; overflow-y: auto; overflow-x: hidden">
                    <dxcp:ASPxCallbackPanel ID="cbpViewAdjCtl" runat="server" Width="100%" ClientInstanceName="cbpViewAdjCtl"
                        ShowLoadingPanel="false" OnCallback="cbpViewAdjCtl_Callback">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="RSAdjustmentType" HeaderText="Tipe" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="300px" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="AdjustmentAmount" HeaderStyle-HorizontalAlign="Right"
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="250px" DataFormatString="{0:N}"
                                                HeaderText="Nilai Penyesuaian Bruto" />
                                            <asp:BoundField DataField="AdjustmentAmount" HeaderStyle-HorizontalAlign="Right"
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="250px" DataFormatString="{0:N}"
                                                HeaderText="Nilai Penyesuaian" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
