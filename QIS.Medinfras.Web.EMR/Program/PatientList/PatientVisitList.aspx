<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master"
    AutoEventWireup="true" CodeBehind="PatientVisitList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PatientVisitList" %>

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
            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });

            setDatePicker('<%=txtRealisationDate.ClientID %>');
            $('#<%=txtRealisationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=txtRealisationDate.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });

            function getHealthcareServiceUnitFilterExpression() {
                var filterExpression = '<%=GetServiceUnitUserRoleFilterParameter() %>;' + cboPatientFrom.GetValue() + ';';
                return filterExpression;
            }

            $('#<%=chkIsIncludeClosed.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });

            onCboPatientFromValueChanged();

            $('#<%=txtBarcodeEntry.ClientID %>').focus();
        });

        $('.lvwView > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if (!isHoverTdExpand && !isHoverTdPatientCall) {
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

        var isHoverTdPatientCall = false;
        $('.lvwView tr:gt(0) td.tdPatientCall').live({
            mouseenter: function () { isHoverTdPatientCall = true; },
            mouseleave: function () { isHoverTdPatientCall = false; }
        });

        $('.lvwView tr:gt(0) td.tdPatientCall').live('click', function () {
            $tr = $(this).parent().next();
            var registrationID = $tr.find('.hdnRegistrationID').val();
            var visitID = $tr.find('.hdnVisitID').val();
            var roomCode = $tr.find('.hdnRoomCode').val();
            cbpView.PerformCallback('call|' + registrationID + '|' + visitID + '|' + roomCode);
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

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            cbpView.PerformCallback('refresh');
        }, interval);

        function onRefreshGridView() {
            window.clearInterval(intervalID);
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpView.PerformCallback('refresh');
            intervalID = window.setInterval(function () {
                onRefreshGridView();
            }, interval);
        }

        function onBeforeOpenTransactionDt() {
            $('#<%=hdnIsGoToPatientPage.ClientID %>').val("1");
            return ($('#<%=hdnTransactionNo.ClientID %>').val() != '');
        }

        function onCboPatientFromValueChanged() {
            var cboValue = cboPatientFrom.GetValue();
            $('#<%=hdnDepartmentID.ClientID %>').val(cboValue);
            if ((cboValue == Constant.Facility.INPATIENT) || (cboValue == Constant.Facility.EMERGENCY))
                $('#trRegistrationDate').attr('style', 'display:none');
            else
                $('#trRegistrationDate').removeAttr('style');

            cbpOnDepartmentChangedCallback.PerformCallback();
        }

        function onCboServiceUnitValueChanged() {
            var cboValue = cboServiceUnit.GetValue();
            $('#<%=hdnServiceUnitID.ClientID %>').val(cboValue);
            cbpView.PerformCallback('refresh');
        }

        //#region Barcode
        $(function () {
            $('#<%=txtBarcodeEntry.ClientID %>').keypress(function (e) {
                var keyCode = e.keyCode || e.which;

                if (keyCode == 9) {
                    cbpBarcodeEntryProcess.PerformCallback();
                }
            });
        });

        function onCbpBarcodeEntryProcessEndCallback(s) {
            if (s.cpUrl != '')
                document.location = s.cpUrl;
            else {
                showToast('Warning', 'Pasien tidak ditemukan', function () {
                    $('#<%=txtBarcodeEntry.ClientID %>').val('');
                });
                hideLoadingPanel();
            }
        }
        //#endregion

        function OnCbpDepartmentChangedEndCallback(s) {
            cbpView.PerformCallback('refresh');
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
    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnPatientPageByDepartment" runat="server" value="0" />
    <input type="hidden" id="hdnIsSetByUserConfig" runat="server" value="0" />
    <input type="hidden" id="hdnIsGoToPatientPage" runat="server" value="0" />
    <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnPhysicianPatientCall" value="" runat="server" />
    <input type="hidden" id="hdnIsBridgingWithMedinlink" value="" runat="server" />
    <input type="hidden" id="hdnEM0099" value="0" runat="server" />
    <div style="padding: 15px">
        <div id="containerDaftar" class="containerOrder">
            <div class="pageTitle">
                <%=GetLabel("My Patient")%></div>
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
                                            <%=GetLabel("Barcode")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBarcodeEntry" Width="555px" Height="23px" runat="server" TextMode="MultiLine" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Asal Pasien ")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboPatientFrom" ClientInstanceName="cboPatientFrom" runat="server"
                                            Width="100%">
                                            <ClientSideEvents ValueChanged="function(s,e) { onCboPatientFromValueChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Unit Pelayanan ")%></label>
                                    </td>
                                    <td>
                                        <dxcp:ASPxCallbackPanel ID="cbpOnDepartmentChangedCallback" runat="server" Width="100%"
                                            ClientInstanceName="cbpOnDepartmentChangedCallback" ShowLoadingPanel="false"
                                            OnCallback="cbpOnDepartmentChanged_Callback">
                                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ OnCbpDepartmentChangedEndCallback(s); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent2" runat="server">
                                                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="350px"
                                                        runat="server">
                                                        <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitValueChanged(); }" />
                                                    </dxe:ASPxComboBox>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dxcp:ASPxCallbackPanel>
                                    </td>
                                </tr>
                                <tr id="trRegistrationDate">
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Tanggal")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRealisationDate" Width="120px" runat="server" CssClass="datepicker" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("Quick Filter")%></label>
                                    </td>
                                    <td>
                                        <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                            Width="100%" Watermark="Search">
                                            <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                            <IntellisenseHints>
                                                <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                                <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                                <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            </IntellisenseHints>
                                        </qis:QISIntellisenseTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsIncludeClosed" runat="server" Checked="false" /><%:GetLabel("Include Closed Registration")%>
                                    </td>
                                </tr>
                            </table>
                            <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                                <%=GetLabel("Halaman Ini Akan")%>
                                <span class="lblLink" id="lblRefresh">[refresh]</span>
                                <%=GetLabel("setiap")%>
                                <%=GetRefreshGridInterval() %>
                                <%=GetLabel("menit")%>
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
                                                            <th style="width: 2px">
                                                            </th>
                                                            <th style="width: 15px">
                                                                <%=GetLabel("SESI")%>
                                                            </th>
                                                            <th style="width: 15px">
                                                                <%=GetLabel("ANTRIAN")%>
                                                            </th>
                                                            <th style="width: 450px" align="left">
                                                                <%=GetLabel("PASIEN")%>
                                                            </th>
                                                            <th style="width: 350px" align="left">
                                                                <%=GetLabel("INFORMASI KUNJUNGAN")%>
                                                            </th>
                                                            <th align="left">
                                                                <%=GetLabel("ALAMAT DAN KONTAK PASIEN")%>
                                                            </th>
                                                            <th style="width: 15px">
                                                                CC
                                                            </th>
                                                            <th style="width: 15px">
                                                                Dx
                                                            </th>
                                                            <th style="width: 15px">
                                                            </th>
                                                        </tr>
                                                        <tr class="trEmpty">
                                                            <td colspan="10">
                                                                <%=GetLabel("Tidak ada data kunjungan pasien")%>
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
                                                            <th style="width: 2px">
                                                            </th>
                                                            <th style="width: 15px">
                                                                <%=GetLabel("SESI")%>
                                                            </th>
                                                            <th style="width: 15px">
                                                                <%=GetLabel("ANTRIAN")%>
                                                            </th>
                                                            <th style="width: 450px" align="left">
                                                                <%=GetLabel("PASIEN")%>
                                                            </th>
                                                            <th style="width: 350px" align="left">
                                                                <%=GetLabel("INFORMASI KUNJUNGAN")%>
                                                            </th>
                                                            <th align="left">
                                                                <%=GetLabel("ALAMAT DAN KONTAK PASIEN")%>
                                                            </th>
                                                            <th style="width: 15px">
                                                            </th>
                                                            <th style="width: 15px">
                                                            </th>
                                                            <th style="width: 15px">
                                                            </th>
                                                            <th style="width: 15px">
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
                                                        <td align="center" id="tdServiceFlag" runat="server">
                                                        </td>
                                                        <td align="center" id="tdIndicatorSession" runat="server" style="width: 30px">
                                                            <div <%# Eval("DepartmentID").ToString() != "INPATIENT" ? "" : "style='display:none'" %>>
                                                                <span style="font-weight: bold; font-size: 12pt">
                                                                    <%#: Eval("Session") %></span>
                                                            </div>
                                                        </td>
                                                        <td align="center" id="tdIndicator" runat="server" style="width: 30px">
                                                            <div <%# Eval("DepartmentID").ToString() != "OUTPATIENT" && Eval("DepartmentID").ToString() != "DIAGNOSTIC" ? "Style='display:none'":"" %>>
                                                                <span style="font-weight: bold; font-size: 12pt">
                                                                    <%#: Eval("QueueNo") %></span>
                                                            </div>
                                                            <div <%# Eval("DepartmentID").ToString() == "OUTPATIENT" ? "Style='display:none'":"style='font-size: 13pt; font-weight: bold'" %>>
                                                                <%#: Eval("BedCode") %></div>
                                                        </td>
                                                        <td>
                                                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td align="center" valign="top" style="width: 20px">
                                                                        <div <%# Eval("DepartmentID").ToString() != "OUTPATIENT" ? "Style='display:none'":"Style='display:none'" %>>
                                                                            <img id="imgPatientSatisfactionLevelImageUri" runat="server" width="24" height="24"
                                                                                alt="" visible="true" />
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div style="text-align: left">
                                                                            <img class="imgPatientImage" src='<%#Eval("PatientImageUrl") %>' alt="" height="55px"
                                                                                width="40px" style="float: left; margin-right: 10px;" /></div>
                                                                        <div>
                                                                            <span style="font-weight: bold; font-size: 11pt">
                                                                                <%#: Eval("cfPatientNameSalutation") %></span></div>
                                                                        <div>
                                                                            <%#: Eval("MedicalNo") %>,
                                                                            <%#: Eval("DateOfBirthInString") %>,
                                                                            <%#: Eval("Sex") %></div>
                                                                        <div style="font-style: italic">
                                                                            <%#: Eval("BusinessPartner")%></div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                                                        <b>
                                                                            <%#: Eval("RegistrationNo") %></b>
                                                                        <div <%#:Eval("DepartmentID").ToString() != "INPATIENT" ? "style='display:none'":"" %>>
                                                                            <%#: Eval("cfPatientLocation") %></div>
                                                                        <div>
                                                                            <%#: Eval("ParamedicName")%></div>
                                                                        <div style="font-style: italic">
                                                                            <%#: Eval("VisitTypeName")%></div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <div>
                                                                <%#: Eval("HomeAddress")%></div>
                                                            <div style="padding: 3px">
                                                                <img src='<%= ResolveUrl("~/Libs/Images/homephone.png")%>' alt='' style="float: left;" /><div
                                                                    style="margin-left: 30px">
                                                                    <%#: Eval("cfPhoneNo")%>&nbsp;</div>
                                                                <img src='<%= ResolveUrl("~/Libs/Images/mobilephone.png")%>' alt='' style="float: left;" /><div
                                                                    style="margin-left: 30px">
                                                                    <%#: Eval("cfMobilePhoneNo")%>&nbsp;</div>
                                                            </div>
                                                        </td>
                                                        <td align="center">
                                                            <div id="divChiefComplaint" runat="server" style="text-align: center; color: blue">
                                                            </div>
                                                        </td>
                                                        <td align="center">
                                                            <div id="divDiagnosis" runat="server" style="text-align: center; color: blue">
                                                            </div>
                                                        </td>
                                                        <td align="center">
                                                            <img class="imgCOB <%# Eval("IsUsingCOB").ToString() == "1" ? "imgDisabled" : "imgLink"%>"
                                                                title='<%# Eval("COB_Name")%>' src=' <%# ResolveUrl("~/Libs/Images/Status/cob.png") %>'
                                                                style='<%# Eval("IsUsingCOB").ToString() == "True" ? "width:25px": "width:25px;height:25px;display:none" %>'
                                                                alt="" />
                                                        </td>
                                                        <td align="center" class="tdPatientCall">
                                                            <div id="divPatientCall" runat="server" style="display: none">
                                                                <input type="button" class="btnPatientCall w3-btn w3-hover-blue" value="Panggil Pasien"
                                                                    style="background-color: Red; color: White; width: 120px;" /></div>
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none" class="trDetail">
                                                        <td>
                                                            <div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div>
                                                                &nbsp;</div>
                                                        </td>
                                                        <td>
                                                            <div>
                                                                &nbsp;</div>
                                                        </td>
                                                        <td>
                                                            <div style="padding: 3px">
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
                                                            <div>
                                                                <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                                                <input type="hidden" class="hdnRegistrationID" value='<%#: Eval("RegistrationID") %>' />
                                                                <input type="hidden" class="hdnRoomCode" value='<%#: Eval("RoomCode") %>' />
                                                                <div style="float: left">
                                                                    <%#: Eval("VisitDateInString")%></div>
                                                                <div style="margin-left: 100px">
                                                                    <%#: Eval("VisitTime")%></div>
                                                                <div id="divDischargeDate" runat="server">
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <div>
                                                                &nbsp;</div>
                                                        </td>
                                                        <td>
                                                            <div>
                                                                &nbsp;</div>
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
    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpBarcodeEntryProcess" runat="server" Width="100%" ClientInstanceName="cbpBarcodeEntryProcess"
            ShowLoadingPanel="false" OnCallback="cbpBarcodeEntryProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpBarcodeEntryProcessEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
