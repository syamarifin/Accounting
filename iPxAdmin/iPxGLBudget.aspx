<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPageUpload.master" AutoEventWireup="false" CodeFile="iPxGLBudget.aspx.vb" Inherits="iPxAdmin_iPxGLBudget" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .cellOneCellPaddingLeft {
            padding-left: 0pt !important;
        }
        .cellOneCellPaddingRight {
            padding-right: 0pt !important;
        }
    </style>
    <script type="text/javascript">
        function sessionSave()
        {
            if($("#Revenue-tab").hasClass("active")){
                document.getElementById("<%=hfCount.ClientID %>").value = "Revenue";
            }else if($("#Cost-tab").hasClass("active")){
                document.getElementById("<%=hfCount.ClientID %>").value = "Cost";
            }else if($("#Statistic-tab").hasClass("active")){
                document.getElementById("<%=hfCount.ClientID %>").value = "Statistic";
            }else if($("#Asset-tab").hasClass("active")){
                document.getElementById("<%=hfCount.ClientID %>").value = "Asset";
            }else if($("#Liability-tab").hasClass("active")){
                document.getElementById("<%=hfCount.ClientID %>").value = "Liability";
            }else if($("#Equity-tab").hasClass("active")){
                document.getElementById("<%=hfCount.ClientID %>").value = "Equity";
            }
        }
        function YearGL() {
            $(".yearGl").datepicker({ format: 'yyyy', viewMode: "years", minViewMode: "years", autoclose: true, todayBtn: 'linked' })
        }
        
        function AssetActive()
        {
            $("#Asset-tab").addClass("active");
            $("#Liability-tab").removeClass("active");
            $("#Equity-tab").removeClass("active");
            $("#Revenue-tab").removeClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Statistic-tab").removeClass("active");
            $("#Asset").addClass("active in");
            $("#Liability").removeClass("active in");
            $("#Equity").removeClass("active in");
            $("#Revenue").removeClass("active in");
            $("#Cost").removeClass("active in");
            $("#Statistic").removeClass("active in");
        } 
        function LiabilityActive()
        {
            $("#Asset-tab").removeClass("active");
            $("#Liability-tab").addClass("active");
            $("#Equity-tab").removeClass("active");
            $("#Revenue-tab").removeClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Statistic-tab").removeClass("active");
            $("#Asset").removeClass("active in");
            $("#Liability").addClass("active in");
            $("#Equity").removeClass("active in");
            $("#Revenue").removeClass("active in");
            $("#Cost").removeClass("active in");
            $("#Statistic").removeClass("active in");
        }
        function EquityActive()
        {
            $("#Asset-tab").removeClass("active");
            $("#Liability-tab").removeClass("active");
            $("#Equity-tab").addClass("active");
            $("#Revenue-tab").removeClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Statistic-tab").removeClass("active");
            $("#Asset").removeClass("active in");
            $("#Liability").removeClass("active in");
            $("#Equity").addClass("active in");
            $("#Revenue").removeClass("active in");
            $("#Cost").removeClass("active in");
            $("#Statistic").removeClass("active in");
        }
        function RevenueActive()
        {
            $("#Asset-tab").removeClass("active");
            $("#Liability-tab").removeClass("active");
            $("#Equity-tab").removeClass("active");
            $("#Revenue-tab").addClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Statistic-tab").removeClass("active");
            $("#Asset").removeClass("active in");
            $("#Liability").removeClass("active in");
            $("#Equity").removeClass("active in");
            $("#Revenue").addClass("active in");
            $("#Cost").removeClass("active in");
            $("#Statistic").removeClass("active in");
        }
        function CostActive()
        {
            $("#Asset-tab").removeClass("active");
            $("#Liability-tab").removeClass("active");
            $("#Equity-tab").removeClass("active");
            $("#Revenue-tab").removeClass("active");
            $("#Cost-tab").addClass("active");
            $("#Statistic-tab").removeClass("active");
            $("#Asset").removeClass("active in");
            $("#Liability").removeClass("active in");
            $("#Equity").removeClass("active in");
            $("#Revenue").removeClass("active in");
            $("#Cost").addClass("active in");
            $("#Statistic").removeClass("active in");
        }
        function StatisticActive()
        {
            $("#Asset-tab").removeClass("active");
            $("#Liability-tab").removeClass("active");
            $("#Equity-tab").removeClass("active");
            $("#Revenue-tab").removeClass("active");
            $("#Cost-tab").removeClass("active");
            $("#Statistic-tab").addClass("active");
            $("#Asset").removeClass("active in");
            $("#Liability").removeClass("active in");
            $("#Equity").removeClass("active in");
            $("#Revenue").removeClass("active in");
            $("#Cost").removeClass("active in");
            $("#Statistic").addClass("active in");
        }

        function hideModalAddDev() {
            $('#formInputDev').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalAddDev() {
            $('#formInputDev').modal({ backdrop: 'static',
                keyboard: false
            },'show');
        }
        function hideModalAddDept() {
            $('#formInputDept').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalAddDept() {
            $('#formInputDept').modal({ backdrop: 'static',
                keyboard: false
            },'show');
        }
        function hideModalAddSubDept() {
            $('#formInputSubDept').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalAddSubDept() {
            $('#formInputSubDept').modal({ backdrop: 'static',
                keyboard: false
            },'show');
        }
        
        function hideModalAddCoa() {
            $('#formInputCoa').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalAddCoa() {
            $('#formInputCoa').modal({ backdrop: 'static',
                keyboard: false
            },'show');
        }
        
        function hideModalImport() {
            $('#formImport').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalImport() {
            $('#formImport').modal({ backdrop: 'static',
                keyboard: false
            },'show');
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- edit Budget Modal-->
    <div id="formInput" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <!-- <asp:LinkButton ID="lbAbortEdit" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton> -->
                    <button type="button" data-dismiss="modal" aria-label="Close" class="close"><span aria-hidden="true">×</span></button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-4">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">COA:</label>
                                <asp:TextBox ID="tbCOA" runat="server" CssClass ="form-control" enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-2">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Description:</label>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-group" style="margin-bottom:3px;">
                                <asp:TextBox ID="tbDesc" runat="server" CssClass ="form-control" Height="60px" enabled="false" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <hr/>
                    <div class="row">
                        <div class="col-lg-4">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">January:</label><font color=red>*</font>
                                <asp:TextBox ID="tbJan" runat="server" CssClass ="form-control" ></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">February:</label><font color=red>*</font>
                                <asp:TextBox ID="tbFeb" runat="server" CssClass ="form-control" ></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">March:</label><font color=red>*</font>
                                <asp:TextBox ID="tbMar" runat="server" CssClass ="form-control" ></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">April:</label><font color=red>*</font>
                                <asp:TextBox ID="tbApr" runat="server" CssClass ="form-control" ></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-4">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">May:</label><font color=red>*</font>
                                <asp:TextBox ID="tbMei" runat="server" CssClass ="form-control" ></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Juni:</label><font color=red>*</font>
                                <asp:TextBox ID="tbJun" runat="server" CssClass ="form-control" ></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">July:</label><font color=red>*</font>
                                <asp:TextBox ID="tbJul" runat="server" CssClass ="form-control" ></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">August:</label><font color=red>*</font>
                                <asp:TextBox ID="tbAgu" runat="server" CssClass ="form-control" ></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-4">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">September:</label><font color=red>*</font>
                                <asp:TextBox ID="tbSep" runat="server" CssClass ="form-control" ></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">October:</label><font color=red>*</font>
                                <asp:TextBox ID="tbOkt" runat="server" CssClass ="form-control" ></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">November:</label><font color=red>*</font>
                                <asp:TextBox ID="tbNov" runat="server" CssClass ="form-control" ></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">December:</label><font color=red>*</font>
                                <asp:TextBox ID="tbDes" runat="server" CssClass ="form-control" ></asp:TextBox>
                            </div>
                            
                        </div>
                    </div>
                </div>
                <div class="modal-footer" style="background-color:Transparent; text-align:right;">
                    <asp:LinkButton ID="lbUpdateBudget" Width="150px" runat="server" CssClass="btn btn-default" OnClientClick = "return sessionSave();"><i class="fa fa-save"></i> Save</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- edit Budget Modal end-->
    <!-- Import Modal -->
    <div id="formImport" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortImport" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H5" class="modal-title">Import Journal Budget(.xslx) </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <asp:FileUpload ID="flMassUpoad" runat="server" accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbStartImport" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> Import</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Import Modal end-->
    <!-- Copy Modal -->
    <div id="formCopyBudget" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortCopy" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H1" class="modal-title">Copy Journal Budget </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group">
                                <label for="usr" style="margin:0;">From Year :</label>
                                <div class="input-group date yearGl" style="padding:0;">
                                    <asp:TextBox ID="tbFromCopy" runat="server" CssClass ="form-control" placeholder="MM yyyy" autocomplete="off"></asp:TextBox>
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="usr" style="margin:0;">To Year :</label>
                                <div class="input-group date yearGl" style="padding:0;">
                                    <asp:TextBox ID="tbToCopy" runat="server" CssClass ="form-control" placeholder="MM yyyy" autocomplete="off"></asp:TextBox>
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbStartCopy" runat="server" CssClass="btn btn-default"><i class="fa fa-copy"></i> Start Copy</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Copy Modal end-->
    <div class="row">
        <div class="col-lg-2">
            <div class="form-group">
                <label for="usr" style="margin:0;">Year :</label>
                <div class="input-group " style="padding:0;">
                    <asp:TextBox ID="tbDate" runat="server" CssClass ="form-control date yearGl" placeholder="MM yyyy" autocomplete="off"></asp:TextBox>
                    <%--<span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>--%>
                    <div class="input-group-btn">
                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-default"><i class="fa fa-rotate-right" style="height:20px"></i></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-10">
            <br />
            <asp:LinkButton ID="lbAddCoa" Width="150px" runat="server" CssClass="btn btn-default btn-top" OnClientClick = "return sessionSave();"><i class="fa fa-save"></i> Save</asp:LinkButton>
            <asp:LinkButton ID="lbFormatExc" Width="150px" runat="server" CssClass="btn btn-default btn-top" ><i class="fa fa-cloud-download"></i> Format Excel</asp:LinkButton>
            <asp:LinkButton ID="lbImport" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> Import From Excel</asp:LinkButton>
            <%--<asp:LinkButton ID="lbPrint" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-print"></i> Print</asp:LinkButton>--%>
            <div class="btn-group">
                <button type="button" style="width:130px;" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                <i class="fa fa-print"></i> Print <span class="caret"></span></button>
                <ul class="dropdown-menu" role="menu">
                    <li><asp:LinkButton ID="lbPrint" runat="server" CssClass="btn" style="text-align:left;">Semester 1(Jan-Jun)</asp:LinkButton></li>
                    <li><asp:LinkButton ID="lbPrint2" runat="server" CssClass="btn" style="text-align:left;">Semester 2(Jul-Des)</asp:LinkButton></li>
                </ul>
            </div>
            <asp:LinkButton ID="lbCopy" runat="server" CssClass="btn btn-default"><i class="fa fa-copy"></i> Copy Budget</asp:LinkButton>
            <br/>
            <asp:HiddenField ID="hfCount" runat="server" Value = "Revenue" />
        </div>
        <div class="col-lg-12">
            <ul class="nav nav-tabs">
                <li id="Revenue-tab" class="active"><a href="#Revenue" data-toggle="tab"><i class="fa fa-list"></i> Revenue&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a></li>
                <li id="Cost-tab">                  <a href="#Cost" data-toggle="tab"><i class="fa fa-list"></i> Cost & Expenses</a></li>
                <li id="Statistic-tab">             <a href="#Statistic" data-toggle="tab"><i class="fa fa-list"></i> Statistic&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a></li>
                <li id="Asset-tab">                 <a href="#Asset" data-toggle="tab"><i class="fa fa-list"></i> Asset&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a></li>
                <li id="Liability-tab">             <a href="#Liability" data-toggle="tab"><i class="fa fa-list"></i> Liability&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a></li>
                <li id="Equity-tab">                <a href="#Equity" data-toggle="tab"><i class="fa fa-list"></i> Equity&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a></li>
            </ul>
            <div id="myTabContent" class="tab-content">
                <div class="tab-pane active in" id="Revenue">
                    <%--<asp:LinkButton ID="lbAddCoa" Width="150px" runat="server" CssClass="btn btn-default btn-top"><i class="fa fa-save"></i> Save</asp:LinkButton>--%>
                    <div style="overflow-x:auto;max-width:1000px">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvRev" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:TemplateField HeaderText="group" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" ItemStyle-HorizontalAlign="left" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdCoa" runat="server"  Value='<%# Eval("Coa") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingRight">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailClear" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' CommandArgument='<%# Eval("Coa") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-Width="50px" DataField="Coa" HeaderText="Coa" ItemStyle-CssClass="cellOneCellPaddingLeft" />
                            <asp:BoundField ItemStyle-Width="200px" DataField="Description" HeaderText="Description" />
                            <asp:TemplateField ItemStyle-Width="60px" HeaderText="Group Level">
                                <ItemTemplate>
                                    <asp:Label ID="lbGrpLevelRev" runat="server" Text='<%# if(Eval("grpLevel").toString()="G","Group","Detail") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jan"  ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln1" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln1")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Feb"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln2" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln2")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mar"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln3" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln3")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Apr"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln4" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln4")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mei"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln5" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln5")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jun"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln6" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln6")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jul"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln7" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln7")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Agst"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln8" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln8")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sep"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln9" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln9")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Okt"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln10" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln10")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nov"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln11" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln11")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Desc"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln12" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln12")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                    </div>
                </div>
                <div class="tab-pane" id="Cost">
                    <div style="overflow-x:auto;max-width:1000px">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvCost" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:TemplateField HeaderText="group" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" ItemStyle-HorizontalAlign="left" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdCoa" runat="server"  Value='<%# Eval("Coa") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingRight">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailClear" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' CommandArgument='<%# Eval("Coa") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-Width="50px" DataField="Coa" HeaderText="Coa" ItemStyle-CssClass="cellOneCellPaddingLeft" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Description" HeaderText="Description" />
                            <asp:TemplateField ItemStyle-Width="60px" HeaderText="Group Level">
                                <ItemTemplate>
                                    <asp:Label ID="lbGrpLevelCost" runat="server" Text='<%# if(Eval("grpLevel").toString()="G","Group","Detail") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jan"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln1" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln1")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Feb"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln2" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln2")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mar"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln3" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln3")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Apr"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln4" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln4")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mei"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln5" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln5")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jun"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln6" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln6")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jul"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln7" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln7")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Agst"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln8" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln8")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sep"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln9" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln9")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Okt"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln10" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln10")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nov"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln11" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln11")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Desc"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln12" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln12")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                    </div>
                </div>
                <div class="tab-pane" id="Statistic">
                    <div style="overflow-x:auto;max-width:1000px">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvStatistic" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:TemplateField HeaderText="group" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" ItemStyle-HorizontalAlign="left" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdCoa" runat="server"  Value='<%# Eval("Coa") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingRight">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailClear" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' CommandArgument='<%# Eval("Coa") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-Width="50px" DataField="Coa" HeaderText="Coa" ItemStyle-CssClass="cellOneCellPaddingLeft" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Description" HeaderText="Description" />
                            <asp:TemplateField ItemStyle-Width="60px" HeaderText="Group Level">
                                <ItemTemplate>
                                    <asp:Label ID="lbGrpLevelStatistic" runat="server" Text='<%# if(Eval("grpLevel").toString()="G","Group","Detail") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jan"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln1" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln1")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Feb"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln2" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln2")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mar"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln3" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln3")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Apr"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln4" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln4")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mei"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln5" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln5")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jun"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln6" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln6")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jul"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln7" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln7")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Agst"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln8" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln8")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sep"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln9" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln9")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Okt"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln10" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln10")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nov"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln11" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln11")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Desc"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln12" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln12")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                    </div>
                </div>
                <div class="tab-pane" id="Asset">
                    <div style="overflow-x:auto;max-width:1000px">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvCoaAsset" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:TemplateField HeaderText="group" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" ItemStyle-HorizontalAlign="left" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdCoa" runat="server"  Value='<%# Eval("Coa") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingRight">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailClear" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' CommandArgument='<%# Eval("Coa") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-Width="50px" DataField="Coa" HeaderText="Coa" ItemStyle-CssClass="cellOneCellPaddingLeft" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Description" HeaderText="Description" />
                            <asp:TemplateField ItemStyle-Width="60px" HeaderText="Group Level">
                                <ItemTemplate>
                                    <asp:Label ID="lbGrpLevel" runat="server" Text='<%# if(Eval("grpLevel").toString()="G","Group","Detail") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jan"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln1" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln1")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Feb"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln2" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln2")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mar"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln3" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln3")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Apr"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln4" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln4")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mei"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln5" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln5")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jun"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln6" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln6")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jul"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln7" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln7")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Agst"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln8" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln8")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sep"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln9" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln9")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Okt"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln10" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln10")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nov"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln11" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln11")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Desc"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln12" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln12")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                    </div>
                </div>
                <div class="tab-pane" id="Liability">
                    <div style="overflow-x:auto;max-width:1000px">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvLia" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:TemplateField HeaderText="group" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" ItemStyle-HorizontalAlign="left" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdCoa" runat="server"  Value='<%# Eval("Coa") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingRight">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailClear" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' CommandArgument='<%# Eval("Coa") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-Width="50px" DataField="Coa" HeaderText="Coa" ItemStyle-CssClass="cellOneCellPaddingLeft" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Description" HeaderText="Description" />
                            <asp:TemplateField ItemStyle-Width="60px" HeaderText="Group Level">
                                <ItemTemplate>
                                    <asp:Label ID="lbGrpLevelLia" runat="server" Text='<%# if(Eval("grpLevel").toString()="G","Group","Detail") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jan"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln1" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln1")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Feb"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln2" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln2")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mar"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln3" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln3")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Apr"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln4" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln4")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mei"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln5" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln5")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jun"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln6" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln6")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jul"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln7" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln7")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Agst"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln8" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln8")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sep"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln9" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln9")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Okt"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln10" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln10")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nov"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln11" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln11")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Desc"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln12" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln12")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                    </div>
                </div>
                <div class="tab-pane" id="Equity">
                    <div style="overflow-x:auto;max-width:1000px">
                    <asp:GridView EmptyDataText="No records has been added." ID="gvEquity" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                        <Columns>
                            <asp:TemplateField HeaderText="group" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" ItemStyle-HorizontalAlign="left" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdCoa" runat="server"  Value='<%# Eval("Coa") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cellOneCellPaddingRight">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDetailClear" CssClass="btn btn-link" style="color:#083765; padding:0; font-size:24px;" runat="server" CommandName="getDetail" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' CommandArgument='<%# Eval("Coa") %>'><i class="fa fa-caret-down"></i> </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-Width="50px" DataField="Coa" HeaderText="Coa" ItemStyle-CssClass="cellOneCellPaddingLeft" />
                            <asp:BoundField ItemStyle-Width="100px" DataField="Description" HeaderText="Description" />
                            <asp:TemplateField ItemStyle-Width="60px" HeaderText="Group Level">
                                <ItemTemplate>
                                    <asp:Label ID="lbGrpLevelEquity" runat="server" Text='<%# if(Eval("grpLevel").toString()="G","Group","Detail") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jan"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln1" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln1")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Feb"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln2" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln2")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mar"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln3" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln3")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Apr"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln4" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln4")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mei"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln5" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln5")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jun"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln6" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln6")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jul"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln7" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln7")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Agst"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln8" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln8")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sep"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln9" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln9")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Okt"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln10" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln10")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nov"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln11" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln11")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Desc"  ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:TextBox Width="125px" ID="txtBln12" CssClass="form-control" style="text-align:right" Enabled='<%# if(Eval("grpLevel").toString()="G","false","true") %>' runat="server" Text='<%# String.Format("{0:N2}", Eval("bln12")  )  %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <pagerstyle cssclass="pagination-ys">
                        </pagerstyle>
                    </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

