<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master"
    AutoEventWireup="true" CodeBehind="ChangeMealOrderStatus.aspx.cs" Inherits="QIS.Medinfras.Web.Nutrition.Program.ChangeMealOrderStatus" %>

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
                            var value = $('#<%=ddlProcessType.ClientID %>').val();
                            if (value == '1') {
                                onCustomButtonClick('propose');
                            }
                            else if (value == '2') {
                                onCustomButtonClick('void');
                            }
                            else {
                                onCustomButtonClick('reopen');
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
                <td colspan="10">
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
                                            <asp:BoundField DataField="NutritionOrderHdId" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                    <input type="hidden" class="TestOrderID" value='<%#: Eval("NutritionOrderHdID")%>' />
                                                    <input type="hidden" class="GCTransactionStatus" value='<%#: Eval("GCTransactionStatus")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="NutritionOrderDateInString" HeaderText="Tanggal Order" HeaderStyle-Width="90px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign = "Center" />
                                                <asp:BoundField DataField="NutritionOrderTime" HeaderText="Jam Order" HeaderStyle-Width="90px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign = "Center" />
                                            <asp:BoundField DataField="NutritionOrderNo" HeaderText="No. Order" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfScheduleOrderDate" HeaderText="Tanggal Dijadwalkan" HeaderStyle-Width="90px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign = "Center" />
                                                <asp:BoundField DataField="ScheduleTime" HeaderText="Jam Dijadwalkan" HeaderStyle-Width="90px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign = "Center" />
                                            <asp:BoundField DataField="PhysicianName" HeaderText="Dokter" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px"/>
                                            <asp:BoundField DataField="ParamedicName" HeaderText="Order Dientry (User)" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px"/>
                                            <asp:BoundField DataField="Remarks" HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px"/>
                                            <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status"
                                                HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left" />
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
