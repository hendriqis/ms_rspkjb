<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="PatientQueueInformation.aspx.cs" Inherits="QIS.Medinfras.Web.Outpatient.Program.PatientQueueInformation" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">

        function oncboClinicValueChanged() {
            cbpViewParamedic.PerformCallback('refresh');
        }

        $('#tblParamedic tr').live('click', function () {
            var id = $(this).find('.ParamedicID').val();
            $('#tblParamedic tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#<%=hdnParamedicID.ClientID %>').val(id);
            cbpViewPatient.PerformCallback('refresh');
        });

        function onCbpViewEndCallback() {
            hideLoadingPanel();
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpViewPatient.PerformCallback('changepage|' + page);
            }, null, currPage);
        });

        function onCbpViewPatientEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);

                setPaging($("#paging"), pageCount, function (page) {
                    cbpViewPatient.PerformCallback('changepage|' + page);
                });
            }
            Methods.checkImageError('imgPatientImage', 'patient', 'hdnPatientGender');
        }
        //#endregion
    </script>
    <div style="padding: 15px">
        <style type="text/css">
            .hrnew {
                margin: 0;
                padding: 0;
            }
            .ulPatient
            {
                margin: 0;
                padding: 0;
            }
            .ulPatient li
            {
                display: inline-block;
                border-radius: 10px;
                list-style-type: none;
                width: 220px;
                height: 135px;
                margin: 0 10px;
                padding: 5px;
            }
            
            .ulFooter li
            {
                display: inline-block;
                border-radius: 2px;
                list-style-type: none;
                width: 75px;
                height: 15px;
                margin: 0 10px;
                padding: 5px;
                font-size: 11px;
            }
            .genderStyle
            {
                font-size: 11px;
            }
            
            .fontCustom
            {
                font-size: 11px;
            }
            
            .liBackground
            {
                background-color: #A1A4A6;
            }
            .liBackgroundM
            {
                background-color: #4ac5e3;
            }
            .liBackgroundF
            {
                background-color: #ffbdde;
            }
            
            .ulTab
            {
                margin: 0;
                padding: 0;
            }
            .ulTab li
            {
                list-style-type: none;
                width: 100px;
                height: 40px;
                margin: 0 10px;
                padding: 5px;
            }
            .TabContent
            {
                background-color: #F8C299;
            }
            
            .parentCell{
                position: relative;
            }
            .tooltip{
                display: none;
                position: absolute; 
                z-index: 100;
                border: 1px;
                background-color: white;
                border: 1px solid green;
                padding: 3px;
                color: green; 
                top: 20px; 
                left: 20px;
            }
            .parentCell:hover span.tooltip{
                display:block;
            }
        </style>
        <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
        <input type="hidden" value="" id="hdnRoomSum" runat="server" />
        <div class="pageTitle">
            <%=GetLabel("Informasi Antrian Pasien")%></div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top" colspan="2">
                    <fieldset id="fsPatientList">
                        <table class="tblEntryContent">
                            <tr id="trServiceUnit" runat="server">
                                <td class="tdLabel" style="width: 120px">
                                    <label class="lblNormal">
                                        <%=GetLabel("Klinik")%></label>
                                </td>
                                <td>
                                    <dxe:aspxcombobox id="cboClinic" clientinstancename="cboClinic" runat="server" width="400px">
                                        <ClientSideEvents ValueChanged="function(s,e) { oncboClinicValueChanged(); }" />
                                    </dxe:aspxcombobox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td style="width: 250px; border: 1px solid #EAEAEA" valign="top">
                    <input type="hidden" value="" id="hdnParamedicID" runat="server" />
                    <dxcp:aspxcallbackpanel id="cbpViewParamedic" runat="server" width="100%" clientinstancename="cbpViewParamedic"
                        showloadingpanel="false" oncallback="cbpViewParamedic_Callback">
                        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { hideLoadingPanel(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Repeater ID="rptParamedic" runat="server">
                                    <HeaderTemplate>
                                        <table class="grdSelected" id="tblParamedic">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <div>
                                                    <%#: Eval("ParamedicName")%></div>
                                                <input type="hidden" value='<%#: Eval("ParamedicID")%>' class="ParamedicID" runat="server" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
        </table>
        </FooterTemplate> </asp:Repeater> </dx:PanelContent> </PanelCollection> </dxcp:ASPxCallbackPanel>
        </td>
        <td align="left" id="footer" valign="top" style="width: auto; height: 500px;">
            <div style="height: 450px; overflow-y: scroll">
                <dxcp:aspxcallbackpanel id="cbpViewPatient" runat="server" width="100%" clientinstancename="cbpViewPatient"
                    showloadingpanel="false" oncallback="cbpViewPatient_Callback">
                            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewPatientEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                    <asp:Repeater ID="rptPatient" runat="server">
                                        <HeaderTemplate>
                                            <ul class="ulPatient">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                                <li class="liBackground<%#:Eval("GenderCodeSuffix") %>">
                                                <table class="fontCustom" id="MyTable">
                                                    <tr>
                                                        <td rowspan="3" align="center" style="width: 80px;">
                                                            <img class="imgPatientImage" src='<%#Eval("PatientImageUrl") %>' alt="" height="100px"
                                                                width="70px" /><br />
                                                            <input type="hidden" value='<%#: Eval("GCGender")%>' class="hdnPatientGender" />
                                                            <div class='trGender<%#:Eval("GenderCodeSuffix")%>' style="margin-top: 5px; color: White;">
                                                                <%#:Eval("Sex")%></div>
                                                        </td>
                                                        <td>
                                                            <table>
                                                                <tr style="margin-right: 20px;">
                                                                    <td>
                                                                        Sesi : 
                                                                        <%#:Eval("Session")%>
                                                                    </td>
                                                                </tr>
                                                                <tr style="margin-right: 20px;">
                                                                    <td>
                                                                        No Antrian : 
                                                                        <%#:Eval("QueueNo")%>
                                                                    </td>
                                                                </tr>
                                                                <tr style="margin-right: 20px;">
                                                                    <td>
                                                                        <%#:Eval("RegistrationNo")%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <hr class="hrnew" style="border-color: blue" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="parentCell">
                                                                        <span class="tooltip"><%#:Eval("PatientName")%></span>
                                                                        <%#:Eval("PatientName").ToString().Length > 15 ? Eval("PatientName").ToString().PadRight(140).Substring(0,15).TrimEnd() + "..." : Eval("PatientName").ToString()%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        No RM :
                                                                        <%#:Eval("MedicalNo")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input id="hdnVisitID" type="hidden" value='<%#: Eval("VisitID")%>' class="VisitID"
                                                    runat="server" />
                                            </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:aspxcallbackpanel>
            </div>
            <div class="containerPaging">
                <div class="wrapperPaging">
                    <div id="paging">
                    </div>
                </div>
            </div>
        </td>
        </tr> </table>
    </div>
</asp:Content>
