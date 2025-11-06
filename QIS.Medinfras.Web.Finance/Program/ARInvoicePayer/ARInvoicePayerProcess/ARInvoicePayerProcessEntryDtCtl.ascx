<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ARInvoicePayerProcessEntryDtCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Finance.Program.ARInvoicePayerProcessEntryDtCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_apinvoicesupplierprocessctl">
    setDatePicker('<%=txtPeriodFrom.ClientID %>');
    setDatePicker('<%=txtPeriodTo.ClientID %>');

    $('#chkCheckAll').live('click', function () {
        var isChecked = $(this).is(':checked');
        $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
            $(this).prop('checked', isChecked);
        });
    });

    $('#btnRefresh').click(function () {
        cbpProcessDetail.PerformCallback('refresh');
    });

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
    }

    function onBeforeSaveRecord(errMessage) {
        getCheckedMember();
        if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
            errMessage.text = 'Silakan Pilih Piutang Terlebih Dahulu';
            return false;
        }
        return true;
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingPopup"), pageCount, function (page) {
            getCheckedMember();
            cbpProcessDetail.PerformCallback('changepage|' + page);
        });
    });

    function onCbpProcessDetailEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                getCheckedMember();
                cbpProcessDetail.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion
</script>

<div style="height:440px; overflow-y:auto;overflow-x: hidden">
    <input type="hidden" id="hdnSelectedMember" runat="server" />
    <input type="hidden" id="hdnARInvoiceID" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>
            <td>
                <table>
                    <colgroup>
                        <col style="width:120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("Asal Pasien")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" Width="200px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><label class="lblNormal"><%=GetLabel("Periode Transaksi") %></label></td>
                        <td colspan="2">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td><asp:TextBox runat="server" Width="120px" ID="txtPeriodFrom" CssClass="datepicker" /></td>
                                    <td style="width: 5px;">s/d</td>
                                    <td><asp:TextBox runat="server" Width="120px" ID="txtPeriodTo" CssClass="datepicker" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td><input type="button" id="btnRefresh" value="Refresh" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                    ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpProcessDetailEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="PaymentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                            <HeaderTemplate>
                                                <input type="checkbox" id="chkCheckAll" style="text-align:center;" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PaymentNo" HeaderText="No Piutang" HeaderStyle-Width="140px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="PaymentDateInString" HeaderText="Tanggal Piutang" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="RegistrationNo" HeaderText="No Registrasi" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left"  />
                                        <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  />
                                        <asp:BoundField DataField="PaymentDateInString" HeaderText="Tanggal Piutang" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="TotalPaymentAmount" HeaderText="Total Piutang" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="200px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup"></div>
                    </div>
                </div> 
            </td>
        </tr>
    </table>
    <div style="width:100%;text-align:right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>