<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.Master" 
    AutoEventWireup="true" CodeBehind="PrescriptionStatusEdit2.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.PrescriptionStatusEdit2" %>
   

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
 <%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

   


<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Proses")%></div></li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
<style type="text/css">
        #tblEpisodeSummary tr td  { border: 1px solid #6E6E6E; vertical-align:top; }
        #tblEpisodeSummary tr td h5  { background-color: #73BE32; border-bottom:1px groove black; font-weight:bold; font-size: 16px; margin:0; padding:0; }
        #tblEpisodeSummary tr td div.containerUl { height:300px; overflow-y: scroll; }
        #tblEpisodeSummary tr td ul  { margin:0; padding:0; margin-left:25px; }
        #tblEpisodeSummary tr td ul:not(:first-child) { margin-top: 10px; }
        #tblEpisodeSummary tr td ul li  { list-style-type: circle; font-size: 12px; }
        #tblEpisodeSummary tr td ul li span  { color:#3E3EE3; }
        #tblEpisodeSummary tr td a        { font-size:11px; color:#3E93E3; cursor: pointer; float: right; margin-right: 20px; }
        #tblEpisodeSummary tr td a:hover  { text-decoration: underline; }
        
        #rightPanel          { border:1px solid #6E6E6E; width:100%;height:100%; position: relative; }    
        #rightPanel > ul       { margin:0; padding:2px; border-bottom:1px groove black; }
        #rightPanel > ul > li    { list-style-type: none; font-size: 12px; display: inline-block; border: 1px solid #848484; padding: 5px 8px; cursor: pointer; }
        #rightPanel > ul > li.selected { background-color: #DB2C12; color: White; }
    </style>
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtPrescriptionDate.ClientID %>');
            $('#<%=txtPrescriptionDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#ulTabTransaction li').click(function () {
                $('#ulTabTransaction li.selected').removeAttr('class');
                $contentID = $(this).attr('contentid');
                $('#<%=hdnContentID.ClientID %>').val($contentID);
                onRefreshGrd();
                $(this).addClass('selected');
            });

            $('#<%=txtPrescriptionDate.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGrd();
            });

