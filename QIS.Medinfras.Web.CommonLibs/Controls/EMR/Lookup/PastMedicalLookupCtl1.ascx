<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PastMedicalLookupCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PastMedicalLookupCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_testorderdetail1">
    $(function () {
    });

    function onBeforeProcess(param) {
        if (!getSelectedItem()) {
            return false;
        }
        else {
            return true;
        }
    }

    $('.chkIsProcessItem input').live('change', function () {
        var $cell = $(this).closest("td");
        var $tr = $cell.closest('tr');
        var isChecked = $(this).is(":checked");
        if ($(this).is(':checked')) {
            $cell.addClass('gridCheckBoxSelectedItem');
        }
        else {
            $cell.removeClass('gridCheckBoxSelectedItem');
        }
    });

    function getSelectedItem() {
        var tempSelectedID = "";
        var tempSelectedItem = "";
        var count = 0;
        $('.grdView .chkIsProcessItem input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var id = $(this).closest('tr').find('.keyField').html();
            var visitDate = $(this).closest('tr').find('.visitDate').html();
            var serviceUnitName = $(this).closest('tr').find('.serviceUnitName').html();
            var paramedicName = $(this).closest('tr').find('.paramedicName').html();
            var diagnosis = $(this).closest('tr').find('.diagnosis').html();

            if (tempSelectedID != "") {
                tempSelectedID += "|";
            }
            if (tempSelectedItem != "") {
                tempSelectedItem += "\n";
            }

            tempSelectedID += id;
            tempSelectedItem += (visitDate + "," + serviceUnitName + "," + paramedicName + ", " + diagnosis);

            count += 1;
        });
        if (count == 0) {
            var messageBody = "Belum ada riwayat kunjungan yang dipilih.";
            displayMessageBox('Riwayat Kunjungan', messageBody);
            return false;
        }
        else {
            $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
            $('#<%=hdnSelectedItem.ClientID %>').val(tempSelectedItem);
            return true;
        }
    }

    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#paging"), pageCount, function (page) {
            cbpView.PerformCallback('changepage|' + page);
        });
    });


    function onCbpViewEndCallback(s) {
        var param = s.cpResult.split('|');

        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            cbpView.PerformCallback('refresh');
        }
        else {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }

        $('#containerImgLoadingViewPopup').hide();
    }

    function onAfterProcessPopupEntry(param) {
        if (typeof onAfterVisitHistoryLookUp == 'function') {
            onAfterVisitHistoryLookUp(param);
        }
    }
</script>
<input type="hidden" id="hdnID" runat="server" value="" />
<input type="hidden" id="hdnSelectedID" runat="server" value="" />
<input type="hidden" id="hdnSelectedItem" runat="server" value="" />
<input type="hidden" id="hdnTemplateGroup" runat="server" value="1" />

    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent6" runat="server">
                    <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 400px">
                        <asp:GridView ID="grdView" runat="server"  CssClass="grdView"
                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsProcessItem" AutoPostBack="false"  />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="VisitDateInString" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn visitDate" />
                                <asp:BoundField DataField="ServiceUnitName" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn serviceUnitName" />
                                <asp:BoundField DataField="ParamedicName" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn paramedicName" />
                                <asp:BoundField DataField="DisplayPatientDiagnosis" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn diagnosis" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <%=GetLabel("Visit Information")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div><%#: Eval("VisitDateInString")%>, <%#: Eval("RegistrationNo")%></div>
                                        <div><%#: Eval("ServiceUnitName")%></div>
                                        <div><%#: Eval("ParamedicName")%></div>
                                        <div style="font-style:italic; font-weight:bold"><%#: Eval("DisplayPatientDiagnosis")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Belum ada riwayat kunjungan untuk pasien ini") %>
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging">
                </div>
            </div>
        </div>
    </div>

