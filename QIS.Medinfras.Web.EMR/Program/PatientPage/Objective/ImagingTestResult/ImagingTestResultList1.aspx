<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master" AutoEventWireup="true" 
    CodeBehind="ImagingTestResultList1.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.ImagingTestResultList1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><div><%=GetLabel("Refresh")%></div></li>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
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
                var testOrderID = $('#<%=hdnID.ClientID %>').val();
                var itemID = $('#<%=hdnItemID.ClientID %>').val();
                if (itemID != '' && testOrderID != '') {
                    var id = itemID + '|' + testOrderID;
                    var url = ResolveUrl("~/Program/PatientPage/Objective/ImagingTestResult/ViewImagingResultCtl1.ascx");
                    openUserControlPopup(url, id, 'Hasil Pemeriksaan Radiologi', 1000, 500);
                }

            });

            $('#<%=btnRefresh.ClientID %>').click(function () {
                cbpView.PerformCallback('refresh');
            });

            $('.btnViewPDF').live('click', function () {
                var fileName = $(this).closest('tr').find('.fileName').html();
                if (fileName != '') {
                    var url = $('#<%=hdnDocumentPath.ClientID %>').val() + fileName;
                    window.open(url, "popupWindow", "width=600, height=600,scrollbars=yes");
                }
                else {
                    showToast("ERROR", 'Error Message : ' + "Tidak ada file hasil untuk pemeriksaan ini !");
                }
            });

            $('.btnViewer').live('click', function () {
                $('#<%=hdnReferenceNo.ClientID %>').val($(this).closest('tr').find('.accessionNo').html());
                var postData = $('#<%=hdnReferenceNo.ClientID %>').val();
                if (postData != '' && postData != '&nbsp;') {
                    if ($('#<%=hdnRISVendor.ClientID %>').val() == "X081^04") {
                        var viewerUrl = $('#<%=hdnViewerUrl.ClientID %>').val() + postData + "&id=1&redirect=y";
                        window.open(viewerUrl, "popupWindow", "width=600, height=600,scrollbars=yes");
                    }
                    else if ($('#<%=hdnRISVendor.ClientID %>').val() == "X081^05") {
                        var viewerUrl = $(this).closest('tr').find('.imageViewerLinkUrl').html();
                        window.open(viewerUrl, "popupWindow", "width=600, height=600,scrollbars=yes");
                    }
                    else {
                        var viewerUrl = $('#<%=hdnViewerUrl.ClientID %>').val() + postData;
                        window.open(viewerUrl, "popupWindow", "width=600, height=600,scrollbars=yes");
                    }
                }
                else {
                    showToast("ERROR", 'Error Message : ' + "Accession Number untuk memnbuka file image tidak tersedia !");
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
            else if (param[0] == "getToken") {
                var token = param[1];
                if (token != "") {
                    var viewerUrl = $('#<%=hdnViewerUrl.ClientID %>').val() + token;
                    window.open(viewerUrl, "popupWindow", "width=600, height=600,scrollbars=yes");
                    $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
                }
                else {
                    showToast("WARNING", "Gambar tidak ditemukan");
                }
            }
            else {
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
            }
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />   
    <input type="hidden" value="" id="hdnItemID" runat="server" />  
    <input type="hidden" value="" id="hdnReferenceNo" runat="server" />   
    <input type="hidden" value="" id="hdnViewerUrl" runat="server" />    
    <input type="hidden" value="" id="hdnDocumentPath" runat="server" />    
    <input type="hidden" value="0" id="hdnLinkedVisitID" runat="server" />    
    <input type="hidden" value="0" id="hdn" runat="server" />    
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnRISVendor" runat="server" value="" />
    <input type="hidden" id="hdnIsRISUsingPDFResult" runat="server" value="" />
    <table style="width:100%">
        <colgroup>
            <col style="width:30%"/>
            <col style="width:70%"/>
        </colgroup>
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
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="TransactionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div><%=GetLabel("Tanggal Transaksi") %> - <%=GetLabel("Time") %></div>
                                                    <div><%=GetLabel("No. Transaksi") %> | <%=GetLabel("Status") %></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div><%#: Eval("TransactionDateInString")%> | <%#: Eval("TransactionTime") %></div>
                                                    <div><%#: Eval("TransactionNo")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada data pemeriksaan untuk pasien ini")%>
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
                                    <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdViewDt_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div><%=GetLabel("Nama Pemeriksaan") %></div>
                                                    <div><%=GetLabel("Dokter Pelaksana") %></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div><span style="font-weight:bold"><%#: Eval("ItemName1") %></span></div>
                                                    <div><span style="color:blue"><%#: Eval("ParamedicName")%></span></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DetailReferenceNo" HeaderText = "Accession No." HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="180px" ItemStyle-CssClass="accessionNo"/>
                                            <asp:BoundField DataField="FileName" HeaderText = "File Name" HeaderStyle-CssClass="hiddenColumn" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="180px" ItemStyle-CssClass="hiddenColumn fileName"/>
                                            <asp:BoundField DataField="ImageViewerLinkUrl" HeaderText = "Viewer Link" HeaderStyle-CssClass="hiddenColumn" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="180px" ItemStyle-CssClass="hiddenColumn imageViewerLinkUrl"/>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="80px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("STATUS")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <%#: Eval("cfRISBridgingStatus")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Report" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <div>
                                                        <input type="button" id="btnViewReport" runat="server" class="btnViewReport" value="View Report"
                                                            style="height: 25px; width: 100px; background-color: Red; color: White;" />
                                                    </div>                                                                       
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="File PDF" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <div>
                                                        <input type="button" id="btnViewPDF" runat="server" class="btnViewPDF" value="View PDF"
                                                            style="height: 25px; width: 100px; background-color: Red; color: White;" />
                                                    </div>                                                                       
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PACS Viewer" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <div>
                                                        <input type="button" id="btnViewer" runat="server" class="btnViewer" value="View Image"
                                                            style="height: 25px; width: 100px; background-color: Red; color: White;" />
                                                    </div>                                                                       
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada data pemeriksaan untuk pasien ini") %>
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
    
</asp:Content>
