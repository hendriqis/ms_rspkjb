<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true" 
    CodeBehind="PatientFluidBalanceList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientFluidBalanceList1" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript" id="dxss_PatientFluidBalanceList1">
        $(function () {
            setDatePicker('<%=txtFromDate.ClientID %>');
            $('#<%=txtFromDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtToDate.ClientID %>');
            $('#<%=txtToDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=btnApplyFilter.ClientID %>').click(function () {
                if (IsParameterValid() == true) {
                    var param = $('#<%=txtFromDate.ClientID %>').val() + "|" + $('#<%=txtToDate.ClientID %>').val() + "|" + $('#<%=txtFromTime.ClientID %>').val() + "|" + $('#<%=txtToTime.ClientID %>').val();
                    cbpCalculateBalanceSummary.PerformCallback(param);
                }
                else {
                    displayErrorMessageBox('Calculate Balance', "Invalid Parameter");
                }
            });

            $('#contentDetailNavPane a').click(function () {
                $('#contentDetailNavPane a.selected').removeClass('selected');
                $(this).addClass('selected');
                var contentID = $(this).attr('contentID');

                if (contentID != null) {
                    showDetailContent(contentID);
                    if (contentID == "contentDetailPage1") {
                        setPaging($("#paging"), pageCount, function (page) {
                            cbpView.PerformCallback('changepage|' + page);
                        });
                    }
                }
            });

            $('#contentDetailNavPane a').first().click();


            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnVisitIDCBCtl.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnLogDateCBCtl.ClientID %>').val($(this).find('.cfLogDate1').html());
                $('#<%=hdnOrderNoCBCtl.ClientID %>').val($(this).find('.orderNo').html());
                if ($('#<%=hdnLogDateCBCtl.ClientID %>').val() != "") {
                    cbpViewDt.PerformCallback('refresh');
                    cbpViewDt2.PerformCallback('refresh');
                    cbpViewDt3.PerformCallback('refresh');
                    cbpViewDt5.PerformCallback('refresh');
                }
            });

            $('#<%=grdViewDt.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdViewDt.ClientID %> tr.selected').removeClass('selected');
                $('#<%=hdnGCFluidTypeCBCtl.ClientID %>').val($(this).find('.gcFluidType').html());
                $('#<%=hdnFluidNameCBCtl.ClientID %>').val($(this).find('.fluidName').html());
                $(this).addClass('selected');
                $('#<%=hdnVisitIDCBCtl.ClientID %>').val($(this).find('.keyField').html());
                cbpViewDt4.PerformCallback('refresh');
            });

            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            //#region Detail Tab
            $('#ulTabOrderDetail li').click(function () {
                $('#ulTabOrderDetail li.selected').removeAttr('class');
                $('.containerOrderDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#<%=hdnSelectedTab.ClientID %>').val($contentID);
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
            //#endregion
        });

        //#region Summary : Action Button
        $('.lblIntakeSummary.lblLink').die('click');
        $('.lblIntakeSummary.lblLink').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/GridViewDt/PatientFluidBalanceSummaryInfoCtl.ascx");
            var group = 'X459^01';
            var visitID = $(this).attr('visitID');
            var logDate = $(this).attr('logDate');
            var param = group + "|" + visitID + "|" + logDate + "|" + "0";
            openUserControlPopup(url, param, "INTAKE : " + $(this).attr('logDate'), 800, 500);
        });

        $('.lblIntake2Summary.lblLink').die('click');
        $('.lblIntake2Summary.lblLink').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/GridViewDt/PatientFluidBalanceSummaryInfoCtl.ascx");
            var group = 'X459^04';
            var visitID = $(this).attr('visitID');
            var logDate = $(this).attr('logDate');
            var param = group + "|" + visitID + "|" + logDate + "|" + "0";
            openUserControlPopup(url, param, "INTAKE YANG TIDAK DIUKUR : " + $(this).attr('logDate'), 800, 500);
        });

        $('.lblOutputSummary.lblLink').die('click');
        $('.lblOutputSummary.lblLink').live('click', function () {
            var group = 'X459^02';
            var url = ResolveUrl("~/libs/Controls/EMR/GridViewDt/PatientFluidBalanceSummaryInfoCtl.ascx");
            var visitID = $(this).attr('visitID');
            var logDate = $(this).attr('logDate');
            var param = group + "|" + visitID + "|" + logDate + "|" + "0";
            openUserControlPopup(url, param, "OUTPUT : " + $(this).attr('logDate'), 800, 500);
        });

        $('.lblOutput2Summary.lblLink').die('click');
        $('.lblOutput2Summary.lblLink').live('click', function () {
            var url = ResolveUrl("~/libs/Controls/EMR/GridViewDt/PatientFluidBalanceSummaryInfoCtl.ascx");
            var group = 'X459^03';
            var visitID = $(this).attr('visitID');
            var logDate = $(this).attr('logDate');
            var param = group + "|" + visitID + "|" + logDate + "|" + "0";
            openUserControlPopup(url, param, "OUTPUT YANG TIDAK DIUKUR : " + $(this).attr('logDate'), 800, 500);
        });
        //#endregion

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
            cbpView6.PerformCallback('refresh');
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

        //#region Paging Dt
        function onCbpViewDtEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount1 = parseInt(param[1]);

                if (pageCount1 > 0)
                    $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt1"), pageCount1, function (page) {
                    cbpViewDt.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt2EndCallback(s) {
            $('#containerImgLoadingViewDt2').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount2 = parseInt(param[1]);

                if (pageCount2 > 0)
                    $('#<%=grdViewDt2.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt2"), pageCount2, function (page) {
                    cbpViewDt2.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt2.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewDt3EndCallback(s) {
            $('#containerImgLoadingViewDt3').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount3 = parseInt(param[1]);

                if (pageCount3 > 0)
                    $('#<%=grdViewDt3.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt3"), pageCount3, function (page) {
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

        function onCbpViewDt5EndCallback(s) {
            $('#containerImgLoadingViewDt5').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount5 = parseInt(param[1]);

                if (pageCount5 > 0)
                    $('#<%=grdViewDt5.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt5"), pageCount5, function (page) {
                    cbpViewDt5.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt5.ClientID %> tr:eq(1)').click();
        }

        function onCbpView6EndCallback(s) {
            $('#containerImgLoadingView6').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount6 = parseInt(param[1]);

                if (pageCount6 > 0)
                    $('#<%=grdView6.ClientID %> tr:eq(1)').click();

                setPaging($("#paging6"), pageCount6, function (page) {
                    cbpView6.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView6.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            return true;
        }

        function onBeforeLoadRightPanelContent(code) {
            return $('#<%:hdnRegistrationIDCBCtl.ClientID %>').val();
        }

        function showDetailContent(contentID) {
            var i, x, tablinks;
            x = document.getElementsByClassName("contentDetail");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(contentID).style.display = "block";
        }

        function IsParameterValid() {
            if ($('#<%=txtFromDate.ClientID %>').val() != "" && $('#<%=txtToDate.ClientID %>').val() && $('#<%=txtFromTime.ClientID %>').val() && $('#<%=txtToTime.ClientID %>').val())
                return true;
            else
                return false;
        }

        function onCbpCalculateBalanceSummaryEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] != 'success') {
                displayErrorMessageBox('Calculate Fluid Balance', param[1]);
            }
            else {
                var valueInfo = param[1].split(';');
                $('#<%=txtTotalIntake1.ClientID %>').val(valueInfo[0]);
                $('#<%=txtTotalIntake2.ClientID %>').val(valueInfo[1]);
                $('#<%=txtTotalOutput1.ClientID %>').val(valueInfo[2]);
                $('#<%=txtTotalOutput2.ClientID %>').val(valueInfo[3]);                
                var intake = parseFloat(valueInfo[0]);
                var output = parseFloat(valueInfo[2]);
                var summary = intake - output;
                $('#<%=txtBalance.ClientID %>').val(summary);
            }
        }
    </script>
    <style type="text/css">
        .keyUser
        {
            display: none;
        }
        
        #contentDetailNavPane > a       { margin:0; font-size:11px}
        #contentDetailNavPane > a.selected { color:#fff!important;background-color:#f44336!important }                  
    </style>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" runat="server" id="hdnVisitIDCBCtl" />
    <input type="hidden" runat="server" id="hdnIDCBCtl" value="0" />
    <input type="hidden" runat="server" id="hdnLogDateCBCtl" value="0" />
    <input type="hidden" runat="server" id="hdnLogTimeCBCtl" value="0" />
    <input type="hidden" runat="server" id="hdnGCFluidTypeCBCtl" value="0" />
    <input type="hidden" runat="server" id="hdnFluidNameCBCtl" value="0" />
    <input type="hidden" id="hdnMRNCBCtl" runat="server" />
    <input type="hidden" id="hdnRegistrationIDCBCtl" runat="server" />
    <input type="hidden" id="hdnDepartmentIDCBCtl" runat="server" />
    <input type="hidden" id="hdnOperatingRoomIDCBCtl" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionCBCtl" runat="server" value="" />
    <input type="hidden" runat="server" id="hdnModuleIDCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnSurgeryReportIDCBCtl" value="0" />
    <input type="hidden" runat="server" id="hdnAssessmentIDCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnOrderNoCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnCurrentUserID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentSessionID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicIDCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnRevisedParamedicIDCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnIsEditableCBCtl" value="0" />
    <input type="hidden" runat="server" id="hdnIsRevisedCBCtl" value="0" />
    <input type="hidden" runat="server" id="hdnPatientNoteTypeCBCtl" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowRevisionCBCtl" value="0" />
    <input type="hidden" runat="server" id="hdnPatientDocumentUrlCBCtl" value="0" />
    <input type="hidden" value="0" id="hdnRowIndex" runat="server" />
    <input type="hidden" value="" id="hdnSelectedTab" runat="server" />
    <div id="contentDetailNavPane" class="w3-bar w3-black">
        <a contentID="contentDetailPage1" class="w3-bar-item w3-button tablink selected">Intake-Output</a>
        <a contentID="contentDetailPage2" class="w3-bar-item w3-button tablink">Intake-Output Balance Summary</a>
    </div>
    <div id="contentDetailPage1" class="container contentDetail  w3-animate-top" style="height:600px;display:none">
         <table style="width:100%">
            <colgroup>
                <col style="width:40%"/>
                <col style="width:60%"/>
            </colgroup>
            <tr>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="1" style="width:100%">
                    <tr>
                        <td style="vertical-align:top">
                            <div style="position: relative; width: 100%; padding-top:26px">
                                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage5">
                                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="cfLogDate1" HeaderStyle-CssClass="hiddenColumn cfLogDate1" ItemStyle-CssClass="hiddenColumn cfLogDate1" />
                                                        <asp:BoundField HeaderText="TANGGAL"  DataField="cfLogDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="110px" ItemStyle-CssClass="cfLogDate" />
                                                        <asp:TemplateField HeaderText="INTAKE" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <div style="text-align: right; color: blue">
                                                                    <label class="lblIntakeSummary lblLink" visitID = '<%#:Eval("VisitID")%>' logDate = '<%#:Eval("cfLogDate")%>'>
                                                                        <%#:Eval("TotalIntake", "{0:N}")%></label>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="INTAKE TIDAK DIUKUR" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <div style="text-align: right; color: blue">
                                                                    <label class="lblIntake2Summary lblLink" visitID = '<%#:Eval("VisitID")%>' logDate = '<%#:Eval("cfLogDate")%>'>
                                                                        <%#:Eval("TotalIntake2", "{0:N}")%>
                                                                        </label>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="OUTPUT" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <div style="text-align: right; color: blue">
                                                                    <label class="lblOutputSummary lblLink" visitID = '<%#:Eval("VisitID")%>' logDate = '<%#:Eval("cfLogDate")%>'>
                                                                        <%#:Eval("TotalOutput1", "{0:N}")%></label>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="OUTPUT TIDAK DIUKUR" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <div style="text-align: right; color: blue">
                                                                    <label class="lblOutput2Summary lblLink" visitID = '<%#:Eval("VisitID")%>' logDate = '<%#:Eval("cfLogDate")%>'>
                                                                        <%#:Eval("TotalOutput2", "{0:N}")%></label>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="BALANCE" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <div <%# Eval("cfIsDehydration").ToString() == "False" ? "Style='text-align: right; color: blue'":"Style='text-align: right; color: red'" %>>
                                                                    <label class="lblBalance" visitID = '<%#:Eval("VisitID")%>' logDate = '<%#:Eval("cfLogDate")%>'>
                                                                        <%#:Eval("cfFluidBalance", "{0:N}")%></label>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Belum ada informasi intake-output untuk pasien ini")%>
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
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top">
                            <dxcp:ASPxCallbackPanel ID="cbpView6" runat="server" Width="100%" ClientInstanceName="cbpView6"
                                ShowLoadingPanel="false" OnCallback="cbpView6_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView6').show(); }"
                                    EndCallback="function(s,e){ onCbpView6EndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent7" runat="server">
                                        <asp:Panel runat="server" ID="Panel6" CssClass="pnlContainerGridPatientPage6">
                                            <asp:GridView ID="grdView6" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                    <asp:BoundField HeaderText="Tanggal"  DataField="cfIVTherapyNoteDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-CssClass="LogDate" />
                                                    <asp:BoundField HeaderText="Jam"  DataField="IVTherapyNoteTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-CssClass="LogTime" />
                                                    <asp:TemplateField HeaderText="Catatan Terapi Infus" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <div style="height: 50px; overflow-y: auto;">
                                                                <%#Eval("IVTherapyNotes").ToString().Replace("\n","<br />")%><br />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Tenaga Medis"  DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <div>
                                                        <div class="blink"><%=GetLabel("Belum ada informasi program infus untuk pasien ini") %></div>
                                                    </div>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>    
                            <div class="imgLoadingGrdView" id="containerImgLoadingView6" >
                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                            </div>
                            <div class="containerPaging">
                                <div class="wrapperPaging">
                                    <div id="paging6"></div>
                                </div>
                            </div> 
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table border="0" cellpadding="0" cellspacing="0" style="width:100%">   
                    <tr>
                        <td>
                            <div class="containerUlTabPage" style="margin-bottom: 3px;">
                                <ul class="ulTabPage" id="ulTabOrderDetail">
                                    <li class="selected" contentid="panIntake">
                                        <%=GetLabel("INTAKE")%></li>
                                    <li contentid="panIntake2">
                                        <%=GetLabel("INTAKE YANG TIDAK DIUKUR")%></li>
                                    <li contentid="panOutput1">
                                        <%=GetLabel("OUTPUT")%></li>
                                    <li contentid="panOutput2">
                                        <%=GetLabel("OUTPUT YANG TIDAK DIUKUR")%></li>
                                </ul>
                            </div>
                        </td>
                    </tr>     
                    <tr>
                        <td>
                            <div class="containerOrderDt" id="panIntake">
                                <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                    <colgroup>
                                        <col style="width:45%" />
                                        <col style="width:55%" />
                                    </colgroup>
                                    <tr>
                                        <td style="vertical-align:top">
                                            <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                                                ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                                    EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                                        <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:BoundField DataField="GCFluidType" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn gcFluidType" />
                                                                    <asp:BoundField DataField="FluidName" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn fluidName" />
                                                                    <asp:BoundField HeaderText="Jam"  DataField="LogTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-CssClass="LogTime" />
                                                                    <asp:TemplateField  HeaderStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <div style="font-weight:bold">Nama Cairan</div>
                                                                            <div>Jenis Cairan</div>
                                                                            <div style="font-style:italic">Tenaga Medis</div>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div style="font-weight:bold"><%#:Eval("FluidName") %></div>
                                                                            <div><%#:Eval("FluidType") %></div>
                                                                            <div style="font-style:italic"><%#:Eval("ParamedicName") %></div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="Jumlah Inisiasi"  DataField="FluidBalance" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="60px" />
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <div>
                                                                        <div class="blink"><%=GetLabel("Belum ada informasi intake pada tanggal ini") %></div>
                                                                    </div>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>    
                                            <div class="imgLoadingGrdView" id="containerImgLoadingViewDt" >
                                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                            </div>
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="pagingDt1"></div>
                                                </div>
                                            </div> 
                                        </td>
                                        <td style="vertical-align:top">
                                            <dxcp:ASPxCallbackPanel ID="cbpViewDt4" runat="server" Width="100%" ClientInstanceName="cbpViewDt4"
                                                ShowLoadingPanel="false" OnCallback="cbpViewDt4_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt4').show(); }"
                                                    EndCallback="function(s,e){ onCbpViewDt4EndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent3" runat="server">
                                                        <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGridPatientPage">
                                                            <asp:GridView ID="grdViewDt4" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:BoundField HeaderText="Jam"  DataField="LogTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-CssClass="LogTime" />
                                                                    <asp:BoundField HeaderText="Cairan Ada"  DataField="FluidBalance" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="70px" />
                                                                    <asp:BoundField HeaderText="Jumlah"  DataField="FluidAmount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="70px"/>
                                                                    <asp:BoundField HeaderText="Tenaga Medis"  DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <div>
                                                                        <div class="blink"><%=GetLabel("Belum ada informasi intake pada tanggal ini") %></div>
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
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="containerOrderDt" id="panIntake2" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt5" runat="server" Width="100%" ClientInstanceName="cbpViewDt5"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt5_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt5').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt5EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent5" runat="server">
                                            <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt5" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Jam"  DataField="LogTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-CssClass="LogTime" />
                                                        <asp:BoundField HeaderText="Tenaga Medis"  DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" />
                                                        <asp:BoundField HeaderText="Jenis"  DataField="FluidType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                                        <asp:BoundField HeaderText="Cairan"  DataField="FluidName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField HeaderText="Frekuensi"  DataField="Frequency" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px"/>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div class="blink"><%=GetLabel("Belum ada informasi intake yang tidak diukur pada tanggal ini") %></div>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>   
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt5" >
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt5"></div>
                                    </div>
                                </div> 
                            </div>
                            <div class="containerOrderDt" id="panOutput1" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt2" runat="server" Width="100%" ClientInstanceName="cbpViewDt2"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt2_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt2').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt2EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent4" runat="server">
                                            <asp:Panel runat="server" ID="Panel4" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt2" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField HeaderText="Jam"  DataField="LogTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-CssClass="LogTime" />
                                                        <asp:BoundField HeaderText="Tenaga Medis"  DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" />
                                                        <asp:BoundField HeaderText="Jenis Cairan"  DataField="FluidType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                                        <asp:BoundField HeaderText="Cairan"  DataField="FluidName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField HeaderText="Jumlah"  DataField="FluidAmount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px"/>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div class="blink"><%=GetLabel("Belum ada informasi output pada tanggal ini") %></div>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>   
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt2" >
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingDt2"></div>
                                    </div>
                                </div> 
                            </div>
                            <div class="containerOrderDt" id="panOutput2" style="display:none">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt3" runat="server" Width="100%" ClientInstanceName="cbpViewDt3"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt3_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt3').show(); }"
                                        EndCallback="function(s,e){ onCbpViewDt3EndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent6" runat="server">
                                            <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                                <asp:GridView ID="grdViewDt3" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Jam"  DataField="LogTime" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-CssClass="LogTime" />
                                                        <asp:BoundField HeaderText="Tenaga Medis"  DataField="ParamedicName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" />
                                                        <asp:BoundField HeaderText="Jenis"  DataField="FluidType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                                        <asp:BoundField HeaderText="Cairan"  DataField="FluidName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField HeaderText="Frekuensi"  DataField="Frequency" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px"/>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div>
                                                            <div class="blink"><%=GetLabel("Belum ada informasi output yang tidak diukur pada tanggal ini") %></div>
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
                        </td>
                    </tr>            
                </table>
            </td>
        </tr>
        </table>
    </div>
    <div id="contentDetailPage2" class="container contentDetail  w3-animate-top" style="height:600px;display:none">        
        <table>
            <colgroup>
                <col style="width: 125px" />
                <col style="width: 150px" />
                <col style="width: 10px" />
                <col style="width: 150px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal ")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" Width="120px" CssClass="datepicker" runat="server" />
                </td>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Jam ")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtFromTime" Width="80px" CssClass="time" runat="server" />
                </td>
                <td rowspan="2">
                    <input id="btnApplyFilter" runat="server" type="button" value='Calculate' style="width:140px;" class="w3-btn w3-hover-blue" />                
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <%=GetLabel("Tanggal ") %>
                </td>
                <td>
                    <asp:TextBox ID="txtToDate" Width="120px" CssClass="datepicker" runat="server" />
                </td>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Jam ")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtToTime" Width="80px" CssClass="time" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <table class = "w3-table">
                        <colgroup>
                            <col width="120px" />
                            <col width="120px" />
                            <col width="120px" />
                            <col width="120px" />
                            <col width="120px" />
                        </colgroup>
                        <tr>
                          <th style="text-align:center"><%=GetLabel("INTAKE ") %></th>
                          <th style="text-align:center"><%=GetLabel("INTAKE TIDAK DIUKUR ") %></th>
                          <th style="text-align:center"><%=GetLabel("OUTPUT ") %></th>
                          <th style="text-align:center"><%=GetLabel("OUTPUT YANG TIDAK DIUKUR ") %></th>
                          <th style="text-align:center"><%=GetLabel("BALANCE ") %></th>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtTotalIntake1" Width="120px" runat="server" class="number" ReadOnly="true" Text="0" />
                            </td>                
                            <td>
                                <asp:TextBox ID="txtTotalIntake2" Width="120px" runat="server" class="number" ReadOnly="true" Text="0" />
                            </td>                
                            <td>
                                <asp:TextBox ID="txtTotalOutput1" Width="120px" runat="server" class="number" ReadOnly="true" Text="0" />
                            </td>                
                            <td>
                                <asp:TextBox ID="txtTotalOutput2" Width="120px" runat="server" class="number" ReadOnly="true" Text="0" />
                            </td>      
                            <td>
                                <asp:TextBox ID="txtBalance" Width="120px" runat="server" class="number" ReadOnly="true" Text="0" />
                            </td>                                          
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div style="display:none">
        <dxcp:ASPxCallbackPanel ID="cbpCalculateBalanceSummary" runat="server" Width="100%" ClientInstanceName="cbpCalculateBalanceSummary"
            ShowLoadingPanel="false" OnCallback="cbpCalculateBalanceSummary_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpCalculateBalanceSummaryEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
