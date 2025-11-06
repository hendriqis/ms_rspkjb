<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChargesEditCtl.ascx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PatientPage.ChargesEditCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_diagnosisentryctl">
</script>

<div style="height: 410px; overflow-y: scroll; border: 0px">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" id="hdnIsAllowChangeChargesQty" runat="server" value="1" />
    <fieldset id="fsTrxService" style="margin:0"> 
        <table class="tblEntryDetail" style="width:100%">
            <colgroup>
                <col style="width:40%"/>
                <col style="width:33%"/>
                <col />
            </colgroup>
            <tr>
                <td valign="top">
                    <table>
                        <colgroup>
                            <col style="width:130px"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblLink lblMandatory" id="lblServiceItem"><%=GetLabel("Pelayanan")%></label></td>
                            <td>
                                <input type="hidden" value="" id="hdnServiceItemID" runat="server" />
                                <input type="hidden" value="" id="hdnServiceRevenueSharingID" runat="server" />
                                <input type="hidden" value="" id="hdnServiceItemUnit" runat="server" />
                                <input type="hidden" value="" id="hdnServiceBaseTariff" runat="server" />
                                <input type="hidden" value="" id="hdnServiceDiscountAmount" runat="server" />
                                <input type="hidden" value="" id="hdnServiceCoverageAmount" runat="server" />
                                <input type="hidden" value="" id="hdnServiceIsDicountInPercentage" runat="server" />
                                <input type="hidden" value="" id="hdnServiceIsCoverageInPercentage" runat="server" />
                                <input type="hidden" value="" id="hdnServiceBaseCITOAmount" runat="server" />
                                <input type="hidden" value="" id="hdnServiceIsCITOInPercentage" runat="server" />
                                <input type="hidden" value="" id="hdnServiceBaseComplicationAmount" runat="server" />
                                <input type="hidden" value="" id="hdnServiceIsComplicationInPercentage" runat="server" />
                                <input type="hidden" value="" id="hdnServiceItemFilterExpression" runat="server" />
                                <input type="hidden" value="" id="hdnServicePrice" runat="server" />
                                <input type="hidden" value="" id="hdnServicePriceComp1" runat="server" />
                                <input type="hidden" value="" id="hdnServicePriceComp2" runat="server" />
                                <input type="hidden" value="" id="hdnServicePriceComp3" runat="server" />
                                <input type="hidden" value="" id="hdnServiceBasePriceComp1" runat="server" />
                                <input type="hidden" value="" id="hdnServiceBasePriceComp2" runat="server" />
                                <input type="hidden" value="" id="hdnServiceBasePriceComp3" runat="server" />
                                <input type="hidden" value="" id="hdnServiceTransactionID" runat="server" />
                                <input type="hidden" value="" id="hdnServiceTransactionDate" runat="server" />
                                <input type="hidden" value="" id="hdnServiceTransactionTime" runat="server" />
                                <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
                                <input type="hidden" value="" id="hdnServiceVisitID" runat="server" />
                                <input type="hidden" value="" id="hdnServiceUnitName" runat="server" />

                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtServiceItemCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtServiceItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblLink lblMandatory" id="lblPhysician"><%=GetLabel("Dokter/Paramedis")%></label></td>
                            <td>
                                <input type="hidden" value="" id="hdnServicePhysicianID" runat="server" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtServicePhysicianCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtServicePhysicianName" ReadOnly="true" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblDisabled" id="lblTestPartner"><%=GetLabel("Test Partner")%></label></td>
                            <td>
                                <input type="hidden" value="" id="hdnTestPartnerID" runat="server" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtTestPartnerCode" Width="100%" runat="server" ReadOnly="true" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtTestPartnerName" ReadOnly="true" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label></label></td>
                            <td>
                                <input type="hidden" id="hdnDefaultTariffComp" runat="server" value="1" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:20px"/>
                                        <col style="width:100px"/>
                                        <col style="width:20px"/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:CheckBox ID="chkServiceIsVariable" runat="server" /></td>
                                        <td class="tdLabel"><label><%=GetLabel("Variable")%></label></td>
                                        <td><asp:CheckBox ID="chkServiceIsUnbilledItem" runat="server" /></td>
                                        <td class="tdLabel"><label><%=GetLabel("Tidak Ditagihkan")%></label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Kelas Tagihan")%></label></td>
                            <td>
                                <dxe:ASPxComboBox ID="cboServiceChargeClassID" ClientInstanceName="cboServiceChargeClassID" Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboServiceChargeClassIDValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td> 
                        </tr> 
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Harga Satuan")%></label></td>
                            <td><asp:TextBox ID="txtServiceUnitTariff" Width="100px" CssClass="txtCurrency" runat="server" />
                            <input type="button" id="btnEditUnitTariff" title='<%=GetLabel("Unit Tariff Component") %>' value="..." style="width:10%"  /></td>
                        </tr>                                        
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Jumlah")%></label></td>
                            <td><asp:TextBox ID="txtServiceQty" Width="100px" CssClass="number" runat="server" /></td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table>    
                        <colgroup>
                            <col style="width:100px"/>
                            <col style="width:20px"/>
                            <col />
                        </colgroup>   
                        
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Harga")%></label></td>
                            <td>&nbsp;</td>
                            <td><asp:TextBox ReadOnly="true" ID="txtServiceTariff" Width="200px" CssClass="txtCurrency" runat="server" /></td>
                        </tr>   
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("CITO")%></label></td>
                            <td><asp:CheckBox ID="chkServiceIsCITO" runat="server" /></td>
                            <td><asp:TextBox ID="txtServiceCITO" ReadOnly="true" Width="200px" CssClass="txtCurrency" runat="server" /></td>
                        </tr>
                        <tr style="display:none">
                            <td class="tdLabel"><label><%=GetLabel("Penyulit")%></label></td>
                            <td><asp:CheckBox ID="chkServiceIsComplication" runat="server" /></td>
                            <td><asp:TextBox ID="txtServiceComplication" ReadOnly="true" Width="200px" CssClass="txtCurrency" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Diskon")%></label></td>
                            <td><asp:CheckBox ID="chkServiceIsDiscount" runat="server" /></td>
                            <td><asp:TextBox ID="txtServiceDiscount" Width="200px" CssClass="txtCurrency" runat="server" />
                            <input type="button" class="btnEditDiscount" title='<%=GetLabel("Discount Component") %>' value="..." style="width:10%"  /></td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table>    
                        <colgroup>
                            <col style="width:100px"/>
                        </colgroup>    
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Pasien")%></label></td>
                            <td><asp:TextBox ID="txtServicePatient" ReadOnly="true" Width="200px" CssClass="txtCurrency" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Instansi")%></label></td>
                            <td><asp:TextBox ID="txtServicePayer" ReadOnly="true" Width="200px" CssClass="txtCurrency" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label><%=GetLabel("Total")%></label></td>
                            <td><asp:TextBox ID="txtServiceTotal" ReadOnly="true" Width="200px" CssClass="txtCurrency" runat="server" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <colgroup>
                <col style="width:100px"/>
                <col />
            </colgroup>
        </table>
    </fieldset>
</div>
