<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master"
    AutoEventWireup="true" CodeBehind="PainAssessmentForm.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PainAssessmentForm" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
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
                $('#<%=hdnAssessmentDate.ClientID %>').val($(this).find('.assessmentDate').html());
                $('#<%=hdnAssessmentTime.ClientID %>').val($(this).find('.assessmentTime').html());
                $('#<%=hdnParamedicID.ClientID %>').val($(this).find('.paramedicID').html());
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

            var param = formType + '~' + id + '~' + date + '~' + time + '~' + ppa + '~' + isInitialAssessment + '~' + layout.replace("~", "Ѽ") + '~' + values + '~' + isPain + '~' + painScoreType + '~' + gcProvoking + '~' + provoking + '~' + gcQuality + '~' + quality + '~' + gcRegio + '~' + regio + '~' + gcTime + '~' + course_timing + '~' + cfProvoking + '~' + cfQuality + '~' + cfRegio + '~' + cfTime + "~" + patientInfo;
            var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/ViewPainAssessmentCtl.ascx");
            openUserControlPopup(url, param, 'Pengkajian Pasien', 800, 600);
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
            var message = "Lakukan copy pengkajian pasien ?";
            displayConfirmationMessageBox('COPY PENGKAJIAN PASIEN :', message, function (result) {
                if (result) {
                    var url = ResolveUrl("~/libs/Controls/EMR/Nursing/CopyPainAssessmentFormEntry.ascx");
                    openUserControlPopup(url, param, 'Pengkajian Pasien', 800, 600);
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
                    newWin.document.write('<html><head><title>Skala Nyeri</title><link rel="stylesheet" type="text/css" href="' + css + '"> </head><style type="text/css" media="print"> .noPrint{display:none;} </style>');
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
    <input type="hidden" runat="server" id="hdnAssessmentDate" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentTime" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicName" value="" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
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
    <div style="position: relative;">
        <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
            <colgroup>
                <col width="25%" />
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
                              <input type="hidden" value="" id="hdnFileString" runat="server" />
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="AssessmentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="cfAssessmentDate" HeaderText = "Tanggal" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center"/>
                                            <asp:BoundField DataField="AssessmentTime" HeaderText = "Jam" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentTime" ItemStyle-CssClass="assessmentTime" />
                                            <asp:BoundField DataField="ParamedicID" HeaderText = "Values" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicID hiddenColumn" ItemStyle-CssClass="paramedicID hiddenColumn"/>
                                            <asp:BoundField DataField="ParamedicName" HeaderText = "Dikaji Oleh" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="paramedicName" HeaderStyle-Width="200px"/>
                                            <asp:BoundField DataField="cfProvoking" HeaderText = "Penyebab" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" ItemStyle-CssClass="cfProvoking"  />
                                            <asp:BoundField DataField="cfQuality" HeaderText = "Kualitas" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="120px"  ItemStyle-CssClass="cfQuality"   />
                                            <asp:BoundField DataField="cfRegio" HeaderText = "Regio/Lokasi" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" ItemStyle-CssClass="cfRegio"  />
                                            <asp:BoundField DataField="PainScore" HeaderText = "Skor" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cfTime" HeaderText = "Waktu" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" ItemStyle-CssClass="cfTime" />
                                            <asp:BoundField DataField="PainScoreType" HeaderText = "Kesimpulan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="120px" />
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
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <div>
                                                        <a class="lnkCopy" recordid="<%#:Eval("AssessmentID") %>"
                                                            visitID="<%#:Eval("VisitID") %>" assessmentType="<%#:Eval("GCPainAssessmentType") %>" assessmentDate="<%#:Eval("cfAssessmentDatePickerFormat") %>" assessmentTime="<%#:Eval("AssessmentTime") %>" assessmentLayout="<%#:Eval("AssessmentFormLayout") %>" assessmentValue="<%#:Eval("AssessmentFormValue") %>"
                                                            paramedicName="<%#:Eval("ParamedicName") %>" isInitialAssessment="<%#:Eval("IsInitialAssessment") %>" isPain="<%#:Eval("cfIsPain") %>" painScoreType="<%#:Eval("GCPainScoreType") %>"
                                                            gcProvoking="<%#:Eval("GCProvoking") %>" provoking="<%#:Eval("Provoking") %>" gcQuality="<%#:Eval("GCQuality") %>" quality="<%#:Eval("Quality") %>" gcRegio="<%#:Eval("GCRegio") %>" regio="<%#:Eval("Regio") %>" gcTime="<%#:Eval("GCTime") %>" time="<%#:Eval("Time") %>"
                                                            cfProvoking="<%#:Eval("cfProvoking") %>" cfQuality="<%#:Eval("cfQuality") %>" cfRegio="<%#:Eval("cfRegio") %>" cfTime = "<%#:Eval("cfTime") %>">Copy</a>
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
</asp:Content>
