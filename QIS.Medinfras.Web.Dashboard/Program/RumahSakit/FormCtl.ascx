<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormCtl.ascx.cs" Inherits="QIS.Medinfras.Web.Dashboard.Program.FormCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<div class="main-wrapper">
    <main class="main users" id="skip-target">
            <div class="containerdb">
                <div class="d-sm-flex align-items-center justify-content-between m-2"></div>
            <div class="row">
                <div id="TABEL" style="width: 94%; margin-top: 20px; margin-left: 3%;">
                    <div class="card shadow mb-4">
                        <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                            <h6 class="m-0 font-weight-bold title-card-header">
                                ISIAN</h6>
                            <div class="dropdown no-arrow">
                                <a class="dropdown-toggle" data-bs-toggle="collapse" href="#collapseExample" role="button"
                                    aria-expanded="false" aria-controls="collapseExample"></a>
                            </div>
                        </div>
                        <div class="card-body">
                             <form class="form-horizontal" action="/action_page.php">
   <div class="form-group">
  <label for="usr">Name:</label>
  <input type="text" class="form-control border" id="usr">
</div>
<div class="form-group">
  <label for="pwd">Password:</label>
  <input type="password" class="form-control border" id="pwd">
</div> 
  <div class="form-group">
    <div class="col-sm-offset-2 col-sm-10">
      <div class="checkbox">
        <label><input type="checkbox"> Remember me</label>
      </div>
    </div>
  </div>
   <div class="form-group">
  <label for="comment">Comment:</label>
  <textarea class="form-control" rows="5" id="comment"></textarea>
</div> 
  <div class="form-group mt-2">
    <div class="col-sm-offset-2 col-sm-10">
      <button type="submit" class="btn border">Submit</button>
    </div>
  </div>
</form> 
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div id="Carousel" style="width: 94%; margin-top: 20px; margin-left: 3%;">
                    <div class="card shadow mb-4">
                        <div class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                            <h6 class="m-0 font-weight-bold title-card-header">
                                CAROUSEL</h6>
                            <div class="dropdown no-arrow">
                                <a class="dropdown-toggle" data-bs-toggle="collapse" href="#collapseExample" role="button"
                                    aria-expanded="false" aria-controls="collapseExample"></a>
                            </div>
                        </div>
                        <div class="card-body">
                                <div id="demo" class="carousel slide" data-ride="carousel">

  <!-- Indicators -->
  <ul class="carousel-indicators">
    <li data-target="#demo" data-slide-to="0" class="active"></li>
    <li data-target="#demo" data-slide-to="1"></li>
    <li data-target="#demo" data-slide-to="2"></li>
  </ul>
  
  <!-- The slideshow -->
  <div class="carousel-inner">
    <div class="carousel-item active">
      <img id="img1" runat="server" alt="Los Angeles" width="500" height="500">
    </div>
    <div class="carousel-item">
      <img id="img2" runat="server" alt="Chicago" width="500" height="500">
    </div>
    <div class="carousel-item">
      <img id="img3" runat="server" alt="New York" width="500" height="500">
    </div>
  </div>
  
  <!-- Left and right controls -->
  <a class="carousel-control-prev" href="#demo" data-slide="prev">
    <span class="carousel-control-prev-icon btn-warning"></span>
  </a>
  <a class="carousel-control-next" href="#demo" data-slide="next">
    <span class="carousel-control-next-icon btn-success"></span>
  </a>
</div>
                        </div>
                    </div>
                </div>
            </div>
            </div>
        </main>
</div>
<script type="text/javascript" id="dxss_formctl">
</script>
