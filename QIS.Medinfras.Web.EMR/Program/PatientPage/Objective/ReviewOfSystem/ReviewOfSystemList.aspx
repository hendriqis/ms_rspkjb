<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master" AutoEventWireup="true" 
    CodeBehind="ReviewOfSystemList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.ReviewOfSystemList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnOpenSymptomChecker" runat="server" CRUDMode="C" style="display:none" ><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><div><%=GetLabel("Symptom Checker")%></div></li>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            $('#<%=btnOpenSymptomChecker.ClientID %>').click(function () {
                var url = ResolveUrl("~/Program/PatientPage/Objective/ReviewOfSystem/SymptomCheckerCtl.ascx");
                openUserControlPopup(url, '', 'Symptom Checker', 900, 630);
            });
        });

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        function onAfterDeleteClickSuccess() {
            cbpView.PerformCallback('refresh');
            $('#<%=hdnID.ClientID %>').val("");
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
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                            OnRowDataBound="grdView_RowDataBound" >
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div><%=GetLabel("Review Of System") %></div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <b>
                                                <%#: Eval("ObservationDateInString")%>,
                                                <%#: Eval("ObservationTime") %>,
                                                <%#: Eval("ParamedicName") %>
                                            </b>
                                        </div>
                                        <div>
                                            <asp:Repeater ID="rptReviewOfSystemDt" runat="server">
                                                <ItemTemplate>
                                                    <div style="padding-left:20px;float:left;width:300px;">
                                                        <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" && Eval("IsOther").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>><strong> <%#: DataBinder.Eval(Container.DataItem, "ROSystem") %> : </strong></span>&nbsp;
                                                        <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" && Eval("IsOther").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>><%#: DataBinder.Eval(Container.DataItem, "cfRemarks")%></span>
                                                    </div>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <br style="clear:both"/>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>
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
