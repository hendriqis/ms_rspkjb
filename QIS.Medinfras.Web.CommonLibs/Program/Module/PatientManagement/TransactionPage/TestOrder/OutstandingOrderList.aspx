<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master"
    AutoEventWireup="true" CodeBehind="OutstandingOrderList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.OutstandingOrderList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />
    <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProposeOrder" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Propose")%></div>
    </li>
    <li id="btnVoidOrder" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onCboServiceUnitPerHealthcareValueChanged() {
            cbpView.PerformCallback('refresh');
        }

        $('#<%=grdView.ClientID %> #chkSelectAll').live('change', function () {
            var isChecked = $(this).is(':checked');
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
            });
        });

        $('#<%=btnProposeOrder.ClientID %>').click(function (evt) {
            getCheckedMember();
            if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                showToast('Warning', 'Please Select Item First');
            }
            else {
                onCustomButtonClick('propose');
            }
        });

        $('#<%=btnVoidOrder.ClientID %>').click(function (evt) {
            getCheckedMember();
            if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                showToast('Warning', 'Please Select Item First');
            }
            else {
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
                var id = '';

                for (var i = 0; i < lstSelectedMember.length; i++) {
                    if (id == '') {
                        id = lstSelectedMember[i];
                    }
                    else {
                        id += "," + lstSelectedMember[i];
                    }
                }

                if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                    var filterExpression = "VisitID = " + visitID + " AND TestOrderID IN (" + id + ") AND IsDeleted = 0";
                    var orderNo = '';
                    Methods.getListObject('GetvPatientSurgeryList', filterExpression, function (resultCheck) {
                        for (i = 0; i < resultCheck.length; i++) {
                            if (orderNo == '') {
                                orderNo = resultCheck[i].TestOrderNo;
                            }
                            else {
                                orderNo += ", " + resultCheck[i].TestOrderNo;
                            }
                        }
                    });

                    if (orderNo != '') {
                        showToast('Warning', "No. Order <b>" + orderNo + "</b> tidak dapat di void karena sudah ada Laporan Operasi");
                    }
                    else {
                        onCustomButtonClick('void');
                    }
                }
            }
        });

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            var lstSelectedMemberType = $('#<%=hdnSelectedMemberType.ClientID %>').val().split(',');
            var lstSelectedMemberStatus = $('#<%=hdnSelectedMemberStatus.ClientID %>').val().split(',');
            $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').html();
                    var ordertype = $tr.find('.OrderType').val();
                    var orderstatus = $tr.find('.GCTransactionStatus').val();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                        lstSelectedMemberType.push(ordertype);
                        lstSelectedMemberStatus.push(orderstatus);
                    }
                    else {
                        lstSelectedMemberType[idx] = ordertype;
                        lstSelectedMemberStatus[idx] = orderstatus;
                    }
                }
                else {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').html();
                    var ordertype = $tr.find('.OrderType').val();
                    var orderstatus = $tr.find('.GCTransactionStatus').val();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                        lstSelectedMemberType.splice(idx, 1);
                        lstSelectedMemberStatus.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
            $('#<%=hdnSelectedMemberType.ClientID %>').val(lstSelectedMemberType.join(','));
            $('#<%=hdnSelectedMemberStatus.ClientID %>').val(lstSelectedMemberStatus.join(','));
        }

        $('.lnkDetail a').live('click', function () {
            var Orderid = $(this).closest('tr').find('.keyField').html();
            var OrderType = $(this).closest('tr').find('.OrderType').val();
            var id = Orderid + '|' + OrderType;
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/TestOrder/OutstandingOrderDetailListCtl.ascx");
            openUserControlPopup(url, id, 'Test Order Detail', 900, 500);
        });

        function onCboVoidReasonValueChanged() {
            if (cboVoidReason.GetValue() == Constant.DeleteReason.OTHER)
                $('#trVoidOtherReason').show();
            else
                $('#trVoidOtherReason').hide();
        }

        function onAfterCustomClickSuccess() {
            cbpView.PerformCallback('refresh');
        }

        function onClosePopUp() {
            cbpView.PerformCallback('refresh');        
        }

    </script>
    <input type="hidden" value="" id="TestOrderID" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMemberType" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMemberStatus" runat="server" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 130px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblPenunjangMedis">
                                    <%=GetLabel("Unit Pelayanan ")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboServiceUnitPerHealthcare" ClientInstanceName="cboServiceUnitPerHealthcare"
                                    Width="350px">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboServiceUnitPerHealthcareValueChanged(); }" />
                                </dxe:ASPxComboBox>
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
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="OrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                    <input type="hidden" class="TestOrderID" value='<%#: Eval("OrderID")%>' />
                                                    <input type="hidden" class="OrderType" value='<%#: Eval("OrderType")%>' />
                                                    <input type="hidden" class="GCTransactionStatus" value='<%#: Eval("GCTransactionStatus")%>' />
                                                    <input type="hidden" class="OrderNo" value='<%#: Eval("OrderNo")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="cfOrderDateInString" HeaderText="Tanggal Order" HeaderStyle-Width="90px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign = "Center" />
                                            <asp:BoundField DataField="OrderTime" HeaderText="Jam Order" HeaderStyle-Width="30px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign = "Center" />
                                            <asp:BoundField DataField="OrderNo" HeaderText="No. Order" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfScheduledDateInString" HeaderText="Tanggal Dijadwalkan" HeaderStyle-Width="90px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign = "Center" />
                                            <asp:BoundField DataField="ScheduledTime" HeaderText="Jam Dijadwalkan" HeaderStyle-Width="30px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign = "Center" />
                                            <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                            <asp:BoundField DataField="ParamedicName" HeaderText="Diorder (Pysician)" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" />
                                            <asp:BoundField DataField="CreatedByName" HeaderText="Diorder (User)" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px"/>
                                            <asp:BoundField DataField="Remarks" HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"/>
                                            <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status"
                                                HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:HyperLinkField HeaderStyle-Width="40px" DataTextField="ItemComparison" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-CssClass="lnkDetail" HeaderStyle-HorizontalAlign="Center" HeaderText="Detail" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <table class="tblEntryContent" style="width: 70%">
                                        <colgroup>
                                            <col style="width: 130px" />
                                            <col />
                                        </colgroup>
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
                                    </table>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </td>
            </tr>
            <%--<tr>
                <td style="padding: 5px; vertical-align: top">
                </td>
            </tr>--%>
        </table>
    </div>
</asp:Content>
