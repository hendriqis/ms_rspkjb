<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LaboratoryTestResultEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Laboratory.Program.LaboratoryTestResultEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<script type="text/javascript" id="dxss_laboratorytestresultentryctl">
    $row = null;
    $(function () {
        $row = $(this).closest('tr');
        $editRow = null;
        $selectedRow = null;

        $('.chkPatient input').die('change');
        $('.chkPatient input').live('change', function () {
            var isChecked = $(this).is(":checked");
            if (isChecked) {
                $(this).closest('tr').find('.txtMetricValue').removeAttr("readonly");
                $(this).closest('tr').find('.txtInternationalValue').removeAttr("readonly");
                $(this).closest('tr').find('.txtNormalValueText').removeAttr("readonly");
                $(this).closest('tr').find('.chkIsDeleted').removeAttr("disabled");
                $(this).closest('tr').find('.chkIsNormal').removeAttr("disabled");
            }
            else {
                $(this).closest('tr').find('.txtMetricValue').attr("readonly", "readonly");
                $(this).closest('tr').find('.txtInternationalValue').attr("readonly", "readonly");
                $(this).closest('tr').find('.txtNormalValueText').attr("readonly", "readonly");
                $(this).closest('tr').find('.chkIsDeleted').attr("disabled", true);
                $(this).closest('tr').find('.chkIsNormal').attr("disabled", true);
            }
        });

        $('.txtMetricValue').die('change');
        $('.txtMetricValue').live('change', function () {
            var row = $(this).closest('tr');
            var value = $(this).val();
            var conversionFactor = parseFloat(row.find('.hdnUnitConversion').val());
            var conversionValue = (value * conversionFactor).toFixed(2);
            row.find('.txtInternationalValue').val(conversionValue);
        });

        $('.txtInternationalValue').die('change');
        $('.txtInternationalValue').live('change', function () {
            var row = $(this).closest('tr');
            var value = $(this).val();
            var conversionFactor = parseFloat(row.find('.hdnUnitConversion').val());
            var conversionValue = (value / conversionFactor).toFixed(2);
            row.find('.txtMetricValue').val(conversionValue);
        });

        $('.chkPatient input').each(function () {
            $(this).change();
        });

        function getTemplateTextExpression() {
            var filterExpression = "GCTemplateGroup = '<%=GCTemplateGroup %>' AND IsDeleted = 0";
            filterExpression += " AND TemplateID IN (SELECT a.TemplateID FROM PhysicianTemplateText a WHERE a.ParamedicID = " + $('#<%=hdnParamedicIDResultDt.ClientID %>').val() + ")";
            return filterExpression;
        }

        $('#lblTestResult.lblLink').click(function () {
            openSearchDialog('templatetext', getTemplateTextExpression(), function (value) {
                var filterExpression = getTemplateTextExpression() + " AND TemplateCode = '" + value + "'";
                Methods.getObjectValue('GetTemplateTextList', filterExpression, "TemplateContent", function (result) {
                    tinyMCE.get('<%=txtTestResult.ClientID %>').execCommand('mceSetContent', false, result);
                });
            });
        });

        setHtmlEditor();

        $('.lblTextResult.lblLink').click(function () {
            if ($(this).closest('tr').find('.hdnIsVerified').val() == "False") {
                $selectedRow = $(this).parent().closest('tr');
                $editRow = $selectedRow;
                if ($selectedRow.find('.hdnIsResultInPDF').val() == "True") {
                    var textResultValue = $selectedRow.find('.hdnTextResult').val();
                    window.open("data:application/pdf;base64, " + textResultValue, "popupWindow", "width=600, height=600,scrollbars=yes");
                }
                else {
                    tinyMCE.get('<%=txtTestResult.ClientID %>').execCommand('mceSetContent', false, $selectedRow.find('.hdnTextResult').val());
                    pcResult.Show();
                }
            }
            else {
                $('#<%=contentResult.ClientID %>').html($(this).closest('tr').find('.hdnTextResult').val());
                pcVerifiedResult.Show();
            }

        });

        $('#btnResultSave').click(function () {
            var result = $('#txtTestResult').val().replaceAll("&amp;", "&").replaceAll("&lt;", "<").replaceAll("&gt;", ">").replaceAll("&quot;", '"').replaceAll("&apos;", "'");
            $editRow.find('.hdnTextResult').val(result);
            pcResult.Hide();
        });
    });

    function getCheckedResult() {
        var lstSelectedResult = '';
        var result = '';
        $('#<%=lvwView.ClientID %> .chkPatient input').each(function () {
            if ($(this).is(':checked')) {
                var key = $(this).closest('tr').find('.keyField').html();
                if (lstSelectedResult != '')
                    lstSelectedResult += ',';
                lstSelectedResult += key;
            }
        });
        $('#<%=hdnSelectedVisit.ClientID %>').val(lstSelectedResult);
    }

    $('#chkSelectAllPatient').die('change');
    $('#chkSelectAllPatient').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkPatient').each(function () {
            $chk = $(this).find('input');
            if (!$chk.is(":disabled")) {
                $chk.prop('checked', isChecked);
                if (isChecked) {
                    $(this).closest('tr').find('.txtMetricValue').removeAttr("readonly");
                    $(this).closest('tr').find('.txtInternationalValue').removeAttr("readonly");
                    $(this).closest('tr').find('.txtNormalValueText').removeAttr("readonly");
                    $(this).closest('tr').find('.chkIsDeleted').removeAttr("disabled");
                    $(this).closest('tr').find('.chkIsNormal').removeAttr("disabled");
                }
                else {
                    $(this).closest('tr').find('.txtMetricValue').attr("readonly", "readonly");
                    $(this).closest('tr').find('.txtInternationalValue').attr("readonly", "readonly");
                    $(this).closest('tr').find('.txtNormalValueText').attr("readonly", "readonly");
                    $(this).closest('tr').find('.chkIsDeleted').attr("disabled", true);
                    $(this).closest('tr').find('.chkIsNormal').attr("disabled", true);
                }
            }
        });
    });
