<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OutstandingOrderDetailListCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.OutstandingOrderDetailListCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div class="toolbarArea">
    <ul>
        <li runat="server" id="btnMPEntryVoid">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
                <%=GetLabel("Void")%></div>
        </li>
    </ul>
</div>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">

    $('#<%=grdView.ClientID %> #chkSelectAllCtl').live('change', function () {
        var isChecked = $(this).is(':checked');
        $('.chkIsSelectedCtl input').each(function () {
            $(this).prop('checked', isChecked);
        });
    });

    $('#<%=btnMPEntryVoid.ClientID %>').click(function (evt) {
        getCheckedMemberCtl();
        if ($('#<%=hdnSelectedMemberCtl.ClientID %>').val() == '') {
            showToast('Warning', 'Please Select Item First');
        }
        else {
            if (IsValid(evt, 'fsOutstandingOrderCtl', 'mpOutstandingOrderCtl'))
                cbpEntryPopupView.PerformCallback('void');
        }
    });

    function getCheckedMemberCtl() {
        var lstSelectedMember = $('#<%=hdnSelectedMemberCtl.ClientID %>').val().split(',');
        var lstSelectedMemberType = $('#<%=hdnSelectedMemberTypeCtl.ClientID %>').val().split(',');
        $('#<%=grdView.ClientID %> .chkIsSelectedCtl input').each(function () {
            if ($(this).is(':checked')) {
                $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').html();
                var ordertype = $tr.find('.hdnOrderType').val();
                var idx = lstSelectedMember.indexOf(key);
                if (idx < 0) {
                    lstSelectedMember.push(key);
                    lstSelectedMemberType.push(ordertype);
                }
                else {
                    lstSelectedMemberType[idx] = ordertype;                
                }
            }
            else {
                $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').html();
                var ordertype = $tr.find('.hdnOrderType').val();
                var idx = lstSelectedMember.indexOf(key);
                if (idx > -1) {
                    lstSelectedMember.splice(idx, 1);
                    lstSelectedMemberType.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedMemberCtl.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberTypeCtl.ClientID %>').val(lstSelectedMemberType.join(','));
    }

    function onCboVoidCtlReasonValueChanged() {
        if (cboVoidCtlReason.GetValue() == Constant.DeleteReason.OTHER)
            $('#trVoidOtherReason').show();
        else
            $('#trVoidOtherReason').hide();
    }
</script>
<input type="hidden" value="" id="hdnSelectedMemberCtl" runat="server" />
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" id="hdnOrderID" value="" runat="server" />
    <input type="hidden" id="hdnOrderTypeID" value="" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMemberTypeCtl" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No Order")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTestOrderHdNo" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Unit Penunjang")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <input id="chkSelectAllCtl" type="checkbox" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox Visible='<%# Eval("GCItemStatus").ToString() == "X126^001" ? true : false %>'
                                                    ID="chkIsSelectedCtl" runat="server" CssClass="chkIsSelectedCtl" />
                                                <input type="hidden" class="hdnOrderID" value='<%#: Eval("OrderID")%>' />
                                                    <input type="hidden" class="hdnOrderType" value='<%#: Eval("OrderType")%>' />
                                                <input type="hidden" class="hdnGCTestOrderStatus" value='<%#: Eval("GCItemStatus")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <div>
                                                    <%=GetLabel("Pelayanan") %></div>
                                                <div style="color:Blue">

                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                <%#: Eval("ItemName1")%></div>
                                                <div style="color:Blue">
                                             </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Right">
                                            <HeaderTemplate>
                                                <div>
                                                    <%=GetLabel("Qty") %></div>
                                                <div style="color:Blue">

                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div align = "Right">
                                                <%#: Eval("Qty")%></div>
                                                <div style="color:Blue">
                                             </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <div>
                                                    <%=GetLabel("Unit") %></div>
                                                <div style="color:Blue">

                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div align = "Center">
                                                <%#: Eval("UnitName")%></div>
                                                <div style="color:Blue">
                                             </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemStatusWatermark" HeaderText="Status" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <fieldset id="fsOutstandingOrderCtl">
                    <table class="tblEntryContent" style="width: 70%">
                        <colgroup>
                            <col style="width: 160px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Alasan Pembatalan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboVoidCtlReason" Width="100%" runat="server" ClientInstanceName="cboVoidCtlReason">
                                    <ClientSideEvents Init="function(s,e){ onCboVoidCtlReasonValueChanged(s); }" ValueChanged="function(s,e){ onCboVoidCtlReasonValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trVoidOtherReason" style="display: none">
                            <td>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVoidOtherReason" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="onClosePopUp();pcRightPanelContent.Hide();" />
    </div>
</div>