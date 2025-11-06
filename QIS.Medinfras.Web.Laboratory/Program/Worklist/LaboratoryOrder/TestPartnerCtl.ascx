<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestPartnerCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Lsboratory.Program.LaboratoryOrder.TestPartnerCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_TestPartnerCtl">

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=txtItemCodeCtl.ClientID %>').val(entity.ItemCode);
        $('#<%=txtItemNameCtl.ClientID %>').val(entity.ItemName1);
        $('#<%=TextPartnerCodeCtl.ClientID %>').val(entity.BusinessPartnerCode);
        $('#<%=TextPartnerNameCtl.ClientID %>').val(entity.BusinessPartnerName);
        $('#<%=dtTransactionIdCtl.ClientID %>').val(entity.ID);
        $('#containertestPartnerEntryDataCtl').show();
    });

    function onCbpViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
                $('#containertestPartnerEntryDataCtl').hide();
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

    $('#btnTestPartnerSaveCtl').click(function (evt) {
        cbpView.PerformCallback('save');
        return false;
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
            cbpView.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion 

    function onRefreshGridView() {
        cbpView.PerformCallback('refresh');
    }

    $(function () {
        $('#lblTestPartnerctl.lblLink').click(function () {
            //            openSearchDialog('testpartner', " GCTestPartnerType = 'X230^001' ", function (value) {
            openSearchDialog('testpartner', "", function (value) {
                $('#<%=TextPartnerCodeCtl.ClientID %>').val(value);
                ontxttestPartnerCodeCtlChanged(value);
            });
        });
    });

    function ontxttestPartnerCodeCtlChanged(value) {
        //        var filterExpression = "GCTestPartnerType = 'X230^001' AND BusinessPartnerCode ='" + value + "'";
        var filterExpression = "BusinessPartnerCode ='" + value + "'";
        Methods.getObject('GetvTestPartnerList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnBusinessPartnerIDctl.ClientID %>').val(result.BusinessPartnerID);
                $('#<%=TextPartnerNameCtl.ClientID %>').val(result.BusinessPartnerName);
            }
            else {

                $('#<%=hdnBusinessPartnerIDctl.ClientID %>').val('');
                $('#<%=TextPartnerNameCtl.ClientID %>').val('');
                $('#<%=TextPartnerCodeCtl.ClientID %>').val('');
            }
        });
    }

    $('#btnTestPartnerSaveCtl').click(function (evt) {
        cbpView.PerformCallback('save');
        return false;
    });

    $('#btnTestPartnerCancelCtl').die('click');
    $('#btnTestPartnerCancelCtl').live('click', function () {
        $('#containertestPartnerEntryDataCtl').hide();
    });

    $('#<%:TextPartnerCodeCtl.ClientID %>').live('change', function () {
        ontxttestPartnerCodeCtlChanged($(this).val());
    });
</script>
<input type="hidden" value="" id="hdnBusinessPartnerIDctl" runat="server" />
<input type="hidden" value="" id="dtTransactionIdCtl" runat="server" />
<input type="hidden" value="" id="HiddenIdCtl" runat="server" />
<div style="height: 500px; overflow-y: auto">
    <div class="pageTitle">
        <%=GetLabel("Edit Test Partner")%></div>
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <div id="containertestPartnerEntryDataCtl" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnIDCtl" runat="server" value="" />
                    <fieldset id="fsTestPartner" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <tr>
                                <td>
                                    <h4>
                                        <%=GetLabel("Entry Data")%></h4>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col style="width: 10%" />
                                            <col style="width: 10%" />
                                            <col style="width: 30%" />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblMandatory" id="lblItem">
                                                    <%=GetLabel("Item")%></label>
                                            </td>
                                            <td style="padding-right: 5px">
                                                <asp:TextBox ID="txtItemCodeCtl" Width="100%" ReadOnly="true" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtItemNameCtl" Width="100%" ReadOnly="true" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblTestPartnerctl">
                                                    <%=GetLabel("Test Partner")%></label>
                                            </td>
                                            <td style="padding-right: 5px">
                                                <asp:TextBox ID="TextPartnerCodeCtl" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextPartnerNameCtl" Width="100%" ReadOnly="true" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <input type="button" id="btnTestPartnerSaveCtl" value='<%= GetLabel("Simpan")%>'
                                                    style="width: 60px" />
                                                <input type="button" id="btnTestPartnerCancelCtl" value='<%= GetLabel("Batal")%>'
                                                    style="width: 60px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlReferrerGrdView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class="imgLink imgEdit" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" />
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                    <input type="hidden" value="<%#:Eval("BusinessPartnerCode") %>" bindingfield="BusinessPartnerCode" />
                                                    <input type="hidden" value="<%#:Eval("BusinessPartnerName") %>" bindingfield="BusinessPartnerName" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="50px"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-VerticalAlign="Top">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Kode Item")%>
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
                                                        <%=GetLabel("Nama Item")%>
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
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Test Partner")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%#: Eval("BusinessPartnerName")%></div>
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
