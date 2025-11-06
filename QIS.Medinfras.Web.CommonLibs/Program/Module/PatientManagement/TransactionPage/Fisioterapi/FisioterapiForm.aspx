<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="FisioterapiForm.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.FisioterapiForm" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div>
        <table cellpadding="0" cellspacing="0" style="width:100%">
            <colgroup>
                <col style="width: 100px" />
                <col style="width: 145px" />
                <col style="width: 10px" />
                <col style="width: 145px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal" style="font-weight:bold">
                        <%=GetLabel("Tanggal ")%></label>
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
                <td>
                    <div class="menuTitle">
                        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_vitalsignlist">
        

        $(function () {

         $('#<%=grdFormList.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdFormList.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnGCAssessmentType.ClientID %>').val($(this).find('.keyField').html());
                cbpView.PerformCallback('refresh');
            });
            $('#<%=grdFormList.ClientID %> tr:eq(1)').click();


            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());

                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnAssessmentDate.ClientID %>').val($(this).find('.assessmentDate').html());
                $('#<%=hdnAssessmentTime.ClientID %>').val($(this).find('.assessmentTime').html());
                $('#<%=hdnParamedicID.ClientID %>').val($(this).find('.paramedicID').html());
                $('#<%=hdnAssessmentValues.ClientID %>').val($(this).find('.formValue').html());
                $('#<%=hdnIsFallRisk.ClientID %>').val($(this).find('.cfIsFallRisk').html());
                $('#<%=hdnFallRiskScore.ClientID %>').val($(this).find('.fallRiskScore').html());
                $('#<%=hdnGCFallRiskScoreType.ClientID %>').val($(this).find('.gcFallRiskScoreType').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setDatePicker('<%=txtFromDate.ClientID %>');
            $('#<%=txtFromDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtToDate.ClientID %>');
            $('#<%=txtToDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtFromDate.ClientID %>').change(function (evt) {
                cbpView.PerformCallback('refresh');
            });

            $('#<%=txtToDate.ClientID %>').change(function (evt) {
                cbpView.PerformCallback('refresh');
            });
        });

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSavePatientPhoto() {
            var MRN = $('#<%:hdnMRN.ClientID %>').val();
            var filterExpression = 'MRN = ' + MRN;
            hideLoadingPanel();
            pcRightPanelContent.Hide();
        }

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

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'patientPhoto') {
                var param = $('#<%:hdnMRN.ClientID %>').val();
                return param;
            }
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" runat="server" id="hdnGCAssessmentType" value="" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" runat="server" id="hdnAssessmentDate" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentTime" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnFallRiskScore" value="" />
    <input type="hidden" runat="server" id="hdnGCFallRiskScoreType" value="" />
    <input type="hidden" runat="server" id="hdnIsFallRisk" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentValues" value="" />
    <div style="position: relative;">
        <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
            <colgroup>
                <col width="30%" />
                <col />
            </colgroup>
            <tr>
                <td style="vertical-align:top">
                    <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpFormList" runat="server" Width="100%" ClientInstanceName="cbpFormList"
                            ShowLoadingPanel="false" OnCallback="cbpFormList_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerHdImgLoadingView').show(); }"
                                EndCallback="function(s,e){ onCbpFormListEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                    <asp:Panel runat="server" ID="panFormList" CssClass="pnlContainerGridPatientPage">
                                        <asp:GridView ID="grdFormList" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" 
                                            EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="StandardCodeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:BoundField DataField="StandardCodeName" HeaderText="Nama Metode" HeaderStyle-CssClass="gridColumnText" ItemStyle-CssClass="gridColumnText" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Tidak ada template form pengkajian resiko jatuh yang bisa digunakan")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>    
                        <div class="imgLoadingGrdView" id="containerHdImgLoadingView" >
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                        <div class="containerPaging">
                            <div class="wrapperPaging">
                                <div id="pagingHd"></div>
                            </div>
                        </div> 
                    </div>
                </td>
                <td style="vertical-align:top">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div>
                                                        Daftar Pemeriksaan Fisik</div>
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
                                                        <asp:Repeater ID="rptVitalSignDt" runat="server">
                                                            <ItemTemplate>
                                                                <div style="padding-left: 20px; float: left; width: 300px;">
                                                                    <strong>
                                                                        <div style="width: 110px; float: left; color: blue" class="labelColumn">
                                                                            <%#: DataBinder.Eval(Container.DataItem, "VitalSignLabel") %></div>
                                                                        <div style="width: 20px; float: left;">
                                                                            :</div>
                                                                    </strong>
                                                                    <div style="float: left;">
                                                                        <%#: DataBinder.Eval(Container.DataItem, "DisplayVitalSignValue") %></div>
                                                                </div>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <br style="clear: both" />
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada data pemeriksaaan Tanda Vital untuk pasien ini") %>
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
</asp:Content>
