<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="MedicalFileRequestList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.MedicalFileRequestList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientBillDetailCtl.ascx" TagName="ctlGrdRegisteredPatient"
    TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li runat="server" id="btnMPEntryPopupSave">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        function onBeforeProcess(filterExpression, errMessage) {
            var registrationID = getCurrentID();
            if (registrationID == '') {
                errMessage.text = 'Please Select Patient First!';
                return false;
            }
            else {
                filterExpression.text = "RegistrationID = " + registrationID;
                return true;
            }
        }

        $(function () {
            $('#<%=btnMPEntryPopupSave.ClientID %>').click(function () {
                var errMessage = { text: "" };
                var filterExpression = { text: "" };
                alert('Process');
            });

            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            $('#<%=txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtRegistrationDate.ClientID %>').change(function (evt) {
                onRefreshGridView();
            });

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

        //        function onRefreshGrid() {
        //            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
        //            if (IsValid(null, 'fsPatientList', 'mpPatientList'))
        //                refreshGrdRegisteredPatient();
        //        }

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
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnHealthCareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <table class="tblContentArea" style="width: 100%">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <fieldset id="fsPatientList">
                    <table>
                        <colgroup>
                            <col style="width: 150px">
                        </colgroup>
                        <tr>
                            <td>
                                <%=GetLabel("Tanggal")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" />
                            </td>
                        </tr>
                        <tr id="trServiceUnit" runat="server">
                            <td>
                                <label>
                                    <%=GetLabel("Unit Pelayanan") %></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="200px"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
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
                                        <qis:QISIntellisenseHint Text="Unit Pelayanan" FieldName="ServiceUnitName" />
                                        <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
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
                    <hr />
                </fieldset>
                <uc1:ctlGrdRegisteredPatient runat="server" id="grdRegisteredPatient" />
            </td>
        </tr>
    </table>
</asp:Content>
