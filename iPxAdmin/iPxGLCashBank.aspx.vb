Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Imports System.Web.Services
Imports System.Web.Script.Serialization
Imports System.Collections.Generic
Partial Class iPxAdmin_iPxGLCashBank
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, forMonth, forYear, a, forMonthCash, forYearCash, x As String
    Dim cIpx As New iPxClass
    <System.Web.Services.WebMethod()> _
    Public Shared Function ChartCash(ByVal dateAnalys As String) As String
        Dim businessID = HttpContext.Current.Session("sBusinessID").ToString()
        Dim Order As New List(Of Object)()
        Dim sSQL As String
        Dim a, b, thn, bln, Cash, namaBln, x As String
        Dim i As Integer
        i = 1
        Cash = "cash" & i & ","
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
                    sSQL += "(select SUM(a.Debit-a.Credit) from iPxAcctGL_JVdtl as a "
                    sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
                    sSQL += "INNER JOIN iPxAcct_Coa as c ON c.businessid=a.businessid and c.Coa=a.Coa "
                    sSQL += "WHERE a.businessid='" & businessID & "' and c.Status ='Cash Bank Account' and b.TransDate between '1900-01-01' and '" & thn & "-" & bln & "-" & x & "' "
                    sSQL += "and b.status<>'D' and a.isActive='Y' ) as " & Cash & ""

                    i += 1
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
                        Order.Add(New With {.bln1 = sdr("bln1"), .Cash1 = sdr("cash1"), _
                                            .bln2 = sdr("bln2"), .Cash2 = sdr("cash2"), _
                                            .bln3 = sdr("bln3"), .Cash3 = sdr("cash3"), _
                                            .bln4 = sdr("bln4"), .Cash4 = sdr("cash4"), _
                                            .bln5 = sdr("bln5"), .Cash5 = sdr("cash5"), _
                                            .bln6 = sdr("bln6"), .Cash6 = sdr("cash6")})
                    End While
                End Using
                conn.Close()
            End Using
            Return (New JavaScriptSerializer().Serialize(Order))
        End Using
    End Function
    Sub listSummary()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        x = Date.DaysInMonth(forYear, forMonth)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcct_Coa.businessid, iPxAcct_Coa.type, iPxAcct_Coa.Coa, iPxAcct_Coa.description, "
        sSQL += "(select SUM(a.Debit-a.Credit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "where a.Coa=iPxAcct_Coa.Coa and b.TransDate between '1900-01-01' and '" & forYear & "-" & forMonth & "-" & x & "' and b.status<>'D' and a.isActive='Y') as amountDebit "
        'sSQL += "(select SUM(a.Debit) from iPxAcctGL_JVdtl as a "
        'sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        'sSQL += "where a.Coa=iPxAcct_Coa.Coa and MONTH(b.TransDate)='" & forMonth & "' and year(b.TransDate)='" & forYear & "' and a.isActive='Y') as amountDebit, "
        'sSQL += "(select SUM(a.Credit) from iPxAcctGL_JVdtl as a "
        'sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        'sSQL += "where a.Coa=iPxAcct_Coa.Coa and MONTH(b.TransDate)='" & forMonth & "' and year(b.TransDate)='" & forYear & "' and a.isActive='Y') as amountCredit "
        sSQL += "FROM iPxAcct_Coa "
        sSQL += "WHERE iPxAcct_Coa.businessid='" & Session("sBusinessID") & "' and iPxAcct_Coa.Status ='Cash Bank Account' "
        sSQL += "order by iPxAcct_Coa.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCashSummary.DataSource = dt
                    gvCashSummary.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCashSummary.DataSource = dt
                    gvCashSummary.DataBind()
                    gvCashSummary.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListCashDetailOpen()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcct_Coa.businessid, iPxAcct_Coa.type, iPxAcct_Coa.Coa, iPxAcct_Coa.description, "
        sSQL += "(select SUM(a.Debit-a.Credit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "where a.Coa=iPxAcct_Coa.Coa and MONTH(b.TransDate)<'" & forMonth & "' and year(b.TransDate)<='" & forYear & "' and a.isActive='Y') as amountDebit "
        'sSQL += "(select SUM(a.Credit) from iPxAcctGL_JVdtl as a "
        'sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        'sSQL += "where a.Coa=iPxAcct_Coa.Coa and MONTH(b.TransDate)<'" & forMonth & "' and year(b.TransDate)<='" & forYear & "' and a.isActive='Y') as amountCredit "
        sSQL += "FROM iPxAcct_Coa "
        sSQL += "WHERE iPxAcct_Coa.businessid='" & Session("sBusinessID") & "' and iPxAcct_Coa.Coa ='" & Session("sCoaCash") & "' "
        sSQL += "order by iPxAcct_Coa.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDetailOpen.DataSource = dt
                    gvDetailOpen.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvDetailOpen.DataSource = dt
                    gvDetailOpen.DataBind()
                    gvDetailOpen.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListCashDetail()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select b.TransID, DAY(b.TransDate) as day, (c.Description) AS GlGrp, a.Description, a.Reff, a.Debit, a.Credit from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON b.businessid=a.businessid and b.TransID=a.TransID "
        sSQL += "INNER JOIN iPxAcctGL_JVGrp as c ON c.id=b.JVgroup "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.Coa='" & Session("sCoaCash") & "' "
        sSQL += "and MONTH(b.TransDate)='" & forMonth & "' and year(b.TransDate)='" & forYear & "' and b.Status <>'D' and a.isActive='Y' "
        sSQL += "order by b.TransDate asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDetailCash.DataSource = dt
                    gvDetailCash.DataBind()
                    'Calculate Sum and display in Footer Row
                    Dim totalDeb As Decimal = dt.Compute("Sum(Debit)", "").ToString()
                    Dim totalCre As Decimal = dt.Compute("Sum(Credit)", "").ToString()
                    gvDetailCash.FooterRow.Cells(3).Text = "Total"
                    gvDetailCash.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Left
                    gvDetailCash.FooterRow.Cells(4).Text = totalDeb.ToString("N2")
                    gvDetailCash.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right
                    gvDetailCash.FooterRow.Cells(5).Text = totalCre.ToString("N2")
                    gvDetailCash.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                    gvDetailCash.Enabled = True
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvDetailCash.DataSource = dt
                    gvDetailCash.DataBind()
                    gvDetailCash.Enabled = False
                    gvDetailCash.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListCashFlow()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim y, z, v As String
        z = Date.DaysInMonth(forYearCash, forMonthCash)
        If forMonthCash = "01" Then
            y = "12"
            v = forYearCash - 1
            x = Date.DaysInMonth(v, y)
        Else
            y = forMonthCash - 1
            v = forYearCash
            x = Date.DaysInMonth(v, y)
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select x.businessid, 'Open' as Grp, 'OPENING CASH & BANK' as Desch, "
        sSQL += "(select SUM(a.Debit-a.Credit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "INNER JOIN iPxAcct_Coa as c ON c.businessid=a.businessid and c.Coa=a.Coa "
        sSQL += "where a.businessid=x.businessid and c.Status ='Cash Bank Account' and b.TransDate between '1900-01-01' and '" & v & "-" & y & "-" & x & "' and b.status<>'D' and a.isActive='Y') as amount "
        sSQL += "from iPx_profile_client as x where x.businessid ='" & Session("sBusinessID") & "' "
        sSQL += "UNION ALL "
        sSQL += "select a.businessid, 'Debit' as Grp, d.description, SUM(a.Debit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "INNER JOIN iPxAcct_Coa as c ON c.businessid=a.businessid and c.Coa=a.Coa "
        sSQL += "INNER JOIN iPxAcctGL_JVGrp as d ON d.id=b.JVgroup "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and c.Status ='Cash Bank Account' and MONTH(b.TransDate)='" & forMonthCash & "' and year(b.TransDate)='" & forYearCash & "' and b.status<>'D' and a.isActive='Y' and a.Debit<>'0' "
        sSQL += "group by a.businessid, d.description "
        sSQL += "UNION ALL "
        sSQL += "select a.businessid, 'Debit' as Grp, 'Total Cash & Bank In' as Desch, SUM(a.Debit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "INNER JOIN iPxAcct_Coa as c ON c.businessid=a.businessid and c.Coa=a.Coa "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and c.Status ='Cash Bank Account' and MONTH(b.TransDate)='" & forMonthCash & "' and year(b.TransDate)='" & forYearCash & "' and b.status<>'D' and a.isActive='Y' and a.Debit<>'0' "
        sSQL += "group by a.businessid "
        sSQL += "UNION ALL "
        sSQL += "select a.businessid, 'Credit' as Grp, d.description, SUM(a.Credit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "INNER JOIN iPxAcct_Coa as c ON c.businessid=a.businessid and c.Coa=a.Coa "
        sSQL += "INNER JOIN iPxAcctGL_JVGrp as d ON d.id=b.JVgroup "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and c.Status ='Cash Bank Account' and MONTH(b.TransDate)='" & forMonthCash & "' and year(b.TransDate)='" & forYearCash & "' and b.status<>'D' and a.isActive='Y' and a.Credit<>'0' "
        sSQL += "group by a.businessid, d.description "
        sSQL += "UNION ALL "
        sSQL += "select a.businessid, 'Credit' as Grp, 'Total Cash & Bank Out' as Desch, SUM(a.Credit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "INNER JOIN iPxAcct_Coa as c ON c.businessid=a.businessid and c.Coa=a.Coa "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and c.Status ='Cash Bank Account' and MONTH(b.TransDate)='" & forMonthCash & "' and year(b.TransDate)='" & forYearCash & "' and b.status<>'D' and a.isActive='Y' and a.Credit<>'0' "
        sSQL += "group by a.businessid "
        sSQL += "UNION ALL "
        sSQL += "select a.businessid, 'Ending' as Grp, 'ENDING CASH & BANK' as Desch, SUM(a.Debit-a.Credit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "INNER JOIN iPxAcct_Coa as c ON c.businessid=a.businessid and c.Coa=a.Coa "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and c.Status ='Cash Bank Account' and b.TransDate between '1900-01-01' and '" & forYearCash & "-" & forMonthCash & "-" & z & "' and b.status<>'D' and a.isActive='Y' "
        sSQL += "group by a.businessid"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCashFlow.DataSource = dt
                    gvCashFlow.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCashFlow.DataSource = dt
                    gvCashFlow.DataBind()
                    gvCashFlow.Enabled = False
                    gvCashFlow.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub

    Public Function listDetailJournal(ByVal businessid As String, ByVal TransID As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.TransID, a.RecID, b.TransDate, a.Coa, (c.description) as coaDesc, a.Description, a.Reff, a.debit, a.Credit, (a.Debit-a.Credit) as balance from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "INNER JOIN iPxAcct_Coa as c ON c.businessid=a.businessid and c.Coa=a.Coa "
        sSQL += " WHERE a.businessid='" & businessid & "' and b.TransID='" & TransID & "' and b.Status<>'D' and a.isActive='Y'"
        sSQL += "order by a.TransID, a.RecID asc "
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
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Session("sCoaCash") = ""
            tbDateCash.Text = Format(Now, "MMMM yyyy")
            Dim dateBirthdayCash As Date = tbDateCash.Text
            forMonthCash = dateBirthdayCash.ToString("MM")
            forYearCash = dateBirthdayCash.ToString("yyy")
            ListCashFlow()

            tbDate.Text = Format(Now, "MMMM yyyy")
            Dim dateBirthday As Date = tbDate.Text
            forMonth = dateBirthday.ToString("MM")
            forYear = dateBirthday.ToString("yyy")
            listSummary()
            ListCashDetailOpen()
            ListCashDetail()

            'Session("sCoaCash") = ""
            'Session("sStatus") = "hidden"
        Else
            Dim dateBirthdayCash As Date = tbDateCash.Text
            forMonthCash = dateBirthdayCash.ToString("MM")
            forYearCash = dateBirthdayCash.ToString("yyy")
            ListCashFlow()

            Dim dateBirthday As Date = tbDate.Text
            forMonth = dateBirthday.ToString("MM")
            forYear = dateBirthday.ToString("yyy")
            listSummary()
        End If
        tbDate.Enabled = False
        tbDateCash.Enabled = False
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthGL", "$(document).ready(function() {MonthGL()});", True)
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvCashSummary.PageIndex = e.NewPageIndex
        Me.listSummary()
        'If Session("sStatus") = "Show" Then
        gvDetailCash.PageIndex = e.NewPageIndex
        Me.ListCashDetail()
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showDetail", "showDetail()", True)
        'ElseIf Session("sStatus") = "hidden" Then
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideDetail", "hideDetail()", True)
        'End If
    End Sub

    Protected Sub gvCashSummary_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCashSummary.PageIndexChanging
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        gvCashSummary.PageIndex = e.NewPageIndex
        Me.listSummary()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ControlActive", "ControlActive()", True)
        'If Session("sStatus") = "Show" Then
        '    ListCashDetail()
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showDetail", "showDetail()", True)
        'ElseIf Session("sStatus") = "hidden" Then
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideDetail", "hideDetail()", True)
        'End If
    End Sub

    Protected Sub gvDetailCash_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDetailCash.PageIndexChanging
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        gvDetailCash.PageIndex = e.NewPageIndex
        Me.ListCashDetail()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ControlActive", "ControlActive()", True)
        'If Session("sStatus") = "Show" Then
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showDetail", "showDetail()", True)
        'ElseIf Session("sStatus") = "hidden" Then
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideDetail", "hideDetail()", True)
        'End If
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvCashSummary.PageIndex = e.NewPageIndex
        Me.listSummary()
        gvDetailCash.PageIndex = e.NewPageIndex
        Me.ListCashDetail()
    End Sub

    Protected Sub cari(ByVal sender As Object, ByVal e As EventArgs)
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        listSummary()
        'ListCashDetail()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthGL", "$(document).ready(function() {MonthGL()});", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ControlActive", "ControlActive()", True)
        'If Session("sStatus") = "Show" Then
        '    ListCashDetail()
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showDetail", "showDetail()", True)
        'ElseIf Session("sStatus") = "hidden" Then
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideDetail", "hideDetail()", True)
        'End If
    End Sub

    Protected Sub cariCash(ByVal sender As Object, ByVal e As EventArgs)
        Dim dateBirthdayCash As Date = tbDateCash.Text
        forMonthCash = dateBirthdayCash.ToString("MM")
        forYearCash = dateBirthdayCash.ToString("yyy")
        Call ListCashFlow()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showGraphCash", "$(document).ready(function() {showGraphCash()});", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthGL", "$(document).ready(function() {MonthGL()});", True)
    End Sub

    Protected Sub gvCashSummary_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCashSummary.RowCommand
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        If e.CommandName = "getDetail" Then
            Session("sCoaCash") = e.CommandArgument
            ListCashDetailOpen()
            ListCashDetail()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ControlActive", "ControlActive()", True)
            'Session("sStatus") = "Show"
        End If
    End Sub
    Private tmpCategoryName As String = ""
    Private tmpHeaderName As String = ""
    Dim group As Integer = 0
    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim drv As DataRowView = CType(e.Row.DataItem, DataRowView)
            If drv("Desch").ToString() = "OPENING CASH & BANK" Or drv("Desch").ToString() = "Total Cash & Bank In" Or drv("Desch").ToString() = "Total Cash & Bank Out" Or drv("Desch").ToString() = "ENDING CASH & BANK" Then

            Else
                If tmpCategoryName <> drv("Grp").ToString() Then
                    tmpCategoryName = drv("Grp").ToString()
                    If drv("Grp").ToString() = "Debit" Then
                        tmpHeaderName = "CASH & BANK IN (Debit)"
                    ElseIf drv("Grp").ToString() = "Credit" Then
                        tmpHeaderName = "CASH & BANK OUT (Credit)"
                    End If
                    Dim tbl As Table = TryCast(e.Row.Parent, Table)

                    If tbl IsNot Nothing Then
                        Dim row As GridViewRow = New GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal)
                        Dim cell As TableCell = New TableCell()
                        cell.ColumnSpan = Me.gvCashFlow.Columns.Count
                        cell.Width = Unit.Percentage(100)
                        cell.Style.Add("Font-weight", "bold")
                        cell.Style.Add("background-color", "#fffff")
                        cell.Style.Add("color", "black")
                        cell.Style.Add("text-transform", "uppercase")
                        Dim span As HtmlGenericControl = New HtmlGenericControl("span")

                        span.InnerHtml = tmpHeaderName
                        cell.Controls.Add(span)
                        row.Cells.Add(cell)
                        tbl.Rows.AddAt(tbl.Rows.Count - 1, row)
                        group += 1
                    End If
                End If
            End If
        End If
    End Sub

    'Protected Sub lbCloseDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCloseDetail.Click
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideDetail", "hideDetail()", True)
    '    Session("sCoaCash") = ""
    '    Session("sStatus") = "hidden"
    'End Sub

    Protected Sub gvDetailCash_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDetailCash.RowCommand
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        If e.CommandName = "getDetail" Then
            Session("sJournalID") = e.CommandArgument
            lbTitleDetailJournal.Text = "Detail GL Jurnal " + e.CommandArgument
            listDetailJournal(Session("sBusinessID"), Session("sJournalID"))
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ControlActive", "ControlActive()", True)
        End If
    End Sub

    Protected Sub lbAbortDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDetail.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ControlActive", "ControlActive()", True)
    End Sub
End Class
