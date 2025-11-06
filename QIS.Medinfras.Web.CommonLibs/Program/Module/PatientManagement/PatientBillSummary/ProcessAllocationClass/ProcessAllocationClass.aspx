<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="ProcessAllocationClass.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ProcessAllocationClass" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process") %></div>
    </li>
    <li id="btnReset" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdelete.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Reset") %></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhHeader" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('#ulTabPatientBillSummaryDetailAll li').click(function () {
                $('#ulTabPatientBillSummaryDetailAll li.selected').removeAttr('class');
                $('.containerBillSummaryDetailAll').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
        }

        $('#<%=btnProcess.ClientID %>').live('click', function () {
            onCustomButtonClick('process');
        });

        $('#<%=btnReset.ClientID %>').live('click', function () {
            onCustomButtonClick('reset');
        });

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback();
        }

        function dateToDMYCustom(date) {
            var d = date.getDate();
            var m = date.getMonth() + 1;
            var y = date.getFullYear();
            return '' + y + (m <= 9 ? '0' + m : m) + (d <= 9 ? '0' + d : d);
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var $tr = $(this).closest('tr');
            var id = $tr.find('.TransactionID').val();
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            var chargeClassID = cboServiceChargeClassID.GetValue();

            if (code == 'PM-00296' || code == 'PM-00297' || code == 'PM-00298' || code == 'PM-00299' || code == 'PM-002100' || code == 'PM-002101' || code == 'PM-002102' || code == 'PM-002103' || code == 'PM-002104' || code == 'PM-002105'
                || code == 'PM-00712' || code == 'PM-00713' || code == 'PM-00714' || code == 'PM-00715') {
                filterExpression.text = 'RegistrationID = ' + registrationID;
                return true;
            } else if (code == 'PM-002154') {
                filterExpression.text = registrationID + '|' + chargeClassID;
                return true;
            }
            if (registrationID == '') {
                errMessage.text = 'Please Select Registration First!';
                return false;
            }
            else if (transDateFrom == '' || transDateTo == '') {
                errMessage.text = 'Please Select Date First!';
                return false;
            }
            else if (code == 'PM-00220') {
                filterExpression.text = id;
                return true;
            }
            else {
                filterExpression.text = registrationID + '|' + transDateYMDFrom + ';' + transDateYMDTo;
                return true;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnVisitID" value="" runat="server" />
    <input type="hidden" id="hdnLinkedRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnPatientChargesHdID" runat="server" value="" />
    <input type="hidden" id="hdnPatientChargesDtID" runat="server" value="" />
    <input type="hidden" id="hdnPatientChargesHdIDLinked" runat="server" value="" />
    <input type="hidden" id="hdnPatientChargesDtIDLinked" runat="server" value="" />
    <table class="tblEntryContent" style="width: 100%">
        <colgroup>
            <col width="50%" />
            <col width="50%" />
        </colgroup>
        <tr>
            <td>
                <table>
                    <colgroup>
                        <col width="150px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kelas Tagihan Tujuan") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboServiceChargeClassID" ClientInstanceName="cboServiceChargeClassID"
                                Width="250px" runat="server">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkIsLinkedRegExclude" Text=" Abaikan Transaksi dari Registrasi Asal?" />
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right">
                <table id="tblInfoWarning" runat="server">
                    <tr>
                        <td style="vertical-align: top" class="blink-alert">
                            <img height="60" src='<%= ResolveUrl("~/Libs/Images/Warning.png")%>' alt='' class="blink-alert" />
                        </td>
                        <td style="vertical-align: middle">
                            <label class="lblWarning" id="lblInfoR1" runat="server">
                                <%=GetLabel("Jika ada perubahan transaksi asli, hasil hitung jatah kelas tidak akan berubah.") %></label><br />
                            <label class="lblWarning" id="lblInfoR2" runat="server">
                                <%=GetLabel("Harap lakukan Reset terlebih dahulu sebelum Proses ulang.") %></label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div>
        <table class="tblContentArea">
            <tr>
                <td>
                    <div>
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { hideLoadingPanel(s); onLoad();} " />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <div class="containerUlTabPage">
                                            <ul class="ulTabPage" id="ulTabPatientBillSummaryDetailAll">
                                                <li class="selected" contentid="containerService">
                                                    <%=GetLabel("Pelayanan") %>
                                                </li>
                                                <li contentid="containerDrugMS">
                                                    <%=GetLabel("Obat & Alkes") %>
                                                </li>
                                                <li contentid="containerLogistics">
                                                    <%=GetLabel("Barang Umum") %>
                                                </li>
                                            </ul>
                                        </div>
                                        <div id="containerService" class="containerBillSummaryDetailAll">
                                            <asp:ListView ID="lvwService" runat="server">
                                                <EmptyDataTemplate>
                                                    <table id="tblView" runat="server" class="grdNormal" cellspacing="0" rules="all">
                                                        <tr>
                                                            <th style="width: 10px" rowspan="2">
                                                                <div style="padding: 3px">
                                                                </div>
                                                            </th>
                                                            <th rowspan="2">
                                                                <div style="text-align: left; padding-left: 3px">
                                                                    <%=GetLabel("Deskripsi")%>
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" style="width: 150px">
                                                                <div style="text-align: center; padding-left: 3px">
                                                                    <%=GetLabel("Tanggal Transaksi")%>
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" style="width: 80px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Harga Satuan")%>
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" style="width: 40px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Jumlah")%>
                                                                </div>
                                                            </th>
                                                            <th colspan="2" align="center">
                                                                <%=GetLabel("Harga")%>
                                                            </th>
                                                            <th colspan="3" align="center">
                                                                <%=GetLabel("Total")%>
                                                            </th>
                                                            <th rowspan="2" style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Created By")%>
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" style="width: 50px">
                                                                <div style="text-align: center;">
                                                                    <%=GetLabel("Verified")%>
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" style="width: 50px">
                                                                <div style="text-align: center;">
                                                                    <%=GetLabel("Reviewed")%>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <th style="width: 70px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("CITO")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 70px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Diskon")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Instansi")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Pasien")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Total")%>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr class="trEmpty">
                                                            <td colspan="25">
                                                                <%=GetLabel("No Data To Display") %>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="tblView" runat="server" class="grdService grdNormal" cellspacing="0" rules="all">
                                                        <tr>
                                                            <th style="width: 10px" rowspan="2">
                                                                <div style="padding: 3px">
                                                                </div>
                                                            </th>
                                                            <th rowspan="2">
                                                                <div style="text-align: left; padding-left: 3px">
                                                                    <%=GetLabel("Deskripsi")%>
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" style="width: 150px">
                                                                <div style="text-align: center; padding-left: 3px">
                                                                    <%=GetLabel("Tanggal Transaksi")%>
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" style="width: 80px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Harga Satuan")%>
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" style="width: 40px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Jumlah")%>
                                                                </div>
                                                            </th>
                                                            <th colspan="2" align="center">
                                                                <%=GetLabel("Harga")%>
                                                            </th>
                                                            <th colspan="3" align="center">
                                                                <%=GetLabel("Total")%>
                                                            </th>
                                                            <th rowspan="2" style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Created By")%>
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" style="width: 50px">
                                                                <div style="text-align: center;">
                                                                    <%=GetLabel("Verified")%>
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" style="width: 50px">
                                                                <div style="text-align: center;">
                                                                    <%=GetLabel("Reviewed")%>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <th style="width: 70px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("CITO")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 70px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Diskon")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Instansi")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Pasien")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Total")%>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr runat="server" id="itemPlaceholder">
                                                        </tr>
                                                        <tr id="Tr1" class="trFooter" runat="server">
                                                            <td colspan="6" align="right" style="padding-right: 3px">
                                                                <%=GetLabel("TOTAL") %>
                                                            </td>
                                                            <td align="right" style="padding-right: 9px" id="tdServiceTotalPayer" class="tdServiceTotalPayer"
                                                                runat="server">
                                                            </td>
                                                            <td align="right" style="padding-right: 9px" id="tdServiceTotalPatient" class="tdServiceTotalPatient"
                                                                runat="server">
                                                            </td>
                                                            <td align="right" style="padding-right: 9px" id="tdServiceTotal" class="tdServiceTotal"
                                                                runat="server">
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td align="center">
                                                            <div style="padding: 3px">
                                                                <input type="hidden" class="hdnKeyField" value="<%#: Eval("ID")%>" />
                                                                <input type="hidden" class="TransactionID" value="<%#: Eval("TransactionID")%>" />
                                                                <input type="hidden" class="VisitID" value="<%#: Eval("VisitID")%>" />
                                                            </div>
                                                            <img class="imgIsCalculate imgDisabled" title='<%=GetLabel("Calculated")%>' src=' <%# ResolveUrl("~/Libs/Images/Button/recalculate.png") %>'
                                                                style='<%# Eval("DataSource").ToString() == "PatientChargesClassCoverage" ? "width:24px;height:24px;": "width:24px;height:24px;display:none" %>'
                                                                alt="" />
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px">
                                                                <div>
                                                                    <b>
                                                                        <%#: Eval("ItemName1")%>&nbsp;(<%#: Eval("ItemCode") %>)</b></div>
                                                                <div>
                                                                    <span>
                                                                        <%#: Eval("ParamedicName")%></span>
                                                                </div>
                                                                <div>
                                                                    <i>Kelas Tagihan</i>&nbsp;<span><b><%#: Eval("ChargeClassName")%></b></span>&nbsp;<br />
                                                                    <span style="color: Maroon">
                                                                        <%#: Eval("TransactionNo")%></span>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td align="center">
                                                            <div style="padding: 3px;">
                                                                <div>
                                                                    <%# Eval("TransactionDateTimeInString")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("Tariff", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("ChargedQuantity")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("CITOAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("DiscountAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("PayerAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("PatientAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("LineAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("CreatedByUserName")%></div>
                                                                <div>
                                                                    <%#: Eval("CreatedDateTimeInString")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: center;">
                                                                <asp:CheckBox ID="chkIsVerified" runat="server" Checked='<%# Eval("IsVerified")%>'
                                                                    Enabled="false" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: center;">
                                                                <asp:CheckBox ID="chkIsReviewed" runat="server" Checked='<%# Eval("IsReviewed")%>'
                                                                    Enabled="false" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                        <div id="containerDrugMS" style="display: none" class="containerBillSummaryDetailAll">
                                            <asp:ListView ID="lvwDrugMS" runat="server">
                                                <EmptyDataTemplate>
                                                    <table id="tblView" runat="server" class="grdDrugMS grdNormal notAllowSelect" cellspacing="0"
                                                        rules="all">
                                                        <tr>
                                                            <th style="width: 10px" rowspan="2">
                                                                <div style="padding: 3px">
                                                                </div>
                                                            </th>
                                                            <th rowspan="2">
                                                                <div style="text-align: left; padding-left: 3px">
                                                                    <%=GetLabel("Deskripsi")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 150px" rowspan="2">
                                                                <div style="text-align: center; padding-left: 3px">
                                                                    <%=GetLabel("Tanggal Transaksi")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 80px" rowspan="2">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Harga Satuan")%>
                                                                </div>
                                                            </th>
                                                            <th colspan="2" align="center">
                                                                <%=GetLabel("Jumlah")%>
                                                            </th>
                                                            <th rowspan="2" style="width: 80px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Harga")%>
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" style="width: 70px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Diskon")%>
                                                                </div>
                                                            </th>
                                                            <th colspan="3" align="center">
                                                                <%=GetLabel("Total")%>
                                                            </th>
                                                            <th rowspan="2" style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Created By")%>
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" style="width: 50px">
                                                                <div style="text-align: center;">
                                                                    <%=GetLabel("Reviewed")%>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <th style="width: 50px">
                                                                <div style="text-align: center; padding-right: 3px">
                                                                    <%=GetLabel("Dibebankan")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 50px">
                                                                <div style="text-align: center; padding-right: 3px">
                                                                    <%=GetLabel("Satuan")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 80px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Instansi")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 80px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Pasien")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 80px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Total")%>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr class="trEmpty">
                                                            <td colspan="25">
                                                                <%=GetLabel("No Data To Display") %>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                        rules="all">
                                                        <tr>
                                                            <th style="width: 10px" rowspan="2">
                                                                <div style="padding: 3px">
                                                                </div>
                                                            </th>
                                                            <th rowspan="2">
                                                                <div style="text-align: left; padding-left: 3px">
                                                                    <%=GetLabel("Deskripsi")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 150px" rowspan="2">
                                                                <div style="text-align: center; padding-left: 3px">
                                                                    <%=GetLabel("Tanggal Transaksi")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 80px" rowspan="2">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Harga Satuan")%>
                                                                </div>
                                                            </th>
                                                            <th colspan="2" align="center">
                                                                <%=GetLabel("Jumlah")%>
                                                            </th>
                                                            <th rowspan="2" style="width: 80px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Harga")%>
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" style="width: 70px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Diskon")%>
                                                                </div>
                                                            </th>
                                                            <th colspan="3" align="center">
                                                                <%=GetLabel("Total")%>
                                                            </th>
                                                            <th rowspan="2" style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Created By")%>
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" style="width: 50px">
                                                                <div style="text-align: center;">
                                                                    <%=GetLabel("Reviewed")%>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <th style="width: 50px">
                                                                <div style="text-align: center; padding-right: 3px">
                                                                    <%=GetLabel("Dibebankan")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 50px">
                                                                <div style="text-align: center; padding-right: 3px">
                                                                    <%=GetLabel("Satuan")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 80px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Instansi")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 80px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Pasien")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 80px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Total")%>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr runat="server" id="itemPlaceholder">
                                                        </tr>
                                                        <tr id="Tr2" class="trFooter" runat="server">
                                                            <td colspan="8" align="right" style="padding-right: 3px">
                                                                <%=GetLabel("Total") %>
                                                            </td>
                                                            <td align="right" style="padding-right: 3px" id="tdDrugMSTotalPayer" runat="server">
                                                            </td>
                                                            <td align="right" style="padding-right: 3px" id="tdDrugMSTotalPatient" runat="server">
                                                            </td>
                                                            <td align="right" style="padding-right: 3px" id="tdDrugMSTotal" runat="server">
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td align="center">
                                                            <div style="padding: 3px">
                                                                <input type="hidden" class="hdnKeyField" value="<%#: Eval("ID")%>" />
                                                            </div>
                                                            <img class="imgIsCalculate imgDisabled" title='<%=GetLabel("Calculated")%>' src=' <%# ResolveUrl("~/Libs/Images/Button/recalculate.png") %>'
                                                                style='<%# Eval("DataSource").ToString() == "PatientChargesClassCoverage" ? "width:24px;height:24px;": "width:24px;height:24px;display:none" %>'
                                                                alt="" />
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px">
                                                                <div>
                                                                    <b>
                                                                        <%#: Eval("ItemName1")%>&nbsp;(<%#: Eval("ItemCode") %>)</b></div>
                                                                <div>
                                                                    <span>
                                                                        <%#: Eval("ParamedicName")%></span>
                                                                </div>
                                                                <div>
                                                                    <i>Kelas Tagihan</i>&nbsp;<span><b><%#: Eval("ChargeClassName")%></b></span>&nbsp;<br />
                                                                    <span style="color: Maroon">
                                                                        <%#: Eval("TransactionNo")%></span>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td align="center">
                                                            <div style="padding: 3px;">
                                                                <div>
                                                                    <%# Eval("TransactionDateTimeInString")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("Tariff", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("ChargedQuantity")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px;">
                                                                <div>
                                                                    <%#: Eval("ItemUnit")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("GrossLineAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("DiscountAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("PayerAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("PatientAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("LineAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("CreatedByUserName")%></div>
                                                                <div>
                                                                    <%#: Eval("CreatedDateTimeInString")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: center;">
                                                                <asp:CheckBox ID="chkIsReviewed" runat="server" Checked='<%# Eval("IsReviewed")%>'
                                                                    Enabled="false" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                        <div id="containerLogistics" style="display: none" class="containerBillSummaryDetailAll">
                                            <asp:ListView ID="lvwLogistic" runat="server">
                                                <EmptyDataTemplate>
                                                    <table id="tblView" runat="server" class="grdDrugMS grdNormal notAllowSelect" cellspacing="0"
                                                        rules="all">
                                                        <tr>
                                                            <th style="width: 10px" rowspan="2">
                                                                <div style="padding: 3px">
                                                                </div>
                                                            </th>
                                                            <th rowspan="2">
                                                                <div style="text-align: left; padding-left: 3px">
                                                                    <%=GetLabel("Deskripsi")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 150px" rowspan="2">
                                                                <div style="text-align: center; padding-left: 3px">
                                                                    <%=GetLabel("Tanggal Transaksi")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 80px" rowspan="2">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Harga Satuan")%>
                                                                </div>
                                                            </th>
                                                            <th colspan="2" align="center">
                                                                <%=GetLabel("Jumlah")%>
                                                            </th>
                                                            <th rowspan="2" style="width: 80px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Harga")%>
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" style="width: 55px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Diskon")%>
                                                                </div>
                                                            </th>
                                                            <th colspan="3" align="center">
                                                                <%=GetLabel("Total")%>
                                                            </th>
                                                            <th rowspan="2" style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Created By")%>
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" style="width: 50px">
                                                                <div style="text-align: center;">
                                                                    <%=GetLabel("Reviewed")%>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <th style="width: 50px">
                                                                <div style="text-align: center; padding-right: 3px">
                                                                    <%=GetLabel("Dibebankan")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 50px">
                                                                <div style="text-align: center; padding-right: 3px">
                                                                    <%=GetLabel("Satuan")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Instansi")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Pasien")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Total")%>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr class="trEmpty">
                                                            <td colspan="25">
                                                                <%=GetLabel("No Data To Display") %>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                        rules="all">
                                                        <tr>
                                                            <th style="width: 10px" rowspan="2">
                                                                <div style="padding: 3px">
                                                                </div>
                                                            </th>
                                                            <th rowspan="2">
                                                                <div style="text-align: left; padding-left: 3px">
                                                                    <%=GetLabel("Deskripsi")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 150px" rowspan="2">
                                                                <div style="text-align: center; padding-left: 3px">
                                                                    <%=GetLabel("Tanggal Transaksi")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 80px" rowspan="2">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Harga Satuan")%>
                                                                </div>
                                                            </th>
                                                            <th colspan="2" align="center">
                                                                <%=GetLabel("Jumlah")%>
                                                            </th>
                                                            <th rowspan="2" style="width: 80px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Harga")%>
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" style="width: 55px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Diskon")%>
                                                                </div>
                                                            </th>
                                                            <th colspan="3" align="center">
                                                                <%=GetLabel("Total")%>
                                                            </th>
                                                            <th rowspan="2" style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Created By")%>
                                                                </div>
                                                            </th>
                                                            <th rowspan="2" style="width: 50px">
                                                                <div style="text-align: center;">
                                                                    <%=GetLabel("Reviewed")%>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <th style="width: 50px">
                                                                <div style="text-align: center; padding-right: 3px">
                                                                    <%=GetLabel("Dibebankan")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 50px">
                                                                <div style="text-align: center; padding-right: 3px">
                                                                    <%=GetLabel("Satuan")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Instansi")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Pasien")%>
                                                                </div>
                                                            </th>
                                                            <th style="width: 90px">
                                                                <div style="text-align: right; padding-right: 3px">
                                                                    <%=GetLabel("Total")%>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr runat="server" id="itemPlaceholder">
                                                        </tr>
                                                        <tr id="Tr3" class="trFooter" runat="server">
                                                            <td colspan="8" align="right" style="padding-right: 3px">
                                                                <%=GetLabel("Total") %>
                                                            </td>
                                                            <td align="right" style="padding-right: 3px" id="tdLogisticTotalPayer" runat="server">
                                                            </td>
                                                            <td align="right" style="padding-right: 3px" id="tdLogisticTotalPatient" runat="server">
                                                            </td>
                                                            <td align="right" style="padding-right: 3px" id="tdLogisticTotal" runat="server">
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td align="center">
                                                            <div style="padding: 3px">
                                                                <input type="hidden" class="hdnKeyField" value="<%#: Eval("ID")%>" />
                                                            </div>
                                                            <img class="imgIsCalculate imgDisabled" title='<%=GetLabel("Calculated")%>' src=' <%# ResolveUrl("~/Libs/Images/Button/recalculate.png") %>'
                                                                style='<%# Eval("DataSource").ToString() == "PatientChargesClassCoverage" ? "width:24px;height:24px;": "width:24px;height:24px;display:none" %>'
                                                                alt="" />
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px">
                                                                <div>
                                                                    <b>
                                                                        <%#: Eval("ItemName1")%>&nbsp;(<%#: Eval("ItemCode") %>)</b></div>
                                                                <div>
                                                                    <span>
                                                                        <%#: Eval("ParamedicName")%></span>
                                                                </div>
                                                                <div>
                                                                    <i>Kelas Tagihan</i>&nbsp;<span><b><%#: Eval("ChargeClassName")%></b></span>&nbsp;<br />
                                                                    <span style="color: Maroon">
                                                                        <%#: Eval("TransactionNo")%></span>
                                                                </div>
                                                                <%--<div>
                                                                    <a class="lnkNursingNote" style='<%# Eval("IsLinkedToNursingNote").ToString() == "False" ? "display:none;": "" %> max-width:20px;
                                                                        min-width: 20px;'>
                                                                        <%=GetLabel("Catatan Perawat") %></a></div>--%>
                                                            </div>
                                                        </td>
                                                        <td align="center">
                                                            <div style="padding: 3px;">
                                                                <div>
                                                                    <%# Eval("TransactionDateTimeInString")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("Tariff", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("ChargedQuantity")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px;">
                                                                <div>
                                                                    <%#: Eval("ItemUnit")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("GrossLineAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("DiscountAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("PayerAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("PatientAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("LineAmount", "{0:N}")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: right;">
                                                                <div>
                                                                    <%#: Eval("CreatedByUserName")%></div>
                                                                <div>
                                                                    <%#: Eval("CreatedDateTimeInString")%></div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px; text-align: center;">
                                                                <asp:CheckBox ID="chkIsReviewed" runat="server" Checked='<%# Eval("IsReviewed")%>'
                                                                    Enabled="false" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
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
