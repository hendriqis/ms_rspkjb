<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="PatientBillDetailReprintList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillDetailReprintList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientBillDetailCtl.ascx" TagName="ctlGrdRegisteredPatient"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <div style="height: 50px">
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = 'RegistrationID = ' + getCurrentID();
            var regID = getCurrentID();
            var linkedRegID = "0";
            var healthcareServiceUnitID = "0";
            Methods.getObject('GetRegistrationList', registrationID, function (result) {
                if (result != null) {
                    linkedRegID = result.LinkedRegistrationID;
                }
            });

            Methods.getObject('GetConsultVisitList', registrationID, function (result1) {
                if (result1 != null) {
                    healthcareServiceUnitID = result1.HealthcareServiceUnitID;
                }
            });

            if (getFilterExpression(filterExpression)) {
                if (code == 'PM-00203' || code == 'PM-00204' || code == 'PM-00210' || code == 'PM-00211' || code == 'PM-00212' || code == 'PM-00213') {
                    if (linkedRegID != 0 && linkedRegID != null) {
                        filterExpression.text = '((RegistrationID = ' + linkedRegID + ' AND IsChargesTransfered = 1) OR RegistrationID = ' + regID + ')';
                    } else {
                        filterExpression.text = registrationID;
                    }
                } else if (code == 'PM-00263' || code == 'PM-00231' || code == 'PM-00224') {
                    filterExpression.text = regID + ';' + '0';
                } else if (code == 'PM-002176' || code == 'PM-002177') {
                    filterExpression.text = registrationID;
                }
                else if (code == 'PM-00323' || code == 'PM-00242' || code == 'PM-00243' || code == 'PM-90028' || code == 'MR000016' || code == 'MR000065') {
                    filterExpression.text = regID;
                }
                else if (code == 'PM-00224' || code == 'PM-00231' || code == 'PM-00262' || code == 'PM-00263'
                    || code == 'PM-00258' || code == 'PM-00259' || code == 'PM-00260' || code == 'PM-00261' || code == 'PM-00262'
                    || code == 'PM-00263' || code == 'PM-00264' || code == 'PM-00265' || code == 'PM-00266' || code == 'PM-00267'
                    || code == 'PM-00268' || code == 'PM-00269' || code == 'PM-00270' || code == 'PM-00271' || code == 'PM-00276'
                    || code == 'PM-00277' || code == 'PM-00332' || code == 'PM-002168' || code == 'PM-00721') {
                    filterExpression.text = regID + ';' + healthcareServiceUnitID;
                }
                else {
                    filterExpression.text = registrationID;
                }
                return true;
            }
            else {
                errMessage.text = 'Please Select Patient First!';
                return false;
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            var regID = getCurrentID();
            if (code == 'paymentLetter') {
                var param = regID + "|pl";
                return param;
            }
            else if (code == 'paymentDifferenceLetter') {
                var param = regID + "|pdl";
                return param;
            } else {
                return regID;
            }
        }

        $(function () {
            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
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

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGridView();
            }, 0);
        }

        function onCboServiceUnitValueChanged(s) {
            onRefreshGridView();
        }
    </script>
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <table class="tblContentArea" style="width: 100%">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <fieldset id="fsPatientList">
                    <table>
                        <colgroup>
                            <col style="width: 150px">
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Quick Search")%></label>
                            </td>
                            <td>
                                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                    Width="300px" Watermark="Search">
                                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                    <IntellisenseHints>
                                        <qis:QISIntellisenseHint Text="Nama" FieldName="PatientName" />
                                        <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                        <qis:QISIntellisenseHint Text="No.Registrasi" FieldName="RegistrationNo" />
                                        <qis:QISIntellisenseHint Text="Alamat (Jalan)" FieldName="StreetName" />
                                        <qis:QISIntellisenseHint Text="Alamat (Kota)" FieldName="City" />
                                        <qis:QISIntellisenseHint Text="Tanggal Masuk" FieldName="RegistrationDate" Description="yyyy-mm-dd" />
                                        <qis:QISIntellisenseHint Text="Tanggal Pulang" FieldName="DischargeDate"/>
                                        
                                    </IntellisenseHints>
                                </qis:QISIntellisenseTextBox>
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
                </fieldset>
                <uc1:ctlGrdRegisteredPatient runat="server" id="grdRegisteredPatient" />
            </td>
        </tr>
    </table>
</asp:Content>
