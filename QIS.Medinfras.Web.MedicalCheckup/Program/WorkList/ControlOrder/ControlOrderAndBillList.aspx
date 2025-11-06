<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="ControlOrderAndBillList.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.ControlOrderAndBillList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            setRightPanelButtonEnabled();
            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            $('#<%=txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtRegistrationDate.ClientID %>').change(function (evt) {
                refreshGrdRegisteredPatient();
            });

            $('#btnRefresh').click(function (evt) {
                showLoadingPanel();
                refreshGrdRegisteredPatient();
            });

            $('#btnProcess').click(function (evt) {
                getCheckedMember();
                if ($('#<%=hdnSelectedMember.ClientID %>').val() != '') {
                    cbpView.PerformCallback("process");
                }
                else {
                    displayMessageBox("WARNING", "Harap pilih registrasi yang akan diproses");
                }
            });
        });

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalID);
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                refreshGrdRegisteredPatient();
                intervalID = window.setInterval(function () {
                    onRefreshGridView();
                }, interval);
            }
        }

        function setRightPanelButtonEnabled() {
            
        }

        function onBeforeLoadRightPanelContent(code) {
            var param = '';
            if (code == 'voidChargesMCUGroup') {
                param = $('#<%=txtRegistrationDate.ClientID %>').val() + '|';
            }
            return param;
        }

        function onCbpViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == "process") {
                if (param[1] == "fail") {
                    displayMessageBox("ERROR", param[2]);
                }
                else {
                    displayMessageBox("SUCCESS", "Berhasil memproses tagihan");
                }
                refreshGrdRegisteredPatient();
            }
            hideLoadingPanel();
        }
        function refreshGrdRegisteredPatient() {
            cbpView.PerformCallback('refresh');
        }

        function onAfterPopupControlClosing() {
            refreshGrdRegisteredPatient();
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGridView();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }

        $('.lnkItemName a').live('click', function () {
            $tr = $(this).closest('tr');
            var visitID = $tr.find('.hdnVisitID').val();
            var regNo = $tr.find('.hdnRegistrationNo').val();
            var patientName = $tr.find('.hdnPatientName').val();
            var itemID = $tr.find('.hdnItemID').val();
            var itemCode = $tr.find('.hdnItemCode').val();
            var itemName = $tr.find('.hdnItemName1').val();
            var id = visitID + '|' + regNo + '|' + patientName + '|' + itemID + '|' + itemCode + '|' + itemName;
            var url = ResolveUrl("~/Program/WorkList/ControlOrder/ControlOrderDtCustomCtl.ascx");
            openUserControlPopup(url, id, 'Item Detail', 1000, 550);
        });

        $('.imgOutstanding.imgLink1').die('click');
        $('.imgOutstanding.imgLink1').live('click', function () {
            $tr = $(this).closest('tr');
            var visitID = $tr.find('.hdnVisitID').val();
            var regNo = $tr.find('.hdnRegistrationNo').val();
            var patientName = $tr.find('.hdnPatientName').val();
            var itemID = $tr.find('.hdnItemID').val();
            var itemCode = $tr.find('.hdnItemCode').val();
            var itemName = $tr.find('.hdnItemName1').val();
            var id = visitID + '|' + regNo + '|' + patientName + '|' + itemID + '|' + itemCode + '|' + itemName;
            var url = ResolveUrl("~/Program/WorkList/ControlOrder/ControlOutstandingOrderCtl.ascx");
            openUserControlPopup(url, id, 'Item Detail', 1000, 550);
        });

        $('.imgPrint.imgLink2').die('click');
        $('.imgPrint.imgLink2').live('click', function () {
            $tr = $(this).closest('tr');
            var reportCode = "PM-00142";
            var regID = $tr.find('.hdnRegistrationID').val();
            var filterExpressionMCU = 'RegistrationID = ' + regID;
            openReportViewer(reportCode, filterExpressionMCU);
        });

        $('.imgPrint.imgLink3').die('click');
        $('.imgPrint.imgLink3').live('click', function () {
            $tr = $(this).closest('tr');
            var reportCode = "PM-00166";
            var regID = $tr.find('.hdnRegistrationID').val();
            var filterExpressionMCU = 'RegistrationID = ' + regID;
            openReportViewer(reportCode, filterExpressionMCU);
        });

        //#region Check Box
        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        function getCheckedMember() {
            var lstParam = '';
            var lstRegID = '';
            var lstVisitID = '';
            $('.chkIsSelected input:checked').each(function () {
                var trxID = $(this).closest('tr').find('.hdnKeyField').val();
                var regID = $(this).closest('tr').find('.hdnRegistrationID').val();
                var visitID = $(this).closest('tr').find('.hdnVisitID').val();
                if (lstParam != '')
                    lstParam += ',';
                lstParam += trxID;
                if (lstRegID != '')
                    lstRegID += ',';
                lstRegID += regID;
                if (lstVisitID != '')
                    lstVisitID += ',';
                lstVisitID += visitID;
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstParam);
            $('#<%=hdnSelectedMemberRegID.ClientID %>').val(lstRegID);
            $('#<%=hdnSelectedMemberVisitID.ClientID %>').val(lstVisitID);
        }

        $('.lblMCUItem.lblLink').live('click', function () {
            $tr = $(this).parent();
            var visitID = $tr.find('.hdnVisitID').val();
            var regNo = $tr.find('.hdnRegistrationNo').val();
            var patientName = $tr.find('.hdnPatientName').val();
            var itemID = $tr.find('.hdnItemID').val();
            var itemCode = $tr.find('.hdnItemCode').val();
            var itemName = $tr.find('.hdnItemName1').val();
            var id = visitID + '|' + regNo + '|' + patientName + '|' + itemID + '|' + itemCode + '|' + itemName;
            var url = ResolveUrl("~/Program/WorkList/ControlOrder/ControlOrderDtCustomCtl.ascx");
            openUserControlPopup(url, id, 'Item Detail', 1000, 550);
        });
       
    </script>
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" id="hdnSelectedRegistration" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberRegID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberVisitID" runat="server" value="" />
    <input type="hidden" id="hdnPembuatanTagihanTidakAdaOutstandingOrder" runat="server" value="" />
    <input type="hidden" id="hdnShift" runat="server" value="" />
    <input type="hidden" id="hdnCashierGroup" runat="server" value="" />
    <div style="padding: 15px">
        <div class="pageTitle">
            <%=GetMenuCaption()%></div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsBedStatus">
                        <table class="tblEntryContent" style="width: 60%;">
                            <colgroup>
                                <col style="width: 25%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal Registrasi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Quick Filter")%></label>
                                </td>
                                <td>
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                        Width="300px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                            <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="Perusahaan" FieldName="BusinesPartnerName" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                            <tr>
                                <table border="0" cellpadding="5" cellspacing="0">
                                    <colgroup>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                                        </td>
                                        <td>
                                            <input type="button" id="btnProcess" value="P r o c e s s" class="btnProcess w3-button w3-orange w3-border w3-border-blue w3-round-large" />
                                        </td>
                                    </tr>
                                </table>
                            </tr>
                        </table>
                    </fieldset>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                                    <asp:ListView runat="server" ID="lvwViewCount">
                                        <EmptyDataTemplate>
                                            <table id="tblViewCount" runat="server" cellspacing="0" class="lvwViewCount" rules="all" width="150px" style="float:right; margin-right:10px">
                                                <tr>
                                                    <th style="width: 150px; margin-right:5px" align="right">
                                                        <%=GetLabel("Jumlah Transaksi")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td style="width: 150px; margin-right:5px; text-align:right">
                                                        <%=GetLabel("0")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblViewCount" runat="server" cellspacing="0" class="lvwViewCount" rules="all" width="150px" style="float:right; margin-right:10px">
                                                <tr>
                                                    <th style="width: 150px; margin-right:5px" align="right">
                                                        <%=GetLabel("Jumlah Transaksi")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td style="width: 150px; text-align:right">
                                                    <div>
                                                        <%#: Eval("TotalRow") %></span>
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                       <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdControlOrder grdSelected" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th class="keyField">
                                                        &nbsp;
                                                    </th>
                                                        <th style="width: 10px" align="center">
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("No Rekam Medis")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("No Registrasi")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Nama Pasien")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Batch No")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Paket MCU")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Pembayar")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="20">
                                                        <%=GetLabel("Tidak ada data.")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdMCUOrder grdSelected" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th class="keyField">
                                                        &nbsp;
                                                    </th>
                                                        <th style="width: 10px" align="center">
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("No Rekam Medis")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("No Registrasi")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Nama Pasien")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Batch No")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Paket MCU")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Pembayar")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField">
                                                    <%#: Eval("ID")%>
                                                </td>
                                                <td align="center">
                                                    <div style="padding: 3px">
                                                        <asp:CheckBox ID="chkIsSelected" CssClass="chkIsSelected" runat="server" />
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                    </div>
                                                </td>
                                                <td align="left">
                                                    <input type="hidden" value="" class="hdnKey" id="hdnKey" runat="server" />
                                                    <%#: Eval("MedicalNo")%>
                                                </td>
                                                <td align="left">
                                                    <%#: Eval("RegistrationNo")%>
                                                </td>
                                                <td align="left">
                                                    <%#: Eval("PatientName")%>
                                                </td>
                                                <td align="left">
                                                    <%#: Eval("ReferenceNo")%>
                                                </td>
                                                <td class="lblLink lblMCUItem" runat="server" id="lblMCUItem" style="color:Blue">
                                                    <%#: Eval("cfItemComparison")%>
                                                </td>
                                                <td align="left">
                                                    <%#: Eval("BusinessPartnerName")%>
                                                </td>
                                                <td align="left" style="display:none">
                                                    <input type="hidden" value="" class="hdnRegistrationID" id="hdnRegistrationID" runat="server" />
                                                    <input type="hidden" value="" class="hdnVisitID" id="hdnVisitID" runat="server" />
                                                    <input type="hidden" value="" class="hdnRegistrationNo" id="hdnRegistrationNo" runat="server" />
                                                    <input type="hidden" value="" class="hdnPatientName" id="hdnPatientName" runat="server" />
                                                    <input type="hidden" value="" class="hdnReferenceNo" id="hdnReferenceNo" runat="server" />
                                                    <input type="hidden" value="" class="hdnItemID" id="hdnItemID" runat="server" />
                                                    <input type="hidden" value="" class="hdnItemCode" id="hdnItemCode" runat="server" />
                                                    <input type="hidden" value="" class="hdnItemName1" id="hdnItemName1" runat="server" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
