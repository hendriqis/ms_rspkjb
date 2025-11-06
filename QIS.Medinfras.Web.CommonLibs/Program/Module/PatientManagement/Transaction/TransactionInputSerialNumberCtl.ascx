<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionInputSerialNumberCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransactionInputSerialNumberCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_TransactionInputSerialNumberCtl">
    $(function () {
        setDatePicker('<%=txtImplantDate.ClientID %>');
        setDatePicker('<%=txtReviewDate.ClientID %>');
        $('#<%=txtImplantDate.ClientID %>').datepicker('option', 'maxDate', '0');
        $('#<%=txtReviewDate.ClientID %>').datepicker('option', 'minDate', '0');
    });

    $('#btnCancel').click(function () {
        $('#containerChargesDtInfoEntryData').hide();
    });

    $('#btnSave').click(function (evt) {
        if (IsValid(evt, 'fsImplant', 'mpImplant'))
            cbpChargesDtInfoSerialNo.PerformCallback('save');
        return false;
    });

    $('.imgEdit.imgLink').die('click')
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=hdnChargesDtIDCtl1.ClientID %>').val(entity.ID);
        $('#<%=txtItemCodeCtl1.ClientID %>').val(entity.ItemCode);
        $('#<%=txtItemNameCtl1.ClientID %>').val(entity.ItemName1);
        $('#<%=txtSerialNoCtl1.ClientID %>').val(entity.SerialNo);

        $('#containerChargesDtInfoEntryData').show();
    });

    function onEndCallbackChargesDtInfoSerialNoEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerChargesDtInfoEntryData').hide();
        }
        hideLoadingPanel();
    }
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnTransactionIDCtl1" />
    <input type="hidden" value="" runat="server" id="hdnVisitIDCtlImplant" />
    <input type="hidden" value="" runat="server" id="hdnTestOrderIDCtlImplant" />
    <input type="hidden" value="" runat="server" id="hdnMRNCtlImplant" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 300px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Transaksi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionNoCtl1" Width="170px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal - Jam Transaksi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionDateTimeCtl1" Width="170px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Unit Transaksi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtServiceUnitNameCtl1" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
                <div id="containerChargesDtInfoEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <input type="hidden" id="hdnChargesDtIDCtl1" runat="server" value="" />
                    <fieldset id="fsImplant" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Tanggal Pemasangan")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtImplantDate" Width="120px" CssClass="datepicker" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Item Code")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtItemCodeCtl1" Width="150px" runat="server" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Item Name")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtItemNameCtl1" Width="100%" runat="server" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Tanggal Review ")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtReviewDate" Width="120px" CssClass="datepicker" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Serial No")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSerialNoCtl1" Width="100%" CssClass="required" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Catatan") %></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtRemarks" runat="server" Width="99%" TextMode="Multiline" Height="250px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnCancel" value='<%= GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpChargesDtInfoSerialNo" runat="server" Width="100%"
                    ClientInstanceName="cbpChargesDtInfoSerialNo" ShowLoadingPanel="false" OnCallback="cbpChargesDtInfoSerialNo_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onEndCallbackChargesDtInfoSerialNoEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlSerialNoGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdChargesSerialNo" runat="server" CssClass="grdView notAllowSelect"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" />
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                <input type="hidden" value="<%#:Eval("SerialNo") %>" bindingfield="SerialNo" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="100px" DataField="ItemCode" HeaderText="Item Code"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ItemName1" HeaderText="Item Name" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderStyle-Width="200px" DataField="SerialNo" HeaderText="Serial No"
                                            HeaderStyle-HorizontalAlign="Left" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingPatientFamily">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
