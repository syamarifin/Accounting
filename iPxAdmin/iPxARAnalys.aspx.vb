Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Imports System.Web.Services
Imports System.Web.Script.Serialization
Imports System.Collections.Generic
Partial Class iPxAdmin_iPxARAnalys
    Inherits System.Web.UI.Page
    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Public oCnct As SqlConnection = New SqlConnection(sCnct)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL As String
    <System.Web.Services.WebMethod()> _
    Public Shared Function ChartAging(ByVal dateAnalys As String) As String
        Dim businessID = HttpContext.Current.Session("sBusinessID").ToString()
        Dim Order As New List(Of Object)()
        Dim sSQL As String
        Dim a, bln1, bln2, bln3, bln4 As String
        Dim dateBirthday As Date = dateAnalys
        Dim forMonth As String = dateBirthday.ToString("MM")
        Dim forYear As String = dateBirthday.ToString("yyy")
        Dim x As String = Date.DaysInMonth(forYear, forMonth)
        Dim y As Date = forYear + "-" + forMonth + "-" + x
        a = Format(y, "yyy-MM-dd")
        'a = Format(Now, "yyy-MM-dd")
        bln1 = Format(DateAdd("d", -30, a), "yyy-MM-dd")
        bln2 = Format(DateAdd("d", -60, a), "yyy-MM-dd")
        bln3 = Format(DateAdd("d", -90, a), "yyy-MM-dd")
        bln4 = Format(DateAdd("d", -120, a), "yyy-MM-dd")
        Using conn As New SqlConnection()
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("iPxCNCT").ConnectionString
            Using cmd As New SqlCommand()
                sSQL = "select a.businessid, "
                sSQL += "(SELECT SUM(amount) FROM (SELECT businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
                sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & businessID & "' and invoiceno ='' and (TransDate > '" & bln1 & "') and (TransDate <= '" & a & "') and isActive='Y' "
                sSQL += "group by businessid UNION ALL "
                sSQL += "SELECT iPxAcctAR_Transaction.businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END)) AS 'amount' "
                sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
                sSQL += "where iPxAcctAR_Transaction.businessid ='" & businessID & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.isActive='Y' and "
                sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln1 & "') and "
                sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & a & "') "
                sSQL += "group by iPxAcctAR_Transaction.businessid) a GROUP BY businessid)as bulan1, "
                sSQL += "(SELECT SUM(amount) FROM (SELECT businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
                sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & businessID & "' and invoiceno ='' and (TransDate > '" & bln2 & "') and (TransDate <= '" & bln1 & "') and isActive='Y' "
                sSQL += "group by businessid UNION ALL "
                sSQL += "SELECT iPxAcctAR_Transaction.businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END)) AS 'amount' "
                sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
                sSQL += "where iPxAcctAR_Transaction.businessid ='" & businessID & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.isActive='Y' and "
                sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln2 & "') and "
                sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln1 & "') "
                sSQL += "group by iPxAcctAR_Transaction.businessid) a GROUP BY businessid)as bulan2, "
                sSQL += "(SELECT SUM(amount) FROM (SELECT businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
                sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & businessID & "' and invoiceno ='' and (TransDate > '" & bln3 & "') and (TransDate <= '" & bln2 & "') and isActive='Y' "
                sSQL += "group by businessid UNION ALL "
                sSQL += "SELECT iPxAcctAR_Transaction.businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END)) AS 'amount' "
                sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
                sSQL += "where iPxAcctAR_Transaction.businessid ='" & businessID & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.isActive='Y' and "
                sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln3 & "') and "
                sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln2 & "') "
                sSQL += "group by iPxAcctAR_Transaction.businessid) a GROUP BY businessid)as bulan3, "
                sSQL += "(SELECT SUM(amount) FROM (SELECT businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
                sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & businessID & "' and invoiceno ='' and (TransDate > '" & bln4 & "') and (TransDate <= '" & bln3 & "') and isActive='Y' "
                sSQL += "group by businessid UNION ALL "
                sSQL += "SELECT iPxAcctAR_Transaction.businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END)) AS 'amount' "
                sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
                sSQL += "where iPxAcctAR_Transaction.businessid ='" & businessID & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.isActive='Y' and "
                sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END > '" & bln4 & "') and "
                sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln3 & "') "
                sSQL += "group by iPxAcctAR_Transaction.businessid) a GROUP BY businessid)as bulan4, "
                sSQL += "(SELECT SUM(amount) FROM (SELECT businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
                sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & businessID & "' and invoiceno ='' and (TransDate <= '" & bln4 & "') and isActive='Y' "
                sSQL += "group by businessid UNION ALL "
                sSQL += "SELECT iPxAcctAR_Transaction.businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END)) AS 'amount' "
                sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
                sSQL += "where iPxAcctAR_Transaction.businessid ='" & businessID & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.isActive='Y' and "
                sSQL += "(CASE WHEN iPxAcctAR_Invoice.InvDate < '" & a & "' THEN iPxAcctAR_Invoice.InvDate WHEN iPxAcctAR_Invoice.InvDate >= '" & a & "' THEN iPxAcctAR_Transaction.TransDate END <= '" & bln4 & "') "
                sSQL += "group by iPxAcctAR_Transaction.businessid) a GROUP BY businessid)as lbh "
                sSQL += "from iPxAcctAR_Cfg_Customer as a "
                sSQL += "INNER JOIN iPxAcctAR_Cfg_CustomerGrp as c ON c.businessid=a.businessid and c.arGroup=a.arGroup "
                sSQL += "WHERE a.businessid='" & businessID & "' and "
                sSQL += "(SELECT SUM(amount) FROM (SELECT businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(iPxAcctAR_Transaction.amountcr)) AS 'amount' "
                sSQL += "FROM iPxAcctAR_Transaction where businessid ='" & businessID & "' and invoiceno ='' and (TransDate <= '" & a & "') and isActive='Y' group by businessid UNION ALL "
                sSQL += "SELECT iPxAcctAR_Transaction.businessid,(SUM(iPxAcctAR_Transaction.amountdr) -SUM(CASE WHEN iPxAcctAR_Transaction.TransDate <= '" & a & "' THEN iPxAcctAR_Transaction.amountcr WHEN iPxAcctAR_Transaction.TransDate > '" & a & "' THEN '0' END)) AS 'amount' "
                sSQL += "FROM iPxAcctAR_Invoice inner join iPxAcctAR_Transaction on iPxAcctAR_Transaction.invoiceno=iPxAcctAR_Invoice.InvoiceNo "
                sSQL += "where iPxAcctAR_Transaction.businessid ='" & businessID & "' and iPxAcctAR_Transaction.invoiceno <> '' and iPxAcctAR_Transaction.isActive='Y' "
                sSQL += "and (iPxAcctAR_Transaction.TransDate <= '" & a & "') group by iPxAcctAR_Transaction.businessid) a GROUP BY businessid) <>'' "
                sSQL += " group by a.businessid"

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
    Public Shared Function ChartReceiptMtd(ByVal dateAnalys As String) As String
        Dim businessID = HttpContext.Current.Session("sBusinessID").ToString()
        Dim Order As New List(Of Object)()
        Dim sSQL As String
        Dim a, bln1, bln2, bln3, bln4 As String
        Dim dateBirthday As Date = dateAnalys
        Dim forMonth As String = dateBirthday.ToString("MM")
        Dim forYear As String = dateBirthday.ToString("yyy")
        Dim x As String = Date.DaysInMonth(forYear, forMonth)
        Dim y As Date = forYear + "-" + forMonth + "-" + x
        a = Format(y, "yyy-MM-dd")
        'a = Format(Now, "yyy-MM-dd")
        'bln1 = Format(DateAdd("d", -30, a), "yyy-MM-dd")
        'bln2 = Format(DateAdd("d", -60, a), "yyy-MM-dd")
        'bln3 = Format(DateAdd("d", -90, a), "yyy-MM-dd")
        'bln4 = Format(DateAdd("d", -120, a), "yyy-MM-dd")
        Using conn As New SqlConnection()
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("iPxCNCT").ConnectionString
            Using cmd As New SqlCommand()
                sSQL = "select a.businessid, a.PaidBy, a.Description, "
                sSQL += "(select sum(y.amountcr) from iPxAcctAR_Receipt as x "
                sSQL += "INNER JOIN iPxAcctAR_Transaction as y ON y.businessid=x.businessid and y.TransID=x.ReceiptID "
                sSQL += "where x.businessid= a.businessid and x.PaidBy=a.PaidBy and x.Status<>'X' and y.isActive='Y' "
                sSQL += "group by x.PaidBy) as amount from iPxAcctAR_Cfg_Paidby as a "
                sSQL += "where a.businessid='" & businessID & "' and a.isActive='Y' "

                cmd.CommandText = sSQL
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        Order.Add(New With {.DescPaid = sdr("Description"), .Amount = sdr("amount")})
                    End While
                End Using
                conn.Close()
            End Using
            Return (New JavaScriptSerializer().Serialize(Order))
        End Using
    End Function

