Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Imports System.Web.Services
Imports System.Web.Script.Serialization
Imports System.Collections.Generic
Partial Class iPxAdmin_iPxAnalysGrafik
    Inherits System.Web.UI.Page

    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Public oCnct As SqlConnection = New SqlConnection(sCnct)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL As String
    <System.Web.Services.WebMethod()> _
    Public Shared Function ChartBS(ByVal dateAnalys As String) As String
        Dim businessID = HttpContext.Current.Session("sBusinessID").ToString()
        Dim Order As New List(Of Object)()
        Dim sSQL As String
        Dim a, bln, thn As String

        'a = dateAnalys
        'bln = Left(a, 2)
        'thn = Right(a, 4)
        Dim dateBirthday As Date = dateAnalys
        bln = dateBirthday.ToString("MM")
        thn = dateBirthday.ToString("yyy")
        Using conn As New SqlConnection()
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("iPxCNCT").ConnectionString
            Using cmd As New SqlCommand()
                sSQL = "select x.businessid, "
                sSQL += "(select sum(a.Debit-a.Credit)as amount "
                sSQL += "from iPxAcctGL_JVdtl as a inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS "
                sSQL += "AND a.Coa = b.Coa INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
                sSQL += "WHERE a.businessid='" & businessID & "' and MONTH(c.TransDate)<='" & bln & "' and year(c.TransDate)<='" & thn & "' and b.type ='Asset' "
                sSQL += "and (a.IsActive='Y' AND c.Status<>'D') group by b.type) as asset, "
                sSQL += "(select sum(a.Credit-a.Debit)as amount "
                sSQL += "from iPxAcctGL_JVdtl as a inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS "
                sSQL += "AND a.Coa = b.Coa INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
                sSQL += "WHERE a.businessid='" & businessID & "' and MONTH(c.TransDate)<='" & bln & "' and year(c.TransDate)<='" & thn & "' and b.type ='Liability' "
                sSQL += "and (a.IsActive='Y' AND c.Status<>'D') group by b.type) as Liability, "
                sSQL += "(select sum(a.Credit-a.Debit)as amount "
                sSQL += "from iPxAcctGL_JVdtl as a inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS "
                sSQL += "AND a.Coa = b.Coa INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
                sSQL += "WHERE a.businessid='" & businessID & "' and MONTH(c.TransDate)<='" & bln & "' and year(c.TransDate)<='" & thn & "' and b.type ='equity' "
                sSQL += "and (a.IsActive='Y' AND c.Status<>'D') group by b.type) as equity, "
                sSQL += "(select sum(a.Credit-a.Debit)as amount "
                sSQL += "from iPxAcctGL_JVdtl as a inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS "
                sSQL += "AND a.Coa = b.Coa INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
                sSQL += "WHERE a.businessid='" & businessID & "' and MONTH(c.TransDate)='" & bln & "' and year(c.TransDate)='" & thn & "' and b.type ='Revenue' "
                sSQL += "and (a.IsActive='Y' AND c.Status<>'D') group by b.type) as Revenue, "
                sSQL += "(select sum(a.Debit-a.Credit)as amount "
                sSQL += "from iPxAcctGL_JVdtl as a inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS "
                sSQL += "AND a.Coa = b.Coa INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
                sSQL += "WHERE a.businessid='" & businessID & "' and MONTH(c.TransDate)='" & bln & "' and year(c.TransDate)='" & thn & "' and b.type ='Cost' "
                sSQL += "and (a.IsActive='Y' AND c.Status<>'D') group by b.type) as Cost, "
                sSQL += "(select sum(a.Debit-a.Credit)as amount "
                sSQL += "from iPxAcctGL_JVdtl as a inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS "
                sSQL += "AND a.Coa = b.Coa INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
                sSQL += "WHERE a.businessid='" & businessID & "' and MONTH(c.TransDate)='" & bln & "' and year(c.TransDate)='" & thn & "' and b.type ='Expenses' "
                sSQL += "and (a.IsActive='Y' AND c.Status<>'D') group by b.type) as Expenses "

                sSQL += "from iPxAcct_Coa as x "
                sSQL += "where x.businessid='" & businessID & "' "
                sSQL += "group by x.businessid "

                cmd.CommandText = sSQL
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        Order.Add(New With {.asset = sdr("asset"), _
                                            .Liability = sdr("Liability"), _
                                            .equity = sdr("equity"), _
                                            .Revenue = sdr("Revenue"), _
                                            .Cost = sdr("Cost"), _
                                            .Expenses = sdr("Expenses")})
                    End While
                End Using
                conn.Close()
            End Using
            Return (New JavaScriptSerializer().Serialize(Order))
            'Return ("{sembarang}")
        End Using
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function ChartBSvsPL(ByVal dateAnalys As String) As String
        Dim businessID = HttpContext.Current.Session("sBusinessID").ToString()
        Dim Order As New List(Of Object)()
        Dim sSQL As String
        Dim a, b, thn, bln, asset, lia, equi, rev, cost, exp, namaBln As String
        Dim i As Integer
        i = 1
        asset = "asset" & i & ","
        lia = "Liability" & i & ","
        equi = "equity" & i & ","
        rev = "Revenue" & i & ","
        cost = "Cost" & i & ","
        exp = "Expenses" & i & ","
        Dim dateBirthday As Date = dateAnalys
        bln = dateBirthday.ToString("MM")
        thn = dateBirthday.ToString("yyy")
        a = bln + "-" + thn
        bln = Left(a, 2)
        thn = Right(a, 4)
        Using conn As New SqlConnection()
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("iPxCNCT").ConnectionString
            Using cmd As New SqlCommand()
                sSQL = "select x.businessid, "
                Do While (i <= 6)
                    If bln = "01" Then
                        namaBln = "Jan"
                    ElseIf bln = "02" Then
                        namaBln = "Feb"
                    ElseIf bln = "03" Then
                        namaBln = "Mar"
                    ElseIf bln = "04" Then
                        namaBln = "Apr"
                    ElseIf bln = "05" Then
                        namaBln = "Mei"
                    ElseIf bln = "06" Then
                        namaBln = "Jun"
                    ElseIf bln = "07" Then
                        namaBln = "Jul"
                    ElseIf bln = "08" Then
                        namaBln = "Agust"
                    ElseIf bln = "09" Then
                        namaBln = "Sep"
                    ElseIf bln = "10" Then
                        namaBln = "Oct"
                    ElseIf bln = "11" Then
                        namaBln = "Nov"
                    ElseIf bln = "12" Then
                        namaBln = "Des"
                    End If
                    sSQL += "('" & namaBln & "')as bln" & i & ", "
                    sSQL += "(select sum(a.Debit-a.Credit)as amount "
                    sSQL += "from iPxAcctGL_JVdtl as a inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS "
                    sSQL += "AND a.Coa = b.Coa INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
                    sSQL += "WHERE a.businessid='" & businessID & "' and MONTH(c.TransDate)='" & bln & "' and year(c.TransDate)='" & thn & "' and b.type ='Asset' "
                    sSQL += "and (a.IsActive='Y' AND c.Status<>'D') group by b.type) as " & asset & ""
                    sSQL += "(select sum(a.Credit-a.Debit)as amount "
                    sSQL += "from iPxAcctGL_JVdtl as a inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS "
                    sSQL += "AND a.Coa = b.Coa INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
                    sSQL += "WHERE a.businessid='" & businessID & "' and MONTH(c.TransDate)='" & bln & "' and year(c.TransDate)='" & thn & "' and b.type ='Liability' "
                    sSQL += "and (a.IsActive='Y' AND c.Status<>'D') group by b.type) as " & lia & ""
                    sSQL += "(select sum(a.Credit-a.Debit)as amount "
                    sSQL += "from iPxAcctGL_JVdtl as a inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS "
                    sSQL += "AND a.Coa = b.Coa INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
                    sSQL += "WHERE a.businessid='" & businessID & "' and MONTH(c.TransDate)='" & bln & "' and year(c.TransDate)='" & thn & "' and b.type ='equity' "
                    sSQL += "and (a.IsActive='Y' AND c.Status<>'D') group by b.type) as " & equi & ""
                    sSQL += "(select sum(a.Credit-a.Debit)as amount "
                    sSQL += "from iPxAcctGL_JVdtl as a inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS "
                    sSQL += "AND a.Coa = b.Coa INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
                    sSQL += "WHERE a.businessid='" & businessID & "' and MONTH(c.TransDate)='" & bln & "' and year(c.TransDate)='" & thn & "' and b.type ='Revenue' "
                    sSQL += "and (a.IsActive='Y' AND c.Status<>'D') group by b.type) as " & rev & ""
                    sSQL += "(select sum(a.Debit-a.Credit)as amount "
                    sSQL += "from iPxAcctGL_JVdtl as a inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS "
                    sSQL += "AND a.Coa = b.Coa INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
                    sSQL += "WHERE a.businessid='" & businessID & "' and MONTH(c.TransDate)='" & bln & "' and year(c.TransDate)='" & thn & "' and b.type ='Cost' "
                    sSQL += "and (a.IsActive='Y' AND c.Status<>'D') group by b.type) as " & cost & ""
                    sSQL += "(select sum(a.Debit-a.Credit)as amount "
                    sSQL += "from iPxAcctGL_JVdtl as a inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS "
                    sSQL += "AND a.Coa = b.Coa INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
                    sSQL += "WHERE a.businessid='" & businessID & "' and MONTH(c.TransDate)='" & bln & "' and year(c.TransDate)='" & thn & "' and b.type ='Expenses' "
                    sSQL += "and (a.IsActive='Y' AND c.Status<>'D') group by b.type) as " & exp & ""

                    i += 1
                    asset = "asset" & i & ","
                    lia = "Liability" & i & ","
                    equi = "equity" & i & ","
                    rev = "Revenue" & i & ","
                    cost = "Cost" & i & ","
                    If i = 6 Then
                        exp = "Expenses" & i & " "
                    Else
                        exp = "Expenses" & i & ","
                    End If
                    b = Format(DateAdd("M", -(i - 1), a), "MM-yyy")
                    bln = Left(b, 2)
                    thn = Right(b, 4)
                Loop
                sSQL += "from iPxAcct_Coa as x "
                sSQL += "where x.businessid='" & businessID & "' "
                sSQL += "group by x.businessid "
                cmd.CommandText = sSQL
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        Order.Add(New With {.bln1 = sdr("bln1"), .asset1 = sdr("asset1"), .Liability1 = sdr("Liability1"), .equity1 = sdr("equity1"), .Revenue1 = sdr("Revenue1"), .Cost1 = sdr("Cost1"), .Expenses1 = sdr("Expenses1"), _
                                            .bln2 = sdr("bln2"), .asset2 = sdr("asset2"), .Liability2 = sdr("Liability2"), .equity2 = sdr("equity2"), .Revenue2 = sdr("Revenue2"), .Cost2 = sdr("Cost2"), .Expenses2 = sdr("Expenses2"), _
                                            .bln3 = sdr("bln3"), .asset3 = sdr("asset3"), .Liability3 = sdr("Liability3"), .equity3 = sdr("equity3"), .Revenue3 = sdr("Revenue3"), .Cost3 = sdr("Cost3"), .Expenses3 = sdr("Expenses3"), _
                                            .bln4 = sdr("bln4"), .asset4 = sdr("asset4"), .Liability4 = sdr("Liability4"), .equity4 = sdr("equity4"), .Revenue4 = sdr("Revenue4"), .Cost4 = sdr("Cost4"), .Expenses4 = sdr("Expenses4"), _
                                            .bln5 = sdr("bln5"), .asset5 = sdr("asset5"), .Liability5 = sdr("Liability5"), .equity5 = sdr("equity5"), .Revenue5 = sdr("Revenue5"), .Cost5 = sdr("Cost5"), .Expenses5 = sdr("Expenses5"), _
                                            .bln6 = sdr("bln6"), .asset6 = sdr("asset6"), .Liability6 = sdr("Liability6"), .equity6 = sdr("equity6"), .Revenue6 = sdr("Revenue6"), .Cost6 = sdr("Cost6"), .Expenses6 = sdr("Expenses6")})
                    End While
                End Using
                conn.Close()
            End Using
            Return (New JavaScriptSerializer().Serialize(Order))
        End Using
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function ChartRevenuevsBudget(ByVal dateAnalys As String) As String
        Dim businessID = HttpContext.Current.Session("sBusinessID").ToString()
        Dim Order As New List(Of Object)()
        Dim sSQL As String
        Dim a, b, thn, bln, rev, cost, exp, Budget, BudgetCost, BudgetExp, namaBln As String
        Dim i As Integer
        i = 1
        Budget = "budgetRevMtd" & i & ","
        BudgetCost = "budgetCostMtd" & i & ","
        BudgetExp = "budgetExpMtd" & i & ","
        rev = "Revenue" & i & ","
        cost = "Cost" & i & ","
        exp = "Expenses" & i & ","
        Dim dateBirthday As Date = dateAnalys
        bln = dateBirthday.ToString("MM")
        thn = dateBirthday.ToString("yyy")
        a = bln + "-" + thn
        bln = Left(a, 2)
        thn = Right(a, 4)
        Using conn As New SqlConnection()
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("iPxCNCT").ConnectionString
            Using cmd As New SqlCommand()
                sSQL = "select x.businessid, "
                Do While (i <= 6)
                    If bln = "01" Then
                        namaBln = "Jan"
                    ElseIf bln = "02" Then
                        namaBln = "Feb"
                    ElseIf bln = "03" Then
                        namaBln = "Mar"
                    ElseIf bln = "04" Then
                        namaBln = "Apr"
                    ElseIf bln = "05" Then
                        namaBln = "Mei"
                    ElseIf bln = "06" Then
                        namaBln = "Jun"
                    ElseIf bln = "07" Then
                        namaBln = "Jul"
                    ElseIf bln = "08" Then
                        namaBln = "Agust"
                    ElseIf bln = "09" Then
                        namaBln = "Sep"
                    ElseIf bln = "10" Then
                        namaBln = "Oct"
                    ElseIf bln = "11" Then
                        namaBln = "Nov"
                    ElseIf bln = "12" Then
                        namaBln = "Des"
                    End If
                    sSQL += "('" & namaBln & "')as bln" & i & ", "
                    If bln = "01" Then
                        sSQL += "(select SUM(i.bln1) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Revenue')as " & Budget & " "
                        sSQL += "(select SUM(i.bln1) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Cost')as " & BudgetCost & " "
                        sSQL += "(select SUM(i.bln1) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Expenses')as " & BudgetExp & " "
                    ElseIf bln = "02" Then
                        sSQL += "(select SUM(i.bln2) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Revenue')as " & Budget & " "
                        sSQL += "(select SUM(i.bln2) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Cost')as " & BudgetCost & " "
                        sSQL += "(select SUM(i.bln2) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Expenses')as " & BudgetExp & " "
                    ElseIf bln = "03" Then
                        sSQL += "(select SUM(i.bln3) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Revenue')as " & Budget & " "
                        sSQL += "(select SUM(i.bln3) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Cost')as " & BudgetCost & " "
                        sSQL += "(select SUM(i.bln3) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Expenses')as " & BudgetExp & " "
                    ElseIf bln = "04" Then
                        sSQL += "(select SUM(i.bln4) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Revenue')as " & Budget & " "
                        sSQL += "(select SUM(i.bln4) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Cost')as " & BudgetCost & " "
                        sSQL += "(select SUM(i.bln4) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Expenses')as " & BudgetExp & " "
                    ElseIf bln = "05" Then
                        sSQL += "(select SUM(i.bln5) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Revenue')as " & Budget & " "
                        sSQL += "(select SUM(i.bln5) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Cost')as " & BudgetCost & " "
                        sSQL += "(select SUM(i.bln5) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Expenses')as " & BudgetExp & " "
                    ElseIf bln = "06" Then
                        sSQL += "(select SUM(i.bln6) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Revenue')as " & Budget & " "
                        sSQL += "(select SUM(i.bln6) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Cost')as " & BudgetCost & " "
                        sSQL += "(select SUM(i.bln6) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Expenses')as " & BudgetExp & " "
                    ElseIf bln = "07" Then
                        sSQL += "(select SUM(i.bln7) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Revenue')as " & Budget & " "
                        sSQL += "(select SUM(i.bln7) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Cost')as " & BudgetCost & " "
                        sSQL += "(select SUM(i.bln7) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Expenses')as " & BudgetExp & " "
                    ElseIf bln = "08" Then
                        sSQL += "(select SUM(i.bln8) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Revenue')as " & Budget & " "
                        sSQL += "(select SUM(i.bln8) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Cost')as " & BudgetCost & " "
                        sSQL += "(select SUM(i.bln8) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Expenses')as " & BudgetExp & " "
                    ElseIf bln = "09" Then
                        sSQL += "(select SUM(i.bln9) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Revenue')as " & Budget & " "
                        sSQL += "(select SUM(i.bln9) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Cost')as " & BudgetCost & " "
                        sSQL += "(select SUM(i.bln9) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Expenses')as " & BudgetExp & " "
                    ElseIf bln = "10" Then
                        sSQL += "(select SUM(i.bln10) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Revenue')as " & Budget & " "
                        sSQL += "(select SUM(i.bln10) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Cost')as " & BudgetCost & " "
                        sSQL += "(select SUM(i.bln10) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Expenses')as " & BudgetExp & " "
                    ElseIf bln = "11" Then
                        sSQL += "(select SUM(i.bln11) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Revenue')as " & Budget & " "
                        sSQL += "(select SUM(i.bln11) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Cost')as " & BudgetCost & " "
                        sSQL += "(select SUM(i.bln11) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Expenses')as " & BudgetExp & " "
                    ElseIf bln = "12" Then
                        sSQL += "(select SUM(i.bln12) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Revenue')as " & Budget & " "
                        sSQL += "(select SUM(i.bln12) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Cost')as " & BudgetCost & " "
                        sSQL += "(select SUM(i.bln12) from iPxAcctGL_COABudget as i INNER JOIN iPxAcct_Coa as j ON j.businessid=i.businessid and j.Coa=i.Coa where i.businessid='" & businessID & "' and i.periode='" & thn & "' and j.type='Expenses')as " & BudgetExp & " "
                    End If
                    sSQL += "(select sum(a.Debit-a.Credit)as amount "
                    sSQL += "from iPxAcctGL_JVdtl as a inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS "
                    sSQL += "AND a.Coa = b.Coa INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
                    sSQL += "WHERE a.businessid='" & businessID & "' and MONTH(c.TransDate)='" & bln & "' and year(c.TransDate)='" & thn & "' and b.type ='Cost' "
                    sSQL += "and (a.IsActive='Y' AND c.Status<>'D') group by b.type) as " & cost & ""
                    sSQL += "(select sum(a.Debit-a.Credit)as amount "
                    sSQL += "from iPxAcctGL_JVdtl as a inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS "
                    sSQL += "AND a.Coa = b.Coa INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
                    sSQL += "WHERE a.businessid='" & businessID & "' and MONTH(c.TransDate)='" & bln & "' and year(c.TransDate)='" & thn & "' and b.type ='Expenses' "
                    sSQL += "and (a.IsActive='Y' AND c.Status<>'D') group by b.type) as " & exp & ""
                    sSQL += "(select sum(a.Credit-a.Debit)as amount "
                    sSQL += "from iPxAcctGL_JVdtl as a inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS "
                    sSQL += "AND a.Coa = b.Coa INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
                    sSQL += "WHERE a.businessid='" & businessID & "' and MONTH(c.TransDate)='" & bln & "' and year(c.TransDate)='" & thn & "' and b.type ='Revenue' "
                    sSQL += "and (a.IsActive='Y' AND c.Status<>'D') group by b.type) as " & rev & ""
                    
                    i += 1
                    Budget = "budgetRevMtd" & i & ","
                    BudgetCost = "budgetCostMtd" & i & ","
                    BudgetExp = "budgetExpMtd" & i & ","
                    cost = "Cost" & i & ","
                    exp = "Expenses" & i & ","
                    If i = 6 Then
                        rev = "Revenue" & i & " "
                    Else
                        rev = "Revenue" & i & ","
                    End If
                    b = Format(DateAdd("M", -(i - 1), a), "MM-yyy")
                    bln = Left(b, 2)
                    thn = Right(b, 4)
                Loop
                sSQL += "from iPxAcct_Coa as x "
                sSQL += "where x.businessid='" & businessID & "' "
                sSQL += "group by x.businessid "
                cmd.CommandText = sSQL
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        Order.Add(New With {.bln1 = sdr("bln1"), .Revenue1 = sdr("Revenue1"), .Budg1 = sdr("budgetRevMtd1"), .Cost1 = sdr("Cost1"), .BudgCost1 = sdr("budgetCostMtd1"), .Expenses1 = sdr("Expenses1"), .BudgExp1 = sdr("budgetExpMtd1"), _
                                            .bln2 = sdr("bln2"), .Revenue2 = sdr("Revenue2"), .Budg2 = sdr("budgetRevMtd2"), .Cost2 = sdr("Cost2"), .BudgCost2 = sdr("budgetCostMtd2"), .Expenses2 = sdr("Expenses2"), .BudgExp2 = sdr("budgetExpMtd2"), _
                                            .bln3 = sdr("bln3"), .Revenue3 = sdr("Revenue3"), .Budg3 = sdr("budgetRevMtd3"), .Cost3 = sdr("Cost3"), .BudgCost3 = sdr("budgetCostMtd3"), .Expenses3 = sdr("Expenses3"), .BudgExp3 = sdr("budgetExpMtd3"), _
                                            .bln4 = sdr("bln4"), .Revenue4 = sdr("Revenue4"), .Budg4 = sdr("budgetRevMtd4"), .Cost4 = sdr("Cost4"), .BudgCost4 = sdr("budgetCostMtd4"), .Expenses4 = sdr("Expenses4"), .BudgExp4 = sdr("budgetExpMtd4"), _
                                            .bln5 = sdr("bln5"), .Revenue5 = sdr("Revenue5"), .Budg5 = sdr("budgetRevMtd5"), .Cost5 = sdr("Cost5"), .BudgCost5 = sdr("budgetCostMtd5"), .Expenses5 = sdr("Expenses5"), .BudgExp5 = sdr("budgetExpMtd5"), _
                                            .bln6 = sdr("bln6"), .Revenue6 = sdr("Revenue6"), .Budg6 = sdr("budgetRevMtd6"), .Cost6 = sdr("Cost6"), .BudgCost6 = sdr("budgetCostMtd5"), .Expenses6 = sdr("Expenses6"), .BudgExp6 = sdr("budgetExpMtd6")})
                    End While
                End Using
                conn.Close()
            End Using
            Return (New JavaScriptSerializer().Serialize(Order))
        End Using
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function ChartCash(ByVal dateAnalys As String) As String
        Dim businessID = HttpContext.Current.Session("sBusinessID").ToString()
        Dim Order As New List(Of Object)()
        Dim sSQL As String
        Dim a, b, thn, bln, Cash, cost, exp, namaBln, x As String
        Dim i As Integer
        i = 1
        Cash = "cash" & i & ","
        cost = "Cost" & i & ","
        exp = "Expenses" & i & ","
        Dim dateBirthday As Date = dateAnalys
        bln = dateBirthday.ToString("MM")
        thn = dateBirthday.ToString("yyy")
        a = bln + "-" + thn
        bln = Left(a, 2)
        thn = Right(a, 4)
        Using conn As New SqlConnection()
            conn.ConnectionString = ConfigurationManager.ConnectionStrings("iPxCNCT").ConnectionString
            Using cmd As New SqlCommand()
                sSQL = "select x.businessid, "
                Do While (i <= 6)
                    If bln = "01" Then
                        namaBln = "Jan"
                    ElseIf bln = "02" Then
                        namaBln = "Feb"
                    ElseIf bln = "03" Then
                        namaBln = "Mar"
                    ElseIf bln = "04" Then
                        namaBln = "Apr"
                    ElseIf bln = "05" Then
                        namaBln = "Mei"
                    ElseIf bln = "06" Then
                        namaBln = "Jun"
                    ElseIf bln = "07" Then
                        namaBln = "Jul"
                    ElseIf bln = "08" Then
                        namaBln = "Agust"
                    ElseIf bln = "09" Then
                        namaBln = "Sep"
                    ElseIf bln = "10" Then
                        namaBln = "Oct"
                    ElseIf bln = "11" Then
                        namaBln = "Nov"
                    ElseIf bln = "12" Then
                        namaBln = "Des"
                    End If
                    x = Date.DaysInMonth(thn, bln)
                    'x = Format(x, "yyy-MM-dd")
                    sSQL += "('" & namaBln & "')as bln" & i & ", "
                    sSQL += "(select sum(a.Debit-a.Credit)as amount "
                    sSQL += "from iPxAcctGL_JVdtl as a inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS "
                    sSQL += "AND a.Coa = b.Coa INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
                    sSQL += "WHERE a.businessid='" & businessID & "' and MONTH(c.TransDate)='" & bln & "' and year(c.TransDate)='" & thn & "' and b.type ='Cost' "
                    sSQL += "and (a.IsActive='Y' AND c.Status<>'D') group by b.type) as " & cost & ""
                    sSQL += "(select sum(a.Debit-a.Credit)as amount "
                    sSQL += "from iPxAcctGL_JVdtl as a inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS "
                    sSQL += "AND a.Coa = b.Coa INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
                    sSQL += "WHERE a.businessid='" & businessID & "' and MONTH(c.TransDate)='" & bln & "' and year(c.TransDate)='" & thn & "' and b.type ='Expenses' "
                    sSQL += "and (a.IsActive='Y' AND c.Status<>'D') group by b.type) as " & exp & ""
                    sSQL += "(select SUM(a.Debit-a.Credit) from iPxAcctGL_JVdtl as a "
                    sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
                    sSQL += "INNER JOIN iPxAcct_Coa as c ON c.businessid=a.businessid and c.Coa=a.Coa "
                    sSQL += "WHERE a.businessid='" & businessID & "' and c.Status ='Cash Bank Account' and b.TransDate between '1900-01-01' and '" & thn & "-" & bln & "-" & x & "' "
                    sSQL += "and b.status<>'D' and a.isActive='Y' ) as " & Cash & ""

                    i += 1
                    cost = "Cost" & i & ","
                    exp = "Expenses" & i & ","
                    If i = 6 Then
                        Cash = "cash" & i & " "
                    Else
                        Cash = "cash" & i & ","
                    End If
                    b = Format(DateAdd("M", -(i - 1), a), "MM-yyy")
                    bln = Left(b, 2)
                    thn = Right(b, 4)
                Loop
                sSQL += "from iPxAcct_Coa as x "
                sSQL += "where x.businessid='" & businessID & "' "
                sSQL += "group by x.businessid "
                cmd.CommandText = sSQL
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        Order.Add(New With {.bln1 = sdr("bln1"), .Cash1 = sdr("cash1"), .Cost1 = sdr("cost1"), .Expenses1 = sdr("Expenses1"), _
                                            .bln2 = sdr("bln2"), .Cash2 = sdr("cash2"), .Cost2 = sdr("cost1"), .Expenses2 = sdr("Expenses1"), _
                                            .bln3 = sdr("bln3"), .Cash3 = sdr("cash3"), .Cost3 = sdr("cost1"), .Expenses3 = sdr("Expenses1"), _
                                            .bln4 = sdr("bln4"), .Cash4 = sdr("cash4"), .Cost4 = sdr("cost1"), .Expenses4 = sdr("Expenses1"), _
                                            .bln5 = sdr("bln5"), .Cash5 = sdr("cash5"), .Cost5 = sdr("cost1"), .Expenses5 = sdr("Expenses1"), _
                                            .bln6 = sdr("bln6"), .Cash6 = sdr("cash6"), .Cost6 = sdr("cost1"), .Expenses6 = sdr("Expenses1")})
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
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showGraphBS", "$(document).ready(function() {showGraphBS()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showGraphBSvsPL", "$(document).ready(function() {showGraphBSvsPL()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showGraphRevenuevsBudget", "$(document).ready(function() {showGraphRevenuevsBudget()});", True)
        End If

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthGL", "$(document).ready(function() {MonthGL()});", True)
    End Sub
    Protected Sub cari(ByVal sender As Object, ByVal e As EventArgs)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showGraphBS", "$(document).ready(function() {showGraphBS()});", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showGraphBSvsPL", "$(document).ready(function() {showGraphBSvsPL()});", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showGraphRevenuevsBudget", "$(document).ready(function() {showGraphRevenuevsBudget()});", True)
    End Sub
End Class
