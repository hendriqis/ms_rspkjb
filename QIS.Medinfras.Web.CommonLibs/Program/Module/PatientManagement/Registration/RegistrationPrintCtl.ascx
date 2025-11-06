<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegistrationPrintCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.RegistrationPrintCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

<script type="text/javascript" id="dxss_registrationprintctl">
    $('#<%=btnRegistrationPrint.ClientID %>').click(function () {
        $rbo = $('input[name=rboRegistrationPrintCtl]:checked');
        if ($rbo.length > 0) {
            var filterExpression = { text: "" };
            var errMessage = { text: "" };
            var reportCode = $rbo.attr('reportcode');
            if (reportCode != '') {
                var isAllowPrint = true;
                if (typeof onBeforeRightPanelPrint == 'function') {
                    isAllowPrint = onBeforeRightPanelPrint(reportCode, filterExpression, errMessage);
                }
                if (isAllowPrint) {
                    openReportViewerCtl(reportCode, filterExpression.text);
                }
                else
                    showToast('Warning', errMessage.text);
            }
        }
    });

    //#region Report Viewer
    function openReportViewerCtl(reportCode, param) {
        var filterCheckParam = "ParameterCode = '" + 'OP0014' + "'";
        var isUsingPDClient = "0";
        Methods.getObject('GetSettingParameterDtList', filterCheckParam, function (result) {
            if (result != null) {
                if (result.ParameterValue == "1") {
                    isUsingPDClient = "1";
                }
            }
        });

        var filterExpression = "ReportCode = '" + reportCode + "'";
        Methods.getObject('GetReportMasterList', filterExpression, function (result) {
            if (result != null) {
                if (result.IsDirectPrint) {
                    $('#<%=hdnReportCode.ClientID %>').val(reportCode);
                    if (isUsingPDClient != "1") {
                        cbpDirectPrintProcessCtl.PerformCallback(param);
                    }
                    else 
                    {
                        cbpDirectPrintProcessCtlDirect.PerformCallback(param);
                    }
                }
                else {
                    var isShowError = false;
                    window.setTimeout(function () {
                        showLoadingPanel();
                        var e = new Image(1, 1);
                        e.onerror = function () {
                            if (!isShowError) {
                                isShowError = true;

                                var win = window.open("", reportCode.replace('-', ''), 'status=1,toolbar=0,menubar=0,resizable=1,location=0,scrollbars=1,width=1150,height=600');
                                win.focus();

                                var mapForm = document.createElement("form");
                                mapForm.target = reportCode.replace('-', '');
                                mapForm.method = "POST";
                                mapForm.action = ResolveUrl('~/Libs/Program/ReportViewer.aspx?id=' + reportCode);

                                var mapInput = document.createElement("input");
                                mapInput.type = "hidden";
                                mapInput.name = "param";
                                mapInput.value = param;
                                mapForm.appendChild(mapInput);

                                document.body.appendChild(mapForm);

                                mapForm.submit();

                                $(mapForm).remove();
                            }
                        };
                        e.src = "http://localhost:60025/buffertext/dummy.gif?healthcareid=<%=HealthcareID %>&userid=<%=UserID %>&username=<%=UserName %>&userfullname=<%=UserFullName %>&type=report&reportid=" + reportCode + "&param=" + param + "&timestamp=" + new Date().getTime();
                        window.setTimeout(function () {
                            hideLoadingPanel();
                        }, 5000);
                        void 0;
                    }, 0);
                }
            }
            else
                showToast("Warning", "Report Dengan Kode <b>" + reportCode + "</b> Tidak Ditemukan");
        });
    }
    //#endregion

    $('.rboRegistrationPrintCtl').change(function () {
        if ($(this).attr('isDisplayPrintCount') == '1') {
            $('#divPrinterLocationCtl').show();
            if ($(this).attr('reportcode') != 'PM-00105' && $(this).attr('reportcode') != 'PM-00131') {
                $('#divJmlLabelCtl').hide();
            }
            else {
                $('#divJmlLabelCtl').show();
                $('#txtJmlLabelCtl').focus();
            }
        }
        else {
            $('#divJmlLabelCtl').hide();
            $('#divPrinterLocationCtl').hide();
        }
    });

    function onCbpDirectPrintProcessCtlDirectEndCallback(s) {
        hideLoadingPanel();
        var value = s.cpZebraPrinting;
        if (value != "") {
            var e_id = 'id_' + new Date().getTime();
            if (window.chrome) {
                $('body').append('<a id=\"' + e_id + '\"></a>');
                $('#' + e_id).attr('href', 'PDClient:' + value);
                var a = $('a#' + e_id)[0];
                var evObj = document.createEvent('MouseEvents');
                evObj.initEvent('click', true, true);
                a.dispatchEvent(evObj)
            } else {
                $('body').append('<iframe name=\"' + e_id + '\" id=\"' + e_id + '\" width=\"1\" height=\"1\" style=\"visibility:hidden;position:absolute\" />');
                $('#' + e_id).attr('src', 'PDClient:' + value)
            }
            setTimeout(function () {
                $('#' + e_id).remove()
            }, 5000)
        }
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnRegistrationPrint" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Print")%></div>
        </li>
    </ul>
