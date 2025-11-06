<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrxDashboard.master" AutoEventWireup="true"
    CodeBehind="PatientList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PatientList" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
<input type="hidden" value="0" id="hdnHealthcareServiceUnitID" runat="server" />
<input type="hidden" value="0" id="hdnVisitID" runat="server" />
<input type="hidden" value="0" id="hdnPatientPageByDepartment" runat="server" />
 
 <div class="container-fluid" style="margin-top:10px">
    
    <div class="row">
        <div class="col-md-12">
        <div class="row stat-cards">
                 
                <div class="col-md-6 col-xl-3">
                    <article class="stat-cards-item">
                        <div class="stat-cards-icon primary">
                            <img src="../../libs/Images/Dashboard/clinic.png" id="ctl00_ctl00_ctl00_plhMPBase_plhMPMain_cbpMPEntryContent_plhEntry_ctlMasterRS_imgClinic" class="img-circle" title="Clinic" alt="Clinic" style="width:100% ; height: 100%;">
                        </div>
                        <div class="stat-cards-info">
                            <p class="stat-cards-info__title2">Pasien Rawat Jalan Hari ini</p>
                            <p class="stat-cards-info__num2">
                                <label id="ctl00_ctl00_ctl00_plhMPBase_plhMPMain_cbpMPEntryContent_plhEntry_ctlMasterRS_lblClinicCount" class="lblClinicCount">28</label>
                            </p>
                        </div>
                    </article>
                </div>
                <div class="col-md-6 col-xl-3">
                    <article class="stat-cards-item">
                        <div class="stat-cards-icon primary">
                            <img src="../../libs/Images/Dashboard/bed.png" id="ctl00_ctl00_ctl00_plhMPBase_plhMPMain_cbpMPEntryContent_plhEntry_ctlMasterRS_imgBed" class="img-circle" title="Bed" alt="Bed" style="width:100% ; height: 100%;">
                        </div>
                        <div class="stat-cards-info">
                            <p class="stat-cards-info__title2">Pasien Dirawat Hari ini</p>
                            <p class="stat-cards-info__num2">
                                <label id="ctl00_ctl00_ctl00_plhMPBase_plhMPMain_cbpMPEntryContent_plhEntry_ctlMasterRS_lblBedCount" class="lblBedCount">100</label>
                            </p>
                        </div>
                    </article>
                </div>
            </div>
        </div>
    </div>
    <br />
    <div class="row">
     <div class="col-sm-4">
       <div class="card">
          <div class="card-header">
            <h6 class="text-white">Informasi</h6></div>
            <div class="card-body">
            <div class="form-group">
              <label>Asal Pasien</label>
                <dxe:ASPxComboBox ID="cboPatientFrom" ClientInstanceName="cboPatientFrom" runat="server" Width="100%" CssClass="form-control">
                                            <ClientSideEvents ValueChanged="function(s,e) { onCboPatientFromValueChanged(e); }"/>
                                        </dxe:ASPxComboBox>
              </div>
               <div class="form-group" id="DateRegistration" style="display:none;">
               <label>Tanggal</label>
                     <asp:TextBox ID="txtRealisationDate" Width="120px" runat="server" CssClass="datepicker form-control"/>
                     </div>
                                    <hr />
                <h5>Unit Pelayanan</h5>
                 <div class="overflow-auto" style="height:320px;">
                  <dxcp:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%" ClientInstanceName="cbpViewServiceUnit"
                    ShowLoadingPanel="false" OnCallback="cbpViewServiceUnit_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewServiceUnitEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel2" >
                                <asp:ListView runat="server" ID="lvwView"  OnItemDataBound="lvwView_ItemDataBound">
                                <LayoutTemplate>   
                                <table id="Table1" runat="server" class="table grdCollapsible lvwView" >   
                                    <tr id="Tr1" runat="server" class="TableHeader">   
                                        <td id="Td1" runat="server"></td>  
                                    </tr>   
                                    <tr id="ItemPlaceholder" runat="server">   
                                    </tr>   
                                     
                                </table>   
                            </LayoutTemplate>    
                            <ItemTemplate>   
                                <tr class="TableData">   
                                    <td>   
                                        <%#: Eval("ServiceUnitName")%>
                                         <input type="hidden" class="hdnHealthcareServiceUnitID" value='<%#: Eval("HealthcareServiceUnitID") %>' />               
                                        <div class="pull-right"> <button type="button" class="btn btn-primary">
                                             <span class="badge badge-light">    
                                             <%#: Eval("NoOfPatients")%></span>
                                            </button>
                                        </div>
                                    </td>   
                                </tr>                   
                            </ItemTemplate>   
                           </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                </div>
            </div>
          </div>
        </div>   
      <div class="col-sm-8">
        <div class="card">
            <div class="card-header"><h6 class="text-white">Daftar Pasien</h6></div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-12">
                    <div class="overflow-auto" style="height:500px;">
                     <dxcp:ASPxCallbackPanel ID="ASPxCallbackPanel2" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                    ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpViewDtEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContentDt" runat="server">
                            <asp:Panel runat="server" ID="PanelDt" >
                               <asp:ListView runat="server" ID="lvwViewDt">
                                <LayoutTemplate>   
                                <table id="Table2" runat="server" class="table   lvwViewDt">   
                               
                                    <tr   class="TableHeader">   
                                        <td >SESI</td>  
                                        <td >Antrian</td>  
                                        <td >Pasien</td>  
                                        <td >Informasi Kunjungan</td>  
                                        <td >Alamat dan Kontak Pasien</td>  
                                    </tr> 
                                      
                                    <tr id="ItemPlaceholder" runat="server">   
                                    </tr> 
                                    
                                </table>   
                            </LayoutTemplate>    
                            <ItemTemplate>   
                            
                                <tr>   
                                    <td>   
                                          
                                         <div <%# Eval("DepartmentID").ToString() != "INPATIENT" ? "" : "style='display:none'" %>>
                                                                <span style="font-weight: bold; font-size:12pt"><%#: Eval("Session") %></span>  
                                                            </div>
                                    </td>   
                                     <td>   
                                          <div <%# Eval("DepartmentID").ToString() != "OUTPATIENT" ? "Style='display:none'":"" %>>
                                                                <span style="font-weight: bold; font-size:12pt"><%#: Eval("QueueNo") %></span>  
                                                            </div>
                                                            <div <%# Eval("DepartmentID").ToString() == "OUTPATIENT" ? "Style='display:none'":"style='font-size: 13pt; font-weight: bold'" %>>
                                                                <%#: Eval("BedCode") %></div>
                                    </td>   
                                    <td>
                                     <table width="100%" cellpadding="0" cellspacing="0" border="0" >
                                                                <tr>
                                                                    <td align="center" valign="top" style="width:20px">
                                                                        <div <%# Eval("DepartmentID").ToString() != "OUTPATIENT" ? "Style='display:none'":"Style='display:none'" %>>
                                                                            <img id="imgPatientSatisfactionLevelImageUri" runat="server" width="24" height="24" alt="" visible="true" />    
                                                                        </div>
                                                                    </td> 
                                                                    <td>
                                                                    <div class="row">
                                                                        <div class="col-md-6">
                                                                             <img class="rounded float-left" src='<%#Eval("PatientImageUrl") %>' alt="" height="55px" width="40px" style="float:left;margin-right: 10px;" />
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <span style="font-weight: bold; font-size:11pt"><%#: Eval("cfPatientNameSalutation") %></span>
                                                                            <div><%#: Eval("MedicalNo") %>, <%#: Eval("DateOfBirthInString") %>, <%#: Eval("Sex") %></div>
                                                                              <div style="font-style:italic"><%#: Eval("BusinessPartner")%></div>
                                                                        </div>
                                                                    </div>
                                                                        <%--<div style="text-align:left">
                                                                        <img class="imgPatientImage" src='<%#Eval("PatientImageUrl") %>' alt="" height="55px" width="40px" style="float:left;margin-right: 10px;" /></div>
                                                                        <div><span style="font-weight: bold; font-size:11pt"><%#: Eval("cfPatientNameSalutation") %></span></div>
                                                                        <div><%#: Eval("MedicalNo") %>, <%#: Eval("DateOfBirthInString") %>, <%#: Eval("Sex") %></div>
                                                                        <div style="font-style:italic"><%#: Eval("BusinessPartner")%></div>
