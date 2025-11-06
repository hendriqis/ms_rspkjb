<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master" AutoEventWireup="true" 
    CodeBehind="VaccinationRecordList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.VaccinationRecordList" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            registerLiHandler();
        });

        function registerLiHandler() {  
            var timer = null;
            $('.ulVaccinationShotDt li').hover(function () {
                $li = $(this);
                timer = setTimeout(function () {
                    $li.find('.divVaccinationShotDtInformation').show('fast');
                }, 300);
            }, function () {
                if (timer) {
                    clearTimeout(timer);
                }
                $(this).find('.divVaccinationShotDtInformation').hide();
            });

            $('.ulVaccinationShotDt li .divVaccinationNo').click(function () {
                $('.ulVaccinationShotDt li .divVaccinationNo.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).parent().find('.keyField').val());
            });
            $('.ulVaccinationShotDt li .divVaccinationNo').first().click();
        }

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        

        function onCbpViewEndCallback(s) {
            registerLiHandler();
        }
    </script>
    <style type="text/css">
        .ulVaccinationShotDt                { height:30px;z-index:10; padding: 0; margin: 0; }
        .ulVaccinationShotDt li             { position:relative; list-style-type: none; display: inline-block; width: 80px; text-align: center; cursor: pointer; }
        .ulVaccinationShotDt li .divVaccinationNo { z-index:10; text-align:center; width:100%; height: 100%;border: 1px solid #AAA;}
        .ulVaccinationShotDt li .divVaccinationNo.selected { border:1px dashed #F36F00 !important; }
        .ulVaccinationShotDt li .divVaccinationShotDtInformation { z-index:11; font-size: 10px; display: none; padding: 5px; position: absolute; top: 15px; width: 200px; border: 1px solid #AAA; text-align: left; background-color: White; }
    </style>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server">
                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                    <asp:GridView ID="grdView" runat="server" CssClass="grdNormal" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                        OnRowDataBound="grdView_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="VaccinationTypeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                            <asp:TemplateField HeaderStyle-Width="250px">
                                <HeaderTemplate><%=GetLabel("Vaccination Type") %></HeaderTemplate>
                                <ItemTemplate><%#: Eval("VaccinationTypeName")%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Repeater ID="rptVaccinationShotDt" runat="server" OnItemDataBound="rptVaccinationShotDt_ItemDataBound">
                                        <HeaderTemplate>
                                            <ul class="ulVaccinationShotDt">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li>
                                                <input type="hidden" class="keyField" value="<%#:Eval("HeaderID") %>" />
                                                <div class="divVaccinationNo" style="background: <%#: DataBinder.Eval(Container.Parent.Parent.Parent, "DataItem.DisplayColor")%> ">
                                                    <table style="margin: 0 auto;">
                                                        <tr>
                                                            <td><img runat="server" id="imgVaccinationStatus" height="20" /></td>
                                                            <td><%#:Eval("VaccinationNo") %></td>
                                                        </tr>
                                                    </table>                                                  
                                                </div>                                                
                                                <div class="divVaccinationShotDtInformation">
                                                    <div><%#:Eval("ParamedicName") %></div>
                                                    <div><%#:Eval("AgeInYear") %> yr <%#:Eval("AgeInMonth") %> mo <%#:Eval("AgeInDay") %> day</div>
                                                    <div><%#:Eval("VaccineName")%> (<%#:Eval("Dose") %> <%#:Eval("DoseUnit") %>) </div>
                                                </div>
                                            </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
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
</asp:Content>
