<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true"
    CodeBehind="LaboratoryResultList2.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.LaboratoryResultList2" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtPeriodFrom.ClientID %>');
            setDatePicker('<%=txtPeriodTo.ClientID %>');
        });

        $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnIDCBCtl.ClientID %>').val($(this).find('.keyField').html());
                cbpViewDt.PerformCallback('refresh');
            }
        });

        $(function () {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            //#region Item
            function onGetItemFilterExpression() {
                var mrn = $('#<%=hdnMRNCBCtl.ClientID %>').val();
                var startDate = $('#<%:txtPeriodFrom.ClientID %>').val();
                var startDateInDatePicker = Methods.getDatePickerDate(startDate);
                var startDateFormatString = Methods.dateToString(startDateInDatePicker);
                var endDate = $('#<%:txtPeriodTo.ClientID %>').val();
                var endDateInDatePicker = Methods.getDatePickerDate(endDate);
                var endDateFormatString = Methods.dateToString(endDateInDatePicker);

                var filterExpression = "ItemID IN (SELECT ItemID FROM vDistinctLaboratoryItem WHERE MRN = '" + mrn + "' AND (ResultDate BETWEEN '" + startDateFormatString + "' AND '" + endDateFormatString + "'))";
                return filterExpression;
            }

            $('#<%:lblItem.ClientID %>.lblLink').click(function () {
                openSearchDialog('item', onGetItemFilterExpression(), function (value) {
                    onTxtItemChanged(value);
                });
            });

            function onTxtItemChanged(value) {
                var filterExpression = onGetItemFilterExpression() + " AND ItemCode = '" + value + "'";
                Methods.getObject('GetItemMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                        $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                    }
                    else {
                        $('#<%=hdnItemID.ClientID %>').val('');
                        $('#<%=txtItemName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('#btnRefresh').live('click', function () {
                var itemName = $('#<%=txtItemName.ClientID %>').val();
                if (itemName != '') {
                    cbpView.PerformCallback('refresh');
                }
                else {
                    ShowSnackbarError('Pilih Item Terlebih dahulu');
                }
            });
        });

        function onCbpViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDtEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();
        }
    </script>
    <input type="hidden" value="" id="hdnIDCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnReferenceNoCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnViewerUrlCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnDocumentPathCBCtl" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitIDCBCtl" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionCBCtl" runat="server" value="" />
    <input type="hidden" id="hdnRISVendorCBCtl" runat="server" value="" />
    <input type="hidden" id="hdnIsRISUsingPDFResultCBCtl" runat="server" value="" />
    <input type="hidden" id="hdnMedicalNoCBCtl" runat="server" value="" />
    <input type="hidden" id="hdnMRNCBCtl" runat="server" value="" />
    <table style="width: 100%">
        <colgroup>
            <col style="width: 30%" />
            <col style="width: 70%" />
        </colgroup>
        <tr>
            <td>
                <table>
                    <colgroup>
                        <col style="width: 110px" />
                        <col style="width: 150px" />
                        <col style="width: 350px" />
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Periode")%></label>
                        </td>
                        <td colspan="2">
                            <table width="100%" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" Width="120px" ID="txtPeriodFrom" CssClass="datepicker" />
                                                </td>
                                                <td style="width: 30px; text-align: center">
                                                    s/d
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" Width="120px" ID="txtPeriodTo" CssClass="datepicker" />
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
                            <input type="hidden" value="0" id="hdnItemID" runat="server" />
                            <label class="lblLink" id="lblItem" runat="server">
                                <%=GetLabel("Pemeriksaan")%></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" Width="292px" ID="txtItemName" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <div id="divRefresh" runat="server" style="float: left; margin-top: 0px;">
                                <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="FractionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div>
                                                        <%=GetLabel("Artikel Pemeriksaan") %>
                                                        <div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <%#: Eval("FractionName1")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada data artikel pemeriksaan untuk pasien ini")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </div>
            </td>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                        ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                            EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                    <table id="grdViewDt" class="grdSelected grdPatientPage" cellspacing="0" rules="all"
                                        style="overflow: auto;" width="100%">
                                        <tr>
                                            <th style="width: 20px">
                                                <<
                                            </th>
                                            <asp:Repeater ID="rptFractionDateHeader" runat="server">
                                                <ItemTemplate>
                                                    <th style="width: 90px; font-weight: bold; font-size: 14pt" align="center">
                                                        <%#: Eval("cfCreatedDate")%>
                                                    </th>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <th>
                                                &nbsp;
                                            </th>
                                            <th style="width: 20px">
                                                >>
                                            </th>
                                        </tr>
                                        <asp:ListView ID="lvwViewDt" runat="server" OnItemDataBound="lvwViewDt_ItemDataBound"
                                            Style="width: 100%">
                                            <LayoutTemplate>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="background-color: Lightgray; text-align: center; font-size: 14pt; font-weight: bold;
                                                        vertical-align: middle">
                                                    </td>
                                                    <asp:Repeater ID="rptFractionDetailValue" runat="server">
                                                        <ItemTemplate>
                                                            <td align="left">
                                                                <table class="rptFractionDetailValue1" border="0" cellpadding="0" cellspacing="0"
                                                                    width="100%">
                                                                    <tr>
                                                                        <td align="center">
                                                                            <div class='<%# Eval("IsNormal").ToString() == "False" ? "blink": "" %>' style='<%# Eval("IsNormal").ToString() == "False" ? "color: red;": "" %> font-size: 25px;
                                                                                height: 50px; width: 150px;' />
                                                                            <b>
                                                                                <%#: Eval("Fractionvalue") %>
                                                                                <%#: Eval("MetricUnitName") %>
                                                                            </b></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tr>
                                            </ItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr class="trEmpty">
                                                    <td colspan="10">
                                                        <%=GetLabel("Tidak ada pasien yang sedang dalam perawatan") %>
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                    </table>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
