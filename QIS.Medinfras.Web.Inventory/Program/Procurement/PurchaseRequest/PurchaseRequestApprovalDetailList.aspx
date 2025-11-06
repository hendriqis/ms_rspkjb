<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="PurchaseRequestApprovalDetailList.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseRequestApprovalDetailList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPurchaseRequestBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" />
        <div>
            <%=GetLabel("Back")%></div>
    </li>
    <li id="btnPurchaseRequestDetailApprove" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('#<%=btnPurchaseRequestBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl('~/Program/Procurement/PurchaseRequest/PurchaseRequestApprovalList.aspx?id=to');
            });

            $('#<%=btnPurchaseRequestDetailApprove.ClientID %>').click(function () {
                getCheckedMember();
                if ($('#<%=hdnSelectedMember.ClientID %>').val() != '' || $('#<%=hdnDeclinedMember.ClientID %>').val() != '') {
                    var message = "Proses approval / decline permintaan pembelian ?";
                    showToastConfirmation(message, function (result) {
                        if (result) onCustomButtonClick('process'); ;
                    });
                }
                else {
                    showToast('Warning', 'Tidak ada item yang dipilih untuk diproses');
                }
            });

            $('.chkIsSelected input').change(function () {
                $(this).closest('tr').find('input:checkbox').removeProp('checked');
                $(this).prop('checked', true);
            });

            $('.chkIsDeclined input').change(function () {
                $(this).closest('tr').find('input:checkbox').removeProp('checked');
                $(this).prop('checked', true);
            });
        }

        function onAfterCustomClickSuccess(type, retval) {
            $('#<%=hdnSelectedMember.ClientID %>').val('');
            cbpView.PerformCallback('refresh');
            showLoadingPanel();
            document.location = ResolveUrl('~/Program/Procurement/PurchaseRequest/PurchaseRequestApprovalList.aspx?id=to');
        }

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            var result = '';
            $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    if (lstSelectedMember.indexOf(key) < 0)
                        lstSelectedMember.push(key);
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    if (lstSelectedMember.indexOf(key) > -1)
                        lstSelectedMember.splice(lstSelectedMember.indexOf(key), 1);
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));

            var lstDeclinedMember = $('#<%=hdnDeclinedMember.ClientID %>').val().split(',');
            $('#<%=grdView.ClientID %> .chkIsDeclined input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    if (lstDeclinedMember.indexOf(key) < 0)
                        lstDeclinedMember.push(key);
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    if (lstDeclinedMember.indexOf(key) > -1)
                        lstDeclinedMember.splice(lstDeclinedMember.indexOf(key), 1);
                }
            });
            $('#<%=hdnDeclinedMember.ClientID %>').val(lstDeclinedMember.join(','));
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                getCheckedMember();
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    getCheckedMember();
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnRequestID" runat="server" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnDeclinedMember" runat="server" value="" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <div style="height: 550px; overflow-y: auto; overflow-x: hidden;">
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%" cellpadding="1" cellspacing="0">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblOrderNo">
                                    <%=GetLabel("No. Permintaan")%></label>
                            </td>
                            <td>
                                <table class="tblEntryContent" style="width: 100%" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col style="width: 140px" />
                                        <col style="width: 60px" />
                                        <col style="width: 120px;" />
                                        <col style="width: 100px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtOrderNo" Width="165px" ReadOnly="true" runat="server" />
                                        </td>
                                        <td id="tdUrgent" runat="server">
                                            <img src='<%= ResolveUrl("~/Libs/Images/Button/warning.png")%>' width="17" height="17"
                                                alt="" title='<%=GetLabel("Urgent") %>' />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                                -
                                <%=GetLabel("Waktu") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtItemOrderDate" Width="100%" CssClass="datepicker" runat="server"
                                                ReadOnly="true" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemOrderTime" Width="64px" CssClass="time" runat="server" Style="text-align: center"
                                                ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblLocation">
                                    <%=GetLabel("Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDFrom" value="" runat="server" />
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationCode" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLocationName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top">
                                <%=GetLabel("Keterangan") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label1">
                                    <%=GetLabel("Dientry oleh")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUserName" Width="165px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                        OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><br style="clear: both" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsDeclined" runat="server" CssClass="chkIsDeclined" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemCode" HeaderText="Kode Item" HeaderStyle-Width="80px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfQuantityMIN" HeaderText="Quantity MIN" HeaderStyle-Width="60px"
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cfQuantityMAX" HeaderText="Quantity MAX" HeaderStyle-Width="60px"
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cfQuantityEND" HeaderText="Quantity On HAND" HeaderStyle-Width="60px"
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cfQuantityOnOrder" HeaderText="Quantity On ORDER" HeaderStyle-Width="60px"
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cfQuantityOnRequest" HeaderText="Quantity On REQUEST"
                                                HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cfQuantity" HeaderText="Quantity Permintaan" HeaderStyle-Width="75px"
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="PurchaseUnit" HeaderText="Satuan Permintaan" HeaderStyle-Width="90px"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfUnitConversion" HeaderText="Faktor Konversi" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfTotalBaseUnitQty" HeaderText="Total Permintaan" HeaderStyle-Width="75px"
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="BaseUnit" HeaderText="Satuan Dasar" HeaderStyle-Width="100px"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CustomTotalInString" HeaderText="Nilai Permintaan" HeaderStyle-Width="120px"
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging">
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
