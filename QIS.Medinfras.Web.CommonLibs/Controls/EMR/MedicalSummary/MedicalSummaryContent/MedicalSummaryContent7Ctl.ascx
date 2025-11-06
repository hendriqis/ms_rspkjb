<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicalSummaryContent7Ctl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.MedicalSummaryContent7Ctl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_MedicalSummaryContent1Ctl">
    $(function () {
        $('#<%=grdContent7View.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdContent7View.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            $('#<%=hdnIsResponse.ClientID %>').val($(this).find('.IsReply').html());
        });
        $('#<%=grdContent7View.ClientID %> tr:eq(1)').click();

        //#region Verify
        $('.btnResponse').live('click', function () {
            var $tr = $(this).closest('tr');
            $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
            var selectedID = $('#<%=hdnID.ClientID %>').val();
            var param = selectedID + '|';
            openUserControlPopup("~/Libs/Controls/EMR/Information/ViewPhysicianReferralResponseCtl.ascx", param, "Jawaban Konsul Rawat Bersama", 700, 400);
        });
        //#endregion
    });

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingContent7"), pageCount, function (page) {
            cbpContent7View.PerformCallback('changepage|' + page);
        });
    });

    function onCbpContent7ViewEndCallback(s) {
        $('#containerImgLoadingViewContent7').hide();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdContent7View.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingContent7"), pageCount, function (page) {
                cbpContent7View.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdContent7View.ClientID %> tr:eq(1)').click();
    }
    //#endregion
</script>

<input type="hidden" value="" id="hdnID" runat="server" />
<input type="hidden" id="hdnIsResponse" runat="server" value="0" />
<input type="hidden" id="hdnFilterExpression" runat="server" value="" />
<div class="w3-border divContent w3-animate-left">
    <dxcp:ASPxCallbackPanel ID="cbpContent7View" runat="server" Width="100%" ClientInstanceName="cbpContent7View"
        ShowLoadingPanel="false" OnCallback="cbpContent7View_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewContent7').show(); }"
            EndCallback="function(s,e){ onCbpContent7ViewEndCallback(s); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server">
                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                    <asp:GridView ID="grdContent7View" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                        OnRowDataBound="grdContent7View_RowDataBound" >
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
                                <ItemTemplate>
                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                    <input type="hidden" value="<%#:Eval("cfReferralDate") %>" bindingfield="cfReferralDate" />
                                    <input type="hidden" value="<%#:Eval("ReferralTime") %>" bindingfield="ReferralTime" />
                                    <input type="hidden" value="<%#:Eval("FromPhysicianID") %>" bindingfield="FromPhysicianID" />
                                    <input type="hidden" value="<%#:Eval("FromPhysicianName") %>" bindingfield="FromPhysicianName" />
                                    <input type="hidden" value="<%#:Eval("GCRefferalType") %>" bindingfield="GCRefferalType" />
                                    <input type="hidden" value="<%#:Eval("ToPhysicianID") %>" bindingfield="ToPhysicianID" />
                                    <input type="hidden" value="<%#:Eval("ToPhysicianName") %>" bindingfield="ToPhysicianName" />
                                    <input type="hidden" value="<%#:Eval("cfResponseDateTime") %>" bindingfield="cfResponseDateTime" />
                                    <input type="hidden" value="<%#:Eval("IsReply") %>" bindingfield="IsReply" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="cfReferralDate" HeaderText="Tanggal" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  />
                            <asp:BoundField DataField="ReferralTime" HeaderText="Jam" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="ReferralType" HeaderText="Jenis Rujukan" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="ToPhysicianName" HeaderText="Dirujuk/Konsultasi Ke" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="Diagnosa" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="250px">
                                <ItemTemplate>
                                    <div style="height:130px; overflow-y:auto;vertical-align:top;white-space: normal; ">
                                        <%#Eval("DiagnosisText").ToString().Replace("\n","<br />")%><br />
                                    </div>                                                                            
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Catatan Medis" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <div style="height:130px; overflow-y:auto;vertical-align:top;white-space: normal; ">
                                        <%#Eval("MedicalResumeText").ToString().Replace("\n","<br />")%><br />
                                    </div>                                                                            
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Terapi yang sudah diberikan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="220px">
                                <ItemTemplate>
                                    <div style="height:130px; overflow-y:auto;vertical-align:top;white-space: normal; ">
                                        <%#Eval("PlanningResumeText").ToString().Replace("\n","<br />")%><br />
                                    </div>                                                                            
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="cfResponseDateTime" HeaderText="Tanggal Response" HeaderStyle-Width="130px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="cfIsResponse" HeaderText="Response" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <div <%# Eval("IsReply").ToString() == "False" ? "Style='display:none'":"" %>>
                                        <input type="button" id="btnResponse" runat="server" class="btnResponse" value="Baca Respon"
                                            style="height: 25px; width: 100px; background-color: Red; color: White;" />
                                    </div>                                                                       
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <%=GetLabel("Tidak ada riwayat konsultasi / rawat bersama")%>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </asp:Panel>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>    
    <div class="imgLoadingGrdView" id="containerImgLoadingViewContent7" >
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div>
    <div class="containerPaging">
        <div class="wrapperPaging">
            <div id="pagingContent7"></div>
        </div>
    </div> 
</div>

