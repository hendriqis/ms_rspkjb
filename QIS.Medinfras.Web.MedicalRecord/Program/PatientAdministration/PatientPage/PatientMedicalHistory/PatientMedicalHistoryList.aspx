<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientDataPageList.master"
    AutoEventWireup="true" CodeBehind="PatientMedicalHistoryList.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.PatientMedicalHistoryList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnViewEpisodeSummary" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("View Rekam Medis")%></div>
    </li>
    <li id="btnGenerateQR" runat="server" crudmode="R" style="display: none">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Generate QR")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery-ui.js")%>' type='text/javascript'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');
            setRightPanelButtonEnabled();
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnRegistrationID.ClientID %>').val($(this).find('.registrationID').html());
                $('#<%=hdnDepartmentID.ClientID %>').val($(this).find('.departmentID').html());
                setRightPanelButtonEnabled();
            });

            $('#<%=btnGenerateQR.ClientID %>').live('click', function () {
                var url = ResolveUrl("~/Program/PatientAdministration/PatientPage/PatientMedicalHistory/ShowQRCode.ascx");
                openUserControlPopup(url, "", 'QR Code', 600, 600);
            });

            $('#<%=btnViewEpisodeSummary.ClientID %>').live('click', function () {
                var id = $('#<%=hdnID.ClientID %>').val();
                if (id != '') {
                    var url = ResolveUrl("~/Libs/Controls/EMR/MedicalSummary/MedicalSummaryCtl.ascx");
                    openUserControlPopup(url, id, 'Ringkasan Perawatan', 1300, 600);
                }
            });
        });

        function onRefreshControl(filterExpression) {
            cbpView.PerformCallback('refresh');
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

        function setRightPanelButtonEnabled() {
            var id = $('#<%=hdnID.ClientID %>').val();
            if (id != '') {
                if ($('#<%=hdnDepartmentID.ClientID %>').val() != 'INPATIENT') {
                    $('#btninfoPatientMutation').attr('enabled', 'false');
                }
                else {
                    $('#btninfoPatientMutation').removeAttr('enabled');
                }
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var id = $('#<%=hdnID.ClientID %>').val();
            if (code == 'MR000017' || code == 'MR000023' || code == 'MR000039' || code == 'PM-90008' || code == 'PM-90009' ||
                code == 'PM-90010' || code == 'PM-90011' || code == 'PM-90012' || code == 'PM-90013' || code == 'PM-90014' ||
                code == 'PM-90015' || code == 'PM-90019' || code == 'PM-00575' || code == 'PM-90020' || code == 'PM-90022' ||
                code == 'PM-90023' || code == 'PM-90035' || code == 'PM-90040' || code == 'PM-90017' || code == 'MR000049' ||
                code == 'MR000009' || code == 'MR000050' || code == 'MR000042' || code == 'MR000058' || code == 'PM-90085' || 
                code == 'PM-90114') {
                flagHasMR = true;
                filterExpression.text = id;
                if (code == 'PM-90008' || code == 'PM-90009' || code == 'PM-90010' || code == 'PM-90011' || code == 'PM-90012' ||
                    code == 'PM-90013' || code == 'PM-90014' || code == 'PM-90015' || code == 'PM-90019' || code == 'PM-00575' ||
                    code == 'PM-90020' || code == 'PM-90022' || code == 'PM-90023' || code == 'PM-90035' || code == 'PM-90040' ||
                    code == 'PM-90017' || code == 'MR000009' || code == 'MR000050' || code == 'MR000042' || code == 'MR000058' ||
                    code == 'PM-90085' || code == 'PM-90114') {
                    var filter2 = 'VisitID = ' + id + ' AND IsDeleted = 0';
                    Methods.getObject('GetvMedicalResumeList', filter2, function (result2) {
                        if (result2 != null) {
                            filterExpression.text = id;
                            flagHasMR = true;
                        }
                        else {
                            errMessage.text = 'Kunjungan ini tidak memiliki Resume Medis';
                            flagHasMR = false;
                        }
                    });
                }
                return flagHasMR;
            }

            if (code == 'MR000007' || code == 'MR000005' || code == 'MR000025' || code == 'MR000021' || code == 'MR000046' || code == 'MR000043') {
                filterExpression.text = "VisitID = " + id;
                return true;
            }
            else if (code == 'PM-00103' || code == 'PM-00429' || code == 'MR000016' || code == 'PM-00523'
                || code == 'PM-00524' || code == 'PM-00546' || code == 'PM-00565' || code == 'PM-00117'
                || code == 'PM-00650' || code == 'MR000041' || code == 'PM-00689' || code == 'MR000048'
                || code == 'PM-90073') {
                var filter = 'VisitID = ' + id;
                Methods.getObject('GetvRegistration5List', filter, function (result) {
                    if (result != null) {
                        filterExpression.text = result.RegistrationID;
                        return true;
                    }
                });
            }
            else if (code == 'PM-00146' || code == 'PM-00147' || code == 'PM-00674' || code == 'PM-00691' || code == 'PM-00692') {
                filterExpression.text = 'VisitID = ' + id;
                return true;
            }
            else {
                filterExpression.text = id;
                return true;
            }
            return true;
        }

        function onBeforeLoadRightPanelContent(code) {
            var id = $('#<%=hdnID.ClientID %>').val();
            var registrationID;
            var filter = 'visitID = ' + id;
            Methods.getObject('GetConsultVisitList', filter, function (result) {
                if (result != null) {
                    registrationID = result.RegistrationID;
                }
            });

            if (code == 'keteranganIstirahat1' || code == 'keteranganKematian' || code == 'keteranganKematian1' || code == 'infoPatientMutation' 
                || code == 'keteranganKematianRSBL') {
                return id;
            }
            else if (code == 'sendToSatuSEHAT') {
                if ($('#<%:hdnID.ClientID %>').val() != '' || $('#<%:hdnID.ClientID %>').val() != '0') {
                    var messageType = "04";
                    var visitID = $('#<%:hdnID.ClientID %>').val();
                    var referenceID = visitID;
                    var param = messageType + "|" + visitID + "|" + referenceID;
                    return param;
                }
            }
            else {
                return registrationID;
            }
        }
        //#region Button Laporan Operasi
        $('.btnSurgeryReportList').live('click', function () {
            var visitID = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/PatientAdministration/PatientPage/PatientMedicalHistory/SurgeryReportListCtl.ascx");

            openUserControlPopup(url, visitID, 'Laporan Operasi', 1300, 600);
        });   									
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnVisitID" runat="server" />
    <input type="hidden" id="hdnRegistrationID" runat="server" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="overflow-y: scroll;">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" RowStyle-CssClass="trDraggable" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="RegistrationID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="registrationID hiddenColumn" />
                                <asp:BoundField DataField="DepartmentID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="departmentID hiddenColumn" />
                                <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <%=GetLabel("Informasi Pendaftaran")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <b>
                                                <%#: Eval("RegistrationNo")%></b></div>
                                        <div>
                                            <i>
                                                <%#: Eval("BusinessPartnerName")%></i></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="350px" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <%=GetLabel("Informasi Kunjungan")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="color: blue">
                                            <%=GetLabel("Masuk : ")%><%#: Eval("VisitDateInString")%>,
                                            <%#: Eval("VisitTime")%></div>
                                        <div style="color: red">
                                            <%=GetLabel("Pulang : ")%><%#: Eval("cfDischargedInfo")%></div>
                                        <div style="color: red">
                                            <%=GetLabel("Disposisi Dokter : ")%><%#: Eval("cfPhysicianDischargedInfo")%></div>
                                        <div>
                                        <div>
                                            <b>
                                                <%#: Eval("ServiceUnitName")%></b> -
                                            <%#: Eval("ParamedicName")%></div>
                                        <div>
                                            <%=GetLabel("Ruang : ")%><%#: Eval("RoomCode")%></div>
                                        <div>
                                            <%=GetLabel("Kamar : ")%><%#: Eval("BedCode")%></div>
                                        <div>
                                            <%#: Eval("cNoSEP")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="220px" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <%=GetLabel("Informasi Cara Keluar")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <%=GetLabel("Kondisi Keluar : ")%><%#: Eval("DischargeCondition")%></div>
                                        <div>
                                            <%=GetLabel("Cara Keluar : ")%><%#: Eval("DischargeMethod")%></div>
                                        <div>
                                            <%#: Eval("cfDisplayReferralTo")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <%=GetLabel("Diagnosis")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <i>
                                                <%=GetLabel("Dokter : ")%></i></div>
                                        <div>
                                            <%#: Eval("cfDisplayDiagnosisDate")%></div>
                                        <div>
                                            <%#: Eval("cfDisplayPatientDiagnosisText")%></div>
                                        <br />
                                        <div>
                                            <i>
                                                <%=GetLabel("Rekam Medis : ")%></i></div>
                                        <div>
                                            <%#: Eval("cfDisplayFinalDiagnosisDate")%></div>
                                        <div>
                                            <%#: Eval("cfDisplayPatientDiagnosisFinalText")%></div>
                                        <br />
                                        <div>
                                            <i>
                                                <%=GetLabel("Klaim : ")%></i></div>
                                        <div>
                                            <%#: Eval("cfDisplayClaimDiagnosisDate")%></div>
                                        <div>
                                            <%#: Eval("cfDisplayPatientDiagnosisClaimText")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="VisitStatus" HeaderText="Status" HeaderStyle-Width="80px"
                                    HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" />
                                <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <%=GetLabel("Dibuat Oleh")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <%#: Eval("cfCreatedDate")%></div>
                                        <div>
                                            <%#: Eval("CreatedByName")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <%=GetLabel("Diubah Oleh")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <%#: Eval("cfLastUpdatedDate")%></div>
                                        <div>
                                            <%#: Eval("LastUpdatedByName")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <%=GetLabel("Laporan Operasi")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <img class="btnSurgeryReportList imgLink" title="<%=GetLabel("View") %>" src='<%# ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>'
                                            alt="" width="30px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Patient Medical History To Display")%>
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
</asp:Content>
