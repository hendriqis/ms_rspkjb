<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master" ValidateRequest="false" 
    AutoEventWireup="true" CodeBehind="PatientEducationForm.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientEducationForm" %>

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
            cbpView.PerformCallback('printout');
        });
        $(function () {
            $('#<%=grdFormList.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdFormList.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnGCAssessmentType.ClientID %>').val($(this).find('.keyField').html());
                cbpView.PerformCallback('refresh');
                $('#<%=hdnID.ClientID %>').val('')
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
                $('#<%=hdnAssessmentLayout.ClientID %>').val($(this).find('.formLayout').html());
                $('#<%=hdnAssessmentValues.ClientID %>').val($(this).find('.formValue').html());
                $('#<%=hdnIsInitialAssessment.ClientID %>').val($(this).find('.isInitialAssessment').html());
            });
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

        $('.lnkCopy a').live('click', function () {
            var formType = $('#<%=hdnGCAssessmentType.ClientID %>').val();
            var visitID = $(this).closest('tr').find('.visitID').html();
            var id = $(this).closest('tr').find('.keyField').html();
            var layout = $('#<%=hdnAssessmentLayout.ClientID %>').val();
            var values = $('#<%=hdnAssessmentValues.ClientID %>').val();
            var message = "Lakukan copy pengkajian pasien ?";
            displayConfirmationMessageBox('COPY PENGKAJIAN PASIEN :', message, function (result) {
                if (result) {
                    var url = ResolveUrl("~/libs/Controls/EMR/Nursing/CopyNurseAssessmentFormEntry.ascx");
                    var param = formType+ '|' + visitID + "|" + id + '|' + layout + '|' + values;
                    openUserControlPopup(url, param, 'Pengkajian Pasien', 800, 600);
                }
            });
        });

        function onBeforeBasePatientPageListEdit() {
            if (($('#<%=hdnCurrentParamedicID.ClientID %>').val() == $('#<%=hdnParamedicID.ClientID %>').val())) {
                return true;
            }
            if ($('#<%=hdnID.ClientID %>').val() == '') {
                displayErrorMessageBox('EDIT', 'Maaf, Pilih Data Pengkajian terlebih dahulu!');
                return false
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
            if ($('#<%=hdnID.ClientID %>').val() == '') {
                displayErrorMessageBox('EDIT', 'Maaf, Pilih Data Pengkajian terlebih dahulu!');
                return false
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
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentDate" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentTime" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicName" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentLayout" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentValues" value="" />
    <input type="hidden" runat="server" id="hdnIsInitialAssessment" value="" />
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
                                            <asp:BoundField DataField="cfAssessmentDatePickerFormat" HeaderText = "Values" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentDate hiddenColumn" ItemStyle-CssClass="assessmentDate hiddenColumn" />
                                            <asp:HyperLinkField HeaderText=" " Text="View" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkView" HeaderStyle-Width="60px" />
                                            <asp:HyperLinkField HeaderText=" " Text="Copy" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkCopy" HeaderStyle-Width="60px" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada data pengkajian populasi khusus untuk pasien ini") %>
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
