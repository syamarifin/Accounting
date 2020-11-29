Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration

Partial Class iPxAdmin_iPxAPInputPayment
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, sSQLPMS, statusinvoice, MonthCekClosing, YearCekClosing As String
    Dim cIpx As New iPxClass
    Dim x, y, creditM As Integer
    Dim totalDebit, totalCredit, total As Decimal
    Sub kosong()
        tbAmountcr.Text = ""
        tbNotes.Text = ""
        tbRecDate.Text = ""
        tbReff.Text = ""
        Paidby()
    End Sub
    Sub ListInvoice()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAP_PV.*, iPxAcctAP_Cfg_Vendor.CoyName, iPx_profile_user.fullname, "
        sSQL += "(select sum(amountdr) from iPxAcctAP_Transaction "
        sSQL += "where VendorID='" & Session("sReceiptInv") & "' AND transactiontype ='PY' and iPxAcctAP_Transaction.PVno = iPxAcctAP_PV.PVNo and iPxAcctAP_Transaction.isActive='Y') as Debit "
        sSQL += ",(select sum(amountcr) from iPxAcctAP_Transaction "
        sSQL += "where VendorID='" & Session("sReceiptInv") & "' AND transactiontype <>'PY' and iPxAcctAP_Transaction.PVno = iPxAcctAP_PV.PVNo and iPxAcctAP_Transaction.isActive='Y') as Credit "
        sSQL += ",(select (sum(amountcr)-sum(amountdr)) from iPxAcctAP_Transaction where PVno = iPxAcctAP_PV.PVNo and iPxAcctAP_Transaction.isActive='Y') as totalInvoice FROM iPxAcctAP_PV "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_vendor ON iPxAcctAP_PV.businessid = iPxAcctAP_Cfg_Vendor.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND "
        sSQL += "iPxAcctAP_PV.VendorID COLLATE Latin1_General_CI_AS = iPxAcctAP_Cfg_Vendor.VendorID "
        sSQL += "INNER JOIN iPx_profile_user ON iPxAcctAP_PV.RegBy = iPx_profile_user.id "
        sSQL += "where iPxAcctAP_PV.businessid ='" & Session("sBusinessID") & "' and iPxAcctAP_PV.VendorID ='" & Session("sReceiptInv") & "' "
        sSQL += "AND (select (sum(amountcr)-sum(amountdr)) from iPxAcctAP_Transaction where PVno = iPxAcctAP_PV.PVNo and isActive='Y') > '0'"
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL += " AND iPxAcctAP_PV.Status<>'X'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by iPxAcctAP_PV.DueDate asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvARInv.DataSource = dt
                    gvARInv.DataBind()
                    'Dim totalDebit As Decimal = dt.Compute("Sum(Debit)", "").ToString()
                    If dt.Compute("Sum(Debit)", "").ToString() = "" Then
                        totalDebit = 0
                    Else
                        totalDebit = dt.Compute("Sum(Debit)", "").ToString()
                    End If
                    If dt.Compute("Sum(Credit)", "").ToString() = "" Then
                        totalCredit = 0
                    Else
                        totalCredit = dt.Compute("Sum(Credit)", "").ToString()
                    End If
                    If dt.Compute("Sum(totalInvoice)", "").ToString() = "" Then
                        total = 0
                    Else
                        total = dt.Compute("Sum(totalInvoice)", "").ToString()
                    End If
                    gvARInv.FooterRow.Cells(7).Text = "Total"
                    gvARInv.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Left
                    gvARInv.FooterRow.Cells(8).Text = totalDebit.ToString("N2")
                    gvARInv.FooterRow.Cells(8).HorizontalAlign = HorizontalAlign.Right
                    gvARInv.FooterRow.Cells(9).Text = totalCredit.ToString("N2")
                    gvARInv.FooterRow.Cells(9).HorizontalAlign = HorizontalAlign.Right
                    gvARInv.FooterRow.Cells(10).Text = total.ToString("N2")
                    gvARInv.FooterRow.Cells(10).HorizontalAlign = HorizontalAlign.Right
                    tbAmountcr.Text = total.ToString("N2")
                    Session("sTotal") = total
                    gvARInv.FooterRow.Cells(11).Visible = False
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
    Sub ListInvoiceHeader()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAP_PV.*, iPxAcctAP_Cfg_Vendor.CoyName, iPx_profile_user.fullname, "
        sSQL += "(select sum(amountdr) from iPxAcctAP_Transaction "
        sSQL += "where VendorID='" & Session("sReceiptInv") & "' AND transactiontype ='PY' and iPxAcctAP_Transaction.PVno = iPxAcctAP_PV.PVNo and isActive='Y') as Debit "
        sSQL += ",(select sum(amountcr) from iPxAcctAP_Transaction "
        sSQL += "where VendorID='" & Session("sReceiptInv") & "' AND transactiontype <>'PY' and iPxAcctAP_Transaction.PVno = iPxAcctAP_PV.PVNo and isActive='Y') as Credit "
        sSQL += ",(select (sum(amountcr)-sum(amountdr)) from iPxAcctAP_Transaction where PVno = iPxAcctAP_PV.PVNo and isActive='Y') as totalInvoice FROM iPxAcctAP_PV "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_Vendor ON iPxAcctAP_PV.businessid = iPxAcctAP_Cfg_Vendor.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND "
        sSQL += "iPxAcctAP_PV.VendorID COLLATE Latin1_General_CI_AS = iPxAcctAP_Cfg_Vendor.VendorID "
        sSQL += "INNER JOIN iPx_profile_user ON iPxAcctAP_PV.RegBy = iPx_profile_user.id "
        sSQL += "where iPxAcctAP_PV.businessid ='" & Session("sBusinessID") & "' and iPxAcctAP_PV.VendorID ='" & Session("sReceiptInv") & "' "
        sSQL += "and iPxAcctAP_PV.PVNo='" & Session("sCustInv") & "'"
        sSQL += " AND iPxAcctAP_PV.Status='A'"
        sSQL += " order by iPxAcctAP_PV.DueDate asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvARInv.DataSource = dt
                    gvARInv.DataBind()
                    'Dim total As Decimal = dt.Compute("Sum(totalInvoice)", "").ToString()
                    'gvARInv.FooterRow.Cells(6).Text = "Total"
                    'gvARInv.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Left
                    'gvARInv.FooterRow.Cells(7).Text = total.ToString("N2")
                    'gvARInv.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
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
    Sub ListInvoiceEdit()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.PaymentID,a.VendorID,c.*, d.CoyName, "
        sSQL += "(select sum(amountdr) from iPxAcctAP_Transaction where VendorID=a.VendorID AND transactiontype ='PY' and iPxAcctAP_Transaction.PVno = c.PVNo and isActive='Y') as Debit, "
        sSQL += "(select sum(amountcr) from iPxAcctAP_Transaction where VendorID=a.VendorID AND transactiontype <>'PY' and iPxAcctAP_Transaction.PVno = c.PVNo and isActive='Y') as Credit, "
        sSQL += "(select (sum(amountcr)-sum(amountdr)) from iPxAcctAP_Transaction where PVno = c.PVNo and isActive='Y') as totalInvoice from iPxAcctAP_Payment as a "
        sSQL += "INNER JOIN iPxAcctAP_Transaction as b ON b.businessid=a.businessid and b.TransID=a.PaymentID "
        sSQL += "INNER JOIN iPxAcctAP_PV as c ON c.businessid=a.businessid and c.PVNo=b.PVno "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_Vendor as d ON d.businessid=a.businessid and d.VendorID=a.VendorID "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.PaymentID='" & lbRecID.Text & "' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL += " AND c.Status<>'X'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by c.DueDate asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvARInv.DataSource = dt
                    gvARInv.DataBind()
                    Dim totalDebit As Decimal = dt.Compute("Sum(Debit)", "").ToString()
                    If dt.Compute("Sum(Credit)", "").ToString() = "" Then
                        totalCredit = 0
                    Else
                        totalCredit = dt.Compute("Sum(Credit)", "").ToString()
                    End If
                    If dt.Compute("Sum(totalInvoice)", "").ToString() = "" Then
                        total = 0
                    Else
                        total = dt.Compute("Sum(totalInvoice)", "").ToString()
                    End If
                    gvARInv.FooterRow.Cells(7).Text = "Total"
                    gvARInv.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Left
                    gvARInv.FooterRow.Cells(8).Text = totalDebit.ToString("N2")
                    gvARInv.FooterRow.Cells(8).HorizontalAlign = HorizontalAlign.Right
                    gvARInv.FooterRow.Cells(9).Text = totalCredit.ToString("N2")
                    gvARInv.FooterRow.Cells(9).HorizontalAlign = HorizontalAlign.Right
                    gvARInv.FooterRow.Cells(10).Text = total.ToString("N2")
                    gvARInv.FooterRow.Cells(10).HorizontalAlign = HorizontalAlign.Right
                    Session("sTotal") = total
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
    Sub listDetailInvoice()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAP_Transaction.RecID, iPxAcctAP_Cfg_Vendor.CoyName, iPxAcctAP_Cfg_Transactiontype.Description, iPxAcctAP_Transaction.PVno, iPxAcctAP_Transaction.POno, iPxAcctAP_Transaction.RRno, "
        sSQL += "iPxAcctAP_Transaction.notes, (iPxAcctAP_Transaction.amountcr) as amount FROM iPxAcctAP_Transaction "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_Vendor ON iPxAcctAP_Transaction.businessid = iPxAcctAP_Cfg_Vendor.businessid AND "
        sSQL += "iPxAcctAP_Transaction.VendorID = iPxAcctAP_Cfg_Vendor.VendorID "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_Transactiontype ON iPxAcctAP_Transaction.businessid COLLATE SQL_Latin1_General_CP1_CI_AS = iPxAcctAP_Cfg_Transactiontype.businessid AND "
        sSQL += "iPxAcctAP_Transaction.transactiontype = iPxAcctAP_Cfg_Transactiontype.TransactionType "
        sSQL += "WHERE iPxAcctAP_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAP_Transaction.PVno<>'' and iPxAcctAP_Transaction.isActive='Y' and iPxAcctAP_Transaction.PVNo='" & Session("sCustInv") & "' and iPxAcctAP_Transaction.VendorID ='" & Session("sReceiptInv") & "' "
        sSQL += " order by iPxAcctAP_Transaction.TransDate asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDetailInvoice.DataSource = dt
                    gvDetailInvoice.DataBind()
                    'Calculate Sum and display in Footer Row
                    Dim total As Decimal = dt.Compute("Sum(amount)", "").ToString()
                    gvDetailInvoice.FooterRow.Cells(4).Text = "Total"
                    gvDetailInvoice.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Left
                    gvDetailInvoice.FooterRow.Cells(5).Text = total.ToString("N2")
                    gvDetailInvoice.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvDetailInvoice.DataSource = dt
                    gvDetailInvoice.DataBind()
                    gvDetailInvoice.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listDetailReceipt()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.TransID, b.CoyName,c.PaymentDate,(d.Description) as Paid, "
        sSQL += "a.amountdr FROM iPxAcctAP_Transaction as a "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_Vendor as b ON a.businessid = b.businessid AND a.VendorID = b.vendorID "
        sSQL += "INNER JOIN iPxAcctAP_Payment as c ON c.businessid=a.businessid and c.PaymentID=a.TransID "
        sSQL += "INNER Join iPxAcctAP_Cfg_Paidby as d ON d.businessid=a.businessid and d.PaidBy=c.PaidBy "
        sSQL += "WHERE a.businessid ='" & Session("sBusinessID") & "' and a.isActive='Y' and a.PVNo='" & Session("sCustInv") & "' "
        sSQL += " order by a.TransDate asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDetailReceipt.DataSource = dt
                    gvDetailReceipt.DataBind()
                    'Calculate Sum and display in Footer Row
                    Dim total As Decimal = dt.Compute("Sum(amountdr)", "").ToString()
                    gvDetailReceipt.FooterRow.Cells(3).Text = "Total"
                    gvDetailReceipt.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Left
                    gvDetailReceipt.FooterRow.Cells(4).Text = total.ToString("N2")
                    gvDetailReceipt.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvDetailReceipt.DataSource = dt
                    gvDetailReceipt.DataBind()
                    gvDetailReceipt.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub Paidby()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select * from iPxAcctAP_Cfg_Paidby WHERE businessid='" & Session("sBusinessID") & "' and isActive='Y'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlPaid.DataSource = dt
                dlPaid.DataTextField = "Description"
                dlPaid.DataValueField = "PaidBy"
                dlPaid.DataBind()
                dlPaid.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub saveReceiptHeader()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim regDate As Date = Date.Now()
        Dim RecDate As Date
        RecDate = Date.ParseExact(tbRecDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)

        sSQL = "INSERT INTO iPxAcctAP_Payment (businessid,PaymentID,Status,PaymentDate,VendorID,ReffNo,PaidBy,Notes,RegDate,RegBy,isActive) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & lbRecID.Text & "','N','" & RecDate & "','" & Session("sReceiptInv") & "'"
        sSQL += ",'" & Replace(tbReff.Text, "'", "''") & "','" & dlPaid.SelectedValue & "','" & Replace(tbNotes.Text, "'", "''") & "'"
        sSQL += ",'" & regDate & "','" & Session("iUserID") & "','Y') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub saveReceiptDetail()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        'Dim regDate As Date = Date.Now()
        Dim regDate As Date = Date.ParseExact(tbRecDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        sSQL = "INSERT INTO iPxAcctAP_Transaction(businessid,TransID,TransDate,VendorID,TransactionType,PVno,POno,RRno,notes, "
        sSQL += "amountdr,amountcr,amountdr_frgn,amountcr_frgn,currency,InvLink,isActive) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & lbRecID.Text & "','" & regDate & "','" & Session("sReceiptInv") & "','PY'"
        sSQL += ",'" & Session("sInvNo") & "','','','','" & creditM & "','0','0','0','0','','Y') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub noInv()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAP_PV.PVNo, (select (sum(amountcr)-sum(amountdr)) from iPxAcctAP_Transaction where PVno = iPxAcctAP_PV.PVNo and isActive='Y') as totalInvoice FROM iPxAcctAP_PV "
        sSQL += "where iPxAcctAP_PV.businessid ='" & Session("sBusinessID") & "' and iPxAcctAP_PV.VendorID ='" & Session("sReceiptInv") & "' "
        sSQL += "AND (select (sum(amountcr)-sum(amountdr)) from iPxAcctAP_Transaction where PVno = iPxAcctAP_PV.PVNo and isActive='Y') <> 0"
        sSQL += " AND iPxAcctAP_PV.Status='A' AND iPxAcctAP_PV.PVNo='" & Session("sInvNo") & "'"
        sSQL += " order by iPxAcctAP_PV.DueDate asc"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            'Session("sInvNo") = oSQLReader.Item("InvoiceNo").ToString
            'Session("sTInv") = oSQLReader.Item("totalInvoice").ToString
            y = CDec(String.Format("{0:N2}", (oSQLReader.Item("totalInvoice"))).ToString)
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub editReceipt()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAP_Payment "
        sSQL += "where businessid ='" & Session("sBusinessID") & "' "
        sSQL += "AND PaymentID = '" & Session("sEditRece") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            dlPaid.SelectedValue = oSQLReader.Item("PaidBy").ToString
            tbReff.Text = oSQLReader.Item("ReffNo").ToString
            tbNotes.Text = oSQLReader.Item("Notes").ToString
            Session("sReceiptInv") = oSQLReader.Item("VendorID").ToString
            Dim RecDate As Date = oSQLReader.Item("PaymentDate").ToString
            tbRecDate.Text = RecDate.ToString("dd/MM/yyyy")
            If oSQLReader.Item("Status").ToString = "N" Then
                Session("sStatusARTrans") = oSQLReader.Item("Status").ToString
                lbStatus.Text = "New"
            ElseIf oSQLReader.Item("Status").ToString = "X" Then
                Session("sStatusARTrans") = oSQLReader.Item("Status").ToString
                lbStatus.Text = "Delete"
            End If
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub editNominal()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT sum(amountdr) as total FROM iPxAcctAP_Transaction "
        sSQL += "where businessid ='" & Session("sBusinessID") & "' "
        sSQL += "AND TransID = '" & Session("sEditRece") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbAmountcr.Text = String.Format("{0:N2}", (oSQLReader.Item("total"))).ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub hapusTrans()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "Delete from iPxAcctAR_Transaction WHERE iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' "
        sSQL += "AND iPxAcctAR_Transaction.TransID = '" & Session("sEditRece") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub SelectInvoice()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim a As Integer
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select count(iPxAcctAR_Transaction.invoiceno) as invoiceno from iPxAcctAR_Transaction "
        sSQL += "inner join iPxAcctAR_Invoice on iPxAcctAR_Invoice.InvoiceNo=iPxAcctAR_Transaction.invoiceno "
        sSQL += "where iPxAcctAR_Invoice.Status='P' and iPxAcctAR_Transaction.transactiontype='RC' and iPxAcctAR_Transaction.TransID='" & Session("sEditRece") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            a = CDec(oSQLReader.Item("invoiceno").ToString)
            oCnct.Close()
        Else
            oCnct.Close()
        End If
        Do Until a = 0
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If

            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "select iPxAcctAR_Transaction.invoiceno from iPxAcctAR_Transaction "
            sSQL += "inner join iPxAcctAR_Invoice on iPxAcctAR_Invoice.InvoiceNo=iPxAcctAR_Transaction.invoiceno "
            sSQL += "where iPxAcctAR_Invoice.Status='P' and iPxAcctAR_Transaction.transactiontype='RC' and iPxAcctAR_Transaction.TransID='" & Session("sEditRece") & "'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            oSQLReader.Read()
            If oSQLReader.HasRows Then
                Session("sInvNo") = oSQLReader.Item("invoiceno").ToString
                oCnct.Close()

                statusinvoice = "N"
                updateInvoice()
            Else
                oCnct.Close()
            End If
            a = Val(a - 1)
        Loop
    End Sub
    Sub updateInvoice()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAP_PV SET Status='" & statusinvoice & "'"
        sSQL = sSQL & " WHERE PVNo ='" & Session("sInvNo") & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

    Sub UpdateReceiptHeader()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim regDate As Date = Date.Now()
        Dim RecDate As Date
        RecDate = Date.ParseExact(tbRecDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)

        sSQL = "UPDATE iPxAcctAP_Payment SET PaymentDate='" & RecDate & "',ReffNo='" & Replace(tbReff.Text, "'", "''") & "', "
        sSQL += "PaidBy='" & dlPaid.SelectedValue & "',Notes='" & Replace(tbNotes.Text, "'", "''") & "' "
        sSQL += "WHERE PaymentID ='" & lbRecID.Text & "' and businessid='" & Session("sBusinessID") & "' "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

    Sub UpdateReceiptDetail()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim regDate As Date = Date.ParseExact(tbRecDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        sSQL = "UPDATE iPxAcctAP_Transaction SET TransDate ='" & regDate & "' "
        sSQL += "WHERE TransID ='" & lbRecID.Text & "' and businessid='" & Session("sBusinessID") & "' "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

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
            lbSaveReceipt.Enabled = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('This Month Period Has been Close !!');", True)
        Else
            lbSaveReceipt.Enabled = True
        End If
        oCnct4.Close()
    End Sub
    Protected Sub cari(ByVal sender As Object, ByVal e As EventArgs)
        MonthCekClosing = Strings.Mid(tbRecDate.Text, 4, 2)
        YearCekClosing = Strings.Right(tbRecDate.Text, 4)
        cekClose()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Session("sQueryTicket") = ""
            If Session("sEditRece") = "" Then
                Session("sChkAll") = ""
                Session("sCustInv") = ""
                ListInvoice()
                Paidby()
                tbRecDate.Text = Format(Now, "dd/MM/yyyy")
                lbRecID.Text = cIpx.GetCounterMBR("PY", "PY")
                tbAmountcr.Enabled = True
                lbStatus.Text = "New"
                lbSaveReceipt.Text = "<i class='fa fa-save'></i> Save Payment"
                lbSaveReceipt.Visible = True
                lbUpdateReceipt.Visible = False
                PnReason.Visible = False
            Else
                lbRecID.Text = Session("sEditRece")
                lbSaveReceipt.Text = "<i class='fa fa-save'></i> Update Payment"
                lbSaveReceipt.Visible = False
                lbUpdateReceipt.Visible = True
                Paidby()
                editReceipt()
                editNominal()
                ListInvoiceEdit()
                tbAmountcr.Enabled = False
                If lbStatus.Text = "Delete" Then
                    PnReason.Visible = False
                    lbSaveReceipt.Enabled = False
                Else
                    PnReason.Visible = False
                    lbSaveReceipt.Enabled = True
                End If
                Dim cb2 As CheckBox = gvARInv.HeaderRow.FindControl("chkAll")
                Dim cbterpilih As Boolean = False
                Dim cb As CheckBox = Nothing
                Dim n As Integer = 0
                cb2.Enabled = False
                cb2.Checked = True
                Do Until n = gvARInv.Rows.Count
                    cb = gvARInv.Rows.Item(n).FindControl("chkitem")
                    cb.Enabled = False
                    cb.Checked = True
                    n += 1
                Loop
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datepicker", "$(document).ready(function() {datetimepicker()});", True)
    End Sub

    'Protected Sub gvCustAR_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCustAR.RowCommand
    '    
    'End Sub

    Protected Sub gvARInv_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvARInv.RowCommand
        If e.CommandName = "getDetailInv" Then
            Session("sCustInv") = e.CommandArgument
            ListInvoiceHeader()
            listDetailInvoice()
            gvARInv.Columns(10).Visible = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showPanelInvoiveDetail", "showPanelInvoiveDetail()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
        ElseIf e.CommandName = "getDetailRec" Then
            Session("sCustInv") = e.CommandArgument
            ListInvoiceHeader()
            listDetailReceipt()
            gvARInv.Columns(10).Visible = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showPanelReceiptDetail", "showPanelReceiptDetail()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
        End If
    End Sub

    Protected Sub lbCloseDetailInvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCloseDetailInvoice.Click
        If Session("sEditRece") = "" Then
            ListInvoice()
        Else
            ListInvoiceEdit()
        End If
        gvARInv.Columns(10).Visible = True
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hidePanelDetail", "hidePanelDetail()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
    End Sub

    Protected Sub lbAbort_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbort.Click
        Response.Redirect("iPxAPPayment.aspx")
    End Sub

    Protected Sub lbSaveReceipt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSaveReceipt.Click
        If tbRecDate.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter receipt date!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            tbRecDate.Focus()
        ElseIf tbReff.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter reff no!');", True)
            tbReff.Focus()
        ElseIf dlPaid.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select paid by!');", True)
            dlPaid.Focus()
        ElseIf tbAmountcr.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter amount credit!');", True)
            tbAmountcr.Focus()
        Else
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT PaymentID FROM iPxAcctAP_Payment WHERE PaymentID = '" & lbRecID.Text & "'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            If oSQLReader.Read Then
                oSQLReader.Close()
                updateInvoice()
                'SelectInvoice()
                'hapusTrans()
                'x = CDec(String.Format("{0:N2}", (tbAmountcr.Text)).ToString)
                'Dim cb2 As CheckBox = gvARInv.HeaderRow.FindControl("chkAll")
                'Dim cbterpilih As Boolean = False
                'Dim cb As CheckBox = Nothing
                'Dim n As Integer = 0
                'Dim m As Integer = 0
                'Do Until n = gvARInv.Rows.Count
                '    cb = gvARInv.Rows.Item(n).FindControl("chkitem")
                '    If cb IsNot Nothing AndAlso cb.Checked Then
                '        cbterpilih = True
                '        'insert & update
                '        If x > 0 Then
                '            Session("sInvNo") = (gvARInv.Rows(n).Cells(1).Text)
                '            noInv()
                '            'invoice yang harus di bayarkan
                '            If x > y Then
                '                creditM = y
                '            ElseIf x <= y Then
                '                creditM = x
                '            End If
                '            statusinvoice = "P"
                '            updateInvoice()
                '            saveReceiptDetail()
                '            x = Val(x - y)
                '        End If
                '    Else

                '    End If
                '    n += 1
                'Loop
                ''ListInvoiceEdit()
                'If x > 0 Then
                '    n = 0
                '    Do Until n = gvARInv.Rows.Count
                '        cb = gvARInv.Rows.Item(n).FindControl("chkitem")
                '        Dim chk As CheckBox = DirectCast(gvARInv.Rows(n).Cells(0) _
                '                        .FindControl("chkitem"), CheckBox)
                '        If chk.Enabled = False Then

                '        Else
                '            If cb IsNot Nothing AndAlso cb.Checked Then
                '                cbterpilih = True
                '                'insert & update

                '            Else
                '                If x > 0 Then
                '                    Session("sInvNo") = (gvARInv.Rows(n).Cells(1).Text)
                '                    noInv()
                '                    'invoice yang harus di bayarkan
                '                    If x > y Then
                '                        creditM = y
                '                    ElseIf x <= y Then
                '                        creditM = x
                '                    End If
                '                    statusinvoice = "P"
                '                    updateInvoice()
                '                    saveReceiptDetail()
                '                    x = Val(x - y)
                '                End If
                '            End If
                '        End If
                '        n += 1
                '    Loop
                'End If
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('data update successfully !');document.getElementById('Buttonx').click()", True)
                Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
                cIpx.saveLog(Session("sBusinessID"), lbRecID.Text, TransDateLog, "AP PAY", "Update", "Update AP Payment " & lbRecID.Text, "", Session("sUserCode"))
                Response.Redirect("iPxAPPayment.aspx")
            Else
                oSQLReader.Close()
                If CDec(String.Format("{0:N2}", (tbAmountcr.Text)).ToString) > Session("sTotal") Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('the nominal amount exceeds the balances limit!');", True)
                    tbAmountcr.Focus()
                Else
                    saveReceiptHeader()
                    Dim cb2 As CheckBox = gvARInv.HeaderRow.FindControl("chkAll")
                    Dim cbterpilih As Boolean = False
                    Dim cb As CheckBox = Nothing
                    Dim n As Integer = 0
                    Dim m As Integer = 0
                    x = CDec(String.Format("{0:N2}", (tbAmountcr.Text)).ToString)
                    Do Until n = gvARInv.Rows.Count
                        cb = gvARInv.Rows.Item(n).FindControl("chkitem")
                        If cb IsNot Nothing AndAlso cb.Checked Then
                            cbterpilih = True
                            'insert & update
                            If x > 0 Then
                                Session("sInvNo") = (gvARInv.Rows(n).Cells(1).Text)
                                noInv()
                                'invoice yang harus di bayarkan
                                If x > y Then
                                    creditM = y
                                ElseIf x <= y Then
                                    creditM = x
                                End If
                                statusinvoice = "P"
                                updateInvoice()
                                saveReceiptDetail()
                                x = Val(x - y)
                            End If
                        Else

                        End If
                        n += 1
                    Loop
                    If x > 0 Then
                        n = 0
                        Do Until n = gvARInv.Rows.Count
                            cb = gvARInv.Rows.Item(n).FindControl("chkitem")
                            If cb IsNot Nothing AndAlso cb.Checked Then
                                cbterpilih = True
                                'insert & update

                            Else
                                If x > 0 Then
                                    Session("sInvNo") = (gvARInv.Rows(n).Cells(1).Text)
                                    noInv()
                                    'invoice yang harus di bayarkan
                                    If x > y Then
                                        creditM = y
                                    ElseIf x <= y Then
                                        creditM = x
                                    End If
                                    statusinvoice = "P"
                                    updateInvoice()
                                    saveReceiptDetail()
                                    x = Val(x - y)
                                End If
                            End If
                            n += 1
                        Loop
                    End If
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('data save successfully !');document.getElementById('Buttonx').click()", True)
                    Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
                    cIpx.saveLog(Session("sBusinessID"), lbRecID.Text, TransDateLog, "AP PAY", "Save", "Create AP Payment " & lbRecID.Text, "", Session("sUserCode"))
                    Response.Redirect("iPxAPPayment.aspx")
                End If
            End If
        End If
    End Sub

    Protected Sub lbCloseDetailReceipt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCloseDetailReceipt.Click
        If Session("sEditRece") = "" Then
            ListInvoice()
        Else
            ListInvoiceEdit()
        End If
        gvARInv.Columns(10).Visible = True
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hidePanelDetail", "hidePanelDetail()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
    End Sub

    Protected Sub lbUpdateReceipt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbUpdateReceipt.Click
        If tbRecDate.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter receipt date!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            tbRecDate.Focus()
        ElseIf tbReff.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter reff no!');", True)
            tbReff.Focus()
        ElseIf dlPaid.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select paid by!');", True)
            dlPaid.Focus()
        ElseIf tbAmountcr.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter amount credit!');", True)
            tbAmountcr.Focus()
        Else
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT PaymentID FROM iPxAcctAP_Payment WHERE PaymentID = '" & lbRecID.Text & "'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            If oSQLReader.Read Then
                oSQLReader.Close()
                UpdateReceiptHeader()
                UpdateReceiptDetail()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('data update successfully !');document.getElementById('Buttonx').click()", True)
                Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
                cIpx.saveLog(Session("sBusinessID"), lbRecID.Text, TransDateLog, "AP PAY", "Update", "Update AP Payment " & lbRecID.Text, "", Session("sUserCode"))
                Response.Redirect("iPxAPPayment.aspx")
            Else

            End If
        End If
    End Sub
End Class
