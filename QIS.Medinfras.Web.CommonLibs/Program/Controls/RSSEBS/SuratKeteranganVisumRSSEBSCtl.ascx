<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuratKeteranganVisumRSSEBSCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SuratKeteranganVisumRSSEBSCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_medicalsickleave">

    $(function () {
        hideLoadingPanel();
    });

    setDatePicker('<%=txtValueDate.ClientID %>');

    $('#<%=btnPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpMedicalSickLeave.PerformCallback('Print');
        }
    });

    $('#<%=txtValueDate.ClientID %>').change(function () {
        var date = $(this).val();
        var YYYY = date.substring(6, 10);
        var MM = date.substring(3, 5);
        var DD = date.substring(0, 2);
        var dateALL = YYYY + '-' + MM + '-' + DD;
        $('#<%=hdnTanggal.ClientID %>').val(dateALL);
        $('#<%=hdnTanggalString.ClientID %>').val(date);
    });

    //#region Pemeriksaan Fisik
    $('#lblObjectiveResumeText').die('click');
    $('#lblObjectiveResumeText').live('click', function (evt) {
        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
        var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND ParamedicID = " + $('#<%=hdnParamedicID.ClientID %>').val() + " AND GCPatientNoteType IN ('X011^011') AND ObjectiveText IS NOT NULL";
        openSearchDialog('planningNote', filterExpression, function (value) {
            $('#<%=hdnVisitNoteID.ClientID %>').val(value);
            onSearchPatientVisitNote(value);
        });
    });

    function onSearchPatientVisitNote(value) {
        var filterExpression = "ID = " + value;
        Methods.getObject('GetPatientVisitNoteList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtObjectiveResumeText.ClientID %>').val(result.ObjectiveText);
            }
            else {
                $('#<%=txtObjectiveResumeText.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onReprintPatientPaymentReceiptSuccess() {
        var reportCode = "PM-00710";

        var date = $('#<%=txtValueDate.ClientID %>').val();
        var day = date.substring(0, 2);
        var month = date.substring(3, 5);
        var year = date.substring(6, 10);
        var newDate = year + '-' + month + '-' + day;

        var number = $('#<%=txtNumber1.ClientID %>').val() + "/" + $('#<%=txtNumber2.ClientID %>').val() + "/" + $('#<%=txtNumber3.ClientID %>').val(); 

        var time1 = $('#<%=txtValueTime1.ClientID %>').val();
        var time2 = $('#<%=txtValueTime2.ClientID %>').val();

        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
        var objectiveText = $('#<%=hdnObjectiveText.ClientID %>').val();
        var remarks = $('#<%=txtRemarks.ClientID %>').val();

        var filterExpression = visitID + "|" + newDate + "|" + time1 + "|" + time2 + "|" + number + "|" + objectiveText + "|" + remarks;

        if (reportCode != "") {
            openReportViewer(reportCode, filterExpression);
        }
        pcRightPanelContent.Hide();
        cbpView.PerformCallback('refresh');

    }

</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnPrint" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Print")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnVisitID" runat="server" value="" />
        <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
        <input type="hidden" id="hdnTanggal" runat="server" value="" />
        <input type="hidden" id="hdnTanggalString" runat="server" value="" />
        <input type="hidden" id="hdnReportCode" runat="server" value="" />
        <input type="hidden" value="" id="hdnObjectiveText" runat="server" />
        <input type="hidden" runat="server" id="hdnIsChanged" value="0" />
        <input type="hidden" runat="server" id="hdnVisitNoteID" value="0" />
        <input type="hidden" runat="server" id="hdnParamedicID" value="0" />
        <table>
            <colgroup>
                <col style="width: 150px" />
                <col style="width: 150px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Nomor Surat")%></label>
                </td>
                <td>
                    <table>
                        <tr>
                             <td>
                                <asp:TextBox ID="txtNumber1" Width="30px" CssClass="required" runat="server" />
                            </td>
                            <td>
                                /
                            </td>
                             <td>
                                <asp:TextBox ID="txtNumber2" Width="30px" CssClass="required" runat="server" />
                            </td>
                            <td>
                                /
                            </td>
                            <td>
                                <asp:TextBox ID="txtNumber3" Width="30px" CssClass="required" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Tanggal")%></label>
                </td>
                    <td class="tdCustomDate">
                        <asp:TextBox ID="txtValueDate" CssClass="txtValueDate datepicker" runat="server"
                            Width="150px" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory" />
                    <%=GetLabel("Jam")%>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtValueTime1" Width="30px" CssClass="number" runat="server"
                                    Style="text-align: center" MaxLength="2" max="24" min="0" />
                            </td>
                            <td>
                                <label class="lblNormal" />
                                <%=GetLabel(":")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtValueTime2" Width="30px" CssClass="number" runat="server"
                                    Style="text-align: center" MaxLength="2" max="59" min="0" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="vertical-align:top">
                    <label class="lblNormal" id="Label5">
                        <%=GetLabel("Pemeriksaan Fisik") %></label>
                </td>
                <td>
                    <asp:TextBox ID="txtObjectiveResumeText" runat="server" Width="98%" TextMode="Multiline"
                        Rows="10" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Kesimpulan")%></label>
                </td>
                 <td>
                    <asp:TextBox ID="txtRemarks" Width="300px" CssClass="required" runat="server"
                        TextMode="Multiline" Rows="2" />
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpMedicalSickLeave" runat="server" Width="100%" ClientInstanceName="cbpMedicalSickLeave"
            ShowLoadingPanel="false" OnCallback="cbpMedicalSickLeave_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    onReprintPatientPaymentReceiptSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
