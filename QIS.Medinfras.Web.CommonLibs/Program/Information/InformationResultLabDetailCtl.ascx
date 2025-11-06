<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InformationResultLabDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.InformationResultLabDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">

    $(function () {
        $('#ulTabLabResult li').click(function () {
            $('#ulTabLabResult li.selected').removeAttr('class');
            $('.containerDaftarDt').filter(':visible').hide();
            $contentID = $(this).attr('contentid');
            $('#' + $contentID).show();
            $(this).addClass('selected');
        });
    });

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpPopupView.PerformCallback('changepage|' + page);
        });
    });
    
    function onCbpPopupViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                cbpPopupView.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion

</script>
<input type="hidden" id="hdnID" runat="server" />
<input type="hidden" id="hdnTransactionID" runat="server" />
<input type="hidden" id="hdnDateFrom" runat="server" />
<input type="hidden" id="hdnDateTo" runat="server" />

<table class="tblContentArea">
    <tr>
        <td>
            <table class="tblEntryContent" style="width:70%">
                <colgroup>
                    <col style="width: 200px" />
                    <col style="width: 150px" />
                    <col style="width: 5px" />
                    <col style="width: 150px" />
                    <col style="width: 120px" />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal" id="lblTransactionNo"><%=GetLabel("No. Transaksi")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" />
                    </td>
                    <td class="tdLabel">
                        <label class="lblNormal" id="lblTransactionDate"><%=GetLabel("Tanggal Order")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTransactionDate" Width="120px" ReadOnly="true" runat="server" CssClass="datepicker" />
                    </td>
                    <td class="tdLabel"> 
                        <label class="lblNormal" id="lblResultDate"><%=GetLabel("Tanggal Hasil")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtResultDate" Width="120px" ReadOnly="true" runat="server" CssClass="datepicker" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal" id="Label1"><%=GetLabel("No. RM")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMedicalNo" Width="150px" ReadOnly="true" runat="server" />
                    </td>
                    <td class="tdLabel"> 
                        <label class="lblNormal" id="Label4"><%=GetLabel("Jam Order")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTransactionTime" Width="120px" ReadOnly="true" runat="server" CssClass="time" Style="text-align: center" />
                    </td>
                    <td class="tdLabel">
                        <label class="lblNormal" id="Label6"><%=GetLabel("Jam Hasil")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtResultTime" Width="120px" ReadOnly="true" runat="server" CssClass="time" Style="text-align: center" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal" id="Label2"><%=GetLabel("Nama Pasien")%></label>
                    </td>
                    <td colspan="4">
                        <asp:TextBox ID="txtPatientName" Width="100%" ReadOnly="true" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal" id="Label3"><%=GetLabel("Dokter Pengirim")%></label>
                    </td>
                    <td colspan="4">
                        <asp:TextBox ID="txtOrderPhysician" Width="100%" ReadOnly="true" runat="server" />
                    </td>
                </tr>
            </table>
            <div>
                <tr>
                    <td colspan="2">
                        <div class="containerUlTabPage">
                            <ul class="ulTabPage" id="ulTabLabResult">
                                <li contentid="containerDaftar" class="selected">
                                    <%=GetLabel("DAFTAR PEMERIKSAAN") %></li>
                                <li contentid="containerHasil">
                                    <%=GetLabel("HASIL PEMERIKSAAN") %></li>
                            </ul>
                        </div>
                        <div style="position: relative;" id="containerDaftar" class="containerDaftarDt">
                            <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                                ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                                <ClientSideEvents EndCallback="function(s,e){onCbpPopupViewEndCallback()}" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                        <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:240px; overflow-y: scroll;">
                                            <asp:GridView ID="grdPopupView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                    <asp:BoundField DataField="ItemCode" HeaderText="Kode Item" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="ItemName1" HeaderText="Jenis Pemeriksaan" HeaderStyle-HorizontalAlign="Left" />
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <%=GetLabel("No Data To Display")%>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>  
                        </div>  
                        <div id="containerHasil" style="display: none" class="containerDaftarDt">
                            <dxcp:ASPxCallbackPanel ID="cbpViewHasil" runat="server" Width="100%" ClientInstanceName="cbpViewHasil"
                                ShowLoadingPanel="false">
                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { oncbpViewHasilEndCallback(); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                        <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGrid" Style="height: 290px">
                                            <asp:GridView ID="grdView2" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Pemeriksaan" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="350px">
                                                        <ItemTemplate>
                                                            <div><%#: Eval("ItemName1") %></div>
                                                            <input type="hidden" value="<%#:Eval("cfReferenceRange") %>" bindingfield="cfReferenceRange" />
                                                            <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                            <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                            <input type="hidden" value="<%#:Eval("FractionID") %>" bindingfield="FractionID" />
                                                            <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Artikel Pemeriksaan" HeaderStyle-HorizontalAlign="Left"
                                                        ItemStyle-Width="250px">
                                                        <ItemTemplate>
                                                            <div class='<%#: Eval("IsNormal").ToString() == "False" ? "isAbnormalColor" : "" %>'>
                                                                <%#: Eval("FractionName1") %></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="TextValue" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="textResultValue hiddenColumn" />
                                                    <asp:TemplateField HeaderText="Hasil" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <div class='<%#: Eval("IsNormal").ToString() == "False" ? "isAbnormalColor" : "" %>'
                                                                <%# Eval("cfIsResultInPDF").ToString() == "True" ? "Style='display:none'" : "Style='text-align: right'" %>>
                                                                <asp:Literal ID="literal" Text='<%# Eval("cfTestResultValue") %>' Mode="PassThrough"
                                                                    runat="server" /></div>
                                                            <div <%# Eval("cfIsResultInPDF").ToString() == "False" ? "Style='display:none'" : "" %>>
                                                                <asp:Label ID="Label5" class="lblViewPDF lblLink" runat="server"><%#: Eval("cfTestReferenceValue") %></asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Flag" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <div class='<%#: Eval("IsNormal").ToString() == "False" ? "isAbnormalColor" : "" %>'
                                                                style="text-align: right">
                                                                <%#: Eval("ResultFlag") %></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Satuan" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Left"
                                                                        ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <div class='<%#: Eval("IsNormal").ToString() == "False" ? "isAbnormalColor" : "" %>'>
                                                            <%#: Eval("MetricUnit") %></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nilai Referensi" ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <div class='<%#: Eval("cfIsReferenceAsLink").ToString() == "True" ? "lblLink lblReffRange" : "" %>'>
                                                            <asp:Literal ID="literal" Text='<%# Eval("cfReferenceRangeCustom") %>' Mode="PassThrough"
                                                                runat="server" /></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada Data Pemeriksaan / Pelayanan Pasien")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </div>
            <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
            </div>
            <div class="containerPaging">
                <div class="wrapperPaging">
                    <div id="pagingPopup"></div>
                </div>
               </div> 
          </td>
      </tr>
</table>