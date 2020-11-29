Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
'Imports ClosedXML.Excel
Partial Class iPxAdmin_iPxGLSummary
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, forMonth, forYear, lastMonth, lastYear, a, x, y, z As String
    Dim cIpx As New iPxClass
    Sub show_Group()
        dlGroup.Items.Clear()
        dlGroup.Items.Insert(0, "Profit and Lost")
        dlGroup.Items.Insert(1, "Statistic Account")
    End Sub
    'Sub ExportExcel()
    '    Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
    '    Using con As New SqlConnection(constr)
    '        Using cmd As New SqlCommand("SELECT * FROM Customers")
    '            Using sda As New SqlDataAdapter()
    '                cmd.Connection = con
    '                sda.SelectCommand = cmd
    '                Using dt As New DataTable()
    '                    sda.Fill(dt)
    '                    Using wb As New XLWorkbook()
    '                        wb.Worksheets.Add(dt, "Customers")

    '                        Response.Clear()
    '                        Response.Buffer = True
    '                        Response.Charset = ""
    '                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
    '                        Response.AddHeader("content-disposition", "attachment;filename=SqlExport.xlsx")
    '                        Using MyMemoryStream As New MemoryStream()
    '                            wb.SaveAs(MyMemoryStream)
    '                            MyMemoryStream.WriteTo(Response.OutputStream)
    '                            Response.Flush()
    '                            Response.End()
    '                        End Using
    '                    End Using
    '                End Using
    '            End Using
    '        End Using
    '    End Using
    'End Sub
#Region "Summary_box"
    'Balancid============================================================================================================================================
    Sub totAsset()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select sum(a.Debit-a.Credit)as amount from iPxAcctGL_JVdtl as a "
        sSQL += "inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND a.Coa = b.Coa "
        sSQL += "INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and MONTH(c.TransDate)='" & forMonth & "' "
        sSQL += "and year(c.TransDate)='" & forYear & "' and b.type ='Asset' and (a.IsActive='Y' AND c.Status<>'D') "
        sSQL += "group by b.type "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Label1.Text = String.Format("{0:N2}", (oSQLReader.Item("amount"))).ToString
            x = oSQLReader.Item("amount").ToString
        Else
            Label1.Text = "0"
            x = "0"
        End If
        oCnct.Close()
    End Sub
    Sub totEquity()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select sum(a.Credit-a.Debit)as amount from iPxAcctGL_JVdtl as a "
        sSQL += "inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND a.Coa = b.Coa "
        sSQL += "INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and MONTH(c.TransDate)='" & forMonth & "' "
        sSQL += "and year(c.TransDate)='" & forYear & "' and b.type ='Equity' and (a.IsActive='Y' AND c.Status<>'D') "
        sSQL += "group by b.type "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Label2.Text = String.Format("{0:N2}", (oSQLReader.Item("amount"))).ToString
            y = oSQLReader.Item("amount").ToString
        Else
            Label2.Text = "0"
            y = "0"
        End If
        oCnct.Close()
    End Sub
    Sub totLiability()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select sum(a.Credit-a.Debit)as amount from iPxAcctGL_JVdtl as a "
        sSQL += "inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND a.Coa = b.Coa "
        sSQL += "INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and MONTH(c.TransDate)='" & forMonth & "' "
        sSQL += "and year(c.TransDate)='" & forYear & "' and b.type ='Liability' and (a.IsActive='Y' AND c.Status<>'D') "
        sSQL += "group by b.type "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Label3.Text = String.Format("{0:N2}", (oSQLReader.Item("amount"))).ToString
            z = oSQLReader.Item("amount").ToString
        Else
            Label3.Text = "0"
            z = "0"
        End If
        oCnct.Close()
    End Sub
    'Profit and Lost================================================================================================================================================
    Sub totRevenue()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select sum(a.Credit-a.Debit)as amount from iPxAcctGL_JVdtl as a "
        sSQL += "inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND a.Coa = b.Coa "
        sSQL += "INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and MONTH(c.TransDate)='" & forMonth & "' "
        sSQL += "and year(c.TransDate)='" & forYear & "' and b.type ='Revenue' and (a.IsActive='Y' AND c.Status<>'D') "
        sSQL += "group by b.type "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Label1.Text = String.Format("{0:N2}", (oSQLReader.Item("amount"))).ToString
            x = oSQLReader.Item("amount").ToString
        Else
            Label1.Text = "0"
            x = "0"
        End If
        oCnct.Close()
    End Sub
    Sub totCost()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select sum(a.Debit-a.Credit)as amount from iPxAcctGL_JVdtl as a "
        sSQL += "inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND a.Coa = b.Coa "
        sSQL += "INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and MONTH(c.TransDate)='" & forMonth & "' "
        sSQL += "and year(c.TransDate)='" & forYear & "' and b.type ='Cost' and (a.IsActive='Y' AND c.Status<>'D') "
        sSQL += "group by b.type "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Label2.Text = String.Format("{0:N2}", (oSQLReader.Item("amount"))).ToString
            y = oSQLReader.Item("amount").ToString
        Else
            Label2.Text = "0"
            y = "0"
        End If
        oCnct.Close()
    End Sub
    Sub totExpenses()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select sum(a.Debit-a.Credit)as amount from iPxAcctGL_JVdtl as a "
        sSQL += "inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND a.Coa = b.Coa "
        sSQL += "INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and MONTH(c.TransDate)='" & forMonth & "' "
        sSQL += "and year(c.TransDate)='" & forYear & "' and b.type ='Expenses' and (a.IsActive='Y' AND c.Status<>'D') "
        sSQL += "group by b.type "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Label3.Text = String.Format("{0:N2}", (oSQLReader.Item("amount"))).ToString
            z = oSQLReader.Item("amount").ToString
        Else
            Label3.Text = "0"
            z = "0"
        End If
        oCnct.Close()
    End Sub
    'Statistic Account=================================================================================================================
    Sub totStatistic()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select sum(a.Debit-a.Credit)as amount from iPxAcctGL_JVdtl as a "
        sSQL += "inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND a.Coa = b.Coa "
        sSQL += "INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and MONTH(c.TransDate)='" & forMonth & "' "
        sSQL += "and year(c.TransDate)='" & forYear & "' and b.type ='Statistic Account' and (a.IsActive='Y' AND c.Status<>'D') "
        sSQL += "group by b.type "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Label1.Text = String.Format("{0:N2}", (oSQLReader.Item("amount"))).ToString
        Else
            Label1.Text = "0"
        End If
        oCnct.Close()
    End Sub
