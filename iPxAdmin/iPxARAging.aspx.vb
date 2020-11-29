Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxARAging
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, bln1, bln2, bln3, bln4, forDate, a As String
    Dim cIpx As New iPxClass
    Sub listAging()
        Dim dateBirthday As Date = tbDate.Text
        a = dateBirthday.ToString("yyy-MM-dd")
        bln1 = Format(DateAdd("d", -30, tbDate.Text), "yyy-MM-dd")
        bln2 = Format(DateAdd("d", -60, tbDate.Text), "yyy-MM-dd")
        bln3 = Format(DateAdd("d", -90, tbDate.Text), "yyy-MM-dd")
        bln4 = Format(DateAdd("d", -120, tbDate.Text), "yyy-MM-dd")
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.CustomerID, (c.Description) as arGroup, CoyName, (SELECT (SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and CustomerID = a.CustomerID and (TransDate <= '" & a & "') and isActive='Y' group by CustomerID) as balance, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate > '" & bln1 & "') and (TransDate <= '" & a & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln1 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & a & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as bulan1, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate > '" & bln2 & "') and (TransDate <= '" & bln1 & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln2 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln1 & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as bulan2, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate > '" & bln3 & "') and (TransDate <= '" & bln2 & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln3 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln2 & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as bulan3, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate > '" & bln4 & "') and (TransDate <= '" & bln3 & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln4 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln3 & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as bulan4, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate <= '" & bln4 & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln4 & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as lbh "
        sSQL += "from iPxAcctAR_Cfg_Customer as a "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_CustomerGrp as c ON c.businessid=a.businessid and c.arGroup=a.arGroup "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.arGroup='CC' and "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate <= '" & a & "') and isActive='Y' group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' "
        sSQL += "and (iPxAcctAR_Transaction.TransDate <= '" & a & "') group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID) <>'' "
        sSQL += " order by c.Description, a.CoyName asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvAging.DataSource = dt
                    gvAging.DataBind()
                    'Calculate Sum and display in Footer Row
                    Dim balance, bln1, bln2, bln3, bln4, lbh As Decimal
                    If dt.Compute("Sum(balance)", "").ToString() <> "" Then
                        balance = dt.Compute("Sum(balance)", "").ToString()
                    Else
                        balance = 0
                    End If
                    If dt.Compute("Sum(bulan1)", "").ToString() <> "" Then
                        bln1 = dt.Compute("Sum(bulan1)", "").ToString()
                    Else
                        bln1 = 0
                    End If
                    If dt.Compute("Sum(bulan2)", "").ToString() <> "" Then
                        bln2 = dt.Compute("Sum(bulan2)", "").ToString()
                    Else
                        bln2 = 0
                    End If
                    If dt.Compute("Sum(bulan3)", "").ToString() <> "" Then
                        bln3 = dt.Compute("Sum(bulan3)", "").ToString()
                    Else
                        bln3 = 0
                    End If
                    If dt.Compute("Sum(bulan4)", "").ToString() <> "" Then
                        bln4 = dt.Compute("Sum(bulan4)", "").ToString()
                    Else
                        bln4 = 0
                    End If
                    If dt.Compute("Sum(lbh)", "").ToString() <> "" Then
                        lbh = dt.Compute("Sum(lbh)", "").ToString()
                    Else
                        lbh = 0
                    End If
                    gvAging.FooterRow.Cells(0).Text = "Grand Total"
                    gvAging.FooterRow.Cells(0).HorizontalAlign = HorizontalAlign.Left
                    gvAging.FooterRow.Cells(1).Text = balance.ToString("N2")
                    gvAging.FooterRow.Cells(1).HorizontalAlign = HorizontalAlign.Right
                    gvAging.FooterRow.Cells(2).Text = bln1.ToString("N2")
                    gvAging.FooterRow.Cells(2).HorizontalAlign = HorizontalAlign.Right
                    gvAging.FooterRow.Cells(3).Text = bln2.ToString("N2")
                    gvAging.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Right
                    gvAging.FooterRow.Cells(4).Text = bln3.ToString("N2")
                    gvAging.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right
                    gvAging.FooterRow.Cells(5).Text = bln4.ToString("N2")
                    gvAging.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                    gvAging.FooterRow.Cells(6).Text = lbh.ToString("N2")
                    gvAging.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Right
                    gvAging.Enabled = True
                    gvAging.FooterRow.Visible = True
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvAging.DataSource = dt
                    gvAging.DataBind()
                    gvAging.Rows(0).Visible = False
                    gvAging.Enabled = False
                    gvAging.FooterRow.Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listAgingCL()
        Dim dateBirthday As Date = tbDateCL.Text
        a = dateBirthday.ToString("yyy-MM-dd")
        bln1 = Format(DateAdd("d", -30, tbDate.Text), "yyy-MM-dd")
        bln2 = Format(DateAdd("d", -60, tbDate.Text), "yyy-MM-dd")
        bln3 = Format(DateAdd("d", -90, tbDate.Text), "yyy-MM-dd")
        bln4 = Format(DateAdd("d", -120, tbDate.Text), "yyy-MM-dd")
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.CustomerID, (c.Description) as arGroup, CoyName, (SELECT (SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and CustomerID = a.CustomerID and (TransDate <= '" & a & "') and isActive='Y' group by CustomerID) as balance, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate > '" & bln1 & "') and (TransDate <= '" & a & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln1 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & a & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as bulan1, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate > '" & bln2 & "') and (TransDate <= '" & bln1 & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln2 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln1 & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as bulan2, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate > '" & bln3 & "') and (TransDate <= '" & bln2 & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln3 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln2 & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as bulan3, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate > '" & bln4 & "') and (TransDate <= '" & bln3 & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln4 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln3 & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as bulan4, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate <= '" & bln4 & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln4 & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as lbh "
        sSQL += "from iPxAcctAR_Cfg_Customer as a "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_CustomerGrp as c ON c.businessid=a.businessid and c.arGroup=a.arGroup "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.arGroup<>'CC' and "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate <= '" & a & "') and isActive='Y' group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' "
        sSQL += "and (iPxAcctAR_Transaction.TransDate <= '" & a & "') group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID) <>'' "
        sSQL += " order by c.Description, a.CoyName asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvAgingCL.DataSource = dt
                    gvAgingCL.DataBind()
                    'Calculate Sum and display in Footer Row
                    Dim balance, bln1, bln2, bln3, bln4, lbh As Decimal
                    If dt.Compute("Sum(balance)", "").ToString() <> "" Then
                        balance = dt.Compute("Sum(balance)", "").ToString()
                    Else
                        balance = 0
                    End If
                    If dt.Compute("Sum(bulan1)", "").ToString() <> "" Then
                        bln1 = dt.Compute("Sum(bulan1)", "").ToString()
                    Else
                        bln1 = 0
                    End If
                    If dt.Compute("Sum(bulan2)", "").ToString() <> "" Then
                        bln2 = dt.Compute("Sum(bulan2)", "").ToString()
                    Else
                        bln2 = 0
                    End If
                    If dt.Compute("Sum(bulan3)", "").ToString() <> "" Then
                        bln3 = dt.Compute("Sum(bulan3)", "").ToString()
                    Else
                        bln3 = 0
                    End If
                    If dt.Compute("Sum(bulan4)", "").ToString() <> "" Then
                        bln4 = dt.Compute("Sum(bulan4)", "").ToString()
                    Else
                        bln4 = 0
                    End If
                    If dt.Compute("Sum(lbh)", "").ToString() <> "" Then
                        lbh = dt.Compute("Sum(lbh)", "").ToString()
                    Else
                        lbh = 0
                    End If
                    gvAgingCL.FooterRow.Cells(0).Text = "Grand Total"
                    gvAgingCL.FooterRow.Cells(0).HorizontalAlign = HorizontalAlign.Left
                    gvAgingCL.FooterRow.Cells(1).Text = balance.ToString("N2")
                    gvAgingCL.FooterRow.Cells(1).HorizontalAlign = HorizontalAlign.Right
                    gvAgingCL.FooterRow.Cells(2).Text = bln1.ToString("N2")
                    gvAgingCL.FooterRow.Cells(2).HorizontalAlign = HorizontalAlign.Right
                    gvAgingCL.FooterRow.Cells(3).Text = bln2.ToString("N2")
                    gvAgingCL.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Right
                    gvAgingCL.FooterRow.Cells(4).Text = bln3.ToString("N2")
                    gvAgingCL.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right
                    gvAgingCL.FooterRow.Cells(5).Text = bln4.ToString("N2")
                    gvAgingCL.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                    gvAgingCL.FooterRow.Cells(6).Text = lbh.ToString("N2")
                    gvAgingCL.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Right
                    gvAgingCL.Enabled = True
                    gvAgingCL.FooterRow.Visible = True
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvAgingCL.DataSource = dt
                    gvAgingCL.DataBind()
                    gvAgingCL.Rows(0).Visible = False
                    gvAgingCL.Enabled = False
                    gvAgingCL.FooterRow.Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub clearAging()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        
        sSQL = "DELETE FROM iPxAcctAR_Aging "
        sSQL = sSQL & "WHERE RegBy ='" & Session("sUserCode") & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub CreatAging()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO iPxAcctAR_Aging (businessid,RegBy,CustomerID,balance,bln1,bln2,bln3,bln4,other) "
        sSQL += "select a.businessid, '" & Session("sUserCode") & "', CustomerID, (SELECT (SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and CustomerID = a.CustomerID and (TransDate <= '" & a & "') and isActive='Y' group by CustomerID) as balance, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate > '" & bln1 & "') and (TransDate <= '" & a & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate < '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate >= '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln1 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & a & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as bulan1, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate > '" & bln2 & "') and (TransDate <= '" & bln1 & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate < '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate >= '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln2 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln1 & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as bulan2, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate > '" & bln3 & "') and (TransDate <= '" & bln2 & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate < '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate >= '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln3 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln2 & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as bulan3, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate > '" & bln4 & "') and (TransDate <= '" & bln3 & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate < '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate >= '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln4 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln3 & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as bulan4, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate <= '" & bln4 & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate < '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate >= '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln4 & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as lbh "
        sSQL += "from iPxAcctAR_Cfg_Customer as a "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_CustomerGrp as c ON c.businessid=a.businessid and c.arGroup=a.arGroup "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.arGroup='CC' and "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate <= '" & a & "') and isActive='Y' group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate < '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate >= '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' "
        sSQL += "and (iPxAcctAR_Transaction.TransDate <= '" & a & "') group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID) <>'' "
        sSQL += " order by a.arGroup, a.CoyName asc"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

    Sub CreatAgingCL()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO iPxAcctAR_Aging (businessid,RegBy,CustomerID,balance,bln1,bln2,bln3,bln4,other) "
        sSQL += "select a.businessid, '" & Session("sUserCode") & "', CustomerID, (SELECT (SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and CustomerID = a.CustomerID and (TransDate <= '" & a & "') and isActive='Y' group by CustomerID) as balance, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate > '" & bln1 & "') and (TransDate <= '" & a & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate < '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate >= '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln1 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & a & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as bulan1, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate > '" & bln2 & "') and (TransDate <= '" & bln1 & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate < '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate >= '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln2 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln1 & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as bulan2, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate > '" & bln3 & "') and (TransDate <= '" & bln2 & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate < '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate >= '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln3 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln2 & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as bulan3, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate > '" & bln4 & "') and (TransDate <= '" & bln3 & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate < '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate >= '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln4 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln3 & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as bulan4, "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate <= '" & bln4 & "') and isActive='Y' "
        sSQL += "group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate < '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate >= '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln4 & "') "
        sSQL += "group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID)as lbh "
        sSQL += "from iPxAcctAR_Cfg_Customer as a "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_CustomerGrp as c ON c.businessid=a.businessid and c.arGroup=a.arGroup "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.arGroup<>'CC' and "
        sSQL += "(SELECT SUM(amount) FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = a.CustomerID and (TransDate <= '" & a & "') and isActive='Y' group by CustomerID UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate < '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate >= '" & a & "' THEN '0' END)) AS 'amount' "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = a.CustomerID and iPxAcctAR_Transaction.isActive='Y' "
        sSQL += "and (iPxAcctAR_Transaction.TransDate <= '" & a & "') group by iPxAcctAR_Transaction.CustomerID) a GROUP BY CustomerID) <>'' "
        sSQL += " order by a.arGroup, a.CoyName asc"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

    Sub clearAgingSub()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "DELETE FROM iPxAcctAR_AgingSub "
        sSQL = sSQL & "WHERE businessid='" & Session("sBusinessID") & "' and CustomerID='" & Session("sCustSummaryCC") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub CreatAgingSub()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO iPxAcctAR_AgingSub(businessid,CustomerID,aging,TransID,TransDate,transactiontype,invoiceno,RoomNo,GuestName,amountdr,amountcr) "
        sSQL += "SELECT iPxAcctAR_Transaction.businessid,iPxAcctAR_Transaction.CustomerID, '1 - 30 Days' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(iPxAcctAR_Transaction.amountcr) "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = '" & Session("sCustSummaryCC") & "' and (TransDate > '" & bln1 & "') and (TransDate <= '" & a & "') and isActive='Y' "
        sSQL += "UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.businessid,iPxAcctAR_Transaction.CustomerID, '1 - 30 Days' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END) "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = '" & Session("sCustSummaryCC") & "' and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln1 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & a & "') "
        sSQL += "UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.businessid,iPxAcctAR_Transaction.CustomerID, '31 - 60 Days' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(iPxAcctAR_Transaction.amountcr) "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = '" & Session("sCustSummaryCC") & "' and (TransDate > '" & bln2 & "') and (TransDate <= '" & bln1 & "') and isActive='Y' "
        sSQL += "UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.businessid,iPxAcctAR_Transaction.CustomerID, '31 - 60 Days' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END) "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = '" & Session("sCustSummaryCC") & "' and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln2 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln1 & "') "
        sSQL += "UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.businessid,iPxAcctAR_Transaction.CustomerID, '61 - 90 Days' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(iPxAcctAR_Transaction.amountcr) "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = '" & Session("sCustSummaryCC") & "' and (TransDate > '" & bln3 & "') and (TransDate <= '" & bln2 & "') and isActive='Y' "
        sSQL += "UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.businessid,iPxAcctAR_Transaction.CustomerID, '61 - 90 Days' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END) "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = '" & Session("sCustSummaryCC") & "' and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln3 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln2 & "') "
        sSQL += "UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.businessid,iPxAcctAR_Transaction.CustomerID, '91 - 120 Days' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(iPxAcctAR_Transaction.amountcr) "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = '" & Session("sCustSummaryCC") & "' and (TransDate > '" & bln4 & "') and (TransDate <= '" & bln3 & "') and isActive='Y' "
        sSQL += "UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.businessid,iPxAcctAR_Transaction.CustomerID, '91 - 120 Days' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END) "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = '" & Session("sCustSummaryCC") & "' and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln4 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln3 & "') "
        sSQL += "UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.businessid,iPxAcctAR_Transaction.CustomerID, '121 days more' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(iPxAcctAR_Transaction.amountcr) "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = '" & Session("sCustSummaryCC") & "' and (TransDate <= '" & bln4 & "') and isActive='Y' "
        sSQL += "UNION ALL "
        sSQL += "SELECT iPxAcctAR_Transaction.businessid,iPxAcctAR_Transaction.CustomerID, '121 days more' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END) "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = '" & Session("sCustSummaryCC") & "' and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln4 & "') "
        sSQL += "order by aging,iPxAcctAR_Transaction.TransDate "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

    Sub ListDetailAging()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT '1' as numb, '1 - 30 Days' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, CustomerID,iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(iPxAcctAR_Transaction.amountcr) "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = '" & Session("sCustSummaryCC") & "' and (TransDate > '" & bln1 & "') and (TransDate <= '" & a & "') and isActive='Y' "
        sSQL += "UNION ALL "
        sSQL += "SELECT '1' as numb, '1 - 30 Days' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, iPxAcctAR_Transaction.CustomerID,iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END) "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = '" & Session("sCustSummaryCC") & "' and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln1 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & a & "') "
        sSQL += "UNION ALL "
        sSQL += "SELECT '2' as numb, '31 - 60 Days' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, CustomerID,iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(iPxAcctAR_Transaction.amountcr) "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = '" & Session("sCustSummaryCC") & "' and (TransDate > '" & bln2 & "') and (TransDate <= '" & bln1 & "') and isActive='Y' "
        sSQL += "UNION ALL "
        sSQL += "SELECT '2' as numb, '31 - 60 Days' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, iPxAcctAR_Transaction.CustomerID,iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END) "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = '" & Session("sCustSummaryCC") & "' and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln2 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln1 & "') "
        sSQL += "UNION ALL "
        sSQL += "SELECT '3' as numb, '61 - 90 Days' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, CustomerID,iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(iPxAcctAR_Transaction.amountcr) "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = '" & Session("sCustSummaryCC") & "' and (TransDate > '" & bln3 & "') and (TransDate <= '" & bln2 & "') and isActive='Y' "
        sSQL += "UNION ALL "
        sSQL += "SELECT '3' as numb, '61 - 90 Days' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, iPxAcctAR_Transaction.CustomerID,iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END) "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = '" & Session("sCustSummaryCC") & "' and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln3 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln2 & "') "
        sSQL += "UNION ALL "
        sSQL += "SELECT '4' as numb, '91 - 120 Days' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, CustomerID,iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(iPxAcctAR_Transaction.amountcr) "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = '" & Session("sCustSummaryCC") & "' and (TransDate > '" & bln4 & "') and (TransDate <= '" & bln3 & "') and isActive='Y' "
        sSQL += "UNION ALL "
        sSQL += "SELECT '4' as numb, '91 - 120 Days' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, iPxAcctAR_Transaction.CustomerID,iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END) "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = '" & Session("sCustSummaryCC") & "' and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln4 & "') and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln3 & "') "
        sSQL += "UNION ALL "
        sSQL += "SELECT '5' as numb, '121 days more' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, CustomerID,iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(iPxAcctAR_Transaction.amountcr) "
        sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and CustomerID = '" & Session("sCustSummaryCC") & "' and (TransDate <= '" & bln4 & "') and isActive='Y' "
        sSQL += "UNION ALL "
        sSQL += "SELECT '5' as numb, '121 days more' as aging, iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate, iPxAcctAR_Transaction.CustomerID,iPxAcctAR_Transaction.transactiontype, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.GuestName, (iPxAcctAR_Transaction.amountdr) ,(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END) "
        sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.CustomerID = '" & Session("sCustSummaryCC") & "' and iPxAcctAR_Transaction.isActive='Y' and "
        sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln4 & "') "
        sSQL += "order by numb,iPxAcctAR_Transaction.TransDate asc "
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDetail.DataSource = dt
                    gvDetail.DataBind()
                    Dim CurDebit, CurCredit As Decimal
                    If dt.Compute("Sum(amountdr)", "").ToString() <> "" Then
                        CurDebit = dt.Compute("Sum(amountdr)", "").ToString()
                    Else
                        CurDebit = 0
                    End If
                    If dt.Compute("Sum(amountcr)", "").ToString() <> "" Then
                        CurCredit = dt.Compute("Sum(amountcr)", "").ToString()
                    Else
                        CurCredit = 0
                    End If

                    gvDetail.FooterRow.Cells(6).Text = "Total"
                    gvDetail.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Right
                    gvDetail.FooterRow.Cells(7).Text = CurDebit.ToString("N2")
                    gvDetail.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                    gvDetail.FooterRow.Cells(8).Text = CurCredit.ToString("N2")
                    gvDetail.FooterRow.Cells(8).HorizontalAlign = HorizontalAlign.Right
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvDetail.DataSource = dt
                    gvDetail.DataBind()
                    gvDetail.Rows(0).Visible = False
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
            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "AR Management") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            tbDate.Text = Format(Now, "dd MMMM yyy")
            tbDateCL.Text = Format(Now, "dd MMMM yyy")
            listAging()
            listAgingCL()
        Else
            listAging()
            listAgingCL()
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker1", "$(document).ready(function() {datetimepicker1()});", True)
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvAging.PageIndex = e.NewPageIndex
        Me.listAging()
        gvAgingCL.PageIndex = e.NewPageIndex
        Me.listAgingCL()
    End Sub

    Protected Sub gvAging_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAging.PageIndexChanging
        gvAging.PageIndex = e.NewPageIndex
        tmpCategoryName = ""
        listAging()
    End Sub

    Protected Sub gvAgingCL_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAgingCL.PageIndexChanging
        gvAgingCL.PageIndex = e.NewPageIndex
        tmpCategoryName = ""
        listAgingCL()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CLActive", "CLActive()", True)
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvAging.PageIndex = e.NewPageIndex
        Me.listAging()
        gvAgingCL.PageIndex = e.NewPageIndex
        Me.listAgingCL()
    End Sub

    Protected Sub cari(ByVal sender As Object, ByVal e As EventArgs)
        listAging()
    End Sub
    Protected Sub cariCL(ByVal sender As Object, ByVal e As EventArgs)
        listAgingCL()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CLActive", "CLActive()", True)
    End Sub

    Private tmpCategoryName As String = ""
    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dblbalance, dblbln1, dblbln2, dblbln3, dblbln4, dbllbh As Double
            strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "arGroup").ToString()
            If DataBinder.Eval(e.Row.DataItem, "balance").ToString() <> Nothing Then
                dblbalance = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "balance").ToString())
            End If
            If DataBinder.Eval(e.Row.DataItem, "bulan1").ToString() <> Nothing Then
                dblbln1 = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "bulan1").ToString())
            End If
            If DataBinder.Eval(e.Row.DataItem, "bulan2").ToString() <> Nothing Then
                dblbln2 = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "bulan2").ToString())
            End If
            If DataBinder.Eval(e.Row.DataItem, "bulan3").ToString() <> Nothing Then
                dblbln3 = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "bulan3").ToString())
            End If
            If DataBinder.Eval(e.Row.DataItem, "bulan4").ToString() <> Nothing Then
                dblbln4 = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "bulan4").ToString())
            End If
            If DataBinder.Eval(e.Row.DataItem, "lbh").ToString() <> Nothing Then
                dbllbh = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "lbh").ToString())
            End If

            'Dim lblTotalRevenue As Label = DirectCast(e.Row.FindControl("lblTotalRevenue"), Label)
            'lblTotalRevenue.Text = String.Format("{0:0.00}", (dblDirectRevenue + dblReferralRevenue))

            dblSubTotalBalance += dblbalance
            dblSubTotalbln1 += dblbln1
            dblSubTotalbln2 += dblbln2
            dblSubTotalbln3 += dblbln3
            dblSubTotalbln4 += dblbln4
            dblSubTotallbh += dbllbh
        End If
    End Sub
    ' To keep track of the previous row Group Identifier
    Dim strPreviousRowID As String = String.Empty
    ' To keep track the Index of Group Total
    Dim intSubTotalIndex As Integer = 1
    Dim group As Integer = 0

    ' To temporarily store Group Total
    Dim dblSubTotalbln1 As Double = 0
    Dim dblSubTotalbln2 As Double = 0
    Dim dblSubTotalbln3 As Double = 0
    Dim dblSubTotalbln4 As Double = 0
    Dim dblSubTotallbh As Double = 0
    Dim dblSubTotalBalance As Double = 0
    'Protected Sub gvAging_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAging.RowCreated
    '    Dim IsSubTotalRowNeedToAdd As Boolean = False
    '    If (strPreviousRowID <> String.Empty) AndAlso (DataBinder.Eval(e.Row.DataItem, "arGroup") IsNot Nothing) Then
    '        If strPreviousRowID <> DataBinder.Eval(e.Row.DataItem, "arGroup").ToString() Then
    '            IsSubTotalRowNeedToAdd = True
    '        End If
    '    End If

    '    If (strPreviousRowID <> String.Empty) AndAlso (DataBinder.Eval(e.Row.DataItem, "arGroup") Is Nothing) Then
    '        IsSubTotalRowNeedToAdd = True
    '        intSubTotalIndex = 0
    '    End If

    '    If IsSubTotalRowNeedToAdd Then
    '        Dim grdViewProducts As GridView = DirectCast(sender, GridView)

    '        ' Creating a Row
    '        Dim SubTotalRow As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

    '        'Adding Total Cell 
    '        Dim HeaderCell As New TableCell()
    '        HeaderCell.Text = "Sub Total"
    '        HeaderCell.HorizontalAlign = HorizontalAlign.Left
    '        'HeaderCell.ColumnSpan = 3
    '        ' For merging first, second row cells to one
    '        HeaderCell.CssClass = "SubTotalRowStyle"
    '        SubTotalRow.Cells.Add(HeaderCell)

    '        'Adding Direct Revenue Column
    '        HeaderCell = New TableCell()
    '        HeaderCell.Text = String.Format("{0:N2}", dblSubTotalBalance)
    '        HeaderCell.HorizontalAlign = HorizontalAlign.Right
    '        HeaderCell.CssClass = "SubTotalRowStyle"
    '        SubTotalRow.Cells.Add(HeaderCell)

    '        'Adding Referral Revenue Column
    '        HeaderCell = New TableCell()
    '        HeaderCell.Text = String.Format("{0:N2}", dblSubTotalbln1)
    '        HeaderCell.HorizontalAlign = HorizontalAlign.Right
    '        HeaderCell.CssClass = "SubTotalRowStyle"
    '        SubTotalRow.Cells.Add(HeaderCell)

    '        'Adding Total Revenue Column
    '        HeaderCell = New TableCell()
    '        HeaderCell.Text = String.Format("{0:N2}", dblSubTotalbln2)
    '        HeaderCell.HorizontalAlign = HorizontalAlign.Right
    '        HeaderCell.CssClass = "SubTotalRowStyle"
    '        SubTotalRow.Cells.Add(HeaderCell)

    '        'Adding Total Revenue Column
    '        HeaderCell = New TableCell()
    '        HeaderCell.Text = String.Format("{0:N2}", dblSubTotalbln3)
    '        HeaderCell.HorizontalAlign = HorizontalAlign.Right
    '        HeaderCell.CssClass = "SubTotalRowStyle"
    '        SubTotalRow.Cells.Add(HeaderCell)

    '        'Adding Total Revenue Column
    '        HeaderCell = New TableCell()
    '        HeaderCell.Text = String.Format("{0:N2}", dblSubTotalbln4)
    '        HeaderCell.HorizontalAlign = HorizontalAlign.Right
    '        HeaderCell.CssClass = "SubTotalRowStyle"
    '        SubTotalRow.Cells.Add(HeaderCell)

    '        'Adding Total Revenue Column
    '        HeaderCell = New TableCell()
    '        HeaderCell.Text = String.Format("{0:N2}", dblSubTotallbh)
    '        HeaderCell.HorizontalAlign = HorizontalAlign.Right
    '        HeaderCell.CssClass = "SubTotalRowStyle"
    '        SubTotalRow.Cells.Add(HeaderCell)

    '        'Adding the Row at the RowIndex position in the Grid
    '        If e.Row.RowIndex > 0 Then
    '            If e.Row.RowIndex = group Then
    '                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex + 1, SubTotalRow)
    '            ElseIf e.Row.RowIndex < group Then
    '                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex + 1, SubTotalRow)
    '            ElseIf e.Row.RowIndex > group Then
    '                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + group + intSubTotalIndex, SubTotalRow)
    '            End If
    '        ElseIf e.Row.RowIndex <= 0 Then
    '            grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, SubTotalRow)
    '        End If
    '        intSubTotalIndex += 1

    '        dblSubTotalbln1 = 0
    '        dblSubTotalbln2 = 0
    '        dblSubTotalbln3 = 0
    '        dblSubTotalbln4 = 0
    '        dblSubTotallbh = 0
    '        dblSubTotalBalance = 0
    '        strPreviousRowID = String.Empty
    '    End If
    'End Sub

    Protected Sub lbPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPrint.Click
        clearAging()
        CreatAging()
        Session("sReport") = "ARAging"
        'Session("iUserID")
        Dim dateBirthday As Date = tbDate.Text
        Session("sForDate") = dateBirthday.ToString("yyy-MM-dd")
        'Session("filename") = "dckCashierSummary_Form.rpt"
        Session("sMapPath") = "~/iPxReportFile/dckAR_Aging.rpt"
        'Session("sFOLink") = txtP2.Text
        Response.Redirect("rptviewer.aspx")
    End Sub

    Protected Sub gvAging_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAging.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sCustSummaryCC") = e.CommandArgument
            lbTitleDetailCoa.Text = "Detail Aging Credit Card"
            Dim dateBirthday As Date = tbDate.Text
            forDate = dateBirthday.ToString("yyy-MM-")
            a = dateBirthday.ToString("yyy-MM-dd")
            ListDetailAging()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CLActive", "CLActive()", True)
    End Sub

    Protected Sub gvAgingCL_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAgingCL.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sCustSummaryCC") = e.CommandArgument
            lbTitleDetailCoa.Text = "Detail Aging Travel Agent/Company"
            Dim dateBirthday As Date = tbDateCL.Text
            forDate = dateBirthday.ToString("yyy-MM-")
            a = dateBirthday.ToString("yyy-MM-dd")
            ListDetailAging()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        End If
    End Sub

    Protected Sub lbAbortDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDetail.Click
        If lbTitleDetailCoa.Text = "Detail Aging Credit Card" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CLActive", "CLActive()", True)
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
    End Sub

    Protected Sub lbPrintDtl_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPrintDtl.Click
        clearAgingSub()
        CreatAgingSub()
        'Session("sReport") = "ARAgingSub"
        ''Session("iUserID")
        'Dim dateBirthday As Date = tbDate.Text
        'Session("sForDate") = dateBirthday.ToString("yyy-MM-dd")
        ''Session("filename") = "dckCashierSummary_Form.rpt"
        'Session("sMapPath") = "~/iPxReportFile/dckAR_Aging.rpt"
        ''Session("sFOLink") = txtP2.Text
        'Response.Redirect("rptviewer.aspx")
    End Sub

    Protected Sub lbPrintCL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPrintCL.Click
        clearAging()
        CreatAgingCL()
        Session("sReport") = "ARAging"
        'Session("iUserID")
        Dim dateBirthday As Date = tbDateCL.Text
        Session("sForDate") = dateBirthday.ToString("yyy-MM-dd")
        'Session("filename") = "dckCashierSummary_Form.rpt"
        Session("sMapPath") = "~/iPxReportFile/dckAR_Aging.rpt"
        'Session("sFOLink") = txtP2.Text
        Response.Redirect("rptviewer.aspx")
    End Sub
End Class
