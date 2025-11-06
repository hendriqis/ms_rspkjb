<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master" AutoEventWireup="true" 
    CodeBehind="DentalAssessment1.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.DentalAssessment1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        var idxSelectedTooth = 0;
        $(function () {
            $('#<%=grdTreatmentDetail.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdTreatmentDetail.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnTreatmentDetailID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdTreatmentDetail.ClientID %> tr:eq(1)').click();


            $('.tblTooth tr td img').click(function () {
                var id = $(this).parent().find('div').html();
                $('#<%=hdnToothID.ClientID %>').val(id);
                cbpTreatmentDetail.PerformCallback(id);
                $('.tblTooth tr td img').attr('class', 'notSelected');
                idxSelectedTooth = $('.tblTooth tr td img').index($(this));
                $(this).attr('class', 'selected');
            });
            $('.tblTooth tr td img').attr('class', 'notSelected');

            $('.tblTooth tr td img:eq(0)').click();
        });

        function onAfterSaveRecordPatientPageEntry() {
            cbpListTooth.PerformCallback();
        }

        $(function () {
            setPaging($("#pagingTreatmentDetail"), 0, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });

            var nextTreatmentPageCount = parseInt('<%=NextTreatmentPageCount %>');
            setPaging($("#pagingNextTreatment"), nextTreatmentPageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });


        function onCbpListToothEndCallback(s) {
            hideLoadingPanel();
            $('.tblTooth tr td img').click(function () {
                var id = $(this).parent().find('div').html();
                $('#<%=hdnToothID.ClientID %>').val(id);
                cbpTreatmentDetail.PerformCallback(id);
                $('.tblTooth tr td img').attr('class', 'notSelected');
                idxSelectedTooth = $('.tblTooth tr td img').index($(this));
                $(this).attr('class', 'selected');
            });
            $('.tblTooth tr td img:eq(' + idxSelectedTooth + ')').click();
            cbpTreatmentDetail.PerformCallback('refresh');
            cbpNextTreatment.PerformCallback('refresh');
        }

        //#region Paging Treatment Detail
        function onCbpTreatmentDetailEndCallback(s) {
            $('#containerImgLoadingTreatmentDetail').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdTreatmentDetail.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingTreatmentDetail"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdTreatmentDetail.ClientID %> tr:eq(1)').click();

            var toothID = $('#<%=hdnToothID.ClientID %>').val();
            $('.thColumnCaption').html($('.hdnTooth[toothid=' + toothID + ']').val());

        }
        //#endregion
        
        //#region Paging Next Detail
        function onCbpNextTreatmentEndCallback(s) {
            $('#containerImgLoadingNextTreatment').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdNextTreatment.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingNextTreatment"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdNextTreatment.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>

    <style type="text/css">
        .tblTooth                           { margin: auto; }
        .tblTooth tr td                     { padding: 2px; font-size: 0.8em; }
        .tblTooth tr td img                 { cursor:pointer; }
        .tblTooth tr td img.selected        { border:2px solid #F4921B; }
        .tblTooth tr td img.notSelected     { border:2px solid transparent; }
        .tblToothHeader                     { width:100%;font-weight:bolder; }
        .tblToothHeader tr td               { background-color: #CCC; font-size: 0.9em;width: 50%; }
    </style>

    <input type="hidden" value="" id="hdnTreatmentDetailID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
        
    <asp:Repeater ID="rptListTooth" runat="server">
        <ItemTemplate>
            <input type="hidden" toothid="<%#: GetToothNumber((string)Eval("StandardCodeID"))%>" class="hdnTooth" value="<%#:Eval("StandardCodeName") %>" />
        </ItemTemplate>
    </asp:Repeater>
    
    <table style="width:100%;font-size:14px">
        <colgroup style="width:50%" />
        <colgroup style="width:25%" />
        <colgroup />
        <tr>
            <td style="vertical-align:top">
                <input type="hidden" runat="server" id="hdnToothID" value="0" />
                <dxcp:ASPxCallbackPanel ID="cbpListTooth" runat="server" Width="100%" ClientInstanceName="cbpListTooth"
                    ShowLoadingPanel="false" OnCallback="cbpListTooth_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpListToothEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent10" runat="server">
                            <asp:Panel runat="server" ID="Panel2">
                                <div style="text-align:center;" id="containerTableTooth" runat="server">
                                </div>    
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>          
            </td>
            <td style="vertical-align:top;padding-left:15px">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpTreatmentDetail" runat="server" Width="100%" ClientInstanceName="cbpTreatmentDetail"
                        ShowLoadingPanel="false" OnCallback="cbpTreatmentDetail_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingTreatmentDetail').show(); }"
                            EndCallback="function(s,e){ onCbpTreatmentDetailEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:300px">
                                    <asp:GridView ID="grdTreatmentDetail" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="thColumnCaption">
                                                <HeaderTemplate>
                                                    <div><%=GetLabel("Dental Treatment") %></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div><%#: Eval("ToothStatus")%></div>
                                                    <div style="width:180px;float:left"><%#: Eval("ParamedicName")%></div>
                                                    <div><%#: Eval("TreatmentDateInString")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display") %>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <div class="imgLoadingGrdView" id="containerImgLoadingTreatmentDetail" >
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingTreatmentDetail"></div>
                        </div>
                    </div> 
                </div>
            </td>
            <td style="vertical-align:top;padding-left:15px">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpNextTreatment" runat="server" Width="100%" ClientInstanceName="cbpNextTreatment"
                        ShowLoadingPanel="false" OnCallback="cbpNextTreatment_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingNextTreatment').show(); }"
                            EndCallback="function(s,e){ onCbpNextTreatmentEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid" Style="height:300px">
                                    <asp:GridView ID="grdNextTreatment" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div><%=GetLabel("Next Planned Treatment") %></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div><%#: Eval("ToothStatus")%></div>
                                                    <div style="width:180px;float:left"><%#: Eval("ParamedicName")%></div>
                                                    <div><%#: Eval("TreatmentDateInString")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display") %>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <div class="imgLoadingGrdView" id="containerImgLoadingNextTreatment" >
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingNextTreatment"></div>
                        </div>
                    </div> 
                </div>
            </td>
        </tr>
    </table>


    
</asp:Content>
