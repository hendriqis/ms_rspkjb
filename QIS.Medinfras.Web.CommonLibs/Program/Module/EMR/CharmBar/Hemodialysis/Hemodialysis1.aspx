<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true" CodeBehind="Hemodialysis1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.Hemodialysis1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript" id="dxss_HemodialysisHistory">

        var gridView1PageCount = parseInt('<%=gridView1PageCount %>');
        var gridView2PageCount = parseInt('<%=gridView2PageCount %>');

        $(function () {
            //#region Header Group
            $('#ulTabHeaderGroup li').click(function () {
                $('#ulTabHeaderGroup li.selected').removeAttr('class');
                $('.containerHeaderTab').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#<%=hdnSelectedTab.ClientID %>').val($contentID);
                $('#' + $contentID).show();
                $(this).addClass('selected');
                switch ($('#<%=hdnSelectedTab.ClientID %>').val()) {
                    case "grpContent1":
                        $('#tabDetail').show();
                        $('#divFormDetailList').hide();
                        break;
                    case "grpContent2":
                        $('#tabDetail').hide();
                        $('#divFormDetailList').show();
                        //cbpFormList.PerformCallback('refresh');
                        break;
                    default:
                        break;
                }
            });
            //#endregion     

            //#region Detail Tab
            $('#ulTabDetail li').click(function () {
                $('#ulTabDetail li.selected').removeAttr('class');
                $('.containerDetailTab').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#<%=hdnSelectedDetailTab.ClientID %>').val($contentID);
                $('#' + $contentID).show();
                $(this).addClass('selected');
                switch ($('#<%=hdnSelectedDetailTab.ClientID %>').val()) {
                    case "grpDetailContent1":
                        cbpViewDt3.PerformCallback("refresh");
                        break;
                    case "grpDetailContent2":
                        cbpViewDt4.PerformCallback("refresh");
                        break;
                    default:
                        break;
                }
            });
            //#endregion                     

            setPaging($("#pagingHd1"), gridView1PageCount, function (page) {
                cbpHistoryView.PerformCallback('changepage|' + page);
            });

            setPaging($("#pagingHd2"), gridView2PageCount, function (page) {
                cbpFormList.PerformCallback('changepage|' + page);
            });


            $('#<%=grdHistoryView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdHistoryView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnAssessmentID.ClientID %>').val($(this).find('.keyField').html());

                $('#<%=txtStartDate.ClientID %>').val($(this).find('.cfStartDate').html().replace(/&nbsp;/gi, " "));
                $('#<%=txtStartTime.ClientID %>').val($(this).find('.startTime').html().replace(/&nbsp;/gi, " "));
                $('#<%=txtEndDate.ClientID %>').val($(this).find('.cfEndDate').html().replace(/&nbsp;/gi, " "));
                $('#<%=txtEndTime.ClientID %>').val($(this).find('.endTime').html().replace(/&nbsp;/gi, " "));
                $('#<%=txtParamedicName.ClientID %>').val($(this).find('.postHDParamedicName').html().replace(/&nbsp;/gi, " "));
                $('#<%=txtFinalUFG.ClientID %>').val($(this).find('.finalUFG').html().replace(/&nbsp;/gi, " "));
                $('#<%=txtTotalOutput.ClientID %>').val($(this).find('.totalOutput').html().replace(/&nbsp;/gi, " "));
                $('#<%=txtPrimingBalance.ClientID %>').val($(this).find('.priming').html().replace(/&nbsp;/gi, " "));
                $('#<%=txtWashOut.ClientID %>').val($(this).find('.washout').html().replace(/&nbsp;/gi, " "));
                $('#<%=txtTotalIntake.ClientID %>').val($(this).find('.totalIntake').html().replace(/&nbsp;/gi, " "));
                $('#<%=txtTotalUF.ClientID %>').val($(this).find('.totalUF').html().replace(/&nbsp;/gi, " ")); 

                if ($(this).find('.postHDAnamnese').html() != "&nbsp;") {
                    $('#<%=txtPostHDAnamnese.ClientID %>').val($(this).find('.postHDAnamnese').html());
                };

                switch ($('#<%=hdnSelectedDetailTab.ClientID %>').val()) {
                    case "grpDetailContent1":
                        cbpViewDt3.PerformCallback("refresh");
                        break;
                    case "grpDetailContent2":
                        cbpViewDt4.PerformCallback("refresh");
                        break;
                    case "grpDetailContent3":
                        break;
                    default:
                        $('#ulTabDetail li:first').click();
                        break;
                }
            });
            $('#<%=grdHistoryView.ClientID %> tr:eq(1)').click();

            $('#<%=grdFormList.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdFormList.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnGCAssessmentType.ClientID %>').val($(this).find('.keyField').html());
                cbpViewFormDt.PerformCallback('refresh');
                $('#<%=hdnFormID.ClientID %>').val('')
            });
            $('#<%=grdFormList.ClientID %> tr:eq(1)').click();

            $('#ulTabHeaderGroup li:first').click();
        });

        //#region Grid : Riwayat Hemodialisa (Group)
         function onCbpHistoryViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdHistoryView.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingHd1"), pageCount, function (page) {
                    cbpHistoryView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdHistoryView.ClientID %> tr:eq(1)').click();
        }

        $('.imgViewAssessment.imgLink').die('click');
        $('.imgViewAssessment.imgLink').live('click', function (evt) {
            var url = ResolveUrl("~/libs/Program/Module/MedicalDiagnostic/Hemodialysis/Assessment/ViewPreHDAssessmentCtl.ascx");
            var recordID = $(this).attr('recordID');
            var param = recordID;
            openUserControlPopup(url, param, "Peresepan Hemodialisa", 700, 500);
        });

        function onCbpViewDt3EndCallback(s) {
            $('#containerImgLoadingViewDt3').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt3.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt3"), pageCount1, function (page) {
                    cbpViewDt3.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt3.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt4EndCallback(s) {
            $('#containerImgLoadingViewDt4').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount4 = parseInt(param[1]);

                if (pageCount4 > 0)
                    $('#<%=grdViewDt4.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt4"), pageCount4, function (page) {
                    cbpViewDt4.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt4.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Grid : Form Pengkajian
        function onCbpFormListEndCallback(s) {
            $('#containerHdImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdFormList.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingHd2"), pageCount, function (page) {
                    cbpFormList.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdFormList.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewFormDtEndCallback(s) {
            $('#containerImgLoadingViewFormDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewFormDt.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingFormDt"), pageCount, function (page) {
                    cbpViewFormDt.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewFormDt.ClientID %> tr:eq(1)').click();
        }

        $('.imgViewForm.imgLink').die('click');
        $('.imgViewForm.imgLink').live('click', function (evt) {
            var formType = $(this).attr("assessmentType");
            var id = $(this).attr('recordID');
            var date = $(this).attr('assessmentDate');
            var time = $(this).attr('assessmentTime');
            var ppa = $(this).attr("paramedicName");
            var isInitialAssessment = $(this).attr("isInitialAssessment");
            var layout = $(this).attr("assessmentLayout");
            var values = $(this).attr("assessmentValue");
            var formGroup = "1";
            var visitID = $(this).attr("visitID");

            var medicalNo = $('#<%=hdnPageMedicalNo.ClientID %>').val();
            var patientName = $('#<%=hdnPagePatientName.ClientID %>').val();
            var patientDOB = $('#<%=hdnPagePatientDOB.ClientID %>').val();
            var registrationNo = $('#<%=hdnPageRegistrationNo.ClientID %>').val();
            var patientInfo = medicalNo + "|" + patientName + "|" + patientDOB + "|" + registrationNo;


            var param = formType + '|' + id + '|' + date + '|' + time + '|' + ppa + '|' + isInitialAssessment + '|' + layout + '|' + values + '|' + formGroup + "|" + visitID + '|' + patientInfo + '|' + '0';
            var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/ViewNursePatientAssessmentCtl.ascx");
            openUserControlPopup(url, param, 'Pengkajian Pasien', 800, 600);
        });
        //#endregion
    </script>
    <style type="text/css">
        .containerHeaderTab 
        {
            max-width : 350px;
        }
    </style>
    <input type="hidden" value="" id="hdnSelectedTab" runat="server" />
    <input type="hidden" value="" id="hdnSelectedDetailTab" runat="server" />
    <input type="hidden" runat="server" id="hdnAssessmentID" value="" />
    <input type="hidden" runat="server" id="hdnGCAssessmentType" value="" />
    <input type="hidden" runat="server" id="hdnGCAssessmentGroup" value="" />
    <input type="hidden" runat="server" id="hdnFormID" value="" />
    <input type="hidden" value="" id="hdnPageMedicalNo" runat="server" />
    <input type="hidden" value="" id="hdnPagePatientName" runat="server" />
    <input type="hidden" value="" id="hdnPagePatientDOB" runat="server" />
    <input type="hidden" value="" id="hdnPageRegistrationNo" runat="server" />
    <div>
        <table border="0" width="100%">
            <colgroup>
                <col style="width: 350px;" />
                <col />
            </colgroup>
            <tr>
                <td valign="top">
                    <div style="position: relative; width: 100%">
                        <table border="0" cellpadding="0" cellspacing="1" width="100%">
                            <colgroup>
                                <col style="width: 100px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td colspan="3">
                                    <div class="containerUlTabPage">
                                        <ul class="ulTabPage" id="ulTabHeaderGroup">
                                            <li class="selected" contentid="grpContent1"><%=GetLabel("Riwayat Hemodialisis") %></li>
                                            <li contentid="grpContent2"><%=GetLabel("Form Pengkajian") %></li>
                                        </ul>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div class="containerHeaderTab" id="grpContent1">
                                        <div style="position: relative; width: 100%">
                                            <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpHistoryView"
                                                ShowLoadingPanel="false" OnCallback="cbpHistoryView_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                    EndCallback="function(s,e){ onCbpHistoryViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContentHistoryView1" runat="server">
                                                        <asp:Panel runat="server" ID="pnlHistoryView" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdHistoryView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                        HeaderStyle-Width="60px">
                                                                        <ItemTemplate>
                                                                            <div id="divView" runat="server" style='margin-top: 5px; text-align: center'>
                                                                                <img class="imgViewAssessment imgLink" title='<%=GetLabel("Lihat Asesmen")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                                    alt="" recordID = "<%#:Eval("ID") %>" visitID = "<%#:Eval("VisitID") %>" />                                
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn visitID" ItemStyle-CssClass="hiddenColumn visitID" />
                                                                    <asp:BoundField DataField="cfAssessmentDate" HeaderText="Tanggal " HeaderStyle-Width="100px"
                                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cfNoteDate" />
                                                                    <asp:BoundField DataField="HDNo" HeaderText="HD Ke- " HeaderStyle-Width="50px"
                                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="hdType" />
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="110px">
                                                                        <HeaderTemplate>
                                                                            <div><%=GetLabel("Jenis Dialiser") %></div>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td style="vertical-align:top">
                                                                                        <div><%#: Eval("HDMachineType")%></div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="HDMethod" HeaderText="Teknik HD" HeaderStyle-Width="80px"
                                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="hdMethod" />
<%--                                                                    <asp:BoundField DataField="QB" HeaderText="QB" HeaderStyle-Width="40px"
                                                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="qb" />
                                                                    <asp:BoundField DataField="QD" HeaderText="QD" HeaderStyle-Width="40px"
                                                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="qd" />
                                                                    <asp:BoundField DataField="UFGoal" HeaderText="UF" HeaderStyle-Width="40px"
                                                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="uf" />--%>
                                                                    <asp:BoundField DataField="cfStartDate" HeaderText="Tanggal Mulai " HeaderStyle-Width="100px"
                                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cfStartDate hiddenColumn" HeaderStyle-CssClass = "hiddenColumn" />
                                                                    <asp:BoundField DataField="StartTime" HeaderText="Jam Mulai" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="startTime hiddenColumn" HeaderStyle-CssClass = "hiddenColumn"/>
                                                                    <asp:BoundField DataField="cfEndDate" HeaderText="Tanggal Selesai " HeaderStyle-Width="100px"
                                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cfEndDate hiddenColumn" HeaderStyle-CssClass = "hiddenColumn" />
                                                                    <asp:BoundField DataField="EndTime" HeaderText="Jam Selesai " HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="endTime hiddenColumn" HeaderStyle-CssClass = "hiddenColumn" />
                                                                    <asp:BoundField DataField="PostHDParamedicName" HeaderText="Pengkajian Post HD oleh " HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left"
                                                                        ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="postHDParamedicName hiddenColumn" HeaderStyle-CssClass = "hiddenColumn" />
                                                                    <asp:BoundField DataField="FinalUFG" HeaderText="UFG Akhir HD" HeaderStyle-Width="80px"
                                                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="finalUFG hiddenColumn" HeaderStyle-CssClass = "hiddenColumn" />
                                                                    <asp:BoundField DataField="TotalOutput" HeaderText="Output (Monitoring)" HeaderStyle-Width="80px"
                                                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="totalOutput hiddenColumn" HeaderStyle-CssClass = "hiddenColumn" />
                                                                    <asp:BoundField DataField="PrimingBalance" HeaderText="Priming" HeaderStyle-Width="80px"
                                                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="priming hiddenColumn" HeaderStyle-CssClass = "hiddenColumn" />
                                                                    <asp:BoundField DataField="WashOut" HeaderText="Wash Out" HeaderStyle-Width="80px"
                                                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="washout hiddenColumn" HeaderStyle-CssClass = "hiddenColumn" />
                                                                    <asp:BoundField DataField="TotalIntake" HeaderText="Intake (Monitoring)" HeaderStyle-Width="80px"
                                                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="totalIntake hiddenColumn" HeaderStyle-CssClass = "hiddenColumn" />
                                                                    <asp:BoundField DataField="TotalUF" HeaderText="Total UF" HeaderStyle-Width="80px"
                                                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="totalUF hiddenColumn" HeaderStyle-CssClass = "hiddenColumn" />
                                                                    <asp:BoundField DataField="PostHDAnamnese" HeaderText="Total UF" HeaderStyle-Width="80px"
                                                                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="postHDAnamnese hiddenColumn" HeaderStyle-CssClass = "hiddenColumn" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("HDNo") %>" bindingfield="hdNo" />
                                                                            <input type="hidden" value="<%#:Eval("HDDuration") %>" bindingfield="hdDuration" />
                                                                            <input type="hidden" value="<%#:Eval("QB") %>" bindingfield="qb" />
                                                                            <input type="hidden" value="<%#:Eval("QD") %>" bindingfield="QD" />
                                                                            <input type="hidden" value="<%#:Eval("UFGoal") %>" bindingfield="ufGoal" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Tidak ada informasi peresepan hemodialisa untuk pasien di kunjungan saat ini")%>
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
                                                    <div id="pagingHd1">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>    
                                    <div class="containerHeaderTab" id="grpContent2"  style="display:none">
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
                                                    <div id="pagingHd2"></div>
                                                </div>
                                            </div> 
                                        </div>
                                    </div>                                                         
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td style="vertical-align:top">
                    <div id="tabDetail">
                        <div class="containerUlTabPage">
                            <ul class="ulTabPage" id="ulTabDetail">
                                <li class="selected" contentid="grpDetailContent1"><%=GetLabel("Tanda Vital/Indikator Lainnya") %></li>
                                <li contentid="grpDetailContent2"><%=GetLabel("Intake-Output") %></li>                                
                                <li contentid="grpDetailContent3"><%=GetLabel("Post Hemodialisa") %></li>
                            </ul>
                        </div>
                        <div class="containerDetailTab" id="grpDetailContent1">
                            <dxcp:ASPxCallbackPanel ID="cbpViewDt3" runat="server" Width="100%" ClientInstanceName="cbpViewDt3"
                                ShowLoadingPanel="false" OnCallback="cbpViewDt3_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt3').show(); }"
                                    EndCallback="function(s,e){ onCbpViewDt3EndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent6" runat="server">
                                        <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                            <asp:GridView ID="grdViewDt3" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdViewDt3_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                    <asp:BoundField DataField="CreatedBy" HeaderStyle-CssClass="keyUser hiddenColumn" ItemStyle-CssClass="keyUser hiddenColumn" />
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                        <HeaderTemplate>
                                                            <div>
                                                                Indikator Pemantauan Intra Hemodialiasa</div>
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
                                                            <div>
                                                                <span style="font-weight:bold; text-decoration: underline; color:Black"><%=GetLabel("Catatan Tambahan :")%></span>
                                                                <br />
                                                                <span style="font-style:italic">
                                                                    <%#: Eval("Remarks")%>
                                                                </span>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>                                                
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <div>
                                                        <div><%=GetLabel("Belum ada informasi pemantauan intra hemodialisa untuk sesi ini.") %></div>
                                                    </div>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>   
                            <div class="imgLoadingGrdView" id="containerImgLoadingViewDt3" >
                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                            </div>
                            <div class="containerPaging">
                                <div class="wrapperPaging">
                                    <div id="pagingDt3"></div>
                                </div>
                            </div> 
                        </div>
                        <div class="containerDetailTab" id="grpDetailContent2">
                            <dxcp:ASPxCallbackPanel ID="cbpViewDt4" runat="server" Width="100%" ClientInstanceName="cbpViewDt4"
                                ShowLoadingPanel="false"  OnCallback="cbpViewDt4_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt4').show(); }"
                                    EndCallback="function(s,e){ onCbpViewDt4EndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent3" runat="server">
                                        <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGridPatientPage">
                                            <asp:GridView ID="grdViewDt4" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                    <asp:BoundField DataField="CreatedBy" HeaderStyle-CssClass="keyUser hiddenColumn" ItemStyle-CssClass="keyUser hiddenColumn" />
                                                    <asp:BoundField HeaderText="Tanggal"  DataField="cfLogDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" ItemStyle-CssClass="LogTime" />
                                                    <asp:BoundField HeaderText="Jam"  DataField="LogTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-CssClass="LogTime" />
                                                    <asp:BoundField HeaderText="Cairan"  DataField="FluidName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px"/>
                                                    <asp:BoundField HeaderText="Intake"  DataField="cfIntakeAmount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="70px"/>
                                                    <asp:BoundField HeaderText="Output"  DataField="cfOutputAmount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="70px"/>
                                                    <asp:BoundField HeaderText="Tenaga Medis"  DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <div>
                                                        <div class="blink"><%=GetLabel("Belum ada informasi intake/output pada tanggal ini") %></div>
                                                    </div>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>    
                            <div class="imgLoadingGrdView" id="containerImgLoadingViewDt4" >
                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                            </div>
                            <div class="containerPaging">
                                <div class="wrapperPaging">
                                    <div id="pagingDt4"></div>
                                </div>
                            </div> 
                        </div>
                        <div class="containerDetailTab" id="grpDetailContent3">

                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 200px" />
                                <col style="width: 30px" />
                                <col style="width: 150px" />
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal dan Waktu Mulai")%></label>
                                </td>
                                <td colspan="4">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtStartDate" Width="100px" runat="server" ReadOnly="true" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStartTime" Width="60px" CssClass="number" runat="server" Style="text-align: center" ReadOnly="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal dan Waktu Selesai")%></label>
                                </td>
                                <td colspan="4">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtEndDate" Width="100px" runat="server" ReadOnly="true" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEndTime" Width="60px" CssClass="number" runat="server" Style="text-align: center" ReadOnly="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Perawat Pengkaji")%></label>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtParamedicName" Width="250px" runat="server" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("UFG Akhir HD")%></label>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtFinalUFG" Width="80px" CssClass="number" runat="server" Style="text-align: right" Text="0" ReadOnly="true" /> cc
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Total Output (Monitoring)")%></label>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtTotalOutput" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" Text="0"/> cc
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Sisa Priming")%></label>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtPrimingBalance" Width="80px" CssClass="number" runat="server" Style="text-align: right" Text="0" ReadOnly="true"/> cc
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Wash Out")%></label>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtWashOut" Width="80px" CssClass="number" runat="server" Style="text-align: right" Text="0" ReadOnly="true"/> cc
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Total Intake (Monitoring)")%></label>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtTotalIntake" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" Text="0"/> cc
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="font-weight:bold">
                                    <label class="lblNormal">
                                        <%=GetLabel("TOTAL UF")%></label>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtTotalUF" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" Text="0"/> cc
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="vertical-align:top;font-weight:bold">
                                    <label class="lblNormal">
                                        <%=GetLabel("Keluhan Post HD")%></label>
                                </td>
                                <td colspan="4" style="vertical-align:top">
                                    <asp:TextBox ID="txtPostHDAnamnese" runat="server" TextMode="MultiLine" Rows="15"
                                        Width="99%" ReadOnly="true" />
                                </td>
                            </tr>
                        </table>
                        </div>
                    </div>
                    <div id="divFormDetailList" style="display:none">
                        <dxcp:ASPxCallbackPanel ID="cbpViewFormDt" runat="server" Width="100%" ClientInstanceName="cbpViewFormDt"
                            ShowLoadingPanel="false" OnCallback="cbpViewFormDt_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewFormDt').show(); }"
                                EndCallback="function(s,e){ onCbpViewFormDtEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelViewFormDt" runat="server">
                                   <input type="hidden" value="" id="hdnFileString" runat="server" />
                                    <asp:Panel runat="server" ID="pnlViewFormDt" CssClass="pnlContainerGridPatientPage">
                                        <asp:GridView ID="grdViewFormDt" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <div id="divViewForm" runat="server" style='text-align: center'>
                                                            <img class="imgViewForm imgLink" title='<%=GetLabel("Lihat Form")%>' src='<%# ResolveUrl("~/Libs/Images/Button/search.png")%>'
                                                                alt="" visitID="<%#:Eval("VisitID") %>" assessmentType="<%#:Eval("GCAssessmentType") %>" assessmentDate="<%#:Eval("cfAssessmentDatePickerFormat") %>" assessmentTime="<%#:Eval("AssessmentTime") %>" assessmentLayout="<%#:Eval("AssessmentFormLayout") %>" assessmentValue="<%#:Eval("AssessmentFormValue") %>"
                                                                paramedicName="<%#:Eval("ParamedicName") %>" isInitialAssessment="<%#:Eval("IsInitialAssessment") %>" />                                
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
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
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Tidak ada data pengkajian untuk pasien ini") %>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="imgLoadingGrdView" id="containerImgLoadingViewFormDt">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                        <div class="containerPaging">
                            <div class="wrapperPaging">
                                <div id="pagingFormDt">
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
