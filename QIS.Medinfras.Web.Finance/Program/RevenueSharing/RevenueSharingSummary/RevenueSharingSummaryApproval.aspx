<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/ParamedicPage/MPBaseParamedicPageTrx.master"
    AutoEventWireup="true" CodeBehind="RevenueSharingSummaryApproval.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingSummaryApproval" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Program/RevenueSharing/RevenueSharingToolbarCtl.ascx" TagName="ToolbarCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRSSummaryApproval" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtApprovalDate.ClientID %>');
            setDatePicker('<%=txtPeriodFrom.ClientID %>');
            setDatePicker('<%=txtPeriodTo.ClientID %>');

            $('#<%=txtPeriodFrom.ClientID %>').change(function () {
                var dateFrom = $('#<%:txtPeriodFrom.ClientID %>').val();
                var datePickerFrom = Methods.getDatePickerDate(dateFrom);
                var dateFromYMD = dateToDMYCustom(datePickerFrom);

                var dateTo = $('#<%:txtPeriodTo.ClientID %>').val();
                var datePickerTo = Methods.getDatePickerDate(dateTo);
                var dateToYMD = dateToDMYCustom(datePickerTo);

                if (dateFromYMD > dateToYMD) {
                    showToast('Information', 'Tanggal Awal harus lebih kecil dari Tanggal Akhir');
                } else {
                    cboVerificationDateChanged();
                }
            });

            $('#<%=txtPeriodTo.ClientID %>').change(function () {
                var dateFrom = $('#<%:txtPeriodFrom.ClientID %>').val();
                var datePickerFrom = Methods.getDatePickerDate(dateFrom);
                var dateFromYMD = dateToDMYCustom(datePickerFrom);

                var dateTo = $('#<%:txtPeriodTo.ClientID %>').val();
                var datePickerTo = Methods.getDatePickerDate(dateTo);
                var dateToYMD = dateToDMYCustom(datePickerTo);

                if (dateFromYMD > dateToYMD) {
                    displayMessageBox('Information', 'Tanggal Awal harus lebih kecil dari Tanggal Akhir');
                } else {
                    cboVerificationDateChanged();
                }
            });

            $('#<%=btnRSSummaryApproval.ClientID %>').click(function () {
                getCheckedMember();
                if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                    displayMessageBox('Warning', 'Pilih No Rekap JasMed Terlebih Dahulu.');
                }
                else {
                    var srsTrouble2 = "";
                    Methods.getListObject('GetSRSCheckCountDataDouble2List', $('#<%=hdnSelectedMember.ClientID %>').val().substr(1), function (resultSRS2) {
                        for (i = 0; i < resultSRS2.length; i++) {
                            if (srsTrouble2 != "") {
                                srsTrouble2 += ", ";
                            }
                            srsTrouble2 += resultSRS2[i].RSSummaryNo;
                        }
                    });

                    if (srsTrouble2 == "") {
                        var srsTrouble = "";
                        Methods.getListObject('GetSRSCheckCountDataDouble1List', $('#<%=hdnSelectedMember.ClientID %>').val().substr(1), function (resultSRS) {
                            for (i = 0; i < resultSRS.length; i++) {
                                if (srsTrouble != "") {
                                    srsTrouble += ", ";
                                }
                                srsTrouble += resultSRS[i].RSSummaryNo;
                            }
                        });

                        if (srsTrouble == "") {
                            onCustomButtonClick('process');
                        } else {
                            var messageErr = "Tidak dapat proses Approved [" + srsTrouble + "] karena masih memiliki detail PRS yang charges detailnya punya kombinasi [Dokter + Visit + Item + Tanggal Transaksi + Nilai Jasmed] yang charges detailnya diproses PRS >1x.";
                            showToastConfirmation(messageErr, function (resultConfirm) {
                                if (resultConfirm) {
                                    onCustomButtonClick('process');
                                } else {
                                    displayMessageBox('Information', 'Tidak ada No Rekap JasMed yang diproses approved.');
                                }
                            });
                        }
                    } else {
                        var messageErr = "Tidak dapat proses Approved [" + srsTrouble2 + "] karena masih memiliki detail PRS yang charges detailnya diproses PRS >1x.";
                        displayErrorMessageBox('Error', messageErr);
                    }
                }
            });
        }

        function cboVerificationDateChanged() {
            cbpView.PerformCallback();
        }

        function onAfterCustomClickSuccess(type, retval) {
            cbpView.PerformCallback();
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            var result = '';

            $('.grdSelected .chkIsSelected input').each(function () {
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
        }

        function onGetRevenueSharingFilterExpression() {
            var filterExpression = "<%:OnGetRevenueSharingFilterExpression() %>";
            return filterExpression;
        };

        $('#chkCheckAll').live('click', function () {
            var isChecked = $(this).is(':checked');
            $('.grdSelected .chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
            });
        });

        $('#lnkRSSummaryNo').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/RevenueSharing/RevenueSharingSummary/RevenueSharingSummaryPreviewCtl.ascx");
            openUserControlPopup(url, id, 'Approval Detail', 1200, 500);
        });

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var paramedicID = $('#<%:hdnParamedicID.ClientID %>').val();

            var dateFrom = $('#<%:txtPeriodFrom.ClientID %>').val();
            var datePickerFrom = Methods.getDatePickerDate(dateFrom);
            var dateFromYMD = dateToDMYCustom(datePickerFrom);

            var dateTo = $('#<%:txtPeriodTo.ClientID %>').val();
            var datePickerTo = Methods.getDatePickerDate(dateTo);
            var dateToYMD = dateToDMYCustom(datePickerTo);

            var param = "1=1";

            if (code == 'FN-00025') {
                param = "ProcessedDate BETWEEN '" + dateFromYMD + "' AND '" + dateToYMD + "'";
            } else if (code == 'FN-00027') {
                param = paramedicID + "|" + dateFromYMD + ";" + dateToYMD;
            }

            filterExpression.text = param;

            return true;
        }

        function dateToDMYCustom(date) {
            var d = date.getDate();
            var m = date.getMonth() + 1;
            var y = date.getFullYear();
            return '' + y + (m <= 9 ? '0' + m : m) + (d <= 9 ? '0' + d : d);
        }
    </script>
    <div>
        <input type="hidden" id="hdnPageTitle" runat="server" />
        <input type="hidden" id="hdnParamedicID" runat="server" />
        <input type="hidden" id="hdnSelectedMember" runat="server" />
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td style="width: 150px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Approval") %></label>
                            </td>
                            <td>
                                <table>
                                    <colgroup>
                                        <col style="width: 150px;" />
                                        <col style="width: 50px;" />
                                        <col style="width: 150px;" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtApprovalDate" CssClass="datepicker" Width="120px" />
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Periode Proses") %></label>
                            </td>
                            <td>
                                <table>
                                    <colgroup>
                                        <col style="width: 150px;" />
                                        <col style="width: 30px;" />
                                        <col style="width: 150px;" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtPeriodFrom" CssClass="datepicker" Width="120px" />
                                        </td>
                                        <td align="center">
                                            <%=GetLabel("s/d") %>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtPeriodTo" CssClass="datepicker" Width="120px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="overflow-y: scroll;">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="RSSummaryID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-Width="40px">
                                                    <HeaderTemplate>
                                                        <input type="checkbox" id="chkCheckAll" style="text-align: center;" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="cfRSSummaryDateInString" HeaderText="Tanggal Rekap" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                                <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Nomor Rekap" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <a id="lnkRSSummaryNo" <%#: Eval("RSSummaryNo").ToString() == "False" ? "style='display:none'" : ""%>>
                                                            <%#:Eval("RSSummaryNo")%></a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="cfTotalTransactionRevenueAmountInString" HeaderText="Total Rekap" HeaderStyle-HorizontalAlign="Right"
                                                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                                                <asp:BoundField DataField="cfTotalAdjustmentAmountInString" HeaderText="Total Penyesuaian" HeaderStyle-HorizontalAlign="Right"
                                                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                                                <asp:BoundField DataField="cfTotalRevenueSharingAmountInString" HeaderText="Total Akhir" HeaderStyle-HorizontalAlign="Right"
                                                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" />
                                                <asp:BoundField DataField="cfCreatedDateInString" HeaderText="Dibuat pada" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                                                <asp:BoundField DataField="CreatedByName" HeaderText="Dibuat oleh" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                No Data To Display
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
