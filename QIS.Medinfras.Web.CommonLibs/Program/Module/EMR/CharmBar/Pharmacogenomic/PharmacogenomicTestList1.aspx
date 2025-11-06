<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true" 
    CodeBehind="PharmacogenomicTestList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PharmacogenomicTestList1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript">
        $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                cbpViewDt.PerformCallback('refresh');
            }
        });

        $('#<%=grdViewDt.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdViewDt.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnItemID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnReferenceNo.ClientID %>').val($(this).find('.accessionNo').html());
            }
        });

        $(function () {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            $('.btnViewReport').live('click', function () {
                $tr = $(this).parent().closest('tr');
                var reportStream = $tr.find('.reportStream').html();
                if (reportStream != '') {
                    window.open("data:application/pdf;base64, " + reportStream, "popupWindow", "width=600, height=600,scrollbars=yes");
                }
                else {
                    displayErrorMessageBox("Pharmacogenomic Report", "Belum ada data report untuk pemeriksaan ini !");
                }
            });
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging Dt
        function onCbpViewDtEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt"), pageCount, function (page) {
                    cbpViewDt.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />   
    <input type="hidden" value="" id="hdnItemID" runat="server" />  
    <input type="hidden" value="" id="hdnReferenceNo" runat="server" />   
    <input type="hidden" value="" id="hdnViewerUrl" runat="server" />    
    <input type="hidden" value="" id="hdnDocumentPath" runat="server" />    
    <input type="hidden" value="0" id="hdn" runat="server" />    
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    
    <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
        <tr>
            <td>
                <div class="containerOrderDt" id="panByTransactionNo">
                    <table style="width:100%">
                        <colgroup>
                            <col style="width:20%"/>
                            <col style="width:80%"/>
                        </colgroup>
                        <tr>
                            <td valign="top">
                                <div style="position: relative;">
                                    <table border="0" cellpadding="0" cellspacing="1" style="width:100%">
                                        <colgroup>
                                            <col style="width:100px"/>
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td colspan="2" valign="top">
                                                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="TransactionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                            <HeaderTemplate>
                                                                                <div><%=GetLabel("Tanggal Transaksi") %> - <%=GetLabel("Time") %></div>
                                                                                <div><%=GetLabel("No. Transaksi") %> | <%=GetLabel("Status") %></div>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div><%#: Eval("cfTransactionDate")%> | <%#: Eval("TransactionTime") %></div>
                                                                                <div><%#: Eval("TransactionNo")%></div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <EmptyDataTemplate>
                                                                        <%=GetLabel("Tidak ada data pemeriksaan pharmacogenomics untuk pasien ini")%>
                                                                    </EmptyDataTemplate>
                                                                </asp:GridView>
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
                                            </td>
                                        </tr>
                                    </table>
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
                                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage2">
                                                    <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Drug Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width = "150px" >
                                                                <ItemTemplate>
<%--                                                                    <div class='<%#: Eval("IsNormal").ToString() == "False" && Eval("cfIsResultInPDF").ToString() == "False" ?  (Eval("cfIsPanicRange").ToString() == "False" ? "isAbnormalColor blink" : "isPanicRangeColor blink") : "" %>'>
                                                                        <%#: Eval("FractionName1") %></div>--%>
                                                                    <div>
                                                                        <%#: Eval("DrugName") %></div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="NalaScore" HeaderText="Nala Score" HeaderStyle-Width = "60px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="NalaScoreV2" HeaderText="Nala Score v2" HeaderStyle-Width = "150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:TemplateField HeaderText="Recommendation Text" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <div style="height:100px; overflow-y:auto;">
                                                                        <%#Eval("RecommendationText").ToString().Replace("\n","<br />")%><br />
                                                                    </div> 
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Caveat Text" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <div style="height:100px; overflow-y:auto;">
                                                                        <%#Eval("CaveatText").ToString().Replace("\n","<br />")%><br />
                                                                    </div> 
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="CaveatType" HeaderText="Caveat Type" HeaderStyle-Width = "150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:HyperLinkField HeaderText="Usage" Text="Usage" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkViewDocument" HeaderStyle-Width="100px" />
                                                            <asp:TemplateField HeaderText="PDF Report" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                                                <ItemTemplate>
                                                                    <div>
                                                                        <input type="button" id="btnViewReport" runat="server" class="btnViewReport  w3-btn w3-hover-blue" value="View Report"
                                                                            style="width: 100px; background-color: Red; color: White;" />
                                                                    </div>                                                                       
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="ReportStream" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="reportStream hiddenColumn" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <span class="blink"><%=GetLabel("Belum ada informasi hasil pemeriksaan untuk transaksi ini") %></span>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>    
                                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt" >
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                    <div class="containerPaging">
                                        <div class="wrapperPaging">
                                            <div id="pagingDt"></div>
                                        </div>
                                    </div> 
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>    
</asp:Content>
