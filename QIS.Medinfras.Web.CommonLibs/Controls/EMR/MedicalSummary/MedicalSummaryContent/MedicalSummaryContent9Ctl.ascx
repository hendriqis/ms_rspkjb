<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicalSummaryContent9Ctl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.MedicalSummaryContent9Ctl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_MedicalSummaryContent9Ctl">
    $('#<%=grdFormList.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
        $('#<%=grdFormList.ClientID %> tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $('#<%=hdnGCAssessmentType.ClientID %>').val($(this).find('.keyField').html());
        cbpFormList.PerformCallback('refresh');
    });

    $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
        $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
        $('#<%=hdnAssessmentDate.ClientID %>').val($(this).find('.assessmentDate').html());
        $('#<%=hdnAssessmentTime.ClientID %>').val($(this).find('.assessmentTime').html());
        $('#<%=hdnParamedicID.ClientID %>').val($(this).find('.paramedicID').html());
        $('#<%=hdnParamedicName.ClientID %>').val($(this).find('.paramedicName').html());
        $('#<%=hdnAssessmentLayout.ClientID %>').val($(this).find('.formLayout').html());
        $('#<%=hdnAssessmentValues.ClientID %>').val($(this).find('.formValue').html());
        $('#<%=hdnIsFallRisk.ClientID %>').val($(this).find('.cfIsFallRisk').html());
        $('#<%=hdnFallRiskScore.ClientID %>').val($(this).find('.fallRiskScore').html());
        $('#<%=hdnGCFallRiskScoreType.ClientID %>').val($(this).find('.gcFallRiskScoreType').html());
        $('#<%=hdnIsInitialAssessment.ClientID %>').val($(this).find('.isInitialAssessment').html());
    });
    $(function () {
        $('#<%=grdFormList.ClientID %> tr:eq(1)').click();
    });

    $('.lnkView a').live('click', function () {
        var formType = $('#<%=hdnGCAssessmentType.ClientID %>').val();
        var id = $(this).closest('tr').find('.keyField').html();
        var date = $('#<%=hdnAssessmentDate.ClientID %>').val();
        var time = $('#<%=hdnAssessmentTime.ClientID %>').val();
        var ppa = $('#<%=hdnParamedicName.ClientID %>').val();
        var isInitialAssessment = $('#<%=hdnIsInitialAssessment.ClientID %>').val();
        var layout = $('#<%=hdnAssessmentLayout.ClientID %>').val();
        var values = $('#<%=hdnAssessmentValues.ClientID %>').val();
        var formGroup = "1";

        var param = formType + '|' + id + '|' + date + '|' + time + '|' + ppa + '|' + isInitialAssessment + '|' + layout + '|' + values + '|' + formGroup;
        var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/ViewNursePatientAssessmentCtl.ascx");
        openUserControlPopup(url, param, 'Pengkajian Pasien', 800, 600);
    });

    //#region Paging Header
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingHd"), pageCount, function (page) {
            cbpFormList.PerformCallback('changepage|' + page);
        });
    });

    function onCbpFormListEndCallback(s) {
        $('#containerHdImgLoadingView').hide();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdFormList.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingHd"), pageCount, function (page) {
                cbpFormList.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdFormList.ClientID %> tr:eq(1)').click();
    }
    //#endregion

    //#region Paging
    function onCbpViewDtEndCallback(s) {
        $('#containerImgLoadingView').hide();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setPaging($("#paging"), pageCount, function (page) {
                cbpViewDt.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
    }
    //#endregion
</script>

<div class="w3-border divContent w3-animate-left">
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" runat="server" id="hdnGCAssessmentType" value="" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentDate" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentTime" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicName" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentLayout" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentValues" value="" />
    <input type="hidden" runat="server" id="hdnIsInitialAssessment" value="" />
    <input type="hidden" runat="server" id="hdnIsFallRisk" value="" />
    <input type="hidden" runat="server" id="hdnFallRiskScore" value="" />
    <input type="hidden" runat="server" id="hdnGCFallRiskScoreType" value="" />
    <table style="width:100%">
        <colgroup>
            <col style="width:30%"/>
            <col style="width:70%"/>
        </colgroup>
        <tr>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpFormList" runat="server" Width="100%" ClientInstanceName="cbpFormList"
                        ShowLoadingPanel="false" OnCallback="cbpFormList_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdFormList" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" 
                                        EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="StandardCodeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="StandardCodeName" HeaderText="Jenis Pengkajian" HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada template form pengkajian yang bisa digunakan")%>
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
                            <div id="pagingHd"></div>
                        </div>
                    </div> 
                </div>          
            </td>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="AssessmentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn visitID" />
                                            <asp:BoundField DataField="cfAssessmentDate" HeaderText = "Tanggal" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center"/>
                                            <asp:BoundField DataField="AssessmentTime" HeaderText = "Jam" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentTime" ItemStyle-CssClass="assessmentTime" />
                                            <asp:BoundField DataField="ParamedicID" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicID hiddenColumn" ItemStyle-CssClass="paramedicID hiddenColumn"/>
                                            <asp:BoundField DataField="GCAssessmentType" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formType hiddenColumn" ItemStyle-CssClass="formType hiddenColumn" />
                                            <asp:BoundField DataField="AssessmentFormLayout" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formLayout hiddenColumn" ItemStyle-CssClass="formLayout hiddenColumn" />
                                            <asp:BoundField DataField="AssessmentFormValue" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="formValue hiddenColumn" ItemStyle-CssClass="formValue hiddenColumn" />
                                            <asp:BoundField DataField="IsInitialAssessment" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="isInitialAssessment hiddenColumn" ItemStyle-CssClass="isInitialAssessment hiddenColumn"/>
                                            <asp:BoundField DataField="ParamedicName" HeaderText = "Dikaji Oleh" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicName" ItemStyle-CssClass="paramedicName" />
                                            <asp:BoundField DataField="cfIsInitialAssessment" HeaderText = "Kajian Awal" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="70px" />
                                            <asp:BoundField DataField="cfAssessmentDatePickerFormat" HeaderText = "Values" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentDate hiddenColumn" ItemStyle-CssClass="assessmentDate hiddenColumn" />
                                            <asp:HyperLinkField HeaderText=" " Text="View" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkView" HeaderStyle-Width="60px" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada data pengkajian populasi khusus untuk pasien ini") %>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <div class="imgLoadingGrdView" id="Div1">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging">
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>