#End Region
#Region "SummaryBalancid"
    Sub listSummaryAsset()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT 1 as numb, iPxAcct_Coa.businessid, iPxAcct_Coa.type, iPxAcct_Coa.Coa, iPxAcct_Coa.description, "
        sSQL += "(select SUM(a.Debit-a.Credit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "where a.Coa=iPxAcct_Coa.Coa and MONTH(b.TransDate)<='" & lastMonth & "' and year(b.TransDate)<='" & lastYear & "' and a.IsActive='Y' AND b.Status<>'D') as lastAmount, "
        sSQL += "(select SUM(a.Debit-a.Credit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "where a.Coa=iPxAcct_Coa.Coa and MONTH(b.TransDate)<='" & forMonth & "' and year(b.TransDate)<='" & forYear & "' and a.IsActive='Y' AND b.Status<>'D') as curentAmount "
        sSQL += "FROM iPxAcct_Coa "
        sSQL += "WHERE iPxAcct_Coa.businessid='" & Session("sBusinessID") & "' and iPxAcct_Coa.type ='Asset' "
        sSQL += "order by numb ,iPxAcct_Coa.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvGLAsset.DataSource = dt
                    gvGLAsset.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvGLAsset.DataSource = dt
                    gvGLAsset.DataBind()
                    gvGLAsset.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listSummaryLiability()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT 1 as numb, iPxAcct_Coa.businessid, iPxAcct_Coa.type, iPxAcct_Coa.Coa, iPxAcct_Coa.description, "
        sSQL += "(select SUM(a.Credit-a.Debit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "where a.Coa=iPxAcct_Coa.Coa and MONTH(b.TransDate)<='" & lastMonth & "' and year(b.TransDate)<='" & lastYear & "' and a.IsActive='Y' AND b.Status<>'D') as lastAmount, "
        sSQL += "(select SUM(a.Credit-a.Debit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "where a.Coa=iPxAcct_Coa.Coa and MONTH(b.TransDate)<='" & forMonth & "' and year(b.TransDate)<='" & forYear & "' and a.IsActive='Y' AND b.Status<>'D') as curentAmount "
        sSQL += "FROM iPxAcct_Coa "
        sSQL += "WHERE iPxAcct_Coa.businessid='" & Session("sBusinessID") & "' and iPxAcct_Coa.type ='Liability' "
        sSQL += "order by numb ,iPxAcct_Coa.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvGLLiability.DataSource = dt
                    gvGLLiability.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvGLLiability.DataSource = dt
                    gvGLLiability.DataBind()
                    gvGLLiability.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listSummaryEquity()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT 1 as numb, iPxAcct_Coa.businessid, iPxAcct_Coa.type, iPxAcct_Coa.Coa, iPxAcct_Coa.description, "
        sSQL += "(select SUM(a.Credit-a.Debit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "where a.Coa=iPxAcct_Coa.Coa and MONTH(b.TransDate)<='" & lastMonth & "' and year(b.TransDate)<='" & lastYear & "' and a.IsActive='Y' AND b.Status<>'D') as lastAmount, "
        sSQL += "(select SUM(a.Credit-a.Debit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "where a.Coa=iPxAcct_Coa.Coa and MONTH(b.TransDate)<='" & forMonth & "' and year(b.TransDate)<='" & forYear & "' and a.IsActive='Y' AND b.Status<>'D') as curentAmount "
        sSQL += "FROM iPxAcct_Coa "
        sSQL += "WHERE iPxAcct_Coa.businessid='" & Session("sBusinessID") & "' and iPxAcct_Coa.type ='Equity' "
        sSQL += "order by numb ,iPxAcct_Coa.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvGLEquity.DataSource = dt
                    gvGLEquity.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvGLEquity.DataSource = dt
                    gvGLEquity.DataBind()
                    gvGLEquity.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
