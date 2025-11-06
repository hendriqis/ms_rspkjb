<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master"
    AutoEventWireup="true" CodeBehind="ChangeOrderStatus.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ChangeOrderStatus" %>

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
    <input type="hidden" value="" id="hdnProcessType" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=ddlProcessType.ClientID %>').change(function () {
                var value = $('#<%=ddlProcessType.ClientID %>').val();
                if (value == '1') {
                    $('#<%=tblVoidReason.ClientID %>').hide();
                }
                else {
                    $('#<%=tblVoidReason.ClientID %>').show();
                }
                $('#<%=hdnProcessType.ClientID %>').val(value);
                cbpView.PerformCallback('refresh');
            });

            $('#<%=tblVoidReason.ClientID %>').hide();
        });

        function onCboServiceUnitPerHealthcareValueChanged() {
            cbpView.PerformCallback('refresh');
        }

        function onCboVoidReasonValueChanged() {
            if (cboVoidReason.GetValue() == Constant.DeleteReason.OTHER)
                $('#trVoidOtherReason').show();
            else
                $('#trVoidOtherReason').hide();
        }

        $('#<%=grdView.ClientID %> #chkSelectAll').live('change', function () {
            var isChecked = $(this).is(':checked');
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
            });
        });

        $('#<%=btnProcess.ClientID %>').click(function (evt) {
            getCheckedMember();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                var messageBody = "Harus ada item yang dipilih untuk diproses";
                displayMessageBox('ERROR : UBAH STATUS ORDER', messageBody);
            }
            else {
                if (cboVoidReason.GetValue() == null) {
                    var messageBody = "Alasan Pembatalan/Reopen harus diisi.";
                    displayMessageBox('ERROR : UBAH STATUS ORDER', messageBody);
                }
                else {
                    displayConfirmationMessageBox('Ubah Status Order', 'Lanjutkan proses perubahan status order ?', function (result) {
                        if (result) {
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

                            var value = $('#<%=ddlProcessType.ClientID %>').val();
                            if (value == '1') {
                                onCustomButtonClick('propose');
                            }
                            else if (value == '2') {
                                if (orderNo != '') {
                                    showToast('Warning', "No. Order <b>" + orderNo + "</b> tidak dapat di void karena sudah ada Laporan Operasi");
                                }
                                else {
                                    onCustomButtonClick('void');
                                }
                            }
                            else {
                                if (orderNo != '') {
                                    showToast('Warning', "No. Order <b>" + orderNo + "</b> tidak dapat di-reopen karena sudah ada Laporan Operasi");
                                }
                                else {
                                    onCustomButtonClick('reopen');
                                }
                            }
                        }
                    });
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
        <table>
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 200px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label id="lblPenunjangMedis">
                        <%=GetLabel("Unit Pelayanan ")%></label>
                </td>
                <td colspan="2">
                    <dxe:ASPxComboBox runat="server" ID="cboServiceUnitPerHealthcare" ClientInstanceName="cboServiceUnitPerHealthcare"
                        Width="350px">
                        <ClientSideEvents ValueChanged="function(s,e){ onCboServiceUnitPerHealthcareValueChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label id="Label1">
                        <%=GetLabel("Ubah status menjadi ")%></label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlProcessType" runat="server" Width="200px">
                        <asp:ListItem Text="Proposed" Value="1" />
                        <asp:ListItem Text="Void" Value="2" />
                        <asp:ListItem Text="Open" Value="3" />
                    </asp:DropDownList>
                </td>
                <td />
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Alasan Void/Reopen")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboVoidReason" Width="200px" runat="server" ClientInstanceName="cboVoidReason">
                        <ClientSideEvents Init="function(s,e){ onCboVoidReasonValueChanged(s); }" ValueChanged="function(s,e){ onCboVoidReasonValueChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
                <td id="trVoidOtherReason" style="display: none">
                    <asp:TextBox ID="txtVoidOtherReason" Width="100%" runat="server" />
                </td>                
            </tr>
            <tr>
                <td colspan="3">
                    <table id="tblVoidReason" runat="server" cellpadding = "0" cellspacing="0">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 200px" />
                            <col />
                        </colgroup>
                        <tr>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
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
                                            <asp:BoundField DataField="ParamedicName" HeaderText="Diorder (Dokter Pengirim)" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" />
                                            <asp:BoundField DataField="CreatedByName" HeaderText="Order Dientry (User)" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px"/>
                                            <asp:BoundField DataField="Remarks" HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"/>
                                            <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status"
                                                HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:HyperLinkField HeaderStyle-Width="40px" DataTextField="ItemComparison" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-CssClass="lnkDetail" HeaderStyle-HorizontalAlign="Center" HeaderText="Detail" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi order untuk pasien ini")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
