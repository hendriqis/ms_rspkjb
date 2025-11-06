<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisitTransactionHistoryDtCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.VisitTransactionHistoryDtCtl" %>
<script type="text/javascript" id="dxss_patiententryctl">

    $('#ulMain li').click(function () {
        $('#ulMain li.selected').removeAttr('class');
        $('.containerMain').filter(':visible').hide();
        $contentID = $(this).attr('contentid');
        $('#' + $contentID).show();
        $(this).addClass('selected');
    });

</script>
    <div class="containerUlTabPage">
        <ul class="ulTabPage" id="ulMain">
            <li class="selected" contentid="containerPelayanan">
                <%=GetLabel("Pelayanan") %></li>
            <li contentid="containerObatMedis">
                <%=GetLabel("Obat & Alkes") %></li>
            <li contentid="containerBarangUmum">
                <%=GetLabel("BarangUmum") %></li>
            <li contentid="containerRadiologi">
                <%=GetLabel("Radiologi") %></li>
            <li contentid="containerLaboratorium">
                <%=GetLabel("Laboratorium") %></li>
            <li contentid="containerOtherDiagnose">
                <%=GetLabel("Penunjang Medis") %></li>
        </ul>
    </div>
</div>
<div id="containerPelayanan" class="containerMain" style="height:500px; overflow-y:scroll;">
    <asp:ListView ID="lvwService" runat="server">
        <EmptyDataTemplate>
            <table id="tblView" runat="server" class="grdService grdNormal" cellspacing="0" rules="all">
                <tr>
                    <th style="width: 150px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("TransactionNo")%>
                        </div>
                    </th>
                    <th style="width: 100px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Kode Item")%>
                        </div>
                    </th>
                    <th>
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Nama Item")%>
                        </div>
                    </th>
                    <th style="width: 200px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Dokter")%>
                        </div>
                    </th>
                    <th style="width: 80px">
                        <div style="text-align: right; padding-right: 3px">
                            <%=GetLabel("Jumlah")%>
                        </div>
                    </th>
                </tr>
                <tr class="trEmpty">
                    <td colspan="20">
                        <%=GetLabel("No Data To Display") %>
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table id="tblView" runat="server" class="grdService grdNormal" cellspacing="0" rules="all">
                <tr>
                    <th style="width: 150px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("TransactionNo")%>
                        </div>
                    </th>
                    <th style="width: 100px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Kode Item")%>
                        </div>
                    </th>
                    <th>
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Nama Item")%>
                        </div>
                    </th>
                    <th style="width: 200px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Dokter")%>
                        </div>
                    </th>
                    <th style="width: 80px">
                        <div style="text-align: right; padding-right: 3px">
                            <%=GetLabel("Jumlah")%>
                        </div>
                    </th>
                </tr>
                <tr runat="server" id="itemPlaceholder">
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <div style="padding: 3px;">
                        <div class="divTransactionNo">
                            <%#: Eval("TransactionNo") %></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px;">
                            <%#: Eval("ItemCode") %></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px">
                        <div>
                            <%#: Eval("ItemName1")%></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px">
                        <div>
                            <%#: Eval("ParamedicName")%></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px; text-align: right;">
                        <div>
                            <%#: Eval("CustomQtySatuan")%></div>
                    </div>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</div>
<div id="containerObatMedis" style="display: none; height:500px; overflow-y:scroll;" class="containerMain">
    <asp:ListView ID="lvwDrugMS" runat="server">
        <EmptyDataTemplate>
            <table id="tblView" runat="server" class="grdService grdNormal" cellspacing="0" rules="all">
                <tr>
                    <th style="width: 150px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("TransactionNo")%>
                        </div>
                    </th>
                    <th style="width: 200px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Lokasi")%>
                        </div>
                    </th>
                    <th style="width: 100px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Kode Item")%>
                        </div>
                    </th>
                    <th>
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Nama Item")%>
                        </div>
                    </th>
                    <th style="width: 230px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Dokter")%>
                        </div>
                    </th>
                    <th style="width: 150px">
                        <div style="text-align: right; padding-right: 3px">
                            <%=GetLabel("Jumlah")%>
                        </div>
                    </th>
                </tr>
                <tr class="trEmpty">
                    <td colspan="20">
                        <%=GetLabel("No Data To Display") %>
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table id="tblView" runat="server" class="grdService grdNormal" cellspacing="0" rules="all">
                <tr>
                    <th style="width: 150px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("TransactionNo")%>
                        </div>
                    </th>
                    <th style="width: 200px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Lokasi")%>
                        </div>
                    </th>
                    <th style="width: 100px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Kode Item")%>
                        </div>
                    </th>
                    <th>
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Nama Item")%>
                        </div>
                    </th>
                    <th style="width: 230px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Dokter")%>
                        </div>
                    </th>
                    <th style="width: 150px">
                        <div style="text-align: right; padding-right: 3px">
                            <%=GetLabel("Jumlah")%>
                        </div>
                    </th>
                </tr>
                <tr runat="server" id="itemPlaceholder">
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <div style="padding: 3px;">
                        <div class="divTransactionNo">
                            <%#: Eval("TransactionNo") %></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px;">
                            <%#: Eval("LocationName") %></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px;">
                            <%#: Eval("ItemCode") %></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px">
                        <div>
                            <%#: Eval("ItemName1")%></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px">
                        <div>
                            <%#: Eval("ParamedicName")%></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px; text-align: right;">
                        <div>
                            <%#: Eval("CustomQtySatuan")%></div>
                    </div>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</div>
