<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true"
    CodeBehind="FallRiskAssessmentFormList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.FallRiskAssessmentFormHistoryList1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript">
        $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnGCAssessmentTypeCBCtl.ClientID %>').val($(this).find('.keyField').html());
            cbpViewDt.PerformCallback('refresh');
        });

        $('#<%=grdViewDt.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdViewDt.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnIDCBCtl.ClientID %>').val($(this).find('.keyField').html());
            $('#<%=hdnAssessmentDateCBCtl.ClientID %>').val($(this).find('.assessmentDate').html());
            $('#<%=hdnAssessmentTimeCBCtl.ClientID %>').val($(this).find('.assessmentTime').html());
            $('#<%=hdnParamedicIDCBCtl.ClientID %>').val($(this).find('.paramedicID').html());
            $('#<%=hdnParamedicNameCBCtl.ClientID %>').val($(this).find('.paramedicName').html());
            $('#<%=hdnAssessmentLayoutCBCtl.ClientID %>').val($(this).find('.formLayout').html());
            $('#<%=hdnAssessmentValuesCBCtl.ClientID %>').val($(this).find('.formValue').html());
            $('#<%=hdnIsFallRiskCBCtl.ClientID %>').val($(this).find('.cfIsFallRisk').html());
            $('#<%=hdnFallRiskScoreCBCtl.ClientID %>').val($(this).find('.fallRiskScore').html());
            $('#<%=hdnGCFallRiskScoreTypeCBCtl.ClientID %>').val($(this).find('.gcFallRiskScoreType').html());
            $('#<%=hdnIsInitialAssessmentCBCtl.ClientID %>').val($(this).find('.isInitialAssessment').html());
        });
        $(function () {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            $('#<%=rblItemType.ClientID %> input').change(function () {
                cbpView.PerformCallback('refresh');
                cbpViewDt.PerformCallback('refresh');
            });
        });

        $('.lnkView').live('click', function () {
            var formType = $(this).attr("assessmentType");
            var id = $(this).attr('recordID');
            var date = $(this).attr('assessmentDate');
            var time = $(this).attr('assessmentTime');
            var ppa = $(this).attr("paramedicName");
            var isInitialAssessment = $(this).attr("isInitialAssessment");
            var layout = $(this).attr("assessmentLayout");
            var values = $(this).attr("assessmentValue");
            var isFallRisk = $(this).attr("isFallRisk");
            var riskScoreType = $(this).attr("riskScoreType");
            var visitID = $(this).attr("visitID");

            var medicalNo = $('#<%=hdnPageMedicalNo.ClientID %>').val();
            var patientName = $('#<%=hdnPagePatientName.ClientID %>').val();
            var patientDOB = $('#<%=hdnPagePatientDOB.ClientID %>').val();
            var registrationNo = $('#<%=hdnPageRegistrationNo.ClientID %>').val();
            var patientInfo = medicalNo + "|" + patientName + "|" + patientDOB + "|" + registrationNo;

            var param = formType + '|' + id + '|' + date + '|' + time + '|' + ppa + '|' + isInitialAssessment + '|' + layout + '|' + values + '|' + isFallRisk + '|' + riskScoreType + '|' + visitID + '|' + patientInfo;
            var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/ViewFallRiskAssessmentCtl.ascx");
            openUserControlPopup(url, param, "Pengkajian Resiko Jatuh", 800, 600);
        });

        function onRefreshControl(filterExpression) {
            cbpView.PerformCallback('refresh');
            cbpViewDt.PerformCallback('refresh');
        }

        //#region Paging Header
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
                    grdView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging Dt
        function onCbpViewDtEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt"), pageCount, function (page) {
                    cbpViewDt.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <input type="hidden" id="hdnPageTitleCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitIDCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentIDCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedicCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnMRNCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnPageMedicalNo" runat="server" />
    <input type="hidden" value="" id="hdnPagePatientName" runat="server" />
    <input type="hidden" value="" id="hdnPagePatientDOB" runat="server" />
    <input type="hidden" value="" id="hdnPageRegistrationNo" runat="server" />
    <input type="hidden" runat="server" id="hdnGCAssessmentTypeCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicIDCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnIDCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentDateCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentTimeCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnParamedicIDCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnParamedicNameCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentLayoutCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentValuesCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnIsInitialAssessmentCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnIsFallRiskCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnFallRiskScoreCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnGCFallRiskScoreTypeCBCtl" value="" />
    <table style="width: 100%">
        <colgroup>
            <col style="width: 30%" />
            <col style="width: 70%" />
        </colgroup>
        <tr>
            <td valign="top">
                <div style="position: relative;">
                    <table border="0" cellpadding="0" cellspacing="1">
                        <colgroup>
                            <col style="width: 100px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Filter Daftar")%></label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblItemType" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table">
                                    <asp:ListItem Text=" Semua" Value="0" Selected="True" />
                                    <asp:ListItem Text=" Kunjungan/Perawatan saat ini " Value="1" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top">
                                <table border="0" cellpadding="0" cellspacing="1">
                                </table>
                                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="StandardCodeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="StandardCodeName" HeaderText="Jenis Pengkajian" HeaderStyle-CssClass="gridColumnText"
                                                            ItemStyle-CssClass="gridColumnText" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Tidak ada template form pengkajian yang bisa digunakan")%>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="paging">
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                        ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                            EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="AssessmentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="cfAssessmentDate" HeaderText = "Tanggal" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center"/>
                                            <asp:BoundField DataField="AssessmentTime" HeaderText = "Jam" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentTime" ItemStyle-CssClass="assessmentTime" />
                                            <asp:BoundField DataField="ParamedicID" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicID hiddenColumn" ItemStyle-CssClass="paramedicID hiddenColumn"/>
                                            <asp:BoundField DataField="ParamedicName" HeaderText = "Dikaji Oleh" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="paramedicName" />
                                            <asp:BoundField DataField="FallRiskScore" HeaderText = "Skor" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="FallRiskScoreType" HeaderText = "Kesimpulan" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfAssessmentDatePickerFormat" HeaderText = "Values" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentDate hiddenColumn" ItemStyle-CssClass="assessmentDate hiddenColumn" />
                                            <asp:BoundField DataField="GCFallRiskAssessmentType" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formType hiddenColumn" ItemStyle-CssClass="formType hiddenColumn" />
                                            <asp:BoundField DataField="AssessmentFormLayout" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formLayout hiddenColumn" ItemStyle-CssClass="formLayout hiddenColumn" />
                                            <asp:BoundField DataField="AssessmentFormValue" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formValue hiddenColumn" ItemStyle-CssClass="formValue hiddenColumn" />
                                            <asp:BoundField DataField="IsInitialAssessment" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="isInitialAssessment hiddenColumn" ItemStyle-CssClass="isInitialAssessment hiddenColumn"/>
                                            <asp:BoundField DataField="cfIsFallRisk" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="cfIsFallRisk hiddenColumn" ItemStyle-CssClass="cfIsFallRisk hiddenColumn" />
                                            <asp:BoundField DataField="FallRiskScore" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="fallRiskScore hiddenColumn" ItemStyle-CssClass="fallRiskScore hiddenColumn" />
                                            <asp:BoundField DataField="GCFallRiskScoreType" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="gcFallRiskScoreType hiddenColumn" ItemStyle-CssClass="gcFallRiskScoreType hiddenColumn" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <div>
                                                        <a class="lnkView" recordid="<%#:Eval("AssessmentID") %>"
                                                            visitID="<%#:Eval("VisitID") %>" assessmentType="<%#:Eval("GCFallRiskAssessmentType") %>" assessmentDate="<%#:Eval("cfAssessmentDatePickerFormat") %>" assessmentTime="<%#:Eval("AssessmentTime") %>" assessmentLayout="<%#:Eval("AssessmentFormLayout") %>" assessmentValue="<%#:Eval("AssessmentFormValue") %>"
                                                            paramedicName="<%#:Eval("ParamedicName") %>" isInitialAssessment="<%#:Eval("IsInitialAssessment") %>" isFallRisk="<%#:Eval("cfIsFallRisk") %>" riskScoreType="<%#:Eval("GCFallRiskScoreType") %>">View</a>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada data pengkajian resiko jatuh untuk pasien ini") %>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingDt">
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