--%>                                                                    </td>
                                                                </tr>
                                                            </table>
                                    </td>
                                    <td>
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <%#: Eval("RegistrationNo") %>
                                                                        <div <%#:Eval("DepartmentID").ToString() != "INPATIENT" ? "style='display:none'":"" %>><%#: Eval("cfPatientLocation") %></div>
                                                                        <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />   
                                                                        <div><%#: Eval("ParamedicName")%></div>                                                                                                                
                                                                    </td>
                                                                </tr>
                                                            </table>
                                    </td>
                                     <td>
                                                            <div><%#: Eval("HomeAddress")%></div>
                                                            <div style="padding:3px">
                                                                <img src='<%= ResolveUrl("~/Libs/Images/homephone.png")%>' alt='' style="float:left;" /><div style="margin-left:30px"><%#: Eval("cfPhoneNo")%>&nbsp;</div>
                                                                <img src='<%= ResolveUrl("~/Libs/Images/mobilephone.png")%>' alt='' style="float:left;" /><div style="margin-left:30px"><%#: Eval("cfMobilePhoneNo")%>&nbsp;</div>     
                                                            </div>
                                                        </td>
                                </tr>     
                                          
                            </ItemTemplate>   
                           </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>   
                    </div>
                    </div>
                </div>
            </div>
            </div>
      </div>    
    </div>
