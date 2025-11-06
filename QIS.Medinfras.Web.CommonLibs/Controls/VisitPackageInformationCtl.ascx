<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisitPackageInformationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.VisitPackageInformationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<input type="hidden" value="" id="hdnID" runat="server" />
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnVisitID" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Pasien")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtCustomerName" ReadOnly="true" Width="300px" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <colgroup>
            <col style="width: 30%" />
            <col style="width: 70%" />
        </colgroup>
        <tr>
            <td valign="top">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdView" runat="server" OnRowDataBound="grdView_RowDataBound" CssClass="grdSelected grdView"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="TransactionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderText="Nama Pelayanan" HeaderStyle-Width="350px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("ItemName1") %>
                                                </div>
                                                <div style="font-style: italic">
                                                    ED : <%#: Eval("cfExpiredDate") %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Quantity" HeaderText="Sisa" HeaderStyle-Width="80px"
                                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
            <td valign="top">
                <dxcp:ASPxCallbackPanel ID="cbpViewDetail" runat="server" Width="100%" ClientInstanceName="cbpViewDetail"
                    ShowLoadingPanel="false" OnCallback="cbpViewDetail_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ onCbpViewDetailEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdViewDetail" runat="server" CssClass="grdSelected grdView"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Informasi Registrasi" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div style="font-style: italic">
                                                    <%#: Eval("RegistrationNo") %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Informasi Transaksi" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("cfTransactionDate") %>
                                                </div>
                                                <div style="font-style: italic">
                                                    <%#: Eval("TransactionNo") %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ParamedicName" HeaderText="Dokter" HeaderStyle-Width="250px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="PackageBalanceQtyTaken" HeaderText="Jumlah" HeaderStyle-Width="100px"
                                            HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
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
<script type="text/javascript" id="dxss_Infoctl1">
    $(function () {
        $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                cbpViewDetail.PerformCallback('refresh');
            }
        });

        $('#<%=grdViewDetail.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdViewDetail.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });

        $('#<%=grdView.ClientID %> tr:eq(1)').click();
    });

    function onCbpViewDetailEndCallback(s) {
        $('#containerImgLoadingViewDt').hide();
    }
</script>
