<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master"
    AutoEventWireup="true" CodeBehind="InPatientList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.InPatientList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridInpatientRegistrationCtl.ascx"
    TagName="ctlGrdInpatientReg" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridInpatientRegistrationAllCtl.ascx"
    TagName="ctlGrdInpatientRegAll" TagPrefix="uc2" %>
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
                onRefreshGridView();
            });

            $('#lblRefresh.lblLink').click(function () {
                onRefreshGridView();
            });
        });

        function onHideContainer() {
            $('#containerDetail.containerInfo').attr('style', 'display:none');
        }
        //#endregion

        //#region Physician
        function onGetPhysicianFilterExpression() {
            var filterExpression = "";
 
            filterExpression = "ParamedicID IN (SELECT DISTINCT ParamedicID FROM ConsultVisit WHERE GCVisitStatus IN ('X020^002','X020^003') AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vHealthcareServiceUnit WHERE DepartmentID = 'INPATIENT'))";
            return filterExpression;
        }
        $('#lblPhysician.lblLink').live('click', function () {
            openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtPhysicianCodeChanged(value);
            });
        });

        $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPhysicianCodeChanged($(this).val());
        });

        function onTxtPhysicianCodeChanged(value) {
            var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%=txtPhysicianCode.ClientID %>').val('');
                    $('#<%=hdnPhysicianID.ClientID %>').val('');
                    $('#<%=txtPhysicianName.ClientID %>').val('');
                }
                cbpView.PerformCallback('refresh');
            });
        }
        //#endregion

        function onCboServiceUnitRekapChanged() {
            onRefreshGridView();
        }

        function onCboServiceUnitDetailChanged() {
            onRefreshGridView();
        }

        function oncboClassChanged() {
            onRefreshGridView();
        }

        $('#<%=chkIsChargeClass.ClientID %>').die();
        $('#<%=chkIsChargeClass.ClientID %>').live('change', function () {
            onRefreshGridView();
        });

        //#region BusinessPartner
        $('#<%=lblBusinessPartner.ClientID %>.lblLink').live('click', function () {
            var filterExpression = "isDeleted = 0 AND isActive = 1 AND isBlackList = 0";
            openSearchDialog('payer', filterExpression, function (value) {
                $('#<%=txtBusinessPartnerCode.ClientID %>').val(value);
                onTxtBusinessPartnerChanged(value);
            });
        });

        $('#<%=txtBusinessPartnerCode.ClientID %>').live('change', function () {
            onTxtBusinessPartnerChanged($(this).val());
        });

        function onTxtBusinessPartnerChanged(value) {
            var filterExpression = "BusinessPartnerCode = '" + value + "'";
            Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnBusinessPartnerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%=txtBusinessPartnerCode.ClientID %>').val(result.BusinessPartnerCode);
                    $('#<%=txtBusinessPartnerName.ClientID %>').val(result.BusinessPartnerName);
                }
                else {
                    $('#<%=hdnBusinessPartnerID.ClientID %>').val('');
                    $('#<%=txtBusinessPartnerCode.ClientID %>').val('');
                    $('#<%=txtBusinessPartnerName.ClientID %>').val('');
                }
                onRefreshGridView();
            });
        }
        //#endregion

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

        //        function onRefreshGrid() {
        //            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
        //            if (IsValid(null, 'fsPatientList', 'mpPatientList'))
        //                refreshGrdRegisteredPatient();
        //        }

        //        function onRefreshGridView() {
        //            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
        //                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
        //                refreshGrdRegisteredPatient();
        //            }
        //        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                    onRefreshGridView();
            }, 0);
        }


        function onBeforeRightPanelPrint(code, filterExpression) {
            if (code == 'PM-00439' || code == 'PM-00624' || code == 'PM-00627') {
              filterExpression.text = getFilterReport();
            }
            return true;
        }
         
    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <div style="padding: 15px">
        <div class="containerUlTabPage" style="margin-bottom: 3px;">
            <ul class="ulTabPage" id="ulTabLabResult">
                <li class="selected" contentid="containerAll">
                    <%=GetLabel("REKAP")%></li>
                <li contentid="containerDetail">
                    <%=GetLabel("DETAIL")%></li>
            </ul>
        </div>
        <div id="containerAll" class="containerInfo">
            <div class="pageTitle">
                <%=GetLabel("Informasi Pasien Dirawat : REKAP")%></div>
            <table class="tblContentArea" style="width: 100%">
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <uc2:ctlGrdInpatientRegAll runat="server" ID="grdInpatientRegAll" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="containerDetail" class="containerInfo" style="display: none">
            <div class="pageTitle">
                <%=GetLabel("Informasi Pasien Dirawat : DETAIL")%></div>
            <table class="tblContentArea" style="width: 100%">
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <fieldset id="fsPatientList">
                            <table>
                                <colgroup>
                                    <col style="width: 100px" />
                                    <col style="width: 150px" />
                                </colgroup>
                                <tr>
                                    <td width="200px">
                                        <%=GetLabel("Ruang Perawatan")%>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboServiceUnitDetail" Width="100%" ClientInstanceName="cboServiceUnitDetail"
                                            runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e){ onCboServiceUnitDetailChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="200px">
                                        <%=GetLabel("Kelas")%>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboClass" Width="100%" ClientInstanceName="cboClass" runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e){ oncboClassChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsChargeClass" runat="server" Checked="false" Text="Kelas Tagihan" />
                                    </td>
                                </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblLink" id="lblPhysician"><%=GetLabel("Dokter / Tenaga Medis")%></label></td>
                                <td>
                                    <input type="hidden" id="hdnPhysicianID" runat="server" value="" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:30%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtPhysicianCode" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                                <tr>
                                    <td>
                                        <label class="lblLink lblNormal" runat="server" id="lblBusinessPartner">
                                            <%=GetLabel("Penjamin Bayar")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnBusinessPartnerID" value="" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 30%" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtBusinessPartnerCode" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBusinessPartnerName" Width="100%" runat="server" ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table>
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
                                                <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                                <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                                <qis:QISIntellisenseHint Text="Alamat" FieldName="StreetName City" />
                                                <qis:QISIntellisenseHint Text="Nama Ayah" FieldName="FatherName" />
                                                <qis:QISIntellisenseHint Text="Nama Ibu" FieldName="MotherName" />
                                                <qis:QISIntellisenseHint Text="Nama Pasangan" FieldName="SpouseName" />
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
                        </fieldset>
                        <uc1:ctlGrdInpatientReg runat="server" ID="grdInpatientReg" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