<div id="containerBarangUmum" style="display: none; height:500px; overflow-y:scroll;" class="containerMain">
    <asp:ListView ID="lvwLogistic" runat="server">
        <EmptyDataTemplate>
            <table id="tblView" runat="server" class="grdService grdNormal" cellspacing="0" rules="all">
                <tr>
                    <th style="width: 150px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("TransactionNo")%>
                        </div>
                    </th>
                    <th style="width: 100px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Kode Item")%>
                        </div>
                    </th>
                    <th>
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Nama Item")%>
                        </div>
                    </th>
                    <th style="width: 200px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Dokter")%>
                        </div>
                    </th>
                    <th style="width: 80px">
                        <div style="text-align: right; padding-right: 3px">
                            <%=GetLabel("Jumlah")%>
                        </div>
                    </th>
                </tr>
                <tr class="trEmpty">
                    <td colspan="20">
                        <%=GetLabel("No Data To Display") %>
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table id="tblView" runat="server" class="grdService grdNormal" cellspacing="0" rules="all">
                <tr>
                    <th style="width: 150px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("TransactionNo")%>
                        </div>
                    </th>
                    <th style="width: 100px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Kode Item")%>
                        </div>
                    </th>
                    <th>
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Nama Item")%>
                        </div>
                    </th>
                    <th style="width: 200px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Dokter")%>
                        </div>
                    </th>
                    <th style="width: 80px">
                        <div style="text-align: right; padding-right: 3px">
                            <%=GetLabel("Jumlah")%>
                        </div>
                    </th>
                </tr>
                <tr runat="server" id="itemPlaceholder">
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <div style="padding: 3px;">
                        <div class="divTransactionNo">
                            <%#: Eval("TransactionNo") %></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px;">
                            <%#: Eval("ItemCode") %></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px">
                        <div>
                            <%#: Eval("ItemName1")%></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px">
                        <div>
                            <%#: Eval("ParamedicName")%></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px; text-align: right;">
                        <div>
                            <%#: Eval("CustomQtySatuan")%></div>
                    </div>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</div>
<div id="containerRadiologi" style="display: none; height:500px; overflow-y:scroll;" class="containerMain">
    <asp:ListView ID="lvwServiceRad" runat="server">
        <EmptyDataTemplate>
            <table id="tblView" runat="server" class="grdService grdNormal" cellspacing="0" rules="all">
                <tr>
                    <th style="width: 150px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("TransactionNo")%>
                        </div>
                    </th>
                    <th style="width: 100px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Kode Item")%>
                        </div>
                    </th>
                    <th>
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Nama Item")%>
                        </div>
                    </th>
                    <th style="width: 200px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Dokter")%>
                        </div>
                    </th>
                    <th style="width: 80px">
                        <div style="text-align: right; padding-right: 3px">
                            <%=GetLabel("Jumlah")%>
                        </div>
                    </th>
                </tr>
                <tr class="trEmpty">
                    <td colspan="20">
                        <%=GetLabel("No Data To Display") %>
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table id="tblView" runat="server" class="grdService grdNormal" cellspacing="0" rules="all">
                <tr>
                    <th style="width: 150px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("TransactionNo")%>
                        </div>
                    </th>
                    <th style="width: 100px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Kode Item")%>
                        </div>
                    </th>
                    <th>
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Nama Item")%>
                        </div>
                    </th>
                    <th style="width: 200px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Dokter")%>
                        </div>
                    </th>
                    <th style="width: 80px">
                        <div style="text-align: right; padding-right: 3px">
                            <%=GetLabel("Jumlah")%>
                        </div>
                    </th>
                </tr>
                <tr runat="server" id="itemPlaceholder">
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <div style="padding: 3px;">
                        <div class="divTransactionNo">
                            <%#: Eval("TransactionNo") %></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px;">
                            <%#: Eval("ItemCode") %></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px">
                        <div>
                            <%#: Eval("ItemName1")%></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px">
                        <div>
                            <%#: Eval("ParamedicName")%></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px; text-align: right;">
                        <div>
                            <%#: Eval("CustomQtySatuan")%></div>
                    </div>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</div>
