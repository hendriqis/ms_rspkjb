<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true"
    CodeBehind="PainAssessmentFormList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PainAssessmentFormHistoryList1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript">
        $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnGCAssessmentType.ClientID %>').val($(this).find('.keyField').html());
            cbpViewDt.PerformCallback('refresh');
        });

        $('#<%=grdViewDt.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdViewDt.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            $('#<%=hdnAssessmentDate.ClientID %>').val($(this).find('.assessmentDate').html());
            $('#<%=hdnAssessmentTime.ClientID %>').val($(this).find('.assessmentTime').html());
            $('#<%=hdnParamedicIDCBCtl.ClientID %>').val($(this).find('.paramedicID').html());
            $('#<%=hdnParamedicName.ClientID %>').val($(this).find('.paramedicName').html());
            $('#<%=hdnAssessmentValues.ClientID %>').val($(this).find('.formValue').html());
            $('#<%=hdnIsInitialAssessment.ClientID %>').val($(this).find('.cfIsInitialAssessment').html());
            $('#<%=hdnIsPain.ClientID %>').val($(this).find('.cfIsPain').html());
            $('#<%=hdnPainScore.ClientID %>').val($(this).find('.PainScore').html());
            $('#<%=hdnGCPainScoreType.ClientID %>').val($(this).find('.gcPainScoreType').html());
            $('#<%=hdnGCProvoking.ClientID %>').val($(this).find('.gcProvoking').html());
            $('#<%=hdnProvoking.ClientID %>').val($(this).find('.provoking').html());
            $('#<%=hdnGCQuality.ClientID %>').val($(this).find('.gcQuality').html());
            $('#<%=hdnQuality.ClientID %>').val($(this).find('.quality').html());
            $('#<%=hdnGCRegio.ClientID %>').val($(this).find('.gcRegio').html());
            $('#<%=hdnRegio.ClientID %>').val($(this).find('.regio').html());
            $('#<%=hdnGCTime.ClientID %>').val($(this).find('.gcTime').html());
            $('#<%=hdnTime.ClientID %>').val($(this).find('.time').html());
            $('#<%=hdnCfProvoking.ClientID %>').val($(this).find('.cfProvoking').html());
            $('#<%=hdnCfQuality.ClientID %>').val($(this).find('.cfQuality').html());
            $('#<%=hdnCfRegio.ClientID %>').val($(this).find('.cfRegio').html());
            $('#<%=hdnCfTime.ClientID %>').val($(this).find('.cfTime').html());
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
            var isPain = $(this).attr("isPain");
            var painScoreType = $(this).attr("painScoreType");

            var gcProvoking = $(this).attr("gcProvoking");
            var provoking = $(this).attr("provoking");
            var gcQuality = $(this).attr("gcQuality");
            var quality = $(this).attr("quality");
            var gcRegio = $(this).attr("gcRegio");
            var regio = $(this).attr("regio");
            var gcTime = $(this).attr("gcTime");
            var course_timing = $(this).attr("time");

            var cfProvoking = $(this).attr("cfProvoking");
            var cfQuality = $(this).attr("cfQuality");
            var cfRegio = $(this).attr("cfRegio");
            var cfTime = $(this).attr("cfTime");

            var medicalNo = $('#<%=hdnPageMedicalNo.ClientID %>').val();
            var patientName = $('#<%=hdnPagePatientName.ClientID %>').val();
            var patientDOB = $('#<%=hdnPagePatientDOB.ClientID %>').val();
            var registrationNo = $('#<%=hdnPageRegistrationNo.ClientID %>').val();
            var patientInfo = medicalNo + "|" + patientName + "|" + patientDOB + "|" + registrationNo;

            var param = formType + '~' + id + '~' + date + '~' + time + '~' + ppa + '~' + isInitialAssessment + '~' + layout + '~' + values + '~' + isPain + '~' + painScoreType + '~' + gcProvoking + '~' + provoking + '~' + gcQuality + '~' + quality + '~' + gcRegio + '~' + regio + '~' + gcTime + '~' + course_timing + '~' + cfProvoking + '~' + cfQuality + '~' + cfRegio + '~' + cfTime + "~" + patientInfo;
            var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/ViewPainAssessmentCtl.ascx");
            openUserControlPopup(url, param, 'Pengkajian Pasien', 800, 600);
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
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" runat="server" id="hdnGCAssessmentType" value="" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitIDCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentIDCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" />
    <input type="hidden" value="" id="hdnMRNCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnPageMedicalNo" runat="server" />
    <input type="hidden" value="" id="hdnPagePatientName" runat="server" />
    <input type="hidden" value="" id="hdnPagePatientDOB" runat="server" />
    <input type="hidden" value="" id="hdnPageRegistrationNo" runat="server" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicIDCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentDate" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentTime" value="" />
    <input type="hidden" runat="server" id="hdnParamedicIDCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnParamedicName" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentLayout" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentValues" value="" />
    <input type="hidden" runat="server" id="hdnIsInitialAssessment" value="" />
    <input type="hidden" runat="server" id="hdnIsPain" value="" />
    <input type="hidden" runat="server" id="hdnPainScore" value="" />
    <input type="hidden" runat="server" id="hdnGCPainScoreType" value="" />
    <input type="hidden" runat="server" id="hdnGCProvoking" value="" />
    <input type="hidden" runat="server" id="hdnProvoking" value="" />
    <input type="hidden" runat="server" id="hdnGCQuality" value="" />
    <input type="hidden" runat="server" id="hdnQuality" value="" />
    <input type="hidden" runat="server" id="hdnGCRegio" value="" />
    <input type="hidden" runat="server" id="hdnRegio" value="" />
    <input type="hidden" runat="server" id="hdnGCTime" value="" />
    <input type="hidden" runat="server" id="hdnTime" value="" />
    <input type="hidden" runat="server" id="hdnCfProvoking" value="" />
    <input type="hidden" runat="server" id="hdnCfQuality" value="" />
    <input type="hidden" runat="server" id="hdnCfRegio" value="" />
    <input type="hidden" runat="server" id="hdnCfTime" value="" />    
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
                                            <asp:BoundField DataField="cfAssessmentDate" HeaderText = "Tanggal" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center"/>
                                            <asp:BoundField DataField="AssessmentTime" HeaderText = "Jam" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentTime" ItemStyle-CssClass="assessmentTime" />
                                            <asp:BoundField DataField="ParamedicID" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicID hiddenColumn" ItemStyle-CssClass="paramedicID hiddenColumn"/>
                                            <asp:BoundField DataField="ParamedicName" HeaderText = "Dikaji Oleh" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="paramedicName" HeaderStyle-Width="200px"/>
                                            <asp:BoundField DataField="PainScore" HeaderText = "Skor" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />                                           
                                            <asp:BoundField DataField="PainScoreType" HeaderText = "Kesimpulan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
                                            <asp:BoundField DataField="cfProvoking" HeaderText = "Penyebab" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" ItemStyle-CssClass="cfProvoking"  />
                                            <asp:BoundField DataField="cfQuality" HeaderText = "Kualitas" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="120px"  ItemStyle-CssClass="cfQuality"   />
                                            <asp:BoundField DataField="cfRegio" HeaderText = "Regio/Lokasi" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" ItemStyle-CssClass="cfRegio"  />                      
                                            <asp:BoundField DataField="cfTime" HeaderText = "Waktu" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" ItemStyle-CssClass="cfTime" />                                            
                                            <asp:BoundField DataField="cfAssessmentDatePickerFormat" HeaderText = "Values" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentDate hiddenColumn" ItemStyle-CssClass="assessmentDate hiddenColumn" />
                                            <asp:BoundField DataField="GCPainAssessmentType" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formType hiddenColumn" ItemStyle-CssClass="formType hiddenColumn" />
                                            <asp:BoundField DataField="GCProvoking" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="gcProvoking hiddenColumn" ItemStyle-CssClass="gcProvoking hiddenColumn" />
                                            <asp:BoundField DataField="Provoking" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="provoking hiddenColumn" ItemStyle-CssClass="provoking hiddenColumn" />
                                            <asp:BoundField DataField="GCQuality" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="gcQuality hiddenColumn" ItemStyle-CssClass="gcQuality hiddenColumn" />
                                            <asp:BoundField DataField="Quality" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="quality hiddenColumn" ItemStyle-CssClass="quality hiddenColumn" />
                                            <asp:BoundField DataField="GCRegio" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="gcRegio hiddenColumn" ItemStyle-CssClass="gcRegio hiddenColumn" />
                                            <asp:BoundField DataField="Regio" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="regio hiddenColumn" ItemStyle-CssClass="regio hiddenColumn" />
                                            <asp:BoundField DataField="GCTime" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="gcRegio hiddenColumn" ItemStyle-CssClass="gcTime hiddenColumn" />
                                            <asp:BoundField DataField="Time" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="time hiddenColumn" ItemStyle-CssClass="time hiddenColumn" />
                                            <asp:BoundField DataField="AssessmentFormLayout" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formLayout hiddenColumn" ItemStyle-CssClass="formLayout hiddenColumn" />
                                            <asp:BoundField DataField="AssessmentFormValue" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formValue hiddenColumn" ItemStyle-CssClass="formValue hiddenColumn" />
                                            <asp:BoundField DataField="IsInitialAssessment" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="isInitialAssessment hiddenColumn" ItemStyle-CssClass="isInitialAssessment hiddenColumn"/>
                                            <asp:BoundField DataField="cfIsInitialAssessment" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="cfIsInitialAssessment hiddenColumn" ItemStyle-CssClass="cfIsInitialAssessment hiddenColumn"/>
                                            <asp:BoundField DataField="cfIsPain" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="cfIsPain hiddenColumn" ItemStyle-CssClass="cfIsPain hiddenColumn" />
                                            <asp:BoundField DataField="PainScore" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="PainScore hiddenColumn" ItemStyle-CssClass="PainScore hiddenColumn" />
                                            <asp:BoundField DataField="GCPainScoreType" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="gcPainScoreType hiddenColumn" ItemStyle-CssClass="gcPainScoreType hiddenColumn" />
                                            <asp:BoundField DataField="cfAssessmentDatePickerFormat" HeaderText="Values" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentDate hiddenColumn"
                                                ItemStyle-CssClass="assessmentDate hiddenColumn" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <div>
                                                        <a class="lnkView" recordid="<%#:Eval("AssessmentID") %>"
                                                            visitID="<%#:Eval("VisitID") %>" assessmentType="<%#:Eval("GCPainAssessmentType") %>" assessmentDate="<%#:Eval("cfAssessmentDatePickerFormat") %>" assessmentTime="<%#:Eval("AssessmentTime") %>" assessmentLayout="<%#:Eval("AssessmentFormLayout") %>" assessmentValue="<%#:Eval("AssessmentFormValue") %>"
                                                            paramedicName="<%#:Eval("ParamedicName") %>" isInitialAssessment="<%#:Eval("IsInitialAssessment") %>" isPain="<%#:Eval("cfIsPain") %>" painScoreType="<%#:Eval("GCPainScoreType") %>"
                                                            gcProvoking="<%#:Eval("GCProvoking") %>" provoking="<%#:Eval("Provoking") %>" gcQuality="<%#:Eval("GCQuality") %>" quality="<%#:Eval("Quality") %>" gcRegio="<%#:Eval("GCRegio") %>" regio="<%#:Eval("Regio") %>" gcTime="<%#:Eval("GCTime") %>" time="<%#:Eval("Time") %>" 
                                                            cfProvoking="<%#:Eval("cfProvoking") %>" cfQuality="<%#:Eval("cfQuality") %>" cfRegio="<%#:Eval("cfRegio") %>" cfTime = "<%#:Eval("cfTime") %>">View</a>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada data pengkajian nyeri untuk pasien ini") %>
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