</div>
  <div style="display:none"><asp:Button ID="btnOpenTransactionDt" runat="server" UseSubmitBehavior="false" OnClientClick="return onBeforeOpenTransactionDt();" OnClick="btnOpenTransactionDt_Click" /></div>
                           
<script type="text/javascript">
    $(function () {

        setDatePicker('<%=txtRealisationDate.ClientID %>');
        $('#<%=txtRealisationDate.ClientID %>').datepicker('option', 'maxDate', '0');

        $('#<%=txtRealisationDate.ClientID %>').change(function () {
            cbpViewDt.PerformCallback('refresh');
        });

        $('#DateRegistration').hide();

    });

    $('.lvwView > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
        showLoadingPanel();
        var hsu = $(this).find('.hdnHealthcareServiceUnitID').val();
        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(hsu);
        cbpViewDt.PerformCallback('refresh');
         
    });
    $('.lvwViewDt > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
        alert("OK");
        var hdnVisitID = $(this).find('.hdnVisitID').val();
        $('#<%=hdnVisitID.ClientID %>').val(hdnVisitID);
        __doPostBack('<%=btnOpenTransactionDt.UniqueID%>', '');
    });
    
    function oncbpViewDtEndCallback(s) {
        hideLoadingPanel();
    }
    function onCboPatientFromValueChanged(e) {
        if (cboPatientFrom.GetValue() == "INPATIENT" || cboPatientFrom.GetValue() == "EMERGENCY") {
            $('#DateRegistration').hide();
        } else {
            $('#DateRegistration').show();
        }

        cbpViewServiceUnit.PerformCallback('refresh');
        cbpViewDt.PerformCallback('refresh');
    }
    function onCbpViewServiceUnitEndCallback(s) {
        hideLoadingPanel();
     }
    function onCboDepartmentChanged() 
    {
        cbpView.PerformCallback('refresh');
        cbpViewDt.PerformCallback('refresh');
    }
     

        
</script>
</asp:Content>
