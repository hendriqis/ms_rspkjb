<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master" AutoEventWireup="true" 
    CodeBehind="PrescriptionOrderList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PrescriptionOrderList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        function onAfterSaveRecordPatientPageEntry() {
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

        //#region Propose
        $('.btnPropose').live('click', function () {
            $btnPropose = $(this);

            $tr = $(this).closest('tr').parent().closest('tr');
            $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
            onCustomButtonClick('Propose');
        });
        //#endregion

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onAfterCustomClickSuccess() {
            $btnPropose.hide();
        }
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
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
                                <asp:BoundField DataField="PrescriptionOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div><%=GetLabel("Date - Time")%></div>
                                        <div style="width:150px;float:left;font-weight:bold"><%=GetLabel("Order No.")%></div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table width="100%">
                                            <colgroup>
                                                <col width="300px" />
                                                <col  />
                                                <col width="100px" />
                                            </colgroup>
                                            <tr>
                                                <td style="vertical-align:top">
                                                    <div><%#: Eval("PrescriptionDateInString")%>, <%#: Eval("PrescriptionTime") %></div>
                                                    <div style="width:150px;float:left;font-weight:bold"><%#: Eval("PrescriptionOrderNo")%></div> <br />
                                                </td>
                                                <td style="vertical-align:top">
                                                    <div><asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Height="130px" Text='<%#: Eval("Remarks")%>' style="background-color:transparent;color:Black; border:0 none" ReadOnly="true" /></div>
                                                </td>
                                                <td align="right" <%# Eval("IsEditable").ToString() == "False" ? "Style='display:none'":"style='vertical-align:top'" %> ><div><input type="button" class="btnPropose w3-btn w3-hover-blue" value="SEND TO PHARMACY" style="height:45px;background-color:Red;color;white" /></div></td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
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
</asp:Content>