#End Region
#Region "Summarry Profit"
    Sub listSummaryRevenue()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT 1 as numb, iPxAcct_Coa.businessid, iPxAcct_Coa.type, iPxAcct_Coa.Coa, iPxAcct_Coa.description, "
        sSQL += "(select SUM(a.Credit-a.Debit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "where a.Coa=iPxAcct_Coa.Coa and MONTH(b.TransDate)='" & forMonth & "' and year(b.TransDate)='" & forYear & "' and a.IsActive='Y' AND b.Status<>'D') as amountMtd, "
        If forMonth = "01" Then
            sSQL += "(select SUM(bln1) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "02" Then
            sSQL += "(select SUM(bln2) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "03" Then
            sSQL += "(select SUM(bln3) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "04" Then
            sSQL += "(select SUM(bln4) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "05" Then
            sSQL += "(select SUM(bln5) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "06" Then
            sSQL += "(select SUM(bln6) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "07" Then
            sSQL += "(select SUM(bln7) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "08" Then
            sSQL += "(select SUM(bln8) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "09" Then
            sSQL += "(select SUM(bln9) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "10" Then
            sSQL += "(select SUM(bln10) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "11" Then
            sSQL += "(select SUM(bln11) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "12" Then
            sSQL += "(select SUM(bln12) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        End If
        sSQL += "(select SUM(a.Credit-a.Debit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "where a.Coa=iPxAcct_Coa.Coa and year(b.TransDate)='" & forYear & "' and a.IsActive='Y' AND b.Status<>'D') as amountYtd, "
        sSQL += "(select SUM(bln1+bln2+bln3+bln4+bln5+bln6+bln7+bln8+bln9+bln10+bln11+bln12) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetYtd "
        sSQL += "FROM iPxAcct_Coa "
        sSQL += "WHERE iPxAcct_Coa.businessid='" & Session("sBusinessID") & "' and iPxAcct_Coa.type ='Revenue' "
        If CheckBox1.Checked = True Then
            sSQL += " and (iPxAcct_Coa.Coa in (select coa from iPxAcctGL_JVdtl as a inner join iPxAcctGL_JVhdr as b ON b.businessid=a.businessid and b.TransID=a.TransID where a.businessid='" & Session("sBusinessID") & "' and YEAR(b.TransDate)='" & forYear & "' and a.isActive='Y' and b.Status<>'D' group by coa) or "
            sSQL += "iPxAcct_Coa.Coa in (select coa from iPxAcctGL_COABudget where businessid='" & Session("sBusinessID") & "' and periode ='" & forYear & "' "
            If forMonth = "01" Then
                sSQL += "and bln1 <> '0.0000'  "
            ElseIf forMonth = "02" Then
                sSQL += "and bln2 <> '0.0000'  "
            ElseIf forMonth = "03" Then
                sSQL += "and bln3 <> '0.0000'  "
            ElseIf forMonth = "04" Then
                sSQL += "and bln4 <> '0.0000'  "
            ElseIf forMonth = "05" Then
                sSQL += "and bln5 <> '0.0000'  "
            ElseIf forMonth = "06" Then
                sSQL += "and bln6 <> '0.0000'  "
            ElseIf forMonth = "07" Then
                sSQL += "and bln7 <> '0.0000'  "
            ElseIf forMonth = "08" Then
                sSQL += "and bln8 <> '0.0000'  "
            ElseIf forMonth = "09" Then
                sSQL += "and bln9 <> '0.0000'  "
            ElseIf forMonth = "10" Then
                sSQL += "and bln10 <> '0.0000'  "
            ElseIf forMonth = "11" Then
                sSQL += "and bln11 <> '0.0000'  "
            ElseIf forMonth = "12" Then
                sSQL += "and bln12 <> '0.0000'  "
            End If
            sSQL += "))"
        End If
        sSQL += "order by numb ,iPxAcct_Coa.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvGLRevenue.DataSource = dt
                    gvGLRevenue.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvGLRevenue.DataSource = dt
                    gvGLRevenue.DataBind()
                    gvGLRevenue.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListSummaryCost()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT 1 as numb, iPxAcct_Coa.businessid, iPxAcct_Coa.type, iPxAcct_Coa.Coa, iPxAcct_Coa.description, "
        sSQL += "(select SUM(a.Debit-a.Credit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "where a.Coa=iPxAcct_Coa.Coa and MONTH(b.TransDate)='" & forMonth & "' and year(b.TransDate)='" & forYear & "' and a.IsActive='Y' AND b.Status<>'D') as amountMtd, "
        If forMonth = "01" Then
            sSQL += "(select SUM(bln1) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "02" Then
            sSQL += "(select SUM(bln2) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "03" Then
            sSQL += "(select SUM(bln3) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "04" Then
            sSQL += "(select SUM(bln4) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "05" Then
            sSQL += "(select SUM(bln5) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "06" Then
            sSQL += "(select SUM(bln6) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "07" Then
            sSQL += "(select SUM(bln7) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "08" Then
            sSQL += "(select SUM(bln8) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "09" Then
            sSQL += "(select SUM(bln9) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "10" Then
            sSQL += "(select SUM(bln10) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "11" Then
            sSQL += "(select SUM(bln11) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "12" Then
            sSQL += "(select SUM(bln12) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        End If
        sSQL += "(select SUM(a.Debit-a.Credit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "where a.Coa=iPxAcct_Coa.Coa and year(b.TransDate)='" & forYear & "' and a.IsActive='Y' AND b.Status<>'D') as amountYtd, "
        sSQL += "(select SUM(bln1+bln2+bln3+bln4+bln5+bln6+bln7+bln8+bln9+bln10+bln11+bln12) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetYtd "
        sSQL += "FROM iPxAcct_Coa "
        sSQL += "WHERE iPxAcct_Coa.businessid='" & Session("sBusinessID") & "' and iPxAcct_Coa.type ='Cost' "
        If CheckBox1.Checked = True Then
            sSQL += " and (iPxAcct_Coa.Coa in (select coa from iPxAcctGL_JVdtl as a inner join iPxAcctGL_JVhdr as b ON b.businessid=a.businessid and b.TransID=a.TransID where a.businessid='" & Session("sBusinessID") & "' and YEAR(b.TransDate)='" & forYear & "' and a.isActive='Y' and b.Status<>'D' group by coa) or "
            sSQL += "iPxAcct_Coa.Coa in (select coa from iPxAcctGL_COABudget where businessid='" & Session("sBusinessID") & "' and periode ='" & forYear & "' "
            If forMonth = "01" Then
                sSQL += "and bln1 <> '0.0000'  "
            ElseIf forMonth = "02" Then
                sSQL += "and bln2 <> '0.0000'  "
            ElseIf forMonth = "03" Then
                sSQL += "and bln3 <> '0.0000'  "
            ElseIf forMonth = "04" Then
                sSQL += "and bln4 <> '0.0000'  "
            ElseIf forMonth = "05" Then
                sSQL += "and bln5 <> '0.0000'  "
            ElseIf forMonth = "06" Then
                sSQL += "and bln6 <> '0.0000'  "
            ElseIf forMonth = "07" Then
                sSQL += "and bln7 <> '0.0000'  "
            ElseIf forMonth = "08" Then
                sSQL += "and bln8 <> '0.0000'  "
            ElseIf forMonth = "09" Then
                sSQL += "and bln9 <> '0.0000'  "
            ElseIf forMonth = "10" Then
                sSQL += "and bln10 <> '0.0000'  "
            ElseIf forMonth = "11" Then
                sSQL += "and bln11 <> '0.0000'  "
            ElseIf forMonth = "12" Then
                sSQL += "and bln12 <> '0.0000'  "
            End If
            sSQL += "))"
        End If
        sSQL += "order by numb ,iPxAcct_Coa.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvGLCost.DataSource = dt
                    gvGLCost.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvGLCost.DataSource = dt
                    gvGLCost.DataBind()
                    gvGLCost.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListSummaryExpenses()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT 1 as numb, iPxAcct_Coa.businessid, iPxAcct_Coa.type, iPxAcct_Coa.Coa, iPxAcct_Coa.description, "
        sSQL += "(select SUM(a.Debit-a.Credit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "where a.Coa=iPxAcct_Coa.Coa and MONTH(b.TransDate)='" & forMonth & "' and year(b.TransDate)='" & forYear & "' and a.IsActive='Y' AND b.Status<>'D') as amountMtd, "
        If forMonth = "01" Then
            sSQL += "(select SUM(bln1) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "02" Then
            sSQL += "(select SUM(bln2) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "03" Then
            sSQL += "(select SUM(bln3) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "04" Then
            sSQL += "(select SUM(bln4) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "05" Then
            sSQL += "(select SUM(bln5) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "06" Then
            sSQL += "(select SUM(bln6) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "07" Then
            sSQL += "(select SUM(bln7) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "08" Then
            sSQL += "(select SUM(bln8) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "09" Then
            sSQL += "(select SUM(bln9) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "10" Then
            sSQL += "(select SUM(bln10) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "11" Then
            sSQL += "(select SUM(bln11) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "12" Then
            sSQL += "(select SUM(bln12) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        End If
        sSQL += "(select SUM(a.Debit-a.Credit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "where a.Coa=iPxAcct_Coa.Coa and year(b.TransDate)='" & forYear & "' and a.IsActive='Y' AND b.Status<>'D') as amountYtd, "
        sSQL += "(select SUM(bln1+bln2+bln3+bln4+bln5+bln6+bln7+bln8+bln9+bln10+bln11+bln12) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetYtd "
        sSQL += "FROM iPxAcct_Coa "
        sSQL += "WHERE iPxAcct_Coa.businessid='" & Session("sBusinessID") & "' and iPxAcct_Coa.type ='Expenses' "
        If CheckBox1.Checked = True Then
            sSQL += " and (iPxAcct_Coa.Coa in (select coa from iPxAcctGL_JVdtl as a inner join iPxAcctGL_JVhdr as b ON b.businessid=a.businessid and b.TransID=a.TransID where a.businessid='" & Session("sBusinessID") & "' and YEAR(b.TransDate)='" & forYear & "' and a.isActive='Y' and b.Status<>'D' group by coa) or "
            sSQL += "iPxAcct_Coa.Coa in (select coa from iPxAcctGL_COABudget where businessid='" & Session("sBusinessID") & "' and periode ='" & forYear & "' "
            If forMonth = "01" Then
                sSQL += "and bln1 <> '0.0000'  "
            ElseIf forMonth = "02" Then
                sSQL += "and bln2 <> '0.0000'  "
            ElseIf forMonth = "03" Then
                sSQL += "and bln3 <> '0.0000'  "
            ElseIf forMonth = "04" Then
                sSQL += "and bln4 <> '0.0000'  "
            ElseIf forMonth = "05" Then
                sSQL += "and bln5 <> '0.0000'  "
            ElseIf forMonth = "06" Then
                sSQL += "and bln6 <> '0.0000'  "
            ElseIf forMonth = "07" Then
                sSQL += "and bln7 <> '0.0000'  "
            ElseIf forMonth = "08" Then
                sSQL += "and bln8 <> '0.0000'  "
            ElseIf forMonth = "09" Then
                sSQL += "and bln9 <> '0.0000'  "
            ElseIf forMonth = "10" Then
                sSQL += "and bln10 <> '0.0000'  "
            ElseIf forMonth = "11" Then
                sSQL += "and bln11 <> '0.0000'  "
            ElseIf forMonth = "12" Then
                sSQL += "and bln12 <> '0.0000'  "
            End If
            sSQL += "))"
        End If
        sSQL += "order by numb ,iPxAcct_Coa.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvGLExpenses.DataSource = dt
                    gvGLExpenses.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvGLExpenses.DataSource = dt
                    gvGLExpenses.DataBind()
                    gvGLExpenses.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
