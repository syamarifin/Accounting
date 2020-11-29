Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Imports System
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Collections
Imports System.Text
Imports System.Net.Mail
Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Partial Class iPxAdmin_iPxARInvoice
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL As String
    Dim cIpx As New iPxClass
    Sub UserAcces()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='6'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as AddInv, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='37'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as ApproveInv "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("AddInv").ToString = "Y" Then
                lbAddInv.Enabled = True
            Else
                lbAddInv.Enabled = False
            End If
            If oSQLReader.Item("ApproveInv").ToString = "Y" Then
                lbAppr.Enabled = True
                lbUndo.Enabled = True
            Else
                lbAppr.Enabled = False
                lbUndo.Enabled = False
            End If
        Else
            lbAddInv.Enabled = False
            lbAppr.Enabled = False
            lbUndo.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub showdata_dropdownCustStatus()
        dlQStatus.Items.Insert(0, "")
        dlQStatus.Items.Insert(1, "NEW")
        dlQStatus.Items.Insert(2, "DELETE")
    End Sub
    Sub ListInvoice()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAR_Invoice.*, iPxAcctAR_Cfg_Customer.CoyName, iPx_profile_user.fullname, "
        sSQL += "(select sum(c.amountdr) from iPxAcctAR_Transaction as c where c.invoiceno=iPxAcctAR_Invoice.InvoiceNo) as amountTrans, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=iPxAcctAR_Invoice.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='7' and x.active='Y') as editInv, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=iPxAcctAR_Invoice.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='8' and x.active='Y') as deleteInv, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=iPxAcctAR_Invoice.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='9' and x.active='Y') as sendInv "
        sSQL += "FROM iPxAcctAR_Invoice "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Customer ON iPxAcctAR_Invoice.businessid = iPxAcctAR_Cfg_Customer.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND "
        sSQL += "iPxAcctAR_Invoice.CustomerID COLLATE Latin1_General_CI_AS = iPxAcctAR_Cfg_Customer.CustomerID "
        sSQL += "INNER JOIN iPx_profile_user ON iPxAcctAR_Invoice.RegBy = iPx_profile_user.id "
        sSQL += "where iPxAcctAR_Invoice.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Invoice.InvoiceNo like 'IV%' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL += " AND iPxAcctAR_Invoice.Status='N'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by iPxAcctAR_Invoice.InvoiceNo asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvARInv.DataSource = dt
                    gvARInv.DataBind()
                    Dim totalAmount As Decimal
                    If dt.Compute("Sum(amountTrans)", "").ToString() <> "" Then
                        totalAmount = dt.Compute("Sum(amountTrans)", "").ToString()
                    Else
                        totalAmount = 0
                    End If
                    gvARInv.FooterRow.Cells(6).Text = "Total"
                    gvARInv.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Right
                    gvARInv.FooterRow.Cells(7).Text = totalAmount.ToString("N2")
                    gvARInv.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvARInv.DataSource = dt
                    gvARInv.DataBind()
                    gvARInv.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListInvoiceApproved()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAR_Invoice.*, iPxAcctAR_Cfg_Customer.CoyName, iPx_profile_user.fullname, "
        sSQL += "(select sum(c.amountdr) from iPxAcctAR_Transaction as c where c.invoiceno=iPxAcctAR_Invoice.InvoiceNo) as amountTrans, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=iPxAcctAR_Invoice.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='9' and x.active='Y') as sendInv "
        sSQL += " FROM iPxAcctAR_Invoice "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Customer ON iPxAcctAR_Invoice.businessid = iPxAcctAR_Cfg_Customer.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND "
        sSQL += "iPxAcctAR_Invoice.CustomerID COLLATE Latin1_General_CI_AS = iPxAcctAR_Cfg_Customer.CustomerID "
        sSQL += "INNER JOIN iPx_profile_user ON iPxAcctAR_Invoice.RegBy = iPx_profile_user.id "
        sSQL += "where iPxAcctAR_Invoice.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Invoice.InvoiceNo like 'IV%' AND iPxAcctAR_Invoice.Status='A' "
        If Session("sQueryAprove") = "" Then
            Session("sQueryAprove") = Session("sCondition")
            If Session("sQueryAprove") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryAprove")
                Session("sCondition") = ""
            Else
                sSQL += " "
            End If
        Else
            sSQL = sSQL & Session("sQueryAprove")
            Session("sCondition") = ""
        End If
        sSQL += " order by iPxAcctAR_Invoice.InvoiceNo asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvApprove.DataSource = dt
                    gvApprove.DataBind()
                    Dim totalAmount As Decimal
                    If dt.Compute("Sum(amountTrans)", "").ToString() <> "" Then
                        totalAmount = dt.Compute("Sum(amountTrans)", "").ToString()
                    Else
                        totalAmount = 0
                    End If
                    gvApprove.FooterRow.Cells(6).Text = "Total"
                    gvApprove.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Right
                    gvApprove.FooterRow.Cells(7).Text = totalAmount.ToString("N2")
                    gvApprove.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvApprove.DataSource = dt
                    gvApprove.DataBind()
                    gvApprove.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListInvoicePaid()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAR_Invoice.*, iPxAcctAR_Cfg_Customer.CoyName, iPx_profile_user.fullname, "
        sSQL += "(select sum(c.amountdr) from iPxAcctAR_Transaction as c where c.invoiceno=iPxAcctAR_Invoice.InvoiceNo) as amountTrans FROM iPxAcctAR_Invoice "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Customer ON iPxAcctAR_Invoice.businessid = iPxAcctAR_Cfg_Customer.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND "
        sSQL += "iPxAcctAR_Invoice.CustomerID COLLATE Latin1_General_CI_AS = iPxAcctAR_Cfg_Customer.CustomerID "
        sSQL += "INNER JOIN iPx_profile_user ON iPxAcctAR_Invoice.RegBy = iPx_profile_user.id "
        sSQL += "where iPxAcctAR_Invoice.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Invoice.InvoiceNo like 'IV%' AND iPxAcctAR_Invoice.Status='P' "
        If Session("sQueryPaid") = "" Then
            Session("sQueryPaid") = Session("sCondition")
            If Session("sQueryPaid") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryPaid")
                Session("sCondition") = ""
            Else
                sSQL += " "
            End If
        Else
            sSQL = sSQL & Session("sQueryPaid")
            Session("sCondition") = ""
        End If
        sSQL += " order by iPxAcctAR_Invoice.InvoiceNo asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvPaid.DataSource = dt
                    gvPaid.DataBind()
                    Dim totalAmount As Decimal
                    If dt.Compute("Sum(amountTrans)", "").ToString() <> "" Then
                        totalAmount = dt.Compute("Sum(amountTrans)", "").ToString()
                    Else
                        totalAmount = 0
                    End If
                    gvPaid.FooterRow.Cells(5).Text = "Total"
                    gvPaid.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                    gvPaid.FooterRow.Cells(6).Text = totalAmount.ToString("N2")
                    gvPaid.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Right
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvPaid.DataSource = dt
                    gvPaid.DataBind()
                    gvPaid.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    'Select All
    Private Sub SetData()
        Dim currentCount As Integer = 0
        Dim chkAll As CheckBox = DirectCast(gvARInv.HeaderRow _
                        .Cells(0).FindControl("chkAll"), CheckBox)
        chkAll.Checked = True
        Dim arr As ArrayList = DirectCast(ViewState("SelectedRecords"), ArrayList)
        For i As Integer = 0 To gvARInv.Rows.Count - 1
            Dim chk As CheckBox = DirectCast(gvARInv.Rows(i).Cells(0) _
                                            .FindControl("chk"), CheckBox)
            If chk IsNot Nothing Then
                chk.Checked = arr.Contains(gvARInv.DataKeys(i).Value)
                If Not chk.Checked Then
                    chkAll.Checked = False
                Else
                    currentCount += 1
                End If
            End If
        Next
        hfCount.Value = (arr.Count - currentCount).ToString()
    End Sub
    Private Sub ApprovedRecord(ByVal InvoiceNO As String)
        Dim constr As String = ConfigurationManager _
                        .ConnectionStrings("iPxCNCT").ConnectionString
        Dim query As String = "UPDATE iPxAcctAR_Invoice SET Status='A' where" & _
                                " businessid = '" & Session("sBusinessID") & "' and InvoiceNo=@InvoiceNO"
        Dim con As New SqlConnection(constr)
        Dim cmd As New SqlCommand(query, con)
        cmd.Parameters.AddWithValue("@InvoiceNO", InvoiceNO)
        con.Open()
        cmd.ExecuteNonQuery()
        con.Close()
    End Sub
    Private Sub UndoApprovedRecord(ByVal InvoiceNO As String)
        Dim constr As String = ConfigurationManager _
                        .ConnectionStrings("iPxCNCT").ConnectionString
        Dim query As String = "UPDATE iPxAcctAR_Invoice SET Status='N' where" & _
                                " businessid = '" & Session("sBusinessID") & "' and InvoiceNo=@InvoiceNO"
        Dim con As New SqlConnection(constr)
        Dim cmd As New SqlCommand(query, con)
        cmd.Parameters.AddWithValue("@InvoiceNO", InvoiceNO)
        con.Open()
        cmd.ExecuteNonQuery()
        con.Close()
    End Sub
    'End Select All
    Sub listTrans()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAR_Transaction.RecID, iPxAcctAR_Transaction.TransDate, iPxAcctAR_Cfg_Customer.CoyName, iPxAcctAR_Cfg_Transactiontype.Description, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.voucherno, iPxAcctAR_Transaction.foliono, "
        sSQL += "iPxAcctAR_Transaction.GuestName, iPxAcctAR_Transaction.notes, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.arrival, iPxAcctAR_Transaction.departure, "
        sSQL += "iPxAcctAR_Transaction.amountdr FROM iPxAcctAR_Transaction "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Customer ON iPxAcctAR_Transaction.businessid = iPxAcctAR_Cfg_Customer.businessid AND "
        sSQL += "iPxAcctAR_Transaction.CustomerID = iPxAcctAR_Cfg_Customer.CustomerID "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Transactiontype ON iPxAcctAR_Transaction.businessid COLLATE SQL_Latin1_General_CP1_CI_AS = iPxAcctAR_Cfg_Transactiontype.businessid AND "
        sSQL += "iPxAcctAR_Transaction.transactiontype = iPxAcctAR_Cfg_Transactiontype.TransactionType "
        sSQL += "WHERE iPxAcctAR_Transaction.businessid = '" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno='" & Session("sEditInv") & "' and iPxAcctAR_Transaction.isActive='Y'"
        sSQL += " order by iPxAcctAR_Cfg_Customer.CoyName asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvInvoice.DataSource = dt
                    gvInvoice.DataBind()
                    'Calculate Sum and display in Footer Row
                    Dim total As Decimal = dt.Compute("Sum(amountdr)", "").ToString()
                    gvInvoice.FooterRow.Cells(9).Text = "Total"
                    gvInvoice.FooterRow.Cells(9).HorizontalAlign = HorizontalAlign.Left
                    gvInvoice.FooterRow.Cells(10).Text = total.ToString("N2")
                    gvInvoice.FooterRow.Cells(10).HorizontalAlign = HorizontalAlign.Right
                    gvInvoice.FooterRow.Cells(11).Visible = False
                    gvInvoice.Enabled = True
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvInvoice.DataSource = dt
                    gvInvoice.DataBind()
                    gvInvoice.Enabled = False
                    gvInvoice.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub tampilFO()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Transaction "
        sSQL += "WHERE invoiceno = '" & Session("sTransID") & "' AND businessid='" & Session("sBusinessID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Session("sFoLink") = oSQLReader.Item("FoLink").ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub EmailCustomer()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.InvoiceNo, a.CustomerID, b.CoyName, b.Email, b.ContPerson, "
        sSQL += "(select sum(amountdr) from iPxAcctAR_Transaction as x "
        sSQL += "WHERE x.invoiceno = a.InvoiceNo AND x.businessid=a.businessid) as amount  "
        sSQL += "from iPxAcctAR_Invoice as a "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Customer as b ON b.CustomerID=a.CustomerID and b.businessid=a.businessid "
        sSQL += "WHERE a.invoiceno = '" & Session("sSendInv") & "' AND a.businessid='" & Session("sBusinessID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbEmailName.Text = oSQLReader.Item("CoyName").ToString
            tbEmailInv.Text = oSQLReader.Item("Email").ToString
            Session("sContPerson") = oSQLReader.Item("ContPerson").ToString
            Session("sAmountEmail") = oSQLReader.Item("amount").ToString
            oCnct.Close()
            'If Session("sEmailTo") = "" Then
            '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Customer email has not been filled!');", True)
            'ElseIf Session("sEmailTo") <> "" Then
            '    SendEmail()
            'End If
        Else
            oCnct.Close()
        End If
    End Sub
    Protected Function SendEmail() As Boolean
        Try
            Dim aRet() As String
            Dim host, enablessl, username, password, port, active As String

            aRet = Split(cIpx.GetSMTP(Session("sBusinessID")), "|") 'parameter dan pemisah
            active = aRet(0)
            If active = "Y" Then
                host = aRet(1)
                enablessl = aRet(2)
                username = aRet(3)
                password = aRet(4)
                port = aRet(5)
            Else
                host = ConfigurationManager.AppSettings("Host")
                enablessl = ConfigurationManager.AppSettings("EnableSsl")
                username = ConfigurationManager.AppSettings("UserName")
                password = ConfigurationManager.AppSettings("Password")
                port = ConfigurationManager.AppSettings("Port")
            End If

            Using mm As New MailMessage(username, tbEmailInv.Text)
                Dim emailbcc As New MailAddress(ConfigurationManager.AppSettings("from"))
                mm.Bcc.Add(emailbcc)
                mm.Subject = "Invoice notification"
                Dim body As String


                'body = "<div class=""center""> <img alt="" src=""../images/logo/alcor-logo.png"" />"
                body = "<br /><br />"
                body += "Kepada <br />" + tbEmailName.Text + "<br />" + Session("sContPerson") + ",<br /><br />"

                body += "Dengan hormat<br />"

                body += "<br />Berikut kami kirimkan tagihan Bapak/Ibu dengan nomer Invoice : " + Session("sSendInv")
                body += "<br />Sejumlah " + Session("sAmountEmail") + "<br />"
                body += "<br />Atas perhatian dan kerjasamanya kami ucapkan terimakasih. <br />"
                body += "<br />Hormat kami, <br />"
                body += "<br />Account Receivable "
                body += "<br />" + Session("sBussinessName")
                body += "<br />" + Session("sBussinessAddress")
                body += "<br />" + Session("sBussinessPhone") + " " + Session("sEmailBussiness")
                mm.Body = body
                mm.IsBodyHtml = True

                '=====================================================
                Dim smtp As New SmtpClient()
                smtp.Host = host
                smtp.EnableSsl = Convert.ToBoolean(enablessl)
                Dim NetworkCred As New NetworkCredential(username, password)
                smtp.UseDefaultCredentials = True
                smtp.Credentials = NetworkCred
                smtp.Port = Integer.Parse(port)
                ServicePointManager.ServerCertificateValidationCallback = Function(s As Object, certificate As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) True

                smtp.Send(mm)
            End Using
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Email can't send!');", True)

        End Try
    End Function
    Sub Docket()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select * from iPxAcctDocket_Report WHERE businessid in ('','" & Session("sBusinessID") & "') and Grp='IV'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDocket.DataSource = dt
                    gvDocket.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvDocket.DataSource = dt
                    gvDocket.DataBind()
                    gvDocket.Enabled = False
                    gvDocket.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Me.IsPostBack Then
            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "AR Invoice") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            Session("sQueryTicket") = ""
            Session("sQueryAprove") = ""
            ListInvoice()
            ListInvoiceApproved()
            ListInvoicePaid()
        End If
        UserAcces()
        'enable select All
        Dim arr As ArrayList
        If ViewState("SelectedRecords") IsNot Nothing Then
            arr = DirectCast(ViewState("SelectedRecords"), ArrayList)
        Else
            arr = New ArrayList()
        End If
        Dim enabled As Integer
        Dim chkAll As CheckBox = DirectCast(gvARInv.HeaderRow _
                    .Cells(0).FindControl("chkAll"), CheckBox)
        For i As Integer = 0 To gvARInv.Rows.Count - 1
            Dim chk As CheckBox = DirectCast(gvARInv.Rows(i).Cells(0) _
                                        .FindControl("chk"), CheckBox)
            If chk.Enabled = False Then
                enabled = enabled + 1
            Else

            End If
        Next
        If enabled = gvARInv.Rows.Count Then
            chkAll.Enabled = False
        End If
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvARInv.PageIndex = e.NewPageIndex
        ListInvoice()
        SetData()
        'enable select All
        Dim arr As ArrayList
        If ViewState("SelectedRecords") IsNot Nothing Then
            arr = DirectCast(ViewState("SelectedRecords"), ArrayList)
        Else
            arr = New ArrayList()
        End If
        Dim chkAll As CheckBox = DirectCast(gvARInv.HeaderRow _
                    .Cells(0).FindControl("chkAll"), CheckBox)
        For i As Integer = 0 To gvARInv.Rows.Count - 1
            Dim chk As CheckBox = DirectCast(gvARInv.Rows(i).Cells(0) _
                                        .FindControl("chk"), CheckBox)
            If chk.Enabled = False Then
                chk.Checked = False
            Else

            End If
        Next
    End Sub

    Protected Sub lbAddInv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddInv.Click
        Session("sEditInv") = ""
        Response.Redirect("iPxInputARInvoice.aspx")
    End Sub

    Protected Sub gvARInv_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvARInv.RowCommand
        If e.CommandName = "getEdit" Then
            Session("sEditInv") = e.CommandArgument
            Response.Redirect("iPxInputARInvoiceEdit.aspx")
        ElseIf e.CommandName = "getDetail" Then
            Session("sEditInv") = e.CommandArgument
            lbIdInv.Text = e.CommandArgument
            listTrans()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        ElseIf e.CommandName = "getPrint" Then
            Session("sTransID") = e.CommandArgument
            Docket()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDocket", "showModalDocket()", True)
        ElseIf e.CommandName = "getDelete" Then
            Session("sDeleteInv") = e.CommandArgument
            tbReason.Text = ""
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
            
        ElseIf e.CommandName = "getEmail" Then
            Session("sSendInv") = e.CommandArgument
            EmailCustomer()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalEmailverived", "showModalEmailverived()", True)
        End If
    End Sub

    Protected Sub lbQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQuery.Click
        Session("sQuery") = "New"
        tbQCustName.Text = ""
        tbQDueDate.Text = ""
        tbQInvDate.Text = ""
        tbQInvID.Text = ""
        tbQReff.Text = ""
        tbQFrom.Text = ""
        tbQUntil.Text = ""
        Session("sQueryTicket") = ""
        dlQStatus.Visible = True
        lblStatus.Visible = True
        dlQStatus.Items.Clear()
        showdata_dropdownCustStatus()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub lbAbortQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortQuery.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        If Session("sQuery") = "Approved" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ApproveActive", "ApproveActive()", True)
        ElseIf Session("sQuery") = "Paid" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PaidActive", "PaidActive()", True)
        End If
    End Sub

    Protected Sub lblQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblQuery.Click
        If Session("sQuery") = "New" Then
            If dlQStatus.SelectedIndex = 0 Then
                Session("sCondition") = Session("sCondition") & " and iPxAcctAR_Invoice.Status='N' "
            ElseIf dlQStatus.SelectedIndex = 1 Then
                Session("sCondition") = Session("sCondition") & " and iPxAcctAR_Invoice.Status='N' "
            ElseIf dlQStatus.SelectedIndex = 2 Then
                Session("sCondition") = Session("sCondition") & " and iPxAcctAR_Invoice.Status='X'"
            End If
        End If
        If tbQReff.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (iPxAcctAR_Invoice.ReffNo like '%" & Replace(tbQReff.Text, "'", "''") & "%') "
        End If
        If tbQCustName.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (iPxAcctAR_Cfg_Customer.CoyName like '%" & Replace(tbQCustName.Text, "'", "''") & "%') "
        End If
        If tbQInvID.Text <> "" Then
            Session("sCondition") = Session("sCondition") & " and (iPxAcctAR_Invoice.InvoiceNo like '%" & Replace(tbQInvID.Text, "'", "''") & "%') "
        End If
        If tbQDueDate.Text <> "" Then
            Dim Duedate As Date
            Duedate = Date.ParseExact(tbQDueDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " and (iPxAcctAR_Invoice.DueDate = '" & Duedate & "') "
        End If
        If tbQInvDate.Text <> "" Then
            Dim Invdate As Date
            Invdate = Date.ParseExact(tbQInvDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " and (iPxAcctAR_Invoice.InvDate = '" & Invdate & "') "
        End If
        If tbQFrom.Text.Trim <> "" And tbQUntil.Text.Trim <> "" Then
            Dim PerFrom As Date = Date.ParseExact(tbQFrom.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Dim PerUntl As Date = Date.ParseExact(tbQUntil.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (iPxAcctAR_Invoice.InvDate >= '" & PerFrom & "') AND (iPxAcctAR_Invoice.InvDate <= '" & PerUntl & "') "
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        If Session("sQuery") = "Approved" Then
            ListInvoiceApproved()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ApproveActive", "ApproveActive()", True)
        ElseIf Session("sQuery") = "Paid" Then
            ListInvoicePaid()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PaidActive", "PaidActive()", True)
        Else
            ListInvoice()
        End If
    End Sub

    Protected Sub lbAppr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAppr.Click
        Dim cb2 As CheckBox = gvARInv.HeaderRow.FindControl("chkAll")
        Dim cbterpilih As Boolean = False
        Dim cb As CheckBox = Nothing
        Dim n As Integer = 0
        Dim m As Integer = 0
        Do Until n = gvARInv.Rows.Count
            cb = gvARInv.Rows.Item(n).FindControl("chk")
            If cb IsNot Nothing AndAlso cb.Checked Then
                cbterpilih = True
                'insert & update
                ApprovedRecord(gvARInv.Rows(n).Cells(1).Text)
                Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
                cIpx.saveLog(Session("sBusinessID"), gvARInv.Rows(n).Cells(1).Text, TransDateLog, "AR INV", "Approve", "Approved AR Invoice from invoice " & gvARInv.Rows(n).Cells(1).Text, "", Session("sUserCode"))
                m = m + 1
            Else
                
            End If
            n += 1
        Loop
        ListInvoice()
        ListInvoiceApproved()
        ListInvoicePaid()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('" & m & " records approved!');", True)
    End Sub

    Protected Sub lbUndo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbUndo.Click
        Dim cb2 As CheckBox = gvApprove.HeaderRow.FindControl("chkAll")
        Dim cbterpilih As Boolean = False
        Dim cb As CheckBox = Nothing
        Dim n As Integer = 0
        Dim m As Integer = 0
        Do Until n = gvApprove.Rows.Count
            cb = gvApprove.Rows.Item(n).FindControl("chk")
            If cb IsNot Nothing AndAlso cb.Checked Then
                cbterpilih = True
                'insert & update
                UndoApprovedRecord(gvApprove.Rows(n).Cells(1).Text)
                Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
                cIpx.saveLog(Session("sBusinessID"), gvApprove.Rows(n).Cells(1).Text, TransDateLog, "AR INV", "Approve", "Undo Approved AR Invoice from invoice " & gvApprove.Rows(n).Cells(1).Text, "", Session("sUserCode"))
                m = m + 1
            Else

            End If
            n += 1
        Loop
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ApproveActive", "ApproveActive()", True)
        ListInvoiceApproved()
        ListInvoice()
        ListInvoicePaid()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('" & m & " records undo approved!');", True)
    End Sub

    Protected Sub gvApprove_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvApprove.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sEditInv") = e.CommandArgument
            lbIdInv.Text = e.CommandArgument
            listTrans()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ApproveActive", "ApproveActive()", True)
        ElseIf e.CommandName = "getPrint" Then
            'Dim cIpx As New iPxClass
            'Session("sReport") = "ARInvoice"
            Session("Tab") = "Approve"
            Session("sTransID") = e.CommandArgument
            Docket()
            'tampilFO()
            ''Session("filename") = "dckCashierSummary_Form.rpt"
            'Session("sMapPath") = "~/iPxReportFile/dckAR_Invoice.rpt"
            ''Session("sFOLink") = txtP2.Text
            'Response.Redirect("rptviewer.aspx")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDocket", "showModalDocket()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ApproveActive", "ApproveActive()", True)
        ElseIf e.CommandName = "getEmail" Then
            Session("sSendInv") = e.CommandArgument
            EmailCustomer()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalEmailverived", "showModalEmailverived()", True)
        End If
    End Sub

    Protected Sub gvPaid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPaid.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sEditInv") = e.CommandArgument
            lbIdInv.Text = e.CommandArgument
            listTrans()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PaidActive", "PaidActive()", True)
        ElseIf e.CommandName = "getPrint" Then
            Session("Tab") = "Paid"
            'Dim cIpx As New iPxClass
            'Session("sReport") = "ARInvoice"
            Session("sTransID") = e.CommandArgument
            Docket()
            'tampilFO()
            'Session("sMapPath") = "~/iPxReportFile/dckAR_Invoice.rpt"
            'Response.Redirect("rptviewer.aspx")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDocket", "showModalDocket()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PaidActive", "PaidActive()", True)
        End If
    End Sub

    Protected Sub lbQueryApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQueryApprove.Click
        Session("sQuery") = "Approved"
        tbQCustName.Text = ""
        tbQDueDate.Text = ""
        tbQInvDate.Text = ""
        tbQInvID.Text = ""
        tbQReff.Text = ""
        tbQFrom.Text = ""
        tbQUntil.Text = ""
        Session("sQueryAprove") = ""
        dlQStatus.Visible = False
        lblStatus.Visible = False
        'dlQStatus.Items.Clear()
        'showdata_dropdownCustStatus()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ApproveActive", "ApproveActive()", True)
    End Sub

    Protected Sub lbQueryPaid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQueryPaid.Click
        Session("sQuery") = "Paid"
        tbQCustName.Text = ""
        tbQDueDate.Text = ""
        tbQInvDate.Text = ""
        tbQInvID.Text = ""
        tbQReff.Text = ""
        tbQFrom.Text = ""
        tbQUntil.Text = ""
        Session("sQueryPaid") = ""
        dlQStatus.Visible = False
        lblStatus.Visible = False
        'dlQStatus.Items.Clear()
        'showdata_dropdownCustStatus()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PaidActive", "PaidActive()", True)
    End Sub

    Protected Sub lbAbortVerived_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortVerived.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalEmailverived", "hideModalEmailverived()", True)
    End Sub

    Protected Sub lbSendEmail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSendEmail.Click
        If tbEmailInv.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Customer email has not been filled!');", True)
        ElseIf tbEmailName.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Customer name has not been filled!');", True)
        ElseIf tbEmailInv.Text <> "" And tbEmailName.Text <> "" Then
            SendEmail()
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalEmailverived", "hideModalEmailverived()", True)
    End Sub

    Protected Sub lbDeleteRes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbDeleteRes.Click
        Dim delete As String = Session("sDeleteInv")
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAR_Cfg_Customer.CoyName FROM iPxAcctAR_Transaction "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Customer ON iPxAcctAR_Transaction.businessid = iPxAcctAR_Cfg_Customer.businessid AND "
        sSQL += "iPxAcctAR_Transaction.CustomerID = iPxAcctAR_Cfg_Customer.CustomerID "
        sSQL += "WHERE iPxAcctAR_Transaction.businessid = '" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno='" & delete & "' and iPxAcctAR_Transaction.isActive='Y' and iPxAcctAR_Transaction.CustomerID='" & Session("sCustInv") & "'"
        sSQL += " order by iPxAcctAR_Cfg_Customer.CoyName asc"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Session("sCoyName") = oSQLReader.Item("CoyName").ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAR_Invoice SET status='X'"
        sSQL = sSQL & "WHERE InvoiceNo ='" & delete & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAR_Transaction SET invoiceno=''"
        sSQL = sSQL & "WHERE invoiceno ='" & delete & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), delete, TransDateLog, "AR INV", "Delete", "Delete AR Invoice from " & Session("sCoyName"), "", Session("sUserCode"))
        ListInvoice()
        ListInvoiceApproved()
        ListInvoicePaid()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
    End Sub

    Protected Sub lbAbortDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDelete.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
    End Sub

    Protected Sub lbAbortDocket_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDocket.Click
        If Session("Tab") = "Approve" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ApproveActive", "ApproveActive()", True)
        ElseIf Session("Tab") = "Paid" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PaidActive", "PaidActive()", True)
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDocket", "hideModalDocket()", True)
    End Sub

    Protected Sub gvDocket_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDocket.RowCommand
        If e.CommandName = "getDocket" Then
            Dim cIpx As New iPxClass
            Session("sReport") = "ARInvoice"
            tampilFO()
            'Session("filename") = "dckCashierSummary_Form.rpt"
            Session("sMapPath") = "~/iPxReportFile/" + e.CommandArgument
            'Session("sFOLink") = txtP2.Text
            Response.Redirect("rptviewer.aspx")
        End If
    End Sub
End Class
