<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true"
    CodeBehind="VaccinationScheduleInfo.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.VaccinationScheduleInfo"
    EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <style type="text/css">
        .grdVaccinationPeriodInfo th
        {
            font-weight: bold;
        }
        .grdVaccinationPeriodInfo th, .grdVaccinationPeriodInfo td
        {
            font-size: 13px;
        }
    </style>
    <div>
        <div style="text-align: center; font-size: 12px;">
            <h2 style="margin-top: 5px">
                JADWAL IMUNISASI ANAK UMUR 0 - 18 tahun</h2>
            <h3 style="margin-top: -5px">
                Rekomendasi Ikatan Dokter Anak Indonesia (IDAI), Tahun 2023</h3>
        </div>
        <div>
            <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                <EmptyDataTemplate>
                    <table id="tblView" runat="server" class="grdVaccinationPeriodInfo grdNormal" cellspacing="0"
                        rules="all">
                        <tr>
                            <th rowspan="3">
                                <%=GetLabel("VAKSIN / IMUNISASI")%>
                            </th>
                            <th colspan="28">
                                <%=GetLabel("USIA")%>
                            </th>
                        </tr>
                        <tr>
                            <th colspan="12">
                                <%=GetLabel("BULAN")%>
                            </th>
                            <th colspan="16">
                                <%=GetLabel("TAHUN")%>
                            </th>
                        </tr>
                        <tr>
                            <th style="width: 40px">
                                <%=GetLabel("Lahir")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("1")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("2")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("3")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("4")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("5")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("6")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("9")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("12")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("15")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("18")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("24")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("3")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("4")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("5")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("6")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("7")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("8")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("9")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("10")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("11")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("12")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("13")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("14")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("15")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("16")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("17")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("18")%>
                            </th>
                        </tr>
                        <tr class="trEmpty">
                            <td colspan="50">
                                <%=GetLabel("No Data To Display")%>
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <table id="tblView" runat="server" class="grdVaccinationPeriodInfo grdNormal" cellspacing="0"
                        rules="all">
                        <tr>
                            <th rowspan="3">
                                <%=GetLabel("VAKSIN / IMUNISASI")%>
                            </th>
                            <th colspan="28">
                                <%=GetLabel("USIA")%>
                            </th>
                        </tr>
                        <tr>
                            <th colspan="12">
                                <%=GetLabel("BULAN")%>
                            </th>
                            <th colspan="16">
                                <%=GetLabel("TAHUN")%>
                            </th>
                        </tr>
                        <tr>
                            <th style="width: 40px">
                                <%=GetLabel("Lahir")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("1")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("2")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("3")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("4")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("5")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("6")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("9")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("12")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("15")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("18")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("24")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("3")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("4")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("5")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("6")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("7")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("8")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("9")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("10")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("11")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("12")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("13")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("14")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("15")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("16")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("17")%>
                            </th>
                            <th style="width: 40px">
                                <%=GetLabel("18")%>
                            </th>
                        </tr>
                        <tr runat="server" id="itemPlaceholder">
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td style="background-color: #6ab04c; font-size: 1.1em; font-weight: bold">
                            <%#: Eval("VaccinationTypeName")%>
                        </td>
                        <td align="center" id="tdCol0" runat="server">
                        </td>
                        <td align="center" id="tdCol1" runat="server">
                        </td>
                        <td align="center" id="tdCol2" runat="server">
                        </td>
                        <td align="center" id="tdCol3" runat="server">
                        </td>
                        <td align="center" id="tdCol4" runat="server">
                        </td>
                        <td align="center" id="tdCol5" runat="server">
                        </td>
                        <td align="center" id="tdCol6" runat="server">
                        </td>
                        <td align="center" id="tdCol9" runat="server">
                        </td>
                        <td align="center" id="tdCol12" runat="server">
                        </td>
                        <td align="center" id="tdCol15" runat="server">
                        </td>
                        <td align="center" id="tdCol18" runat="server">
                        </td>
                        <td align="center" id="tdCol24" runat="server">
                        </td>
                        <td align="center" id="tdCol36" runat="server">
                        </td>
                        <td align="center" id="tdCol48" runat="server">
                        </td>
                        <td align="center" id="tdCol60" runat="server">
                        </td>
                        <td align="center" id="tdCol72" runat="server">
                        </td>
                        <td align="center" id="tdCol84" runat="server">
                        </td>
                        <td align="center" id="tdCol96" runat="server">
                        </td>
                        <td align="center" id="tdCol108" runat="server">
                        </td>
                        <td align="center" id="tdCol120" runat="server">
                        </td>
                        <td align="center" id="tdCol132" runat="server">
                        </td>
                        <td align="center" id="tdCol144" runat="server">
                        </td>
                        <td align="center" id="tdCol156" runat="server">
                        </td>
                        <td align="center" id="tdCol168" runat="server">
                        </td>
                        <td align="center" id="tdCol180" runat="server">
                        </td>
                        <td align="center" id="tdCol192" runat="server">
                        </td>
                        <td align="center" id="tdCol204" runat="server">
                        </td>
                        <td align="center" id="tdCol216" runat="server">
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
        <br />
        <div>
            <table border="0" cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width: 270px" />
                    <col style="width: 20px" />
                    <col style="width: 270px" />
                    <col style="width: 20px" />
                    <col style="width: 270px" />
                    <col style="width: 20px" />
                    <col style="width: 270px" />
                    <col style="width: 20px" />
                    <col style="width: 270px" />
                </colgroup>
                <tr>
                    <td style="background-color: #89e1fb; text-align: center; color: Black">
                        Primer
                    </td>
                    <td>
                    </td>
                    <td style="background-color: #fbf489; text-align: center; color: Black">
                        Catch-up
                    </td>
                    <td>
                    </td>
                    <td style="background-color: #89fb89; text-align: center; color: Black">
                        Booster
                    </td>
                    <td>
                    </td>
                    <td style="background-color: #fb89fb; text-align: center; color: Black">
                        Daerah Endemis
                    </td>
                    <td>
                    </td>
                    <td style="background-color: #ffa233; text-align: center; color: Black">
                        Untuk Anak dengan Resiko Tinggi
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
