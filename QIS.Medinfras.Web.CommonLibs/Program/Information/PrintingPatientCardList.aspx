<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true" 
    CodeBehind="PrintingPatientCardList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PrintingPatientCardList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientBillDetailCtl.ascx" TagName="ctlGrdRegisteredPatientPrint"
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
        $(function () {
            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            $('#<%=txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            
            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });

        });

        $('#<%=chkIsIgnoreDate.ClientID %>').die();
        $('#<%=chkIsIgnoreDate.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtRegistrationDate.ClientID %>').attr('readonly', 'readonly');
            }
            else $('#<%=txtRegistrationDate.ClientID %>').removeAttr('readonly');
            onRefreshGridView();
        });

        $('#<%=txtRegistrationDate.ClientID %>').live('change', function (evt) {
            onRefreshGridView();
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

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = 'RegistrationID = ' + getCurrentID();
            var MRN = 0;
            Methods.getObject('GetRegistrationList', registrationID, function (result) {
                if (result != null) {
                    MRN = result.MRN;
                }
            });
            if (getFilterExpression(filterExpression)) {
                if (code == 'PM-00106') {
                    filterExpression.text = MRN;
                } else {
                    filterExpression.text = registrationID;
                }
                return true;
            }
            else {
                errMessage.text = 'Please Select Patient First!';
                return false;
            }
        }
    </script>
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" />
    <table class="tblContentArea" style="width: 100%">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <fieldset id="fsPatientList">
                    <table>
                        <colgroup>
                            <col style="width: 150px">
                        </colgroup>
                        <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal Registrasi")%></label></td>
                                <td><asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" /><asp:CheckBox ID="chkIsIgnoreDate" runat="server" Checked="false" /><%:GetLabel("Abaikan Tanggal")%>&nbsp;</td>
                        </tr>
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
                                        <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                        <qis:QISIntellisenseHint Text="No. Registrasi" FieldName="RegistrationNo" />
                                        <qis:QISIntellisenseHint Text="Tanggal Masuk" FieldName="RegistrationDate" Description="yyyy-mm-dd" />
                                        <qis:QISIntellisenseHint Text="Tanggal Pulang" FieldName="DischargeDate" Description="yyyy-mm-dd" />
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
                <uc1:ctlGrdRegisteredPatientPrint runat="server" id="grdRegisteredPatientPrint" />
            </td>
        </tr>
    </table>
</asp:Content>
