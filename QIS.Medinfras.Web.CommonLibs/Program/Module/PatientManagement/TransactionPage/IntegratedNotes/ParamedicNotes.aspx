<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="ParamedicNotes.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ParamedicNotes" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <table cellpadding="0" cellspacing="0" style="width: 100%">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 145px" />
            <col style="width: 10px" />
            <col style="width: 145px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal" style="font-weight: bold">
                    <%=GetLabel("Tanggal ")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtFromDate" Width="120px" CssClass="datepicker" runat="server" />
            </td>
            <td style="text-align: center">
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
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erevaluationnotes">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnCurrentUserID.ClientID %>').val($(this).find('.keyUser').html());
                $('#<%=hdnIsEditable.ClientID %>').val($(this).find('.hiddenColumn').html());
                $('#<%=hdnPatientNoteType.ClientID %>').val($(this).find('.controlColumn').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            //#region Signature
            $('.btnSignature').live('click', function () {
                $tr = $(this).closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                var ppa = $tr.find('.paramedicName').html();
                var noteDate = $tr.find('.cfNoteDate').html();
                var noteTime = $tr.find('.noteTime').html();
                var data = ($('#<%=hdnID.ClientID %>').val() + "|" + ppa + "|" + noteDate + "|" + noteTime + "|" + "1");
                var url = ResolveUrl("~/Libs/Controls/DigitalSignatureCtl.ascx");
                openUserControlPopup(url, data, 'Tanda Tangan Digital', 400, 500);
            });

            $('.lblParamedicName').live('click', function () {
                $tr = $(this).closest('tr');
                $('#<%=hdnID.ClientID %>').val($tr.find('.keyField').html());
                var ppa = $tr.find('.paramedicName').html();
                var noteDate = $tr.find('.cfNoteDate').html();
                var noteTime = $tr.find('.noteTime').html();
                var signature1 = $tr.find('.signature1').html();
                var signature2 = $tr.find('.signature2').html();
                var signature3 = $tr.find('.signature3').html();
                var signature4 = $tr.find('.signature4').html();
                var signatureData = signature1 + "|" + signature2 + "|" + signature3 + "|" + signature4;
                var data = ($('#<%=hdnID.ClientID %>').val() + "|" + ppa + "|" + noteDate + "|" + noteTime + "|" + "1" + "|" + signatureData);
                var url = ResolveUrl("~/Libs/Controls/ViewDigitalSignatureCtl.ascx");
                openUserControlPopup(url, data, 'Tanda Tangan Digital', 400, 450);
            });
            //#endregion

            $('.btnView').live('click', function () {
                var id = $(this).closest('tr').find('.keyField').html();
                var visitID = $(this).closest('tr').find('.visitID').html();
                var deptID = $(this).closest('tr').find('.departmentID').html();
                var paramedicMasterType = $(this).attr("paramedicMasterType");
                var chiefComplaintID = $(this).attr("ChiefComplaintID");
                var nurseChiefComplaintID = $(this).attr("NurseChiefComplaintID");
                var param = visitID + "|" + id;


                if (paramedicMasterType == Constant.ParamedicType.Physician) {
                    if (deptID == Constant.Facility.EMERGENCY) {
                        var url = ResolveUrl("~/libs/Controls/EMR/Physician/Assessment/ERPhysicianInitialAssessmentCtl1.ascx");
                    }
                    else if (deptID == Constant.Facility.INPATIENT) {
                        var url = ResolveUrl("~/libs/Controls/EMR/Physician/Assessment/IPPhysicianInitialAssessmentCtl1.ascx");
                    }
                    else if (deptID == Constant.Facility.OUTPATIENT) {
                        var url = ResolveUrl("~/libs/Controls/EMR/Physician/Assessment/OPPhysicianInitialAssessmentCtl1.ascx");
                    }
                    else {
                        var url = ResolveUrl("~/libs/Controls/EMR/Physician/Assessment/MDPhysicianInitialAssessmentCtl1.ascx");
                    }
                    param = visitID + "|" + id + "|" + chiefComplaintID;
                }
                else {
                    if (deptID == Constant.Facility.EMERGENCY) {
                        var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/ERNurseInitialAssessmentCtl1.ascx");
                    }
                    else if (deptID == Constant.Facility.INPATIENT) {
                        var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/IPNurseInitialAssessmentCtl1.ascx");
                    }
                    else if (deptID == Constant.Facility.OUTPATIENT) {
                        var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/OPNurseInitialAssessmentCtl1.ascx");
                    }
                    else {
                        var url = ResolveUrl("~/libs/Controls/EMR/Nursing/Assessment/MDNurseInitialAssessmentCtl1.ascx");
                    }
                }

                openUserControlPopup(url, param, 'Detail Kajian', 1300, 600);
            });

            $('.btnViewDetail').live('click', function () {
                var id = $(this).closest('tr').find('.keyField').html();
                var visitID = $(this).closest('tr').find('.visitID').html();
                var testOrderID = "";
                var isPostSurgeryInstruction = $(this).attr("isPostSurgeryInstruction");
                var postSurgeryInstructionID = $(this).attr("postSurgeryInstructionID");
                var param = visitID + "|" + id;
                var url = "";
                var title = "Detail Catatan";

                if (isPostSurgeryInstruction == "True") {
                    param = visitID + "|" + testOrderID + "|" + postSurgeryInstructionID;
                    url = ResolveUrl("~/libs/Controls/EMR/Surgery/ViewPostSurgeryInstructionCtl1.ascx");
                    title = "Instruksi Paska Bedah";
                }
                else {
                    param = visitID + "|" + testOrderID + "|" + postSurgeryInstructionID;
                    url = ResolveUrl("~/libs/Controls/EMR/Surgery/ViewPostSurgeryInstructionCtl1.ascx");
                    title = "Instruksi Paska Bedah";
                }
                openUserControlPopup(url, param, title, 1300, 600);
            });

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
        function onBeforeBasePatientPageListEdit() {
            if ($('#<%=hdnPatientNoteType.ClientID %>').val() != Constant.SOAPNoteType.NUTRITION_ASSESSMENT) {
                if ($('#<%=hdnCurrentUserID.ClientID %>').val() == $('#<%=hdnCurrentSessionID.ClientID %>').val()) {
                    if ($('#<%=hdnIsEditable.ClientID %>').val() == "False") {
                        displayErrorMessageBox('Catatan Terintegrasi', 'Maaf, Catatan sudah dikonfirmasi (Readback) atau diverifikasi oleh Dokter, tidak bisa diubah lagi.');
                        return false;
                    }
                    else
                        return true;
                }
                else {
                    displayErrorMessageBox('Catatan Terintegrasi', 'Maaf, tidak diijinkan mengedit catatan user lain.');
                    return false;
                }
            }
            else {
                displayErrorMessageBox('Catatan Terintegrasi', 'Maaf, Catatan Assessment Gizi tidak bisa diubah melalui menu ini');
                return false;             
            }
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        $('#imgNotesHistory.imgLink').live('click', function () {
            $(this).closest('tr').click();
            var id = $('#<%=hdnID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/IntegratedNotes/IntegratedNotesHistoryCtl.ascx");
            openUserControlPopup(url, id, 'History Catatan Perawat', 900, 500);
        });

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

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var hdnID = $('#<%=hdnID.ClientID %>').val();
            if (hdnID == '' || hdnID == '0') {
                errMessage.text = 'Pasien tidak memiliki Catatan Perawat';
                return false;
            }
            else {
                if (code == 'PH-00027') {
                    filterExpression.text = visitID;
                    return true;
                }
                else 
                {
                    filterExpression.text = 'VisitID = ' + visitID;
                    return true;
                }
            }
        }

    </script>
    <style type="text/css">
        .keyUser
        {
            display:none;
        }
    </style>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnVisitID" runat="server" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" id="hdnSubMenuType" runat="server" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentUserID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentSessionID" value="" />
    <input type="hidden" runat="server" id="hdnIsEditable" value="0" />
    <input type="hidden" runat="server" id="hdnPatientNoteType" value="0" />
    <div style="position: relative;">
        <div id="filterArea">
            <table style="margin-top:10px; margin-bottom:10px">
                <tr>
                    <td class="tdLabel" style="width:150px">
                        <label>
                            <%=GetLabel("Tampilkan")%></label>
                    </td>
                    <td>
                        <dxe:ASPxComboBox ID="cboDisplay" ClientInstanceName="cboDisplay" runat="server"
                            Width="300px">
                            <ClientSideEvents ValueChanged="function() { cbpView.PerformCallback('refresh'); }" />
                        </dxe:ASPxComboBox>
                    </td>
                </tr>
            </table>
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty" ShowHeader="true"  OnRowDataBound="grdView_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn visitID" />
                                <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn paramedicID" />
                                <asp:BoundField DataField="ParamedicName" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn paramedicName" />
                                <asp:BoundField DataField="GCPatientNoteType" HeaderStyle-CssClass="controlColumn" ItemStyle-CssClass="controlColumn" />
                                <asp:BoundField DataField="CreatedBy" HeaderStyle-CssClass="keyUser" ItemStyle-CssClass="keyUser" />
                                <asp:BoundField DataField="cfNoteDate" HeaderText="Tanggal " HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cfNoteDate" />
                                <asp:BoundField DataField="NoteTime" HeaderText="Jam " HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="noteTime" />
                                <asp:TemplateField HeaderText="PPA" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div>
                                            <%#:Eval("cfPPA") %>
                                        </div>
                                        <div>
                                            <img class="imgNeedConfirmation" src='<%# ResolveUrl("~/Libs/Images/Status/warning.png")%>'
                                                alt="" style='<%# Eval("cfIsShowWarningIcon").ToString() == "False" ? "display:none;": "" %> max-width:30px;
                                                cursor: pointer; min-width: 30px;' title="Need confirmation" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SOAP" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div>
                                            <span style="color: blue; font-style: italic; vertical-align: top">
                                                <%#:Eval("ParamedicName") %>
                                                - <b>
                                                    <%#:Eval("DepartmentID") %>
                                                    (<%#:Eval("ServiceUnitName") %>)
                                                    <%#:Eval("cfParamedicMasterType") %>
                                                    <span style="float: right; <%# Eval("IsEdited").ToString() == "False" ? "display:none": "" %>">
                                                            <img class="imgLink" id="imgNotesHistory" src='<%= ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>'
                                                                alt="" title="<%=GetLabel("Catatan Pasien")%>" width="32" height="32" />
                                                    </span>
                                        </div>
                                        <div style="height: 130px; overflow-y: auto; margin-top: 15px;">
                                            <%#Eval("NoteText").ToString().Replace("\n","<br />")%><br />
                                        </div>
                                        <div id="divView" runat="server" style='margin-top: 5px;'>
                                            <input type="button" id="btnView" runat="server" class="btnView w3-btn w3-hover-blue" value="Detail Kajian Awal"
                                                style='width: 150px; background-color: Green; color: White;' recordID = '<%#:Eval("ID") %>' paramedicMasterType = '<%#:Eval("GCParamedicMasterType") %>' 
                                                chiefComplaintID = '<%#Eval("ChiefComplaintID") %>' nurseChiefComplaintID = '<%#Eval("NurseChiefComplaintID") %>' />
                                        </div>
                                        <div id="divViewDetail" runat="server" style='margin-top: 5px;'>
                                            <input type="button" id="btnViewDetail" runat="server" class="btnViewDetail w3-btn w3-hover-blue" value="Lihat Detail"
                                                style='width: 150px; background-color: Green; color: White;' recordID = '<%#:Eval("ID") %>' isPostSurgeryInstruction = '<%#Eval("cfIsPostSurgeryInstruction") %>' postSurgeryInstructionID = '<%#Eval("PostSurgeryInstructionID") %>'  />
                                        </div>
                                        <div id="divConfirmationInfo" runat="server" style="margin-top: 10px; text-align: left">
                                            <table border="0" cellpadding="1" cellspacing="0">
                                               <tr>
                                                    <td>
                                                       <asp:CheckBox ID="chkIsWritten" runat="server" Enabled="false" Checked='<%# Eval("IsWrite")%>' /> TULIS
                                                    </td>
                                                    <td>
                                                       <asp:CheckBox ID="chkIsReadback" runat="server" Enabled="false" Checked='<%# Eval("IsReadback")%>'/> BACA
                                                    </td>
                                                    <td>
                                                       <asp:CheckBox ID="chkIsConfirmed" runat="server" Enabled="false" Checked='<%# Eval("IsConfirmed")%>'/> KONFIRMASI
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <div style="white-space: normal; overflow: auto; font-weight: bold">
                                                            <span style='color: red;'>Konfirmasi : </span>
                                                            <span style='color: Blue;'>                                                    
                                                                <%#:Eval("cfConfirmationDateTime") %>, <%#:Eval("ConfirmationPhysicianName") %></span>
                                                                <div id="divConfirmationRemarks">
                                                                    <br />
                                                                    <textarea style="border: 0; width: 99%; height: auto; background-color: transparent; font-style:italic "
                                                                        readonly><%#: DataBinder.Eval(Container.DataItem, "ConfirmationRemarks") %> </textarea>
                                                                </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="float: right; <%# Eval("IsPanicValueReporting").ToString() == "False" ? "display:none": "" %>">
                                                            <asp:CheckBox ID="chkIsPanicValueReporting" runat="server" Enabled="false" Checked='<%# Eval("IsPanicValueReporting")%>' /> <span style="color:Red">Pelaporan Nilai Kritis</span>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="float: right; <%# Eval("IsRMOHandsover").ToString() == "False" ? "display:none": "" %>">
                                                            <asp:CheckBox ID="chkIsRMOHandsover" runat="server" Enabled="false" Checked='<%# Eval("IsRMOHandsover")%>' /> Catatan Hand over Dokter Jaga
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style='<%# Eval("IsNeedNotification").ToString() == "False" ? "display:none;": "" %>'>
                                            <br />
                                            <br />
                                            <span style='color: red;'>Konfirmasi Notifikasi : </span>
                                            <br />
                                            <span style='color: Blue;'>
                                                <%#:Eval("cfNotificationConfirmedDateTime") %>
                                                <br />
                                                <%#:Eval("NotificationUserName") %></span>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Instruksi" HeaderStyle-Width="350px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div>
                                            <textarea style="padding-left: 10px; border: 0; width: 99%; height: 200px; background-color: transparent"
                                                readonly><%#: DataBinder.Eval(Container.DataItem, "InstructionText") %> </textarea>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                    <ItemTemplate>
                                        <div style="color: blue; font-style: italic; vertical-align: top; display:none">
                                            <%#:Eval("cfCreatedDate") %>,
                                        </div>
                                        <div>
                                            <b><label id="lblParamedicName" class='<%# Eval("cfIsHasSignature1").ToString() == "True" ? "lblLink lblParamedicName": "lblNormal" %>'>
                                                <%#:Eval("cfCreatedByName") %></label></b>                                            
                                        </div>
                                        <div id="divParamedicSignature" runat="server" style='margin-top: 5px; text-align: left'>
                                            <input type="button" id="btnSignature" runat="server" class="btnSignature" value="Ttd" title="Tanda Tangan"
                                                style='<%# Eval("cfIsHasSignature1").ToString() == "True" ? "display:none;": "height: 25px; width: 60px; background-color: Red; color: White;" %>' />
                                        </div>
                                        <div><img class="imgNeedNotification" src='<%# ResolveUrl("~/Libs/Images/Status/warning.png")%>' alt="" style='<%# Eval("IsNeedNotification").ToString() == "False" ? "display:none;": "" %> max-width:30px; cursor:pointer; min-width: 30px; float: left;' title="Using Notification" /></div>
                                        <div id="divPhysicianVerifiedInformation" runat="server" style="margin-top: 10px; text-align: left">
                                            <div style="white-space: normal; overflow: auto; font-weight: bold">
                                                <span style='color: red;'>Verifikasi :</span>
                                                <br />
                                                <span style='color: Blue;'>
                                                    <%#:Eval("cfVerifiedDateTime") %>, <%#:Eval("VerifiedPhysicianName") %></span>
                                                    <div id="divVerificationRemarks">
                                                        <br />
                                                        <textarea style="border: 0; width: 99%; height: auto; background-color: transparent; font-style:italic "
                                                            readonly><%#: DataBinder.Eval(Container.DataItem, "VerificationRemarks") %> </textarea>
                                                    </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Signature1" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn signature1" />
                                <asp:BoundField DataField="Signature2" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn signature2" />
                                <asp:BoundField DataField="Signature3" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn signature3" />
                                <asp:BoundField DataField="Signature4" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn signature4" />
                                <asp:BoundField DataField="cfIsEditable" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada catatan tenaga medis untuk pasien ini") %>
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
