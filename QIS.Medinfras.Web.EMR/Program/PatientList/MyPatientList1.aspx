<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrxDashboard.master" AutoEventWireup="true" CodeBehind="MyPatientList1.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.MyPatientList1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <div class="page-flex">
        <!-- Sidebar -->
        <aside class="sidebar">
            <div class="sidebar-start">
                <div class="sidebar-head">
            <a href="/" class="logo-wrapper" title="Home">
                <span class="sr-only">Home</span>
                <span class="icon logo" aria-hidden="true"></span>
            </a>
            <button class="sidebar-toggle transparent-btn" title="Menu" type="button">
                <span class="sr-only">Toggle menu</span>
                <span class="icon menu-toggle" aria-hidden="true"></span>
            </button>
                </div>
                <div class="sidebar-body" id="leftPageNavPanelDashboard">
                    <ul class="sidebar-body-menu">
                        <li class="nav-item" contentid="navInpatient">
                            <a href="#" contentid="NavItem1"><span class="icon home" aria-hidden="true"></span>Nav Item 1</a>
                        </li>
                        <li class="nav-item" contentid="navInpatient">
                            <a href="#" contentid="NavItem1"><span class="icon home" aria-hidden="true"></span>Nav Item 2</a>
                        </li>
                    </ul>
                </div>
            </div>
        </aside>
        <div class="main-wrapper">
        </div>
    </div>
</asp:Content>

