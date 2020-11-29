Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxGLTrialBalance
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, forMonth, forYear, lastMonth, lastYear, COAid, a As String
    Dim cIpx As New iPxClass
    Sub ListCOA()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.businessid, a.Coa, a.type, a.description, "
        sSQL += "(select SUM(i.Debit-i.Credit) from iPxAcctGL_JVdtl as i "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as j on j.businessid = i.businessid and j.TransID=i.TransID "
        sSQL += "where i.businessid=a.businessid and i.coa=a.Coa and MONTH(j.TransDate)<='" & lastMonth & "' and YEAR(j.TransDate)<='" & lastYear & "' and a.type='Asset' and i.isActive='Y' and j.Status<>'D') as PrevDebit, "
        sSQL += "(select sum(i.Credit-i.Debit) from iPxAcctGL_JVdtl as i "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as j on j.businessid = i.businessid and j.TransID=i.TransID "
        sSQL += "where i.businessid=a.businessid and i.coa=a.Coa and MONTH(j.TransDate)<='" & lastMonth & "' and YEAR(j.TransDate)<='" & lastYear & "' and (a.type='Equity' or a.type='Liability') and i.isActive='Y' and j.Status<>'D') as PrevCredit, "
        sSQL += "(select sum(i.Debit) from iPxAcctGL_JVdtl as i "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as j on j.businessid = i.businessid and j.TransID=i.TransID "
        sSQL += "where i.businessid=a.businessid and i.coa=a.Coa and MONTH(j.TransDate)='" & forMonth & "' and YEAR(j.TransDate)='" & forYear & "' and i.isActive='Y' and j.Status<>'D') as CurDebit, "
        sSQL += "(select sum(i.Credit) from iPxAcctGL_JVdtl as i "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as j on j.businessid = i.businessid and j.TransID=i.TransID "
        sSQL += "where i.businessid=a.businessid and i.coa=a.Coa and MONTH(j.TransDate)='" & forMonth & "' and YEAR(j.TransDate)='" & forYear & "' and i.isActive='Y' and j.Status<>'D') as CurCredit, "
        sSQL += "(select SUM(i.Debit-i.Credit) from iPxAcctGL_JVdtl as i "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as j on j.businessid = i.businessid and j.TransID=i.TransID "
        sSQL += "where i.businessid=a.businessid and i.coa=a.Coa and MONTH(j.TransDate)<='" & forMonth & "' and YEAR(j.TransDate)<='" & forYear & "' and a.type='Asset' and i.isActive='Y' and j.Status<>'D') as NetDebit, "
        sSQL += "(select sum(i.Credit-i.Debit) from iPxAcctGL_JVdtl as i "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as j on j.businessid = i.businessid and j.TransID=i.TransID "
        sSQL += "where i.businessid=a.businessid and i.coa=a.Coa and MONTH(j.TransDate)<='" & forMonth & "' and YEAR(j.TransDate)<='" & forYear & "' and (a.type='Equity' or a.type='Liability') and i.isActive='Y' and j.Status<>'D') as NetCredit "
        sSQL += "FROM iPxAcct_Coa AS a "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.isactive = 'Y' "
        If Session("sGrpType") = "" Then
            sSQL += "and (a.type='Asset' or a.type='Equity' or a.type='Liability')"
        Else
            If Session("sGrpType") = "Asset" Or Session("sGrpType") = "Equity" Or Session("sGrpType") = "Liability" Then
                sSQL += "and (a.type='" & Session("sGrpType") & "' )"
            Else
                sSQL += "and (a.type='' )"
            End If
        End If
        If CheckBox1.Checked = True Then
            sSQL += " and a.Coa in (select coa from iPxAcctGL_JVdtl as a inner join iPxAcctGL_JVhdr as b ON b.businessid=a.businessid and b.TransID=a.TransID where a.businessid='" & Session("sBusinessID") & "' and MONTH(b.TransDate)<='" & forMonth & "' and YEAR(b.TransDate)<='" & forYear & "' and a.isActive='Y' and b.Status<>'D' group by coa) "
        End If
        sSQL += "UNION ALL "
        sSQL += "SELECT a.businessid, a.Coa, a.type, a.description, "
        sSQL += "('0') as PrevDebit, ('0') as PrevCredit, "
        sSQL += "(select sum(i.Debit) from iPxAcctGL_JVdtl as i "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as j on j.businessid = i.businessid and j.TransID=i.TransID "
        sSQL += "where i.businessid=a.businessid and i.coa=a.Coa and MONTH(j.TransDate)='" & forMonth & "' and YEAR(j.TransDate)='" & forYear & "' and i.isActive='Y' and j.Status<>'D') as CurDebit, "
        sSQL += "(select sum(i.Credit) from iPxAcctGL_JVdtl as i "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as j on j.businessid = i.businessid and j.TransID=i.TransID "
        sSQL += "where i.businessid=a.businessid and i.coa=a.Coa and MONTH(j.TransDate)='" & forMonth & "' and YEAR(j.TransDate)='" & forYear & "' and i.isActive='Y' and j.Status<>'D') as CurCredit, "
        sSQL += "(select SUM(i.Debit-i.Credit) from iPxAcctGL_JVdtl as i "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as j on j.businessid = i.businessid and j.TransID=i.TransID "
        sSQL += "where i.businessid=a.businessid and i.coa=a.Coa and MONTH(j.TransDate)='" & forMonth & "' and YEAR(j.TransDate)='" & forYear & "' and (a.type='Statistic' or a.type='Expenses' or a.type='Cost') and i.isActive='Y' and j.Status<>'D') as NetDebit, "
        sSQL += "(select sum(i.Credit-i.Debit) from iPxAcctGL_JVdtl as i "
        sSQL += "INNER JOIN iPxAcctGL_JVhdr as j on j.businessid = i.businessid and j.TransID=i.TransID "
        sSQL += "where i.businessid=a.businessid and i.coa=a.Coa and MONTH(j.TransDate)='" & forMonth & "' and YEAR(j.TransDate)='" & forYear & "' and (a.type='Revenue') and i.isActive='Y' and j.Status<>'D') as NetCredit "
        sSQL += "FROM iPxAcct_Coa AS a "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.isactive = 'Y' "
        If Session("sGrpType") = "" Then
            sSQL += "and (a.type='Statistic' or a.type='Expenses' or a.type='Cost' or a.type='Revenue')"
        Else
            If Session("sGrpType") = "Statistic" Or Session("sGrpType") = "Expenses" Or Session("sGrpType") = "Cost" Or Session("sGrpType") = "Revenue" Then
                sSQL += "and (a.type='" & Session("sGrpType") & "' )"
            Else
                sSQL += "and (a.type='' )"
            End If
        End If
        If CheckBox1.Checked = True Then
            sSQL += " and a.Coa in (select coa from iPxAcctGL_JVdtl as a inner join iPxAcctGL_JVhdr as b ON b.businessid=a.businessid and b.TransID=a.TransID where a.businessid='" & Session("sBusinessID") & "' and MONTH(b.TransDate)<='" & forMonth & "' and YEAR(b.TransDate)<='" & forYear & "' and a.isActive='Y' and b.Status<>'D' group by coa) "
        End If
        sSQL += " order by a.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvTrialBalance.DataSource = dt
                    gvTrialBalance.DataBind()
                    Dim PrevDebit, PrevCredit, CurDebit, CurCredit, NetDeb, NetCre As Decimal
                    'Dim typeCoa As String = dt.Columns("type").ToString
                    If dt.Compute("Sum(PrevDebit)", "").ToString() <> "" Then
                        PrevDebit = dt.Compute("Sum(PrevDebit)", "").ToString()
                    Else
                        PrevDebit = 0
                    End If
                    If dt.Compute("Sum(PrevCredit)", "").ToString() <> "" Then
                        PrevCredit = dt.Compute("Sum(PrevCredit)", "").ToString()
                    Else
                        PrevCredit = 0
                    End If
                    If dt.Compute("Sum(CurDebit)", "").ToString() <> "" Then
                        CurDebit = dt.Compute("Sum(CurDebit)", "").ToString()
                    Else
                        CurDebit = 0
                    End If
                    If dt.Compute("Sum(CurCredit)", "").ToString() <> "" Then
                        CurCredit = dt.Compute("Sum(CurCredit)", "").ToString()
                    Else
                        CurCredit = 0
                    End If
                    If dt.Compute("Sum(NetDebit)", "").ToString() <> "" Then
                        NetDeb = dt.Compute("Sum(NetDebit)", "").ToString()
                    Else
                        NetDeb = 0
                    End If
                    If dt.Compute("Sum(NetCredit)", "").ToString() <> "" Then
                        NetCre = dt.Compute("Sum(NetCredit)", "").ToString()
                    Else
                        NetCre = 0
                    End If

                    gvTrialBalance.FooterRow.Cells(1).Text = "Total"
                    gvTrialBalance.FooterRow.Cells(1).HorizontalAlign = HorizontalAlign.Right
                    gvTrialBalance.FooterRow.Cells(3).Text = PrevDebit.ToString("N2")
                    gvTrialBalance.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Right
                    gvTrialBalance.FooterRow.Cells(4).Text = PrevCredit.ToString("N2")
                    gvTrialBalance.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right
                    gvTrialBalance.FooterRow.Cells(5).Text = CurDebit.ToString("N2")
                    gvTrialBalance.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                    gvTrialBalance.FooterRow.Cells(6).Text = CurCredit.ToString("N2")
                    gvTrialBalance.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Right
                    gvTrialBalance.FooterRow.Cells(7).Text = NetDeb.ToString("N2")
                    gvTrialBalance.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                    gvTrialBalance.FooterRow.Cells(8).Text = NetCre.ToString("N2")
                    gvTrialBalance.FooterRow.Cells(8).HorizontalAlign = HorizontalAlign.Right
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvTrialBalance.DataSource = dt
                    gvTrialBalance.DataBind()
                    gvTrialBalance.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListDetailJournal()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid,'GLOpen' as TransID,'' as TransDate,'' as Status,'' as JVgroup,'' as ReffNo,'' as Description, ('Open') as GroupGL, "
        sSQL += "('0') as PrevDeb, "
        sSQL += "('0') as PrevCre, "
        sSQL += "(select sum(i.Debit-i.Credit) from iPxAcctGL_JVdtl as i INNER JOIN iPxAcctGL_JVhdr as j ON j.businessid=i.businessid and j.TransID=i.TransID INNER JOIN iPxAcct_Coa as k ON k.businessid=i.businessid and k.Coa=i.Coa where i.businessid=a.businessid and i.Coa=b.Coa and MONTH(j.TransDate)<='" & lastMonth & "' and YEAR(j.TransDate)<='" & lastYear & "' and k.type='Asset') as amountDeb, "
        sSQL += "(select sum(i.Credit-i.Debit) from iPxAcctGL_JVdtl as i INNER JOIN iPxAcctGL_JVhdr as j ON j.businessid=i.businessid and j.TransID=i.TransID INNER JOIN iPxAcct_Coa as k ON k.businessid=i.businessid and k.Coa=i.Coa where i.businessid=a.businessid and i.Coa=b.Coa and MONTH(j.TransDate)<='" & lastMonth & "' and YEAR(j.TransDate)<='" & lastYear & "' and (k.type='Equity' or k.type='Liability')) as amountCre "
        sSQL += "from iPxAcctGL_JVhdr as a "
        sSQL += "INNER JOIN iPxAcctGL_JVdtl as b ON b.businessid=a.businessid and b.TransID=a.TransID "
        sSQL += "INNER JOIN iPxAcctGL_JVGrp as c ON c.id=a.JVgroup "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.Status<>'D' and b.Coa='" & Session("sCoaDtl") & "' and MONTH(a.TransDate)<'" & forMonth & "' and YEAR(a.TransDate)<='" & forYear & "' "
        sSQL += "group by a.businessid,b.Coa "
        sSQL += "UNION ALL "
        sSQL += "select a.*, (c.Description) as GroupGL, "
        sSQL += "('0') as PrevDeb, "
        sSQL += "('0') as PrevCre, "
        sSQL += "(select sum(i.Debit) from iPxAcctGL_JVdtl as i INNER JOIN iPxAcctGL_JVhdr as j ON j.businessid=i.businessid and j.TransID=i.TransID where i.businessid=a.businessid and i.Coa=b.Coa and i.TransID=a.TransID and MONTH(j.TransDate)='" & forMonth & "' and YEAR(j.TransDate)='" & forYear & "') as amountDeb, "
        sSQL += "(select sum(i.Credit) from iPxAcctGL_JVdtl as i INNER JOIN iPxAcctGL_JVhdr as j ON j.businessid=i.businessid and j.TransID=i.TransID where i.businessid=a.businessid and i.Coa=b.Coa and i.TransID=a.TransID and MONTH(j.TransDate)='" & forMonth & "' and YEAR(j.TransDate)='" & forYear & "') as amountCre "
        sSQL += "from iPxAcctGL_JVhdr as a "
        sSQL += "INNER JOIN iPxAcctGL_JVdtl as b ON b.businessid=a.businessid and b.TransID=a.TransID "
        sSQL += "INNER JOIN iPxAcctGL_JVGrp as c ON c.id=a.JVgroup "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.Status<>'D' and b.Coa='" & Session("sCoaDtl") & "' and MONTH(a.TransDate)='" & forMonth & "' and YEAR(a.TransDate)='" & forYear & "' "
        'sSQL += "order by a.TransDate"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDetail.DataSource = dt
                    gvDetail.DataBind()
                    Dim CurDebit, CurCredit As Decimal
                    If dt.Compute("Sum(amountDeb)", "").ToString() <> "" Then
                        CurDebit = dt.Compute("Sum(amountDeb)", "").ToString()
                    Else
                        CurDebit = 0
                    End If
                    If dt.Compute("Sum(amountCre)", "").ToString() <> "" Then
                        CurCredit = dt.Compute("Sum(amountCre)", "").ToString()
                    Else
                        CurCredit = 0
                    End If

                    gvDetail.FooterRow.Cells(2).Text = "Total"
                    gvDetail.FooterRow.Cells(2).HorizontalAlign = HorizontalAlign.Right
                    gvDetail.FooterRow.Cells(3).Text = CurDebit.ToString("N2")
                    gvDetail.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Right
                    gvDetail.FooterRow.Cells(4).Text = CurCredit.ToString("N2")
                    gvDetail.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right
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
    Sub CoaDescription()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        sSQL = "Select * From iPxAcct_Coa Where businessid = '" & Session("sBusinessID") & "' AND Coa='" & COAid & "' "
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            lbTitleDetailCoa.Text = "Detail Journal COA (" + COAid + " " + oSQLReader.Item("description").ToString.Trim + ") " + tbDate.Text
        End If
        oCnct.Close()
    End Sub
    Sub GrpType()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select type from iPxAcct_Coa "
        sSQL += "WHERE businessid='" & Session("sBusinessID") & "' group by type order by type asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlGrp.DataSource = dt
                dlGrp.DataTextField = "type"
                dlGrp.DataValueField = "type"
                dlGrp.DataBind()
                dlGrp.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub editGLHeader()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*,(select 'Close' from iPxAcctGL_JVhdr as x where x.businessid ='" & Session("sBusinessID") & "' AND x.Status <> 'D' AND x.ReffNo like'PL%' and YEAR(x.TransDate)=YEAR(a.TransDate) AND MONTH(x.TransDate)=MONTH(a.TransDate)) as CloseStatus FROM iPxAcctGL_JVhdr as a "
        sSQL += "WHERE a.businessid ='" & Session("sBusinessID") & "' and a.TransID ='" & tbTransID.Text & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Dim InvDate As Date = oSQLReader.Item("TransDate").ToString
            TextBox1.Text = InvDate.ToString("dd/MM/yyyy")
            tbReff.Text = oSQLReader.Item("ReffNo").ToString
            tbDesc.Text = oSQLReader.Item("Description").ToString
            tbGroup.Text = oSQLReader.Item("JVgroup").ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub ListGLDetail()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*,b.*,(c.description)as CoaDesc, "
        sSQL += "(select 'Close' from iPxAcctGL_JVhdr as x where x.businessid ='" & Session("sBusinessID") & "' AND x.Status <> 'D' AND x.ReffNo like'PL%' and YEAR(x.TransDate)=YEAR(b.TransDate) AND MONTH(x.TransDate)=MONTH(b.TransDate)) as CloseStatus, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='24' and x.active='Y') as editGL, "
        sSQL += "(select COUNT(id) from iPxAcct_Log where businessid=a.businessid and funct='Posting' and (transID='" & tbTransID.Text & "'or transID='" & tbReff.Text & "')) as autoPost "
        sSQL += "from iPxAcctGL_JVdtl as a INNER JOIN iPxAcctGL_JVhdr as b ON b.TransID=a.TransID "
        sSQL += "LEFT JOIN iPxAcct_Coa as c ON c.Coa=a.Coa "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.TransID='" & tbTransID.Text & "' "
        sSQL += " and a.isactive = 'Y' order by a.RecID asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvGLDetail.DataSource = dt
                    gvGLDetail.DataBind()
                    'Calculate Sum and display in Footer Row
                    Dim totalDeb As Decimal = dt.Compute("Sum(Debit)", "").ToString()
                    Dim totalCre As Decimal = dt.Compute("Sum(Credit)", "").ToString()
                    gvGLDetail.FooterRow.Cells(3).Text = "Total"
                    gvGLDetail.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Left
                    gvGLDetail.FooterRow.Cells(4).Text = totalDeb.ToString("N2")
                    gvGLDetail.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right
                    gvGLDetail.FooterRow.Cells(5).Text = totalCre.ToString("N2")
                    gvGLDetail.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                    gvGLDetail.Enabled = True
                    If totalCre = totalDeb Then
                        Session("sBalanceTot") = True
                    Else
                        Session("sBalanceTot") = False
                    End If
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvGLDetail.DataSource = dt
                    gvGLDetail.DataBind()
                    gvGLDetail.Enabled = False
                    gvGLDetail.Rows(0).Visible = False
                    Session("sBalanceTot") = True
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
            CheckBox1.Checked = True
            Session("sGrpType") = ""
            tbDate.Enabled = False
            tbDate.Text = Format(Now, "MMMM yyy")
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
            'GrpType()
            ListCOA()
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
            ListCOA()
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthGL", "$(document).ready(function() {MonthGL()});", True)
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvTrialBalance.PageIndex = e.NewPageIndex
        Me.ListCOA()
        gvDetail.PageIndex = e.NewPageIndex
        Me.ListDetailJournal()
    End Sub

    Protected Sub gvTrialBalance_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrialBalance.PageIndexChanging
        gvTrialBalance.PageIndex = e.NewPageIndex
        Me.ListCOA()
    End Sub

    Protected Sub gvDetail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDetail.PageIndexChanging
        gvDetail.PageIndex = e.NewPageIndex
        CoaDescription()
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
        Me.ListDetailJournal()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
    End Sub
    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvTrialBalance.PageIndex = e.NewPageIndex
        Me.ListCOA()
        gvDetail.PageIndex = e.NewPageIndex
        Me.ListDetailJournal()
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
        ListCOA()
    End Sub
    'Protected Sub OnRowDataBoundTrial(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        Dim typeCoa As String = e.Row.Cells(2).Text
    '        Dim PrevDeb As String = e.Row.Cells(3).Text
    '        Dim PrevCre As String = e.Row.Cells(4).Text
    '        Dim CurDeb As String = e.Row.Cells(5).Text
    '        Dim CurCre As String = e.Row.Cells(6).Text
    '        Dim NetDeb, NetCre As Integer
    '        If typeCoa = "Asset" Then
    '            NetDeb = ((Val(PrevDeb) + Val(CurDeb)) - Val(CurCre))
    '            NetCre = 0
    '        ElseIf typeCoa = "Equity" Or typeCoa = "Liability" Then
    '            NetDeb = 0
    '            NetCre = ((Val(PrevCre) + Val(CurCre)) - Val(CurDeb))
    '        ElseIf typeCoa = "Cost" Or typeCoa = "Expenses" Or typeCoa = "Statistic" Then
    '            NetDeb = (Val(CurDeb) - Val(CurCre))
    '            NetCre = 0
    '        ElseIf typeCoa = "Revenue" Then
    '            NetDeb = 0
    '            NetCre = (Val(CurCre) - Val(CurDeb))
    '        End If
    '        'NetDeb = (Val(PrevDeb) + Val(CurDeb))
    '        'NetCre = (Val(PrevCre) + Val(CurCre))
    '        e.Row.Cells(11).Text = String.Format("{0:N2}", (NetDeb)).ToString
    '        e.Row.Cells(12).Text = String.Format("{0:N2}", (NetCre)).ToString


    '    End If
    'End Sub

    Protected Sub gvTrialBalance_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrialBalance.RowCommand
        If e.CommandName = "getDetail" Then
            COAid = e.CommandArgument
            Session("sCoaDtl") = COAid
            CoaDescription()
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
            ListDetailJournal()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        End If
    End Sub

    Protected Sub lbAbortDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDetail.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
    End Sub

    Protected Sub lbPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPrint.Click
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        Session("sReport") = "GLTrialBalance"
        Session("sMapPath") = "~/iPxReportFile/dckGL_TrialBalance.rpt"
        Session("sPeriod") = forMonth + "-" + forYear
        Response.Redirect("rptviewer.aspx")
    End Sub

    Protected Sub lbPrintDtl_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPrintDtl.Click
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        Session("sReport") = "GLTrialBalanceDtl"
        Session("sMapPath") = "~/iPxReportFile/dckGL_SubsidiaryAccount.rpt"
        Session("sPeriod") = forMonth + "-" + forYear
        Response.Redirect("rptviewer.aspx")
    End Sub

    Protected Sub dlGrp_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlGrp.SelectedIndexChanged
        If dlGrp.Text.ToString = "" Then
            Session("sGrpType") = ""
        Else
            Session("sGrpType") = dlGrp.SelectedValue
        End If
        ListCOA()
    End Sub

    Protected Sub gvDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDetail.RowCommand
        If e.CommandName = "getDetail" Then
            tbTransID.Text = e.CommandArgument
            editGLHeader()
            ListGLDetail()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
        End If
    End Sub

    Protected Sub lbAbortJournalTrans_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles lbAbortJournalTrans.Command
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddTrans", "hideModalAddTrans()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
    End Sub

    Protected Sub CheckBox1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        ListCOA()
    End Sub
End Class
