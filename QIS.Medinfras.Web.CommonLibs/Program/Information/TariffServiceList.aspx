<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true" 
    CodeBehind="TariffServiceList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TariffServiceList" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>


<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            $('.btnViewComponent').die('click');
            $('.btnViewComponent').live('click', function () {
                $('#<%=txtServiceTotalTariff.ClientID %>').val($(this).closest("td").find('.hdnTariff').val());
                $('#<%=txtServiceTariffComp1.ClientID %>').val($(this).closest("td").find('.hdnTariffComp1').val());
                $('#<%=txtServiceTariffComp2.ClientID %>').val($(this).closest("td").find('.hdnTariffComp2').val());
                $('#<%=txtServiceTariffComp3.ClientID %>').val($(this).closest("td").find('.hdnTariffComp3').val());
                pcTariffComponent.Show();
            });


            //#region Item Group
            $('#lblItemGroup.lblLink').click(function () {
                var filterExpression = "<%:OnGetItemGroupFilterExpression() %> AND IsDeleted = 0";
                openSearchDialog('itemgroup', filterExpression, function (value) {
                    $('#<%=txtItemGroupCode.ClientID %>').val(value);
                    onTxtItemGroupCodeChanged(value);
                });
            });

            $('#<%=txtItemGroupCode.ClientID %>').change(function () {
                onTxtItemGroupCodeChanged($(this).val());
            });

            function onTxtItemGroupCodeChanged(value) {
                var filterExpression = "ItemGroupCode = '" + value + "'";
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
                    cbpView.PerformCallback('refresh');
                });
            }
            //#endregion
        });

        function onRefreshGrid() {
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpView.PerformCallback('refresh');
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrid();
            }, 0);
        }

        function onCboTariffSchemeValueChanged(s) {
            onRefreshGrid();
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
                    $('#grdView tr:eq(2)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#grdView tr:eq(2)').click();
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnListClassID" runat="server" />
    <input type="hidden" value="" id="hdnListClassName" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <div style="padding: 15px">
        <div class="pageTitle">
            <%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsTariffList">
                        <table>
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 400px" />
                            </colgroup>
                            <tr id="trServiceUnit" runat="server">
                                <td><label><%=GetLabel("Skema Tarif") %></label></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboTariffScheme" ClientInstanceName="cboTariffScheme" Width="200px" runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboTariffSchemeValueChanged(s); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label><%=GetLabel("Quick Filter")%></label></td>
                                <td>
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView" Width="300px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="ItemName1" FieldName="ItemName1" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblLink" id="lblItemGroup"><%=GetLabel("Kelompok Item")%></label></td>
                                <td>
                                    <input type="hidden" id="hdnItemGroupID" value="" runat="server" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:100px"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtItemGroupName" ReadOnly="true" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td>
                    <input type="hidden" value="" id="hdnID" runat="server" />
                    <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="overflow-y: scroll;">
                                        <table id="grdView" class="grdSelected grdPatientPage" cellspacing="0" rules="all" >
                                            <tr>
                                                <th class="keyField" rowspan="2"></th>
                                                <th class="hiddenColumn" rowspan="2"></th>
                                                <th style="width:300px" rowspan="2" align="left"><%=GetLabel("Item")%></th>
                                                <th colspan="<%=ClassCount %>"><%=GetLabel("Kelas") %></th>
                                            </tr>
                                            <tr>                                
                                                <asp:Repeater ID="rptTariffClassHeader" runat="server">
                                                    <ItemTemplate>
                                                        <th style="width:150px"><%#:Eval("ClassName") %></th>
                                                    </ItemTemplate>
                                                </asp:Repeater>     
                                            </tr>
                                            <asp:ListView ID="lvwView" runat="server" OnItemDataBound="lvwView_ItemDataBound">
                                            <LayoutTemplate>                                
                                                <tr runat="server" id="itemPlaceholder" ></tr>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="keyField"><%#: Eval("ItemID") %></td>
                                                    <td class="hiddenColumn">
                                                        <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                        <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                        <input type="hidden" value="<%#:Eval("ClassID") %>" bindingfield="ClassID" />
                                                        <input type="hidden" value="<%#:Eval("Tariff") %>" bindingfield="Tariff" />
                                                    </td>
                                                    <td><%#: Eval("ItemName1")%></td>
                                                    <asp:Repeater ID="rptTariffClass" runat="server">
                                                        <ItemTemplate>
                                                            <td align="right"><%#:Eval("Tariff", "{0, 0:N2}")%>&nbsp;
                                                            <span <%#: IsItemProduct.ToString() == "True" ? "style='display:none'" : ""%>><input type="button" class="btnViewComponent" title='<%=GetLabel("Komponen Tarif") %>' value="..." style="width:12%"  /></span>
                                                            <input type="hidden" value="<%#:Eval("TariffComp1",  "{0, 0:N2}") %>" class="hdnTariffComp1"/>
                                                            <input type="hidden" value="<%#:Eval("TariffComp2",  "{0, 0:N2}") %>" class="hdnTariffComp2"/>
                                                            <input type="hidden" value="<%#:Eval("TariffComp3",  "{0, 0:N2}") %>" class="hdnTariffComp3"/>
                                                            <input type="hidden" value="<%#:Eval("Tariff",  "{0, 0:N2}") %>" class="hdnTariff"/>
                                                            </td>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tr>
                                            </ItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr class="trEmpty">
                                                    <td colspan="12">
                                                        <%=GetLabel("Data Tidak Tersedia") %>
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                        </table>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>    
                        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                        <div class="containerPaging">
                            <div class="wrapperPaging">
                                <div id="paging"></div>
                            </div>
                        </div> 
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <!-- Popup Tariff Component -->
    <dxpc:ASPxPopupControl ID="pcTariffComponent" runat="server" ClientInstanceName="pcTariffComponent" CloseAction="CloseButton"
        Height="200px" HeaderText="Tariff Component" Width="200px" Modal="True" PopupAction="None"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dxpc:PopupControlContentControl runat="server" ID="pccc1">
                <dx:ASPxPanel ID="pnlTariffComponent" runat="server" Width="100%">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                                <fieldset id="fsUnitTariff" style="margin:0"> 
                                <div style="text-align: left; width: 100%;">
                                    <table>
                                        <colgroup>
                                            <col style="width: 200px"/>
                                        </colgroup>
                                        <tr>
                                            <td valign="top">
                                                <table>
                                                    <colgroup>
                                                        <col style="width:70px"/>
                                                        <col style="width:120px"/>
                                                    </colgroup>
                                                    <tr>
                                                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Total")%></label></td>
                                                        <td><span><asp:TextBox ID="txtServiceTotalTariff" runat="server" ReadOnly ="true" Width="100%" CssClass="txtCurrency"/></span></td>
                                                    </tr>
                                                    <tr><td colspan="2"><hr /></td></tr>  
                                                    <tr>
                                                        <td class="tdLabel"><label class="lblNormal"><%=GetTariffComponent1Text()%></label></td>
                                                        <td><asp:TextBox ID="txtServiceTariffComp1" CssClass="txtCurrency" ReadOnly ="true" Width="100%" runat="server"/></td>
                                                    </tr>                          
                                                    <tr>
                                                        <td class="tdLabel"><label class="lblNormal"><%=GetTariffComponent2Text()%></label></td>
                                                        <td><asp:TextBox ID="txtServiceTariffComp2" CssClass="txtCurrency" ReadOnly ="true" Width="100%" runat="server"/></td>
                                                    </tr>                           
                                                    <tr>    
                                                        <td class="tdLabel"><label class="lblNormal"><%=GetTariffComponent3Text()%></label></td>
                                                        <td><asp:TextBox ID="txtServiceTariffComp3" CssClass="txtCurrency" ReadOnly ="true" Width="100%" runat="server"/></td>
                                                    </tr>                             
                                                </table>  
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="margin-left: auto; margin-right: auto; margin-top: 10px;">
                                        <tr>
                                            <td>
                                                <input type="button" id="btnUnitTariffOK" value='<%= GetLabel("OK")%>' onclick="pcTariffComponent.Hide();"/>
                                            </td>
                                        </tr>
                                    </table>
                                </div>                                
                            </fieldset>                                
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
