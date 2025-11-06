<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageListEntry.master" AutoEventWireup="true" 
    CodeBehind="ReferralResponseList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.ReferralResponseList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><div><%=GetLabel("Refresh")%></div></li>
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

            $('#<%=btnRefresh.ClientID %>').click(function () {
                cbpView.PerformCallback('refresh');
            });

            //#region Verify
            $('.btnResponse').live('click', function () {
                var $tr = $(this).closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                var selectedID = $('#<%=hdnID.ClientID %>').val();
                var param = selectedID + '|';
                openUserControlPopup("~/Program/PatientPage/Planning/Referral/ReferralResponseEntryCtl.ascx", param, "Jawaban Konsul Rawat Bersama", 700, 400);
            });
            //#endregion
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
        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var id = $('#<%:hdnID.ClientID %>').val();
            if (id == '') {
                errMessage.text = 'Belum ada record yang dipilih!';
                return false;
            }
            else {
                if (code == 'PM-00146' || code == 'PM-00147' || code == 'PM-00671' || code == 'PM-00672') {
                    filterExpression.text = "ID = " + id;
                    return true;
                }
            }
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
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                            OnRowDataBound="grdView_RowDataBound" >
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
                                <asp:TemplateField HeaderText="Rujukan Dari" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                    <ItemTemplate>
                                        <div>
                                            <%#Eval("FromPhysicianName")%><br />
                                        </div> 
                                        <div <%# Eval("GCRefferalType").ToString() == "X075^01" ? "Style='display:none'":"" %>>
                                            <%#Eval("FromServiceUnitName")%><br />
                                        </div> 
                                        <div>
                                            Jenis : <span style="font-style: italic"><%#Eval("ReferralType")%> </span>
                                        </div> 
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ToPhysicianName" HeaderText="Dirujuk/Konsultasi Ke" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                <asp:TemplateField HeaderText="Diagnosa" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="250px">
                                    <ItemTemplate>
                                        <div style="height:130px; overflow-y:auto;vertical-align:top;white-space: normal; ">
                                            <%#Eval("DiagnosisText").ToString().Replace("\n","<br />")%><br />
                                        </div>                                                                            
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Catatan Medis" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
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
                                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                    <ItemTemplate>
                                        <div>
                                            <input type="button" id="btnResponse" runat="server" class="btnResponse  w3-btn w3-hover-green" value="Response"
                                                style="height: 35px; width: 100px; background-color: Red; color: White;" />
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