#End Region
    
    Sub listSummaryStatistic()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcct_Coa.businessid, iPxAcct_Coa.type, iPxAcct_Coa.Coa, iPxAcct_Coa.description, "
        sSQL += "(select SUM(a.Debit-a.Credit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "where a.Coa=iPxAcct_Coa.Coa and MONTH(b.TransDate)='" & forMonth & "' and year(b.TransDate)='" & forYear & "' and a.IsActive='Y' AND b.Status<>'D') as amountMtd, "
        If forMonth = "01" Then
            sSQL += "(select SUM(bln1) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "02" Then
            sSQL += "(select SUM(bln2) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "03" Then
            sSQL += "(select SUM(bln3) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "04" Then
            sSQL += "(select SUM(bln4) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "05" Then
            sSQL += "(select SUM(bln5) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "06" Then
            sSQL += "(select SUM(bln6) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "07" Then
            sSQL += "(select SUM(bln7) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "08" Then
            sSQL += "(select SUM(bln8) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "09" Then
            sSQL += "(select SUM(bln9) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "10" Then
            sSQL += "(select SUM(bln10) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "11" Then
            sSQL += "(select SUM(bln11) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        ElseIf forMonth = "12" Then
            sSQL += "(select SUM(bln12) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetMtd, "
        End If
        sSQL += "(select SUM(a.Debit-a.Credit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "where a.Coa=iPxAcct_Coa.Coa and year(b.TransDate)='" & forYear & "' and a.IsActive='Y' AND b.Status<>'D') as amountYtd, "
        sSQL += "(select SUM(bln1+bln2+bln3+bln4+bln5+bln6+bln7+bln8+bln9+bln10+bln11+bln12) from iPxAcctGL_COABudget where Coa=iPxAcct_Coa.Coa and periode='" & forYear & "')as budgetYtd "
        sSQL += "FROM iPxAcct_Coa "
        sSQL += "WHERE iPxAcct_Coa.businessid='" & Session("sBusinessID") & "' and iPxAcct_Coa.type ='Statistic Account'"
        If CheckBox1.Checked = True Then
            sSQL += " and (iPxAcct_Coa.Coa in (select coa from iPxAcctGL_JVdtl as a inner join iPxAcctGL_JVhdr as b ON b.businessid=a.businessid and b.TransID=a.TransID where a.businessid='" & Session("sBusinessID") & "' and YEAR(b.TransDate)='" & forYear & "' and a.isActive='Y' and b.Status<>'D' group by coa) or "
            sSQL += "iPxAcct_Coa.Coa in (select coa from iPxAcctGL_COABudget where businessid='" & Session("sBusinessID") & "' and periode ='" & forYear & "' "
            If forMonth = "01" Then
                sSQL += "and bln1 <> '0.0000'  "
            ElseIf forMonth = "02" Then
                sSQL += "and bln2 <> '0.0000'  "
            ElseIf forMonth = "03" Then
                sSQL += "and bln3 <> '0.0000'  "
            ElseIf forMonth = "04" Then
                sSQL += "and bln4 <> '0.0000'  "
            ElseIf forMonth = "05" Then
                sSQL += "and bln5 <> '0.0000'  "
            ElseIf forMonth = "06" Then
                sSQL += "and bln6 <> '0.0000'  "
            ElseIf forMonth = "07" Then
                sSQL += "and bln7 <> '0.0000'  "
            ElseIf forMonth = "08" Then
                sSQL += "and bln8 <> '0.0000'  "
            ElseIf forMonth = "09" Then
                sSQL += "and bln9 <> '0.0000'  "
            ElseIf forMonth = "10" Then
                sSQL += "and bln10 <> '0.0000'  "
            ElseIf forMonth = "11" Then
                sSQL += "and bln11 <> '0.0000'  "
            ElseIf forMonth = "12" Then
                sSQL += "and bln12 <> '0.0000'  "
            End If
            sSQL += "))"
        End If
        sSQL += "order by iPxAcct_Coa.description asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvGLStatistic.DataSource = dt
                    gvGLStatistic.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvGLStatistic.DataSource = dt
                    gvGLStatistic.DataBind()
                    gvGLStatistic.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
