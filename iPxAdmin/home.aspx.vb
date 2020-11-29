
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Imports System.Web.Services
Imports System.Web.Script.Serialization
Imports System.Collections.Generic

Partial Class iPxAdmin_home
    Inherits System.Web.UI.Page

    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Public oCnct As SqlConnection = New SqlConnection(sCnct)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL As String
    <System.Web.Services.WebMethod()> _
    Public Shared Function ChartAging() As String
        Dim businessID = HttpContext.Current.Session("sBusinessID").ToString()
        Dim Order As New List(Of Object)()
        Dim sSQL As String
        Dim a, bln1, bln2, bln3, bln4 As String
        a = Format(Now, "yyy-MM-dd")
        bln1 = Format(DateAdd("d", -30, a), "yyy-MM-dd")
        bln2 = Format(DateAdd("d", -60, a), "yyy-MM-dd")
        bln3 = Format(DateAdd("d", -90, a), "yyy-MM-dd")
        bln4 = Format(DateAdd("d", -120, a), "yyy-MM-dd")
        Using conn As New SqlConnection()
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("iPxCNCT").ConnectionString
            Using cmd As New SqlCommand()
                sSQL = "select businessid,  "
                sSQL += "(SELECT SUM(amount) FROM (SELECT businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount'  "
                sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & businessID & "' and invoiceno ='' and (TransDate > '" & bln1 & "') and isActive='Y' "
                sSQL += "and (TransDate <= '" & a & "') group by businessid  "
                sSQL += "UNION ALL  "
                sSQL += "SELECT iPxAcctAR_Transaction.businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount'  "
                sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo  "
                sSQL += "where iPxAcctAR_Transaction.businessid ='" & businessID & "' and iPxAcctAR_Transaction.invoiceno <> ''  and iPxAcctAR_Transaction.isActive='Y' "
                sSQL += "and (iPxAcctAR_Invoice.InvDate > '" & bln1 & "') and (iPxAcctAR_Invoice.InvDate <= '" & a & "') group by iPxAcctAR_Transaction.businessid) a  "
                sSQL += "GROUP BY businessid)as bulan1,  "
                sSQL += "(SELECT SUM(amount) FROM (SELECT businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount'  "
                sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & businessID & "' and invoiceno ='' and (TransDate > '" & bln2 & "')  and isActive='Y' "
                sSQL += "and (TransDate <= '" & bln1 & "') group by businessid  "
                sSQL += "UNION ALL  "
                sSQL += "SELECT iPxAcctAR_Transaction.businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount'  "
                sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo  "
                sSQL += "where iPxAcctAR_Transaction.businessid ='" & businessID & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.isActive='Y' "
                sSQL += "and (iPxAcctAR_Invoice.InvDate > '" & bln2 & "') and (iPxAcctAR_Invoice.InvDate <= '" & bln1 & "') group by iPxAcctAR_Transaction.businessid) a  "
                sSQL += "GROUP BY businessid)as bulan2,  "
                sSQL += "(SELECT SUM(amount) FROM (SELECT businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount'  "
                sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & businessID & "' and invoiceno ='' and (TransDate > '" & bln3 & "')  and isActive='Y' "
                sSQL += "and (TransDate <= '" & bln2 & "') group by businessid  "
                sSQL += "UNION ALL  "
                sSQL += "SELECT iPxAcctAR_Transaction.businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount'  "
                sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo  "
                sSQL += "where iPxAcctAR_Transaction.businessid ='" & businessID & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.isActive='Y' "
                sSQL += "and (iPxAcctAR_Invoice.InvDate > '" & bln3 & "') and (iPxAcctAR_Invoice.InvDate <= '" & bln2 & "') group by iPxAcctAR_Transaction.businessid) a  "
                sSQL += "GROUP BY businessid)as bulan3,  "
                sSQL += "(SELECT SUM(amount) FROM (SELECT businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
                sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & businessID & "' and invoiceno ='' and (TransDate > '" & bln4 & "')  and isActive='Y' "
                sSQL += "and (TransDate <= '" & bln3 & "') group by businessid  "
                sSQL += "UNION ALL  "
                sSQL += "SELECT iPxAcctAR_Transaction.businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount'  "
                sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo  "
                sSQL += "where iPxAcctAR_Transaction.businessid ='" & businessID & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.isActive='Y' "
                sSQL += "and (iPxAcctAR_Invoice.InvDate > '" & bln4 & "') and (iPxAcctAR_Invoice.InvDate <= '" & bln3 & "') group by iPxAcctAR_Transaction.businessid) a  "
                sSQL += "GROUP BY businessid)as bulan4,  "
                sSQL += "(SELECT SUM(amount) FROM (SELECT businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount'  "
                sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & businessID & "' and invoiceno ='' and (TransDate <= '" & bln4 & "') and isActive='Y' group by businessid  "
                sSQL += "UNION ALL  "
                sSQL += "SELECT iPxAcctAR_Transaction.businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount'  "
                sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo  "
                sSQL += "where iPxAcctAR_Transaction.businessid ='" & businessID & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.isActive='Y' "
                sSQL += "and (iPxAcctAR_Invoice.InvDate <= '" & bln4 & "') group by iPxAcctAR_Transaction.businessid) a  "
                sSQL += "GROUP BY businessid)as lbh "

                sSQL += "from iPxAcctAR_Cfg_Customer as a WHERE a.businessid='" & businessID & "' group by a.businessid "

                cmd.CommandText = sSQL
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        Order.Add(New With {.Amount30 = sdr("bulan1"), .Amount60 = sdr("bulan2"), .Amount90 = sdr("bulan3"), .Amount120 = sdr("bulan4"), .Amountlbh = sdr("lbh")})
                    End While
                End Using
                conn.Close()
            End Using
            Return (New JavaScriptSerializer().Serialize(Order))
        End Using
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function ChartSalesRev() As String
        Dim businessID = HttpContext.Current.Session("sBusinessID").ToString()
        Dim Order As New List(Of Object)()
        Dim sSQL As String
        Dim a, b, c As String
        a = Format(Now, "yyy-MM-dd")
        b = Format(Now, "yyy-MM-")
        c = Format(Now, "MM-yyy")
        Using conn As New SqlConnection()
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("iPxCNCT").ConnectionString
            Using cmd As New SqlCommand()
                sSQL = "select businessid, "
                sSQL += "(select sum(y.revenue) from iPxPMS_postingrecord as y INNER JOIN iPxPMS_cfg_pos as x ON x.businessid=y.businessid and x.poscode=y.poscode "
                sSQL += "where y.businessid in(SELECT FoLink FROM iPxAcct_FOlink WHERE (businessid = 'Q5C1SX')) and y.auditdate >= '" & b & "01' and y.auditdate <= '" & a & "' and x.revenuegroup='RO' )as ActMtdRO, "
                sSQL += "(select sum(z.Revenue_Bud) from iPxAcctSales_Budget z INNER JOIN iPxPMS_cfg_pos as x ON z.Code=x.poscode and z.pmsID=x.businessid where z.businessid='Q5C1SX' AND z.date = '" & c & "' and x.revenuegroup='RO' )as BudgetMtdRO, "
                sSQL += "(select sum(y.revenue) from iPxPMS_postingrecord as y INNER JOIN iPxPMS_cfg_pos as x ON x.businessid=y.businessid and x.poscode=y.poscode "
                sSQL += "where y.businessid in(SELECT FoLink FROM iPxAcct_FOlink WHERE (businessid = 'Q5C1SX')) and y.auditdate >= '" & b & "01' and y.auditdate <= '" & a & "' and x.revenuegroup='FB' )as ActMtdFB, "
                sSQL += "(select sum(z.Revenue_Bud) from iPxAcctSales_Budget z INNER JOIN iPxPMS_cfg_pos as x ON z.Code=x.poscode and z.pmsID=x.businessid where z.businessid='Q5C1SX' AND z.date = '" & c & "' and x.revenuegroup='FB' )as BudgetMtdFB, "
                sSQL += "(select sum(y.revenue) from iPxPMS_postingrecord as y INNER JOIN iPxPMS_cfg_pos as x ON x.businessid=y.businessid and x.poscode=y.poscode "
                sSQL += "where y.businessid in(SELECT FoLink FROM iPxAcct_FOlink WHERE (businessid = 'Q5C1SX')) and y.auditdate >= '" & b & "01' and y.auditdate <= '" & a & "' and x.revenuegroup<>'RO' and x.revenuegroup<>'FB' and x.revenuegroup<>'SY' )as ActMtdOther, "
                sSQL += "(select sum(z.Revenue_Bud) from iPxAcctSales_Budget z INNER JOIN iPxPMS_cfg_pos as x ON z.Code=x.poscode and z.pmsID=x.businessid where z.businessid='Q5C1SX' AND z.date = '" & c & "' and x.revenuegroup<>'RO' and x.revenuegroup<>'FB' and x.revenuegroup<>'SY' )as BudgetMtdOther "
                sSQL += "from iPxAcct_FOlink where businessid='" & businessID & "' group by businessid"
                cmd.CommandText = sSQL
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        Order.Add(New With {.ActMtdRO = sdr("ActMtdRO"), .BudgetMtdRO = sdr("BudgetMtdRO"), .ActMtdFB = sdr("ActMtdFB"), .BudgetMtdFB = sdr("BudgetMtdFB"), .ActMtdOther = sdr("ActMtdOther"), .BudgetMtdOther = sdr("BudgetMtdOther")})
                    End While
                End Using
                conn.Close()
            End Using
            Return (New JavaScriptSerializer().Serialize(Order))
        End Using
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function ChartSalesSet() As String
        Dim businessID = HttpContext.Current.Session("sBusinessID").ToString()
        Dim Order As New List(Of Object)()
        Dim sSQL As String
        Dim a, b, c As String
        a = Format(Now, "yyy-MM-dd")
        b = Format(Now, "yyy-MM-")
        Using conn As New SqlConnection()
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("iPxCNCT").ConnectionString
            Using cmd As New SqlCommand()
                sSQL = "select businessid,"
                sSQL += "(select CASE WHEN sum(y.amount) < 0 THEN sum(y.amount*-1) WHEN sum(y.amount) >=0 THEN sum(y.amount) END as amount from iPxPMS_postingrecord as y where y.businessid in(SELECT FoLink FROM iPxAcct_FOlink WHERE (businessid = '" & businessID & "')) "
                sSQL += "and y.auditdate >= '" & b & "01' and y.auditdate <= '" & a & "' and poscode <> 'SYDP')as GL, "
                sSQL += "(select CASE WHEN sum(y.amount) < 0 THEN sum(y.amount*-1) WHEN sum(y.amount) >=0 THEN sum(y.amount) END as amount from iPxPMS_postingrecord as y where y.businessid in(SELECT FoLink FROM iPxAcct_FOlink WHERE (businessid = '" & businessID & "')) "
                sSQL += "and y.auditdate >= '" & b & "01' and y.auditdate <= '" & a & "' and paymenttype='CS')as CS, "
                sSQL += "(select CASE WHEN sum(y.amount) < 0 THEN sum(y.amount*-1) WHEN sum(y.amount) >=0 THEN sum(y.amount) END as amount from iPxPMS_postingrecord as y where y.businessid in(SELECT FoLink FROM iPxAcct_FOlink WHERE (businessid = '" & businessID & "')) "
                sSQL += "and y.auditdate >= '" & b & "01' and y.auditdate <= '" & a & "' and paymenttype='CR')as CR, "
                sSQL += "(select CASE WHEN sum(y.amount) < 0 THEN sum(y.amount*-1) WHEN sum(y.amount) >=0 THEN sum(y.amount) END as amount from iPxPMS_postingrecord as y where y.businessid in(SELECT FoLink FROM iPxAcct_FOlink WHERE (businessid = '" & businessID & "')) "
                sSQL += "and y.auditdate >= '" & b & "01' and y.auditdate <= '" & a & "' and paymenttype='CL')as CL, "
                sSQL += "(select CASE WHEN sum(y.amount) < 0 THEN sum(y.amount*-1) WHEN sum(y.amount) >=0 THEN sum(y.amount) END as amount from iPxPMS_postingrecord as y where y.businessid in(SELECT FoLink FROM iPxAcct_FOlink WHERE (businessid = '" & businessID & "')) "
                sSQL += "and y.auditdate >= '" & b & "01' and y.auditdate <= '" & a & "' and paymenttype='WB')as WB "
                sSQL += "from iPxAcct_FOlink where businessid='" & businessID & "' AND FoLink in(SELECT        FoLink "
                sSQL += "FROM iPxAcct_FOlink "
                sSQL += "WHERE        (businessid = '" & businessID & "')) group by businessid"
                cmd.CommandText = sSQL
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        Order.Add(New With {.GL = sdr("GL"), .CS = sdr("CS"), .CR = sdr("CR"), .CL = sdr("CL"), .WB = sdr("WB")})
                    End While
                End Using
                conn.Close()
            End Using
            Return (New JavaScriptSerializer().Serialize(Order))
        End Using
    End Function
    'Sub tampilAmountBlnIni()
    '    If oCnct.State = ConnectionState.Closed Then
    '        oCnct.Open()
    '    End If

    '    'Dim a, bln1, bln2, bln3, bln4 As String
    '    'a = Format(Now, "yyy-MM-dd")
    '    'bln1 = Format(DateAdd("d", -30, a), "yyy-MM-dd")
    '    'bln2 = Format(DateAdd("d", -60, a), "yyy-MM-dd")
    '    'bln3 = Format(DateAdd("d", -90, a), "yyy-MM-dd")
    '    'bln4 = Format(DateAdd("d", -120, a), "yyy-MM-dd")
    '    oSQLCmd = New SqlCommand(sSQL, oCnct)
    '    sSQL = "select sum(amountdr) as invoiceBlnIni, sum(amountcr) as receiptBlnIni, (sum(amountdr) - sum(amountcr)) As Balance from iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno<>'' and month(Transdate)='" & Format(Now, "MM") & "' and year(Transdate)='" & Format(Now, "yyy") & "'"
    '    'sSQL = "select businessid,  "
    '    'sSQL += "(SELECT SUM(amount) FROM (SELECT businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount'  "
    '    'sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and (TransDate > '" & bln1 & ")  "
    '    'sSQL += "and (TransDate <= '" & a & "') group by businessid  "
    '    'sSQL += "UNION ALL  "
    '    'sSQL += "SELECT iPxAcctAR_Transaction.businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount'  "
    '    'sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo  "
    '    'sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> ''  "
    '    'sSQL += "and (iPxAcctAR_Invoice.InvDate > '" & bln1 & "') and (iPxAcctAR_Invoice.InvDate <= '" & a & "') group by iPxAcctAR_Transaction.businessid) a  "
    '    'sSQL += "GROUP BY businessid)as bulan1,  "
    '    'sSQL += "(SELECT SUM(amount) FROM (SELECT businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount'  "
    '    'sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and (TransDate > '" & bln2 & "')  "
    '    'sSQL += "and (TransDate <= '" & bln1 & "') group by businessid  "
    '    'sSQL += "UNION ALL  "
    '    'sSQL += "SELECT iPxAcctAR_Transaction.businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount'  "
    '    'sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo  "
    '    'sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> ''  "
    '    'sSQL += "and (iPxAcctAR_Invoice.InvDate > '" & bln2 & "') and (iPxAcctAR_Invoice.InvDate <= '" & bln1 & "') group by iPxAcctAR_Transaction.businessid) a  "
    '    'sSQL += "GROUP BY businessid)as bulan2,  "
    '    'sSQL += "(SELECT SUM(amount) FROM (SELECT businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount'  "
    '    'sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and (TransDate > '" & bln3 & "')  "
    '    'sSQL += "and (TransDate <= '" & bln3 & "') group by businessid  "
    '    'sSQL += "UNION ALL  "
    '    'sSQL += "SELECT iPxAcctAR_Transaction.businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount'  "
    '    'sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo  "
    '    'sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> ''  "
    '    'sSQL += "and (iPxAcctAR_Invoice.InvDate > '" & bln3 & "') and (iPxAcctAR_Invoice.InvDate <= '" & bln2 & "') group by iPxAcctAR_Transaction.businessid) a  "
    '    'sSQL += "GROUP BY businessid)as bulan3,  "
    '    'sSQL += "(SELECT SUM(amount) FROM (SELECT businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
    '    'sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and (TransDate > '" & bln4 & "')  "
    '    'sSQL += "and (TransDate <= '" & bln3 & "') group by businessid  "
    '    'sSQL += "UNION ALL  "
    '    'sSQL += "SELECT iPxAcctAR_Transaction.businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount'  "
    '    'sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo  "
    '    'sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> ''  "
    '    'sSQL += "and (iPxAcctAR_Invoice.InvDate > '" & bln4 & "') and (iPxAcctAR_Invoice.InvDate <= '" & bln3 & "') group by iPxAcctAR_Transaction.businessid) a  "
    '    'sSQL += "GROUP BY businessid)as bulan4,  "
    '    'sSQL += "(SELECT SUM(amount) FROM (SELECT businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount'  "
    '    'sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and (TransDate <= '" & bln4 & "') group by businessid  "
    '    'sSQL += "UNION ALL  "
    '    'sSQL += "SELECT iPxAcctAR_Transaction.businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount'  "
    '    'sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo  "
    '    'sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> ''  "
    '    'sSQL += "and (TransDate <= '" & bln4 & "') group by iPxAcctAR_Transaction.businessid) a  "
    '    'sSQL += "GROUP BY businessid)as lbh "

    '    'sSQL += "from iPxAcctAR_Cfg_Customer as a WHERE a.businessid='" & Session("sBusinessID") & " group by a.businessid "
    '    oSQLCmd.CommandText = sSQL
    '    oSQLReader = oSQLCmd.ExecuteReader

    '    oSQLReader.Read()
    '    If oSQLReader.HasRows Then
    '        'lbInvBlnIni.Text = String.Format("{0:N2}", (oSQLReader.Item("invoiceBlnIni"))).ToString
    '        lbRecBlnIni.Text = String.Format("{0:N2}", (oSQLReader.Item("receiptBlnIni"))).ToString
    '        'If lbInvBlnIni.Text = "" Then
    '        '    lbInvBlnIni.Text = "0.00"
    '        'End If
    '        oCnct.Close()
    '    Else
    '        oCnct.Close()
    '    End If
    'End Sub
    'Sub RecBlnLalu()
    '    Dim bln, thn As String
    '    If Format(Now, "MM") = 1 Then
    '        bln = "12"
    '        thn = Format(Now, "yyy") - 1
    '    Else
    '        bln = Format(Now, "MM") - 1
    '        thn = Format(Now, "yyy")
    '    End If
    '    If oCnct.State = ConnectionState.Closed Then
    '        oCnct.Open()
    '    End If
    '    oSQLCmd = New SqlCommand(sSQL, oCnct)
    '    sSQL = "select sum(amountcr) as receiptBlnlalu from iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno<>'' and month(Transdate)='" & bln & "' and year(Transdate)='" & thn & "'"
    '    oSQLCmd.CommandText = sSQL
    '    oSQLReader = oSQLCmd.ExecuteReader

    '    oSQLReader.Read()
    '    If oSQLReader.HasRows Then
    '        If String.Format("{0:N2}", (oSQLReader.Item("receiptBlnlalu"))).ToString = "" Then
    '            lbRecBlnLalu.Text = "0.00"
    '        Else
    '            lbRecBlnLalu.Text = String.Format("{0:N2}", (oSQLReader.Item("receiptBlnlalu"))).ToString
    '        End If
    '        oCnct.Close()
    '    Else
    '        lbRecBlnLalu.Text = "0.00"
    '        oCnct.Close()
    '    End If
    'End Sub
    'Sub InvBlnDpn()
    '    'Dim bln, thn As String
    '    'If Format(Now, "MM") = 12 Then
    '    '    bln = "1"
    '    '    thn = Format(Now, "yyy") + 1
    '    'Else
    '    '    bln = Format(Now, "MM") + 1
    '    '    thn = Format(Now, "yyy")
    '    'End If
    '    'If oCnct.State = ConnectionState.Closed Then
    '    '    oCnct.Open()
    '    'End If
    '    'oSQLCmd = New SqlCommand(sSQL, oCnct)
    '    'sSQL = "select sum(amountdr) as invoiceBlnDpn from iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno<>'' and month(Transdate)='" & bln & "' and year(Transdate)='" & thn & "'"
    '    'oSQLCmd.CommandText = sSQL
    '    'oSQLReader = oSQLCmd.ExecuteReader

    '    'oSQLReader.Read()
    '    'If oSQLReader.HasRows Then
    '    '    If String.Format("{0:N2}", (oSQLReader.Item("invoiceBlnDpn"))).ToString = "" Then
    '    '        lbInvBlnDpn.Text = "0.00"
    '    '    Else
    '    '        lbInvBlnDpn.Text = String.Format("{0:N2}", (oSQLReader.Item("invoiceBlnDpn"))).ToString
    '    '    End If
    '    '    oCnct.Close()
    '    'Else
    '    '    lbRecBlnLalu.Text = "0.00"
    '    '    oCnct.Close()
    '    'End If
    'End Sub
    'Sub ARAging()
    '    Dim a, bln1, bln2, bln3, bln4 As String
    '    a = Format(Now, "yyy-MM-dd")
    '    bln1 = Format(DateAdd("d", -30, a), "yyy-MM-dd")
    '    bln2 = Format(DateAdd("d", -60, a), "yyy-MM-dd")
    '    bln3 = Format(DateAdd("d", -90, a), "yyy-MM-dd")
    '    bln4 = Format(DateAdd("d", -120, a), "yyy-MM-dd")
    '    'kurang dari samadengan 30 hari
    '    If oCnct.State = ConnectionState.Closed Then
    '        oCnct.Open()
    '    End If
    '    oSQLCmd = New SqlCommand(sSQL, oCnct)
    '    sSQL = "SELECT SUM(amount) as amount FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
    '    sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and (TransDate > '" & bln1 & "') and (TransDate <= '" & a & "') "
    '    sSQL += "group by CustomerID UNION ALL "
    '    sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
    '    sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
    '    sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and (iPxAcctAR_Invoice.InvDate > '" & bln1 & "') and (iPxAcctAR_Invoice.InvDate <= '" & a & "') "
    '    sSQL += "group by iPxAcctAR_Transaction.CustomerID ) a "
    '    oSQLCmd.CommandText = sSQL
    '    oSQLReader = oSQLCmd.ExecuteReader

    '    oSQLReader.Read()
    '    If oSQLReader.HasRows Then
    '        lblAging30.Text = String.Format("{0:N2}", (oSQLReader.Item("amount"))).ToString
    '        If lblAging30.Text = "" Then
    '            lblAging30.Text = "0.00"
    '        End If
    '    End If
    '    oCnct.Close()

    '    'kurang dari samadengan 60 hari
    '    If oCnct.State = ConnectionState.Closed Then
    '        oCnct.Open()
    '    End If
    '    oSQLCmd = New SqlCommand(sSQL, oCnct)
    '    sSQL = "SELECT SUM(amount) as amount FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
    '    sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and (TransDate > '" & bln2 & "') and (TransDate <= '" & bln1 & "') "
    '    sSQL += "group by CustomerID UNION ALL "
    '    sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
    '    sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
    '    sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and (iPxAcctAR_Invoice.InvDate > '" & bln2 & "') and (iPxAcctAR_Invoice.InvDate <= '" & bln1 & "') "
    '    sSQL += "group by iPxAcctAR_Transaction.CustomerID ) a "
    '    oSQLCmd.CommandText = sSQL
    '    oSQLReader = oSQLCmd.ExecuteReader

    '    oSQLReader.Read()
    '    If oSQLReader.HasRows Then
    '        lblAging60.Text = String.Format("{0:N2}", (oSQLReader.Item("amount"))).ToString
    '        If lblAging60.Text = "" Then
    '            lblAging60.Text = "0.00"
    '        End If
    '    End If
    '    oCnct.Close()

    '    'kurang dari samadengan 90 hari
    '    If oCnct.State = ConnectionState.Closed Then
    '        oCnct.Open()
    '    End If
    '    oSQLCmd = New SqlCommand(sSQL, oCnct)
    '    sSQL = "SELECT SUM(amount) as amount FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
    '    sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and (TransDate > '" & bln3 & "') and (TransDate <= '" & bln2 & "') "
    '    sSQL += "group by CustomerID UNION ALL "
    '    sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
    '    sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
    '    sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and (iPxAcctAR_Invoice.InvDate > '" & bln3 & "') and (iPxAcctAR_Invoice.InvDate <= '" & bln2 & "') "
    '    sSQL += "group by iPxAcctAR_Transaction.CustomerID ) a "
    '    oSQLCmd.CommandText = sSQL
    '    oSQLReader = oSQLCmd.ExecuteReader

    '    oSQLReader.Read()
    '    If oSQLReader.HasRows Then
    '        lblAging90.Text = String.Format("{0:N2}", (oSQLReader.Item("amount"))).ToString
    '        If lblAging90.Text = "" Then
    '            lblAging90.Text = "0.00"
    '        End If
    '    End If
    '    oCnct.Close()

    '    'kurang dari samadengan 60 hari
    '    If oCnct.State = ConnectionState.Closed Then
    '        oCnct.Open()
    '    End If
    '    oSQLCmd = New SqlCommand(sSQL, oCnct)
    '    sSQL = "SELECT SUM(amount) as amount FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
    '    sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and (TransDate > '" & bln4 & "') and (TransDate <= '" & bln3 & "') "
    '    sSQL += "group by CustomerID UNION ALL "
    '    sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
    '    sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
    '    sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and (iPxAcctAR_Invoice.InvDate > '" & bln4 & "') and (iPxAcctAR_Invoice.InvDate <= '" & bln3 & "') "
    '    sSQL += "group by iPxAcctAR_Transaction.CustomerID ) a "
    '    oSQLCmd.CommandText = sSQL
    '    oSQLReader = oSQLCmd.ExecuteReader

    '    oSQLReader.Read()
    '    If oSQLReader.HasRows Then
    '        lblAging120.Text = String.Format("{0:N2}", (oSQLReader.Item("amount"))).ToString
    '        If lblAging120.Text = "" Then
    '            lblAging120.Text = "0.00"
    '        End If
    '    End If
    '    oCnct.Close()

    '    'lebih dari 120 hari
    '    If oCnct.State = ConnectionState.Closed Then
    '        oCnct.Open()
    '    End If
    '    oSQLCmd = New SqlCommand(sSQL, oCnct)
    '    sSQL = "SELECT SUM(amount) as amount FROM (SELECT CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
    '    sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & Session("sBusinessID") & "' and invoiceno ='' and (TransDate <= '" & bln4 & "') "
    '    sSQL += "group by CustomerID UNION ALL "
    '    sSQL += "SELECT iPxAcctAR_Transaction.CustomerID,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
    '    sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
    '    sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.invoiceno <> '' and (iPxAcctAR_Invoice.InvDate <= '" & bln4 & "') "
    '    sSQL += "group by iPxAcctAR_Transaction.CustomerID ) a "
    '    oSQLCmd.CommandText = sSQL
    '    oSQLReader = oSQLCmd.ExecuteReader

    '    oSQLReader.Read()
    '    If oSQLReader.HasRows Then
    '        lblAging121.Text = String.Format("{0:N2}", (oSQLReader.Item("amount"))).ToString
    '        If lblAging121.Text = "" Then
    '            lblAging121.Text = "0.00"
    '        End If
    '    End If
    '    oCnct.Close()
    'End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Me.IsPostBack Then
            'tampilAmountBlnIni()
            'RecBlnLalu()
            'InvBlnDpn()
            'If lbRecBlnIni.Text = "" Then
            '    lbRecBlnIni.Text = "0.00"
            'End If
            'If lbRecBlnLalu.Text = "" Then
            '    lbRecBlnLalu.Text = "0.00"
            'End If
            'ARAging()
            'lbRecTtl.Text = String.Format("{0:N2}", (CDec(lbRecBlnIni.Text) + CDec(lbRecBlnLalu.Text))).ToString
        End If
    End Sub

End Class
