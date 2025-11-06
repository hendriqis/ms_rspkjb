<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/ParamedicPage/MPBaseParamedicPageTrx.master"
    AutoEventWireup="true" CodeBehind="RevenueSharingVerificationEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingVerificationEntry" %>

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
    <li id="btnRevSharingVerification" runat="server" crudmode="R">
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
            setDatePicker('<%=txtVerificationDate.ClientID %>');
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
                    showToast('Information', 'Tanggal Awal harus lebih kecil dari Tanggal Akhir');
                } else {
                    cboVerificationDateChanged();
                }
            });

            $('#<%=btnRevSharingVerification.ClientID %>').click(function () {
                getCheckedMember();
                if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                    showToast('Warning', 'Pilih No Bukti Terlebih Dahulu');
                }
                else {
                    onCustomButtonClick('process');
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

        $('#lnkTransactionNo').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/RevenueSharing/RevenueSharingVerification/RevenueSharingVerificationEntryCtl.ascx");
            openUserControlPopup(url, id, 'Verification Detail', 1200, 500);
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
        <input type="hidden" id="hdnSelectedMember" runat="server" />
        <input type="hidden" id="hdnParamedicID" runat="server" />
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td style="width: 150px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Verifikasi") %></label>
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
                                            <asp:TextBox runat="server" ID="txtVerificationDate" CssClass="datepicker" Width="120px" />
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
                                        <col style="width: 50px;" />
                                        <col style="width: 150px;" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtPeriodFrom" CssClass="datepicker" Width="120px" />
                                        </td>
                                        <td>
                                            &nbsp;s/d&nbsp;
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
                                                <asp:BoundField DataField="RSTransactionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-Width="60px">
                                                    <HeaderTemplate>
                                                        <input type="checkbox" id="chkCheckAll" style="text-align: center;" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ProcessedDateInString" HeaderText="Tanggal Proses" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                                                <asp:TemplateField HeaderStyle-Width="200px" HeaderText="No. Bukti" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <a id="lnkTransactionNo" <%#: Eval("RevenueSharingNo").ToString() == "False" ? "style='display:none'" : ""%>>
                                                            <%#:Eval("RevenueSharingNo")%></a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DepartmentName" HeaderText="Asal Pasien" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="TotalTransactionAmount" HeaderText="Nilai Transaksi" HeaderStyle-HorizontalAlign="Right"
                                                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="150px" />
                                                <asp:BoundField DataField="TotalRevenueSharingAmount" HeaderText="Nett Jasa Medis"
                                                    HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}"
                                                    HeaderStyle-Width="150px" />
                                                <%--<asp:TemplateField HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <img class="imgPrint1 imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/printstrook.png")%>' alt="" title="Cetak Rincian Jasa Medis" style="display:none" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
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
                        <div class="containerPaging">
                            <div class="wrapperPaging">
                                <div id="paging">
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
