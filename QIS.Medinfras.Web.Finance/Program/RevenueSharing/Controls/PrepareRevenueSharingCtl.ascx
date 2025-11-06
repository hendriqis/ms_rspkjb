<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrepareRevenueSharingCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PrepareRevenueSharingCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_preparerevenuesharingctl">

    setDatePicker('<%=txtValueDateFrom.ClientID %>');

    setDatePicker('<%=txtValueDateTo.ClientID %>');

    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpRevenueSharing.PerformCallback('Print');
        }
    });

    $('#<%=txtValueDateFrom.ClientID %>').change(function () {
        var date = $(this).val();
        var YYYY = date.substring(6, 10);
        var MM = date.substring(3, 5);
        var DD = date.substring(0, 2);
        var dateALL = YYYY + '' + MM + '' + DD;
        $('#<%=hdnTanggalDari.ClientID %>').val(dateALL);
        $('#<%=hdnTanggalDariString.ClientID %>').val(date);
    });

    $('#<%=txtValueDateTo.ClientID %>').change(function () {
        var date = $(this).val();
        var YYYY = date.substring(6, 10);
        var MM = date.substring(3, 5);
        var DD = date.substring(0, 2);
        var dateALL = YYYY + '' + MM + '' + DD;
        $('#<%=hdnTanggalSampai.ClientID %>').val(dateALL);
        $('#<%=hdnTanggalSampaiString.ClientID %>').val(date);
    });

    function onCboDepartmentValueChanged() {
        var dept = cboDepartment.GetValue();
        $('#<%=hdnDepartmentID.ClientID %>').val(dept);        
    }

    function onPrintPreparationRevenueSuccess() {
        var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
        var from = $('#<%=hdnTanggalDari.ClientID %>').val();
        var to = $('#<%=hdnTanggalSampai.ClientID %>').val();
        var dept = $('#<%=hdnDepartmentID.ClientID %>').val();

        var errMessage = { text: "" };
        var filterExpression = paramedicID + "|" + from + ";" + to + "|" + dept;
        var reportCode = "FN-00024";
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
        <input type="hidden" id="hdnParamedicID" runat="server" value="" />
        <input type="hidden" id="hdnTanggalDari" runat="server" value="" />
        <input type="hidden" id="hdnTanggalSampai" runat="server" value="" />
        <input type="hidden" id="hdnTanggalDariString" runat="server" value="" />
        <input type="hidden" id="hdnTanggalSampaiString" runat="server" value="" />
        <input type="hidden" id="hdnDepartmentID" runat="server" value="" />
        <table>
            <colgroup>
                <col style="width: 100px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Tanggal")%></label>
                </td>
                <td>
                    <table>
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 5px" />
                            <col style="width: 150px" />
                        </colgroup>
                        <tr>
                            <td class="tdCustomDate">
                                <asp:TextBox ID="txtValueDateFrom" CssClass="txtValueDateFrom datepicker" runat="server"
                                    Width="120px" />
                            </td>
                            <td>
                                <label class="blNormal">
                                    <%=GetLabel("s/d")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtValueDateTo" CssClass="txtValueDateTo datepicker" runat="server"
                                    Width="120px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <label class="lblMandatory">
                        <%=GetLabel("Department")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" Width="100%"
                        runat="server">
                        <ClientSideEvents ValueChanged="function(s,e){ onCboDepartmentValueChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpRevenueSharing" runat="server" Width="100%" ClientInstanceName="cbpRevenueSharing"
            ShowLoadingPanel="false" OnCallback="cbpRevenueSharing_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    onPrintPreparationRevenueSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
