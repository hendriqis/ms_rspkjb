<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true" 
    CodeBehind="MRNMergeHistory.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.MRNMergeHistory" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Refresh")%></div></li>
    <li id="btnSendToRIS" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("RIS (HL7)")%></div></li>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=GetMenuCaption()%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtFromLogDate.ClientID %>');
            setDatePicker('<%=txtToLogDate.ClientID %>');

            $('#<%=txtFromLogDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtToLogDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=btnRefresh.ClientID %>').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    cbpView.PerformCallback('refresh');
                }
            });

            $('#<%=btnSendToRIS.ClientID %>').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    cbpSendToRIS.PerformCallback($('#<%=hdnToMRN.ClientID %>').val());
                }
            });

            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live('click', function () {
            var detail = $(this).find('.keyField').html();
            var fromMRN = $(this).find('.FromMRN').html();
            var toMRN = $(this).find('.ToMRN').html();
            $('#<%=hdnID.ClientID %>').val(detail);
            $('#<%=hdnFromMRN.ClientID %>').val(fromMRN);
            $('#<%=hdnToMRN.ClientID %>').val(toMRN);
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
        });

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGridView();
            }, 0);
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)

                    setPaging($("#paging"), pageCount, function (page) {
                        cbpView.PerformCallback('changepage|' + page);
                    });
            }
            else {
            }
        }

        function onRefreshGridView() {
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpView.PerformCallback('refresh');
        }
        //#endregion

        function onCbpSendToRISEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            var recordID = s.cpTransactionID;
            if (param[0] == 'success') {
                showToast('SEND TO RIS : SUCCESS', 'Proses Notifikasi Penggabungan Data Pasien ke RIS sukses dilakukan (' + recordID + ')');
            }
            else {
                showToast('SEND TO RIS : FAILED', 'Error Message : ' + param[1]);
            }
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnFromMRN" runat="server" />
    <input type="hidden" value="" id="hdnToMRN" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <div style="padding:=5px">        
        <table>
            <colgroup>
                <col style="width:120px"/>
                <col/>
            </colgroup>
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Change Date")%></label></td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td><asp:TextBox runat="server" CssClass="datepicker" ID="txtFromLogDate" Width="120px" /></td>
                            <td>&nbsp;</td>
                            <td><asp:TextBox runat="server" CssClass="datepicker" ID="txtToLogDate" Width="120px" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Quick Filter")%></label>
                </td>
                <td>
                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                        Width="378px" Watermark="Search">
                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                        <IntellisenseHints>
                            <qis:QISIntellisenseHint Text="Nama" FieldName="FromFullName" />
                            <qis:QISIntellisenseHint Text="No. RM" FieldName="FromMedicalNo" />
                        </IntellisenseHints>
                    </qis:QISIntellisenseTextBox>
                </td>
            </tr>
        </table>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView" ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent2" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="FromMRN" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="hiddenColumn FromMRN" />
                                <asp:BoundField DataField="ToMRN" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="hiddenColumn ToMRN" />
                                <asp:BoundField DataField="cfLogDate" HeaderText="Log Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                                <asp:BoundField DataField="FromMedicalNo" HeaderText="From" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                <asp:BoundField DataField="ToMedicalNo" HeaderText="To" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                <asp:BoundField DataField="ToFullName" HeaderText="Patient Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="CreatedByName" HeaderText="Created By" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="300px"/>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada history penggabungan data pasien")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpSendToRIS" runat="server" Width="100%" ClientInstanceName="cbpSendToRIS"
        ShowLoadingPanel="false" OnCallback="cbpSendToRIS_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpSendToRISEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
