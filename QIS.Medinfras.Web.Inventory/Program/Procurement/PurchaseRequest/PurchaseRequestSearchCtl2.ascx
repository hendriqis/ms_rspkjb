<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseRequestSearchCtl2.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseRequestSearchCtl2" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_PurchaseRequestSearchCtl2">

    $('#<%=btnMPEntryPopupSave.ClientID %>').live('click', function () {
        getCheckedPurchaseRequest();
        setPurchaseRequest($('#<%=hdnSelectedPurchaseRequestID.ClientID %>').val().substr(1), $('#<%=hdnSelectedPurchaseRequestNo.ClientID %>').val().substr(1));
        pcRightPanelContent.Hide();
    });

    function getCheckedPurchaseRequest() {
        var lstSelectedPurchaseRequestID = $('#<%=hdnSelectedPurchaseRequestID.ClientID %>').val().split(',');
        var lstSelectedPurchaseRequestNo = $('#<%=hdnSelectedPurchaseRequestNo.ClientID %>').val().split(',');
        $('.chkSelected input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');
                var oPurchaseRequestID = $tr.find('.PurchaseRequestID').val();
                var oPurchaseRequestNo = $tr.find('.PurchaseRequestNo').val();
                var idx = lstSelectedPurchaseRequestID.indexOf(oPurchaseRequestID);
                if (idx < 0) {
                    lstSelectedPurchaseRequestID.push(oPurchaseRequestID);
                    lstSelectedPurchaseRequestNo.push(oPurchaseRequestNo);
                }
                else {
                    lstSelectedPurchaseRequestNo[idx] = oPurchaseRequestNo;
                }
            }
            else {
                var oPurchaseRequestID = $(this).closest('tr').find('.PurchaseRequestID').val();
                var idx = lstSelectedPurchaseRequestID.indexOf(oPurchaseRequestID);
                if (idx > -1) {
                    lstSelectedPurchaseRequestID.splice(idx, 1);
                    lstSelectedPurchaseRequestNo.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedPurchaseRequestID.ClientID %>').val(lstSelectedPurchaseRequestID.join(','));
        $('#<%=hdnSelectedPurchaseRequestNo.ClientID %>').val(lstSelectedPurchaseRequestNo.join(','));
    }

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkSelected').each(function () {
            $chk = $(this).find('input');
            if (!$chk.is(":disabled")) {
                $chk.prop('checked', isChecked);
            }
        });
    });
</script>
<div class="toolbarArea">
    <ul>
        <li runat="server" id="btnMPEntryPopupSave">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div>
                <%=GetLabel("Set")%></div>
        </li>
    </ul>
</div>
<div style="height: 500px; overflow-y: auto; overflow-x: hidden" id="containerPopup">
    <input type="hidden" id="hdnSelectedPurchaseRequestID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedPurchaseRequestNo" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnLocationParam" runat="server" value="" />
    <input type="hidden" id="hdnPurchaseOrderType" runat="server" value="" />
    <input type="hidden" id="hdnProductLineIDCtl" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                    ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 40px" align="center">
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </th>
                                                <th style="width: 170px" align="left">
                                                    <%=GetLabel("No Permintaan Pembelian")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Jenis Pemesanan")%>
                                                </th>
                                                <th align="left">
                                                    <%=GetLabel("Remarks")%>
                                                </th>
                                                <th style="width: 180px" align="center">
                                                    <%=GetLabel("Info Dibuat")%>
                                                </th>
                                                <th style="width: 180px" align="center">
                                                    <%=GetLabel("Info Approved")%>
                                                </th>
                                                <th style="width: 40px" align="center">
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="15">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 40px" align="center">
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </th>
                                                <th style="width: 170px" align="left">
                                                    <%=GetLabel("No Permintaan Pembelian")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Jenis Pemesanan")%>
                                                </th>
                                                <th align="left">
                                                    <%=GetLabel("Remarks")%>
                                                </th>
                                                <th style="width: 180px" align="center">
                                                    <%=GetLabel("Info Dibuat")%>
                                                </th>
                                                <th style="width: 180px" align="center">
                                                    <%=GetLabel("Info Approved")%>
                                                </th>
                                                <th style="width: 40px" align="center">
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkSelected" runat="server" CssClass="chkSelected" />
                                                <input type="hidden" class="PurchaseRequestID" id="PurchaseRequestID" runat="server" value='<%#: Eval("PurchaseRequestID")%>' />
                                                <input type="hidden" class="PurchaseRequestNo" id="PurchaseRequestNo" runat="server" value='<%#: Eval("PurchaseRequestNo")%>' />
                                            </td>
                                            <td valign="top">
                                                <b>
                                                    <%#: Eval("PurchaseRequestNo")%></b>
                                                <br />
                                                <%#: Eval("TransactionDateInString")%>
                                            </td>
                                            <td align="center" valign="top">
                                                <%#: Eval("PurchaseOrderType")%>
                                            </td>
                                            <td valign="top">
                                                <%#: Eval("Remarks")%>
                                            </td>
                                            <td align="center" valign="top">
                                                <b>
                                                    <%#: Eval("CreatedByName")%></b>
                                                <br />
                                                <%#: Eval("cfRequestCreatedDatetimeInStringFull")%>
                                            </td>
                                            <td align="center" valign="top">
                                                <b>
                                                    <%#: Eval("ApprovedByName")%></b>
                                                <br />
                                                <%#: Eval("cfRequestApprovedDatetimeInStringFull")%>
                                            </td>
                                            <td align="center" valign="top">
                                                <div <%# Eval("IsUrgent").ToString() != "True" ? "Style='display:none'":"" %>>
                                                    <img src='<%# ResolveUrl("~/Libs/Images/Button/warning.png") %>' width="17" height="17"
                                                        alt="" visible="true" title='<%=GetLabel("Urgent") %>' />
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