</script>
<div style="height: 440px; width: 100%; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnSelectedVisit" runat="server" value="" />
    <input type="hidden" id="hdnVisitID" value="" runat="server" />
    <input type="hidden" id="hdnItemID" value="" runat="server" />
    <input type="hidden" id="hdnLabResultID" value="" runat="server" />
    <input type="hidden" id="hdnParamedicIDResultDt" value="" runat="server" />
    <input type="hidden" id="hdnTransactionID" value="" runat="server" />
    <input type="hidden" id="hdnIsNormal" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                    ShowLoadingPanel="false">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 1em;">
                                <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 30px" rowspan="2" align="center">
                                                    <input id="chkSelectAllPatient" type="checkbox" />
                                                </th>
                                                <th style="width: 120px" rowspan="2" align="left">
                                                    <%=GetLabel("PEMERIKSAAN")%>
                                                </th>
                                                <th colspan="3" align="center">
                                                    <%=GetLabel("CONVENTIONAL")%>
                                                </th>
                                                <th colspan="3" align="center">
                                                    <%=GetLabel("INTERNATIONAL (SI)")%>
                                                </th>
                                                <th style="width: 80px" align="center" rowspan="2">
                                                    <%=GetLabel("HASIL TEXT")%>
                                                </th>
                                                <th style="width: 30px" rowspan="2" align="center">
                                                    <%=GetLabel("BATAL")%>
                                                </th>
                                                <th style="width: 30px" rowspan="2" align="center">
                                                    <%=GetLabel("NORMAL")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 60px" align="center">
                                                    <%=GetLabel("NILAI")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("NORMAL RANGE")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("PANIC RANGE")%>
                                                </th>
                                                <th style="width: 60px" align="center">
                                                    <%=GetLabel("NILAI")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("NORMAL RANGE")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("PANIC RANGE")%>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="11">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 30px" rowspan="2" align="center">
                                                    <input id="chkSelectAllPatient" type="checkbox" />
                                                </th>
                                                <th style="width: 120px" rowspan="2" align="left">
                                                    <%=GetLabel("PEMERIKSAAN")%>
                                                </th>
                                                <th colspan="3" align="center">
                                                    <%=GetLabel("CONVENTIONAL")%>
                                                </th>
                                                <th colspan="3" align="center">
                                                    <%=GetLabel("INTERNATIONAL (SI)")%>
                                                </th>
                                                <th style="width: 80px" align="center" rowspan="2">
                                                    <%=GetLabel("HASIL TEXT")%>
                                                </th>
                                                <th style="width: 30px" rowspan="2" align="center">
                                                    <%=GetLabel("BATAL")%>
                                                </th>
                                                <th style="width: 30px" rowspan="2" align="center">
                                                    <%=GetLabel("NORMAL")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 60px" align="center">
                                                    <%=GetLabel("NILAI")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("NORMAL RANGE")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("PANIC RANGE")%>
                                                </th>
                                                <th style="width: 60px" align="center">
                                                    <%=GetLabel("NILAI")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("NORMAL RANGE")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("PANIC RANGE")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkPatient" runat="server" CssClass="chkPatient" />
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("FractionID")%>' />
                                                <input type="hidden" id="hdnInternationalUnitMax" runat="server" value='<%#: Eval("InternationalUnitMax")%>' />
                                                <input type="hidden" id="hdnInternationalUnitMin" runat="server" value='<%#: Eval("InternationalUnitMin")%>' />
                                                <input type="hidden" id="hdnMetricUnitMin" runat="server" value='<%#: Eval("MetricUnitMin")%>' />
                                                <input type="hidden" id="hdnMetricUnitMax" runat="server" value='<%#: Eval("MetricUnitMax")%>' />
                                                <input type="hidden" id="hdnGCMetricUnit" runat="server" value='<%#: Eval("GCMetricUnit")%>' />
                                                <input type="hidden" id="hdnGCInternationalUnit" runat="server" value='<%#: Eval("GCInternationalUnit")%>' />
                                                <input type="hidden" class="hdnUnitConversion" id="hdnUnitConversion" runat="server"
                                                    value='<%#: Eval("ConversionFactor")%>' />
                                                <input type="hidden" class="hdnIsNumeric" id="hdnIsNumeric" runat="server" value='<%#: Eval("IsNumericResult")%>' />
                                                <input type="hidden" class="hdnTextResult" id="hdnTextResult" runat="server" value='<%#: Eval("TextValue")%>' />
                                                <input type="hidden" class="hdnIsResultInPDF" id="hdnIsResultInPDF" runat="server" value='<%#: Eval("IsResultInPDF")%>' />
                                                <input type="hidden" class="hdnIsVerified" id="hdnIsVerified" runat="server" value='<%# Eval("IsVerified")%>' />
                                                <input type="hidden" class="hdnIsNormal" id="hdnIsNormal" runat="server" value='<%# Eval("isNormal")%>' />
                                            </td>
                                            <td>
                                                <%#: Eval("FractionName1")%>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblMetricValue" runat="server" Text=""  />
                                                <asp:TextBox ID="txtMetricValue" Text="0" Width="100%" runat="server" CssClass="txtMetricValue number" />
                                            </td>
                                            <td align="right">
                                                <%#: Eval("MetricRangeLabel")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("PanicMetricRangeLabel")%>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblInternationalValue" runat="server" Text="" />
                                                <asp:TextBox ID="txtInternationalValue" Text="0" Width="100%" runat="server" CssClass="txtInternationalValue number" />
                                            </td>
                                            <td align="right">
                                                <%#: Eval("InternationalRangeLabel")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("PanicInternationalRangeLabel")%>
                                            </td>
                                            <td align="center">
                                                <div id="divTextResult" runat="server">
                                                   <%-- <label class="lblLink" id="lblTextResult"><%=GetLabel("Text Result")%></label>--%>
                                                   <asp:Label CssClass="lblTextResult lblLink" ID="lblTextResult" runat="server" Text="Text Result" />
                                                </div>
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsDeleted" runat="server" CssClass="chkIsDeleted" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsNormal" runat="server" CssClass="chkIsNormal" />
                                            </td>
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
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
    <dxpc:ASPxPopupControl ID="pcResult" runat="server" ClientInstanceName="pcResult"
        CloseAction="CloseButton" Height="300px" HeaderText="Text Result" Width="600px"
        Modal="True" PopupAction="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dxpc:PopupControlContentControl runat="server" ID="pccc1">
                <dx:ASPxPanel ID="pnlResult" runat="server" Width="100%">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <fieldset id="fsResult" style="margin: 0">
                                <div style="text-align: left; width: 100%;">
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <table>
                                                    <tr>
                                                        <td class="tdLabel">
                                                            <label class="lblNormal lblLink" id="lblTestResult">
                                                                Template Hasil</label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox TextMode="MultiLine" Width="100%" Height="300px" ID="txtTestResult"
                                                                runat="server" CssClass="htmlEditor" ClientIDMode="Static" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="margin-left: auto; margin-right: auto; margin-top: 10px;">
                                        <tr>
                                            <td>
                                                <input type="button" id="btnResultSave" value='Save' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnResultCancel" value='Cancel' onclick="pcResult.Hide();" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </fieldset>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
    <dxpc:ASPxPopupControl ID="pcVerifiedResult" runat="server" ClientInstanceName="pcVerifiedResult"
        CloseAction="CloseButton" Height="300px" HeaderText="Text Result" Width="600px"
        Modal="True" PopupAction="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dxpc:PopupControlContentControl runat="server" ID="pccc2">
                <dx:ASPxPanel ID="pnlVerifiedResult" runat="server" Width="100%">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent3" runat="server">
                            <fieldset id="fsVerifiedResult" style="margin: 0">
                                <div style="text-align: left; width: 100%;">
                                    <table>
                                        <tr>
                                            <td valign="top">
                                                <table>
                                                    <tr>
                                                        <td colspan="2">
                                                            <div id="contentResult" runat="server">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </fieldset>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</div>