</div>

<input type="hidden" id="hdnReportCode" runat="server" value="" />
<div id="divJmlLabelCtl" style="display:none">
    <input type="hidden" id="hdnIsMultiLocationCtl" value="0" runat="server" />
    <input type="hidden" id="hdnMaxLabelNoCtl" value="20" runat="server" />
    <table border="0" cellpadding="0" cellspacing="0">
        <colgroup>
            <col style="width:100px" />
            <col />
        </colgroup>
        <tr>
            <td >Jumlah Label</td>
            <td style="padding-left:3px"><asp:TextBox runat="server" ID="txtJmlLabelCtl" Text="1" Width="95%" TextMode="Number"></asp:TextBox></td>
        </tr>
    </table>                           
</div>
<div id="divPrinterLocationCtl" style="display:none">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>       
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="width:100px;">Lokasi Printer</td>
                        <td><dxe:ASPxComboBox ID="cboPrinterUrlCtl" ClientInstanceName="cboPrinterUrlCtl" Width="100%" runat="server"> <ClientSideEvents ValueChanged="function(s,e){ onCboPrinterUrlCtlValueChanged(s); }" /> </dxe:ASPxComboBox></td>
                    </tr>
                </table>                                 
            </td>
        </tr>
    </table>
</div>
<asp:Repeater ID="rptPrint" runat="server">
    <ItemTemplate>
        <input type="radio" class="rboRegistrationPrintCtl" name="rboRegistrationPrintCtl" value="1" reportcode='<%#: Eval("ReportCode")%>' isDisplayPrintCount='<%#: Eval("isDisplayPrintCount")%>' /><%#: Eval("Title")%><br />
    </ItemTemplate>
</asp:Repeater>
<div style="display: none">
    <dxcp:ASPxCallbackPanel ID="cbpDirectPrintProcessCtl" runat="server" Width="100%" ClientInstanceName="cbpDirectPrintProcessCtl"
        ShowLoadingPanel="false" OnCallback="cbpDirectPrintProcessCtl_Callback">
        <ClientSideEvents BeginCallback="function(s,e){
            showLoadingPanel();
        }" EndCallback="function(s,e){         
            var printResult = s.cpZebraPrinting;
            if (printResult != '')
                showToast('Warning', printResult);
            hideLoadingPanel();}" />
    </dxcp:ASPxCallbackPanel>

    <dxcp:ASPxCallbackPanel ID="cbpDirectPrintProcessCtlDirect" runat="server" Width="100%" ClientInstanceName="cbpDirectPrintProcessCtlDirect"
        ShowLoadingPanel="false" OnCallback="cbpDirectPrintProcessCtlDirect_Callback">
        <ClientSideEvents BeginCallback="function(s,e){
            showLoadingPanel();
        }" EndCallback="function(s,e){ onCbpDirectPrintProcessCtlDirectEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
