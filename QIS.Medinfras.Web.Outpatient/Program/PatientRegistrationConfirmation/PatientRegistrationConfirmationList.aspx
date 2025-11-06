<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true" 
    CodeBehind="PatientRegistrationConfirmationList.aspx.cs" Inherits="QIS.Medinfras.Web.Outpatient.Program.PatientRegistrationConfirmationList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            $('#<%=txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtRegistrationDate.ClientID %>').change(function (evt) {
                onRefreshGridView();
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });

            $('#btnConfirm').click(function () {
                getCheckedRegistration();
                if ($('#<%=hdnSelectedVisit.ClientID %>').val() == '')
                    showToast('Warning', '<%=GetErrMessageSelectRegistrationFirst() %>');
                else
                    cbpRegistrationConfirmation.PerformCallback();
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

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        function getCheckedRegistration() {
            var lstSelectedRegistration = '';
            var result = '';
            $('#<%=grdView.ClientID %> .chkPatient input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    if (lstSelectedRegistration != '')
                        lstSelectedRegistration += ',';
                    lstSelectedRegistration += key;
                }
            });
            $('#<%=hdnSelectedVisit.ClientID %>').val(lstSelectedRegistration);
        }

        $('#chkSelectAllPatient').die('change');
        $('#chkSelectAllPatient').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkPatient').each(function () {
                $(this).find('input').prop('checked', isChecked);
            });
        });

        function onCbpRegistrationConfirmationEndCallback(s) {
            var result = s.cpResult.split('|');
            if (result[0] == 'success') {
                onRefreshGridView();
            }
            else {
                if (result[1] != '')
                    showToast('Save Failed', 'Error Message : ' + result[1]);
                else
                    showToast('Save Failed', '');
            }
            hideLoadingPanel();
        }

        function refreshGrdRegisteredPatient() {
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
    <input type="hidden" id="hdnSelectedVisit" runat="server" value="" />
    <div style="padding:15px">
        <div class="pageTitle"><%=GetMenuCaption()%></div>
        <table class="tblContentArea" style="width:100%">
            <tr>
                <td style="padding:5px;vertical-align:top">
                    <fieldset id="fsBedStatus">  
                        <table class="tblEntryContent" style="width:60%;">
                            <colgroup>
                                <col style="width:25%"/>
                                <col/>
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal Registrasi")%></label></td>
                                <td><asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Klinik")%></label></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%" runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { cbpView.PerformCallback('refresh'); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblLink" id="lblPhysician"><%=GetLabel("Dokter / Paramedis")%></label></td>
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
                                <td class="tdLabel"><label><%=GetLabel("Quick Filter")%></label></td>
                                <td>
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                        Width="300px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                            <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>

                    <input type="button" id="btnConfirm" value='<%=GetLabel("Confirm") %>' />
                    <div style="padding:7px 0 0 3px;font-size:0.95em">
                        <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefresh">[refresh]</span> <%=GetLabel("Setiap")%> <%=GetRefreshGridInterval() %> <%=GetLabel("Menit")%>
                    </div>

                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;height:365px;overflow-y:scroll;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                             <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input id="chkSelectAllPatient" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkPatient" runat="server" CssClass="chkPatient" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="MedicalNo" HeaderText="No RM" HeaderStyle-Width="150px" />
                                            <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" />
                                            <asp:BoundField DataField="RegistrationNo" HeaderText="No Registrasi" HeaderStyle-Width="150px" />                              
                                            <asp:BoundField DataField="SpecialtyName" HeaderText="Spesialisasi" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="BusinessPartner" HeaderText="Pembayar" HeaderStyle-Width="100px" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
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
    <div style="display:none">
        <dxcp:ASPxCallbackPanel ID="cbpRegistrationConfirmation" runat="server" Width="100%" ClientInstanceName="cbpRegistrationConfirmation"
            ShowLoadingPanel="false" OnCallback="cbpRegistrationConfirmation_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpRegistrationConfirmationEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
