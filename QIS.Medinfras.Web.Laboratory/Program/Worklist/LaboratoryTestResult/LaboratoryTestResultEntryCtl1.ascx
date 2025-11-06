<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LaboratoryTestResultEntryCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.Laboratory.Program.LaboratoryTestResultEntryCtl1" %>
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

        $editCommentRow = null;
        $selectedCommentRow = null;


        $('.chkResultItem input').die('change');
        $('.chkResultItem input').live('change', function () {
            var isChecked = $(this).is(":checked");
            if (isChecked) {
                $(this).closest('tr').find('.txtMetricValue').removeAttr("readonly");
                $(this).closest('tr').find('.txtNormalValueText').removeAttr("readonly");
                $(this).closest('tr').find('.chkIsDeleted').removeAttr("disabled");
                $(this).closest('tr').find('.cboResultFlag').removeAttr("disabled");
                $(this).closest('tr').find('.divResultText').show();
                $(this).closest('tr').find('.divIsPendingResult').show();
                $(this).closest('tr').find('.divResultComment').show();
            }
            else {
                $(this).closest('tr').find('.txtMetricValue').attr("readonly", "readonly");
                $(this).closest('tr').find('.txtNormalValueText').attr("readonly", "readonly");
                $(this).closest('tr').find('.chkIsPendingResult').attr("disabled", true);
                $(this).closest('tr').find('.chkIsDeleted').attr("disabled", true);
                $(this).closest('tr').find('.cboResultFlag').attr("disabled", true);
                $(this).closest('tr').find('.divResultText').hide();
                $(this).closest('tr').find('.divIsPendingResult').hide();
                $(this).closest('tr').find('.divResultComment').hide();
            }
        });

        $('.txtMetricValue').die('change');
        $('.txtMetricValue').live('change', function () {
            var row = $(this).closest('tr');
            var value = $(this).val();
            var minValue = parseFloat(row.find('.hdnMetricUnitMin').val());
            var maxValue = parseFloat(row.find('.hdnMetricUnitMax').val());
            if (isNaN(minValue) == false && isNaN(maxValue) == false) {
                if (value >= minValue && value <= maxValue) {
                    row.find('.cboResultFlag').val("N");
                }
                else {
                    if (value < minValue) {
                        row.find('.cboResultFlag').val("L");
                    }
                    else {
                        if (value > maxValue) {
                            row.find('.cboResultFlag').val("H");
                        }
                    }
                }
            }
        });


        $('.chkResultItem input').each(function () {
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

        $('#lblResultComment.lblLink').click(function () {
            if ($(this).closest('tr').find('.hdnIsVerified').val() == "False") {
                $selectedCommentRow = $(this).parent().closest('tr');
                $editCommentRow = $selectedCommentRow;
                $('#txtRemarks').val($selectedCommentRow.find('.hdnRemarks').val());
                pcRemarks.Show();
            }
            else {
                $('#<%=contentResult.ClientID %>').html($(this).closest('tr').find('.hdnRemarks').val());
                pcVerifiedResult.Show();
            }

        });

        $('#btnRemarksApply').click(function () {
            $editCommentRow.find('.hdnRemarks').val($('#txtRemarks').val());
            pcRemarks.Hide();
        });
    });

    function getCheckedResult() {
        var lstSelectedResult = '';
        var result = '';
        $('#<%=lvwView.ClientID %> .chkResultItem input').each(function () {
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
        $('.chkResultItem').each(function () {
            $chk = $(this).find('input');
            if (!$chk.is(":disabled")) {
                $chk.prop('checked', isChecked);
                if (isChecked) {
                    $(this).closest('tr').find('.txtMetricValue').removeAttr("readonly");
                    $(this).closest('tr').find('.txtNormalValueText').removeAttr("readonly");
                    $(this).closest('tr').find('.chkIsPendingResult').removeAttr("disabled");
                    $(this).closest('tr').find('.chkIsDeleted').removeAttr("disabled");
                    $(this).closest('tr').find('.cboResultFlag').removeAttr("disabled");
                    $(this).closest('tr').find('.divResultText').show();
                    $(this).closest('tr').find('.divIsPendingResult').show();
                    $(this).closest('tr').find('.divResultComment').show(); 
                }
                else {
                    $(this).closest('tr').find('.txtMetricValue').attr("readonly", "readonly");
                    $(this).closest('tr').find('.txtNormalValueText').attr("readonly", "readonly");
                    $(this).closest('tr').find('.chkIsPendingResult').attr("disabled", true);
                    $(this).closest('tr').find('.chkIsDeleted').attr("disabled", true);
                    $(this).closest('tr').find('.cboResultFlag').attr("disabled", true);
                    $(this).closest('tr').find('.divResultText').hide();
                    $(this).closest('tr').find('.divIsPendingResult').hide();
                    $(this).closest('tr').find('.divResultComment').hide(); 
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
                                                <th style="width: 30px" align="center">
                                                    <input id="chkSelectAllPatient" type="checkbox" />
                                                </th>
                                                <th style="width: 120px" align="left">
                                                    <%=GetLabel("PEMERIKSAAN")%>
                                                </th>
                                                <th style="width: 40px" align="left">
                                                    <%=GetLabel("UNIT")%>
                                                </th>
                                                <th style="width: 80px" align="center">
                                                    <%=GetLabel("HASIL")%>
                                                </th>
                                                <th style="width: 80px" align="left">
                                                    <%=GetLabel("NORMAL RANGE")%>
                                                </th>
                                                <th style="width: 80px" align="left">
                                                    <%=GetLabel("PANIC RANGE")%>
                                                </th>
                                                <th style="width: 50px" align="center">
                                                    <%=GetLabel("FLAG")%>
                                                </th>
                                                <th style="width: 30px" align="center">
                                                    <%=GetLabel("PENDING HASIL")%>
                                                </th>
                                                <th style="width: 60px" align="center">
                                                    <%=GetLabel("COMMENT")%>
                                                </th>
                                                <th style="width: 30px" align="center">
                                                    <%=GetLabel("BATAL")%>
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
                                                <th style="width: 30px" align="center">
                                                    <input id="chkSelectAllPatient" type="checkbox" />
                                                </th>
                                                <th style="width: 120px" align="left">
                                                    <%=GetLabel("PEMERIKSAAN")%>
                                                </th>
                                                <th style="width: 40px" align="left">
                                                    <%=GetLabel("UNIT")%>
                                                </th>
                                                <th style="width: 80px" align="center">
                                                    <%=GetLabel("HASIL")%>
                                                </th>
                                                <th style="width: 80px" align="left">
                                                    <%=GetLabel("NORMAL RANGE")%>
                                                </th>
                                                <th style="width: 80px" align="left">
                                                    <%=GetLabel("PANIC RANGE")%>
                                                </th>
                                                <th style="width: 50px" align="center">
                                                    <%=GetLabel("FLAG")%>
                                                </th>
                                                <th style="width: 30px" align="center">
                                                    <%=GetLabel("PENDING HASIL")%>
                                                </th>
                                                <th style="width: 60px" align="center">
                                                    <%=GetLabel("COMMENT")%>
                                                </th>
                                                <th style="width: 30px" align="center">
                                                    <%=GetLabel("BATAL")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkResultItem" runat="server" CssClass="chkResultItem" />
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("FractionID")%>' />
                                                <input type="hidden" id="hdnInternationalUnitMax" runat="server" value='<%#: Eval("InternationalUnitMax")%>' />
                                                <input type="hidden" id="hdnInternationalUnitMin" runat="server" value='<%#: Eval("InternationalUnitMin")%>' />
                                                <input type="hidden" class="hdnMetricUnitMin" id="hdnMetricUnitMin" runat="server" value='<%#: Eval("MetricUnitMin")%>' />
                                                <input type="hidden" class="hdnMetricUnitMax" id="hdnMetricUnitMax" runat="server" value='<%#: Eval("MetricUnitMax")%>' />
                                                <input type="hidden" id="hdnGCMetricUnit" runat="server" value='<%#: Eval("GCMetricUnit")%>' />
                                                <input type="hidden" id="hdnGCInternationalUnit" runat="server" value='<%#: Eval("GCInternationalUnit")%>' />
                                                <input type="hidden" class="hdnUnitConversion" id="hdnUnitConversion" runat="server"
                                                    value='<%#: Eval("ConversionFactor")%>' />
                                                <input type="hidden" class="hdnIsNumeric" id="hdnIsNumeric" runat="server" value='<%#: Eval("IsNumericResult")%>' />
                                                <input type="hidden" class="hdnTextResult" id="hdnTextResult" runat="server" value='<%#: Eval("TextValue")%>' />
                                                <input type="hidden" class="hdnIsResultInPDF" id="hdnIsResultInPDF" runat="server" value='<%#: Eval("IsResultInPDF")%>' />
                                                <input type="hidden" class="hdnIsVerified" id="hdnIsVerified" runat="server" value='<%# Eval("IsVerified")%>' />
                                                <input type="hidden" class="hdnIsNormal" id="hdnIsNormal" runat="server" value='<%# Eval("isNormal")%>' />
                                                <input type="hidden" class="hdnRemarks" id="hdnRemarks" runat="server" value='<%#: Eval("Remarks")%>' />
                                            </td>
                                            <td>
                                                <%#: Eval("FractionName1")%>
                                            </td>
                                            <td>
                                                <%#: Eval("MetricUnit")%>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMetricValue" Text="0" Width="100%" runat="server" CssClass="txtMetricValue number" />
                                                <div id="divTextResult" runat="server">
                                                    <asp:Label class="lblLink lblTextResult" id="lblTextResult" runat="server" Text="Hasil Text" />
                                                   
                                                    </div>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("MetricRangeLabel")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("PanicMetricRangeLabel")%>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList runat="server" ID="cboResultFlag" CssClass="cboResultFlag" Width="100%"></asp:DropDownList>
                                            </td>
                                            <td align="center">
                                                <div id="divIsPendingResult" class="divIsPendingResult" runat="server">
                                                     <asp:CheckBox ID="chkIsPendingResult" runat="server" CssClass="chkIsPendingResult" />
                                                </div>
                                            </td>
                                            <td align="center">
                                                <div id="divResultComment" class="divResultComment" runat="server">
                                                    <label class="lblLink" id="lblResultComment">
                                                        <%=GetLabel("Comment")%></label></div>
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsDeleted" runat="server" CssClass="chkIsDeleted" />
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
    <div style="width: 100%; text-align: right; display:none">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
    <dxpc:ASPxPopupControl ID="pcResult" runat="server" ClientInstanceName="pcResult"
        CloseAction="CloseButton" Height="500px" HeaderText="Entry Hasil Text" Width="600px"
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
                                                            <label class="lblNormal lblTestResult lblLink" id="lblTestResult">
                                                                Template Hasil</label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox TextMode="MultiLine" Width="100%" Height="450px" ID="txtTestResult"
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
                                                <input type="button" id="btnResultSave" value='APPLY' class="w3-btn w3-hover-blue" style="width: 80px" />
                                            </td>
                                            <td>
                                                <input type="button" id="btnResultCancel" value='CANCEL' class="w3-btn w3-hover-blue" style="width: 80px" onclick="pcResult.Hide();" />
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
        CloseAction="CloseButton" Height="300px" HeaderText="Entry Hasil Text" Width="600px"
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

    <dxpc:ASPxPopupControl ID="pcRemarks" runat="server" ClientInstanceName="pcRemarks"
    CloseAction="CloseButton" Height="300px" HeaderText="Entry Comment" Width="600px"
    Modal="True" PopupAction="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
    <ContentCollection>
        <dxpc:PopupControlContentControl runat="server" ID="PopupControlContentControl1">
            <dx:ASPxPanel ID="pnlRemarks" runat="server" Width="100%">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent4" runat="server">
                        <fieldset id="Fieldset1" style="margin: 0">
                            <div style="text-align: left; width: 100%;">
                                <table width="100%" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col width="150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel" valign="top">
                                            <label class="lblNormal lblTestRemarks" id="lblTestRemarks">
                                                Comment/Remarks</label>
                                        </td>
                                        <td>
                                            <asp:TextBox TextMode="MultiLine" Width="100%" Height="200px" ID="txtRemarks"
                                                runat="server" ClientIDMode="Static" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table style="margin-left: auto; margin-right: auto; margin-top: 10px;">
                                                <tr>
                                                    <td>
                                                        <input type="button" id="btnRemarksApply" value='APPLY' class="w3-btn w3-hover-blue" style="width: 80px" />
                                                    </td>
                                                    <td>
                                                        <input type="button" id="btnRemarksCancel" value='CANCEL' class="w3-btn w3-hover-blue" style="width: 80px" onclick="pcRemarks.Hide();" />
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
