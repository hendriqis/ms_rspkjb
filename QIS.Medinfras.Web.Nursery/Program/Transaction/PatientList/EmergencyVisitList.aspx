<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true" 
    CodeBehind="EmergencyVisitList.aspx.cs" Inherits="QIS.Medinfras.Web.Nursing.Program.EmergencyVisitList" %>

<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientVisitCtl.ascx" TagName="ctlGrdRegisteredPatient" TagPrefix="uc1" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            $('#<%=txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtRegistrationDate.ClientID %>').change(function () {
                onRefreshGridView();
            });

            $('#lblRefresh.lblLink').click(function () {
                onRefreshGridView();
            });
        });

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setTimeout(function () {
            onRefreshGridView();
        }, interval);

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalID);
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                refreshGrdRegisteredPatient();
                intervalID = window.setTimeout(function () {
                    onRefreshGridView();
                }, interval);
            }
        }

        $(function () {
            $('#<%=txtBarcodeEntry.ClientID %>').keypress(function (e) {
                var keyCode = e.keyCode || e.which;

                if (keyCode == 9 || keyCode == 13)
                    cbpBarcodeEntryProcess.PerformCallback();
            });
        });

        function onCbpBarcodeEntryProcessEndCallback(s) {
            if (s.cpUrl != '')
                document.location = s.cpUrl;
            else {
                showToast('Warning', 'No RM Tidak Ditemukan', function () {
                    $('#<%=txtBarcodeEntry.ClientID %>').val('');
                });
                hideLoadingPanel();
            }
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

        $('#<%=chkIsPreviousEpisodePatient.ClientID %>').die();
        $('#<%=chkIsPreviousEpisodePatient.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtRegistrationDate.ClientID %>').attr('readonly', 'readonly');
            }
            else $('#<%=txtRegistrationDate.ClientID %>').removeAttr('readonly');
            onRefreshGridView();
        });
    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnQuickText" runat="server" />
    <div style="padding:15px">
        <div class="pageTitle"><%=GetMenuCaption()%> : <%=GetLabel("Pilih Pasien")%></div>
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
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal Registrasi")%></label></td>
                                <td>
                                    <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" />
                                    <asp:CheckBox ID="chkIsPreviousEpisodePatient" runat="server" Checked="false" Text="Abaikan Tanggal" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label><%=GetLabel("Quick Filter")%></label></td>
                                <td>
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                        Width="300px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No. Rekam Medis")%></label></td>
                                <td><asp:TextBox ID="txtBarcodeEntry" Width="120px" runat="server" /></td>
                            </tr>
                        </table>
                    </fieldset>
                    <div style="padding:7px 0 0 3px;font-size:0.95em">
                        <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefresh">[refresh]</span> <%=GetLabel("setiap")%> <%=GetRefreshGridInterval() %> <%=GetLabel("menit")%>
                    </div>

                    <uc1:ctlGrdRegisteredPatient runat="server" id="grdRegisteredPatient" />
                </td>
            </tr>
        </table>
    </div>

    <div style="display: none">
        <dxcp:ASPxCallbackPanel ID="cbpBarcodeEntryProcess" runat="server" Width="100%" ClientInstanceName="cbpBarcodeEntryProcess"
            ShowLoadingPanel="false" OnCallback="cbpBarcodeEntryProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" 
                EndCallback="function(s,e){ onCbpBarcodeEntryProcessEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
    <script type="text/javascript">
        $(function () {
            txtSearchView.SetText($('#<%=hdnQuickText.ClientID %>').val());
        });
    </script>
</asp:Content>
