<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true" CodeBehind="TransactionVerificationDetail.aspx.cs"
    Inherits="QIS.Medinfras.Web.Inpatient.Program.TransactionVerificationDetail" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Program/TransactionVerification/TransactionVerificationServiceCtl.ascx" TagName="ServiceCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Program/TransactionVerification/TransactionVerificationDrugLogisticCtl.ascx" TagName="DrugLogisticCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl" TagPrefix="uc1" %>

<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbarLeft" runat="server">
    <li id="btnTransactionVerificationBack" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div><%=GetLabel("Back")%></div></li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div><%=GetLabel("Process")%></div></li>
    <li id="btnDecline" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div><%=GetLabel("Decline")%></div></li>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
     <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">   
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />        
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />        
    <input type="hidden" value="" id="hdnKdGudang" runat="server" />
    <input type="hidden" value="" id="hdnDefaultLocation" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianID" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianCode" runat="server" />  
    <input type="hidden" value="" id="hdnPhysicianName" runat="server" />  
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnIsHealthcareServiceUnitHasParamedic" runat="server" /> 
    <input type="hidden" value="" id="hdnPageTitle" runat="server" /> 

    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript">
        function getPhysicianID() {
            return $('#<%=hdnPhysicianID.ClientID %>').val();
        }
        function getBusinessPartnerID() {
            return $('#<%=hdnBusinessPartnerID.ClientID %>').val();
        }
        function getClassID() {
            return $('#<%=hdnClassID.ClientID %>').val();
        }
        function getRegistrationID() {
            return $('#<%=hdnRegistrationID.ClientID %>').val();
        }
        function getVisitID() {
            return $('#<%=hdnVisitID.ClientID %>').val();
        }

        function onLoad() {

            $('#ulTabClinicTransaction li').click(function () {
                $('#ulTabClinicTransaction li.selected').removeAttr('class');
                $('.containerTransDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });

            $('#<%=btnTransactionVerificationBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl('~/Program/PatientList/VisitList.aspx?id=vt');
            });

            $('#<%=btnProcess.ClientID %>').click(function () {
                getCheckedMember();
                if ($('#<%=txtTransactionNo.ClientID %>').val() == '')
                    showToast('Proses Gagal', 'Pilih No Bukti Terlebih Dahulu');
                else
                    onCustomButtonClick('process');
            });

            $('#<%=btnDecline.ClientID %>').click(function () {
                if ($('#<%=txtTransactionNo.ClientID %>').val() == '')
                    showToast('Proses Gagal', 'Pilih No Bukti Terlebih Dahulu');
                else
                    onCustomButtonClick('decline');
            });

            //#region Transaction No 
            $('#lblTransactionNo.lblLink').live('click', function () {
                var filterExpression = "<%:OnGetTransactionNoFilterExpression() %>";
                openSearchDialog('patientchargeshd',filterExpression , function (value) {
                    $('#<%=txtTransactionNo.ClientID %>').val(value);
                    onTxtTransactionNoChanged(value);
                });
            });

            $('#<%=txtTransactionNo.ClientID %>').change(function () {
                onTxtTransactionNoChanged($(this).val());
            });

            function onTxtTransactionNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion
        }

        function getCheckedMember() {
            var lstSelectedMember = [];
            var lstUnselectedMember = [];

            var result = '';
            $('.grdService .chkIsSelected input').each(function () {
                var isChecked = $(this).is(":checked");
                var key = $(this).closest('tr').find('.keyField').val();

                if (isChecked)
                    lstSelectedMember.push(key);
                else
                    lstUnselectedMember.push(key);
            });
            $('.grdDrugMS .chkIsSelectedDrugLogistic input').each(function () {
                var isChecked = $(this).is(":checked");
                var key = $(this).closest('tr').find('.keyFieldDrugLogistic').val();

                if (isChecked)
                    lstSelectedMember.push(key);
                else
                    lstUnselectedMember.push(key);
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
            $('#<%=hdnUnselectedMember.ClientID %>').val(lstUnselectedMember.join(','));
        }

        function onAfterCustomClickSuccess(type, retval) {
            if (type == 'process')
                cbpView.PerformCallback();
            else if (type == 'decline')
                $('#<%=btnTransactionVerificationBack.ClientID %>').click();
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            onLoad();
        }

        function onCboDisplayChanged() {
            $('#<%=hdnCboDisplayValue.ClientID %>').val(cboDisplay.GetValue());
            cbpView.PerformCallback();
        }
    </script> 
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnUnselectedMember" runat="server" value="" />
    <input type="hidden" id="hdnSelectedCbo" runat="server" value="" />
    <input type="hidden" id="hdnCboDisplayValue" runat="server" value="" />
    
    <div style="height:435px;overflow-y:auto;">
        <div class="pageTitle">
            <div style="font-size: 1.1em"><%=GetLabel("Verifikasi Transaksi Perawatan")%></div>
        </div>
 
        <table class="tblContentArea">
            <colgroup>
                <col style="width:50%"/>
                <col style="width:50%"/>
            </colgroup>
            <tr>
                <td style="padding:5px;vertical-align:top">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:30%"/>
                            <col/>
                        </colgroup>
                        <tr>
                            <td style="display:none"><input type="hidden" id="hdnTransactionHdID" runat="server" value="" /></td>
                            <td class="tdLabel"><label class="lblLink" id="lblTransactionNo"><%=GetLabel("No Bukti")%></label></td>
                            <td><asp:TextBox ID="txtTransactionNo" Width="250px" runat="server" ReadOnly="true" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tampilan")%></label></td>
                            <td>
                                <dxe:ASPxComboBox ID="cboDisplay" ClientInstanceName="cboDisplay" Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboDisplayChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;height:365px;overflow-y:scroll;">
                         <table class="tblContentArea">  
                            <tr>
                                <td colspan="2">
                                    <div class="containerUlTabPage">
                                        <ul class="ulTabPage" id="ulTabClinicTransaction">
                                            <li class="selected" contentid="containerService"><%=GetLabel("Pelayanan") %></li>
                                            <li contentid="containerDrug"><%=GetLabel("Obat & Alkes") %></li>
                                            <li contentid="containerLogistic"><%=GetLabel("Barang Umum") %></li>
                                            <li contentid="containerLaboratory"><%=GetLabel("Laboratorium") %></li>
                                            <li contentid="containerImaging"><%=GetLabel("Radiologi") %></li>
                                            <li contentid="containerMedical"><%=GetLabel("Penunjang Medis") %></li>
                                        </ul>
                                    </div>
                                    <div id="containerService" class="containerTransDt">
                                        <uc1:ServiceCtl ID="ctlService" runat="server" />
                                    </div>   
                                    <div id="containerDrug" style="display:none" class="containerTransDt">
                                        <uc1:DrugLogisticCtl ID="ctlDrug" runat="server" />
                                    </div> 
                                    <div id="containerLogistic" style="display:none" class="containerTransDt">
                                        <uc1:DrugLogisticCtl ID="ctlLogistic" runat="server" />
                                    </div>
                                    <div id="containerLaboratory" style="display:none" class="containerTransDt">
                                        <uc1:ServiceCtl ID="ctlLaboratory" runat="server" />
                                    </div> 
                                    <div id="containerImaging" style="display:none" class="containerTransDt">
                                        <uc1:ServiceCtl ID="ctlImaging" runat="server" />
                                    </div> 
                                    <div id="containerMedical" style="display:none" class="containerTransDt">
                                        <uc1:ServiceCtl ID="ctlMedical" runat="server" />
                                    </div>  
                                 </td>
                            </tr>
                        </table>
                        <table style="width:100%" cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col style="width:15%"/>
                                <col style="width:35%"/>
                                <col style="width:15%"/>
                                <col style="width:35%"/>
                            </colgroup>
                            <tr>
                                <td><div class="lblComponent" style="text-align: left; padding-left: 5px;"><%=GetLabel("Total Instansi") %>  : </div></td>
                                <td style="text-align:right;padding-right: 10px;">
                                    <input type="hidden" value="" id="hdnTotalPayer" runat="server" />
                                    Rp. <asp:TextBox ID="txtTotalPayer" ReadOnly="true" CssClass="number" runat="server" Width="200px" />
                                </td>
                                <td><div class="lblComponent" style="text-align: left; padding-left: 5px;"><%=GetLabel("Total Total Pasien") %>  : </div></td>
                                <td style="text-align:right;padding-right: 10px;">
                                    <input type="hidden" value="" id="hdnTotalPatient" runat="server" />
                                    Rp. <asp:TextBox ID="txtTotalPatient" ReadOnly="true" CssClass="number" runat="server" Width="200px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
    </div>
</asp:Content>