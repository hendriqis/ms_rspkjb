<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlOutstandingOrderCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.ControlOutstandingOrderCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<div class="toolbarArea">
    <ul>
        <li id="btnVoid">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
                <%=GetLabel("Void")%></div>
        </li>
    </ul>
</div>
<script type="text/javascript" id="dxss_mcuoutstandingorderctl">
    function onCboServiceUnitChanged() {
        cbpViewPopup.PerformCallback();
    }

    function onCboVoidReasonValueChanged() {
        if (cboVoidReason.GetValue() == Constant.DeleteReason.OTHER)
            $('#trVoidOtherReason').show();
        else
            $('#trVoidOtherReason').hide();
    }

    $('.chkSelectAll input').live('change', function () {
        var value = $(this).prop("checked");
        $('.chkIsSelected input').each(function () {
            $(this).prop("checked", value);
        });
    });

    $('#btnVoid').click(function () {
        getCheckedMember();
        var voidReason = cboVoidReason.GetValue();
        if (voidReason != null) {
            cbpViewPopup.PerformCallback('void');
        } else {
            showToast("Harap isi Alasan Pembatalan terlebih dahulu !");
        }
    });

    function getCheckedMember() {
        var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
        $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
            if ($(this).is(':checked')) {
                $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').html();
                var idx = lstSelectedMember.indexOf(key);
                if (idx < 0) {
                    lstSelectedMember.push(key);
                }
            }
            else {
                $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').html();
                var idx = lstSelectedMember.indexOf(key);
                if (idx > -1) {
                    lstSelectedMember.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
    }

    function onCbpViewPopupEndCallback(s) {
        hideLoadingPanel();
    }
</script>
<div style="padding: 10px;">
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <input type="hidden" id="hdnVisitIDCtl" runat="server" />
    <input type="hidden" id="hdnRegistrationNoCtl" runat="server" />
    <input type="hidden" id="hdnPatientNameCtl" runat="server" />
    <input type="hidden" id="hdnItemIDCtl" runat="server" />
    <input type="hidden" id="hdnItemCodeCtl" runat="server" />
    <input type="hidden" id="hdnItemNameCtl" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitIDCtl" runat="server" />
    <input type="hidden" id="hdnSelectedOrderID" runat="server" />
    <input type="hidden" id="hdnSelectedOrderDtID" runat="server" />
    <input type="hidden" id="hdnSelectedTransactionID" runat="server" />
    <input type="hidden" id="hdnSelectedTransactionDtID" runat="server" />
    <input type="hidden" id="hdnSelectedHealthcareServiceUnitID" runat="server" />
    <input type="hidden" id="hdnSelectedDepartmentID" runat="server" />
    <table width="100%">
        <colgroup>
            <col width="100px" />
            <col width="250px" />
            <col width="50%" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No. Registrasi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtNoReg" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Nama Paket")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtItemServiceName" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label id="lblPenunjangMedis">
                    <%=GetLabel("Unit Pelayanan ")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox runat="server" ID="cboServiceUnitPerHealthcare" ClientInstanceName="cboServiceUnitPerHealthcare"
                    Width="100%">
                    <ClientSideEvents ValueChanged="function(s,e){ onCboServiceUnitChanged(); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Alasan Pembatalan")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboVoidReason" Width="200px" runat="server" ClientInstanceName="cboVoidReason">
                    <ClientSideEvents Init="function(s,e){ onCboVoidReasonValueChanged(s); }" ValueChanged="function(s,e){ onCboVoidReasonValueChanged(); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr id="trVoidOtherReason" style="display: none">
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtVoidOtherReason" Width="300px" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                        ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em; max-height: 420px; overflow-y: auto">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                    <input type="hidden" class="TestOrderID" value='<%#: Eval("OrderID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="OrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="OrderNo" HeaderText="No. Order" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="130px" />
                                            <asp:TemplateField HeaderStyle-Width="250px" HeaderText="Unit Pelayanan" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <div>
                                                        <%#:Eval("ServiceUnitName") %></div>
                                                    <div style="font-style: italic">
                                                        <%#:Eval("DepartmentID") %></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="300px" HeaderText="Dokter" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <div style="font-style: italic">
                                                        <%#:Eval("ParamedicName") %></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="OrderStatus" HeaderText="Status" HeaderStyle-Width="140px" />
                                            <asp:BoundField DataField="ItemComparison" HeaderText="Detail Item" HeaderStyle-Width="80px"
                                                HeaderStyle-HorizontalAlign="Center" />
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
            </td>
        </tr>
    </table>
</div>
