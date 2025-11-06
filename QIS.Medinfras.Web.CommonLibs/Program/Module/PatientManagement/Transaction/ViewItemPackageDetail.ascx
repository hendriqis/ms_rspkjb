<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewItemPackageDetail.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ViewItemPackageDetail" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#ulTabLabResult li').click(function () {
        $('#ulTabLabResult li.selected').removeAttr('class');
        $('.containerTransDtCtl').filter(':visible').hide();
        $contentID = $(this).attr('contentid');
        $('#' + $contentID).show();
        $(this).addClass('selected');
    });
</script>
<div style="height:500px; overflow-y:auto; overflow-x:hidden">
    <input type="hidden" id="hdnGCItemType" runat="server" value="" />
    <div class="containerUlTabPage">
        <ul class="ulTabPage" id="ulTabLabResult">
            <li class="selected" contentid="containerServiceCtl">
                <%=GetLabel("PELAYANAN") %></li>
            <li contentid="containerDrugMSCtl">
                <%=GetLabel("OBAT & ALKES") %></li>
            <li contentid="containerLogisticsCtl">
                <%=GetLabel("BARANG UMUM") %></li>
        </ul>
    </div>
    <div id="containerServiceCtl" class="containerTransDtCtl">
        <table class="tblContentArea">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 90%">
                        <colgroup>
                            <col style="width: 120px" />
                            <col style="width: 400px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Item")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemServiceName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                        margin-right: auto; position: relative; font-size: 0.95em;">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                            OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ItemType" HeaderText="Jenis Item" />
                                <asp:BoundField DataField="DetailItemName1" HeaderText="Nama Detail Item" />
                                <asp:BoundField DataField="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                    HeaderText="Jumlah" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Data Tidak Tersedia")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <div style="width: 100%; text-align: right">
            <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
        </div>
    </div>
    <div id="containerDrugMSCtl" style="display: none" class="containerTransDtCtl">
        <table class="tblContentArea">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 90%">
                        <colgroup>
                            <col style="width: 120px" />
                            <col style="width: 400px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Item")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemServiceName3" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <asp:Panel runat="server" ID="pnlEntryPopupgrdViewObat" Style="width: 100%; margin-left: auto;
                        margin-right: auto; position: relative; font-size: 0.95em;">
                        <asp:GridView ID="grdViewObat" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                            OnRowDataBound="grdViewObat_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ItemType" HeaderText="Jenis Item" />
                                <asp:BoundField DataField="DetailItemName1" HeaderText="Nama Detail Item" />
                                <asp:BoundField DataField="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                    HeaderText="Jumlah" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Data Tidak Tersedia")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <div style="width: 100%; text-align: right">
            <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
        </div>
    </div>
    <div id="containerLogisticsCtl" style="display: none" class="containerTransDtCtl">
        <table class="tblContentArea">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 90%">
                        <colgroup>
                            <col style="width: 120px" />
                            <col style="width: 400px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Item")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemServiceName2" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <asp:Panel runat="server" ID="pnlEntryPopupgrdViewBarang" Style="width: 100%; margin-left: auto;
                        margin-right: auto; position: relative; font-size: 0.95em;">
                        <asp:GridView ID="grdViewBarang" runat="server" CssClass="grdView notAllowSelect"
                            AutoGenerateColumns="false" OnRowDataBound="grdViewBarang_RowDataBound" ShowHeaderWhenEmpty="true"
                            EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ItemType" HeaderText="Jenis Item" />
                                <asp:BoundField DataField="DetailItemName1" HeaderText="Nama Detail Item" />
                                <asp:BoundField DataField="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                    HeaderText="Jumlah" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Data Tidak Tersedia")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <div style="width: 100%; text-align: right">
            <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
        </div>
    </div>
</div>
