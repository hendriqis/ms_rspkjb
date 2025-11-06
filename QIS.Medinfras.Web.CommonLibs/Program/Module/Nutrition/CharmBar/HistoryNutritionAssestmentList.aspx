<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true"
    CodeBehind="HistoryNutritionAssestmentList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.HistoryNutritionAssestmentList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript">

        $(function () {
            setDatePicker('<%=txtFromDate.ClientID %>');
            ////// $('#<%=txtFromDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtFromDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtToDate.ClientID %>');
            /////$('#<%=txtToDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtToDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=txtFromDate.ClientID %>').change(function (evt) {
                $('#<%=hdnFirstSelected.ClientID %>').val('0');
                cbpViewDt.PerformCallback('refresh');
               
            });

            $('#<%=txtToDate.ClientID %>').change(function (evt) {
                $('#<%=hdnFirstSelected.ClientID %>').val('0');
                cbpViewDt.PerformCallback('refresh');
            });



            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $row = $(this).parent().closest('tr');
                    var entity = rowToObject($row);


                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnRegistrationNo.ClientID %>').val($(this).find('.keyField').html());
                    cbpViewDt.PerformCallback('refresh');
                }
            });


            $('#<%=grdViewDt.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdViewDt.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnCurrentUserID.ClientID %>').val($(this).find('.keyUser').html());
                $('#<%=hdnIsEditable.ClientID %>').val($(this).find('.hiddenColumn').html());
                $('#<%=hdnPatientNoteType.ClientID %>').val($(this).find('.controlColumn').html());

                var index = $('#<%=grdViewDt.ClientID %> tr').index(this);
                $('#<%=hdnLastIndexSelected.ClientID %>').val(index);
            });
           
            $('#<%=rblItemType.ClientID %> input').change(function () {
                cbpView.PerformCallback('refresh');
                cbpViewDt.PerformCallback('refresh');
            });

        });
        function onCboDisplay() {

        }
        function onCbpViewEndCallback(s) {
            $('#containerImgLoadingView').hide();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                   
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
           
            $('#<%=hdnFirstSelected.ClientID %>').val('1');

        }

        var pageCount = parseInt('<%=PageCount %>');
        var pageCount2 = parseInt('<%=PageCount2 %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });

            setPaging($("#pagingDt"), pageCount2, function (page) {
                cbpViewDt.PerformCallback('changepage|' + page);
            });
            
        });
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
             else {
                 $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
             }

             var FirstSelected = $('#<%=hdnFirstSelected.ClientID %>').val();
             if (FirstSelected == "1") {
                 var fromDate = $('#<%=hdnDateFrom.ClientID %>').val();
                 var toDate = $('#<%=hdnDateTo.ClientID %>').val();
                 $('#<%=txtFromDate.ClientID %>').val(fromDate);
                 $('#<%=txtToDate.ClientID %>').val(toDate);
             }


         }
          
         $('.btnView').live('click', function () {
             var id = $(this).closest('tr').find('.keyField').html();
             alert(id);
             var visitID = $(this).closest('tr').find('.visitID').html();
             var deptID = $(this).closest('tr').find('.departmentID').html();

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

             openUserControlPopup(url, visitID, 'Asesmen Perawat', 1300, 600);
         });

         $('#imgVisitNote.imgLink').live('click', function () {
             $(this).closest('tr').click();
             var id = $('#<%=hdnID.ClientID %>').val();
             var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/NursingNotes/VisitNotesHistoryCtl.ascx");
             openUserControlPopup(url, id, 'History Catatan Perawat', 900, 500);
         });
    </script>
        <style type="text/css">
        .keyUser
        {
            display: none;
        }
    </style>
     <input type="hidden" runat="server" id="hdnCurrentUserID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentSessionID" value="" />
    <input type="hidden" runat="server" id="hdnIsEditable" value="0" />
    <input type="hidden" runat="server" id="hdnPatientNoteType" value="0" />
    
    <input type="hidden" value="" id="hdnLastIndexSelected" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnID2" runat="server" />
    <input type="hidden" value="" id="hdnItemID" runat="server" />
    <input type="hidden" value="" id="hdnItemID2" runat="server" />
    <input type="hidden" value="" id="hdnReferenceNo" runat="server" />
    <input type="hidden" value="" id="hdnReferenceNo2" runat="server" />
    <input type="hidden" value="" id="hdnViewerUrl" runat="server" />
    <input type="hidden" value="" id="hdnDocumentPath" runat="server" />
    <input type="hidden" id="hdnMRN" runat="server" value="" />
    <input type="hidden" id="hdnVisitID" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationNo" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnFirstSelected" runat="server" value="1" />
 
     <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
      
        <tr>
            <td>
                <div class="containerOrderDt" id="panByTransactionNo">
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
                                                        <td class="tdLabel" style="width: 150px">
                                                            <label>
                                                                <%=GetLabel("Display Option")%></label>
                                                        </td>
                                                        <td>
                                                            <dxe:ASPxComboBox ID="cboDisplay" ClientInstanceName="cboDisplay" runat="server"
                                                                Width="300px">
                                                                <ClientSideEvents ValueChanged="function() { onCboDisplay(); }" />
                                                            </dxe:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                              
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Filter Kunjungan")%></label>
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
                                                                        <asp:BoundField DataField="RegistrationNo" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                            <HeaderTemplate>
                                                                                <%=GetLabel("Visit Information")%>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div><%#: Eval("VisitDateInString")%>, <%#: Eval("RegistrationNo")%></div>
                                                                                <div><%#: Eval("ServiceUnitName")%></div>
                                                                                <div><%#: Eval("ParamedicName")%></div>
                                                                                <div><%#: Eval("DisplayPatientDiagnosis")%></div>

                                                                                <input type="hidden" value="<%#:Eval("VisitDate") %>" bindingfield="VisitDate" />
                                                                    
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <EmptyDataTemplate>
                                                                        <%=GetLabel("Tidak ada data pemeriksaan untuk pasien ini")%>
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
                                <div style="position: relative; padding-top: 25px">
                                  <table>
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
                                               
                                            </tr>
                                            </table>
                                    <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                                        ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                            EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent2" runat="server">
                                            <input type="hidden" value="" runat="server" id="hdnDateFrom" />
                                            <input type="hidden" value="" runat="server" id="hdnDateTo" />

                                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                                    <asp:GridView ID="grdViewDt" runat="server" CssClass="grdLabResultDt grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                         OnRowDataBound="grdViewDt_RowDataBound">
                                                        <Columns>
                                                           <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn visitID" />
                                <asp:BoundField DataField="DepartmentID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn departmentID" />
                                <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn paramedicID" />
                                <asp:BoundField DataField="ParamedicName" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn paramedicName" />
                                <asp:BoundField DataField="GCPatientNoteType" HeaderStyle-CssClass="controlColumn"
                                    ItemStyle-CssClass="controlColumn" />
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
                                                            <img class="imgLink" id="imgVisitNote" src='<%= ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>'
                                                                alt="" title="<%=GetLabel("Catatan Pasien")%>" width="32" height="32" />
                                                    </span>
                                        </div>
                                        <div style="height: auto; max-height:150px; overflow-y: auto; margin-top: 15px;">
                                            <%#Eval("NoteText").ToString().Replace("\n","<br />")%><br />
                                        </div>
                                        <div id="divView" runat="server" style='margin-top: 5px;'>
                                            <input type="button" id="btnView" runat="server" class="btnView w3-btn w3-hover-blue" value="Detail Kajian Awal"
                                                style='width: 150px; background-color: Green; color: White;'  />
                                        </div>
                                        <div style="margin-top: 10px; text-align: left">
                                            <table border="0" cellpadding="1" cellspacing="0">
                                               <tr id="divNursingNotesInfo" runat="server" >
                                                    <td>
                                                       <asp:CheckBox ID="chkIsWritten" runat="server" Enabled="false" Checked='<%# Eval("IsWrite")%>' /> TULIS
                                                       <asp:CheckBox ID="chkIsReadback" runat="server" Enabled="false" Checked='<%# Eval("IsReadback")%>'/> BACA
                                                       <asp:CheckBox ID="chkIsConfirmed" runat="server" Enabled="false" Checked='<%# Eval("IsConfirmed")%>'/> KONFIRMASI
                                                    </td>
                                                </tr>
                                                <tr id="divConfirmationInfo" runat="server" >
                                                    <td>
                                                        <div style="white-space: normal; overflow-y: auto; font-weight: bold;overflow-x: hidden">
                                                            <span style='color: red;'>Konfirmasi : </span>
                                                            <span style='color: Blue;'>                                                    
                                                                <%#:Eval("cfConfirmationDateTime") %>, <%#:Eval("ConfirmationPhysicianName") %></span>
                                                                <div id="divConfirmationRemarks">
                                                                    <br />
                                                                    <textarea style="border: 0; width: 99%; height: auto; background-color: transparent; font-style:italic;overflow-x: hidden"
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
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Instruksi" HeaderStyle-Width="350px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div>
                                            <textarea style="padding-left: 10px; border: 0; width: 99%; height: 200px; max-height:250px; background-color: transparent"
                                                readonly><%#: DataBinder.Eval(Container.DataItem, "InstructionText") %> </textarea>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-Width="150px">

                                    
                                    <ItemTemplate>
                                        <div style="color: blue; font-style: italic; vertical-align: top; display:none">
                                            <%#:Eval("cfCreatedDate") %>,
                                        </div>
                                        <div>
                                            <b><label id="lblParamedicName" class='<%# Eval("cfIsHasSignature1").ToString() == "True" ? "lblLink lblParamedicName": "lblNormal" %>'>
                                                <%#:Eval("cfCreatedByName") %></label></b>                                            
                                        </div>
                                       
                                        <div>
                                            <img class="imgNeedNotification" src='<%# ResolveUrl("~/Libs/Images/Status/warning.png")%>'
                                                alt="" style='<%# Eval("IsNeedNotification").ToString() == "False" ? "display:none;": "" %> max-width:30px;
                                                cursor: pointer; min-width: 30px; float: left;' title="Using Notification" />
                                        </div>
                                        <div style='<%# Eval("IsNeedNotification").ToString() == "False" ? "display:none;": "" %>'>
                                            <br />
                                            <br />
                                            <span style='color: Blue;'>
                                                <%#:Eval("cfNotificationConfirmedDateTime") %>
                                                <br />
                                                <%#:Eval("NotificationUserName") %></span>
                                        </div>
                                        <div id="divVerified" align="center" style='<%# Eval("IsVerified").ToString() == "True" ? "display:none;": "" %>'>
                                            <br />
                                            <div>
                                                <input type="button" id="btnVerify" runat="server" class="btnVerify" value="VERIFY"
                                                    style="height: 25px; width: 100px; background-color: Red; color: White;" />
                                            </div>
                                        </div>
                                        <div id="divVerifiedInformation" runat="server" style="margin-top: 10px; text-align: left">
                                            <div style="white-space: normal; overflow: auto; font-weight: bold">
                                                <span style='color: red;'>Verifikasi :</span>
                                                <br />
                                                <span style='color: Blue;'>
                                                    <%#:Eval("cfVerifiedPrimaryNurseDateTime") %>
                                                    <br />
                                                    <%#:Eval("VerifiedPrimaryNurseName") %></span>
                                            </div>
                                        </div>
                                        <div id="divPhysicianVerifiedInformation" runat="server" style="margin-top: 10px; text-align: left">
                                            <div style="white-space: normal; overflow: auto; font-weight: bold">
                                                <span style='color: red;'>Verified :</span>
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
                                                            <span class="blink">
                                                                <%=GetLabel("Belum ada informasi yang ditampilkan saat ini") %></span>
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
                
            </td>
        </tr>
    </table>
</asp:Content>
