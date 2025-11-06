<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true" 
    CodeBehind="PatientBillDiscountEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillDiscountEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl" TagPrefix="uc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnGenerateBillBack" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div><%=GetLabel("Back")%></div></li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">   
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />  
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />  
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript">
        $(function () {
            $('#<%=btnGenerateBillBack.ClientID %>').click(function () {
                document.location = ResolveUrl('~/Program/PatientList/RegistrationList.aspx?id=pbd');
            });
        });

        $('.lnkTransactionNo').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillDiscount/PatientBillDiscountDtCtl.ascx");
            openUserControlPopup(url, id, 'Bill Pasien', 1100, 500);
        });

        $('.lnkChangeDiscount').live('click', function () {
            var id = $(this).closest('tr').find('.hdnKeyField').val();
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillDiscount/PatientBillDiscountEntryCtl.ascx");
            openUserControlPopup(url, id, 'Ubah Bill Pasien', 900, 500);
        });

        function onAfterSaveEditRecordEntryPopup(param) {
            cbpView.PerformCallback();
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }
    </script> 
    
    <input type="hidden" value="" id="hdnTotalPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalPayerAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalAmount" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />  
    <div style="height:435px;overflow-y:auto;">
        <div class="pageTitle">
            <div style="font-size: 1.1em"><%=GetLabel("Pemberian Diskon Pasien")%></div>
        </div>
        <table class="tblContentArea" style="width:100%">
            <tr>
                <td>
                    <div style="padding: 5px;min-height: 150px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                        <asp:ListView ID="lvwView" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0" rules="all" >
                                                    <tr>  
                                                        <th rowspan="2" align="left">
                                                            <div style="padding:3px;float:left;">
                                                                <div><%= GetLabel("No Bill")%></div>
                                                                <div><%= GetLabel("Tanggal")%></div>                                                    
                                                            </div>
                                                            <div style="padding:3px;margin-left: 200px;">
                                                                <div><%= GetLabel("Dibuat Oleh")%></div>                                                      
                                                            </div>
                                                        </th>
                                                        <th colspan="3"><%=GetLabel("Jumlah")%></th>
                                                        <th rowspan="2" align="left" style="width: 120px">
                                                            &nbsp;
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width:150px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:150px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:150px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="5">
                                                            <%=GetLabel("No Data To Display") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>                                
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0" rules="all" >
                                                    <tr>  
                                                        <th rowspan="2" align="left" style="width: 230px">
                                                            <div style="padding:3px;float:left;">
                                                                <div><%= GetLabel("No Bill")%></div>
                                                                <div><%= GetLabel("Tanggal")%></div>                                                 
                                                            </div>
                                                            <div style="padding:3px;margin-left: 150px;">
                                                                <div><%= GetLabel("Dibuat Oleh")%></div>                                                      
                                                            </div>
                                                        </th>
                                                        <th style="width:150px" colspan="3">
                                                            <div style="text-align:center;padding-right:3px">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:150px" colspan="3">
                                                            <div style="text-align:center;padding-right:3px">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:150px" colspan="3">
                                                            <div style="text-align:center;padding-right:3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding-right:3px">
                                                                <%=GetLabel("Alasan Diskon")%>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left" style="width: 100px">
                                                            &nbsp;
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width:90px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Tagihan")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:90px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Diskon")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:90px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:90px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Tagihan")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:90px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Diskon")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:90px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:90px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Tagihan")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:90px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Diskon")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:90px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder" ></tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <div style="padding:3px;float:left;">
                                                            <input type="hidden" class="hdnKeyField" value="<%#: Eval("PatientBillingID")%>" />
                                                            <a class="lnkTransactionNo"><%#: Eval("PatientBillingNo")%></a>
                                                            <div><%#: Eval("BillingDateInString")%></div>                                                    
                                                        </div>
                                                        <div style="padding:3px;margin-left: 150px;">
                                                            <div><%#: Eval("LastUpdatedByUserName")%></div>                                                    
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding:3px;text-align:right;">
                                                            <div><%#: Eval("TotalPayerAmount", "{0:N}")%></div>                                                   
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding:3px;text-align:right;">
                                                            <div><%#: Eval("PayerDiscountAmount", "{0:N}")%></div>                                                   
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding:3px;text-align:right;"> 
                                                            <div><%#: Eval("TotalPayer", "{0:N}")%></div>                                                   
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding:3px;text-align:right;"> 
                                                            <div><%#: Eval("TotalPatientAmount", "{0:N}")%></div>                                                   
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding:3px;text-align:right;">
                                                            <div><%#: Eval("PatientDiscountAmount", "{0:N}")%></div>                                                   
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding:3px;text-align:right;"> 
                                                            <div><%#: Eval("TotalPatient", "{0:N}")%></div>                                                   
                                                        </div>                                                        
                                                    </td>
                                                    <td>
                                                        <div style="padding:3px;text-align:right;">
                                                            <div><%#: Eval("TotalAmount", "{0:N}")%></div>                                                   
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding:3px;text-align:right;">
                                                            <div><%#: Eval("DiscountAmount", "{0:N}")%></div>                                                   
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding:3px;text-align:right;">
                                                            <div><%#: Eval("Total", "{0:N}")%></div>                                                   
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding:3px">
                                                            <div><%#: Eval("DiscountReason")%></div>     
                                                        </div>
                                                    </td>
                                                    <td align="center">
                                                        <div style="padding:3px">
                                                            <a class="lnkChangeDiscount"><%=GetLabel("Ubah Diskon") %></a>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingService">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>