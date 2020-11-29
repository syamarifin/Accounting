Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxGLTransaction
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, IDTrans, status, year, month As String
    Dim cIpx As New iPxClass
    Sub UserAcces()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='23'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as AddGL "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("AddGL").ToString = "Y" Then
                lbAddGL.Enabled = True
            Else
                lbAddGL.Enabled = False
            End If
        Else
            lbAddGL.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub kosongQuery()
        tbQIdGL.Text = ""
        tbQDate.Text = ""
        tbQFrom.Text = ""
        tbQReff.Text = ""
        tbQUntil.Text = ""
    End Sub
    Sub ListGLALL()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        month = Left(tbDateWork.Text, 2)
        year = Right(tbDateWork.Text, 4)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*, (b.Description) AS GlGrp,(select sum(Credit) from iPxAcctGL_JVdtl where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totCredit, "
        sSQL += "(select sum(Debit) from iPxAcctGL_JVdtl where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totDebit, "
        sSQL += "(select 'Close' from iPxAcctGL_JVhdr as x where x.businessid ='" & Session("sBusinessID") & "' AND x.Status <> 'D' AND x.ReffNo like'PL%' and YEAR(x.TransDate)=YEAR(a.TransDate) AND MONTH(x.TransDate)=MONTH(a.TransDate)) as CloseStatus, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='10037' and x.active='Y') as verifedGL, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='25' and x.active='Y') as deleteGL "
        sSQL += "from iPxAcctGL_JVhdr AS a INNER JOIN iPxAcctGL_JVGrp AS b ON b.id=a.JVgroup "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " AND month(a.TransDate)='" & month & "' AND year(a.TransDate)='" & year & "' AND a.Status <> 'D' "
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.TransDate asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvALL.DataSource = dt
                    gvALL.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvALL.DataSource = dt
                    gvALL.DataBind()
                    gvALL.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListGLSales()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        month = Left(tbDateWork.Text, 2)
        year = Right(tbDateWork.Text, 4)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*, (b.Description) AS GlGrp,(select sum(Credit) from iPxAcctGL_JVdtl where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totCredit, "
        sSQL += "(select sum(Debit) from iPxAcctGL_JVdtl where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totDebit, "
        sSQL += "(select 'Close' from iPxAcctGL_JVhdr as x where x.businessid ='" & Session("sBusinessID") & "' AND x.Status <> 'D' AND x.ReffNo like'PL%' and YEAR(x.TransDate)=YEAR(a.TransDate) AND MONTH(x.TransDate)=MONTH(a.TransDate)) as CloseStatus, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='10037' and x.active='Y') as verifedGL, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='25' and x.active='Y') as deleteGL "
        sSQL += "from iPxAcctGL_JVhdr AS a INNER JOIN iPxAcctGL_JVGrp AS b ON b.id=a.JVgroup "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and b.id='G1' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " AND month(a.TransDate)='" & month & "' AND year(a.TransDate)='" & year & "' AND a.Status <> 'D' "
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.TransDate asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvSales.DataSource = dt
                    gvSales.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvSales.DataSource = dt
                    gvSales.DataBind()
                    gvSales.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListGLAR()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        month = Left(tbDateWork.Text, 2)
        year = Right(tbDateWork.Text, 4)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*, (b.Description) AS GlGrp,(select sum(Credit) from iPxAcctGL_JVdtl where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totCredit, "
        sSQL += "(select sum(Debit) from iPxAcctGL_JVdtl where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totDebit, "
        sSQL += "(select 'Close' from iPxAcctGL_JVhdr as x where x.businessid ='" & Session("sBusinessID") & "' AND x.Status <> 'D' AND x.ReffNo like'PL%' and YEAR(x.TransDate)=YEAR(a.TransDate) AND MONTH(x.TransDate)=MONTH(a.TransDate)) as CloseStatus, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='10037' and x.active='Y') as verifedGL, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='25' and x.active='Y') as deleteGL "
        sSQL += "from iPxAcctGL_JVhdr AS a INNER JOIN iPxAcctGL_JVGrp AS b ON b.id=a.JVgroup "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and b.id='G2' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " AND month(a.TransDate)='" & month & "' AND year(a.TransDate)='" & year & "' AND a.Status <> 'D' "
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.TransDate asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvAR.DataSource = dt
                    gvAR.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvAR.DataSource = dt
                    gvAR.DataBind()
                    gvAR.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListGL()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        month = Left(tbDateWork.Text, 2)
        year = Right(tbDateWork.Text, 4)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*, (b.Description) AS GlGrp,(select sum(Credit) from iPxAcctGL_JVdtl where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totCredit, "
        sSQL += "(select sum(Debit) from iPxAcctGL_JVdtl where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totDebit, "
        sSQL += "(select 'Close' from iPxAcctGL_JVhdr as x where x.businessid ='" & Session("sBusinessID") & "' AND x.Status <> 'D' AND x.ReffNo like'PL%' and YEAR(x.TransDate)=YEAR(a.TransDate) AND MONTH(x.TransDate)=MONTH(a.TransDate)) as CloseStatus, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='10037' and x.active='Y') as verifedGL, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='25' and x.active='Y') as deleteGL "
        sSQL += "from iPxAcctGL_JVhdr AS a INNER JOIN iPxAcctGL_JVGrp AS b ON b.id=a.JVgroup "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and b.id='G3' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " AND month(a.TransDate)='" & month & "' AND year(a.TransDate)='" & year & "' AND a.Status <> 'D' "
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.TransDate asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvGL.DataSource = dt
                    gvGL.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvGL.DataSource = dt
                    gvGL.DataBind()
                    gvGL.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListGLAP()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        month = Left(tbDateWork.Text, 2)
        year = Right(tbDateWork.Text, 4)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*, (b.Description) AS GlGrp,(select sum(Credit) from iPxAcctGL_JVdtl where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totCredit, "
        sSQL += "(select sum(Debit) from iPxAcctGL_JVdtl where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totDebit, "
        sSQL += "(select 'Close' from iPxAcctGL_JVhdr as x where x.businessid ='" & Session("sBusinessID") & "' AND x.Status <> 'D' AND x.ReffNo like'PL%' and YEAR(x.TransDate)=YEAR(a.TransDate) AND MONTH(x.TransDate)=MONTH(a.TransDate)) as CloseStatus, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='10037' and x.active='Y') as verifedGL, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='25' and x.active='Y') as deleteGL "
        sSQL += "from iPxAcctGL_JVhdr AS a INNER JOIN iPxAcctGL_JVGrp AS b ON b.id=a.JVgroup "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and b.id='G4' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " AND month(a.TransDate)='" & month & "' AND year(a.TransDate)='" & year & "' AND a.Status <> 'D' "
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.TransDate asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvAP.DataSource = dt
                    gvAP.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvAP.DataSource = dt
                    gvAP.DataBind()
                    gvAP.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListGLCost()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        month = Left(tbDateWork.Text, 2)
        year = Right(tbDateWork.Text, 4)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*, (b.Description) AS GlGrp,(select sum(Credit) from iPxAcctGL_JVdtl where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totCredit, "
        sSQL += "(select sum(Debit) from iPxAcctGL_JVdtl where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totDebit, "
        sSQL += "(select 'Close' from iPxAcctGL_JVhdr as x where x.businessid ='" & Session("sBusinessID") & "' AND x.Status <> 'D' AND x.ReffNo like'PL%' and YEAR(x.TransDate)=YEAR(a.TransDate) AND MONTH(x.TransDate)=MONTH(a.TransDate)) as CloseStatus, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='10037' and x.active='Y') as verifedGL, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='25' and x.active='Y') as deleteGL "
        sSQL += "from iPxAcctGL_JVhdr AS a INNER JOIN iPxAcctGL_JVGrp AS b ON b.id=a.JVgroup "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and b.id='G5' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " AND month(a.TransDate)='" & month & "' AND year(a.TransDate)='" & year & "' AND a.Status <> 'D' "
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.TransDate asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCost.DataSource = dt
                    gvCost.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCost.DataSource = dt
                    gvCost.DataBind()
                    gvCost.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListGLReceiving()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        month = Left(tbDateWork.Text, 2)
        year = Right(tbDateWork.Text, 4)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*, (b.Description) AS GlGrp,(select sum(Credit) from iPxAcctGL_JVdtl where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totCredit, "
        sSQL += "(select sum(Debit) from iPxAcctGL_JVdtl where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totDebit, "
        sSQL += "(select 'Close' from iPxAcctGL_JVhdr as x where x.businessid ='" & Session("sBusinessID") & "' AND x.Status <> 'D' AND x.ReffNo like'PL%' and YEAR(x.TransDate)=YEAR(a.TransDate) AND MONTH(x.TransDate)=MONTH(a.TransDate)) as CloseStatus, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='10037' and x.active='Y') as verifedGL, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='25' and x.active='Y') as deleteGL "
        sSQL += "from iPxAcctGL_JVhdr AS a INNER JOIN iPxAcctGL_JVGrp AS b ON b.id=a.JVgroup "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and b.id='G6' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " AND month(a.TransDate)='" & month & "' AND year(a.TransDate)='" & year & "' AND a.Status <> 'D' "
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.TransDate asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvReceiving.DataSource = dt
                    gvReceiving.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvReceiving.DataSource = dt
                    gvReceiving.DataBind()
                    gvReceiving.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListGLAdjustment()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        month = Left(tbDateWork.Text, 2)
        year = Right(tbDateWork.Text, 4)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*, (b.Description) AS GlGrp,(select sum(Credit) from iPxAcctGL_JVdtl where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totCredit, "
        sSQL += "(select sum(Debit) from iPxAcctGL_JVdtl where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totDebit, "
        sSQL += "(select 'Close' from iPxAcctGL_JVhdr as x where x.businessid ='" & Session("sBusinessID") & "' AND x.Status <> 'D' AND x.ReffNo like'PL%' and YEAR(x.TransDate)=YEAR(a.TransDate) AND MONTH(x.TransDate)=MONTH(a.TransDate)) as CloseStatus, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='10037' and x.active='Y') as verifedGL, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='25' and x.active='Y') as deleteGL "
        sSQL += "from iPxAcctGL_JVhdr AS a INNER JOIN iPxAcctGL_JVGrp AS b ON b.id=a.JVgroup "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and b.id='G7' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " AND month(a.TransDate)='" & month & "' AND year(a.TransDate)='" & year & "' AND a.Status <> 'D' "
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.TransDate asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvAdjustment.DataSource = dt
                    gvAdjustment.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvAdjustment.DataSource = dt
                    gvAdjustment.DataBind()
                    gvAdjustment.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListGLStatistic()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        month = Left(tbDateWork.Text, 2)
        year = Right(tbDateWork.Text, 4)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*, (b.Description) AS GlGrp,(select sum(Credit) from iPxAcctGL_JVdtl where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totCredit, "
        sSQL += "(select sum(Debit) from iPxAcctGL_JVdtl where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totDebit, "
        sSQL += "(select 'Close' from iPxAcctGL_JVhdr as x where x.businessid ='" & Session("sBusinessID") & "' AND x.Status <> 'D' AND x.ReffNo like'PL%' and YEAR(x.TransDate)=YEAR(a.TransDate) AND MONTH(x.TransDate)=MONTH(a.TransDate)) as CloseStatus, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='10037' and x.active='Y') as verifedGL, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='25' and x.active='Y') as deleteGL "
        sSQL += "from iPxAcctGL_JVhdr AS a INNER JOIN iPxAcctGL_JVGrp AS b ON b.id=a.JVgroup "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and b.id='G8' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " AND month(a.TransDate)='" & month & "' AND year(a.TransDate)='" & year & "' AND a.Status <> 'D' "
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.TransDate asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvStatistic.DataSource = dt
                    gvStatistic.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvStatistic.DataSource = dt
                    gvStatistic.DataBind()
                    gvStatistic.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub Glgroup()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctGL_JVGrp where (businessid = '" & Session("sBusinessID") & "' or businessid = 'DF')"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlGroup.DataSource = dt
                dlGroup.DataTextField = "Description"
                dlGroup.DataValueField = "id"
                dlGroup.DataBind()
                dlGroup.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    'Sub GlgroupQuery()
    '    If oCnct.State = ConnectionState.Closed Then
    '        oCnct.Open()
    '    End If
    '    oSQLCmd = New SqlCommand(sSQL, oCnct)
    '    sSQL = "SELECT * FROM iPxAcctGL_JVGrp where (businessid = '" & Session("sBusinessID") & "' or businessid = 'DF')"
    '    Using sda As New SqlDataAdapter()
    '        oSQLCmd.CommandText = sSQL
    '        sda.SelectCommand = oSQLCmd
    '        Using dt As New DataTable()
    '            sda.Fill(dt)
    '            dlQGrp.DataSource = dt
    '            dlQGrp.DataTextField = "Description"
    '            dlQGrp.DataValueField = "id"
    '            dlQGrp.DataBind()
    '            dlQGrp.Items.Insert(0, "")
    '        End Using
    '    End Using
    'End Sub
    Sub showdata_dropdownStatus()
        dlQStatus.Items.Clear()
        dlQStatus.Items.Insert(0, "")
        dlQStatus.Items.Insert(1, "Open")
        dlQStatus.Items.Insert(2, "Verify")
        dlQStatus.Items.Insert(3, "Delete")
        dlQStatus.Items.Insert(4, "All")
    End Sub
    Sub editGLHeader()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctGL_JVhdr "
        sSQL += "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & tbTransID.Text & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Dim InvDate As Date = oSQLReader.Item("TransDate").ToString
            tbDate.Text = InvDate.ToString("dd/MM/yyyy")
            tbTransID.Text = oSQLReader.Item("TransID").ToString
            tbReff.Text = oSQLReader.Item("ReffNo").ToString
            tbDesc.Text = oSQLReader.Item("Description").ToString
            Dim grp As String = oSQLReader.Item("JVgroup").ToString
            oCnct.Close()
            Glgroup()
            dlGroup.SelectedValue = grp
        Else
            oCnct.Close()
        End If
    End Sub
    Sub UpdateGLHeader()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim GLDate As Date
        GLDate = Date.ParseExact(tbDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)

        sSQL = "UPDATE iPxAcctGL_JVhdr SET  TransDate='" & GLDate & "', JVgroup='" & dlGroup.SelectedValue & "', ReffNo='" & Replace(tbReff.Text, "'", "''") & "', Description='" & Replace(tbDesc.Text, "'", "''") & "'"
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & tbTransID.Text & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub Verify()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "UPDATE iPxAcctGL_JVhdr SET  Status='V' "
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & IDTrans & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub Unverify()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "UPDATE iPxAcctGL_JVhdr SET  Status='O' "
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & IDTrans & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub cekStatus()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctGL_JVhdr "
        sSQL += "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & Session("sTransID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            status = oSQLReader.Item("Status").ToString
            oCnct.Close()
            If status = "D" Then
                recycle()
                'deleteReason()
                Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
                cIpx.saveLog(Session("sBusinessID"), Session("sTransID"), TransDateLog, "GL", "Recycle", "Recycle general ledger " & Session("sTransID"), "", Session("sUserCode"))

            Else
                Delete()
                'saveReason()
                Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
                cIpx.saveLog(Session("sBusinessID"), Session("sTransID"), TransDateLog, "GL", "Delete", Replace(tbReason.Text, "'", "''"), "", Session("sUserCode"))
            End If
        Else
            oCnct.Close()
        End If
    End Sub
    Sub Delete()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "UPDATE iPxAcctGL_JVhdr SET  Status='D' "
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & Session("sTransID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "UPDATE iPxAcctAR_TransHdr SET  Status='N' "
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and TransID =(SELECT ReffNo FROM iPxAcctGL_JVhdr "
        sSQL += "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & Session("sTransID") & "')"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "UPDATE iPxAcctAR_Receipt SET  Status='N' "
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and ReceiptID=(SELECT ReffNo FROM iPxAcctGL_JVhdr "
        sSQL += "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & Session("sTransID") & "')"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

        'AP


        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "UPDATE iPxAcctAP_TransHdr SET  Status='N' "
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and TransID =(SELECT ReffNo FROM iPxAcctGL_JVhdr "
        sSQL += "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & Session("sTransID") & "')"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "UPDATE iPxAcctAP_Payment SET  Status='N' "
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and PaymentID =(SELECT ReffNo FROM iPxAcctGL_JVhdr "
        sSQL += "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & Session("sTransID") & "')"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub recycle()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "UPDATE iPxAcctGL_JVhdr SET  Status='O' "
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & Session("sTransID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "UPDATE iPxAcctAR_TransHdr SET  Status='P' "
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and TransID =(SELECT ReffNo FROM iPxAcctGL_JVhdr "
        sSQL += "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & Session("sTransID") & "')"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "UPDATE iPxAcctAR_Receipt SET  Status='P' "
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and ReceiptID=(SELECT ReffNo FROM iPxAcctGL_JVhdr "
        sSQL += "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & Session("sTransID") & "')"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub saveReason()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim regDate As Date = Date.Now()
        sSQL = "INSERT INTO iPxAcct_ReasonDelete(businessid,idAcct,dateReason,RegBy,reason,isActive)"
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & Session("sTransID") & "','" & regDate & "'"
        sSQL += ",'" & Session("iUserID") & "','" & Replace(tbReason.Text, "'", "''") & "','Y') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub deleteReason()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim regDate As Date = Date.Now()
        sSQL = "DELETE FROM iPxAcct_ReasonDelete "
        sSQL += "WHERE businessid='" & Session("sBusinessID") & "' AND idAcct ='" & Session("sTransID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub Docket()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select * from iPxAcctDocket_Report WHERE businessid in ('','" & Session("sBusinessID") & "') and Grp='GL'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDocket.DataSource = dt
                    gvDocket.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvDocket.DataSource = dt
                    gvDocket.DataBind()
                    gvDocket.Enabled = False
                    gvDocket.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub cekClose()
        Dim MonthCekClosing, YearCekClosing As String
        MonthCekClosing = Strings.Mid(tbDateWork.Text, 1, 2)
        YearCekClosing = Strings.Right(tbDateWork.Text, 4)
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
            lblStatusClose.Visible = True
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('This Month Period Has been Close !!');", True)
        Else
            lblStatusClose.Visible = False
        End If
        oCnct4.Close()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Me.IsPostBack Then
            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "GL Journal") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            Session("sQueryTicket") = ""
            If Session("sDateWorkGL") = "" Then
                tbDateWork.Text = Format(Now, "MM-yyyy")
            Else
                tbDateWork.Text = Session("sDateWorkGL")
            End If
            ListGLALL()
            ListGLSales()
            ListGLAR()
            ListGL()
            ListGLAP()
            ListGLCost()
            ListGLReceiving()
            ListGLAdjustment()
            ListGLStatistic()
            cekClose()
        Else
            ListGLALL()
            Session("sQueryTicket") = ""
            ListGLSales()
            Session("sQueryTicket") = ""
            ListGLAR()
            Session("sQueryTicket") = ""
            ListGL()
            Session("sQueryTicket") = ""
            ListGLAP()
            Session("sQueryTicket") = ""
            ListGLCost()
            Session("sQueryTicket") = ""
            ListGLReceiving()
            Session("sQueryTicket") = ""
            ListGLAdjustment()
            Session("sQueryTicket") = ""
            ListGLStatistic()
        End If
        UserAcces()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthGL", "$(document).ready(function() {MonthGL()});", True)
    End Sub
    Protected Sub TransdateWork(ByVal sender As Object, ByVal e As EventArgs)
        Session("sDateWorkGL") = tbDateWork.Text
        Session("sQueryTicket") = ""
        cekClose()
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvALL.PageIndex = e.NewPageIndex
        Me.ListGLALL()
        gvSales.PageIndex = e.NewPageIndex
        Me.ListGLSales()
        gvAR.PageIndex = e.NewPageIndex
        Me.ListGLAR()
        gvGL.PageIndex = e.NewPageIndex
        Me.ListGL()
        gvAP.PageIndex = e.NewPageIndex
        Me.ListGLAP()
        gvCost.PageIndex = e.NewPageIndex
        Me.ListGLCost()
        gvReceiving.PageIndex = e.NewPageIndex
        Me.ListGLReceiving()
        gvAdjustment.PageIndex = e.NewPageIndex
        Me.ListGLAdjustment()
        gvStatistic.PageIndex = e.NewPageIndex
        Me.ListGLStatistic()
    End Sub

    Protected Sub gvALL_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvALL.PageIndexChanging
        gvALL.PageIndex = e.NewPageIndex
        Me.ListGLALL()
    End Sub

    Protected Sub gvSales_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSales.PageIndexChanging
        gvSales.PageIndex = e.NewPageIndex
        Me.ListGLSales()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SalesActive", "SalesActive()", True)
    End Sub

    Protected Sub gvAR_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAR.PageIndexChanging
        gvAR.PageIndex = e.NewPageIndex
        Me.ListGLAR()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ARActive", "ARActive()", True)
    End Sub

    Protected Sub gvGL_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvGL.PageIndexChanging
        gvGL.PageIndex = e.NewPageIndex
        Me.ListGL()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GLActive", "GLActive()", True)
    End Sub

    Protected Sub gvAP_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAP.PageIndexChanging
        gvAP.PageIndex = e.NewPageIndex
        Me.ListGLAP()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "APActive", "APActive()", True)
    End Sub

    Protected Sub gvCost_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCost.PageIndexChanging
        gvCost.PageIndex = e.NewPageIndex
        Me.ListGLCost()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
    End Sub

    Protected Sub gvReceiving_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvReceiving.PageIndexChanging
        gvReceiving.PageIndex = e.NewPageIndex
        Me.ListGLReceiving()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ReceivingActive", "ReceivingActive()", True)
    End Sub

    Protected Sub gvAdjustment_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAdjustment.PageIndexChanging
        gvAdjustment.PageIndex = e.NewPageIndex
        Me.ListGLAdjustment()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AdjustmentActive", "AdjustmentActive()", True)
    End Sub

    Protected Sub gvStatistic_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvStatistic.PageIndexChanging
        gvStatistic.PageIndex = e.NewPageIndex
        Me.ListGLStatistic()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive()", True)
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvALL.PageIndex = e.NewPageIndex
        Me.ListGLALL()
        gvSales.PageIndex = e.NewPageIndex
        Me.ListGLSales()
        gvAR.PageIndex = e.NewPageIndex
        Me.ListGLAR()
        gvGL.PageIndex = e.NewPageIndex
        Me.ListGL()
        gvAP.PageIndex = e.NewPageIndex
        Me.ListGLAP()
        gvCost.PageIndex = e.NewPageIndex
        Me.ListGLCost()
        gvReceiving.PageIndex = e.NewPageIndex
        Me.ListGLReceiving()
        gvAdjustment.PageIndex = e.NewPageIndex
        Me.ListGLAdjustment()
        gvStatistic.PageIndex = e.NewPageIndex
        Me.ListGLStatistic()
    End Sub
    Protected Sub lbAddGL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddGL.Click
        Session("sEditGL") = ""
        Response.Redirect("iPxGLInputTransaction.aspx")
    End Sub

    Protected Sub gvALL_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvALL.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sEditGL") = e.CommandArgument
            Response.Redirect("iPxGLInputTransaction.aspx")
        ElseIf e.CommandName = "getEdit" Then
            tbTransID.Text = e.CommandArgument
            editGLHeader()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "$(document).ready(function() {datetimepicker()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        ElseIf e.CommandName = "getVer" Then
            IDTrans = e.CommandArgument
            Verify()
            ListGLALL()
        ElseIf e.CommandName = "getUnver" Then
            IDTrans = e.CommandArgument
            Unverify()
            ListGLALL()
        ElseIf e.CommandName = "getDelete" Then
            Session("sTransID") = e.CommandArgument
            tbReason.Text = ""
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
        ElseIf e.CommandName = "getRestore" Then
            Session("sTransID") = e.CommandArgument
            cekStatus()
            ListGLALL()
        ElseIf e.CommandName = "getPrint" Then
            Session("sTransID") = e.CommandArgument
            Docket()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDocket", "showModalDocket()", True)
        End If
    End Sub

    Protected Sub gvSales_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSales.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sEditGL") = e.CommandArgument
            Response.Redirect("iPxGLInputTransaction.aspx")
        ElseIf e.CommandName = "getEdit" Then
            tbTransID.Text = e.CommandArgument
            editGLHeader()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "$(document).ready(function() {datetimepicker()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SalesActive", "SalesActive()", True)
        ElseIf e.CommandName = "getVer" Then
            IDTrans = e.CommandArgument
            Verify()
            ListGLSales()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SalesActive", "SalesActive()", True)
        ElseIf e.CommandName = "getUnver" Then
            IDTrans = e.CommandArgument
            Unverify()
            ListGLSales()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SalesActive", "SalesActive()", True)
        ElseIf e.CommandName = "getDelete" Then
            Session("sTransID") = e.CommandArgument
            tbReason.Text = ""
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SalesActive", "SalesActive()", True)
        ElseIf e.CommandName = "getRestore" Then
            Session("sTransID") = e.CommandArgument
            cekStatus()
            ListGLSales()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SalesActive", "SalesActive()", True)
        ElseIf e.CommandName = "getPrint" Then
            Session("Tab") = "Sales"
            Session("sTransID") = e.CommandArgument
            Docket()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDocket", "showModalDocket()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SalesActive", "SalesActive()", True)
        End If
    End Sub

    Protected Sub gvAR_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAR.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sEditGL") = e.CommandArgument
            Response.Redirect("iPxGLInputTransaction.aspx")
        ElseIf e.CommandName = "getEdit" Then
            tbTransID.Text = e.CommandArgument
            editGLHeader()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "$(document).ready(function() {datetimepicker()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ARActive", "ALLActive()", True)
        ElseIf e.CommandName = "getVer" Then
            IDTrans = e.CommandArgument
            Verify()
            ListGLAR()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ARActive", "ALLActive()", True)
        ElseIf e.CommandName = "getUnver" Then
            IDTrans = e.CommandArgument
            Unverify()
            ListGLAR()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ARActive", "ALLActive()", True)
        ElseIf e.CommandName = "getDelete" Then
            Session("sTransID") = e.CommandArgument
            tbReason.Text = ""
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ARActive", "ALLActive()", True)
        ElseIf e.CommandName = "getRestore" Then
            Session("sTransID") = e.CommandArgument
            cekStatus()
            ListGLAR()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ARActive", "ALLActive()", True)
        ElseIf e.CommandName = "getPrint" Then
            Session("Tab") = "AR"
            Session("sTransID") = e.CommandArgument
            Docket()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDocket", "showModalDocket()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ARActive", "ALLActive()", True)
        End If
    End Sub

    Protected Sub gvGL_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGL.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sEditGL") = e.CommandArgument
            Response.Redirect("iPxGLInputTransaction.aspx")
        ElseIf e.CommandName = "getEdit" Then
            tbTransID.Text = e.CommandArgument
            editGLHeader()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "$(document).ready(function() {datetimepicker()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GLActive", "GLActive()", True)
        ElseIf e.CommandName = "getVer" Then
            IDTrans = e.CommandArgument
            Verify()
            ListGL()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GLActive", "GLActive()", True)
        ElseIf e.CommandName = "getUnver" Then
            IDTrans = e.CommandArgument
            Unverify()
            ListGL()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GLActive", "GLActive()", True)
        ElseIf e.CommandName = "getDelete" Then
            Session("sTransID") = e.CommandArgument
            tbReason.Text = ""
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GLActive", "GLActive()", True)
        ElseIf e.CommandName = "getRestore" Then
            Session("sTransID") = e.CommandArgument
            cekStatus()
            ListGL()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GLActive", "GLActive()", True)
        ElseIf e.CommandName = "getPrint" Then
            Session("Tab") = "GL"
            Session("sTransID") = e.CommandArgument
            Docket()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDocket", "showModalDocket()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GLActive", "GLActive()", True)
        End If
    End Sub

    Protected Sub gvAP_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAP.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sEditGL") = e.CommandArgument
            Response.Redirect("iPxGLInputTransaction.aspx")
        ElseIf e.CommandName = "getEdit" Then
            tbTransID.Text = e.CommandArgument
            editGLHeader()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "$(document).ready(function() {datetimepicker()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "APActive", "APActive()", True)
        ElseIf e.CommandName = "getVer" Then
            IDTrans = e.CommandArgument
            Verify()
            ListGLAP()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "APActive", "APActive()", True)
        ElseIf e.CommandName = "getUnver" Then
            IDTrans = e.CommandArgument
            Unverify()
            ListGLAP()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "APActive", "APActive()", True)
        ElseIf e.CommandName = "getDelete" Then
            Session("sTransID") = e.CommandArgument
            tbReason.Text = ""
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "APActive", "APActive()", True)
        ElseIf e.CommandName = "getRestore" Then
            Session("sTransID") = e.CommandArgument
            cekStatus()
            ListGLAP()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "APActive", "APActive()", True)
        ElseIf e.CommandName = "getPrint" Then
            Session("Tab") = "AP"
            Session("sTransID") = e.CommandArgument
            Docket()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDocket", "showModalDocket()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "APActive", "APActive()", True)
        End If
    End Sub

    Protected Sub gvCost_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCost.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sEditGL") = e.CommandArgument
            Response.Redirect("iPxGLInputTransaction.aspx")
        ElseIf e.CommandName = "getEdit" Then
            tbTransID.Text = e.CommandArgument
            editGLHeader()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "$(document).ready(function() {datetimepicker()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
        ElseIf e.CommandName = "getVer" Then
            IDTrans = e.CommandArgument
            Verify()
            ListGLCost()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
        ElseIf e.CommandName = "getUnver" Then
            IDTrans = e.CommandArgument
            Unverify()
            ListGLCost()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
        ElseIf e.CommandName = "getDelete" Then
            Session("sTransID") = e.CommandArgument
            tbReason.Text = ""
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
        ElseIf e.CommandName = "getRestore" Then
            Session("sTransID") = e.CommandArgument
            cekStatus()
            ListGLCost()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
        ElseIf e.CommandName = "getPrint" Then
            Session("Tab") = "Cost"
            Session("sTransID") = e.CommandArgument
            Docket()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDocket", "showModalDocket()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
        End If
    End Sub

    Protected Sub gvReceiving_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvReceiving.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sEditGL") = e.CommandArgument
            Response.Redirect("iPxGLInputTransaction.aspx")
        ElseIf e.CommandName = "getEdit" Then
            tbTransID.Text = e.CommandArgument
            editGLHeader()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "$(document).ready(function() {datetimepicker()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ReceivingActive", "ReceivingActive()", True)
        ElseIf e.CommandName = "getVer" Then
            IDTrans = e.CommandArgument
            Verify()
            ListGLReceiving()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ReceivingActive", "ReceivingActive()", True)
        ElseIf e.CommandName = "getUnver" Then
            IDTrans = e.CommandArgument
            Unverify()
            ListGLReceiving()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ReceivingActive", "ReceivingActive()", True)
        ElseIf e.CommandName = "getDelete" Then
            Session("sTransID") = e.CommandArgument
            tbReason.Text = ""
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ReceivingActive", "ReceivingActive()", True)
        ElseIf e.CommandName = "getRestore" Then
            Session("sTransID") = e.CommandArgument
            cekStatus()
            ListGLReceiving()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ReceivingActive", "ReceivingActive()", True)
        ElseIf e.CommandName = "getPrint" Then
            Session("Tab") = "Receiving"
            Session("sTransID") = e.CommandArgument
            Docket()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDocket", "showModalDocket()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ReceivingActive", "ReceivingActive()", True)
        End If
    End Sub

    Protected Sub gvAdjustment_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAdjustment.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sEditGL") = e.CommandArgument
            Response.Redirect("iPxGLInputTransaction.aspx")
        ElseIf e.CommandName = "getEdit" Then
            tbTransID.Text = e.CommandArgument
            editGLHeader()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "$(document).ready(function() {datetimepicker()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AdjustmentActive", "AdjustmentActive()", True)
        ElseIf e.CommandName = "getVer" Then
            IDTrans = e.CommandArgument
            Verify()
            ListGLAdjustment()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AdjustmentActive", "AdjustmentActive()", True)
        ElseIf e.CommandName = "getUnver" Then
            IDTrans = e.CommandArgument
            Unverify()
            ListGLAdjustment()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AdjustmentActive", "AdjustmentActive()", True)
        ElseIf e.CommandName = "getDelete" Then
            Session("sTransID") = e.CommandArgument
            tbReason.Text = ""
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AdjustmentActive", "AdjustmentActive()", True)
        ElseIf e.CommandName = "getRestore" Then
            Session("sTransID") = e.CommandArgument
            cekStatus()
            ListGLAdjustment()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AdjustmentActive", "AdjustmentActive()", True)
        ElseIf e.CommandName = "getPrint" Then
            Session("Tab") = "Adjustment"
            Session("sTransID") = e.CommandArgument
            Docket()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDocket", "showModalDocket()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AdjustmentActive", "AdjustmentActive()", True)
        End If
    End Sub

    Protected Sub gvStatistic_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvStatistic.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sEditGL") = e.CommandArgument
            Response.Redirect("iPxGLInputTransaction.aspx")
        ElseIf e.CommandName = "getEdit" Then
            tbTransID.Text = e.CommandArgument
            editGLHeader()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "$(document).ready(function() {datetimepicker()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive()", True)
        ElseIf e.CommandName = "getVer" Then
            IDTrans = e.CommandArgument
            Verify()
            ListGLStatistic()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive()", True)
        ElseIf e.CommandName = "getUnver" Then
            IDTrans = e.CommandArgument
            Unverify()
            ListGLStatistic()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive()", True)
        ElseIf e.CommandName = "getDelete" Then
            Session("sTransID") = e.CommandArgument
            tbReason.Text = ""
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive()", True)
        ElseIf e.CommandName = "getRestore" Then
            Session("sTransID") = e.CommandArgument
            cekStatus()
            ListGLStatistic()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive()", True)
        ElseIf e.CommandName = "getPrint" Then
            Session("Tab") = "Statistic"
            Session("sTransID") = e.CommandArgument
            Docket()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDocket", "showModalDocket()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive()", True)
        End If
    End Sub

    Protected Sub lbAbortEditHeader_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortEditHeader.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
    End Sub

    Protected Sub lbUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbUpdate.Click
        If tbReff.Text = "" Then
            editGLHeader()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "$(document).ready(function() {datetimepicker()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Reff No !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            Dim a As String = hfCount.Value
            If a = "Sales" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SalesActive", "SalesActive()", True)
            ElseIf a = "AR" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ARActive", "ALLActive()", True)
            ElseIf a = "GL" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GLActive", "GLActive()", True)
            ElseIf a = "AP" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "APActive", "APActive()", True)
            ElseIf a = "Cost" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
            ElseIf a = "Receiving" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ReceivingActive", "ReceivingActive()", True)
            ElseIf a = "Adjustment" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AdjustmentActive", "AdjustmentActive()", True)
            ElseIf a = "Statistic" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive()", True)
            End If
        ElseIf tbDate.Text = "" Then
            editGLHeader()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "$(document).ready(function() {datetimepicker()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Date !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            Dim a As String = hfCount.Value
            If a = "Sales" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SalesActive", "SalesActive()", True)
            ElseIf a = "AR" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ARActive", "ALLActive()", True)
            ElseIf a = "GL" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GLActive", "GLActive()", True)
            ElseIf a = "AP" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "APActive", "APActive()", True)
            ElseIf a = "Cost" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
            ElseIf a = "Receiving" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ReceivingActive", "ReceivingActive()", True)
            ElseIf a = "Adjustment" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AdjustmentActive", "AdjustmentActive()", True)
            ElseIf a = "Statistic" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive()", True)
            End If
        ElseIf dlGroup.Text = "" Then
            editGLHeader()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "$(document).ready(function() {datetimepicker()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select Group !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            Dim a As String = hfCount.Value
            If a = "Sales" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SalesActive", "SalesActive()", True)
            ElseIf a = "AR" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ARActive", "ALLActive()", True)
            ElseIf a = "GL" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GLActive", "GLActive()", True)
            ElseIf a = "AP" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "APActive", "APActive()", True)
            ElseIf a = "Cost" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
            ElseIf a = "Receiving" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ReceivingActive", "ReceivingActive()", True)
            ElseIf a = "Adjustment" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AdjustmentActive", "AdjustmentActive()", True)
            ElseIf a = "Statistic" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive()", True)
            End If
        Else
            UpdateGLHeader()
            ListGLALL()
            ListGLSales()
            ListGLAR()
            ListGL()
            ListGLAP()
            ListGLCost()
            ListGLReceiving()
            ListGLAdjustment()
            ListGLStatistic()
            Dim a As String = hfCount.Value
            If a = "Sales" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SalesActive", "SalesActive()", True)
            ElseIf a = "AR" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ARActive", "ALLActive()", True)
            ElseIf a = "GL" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GLActive", "GLActive()", True)
            ElseIf a = "AP" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "APActive", "APActive()", True)
            ElseIf a = "Cost" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
            ElseIf a = "Receiving" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ReceivingActive", "ReceivingActive()", True)
            ElseIf a = "Adjustment" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AdjustmentActive", "AdjustmentActive()", True)
            ElseIf a = "Statistic" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive()", True)
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
        End If
    End Sub

    Protected Sub lbQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQuery.Click
        kosongQuery()
        showdata_dropdownStatus()
        'GlgroupQuery()
        Dim a As String = hfCount.Value
        If a = "Sales" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SalesActive", "SalesActive()", True)
        ElseIf a = "AR" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ARActive", "ALLActive()", True)
        ElseIf a = "GL" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GLActive", "GLActive()", True)
        ElseIf a = "AP" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "APActive", "APActive()", True)
        ElseIf a = "Cost" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
        ElseIf a = "Receiving" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ReceivingActive", "ReceivingActive()", True)
        ElseIf a = "Adjustment" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AdjustmentActive", "AdjustmentActive()", True)
        ElseIf a = "Statistic" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive()", True)
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "$(document).ready(function() {datetimepicker()});", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub lbAbortQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortQuery.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        Dim a As String = hfCount.Value
        If a = "ALL" Then
        ElseIf a = "Sales" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SalesActive", "SalesActive()", True)
        ElseIf a = "AR" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ARActive", "ALLActive()", True)
        ElseIf a = "GL" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GLActive", "GLActive()", True)
        ElseIf a = "AP" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "APActive", "APActive()", True)
        ElseIf a = "Cost" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
        ElseIf a = "Receiving" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ReceivingActive", "ReceivingActive()", True)
        ElseIf a = "Adjustment" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AdjustmentActive", "AdjustmentActive()", True)
        ElseIf a = "Statistic" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive()", True)
        End If
    End Sub

    Protected Sub lblQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblQuery.Click
        Session("sQueryTicket") = ""
        If dlQStatus.SelectedIndex = 0 Then
            If tbQDate.Text = "" And tbQFrom.Text = "" And tbQUntil.Text = "" Then
                month = Left(tbDateWork.Text, 2)
                year = Right(tbDateWork.Text, 4)
                Session("sCondition") = Session("sCondition") & " AND month(a.TransDate)='" & month & "' AND year(a.TransDate)='" & year & "' AND a.Status='O' "
            ElseIf tbQDate.Text <> "" Or tbQFrom.Text <> "" Or tbQUntil.Text <> "" Then
                Session("sCondition") = Session("sCondition") & " and a.Status= 'O' "
                tbDateWork.Text = Format(Now, "MM-yyyy")
                Session("sDateWorkGL") = tbDate.Text
            End If
        ElseIf dlQStatus.SelectedIndex = 1 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'O'"
        ElseIf dlQStatus.SelectedIndex = 2 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'V'"
        ElseIf dlQStatus.SelectedIndex = 3 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'D'"
        ElseIf dlQStatus.SelectedIndex = 4 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'O' or a.Status = 'V' or a.Status = 'D'"
        End If
        If tbQIdGL.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (a.TransID = '" & Replace(tbQIdGL.Text, "'", "''") & "') "
        End If
        If tbQReff.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (a.ReffNo like '%" & Replace(tbQReff.Text, "'", "''") & "%') "
        End If
        If tbQDate.Text.Trim <> "" Then
            Dim transDate As Date = Date.ParseExact(tbQDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate between '" & transDate & " 00:00:00' and '" & transDate & " 23:59:00') "
        End If
        If tbQUntil.Text.Trim <> "" Then
            Dim PerUntl As Date = Date.ParseExact(tbQUntil.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate <= '" & PerUntl & " 23:59:00') "
        End If

        If tbQFrom.Text.Trim <> "" Then
            Dim PerFrom As Date = Date.ParseExact(tbQFrom.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate >= '" & PerFrom & " 00:00:00') "
        End If
        ListGLALL()
        Session("sQueryTicket") = ""
        If dlQStatus.SelectedIndex = 0 Then
            If tbQDate.Text = "" And tbQFrom.Text = "" And tbQUntil.Text = "" Then
                month = Left(tbDateWork.Text, 2)
                year = Right(tbDateWork.Text, 4)
                Session("sCondition") = Session("sCondition") & " AND month(a.TransDate)='" & month & "' AND year(a.TransDate)='" & year & "' AND a.Status='O' "
            ElseIf tbQDate.Text <> "" Or tbQFrom.Text <> "" Or tbQUntil.Text <> "" Then
                Session("sCondition") = Session("sCondition") & " and a.Status= 'O' "
                tbDateWork.Text = Format(Now, "MM-yyyy")
                Session("sDateWorkGL") = tbDate.Text
            End If
        ElseIf dlQStatus.SelectedIndex = 1 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'O'"
        ElseIf dlQStatus.SelectedIndex = 2 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'V'"
        ElseIf dlQStatus.SelectedIndex = 3 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'D'"
        ElseIf dlQStatus.SelectedIndex = 4 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'O' or a.Status = 'V' or a.Status = 'D'"
        End If
        If tbQReff.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (a.ReffNo like '%" & Replace(tbQReff.Text, "'", "''") & "%') "
        End If
        'If dlQGrp.Text.Trim <> "" Then
        '    Session("sCondition") = Session("sCondition") & " and (a.JVgroup = '" & dlQGrp.SelectedValue & "') "
        'End If
        If tbQDate.Text.Trim <> "" Then
            Dim transDate As Date = Date.ParseExact(tbQDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate between '" & transDate & " 00:00:00' and '" & transDate & " 23:59:00') "
        End If
        If tbQUntil.Text.Trim <> "" Then
            Dim PerUntl As Date = Date.ParseExact(tbQUntil.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate <= '" & PerUntl & " 23:59:00') "
        End If

        If tbQFrom.Text.Trim <> "" Then
            Dim PerFrom As Date = Date.ParseExact(tbQFrom.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate >= '" & PerFrom & " 00:00:00') "
        End If
        ListGLSales()
        Session("sQueryTicket") = ""
        If dlQStatus.SelectedIndex = 0 Then
            If tbQDate.Text = "" And tbQFrom.Text = "" And tbQUntil.Text = "" Then
                month = Left(tbDateWork.Text, 2)
                year = Right(tbDateWork.Text, 4)
                Session("sCondition") = Session("sCondition") & " AND month(a.TransDate)='" & month & "' AND year(a.TransDate)='" & year & "' AND a.Status='O' "
            ElseIf tbQDate.Text <> "" Or tbQFrom.Text <> "" Or tbQUntil.Text <> "" Then
                Session("sCondition") = Session("sCondition") & " and a.Status= 'O' "
                tbDateWork.Text = Format(Now, "MM-yyyy")
                Session("sDateWorkGL") = tbDate.Text
            End If
        ElseIf dlQStatus.SelectedIndex = 1 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'O'"
        ElseIf dlQStatus.SelectedIndex = 2 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'V'"
        ElseIf dlQStatus.SelectedIndex = 3 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'D'"
        ElseIf dlQStatus.SelectedIndex = 4 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'O' or a.Status = 'V' or a.Status = 'D'"
        End If
        If tbQReff.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (a.ReffNo like '%" & Replace(tbQReff.Text, "'", "''") & "%') "
        End If
        'If dlQGrp.Text.Trim <> "" Then
        '    Session("sCondition") = Session("sCondition") & " and (a.JVgroup = '" & dlQGrp.SelectedValue & "') "
        'End If
        If tbQDate.Text.Trim <> "" Then
            Dim transDate As Date = Date.ParseExact(tbQDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate between '" & transDate & " 00:00:00' and '" & transDate & " 23:59:00') "
        End If
        If tbQUntil.Text.Trim <> "" Then
            Dim PerUntl As Date = Date.ParseExact(tbQUntil.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate <= '" & PerUntl & " 23:59:00') "
        End If

        If tbQFrom.Text.Trim <> "" Then
            Dim PerFrom As Date = Date.ParseExact(tbQFrom.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate >= '" & PerFrom & " 00:00:00') "
        End If
        ListGLAR()
        Session("sQueryTicket") = ""
        If dlQStatus.SelectedIndex = 0 Then
            If tbQDate.Text = "" And tbQFrom.Text = "" And tbQUntil.Text = "" Then
                month = Left(tbDateWork.Text, 2)
                year = Right(tbDateWork.Text, 4)
                Session("sCondition") = Session("sCondition") & " AND month(a.TransDate)='" & month & "' AND year(a.TransDate)='" & year & "' AND a.Status='O' "
            ElseIf tbQDate.Text <> "" Or tbQFrom.Text <> "" Or tbQUntil.Text <> "" Then
                Session("sCondition") = Session("sCondition") & " and a.Status= 'O' "
                tbDateWork.Text = Format(Now, "MM-yyyy")
                Session("sDateWorkGL") = tbDate.Text
            End If
        ElseIf dlQStatus.SelectedIndex = 1 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'O'"
        ElseIf dlQStatus.SelectedIndex = 2 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'V'"
        ElseIf dlQStatus.SelectedIndex = 3 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'D'"
        ElseIf dlQStatus.SelectedIndex = 4 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'O' or a.Status = 'V' or a.Status = 'D'"
        End If
        If tbQReff.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (a.ReffNo like '%" & Replace(tbQReff.Text, "'", "''") & "%') "
        End If
        'If dlQGrp.Text.Trim <> "" Then
        '    Session("sCondition") = Session("sCondition") & " and (a.JVgroup = '" & dlQGrp.SelectedValue & "') "
        'End If
        If tbQDate.Text.Trim <> "" Then
            Dim transDate As Date = Date.ParseExact(tbQDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate between '" & transDate & " 00:00:00' and '" & transDate & " 23:59:00') "
        End If
        If tbQUntil.Text.Trim <> "" Then
            Dim PerUntl As Date = Date.ParseExact(tbQUntil.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate <= '" & PerUntl & " 23:59:00') "
        End If

        If tbQFrom.Text.Trim <> "" Then
            Dim PerFrom As Date = Date.ParseExact(tbQFrom.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate >= '" & PerFrom & " 00:00:00') "
        End If
        ListGL()
        Session("sQueryTicket") = ""
        If dlQStatus.SelectedIndex = 0 Then
            If tbQDate.Text = "" And tbQFrom.Text = "" And tbQUntil.Text = "" Then
                month = Left(tbDateWork.Text, 2)
                year = Right(tbDateWork.Text, 4)
                Session("sCondition") = Session("sCondition") & " AND month(a.TransDate)='" & month & "' AND year(a.TransDate)='" & year & "' AND a.Status='O' "
            ElseIf tbQDate.Text <> "" Or tbQFrom.Text <> "" Or tbQUntil.Text <> "" Then
                Session("sCondition") = Session("sCondition") & " and a.Status= 'O' "
                tbDateWork.Text = Format(Now, "MM-yyyy")
                Session("sDateWorkGL") = tbDate.Text
            End If
        ElseIf dlQStatus.SelectedIndex = 1 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'O'"
        ElseIf dlQStatus.SelectedIndex = 2 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'V'"
        ElseIf dlQStatus.SelectedIndex = 3 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'D'"
        ElseIf dlQStatus.SelectedIndex = 4 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'O' or a.Status = 'V' or a.Status = 'D'"
        End If
        If tbQReff.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (a.ReffNo like '%" & Replace(tbQReff.Text, "'", "''") & "%') "
        End If
        'If dlQGrp.Text.Trim <> "" Then
        '    Session("sCondition") = Session("sCondition") & " and (a.JVgroup = '" & dlQGrp.SelectedValue & "') "
        'End If
        If tbQDate.Text.Trim <> "" Then
            Dim transDate As Date = Date.ParseExact(tbQDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate between '" & transDate & " 00:00:00' and '" & transDate & " 23:59:00') "
        End If
        If tbQUntil.Text.Trim <> "" Then
            Dim PerUntl As Date = Date.ParseExact(tbQUntil.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate <= '" & PerUntl & " 23:59:00') "
        End If

        If tbQFrom.Text.Trim <> "" Then
            Dim PerFrom As Date = Date.ParseExact(tbQFrom.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate >= '" & PerFrom & " 00:00:00') "
        End If
        ListGLAP()
        Session("sQueryTicket") = ""
        If dlQStatus.SelectedIndex = 0 Then
            If tbQDate.Text = "" And tbQFrom.Text = "" And tbQUntil.Text = "" Then
                month = Left(tbDateWork.Text, 2)
                year = Right(tbDateWork.Text, 4)
                Session("sCondition") = Session("sCondition") & " AND month(a.TransDate)='" & month & "' AND year(a.TransDate)='" & year & "' AND a.Status='O' "
            ElseIf tbQDate.Text <> "" Or tbQFrom.Text <> "" Or tbQUntil.Text <> "" Then
                Session("sCondition") = Session("sCondition") & " and a.Status= 'O' "
                tbDateWork.Text = Format(Now, "MM-yyyy")
                Session("sDateWorkGL") = tbDate.Text
            End If
        ElseIf dlQStatus.SelectedIndex = 1 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'O'"
        ElseIf dlQStatus.SelectedIndex = 2 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'V'"
        ElseIf dlQStatus.SelectedIndex = 3 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'D'"
        ElseIf dlQStatus.SelectedIndex = 4 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'O' or a.Status = 'V' or a.Status = 'D'"
        End If
        If tbQReff.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (a.ReffNo like '%" & Replace(tbQReff.Text, "'", "''") & "%') "
        End If
        'If dlQGrp.Text.Trim <> "" Then
        '    Session("sCondition") = Session("sCondition") & " and (a.JVgroup = '" & dlQGrp.SelectedValue & "') "
        'End If
        If tbQDate.Text.Trim <> "" Then
            Dim transDate As Date = Date.ParseExact(tbQDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate between '" & transDate & " 00:00:00' and '" & transDate & " 23:59:00') "
        End If
        If tbQUntil.Text.Trim <> "" Then
            Dim PerUntl As Date = Date.ParseExact(tbQUntil.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate <= '" & PerUntl & " 23:59:00') "
        End If

        If tbQFrom.Text.Trim <> "" Then
            Dim PerFrom As Date = Date.ParseExact(tbQFrom.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate >= '" & PerFrom & " 00:00:00') "
        End If
        ListGLCost()
        Session("sQueryTicket") = ""
        If dlQStatus.SelectedIndex = 0 Then
            If tbQDate.Text = "" And tbQFrom.Text = "" And tbQUntil.Text = "" Then
                month = Left(tbDateWork.Text, 2)
                year = Right(tbDateWork.Text, 4)
                Session("sCondition") = Session("sCondition") & " AND month(a.TransDate)='" & month & "' AND year(a.TransDate)='" & year & "' AND a.Status='O' "
            ElseIf tbQDate.Text <> "" Or tbQFrom.Text <> "" Or tbQUntil.Text <> "" Then
                Session("sCondition") = Session("sCondition") & " and a.Status= 'O' "
                tbDateWork.Text = Format(Now, "MM-yyyy")
                Session("sDateWorkGL") = tbDate.Text
            End If
        ElseIf dlQStatus.SelectedIndex = 1 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'O'"
        ElseIf dlQStatus.SelectedIndex = 2 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'V'"
        ElseIf dlQStatus.SelectedIndex = 3 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'D'"
        ElseIf dlQStatus.SelectedIndex = 4 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'O' or a.Status = 'V' or a.Status = 'D'"
        End If
        If tbQReff.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (a.ReffNo like '%" & Replace(tbQReff.Text, "'", "''") & "%') "
        End If
        'If dlQGrp.Text.Trim <> "" Then
        '    Session("sCondition") = Session("sCondition") & " and (a.JVgroup = '" & dlQGrp.SelectedValue & "') "
        'End If
        If tbQDate.Text.Trim <> "" Then
            Dim transDate As Date = Date.ParseExact(tbQDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate between '" & transDate & " 00:00:00' and '" & transDate & " 23:59:00') "
        End If
        If tbQUntil.Text.Trim <> "" Then
            Dim PerUntl As Date = Date.ParseExact(tbQUntil.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate <= '" & PerUntl & " 23:59:00') "
        End If

        If tbQFrom.Text.Trim <> "" Then
            Dim PerFrom As Date = Date.ParseExact(tbQFrom.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (a.TransDate >= '" & PerFrom & " 00:00:00') "
        End If
        ListGLReceiving()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        Dim a As String = hfCount.Value
        If a = "ALL" Then
        ElseIf a = "Sales" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SalesActive", "SalesActive()", True)
        ElseIf a = "AR" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ARActive", "ALLActive()", True)
        ElseIf a = "GL" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GLActive", "GLActive()", True)
        ElseIf a = "AP" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "APActive", "APActive()", True)
        ElseIf a = "Cost" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
        ElseIf a = "Receiving" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ReceivingActive", "ReceivingActive()", True)
        ElseIf a = "Adjustment" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AdjustmentActive", "AdjustmentActive()", True)
        ElseIf a = "Statistic" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive()", True)
        End If
    End Sub

    Protected Sub lbDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbDelete.Click
        If tbReason.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter your reason first !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
        Else
            IDTrans = Session("sTransID")
            cekStatus()
            ListGLALL()
            ListGLSales()
            ListGLAR()
            ListGL()
            ListGLAP()
            ListGLCost()
            ListGLReceiving()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
            Dim a As String = hfCount.Value
            If a = "ALL" Then
            ElseIf a = "Sales" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SalesActive", "SalesActive()", True)
            ElseIf a = "AR" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ARActive", "ALLActive()", True)
            ElseIf a = "GL" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GLActive", "GLActive()", True)
            ElseIf a = "AP" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "APActive", "APActive()", True)
            ElseIf a = "Cost" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
            ElseIf a = "Receiving" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ReceivingActive", "ReceivingActive()", True)
            ElseIf a = "Adjustment" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AdjustmentActive", "AdjustmentActive()", True)
            ElseIf a = "Statistic" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive()", True)
            End If
        End If
    End Sub

    Protected Sub lbAbortDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDelete.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
        Dim a As String = hfCount.Value
        If a = "ALL" Then
        ElseIf a = "Sales" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SalesActive", "SalesActive()", True)
        ElseIf a = "AR" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ARActive", "ALLActive()", True)
        ElseIf a = "GL" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GLActive", "GLActive()", True)
        ElseIf a = "AP" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "APActive", "APActive()", True)
        ElseIf a = "Cost" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
        ElseIf a = "Receiving" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ReceivingActive", "ReceivingActive()", True)
        End If
    End Sub

    Protected Sub gvDocket_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDocket.RowCommand
        If e.CommandName = "getDocket" Then
            Session("sReport") = "GLJurnal"
            Session("sMapPath") = "~/iPxReportFile/" + e.CommandArgument
            Response.Redirect("rptviewer.aspx")
        End If
    End Sub

    Protected Sub lbAbortDocket_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDocket.Click
        If Session("Tab") = "Sales" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SalesActive", "SalesActive()", True)
        ElseIf Session("Tab") = "AR" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ARActive", "ARActive()", True)
        ElseIf Session("Tab") = "GL" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "GLActive", "GLActive()", True)
        ElseIf Session("Tab") = "AP" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "APActive", "APActive()", True)
        ElseIf Session("Tab") = "Cost" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive()", True)
        ElseIf Session("Tab") = "Receiving" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ReceivingActive", "ReceivingActive()", True)
        ElseIf Session("Tab") = "Adjustment" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AdjustmentActive", "AdjustmentActive()", True)
        ElseIf Session("Tab") = "Statistic" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive()", True)
        End If
    End Sub
End Class
