Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxGLRecaring
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
        tbQDate.Text = ""
        tbQFrom.Text = ""
        tbQReff.Text = ""
        tbQUntil.Text = ""
    End Sub
    Sub ListGL()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        'month = Left(tbDateWork.Text, 2)
        'year = Right(tbDateWork.Text, 4)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*, (b.Description) AS GlGrp,(select sum(Credit) from iPxAcctGL_JVdtlRec where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totCredit, "
        sSQL += "(select sum(Debit) from iPxAcctGL_JVdtlRec where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totDebit, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='25' and x.active='Y') as deleteGL "
        sSQL += "from iPxAcctGL_JVhdrRec AS a INNER JOIN iPxAcctGL_JVGrp AS b ON b.id=a.JVgroup "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " AND a.Status <> 'D' "
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
    Sub GlgroupQuery()
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
                dlQGrp.DataSource = dt
                dlQGrp.DataTextField = "Description"
                dlQGrp.DataValueField = "id"
                dlQGrp.DataBind()
                dlQGrp.Items.Insert(0, "")
            End Using
        End Using
    End Sub
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
        sSQL = "SELECT * FROM iPxAcctGL_JVhdrRec "
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

        sSQL = "UPDATE iPxAcctGL_JVhdrRec SET  TransDate='" & GLDate & "', JVgroup='" & dlGroup.SelectedValue & "', ReffNo='" & Replace(tbReff.Text, "'", "''") & "', Description='" & Replace(tbDesc.Text, "'", "''") & "'"
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

        sSQL = "UPDATE iPxAcctGL_JVhdrRec SET  Status='V' "
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

        sSQL = "UPDATE iPxAcctGL_JVhdrRec SET  Status='O' "
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
        sSQL = "SELECT * FROM iPxAcctGL_JVhdrRec "
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

        sSQL = "UPDATE iPxAcctGL_JVhdrRec SET  Status='D' "
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & Session("sTransID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

        'If oCnct.State = ConnectionState.Closed Then
        '    oCnct.Open()
        'End If
        'oSQLCmd = New SqlCommand(sSQL, oCnct)

        'sSQL = "UPDATE iPxAcctAR_TransHdrRec SET  Status='N' "
        'sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and TransID =(SELECT ReffNo FROM iPxAcctGL_JVhdr "
        'sSQL += "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & Session("sTransID") & "')"

        'oSQLCmd.CommandText = sSQL
        'oSQLCmd.ExecuteNonQuery()

        'oCnct.Close()

        'If oCnct.State = ConnectionState.Closed Then
        '    oCnct.Open()
        'End If
        'oSQLCmd = New SqlCommand(sSQL, oCnct)

        'sSQL = "UPDATE iPxAcctAR_Receipt SET  Status='N' "
        'sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and ReceiptID=(SELECT ReffNo FROM iPxAcctGL_JVhdr "
        'sSQL += "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & Session("sTransID") & "')"

        'oSQLCmd.CommandText = sSQL
        'oSQLCmd.ExecuteNonQuery()

        'oCnct.Close()
    End Sub
    Sub recycle()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "UPDATE iPxAcctGL_JVhdrRec SET  Status='O' "
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & Session("sTransID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        'If oCnct.State = ConnectionState.Closed Then
        '    oCnct.Open()
        'End If
        'oSQLCmd = New SqlCommand(sSQL, oCnct)

        'sSQL = "UPDATE iPxAcctAR_TransHdr SET  Status='P' "
        'sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and TransID =(SELECT ReffNo FROM iPxAcctGL_JVhdr "
        'sSQL += "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & Session("sTransID") & "')"

        'oSQLCmd.CommandText = sSQL
        'oSQLCmd.ExecuteNonQuery()

        'oCnct.Close()

        'If oCnct.State = ConnectionState.Closed Then
        '    oCnct.Open()
        'End If
        'oSQLCmd = New SqlCommand(sSQL, oCnct)

        'sSQL = "UPDATE iPxAcctAR_Receipt SET  Status='P' "
        'sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and ReceiptID=(SELECT ReffNo FROM iPxAcctGL_JVhdr "
        'sSQL += "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & Session("sTransID") & "')"

        'oSQLCmd.CommandText = sSQL
        'oSQLCmd.ExecuteNonQuery()

        'oCnct.Close()
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
            'If Session("sDateWorkGL") = "" Then
            '    tbDateWork.Text = Format(Now, "MM-yyyy")
            'Else
            '    tbDateWork.Text = Session("sDateWorkGL")
            'End If
            ListGL()
        Else
            ListGL()
        End If
        UserAcces()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthGL", "$(document).ready(function() {MonthGL()});", True)
    End Sub
    Protected Sub TransdateWork(ByVal sender As Object, ByVal e As EventArgs)
        'Session("sDateWorkGL") = tbDateWork.Text
        Session("sQueryTicket") = ""
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvGL.PageIndex = e.NewPageIndex
        Me.ListGL()
    End Sub

    Protected Sub gvGL_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvGL.PageIndexChanging
        gvGL.PageIndex = e.NewPageIndex
        Me.ListGL()
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvGL.PageIndex = e.NewPageIndex
        Me.ListGL()
    End Sub
    Protected Sub lbAddGL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddGL.Click
        Session("sEditGL") = ""
        Response.Redirect("iPxGLInputRecaring.aspx")
    End Sub

    Protected Sub gvGL_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGL.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sEditGL") = e.CommandArgument
            Response.Redirect("iPxGLInputRecaring.aspx")
        ElseIf e.CommandName = "getEdit" Then
            tbTransID.Text = e.CommandArgument
            editGLHeader()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "$(document).ready(function() {datetimepicker()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        ElseIf e.CommandName = "getVer" Then
            IDTrans = e.CommandArgument
            Verify()
            ListGL()
        ElseIf e.CommandName = "getUnver" Then
            IDTrans = e.CommandArgument
            Unverify()
            ListGL()
        ElseIf e.CommandName = "getDelete" Then
            Session("sTransID") = e.CommandArgument
            tbReason.Text = ""
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
        ElseIf e.CommandName = "getRestore" Then
            Session("sTransID") = e.CommandArgument
            cekStatus()
            ListGL()
        ElseIf e.CommandName = "getPrint" Then
            Session("sTransID") = e.CommandArgument
            Session("sReport") = "GLJurnal"
            Session("sMapPath") = "~/iPxReportFile/dckGL_Journal.rpt"
            Response.Redirect("rptviewer.aspx")
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
        ElseIf tbDate.Text = "" Then
            editGLHeader()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "$(document).ready(function() {datetimepicker()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Date !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        ElseIf dlGroup.Text = "" Then
            editGLHeader()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "$(document).ready(function() {datetimepicker()});", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select Group !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        Else
            UpdateGLHeader()
            ListGL()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
        End If
    End Sub

    Protected Sub lbQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQuery.Click
        kosongQuery()
        showdata_dropdownStatus()
        GlgroupQuery()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "$(document).ready(function() {datetimepicker()});", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub lbAbortQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortQuery.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
    End Sub

    Protected Sub lblQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblQuery.Click
        Session("sQueryTicket") = ""
        If dlQStatus.SelectedIndex = 0 Then
            'If tbQDate.Text = "" And tbQFrom.Text = "" And tbQUntil.Text = "" Then
            '    month = Left(tbDateWork.Text, 2)
            '    year = Right(tbDateWork.Text, 4)
            Session("sCondition") = Session("sCondition") & " AND a.Status='O' "
            'ElseIf tbQDate.Text <> "" Or tbQFrom.Text <> "" Or tbQUntil.Text <> "" Then
            '    Session("sCondition") = Session("sCondition") & " and a.Status= 'O' "
            '    tbDateWork.Text = Format(Now, "MM-yyyy")
            '    Session("sDateWorkGL") = tbDate.Text
            'End If
        ElseIf dlQStatus.SelectedIndex = 1 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'O'"
        ElseIf dlQStatus.SelectedIndex = 2 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'V'"
        ElseIf dlQStatus.SelectedIndex = 3 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'D'"
        ElseIf dlQStatus.SelectedIndex = 4 Then
            Session("sCondition") = Session("sCondition") & " and a.Status = 'O' or a.Status = 'V' or a.Status = 'D'"
        End If
        If tbReff.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (a.Reff like '%" & Replace(tbQReff.Text, "'", "''") & "%') "
        End If
        If dlQGrp.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (a.JVgroup = '" & dlQGrp.SelectedValue & "') "
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
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        ListGL()
    End Sub

    Protected Sub lbDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbDelete.Click
        If tbReason.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter your reason first !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
        Else
            IDTrans = Session("sTransID")
            cekStatus()
            ListGL()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
        End If
    End Sub

    Protected Sub lbAbortDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDelete.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
    End Sub
End Class
