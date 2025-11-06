<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LaboratoryResultBridgingCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.LaboratoryResultBridgingCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    $('.lnkDetail a').live('click', function () {
    });

    $('.lblPrintPDF.lblLink').die('click');
    $('.lblPrintPDF.lblLink').live('click', function () {
        $row = $(this).parent().closest('tr');
        var entity = rowToObject($row);
        window.open("data:application/pdf;base64, " + entity.TextValue, "popupWindow", "width=600, height=600,scrollbars=yes");
    });

    $('.lblReffRange.lblLink').die('click');
    $('.lblReffRange.lblLink').live('click', function () {
        $row = $(this).parent().closest('tr');
        var entity = rowToObject($row);
        showToast('Nilai Referensi', entity.cfReferenceRange);
    });

    $('.lblCommentDetail.lblLink').die('click');
    $('.lblCommentDetail.lblLink').live('click', function () {
        $row = $(this).parent().closest('tr');
        var entity = rowToObject($row);
        var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();

        var filterExpressionOrder = "ChargeTransactionID = '" + transactionID + "' AND ItemID = '" + entity.ItemID + "' AND FractionID = '" + entity.FractionID + "'";
        Methods.getObject('GetvLaboratoryResultDtList', filterExpressionOrder, function (result1) {
            if (result1 != null) {
                showToast('Comment', result1.Remarks);
            }
            else {
                var filterExpressionChargesInfo = "ID = (SELECT ID FROM PatientChargesDt WHERE TransactionID = " + transactionID + " AND ItemID = " + entity.ItemID + " AND IsDeleted = 0)";
                Methods.getObject('GetPatientChargesDtInfoList', filterExpressionChargesInfo, function (result2) {
                    if (result2 != null) {
                        showToast('Comment', result2.Remarks);
                    }
                });
            }
        });
    });
</script>
<style type="text/css">
    .tblTestItem
    {
        border: 1px solid #AAA;
        margin-top: 10px;
    }
    .tblTestItem > tbody > tr > th
    {
        background-color: #EAEAEA;
        padding: 5px;
        border-bottom: 1px solid #AAA;
    }
    .tblTestItem > tbody > tr > td
    {
        padding: 5px;
    }
</style>
<div style="height: 370px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnTestOrderID" value="" />
    <input type="hidden" runat="server" id="hdnTransactionID" value="" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <table style="width: 100%" cellpadding="0" cellspacing="0">
        <colgroup>
            <col style="width: 50%" />
            <col />
        </colgroup>
        <tr>
            <td valign="top">
                <table id="Table2" runat="server" cellpadding="0">
                    <colgroup>
                        <col style="width: 250px" />
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Nomor Laboratorium")%>
                        </td>
                        <td colspan="2" style="padding-left: 5px">
                            <asp:TextBox ID="txtTransactionNo" ReadOnly="true" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Tanggal / Jam Hasil")%>
                        </td>
                        <td style="padding-left: 5px">
                            <asp:TextBox ID="txtResultDate" ReadOnly="true" Width="100%" runat="server" Style="text-align: center" />
                        </td>
                        <td style="padding-left: 6px">
                            <asp:TextBox ID="txtResultTime" ReadOnly="true" Width="100%" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table id="Table1" runat="server" cellpadding="0">
                    <colgroup>
                        <col style="width: 250px" />
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Nomor Order")%>
                        </td>
                        <td colspan="2" style="padding-left: 5px">
                            <asp:TextBox ID="txtOrderNumber" ReadOnly="true" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Tanggal / Jam Order")%>
                        </td>
                        <td style="padding-left: 5px">
                            <asp:TextBox ID="txtTestOrderDate" ReadOnly="true" Width="100%" runat="server" Style="text-align: center" />
                        </td>
                        <td style="padding-left: 6px">
                            <asp:TextBox ID="txtTestOrderTime" Width="100%" ReadOnly="true" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Diorder Oleh") %>
                        </td>
                        <td colspan="2" style="padding-left: 5px">
                            <asp:TextBox ID="txtOrderedBy" ReadOnly="true" runat="server" Width="100%" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
        <Columns>
            <asp:TemplateField HeaderStyle-Width="5%">
                <ItemTemplate>
                    <input type="hidden" value="<%#:Eval("TextValue") %>" bindingfield="TextValue" />
                    <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                    <input type="hidden" value="<%#:Eval("TestOrderID") %>" bindingfield="TestOrderID" />
                    <input type="hidden" value="<%#:Eval("ChargeTransactionID") %>" bindingfield="ChargeTransactionID" />
                </ItemTemplate>
                <HeaderStyle Width="50px"></HeaderStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Pemeriksaan" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <div class='<%#: Eval("IsNormal").ToString() == "False" ?  (Eval("cfIsPanicRange").ToString() == "False" ? "isAbnormalColor" : "isPanicRangeColor") : "" %>'>
                        <%#: Eval("FractionName1") %></div>
                    <div style="font-style: italic">
                        <%#: Eval("ItemName1") %></div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Hasil" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                ItemStyle-Width="150px">
                <ItemTemplate>
                    <div class='<%#: Eval("IsNormal").ToString() == "False" ?  (Eval("cfIsPanicRange").ToString() == "False" ? "isAbnormalColor" : "isPanicRangeColor") : "" %>'
                        style="text-align: right">
                        <div class='<%#: Eval("cfIsResultInPDF").ToString() != "False" ? "lblLink lblPrintPDF" : "" %>'>
                            <asp:Literal ID="literal" Text='<%# Eval("cfTestResultValue") %>' Mode="PassThrough"
                                runat="server" /></div>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Satuan" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Left"
                ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <div class='<%#: Eval("IsNormal").ToString() == "False" ?  (Eval("cfIsPanicRange").ToString() == "False" ? "isAbnormalColor" : "isPanicRangeColor") : "" %>'>
                        <%#: Eval("MetricUnit") %></div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Nilai Referensi" ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <div class='<%#: Eval("cfIsReferenceAsLink").ToString() == "True" ? "lblLink lblReffRange" : "" %>'>
                        <asp:Literal ID="literal" Text='<%# Eval("cfReferenceRangeCustom") %>' Mode="PassThrough"
                            runat="server" /></div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Flag" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center"
                ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <div class='<%#: Eval("IsNormal").ToString() == "False" ?  (Eval("cfIsPanicRange").ToString() == "False" ? "isAbnormalColor" : "isPanicRangeColor") : "" %>'
                        style="text-align: right">
                        <%#: Eval("ResultFlag") %></div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:HyperLinkField HeaderStyle-Width="50px" Text="View" ItemStyle-HorizontalAlign="Center"
                ItemStyle-CssClass="lnkDetail" HeaderStyle-HorizontalAlign="Center" HeaderText="Text Result"
                Visible="false" />
            <asp:TemplateField HeaderText="" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left"
                ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <div class='lblLink lblCommentDetail'>
                        <%=GetLabel("Comment")%>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Verifikasi" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Left"
                ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <div>
                        <%#: Eval("VerifiedUserName") %></div>
                    <div>
                        <%#: Eval("VerifiedDateInString") %></div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <%=GetLabel("No Data To Display")%>
        </EmptyDataTemplate>
    </asp:GridView>
    <table width="50%">
        <tr>
            <td>
                <b>
                    <%=GetLabel("Remarks : ") %></b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtRemarksHd" Width="100%" runat="server" ReadOnly="true" TextMode="MultiLine"
                    Rows="5" />
            </td>
        </tr>
    </table>
</div>
