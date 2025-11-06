<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master" AutoEventWireup="true" 
    CodeBehind="EpisodeSummary.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.EpisodeSummary" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <style type="text/css">
        #tblEpisodeSummary tr td  { border: 1px solid #6E6E6E; vertical-align:top; }
        #tblEpisodeSummary tr td h5  { background-color: #73BE32; border-bottom:1px groove black; font-weight:bold; font-size: 16px; margin:0; padding:0; }
        #tblEpisodeSummary tr td div.containerUl { height:190px; overflow-y: scroll; }
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
            $('#rightPanel ul li').click(function () {
                $('#rightPanel ul li.selected').removeClass('selected');
                $(this).addClass('selected');
                var url = $(this).attr('url');

                $('#containerLeftPanelContent').html('');
                if (url != '#') {
                    $('#divLeftPanelContentLoading').show();
                    Methods.getHtmlControl(ResolveUrl(url), "", function (result) {
                        $('#divLeftPanelContentLoading').hide();
                        $('#containerLeftPanelContent').html(result.replace(/\VIEWSTATE/g, ''));
                    }, function () {
                        $('#divLeftPanelContentLoading').hide();
                    });
                }
            });
            $('#rightPanel ul li').first().click();

            $('#tblEpisodeSummary tr td a').click(function () {
                var url = $(this).attr('url');
                var width = $(this).attr('popupwidth');
                var height = $(this).attr('popupheight');
                var headerText = $(this).attr('headertext');
                openUserControlPopup(url, "", headerText, width, height);
            });
        });

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            var visitID = $('#<%:hdnVisitID.ClientID %>').val();
            if (registrationID == '') {
                errMessage.text = 'Please Select Registration First!';
                return false;
            }
            else {
                if (code == 'MR000008')
                {
                    filterExpression.text = registrationID;
                    return true;
                }
                    if (code == 'PM-00119' || code == 'PM-00120' || code == 'PM-00121' || code == 'PM-00122' || code == 'PM-00123') {
                        filterExpression.text = 'RegistrationID = ' + registrationID;
                        return true;
                    }
                else {
                    filterExpression.text = visitID;
                    return true;
                }
            }
        }
    </script>

    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnVisitID" value="" runat="server" />
    <table style="width:100%" cellpadding="0" cellspacing="0">
        <colgroup>
            <col />
            <col style="width:10px"/>
            <col style="width:36%"/>
        </colgroup>
        <tr style="height:480px">
            <td>
                <table id="tblEpisodeSummary" style="height:100%;width:100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width:50%"/>
                        <col style="width:50%"/>
                    </colgroup>
                    <tr style="height:50%;">
                        <td>
                            <h5><%=GetLabel("CHIEF COMPLAINT & DIAGNOSIS")%></h5>
                            <div class="containerUl">
                                <asp:Repeater ID="rptChiefComplaint" runat="server">
                                    <HeaderTemplate>
                                        <ul>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <div><%#: Eval("ChiefComplaintText")%></div>
                                            <span><%#: Eval("ObservationDateInString")%> <%#: Eval("ObservationTime")%>, <%#: Eval("ParamedicName")%></span>
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
                    <tr style="height:50%;">
                        <td>
                            <h5><%=GetLabel("TEST ORDER")%></h5>
                            <div class="containerUl">
                                <asp:Repeater ID="rptTestOrder" runat="server" OnItemDataBound="rptTestOrder_ItemDataBound">
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
                                </asp:Repeater>
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
            </td>
            <td>&nbsp;</td>
            <td style="height:460px;vertical-align:top">
                <div id="rightPanel">
                    <ul>
                        <li url="~/Program/PatientPage/Summary/EpisodeSummary/VitalSignInformationCtl.ascx" title="Vital Signs">VS</li>
                        <li url="~/Program/PatientPage/Summary/EpisodeSummary/EpisodeSummaryMedicalChartContainerCtl.ascx" title="Medical Chart">MC</li>
                        <li url="~/Program/PatientPage/Summary/EpisodeSummary/EpisodeSummaryBodyDiagramContainerCtl.ascx" title="Body Diagram">BD</li>
                    </ul>
                    <div id="containerLeftPanelContent">
                    </div>
                    <div id="divLeftPanelContentLoading" style="position:absolute;top:30%;left:48%;display:none">
                        <div style="margin:0 auto">
                            <img src="<%= ResolveUrl("~/Libs/Images/Loading.gif")%>" alt="" />
                        </div>
                    </div>          
                </div> 
            </td>
        </tr>
    </table>
</asp:Content>
