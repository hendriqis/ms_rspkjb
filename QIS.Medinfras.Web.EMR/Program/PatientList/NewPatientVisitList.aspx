<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master"
    AutoEventWireup="true" CodeBehind="NewPatientVisitList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.NewPatientVisitList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <link rel="stylesheet" href='<%= ResolveUrl("~/Libs/Styles/bootstrap/bootstrap-grid.css")%>' type="text/css" />
     <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
   
    <style>
        #leftPageNavPanel{height:auto !important;}
    </style>
    
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnPatientPageByDepartment" runat="server" value="0" />
    <input type="hidden" id="hdnIsSetByUserConfig" runat="server" value="0" />
    <input type="hidden" id="hdnIsGoToPatientPage" runat="server" value="0" />
    <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnPhysicianPatientCall" value="" runat="server" />
    <input type="hidden" id="hdnIsBridgingWithMedinlink" value="" runat="server" />
    <input type="hidden" value="" id="hdnHsuID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedDepartment" runat="server" />
    <div style="padding: 15px">
    <div class="row">
        <div class="col-md-2">
        <div id="leftPageNavPanel" class="w3-border">
                    <ul>
                    <asp:Repeater ID="RepterDepartement" runat="server">  
                <ItemTemplate>  
                    <li class="nav-item" contentid="<%#Eval("DepartmentID") %>">
                        <a href="#" contentid="<%#Eval("DepartmentID") %>"><%#Eval("DepartmentName") %></a>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
                        
                    </ul>
                </div>
        </div>
        <div class="col-md-4">
         <dxcp:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%" ClientInstanceName="cbpView"
                                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent3" runat="server">
                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridProcessList">
                                                <input type="hidden" value="" id="Hidden1" runat="server" />
                                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="HealthcareServiceUnitID" HeaderStyle-CssClass="keyField"
                                                            ItemStyle-CssClass="keyField" />
                                                        <asp:BoundField DataField="ServiceUnitName" HeaderStyle-HorizontalAlign="Left" HeaderText="Unit Pelayanan"
                                                            HeaderStyle-Width="250px" />
                                                        <asp:BoundField DataField="Jumlah" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                            HeaderText="Total" HeaderStyle-Width="20px" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("Tidak ada kunjungan pasien")%>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
        </div>
        <div class="col-md-6">
        
        
     
        <div id="containerDaftar" class="containerOrder">
            <div class="pageTitle">
                <%=GetLabel("My Patient")%></div>
            <fieldset id="fsPatientListReg">
                <table class="tblContentArea" style="width: 100%">
                    <tr>
                        <td style="padding: 5px; vertical-align: top">
                            <table class="tblEntryContent" style="width: 60%;">
                                
                                <tr id="trRegistrationDate">
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Tanggal")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRealisationDate" runat="server" CssClass="datepicker" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("Nama Pasien")%></label>
                                    </td>
                                    <td>
                                     <asp:TextBox ID="txtFindPatientName" Width="120px" runat="server"   />
                                       <%-- <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                            Width="100%" Watermark="Search">
                                            <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                            <IntellisenseHints>
                                                <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                                <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                                <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            </IntellisenseHints>
                                        </qis:QISIntellisenseTextBox>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsIncludeClosed" runat="server" Checked="false" /><%:GetLabel("Include Closed Registration")%>
                                    </td>
                                </tr>
                            </table>
                            <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                                <%=GetLabel("Halaman Ini Akan")%>
                                <span class="lblLink" id="lblRefresh">[refresh]</span>
                                <%=GetLabel("setiap")%>
                                <%=GetRefreshGridInterval() %>
                                <%=GetLabel("menit")%>
                            </div> 
            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                             <div style="display: none">
                                    <asp:Button ID="btnOpenTransactionDt" runat="server" UseSubmitBehavior="false" OnClientClick="return onBeforeOpenTransactionDt();"
                                        OnClick="btnOpenTransactionDt_Click" /></div>
                                <input type="hidden" runat="server" id="hdnTransactionNo" value="" />
                                <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent2" runat="server">
                                            <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                                margin-right: auto; position: relative; font-size: 0.95em; height: 550px; overflow-y: scroll;">
                                                <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                                    <EmptyDataTemplate>
                                                        <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                                                            rules="all">
                                                            <tr>
                                                                <th style="width: 15px">
                                                                </th>
                                                                <th style="width: 2px">
                                                                </th>
                                                                <th style="width: 15px">
                                                                    <%=GetLabel("SESI")%>
                                                                </th>
                                                                <th style="width: 15px">
                                                                    <%=GetLabel("ANTRIAN")%>
                                                                </th>
                                                                <th style="width: 450px" align="left">
                                                                    <%=GetLabel("PASIEN")%>
                                                                </th>
                                                                <th style="width: 350px" align="left">
                                                                    <%=GetLabel("INFORMASI KUNJUNGAN")%>
                                                                </th>
                                                                <th style="width: 15px">
                                                                    CC
                                                                </th>
                                                                <th style="width: 15px">
                                                                    Dx
                                                                </th>
                                                                <th style="width: 15px">
                                                                </th>
                                                            </tr>
                                                            <tr class="trEmpty">
                                                                <td colspan="10">
                                                                    <%=GetLabel("Tidak ada data kunjungan pasien")%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EmptyDataTemplate>
                                                    <LayoutTemplate>
                                                        <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                                                            rules="all">
                                                            <tr>
                                                                <th style="width: 15px">
                                                                </th>
                                                                <th style="width: 2px">
                                                                </th>
                                                                <th style="width: 15px">
                                                                    <%=GetLabel("SESI")%>
                                                                </th>
                                                                <th style="width: 15px">
                                                                    <%=GetLabel("ANTRIAN")%>
                                                                </th>
                                                                <th style="width: 450px" align="left">
                                                                    <%=GetLabel("PASIEN")%>
                                                                </th>
                                                                <th style="width: 350px" align="left">
                                                                    <%=GetLabel("INFORMASI KUNJUNGAN")%>
                                                                </th>
                                                                <th style="width: 15px">
                                                                </th>
                                                                <th style="width: 15px">
                                                                </th>
                                                                <th style="width: 15px">
                                                                </th>
                                                                <th style="width: 15px">
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td class="tdExpand" style="text-align: center">
                                                                <img class="imgExpand" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>' alt='' />
                                                            </td>
                                                            <td align="center" id="tdServiceFlag" runat="server">
                                                            </td>
                                                            <td align="center" id="tdIndicatorSession" runat="server" style="width: 30px">
                                                                <div <%# Eval("DepartmentID").ToString() != "INPATIENT" ? "" : "style='display:none'" %>>
                                                                    <span style="font-weight: bold; font-size: 12pt">
                                                                        <%#: Eval("Session") %></span>
                                                                </div>
                                                            </td>
                                                            <td align="center" id="tdIndicator" runat="server" style="width: 30px">
                                                                <div <%# Eval("DepartmentID").ToString() != "OUTPATIENT" && Eval("DepartmentID").ToString() != "DIAGNOSTIC" ? "Style='display:none'":"" %>>
                                                                    <span style="font-weight: bold; font-size: 12pt">
                                                                        <%#: Eval("QueueNo") %></span>
                                                                </div>
                                                                <div <%# Eval("DepartmentID").ToString() == "OUTPATIENT" ? "Style='display:none'":"style='font-size: 13pt; font-weight: bold'" %>>
                                                                    <%#: Eval("BedCode") %></div>
                                                            </td>
                                                            <td>
                                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td align="center" valign="top" style="width: 20px">
                                                                            <div <%# Eval("DepartmentID").ToString() != "OUTPATIENT" ? "Style='display:none'":"Style='display:none'" %>>
                                                                                <img id="imgPatientSatisfactionLevelImageUri" runat="server" width="24" height="24"
                                                                                    alt="" visible="true" />
                                                                            </div>
                                                                        </td>
                                                                        <td>
                                                                            <input type="hidden" class="hdnVisitIDMain" value='<%#: Eval("VisitID") %>' />
                                                                            <input type="hidden" class="hdnRegistrationIDMain" value='<%#: Eval("RegistrationID") %>' />
                                                                            <div style="text-align: left">
                                                                                <img class="imgPatientImage" src='<%#Eval("PatientImageUrl") %>' alt="" height="55px"
                                                                                    width="40px" style="float: left; margin-right: 10px;" /></div>
                                                                            <div>
                                                                                <span style="font-weight: bold; font-size: 11pt">
                                                                                    <%#: Eval("cfPatientNameSalutation") %></span></div>
                                                                            <div>
                                                                                <%#: Eval("MedicalNo") %>,
                                                                                <%#: Eval("DateOfBirthInString") %>,
                                                                                <%#: Eval("Sex") %></div>
                                                                            <div style="font-style: italic">
                                                                                <%#: Eval("BusinessPartner")%></div>
                                                                            <div class="divAssesment" width="120px" style="display:none;">
                                                                                <input type="button" class="btnAssesment w3-btn w3-hover-blue" value="Assesment"
                                                                                    style="background-color: Red; color: White; width: 120px;" /></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td>
                                                                <table width="100%" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                                                            <b>
                                                                                <%#: Eval("RegistrationNo") %></b>
                                                                            <div <%#:Eval("DepartmentID").ToString() != "INPATIENT" ? "style='display:none'":"" %>>
                                                                                <%#: Eval("cfPatientLocation") %></div>
                                                                            <div>
                                                                                <%#: Eval("ParamedicName")%></div>
                                                                            <div style="font-style: italic">
                                                                                <%#: Eval("VisitTypeName")%></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="center">
                                                                <div id="divChiefComplaint" runat="server" style="text-align: center; color: blue">
                                                                </div>
                                                            </td>
                                                            <td align="center">
                                                                <div id="divDiagnosis" runat="server" style="text-align: center; color: blue">
                                                                </div>
                                                            </td>
                                                            <td align="center">
                                                                <img class="imgCOB <%# Eval("IsUsingCOB").ToString() == "1" ? "imgDisabled" : "imgLink"%>"
                                                                    title='<%# Eval("COB_Name")%>' src=' <%# ResolveUrl("~/Libs/Images/Status/cob.png") %>'
                                                                    style='<%# Eval("IsUsingCOB").ToString() == "True" ? "width:25px": "width:25px;height:25px;display:none" %>'
                                                                    alt="" />
                                                            </td>
                                                            <td align="center" class="tdPatientCall">
                                                                <div id="divPatientCall" runat="server" style="display: none">
                                                                    <input type="button" class="btnPatientCall w3-btn w3-hover-blue" value="Panggil Pasien"
                                                                        style="background-color: Red; color: White; width: 120px;" /></div>
                                                            </td>
                                                        </tr>
                                                        <tr style="display: none" class="trDetail">
                                                        <td colspan="20">
                                                         
                                                            <table>
                                                                <tr>
                                                                    <td>Alamat</td>
                                                                     <td><%#: Eval("HomeAddress")%></td>
                                                                </tr>
                                                                 <tr>
                                                                        <td> <img src='<%= ResolveUrl("~/Libs/Images/homephone.png")%>' alt='' style="float: left;" /><div
                                                                        style="margin-left: 30px"></td>
                                                                     <td> <%#: Eval("cfPhoneNo")%></td>
                                                                </tr>
                                                                 <tr>
                                                                        <td>  <img src='<%= ResolveUrl("~/Libs/Images/mobilephone.png")%>' alt='' style="float: left;" /><div
                                                                        style="margin-left: 30px"></td>
                                                                     <td>  <%#: Eval("cfMobilePhoneNo")%></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Tanggal Kunjungan</td>
                                                                     <td>  <div style="float: left">
                                                                        <%#: Eval("VisitDateInString")%></div> <div style="margin-left: 100px">
                                                                        <%#: Eval("VisitTime")%></div></td>
                                                                </tr>
                                                            </table>
                                                                <div>
                                                                    <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                                                    <input type="hidden" class="hdnRegistrationID" value='<%#: Eval("RegistrationID") %>' />
                                                                    <input type="hidden" class="hdnRoomCode" value='<%#: Eval("RoomCode") %>' />
                                                                  
                                                                    
                                                                    <div id="divDischargeDate" runat="server">
                                                                    </div>
                                                                </div>
                                                        </td>
                                                            <%--<td>
                                                                <div>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div>
                                                                    &nbsp;</div>
                                                            </td>
                                                            <td>
                                                                <div>
                                                                    &nbsp;</div>
                                                            </td>
                                                            <td>
                                                                <div style="padding: 3px">
                                                                    <input type="hidden" value='<%#: Eval("GCSex")%>' class="hdnPatientGender" />
                                                                    <table cellpadding="0" cellspacing="0">
                                                                        <colgroup>
                                                                            <col style="width: 100px" />
                                                                            <col style="width: 10px" />
                                                                            <col style="width: 80px" />
                                                                            <col style="width: 50px" />
                                                                            <col style="width: 10px" />
                                                                            <col style="width: 120px" />
                                                                        </colgroup>
                                                                        <tr>
                                                                            <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                                                <%=GetLabel("Nama Panggilan")%>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <%#: Eval("PreferredName")%>
                                                                            </td>
                                                                            <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                                                <%=GetLabel("No RM")%>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <%#: Eval("MedicalNo")%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                                                <%=GetLabel("Tanggal Lahir")%>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <%#: Eval("DateOfBirthInString")%>
                                                                            </td>
                                                                            <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                                                <%=GetLabel("Umur")%>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <%#: Eval("PatientAge")%>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div>
                                                                    <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                                                    <input type="hidden" class="hdnRegistrationID" value='<%#: Eval("RegistrationID") %>' />
                                                                    <input type="hidden" class="hdnRoomCode" value='<%#: Eval("RoomCode") %>' />
                                                                    <div style="float: left">
                                                                        <%#: Eval("VisitDateInString")%></div>
                                                                    <div style="margin-left: 100px">
                                                                        <%#: Eval("VisitTime")%></div>
                                                                    <div id="divDischargeDate" runat="server">
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <div>
                                                                    &nbsp;</div>
                                                            </td>
                                                            <td>
                                                                <div>
                                                                    &nbsp;</div>
                                                            </td>
                                                            <td>
                                                                <div>
                                                                    &nbsp;</div>
                                                            </td>--%>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </div>
    </div>
 
    <script type="text/javascript">
        $(function () {
            LastSelected();
            setDatePicker('<%=txtRealisationDate.ClientID %>');
            $('#<%=txtRealisationDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });

            $('#<%=txtRealisationDate.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });

            $('#<%=chkIsIncludeClosed.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });

            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnHsuID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {

                if ($(this).attr('class') == 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnHsuID.ClientID %>').val($(this).find('.keyField').html());
                    cbpViewDt.PerformCallback('refresh');
                }
              
            });

            
            

            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            //#region Left Navigation Panel
            $('#leftPageNavPanel ul li').first().addClass('nav-link active');
            $('#leftPageNavPanel ul li a').first().addClass('active');

            $('#leftPageNavPanel ul li').click(function () {
                //                alert("OK");
                //                $('#leftPageNavPanel ul li.selected').removeClass('nav-item active');
                //                $(this).addClass('nav-link active');
                $('#leftPageNavPanel ul li.selected').removeClass('selected');
                $(this).addClass('selected');


                var name = $(this).attr('contentid');
                $('#' + name).removeAttr('style');
                $('#leftPageNavPanel ul li').each(function () {
                    var tempNameContainer = $(this).attr('contentid');
                    if (tempNameContainer != name) {
                        $(this).removeClass('nav-item active');
                        $(this).removeClass('nav-link active');
                        $('#' + tempNameContainer).attr('style', 'display:none');
                    }
                });

                var contentID = $(this).attr('contentID');
                $('#<%=hdnDepartmentID.ClientID %>').val(contentID);

                if (contentID != 'OUTPATIENT' && contentID != 'DIAGNOSTIC' && contentID != 'MCU') {
                    $('#trRegistrationDate').attr('style', 'display:none');
                }
                else {
                    $('#trRegistrationDate').removeAttr('style');
                }

                cbpView.PerformCallback('refresh');
            });

            $('#leftPageNavPanel ul li a').click(function () {
                $('#leftPageNavPanel ul li a.selected').removeClass('nav-item active');
                $(this).addClass('active');
                var name = $(this).attr('contentid');
                $('#' + name).removeAttr('style');
                $('#leftPageNavPanel ul li a').each(function () {
                    var tempNameContainer = $(this).attr('contentid');
                    if (tempNameContainer != name) {
                        $(this).removeClass('active');
                    }
                });
            });

            function showContent(contentID) {
                var i, x, tablinks;
                x = document.getElementsByClassName("divPageNavPanelContent");
                for (i = 0; i < x.length; i++) {
                    x[i].style.display = "none";
                }
                document.getElementById(contentID).style.display = "block";
            }
            //#endregion
        });

 

        var interval = 1000;
        var intervalID = window.setInterval(function () {
            onRefreshDateTime();
        }, interval);

        function onRefreshDateTime() {
            window.clearInterval(intervalID);
            /////cbpViewTime.PerformCallback('refreshHour');
            intervalID = window.setInterval(function () {
                onRefreshDateTime();
            }, interval);
        }

        var intervalGrid = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalIDGrid = window.setInterval(function () {
            cbpView.PerformCallback('refresh');
        }, intervalGrid);

        function onRefreshGridView() {
            window.clearInterval(intervalGrid);
            $('#<%=txtFindPatientName.ClientID %>').val("");
            cbpView.PerformCallback('refresh');
            intervalIDGrid = window.setInterval(function () {
                onRefreshGridView();
            }, intervalGrid);
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();

            cbpViewDt.PerformCallback('refresh');
        }

        function onCbpViewDtEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'find') {
                var hsuid = param[1];
                if (hsuid != "" && hsuid != "0") {
                    SelectedServiceUnit(hsuid); 
                }
            }
            Methods.checkImageError('imgPatientImage', 'patient', 'hdnPatientGender');
        }

        function SelectedServiceUnit(id) {
            $('#<%=grdView.ClientID %> > tbody> tr').each(function (index, value) {
                var idx = $(this).find('.keyField').html();
                if (idx == id) {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                }
                else {
                    console.log(id);
                }
            });

        }

        var isHoverTdExpand = false;
        $('.lvwView tr:gt(0) div.divAssesment').live('click', function () {
            $tr = $(this).closest('tr').parent().closest('tr');
           
            var visitID = $tr.find('.hdnVisitIDMain').val();
            $('#<%=hdnTransactionNo.ClientID %>').val(visitID);
            __doPostBack('<%=btnOpenTransactionDt.UniqueID%>', '');
        });
        $('.lvwView > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            
            if (!isHoverTdExpand && !isHoverTdPatientCall) {
                showLoadingPanel();
                $('#<%=hdnTransactionNo.ClientID %>').val($(this).find('.hdnVisitID').val());
                __doPostBack('<%=btnOpenTransactionDt.UniqueID%>', '');
            }
        });

        var isHoverTdPatientCall = false;
        $('.lvwView tr:gt(0) td.tdPatientCall').live({
            mouseenter: function () { isHoverTdPatientCall = true; },
            mouseleave: function () { isHoverTdPatientCall = false; }
        });

        $('.lvwView tr:gt(0) td.tdPatientCall').live('click', function () {
            $tr = $(this).parent().next();
            var registrationID = $tr.find('.hdnRegistrationID').val();
            var visitID = $tr.find('.hdnVisitID').val();
            var roomCode = $tr.find('.hdnRoomCode').val();
            cbpView.PerformCallback('call|' + registrationID + '|' + visitID + '|' + roomCode);
        });

        function onBeforeOpenTransactionDt() {
            return true;
        }

        $('.lvwView tr:gt(0) td.tdExpand').live({
            mouseenter: function () { isHoverTdExpand = true; },
            mouseleave: function () { isHoverTdExpand = false; }
        });

        $('.lvwView tr:gt(0) td.tdExpand').live('click', function () {
            $tr = $(this).parent().next();
            if (!$tr.is(":visible")) {
                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
                $tr.show('slow');
            }
            else {
                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                $tr.hide('fast');
            }
        });
//        function onTxtSearchViewSearchClick(s) {
//          
//            setTimeout(function () {
//                s.SetBlur();
//                onRefreshGridView();
//                setTimeout(function () {
//                    s.SetFocus();
//                }, 0);
//            }, 0);
//        }

        function LastSelected() {
         
            var name = $('#<%=hdnSelectedDepartment.ClientID %>').val();

            $('#leftPageNavPanel ul li').each(function () {
                var tempNameContainer = $(this).attr('contentid');
                if (tempNameContainer != name) {
                    $(this).removeClass('active');
                } else {

                    $(this).addClass('selected');
                }
            });

            var contentID = name;  //// $(this).attr('contentID');
            $('#<%=hdnDepartmentID.ClientID %>').val(contentID);

            if (contentID != 'OUTPATIENT' && contentID != 'DIAGNOSTIC' && contentID != 'MCU') {
                $('#trRegistrationDate').attr('style', 'display:none');
            }
            else {
                $('#trRegistrationDate').removeAttr('style');
            }

            cbpView.PerformCallback('refresh');
        }

        $('#<%=txtFindPatientName.ClientID %>').change(function () {
            cbpViewDt.PerformCallback('find');
        });

    </script>
</asp:Content>
