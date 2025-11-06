<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master"
    AutoEventWireup="true" CodeBehind="InformationPatientTransferList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.InformationPatientTransferList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientRegistrationTransferCtl.ascx"
    TagName="ctlGrdInpatientReg" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        //#region tab
        $(function () {
            $('#ulTabLabResult li').click(function () {
                $('#ulTabLabResult li.selected').removeAttr('class');
                $('.containerInfo').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
        });
        //#endregion

        //#region Sudah Registrasi Ranap
        $(function () {
            $('#lblRefresh.lblLink').click(function () {
                onRefreshGridView();
            });

            setDatePicker('<%=txtFromRegistrationDate.ClientID %>');
            $('#<%=txtFromRegistrationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtToRegistrationDate.ClientID %>');
            $('#<%=txtToRegistrationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
        });

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                refreshGrdRegisteredPatient();
            }
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                    onRefreshGridView();
            }, 0);
        }
        //#endregion

        //#region Belum Registrasi Ranap
        $(function () {
            $('#lblRefresh2.lblLink').click(function () {
                onRefreshGridViewPatientDischargeTransferInpatient();
            });

            setDatePicker('<%=txtFromRegistrationDate2.ClientID %>');
            $('#<%=txtFromRegistrationDate2.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtToRegistrationDate2.ClientID %>');
            $('#<%=txtToRegistrationDate2.ClientID %>').datepicker('option', 'maxDate', '0');
        });

        function onCboChanged() {
            $('#<%=hdnFilterCboServiceUnit2.ClientID %>').val(cboServiceUnit2.GetValue());
        }

        function oncbpViewPatientDischargeTransferInpatientEndCallback() {
            hideLoadingPanel();
        }

        function onRefreshGridViewPatientDischargeTransferInpatient() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                $('#<%=hdnFilterCboServiceUnit2.ClientID %>').val(cboServiceUnit2.GetValue());
                $('#<%=hdnFilterExpressionQuickSearch2.ClientID %>').val(txtSearchView2.GenerateFilterExpression());
                cbpViewPatientDischargeTransferInpatient.PerformCallback('refresh');
            }
        }

        function onTxtSearchView2SearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                    onRefreshGridViewPatientDischargeTransferInpatient();
            }, 0);
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch2" runat="server" />
    <input type="hidden" value="" id="hdnFilterCboServiceUnit2" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <div style="padding: 15px">
        <div class="containerUlTabPage" style="margin-bottom: 3px;">
            <ul class="ulTabPage" id="ulTabLabResult">
                <li contentid="containerAfterRegistration" class="selected">
                    <%=GetLabel("Sudah Transfer Ranap")%></li>
                <li contentid="containerBeforeRegistration">
                    <%=GetLabel("Belum Transfer Ranap")%></li>
            </ul>
        </div>
        <div id="containerAfterRegistration" class="containerInfo">
            <div class="pageTitle">
                <%=GetLabel("Informasi Pasien Transfer Ke Rawat Inap")%></div>
            <table class="tblContentArea" style="width: 100%">
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <fieldset id="fsPatientList">
                            <table>
                                <colgroup>
                                    <col style="width: 10%" />
                                </colgroup>
                                <tr>
                                    <td width="200px">
                                        <%=GetLabel("Unit Pelayanan")%>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboServiceUnit" Width="300px" ClientInstanceName="cboKlinik"
                                            runat="server">
                                        </dxe:ASPxComboBox>
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
                                                <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                                <qis:QISIntellisenseHint Text="No Register" FieldName="RegistrationNo" />
                                                <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                                <qis:QISIntellisenseHint Text="Alamat" FieldName="StreetName" />
                                            </IntellisenseHints>
                                        </qis:QISIntellisenseTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Tanggal Registrasi")%></label>
                                    </td>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 145px" />
                                                <col style="width: 3px" />
                                                <col style="width: 145px" />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtFromRegistrationDate" Width="120px" CssClass="datepicker" runat="server" />
                                                </td>
                                                <td>
                                                    <%=GetLabel("s/d") %>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtToRegistrationDate" Width="120px" CssClass="datepicker" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <div style="padding: 7px 0 0 3px; font-size: 1em">
                                <span class="lblLink" id="lblRefresh">[ REFRESH ]</span>
                            </div>
                        </fieldset>
                        <uc1:ctlGrdInpatientReg runat="server" ID="grdInpatientReg" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="containerBeforeRegistration" class="containerInfo" style="display: none">
            <div class="pageTitle">
                <%=GetLabel("Informasi Pasien Transfer : Baru Discharge Dokter")%></div>
            <table class="tblContentArea" style="width: 100%">
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <fieldset id="Fieldset3">
                            <table>
                                <colgroup>
                                    <col style="width: 10%" />
                                </colgroup>
                                <tr>
                                    <td width="200px">
                                        <%=GetLabel("Unit Pelayanan")%>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboServiceUnit2" Width="300px" ClientInstanceName="cboServiceUnit2"
                                            runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e){ onCboChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("Quick Filter")%></label>
                                    </td>
                                    <td>
                                        <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView2" ID="txtSearchView2"
                                            Width="300px" Watermark="Search">
                                            <ClientSideEvents SearchClick="function(s){ onTxtSearchView2SearchClick(s); }" />
                                            <IntellisenseHints>
                                                <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                                <qis:QISIntellisenseHint Text="No Register" FieldName="RegistrationNo" />
                                                <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                                <qis:QISIntellisenseHint Text="Alamat" FieldName="StreetName" />
                                            </IntellisenseHints>
                                        </qis:QISIntellisenseTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Tanggal Registrasi")%></label>
                                    </td>
                                    <td>
                                        <table cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 145px" />
                                                <col style="width: 3px" />
                                                <col style="width: 145px" />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtFromRegistrationDate2" Width="120px" CssClass="datepicker" runat="server" />
                                                </td>
                                                <td>
                                                    <%=GetLabel("s/d") %>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtToRegistrationDate2" Width="120px" CssClass="datepicker" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <div style="padding: 7px 0 0 3px; font-size: 1em">
                                <span class="lblLink" id="lblRefresh2">[ REFRESH ]</span>
                            </div>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <dxcp:ASPxCallbackPanel ID="cbpViewPatientDischargeTransferInpatient" runat="server"
                            Width="100%" ClientInstanceName="cbpViewPatientDischargeTransferInpatient" OnCallback="cbpViewPatientDischargeTransferInpatient_Callback"
                            ShowLoadingPanel="false">
                            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { oncbpViewPatientDischargeTransferInpatientEndCallback(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent4" runat="server">
                                    <asp:Panel runat="server" ID="pnlGridView2" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em; height: 500px; overflow-y: scroll;">
                                        <table id="Table1" class="grdSelected grdPatientPage" cellspacing="0" rules="all">
                                            <asp:ListView ID="lvwPatientDischargeTransferInpatient" runat="server">
                                                <EmptyDataTemplate>
                                                    <table id="tblViewPatientDischargeTransferInpatient" runat="server" class="grdCollapsible lvwPatientDischargeTransferInpatient"
                                                        cellspacing="0" rules="all">
                                                        <tr>
                                                            <th style="width: 150px" align="left">
                                                                <%=GetLabel("DATA KUNJUNGAN")%>
                                                            </th>
                                                            <th style="width: 120px" align="left">
                                                                <%=GetLabel("UNIT PELAYANAN")%>
                                                            </th>
                                                            <th style="width: 200px" align="left">
                                                                <%=GetLabel("PASIEN")%>
                                                            </th>
                                                            <th style="width: 140px" align="left">
                                                                <%=GetLabel("DPJP TUJUAN RANAP")%>
                                                            </th>
                                                            <th style="width: 160px" align="left">
                                                                <%=GetLabel("INFORMASI DISCHARGE")%>
                                                            </th>
                                                        </tr>
                                                        <tr class="trEmpty">
                                                            <td colspan="5">
                                                                <%=GetLabel("No Data To Display") %>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="tblViewPatientDischargeTransferInpatient" runat="server" class="grdCollapsible lvwPatientDischargeTransferInpatient"
                                                        cellspacing="0" rules="all">
                                                        <tr>
                                                            <th style="width: 150px" align="left">
                                                                <%=GetLabel("DATA KUNJUNGAN")%>
                                                            </th>
                                                            <th style="width: 120px" align="left">
                                                                <%=GetLabel("UNIT PELAYANAN")%>
                                                            </th>
                                                            <th style="width: 200px" align="left">
                                                                <%=GetLabel("PASIEN")%>
                                                            </th>
                                                            <th style="width: 140px" align="left">
                                                                <%=GetLabel("DPJP TUJUAN RANAP")%>
                                                            </th>
                                                            <th style="width: 160px" align="left">
                                                                <%=GetLabel("INFORMASI DISCHARGE DOKTER")%>
                                                            </th>
                                                        </tr>
                                                        <tr runat="server" id="itemPlaceholder">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td align="left">
                                                            <div style="color: Blue; font-weight: bold;">
                                                                <%#: Eval("RegistrationNo") %></div>
                                                            <div>
                                                                <%#: Eval("cfRegistrationDate")%>
                                                                <%#: Eval("RegistrationTime")%></div>
                                                            <div>
                                                                <%#: Eval("ParamedicName")%>
                                                            </div>
                                                        </td>
                                                        <td align="left">
                                                            <%#: Eval("cfServiceUnitRoomBed")%>
                                                        </td>
                                                        <td align="left">
                                                            <div>
                                                                <b>
                                                                    <%#: Eval("PatientName") %>
                                                                    (<%#: Eval("MedicalNo") %>)</b></div>
                                                            <div>
                                                                <%#: Eval("cfDateOfBirth")%>
                                                                (<%#: Eval("PatientAge") %>,
                                                                <%#: Eval("cfGenderInitial") %>,
                                                                <%#: Eval("Religion") %>)</div>
                                                            <div>
                                                                <%#: Eval("StreetName")%>
                                                                <%#: Eval("cfPhoneNo")%>
                                                                <%#: Eval("cfMobilePhoneNo")%></div>
                                                        </td>
                                                        <td align="left">
                                                            <b>
                                                                <%#: Eval("ReferralPhysicianByName")%></b>
                                                        </td>
                                                        <td align="left">
                                                            <div>
                                                                <%#: Eval("cfPhysicianDischargedDate")%>
                                                                <%#: Eval("cfPhysicianDischargedTime")%></div>
                                                            <div>
                                                                Oleh :
                                                                <%#: Eval("PhysicianDischargedByName")%></div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </table>
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
    </div>
</asp:Content>
