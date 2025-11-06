<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true" 
    CodeBehind="RegisteredPatientList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.RegisteredPatientList" %>

<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientVisitCtl.ascx" TagName="ctlGrdRegisteredPatient" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            $('#<%=txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtRegistrationNo.ClientID %>').change(function (evt) {
                if (IsValid(evt, 'fsPatientList', 'mpPatientList'))
                    onRefreshGridView();
            });

            $('#<%=txtRegistrationDate.ClientID %>').change(function (evt) {
                if (IsValid(evt, 'fsPatientList', 'mpPatientList'))
                    onRefreshGridView();
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                if (IsValid(evt, 'fsPatientList', 'mpPatientList'))
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
                refreshGrdRegisteredPatient();
                intervalID = window.setInterval(function () {
                    onRefreshGridView();
                }, interval);
            }
        }

        function onCboServiceUnitChanged(s) {
            onRefreshGridView();
        }

        //#region Physician
        $('#lblPhysician.lblLink').live('click', function () {
            var polyclinicID = cboServiceUnit.GetValue();
            var filterExpression = '';
            if (polyclinicID != '')
                filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + polyclinicID + ")";
            openSearchDialog('paramedic', filterExpression, function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtPhysicianCodeChanged(value);
            });
        });

        $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPhysicianCodeChanged($(this).val());
        });

        function onTxtPhysicianCodeChanged(value) {
            var filterExpression = "ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%=hdnPhysicianID.ClientID %>').val('');
                    $('#<%=txtPhysicianCode.ClientID %>').val('');
                    $('#<%=txtPhysicianName.ClientID %>').val('');
                }
            });
            onRefreshGridView();
        }
        //#endregion

        function onCboVisitStatusValueChanged() {
            onRefreshGridView();
        }
        

    </script>
    <div style="padding:15px">
        <div class="pageTitle"><%=GetLabel("Previous Patient List")%></div>
        <table class="tblContentArea" style="width:100%">
            <tr>
                <td style="padding:5px;vertical-align:top">
                    <fieldset id="fsPatientList">  
                        <table class="tblEntryContent" style="width:60%;">
                            <colgroup>
                                <col style="width:25%"/>
                                <col/>
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Asal Pasien")%></label></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboPatientFrom" ClientInstanceName="cboPatientFrom" runat="server" Width="100%">
<%--                                        <ClientSideEvents ValueChanged="function(s,e) { onCboPatientFromValueChanged(); }"/>--%>
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Unit Pelayanan")%></label></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" runat="server" Width="100%">
                                        <ClientSideEvents ValueChanged="function(s,e){ onCboServiceUnitChanged(s); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Registration Date")%></label></td>
                                <td><asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblLink" id="lblPhysician"><%=GetLabel("Physician")%></label></td>
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
                                            <td><asp:TextBox ID="txtPhysicianName" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Registration No")%></label></td>
                                <td>
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:30%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtRegistrationNo" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tampilan Hasil") %></label></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboVisitStatus" ClientInstanceName="cboVisitStatus" Width="150px" runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboVisitStatusValueChanged(); }" />   
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <div style="padding:7px 0 0 3px;font-size:0.95em">
                        <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefresh">[refresh]</span> <%=GetLabel("Setiap")%><%=GetRefreshGridInterval() %> <%=GetLabel("Menit")%>
                    </div>

                    <uc1:ctlGrdRegisteredPatient runat="server" id="grdRegisteredPatient" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
