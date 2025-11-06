<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BedInformationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Dashboard.Program.BedInformationCtl" %>
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
            <div class="row stat-cards">
                <div id="ROOMPANEL" class="col-md-6 col-xl-3">
                    <article class="stat-cards-item">
                        <div class="stat-cards-icon primary">
                            <img id="imgDoctor" class="img-circle" runat="server" title="Doctor" alt="Doctor"/>
                        </div>
                        <div id="tes" class="stat-cards-info">
                            <p id="p1" class="stat-cards-info__title2" runat="server"></p>
                            <p class="stat-cards-info__num2">
                                <label class="lblParamedicCount" runat="server" id="lblParamedicCount">
                                </label>
                            </p>
                        </div>
                    </article>
                </div>
            </div>
            <%--<div class="row">
                <div id="TABEL" class="col-xl-12 col-lg-7">
                    <div class="card shadow mb-4">
                        <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                            <h6 class="m-0 font-weight-bold title-card-header">
                                Informasi Tempat Tidur</h6>
                            <div class="dropdown no-arrow">
                                <a class="dropdown-toggle" data-bs-toggle="collapse" href="#collapseExample" role="button"
                                    aria-expanded="false" aria-controls="collapseExample"></a>
                            </div>
                        </div>
                        <div class="card-body">
                                <table id="example" class="display" width="100%"></table>
                        </div>
                    </div>
                </div>
            </div>--%>
            </div>
        </main>
</div>
<input type="hidden" value="" id="DataTable" runat="server" />
<input type="hidden" value="" id="hdnServiceUnitCount" runat="server" />
<script type="text/javascript" id="dxss_bedinformationctl">
//    $(function () {
//        var ex = $('#example').DataTable({
//            data: JSON.parse($('#<%=DataTable.ClientID %>').val()),
//            columns: [
//            { title: "Ruang Perawatan", data: "ServiceUnitName" },
//            { title: "Kamar", data: "RoomName" },
//            { title: "Tempat Tidur", data: "BedCode" },
//            { title: "Kelas", data: "ClassName" },
//            { title: "Status", data: "BedStatus" },
//        ]
//        });
//        for (let i = 0; i < 3; i++) {
//            myFunction();
//        }
//    });

    function myFunction() {
        var newItem = document.createElement("DIV");
        var textnode = document.createTextNode("water");
        newItem.appendChild(textnode);newItem.attributes

        var list = document.getElementById("tes");
        list.insertBefore(newItem, list.childNodes[0]);
    }
</script>
