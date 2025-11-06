<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalculateCoverageLimitCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.CalculateCoverageLimitCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_recalculatecoveragelimitctl">

    $('.txtPatientAmount').live('change', function () {
        $tr = $(this).closest('tr');
        var txtPatientAmount = parseFloat($tr.find('.txtPatientAmount').val());
        var txtPayerAmount = parseFloat($tr.find('.txtPayerAmount').val());
        var LineAmount = parseFloat($tr.find('.LineAmount').val());

        var payerNew = LineAmount - txtPatientAmount;

        $tr.find('.txtPayerAmount').val(payerNew).trigger('changeValue');
    });

    $('.txtPayerAmount').live('change', function () {
        $tr = $(this).closest('tr');
        var txtPatientAmount = parseFloat($tr.find('.txtPatientAmount').val());
        var txtPayerAmount = parseFloat($tr.find('.txtPayerAmount').val());
        var LineAmount = parseFloat($tr.find('.LineAmount').val());

        var patientNew = LineAmount - txtPayerAmount;

        $tr.find('.txtPatientAmount').val(patientNew).trigger('changeValue');
    });

    function getCheckedMemberCoverage() {
        var hdnSelectedBillingGroupID = $('#<%=hdnSelectedBillingGroupID.ClientID %>').val().split(',');
        var hdnSelectedPatientAmount = $('#<%=hdnSelectedPatientAmount.ClientID %>').val().split(',');
        var hdnSelectedPayerAmount = $('#<%=hdnSelectedPayerAmount.ClientID %>').val().split(',');
        var hdnSelectedLineAmount = $('#<%=hdnSelectedLineAmount.ClientID %>').val().split(',');

        $('.chkCoverage input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');
                var BillingGroupID = $tr.find('.BillingGroupID').val();
                var txtPatientAmount = $tr.find('.txtPatientAmount').val();
                var txtPayerAmount = $tr.find('.txtPayerAmount').val();
                var txtLineAmount = $tr.find('.LineAmount').val();
                var idx = hdnSelectedBillingGroupID.indexOf(BillingGroupID);
                if (idx < 0) {
                    hdnSelectedBillingGroupID.push(BillingGroupID);
                    hdnSelectedPatientAmount.push(txtPatientAmount);
                    hdnSelectedPayerAmount.push(txtPayerAmount);
                    hdnSelectedLineAmount.push(txtLineAmount);
                }
                else {
                    hdnSelectedPatientAmount[idx] = txtPatientAmount;
                    hdnSelectedPayerAmount[idx] = txtPayerAmount;
                    hdnSelectedLineAmount[idx] = txtLineAmount;
                }
            }
            else {
                var BillingGroupID = $(this).closest('tr').find('.BillingGroupID').val();
                var idx = hdnSelectedBillingGroupID.indexOf(BillingGroupID);
                if (idx > -1) {
                    hdnSelectedBillingGroupID.splice(idx, 1);
                    hdnSelectedPatientAmount.splice(idx, 1);
                    hdnSelectedPayerAmount.splice(idx, 1);
                    hdnSelectedLineAmount.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedBillingGroupID.ClientID %>').val(hdnSelectedBillingGroupID.join(','));
        $('#<%=hdnSelectedPatientAmount.ClientID %>').val(hdnSelectedPatientAmount.join(','));
        $('#<%=hdnSelectedPayerAmount.ClientID %>').val(hdnSelectedPayerAmount.join(','));
        $('#<%=hdnSelectedLineAmount.ClientID %>').val(hdnSelectedLineAmount.join(','));
    }

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkCoverage').each(function () {
            $chk = $(this).find('input');
            if (!$chk.is(":disabled")) {
                $chk.prop('checked', isChecked);
            }
        });
    });

    $('#btnProcess').click(function (evt) {
        getCheckedMemberCoverage();
        if ($('#<%=hdnSelectedBillingGroupID.ClientID %>').val() == '') {
            errMessage.text = 'Please Select Billing Group First !';
        } else {
            cbpProcessDetail.PerformCallback('calculate');
        }
    });

    function onCbpProcessDetailEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        var retval = s.cpRetval.split('|');
        if (param[0] == 'calculate') {
            if (param[1] == 'fail') {
                var messageBody = param[2];
                displayMessageBox('ERROR', messageBody);
            }
            else {
                pcRightPanelContent.Hide();
            }
        }
    }
</script>
<div style="height: 400px; overflow-y: auto; overflow-x: hidden" id="containerPopup">
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedBillingGroupID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedPatientAmount" runat="server" value="" />
    <input type="hidden" id="hdnSelectedPayerAmount" runat="server" value="" />
    <input type="hidden" id="hdnSelectedLineAmount" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                    ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpProcessDetailEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 40px" align="center">
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </th>
                                                <th style="width: 180px">
                                                    <%=GetLabel("Service Unit Visit")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("Billing Group")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Patient Amount")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Payer Amount")%>
                                                </th>
                                                <th style="width: 200px" align="right">
                                                    <%=GetLabel("Line Amount")%>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="10">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 40px" align="center">
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </th>
                                                <th style="width: 180px">
                                                    <%=GetLabel("Service Unit Visit")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("Billing Group")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Patient Amount")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Payer Amount")%>
                                                </th>
                                                <th style="width: 200px" align="right">
                                                    <%=GetLabel("Line Amount")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkCoverage" runat="server" CssClass="chkCoverage" Checked="true" />
                                                <input type="hidden" class="BillingGroupID" id="BillingGroupID" runat="server" value='<%#: Eval("BillingGroupID")%>' />
                                                <input type="hidden" class="LineAmount" id="LineAmount" runat="server" value='<%#: Eval("LineAmount")%>' />
                                            </td>
                                            <td>
                                                <b><%#: Eval("FromDepartmentID")%></b>
                                                <br />
                                                <%#: Eval("FromServiceUnitName")%>
                                            </td>
                                            <td>
                                                <b><%#: Eval("BillingGroupCode")%></b>
                                                <br />
                                                <%#: Eval("BillingGroupName1")%>
                                            </td>
                                            <td align="right">
                                                <input type="text" runat="server" id="txtPatientAmount" class="txtPatientAmount number"
                                                    style="width: 100%" />
                                            </td>
                                            <td align="right">
                                                <input type="text" runat="server" id="txtPayerAmount" class="txtPayerAmount number"
                                                    style="width: 100%" />
                                            </td>
                                            <td align="right">
                                                <asp:TextBox runat="server" ID="txtLineAmount" CssClass="txtLineAmount txtCurrency"
                                                    Width="150px" ReadOnly="true" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="containerPaging" style="display: none">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: left">
        <input type="button" id="btnProcess" value='<%= GetLabel("Process")%>' />
    </div>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
