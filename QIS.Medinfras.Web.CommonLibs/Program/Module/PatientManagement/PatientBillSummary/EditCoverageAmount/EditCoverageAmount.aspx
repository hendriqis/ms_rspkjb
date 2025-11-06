<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master"
    AutoEventWireup="true" CodeBehind="EditCoverageAmount.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.EditCoverageAmount" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback('refresh');
        }

        $('#<%=btnProcess.ClientID %>').live('click', function () {
            if ($('.chkIsSelected input:checked').length < 1)
                showToast('Warning', '<%=GetErrorMsgSelectTransactionFirst() %>');
            else {
                getCheckedMember();
                onCustomButtonClick('editcoverage');
            }
        });

        function getCheckedMember() {
            var lstTransactionDtID = $('#<%=hdnSelectedTransactionDtID.ClientID %>').val().split(',');
            var lstPayerAmount = $('#<%=hdnSelectedPayerAmount.ClientID %>').val().split(',');
            var lstPatientAmount = $('#<%=hdnSelectedPatientAmount.ClientID %>').val().split(',');
            $('.chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var $tr = $(this).closest('tr');
                    var keyField = $tr.find('.keyField').val();
                    var payerAmountCheck = "";
                    var patientAmountCheck = "";
                    if ($tr.find('.txtPayerAmount').attr('hiddenVal') != undefined) {
                        payerAmountCheck = $tr.find('.txtPayerAmount').attr('hiddenVal');
                    }
                    else {
                        payerAmountCheck = $tr.find('.txtPayerAmount').val();
                    }
                    if ($tr.find('.txtPatientAmount').attr('hiddenVal') != undefined) {
                        patientAmountCheck = $tr.find('.txtPatientAmount').attr('hiddenVal');
                    }
                    else {
                        patientAmountCheck = $tr.find('.txtPatientAmount').val();
                    }
                    var payerAmount = parseFloat(parseFloat(payerAmountCheck).toFixed(2));
                    var patientAmount = parseFloat(parseFloat(patientAmountCheck).toFixed(2));
                    var idx = lstTransactionDtID.indexOf(keyField);
                    if (idx < 0) {
                        lstTransactionDtID.push(keyField);
                        lstPayerAmount.push(payerAmount);
                        lstPatientAmount.push(patientAmount);
                    }
                    else {
                        lstPayerAmount[idx] = payerAmount;
                        lstPatientAmount[idx] = patientAmount;
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').val();
                    var idx = lstTransactionDtID.indexOf(key);
                    if (idx > -1) {
                        lstTransactionDtID.splice(idx, 1);
                        lstPayerAmount.splice(idx, 1);
                        lstPatientAmount.splice(idx, 1);
                    }
                }
            });

            $('#<%=hdnSelectedTransactionDtID.ClientID %>').val(lstTransactionDtID.join(','));
            $('#<%=hdnSelectedPayerAmount.ClientID %>').val(lstPayerAmount.join(','));
            $('#<%=hdnSelectedPatientAmount.ClientID %>').val(lstPatientAmount.join(','));
        }

        $('.chkSelectAll input').die('change');
        $('.chkSelectAll input').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        $('.chkIsSelected input').die('change');
        $('.chkIsSelected input').live('change', function () {
            $tr = $(this).closest('tr');
            if ($(this).is(':checked')) {
                if ($('#<%=hdnBusinessPartnerID.ClientID %>').val() == "1") {
                    displayErrorMessageBox("Silahkan coba lagi", "Tidak dapat diproses karena penjamin bayarnya pribadi.");
                    $tr.find('.txtPayerAmount').attr('readonly', 'readonly');
                    $tr.find('.txtPatientAmount').attr('readonly', 'readonly');
                } else {
                    $tr.find('.txtPayerAmount').removeAttr('readonly');
                    $tr.find('.txtPatientAmount').removeAttr('readonly');
                }
            }
            else {
                $tr.find('.txtPayerAmount').attr('readonly', 'readonly');
                $tr.find('.txtPatientAmount').attr('readonly', 'readonly');
            }
        });

        $('.txtPayerAmount').die('change');
        $('.txtPayerAmount').live('change', function () {
            $(this).blur();
            var $tr = $(this).closest('tr');
            var payerAmountOri = parseFloat(parseFloat($tr.find('.hdnPayerAmountOri').val()).toFixed(2));
            var patientAmountOri = parseFloat(parseFloat($tr.find('.hdnPatientAmountOri').val()).toFixed(2));
            var lineAmount = parseFloat(parseFloat($tr.find('.hdnLineAmount').val()).toFixed(2));

            var payerAmount = parseFloat(parseFloat($tr.find('.txtPayerAmount').attr('hiddenVal')).toFixed(2));
            var patientAmount = parseFloat(parseFloat(lineAmount - payerAmount).toFixed(2));

            if (patientAmount < 0) {
                $tr.find('.txtPayerAmount').val(payerAmountOri).trigger('changeValue');
                $tr.find('.txtPatientAmount').val(patientAmountOri).trigger('changeValue');
            } else {
                $tr.find('.txtPatientAmount').val(patientAmount).trigger('changeValue');
            }
        });

        $('.txtPatientAmount').die('change');
        $('.txtPatientAmount').live('change', function () {
            $(this).blur();
            var $tr = $(this).closest('tr');
            var payerAmountOri = parseFloat(parseFloat($tr.find('.hdnPayerAmountOri').val()).toFixed(2));
            var patientAmountOri = parseFloat(parseFloat($tr.find('.hdnPatientAmountOri').val()).toFixed(2));
            var lineAmount = parseFloat(parseFloat($tr.find('.hdnLineAmount').val()).toFixed(2));

            var patientAmount = parseFloat(parseFloat($tr.find('.txtPatientAmount').attr('hiddenVal')).toFixed(2));
            var payerAmount = parseFloat(parseFloat(lineAmount - patientAmount).toFixed(2));

            if (payerAmount < 0) {
                $tr.find('.txtPayerAmount').val(payerAmountOri).trigger('changeValue');
                $tr.find('.txtPatientAmount').val(patientAmountOri).trigger('changeValue');
            } else {
                $tr.find('.txtPayerAmount').val(payerAmount).trigger('changeValue');
            }
        });
    </script>
    <input type="hidden" value="" id="hdnSelectedTransactionDtID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedPayerAmount" runat="server" />
    <input type="hidden" value="" id="hdnSelectedPatientAmount" runat="server" />
    <div style="height: 450px; overflow-y: auto;">
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td>
                    <div style="padding: 5px; min-height: 150px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { hideLoadingPanel(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:ListView ID="lvwView" runat="server" OnItemDataBound="lvwView_ItemDataBound">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 40px" rowspan="2">
                                                            <div style="padding: 3px">
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                            </div>
                                                        </th>
                                                        <th style="width: 170px" rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left">
                                                                <div>
                                                                    <%= GetLabel("Transaksi")%></div>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px" rowspan="2" align="center">
                                                            <div style="padding: 3px">
                                                                <div>
                                                                    <%= GetLabel("Status Transaksi")%></div>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left">
                                                                <div>
                                                                    <%= GetLabel("Item")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("Jumlah")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="15">
                                                            <%=GetLabel("No Data To Display") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 40px" rowspan="2">
                                                            <div style="padding: 3px">
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                            </div>
                                                        </th>
                                                        <th style="width: 170px" rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left">
                                                                <div>
                                                                    <%= GetLabel("Transaksi")%></div>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px" rowspan="2" align="center">
                                                            <div style="padding: 3px">
                                                                <div>
                                                                    <%= GetLabel("Status Transaksi")%></div>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left">
                                                                <div>
                                                                    <%= GetLabel("Item")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("Jumlah")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr style="<%#: Eval("GCTransactionStatus").ToString() == "X121^001" ? "background-color:#FFE4E1" : ""%>">
                                                    <td align="center">
                                                        <div style="padding: 3px">
                                                            <asp:CheckBox ID="chkIsSelected" CssClass="chkIsSelected" runat="server" />
                                                            <input type="hidden" class="keyField" value="<%#: Eval("ID")%>" />
                                                            <input type="hidden" class="TransactionID" value="<%#: Eval("TransactionID")%>" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: left; font-weight: bold">
                                                            <%#: Eval("TransactionNo")%></div>
                                                        <div style="padding: 3px; text-align: left; font-weight: bold">
                                                            <%#: Eval("TransactionDateInString")%></div>
                                                        <div style="padding: 3px; text-align: left; font-size: x-small; font-style: italic">
                                                            <%=GetLabel("NoReg : ")%><%#: Eval("RegistrationNo")%></div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: center;">
                                                            <%#: Eval("TransactionStatus")%></div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: left; font-weight: bold">
                                                            <%#: Eval("ItemName1")%></div>
                                                        <div style="padding: 3px; text-align: left; font-style: italic">
                                                            <%#: Eval("ItemCode")%></div>
                                                        <div style="padding: 3px; text-align: left;">
                                                            <%#: Eval("ItemType")%></div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnPayerAmountOri" value='<%#: Eval("PayerAmount")%>' />
                                                            <asp:TextBox ID="txtPayerAmount" ReadOnly="true" Width="90%" runat="server" CssClass="txtCurrency txtPayerAmount" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnPatientAmountOri" value='<%#: Eval("PatientAmount")%>' />
                                                            <asp:TextBox ID="txtPatientAmount" ReadOnly="true" Width="90%" runat="server" CssClass="txtCurrency txtPatientAmount" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnLineAmount" value='<%#: Eval("LineAmount")%>' />
                                                            <div>
                                                                <%#: Eval("LineAmount", "{0:N2}")%></div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingService">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
