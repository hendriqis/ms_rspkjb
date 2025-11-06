<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master"
    AutoEventWireup="true" CodeBehind="PatientVisitListLink.aspx.cs" Inherits="QIS.Medinfras.Web.Nursing.Program.PatientVisitListLink" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            $('#trServiceUnit').attr('style', 'display:none');

            $('#<%=txtRegistrationDate.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });

            function getHealthcareServiceUnitFilterExpression() {
                var filterExpression = '<%=GetServiceUnitUserRoleFilterParameter() %>;' + cboPatientFrom.GetValue() + ';';
                return filterExpression;
            }

            $('#txtBarcodeEntry').focus();
        });

        $('.lvwView > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if (!isHoverTdExpand) {
                showLoadingPanel();
                $('#<%=hdnTransactionNo.ClientID %>').val($(this).find('.hdnVisitID').val());
                __doPostBack('<%=btnOpenTransactionDt.UniqueID%>', '');
            }
        });

        var isHoverTdExpand = false;
        $('.lvwView tr:gt(0) td.tdExpand').live({
            mouseenter: function () { isHoverTdExpand = true; },
            mouseleave: function () { isHoverTdExpand = false; }
        });

        $('.lvwView tr:gt(0) td.tdExpand').live('click', function () {
            $tr = $(this).parent().next();
            if (!$tr.is(":visible")) {

                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
                $tr.show('slow');
            }
            else {
                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                $tr.hide('fast');
            }
        });

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            Methods.checkImageError('imgPatientImage', 'patient', 'hdnPatientGender');
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            Methods.checkImageError('imgPatientImage', 'patient', 'hdnPatientGender');
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion

        function onBeforeOpenTransactionDt() {
            return ($('#<%=hdnTransactionNo.ClientID %>').val() != '');
        }

        function onCboPatientFromValueChanged() {
            var cboValue = cboPatientFrom.GetValue();
            if (cboValue == Constant.Facility.INPATIENT) {
                $('#trRegistrationDate').attr('style', 'display:none');
                $('#trServiceUnit').removeAttr('style');
            }
            else if (cboValue == Constant.Facility.EMERGENCY) {
                $('#trRegistrationDate').removeAttr('style');
                $('#trServiceUnit').attr('style', 'display:none');
            }   
            else {
                $('#trRegistrationDate').removeAttr('style');
                $('#trServiceUnit').removeAttr('style');
            }

            cbpView.PerformCallback('refresh');
        }

    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchReg" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="padding: 15px">
        <div id="containerDaftar" class="containerOrder">
            <div class="pageTitle">
                <%=GetLabel("Asuhan Keperawatan")%>
                :
                <%=GetLabel("Pilih Pasien")%></div>
            <fieldset id="fsPatientListReg">
                <table class="tblContentArea" style="width: 100%">
                    <tr>
                        <td style="padding: 5px; vertical-align: top">
                            <table class="tblEntryContent" style="width: 60%;">
                                <colgroup>
                                    <col style="width: 25%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Scan Barcode No. RM")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBarcodeEntry" Width="120px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Asal Pasien")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboPatientFrom" ClientInstanceName="cboPatientFrom" runat="server"
                                            Width="100%">
                                            <ClientSideEvents ValueChanged="function(s,e) { onCboPatientFromValueChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr id="trServiceUnit">
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Unit Pelayanan")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%"
                                            runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e) { onRefreshGridView(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr id="trRegistrationDate">
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Tanggal")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRegistrationDate" Width="120px" CssClass="datepicker" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("Quick Filter")%></label>
                                    </td>
                                    <td>
                                        <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchViewReg"
                                            ID="txtSearchViewReg" Width="100%" Watermark="Search">
                                            <ClientSideEvents SearchClick="function(s){ onTxtSearchViewRegSearchClick(s); }" />
                                            <IntellisenseHints>
                                                <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                                <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                                <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            </IntellisenseHints>
                                        </qis:QISIntellisenseTextBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                                <%=GetLabel("Halaman Ini Akan")%>
                                <span class="lblLink" id="lblRefresh">[refresh]</span>
                                <%=GetLabel("Setiap")%>
                                <%=GetRefreshGridInterval() %>
                                <%=GetLabel("Menit")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="display: none">
                                <asp:Button ID="btnOpenTransactionDt" runat="server" UseSubmitBehavior="false" OnClientClick="return onBeforeOpenTransactionDt();"
                                    OnClick="btnOpenTransactionDt_Click" /></div>
                            <input type="hidden" runat="server" id="hdnTransactionNo" value="" />
                            <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                        <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                            margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                                            <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                                <EmptyDataTemplate>
                                                    <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                                                        rules="all">
                                                        <tr>
                                                            <th style="width: 15px">
                                                            </th>
                                                            <th style="width: 250px" align="left">
                                                                <%=GetLabel("Informasi Registrasi")%>
                                                            </th>
                                                            <th style="width: 400px" align="left">
                                                                <%=GetLabel("Informasi Pasien")%>
                                                            </th>
                                                            <th style="width: 350px" align="left">
                                                                <%=GetLabel("Informasi Kontak")%>
                                                            </th>
                                                            <th>
                                                                <%=GetLabel("Pembayar")%>
                                                            </th>
                                                        </tr>
                                                        <tr class="trEmpty">
                                                            <td colspan="6">
                                                                <%=GetLabel("No Data To Display")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                                                        rules="all">
                                                        <tr>
                                                            <th style="width: 15px">
                                                            </th>
                                                            <th style="width: 250px">
                                                                <%=GetLabel("Informasi Registrasi")%>
                                                            </th>
                                                            <th style="width: 400px">
                                                                <%=GetLabel("Informasi Pasien")%>
                                                            </th>
                                                            <th style="width: 350px">
                                                                <%=GetLabel("Informasi Kontak")%>
                                                            </th>
                                                            <th>
                                                                <%=GetLabel("Pembayar")%>
                                                            </th>
                                                        </tr>
                                                        <tr runat="server" id="itemPlaceholder">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td class="tdExpand" style="text-align: center">
                                                            <img class="imgExpand" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>' alt='' />
                                                        </td>
                                                        <td>
                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td <%#:Eval("DepartmentID").ToString() != "OUTPATIENT" ? "style='display:none'":"style='width:24px'" %>>
                                                                        <div>
                                                                            <img id="imgPatientSatisfactionLevelImageUri" runat="server" width="24" height="24"
                                                                                alt="" visible="false" />
                                                                        </div>
                                                                    </td>
                                                                    <td align="center" <%#:Eval("TriageColor").ToString() != "" ? String.Format("style='background-color:{0}; width:20px'",Eval("TriageColor")) : "style='display:none'"%>>
                                                                    </td>
                                                                    <td <%#:Eval("DepartmentID").ToString() != "OUTPATIENT" && Eval("DepartmentID").ToString() != "EMERGENCY" ? "style='display:none'":"" %>>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <%#: Eval("RegistrationNo") %>
                                                                        (<%#:Eval("ServiceUnitName") %>)</span>
                                                                        <div <%#:Eval("DepartmentID").ToString() != "INPATIENT" ? "style='display:none'":"" %>>
                                                                            <%=GetLabel("Lama Rawat :") %>
                                                                            <%#:Eval("LOS") %></div>
                                                                        <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <%#: Eval("PatientName") %>
                                                                (<%#: Eval("DateOfBirthInString") %>,
                                                                <%#: Eval("Sex") %>,
                                                                <%#: Eval("MedicalNo") %>)</div>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <%#: Eval("BusinessPartner")%></div>
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none" class="trDetail">
                                                        <td>
                                                            <div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <div>
                                                                    <%#: Eval("RegistrationNo") %></span></div>
                                                                <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                                                <div style="float: left">
                                                                    <%#: Eval("VisitDateInString")%></div>
                                                                <div style="margin-left: 100px">
                                                                    <%#: Eval("VisitTime")%></div>
                                                                <div>
                                                                    <%#: Eval("ParamedicName")%></div>
                                                                <div id="divDischargeDate" runat="server">
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px">
                                                                <img class="imgPatientImage" src='<%#Eval("PatientImageUrl") %>' alt="" height="55px"
                                                                    width="40px" style="float: left; margin-right: 10px;" />
                                                                <div>
                                                                    <%#: Eval("PatientName") %></div>
                                                                <input type="hidden" value='<%#: Eval("GCSex")%>' class="hdnPatientGender" />
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <colgroup>
                                                                        <col style="width: 100px" />
                                                                        <col style="width: 10px" />
                                                                        <col style="width: 80px" />
                                                                        <col style="width: 50px" />
                                                                        <col style="width: 10px" />
                                                                        <col style="width: 120px" />
                                                                    </colgroup>
                                                                    <tr>
                                                                        <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                                            <%=GetLabel("Nama Panggilan")%>
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <%#: Eval("PreferredName")%>
                                                                        </td>
                                                                        <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                                            <%=GetLabel("No RM")%>
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <%#: Eval("MedicalNo")%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                                            <%=GetLabel("Tanggal Lahir")%>
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <%#: Eval("DateOfBirthInString")%>
                                                                        </td>
                                                                        <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                                            <%=GetLabel("Umur")%>
                                                                        </td>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <%#: Eval("PatientAge")%>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px">
                                                                <div>
                                                                    <%#: Eval("HomeAddress")%></div>
                                                                <img src='<%= ResolveUrl("~/Libs/Images/homephone.png")%>' alt='' style="float: left;" /><div
                                                                    style="margin-left: 30px">
                                                                    <%#: Eval("cfPhoneNo")%>&nbsp;</div>
                                                                <img src='<%= ResolveUrl("~/Libs/Images/mobilephone.png")%>' alt='' style="float: left;" /><div
                                                                    style="margin-left: 30px">
                                                                    <%#: Eval("cfMobilePhoneNo")%>&nbsp;</div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div>
                                                                &nbsp;</div>
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
                            <div class="containerPaging">
                                <div class="wrapperPaging">
                                    <div id="paging">
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </div>
</asp:Content>
