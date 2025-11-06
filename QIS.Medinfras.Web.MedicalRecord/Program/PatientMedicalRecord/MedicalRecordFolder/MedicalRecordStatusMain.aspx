<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master" AutoEventWireup="true" 
CodeBehind="MedicalRecordStatusMain.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.MedicalRecordStatusMain" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">

    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            $('#<%=txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtRegistrationDate.ClientID %>').change(function (evt) {
                if (IsValid(evt, 'fsPatientList', 'mpPatientList'))
                    refreshGrdRegisteredPatient();
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                if (IsValid(evt, 'fsPatientList', 'mpPatientList'))
                    refreshGrdRegisteredPatient();
            });

        //#region Service Unit
        function getServiceUnitExpression() {
            var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + cboDepartment.GetValue() + "' AND IsDeleted = 0";
            return filterExpression;
        }

        $('#lblServiceUnit.lblLink').click(function () {
            openSearchDialog('serviceunitperhealthcare', getServiceUnitExpression(), function (value) {
                $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                onTxtServiceUnitCodeChanged(value);
            });
        });

        $('#<%=txtServiceUnitCode.ClientID %>').change(function () {
            onTxtServiceUnitCodeChanged($(this).val());
        });

        function onTxtServiceUnitCodeChanged(value) {
            var filterExpression = "ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                }
                else {
                    $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                    $('#<%=txtServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtServiceUnitName.ClientID %>').val('');
                }
                cbpView.PerformCallback('refresh');
            });
        }
        //#endregion

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion

        function onCboDepartmentChanged() {
            $('#<%=txtServiceUnitCode.ClientID %>').val('');
            $('#<%=txtServiceUnitName.ClientID %>').val('');
            cbpView.PerformCallback('refresh');
        }

    </script>


    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Medical Record Form")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:30%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Rencana Realisasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Asal Pasien")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboDepartment" Width="300px" runat="server" /></td>
                    </tr>                    
                    <tr>
                        <td class="tdLabel"><label class="lblNormal" id="lblServiceUnit"><%=GetLabel("Unit Pelayanan")%></label></td>
                        <td>
                            <input type="hidden" value="" runat="server" id="hdnHealthcareServiceUnitID" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtServiceUnitCode" Width="200px" runat="server" /></td>
                                    <td><asp:TextBox ID="txtServiceUnitName" Width="300px" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                    <%=GetLabel("This Page Will")%>
                    <span class="lblLink" id="lblRefresh">[refresh]</span>
                    <%=GetLabel("Every 10 Minutes")%>
                </div>
            </td>
        </tr>
        <tr>
            <td>                    
                <input type="hidden" value="" id="Hidden1" runat="server" />
                <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
                <div style="position: relative;">
                    <dxcp:aspxcallbackpanel id="cbpView" runat="server" width="100%" clientinstancename="cbpView"
                        showloadingpanel="false" oncallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="RegistrationID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="RegistrationNo" HeaderText="Nomor Registrasi"/>
                                            <asp:BoundField DataField="MedicalNo" HeaderText="Nomor Medis"/>
                                            <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="BusinessPartnerName" HeaderText="Penjamin"  />
                                            <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" />                                            
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                </dxcp:aspxcallbackpanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging">
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>



    </table>
</asp:Content>
