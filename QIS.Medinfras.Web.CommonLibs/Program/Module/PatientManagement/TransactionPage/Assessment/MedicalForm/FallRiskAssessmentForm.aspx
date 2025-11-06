<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="FallRiskAssessmentForm.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.FallRiskAssessmentForm" %>

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
    <li id="btnPrint" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><div>
            <%=GetLabel("Print")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_vitalsignlist">
        $('#<%=btnPrint.ClientID %>').click(function () {
            cbpPrint.PerformCallback('printout');
        });
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
                $('#<%=hdnAssessmentID.ClientID %>').val($(this).find('.assessmentID').html());
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


            var param = formType + '|' + id + '|' + date + '|' + time + '|' + ppa + '|' + isInitialAssessment + '|' + layout + '|' + values + '|' + isFallRisk + '|' + riskScoreType + "|" + visitID + "|" + patientInfo;
            var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/ViewFallRiskAssessmentCtl.ascx");
            openUserControlPopup(url, param, $('#<%=hdnPopupTitle.ClientID %>').val(), 800, 600);
        });

        $('.lnkCopy').live('click', function () {
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

            var param = formType + '|' + id + '|' + date + '|' + time + '|' + ppa + '|' + isInitialAssessment + '|' + layout + '|' + values + '|' + isFallRisk + '|' + riskScoreType + "|" + visitID  + '|' + patientInfo;
            var message = "Lakukan copy pengkajian pasien ?";
            displayConfirmationMessageBox('COPY PENGKAJIAN PASIEN :', message, function (result) {
                if (result) {
                    var url = ResolveUrl("~/libs/Controls/EMR/Nursing/CopyFallRiskAssessmentFormEntry.ascx");
                     openUserControlPopup(url, param, $('#<%=hdnPopupTitle.ClientID %>').val(), 800, 600);
                }
            });
        });

        function onBeforeBasePatientPageListEdit() {
            if (($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val())) {
                return true;
            }
            else {
                displayErrorMessageBox('EDIT', 'Maaf, Perubahan Pengkajian hanya bisa dilakukan oleh user yang mengkaji.');
                return false;
            }
        }

        function onBeforeBasePatientPageListDelete() {
            if ($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val()) {
                return true;
            }
            else {
                displayErrorMessageBox('EDIT', 'Maaf, Pengkajian hanya bisa dihapus oleh user yang mengkaji.');
                return false;
            }
        }

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
            else if (param[0] == 'printout') {

                var FileString = $('#<%:hdnFileString.ClientID %>').val();
                if (FileString != "") {
                    //window.open("data:application/pdf;base64, " + FileString);
                    var random = Math.random().toString(36).substring(7);
                    var css = '<%=ResolveUrl("~/Libs/Styles/PrintLayout/paper.css")%>' + "?" + random;
                    var newWin = open('url', 'windowName', 'scrollbars=1,resizable=1,width=1000,height=580,left=0,top=0');
                    newWin.document.write('<html><head><title>Resiko Jatuh</title><link rel="stylesheet" type="text/css" href="' + css + '"> </head><style type="text/css" media="print"> .noPrint{display:none;} </style>');
                    var html = '<style>@page { size: A4 landscape }</style>';
                    html = '<body class="A4 landscape"> <div style="margin-left:20px;" class="noPrint"><input type="button" value="Print Halaman" onclick="javascript:window.print()" /></div>' + FileString + ' </body>';
                    newWin.document.write(html);
                    newWin.document.close();
                    newWin.focus();
                    newWin.print();
                }
                $('#<%:hdnFileString.ClientID %>').val('');
                cbpView.PerformCallback('refresh');
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onCbpPrintEndCallback(s) {
            $('#containerImgLoadingView').hide();
            var param = s.cpResult.split('|');
            var FileString = $('#<%:hdnFileString.ClientID %>').val();
            if (FileString != "") {
                //window.open("data:application/pdf;base64, " + FileString);
                var random = Math.random().toString(36).substring(7);
                var css = '<%=ResolveUrl("~/Libs/Styles/PrintLayout/paper.css")%>' + "?" + random;
                var newWin = open('url', 'windowName', 'scrollbars=1,resizable=1,width=1000,height=580,left=0,top=0');
                newWin.document.write('<html><head><title>Resiko Jatuh</title><link rel="stylesheet" type="text/css" href="' + css + '"> </head><style type="text/css" media="print"> .noPrint{display:none;} </style>');
                var html = '<style>@page { size: A4 landscape }</style>';
                html = '<body class="A4 landscape"> <div style="margin-left:20px;" class="noPrint"><input type="button" value="Print Halaman" onclick="javascript:window.print()" /></div>' + FileString + ' </body>';
                newWin.document.write(html);
                newWin.document.close();
                newWin.focus();
                newWin.print();
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'patientPhoto') {
                var param = $('#<%:hdnMRN.ClientID %>').val();
                return param;
            }
        }
    </script> 
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" runat="server" id="hdnGCAssessmentType" value="" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" value="" id="hdnPageMedicalNo" runat="server" />
    <input type="hidden" value="" id="hdnPagePatientName" runat="server" />
    <input type="hidden" value="" id="hdnPagePatientDOB" runat="server" />
    <input type="hidden" value="" id="hdnPageRegistrationNo" runat="server" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentID" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentDate" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentTime" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicName" value="" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentLayout" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentValues" value="" />
    <input type="hidden" runat="server" id="hdnIsInitialAssessment" value="" />
    <input type="hidden" runat="server" id="hdnIsFallRisk" value="" />
    <input type="hidden" runat="server" id="hdnFallRiskScore" value="" />
    <input type="hidden" runat="server" id="hdnGCFallRiskScoreType" value="" />
    <input type="hidden" runat="server" id="hdnPopupTitle" value="" />
    <div style="position: relative;">
        <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
            <colgroup>
                <col width="30%" />
                <col />
            </colgroup>
            <tr>
                <td style="vertical-align: top">
                    <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpFormList" runat="server" Width="100%" ClientInstanceName="cbpFormList"
                            ShowLoadingPanel="false" OnCallback="cbpFormList_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerHdImgLoadingView').show(); }"
                                EndCallback="function(s,e){ onCbpFormListEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                    <asp:Panel runat="server" ID="panFormList" CssClass="pnlContainerGridPatientPage">
                                        <asp:GridView ID="grdFormList" runat="server" CssClass="grdSelected grdPatientPage"
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="StandardCodeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:BoundField DataField="StandardCodeName" HeaderText="Nama Metode" HeaderStyle-CssClass="gridColumnText"
                                                    ItemStyle-CssClass="gridColumnText" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Tidak ada template form pengkajian resiko jatuh yang bisa digunakan")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="imgLoadingGrdView" id="containerHdImgLoadingView">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                        <div class="containerPaging">
                            <div class="wrapperPaging">
                                <div id="pagingHd">
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
                <td style="vertical-align: top">
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
                                            <asp:BoundField DataField="AssessmentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="cfAssessmentDate" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="AssessmentTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentTime"
                                                ItemStyle-CssClass="assessmentTime" />
                                            <asp:BoundField DataField="ParamedicID" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicID hiddenColumn"
                                                ItemStyle-CssClass="paramedicID hiddenColumn" />
                                            <asp:BoundField DataField="ParamedicName" HeaderText="Dikaji Oleh" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="paramedicName" />
                                            <asp:BoundField DataField="FallRiskScore" HeaderText="Skor" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="FallRiskScoreType" HeaderText="Kesimpulan" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="AssessmentID" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="assessmentID hiddenColumn"
                                                ItemStyle-CssClass="assessmentID hiddenColumn" />
                                            <asp:BoundField DataField="cfAssessmentDatePickerFormat" HeaderText="Values" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentDate hiddenColumn"
                                                ItemStyle-CssClass="assessmentDate hiddenColumn" />
                                            <asp:BoundField DataField="GCFallRiskAssessmentType" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-CssClass="formType hiddenColumn" ItemStyle-CssClass="formType hiddenColumn" />
                                            <asp:BoundField DataField="AssessmentFormLayout" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-CssClass="formLayout hiddenColumn" ItemStyle-CssClass="formLayout hiddenColumn" />
                                            <asp:BoundField DataField="AssessmentFormValue" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-CssClass="formValue hiddenColumn" ItemStyle-CssClass="formValue hiddenColumn" />
                                            <asp:BoundField DataField="IsInitialAssessment" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="isInitialAssessment hiddenColumn"
                                                ItemStyle-CssClass="isInitialAssessment hiddenColumn" />
                                            <asp:BoundField DataField="cfIsFallRisk" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-CssClass="cfIsFallRisk hiddenColumn" ItemStyle-CssClass="cfIsFallRisk hiddenColumn" />
                                            <asp:BoundField DataField="FallRiskScore" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-CssClass="fallRiskScore hiddenColumn" ItemStyle-CssClass="fallRiskScore hiddenColumn" />
                                            <asp:BoundField DataField="GCFallRiskScoreType" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-CssClass="gcFallRiskScoreType hiddenColumn" ItemStyle-CssClass="gcFallRiskScoreType hiddenColumn" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <div>
                                                        <a class="lnkView" recordid="<%#:Eval("AssessmentID") %>"
                                                            visitID="<%#:Eval("VisitID") %>" assessmentType="<%#:Eval("GCFallRiskAssessmentType") %>" assessmentDate="<%#:Eval("cfAssessmentDatePickerFormat") %>" assessmentTime="<%#:Eval("AssessmentTime") %>" assessmentLayout="<%#:Eval("AssessmentFormLayout") %>" assessmentValue="<%#:Eval("AssessmentFormValue") %>"
                                                            paramedicName="<%#:Eval("ParamedicName") %>" isInitialAssessment="<%#:Eval("IsInitialAssessment") %>" isFallRisk="<%#:Eval("cfIsFallRisk") %>" riskScoreType="<%#:Eval("GCFallRiskScoreType") %>">View</a>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <div>
                                                        <a class="lnkCopy" recordid="<%#:Eval("AssessmentID") %>"
                                                            visitID="<%#:Eval("VisitID") %>" assessmentType="<%#:Eval("GCFallRiskAssessmentType") %>" assessmentDate="<%#:Eval("cfAssessmentDatePickerFormat") %>" assessmentTime="<%#:Eval("AssessmentTime") %>" assessmentLayout="<%#:Eval("AssessmentFormLayout") %>" assessmentValue="<%#:Eval("AssessmentFormValue") %>"
                                                            paramedicName="<%#:Eval("ParamedicName") %>" isInitialAssessment="<%#:Eval("IsInitialAssessment") %>" isFallRisk="<%#:Eval("cfIsFallRisk") %>" riskScoreType="<%#:Eval("GCFallRiskScoreType") %>">Copy</a>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada data pengkajian untuk pasien ini") %>
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
    <dxcp:ASPxCallbackPanel ID="cbpPrint" runat="server" Width="0%" ClientInstanceName="cbpPrint"
        ShowLoadingPanel="false" OnCallback="cbpPrint_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
            EndCallback="function(s,e){ onCbpPrintEndCallback(s); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">
                <input type="hidden" value="" id="hdnFileString" runat="server" />
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
</asp:Content>
