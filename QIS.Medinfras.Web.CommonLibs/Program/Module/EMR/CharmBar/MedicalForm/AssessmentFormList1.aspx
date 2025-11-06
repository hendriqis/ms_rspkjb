<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true"
    CodeBehind="AssessmentFormList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.MedicalAssessmentFormList1" %>

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
            $('#<%=hdnGCAssessmentTypeCBCtl.ClientID %>').val($(this).find('.keyField').html());
            $('#<%=hdnAssessmentDate.ClientID %>').val($(this).find('.assessmentDate').html());
            $('#<%=hdnAssessmentTime.ClientID %>').val($(this).find('.assessmentTime').html());
            $('#<%=hdnParamedicIDCBCtl.ClientID %>').val($(this).find('.paramedicID').html());
            $('#<%=hdnParamedicNameCBCtl.ClientID %>').val($(this).find('.paramedicName').html());
            $('#<%=hdnAssessmentLayoutCBCtl.ClientID %>').val($(this).find('.formLayout').html());
            $('#<%=hdnAssessmentValuesCBCtl.ClientID %>').val($(this).find('.formValue').html());
        });

        $('#<%=grdView1.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdView1.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnGCAssessmentTypeCBCtl1.ClientID %>').val($(this).find('.keyField').html());
            cbpViewDt1.PerformCallback('refresh');
        });

        $('#<%=grdViewDt1.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdViewDt1.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnGCAssessmentTypeCBCtl1.ClientID %>').val($(this).find('.keyField').html());
            $('#<%=hdnAssessmentDate1.ClientID %>').val($(this).find('.assessmentDate').html());
            $('#<%=hdnAssessmentTime1.ClientID %>').val($(this).find('.assessmentTime').html());
            $('#<%=hdnParamedicIDCBCtl1.ClientID %>').val($(this).find('.paramedicID').html());
            $('#<%=hdnParamedicNameCBCtl1.ClientID %>').val($(this).find('.paramedicName').html());
            $('#<%=hdnAssessmentLayoutCBCtl1.ClientID %>').val($(this).find('.formLayout').html());
            $('#<%=hdnAssessmentValuesCBCtl1.ClientID %>').val($(this).find('.formValue').html());
        });
        $(function () {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
            $('#<%=grdView1.ClientID %> tr:eq(1)').click();

            $('#<%=rblItemType.ClientID %> input').change(function () {
                cbpView.PerformCallback('refresh');
                cbpViewDt.PerformCallback('refresh');
            });

            $('#<%=rblItemType1.ClientID %> input').change(function () {
                cbpView1.PerformCallback('refresh');
                cbpViewDt1.PerformCallback('refresh');
            });

            //#region Detail Tab
            $('#ulResultDetail li').click(function () {
                $('#ulResultDetail li.selected').removeAttr('class');
                $('.containerOrderDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
            //#endregion
        });

        $('.lnkView').live('click', function () {
            var formType = $(this).attr("assessmentType");
            var id = $(this).attr('recordID');
            var date = $(this).attr('assessmentDate');
            var time = $(this).attr('assessmentTime');
            var ppa = $(this).attr("paramedicName");
            var isInitialAssessment = $(this).attr("isInitialAssessment");
            var isNeedVerified = $(this).attr("isNeedVerified");
            var layout = $(this).attr("assessmentLayout");
            var values = $(this).attr("assessmentValue");
            var formGroup = "1";
            var visitID = $(this).attr("visitID");

            var medicalNo = $('#<%=hdnPageMedicalNo.ClientID %>').val();
            var patientName = $('#<%=hdnPagePatientName.ClientID %>').val();
            var patientDOB = $('#<%=hdnPagePatientDOB.ClientID %>').val();
            var registrationNo = $('#<%=hdnPageRegistrationNo.ClientID %>').val();
            var patientInfo = medicalNo + "|" + patientName + "|" + patientDOB + "|" + registrationNo;

            var param = formType + '|' + id + '|' + date + '|' + time + '|' + ppa + '|' + isInitialAssessment + '|' + layout + '|' + values + '|' + formGroup + "|" + visitID + '|' + patientInfo + "|" + isNeedVerified;

            if (formType == 'X401^113' || formType == 'X401^003' || formType == 'X401^004' || formType == 'X401^013' || formType == 'X401^017') {
                patientInfo = medicalNo + "@" + patientName + "@" + patientDOB + "@" + registrationNo;
                param = formType + '@' + id + '@' + date + '@' + time + '@' + ppa + '@' + isInitialAssessment + '@' + layout + '@' + values + '@' + formGroup + "@" + visitID + '@' + patientInfo + "@" + isNeedVerified;
            }

            var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/ViewNursePatientAssessmentCtl.ascx");
            openUserControlPopup(url, param, 'Pengkajian Pasien', 800, 600);
        });

        $('.lnkView1').live('click', function () {
            var formType = $(this).attr("assessmentType");
            var id = $(this).attr('recordID');
            var date = $(this).attr('assessmentDate');
            var time = $(this).attr('assessmentTime');
            var ppa = $(this).attr("paramedicName");
            var isInitialAssessment = $(this).attr("isInitialAssessment");
            var layout = $(this).attr("assessmentLayout");
            var values = $(this).attr("assessmentValue");
            var formGroup = "0";
            var visitID = $(this).attr("visitID");

            var medicalNo = $('#<%=hdnPageMedicalNo.ClientID %>').val();
            var patientName = $('#<%=hdnPagePatientName.ClientID %>').val();
            var patientDOB = $('#<%=hdnPagePatientDOB.ClientID %>').val();
            var registrationNo = $('#<%=hdnPageRegistrationNo.ClientID %>').val();
            var patientInfo = medicalNo + "|" + patientName + "|" + patientDOB + "|" + registrationNo;

            var param = formType + '|' + id + '|' + date + '|' + time + '|' + ppa + '|' + isInitialAssessment + '|' + layout + '|' + values + '|' + formGroup + "|" + visitID + '|' + patientInfo + "|0";
            var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/ViewNursePatientAssessmentCtl.ascx");
            openUserControlPopup(url, param, 'Pengkajian Pasien', 800, 600);
        });

        function onRefreshControl(filterExpression) {
            cbpView.PerformCallback('refresh');
            cbpViewDt.PerformCallback('refresh');
        }

        function onRefreshControl1(filterExpression) {
            cbpView1.PerformCallback('refresh');
            cbpViewDt1.PerformCallback('refresh');
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

        function onCbpView1EndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView1.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    grdView1.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView1.ClientID %> tr:eq(1)').click();
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

        function onCbpViewDt1EndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewDt1.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt"), pageCount, function (page) {
                    cbpViewDt1.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt1.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" runat="server" id="hdnGCAssessmentTypeCBCtl" value="" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitIDCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentIDCBCtl" runat="server" />
    <input type="hidden" runat="server" id="hdnGCAssessmentTypeCBCtl1" value="" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitIDCBCtl1" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentIDCBCtl1" runat="server" />
    <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" />
    <input type="hidden" value="" id="hdnMRNCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnMRNCBCtl1" runat="server" />
    <input type="hidden" value="" id="hdnPageMedicalNo" runat="server" />
    <input type="hidden" value="" id="hdnPagePatientName" runat="server" />
    <input type="hidden" value="" id="hdnPagePatientDOB" runat="server" />
    <input type="hidden" value="" id="hdnPageRegistrationNo" runat="server" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentDate" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentDate1" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentTime" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentTime1" value="" />
    <input type="hidden" runat="server" id="hdnParamedicIDCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnParamedicNameCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentLayoutCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentValuesCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnParamedicIDCBCtl1" value="" />
    <input type="hidden" runat="server" id="hdnParamedicNameCBCtl1" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentLayoutCBCtl1" value="" />
    <input type="hidden" runat="server" id="hdnAssessmentValuesCBCtl1" value="" />
    <input type="hidden" runat="server" id="hdnIsInitialAssessment" value="" />
    <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
        <tr>
            <td>
                <div class="containerUlTabPage" style="margin-bottom: 3px;">
                    <ul class="ulTabPage" id="ulResultDetail">
                        <li class="selected" contentid="panByPengkajianPasien">
                            <%=GetLabel("Pengkajian Pasien")%></li>
                        <li contentid="panByPopulasi">
                            <%=GetLabel("Asesmen Tambahan (Populasi Khusus)")%></li>
                    </ul>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="containerOrderDt" id="panByPengkajianPasien">
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
                                                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
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
                                                    <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="AssessmentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn visitID" />
                                                            <asp:BoundField DataField="cfAssessmentDate" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="AssessmentTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentTime"
                                                                ItemStyle-CssClass="assessmentTime" />
                                                            <asp:BoundField DataField="ParamedicID" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicID hiddenColumn"
                                                                ItemStyle-CssClass="paramedicID hiddenColumn" />
                                                            <asp:BoundField DataField="GCAssessmentType" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                HeaderStyle-CssClass="formType hiddenColumn" ItemStyle-CssClass="formType hiddenColumn" />
                                                            <asp:BoundField DataField="AssessmentFormLayout" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                HeaderStyle-CssClass="formLayout hiddenColumn" ItemStyle-CssClass="formLayout hiddenColumn" />
                                                            <asp:BoundField DataField="AssessmentFormValue" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                HeaderStyle-CssClass="formValue hiddenColumn" ItemStyle-CssClass="formValue hiddenColumn" />
                                                            <asp:BoundField DataField="IsInitialAssessment" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="isInitialAssessment hiddenColumn"
                                                                ItemStyle-CssClass="isInitialAssessment hiddenColumn" />
                                                            <asp:BoundField DataField="IsNeedVerified" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="isNeedVerified hiddenColumn"
                                                                ItemStyle-CssClass="isNeedVerified hiddenColumn" />
                                                            <asp:BoundField DataField="ParamedicName" HeaderText="Dikaji Oleh" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicName" ItemStyle-CssClass="paramedicName" />
                                                            <asp:BoundField DataField="cfIsInitialAssessment" HeaderText="Kajian Awal" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="70px" />
                                                            <asp:BoundField DataField="cfAssessmentDatePickerFormat" HeaderText="Values" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentDate hiddenColumn"
                                                                ItemStyle-CssClass="assessmentDate hiddenColumn" />
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="60px">
                                                                <ItemTemplate>
                                                                    <div>
                                                                        <a class="lnkView" recordid="<%#:Eval("AssessmentID") %>" visitid="<%#:Eval("VisitID") %>"
                                                                            assessmenttype="<%#:Eval("GCAssessmentType") %>" assessmentdate="<%#:Eval("cfAssessmentDatePickerFormat") %>"
                                                                            assessmenttime="<%#:Eval("AssessmentTime") %>" assessmentlayout="<%#:Eval("AssessmentFormLayout") %>"
                                                                            assessmentvalue="<%#:Eval("AssessmentFormValue") %>" paramedicname="<%#:Eval("ParamedicName") %>"
                                                                            isinitialassessment="<%#:Eval("IsInitialAssessment") %>">View</a>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Tidak ada data pengkajian populasi khusus untuk pasien ini") %>
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
                </div>
                <div class="containerOrderDt" id="panByPopulasi" style="display:none">
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
                                                <asp:RadioButtonList ID="rblItemType1" runat="server" RepeatDirection="Horizontal"
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
                                                <dxcp:ASPxCallbackPanel ID="cbpView1" runat="server" Width="100%" ClientInstanceName="cbpView1"
                                                    ShowLoadingPanel="false" OnCallback="cbpView1_Callback">
                                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                        EndCallback="function(s,e){ onCbpView1EndCallback(s); }" />
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent3" runat="server">
                                                            <asp:Panel runat="server" ID="pnlView3" CssClass="pnlContainerGridPatientPage">
                                                                <asp:GridView ID="grdView1" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
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
                                                <div class="imgLoadingGrdView" id="Div1">
                                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                </div>
                                                <div class="containerPaging">
                                                    <div class="wrapperPaging">
                                                        <div id="Div2">
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
                                    <dxcp:ASPxCallbackPanel ID="cbpViewDt1" runat="server" Width="100%" ClientInstanceName="cbpViewDt1"
                                        ShowLoadingPanel="false" OnCallback="cbpViewDt1_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                            EndCallback="function(s,e){ onCbpViewDt1EndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent4" runat="server">
                                                <asp:Panel runat="server" ID="Panel4" CssClass="pnlContainerGridPatientPage">
                                                    <asp:GridView ID="grdViewDt1" runat="server" CssClass="grdSelected grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="AssessmentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn visitID" />
                                                            <asp:BoundField DataField="cfAssessmentDate" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" />
                                                            <asp:BoundField DataField="AssessmentTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentTime"
                                                                ItemStyle-CssClass="assessmentTime" />
                                                            <asp:BoundField DataField="ParamedicID" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicID hiddenColumn"
                                                                ItemStyle-CssClass="paramedicID hiddenColumn" />
                                                            <asp:BoundField DataField="GCAssessmentType" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                HeaderStyle-CssClass="formType hiddenColumn" ItemStyle-CssClass="formType hiddenColumn" />
                                                            <asp:BoundField DataField="AssessmentFormLayout" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                HeaderStyle-CssClass="formLayout hiddenColumn" ItemStyle-CssClass="formLayout hiddenColumn" />
                                                            <asp:BoundField DataField="AssessmentFormValue" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                HeaderStyle-CssClass="formValue hiddenColumn" ItemStyle-CssClass="formValue hiddenColumn" />
                                                            <asp:BoundField DataField="IsInitialAssessment" HeaderText="Values" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="isInitialAssessment hiddenColumn"
                                                                ItemStyle-CssClass="isInitialAssessment hiddenColumn" />
                                                            <asp:BoundField DataField="ParamedicName" HeaderText="Dikaji Oleh" HeaderStyle-HorizontalAlign="Left"
                                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="paramedicName" ItemStyle-CssClass="paramedicName" />
                                                            <asp:BoundField DataField="cfAssessmentDatePickerFormat" HeaderText="Values" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="assessmentDate hiddenColumn"
                                                                ItemStyle-CssClass="assessmentDate hiddenColumn" />
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="60px">
                                                                <ItemTemplate>
                                                                    <div>
                                                                        <a class="lnkView1" recordid="<%#:Eval("AssessmentID") %>" visitid="<%#:Eval("VisitID") %>"
                                                                            assessmenttype="<%#:Eval("GCAssessmentType") %>" assessmentdate="<%#:Eval("cfAssessmentDatePickerFormat") %>"
                                                                            assessmenttime="<%#:Eval("AssessmentTime") %>" assessmentlayout="<%#:Eval("AssessmentFormLayout") %>"
                                                                            assessmentvalue="<%#:Eval("AssessmentFormValue") %>" paramedicname="<%#:Eval("ParamedicName") %>"
                                                                            isinitialassessment="<%#:Eval("IsInitialAssessment") %>">View</a>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Tidak ada data pengkajian populasi khusus untuk pasien ini") %>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="imgLoadingGrdView" id="Div3">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                    <div class="containerPaging">
                                        <div class="wrapperPaging">
                                            <div id="Div4">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
