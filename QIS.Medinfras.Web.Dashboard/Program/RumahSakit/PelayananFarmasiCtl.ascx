<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PelayananFarmasiCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Dashboard.Program.PelayananFarmasiCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<div class="main-wrapper">
    <main class="main users chart-page" id="skip-target">
        <div class="containerdb">
        <div class="d-sm-flex align-items-center justify-content-between m-2"></div>
        <div class="card shadow mb-3">
            <div class="card" id="headerCard">
                <img src='<%=ResolveUrl("~/Libs/Images/Dashboard/farmasiHeader.png") %>' class="card-img-top" alt="...">
            </div>
            <div class"row">
            <div class"column">
                <div class"row" id="progressDiv">
                    <div class="columnFarmasi shadow mb-4">
                        <div class="bg-primary headerProgress">
                            <p class="headerFarmasiTxt">
                                ORDER MASUK
                            </p>
                        </div>
                            <div class="bg-warning rowFarmasiCategory">
                            <div class="cellRacikan">
                                <p>
                                    RAWAT JALAN
                                </p>
                            </div>
                            <div class="cellRacikan">
                                <p>
                                    FARMASI
                                </p>
                            </div>
                        </div>
                        <div class="rowFarmasi">
                            <div class="cellRacikanCount">
                                <p class="totalOrderCnt"><label class="lblOrderRJCount" runat="server" id="lblOrderRJCount"></label></p>
                            </div>
                            <div class="cellRacikanCount">
                                <p class="totalOrderCnt"><label class="lblOrderFCount" runat="server" id="lblOrderFCount"></label></p>
                            </div>
                        </div>
                    </div>
                    <div class="columnFarmasi shadow mb-4">
                        <div class="bg-primary headerProgress">
                            <p class="headerFarmasiTxt">
                                ORDER DIKERJAKAN
                            </p>
                        </div>
                            <div class="bg-warning rowFarmasiCategory">
                            <div class="cellRacikan">
                                <p>
                                    RACIKAN
                                </p>
                            </div>
                            <div class="cellRacikan">
                                <p>
                                    NON-RACIKAN
                                </p>
                            </div>
                        </div>
                        <div class="rowFarmasi">
                            <div class="cellRacikanCount">
                                <p class="totalOrderCnt"><label class="lblOrderRacikProgressCount" runat="server" id="lblOrderRacikProgressCount"></label></p>
                            </div>
                            <div class="cellRacikanCount">
                                <p class="totalOrderCnt"><label class="lblOrderNonRacikProgressCount" runat="server" id="lblOrderNonRacikProgressCount"></label></p>
                            </div>
                        </div>
                    </div>
                    <div class="columnFarmasi shadow mb-4">
                        <div class="bg-primary headerProgress">
                            <p class="headerFarmasiTxt">
                                ORDER SIAP DISERAHKAN
                            </p>
                        </div>
                            <div class="bg-warning rowFarmasiCategory">
                            <div class="cellRacikan">
                                <p>
                                    RACIKAN
                                </p>
                            </div>
                            <div class="cellRacikan">
                                <p>
                                    NON-RACIKAN
                                </p>
                            </div>
                        </div>
                        <div class="rowFarmasi">
                            <div class="cellRacikanCount">
                                <p class="totalOrderCnt"><label class="lblOrderRacikCompleteCount" runat="server" id="lblOrderRacikCompleteCount"></label></p>
                            </div>
                            <div class="cellRacikanCount">
                                <p class="totalOrderCnt"><label class="lblOrderNonRacikCompleteCount" runat="server" id="lblOrderNonRacikCompleteCount"></label></p>
                            </div>
                        </div>
                    </div>
                    <div class="columnFarmasi shadow mb-4" id="colFarmasiEnd">
                        <div class="bg-primary headerProgress">
                            <p class="headerFarmasiTxt">
                                ORDER DISERAHKAN
                            </p>
                        </div>
                            <div class="bg-warning rowFarmasiCategory">
                            <div class="cellRacikan">
                                <p>
                                    RACIKAN
                                </p>
                            </div>
                            <div class="cellRacikan">
                                <p>
                                    NON-RACIKAN
                                </p>
                            </div>
                        </div>
                        <div class="rowFarmasi">
                            <div class="cellRacikanCount">
                                <p class="totalOrderCnt"><label class="lblOrderRacikGivenCount" runat="server" id="lblOrderRacikGivenCount"></label></p>
                            </div>
                            <div class="cellRacikanCount">
                                <p class="totalOrderCnt"><label class="lblOrderNonRacikGivenCount" runat="server" id="lblOrderNonRacikGivenCount"></label></p>
                            </div>
                        </div>
                    </div>
                </div>
                    <div class="waitTimeContainer shadow mb-4">
                        <div class="row bg-primary" id="waitTimeHeader">
                        <p>
                            WAKTU TUNGGU
                        </p>
                    </div>
                    <div class="row bg-white" id="waitTimeContainer">
                        <div class="waitDuration">
                            <p class="waitTimeCategory"><60 MENIT: <label class="lblWaitUnder60m" runat="server" id="lblWaitUnder60m"></label> PASIEN</p>
                        </div>
                        <div class="waitDuration">
                            <p class="waitTimeCategory"><90 MENIT: <label class="lblWaitUnder90m" runat="server" id="lblWaitUnder90m"></label> PASIEN</p>
                        </div>
                        <div class="waitDuration">
                            <p class="waitTimeCategory"><120 MENIT: <label class="lblWaitUnder120m" runat="server" id="lblWaitUnder120m"></label> PASIEN</p>
                        </div>
                        <div class="waitDurationEnd">
                            <p class="waitTimeCategory">>120 MENIT: <label class="lblWaitUnderXm" runat="server" id="lblWaitUnderXm"></label> PASIEN</p>
                        </div>
                    </div>
                    </div>
                </div>
            </div>
        </div>
        <div>
            <div class="card shadow mb-3 statusOrderFarmasiContainer">
                 <div class="TabelStatusFarmasi shadow mb-4">
                    <div class="bg-primary headerProgress">
                        <p class="headerTabelFarmasiTxt">
                            STATUS ORDER
                        </p>
                    </div>
                    <div class="bg-warning rowFarmasiCategory">
                        <div class="cellTabelStatusHeader" id="TabelStatusHeader1">
                            <p>
                                NO ORDER
                            </p>
                        </div>
                        <div class="cellTabelStatusHeader" id="TabelStatusHeader2">
                            <p>
                                NAMA PASIEN
                            </p>
                        </div>
                        <div class="cellTabelStatusHeader" id="TabelStatusHeader3">
                            <p>
                                STATUS
                            </p>
                        </div>
                    </div>
                    <div class="colTabelStatusFarmasi" id="TabelStatusFarmasiID">
                        <div class="rowTabelStatusFarmasi" id="RowTabelStatusFarmasi1">
                            <div class="TabelStatusContent1">
                                <p class="totalOrderCnt"><label class="lblTransactionNo1" runat="server" id="lblTransactionNo1"></label></p>
                            </div>
                            <div class="TabelStatusContent2">
                                <p class="totalOrderCnt"><label class="lblNamaPasien1" runat="server" id="lblNamaPasien1"></label></p>
                            </div>
                            <div class="TabelStatusContent3">
                                <p class="totalOrderCnt"><label class="lblStatus1" runat="server" id="lblStatus1"></label></p>
                            </div>
                        </div>
                        <div class="rowTabelStatusFarmasi" id="RowTabelStatusFarmasi2">
                            <div class="TabelStatusContent1">
                                <p class="totalOrderCnt"><label class="lblTransactionNo2" runat="server" id="lblTransactionNo2"></label></p>
                            </div>
                            <div class="TabelStatusContent2">
                                <p class="totalOrderCnt"><label class="lblNamaPasien2" runat="server" id="lblNamaPasien2"></label></p>
                            </div>
                            <div class="TabelStatusContent3">
                                <p class="totalOrderCnt"><label class="lblStatus2" runat="server" id="lblStatus2"></label></p>
                            </div>
                        </div>
                        <div class="rowTabelStatusFarmasi" id="RowTabelStatusFarmasi3">
                            <div class="TabelStatusContent1">
                                <p class="totalOrderCnt"><label class="lblTransactionNo3" runat="server" id="lblTransactionNo3"></label></p>
                            </div>
                            <div class="TabelStatusContent2">
                                <p class="totalOrderCnt"><label class="lblNamaPasien3" runat="server" id="lblNamaPasien3"></label></p>
                            </div>
                            <div class="TabelStatusContent3">
                                <p class="totalOrderCnt"><label class="lblStatus3" runat="server" id="lblStatus3"></label></p>
                            </div>
                        </div>
                        <div class="rowTabelStatusFarmasi" id="RowTabelStatusFarmasi4">
                            <div class="TabelStatusContent1">
                                <p class="totalOrderCnt"><label class="lblTransactionNo4" runat="server" id="lblTransactionNo4"></label></p>
                            </div>
                            <div class="TabelStatusContent2">
                                <p class="totalOrderCnt"><label class="lblNamaPasien4" runat="server" id="lblNamaPasien4"></label></p>
                            </div>
                            <div class="TabelStatusContent3">
                                <p class="totalOrderCnt"><label class="lblStatus4" runat="server" id="lblStatus4"></label></p>
                            </div>
                        </div>
                        <div class="rowTabelStatusFarmasi" id="RowTabelStatusFarmasi5">
                            <div class="TabelStatusContent1">
                                <p class="totalOrderCnt"><label class="lblTransactionNo5" runat="server" id="lblTransactionNo5"></label></p>
                            </div>
                            <div class="TabelStatusContent2">
                                <p class="totalOrderCnt"><label class="lblNamaPasien5" runat="server" id="lblNamaPasien5"></label></p>
                            </div>
                            <div class="TabelStatusContent3">
                                <p class="totalOrderCnt"><label class="lblStatus5" runat="server" id="lblStatus5"></label></p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>
