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
Partial Class iPxAdmin_iPxARCreditCard
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, statusinvoice, RecID, MonthCekClosing, YearCekClosing, coaDebit, coaCredit, komisi, coaKomisi As String
    Dim oCnct1 As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd1 As SqlCommand
    Dim oSQLReader1 As SqlDataReader
    Dim sSQL1 As String
    Dim oCnct2 As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd2 As SqlCommand
    Dim oSQLReader2 As SqlDataReader
    Dim sSQL2 As String
    Dim cIpx As New iPxClass
    Sub UserAccesBatch()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='14'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as Batch "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("Batch").ToString = "Y" Then
                lbCreateBath.Enabled = True
            Else
                lbCreateBath.Enabled = False
            End If
        Else
            lbCreateBath.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub UserAccesClear()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='17'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as Clear "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("Clear").ToString = "Y" Then
                lbCreateClear.Enabled = True
            Else
                lbCreateClear.Enabled = False
            End If
        Else
            lbCreateClear.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub UserAccesPosting()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='19'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as posting "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("posting").ToString = "Y" Then
                lbPosting.Enabled = True
            Else
                lbPosting.Enabled = False
            End If
        Else
            lbPosting.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub listCustomerOpen()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.CustomerID,a.CoyName, b.Description FROM iPxAcctAR_Cfg_Customer as a "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Paidby as b ON b.businessid=a.businessid and b.PaidBy=a.DefaultPaid where a.businessid ='" & Session("sBusinessID") & "' AND a.arGroup='CC' AND a.CustomerID = "
        sSQL += "(select CustomerID from iPxAcctAR_Transaction where iPxAcctAR_Transaction.CustomerID = a.CustomerID and invoiceno='' and iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.IsActive ='Y' group by CustomerID) AND a.IsActive='" & "Y" & "'"
        If Session("sQueryTicketOpen") = "" Then
            Session("sQueryTicketOpen") = Session("sConditionOpen")
            If Session("sQueryTicketOpen") <> "" Or Session("sConditionOpen") <> "" Then
                sSQL = sSQL & Session("sQueryTicketOpen")
                Session("sConditionOpen") = ""
            Else
                sSQL = sSQL & ""
            End If
        Else
            sSQL = sSQL & Session("sQueryTicketOpen")
            Session("sConditionOpen") = ""
        End If
        sSQL += " order by a.CustomerID asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCustAR.DataSource = dt
                    gvCustAR.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCustAR.DataSource = dt
                    gvCustAR.DataBind()
                    gvCustAR.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listCustomerBath()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.CustomerID,a.CoyName, b.Description FROM iPxAcctAR_Cfg_Customer as a "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Paidby as b ON b.businessid=a.businessid and b.PaidBy=a.DefaultPaid "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' AND a.arGroup='CC' AND a.CustomerID = "
        sSQL += "(select iPxAcctAR_Transaction.CustomerID from iPxAcctAR_Transaction inner join iPxAcctAR_Invoice on iPxAcctAR_Invoice.InvoiceNo = iPxAcctAR_Transaction.invoiceno "
        sSQL += "where iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.invoiceno like 'BC%' and iPxAcctAR_Invoice.Status ='N' and iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.IsActive ='Y' group by iPxAcctAR_Transaction.CustomerID) "
        sSQL += "AND (select (sum(amountdr)-sum(amountcr)) from iPxAcctAR_Transaction where iPxAcctAR_Transaction.invoiceno<>'' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y')>0 AND a.IsActive='" & "Y" & "'"
        If Session("sQueryTicketBath") = "" Then
            Session("sQueryTicketBath") = Session("sConditionBath")
            If Session("sQueryTicketBath") <> "" Or Session("sConditionBath") <> "" Then
                sSQL = sSQL & Session("sQueryTicketBath")
                Session("sConditionBath") = ""
            Else
                sSQL = sSQL & ""
            End If
        Else
            sSQL = sSQL & Session("sQueryTicketBath")
            Session("sConditionBath") = ""
        End If
        sSQL += " order by a.CustomerID asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCustBath.DataSource = dt
                    gvCustBath.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCustBath.DataSource = dt
                    gvCustBath.DataBind()
                    gvCustBath.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listCustomerClear()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.CustomerID,a.CoyName, b.Description FROM iPxAcctAR_Cfg_Customer as a INNER JOIN iPxAcctAR_Cfg_Paidby as b ON b.businessid=a.businessid and b.PaidBy=a.DefaultPaid "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' AND a.arGroup='CC' AND a.IsActive='Y' "
        sSQL += "AND a.CustomerID = (select customerID from iPxAcctAR_Receipt where Status = 'N' AND CustomerID = a.CustomerID COLLATE Latin1_General_CI_AS group by CustomerID) COLLATE Latin1_General_CI_AS "
        If Session("sQueryTicketClear") = "" Then
            Session("sQueryTicketClear") = Session("sConditionClear")
            If Session("sQueryTicketClear") <> "" Or Session("sConditionClear") <> "" Then
                sSQL = sSQL & Session("sQueryTicketClear")
                Session("sConditionClear") = ""
            Else
                sSQL = sSQL & ""
            End If
        Else
            sSQL = sSQL & Session("sQueryTicketClear")
            Session("sConditionClear") = ""
        End If
        sSQL += " order by a.CustomerID asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCustClear.DataSource = dt
                    gvCustClear.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCustClear.DataSource = dt
                    gvCustClear.DataBind()
                    gvCustClear.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listCustomerPosted()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.CustomerID,a.CoyName, b.Description FROM iPxAcctAR_Cfg_Customer as a INNER JOIN iPxAcctAR_Cfg_Paidby as b ON b.businessid=a.businessid and b.PaidBy=a.DefaultPaid "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' AND a.arGroup='CC' AND a.IsActive='Y' "
        sSQL += "AND a.CustomerID = (select customerID from iPxAcctAR_Receipt where Status = 'P' AND CustomerID = a.CustomerID COLLATE Latin1_General_CI_AS group by CustomerID) COLLATE Latin1_General_CI_AS "
        If Session("sQueryTicketClear") = "" Then
            Session("sSelectCustPosted") = Session("sConditionPosted")
            If Session("sSelectCustPosted") <> "" Or Session("sConditionPosted") <> "" Then
                sSQL = sSQL & Session("sSelectCustPosted")
                Session("sConditionPosted") = ""
            Else
                sSQL = sSQL & ""
            End If
        Else
            sSQL = sSQL & Session("sSelectCustPosted")
            Session("sConditionPosted") = ""
        End If
        sSQL += " order by a.CustomerID asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCustPosted.DataSource = dt
                    gvCustPosted.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCustPosted.DataSource = dt
                    gvCustPosted.DataBind()
                    gvCustPosted.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listCardOpen()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.RecID, iPxAcctAR_Transaction.TransDate, iPxAcctAR_Cfg_Transactiontype.Description, iPxAcctAR_Transaction.GuestName, "
        sSQL += "iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.arrival, iPxAcctAR_Transaction.departure, iPxAcctAR_Transaction.amountdr FROM iPxAcctAR_Transaction "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Transactiontype ON iPxAcctAR_Cfg_Transactiontype.TransactionType = iPxAcctAR_Transaction.transactiontype and iPxAcctAR_Cfg_Transactiontype.businessid = iPxAcctAR_Transaction.businessid "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Customer ON iPxAcctAR_Cfg_Customer.businessid=iPxAcctAR_Transaction.businessid AND iPxAcctAR_Cfg_Customer.CustomerID=iPxAcctAR_Transaction.CustomerID "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' AND iPxAcctAR_Transaction.CustomerID = '" & Session("sSelectCustOpen") & "' AND iPxAcctAR_Transaction.invoiceno ='' AND iPxAcctAR_Cfg_Customer.arGroup='CC' "
        sSQL += "AND iPxAcctAR_Transaction.isActive='Y' order by iPxAcctAR_Transaction.TransDate desc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvTransOpen.DataSource = dt
                    gvTransOpen.DataBind()
                    gvTransOpen.Enabled = True
                    lbCreateBath.Enabled = True
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvTransOpen.DataSource = dt
                    gvTransOpen.DataBind()
                    gvTransOpen.Rows(0).Visible = False
                    gvTransOpen.Enabled = False
                    lbCreateBath.Enabled = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listCardBath()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAR_Invoice.InvoiceNo, iPxAcctAR_Cfg_Customer.CoyName, iPxAcctAR_Invoice.ReffNo, iPxAcctAR_Invoice.InvDate, iPxAcctAR_Invoice.Notes, iPxAcctAR_Invoice.Status, "
        sSQL += "(select sum(amountdr) from iPxAcctAR_Transaction where invoiceno LIKE 'BC%' and iPxAcctAR_Transaction.invoiceno = iPxAcctAR_Invoice.InvoiceNo) as Debit, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='15'and x.active='Y' and x.businessid=iPxAcctAR_Invoice.businessid and x.usercode='" & Session("sUserCode") & "') as editBatch, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='16'and x.active='Y' and x.businessid=iPxAcctAR_Invoice.businessid and x.usercode='" & Session("sUserCode") & "') as deleteBatch "
        sSQL += "FROM iPxAcctAR_Invoice INNER JOIN iPxAcctAR_Cfg_Customer ON iPxAcctAR_Invoice.businessid = iPxAcctAR_Cfg_Customer.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND "
        sSQL += "iPxAcctAR_Invoice.CustomerID COLLATE Latin1_General_CI_AS = iPxAcctAR_Cfg_Customer.CustomerID "
        sSQL += "where iPxAcctAR_Invoice.businessid ='" & Session("sBusinessID") & "' AND iPxAcctAR_Invoice.CustomerID = '" & Session("sSelectCustBath") & "' AND iPxAcctAR_Invoice.invoiceno like 'BC%' "
        sSQL += "AND (select (sum(amountdr)-sum(amountcr)) from iPxAcctAR_Transaction where iPxAcctAR_Transaction.invoiceno = iPxAcctAR_Invoice.InvoiceNo and iPxAcctAR_Transaction.isActive='Y')>0 AND iPxAcctAR_Invoice.Status = 'N' "
        sSQL += "order by iPxAcctAR_Invoice.InvDate desc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvTransBath.DataSource = dt
                    gvTransBath.DataBind()
                    gvTransBath.Enabled = True
                    lbCreateClear.Enabled = True
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvTransBath.DataSource = dt
                    gvTransBath.DataBind()
                    gvTransBath.Rows(0).Visible = False
                    gvTransBath.Enabled = False
                    lbCreateClear.Enabled = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listDetailBath()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.RecID, iPxAcctAR_Transaction.TransDate, iPxAcctAR_Cfg_Transactiontype.Description, iPxAcctAR_Transaction.GuestName, "
        sSQL += "iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.arrival, iPxAcctAR_Transaction.departure, (iPxAcctAR_Transaction.amountdr) as amount FROM iPxAcctAR_Transaction "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Transactiontype ON iPxAcctAR_Cfg_Transactiontype.businessid = iPxAcctAR_Transaction.businessid and iPxAcctAR_Cfg_Transactiontype.TransactionType = iPxAcctAR_Transaction.transactiontype "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' AND iPxAcctAR_Transaction.invoiceno = '" & Session("sSelectCustBathD") & "' AND iPxAcctAR_Transaction.TransID like 'AR%' "
        sSQL += "order by iPxAcctAR_Transaction.TransDate desc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDetailBath.DataSource = dt
                    gvDetailBath.DataBind()
                    Dim total As Decimal = dt.Compute("Sum(amount)", "").ToString()
                    gvDetailBath.FooterRow.Cells(6).Text = "Total"
                    gvDetailBath.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Left
                    gvDetailBath.FooterRow.Cells(7).Text = total.ToString("N2")
                    gvDetailBath.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                    'gvDetailBath.FooterRow.Cells(8).Visible = False
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvDetailBath.DataSource = dt
                    gvDetailBath.DataBind()
                    gvDetailBath.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listDetailClear()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Cfg_Customer.CoyName, iPxAcctAR_Invoice.ReffNo, "
        sSQL += "iPxAcctAR_Invoice.InvDate, iPxAcctAR_Invoice.Notes, iPxAcctAR_Invoice.Status, "
        sSQL += "(select sum(amountdr) from iPxAcctAR_Transaction where invoiceno LIKE 'BC%' and iPxAcctAR_Transaction.invoiceno = iPxAcctAR_Invoice.InvoiceNo) as Debit "
        sSQL += "from iPxAcctAR_Transaction "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Customer ON iPxAcctAR_Cfg_Customer.CustomerID = iPxAcctAR_Transaction.CustomerID "
        sSQL += "INNER JOIN iPxAcctAR_Invoice ON iPxAcctAR_Invoice.InvoiceNo = iPxAcctAR_Transaction.invoiceno where iPxAcctAR_Transaction.TransID='" & Session("sSelectCustClearD") & "' "
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDetailClear.DataSource = dt
                    gvDetailClear.DataBind()
                    Dim total As Decimal = dt.Compute("Sum(Debit)", "").ToString()
                    gvDetailClear.FooterRow.Cells(3).Text = "Total"
                    gvDetailClear.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Left
                    gvDetailClear.FooterRow.Cells(4).Text = total.ToString("N2")
                    gvDetailClear.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right
                    gvDetailClear.FooterRow.Cells(4).CssClass = "cellOneCellPaddingRight"
                    'gvDetailBath.FooterRow.Cells(8).Visible = False
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvDetailClear.DataSource = dt
                    gvDetailClear.DataBind()
                    gvDetailClear.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub tittleOpen()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Cfg_Customer "
        sSQL += "WHERE businessid = '" & Session("sBusinessID") & "' and CustomerID ='" & Session("sSelectCustOpen") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            lblTitleOpen.Text = UCase(oSQLReader.Item("CoyName").ToString)
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub tittleBath()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT CoyName, (b.PaidBy) as DefaultPaid, (b.Description) as DefaultPaidDesc, c.CommissionPct FROM iPxAcctAR_Cfg_Customer as a "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Paidby as b ON b.PaidBy=a.DefaultPaid "
        sSQL += "LEFT JOIN iPxAcctAR_Commission as c ON c.businessid=a.businessid and c.CustomerID=a.CustomerID "
        sSQL += "WHERE a.businessid = '" & Session("sBusinessID") & "' and a.CustomerID ='" & Session("sSelectCustBath") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            lblTitleBath.Text = UCase(oSQLReader.Item("CoyName").ToString)
            Session("sPaidBy") = oSQLReader.Item("DefaultPaid").ToString
            Session("sPaidByDesc") = oSQLReader.Item("DefaultPaidDesc").ToString
            Session("sComissionClear") = oSQLReader.Item("CommissionPct").ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub saveBath()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim regDate As Date = Date.Now()
        Dim InvDate As Date
        'If tbTransDate.Text <> "" Then
        InvDate = Date.ParseExact(tbBathDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        'Else
        'dateBirthday = Date.ParseExact("01/01/1900", "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        'End If
        sSQL = "INSERT INTO iPxAcctAR_Invoice(businessid,InvoiceNo,Status,InvDate,DueDate,CustomerID,ReffNo,Notes,RegDate,RegBy) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & lbIdBath.Text & "','N','" & InvDate & "',''"
        sSQL += ",'" & Session("sSelectCustOpen") & "','" & Replace(tbReff.Text, "'", "''") & "','" & Replace(tbNotes.Text, "'", "''") & "'"
        sSQL += ",'" & regDate & "','" & Session("iUserID") & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Private Sub updateBathTrans(ByVal TransID As String)
        Dim constr As String = ConfigurationManager _
                        .ConnectionStrings("iPxCNCT").ConnectionString
        Dim query As String = "UPDATE iPxAcctAR_Transaction SET invoiceno='" & lbIdBath.Text & "' where" & _
                                " businessid = '" & Session("sBusinessID") & "' and RecID=@TransID"
        Dim con As New SqlConnection(constr)
        Dim cmd As New SqlCommand(query, con)
        cmd.Parameters.AddWithValue("@TransID", TransID)
        con.Open()
        cmd.ExecuteNonQuery()
        con.Close()
    End Sub
    Sub saveClearHeader()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim regDate As Date = Date.Now()
        Dim ClearDate As Date
        ClearDate = Date.ParseExact(tbClearDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)

        sSQL = "INSERT INTO iPxAcctAR_Receipt (businessid,ReceiptID,Status,ReceiptDate,CustomerID,ReffNo,PaidBy,Notes,RegDate,RegBy,isActive) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & lbIdClear.Text & "','N','" & ClearDate & "','" & Session("sSelectCustBath") & "'"
        sSQL += ",'" & Replace(tbReffClear.Text, "'", "''") & "','" & Session("sPaidBy") & "','" & Replace(tbNotesClear.Text, "'", "''") & "'"
        sSQL += ",'" & regDate & "','" & Session("iUserID") & "','Y') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub saveClearDetail()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim regDate As Date = Date.Now()
        sSQL = "INSERT INTO iPxAcctAR_Transaction(businessid,TransID,TransDate,CustomerID,transactiontype,invoiceno,voucherno, "
        sSQL += "foliono,GuestName,RoomNo,arrival,departure,notes,amountdr,amountcr,amountdr_frgn, amountcr_frgn, currency, FoLink, isActive) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & lbIdClear.Text & "','" & regDate & "','" & Session("sSelectCustBath") & "','CL'"
        sSQL += ",'" & Session("sInvNoClear") & "','','','','','','','','0','" & CDec(Session("sAmountClear")) & "','0','0','0','','Y') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub saveClearComission()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "INSERT INTO iPxAcctAR_ClearCommission(businessid,ReceiptID,comission,amount) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & lbIdClear.Text & "','" & tbComission.Text & "','" & tbAmountComission.Text & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub tittleClear()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Cfg_Customer "
        sSQL += "WHERE businessid = '" & Session("sBusinessID") & "' and CustomerID ='" & Session("sSelectCustClear") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            lblTitleTableClear.Text = UCase(oSQLReader.Item("CoyName").ToString)
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub tittlePosted()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Cfg_Customer "
        sSQL += "WHERE businessid = '" & Session("sBusinessID") & "' and CustomerID ='" & Session("sSelectCustPosted") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            lbCustPost.Text = UCase(oSQLReader.Item("CoyName").ToString)
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub listCardClear()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAR_Receipt.ReceiptID, iPxAcctAR_Receipt.ReceiptDate, iPxAcctAR_Receipt.Notes, iPxAcctAR_Receipt.ReffNo, "
        sSQL += "(select sum(amountcr) from iPxAcctAR_Transaction where iPxAcctAR_Transaction.TransID = iPxAcctAR_Receipt.ReceiptID) as Paid, "
        sSQL += "(select sum(amount) from iPxAcctAR_ClearCommission where iPxAcctAR_ClearCommission.ReceiptID = iPxAcctAR_Receipt.ReceiptID) as Comission, "
        sSQL += "(select (sum(amountcr)-sum(amount)) from iPxAcctAR_Transaction as a INNER JOIN iPxAcctAR_ClearCommission as b ON b.businessid=a.businessid and b.ReceiptID=a.TransID "
        sSQL += "where a.TransID = iPxAcctAR_Receipt.ReceiptID) as Net, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='18'and x.active='Y' and x.businessid=iPxAcctAR_Receipt.businessid and x.usercode='" & Session("sUserCode") & "') as editClear, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='10038'and x.active='Y' and x.businessid=iPxAcctAR_Receipt.businessid and x.usercode='" & Session("sUserCode") & "') as deleteClear "
        sSQL += "FROM iPxAcctAR_Receipt "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Customer ON iPxAcctAR_Receipt.businessid = iPxAcctAR_Cfg_Customer.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND "
        sSQL += "iPxAcctAR_Receipt.CustomerID COLLATE Latin1_General_CI_AS = iPxAcctAR_Cfg_Customer.CustomerID "
        sSQL += "where iPxAcctAR_Receipt.ReceiptID like 'CL%' AND "
        sSQL += "iPxAcctAR_Receipt.businessid ='" & Session("sBusinessID") & "' AND iPxAcctAR_Receipt.CustomerID = '" & Session("sSelectCustClear") & "' AND iPxAcctAR_Receipt.Status = 'N' "
        sSQL += "order by iPxAcctAR_Receipt.ReceiptDate desc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvTransClear.DataSource = dt
                    gvTransClear.DataBind()
                    gvTransClear.Enabled = True
                    lbPosting.Enabled = True
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvTransClear.DataSource = dt
                    gvTransClear.DataBind()
                    gvTransClear.Rows(0).Visible = False
                    gvTransClear.Enabled = False
                    lbPosting.Enabled = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listCardPosted()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAR_Receipt.ReceiptID, iPxAcctAR_Receipt.ReceiptDate, iPxAcctAR_Receipt.Notes, iPxAcctAR_Receipt.ReffNo, "
        sSQL += "(select sum(amountcr) from iPxAcctAR_Transaction where iPxAcctAR_Transaction.TransID = iPxAcctAR_Receipt.ReceiptID) as Paid "
        sSQL += "FROM iPxAcctAR_Receipt "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Customer ON iPxAcctAR_Receipt.businessid = iPxAcctAR_Cfg_Customer.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND "
        sSQL += "iPxAcctAR_Receipt.CustomerID COLLATE Latin1_General_CI_AS = iPxAcctAR_Cfg_Customer.CustomerID "
        sSQL += "where iPxAcctAR_Receipt.ReceiptID like 'CL%' AND "
        sSQL += "iPxAcctAR_Receipt.businessid ='" & Session("sBusinessID") & "' AND iPxAcctAR_Receipt.CustomerID = '" & Session("sSelectCustPosted") & "' AND iPxAcctAR_Receipt.Status = 'P' "
        sSQL += "order by iPxAcctAR_Receipt.ReceiptDate desc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvTransPosted.DataSource = dt
                    gvTransPosted.DataBind()
                    gvTransPosted.Enabled = True
                    lbCustPost.Enabled = True
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvTransPosted.DataSource = dt
                    gvTransPosted.DataBind()
                    gvTransPosted.Rows(0).Visible = False
                    gvTransPosted.Enabled = False
                    lbCustPost.Enabled = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Private Sub PostingRecord(ByVal ReceiptID As String)
        Dim constr As String = ConfigurationManager _
                        .ConnectionStrings("iPxCNCT").ConnectionString
        Dim query As String = "UPDATE iPxAcctAR_Receipt SET Status='P' where" & _
                                " businessid = '" & Session("sBusinessID") & "' and ReceiptID=@ReceiptID"
        Dim con As New SqlConnection(constr)
        Dim cmd As New SqlCommand(query, con)
        cmd.Parameters.AddWithValue("@ReceiptID", ReceiptID)
        con.Open()
        cmd.ExecuteNonQuery()
        con.Close()
    End Sub
    Sub EditBatch()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.* FROM iPxAcctAR_Invoice as a "
        sSQL += "WHERE a.businessid ='" & Session("sBusinessID") & "' and a.InvoiceNo ='" & Replace(lbIdBath.Text, "'", "''") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Dim InvDate As Date = oSQLReader.Item("InvDate").ToString
            tbBathDate.Text = InvDate.ToString("dd/MM/yyyy")
            tbReff.Text = oSQLReader.Item("ReffNo").ToString
            tbNotes.Text = oSQLReader.Item("Notes").ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub updateInvoiceHeader()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim regDate As Date = Date.Now()
        Dim InvDate, Duedate As Date
        InvDate = Date.ParseExact(tbBathDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        sSQL = "UPDATE iPxAcctAR_Invoice SET InvDate='" & InvDate & "',ReffNo='" & tbReff.Text & "',Notes='" & tbNotes.Text & "'"
        sSQL = sSQL & "WHERE invoiceno='" & lbIdBath.Text & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub DeleteBatch()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAR_Invoice SET status='X' "
        sSQL = sSQL & "WHERE InvoiceNo ='" & Session("sSelectCustBathD") & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAR_Transaction SET invoiceno=''"
        sSQL = sSQL & "WHERE invoiceno ='" & Session("sSelectCustBathD") & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub EditClear()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*, (b.Description) as DefaultPaid, (select sum(amountcr) from iPxAcctAR_Transaction where iPxAcctAR_Transaction.TransID = a.ReceiptID) as Paid "
        sSQL += "FROM iPxAcctAR_Receipt as a "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Paidby as b ON b.PaidBy=a.PaidBy "
        sSQL += "WHERE a.businessid ='" & Session("sBusinessID") & "' and a.ReceiptID ='" & Replace(lbIdClear.Text, "'", "''") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Dim InvDate As Date = oSQLReader.Item("ReceiptDate").ToString
            tbClearDate.Text = InvDate.ToString("dd/MM/yyyy")
            tbReffClear.Text = oSQLReader.Item("ReffNo").ToString
            tbBankClearance.Text = oSQLReader.Item("DefaultPaid").ToString
            tbNotesClear.Text = oSQLReader.Item("Notes").ToString
            tbTotalAmount.Text = String.Format("{0:N2}", (oSQLReader.Item("Paid"))).ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub updateReceiptHeader()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim RecDate As Date
        RecDate = Date.ParseExact(tbClearDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        sSQL = "UPDATE iPxAcctAR_Receipt SET ReceiptDate='" & RecDate & "',ReffNo='" & tbReffClear.Text & "',Notes='" & tbNotesClear.Text & "'"
        sSQL = sSQL & "WHERE ReceiptID='" & lbIdClear.Text & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub DeleteClear()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAR_Receipt SET status='X' "
        sSQL = sSQL & "WHERE ReceiptID ='" & Session("sSelectCustBathD") & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAR_Transaction SET isActive='N'"
        sSQL = sSQL & "WHERE TransID ='" & Session("sSelectCustBathD") & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "DELETE FROM iPxAcctAR_ClearCommission "
        sSQL = sSQL & "WHERE ReceiptID ='" & Session("sSelectCustBathD") & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        InvoiceNo()
    End Sub
    Sub InvoiceNo()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.invoiceno from iPxAcctAR_Transaction as a "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' "
        sSQL += "AND a.TransID = '" & Session("sSelectCustBathD") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Dim invDel As String = oSQLReader.Item("invoiceno").ToString
            oCnct.Close()
            findInvPaid(invDel)
        Else
            oCnct.Close()
        End If
    End Sub
    Public Function findInvPaid(ByVal invNo As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.invoiceno from iPxAcctAR_Transaction as a "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' "
        sSQL += "AND a.invoiceno = '" & invNo & "' and TransID like 'CL%' and isActive='Y'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            oCnct.Close()
            statusinvoice = "P"
            updateInvoice(invNo)
        Else
            oCnct.Close()
            statusinvoice = "N"
            updateInvoice(invNo)
        End If
    End Function
    Public Function updateInvoice(ByVal invNo As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAR_Invoice SET Status='" & statusinvoice & "'"
        sSQL = sSQL & " WHERE InvoiceNo ='" & invNo & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Function
    Public Function Posting(ByVal businessid As String, ByVal TransID As String) As Boolean

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Receipt where businessid ='" & businessid & "' and ReceiptID='" & TransID & "' "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        While oSQLReader.Read
            'oSQLReader.Close()
            Dim GLid As String = cIpx.GetCounterMBR("GL", "GL")
            saveGLHeader(GLid, businessid, TransID, oSQLReader.Item("ReceiptDate"), oSQLReader.Item("Notes"))
            GLDetail(businessid, TransID, GLid)
        End While
        oCnct.Close()

    End Function

    Public Function saveGLHeader(ByVal GLid As String, ByVal businessid As String, ByVal TransID As String, ByVal GLDate As String, ByVal GLDesc As String) As Boolean
        If oCnct1.State = ConnectionState.Closed Then
            oCnct1.Open()
        End If
        oSQLCmd1 = New SqlCommand(sSQL1, oCnct1)

        sSQL1 = "INSERT INTO iPxAcctGL_JVhdr(businessid, TransID, TransDate, Status, JVgroup, ReffNo, Description) "
        sSQL1 += "VALUES ('" & businessid & "','" & GLid & "','" & GLDate & "', "
        sSQL1 += "'O','G2','" & TransID & "','" & GLDesc & "') "
        oSQLCmd1.CommandText = sSQL1
        oSQLCmd1.ExecuteNonQuery()

        oCnct1.Close()
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting header success !');document.getElementById('Buttonx').click()", True)
    End Function
    Public Function GLDetail(ByVal businessid As String, ByVal TransID As String, ByVal GLid As String) As Boolean

        If oCnct2.State = ConnectionState.Closed Then
            oCnct2.Open()
        End If
        oSQLCmd2 = New SqlCommand(sSQL2, oCnct2)
        sSQL2 = "SELECT a.RecID, b.CustomerID, b.CoyName, b.CoyName, (b.CoaLink) as coaCredit, d.Description, (d.Coa) as coaDebit, a.GuestName, a.amountcr, "
        sSQL2 += "(select x.CommissionPct from iPxAcctAR_Commission as x where x.businessid=a.businessid and x.CustomerID=b.CustomerID) as kimisi, "
        sSQL2 += "(select y.CommissionCoa from iPxAcctAR_Commission as y where y.businessid=a.businessid and y.CustomerID=b.CustomerID) as coaKomisi "
        sSQL2 += "FROM iPxAcctAR_Transaction as a "
        sSQL2 += "INNER JOIN iPxAcctAR_Cfg_Customer as b ON a.businessid = b.businessid AND a.CustomerID = b.CustomerID "
        sSQL2 += "INNER JOIN iPxAcctAR_Receipt as c ON c.businessid=a.businessid and c.ReceiptID=a.TransID "
        sSQL2 += "INNER JOIN iPxAcctAR_Cfg_Paidby as d ON d.businessid=c.businessid and d.PaidBy=c.PaidBy "
        sSQL2 += "where a.businessid ='" & businessid & "' and a.TransID='" & TransID & "' and a.isActive = 'Y' order by a.RecID asc "
        oSQLCmd2.CommandText = sSQL2
        oSQLReader2 = oSQLCmd2.ExecuteReader
        While oSQLReader2.Read
            If oSQLReader2.Item("kimisi").ToString <> "" Then
                Dim komisi, sisa As Integer
                komisi = (Val(oSQLReader2.Item("amountcr").ToString) * Val(oSQLReader2.Item("kimisi").ToString)) / 100
                sisa = Val(oSQLReader2.Item("amountcr").ToString) - komisi
                saveGLDetailDebit(businessid, GLid, oSQLReader2.Item("coaDebit"), oSQLReader2.Item("CoyName"), TransID, sisa)
                saveGLDetailDebit(businessid, GLid, oSQLReader2.Item("coaKomisi"), oSQLReader2.Item("CoyName"), TransID, komisi)
            Else
                saveGLDetailDebit(businessid, GLid, oSQLReader2.Item("coaDebit"), oSQLReader2.Item("CoyName"), TransID, oSQLReader2.Item("amountcr"))
            End If
            saveGLDetailCredit(businessid, GLid, oSQLReader2.Item("coaCredit"), oSQLReader2.Item("CoyName"), TransID, oSQLReader2.Item("amountcr"))
        End While
        oCnct2.Close()

    End Function

    Sub idGLDtl()
        Dim oCnct4 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLReader4 As SqlDataReader
        Dim oSQLCmd4 As SqlCommand
        Dim sSQL4 As String
        If oCnct4.State = ConnectionState.Closed Then
            oCnct4.Open()
        End If
        oSQLCmd4 = New SqlCommand(sSQL4, oCnct4)
        sSQL4 = "SELECT MAX(RecID) as RecID FROM iPxAcctGL_JVdtl where businessid = '" & Session("sBusinessID") & "' and TransID = '" & Session("sTransIDGL") & "'"
        oSQLCmd4.CommandText = sSQL4
        oSQLReader4 = oSQLCmd4.ExecuteReader

        oSQLReader4.Read()
        If oSQLReader4.HasRows Then
            RecID = Val(oSQLReader4.Item("RecID").ToString) + 1
        Else
            RecID = "1"
        End If
        oCnct4.Close()
    End Sub
    Public Function saveGLDetailDebit(ByVal businessid As String, ByVal TransID As String, ByVal Coa As String, ByVal Desc As String, ByVal Reff As String, ByVal Amount As String) As Boolean
        Dim oCnct3 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd3 As SqlCommand
        Dim sSQL3 As String
        Session("sTransIDGL") = TransID
        idGLDtl()
        If oCnct3.State = ConnectionState.Closed Then
            oCnct3.Open()
        End If
        oSQLCmd3 = New SqlCommand(sSQL3, oCnct3)
        sSQL3 = "INSERT INTO iPxAcctGL_JVdtl(RecID, businessid, TransID, Coa, Description, Reff, Debit, Credit, isActive) "
        sSQL3 += "VALUES ('" & RecID & "','" & businessid & "','" & TransID & "','" & Coa & "', "
        sSQL3 += "'" & Desc & "','" & Reff & "','" & Amount & "','0', "
        sSQL3 += "'Y') "
        oSQLCmd3.CommandText = sSQL3
        oSQLCmd3.ExecuteNonQuery()

        oCnct3.Close()
    End Function
    Public Function saveGLDetailCredit(ByVal businessid As String, ByVal TransID As String, ByVal Coa As String, ByVal Desc As String, ByVal Reff As String, ByVal Amount As String) As Boolean
        Dim oCnct3 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd3 As SqlCommand
        Dim sSQL3 As String
        Session("sTransIDGL") = TransID
        idGLDtl()
        If oCnct3.State = ConnectionState.Closed Then
            oCnct3.Open()
        End If
        oSQLCmd3 = New SqlCommand(sSQL3, oCnct3)
        sSQL3 = "INSERT INTO iPxAcctGL_JVdtl(RecID, businessid, TransID, Coa, Description, Reff, Debit, Credit, isActive) "
        sSQL3 += "VALUES ('" & RecID & "','" & businessid & "','" & TransID & "','" & Coa & "', "
        sSQL3 += "'" & Desc & "','" & Reff & "','0','" & Amount & "', "
        sSQL3 += "'Y') "
        oSQLCmd3.CommandText = sSQL3
        oSQLCmd3.ExecuteNonQuery()

        oCnct3.Close()
    End Function
    Sub cekClose()
        Dim oCnct4 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLReader4 As SqlDataReader
        Dim oSQLCmd4 As SqlCommand
        Dim sSQL4 As String
        If oCnct4.State = ConnectionState.Closed Then
            oCnct4.Open()
        End If
        oSQLCmd4 = New SqlCommand(sSQL4, oCnct4)
        sSQL4 = "SELECT * FROM iPxAcctGL_JVhdr where businessid='" & Session("sBusinessID") & "' "
        sSQL4 += "and ReffNo like 'PL " & MonthCekClosing & "-" & YearCekClosing & "' and status<>'D'"
        oSQLCmd4.CommandText = sSQL4
        oSQLReader4 = oSQLCmd4.ExecuteReader

        oSQLReader4.Read()
        If oSQLReader4.HasRows Then
            lbSaveClear.Enabled = False
            lbUpdateClear.Enabled = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('This Month Period Has been Close !!');", True)
        Else
            lbSaveClear.Enabled = True
            lbUpdateClear.Enabled = True
        End If
        oCnct4.Close()
    End Sub
    Protected Sub cari(ByVal sender As Object, ByVal e As EventArgs)
        MonthCekClosing = Strings.Mid(tbClearDate.Text, 4, 2)
        YearCekClosing = Strings.Right(tbClearDate.Text, 4)
        cekClose()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCreatClear", "hideModalCreatClear()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCreatClear", "showModalCreatClear()", True)
        If lbSaveClear.Visible = True Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BatchActive", "BatchActive()", True)
        ElseIf lbUpdateClear.Visible = True Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ClearActive", "ClearActive()", True)
        End If
    End Sub
    Public Function CekCoaGL(ByVal businessid As String, ByVal TransID As String) As Boolean

        If oCnct2.State = ConnectionState.Closed Then
            oCnct2.Open()
        End If
        oSQLCmd2 = New SqlCommand(sSQL2, oCnct2)
        sSQL2 = "SELECT a.CustomerID,a.CoyName,(a.CoaLink) as coaCredit, b.Description,(b.Coa) as coaDebit, c.CommissionPct, c.CommissionCoa FROM iPxAcctAR_Cfg_Customer as a "
        sSQL2 += "INNER JOIN iPxAcctAR_Cfg_Paidby as b ON b.PaidBy=a.DefaultPaid "
        sSQL2 += "LEFT JOIN iPxAcctAR_Commission as c ON c.businessid=a.businessid and c.CustomerID=a.CustomerID "
        sSQL2 += "where a.businessid ='" & businessid & "' AND a.arGroup='CC' AND a.IsActive='Y' AND a.CustomerID = '" & TransID & "' order by a.CustomerID asc "
        oSQLCmd2.CommandText = sSQL2
        oSQLReader2 = oSQLCmd2.ExecuteReader

        oSQLReader2.Read()
        If oSQLReader2.HasRows Then
            coaCredit = oSQLReader2.Item("coaCredit").ToString
            coaDebit = oSQLReader2.Item("coaDebit").ToString
            komisi = oSQLReader2.Item("CommissionPct").ToString
            coaKomisi = oSQLReader2.Item("CommissionCoa").ToString
        Else

        End If
        oCnct2.Close()

    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Me.IsPostBack Then
            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "AR Credit card") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            Session("sQueryTicketOpen") = ""
            Session("sQueryTicketBath") = ""
            Session("sQueryTicketClear") = ""
            Session("sSelectCustPosted") = ""
            Session("sSelectCustOpen") = ""
            Session("sSelectCustBath") = ""
            Session("sSelectCustClear") = ""
            Session("sSelectCustPosted") = ""
            listCustomerOpen()
            listCardOpen()
            listCustomerBath()
            listCardBath()
            listCustomerClear()
            listCardClear()
            listCustomerPosted()
            listCardPosted()
        End If
    End Sub

    Protected Sub gvCustAR_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCustAR.RowCommand
        If e.CommandName = "getSelect" Then
            Session("sSelectCustOpen") = e.CommandArgument
            tittleOpen()
            listCardOpen()
            UserAccesBatch()
        End If
    End Sub

    Protected Sub gvCustBath_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCustBath.RowCommand
        If e.CommandName = "getSelect" Then
            Session("sSelectCustBath") = e.CommandArgument
            tittleBath()
            listCardBath()
            UserAccesClear()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BatchActive", "BatchActive()", True)
        End If
    End Sub

    Protected Sub lbCreateBath_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCreateBath.Click
        lbSaveBath.Visible = True
        lbUpdateBath.Visible = False
        lbTitle.Text = "Batch "
        lbIdBath.Text = cIpx.GetCounterMBR("BC", "BC")
        tbBathDate.Text = Format(Now, "dd/MM/yyy")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCreatBath", "showModalCreatBath()", True)
    End Sub

    Protected Sub lbCreateClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCreateClear.Click
        lbIdClear.Text = cIpx.GetCounterMBR("CL", "CL")
        Dim cb2 As CheckBox = gvTransBath.HeaderRow.FindControl("chkAll")
        Dim cbterpilih As Boolean = False
        Dim cb As CheckBox = Nothing
        Dim n As Integer = 0
        Dim m As Integer = 0
        Do Until n = gvTransBath.Rows.Count
            cb = gvTransBath.Rows.Item(n).FindControl("chk")
            If cb IsNot Nothing AndAlso cb.Checked Then
                cbterpilih = True
                'insert & update
                m = m + CDec(gvTransBath.Rows(n).Cells(5).Text)
            End If
            n += 1
        Loop
        tbClearDate.Text = Format(Now, "dd/MM/yyy")
        tbBankClearance.Text = Session("sPaidByDesc")
        Dim i As Double
        If Session("sComissionClear") = "" Then
            i = 0
        Else
            i = Session("sComissionClear")
        End If
        tbComission.Text = i.ToString("N2")
        tbAmountComission.Text = ((m * tbComission.Text) / 100).ToString("N2")
        tbTotalAmount.Text = m.ToString("N2")
        tbNetClear.Text = (m - tbAmountComission.Text).ToString("N2")
        lbSaveClear.Visible = True
        lbUpdateClear.Visible = False
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCreatClear", "showModalCreatClear()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BatchActive", "BatchActive()", True)
    End Sub

    Protected Sub gvTransBath_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTransBath.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sSelectCustBathD") = e.CommandArgument
            lbTitleDetail.Text = "Detail Bath " + Session("sSelectCustBathD")
            listDetailBath()
            lbAbortDetailBath.Visible = True
            lbAbortDetailClear.Visible = False
            lbAbortDetailPosted.Visible = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BatchActive", "BatchActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDetailBath", "showModalDetailBath()", True)
        ElseIf e.CommandName = "getEditBatch" Then
            'Session("sSelectCustBathD") = e.CommandArgument
            lbSaveBath.Visible = False
            lbUpdateBath.Visible = True
            lbTitle.Text = "Edit Batch "
            lbIdBath.Text = e.CommandArgument
            EditBatch()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BatchActive", "BatchActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCreatBath", "showModalCreatBath()", True)
        ElseIf e.CommandName = "getDeleteBatch" Then
            Session("sSelectCustBathD") = e.CommandArgument
            DeleteBatch()
            listCustomerOpen()
            listCardOpen()
            listCustomerBath()
            listCardBath()
            Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
            cIpx.saveLog(Session("sBusinessID"), Session("sSelectCustBathD"), TransDateLog, "AR CC", "Delete", "Delete batch AR Credit card " & Session("sSelectCustBathD"), "", Session("sUserCode"))
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Delete batch success !');document.getElementById('Buttonx').click()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BatchActive", "BatchActive()", True)
        End If
    End Sub

    Protected Sub lbSaveBath_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSaveBath.Click
        If tbBathDate.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please enter bath date first!');document.getElementById('Buttonx').click()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCreatBath", "hideModalCreatBath()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCreatBath", "showModalCreatBath()", True)
        ElseIf tbReff.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please enter reff no first !');document.getElementById('Buttonx').click()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCreatBath", "hideModalCreatBath()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCreatBath", "showModalCreatBath()", True)
        Else
            Dim cb2 As CheckBox = gvTransOpen.HeaderRow.FindControl("chkAll")
            Dim cbterpilih As Boolean = False
            Dim cb As CheckBox = Nothing
            Dim n As Integer = 0
            Dim m As Integer = 0
            saveBath()
            Do Until n = gvTransOpen.Rows.Count
                cb = gvTransOpen.Rows.Item(n).FindControl("chk")
                If cb IsNot Nothing AndAlso cb.Checked Then
                    cbterpilih = True
                    'insert & update
                    updateBathTrans(gvTransOpen.Rows(n).Cells(1).Text)
                End If
                n += 1
            Loop
            Session("sSelectCustOpen") = ""
            listCustomerOpen()
            listCardOpen()
            listCustomerBath()
            listCardBath()
            Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
            cIpx.saveLog(Session("sBusinessID"), lbIdBath.Text, TransDateLog, "AR CC", "Save", "Create Batch AR Credit Card " & lbIdBath.Text, "", Session("sUserCode"))
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('create batch successfully !');document.getElementById('Buttonx').click()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCreatBath", "hideModalCreatBath()", True)
        End If
    End Sub

    Protected Sub lbQueryOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQueryOpen.Click
        Session("sQueryTicketOpen") = ""
        pnOpen.Visible = True
        pnBath.Visible = False
        pnClear.Visible = False
        tbQCustOpen.Text = ""
        tbQCoyOpen.Text = ""
        tbQBankOpen.Text = ""
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub lbQueryBath_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQueryBath.Click
        Session("sQueryTicketBath") = ""
        pnOpen.Visible = False
        pnBath.Visible = True
        pnClear.Visible = False
        tbQCustBath.Text = ""
        tbQCoyBath.Text = ""
        tbQBankBath.Text = ""
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BatchActive", "BatchActive()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub lblQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblQuery.Click
        If pnOpen.Visible = True Then
            If tbQCustOpen.Text.Trim <> "" Then
                Session("sConditionOpen") = Session("sConditionOpen") & " and (iPxAcctAR_Cfg_Customer.CustomerID like '%" & Replace(tbQCustOpen.Text.Trim, "'", "''") & "%') "
            End If
            If tbQCoyOpen.Text.Trim <> "" Then
                Session("sConditionOpen") = Session("sConditionOpen") & " and (iPxAcctAR_Cfg_Customer.CoyName like '%" & Replace(tbQCoyOpen.Text.Trim, "'", "''") & "%') "
            End If
            If tbQBankOpen.Text.Trim <> "" Then
                Session("sConditionOpen") = Session("sConditionOpen") & " and (iPxAcctAR_Cfg_Customer.DefaultPaid like '%" & Replace(tbQBankOpen.Text.Trim, "'", "''") & "%') "
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
            listCustomerOpen()
        ElseIf pnBath.Visible = True Then
            If tbQCustBath.Text.Trim <> "" Then
                Session("sConditionBath") = Session("sConditionBath") & " and (iPxAcctAR_Cfg_Customer.CustomerID like '%" & Replace(tbQCustBath.Text.Trim, "'", "''") & "%') "
            End If
            If tbQCoyBath.Text.Trim <> "" Then
                Session("sConditionBath") = Session("sConditionBath") & " and (iPxAcctAR_Cfg_Customer.CoyName like '%" & Replace(tbQCoyBath.Text.Trim, "'", "''") & "%') "
            End If
            If tbQBankBath.Text.Trim <> "" Then
                Session("sConditionBath") = Session("sConditionBath") & " and (iPxAcctAR_Cfg_Customer.DefaultPaid like '%" & Replace(tbQBankBath.Text.Trim, "'", "''") & "%') "
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BatchActive", "BatchActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
            listCustomerBath()
        ElseIf pnClear.Visible = True Then
            If tbQCustClear.Text.Trim <> "" Then
                Session("sConditionClear") = Session("sConditionClear") & " and (iPxAcctAR_Cfg_Customer.CustomerID like '%" & Replace(tbQCustClear.Text.Trim, "'", "''") & "%') "
            End If
            If tbQCoyClear.Text.Trim <> "" Then
                Session("sConditionClear") = Session("sConditionClear") & " and (iPxAcctAR_Cfg_Customer.CoyName like '%" & Replace(tbQCoyClear.Text.Trim, "'", "''") & "%') "
            End If
            If tbQBankClear.Text.Trim <> "" Then
                Session("sConditionClear") = Session("sConditionClear") & " and (iPxAcctAR_Cfg_Customer.DefaultPaid like '%" & Replace(tbQBankClear.Text.Trim, "'", "''") & "%') "
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ClearActive", "ClearActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
            listCustomerClear()
        End If
    End Sub

    Protected Sub lbSaveClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSaveClear.Click
        If tbClearDate.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please enter clear date first!');document.getElementById('Buttonx').click()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCreatClear", "hideModalCreatClear()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BatchActive", "BatchActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCreatClear", "showModalCreatClear()", True)
        ElseIf tbReffClear.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please enter reff no first !');document.getElementById('Buttonx').click()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCreatClear", "hideModalCreatClear()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BatchActive", "BatchActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCreatClear", "showModalCreatClear()", True)
        Else
            Dim cb2 As CheckBox = gvTransBath.HeaderRow.FindControl("chkAll")
            Dim cbterpilih As Boolean = False
            Dim cb As CheckBox = Nothing
            Dim n As Integer = 0
            Dim m As Integer = 0
            saveClearHeader()
            Do Until n = gvTransBath.Rows.Count
                cb = gvTransBath.Rows.Item(n).FindControl("chk")
                If cb IsNot Nothing AndAlso cb.Checked Then
                    cbterpilih = True
                    'insert & update
                    Session("sInvNoClear") = (gvTransBath.Rows(n).Cells(1).Text)
                    Session("sAmountClear") = (gvTransBath.Rows(n).Cells(5).Text)
                    Session("sAmountComission") = ((CDec(Session("sAmountClear")) * tbComission.Text) / 100).ToString("N2")
                    saveClearDetail()
                End If
                n += 1
            Loop
            saveClearComission()
            Session("sSelectCustBath") = ""
            Dim invNo As String = Session("sInvNoClear")
            statusinvoice = "P"
            updateInvoice(invNo)
            listCustomerBath()
            listCardBath()
            listCustomerClear()
            listCardClear()
            Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
            cIpx.saveLog(Session("sBusinessID"), lbIdClear.Text, TransDateLog, "AR CC", "Save", "Create Clearance AR Credit card " & lbIdClear.Text, "", Session("sUserCode"))
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('create clearance successfully !');document.getElementById('Buttonx').click()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BatchActive", "BatchActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCreatBath", "hideModalCreatBath()", True)
        End If
    End Sub

    Protected Sub gvCustClear_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCustClear.RowCommand
        If e.CommandName = "getSelect" Then
            Session("sSelectCustClear") = e.CommandArgument
            tittleClear()
            listCardClear()
            UserAccesPosting()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ClearActive", "ClearActive()", True)
        End If
    End Sub

    Protected Sub lbQueryClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQueryClear.Click
        Session("sQueryTicketClear") = ""
        pnOpen.Visible = False
        pnBath.Visible = False
        pnClear.Visible = True
        tbQCustClear.Text = ""
        tbQCoyClear.Text = ""
        tbQBankClear.Text = ""
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ClearActive", "ClearActive()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub lbPosting_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPosting.Click
        CekCoaGL(Session("sBusinessID"), Session("sSelectCustClear"))
        If coaCredit = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Posting failed, COA Credit is not set!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ClearActive", "ClearActive()", True)
        ElseIf coaDebit = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Posting failed, COA Debit is not set!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ClearActive", "ClearActive()", True)
        ElseIf komisi <> "" And coaKomisi = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Posting failed, COA Commission is not set!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ClearActive", "ClearActive()", True)
        Else
            Dim cb2 As CheckBox = gvTransClear.HeaderRow.FindControl("chkAll")
            Dim cbterpilih As Boolean = False
            Dim cb As CheckBox = Nothing
            Dim n As Integer = 0
            Dim m As Integer = 0
            Do Until n = gvTransClear.Rows.Count
                cb = gvTransClear.Rows.Item(n).FindControl("chk")
                If cb IsNot Nothing AndAlso cb.Checked Then
                    cbterpilih = True
                    'insert & update
                    Posting(Session("sBusinessID"), gvTransClear.Rows(n).Cells(1).Text)
                    PostingRecord(gvTransClear.Rows(n).Cells(1).Text)
                    Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
                    cIpx.saveLog(Session("sBusinessID"), gvTransClear.Rows(n).Cells(1).Text, TransDateLog, "AR CC", "Posting", "Posting AR Credit card " & gvTransClear.Rows(n).Cells(1).Text, "", Session("sUserCode"))
                    m = m + 1
                Else

                End If
                n += 1
            Loop
            listCustomerClear()
            listCardClear()
            listCustomerPosted()
            listCardPosted()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('" & m & " records has been posted!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ClearActive", "ClearActive()", True)
        End If
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvCustAR.PageIndex = e.NewPageIndex
        Me.listCustomerOpen()
        gvCustBath.PageIndex = e.NewPageIndex
        Me.listCustomerBath()
        gvCustClear.PageIndex = e.NewPageIndex
        Me.listCustomerClear()
        gvCustPosted.PageIndex = e.NewPageIndex
        Me.listCustomerPosted()
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvCustAR.PageIndex = e.NewPageIndex
        Me.listCustomerOpen()
        gvCustBath.PageIndex = e.NewPageIndex
        Me.listCustomerBath()
        gvCustClear.PageIndex = e.NewPageIndex
        Me.listCustomerClear()
        gvCustPosted.PageIndex = e.NewPageIndex
        Me.listCustomerPosted()
    End Sub

    Protected Sub gvCustAR_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCustAR.PageIndexChanging
        gvCustAR.PageIndex = e.NewPageIndex
        Me.listCustomerOpen()
    End Sub

    Protected Sub gvCustBath_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCustBath.PageIndexChanging
        gvCustBath.PageIndex = e.NewPageIndex
        Me.listCustomerBath()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BatchActive", "BatchActive()", True)
    End Sub

    Protected Sub gvCustClear_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCustClear.PageIndexChanging
        gvCustClear.PageIndex = e.NewPageIndex
        Me.listCustomerClear()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ClearActive", "ClearActive()", True)
    End Sub
    Protected Sub gvCustPosted_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCustPosted.PageIndexChanging
        gvCustPosted.PageIndex = e.NewPageIndex
        Me.listCustomerPosted()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PostedActive", "PostedActive()", True)
    End Sub

    Protected Sub gvTransClear_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTransClear.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sSelectCustClearD") = e.CommandArgument
            lbTitleDetailClear.Text = "Detail Clear " + Session("sSelectCustClearD")
            listDetailClear()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDetailClear", "showModalDetailClear()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ClearActive", "ClearActive()", True)
        ElseIf e.CommandName = "getEditClear" Then
            lbIdClear.Text = e.CommandArgument
            EditClear()
            lbSaveClear.Visible = False
            lbUpdateClear.Visible = True
            lbUpdateClear.Enabled = True
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCreatClear", "showModalCreatClear()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ClearActive", "ClearActive()", True)
        ElseIf e.CommandName = "getDeleteClear" Then
            Session("sSelectCustBathD") = e.CommandArgument
            DeleteClear()
            listCustomerBath()
            listCardBath()
            listCustomerClear()
            listCardClear()
            Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
            cIpx.saveLog(Session("sBusinessID"), Session("sSelectCustBathD"), TransDateLog, "AR CC", "Delete", "Delete clearance AR Credit card " & Session("sSelectCustBathD"), "", Session("sUserCode"))
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Delete batch success !');document.getElementById('Buttonx').click()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ClearActive", "ClearActive()", True)
        End If
    End Sub

    Protected Sub lbAbortDetailBath_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDetailBath.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDetailBath", "hideModalDetailBath()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BatchActive", "BatchActive()", True)
    End Sub

    Protected Sub gvDetailClear_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDetailClear.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sSelectCustBathD") = e.CommandArgument
            lbTitleDetail.Text = "Detail Bath " + Session("sSelectCustBathD")
            listDetailBath()
            lbAbortDetailBath.Visible = False
            If lbTitleDetailClear.Text = "Detail Posted " + Session("sSelectCustClearD") Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PostedActive", "PostedActive()", True)
                lbAbortDetailPosted.Visible = True
                lbAbortDetailClear.Visible = False
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ClearActive", "ClearActive()", True)
                lbAbortDetailPosted.Visible = False
                lbAbortDetailClear.Visible = True
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDetailClear", "hideModalDetailClear()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDetailBath", "showModalDetailBath()", True)
        End If
    End Sub

    Protected Sub lbAbortDetailClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDetailClear.Click
        lbTitleDetailClear.Text = "Detail Clear " + Session("sSelectCustClearD")
        listDetailClear()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDetailBath", "hideModalDetailBath()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDetailClear", "showModalDetailClear()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ClearActive", "ClearActive()", True)
    End Sub

    Protected Sub gvCustPosted_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCustPosted.RowCommand
        If e.CommandName = "getSelect" Then
            Session("sSelectCustPosted") = e.CommandArgument
            tittlePosted()
            listCardPosted()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PostedActive", "PostedActive()", True)
        End If
    End Sub

    Protected Sub gvTransPosted_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTransPosted.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sSelectCustClearD") = e.CommandArgument
            lbTitleDetailClear.Text = "Detail Posted " + Session("sSelectCustClearD")
            listDetailClear()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDetailClear", "showModalDetailClear()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PostedActive", "PostedActive()", True)
        End If
    End Sub

    Protected Sub lbAbortDetailPosted_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDetailPosted.Click
        lbTitleDetailClear.Text = "Detail Posted " + Session("sSelectCustClearD")
        listDetailClear()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDetailBath", "hideModalDetailBath()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDetailClear", "showModalDetailClear()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PostedActive", "PostedActive()", True)
    End Sub

    Protected Sub lbUpdateBath_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbUpdateBath.Click
        updateInvoiceHeader()
        listCardBath()
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), lbIdBath.Text, TransDateLog, "AR CC", "Update", "Update Batch AR Credit Card " & lbIdBath.Text, "", Session("sUserCode"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BatchActive", "BatchActive()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCreatBath", "hideModalCreatBath()", True)
    End Sub

    Protected Sub lbUpdateClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbUpdateClear.Click
        updateReceiptHeader()
        listCardClear()
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), lbIdClear.Text, TransDateLog, "AR CC", "Update", "Update clearance AR Credit card " & lbIdClear.Text, "", Session("sUserCode"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCreatClear", "hideModalCreatClear()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ClearActive", "ClearActive()", True)
    End Sub
End Class
