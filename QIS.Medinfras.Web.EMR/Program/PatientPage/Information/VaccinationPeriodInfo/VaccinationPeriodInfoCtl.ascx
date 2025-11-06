<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VaccinationPeriodInfoCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.VaccinationPeriodInfoCtl" %>

<style type="text/css">
    .grdVaccinationPeriodInfo th    { font-weight:bold; }
    .grdVaccinationPeriodInfo th, .grdVaccinationPeriodInfo td    { font-size: 13px; }
</style>
<div>
    <div style="text-align: center;font-size:12px;" >
        <h2 style="margin-top:-2px">
            JADWAL IMUNISASI ANAK UMUR 0 - 18 tahun</h2>
        <h3 style="margin-top:-5px">
            Rekomendasi Ikatan Dokter Anak Indonesia (IDAI), Tahun 2011</h3>
    </div>
    <div>
        <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
            <EmptyDataTemplate>
                <table id="tblView" runat="server" class="grdVaccinationPeriodInfo grdNormal" cellspacing="0" rules="all" >
                    <tr>  
                        <th rowspan="3"><%=GetLabel("JENIS VAKSIN")%></th>
                        <th colspan="20"><%=GetLabel("UMUR PEMBERIAN VAKSIN")%></th>                  
                    </tr>
                    <tr>  
                        <th colspan="12"><%=GetLabel("BULAN")%></th>
                        <th colspan="8"><%=GetLabel("TAHUN")%></th>                  
                    </tr>
                    <tr>
                        <th style="width:40px"><%=GetLabel("LHR")%></th>
                        <th style="width:40px"><%=GetLabel("1")%></th>
                        <th style="width:40px"><%=GetLabel("2")%></th> 
                        <th style="width:40px"><%=GetLabel("3")%></th>  
                        <th style="width:40px"><%=GetLabel("4")%></th>  
                        <th style="width:40px"><%=GetLabel("5")%></th>  
                        <th style="width:40px"><%=GetLabel("6")%></th>  
                        <th style="width:40px"><%=GetLabel("9")%></th>  
                        <th style="width:40px"><%=GetLabel("12")%></th>  
                        <th style="width:40px"><%=GetLabel("15")%></th>  
                        <th style="width:40px"><%=GetLabel("18")%></th>  
                        <th style="width:40px"><%=GetLabel("24")%></th>  
                         
                        <th style="width:40px"><%=GetLabel("3")%></th>  
                        <th style="width:40px"><%=GetLabel("5")%></th>  
                        <th style="width:40px"><%=GetLabel("6")%></th>  
                        <th style="width:40px"><%=GetLabel("7")%></th>  
                        <th style="width:40px"><%=GetLabel("8")%></th>  
                        <th style="width:40px"><%=GetLabel("10")%></th>  
                        <th style="width:40px"><%=GetLabel("12")%></th>  
                        <th style="width:40px"><%=GetLabel("18")%></th> 
                    </tr>
                    <tr class="trEmpty">
                        <td colspan="21">
                            No Data To Display
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <table id="tblView" runat="server" class="grdVaccinationPeriodInfo grdNormal" cellspacing="0" rules="all" >
                    <tr>  
                        <th rowspan="3"><%=GetLabel("JENIS VAKSIN")%></th>
                        <th colspan="20"><%=GetLabel("UMUR PEMBERIAN VAKSIN")%></th>                  
                    </tr>
                    <tr>  
                        <th colspan="12"><%=GetLabel("BULAN")%></th>
                        <th colspan="12"><%=GetLabel("TAHUN")%></th>                  
                    </tr>
                    <tr>
                        <th style="width:40px"><%=GetLabel("LHR")%></th>
                        <th style="width:40px"><%=GetLabel("1")%></th>
                        <th style="width:40px"><%=GetLabel("2")%></th> 
                        <th style="width:40px"><%=GetLabel("3")%></th>  
                        <th style="width:40px"><%=GetLabel("4")%></th>  
                        <th style="width:40px"><%=GetLabel("5")%></th>  
                        <th style="width:40px"><%=GetLabel("6")%></th>  
                        <th style="width:40px"><%=GetLabel("9")%></th>  
                        <th style="width:40px"><%=GetLabel("12")%></th>  
                        <th style="width:40px"><%=GetLabel("15")%></th>  
                        <th style="width:40px"><%=GetLabel("18")%></th>  
                        <th style="width:40px"><%=GetLabel("24")%></th>  
                         
                        <th style="width:40px"><%=GetLabel("3")%></th>  
                        <th style="width:40px"><%=GetLabel("5")%></th>  
                        <th style="width:40px"><%=GetLabel("6")%></th>  
                        <th style="width:40px"><%=GetLabel("7")%></th>  
                        <th style="width:40px"><%=GetLabel("8")%></th>  
                        <th style="width:40px"><%=GetLabel("10")%></th>  
                        <th style="width:40px"><%=GetLabel("12")%></th>  
                        <th style="width:40px"><%=GetLabel("18")%></th> 
                    </tr>
                    <tr runat="server" id="itemPlaceholder" ></tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td style="background-color:<%#: Eval("DisplayColor")%>"><%#: Eval("VaccinationTypeName")%></td>
                    <td align="center" id="tdCol0" runat="server"></td>
                    <td align="center" id="tdCol1" runat="server"></td>
                    <td align="center" id="tdCol2" runat="server"></td>
                    <td align="center" id="tdCol3" runat="server"></td>
                    <td align="center" id="tdCol4" runat="server"></td>
                    <td align="center" id="tdCol5" runat="server"></td>
                    <td align="center" id="tdCol6" runat="server"></td>
                    <td align="center" id="tdCol9" runat="server"></td>
                    <td align="center" id="tdCol12" runat="server"></td>
                    <td align="center" id="tdCol15" runat="server"></td>
                    <td align="center" id="tdCol18" runat="server"></td>
                    <td align="center" id="tdCol24" runat="server"></td>

                    <td align="center" id="tdCol36" runat="server"></td>
                    <td align="center" id="tdCol60" runat="server"></td>
                    <td align="center" id="tdCol72" runat="server"></td>
                    <td align="center" id="tdCol84" runat="server"></td>
                    <td align="center" id="tdCol96" runat="server"></td>
                    <td align="center" id="tdCol120" runat="server"></td>
                    <td align="center" id="tdCol144" runat="server"></td>
                    <td align="center" id="tdCol216" runat="server"></td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>

</div>