<div id="containerLaboratorium" style="display: none; height:500px; overflow-y:scroll;" class="containerMain">
    <asp:ListView ID="lvwServiceLab" runat="server">
        <EmptyDataTemplate>
            <table id="tblView" runat="server" class="grdService grdNormal" cellspacing="0" rules="all">
                <tr>
                    <th style="width: 150px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("TransactionNo")%>
                        </div>
                    </th>
                    <th style="width: 100px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Kode Item")%>
                        </div>
                    </th>
                    <th>
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Nama Item")%>
                        </div>
                    </th>
                    <th style="width: 200px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Dokter")%>
                        </div>
                    </th>
                    <th style="width: 80px">
                        <div style="text-align: right; padding-right: 3px">
                            <%=GetLabel("Jumlah")%>
                        </div>
                    </th>
                </tr>
                <tr class="trEmpty">
                    <td colspan="20">
                        <%=GetLabel("No Data To Display") %>
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table id="tblView" runat="server" class="grdService grdNormal" cellspacing="0" rules="all">
                <tr>
                    <th style="width: 150px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("TransactionNo")%>
                        </div>
                    </th>
                    <th style="width: 100px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Kode Item")%>
                        </div>
                    </th>
                    <th>
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Nama Item")%>
                        </div>
                    </th>
                    <th style="width: 200px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Dokter")%>
                        </div>
                    </th>
                    <th style="width: 80px">
                        <div style="text-align: right; padding-right: 3px">
                            <%=GetLabel("Jumlah")%>
                        </div>
                    </th>
                </tr>
                <tr runat="server" id="itemPlaceholder">
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <div style="padding: 3px;">
                        <div class="divTransactionNo">
                            <%#: Eval("TransactionNo") %></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px;">
                            <%#: Eval("ItemCode") %></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px">
                        <div>
                            <%#: Eval("ItemName1")%></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px">
                        <div>
                            <%#: Eval("ParamedicName")%></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px; text-align: right;">
                        <div>
                            <%#: Eval("CustomQtySatuan")%></div>
                    </div>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</div>
<div id="containerOtherDiagnose" style="display: none; height:500px; overflow-y:scroll;" class="containerMain">
    <asp:ListView ID="lvwServiceOtherDiagnose" runat="server">
        <EmptyDataTemplate>
            <table id="tblView" runat="server" class="grdService grdNormal" cellspacing="0" rules="all">
                <tr>
                    <th style="width: 150px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("TransactionNo")%>
                        </div>
                    </th>
                    <th style="width: 100px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Kode Item")%>
                        </div>
                    </th>
                    <th>
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Nama Item")%>
                        </div>
                    </th>
                    <th style="width: 200px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Dokter")%>
                        </div>
                    </th>
                    <th style="width: 80px">
                        <div style="text-align: right; padding-right: 3px">
                            <%=GetLabel("Jumlah")%>
                        </div>
                    </th>
                </tr>
                <tr class="trEmpty">
                    <td colspan="20">
                        <%=GetLabel("No Data To Display") %>
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table id="tblView" runat="server" class="grdService grdNormal" cellspacing="0" rules="all">
                <tr>
                    <th style="width: 150px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("TransactionNo")%>
                        </div>
                    </th>
                    <th style="width: 100px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Kode Item")%>
                        </div>
                    </th>
                    <th>
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Nama Item")%>
                        </div>
                    </th>
                    <th style="width: 200px">
                        <div style="text-align: left; padding-left: 3px">
                            <%=GetLabel("Dokter")%>
                        </div>
                    </th>
                    <th style="width: 80px">
                        <div style="text-align: right; padding-right: 3px">
                            <%=GetLabel("Jumlah")%>
                        </div>
                    </th>
                </tr>
                <tr runat="server" id="itemPlaceholder">
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <div style="padding: 3px;">
                        <div class="divTransactionNo">
                            <%#: Eval("TransactionNo") %></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px;">
                            <%#: Eval("ItemCode") %></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px">
                        <div>
                            <%#: Eval("ItemName1")%></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px">
                        <div>
                            <%#: Eval("ParamedicName")%></div>
                    </div>
                </td>
                <td>
                    <div style="padding: 3px; text-align: right;">
                        <div>
                            <%#: Eval("CustomQtySatuan")%></div>
                    </div>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</div>
