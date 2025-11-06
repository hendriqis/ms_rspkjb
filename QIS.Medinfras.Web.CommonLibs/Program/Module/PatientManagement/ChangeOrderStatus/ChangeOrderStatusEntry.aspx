<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ChangeOrderStatusEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ChangeOrderStatusEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnChangeTransactionStatusBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
    <li id="btnChangeTransactionStatus" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=GetLabel("Re-open Order Pelayanan Pasien")%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnModuleID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $('#<%=grdView.ClientID %> #chkSelectAll').live('change', function () {
            var isChecked = $(this).is(':checked');
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
            });
        });

        $(function () {
            $('#<%=btnChangeTransactionStatusBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl('~/Libs/Program/Module/PatientManagement/ChangeOrderStatus/ChangeOrderStatusList.aspx');
            });
        });

        $('#<%=btnChangeTransactionStatus.ClientID %>').live('click', function (evt) {
            if ($('.chkIsSelected input:checked').length < 1) {
                showToast('Warning', '<%=GetErrorMsgSelectTransactionFirst() %>');
            }
            else {
                var param = '';
                var paramtype = '';
                $('.chkIsSelected input:checked').each(function () {
                    var trxID = $(this).closest('tr').find('.hdnKeyField').val();
                    var trxType = $(this).closest('tr').find('.hdnOrderType').val();
                    if (param != '' || paramtype != '') {
                        param += ',';
                        paramtype += ',';
                    }
                    param += trxID;
                    paramtype += trxType;
                });
                $('#<%=hdnParam.ClientID %>').val(param);
                $('#<%=hdnParamType.ClientID %>').val(paramtype);

                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/ChangeOrderStatus/ChangeOrderStatusReopenCtl.ascx');
                var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                var orderID = $('#<%=hdnParam.ClientID %>').val();
                var ordertype = $('#<%=hdnParamType.ClientID %>').val();
                var id = registrationID + '|' + orderID + '|' + ordertype;
                openUserControlPopup(url, id, 'Reopen Transaction', 400, 230);
            }
        });

        $('.lnkDetail a').live('click', function () {
            var Orderid = $(this).closest('tr').find('.keyField').html();
            var OrderType = $(this).closest('tr').find('.hdnOrderType').val();
            var id = Orderid + '|' + OrderType;
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/ChangeOrderStatus/ChangeOrderStatusDetailListCtl.ascx");
            openUserControlPopup(url, id, 'Test Order Detail', 900, 500);
        });

        function onLoadGenerateBill() {
            calculateTotal();
            $('.chkIsSelected input').change(function () {
                $('.chkSelectAll input').prop('checked', false);
                calculateTotal();
            });

            $('.chkSelectAll input').change(function () {
                var isChecked = $(this).is(":checked");
                $('.chkIsSelected').each(function () {
                    $(this).find('input').prop('checked', isChecked);
                });
                calculateTotal();
            });
        }

        function onRefreshControl() {
            cbpView.PerformCallback();
        }

        function onCboServiceUnitPerHealthcareValueChanged() {
            cbpView.PerformCallback('refresh');
        }
    </script>
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnParamType" runat="server" />
    <input type="hidden" value="" id="hdnCboServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnFilterServiceUnitID" runat="server" />
    <div style="height: 435px; overflow-y: auto;">
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 160px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblPenunjangMedis">
                                    <%=GetLabel("Unit Pelayanan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboServiceUnitPerHealthcare" ClientInstanceName="cboServiceUnitPerHealthcare"
                                    Width="200px">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboServiceUnitPerHealthcareValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td>
                    <div style="padding: 5px; min-height: 150px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                        position: relative; font-size: 0.95em;">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="OrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                        <input type="hidden" class="hdnKeyField" value='<%#: Eval("OrderID")%>' />
                                                        <input type="hidden" class="hdnOrderType" value='<%#: Eval("OrderType")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="cfOrderDateInString" HeaderText="Tanggal" HeaderStyle-Width="90px"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="OrderTime" HeaderText="Jam" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="OrderNo" HeaderText="No. Order" HeaderStyle-Width="150px"
                                                    HeaderStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="150px" />
                                                <asp:BoundField DataField="ParamedicName" HeaderText="Dokter Order" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="200px" />
                                                <asp:BoundField DataField="CreatedByName" HeaderText="Dibuat Oleh" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="200px" />
                                                <asp:BoundField DataField="Remarks" HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status" HeaderStyle-Width="100px"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                <asp:HyperLinkField HeaderStyle-Width="50px" DataTextField="ItemComparison" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-CssClass="lnkDetail" HeaderStyle-HorizontalAlign="Center" HeaderText="Detail" />
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
</asp:Content>
