<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true" 
    CodeBehind="OutpatientVisitHistory.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.OutpatientVisitHistory" EnableViewState="false" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript">
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

        $(function () {
            setDatePicker('<%=txtFromDate.ClientID %>');
            $('#<%=txtFromDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtToDate.ClientID %>');
            $('#<%=txtToDate.ClientID %>').datepicker('option', 'maxDate', '0');
        });

    </script>

    <table style="width:100%; margin-top: 10px;" cellpadding="0" cellspacing="0">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" style = " margin-bottom:10px">
                    <colgroup>
                        <col style="width: 125px" />
                        <col style="width: 150px" />
                        <col style="width: 10px" />
                        <col style="width: 150px" />
                        <col style="width: 145px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Kunjungan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td style="text-align:center">
                            <%=GetLabel("s/d") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td rowspan="2">
                            <input type="button" value='<%= GetLabel("Apply Filter")%>' onclick="cbpView.PerformCallback('refresh');" style="width:100px;" class="w3-btn w3-hover-blue" />
                        </td>
                    </tr>
                    <tr>
                        <td />
                        <td colspan="3">
                            <asp:CheckBox ID="chkIsComplexVisit" runat="server" Text=" Kasus Kompleks (Multi-Diagnosa)" Checked="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
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
                                            <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="cfVisitDate" HeaderText = "Tanggal" HeaderStyle-Width="120" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width = "200px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Unit Pelayanan ")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div><%#: Eval("ParamedicName")%></div>
                                                    <div><%#: Eval("ServiceUnitName")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="300">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Diagnosa ")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                  <div><%#: Eval("cfPatientDiagnosis")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="350">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Obat yang diberikan")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <textarea style="padding-left: 10px; border: 0; width: 99%; height: 100px; background-color: transparent; overflow:auto"
                                                readonly><%#: DataBinder.Eval(Container.DataItem, "cfMedicationSummary") %> </textarea>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="350">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Pemeriksaan Penunjang")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <textarea style="padding-left: 10px; border: 0; width: 99%; height: 100px; background-color: transparent; overflow:auto"
                                                readonly><%#: DataBinder.Eval(Container.DataItem, "cfExaminationSummary") %> </textarea>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="350">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Tindakan")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <textarea style="padding-left: 10px; border: 0; width: 99%; height: 100px; background-color: transparent; overflow:auto"
                                                readonly><%#: DataBinder.Eval(Container.DataItem, "cfTreatmentSummary") %> </textarea>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="350">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Alergi")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <textarea style="border: 0; width: 99%; height: 100px; background-color: transparent; overflow:auto"
                                                readonly><%#: DataBinder.Eval(Container.DataItem, "cfPatientAllergy") %> </textarea>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada kunjungan rawat jalan untuk pasien ini")%>
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