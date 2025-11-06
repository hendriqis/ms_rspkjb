<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VitalSignLookupCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.VitalSignLookupCtl1" %>
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

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpView.PerformCallback('save');
        return false;
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        $('#<%=hdnID.ClientID %>').val($row.find('.hdnID').val());
        var itemName = $row.find('.hdnItemName').val();
        var message = "Are You Sure Want To Delete This Record <b>" + itemName + "</b> ?";
        showToastConfirmation(message, function (result) {
            if (result) {
                cbpView.PerformCallback('delete');
            }
        });
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var itemID = $row.find('.hdnItemID').val();
        var diagnoseID = $row.find('.hdnDiagnoseID').val();
        var itemName = $row.find('.hdnItemName').val();
        var remarks = $row.find('.hdnRemarks').val();
        var isCITO = $row.find('.hdnIsCITO').val();

        ledItem.SetValue(itemID);
        $('#<%=hdnID.ClientID %>').val($row.find('.hdnID').val());
        $('#<%=hdnItemID.ClientID %>').val(itemID);
        cboDiagnose.SetValue(diagnoseID);
        $('#<%=txtRemarks.ClientID %>').val(remarks);
        $('#<%:chkIsCITO.ClientID %>').prop("checked", isCITO == 'True');

        $('#containerPopupEntryData').show();
    });

    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#paging"), pageCount, function (page) {
            cbpView.PerformCallback('changepage|' + page);
        });
    });

    function onLedItemLostFocus(value) {
        $('#<%=hdnItemID.ClientID %>').val(value);
    }

    function onCbpViewEndCallback(s) {
        var param = s.cpResult.split('|');

        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
            }
            else {
                $('#containerPopupEntryData').hide();
                cbpView.PerformCallback('refresh');
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            }
            else {
                var pageCount = parseInt(param[2]);
                cbpView.PerformCallback('refresh');
            }
        }
        else if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            cbpView.PerformCallback('refresh');
        }
        else {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }

        $('#containerPopupEntryData').hide();
        $('#containerImgLoadingViewPopup').hide();
    }
</script>
<input type="hidden" id="hdnID" runat="server" value="" />
<input type="hidden" id="hdnTestOrderType" runat="server" value="" />
<input type="hidden" id="hdnTestOrderID" runat="server" value="" />
<input type="hidden" id="hdnItemID" runat="server" value="" />

    <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
        <fieldset id="fsEntryPopup" style="margin: 0">
            <table class="tblEntryDetail" style="width: 100%">
                <tr>
                    <td>
                        <table style="width: 100%">
                            <colgroup>
                                <col style="width: 100px" />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Item")%></label>
                                </td>
                                <td colspan="2">
                                    <qis:QISSearchTextBox ID="ledItem" ClientInstanceName="ledItem" runat="server" Width="500px"
                                        ValueText="ItemID" FilterExpression="IsDeleted = 0" DisplayText="ItemName1" MethodName="GetvServiceUnitItemList">
                                        <ClientSideEvents ValueChanged="function(s){ onLedItemLostFocus(s.GetValueText()); }" />
                                        <Columns>
                                            <qis:QISSearchTextBoxColumn Caption="Item Name" FieldName="ItemName1" Description="i.e. Cholera"
                                                Width="300px" />
                                            <qis:QISSearchTextBoxColumn Caption="Item Code" FieldName="ItemCode" Description="i.e. A09"
                                                Width="100px" />
                                        </Columns>
                                    </qis:QISSearchTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Diagnose")%></label></td>
                                <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboDiagnose" ClientInstanceName="cboDiagnose" Width="500px" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Remarks")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" Width="500px" runat="server" TextMode="MultiLine" />
                                </td>
                            </tr>
                            <tr>
                                <td />
                                <td><asp:CheckBox ID="chkIsCITO" Width="100px" runat="server" Text="CITO" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' style="width: 80px;
                                                        height: 25px" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' style="width: 80px;
                                                        height: 25px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent6" runat="server">
                    <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 400px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage"
                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                            OnRowDataBound="grdView_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div style="text-align: left">
                                            <%= GetLabel("Daftar Pemeriksaan")%>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <b>
                                                <%#: Eval("ObservationDateInString")%>,
                                                <%#: Eval("ObservationTime") %>,
                                                <%#: Eval("ParamedicName") %>
                                            </b>
                                        </div>
                                        <div>
                                            <asp:Repeater ID="rptVitalSignDt" runat="server">
                                                <ItemTemplate>
                                                    <div style="padding-left: 20px; float: left; width: 300px;">
                                                        <strong>
                                                            <div style="width: 110px; float: left;" class="labelColumn">
                                                                <%#: DataBinder.Eval(Container.DataItem, "VitalSignLabel") %></div>
                                                            <div style="width: 20px; float: left;">
                                                                :</div>
                                                        </strong>
                                                        <div style="float: left;">
                                                            <%#: DataBinder.Eval(Container.DataItem, "DisplayVitalSignValue") %></div>
                                                    </div>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <br style="clear: both" />
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display") %>
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

