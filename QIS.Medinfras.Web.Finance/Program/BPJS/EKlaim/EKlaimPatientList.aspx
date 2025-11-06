<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="EKlaimPatientList.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.EKlaimPatientList" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnUpdatePatient" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbedit.png") %>' alt="" />
        <div>
            <%=GetLabel("Ubah Data Pasien") %></div>
    </li>
    <li id="btnDeletePatient" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdelete.png") %>' alt="" />
        <div>
            <%=GetLabel("Hapus Data Pasien") %></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

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
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion
        

        function GetPatientData(MRN) {
            var filterExpression = "MRN = " + MRN;
            Methods.getObject('GetvPatientBPJSList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnMedicalNo.ClientID %>').val(result.MedicalNo);
                    $('#<%=hdnNHSRegistrationNo.ClientID %>').val(result.NHSRegistrationNo);
                    $('#<%=hdnPatientName.ClientID %>').val(result.PatientName);
                    $('#<%=hdnDateOfBirth.ClientID %>').val(result.cfDateOfBirthInDateFormat4);
                    $('#<%=hdnGender.ClientID %>').val(result.GCGender);
                }
                else {
                    $('#<%=hdnMedicalNo.ClientID %>').val("");
                    $('#<%=hdnNHSRegistrationNo.ClientID %>').val("");
                    $('#<%=hdnPatientName.ClientID %>').val("");
                    $('#<%=hdnDateOfBirth.ClientID %>').val("");
                    $('#<%=hdnGender.ClientID %>').val("");
                }
            });
        }

        //#region Update Patient
        $('#<%=btnUpdatePatient.ClientID %>').live('click', function (evt) {
            var isBridgingToEKlaim = $('#<%=hdnIsBridgingToEKlaim.ClientID %>').val();
            if (isBridgingToEKlaim == "1") {
                UpdatePatient();
            } else {
                showToast('INFORMATION', 'Status bridging dengan e-klaim sedang nonaktif.');
            }
        });

        function UpdatePatient() {
            var mrn = $('#<%:hdnID.ClientID %>').val();
            GetPatientData(mrn);

            var nomor_kartu = $('#<%:hdnNHSRegistrationNo.ClientID %>').val();
            var nomor_rm = $('#<%:hdnMedicalNo.ClientID %>').val();
            var nama_pasien = $('#<%:hdnPatientName.ClientID %>').val();

            var paramDOB = $('#<%:hdnDateOfBirth.ClientID %>').val();

            var tgl_lahir = paramDOB + " 00:00:00";
            //////// paramDOB.substring(10, 6) + "-" + paramDOB.substring(5, 3) + "-" + paramDOB.substring(2, 0) + " 00:00:00";

            var paramGender = $('#<%:hdnGender.ClientID %>').val();
            var gender = "";

            if (paramGender == "0003^M") {
                gender = "1";
            } else {
                gender = "2";
            }

            EKlaimService.updatePatient(nomor_kartu, nomor_rm, nama_pasien, tgl_lahir, gender, function (result) {
                if (result != null) {
                    showToast('INFORMATION', result);
                }
            });

        }
        //#endregion

        //#region Delete Patient
        $('#<%=btnDeletePatient.ClientID %>').live('click', function (evt) {
            var isBridgingToEKlaim = $('#<%=hdnIsBridgingToEKlaim.ClientID %>').val();
            if (isBridgingToEKlaim == "1") {
                DeletePatient();
            } else {
                showToast('INFORMATION', 'Status bridging dengan e-klaim sedang nonaktif.');
            }
        });

        function DeletePatient() {
            var mrn = $('#<%:hdnID.ClientID %>').val();
            GetPatientData(mrn);

            var nomor_rm = $('#<%:hdnMedicalNo.ClientID %>').val();
            var coder_nik = $('#<%:hdnUsernameLogin.ClientID %>').val();

            EKlaimService.deletePatient(nomor_rm, coder_nik, function (result) {
                if (result != null) {
                    showToast('INFORMATION', result);
                }
            });

        }
        //#endregion

    </script>
    <input type="hidden" id="hdnIsBridgingToEKlaim" value="" runat="server" />
    <input type="hidden" id="hdnUsernameLogin" value="" runat="server" />
    <input type="hidden" id="hdnMedicalNo" value="" runat="server" />
    <input type="hidden" id="hdnNHSRegistrationNo" value="" runat="server" />
    <input type="hidden" id="hdnPatientName" value="" runat="server" />
    <input type="hidden" id="hdnDateOfBirth" value="" runat="server" />
    <input type="hidden" id="hdnGender" value="" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="MRN" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="MedicalNo" HeaderText="Medical No" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="PatientName" HeaderText="Patient Name" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="cfDateOfBirthInDateFormat" HeaderText="Date of Birth" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Gender" HeaderText="Gender" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="cfHomeAddress" HeaderText="Address" HeaderStyle-Width="400px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No data to display.")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>    
        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging"></div>
            </div>
        </div> 
    </div>
</asp:Content>