<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master" AutoEventWireup="true" 
    CodeBehind="NursingOutcomeList.aspx.cs" Inherits="QIS.Medinfras.Web.Nursing.Program.NursingOutcomeList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle" style="padding-top:10px; min-height: 50px; height:50px">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $('#<%=grdView.ClientID %> tr:gt(0)').live('click', function () {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            cbpView1.PerformCallback('refresh');
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }


        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        function onCbpView1EndCallback(s) {
            hideLoadingPanel();
        }

        //#region Diagnose
        function onGetDiagnoseFilterExpression() {
            var filterExpression = "<%:OnGetDiagnoseFilterExpression() %>";
            return filterExpression;
        }

        $(function () {
            $('#lblNursingDiagnose.lblLink').live('click',function () {
                openSearchDialog('nursingDiagnose', onGetDiagnoseFilterExpression(), function (value) {
                    $('#<%=txtDiagnoseCode.ClientID %>').val(value);
                    onTxtDiagnoseCodeChanged(value);
                });
            });


            $('#<%=txtDiagnoseCode.ClientID %>').change(function () {
                onTxtDiagnoseCodeChanged($(this).val());
            });

            function onTxtDiagnoseCodeChanged(value) {
                var filterExpression = onGetDiagnoseFilterExpression() + " AND NurseDiagnoseCode = '" + value + "'";
                Methods.getObject('GetNursingDiagnoseList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnNursingDiagnoseID.ClientID %>').val(result.NurseDiagnoseID);
                        $('#<%=txtDiagnoseName.ClientID %>').val(result.NurseDiagnoseName);
                    }
                    else {
                        $('#<%=hdnNursingDiagnoseID.ClientID %>').val('0');
                        $('#<%=txtDiagnoseCode.ClientID %>').val('');
                        $('#<%=txtDiagnoseName.ClientID %>').val('');
                    }
                });
                cbpView.PerformCallback('refresh');
            }
        });
        //#endregion

        $('.lnkNursingIndicator').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/NursingOutcome/NursingDiagnoseItemIndicatorEntryCtl.ascx");
            openUserControlPopup(url, id, 'Kriteria Hasil Diagnosis', 900, 500);
        });


    </script>
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <div style="position: relative;">
        <table width="50%">
            <colgroup>
                <col width="150px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel"><label class="lblLink" id="lblNursingDiagnose"><%=GetLabel("Diagnosa Keperawatan")%></label></td>
                <td>
                    <input type="hidden" value="" runat="server" id="hdnNursingDiagnoseID" />
                    <table style="width:100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width:20%"/>
                            <col style="width:3px"/>
                            <col/>
                        </colgroup>
                        <tr>
                            <td><asp:TextBox ID="txtDiagnoseCode" Width="100%" runat="server" /></td>
                            <td>&nbsp;</td>
                            <td><asp:TextBox ID="txtDiagnoseName" Width="100%" runat="server" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table width="100%">
            <colgroup>
                <col width="25%" />
                <col />
            </colgroup>
            <tr valign="top">
                <td>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="NursingItemGroupSubGroupID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div style="padding-left:3px">
                                                        <%=GetLabel("Komponen Diagnosa")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style='margin-left:<%#: Eval("CfMargin") %>0px;'><%#: Eval("NursingItemGroupSubGroupText")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="#Item" DataField="CfComponentCount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No Data To Display
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </td>
                <td>
                    <dxcp:ASPxCallbackPanel ID="cbpView1" runat="server" Width="100%" ClientInstanceName="cbpView1"
                        ShowLoadingPanel="false" OnCallback="cbpView1_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e){ onCbpView1EndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="pnlView1" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdView1" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="NursingDiagnoseItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="NursingItemText" HeaderText="Deskripsi" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>                                            
                                            <asp:BoundField DataField="Scale1Text" HeaderText="Skala 1" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px"/> 
                                            <asp:BoundField DataField="Scale2Text" HeaderText="Skala 2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px"/> 
                                            <asp:BoundField DataField="Scale3Text" HeaderText="Skala 3" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px"/> 
                                            <asp:BoundField DataField="Scale4Text" HeaderText="Skala 4" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px"/> 
                                            <asp:BoundField DataField="Scale5Text" HeaderText="Skala 5" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px"/> 
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                                                <ItemTemplate>
                                                    <a <%#: Eval("IsUsingIndicator").ToString() == "False" ? "style='display:none'" : ""%> class="lnkNursingIndicator">Indikator</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" HeaderText="Indikator">
                                                <ItemTemplate>
                                                    <div <%#: Eval("IndicatorCount").ToString() == "0" ? "style='display:none'" : ""%>>
                                                        <asp:Label runat="server" ID="lblIndicatorCount" Text='<%#: Eval("IndicatorCount") %>' />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No Data To Display
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>     
                </td>
            </tr>
        </table>
        
    </div>
</asp:Content>