#Region "Detail Coa"
    Sub listDetailCOAAsset()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.*, (b.Description) AS GroupGL, "
        sSQL += "(select SUM(x.Debit) from iPxAcctGL_JVdtl as x INNER JOIN iPxAcctGL_JVhdr as y "
        sSQL += "ON x.businessid = y.businessid COLLATE Latin1_General_CI_AS AND x.TransID = y.TransID "
        sSQL += "where x.Coa='" & Session("sCoaDetail") & "' and MONTH(y.TransDate)='" & forMonth & "' and year(y.TransDate)='" & forYear & "' and x.IsActive='Y' and x.TransID=a.TransID) as debit, "
        sSQL += "(select SUM(x.Credit) from iPxAcctGL_JVdtl as x INNER JOIN iPxAcctGL_JVhdr as y "
        sSQL += "ON x.businessid = y.businessid COLLATE Latin1_General_CI_AS AND x.TransID = y.TransID "
        sSQL += "where x.Coa='" & Session("sCoaDetail") & "' and MONTH(y.TransDate)='" & forMonth & "' and year(y.TransDate)='" & forYear & "' and x.IsActive='Y' and x.TransID=a.TransID) as credit, "
        sSQL += "(select SUM(x.Debit-x.Credit) from iPxAcctGL_JVdtl as x INNER JOIN iPxAcctGL_JVhdr as y "
        sSQL += "ON x.businessid = y.businessid COLLATE Latin1_General_CI_AS AND x.TransID = y.TransID "
        sSQL += "where x.Coa='" & Session("sCoaDetail") & "' and MONTH(y.TransDate)='" & forMonth & "' and year(y.TransDate)='" & forYear & "' and x.IsActive='Y' and x.TransID=a.TransID) as balance "
        sSQL += "from iPxAcctGL_JVhdr AS a "
        sSQL += "INNER JOIN iPxAcctGL_JVGrp AS b ON b.id=a.JVgroup "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.Status<>'D' and MONTH(a.TransDate)='" & forMonth & "' and year(a.TransDate)='" & forYear & "' "
        sSQL += "and (select b.TransID from iPxAcctGL_JVdtl AS b WHERE b.businessid='" & Session("sBusinessID") & "' "
        sSQL += "and b.Coa='" & Session("sCoaDetail") & "' and b.isActive='Y' and b.TransID=a.TransID group by TransID)<>'ISNULL' "
        sSQL += "Order by a.TransDate"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDetail.DataSource = dt
                    gvDetail.DataBind()
                    Dim totalDeb As Decimal = dt.Compute("Sum(debit)", "").ToString()
                    Dim totalCre As Decimal = dt.Compute("Sum(Credit)", "").ToString()
                    Dim totalBal As Decimal = dt.Compute("Sum(balance)", "").ToString()
                    gvDetail.FooterRow.Cells(4).Text = "Total"
                    gvDetail.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Left
                    gvDetail.FooterRow.Cells(5).Text = totalDeb.ToString("N2")
                    gvDetail.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                    gvDetail.FooterRow.Cells(6).Text = totalCre.ToString("N2")
                    gvDetail.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Right
                    gvDetail.FooterRow.Cells(7).Text = totalBal.ToString("N2")
                    gvDetail.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                    gvDetail.FooterRow.Cells(7).CssClass = "cellOneCellPaddingRight"
                    gvDetail.Enabled = True
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
    Sub listDetailCOALiability()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.*, (b.Description) AS GroupGL, "
        sSQL += "(select SUM(x.Debit) from iPxAcctGL_JVdtl as x INNER JOIN iPxAcctGL_JVhdr as y "
        sSQL += "ON x.businessid = y.businessid COLLATE Latin1_General_CI_AS AND x.TransID = y.TransID "
        sSQL += "where x.Coa='" & Session("sCoaDetail") & "' and MONTH(y.TransDate)='" & forMonth & "' and year(y.TransDate)='" & forYear & "' and x.IsActive='Y' and x.TransID=a.TransID) as debit, "
        sSQL += "(select SUM(x.Credit) from iPxAcctGL_JVdtl as x INNER JOIN iPxAcctGL_JVhdr as y "
        sSQL += "ON x.businessid = y.businessid COLLATE Latin1_General_CI_AS AND x.TransID = y.TransID "
        sSQL += "where x.Coa='" & Session("sCoaDetail") & "' and MONTH(y.TransDate)='" & forMonth & "' and year(y.TransDate)='" & forYear & "' and x.IsActive='Y' and x.TransID=a.TransID) as credit, "
        sSQL += "(select SUM(x.Credit-x.Debit) from iPxAcctGL_JVdtl as x INNER JOIN iPxAcctGL_JVhdr as y "
        sSQL += "ON x.businessid = y.businessid COLLATE Latin1_General_CI_AS AND x.TransID = y.TransID "
        sSQL += "where x.Coa='" & Session("sCoaDetail") & "' and MONTH(y.TransDate)='" & forMonth & "' and year(y.TransDate)='" & forYear & "' and x.IsActive='Y' and x.TransID=a.TransID) as balance "
        sSQL += "from iPxAcctGL_JVhdr AS a "
        sSQL += "INNER JOIN iPxAcctGL_JVGrp AS b ON b.id=a.JVgroup "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.Status<>'D' and MONTH(a.TransDate)='" & forMonth & "' and year(a.TransDate)='" & forYear & "' "
        sSQL += "and (select b.TransID from iPxAcctGL_JVdtl AS b WHERE b.businessid='" & Session("sBusinessID") & "' "
        sSQL += "and b.Coa='" & Session("sCoaDetail") & "' and b.isActive='Y' and b.TransID=a.TransID group by TransID)<>'ISNULL' "
        sSQL += "Order by a.TransDate"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDetail.DataSource = dt
                    gvDetail.DataBind()
                    Dim totalDeb As Decimal = dt.Compute("Sum(debit)", "").ToString()
                    Dim totalCre As Decimal = dt.Compute("Sum(Credit)", "").ToString()
                    Dim totalBal As Decimal = dt.Compute("Sum(balance)", "").ToString()
                    gvDetail.FooterRow.Cells(4).Text = "Total"
                    gvDetail.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Left
                    gvDetail.FooterRow.Cells(5).Text = totalDeb.ToString("N2")
                    gvDetail.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                    gvDetail.FooterRow.Cells(6).Text = totalCre.ToString("N2")
                    gvDetail.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Right
                    gvDetail.FooterRow.Cells(7).Text = totalBal.ToString("N2")
                    gvDetail.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                    gvDetail.FooterRow.Cells(7).CssClass = "cellOneCellPaddingRight"
                    gvDetail.Enabled = True
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
#End Region
#Region "Detail Coa Transaction"
    Public Function listHeaderGL(ByVal businessid As String, ByVal TransID As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*, (b.Description) AS GlGrp from iPxAcctGL_JVhdr AS a INNER JOIN iPxAcctGL_JVGrp AS b ON b.id=a.JVgroup "
        sSQL += " WHERE a.businessid='" & businessid & "' and a.TransID='" & TransID & "' and a.Status <> 'D'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvHeaderGL.DataSource = dt
                    gvHeaderGL.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvHeaderGL.DataSource = dt
                    gvHeaderGL.DataBind()
                    gvHeaderGL.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Function
    Public Function listDetailCOALvl2a(ByVal businessid As String, ByVal coa As String, ByVal TransID As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.TransID, a.RecID, b.TransDate, a.Coa, (c.description) as coaDesc, a.Description, a.Reff, a.debit, a.Credit, (a.Debit-a.Credit) as balance from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "INNER JOIN iPxAcct_Coa as c ON c.businessid=a.businessid and c.Coa=a.Coa "
        sSQL += " WHERE a.businessid='" & businessid & "' and b.TransID='" & TransID & "' and a.isActive='Y'"
        sSQL += "order by a.TransID, a.RecID asc "
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDetailCoaTrans.DataSource = dt
                    gvDetailCoaTrans.DataBind()
                    Dim totalDeb As Decimal = dt.Compute("Sum(debit)", "").ToString()
                    Dim totalCre As Decimal = dt.Compute("Sum(Credit)", "").ToString()
                    Dim totalBal As Decimal = dt.Compute("Sum(balance)", "").ToString()
                    gvDetailCoaTrans.FooterRow.Cells(4).Text = "Total"
                    gvDetailCoaTrans.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Left
                    gvDetailCoaTrans.FooterRow.Cells(5).Text = totalDeb.ToString("N2")
                    gvDetailCoaTrans.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                    gvDetailCoaTrans.FooterRow.Cells(6).Text = totalCre.ToString("N2")
                    gvDetailCoaTrans.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Right
                    gvDetailCoaTrans.FooterRow.Cells(7).Text = totalBal.ToString("N2")
                    gvDetailCoaTrans.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                    gvDetailCoaTrans.Enabled = True
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvDetailCoaTrans.DataSource = dt
                    gvDetailCoaTrans.DataBind()
                    gvDetailCoaTrans.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Function
    Public Function listDetailCOALvl2b(ByVal businessid As String, ByVal coa As String, ByVal TransID As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.TransID, a.RecID, b.TransDate, a.Coa, (c.description) as coaDesc, a.Description, a.Reff, a.debit, a.Credit, (a.Credit-a.Debit) as balance from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "INNER JOIN iPxAcct_Coa as c ON c.businessid=a.businessid and c.Coa=a.Coa "
        sSQL += " WHERE a.businessid='" & businessid & "' and b.TransID='" & TransID & "' and a.isActive='Y'"
        sSQL += "order by a.TransID, a.RecID asc "
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDetailCoaTrans.DataSource = dt
                    gvDetailCoaTrans.DataBind()
                    Dim totalDeb As Decimal = dt.Compute("Sum(debit)", "").ToString()
                    Dim totalCre As Decimal = dt.Compute("Sum(Credit)", "").ToString()
                    Dim totalBal As Decimal = dt.Compute("Sum(balance)", "").ToString()
                    gvDetailCoaTrans.FooterRow.Cells(4).Text = "Total"
                    gvDetailCoaTrans.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Left
                    gvDetailCoaTrans.FooterRow.Cells(5).Text = totalDeb.ToString("N2")
                    gvDetailCoaTrans.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                    gvDetailCoaTrans.FooterRow.Cells(6).Text = totalCre.ToString("N2")
                    gvDetailCoaTrans.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Right
                    gvDetailCoaTrans.FooterRow.Cells(7).Text = totalBal.ToString("N2")
                    gvDetailCoaTrans.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                    gvDetailCoaTrans.Enabled = True
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvDetailCoaTrans.DataSource = dt
                    gvDetailCoaTrans.DataBind()
                    gvDetailCoaTrans.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Function
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        tbDate.Enabled = False
        If Not Me.IsPostBack Then
            CheckBox1.Checked = True
            show_Group()
            tbDate.Text = Format(Now, "MM-yyyy")
            Dim dateBirthday As Date = tbDate.Text
            forMonth = dateBirthday.ToString("MM")
            forYear = dateBirthday.ToString("yyy")
            If forMonth = "01" Then
                lastMonth = "12"
                lastYear = forYear - 1
            Else
                lastMonth = forMonth - 1
                lastYear = forYear
            End If
            'listSummaryAsset()
            'listSummaryLiability()
            'listSummaryEquity()
            'totAsset()
            'totEquity()
            'totLiability()
            'Label6.Text = String.Format("{0:N2}", (Label1.Text - (Label2.Text + Label3.Text))).ToString
            'If Label6.Text = "0.00" Then
            '    Label6.Text = "0"
            'End If
            lblInfo1.Text = "Revenue"
            lblInfo2.Text = "Cost"
            lblInfo3.Text = "Expenses"
            listSummaryRevenue()
            ListSummaryCost()
            ListSummaryExpenses()
            totRevenue()
            totCost()
            totExpenses()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "$(document).ready(function() {StatistiNonActive()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ProfitTabShow", "$(document).ready(function() {ProfitTabShow()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "revenueActive", "$(document).ready(function() {revenueActive()});", True)
            Label6.Text = String.Format("{0:N2}", (Val(x) - (Val(y) + Val(z)))).ToString
            If Label6.Text = "0.00" Then
                Label6.Text = "0"
            End If
            'listDetailCOA()
        Else
            Dim dateBirthday As Date = tbDate.Text
            forMonth = dateBirthday.ToString("MM")
            forYear = dateBirthday.ToString("yyy")
            If forMonth = "01" Then
                lastMonth = "12"
                lastYear = forYear - 1
            Else
                lastMonth = forMonth - 1
                lastYear = forYear
            End If
            group = 0
            If dlGroup.SelectedIndex = 0 Then
                lblInfo1.Text = "Revenue"
                lblInfo2.Text = "Cost"
                lblInfo3.Text = "Expenses"
                listSummaryRevenue()
                ListSummaryCost()
                ListSummaryExpenses()
                totRevenue()
                totCost()
                totExpenses()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ProfitTabShow", "ProfitTabShow()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "revenueActive", "revenueActive()", True)
                Label6.Text = String.Format("{0:N2}", (Val(x) - (Val(y) + Val(z)))).ToString
                If Label6.Text = "0.00" Then
                    Label6.Text = "0"
                End If
            ElseIf dlGroup.SelectedIndex = 1 Then
                lblInfo1.Text = "Statistic Account"
                listSummaryStatistic()
                totStatistic()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiActive", "StatistiActive()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticTabShow", "StatisticTabShow()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticTabActive", "StatisticTabActive()", True)
            End If
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthGL", "$(document).ready(function() {MonthGL()});", True)
    End Sub
    Protected Sub cari(ByVal sender As Object, ByVal e As EventArgs)
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        If forMonth = "01" Then
            lastMonth = "12"
            lastYear = forYear - 1
        Else
            lastMonth = forMonth - 1
            lastYear = forYear
        End If
        group = 0
        If dlGroup.SelectedIndex = 0 Then
            lblInfo1.Text = "Revenue"
            lblInfo2.Text = "Cost"
            lblInfo3.Text = "Expenses"
            listSummaryRevenue()
            ListSummaryCost()
            ListSummaryExpenses()
            totRevenue()
            totCost()
            totExpenses()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ProfitTabShow", "ProfitTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "revenueActive", "revenueActive()", True)
            Label6.Text = String.Format("{0:N2}", (Val(x) - (Val(y) + Val(z)))).ToString
            If Label6.Text = "0.00" Then
                Label6.Text = "0"
            End If
        ElseIf dlGroup.SelectedIndex = 1 Then
            lblInfo1.Text = "Statistic Account"
            listSummaryStatistic()
            totStatistic()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiActive", "StatistiActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticTabShow", "StatisticTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticTabActive", "StatisticTabActive()", True)
        End If
    End Sub
    Private tmpCategoryName As String = ""
    Protected Sub OnRowDataBoundAsset(ByVal sender As Object, ByVal e As GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Curent As String = e.Row.Cells(1).Text
            Dim Last As String = e.Row.Cells(2).Text
            Dim Net As Integer
            If (Curent = "&nbsp;" Or Curent = "0.0000") And (Last = "&nbsp;" Or Last = "0.0000") Then
                Net = 0
            ElseIf (Curent <> "&nbsp;" Or Curent <> "0.0000") And (Last = "&nbsp;" Or Last = "0.0000") Then
                Net = (Val(Curent))
            ElseIf (Curent = "&nbsp;" Or Curent = "0.0000") And (Last <> "&nbsp;" Or Last <> "0.0000") Then
                Net = (0 - Val(Last))
            Else
                Net = (Val(Curent) - Val(Last))
            End If
            e.Row.Cells(6).Text = String.Format("{0:N2}", (Net)).ToString
        End If
    End Sub
    Protected Sub OnRowDataBoundEquity(ByVal sender As Object, ByVal e As GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Curent As String = e.Row.Cells(1).Text
            Dim Last As String = e.Row.Cells(2).Text
            Dim Net As Integer
            If (Curent = "&nbsp;" Or Curent = "0.0000") And (Last = "&nbsp;" Or Last = "0.0000") Then
                Net = 0
            ElseIf (Curent <> "&nbsp;" Or Curent <> "0.0000") And (Last = "&nbsp;" Or Last = "0.0000") Then
                Net = (Val(Curent))
            ElseIf (Curent = "&nbsp;" Or Curent = "0.0000") And (Last <> "&nbsp;" Or Last <> "0.0000") Then
                Net = (0 - Val(Last))
            Else
                Net = (Val(Curent) - Val(Last))
            End If
            e.Row.Cells(6).Text = String.Format("{0:N2}", (Net)).ToString
        End If
    End Sub
    Protected Sub OnRowDataBoundLiability(ByVal sender As Object, ByVal e As GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Curent As String = e.Row.Cells(1).Text
            Dim Last As String = e.Row.Cells(2).Text
            Dim Net As Integer
            If (Curent = "&nbsp;" Or Curent = "0.0000") And (Last = "&nbsp;" Or Last = "0.0000") Then
                Net = 0
            ElseIf (Curent <> "&nbsp;" Or Curent <> "0.0000") And (Last = "&nbsp;" Or Last = "0.0000") Then
                Net = (Val(Curent))
            ElseIf (Curent = "&nbsp;" Or Curent = "0.0000") And (Last <> "&nbsp;" Or Last <> "0.0000") Then
                Net = (0 - Val(Last))
            Else
                Net = (Val(Curent) - Val(Last))
            End If
            e.Row.Cells(6).Text = String.Format("{0:N2}", (Net)).ToString
        End If
    End Sub
    ' To keep track of the previous row Group Identifier
    Dim strPreviousRowID As String = String.Empty
    ' To keep track the Index of Group Total
    Dim intSubTotalIndex As Integer = 1
    Dim group As Integer = 0

    ' To temporarily store Group Total
    Dim dblSubTotalOpen As Double = 0
    Dim dblSubTotalDebit As Double = 0
    Dim dblSubTotalKredit As Double = 0
    Dim dblSubTotalBalance As Double = 0

    Protected Sub gvGLAsset_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGLAsset.RowCommand
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        group = 0
        If e.CommandName = "getDetail" Then
            Session("sCoaDetail") = e.CommandArgument
            lblInfo1.Text = "Asset"
            lblInfo2.Text = "Equity"
            lblInfo3.Text = "Liability"
            listSummaryAsset()
            listSummaryLiability()
            listSummaryEquity()
            totAsset()
            totEquity()
            totLiability()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BalanceTabShow", "BalanceTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AssetActive", "AssetActive()", True)
            listDetailCOAAsset()
            lbTitleDetailCoa.Text = "Detail Coa Asset"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            'listDetailCOA()
        End If
    End Sub

    Protected Sub gvGLLiability_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGLLiability.RowCommand
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        group = 0
        If e.CommandName = "getDetail" Then
            Session("sCoaDetail") = e.CommandArgument
            lblInfo1.Text = "Asset"
            lblInfo2.Text = "Equity"
            lblInfo3.Text = "Liability"
            listSummaryAsset()
            listSummaryLiability()
            listSummaryEquity()
            totAsset()
            totEquity()
            totLiability()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BalanceTabShow", "BalanceTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "LiabilityActive", "LiabilityActive()", True)
            listDetailCOALiability()
            lbTitleDetailCoa.Text = "Detail Coa Liability"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            'listDetailCOA()
        End If
    End Sub

    Protected Sub gvGLEquity_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGLEquity.RowCommand
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        group = 0
        If e.CommandName = "getDetail" Then
            Session("sCoaDetail") = e.CommandArgument
            lblInfo1.Text = "Asset"
            lblInfo2.Text = "Equity"
            lblInfo3.Text = "Liability"
            listSummaryAsset()
            listSummaryLiability()
            listSummaryEquity()
            totAsset()
            totEquity()
            totLiability()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BalanceTabShow", "BalanceTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "EquityActive", "EquityActive()", True)
            listDetailCOALiability()
            lbTitleDetailCoa.Text = "Detail Coa Equity"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            'listDetailCOA()
        End If
    End Sub

    Protected Sub gvGLRevenue_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGLRevenue.RowCommand
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        group = 0
        If e.CommandName = "getDetail" Then
            Session("sCoaDetail") = e.CommandArgument
            Dim a As String = e.CommandArgument
            lblInfo1.Text = "Revenue"
            lblInfo2.Text = "Cost"
            lblInfo3.Text = "Expenses"
            listSummaryRevenue()
            ListSummaryCost()
            ListSummaryExpenses()
            totRevenue()
            totCost()
            totExpenses()
            listDetailCOALiability()
            lbTitleDetailCoa.Text = "Detail Coa Revenue"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ProfitTabShow", "ProfitTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "revenueActive", "revenueActive()", True)
            'listDetailCOA()
        End If
    End Sub

    Protected Sub gvGLCost_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGLCost.RowCommand
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        group = 0
        If e.CommandName = "getDetail" Then
            Session("sCoaDetail") = e.CommandArgument
            lblInfo1.Text = "Revenue"
            lblInfo2.Text = "Cost"
            lblInfo3.Text = "Expenses"
            listSummaryRevenue()
            ListSummaryCost()
            ListSummaryExpenses()
            totRevenue()
            totCost()
            totExpenses()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ProfitTabShow", "ProfitTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "costActive()", True)
            listDetailCOAAsset()
            lbTitleDetailCoa.Text = "Detail Coa Cost"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        'listDetailCOA()
        End If
    End Sub

    Protected Sub gvGLExpenses_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGLExpenses.RowCommand
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        group = 0
        If e.CommandName = "getDetail" Then
            Dim a As String = e.CommandArgument
            'Session("sCoaDetail") = e.CommandArgument
            lblInfo1.Text = "Revenue"
            lblInfo2.Text = "Cost"
            lblInfo3.Text = "Expenses"
            listSummaryRevenue()
            ListSummaryCost()
            ListSummaryExpenses()
            totRevenue()
            totCost()
            totExpenses()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ProfitTabShow", "ProfitTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ExpensesActive", "ExpensesActive()", True)
            listDetailCOAAsset()
            lbTitleDetailCoa.Text = "Detail Coa Expenses"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            'listDetailCOA()
        End If
    End Sub

    Protected Sub gvGLStatistic_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGLStatistic.RowCommand
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        group = 0
        If e.CommandName = "getDetail" Then
            Session("sCoaDetail") = e.CommandArgument
            lblInfo1.Text = "Statistic Account"
            listSummaryStatistic()
            totStatistic()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiActive", "StatistiActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticTabShow", "StatisticTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticTabActive", "StatisticTabActive()", True)
            listDetailCOAAsset()
            lbTitleDetailCoa.Text = "Detail Coa Statistic"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        'listDetailCOA()
        End If
    End Sub

    Protected Sub dlGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlGroup.SelectedIndexChanged
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        group = 0
        'If dlGroup.SelectedIndex = 0 Then
        Select Case dlGroup.SelectedIndex
            'Case 0
            '    lblInfo1.Text = "Asset"
            '    lblInfo2.Text = "Equity"
            '    lblInfo3.Text = "Liability"
            '    Label5.Text = "Control Balance"
            '    listSummaryAsset()
            '    totAsset()
            '    totEquity()
            '    totLiability()
            '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BalanceTabShow", "BalanceTabShow()", True)
            '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AssetActive", "AssetActive()", True)
            '    Label6.Text = String.Format("{0:N2}", (Val(x) - (Val(y) + Val(z)))).ToString
            '    If Label6.Text = "0.00" Then
            '        Label6.Text = "0"
            '    End If
            Case 0
                lblInfo1.Text = "Revenue"
                lblInfo2.Text = "Cost"
                lblInfo3.Text = "Expenses"
                Label5.Text = "Profit and Loss"
                listSummaryRevenue()
                ListSummaryCost()
                ListSummaryExpenses()
                totRevenue()
                totCost()
                totExpenses()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ProfitTabShow", "ProfitTabShow()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "revenueActive", "revenueActive()", True)
                Label6.Text = String.Format("{0:N2}", (Val(x) - (Val(y) + Val(z)))).ToString
                If Label6.Text = "0.00" Then
                    Label6.Text = "0"
                End If
            Case 1
                lblInfo1.Text = "Statistic Account"
                listSummaryStatistic()
                totStatistic()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiActive", "StatistiActive()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticTabShow", "StatisticTabShow()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticTabActive", "StatisticTabActive()", True)
        End Select
        
        'ElseIf dlGroup.SelectedIndex = 1 Then

        'ElseIf dlGroup.SelectedIndex = 2 Then

        'End If
    End Sub

    Protected Sub lbAbortDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDetail.Click
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
        If lbTitleDetailCoa.Text = "Detail Coa Asset" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BalanceTabShow", "BalanceTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AssetActive", "AssetActive()", True)
        ElseIf lbTitleDetailCoa.Text = "Detail Coa Liability" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BalanceTabShow", "BalanceTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "LiabilityActive", "LiabilityActive()", True)
        ElseIf lbTitleDetailCoa.Text = "Detail Coa Equity" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BalanceTabShow", "BalanceTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "EquityActive", "EquityActive()", True)
        ElseIf lbTitleDetailCoa.Text = "Detail Coa Revenue" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ProfitTabShow", "ProfitTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "revenueActive", "revenueActive()", True)
        ElseIf lbTitleDetailCoa.Text = "Detail Coa Cost" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ProfitTabShow", "ProfitTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
        ElseIf lbTitleDetailCoa.Text = "Detail Coa Expenses" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ProfitTabShow", "ProfitTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ExpensesActive", "ExpensesActive()", True)
        ElseIf lbTitleDetailCoa.Text = "Detail Coa Statistic" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiActive", "StatistiActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticTabShow", "StatisticTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticTabActive", "StatisticTabActive()", True)
        End If
        listSummaryAsset()
        listSummaryEquity()
        listSummaryLiability()
        listSummaryRevenue()
        ListSummaryCost()
        ListSummaryExpenses()
        listSummaryStatistic()
    End Sub

    Protected Sub gvDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDetail.RowCommand
        If e.CommandName = "getDetail" Then
            Label4.Text = "Detail GL Jurnal " + e.CommandArgument
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            If lbTitleDetailCoa.Text = "Detail Coa Asset" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BalanceTabShow", "BalanceTabShow()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AssetActive", "AssetActive()", True)
                listHeaderGL(Session("sBusinessID"), e.CommandArgument)
                listDetailCOALvl2a(Session("sBusinessID"), Session("sCoaDetail"), e.CommandArgument)
            ElseIf lbTitleDetailCoa.Text = "Detail Coa Liability" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BalanceTabShow", "BalanceTabShow()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "LiabilityActive", "LiabilityActive()", True)
                listHeaderGL(Session("sBusinessID"), e.CommandArgument)
                listDetailCOALvl2b(Session("sBusinessID"), Session("sCoaDetail"), e.CommandArgument)
            ElseIf lbTitleDetailCoa.Text = "Detail Coa Equity" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BalanceTabShow", "BalanceTabShow()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "EquityActive", "EquityActive()", True)
                listHeaderGL(Session("sBusinessID"), e.CommandArgument)
                listDetailCOALvl2b(Session("sBusinessID"), Session("sCoaDetail"), e.CommandArgument)
            ElseIf lbTitleDetailCoa.Text = "Detail Coa Revenue" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ProfitTabShow", "ProfitTabShow()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "revenueActive", "revenueActive()", True)
                listHeaderGL(Session("sBusinessID"), e.CommandArgument)
                listDetailCOALvl2b(Session("sBusinessID"), Session("sCoaDetail"), e.CommandArgument)
            ElseIf lbTitleDetailCoa.Text = "Detail Coa Cost" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ProfitTabShow", "ProfitTabShow()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
                listHeaderGL(Session("sBusinessID"), e.CommandArgument)
                listDetailCOALvl2a(Session("sBusinessID"), Session("sCoaDetail"), e.CommandArgument)
            ElseIf lbTitleDetailCoa.Text = "Detail Coa Expenses" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ProfitTabShow", "ProfitTabShow()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ExpensesActive", "ExpensesActive()", True)
                listHeaderGL(Session("sBusinessID"), e.CommandArgument)
                listDetailCOALvl2a(Session("sBusinessID"), Session("sCoaDetail"), e.CommandArgument)
            ElseIf lbTitleDetailCoa.Text = "Detail Coa Statistic" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiActive", "StatistiActive()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticTabShow", "StatisticTabShow()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticTabActive", "StatisticTabActive()", True)
                listHeaderGL(Session("sBusinessID"), e.CommandArgument)
                listDetailCOALvl2a(Session("sBusinessID"), Session("sCoaDetail"), e.CommandArgument)
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCreatType", "showModalCreatType()", True)
        End If
    End Sub

    Protected Sub lbAbortDetailLv2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDetailLv2.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCreatType", "hideModalCreatType()", True)
        If lbTitleDetailCoa.Text = "Detail Coa Asset" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BalanceTabShow", "BalanceTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AssetActive", "AssetActive()", True)
        ElseIf lbTitleDetailCoa.Text = "Detail Coa Liability" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BalanceTabShow", "BalanceTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "LiabilityActive", "LiabilityActive()", True)
        ElseIf lbTitleDetailCoa.Text = "Detail Coa Equity" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BalanceTabShow", "BalanceTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "EquityActive", "EquityActive()", True)
        ElseIf lbTitleDetailCoa.Text = "Detail Coa Revenue" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ProfitTabShow", "ProfitTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "revenueActive", "revenueActive()", True)
        ElseIf lbTitleDetailCoa.Text = "Detail Coa Cost" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ProfitTabShow", "ProfitTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
        ElseIf lbTitleDetailCoa.Text = "Detail Coa Expenses" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ProfitTabShow", "ProfitTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ExpensesActive", "ExpensesActive()", True)
        ElseIf lbTitleDetailCoa.Text = "Detail Coa Statistic" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiActive", "StatistiActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticTabShow", "StatisticTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticTabActive", "StatisticTabActive()", True)
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
    End Sub

    Protected Sub lbPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPrint.Click
        Session("sReport") = "GLSummary"
        If dlGroup.SelectedIndex = 0 Then
            Session("sMapPath") = "~/iPxReportFile/dckGL_Profit&Lost.rpt"
            Session("sPeriod") = tbDate.Text
            Response.Redirect("rptviewer.aspx")
        ElseIf dlGroup.SelectedIndex = 1 Then
            Session("sMapPath") = "~/iPxReportFile/dckGL_Journal.rpt"
        End If
    End Sub

    Protected Sub CheckBox1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        If forMonth = "01" Then
            lastMonth = "12"
            lastYear = forYear - 1
        Else
            lastMonth = forMonth - 1
            lastYear = forYear
        End If
        group = 0
        If dlGroup.SelectedIndex = 0 Then
            lblInfo1.Text = "Revenue"
            lblInfo2.Text = "Cost"
            lblInfo3.Text = "Expenses"
            listSummaryRevenue()
            ListSummaryCost()
            ListSummaryExpenses()
            totRevenue()
            totCost()
            totExpenses()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiNonActive", "StatistiNonActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ProfitTabShow", "ProfitTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "revenueActive", "revenueActive()", True)
            Label6.Text = String.Format("{0:N2}", (Val(x) - (Val(y) + Val(z)))).ToString
            If Label6.Text = "0.00" Then
                Label6.Text = "0"
            End If
        ElseIf dlGroup.SelectedIndex = 1 Then
            lblInfo1.Text = "Statistic Account"
            listSummaryStatistic()
            totStatistic()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatistiActive", "StatistiActive()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticTabShow", "StatisticTabShow()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticTabActive", "StatisticTabActive()", True)
        End If
    End Sub
End Class