//            $('#<%=txtBarcodeEntry.ClientID %>').focus();
        });

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalReg = window.setInterval(function () {
            onRefreshGrd();
        }, interval);

        function onRefreshGrd() {

            window.clearInterval(intervalReg);
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpView.PerformCallback('refresh');
            intervalReg = window.setInterval(function () {
                onRefreshGrd();
            }, interval);
        }
        
        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrd();
            }, 0);
        }

        $('#<%=btnProcess.ClientID %>').die('click');


        function onCboDepartmentValueChanged() {
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
        });


        //#endregion

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail') showToast('Save Failed', 'Error Message : ' + param[2]);
            }
            cbpView.PerformCallback('refresh');
        }

        $('#<%=chkIsIgnoreDate.ClientID %>').die();
        $('#<%=chkIsIgnoreDate.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtPrescriptionDate.ClientID %>').attr('readonly', 'readonly');
            }
            else $('#<%=txtPrescriptionDate.ClientID %>').removeAttr('readonly');
            onRefreshGrd();
        });
    </script> 
    <input type="hidden" id="hdnLstSelected" value="" runat="server" />
    <input type="hidden" value="containerProses" id="hdnContentID" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" runat="server" value="" />

    <div style="height:435px;">
        <table class="tblContentArea">
            <colgroup>
                <col style="width:100%"/>
            </colgroup>
            <tr>
                <td style="padding:5px;vertical-align:top">
                    <table>
                        <colgroup>
                            <col style="width:120px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Farmasi")%></label></td>
                            <td>
                                <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onRefreshGrd(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal") %></label></td>
                            <td><asp:TextBox runat="server" ID="txtPrescriptionDate" Width="120px" CssClass="datepicker" />&nbsp;<asp:CheckBox ID="chkIsIgnoreDate" runat="server" Checked="false" /><%:GetLabel("Abaikan Tanggal")%>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Quick Filter")%></label></td>
                            <td>
                                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                    Width="300px" Watermark="Search">
                                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                    <IntellisenseHints>
                                        <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                        <qis:QISIntellisenseHint Text="No.Order" FieldName="PrescriptionReturnOrderNo" />
                                        <qis:QISIntellisenseHint Text="No.Registrasi" FieldName="RegistrationNo" />
                                        <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                    </IntellisenseHints>
                                </qis:QISIntellisenseTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Asal Pasien") %></label></td>
                            <td>
                                <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" Width="300px" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboDepartmentValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Scan No. Resep")%></label></td>
                            <td><asp:TextBox ID="txtBarcodeEntry" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="padding-top:5px;padding-bottom:1px; font-size:0.95em">
                                    <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefresh">[refresh]</span> <%=GetLabel("setiap")%> <%=GetRefreshGridInterval() %> <%=GetLabel("menit")%>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
           
       <%-- -----------------%>
       <table id="tblEpisodeSummary" style="height:100%;width:100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width:30%"/>
                        <col style="width:35%"/>
                    </colgroup>

            
                    <tr>
                    <td rowspan="2">
                            <h5><%=GetLabel("Coba")%></h5>
                          
                            <a url="~/Program/PatientPage/Summary/EpisodeSummary/Detail/EpisodeSummaryDtChiefComplaintDiagnosisCtl.ascx" popupheight="500" popupwidth="1030" headertext="Chief Complaint & Diagnosis"><%=GetLabel("View More")%></a>
                        </td>
                        <td>
                            <h5><%=GetLabel("CHIEF COMPLAINT & DIAGNOSIS")%></h5>
                            <div class="containerUl">
                                <asp:Repeater ID="rptChiefComplaint" runat="server">
                                    <HeaderTemplate>
                                        <ul>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li>
                                        <div></div>
                                        <div><b><%#: Eval("DiagnoseType")%></b> - <%#: Eval("DifferentialStatus")%></div>
                                            
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>

                                <hr />

                                <asp:Repeater ID="rptDiagnosis" runat="server">
                                    <HeaderTemplate>
                                        <ul>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <div><%#: Eval("DiagnosisText")%></div>
                                            <div><b><%#: Eval("DiagnoseType")%></b> - <%#: Eval("DifferentialStatus")%></div>
                                            <span><%#: Eval("DifferentialDateInString")%> <%#: Eval("DifferentialTime")%>, <%#: Eval("ParamedicName")%></span>
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <a url="~/Program/PatientPage/Summary/EpisodeSummary/Detail/EpisodeSummaryDtChiefComplaintDiagnosisCtl.ascx" popupheight="500" popupwidth="1030" headertext="Chief Complaint & Diagnosis"><%=GetLabel("View More")%></a>
                        </td>
                        <td>
                            <h5><%=GetLabel("MEDICATION")%></h5>
                            <div class="containerUl">
                                <asp:Repeater ID="rptMedication" runat="server">
                                    <HeaderTemplate>
                                        <ul>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <div><%#: Eval("InformationLine1")%></div>
                                            <div><div style="color:Blue;width:35px;float:left;">DOSE</div> <%#: Eval("NumberOfDosage")%> <%#: Eval("DosingUnit")%> - <%#: Eval("Route")%> - <%#: Eval("cfDoseFrequency")%></div>
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <a url="~/Program/PatientPage/Summary/EpisodeSummary/Detail/EpisodeSummaryDtMedicationCtl.ascx" popupheight="500" popupwidth="1030" headertext="Medication"><%=GetLabel("View More")%></a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h5><%=GetLabel("TEST ORDER")%></h5>
                            <div class="containerUl">
                               <%-- <asp:Repeater ID="rptTestOrder" runat="server" OnItemDataBound="rptTestOrder_ItemDataBound">--%>
                                    <HeaderTemplate>
                                        <ul>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <div><%#: Eval("ItemName1") %></div>
                                            <div><%#: Eval("DiagnoseName")%> | <%#: Eval("cfToBePerformed")%> | <%#: Eval("TestOrderStatus")%></div>
                                            <span id="spnTestOrderDtInformation" runat="server"></span>
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                               <%-- </asp:Repeater>--%>
                            </div>
                            <a url="~/Program/PatientPage/Summary/EpisodeSummary/Detail/EpisodeSummaryDtTestOrderCtl.ascx" popupheight="500" popupwidth="1030" headertext="Test Order"><%=GetLabel("View More")%></a>
                        </td>
                        <td>
                            <h5><%=GetLabel("PATIENT DISCHARGE")%></h5>
                            <div class="containerUl">
                                <asp:Repeater ID="rptFollowUpVisit" runat="server">
                                    <HeaderTemplate>
                                        <ul>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <div><%#: Eval("VisitTypeName")%>; <%#: Eval("Notes")%> </div>
                                            <span><%#: Eval("StartDateTimeInString")%>, <%#: Eval("ParamedicName")%></span>
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>

                                <hr />

                                <asp:Repeater ID="rptPatientInstruction" runat="server">
                                    <HeaderTemplate>
                                        <ul>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <div><%#: Eval("InstructionGroup")%></div>
                                            <div><%#: Eval("Description")%></div>
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <a url="~/Program/PatientPage/Summary/EpisodeSummary/Detail/EpisodeSummaryDtPatientDischargeCtl.ascx" popupheight="500" popupwidth="1030" headertext="Patient Discharge"><%=GetLabel("View More")%></a>
                        </td>
                    </tr>
                </table>
       <%-- -----------------%>
        </table>
    </div>
</asp:Content>
