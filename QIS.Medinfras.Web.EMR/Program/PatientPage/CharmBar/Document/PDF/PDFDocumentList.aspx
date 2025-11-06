<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true" 
    CodeBehind="PDFDocumentList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PDFDocumentList" EnableViewState="false" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.core.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.widget2.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.mouse.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.draggable.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.droppable.js")%>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.effects.core.js")%>'></script>

    <script id="dxis_episodesummaryctl1" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery-ui.js")%>' type='text/javascript'></script>
    <script id="dxis_episodesummaryctl2" src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.easing.1.3.js")%>' type='text/javascript'></script>
    <script id="dxis_episodesummaryctl3" src='<%= ResolveUrl("~/Libs/Scripts/jquery/booklet/jquery.booklet.1.3.1.js")%>' type='text/javascript'></script>
    <script type="text/javascript">
        $(function () {

            $('.lnkViewDocument a').live('click', function () {
                var fileName = $(this).closest('tr').find('.fileName').html();
                var DocumentPath = $(this).closest('tr').find('.DocumentPath').html();
                var url = $('#<%:hdnPatientDocumentUrl.ClientID %>').val() + fileName;
                DocumentPath = DocumentPath.replace('&nbsp;', '');
                if (DocumentPath != "") {
                    url = $('#<%:hdnPatientDocumentUrl1.ClientID %>').val() + DocumentPath + fileName;
                }  
                window.open(url, "popupWindow", "width=600, height=600,scrollbars=yes");
            });

        });


        function onRefreshControl(filterExpression) {
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
    </script>

    <input type="hidden" id="hdnPatientDocumentUrl" value="" runat="server" />
    <input type="hidden" id="hdnPatientDocumentUrl1" value="" runat="server" />
    <table style="width:100%" cellpadding="0" cellspacing="0">
        <tr style="height:550px">
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:510px">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" RowStyle-CssClass="trDraggable" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="cfDocumentDate" HeaderText="Date" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="DocumentType" HeaderText="Document Type" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name" HeaderStyle-Width = "200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="FileName" HeaderText="File Name" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn fileName" />
                                            <asp:BoundField DataField="cfDocumentPath" HeaderText="DocumentPath" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn DocumentPath" />
                                            <asp:TemplateField HeaderText="Remarks" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <div style="height:100px; overflow-y:auto;">
                                                        <%#Eval("Notes").ToString().Replace("\n","<br />")%><br />
                                                    </div> 
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:HyperLinkField HeaderText="View" Text="View" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkViewDocument" HeaderStyle-Width="100px" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada dokumen elektronik untuk pasien ini")%>
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
        </tr>
    </table>

    
</asp:Content>