<System.Web.Services.WebMethod()> _
    Public Shared Function ChartTransSix(ByVal dateAnalys As String) As String
        Dim businessID = HttpContext.Current.Session("sBusinessID").ToString()
        Dim Order As New List(Of Object)()
        Dim sSQL As String
        Dim a, b, amount, namaBln As String
        Dim i, thn, bln As Integer
        i = 1
        amount = "amountBln" & i & ","
        Dim dateBirthday As Date = dateAnalys
        bln = dateBirthday.ToString("MM")
        thn = dateBirthday.ToString("yyy")
        Using conn As New SqlConnection()
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("iPxCNCT").ConnectionString
            Using cmd As New SqlCommand()
                sSQL = "select z.businessid, "
                Do While (i <= 6)
                    If bln = "1" Then
                        namaBln = "Jan"
                    ElseIf bln = "2" Then
                        namaBln = "Feb"
                    ElseIf bln = "3" Then
                        namaBln = "Mar"
                    ElseIf bln = "4" Then
                        namaBln = "Apr"
                    ElseIf bln = "5" Then
                        namaBln = "Mei"
                    ElseIf bln = "6" Then
                        namaBln = "Jun"
                    ElseIf bln = "7" Then
                        namaBln = "Jul"
                    ElseIf bln = "8" Then
                        namaBln = "Agust"
                    ElseIf bln = "9" Then
                        namaBln = "Sep"
                    ElseIf bln = "10" Then
                        namaBln = "Oct"
                    ElseIf bln = "11" Then
                        namaBln = "Nov"
                    ElseIf bln = "12" Then
                        namaBln = "Des"
                    End If
                    sSQL += "('" & namaBln & "')as bln" & i & ", "
                    sSQL += "(select sum(a.amountdr) from iPxAcctAR_Transaction as a "
                    sSQL += "where a.businessid=z.businessid and MONTH(a.TransDate)='"& bln &"' and YEAR(a.TransDate)='"& thn &"' and a.isActive='Y') as " & amount & ""

                    i += 1
                    If i = 6 Then
                        amount = "amountBln" & i & " "
                    Else
                        amount = "amountBln" & i & ","
                    End If
                    If bln = "1" Then
                        bln = "12"
                        thn = thn - 1
                    Else
                        bln = bln - 1
                        thn = thn
                    End If
                    
                Loop
                sSQL += "from iPxAcctAR_TransHdr as z "
                sSQL += "where z.businessid='"& businessID &"' and z.Status<>'X' "
                sSQL += "group by z.businessid "
                cmd.CommandText = sSQL
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        Order.Add(New With {.bln1 = sdr("bln1"), .amountBln1 = sdr("amountBln1"), _
                                            .bln2 = sdr("bln2"), .amountBln2 = sdr("amountBln2"), _
                                            .bln3 = sdr("bln3"), .amountBln3 = sdr("amountBln3"), _
                                            .bln4 = sdr("bln4"), .amountBln4 = sdr("amountBln4"), _
                                            .bln5 = sdr("bln5"), .amountBln5 = sdr("amountBln5"), _
                                            .bln6 = sdr("bln6"), .amountBln6 = sdr("amountBln6")})
                    End While
                End Using
                conn.Close()
            End Using
            Return (New JavaScriptSerializer().Serialize(Order))
        End Using
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function ChartReceiptSix(ByVal dateAnalys As String) As String
        Dim businessID = HttpContext.Current.Session("sBusinessID").ToString()
        Dim Order As New List(Of Object)()
        Dim sSQL As String
        Dim a, b, amount, namaBln As String
        Dim i, thn, bln As Integer
        i = 1
        amount = "amountBln" & i & ","
        Dim dateBirthday As Date = dateAnalys
        bln = dateBirthday.ToString("MM")
        thn = dateBirthday.ToString("yyy")
        Using conn As New SqlConnection()
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("iPxCNCT").ConnectionString
            Using cmd As New SqlCommand()
                sSQL = "select z.businessid, "
                Do While (i <= 6)
                    If bln = "1" Then
                        namaBln = "Jan"
                    ElseIf bln = "2" Then
                        namaBln = "Feb"
                    ElseIf bln = "3" Then
                        namaBln = "Mar"
                    ElseIf bln = "4" Then
                        namaBln = "Apr"
                    ElseIf bln = "5" Then
                        namaBln = "Mei"
                    ElseIf bln = "6" Then
                        namaBln = "Jun"
                    ElseIf bln = "7" Then
                        namaBln = "Jul"
                    ElseIf bln = "8" Then
                        namaBln = "Agust"
                    ElseIf bln = "9" Then
                        namaBln = "Sep"
                    ElseIf bln = "10" Then
                        namaBln = "Oct"
                    ElseIf bln = "11" Then
                        namaBln = "Nov"
                    ElseIf bln = "12" Then
                        namaBln = "Des"
                    End If
                    sSQL += "('" & namaBln & "')as bln" & i & ", "
                    sSQL += "(select sum(a.amountcr) from iPxAcctAR_Transaction as a "
                    sSQL += "INNER JOIN iPxAcctAR_Receipt as b ON b.businessid=a.businessid and b.ReceiptID=a.TransID "
                    sSQL += "where a.businessid=z.businessid and b.ReceiptID like 'RC%' and MONTH(b.ReceiptDate)='" & bln & "' and YEAR(b.ReceiptDate)='" & thn & "' and a.isActive='Y') as " & amount & ""

                    i += 1
                    If i = 6 Then
                        amount = "amountBln" & i & " "
                    Else
                        amount = "amountBln" & i & ","
                    End If
                    If bln = "1" Then
                        bln = "12"
                        thn = thn - 1
                    Else
                        bln = bln - 1
                        thn = thn
                    End If

                Loop
                sSQL += "from iPxAcctAR_Receipt as z "
                sSQL += "where z.businessid='" & businessID & "' and z.Status<>'X' and ReceiptID like 'RC%' "
                sSQL += "group by z.businessid "
                cmd.CommandText = sSQL
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        Order.Add(New With {.bln1 = sdr("bln1"), .amountBln1 = sdr("amountBln1"), _
                                            .bln2 = sdr("bln2"), .amountBln2 = sdr("amountBln2"), _
                                            .bln3 = sdr("bln3"), .amountBln3 = sdr("amountBln3"), _
                                            .bln4 = sdr("bln4"), .amountBln4 = sdr("amountBln4"), _
                                            .bln5 = sdr("bln5"), .amountBln5 = sdr("amountBln5"), _
                                            .bln6 = sdr("bln6"), .amountBln6 = sdr("amountBln6")})
                    End While
                End Using
                conn.Close()
            End Using
            Return (New JavaScriptSerializer().Serialize(Order))
        End Using
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            tbDate.Text = Format(Now, "MM-yyyy")
            tbDate.Enabled = False
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showGraph", "$(document).ready(function() {showGraph()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showGraphARTransSix", "$(document).ready(function() {showGraphARTransSix()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showGraphARRecSix", "$(document).ready(function() {showGraphARRecSix()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showReceiptMtd", "$(document).ready(function() {showReceiptMtd()});", True)
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthGL", "$(document).ready(function() {MonthGL()});", True)
    End Sub

    Protected Sub cari(ByVal sender As Object, ByVal e As EventArgs)
        
    End Sub
End Class
