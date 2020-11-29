Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxGLClearance
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, forMonth, forYear, a As String
    Dim cIpx As New iPxClass
    Sub listSummary()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcct_Coa.businessid, iPxAcct_Coa.type, iPxAcct_Coa.Coa, iPxAcct_Coa.description, "
        sSQL += "(select SUM(a.Debit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "where a.Coa=iPxAcct_Coa.Coa and MONTH(b.TransDate)='" & forMonth & "' and year(b.TransDate)='" & forYear & "' and b.status<>'D' and a.isActive='Y') as amountDebit, "
        sSQL += "(select SUM(a.Credit) from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.TransID = b.TransID "
        sSQL += "where a.Coa=iPxAcct_Coa.Coa and MONTH(b.TransDate)='" & forMonth & "' and year(b.TransDate)='" & forYear & "' and b.status<>'D' and a.isActive='Y') as amountCredit "
        sSQL += "FROM iPxAcct_Coa "
        sSQL += "WHERE iPxAcct_Coa.businessid='" & Session("sBusinessID") & "' and iPxAcct_Coa.Status ='Clearance Account' "
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

    Sub ListCashDetail()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select  DAY(x.TransDate) as day, "
        sSQL += "(select  sum(a.Debit) from iPxAcctGL_JVdtl as a INNER JOIN iPxAcctGL_JVhdr as b ON b.businessid=a.businessid and b.TransID=a.TransID "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.Coa='" & Session("sCoaCash") & "' AND DAY(b.TransDate)=DAY(x.TransDate) and MONTH(b.TransDate)='" & forMonth & "' and year(b.TransDate)='" & forYear & "' and b.status<>'D' and a.isActive='Y') as debit,"
        sSQL += "(select  sum(a.Credit) from iPxAcctGL_JVdtl as a INNER JOIN iPxAcctGL_JVhdr as b ON b.businessid=a.businessid and b.TransID=a.TransID "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.Coa='" & Session("sCoaCash") & "' AND DAY(b.TransDate)=DAY(x.TransDate) and MONTH(b.TransDate)='" & forMonth & "' and year(b.TransDate)='" & forYear & "' and b.status<>'D' and a.isActive='Y') as credit, "
        sSQL += "(select  sum(a.Debit-a.Credit) from iPxAcctGL_JVdtl as a INNER JOIN iPxAcctGL_JVhdr as b ON b.businessid=a.businessid and b.TransID=a.TransID "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.Coa='" & Session("sCoaCash") & "' AND DAY(b.TransDate)=DAY(x.TransDate) and MONTH(b.TransDate)='" & forMonth & "' and year(b.TransDate)='" & forYear & "' and b.status<>'D' and a.isActive='Y') as balance "
        sSQL += "from iPxAcctGL_JVhdr as x "
        sSQL += "INNER JOIN iPxAcctGL_JVdtl as b ON b.businessid=x.businessid and b.TransID=x.TransID  "
        sSQL += "where x.businessid ='" & Session("sBusinessID") & "' and b.Coa='" & Session("sCoaCash") & "' and MONTH(x.TransDate)='" & forMonth & "' and year(x.TransDate)='" & forYear & "' and x.status<>'D' and b.isActive='Y' "
        sSQL += "GROUP BY x.TransDate order by x.TransDate asc"
        'sSQL = "select b.TransID, DAY(b.TransDate) as day, (b.Description) AS GlGrp, a.Description, a.Reff, a.Debit, a.Credit from iPxAcctGL_JVdtl as a "
        'sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON b.businessid=a.businessid and b.TransID=a.TransID "
        'sSQL += "INNER JOIN iPxAcctGL_JVGrp as c ON c.id=b.JVgroup "
        'sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.Coa='" & Session("sCoaCash") & "' "
        'sSQL += "and MONTH(b.TransDate)='" & forMonth & "' and year(b.TransDate)='" & forYear & "' "
        'sSQL += "order by b.TransDate asc"
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
                    Dim totalBal As Decimal = dt.Compute("Sum(balance)", "").ToString()
                    gvDetailCash.FooterRow.Cells(0).Text = "Total"
                    gvDetailCash.FooterRow.Cells(0).HorizontalAlign = HorizontalAlign.Right
                    gvDetailCash.FooterRow.Cells(1).Text = totalDeb.ToString("N2")
                    gvDetailCash.FooterRow.Cells(1).HorizontalAlign = HorizontalAlign.Right
                    gvDetailCash.FooterRow.Cells(2).Text = totalCre.ToString("N2")
                    gvDetailCash.FooterRow.Cells(2).HorizontalAlign = HorizontalAlign.Right
                    gvDetailCash.FooterRow.Cells(3).Text = totalBal.ToString("N2")
                    gvDetailCash.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Right
                    gvDetailCash.FooterRow.Cells(3).CssClass = "cellOneCellPaddingRight"
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
    Sub ListCashDetailGL()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select b.TransID, DAY(b.TransDate) as day, (b.Description) AS GlGrp, a.Description, a.Reff, a.Debit, a.Credit from iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as b ON b.businessid=a.businessid and b.TransID=a.TransID "
        sSQL += "INNER JOIN iPxAcctGL_JVGrp as c ON c.id=b.JVgroup "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.Coa='" & Session("sCoaCash") & "' and DAY(b.TransDate)='" & Session("sDay") & "' "
        sSQL += "and MONTH(b.TransDate)='" & forMonth & "' and year(b.TransDate)='" & forYear & "' and b.Status <>'D' and a.isActive='Y' "
        sSQL += "order by b.TransID asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDetailGl.DataSource = dt
                    gvDetailGl.DataBind()
                    'Calculate Sum and display in Footer Row
                    Dim totalDeb As Decimal = dt.Compute("Sum(Debit)", "").ToString()
                    Dim totalCre As Decimal = dt.Compute("Sum(Credit)", "").ToString()
                    gvDetailGl.FooterRow.Cells(3).Text = "Total"
                    gvDetailGl.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Right
                    gvDetailGl.FooterRow.Cells(4).Text = totalDeb.ToString("N2")
                    gvDetailGl.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right
                    gvDetailGl.FooterRow.Cells(5).Text = totalCre.ToString("N2")
                    gvDetailGl.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                    gvDetailGl.Enabled = True
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvDetailGl.DataSource = dt
                    gvDetailGl.DataBind()
                    gvDetailGl.Enabled = False
                    gvDetailGl.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Public Function listDetailCOALvl2a(ByVal businessid As String, ByVal TransID As String) As Boolean
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
                    gvDetailJournalTrans.DataSource = dt
                    gvDetailJournalTrans.DataBind()
                    Dim totalDeb As Decimal = dt.Compute("Sum(debit)", "").ToString()
                    Dim totalCre As Decimal = dt.Compute("Sum(Credit)", "").ToString()
                    Dim totalBal As Decimal = dt.Compute("Sum(balance)", "").ToString()
                    gvDetailJournalTrans.FooterRow.Cells(4).Text = "Total"
                    gvDetailJournalTrans.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Left
                    gvDetailJournalTrans.FooterRow.Cells(5).Text = totalDeb.ToString("N2")
                    gvDetailJournalTrans.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                    gvDetailJournalTrans.FooterRow.Cells(6).Text = totalCre.ToString("N2")
                    gvDetailJournalTrans.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Right
                    gvDetailJournalTrans.FooterRow.Cells(7).Text = totalBal.ToString("N2")
                    gvDetailJournalTrans.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                    gvDetailJournalTrans.Enabled = True
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvDetailJournalTrans.DataSource = dt
                    gvDetailJournalTrans.DataBind()
                    gvDetailJournalTrans.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Session("sCoaCash") = ""
            tbDate.Text = Format(Now, "MMMM yyyy")
            Dim dateBirthday As Date = tbDate.Text
            forMonth = dateBirthday.ToString("MM")
            forYear = dateBirthday.ToString("yyy")
            listSummary()
            ListCashDetail()
        End If
        tbDate.Enabled = False
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthGL", "$(document).ready(function() {MonthGL()});", True)
    End Sub
    Protected Sub cari(ByVal sender As Object, ByVal e As EventArgs)
        Session("sCoaCash") = ""
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        listSummary()
        'ListCashDetail()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthGL", "$(document).ready(function() {MonthGL()});", True)
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvCashSummary.PageIndex = e.NewPageIndex
        Me.listSummary()
        gvDetailCash.PageIndex = e.NewPageIndex
        Me.ListCashDetail()
    End Sub

    Protected Sub gvCashSummary_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCashSummary.PageIndexChanging
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        gvCashSummary.PageIndex = e.NewPageIndex
        Me.listSummary()
    End Sub

    Protected Sub gvDetailCash_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDetailCash.PageIndexChanging
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        gvDetailCash.PageIndex = e.NewPageIndex
        Me.ListCashDetail()
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvCashSummary.PageIndex = e.NewPageIndex
        Me.listSummary()
        gvDetailCash.PageIndex = e.NewPageIndex
        Me.ListCashDetail()
    End Sub

    Protected Sub gvCashSummary_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCashSummary.RowCommand
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        If e.CommandName = "getDetail" Then
            Session("sCoaCash") = e.CommandArgument
            ListCashDetail()
        End If
    End Sub

    Protected Sub gvDetailCash_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDetailCash.RowCommand
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        If e.CommandName = "getDetail" Then
            Session("sDay") = e.CommandArgument
            Dim a As String = Session("sDay")
            lbDateDetail.Text = Session("sDay") + " " + tbDate.Text
            ListCashDetailGL()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        End If
    End Sub

    Protected Sub lbAbortDetailLv2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDetailLv2.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCreatType", "hideModalCreatType()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
    End Sub

    Protected Sub gvDetailGl_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDetailGl.RowCommand
        If e.CommandName = "getDetail" Then
            Label4.Text = "Detail Journal " + e.CommandArgument
            listDetailCOALvl2a(Session("sBusinessID"), e.CommandArgument)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCreatType", "showModalCreatType()", True)
        End If
    End Sub
End Class
