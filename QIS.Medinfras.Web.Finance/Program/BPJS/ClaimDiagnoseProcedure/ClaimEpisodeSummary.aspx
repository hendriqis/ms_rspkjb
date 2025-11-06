<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="ClaimEpisodeSummary.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ClaimEpisodeSummary" %>

<%@ Register Src="~/Program/BPJS/ClaimDiagnoseProcedure/BPJSClaimToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script id="dxis_patientinstructionquickpicks1" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.core.js")%>'
        type='text/javascript'></script>
    <script id="dxis_patientinstructionquickpicks2" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.widget2.js")%>'
        type='text/javascript'></script>
    <script id="dxis_patientinstructionquickpicks3" src='<%= ResolveUrl("~/Libs/Scripts/jquery/ui/jquery.ui.accordion.js")%>'
        type='text/javascript'></script>
    <style type="text/css">
        #listSummary div.containerUl ul
        {
            margin: 0;
            padding: 0;
            margin-left: 25px;
        }
        #listSummary div.containerUl ul:not(:first-child)
        {
            margin-top: 10px;
        }
        #listSummary div.containerUl ul li
        {
            list-style-type: none;
            font-size: 12px;
            padding-bottom: 5px;
        }
        #listSummary div.containerUl ul li span
        {
            color: #3E3EE3;
        }
        #listSummary div.containerUl a
        {
            font-size: 12px;
            color: #3E93E3;
            cursor: pointer;
            float: right;
            margin-right: 20px;
        }
        #listSummary div.containerUl a:hover
        {
            text-decoration: underline;
        }
        #listSummary div.headerHistoryContent
        {
            text-align: left;
            border-bottom: 1px groove black;
            font-size: 13px;
            margin: 0;
            padding: 0;
            margin-bottom: 5px;
        }
        
        #listSummary .divContentTitle
        {
            margin-left: 25px;
            font-weight: bold;
            font-size: 12px;
            text-decoration: underline;
        }
        #listSummary .divContent
        {
            margin-left: 25px;
            font-weight: bold;
            font-size: 11px;
        }
        
        #listSummary .divNotAvailableContent
        {
            margin-left: 25px;
            font-size: 11px;
            font-style: italic;
            color: red;
        }
        
        
        #tblEpisodeSummary tr td
        {
            border: 1px solid #6E6E6E;
            vertical-align: top;
        }
        #tblEpisodeSummary tr td h5
        {
            background-color: #73BE32;
            border-bottom: 1px groove black;
            font-weight: bold;
            font-size: 16px;
            margin: 0;
            padding: 0;
        }
        #tblEpisodeSummary tr td div.containerUl
        {
            height: 190px;
            overflow-y: scroll;
        }
        #tblEpisodeSummary tr td ul
        {
            margin: 0;
            padding: 0;
            margin-left: 25px;
        }
        #tblEpisodeSummary tr td ul:not(:first-child)
        {
            margin-top: 10px;
        }
        #tblEpisodeSummary tr td ul li
        {
            list-style-type: circle;
            font-size: 13px;
        }
        #tblEpisodeSummary tr td ul li span
        {
            color: #3E3EE3;
        }
        #tblEpisodeSummary tr td a
        {
            font-size: 11px;
            color: #3E93E3;
            cursor: pointer;
            float: right;
            margin-right: 20px;
        }
        #tblEpisodeSummary tr td a:hover
        {
            text-decoration: underline;
        }
        
        #rightPanel
        {
            border: 1px solid #6E6E6E;
            width: 100%;
            height: 100%;
            position: relative;
        }
        #rightPanel > ul
        {
            margin: 0;
            padding: 2px;
            border-bottom: 1px groove black;
        }
        #rightPanel > ul > li
        {
            list-style-type: none;
            font-size: 13px;
            display: inline-block;
            border: 1px solid #848484;
            padding: 5px 8px;
            cursor: pointer;
            background-color: #95a5a6;
        }
        #rightPanel > ul > li.selected
        {
            background-color: #f39c12;
            color: White;
        }
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

                $('#listSummary').show();
                $('#listSummary').accordion({ fillSpace: true });
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
    </script>
    <input type="hidden" value="" id="hdnHSUImagingID" runat="server" />
    <input type="hidden" value="" id="hdnHSULaboratoryID" runat="server" />
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <colgroup>
            <col width="60%" />
            <col />
        </colgroup>
        <tr>
            <td>
                <div style="height: 450px;">
                    <div class="accordion" id="listSummary" style="text-align: left;">
                        <a style="font-size: 14px;">
                            <%= GetLabel("SUBJECTIVE")%></a>
                        <div class="containerUl">
                            <div class="divContentTitle">
                                <%= GetLabel("CHIEF COMPLAINT & HPI")%></div>
                            <asp:Repeater ID="rptChiefComplaint" runat="server">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li>
                                        <div>
                                            <span>
                                                <%#: Eval("HistoryDate")%>
                                                <%#: Eval("HistoryTime")%>
                                                <%#: Eval("ParamedicName")%></span></div>
                                        <div>
                                            <textarea style="border: 0; width: 625px; height: 250px" readonly><%#: Eval("ChiefComplaintText")%></textarea></div>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                            <div id="divHPI" class="divContent" runat="server">
                            </div>
                        </div>
                        <a style="font-size: 14px;">
                            <%= GetLabel("OBJECTIVE")%></a>
                        <div class="containerUl">
                            <div class="divContentTitle">
                                <%= GetLabel("REVIEW OF SYSTEM")%></div>
                            <asp:Repeater ID="rptReviewOfSystemHd" OnItemDataBound="rptReviewOfSystemHd_ItemDataBound"
                                runat="server">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li><span>
                                        <%#: Eval("ObservationDateInString")%>
                                        <%#: Eval("ObservationTime")%>
                                        <br />
                                        <%#: Eval("ParamedicName")%></span>
                                        <asp:Repeater ID="rptReviewOfSystemDt" runat="server">
                                            <ItemTemplate>
                                                <div style="padding-left: 5px;">
                                                    <strong>
                                                        <%#: DataBinder.Eval(Container.DataItem, "ROSystem") %>
                                                        : </strong>&nbsp;
                                                    <%#: DataBinder.Eval(Container.DataItem, "cfRemarks")%>
                                                </div>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <br style="clear: both" />
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                            <hr />
                            <div class="divContentTitle">
                                <%= GetLabel("VITAL SIGN")%></div>
                            <asp:Repeater ID="rptVitalSignHd" OnItemDataBound="rptVitalSignHd_ItemDataBound"
                                runat="server">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li><span>
                                        <%#: Eval("ObservationDateInString")%>
                                        <%#: Eval("ObservationTime")%>
                                        <br />
                                        <%#: Eval("ParamedicName")%></span>
                                        <asp:Repeater ID="rptVitalSignDt" runat="server">
                                            <ItemTemplate>
                                                <div style="padding-left: 5px;">
                                                    <strong>
                                                        <%#: DataBinder.Eval(Container.DataItem, "VitalSignLabel") %>
                                                        : </strong>&nbsp;
                                                    <%#: DataBinder.Eval(Container.DataItem, "DisplayVitalSignValue") %>
                                                </div>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <br style="clear: both" />
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                        <a style="font-size: 14px;">
                            <%= GetLabel("ASSESSMENT")%></a>
                        <div class="containerUl">
                            <asp:Repeater ID="rptDifferentDiagnosis" runat="server">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li><span>
                                        <%#: Eval("DiagnosisDate")%>
                                        <%#: Eval("DiagnosisTime")%>
                                        <%#: Eval("PhysicianName")%></span>
                                        <div style="font-weight: bold">
                                            <%#:Eval("DiagnosisText")%></div>
                                        <div>
                                            <%#: Eval("DiagnosisType")%></div>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <a style="font-size: 14px;">
                            <%= GetLabel("PLANNING : LABORATORY")%></a>
                        <div class="containerUl">
                            <asp:Repeater ID="rptLabTestOrder" runat="server" OnItemDataBound="rptLabTestOrder_ItemDataBound">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li><span>
                                        <%#: Eval("TestDate")%>
                                        <%#: Eval("TestTime")%>,
                                        <%#: Eval("ItemName") %></span>
                                        <asp:Repeater ID="rptLaboratoryDt" runat="server">
                                            <ItemTemplate>
                                                <div style="padding-left: 10px;">
                                                    <strong>
                                                        <%#: DataBinder.Eval(Container.DataItem, "FractionName") %>
                                                        : </strong>&nbsp; <span <%# Eval("ResultFlag").ToString() != "N" ? "Style='color:red'":"Style='color:black'" %>>
                                                            <%#: DataBinder.Eval(Container.DataItem, "ResultValue") %>
                                                        </span>
                                                    <%#: DataBinder.Eval(Container.DataItem, "ResultUnit") %>&nbsp;&nbsp; <span <%# Eval("RefRange").ToString() == "" ? "Style='display:none'":"Style='color:black;font-style:italic'" %>>
                                                        (<%#: DataBinder.Eval(Container.DataItem, "RefRange") %>)</span>
                                                </div>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <br style="clear: both" />
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                            <div id="divLaboratoryNA" class="divNotAvailableContent" runat="server">
                                This information is not available</div>
                        </div>
                        <a style="font-size: 14px;">
                            <%= GetLabel("PLANNING : IMAGING")%></a>
                        <div class="containerUl">
                            <asp:Repeater ID="rptImagingTestOrder" runat="server" OnItemDataBound="rptImagingTestOrder_ItemDataBound">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li><span>
                                        <%#: Eval("TestDate")%>
                                        <%#: Eval("TestTime")%>,
                                        <%#: Eval("ItemName") %></span>
                                        <asp:Repeater ID="rptImagingTestOrderDt" runat="server">
                                            <ItemTemplate>
                                                <textarea style="padding-left: 10px; border: 0; width: 350px; height: 150px" readonly><%#: DataBinder.Eval(Container.DataItem, "Resultvalue") %> </textarea>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <br style="clear: both" />
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                            <div id="divImagingNA" class="divNotAvailableContent" runat="server">
                                This information is not available</div>
                        </div>
                        <a style="font-size: 14px;">
                            <%= GetLabel("PLANNING : MEDICATION")%></a>
                        <div class="containerUl">
                            <asp:Repeater ID="rptMedication" runat="server">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li>
                                        <div>
                                            <span <%# Eval("IsRFlag").ToString() == "0" ? "Style='display:none'":"Style='color:black;font-weight:bold'" %>>
                                                R/&nbsp;&nbsp</span> <strong>
                                                    <%#: Eval("InformationLine1")%></strong>
                                        </div>
                                        <div <%# Eval("IsCompound").ToString() == "0" ? "Style='display:none'":"Style='color:black;font-style:italic;margin-left:20px'" %>>
                                            <%#: Eval("MedicationLine")%>
                                        </div>
                                        <div style="color: Blue; width: 35px; float: left; margin-left: 20px">
                                            DOSE</div>
                                        <%#: Eval("NumberOfDosage")%>
                                        <%#: Eval("DosingUnit")%>
                                        -
                                        <%#: Eval("Route")%>
                                        -
                                        <%#: Eval("cfDoseFrequency")%>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                            <div id="divMedicationNA" class="divNotAvailableContent" runat="server">
                                This information is not available</div>
                        </div>
                        <a style="font-size: 14px;">
                            <%= GetLabel("DISCHARGE AND FOLLOW-UP VISIT")%></a>
                        <div class="containerUl">
                            <asp:Repeater ID="rptDischargeInformation" runat="server">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li>
                                        <div>
                                            <span>
                                                <%#: Eval("DischargeDateInString")%>,
                                                <%#: Eval("DischargeCondition")%></span></div>
                                        <%#: Eval("DischargeMethod")%>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                            <div class="divContentTitle">
                                <%= GetLabel("Follow-up Visit")%></div>
                            <asp:Repeater ID="rptFollowUpVisit" runat="server">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li>
                                        <div>
                                            <span>
                                                <%#: Eval("StartDateTimeInString")%>,
                                                <%#: Eval("ParamedicName")%></span></div>
                                        <%#: Eval("VisitTypeName")%>;
                                        <%#: Eval("Notes")%>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </td>
            <td style="height: 460px; vertical-align: top">
                <div id="rightPanel">
                    <ul>
                        <li url="~/Program/BPJS/ClaimDiagnoseProcedure/ClaimVitalSignInformationCtl.ascx"
                            title="Vital Signs">Vital Sign</li>
                        <li url="~/Program/PatientPage/Summary/EpisodeSummary/EpisodeSummaryMedicalChartContainerCtl.ascx"
                            title="Medical Chart" style="display: none">MC</li>
                        <li url="~/Program/PatientPage/Summary/EpisodeSummary/EpisodeSummaryBodyDiagramContainerCtl.ascx"
                            title="Body Diagram" style="display: none">Body Diagram</li>
                    </ul>
                    <div id="containerLeftPanelContent">
                    </div>
                    <div id="divLeftPanelContentLoading" style="position: absolute; top: 30%; left: 48%;
                        display: none">
                        <div style="margin: 0 auto">
                            <img src="<%= ResolveUrl("~/Libs/Images/Loading.gif")%>" alt="" />
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
