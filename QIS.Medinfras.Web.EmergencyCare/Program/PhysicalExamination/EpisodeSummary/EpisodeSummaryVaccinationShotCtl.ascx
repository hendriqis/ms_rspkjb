<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryVaccinationShotCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EmergencyCare.Program.EpisodeSummaryVaccinationShotCtl" %>

<script type="text/javascript" id="dxss_episodesummaryvaccinationshotctl">
    $(function () {
        registerLiHandler();
    });

    function registerLiHandler() {
        $('.ulVaccinationShotDt li').hover(function () {
            $(this).find('.divVaccinationShotDtInformation').show('fast');
        }, function () {
            $(this).find('.divVaccinationShotDtInformation').hide();
        });
    }
</script>

<style type="text/css">
    .ulVaccinationShotDt                { height:18px;z-index:10; padding: 0; margin: 0; }
    .ulVaccinationShotDt li             { position:relative; list-style-type: none; display: inline-block; width: 40px; text-align: center; cursor: pointer; }
    .ulVaccinationShotDt li .divVaccinationNo { z-index:10; text-align:center; width:100%; height: 15px;border: 1px solid #AAA;}
    .ulVaccinationShotDt li .divVaccinationNo.selected { border:2px dashed #F36F00 !important; }
    .ulVaccinationShotDt li .divVaccinationShotDtInformation { z-index:11; font-size: 9px; display: none; padding: 5px; position: absolute; top: 15px; width: 200px; border: 1px solid #AAA; text-align: left; background-color: White; }
    
    .grdNormal td, .grdNormal th           { font-size: 11px; }
</style>
<h3 class="headerContent" style="padding-left:5px;"><%=GetLabel("Vaccination Shot")%></h3>
<asp:GridView ID="grdView" runat="server" CssClass="grdNormal" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
    OnRowDataBound="grdView_RowDataBound">
    <Columns>
        <asp:BoundField DataField="VaccinationTypeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
        <asp:TemplateField HeaderStyle-Width="100px">
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
                                <img runat="server" id="imgVaccinationStatus" height="13" /><%#:Eval("VaccinationNo") %>                           
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
        No Data To Display
    </EmptyDataTemplate>
</asp:GridView>