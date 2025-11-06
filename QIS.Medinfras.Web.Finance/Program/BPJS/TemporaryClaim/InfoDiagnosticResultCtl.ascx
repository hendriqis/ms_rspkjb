<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoDiagnosticResultCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.InfoDiagnosticResultCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_infochargeseklaimparameter">

    //#region Laboratory
    $('.btnPrintLBNum').die('click');
    $('.btnPrintLBNum').live('click', function () {
        $tr = $(this).closest('tr');
        var transactionID = $tr.find('.TransactionID').val();
        var hsuID = $tr.find('.HealthcareServiceUnitID').val();
        var lbHSUID = $('#<%=hdnLaboratoryHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = { text: "" };

        var reportCode = "LB-00001";
        if (lbHSUID == hsuID) {
            if (transactionID != 0 && transactionID != "" && transactionID != null) {
                filterExpression.text = "ChargeTransactionID = " + transactionID;

                openReportViewer(reportCode, filterExpression.text);
            }
        } else {
            displayMessageBox('INFORMATION', "Please select Print Other Diagnostic Result");
        }
    });

    $('.btnPrintLBText').die('click');
    $('.btnPrintLBText').live('click', function () {
        $tr = $(this).closest('tr');
        var transactionID = $tr.find('.TransactionID').val();
        var hsuID = $tr.find('.HealthcareServiceUnitID').val();
        var IsPathologicalAnatomyTest = $tr.find('.IsPathologicalAnatomyTest').val();
        var IsLaboratoryUnit = $tr.find('.IsLaboratoryUnit').val();
        var lbHSUID = $('#<%=hdnLaboratoryHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = { text: "" };

        var reportCode = "LB-00005";
        if (IsPathologicalAnatomyTest == 'True') {
            reportCode = "LB-00026";
        }
        if (IsLaboratoryUnit == 'True') {
            if (transactionID != 0 && transactionID != "" && transactionID != null) {
                filterExpression.text = "ChargeTransactionID = " + transactionID;

                openReportViewer(reportCode, filterExpression.text);
            }
        } else {
            displayMessageBox('INFORMATION', "Please select Print Other Diagnostic Result");
        }
    });

    $('.btnUploadLBNum').die('click');
    $('.btnUploadLBNum').live('click', function () {
        $tr = $(this).closest('tr');
        var transactionID = $tr.find('.TransactionID').val();
        var hsuID = $tr.find('.HealthcareServiceUnitID').val();
        var lbHSUID = $('#<%=hdnLaboratoryHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = { text: "" };

        var reportCode = "LB-00001";
        if (lbHSUID == hsuID) {
            if (transactionID != 0 && transactionID != "" && transactionID != null) {
                filterExpression.text = "ChargeTransactionID = " + transactionID;

                openReportViewer(reportCode, filterExpression.text);
            }
        } else {
            displayMessageBox('INFORMATION', "Please select Print Other Diagnostic Result");
        }
    });

    $('.btnUploadLBText').die('click');
    $('.btnUploadLBText').live('click', function () {
        $tr = $(this).closest('tr');
        var transactionID = $tr.find('.TransactionID').val();
        var hsuID = $tr.find('.HealthcareServiceUnitID').val();
        var lbHSUID = $('#<%=hdnLaboratoryHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = { text: "" };

        var reportCode = "LB-00005";
        if (lbHSUID == hsuID) {
            if (transactionID != 0 && transactionID != "" && transactionID != null) {
                filterExpression.text = "ChargeTransactionID = " + transactionID;

                openReportViewer(reportCode, filterExpression.text);
            }
        } else {
            displayMessageBox('INFORMATION', "Please select Print Other Diagnostic Result");
        }
    });
    //#endregion

    //#region Imaging
    $('.btnPrintISIndo').die('click');
    $('.btnPrintISIndo').live('click', function () {
        $tr = $(this).closest('tr');
        var transactionID = $tr.find('.TransactionID').val();
        var healthcareServiceUnitID = $tr.find('.HealthcareServiceUnitID').val();
        var lbHSUID = $('#<%=hdnImagingHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = { text: "" };

        var reportCode = "IS-00001";
        if (lbHSUID == healthcareServiceUnitID) {
            if (transactionID != 0 && transactionID != "" && transactionID != null) {
                filterExpression.text = "TransactionID = " + transactionID;

                openReportViewer(reportCode, filterExpression.text);
            }
        } else {
            displayMessageBox('INFORMATION', "Please select Print Other Diagnostic Result");
        }
    });

    $('.btnPrintISEng').die('click');
    $('.btnPrintISEng').live('click', function () {
        $tr = $(this).closest('tr');
        var transactionID = $tr.find('.TransactionID').val();
        var healthcareServiceUnitID = $tr.find('.HealthcareServiceUnitID').val();
        var lbHSUID = $('#<%=hdnImagingHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = { text: "" };

        var reportCode = "IS-00002";
        if (lbHSUID == healthcareServiceUnitID) {
            if (transactionID != 0 && transactionID != "" && transactionID != null) {
                filterExpression.text = "TransactionID = " + transactionID;

                openReportViewer(reportCode, filterExpression.text);
            }
        } else {
            displayMessageBox('INFORMATION', "Please select Print Other Diagnostic Result");
        }
    });

    $('.btnUploadISIndo').die('click');
    $('.btnUploadISIndo').live('click', function () {
        $tr = $(this).closest('tr');
        var transactionID = $tr.find('.TransactionID').val();
        var healthcareServiceUnitID = $tr.find('.HealthcareServiceUnitID').val();
        var lbHSUID = $('#<%=hdnImagingHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = { text: "" };

        var reportCode = "IS-00001";
        if (lbHSUID == healthcareServiceUnitID) {
            if (transactionID != 0 && transactionID != "" && transactionID != null) {
                filterExpression.text = "TransactionID = " + transactionID;

                openReportViewer(reportCode, filterExpression.text);
            }
        } else {
            displayMessageBox('INFORMATION', "Please select Print Other Diagnostic Result");
        }
    });

    $('.btnUploadISEng').die('click');
    $('.btnUploadISEng').live('click', function () {
        $tr = $(this).closest('tr');
        var transactionID = $tr.find('.TransactionID').val();
        var healthcareServiceUnitID = $tr.find('.HealthcareServiceUnitID').val();
        var lbHSUID = $('#<%=hdnImagingHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = { text: "" };

        var reportCode = "IS-00002";
        if (lbHSUID == healthcareServiceUnitID) {
            if (transactionID != 0 && transactionID != "" && transactionID != null) {
                filterExpression.text = "TransactionID = " + transactionID;

                openReportViewer(reportCode, filterExpression.text);
            }
        } else {
            displayMessageBox('INFORMATION', "Please select Print Other Diagnostic Result");
        }
    });
    //#endregion

    //#region Diagnostic
    $('.btnPrintMDIndo').die('click');
    $('.btnPrintMDIndo').live('click', function () {
        $tr = $(this).closest('tr');
        var transactionID = $tr.find('.TransactionID').val();
        var healthcareServiceUnitID = $tr.find('.HealthcareServiceUnitID').val();
        var lbHSUIDLB = $('#<%=hdnLaboratoryHealthcareServiceUnitID.ClientID %>').val();
        var lbHSUIDIS = $('#<%=hdnImagingHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = { text: "" };

        var reportCode = "MD-00002";
        if (lbHSUIDLB != healthcareServiceUnitID && lbHSUIDIS != healthcareServiceUnitID) {
            if (transactionID != 0 && transactionID != "" && transactionID != null) {
                filterExpression.text = "TransactionID = " + transactionID;

                openReportViewer(reportCode, filterExpression.text);
            }
        } else {
            displayMessageBox('INFORMATION', "Please select Print Other Diagnostic Result");
        }
    });

    $('.btnPrintMDEng').die('click');
    $('.btnPrintMDEng').live('click', function () {
        $tr = $(this).closest('tr');
        var transactionID = $tr.find('.TransactionID').val();
        var healthcareServiceUnitID = $tr.find('.HealthcareServiceUnitID').val();
        var lbHSUIDLB = $('#<%=hdnLaboratoryHealthcareServiceUnitID.ClientID %>').val();
        var lbHSUIDIS = $('#<%=hdnImagingHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = { text: "" };

        var reportCode = "MD-00003";
        if (lbHSUIDLB != healthcareServiceUnitID && lbHSUIDIS != healthcareServiceUnitID) {
            if (transactionID != 0 && transactionID != "" && transactionID != null) {
                filterExpression.text = "TransactionID = " + transactionID;

                openReportViewer(reportCode, filterExpression.text);
            }
        } else {
            displayMessageBox('INFORMATION', "Please select Print Other Diagnostic Result");
        }
    });

    $('.btnUploadMDIndo').die('click');
    $('.btnUploadMDIndo').live('click', function () {
        $tr = $(this).closest('tr');
        var transactionID = $tr.find('.TransactionID').val();
        var healthcareServiceUnitID = $tr.find('.HealthcareServiceUnitID').val();
        var lbHSUIDLB = $('#<%=hdnLaboratoryHealthcareServiceUnitID.ClientID %>').val();
        var lbHSUIDIS = $('#<%=hdnImagingHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = { text: "" };

        var reportCode = "MD-00002";
        if (lbHSUIDLB != healthcareServiceUnitID && lbHSUIDIS != healthcareServiceUnitID) {
            if (transactionID != 0 && transactionID != "" && transactionID != null) {
                filterExpression.text = "TransactionID = " + transactionID;

                openReportViewer(reportCode, filterExpression.text);
            }
        } else {
            displayMessageBox('INFORMATION', "Please select Print Other Diagnostic Result");
        }
    });

    $('.btnUploadMDEng').die('click');
    $('.btnUploadMDEng').live('click', function () {
        $tr = $(this).closest('tr');
        var transactionID = $tr.find('.TransactionID').val();
        var healthcareServiceUnitID = $tr.find('.HealthcareServiceUnitID').val();
        var lbHSUIDLB = $('#<%=hdnLaboratoryHealthcareServiceUnitID.ClientID %>').val();
        var lbHSUIDIS = $('#<%=hdnImagingHealthcareServiceUnitID.ClientID %>').val();
        var filterExpression = { text: "" };

        var reportCode = "MD-00003";
        if (lbHSUIDLB != healthcareServiceUnitID && lbHSUIDIS != healthcareServiceUnitID) {
            if (transactionID != 0 && transactionID != "" && transactionID != null) {
                filterExpression.text = "TransactionID = " + transactionID;

                openReportViewer(reportCode, filterExpression.text);
            }
        } else {
            displayMessageBox('INFORMATION', "Please select Print Other Diagnostic Result");
        }
    });
    //#endregion

</script>
<div style="height: 400px; overflow-y: auto; overflow-x: hidden" id="containerPopup">
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationNo" runat="server" value="" />
    <input type="hidden" id="hdnLaboratoryHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnImagingHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnInitialHealthcare" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td align="left">
                <table>
                    <colgroup>
                        <col style="width: 140px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <%=GetLabel("Registration No")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationNo" Width="170px" runat="server" ReadOnly="true"
                                Style="text-align: left" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=GetLabel("SEP No")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSEPNo" Width="170px" runat="server" ReadOnly="true" Style="text-align: left" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=GetLabel("Patient")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatient" Width="350px" runat="server" ReadOnly="true" Style="text-align: left" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding: 5px; vertical-align: top">
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
                                                <th style="width: 200px" align="left">
                                                    <%=GetLabel("Unit Pelayanan")%>
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("No Transaksi")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Tanggal-Jam Hasil")%>
                                                </th>
                                                <th align="center">
                                                    <%=GetLabel("Cetak Hasil Laboratorium")%>
                                                </th>
                                                <th align="center" style="display: none">
                                                    <%=GetLabel("Upload Hasil Laboratorium")%>
                                                </th>
                                                <th align="center">
                                                    <%=GetLabel("Cetak Hasil Penunjang Lainnya")%>
                                                </th>
                                                <th align="center" style="display: none">
                                                    <%=GetLabel("Upload Hasil Penunjang Lainnya")%>
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
                                                <th style="width: 200px" align="left">
                                                    <%=GetLabel("Unit Pelayanan")%>
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("No Transaksi")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Tanggal-Jam Hasil")%>
                                                </th>
                                                <th align="center">
                                                    <%=GetLabel("Hasil Laboratorium")%>
                                                </th>
                                                <th align="center" style="display: none">
                                                    <%=GetLabel("Upload Hasil Laboratorium")%>
                                                </th>
                                                <th align="center">
                                                    <%=GetLabel("Hasil Radiologi")%>
                                                </th>
                                                <th align="center" style="display: none">
                                                    <%=GetLabel("Upload Hasil Radiologi")%>
                                                </th>
                                                <th align="center">
                                                    <%=GetLabel("Hasil Penunjang Lainnya")%>
                                                </th>
                                                <th align="center" style="display: none">
                                                    <%=GetLabel("Upload Hasil Penunjang Lainnya")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <input type="hidden" class="HealthcareServiceUnitID" id="HealthcareServiceUnitID"
                                                    runat="server" value='<%#: Eval("HealthcareServiceUnitID")%>' />
                                                <%#: Eval("ServiceUnitName")%>
                                            </td>
                                            <td>
                                                <input type="hidden" class="TransactionID" id="TransactionID" runat="server" value='<%#: Eval("TransactionID")%>' />
                                                <input type="hidden" class="IsPathologicalAnatomyTest" id="IsPathologicalAnatomyTest"
                                                    runat="server" value='<%#: Eval("IsPathologicalAnatomyTest")%>' />
                                                <input type="hidden" class="IsLaboratoryUnit" id="IsLaboratoryUnit" runat="server"
                                                    value='<%#: Eval("IsLaboratoryUnit")%>' />
                                                <%#: Eval("TransactionNo")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("cfResultDateTime")%>
                                            </td>
                                            <td align="center">
                                                <input type="button" id="btnPrintLBNum" class="btnPrintLBNum w3-btn w3-white w3-border w3-border-purple w3-round-xxlarge"
                                                    value="Angka" runat="server" />
                                                <input type="button" id="btnPrintLBText" class="btnPrintLBText w3-btn w3-white w3-border w3-border-purple w3-round-xxlarge"
                                                    value="Text" runat="server" />
                                            </td>
                                            <td align="center" style="display: none">
                                                <input type="button" id="btnUploadLBNum" class="btnUploadLBNum w3-button w3-purple w3-border w3-border-purple w3-round-xxlarge"
                                                    value="Angka" runat="server" />
                                                <input type="button" id="btnUploadLBText" class="btnUploadLBText w3-button w3-purple w3-border w3-border-purple w3-round-xxlarge"
                                                    value="Text" runat="server" />
                                            </td>
                                            <td align="center">
                                                <input type="button" id="btnPrintISIndo" class="btnPrintISIndo w3-btn w3-white w3-border w3-border-orange w3-round-xxlarge"
                                                    value="ID" runat="server" />
                                                <input type="button" id="btnPrintISEng" class="btnPrintISEng w3-btn w3-white w3-border w3-border-orange w3-round-xxlarge"
                                                    value="EN" runat="server" />
                                            </td>
                                            <td align="center" style="display: none">
                                                <input type="button" id="btnUploadISIndo" class="btnUploadISIndo w3-button w3-orange w3-border w3-border-orange w3-round-xxlarge"
                                                    value="ID" runat="server" />
                                                <input type="button" id="btnUploadISEng" class="btnUploadISEng w3-button w3-orange w3-border w3-border-orange w3-round-xxlarge"
                                                    value="EN" runat="server" />
                                            </td>
                                            <td align="center">
                                                <input type="button" id="btnPrintMDIndo" class="btnPrintMDIndo w3-btn w3-white w3-border w3-border-maroon w3-round-xxlarge"
                                                    value="ID" runat="server" />
                                                <input type="button" id="btnPrintMDEng" class="btnPrintMDEng w3-btn w3-white w3-border w3-border-maroon w3-round-xxlarge"
                                                    value="EN" runat="server" />
                                            </td>
                                            <td align="center" style="display: none">
                                                <input type="button" id="btnUploadMDIndo" class="btnUploadMDIndo w3-button w3-orange w3-border w3-border-maroon w3-round-xxlarge"
                                                    value="ID" runat="server" />
                                                <input type="button" id="btnUploadMDEng" class="btnUploadMDEng w3-button w3-orange w3-border w3-border-maroon w3-round-xxlarge"
                                                    value="EN" runat="server" />
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
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
