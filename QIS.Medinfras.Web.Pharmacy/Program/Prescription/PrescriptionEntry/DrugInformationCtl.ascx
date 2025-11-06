<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DrugInformationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.DrugInformationCtl" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_Referralctl">

    $('.imgEditCtl.imgLink').die('click')
    $('.imgEditCtl.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=hdnIDCtl.ClientID %>').val(entity.ID);
        $('#<%=txtPurposeOfMedication.ClientID %>').val(entity.MedicationPurpose);
        $('#<%=txtMedicationAdministration.ClientID %>').val(entity.MedicationAdministration);
        $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
        $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
        cboCoenamRuleCtl.SetValue(entity.GCCoenamRule);
        $('#containerEntryDataCtl').show();
    });

    //#region Item Group
    function onGetItemGroupFilterExpression() {
        return "<%=OnGetItemGroupFilterExpression() %>";
    }
    $('#lblItemGroupMaster.lblLink').die('click');
    $('#lblItemGroupMaster.lblLink').live('click', function () {
        openSearchDialog('itemgroup', onGetItemGroupFilterExpression(), function (value) {
            $('#<%=txtItemGroupCode.ClientID %>').val(value);
            onTxtItemGroupCodeChanged(value);
        });
    });

    $('#lblItemGroupMaster.lblLink').die('change');
    $('#<%=txtItemGroupCode.ClientID %>').live('change', function () {
        onTxtItemGroupCodeChanged($(this).val());
    });

    function onTxtItemGroupCodeChanged(value) {
        var filterExpression = onGetItemGroupFilterExpression() + " AND ItemGroupCode = '" + value + "'";
        Methods.getObject('GetItemGroupMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemGroupID.ClientID %>').val(result.ItemGroupID);
                $('#<%=txtItemGroupName.ClientID %>').val(result.ItemGroupName1);
            }
            else {
                $('#<%=hdnItemGroupID.ClientID %>').val('');
                $('#<%=txtItemGroupCode.ClientID %>').val('');
                $('#<%=txtItemGroupName.ClientID %>').val('');
            }
            cbpViewCtl.PerformCallback('refresh');
        });
    }
    //#endregion

    function onCbpViewCtlEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
                $('#containerEntryDataCtl').hide();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
            }
        }
        else if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPagingDetailItem(pageCount);
        }
        else {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        hideLoadingPanel();
    }

    $('#btnSaveCtl').click(function (evt) {
        if (IsValid(evt, 'fsDrug', 'mpDrug'))
            cbpViewCtl.PerformCallback('save');
        return false;
    });

    $('#btnCancelCtl').die('click');
    $('#btnCancelCtl').live('click', function () {
        $('#containerEntryDataCtl').hide();
    });

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpViewCtl.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion 

    function oncboReferrerSearchCodeValueChanged() {
        onRefreshGridView();
    }

    function onRefreshGridView() {
        $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
        cbpViewCtl.PerformCallback('refresh');
    }

    function onTxtSearchViewSearchClick(s) {
        setTimeout(function () {
            s.SetBlur();
            onRefreshGridView();
        }, 0);
    }
</script>
<input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
<div style="height: 440px; overflow-y: auto; overflow-x: hidden">
    <table class="tblContentArea">
        <tr>
            <td>
                <table>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Obat")%></label>
                        </td>
                        <td>
                            <qis:qisintellisensetextbox runat="server" clientinstancename="txtSearchView" id="txtSearchView"
                                width="378px" watermark="Search">
                                <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                <IntellisenseHints>
                                    <qis:QISIntellisenseHint Text="Nama" FieldName="ItemName1" />
                                    <qis:QISIntellisenseHint Text="Kode" FieldName="ItemCode" />
                                </IntellisenseHints>
                            </qis:qisintellisensetextbox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="width: 150px">
                            <label class="lblLink" id="lblItemGroupMaster">
                                <%=GetLabel("Kelompok Item")%></label>
                        </td>
                        <td>
                            <input type="hidden" value="" runat="server" id="hdnItemGroupID" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 120px" />
                                    <col style="width: 3px" />
                                    <col style="width: 250px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtItemGroupName" ReadOnly="true" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <div id="containerEntryDataCtl" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Edit")%></div>
                    <input type="hidden" id="hdnIDCtl" runat="server" value="" />
                    <fieldset id="fsDrug" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Nama Item")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 120px" />
                                            <col style="width: 3px" />
                                            <col style="width: 250px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtItemCode" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblNormal">
                                    <label>
                                        <%=GetLabel("Coenam Rule")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboCoenamRuleCtl" ClientInstanceName="cboCoenamRuleCtl" Width="125px"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Fungsi Obat")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPurposeOfMedication" Width="350px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Special Instruction")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMedicationAdministration" Width="350px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnSaveCtl" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnCancelCtl" value='<%= GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div>
                    <dxcp:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%" ClientInstanceName="cbpViewCtl"
                        ShowLoadingPanel="false" OnCallback="cbpViewCtl_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ onCbpViewCtlEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent4" runat="server">
                                <asp:Panel runat="server" ID="pnlReferrerGrdView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <img class="imgEditCtl imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" style="float: left; margin-left: 7px" />
                                                    <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                    <input type="hidden" value="<%#:Eval("GCCoenamRule") %>" bindingfield="GCCoenamRule" />
                                                    <input type="hidden" value="<%#:Eval("MedicationPurpose") %>" bindingfield="MedicationPurpose" />
                                                    <input type="hidden" value="<%#:Eval("MedicationAdministration") %>" bindingfield="MedicationAdministration" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="50px"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="100px">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Kode")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%#: Eval("ItemCode")%></div>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Nama Obat")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%#: Eval("ItemName1")%></div>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingReferrer">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingPopup">
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
