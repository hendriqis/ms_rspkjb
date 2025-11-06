<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SurgeryProcedureGroupLookupCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SurgeryProcedureGroupLookupCtl1" %>
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
        setDatePicker('<%=txtOrderDate.ClientID %>');
        $('#<%=txtOrderDate.ClientID %>').datepicker('option', 'maxDate', '0');
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
        $('.grdLookupProcedureGroupView .chkIsProcessItem input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var id = $tr.find('.keyField').html();

            if (tempSelectedID != "") {
                tempSelectedID += ",";
            }
            tempSelectedID += id;
            count += 1;
        });
        if (count == 0) {
            displayErrorMessageBox("Jenis Tindakan Operasi", "Belum ada jenis tindakan operasi yang dipilih !");
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
            $('#<%=grdLookupProcedureGroupView.ClientID %> tr:eq(1)').click();
        }

        $('#containerImgLoadingViewPopup').hide();
    }

    function onAfterProcessPopupEntry(param) {
        if (typeof onRefreshProcedureGroupGrid == 'function')
            onRefreshProcedureGroupGrid();
    }
</script>
<input type="hidden" id="hdnSelectedID" runat="server" value="" />
<input type="hidden" id="hdnIsNewRecord" runat="server" value="1" />
<input type="hidden" id="hdnLookupID" runat="server" value="" />
<input type="hidden" id="hdnLookupTestOrderID" runat="server" value="0" />
<input type="hidden" id="hdnLookupSurgeryReportID" runat="server" value="0" />
<input type="hidden" id="hdnLinkedRegistrationVisitID" runat="server" value="" />
<input type="hidden" id="hdnVisitID" runat="server" value="" />

    <div>
        <table class="tblEntryContent" style="width: 100%">
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 150px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Tanggal ")%>
                        -
                        <%=GetLabel("Jam Order")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtOrderDate" Width="120px" CssClass="datepicker" runat="server" Enabled="False" />
                </td>
                <td>
                    <asp:TextBox ID="txtOrderTime" Width="80px" CssClass="time" runat="server"
                        Style="text-align: center" Enabled="False" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Pasien")%></label>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtPatientName" Width="100%" runat="server" Enabled="False" />                                
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("No. RM")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMedicalNo" Width="100%" runat="server" Enabled="False" />                                
                </td>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" >
                        <colgroup>
                            <col style="width:120px" />
                            <col  />
                        </colgroup>
                        <tr>
                            <td><label class="lblNormal"><%=GetLabel("No. Registrasi")%></label></td>
                            <td><asp:TextBox ID="txtRegistrationNo" Width="100%" runat="server" Enabled="False" /> </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("No. Order")%></label>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtOrderNo" Width="100%" runat="server" Enabled="False" />                                
                </td>
            </tr>
        </table>
    </div>
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent6" runat="server">
                    <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 400px">
                        <asp:GridView ID="grdLookupProcedureGroupView" runat="server" CssClass="grdLookupProcedureGroupView grdSelected grdPatientPage"
                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsProcessItem" AutoPostBack="false"  />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <%=GetLabel("Jenis Operasi")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div><%#: Eval("ProcedureGroupName")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Kategori Operasi"  DataField="SurgeryClassification" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Belum ada informasi jenis operasi untuk pasien ini") %>
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

