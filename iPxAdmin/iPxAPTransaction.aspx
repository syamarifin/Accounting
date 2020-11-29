<%@ Page Language="VB" MasterPageFile="~/iPxAdmin/MasterPage.master" AutoEventWireup="false" CodeFile="iPxAPTransaction.aspx.vb" Inherits="iPxAdmin_iPxAPTransaction" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .textBlack  
        {
        	color:Black;
        	font-weight:bold;
        }
        .textRed  
        {
        	color:Red;
        	font-weight:bold;
        }
        .textgreen  
        {
        	color:green;
        	font-weight:bold;
        }
    </style>
    <script type="text/javascript">
        function MonthGL() {
            $(".monthGl").datepicker({ format: 'mm-yyyy', viewMode: "months", minViewMode: "months", autoclose: true, todayBtn: 'linked' })
        }
    </script>
    <script type = "text/javascript">
    function Check_Click(objRef)
    {
        //Get the Row based on checkbox
        var row = objRef.parentNode.parentNode;
        
        //Get the reference of GridView
        var GridView = row.parentNode;
        
        //Get all input elements in Gridview
        var inputList = GridView.getElementsByTagName("input");
        var en=0;
        for (var i=0;i<inputList.length;i++)
        {
            //The First element is the Header Checkbox
            var headerCheckBox = inputList[0];
            
            //Based on all or none checkboxes
            //are checked check/uncheck Header Checkbox
            var checked = true;
            if(inputList[i].type == "checkbox" && inputList[i] != headerCheckBox)
            {
                if(inputList[i].disabled!=false){
                }else{
                    if(!inputList[i].checked)
                    {
                        checked = false;
                        break;
                    }
                }
            }
        }
        headerCheckBox.checked = checked;
    }
    function checkAll(objRef)
    {
        var GridView = objRef.parentNode.parentNode.parentNode;
        var inputList = GridView.getElementsByTagName("input");
        for (var i=0;i<inputList.length;i++)
        {
            var row = inputList[i].parentNode.parentNode;
            if(inputList[i].type == "checkbox"  && objRef != inputList[i])
            {
                if (objRef.checked)
                {
                    if(inputList[i].disabled!=false){}
                    else{
                        inputList[i].checked=true;
                        }
                }
                else
                {
                    inputList[i].checked=false;
                }
            }
        }
    }
    //-->
    </script>
    <script type="text/javascript">
        function MonthPost() {
            $(".monthGlPost").datepicker({ format: 'MM yyyy', viewMode: "months", minViewMode: "months", autoclose: true, todayBtn: 'linked' })
        }
        function hideModalDatePost() {
            $('#formDatePost').modal('hide');
            $('body').removeClass('modal-open');
            $('body').css("padding-right", "");
            $('.modal-backdrop').hide();
        }
        function showModalDatePost() {
            $('#formDatePost').modal({ backdrop: 'static',
                keyboard: false
            }, 'show');
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--Delete Customer AR Modal--%>
    <div id="formDeleteTrans" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortDelete" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H2" class="modal-title">Do you want to delete Transaction ?</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="usr">Reason to delete Transaction AR :</label><font color=red>*</font>
                        <asp:TextBox ID="tbReason" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbDelete" runat="server" CssClass="btn btn-default"><i class="fa fa-trash"></i> Delete</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <%--End Delete Customer AR Modal--%>
    <!-- Query Customer AR Modal-->
    <div id="formQuery" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-md" style="width:400px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortQuery" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="login-modalLabel" class="modal-title">Query Customer AR</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Transaction ID:</label>
                                <asp:TextBox ID="tbTransID" runat="server" CssClass ="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Transaction Date:</label>
                                <div class="input-group date datepicker" style="padding:0;">
                                    <asp:TextBox ID="tbTransDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                                </div>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">FO Link:</label>
                                <asp:DropDownList ID="dlFOLink" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Reg Date:</label>
                                <div class="input-group date datepicker" style="padding:0;">
                                    <asp:TextBox ID="tbRegDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                                </div>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Status:</label>
                                <asp:DropDownList ID="dlQStatus" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div style="text-align:center;">
                                <asp:Label ID="Label1" runat="server" Text="By Periode Transaction"></asp:Label>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Periode From:</label>
                                <div class="input-group date datepicker" style="padding:0;">
                                    <asp:TextBox ID="tbQFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                                </div>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Periode Until:</label>
                                <div class="input-group date datepicker" style="padding:0;">
                                    <asp:TextBox ID="tbQUntil" runat="server" CssClass="form-control"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lblQuery" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Query Customer AR modal end-->
    <!-- Add Transaction Type Modal-->
    <div id="formInput" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <button type="button" data-dismiss="modal" aria-label="Close" class="close"><span aria-hidden="true">×</span></button>
                    <h4 id="H1" class="modal-title">Import AR Transaction</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Transaction Date:</label>
                                <div class="input-group date datepicker" style="padding:0;">
                                    <asp:TextBox ID="tbDateImport" runat="server" CssClass="form-control" Enabled="false" OnTextChanged="cari" AutoPostBack="true"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                                </div>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">FO Link:</label><font color=red>*</font>
                                <asp:DropDownList ID="dlFOLinkImport" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <asp:HiddenField ID="hfCount" runat="server" Value = "0" />
                            <asp:GridView EmptyDataText="No records has been added." ID="gvFO" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="20px">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAll" runat="server" onclick = "checkAll(this);" />
                                        </HeaderTemplate> 
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk" runat="server" onclick = "Check_Click(this)"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="250px" DataField="businessname" HeaderText="FO Name" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbStartImportCL" runat="server" CssClass="btn btn-default" OnClientClick="return confirmationImport();" ><asp:Image ID="Image2" runat="server" ImageUrl="../assets/images/icon/import-26.png" Width="15" /> Start Import</asp:LinkButton>
                    <asp:LinkButton ID="lbStartImportCC" runat="server" CssClass="btn btn-default" OnClientClick="return confirmationImport();" ><asp:Image ID="Image3" runat="server" ImageUrl="../assets/images/icon/import-26.png" Width="15" /> Start Import</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Add Transaction Type modal end-->
    <!-- Option Modal-->
    <div id="formDatePost" tabindex="-1" role="dialog" aria-labelledby="login-modalLabel" aria-hidden="true" class="modal fade">
        <div role="document" class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header" style="background-color:Transparent;">
                    <asp:LinkButton ID="lbAbortDatePost" runat="server" CssClass="close" aria-label="Close"><span aria-hidden="true">&times;</span></asp:LinkButton>
                    <h4 id="H3" class="modal-title">Date Posting </h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">Hotel Name:</label>
                                <asp:DropDownList ID="dlFOLinkDatePosting" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dlFOLinkDatePosting_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group" style="margin-bottom:3px;">
                                <label for="usr">For Month:</label>
                                <div class="input-group date monthGlPost" style="padding:0;">
                                    <asp:TextBox ID="tbDatePost" runat="server" CssClass="form-control" Enabled="false" OnTextChanged="cariPost" AutoPostBack="true"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                                </div>
                            </div>
                            <asp:GridView EmptyDataText="No records has been added." ID="gvDatePost" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10">
                                <Columns>
                                    <asp:BoundField DataField="numb" HeaderText="Date" ItemStyle-Width="100px"/>
                                    <asp:TemplateField HeaderText="City Ledger">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesc" runat="server" Text='<%# if(Eval("DataCL").toString()>"0",if(Eval("PostCL").toString()="","NOT YET","IMPORTED"),"NO DATA") %>' CssClass='<%# if(Eval("DataCL").toString()>"0",if(Eval("PostCL").toString()="","textgreen","textRed"),"textBlack") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Credit Card">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesc" runat="server" Text='<%# if(Eval("DataCC").toString()>"0",if(Eval("PostCC").toString()="","NOT YET","IMPORTED"),"NO DATA") %>' CssClass='<%# if(Eval("DataCC").toString()>"0",if(Eval("PostCC").toString()="","textgreen","textRed"),"textBlack") %>' ></asp:Label>
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
    </div>
    <!-- Option modal end-->
    <div class="row">
        <div class="col-lg-8">
            <div class="btn-group">
                <button type="button" style="width:130px;" class="btn btn-default dropdown-toggle hidden" data-toggle="dropdown">
                <asp:Image ID="Image1" runat="server" ImageUrl="../assets/images/icon/import-26.png" Width="15" /> Import <span class="caret"></span></button>
                <ul class="dropdown-menu" role="menu">
                  <li><asp:LinkButton ID="lbImportCL" runat="server" CssClass="btn"> City Ledger</asp:LinkButton></li>
                  <li><asp:LinkButton ID="lbImportCC" runat="server" CssClass="btn"> Credit Card</asp:LinkButton></li>
                </ul>
            </div>
            <asp:LinkButton ID="lbAddAR" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-plus"></i> New Transaction</asp:LinkButton>
            <asp:LinkButton ID="lbQuery" Width="150px" runat="server" CssClass="btn btn-default"><i class="fa fa-filter"></i> Query</asp:LinkButton>
            <div class="btn-group">
            <button type="button" style="width:150px;" class="btn btn-default dropdown-toggle hidden" data-toggle="dropdown">
            <i class="fa fa-cogs"></i> Option <span class="caret"></span></button>
            <ul class="dropdown-menu" role="menu">
                <li><asp:LinkButton ID="lbOption" runat="server" CssClass="btn" style="text-align:left;">Date Posting</asp:LinkButton></li>
                <li><%--<asp:LinkButton ID="lbDetail" runat="server" CssClass="btn" style="text-align:left;">Detail Transaction</asp:LinkButton>--%></li>
            </ul>
        </div>
            <hr style="margin-top:5px; margin-bottom:5px;" />
        </div>
        <div class="col-lg-2" style="text-align:right; padding-right:0;">
        <br style="line-height: 7px;"/>
        For Periode :
        </div>
        <div class="col-lg-2">
            <div class="form-group" style="margin-bottom:3px;">
                <div class="input-group date monthGl" style="padding:0;">
                    <asp:TextBox ID="tbDate" runat="server" CssClass="form-control" Enabled="false" OnTextChanged="TransdateWork" AutoPostBack="true"></asp:TextBox>
                    <span class="input-group-addon"><i class="fa fa-calendar" style="font-size:20px;"></i></span>
                </div>
            </div>
        </div>
        <div class="col-lg-12">
            <asp:GridView EmptyDataText="No records has been added." ID="gvARTrans" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None" AllowPaging="true" PageSize="10" ShowFooter="true">
                <Columns>
                    <asp:BoundField ItemStyle-Width="100px" DataField="TransID" HeaderText="ID" />
                    <asp:BoundField ItemStyle-Width="100px" DataField="TransDate" HeaderText="Transaction Date" DataFormatString="{0:dd MMM yyyy}"/>
                    <asp:BoundField ItemStyle-Width="200px" DataField="FOLINK" HeaderText="PMS Link" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="header-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lblAmountTot" runat="server" Text='<%# if(Eval("amountTrans").toString()="","0.00",Eval("amountTrans","{0:N2}")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField ItemStyle-Width="100px" DataField="RegDate" HeaderText="Reg Date" DataFormatString="{0:dd MMM yyyy}"/>--%>
                    <%--<asp:BoundField ItemStyle-Width="150px" DataField="fullname" HeaderText="by" />--%>
                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbEdit" CssClass="btn btn-default" runat="server" CommandName="getEdit" CommandArgument='<%# Eval("TransID") %>'><i class="fa fa-edit"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Post" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbPost" CssClass="btn btn-default" runat="server" CommandName="getPost" OnClientClick="return confirmationPost();" Enabled='<%# if(Eval("status").toString()="N",if(Eval("posting").toString()="Y","true","false"),"false") %>' CommandArgument='<%# Eval("TransID") %>'><i class="fa fa-send"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbDel" CssClass="btn btn-default" runat="server" CommandName="getDelete" Enabled='<%# if(Eval("status").toString()="N",if(Eval("totInv").toString()="0",if(Eval("deleteAR").toString()="Y","true","false"),"false"),"false") %>' CommandArgument='<%# Eval("TransID") %>'><i class="fa fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="50px" HeaderText="Print" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbPrint" CssClass="btn btn-default" runat="server" CommandName="getPrint" CommandArgument='<%# Eval("TransID") %>'><i class="fa fa-print"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <pagerstyle cssclass="pagination-ys">
                </pagerstyle>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
</asp:Content>