</div>
<input type="hidden" value="" id="JsonDataStatusOrder" runat="server" />
<script type="text/javascript" id="dxss_bedinformationctl">
//    $(function () {
//        addStatus();
//    });

//    function addStatus() {
//        var data = JSON.parse($('#<%=JsonDataStatusOrder.ClientID %>').val());

//        for (i = 0; i < data.length; i++) {
//            var selectCol = document.getElementById("TabelStatusFarmasiID");
//            var addRow = document.createElement("div", {"class":"rowTabelStatusFarmasi", "id":"RowTabelStatusFarmasi1"});
//            selectCol.appendChild(addRow);
//            var selectRow = document.getElementById('rowTabelStatusFarmasi' + i.toString());
//            var addCell = document.createElement("div", {"class":"TabelStatusContent" + i.toString(), "id":"StatusCellContent" + i.toString()});
//            selectRow.appendChild(addCell);

//            var selectCell = document.getElementById('StatusCellContent' + i.toString());
//            for (x = 1; x <= 2; x++){
//                var addP = document.createElement("p", {"class":"totalOrderCnt", "id":"StatusTextContent" + x.toString()});
//                selectCell.appendChild(addP);
//            }

//            var selectP1 = document.getElementById('StatusTextContent1');
//            var addDataNama = document.createTextNode(data[i].PatientName);
//            selectP1.appendChild(addDataNama);
//            var selectP1 = document.getElementById('StatusTextContent2');
//            var addDataNama = document.createTextNode(data[i].Status);
//            selectP1.appendChild(addDataNama);
//            }
//    }

    
</script>
