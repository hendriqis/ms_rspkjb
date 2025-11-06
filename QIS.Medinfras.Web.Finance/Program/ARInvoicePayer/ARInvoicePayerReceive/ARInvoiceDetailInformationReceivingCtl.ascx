<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ARInvoiceDetailInformationReceivingCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ARInvoiceDetailInformationReceivingCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_infopurchaseorderctl">
    $(function () {
        $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });
    });

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpViewCtl.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion 

    function oncbpViewCtlEndCallback(s) {
        $('#containerImgLoadingView').hide();
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingPopup"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
    }
</script>
<input type="hidden" value="" id="hdnParam" runat="server" />
<input type="hidden" id="hdnFilterExpression" runat="server" value="" />
<table style="width: 100%">
    <tr>
        <td valign="top">
            <div style="position: relative;">
                <table style="width: 50%">
                    <colgroup>
                        <col style="width: 20%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <b><%=GetLabel("No Invoice : ")%></b></label>
                        </td>
                        <td>
                            <asp:textbox id="txtInvoiceNo" readonly="true" width="35%" runat="server" />
                        </td>
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpViewCtl" runat="server" Width="100%" ClientInstanceName="cbpViewCtl"
                    ShowLoadingPanel="false" OnCallback="cbpViewCtl_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ oncbpViewCtlEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:panel runat="server" id="Panel1" cssclass="pnlContainerGridPatientPage">
                                <asp:gridview id="grdView" runat="server" cssclass="grdSelected grdView" autogeneratecolumns="false"
                                    showheaderwhenempty="true" emptydatarowstyle-cssclass="trEmpty">
                                    <columns>
                                        <asp:BoundField DataField="ARInvoiceID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="RegistrationNo" HeaderText="No Registrasi" HeaderStyle-Width="150px"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" HeaderStyle-Width="220px"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="cfARInvoiceDateInString" HeaderText="Tanggal Piutang" HeaderStyle-Width="120px"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="TotalClaimedAmountInString" HeaderText="Jumlah Klaim" HeaderStyle-Width="150px"
                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                    </columns>
                                    <emptydatatemplate>
                                        <%=GetLabel("No Data To Display") %>
                                    </emptydatatemplate>
                                </asp:gridview>
                            </asp:panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
            </div>
        </td>
    </tr>
</table>
