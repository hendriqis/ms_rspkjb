<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HcLabLISDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.HcLabLISDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    function oncbpViewDetailCtlEndCallback(s) {
        hideLoadingPanel();
    }

//    $('.lblDtResult').live('click', function () {
//        var transactionID = $(this).closest('tr').find('.keyField').html();
//        var id = transactionID;
//        var url = ResolveUrl("~/Libs/Program/Module/SystemTool/Bridging/LIS/WynacomLISDetailResultCtl.ascx");
//        openUserControlPopup(url, id, 'Hasil Pemeriksaan Laboratorium', 550, 500);
//    });
</script>
<input type="hidden" value="" id="hdnSelectedMemberCtl" runat="server" />
<input type="hidden" id="hdnTransactionID" value="" runat="server" />
<input type="hidden" id="hdnTestOrderID" value="" runat="server" />
<input type="hidden" id="hdnBridgingStatus" value="" runat="server" />
<div style="height: 440px; overflow-y: auto"> 
    <table border="0">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td>
                <fieldset id="fsEntryPopup" style="margin: 0">
                    <table cellpadding="0" cellspacing="1" border="0">
                        <colgroup>
                            <col style="width: 200px" />
                            <col style="width: 150px" />
                            <col style="width: 5px" />
                            <col style="width: 150px" />
                            <col style="width: 120px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblTransactionNo">
                                    <%=GetLabel("No. Transaksi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                            <td />
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblTransactionDate">
                                    <%=GetLabel("Tanggal ")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransactionDate" Width="120px" ReadOnly="true" runat="server"
                                    CssClass="datepicker" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label1">
                                    <%=GetLabel("No. RM")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMedicalNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                            <td />
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label2">
                                    <%=GetLabel("Nama Pasien")%></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtPatientName" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label3">
                                    <%=GetLabel("Dokter Pengirim")%></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtOrderPhysician" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                            <td />
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
    <div>
        <dxcp:ASPxCallbackPanel ID="cbpViewDetailCtl" runat="server" Width="100%" ClientInstanceName="cbpViewDetailCtl"
            ShowLoadingPanel="false">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpViewDetailCtlEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlRujukan" Style="width: 100%; margin-left: auto;
                        margin-right: auto; position: relative; font-size: 0.95em;">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdView" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                    <HeaderTemplate>
                                        <%=GetLabel("Kode ")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div <%# Eval("IsDeleted").ToString() == "True" ? "Style='text-decoration: line-through; color:red;font-style:italic'":"" %>>
                                            <%#: Eval("ItemCode")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <%=GetLabel("Nama Pemeriksaan")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div <%# Eval("IsDeleted").ToString() == "True" ? "Style='text-decoration: line-through; color:red;font-style:italic'":"" %>>
                                            <%#: Eval("ItemName1")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DetailReferenceNo" HeaderText="Reference No." HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="80px">
                                    <HeaderTemplate>
                                        <%=GetLabel("STATUS")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <%#: Eval("cfLISBridgingStatus")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
<%--                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="80px">
                                    <HeaderTemplate>
                                        <%=GetLabel("HASIL")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                    <ItemTemplate>
                                            <label class="lblDtResult lblLink">
                                                Status</label>
                                    </ItemTemplate>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
                        <div class="imgLoadingGrdView" id="containerImgLoadingReferrer">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </div>
</div>
