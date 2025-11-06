<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SOAPTemplateLookupCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SOAPTemplateLookupCtl1" %>
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
        var count = 0;
        $('.grdView .chkIsProcessItem input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var id = $(this).closest('tr').find('.keyField').html();

            if (tempSelectedID != "") {
                tempSelectedID += ",";
            }
            tempSelectedID += id;
            count += 1;
        });
        if (count == 0) {
            var messageBody = "Belum ada template text yang dipilih.";
            displayMessageBox('SOAP Template', messageBody);
            return false;
        }
        else if (count > 1) {
            var messageBody = "Maaf, hanya 1 (satu) record template yang dapat dipilih.";
            displayMessageBox('SOAP Template', messageBody);
            return false;
        }
        else {
            $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
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
        if (typeof onAfterLookUpSOAPTemplate == 'function') {
            onAfterLookUpSOAPTemplate(param);
        }
    }
</script>
<input type="hidden" id="hdnID" runat="server" value="" />
<input type="hidden" id="hdnSelectedID" runat="server" value="" />
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
                                <asp:BoundField DataField="TemplateCode" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsProcessItem" AutoPostBack="false"  />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TemplateCode" HeaderText="Kode" HeaderStyle-Width="60px"/>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div style="text-align: left">
                                            <%= GetLabel("SOAP Text")%>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <textarea style="padding-left: 10px; border: 0; width: 99%; height: 200px; background-color: transparent"
                                                readonly><%#: DataBinder.Eval(Container.DataItem, "TemplateText") %> </textarea>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada template yang tersedia") %>
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

