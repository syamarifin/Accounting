Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing

Partial Class iPxDashboard_releasenotedtl
    Inherits System.Web.UI.Page
    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Public oCnct As SqlConnection = New SqlConnection(sCnct)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL As String
    '-------------------------------
    Dim Code As String

    Dim cipx As New iPxClass

    Public Function ExecuteQuery(ByVal cmd As SqlCommand, ByVal action As String) As DataTable
        Dim conString As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ConnectionString
        Using con As New SqlConnection(conString)
            cmd.Connection = con
            Select Case action
                Case "SELECT"
                    Using sda As New SqlDataAdapter()
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            Return dt
                        End Using
                    End Using
                Case "UPDATE"
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                    Exit Select
            End Select
            Return Nothing
        End Using
    End Function


    Sub dropdownAPP()
        sSQL = "select * from  iPx_general_verGRP where  Active='Y'"
        Dim cmd As New SqlCommand(sSQL)
        ddlApp.DataSource = ExecuteQuery(cmd, "SELECT")
        ddlApp.DataTextField = "app"
        ddlApp.DataValueField = "id"
        ddlApp.DataBind()

    End Sub
    Sub dropdownVer()
        sSQL = "select * from iPx_general_ver where appid='" & ddlApp.SelectedValue & "' "
        If txtVer.Text = "" Then
            sSQL += " "
        Else
            sSQL += "and version='" & txtVer.Text & "' "
        End If
        sSQL += "and  Active='Y' order by id desc "
        Dim cmd As New SqlCommand(sSQL)
        ddlver.DataSource = ExecuteQuery(cmd, "SELECT")
        ddlver.DataTextField = "version"
        ddlver.DataValueField = "id"
        ddlver.DataBind()
    End Sub

    Sub dropdownVerActive()
        sSQL = "select * from iPx_general_ver where appid='" & ddlApp.SelectedValue & "' and  Active='Y' and id = (select MAX(iPx_general_verDTL.verid) from iPx_general_verDTL where iPx_general_verDTL .active='Y') order by id desc "
        Dim cmd As New SqlCommand(sSQL)
        ddlver.DataSource = ExecuteQuery(cmd, "SELECT")
        ddlver.DataTextField = "version"
        ddlver.DataValueField = "id"
        ddlver.DataBind()
    End Sub
    Sub dropdownMDL()
        sSQL = "select * from iPx_general_verMDL where appid='" & ddlApp.SelectedValue & "' and  Active='Y'  "
        Dim cmd As New SqlCommand(sSQL)
        ddlmodule.DataSource = ExecuteQuery(cmd, "SELECT")
        ddlmodule.DataTextField = "description"
        ddlmodule.DataValueField = "id"
        ddlmodule.DataBind()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sEmailClient") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Page.IsPostBack Then
            dropdownAPP()

            ddlApp.SelectedValue = Session("sCode")
            dropdownMDL()
            dropdownVerActive()
            grid()
            Session("sNewRelease") = True

        End If
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        GridView1.PageIndex = e.NewPageIndex
        Me.grid()
    End Sub

    Protected Sub gvTicket_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        grid()
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        GridView1.PageIndex = e.NewPageIndex
        Me.grid()
    End Sub

    Protected Sub ddlmodule_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlApp.SelectedIndexChanged
        dropdownVer()
        dropdownMDL()
    End Sub

    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsave.Click
        additem()
    End Sub
    Sub additem()
        oCnct.Close()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select * FROM iPx_general_verDTL where appid='" & ddlApp.SelectedValue & "' and  verid='" & ddlver.SelectedValue & "' and moduleid='" & ddlmodule.SelectedValue & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then

            If Session("sNewRelease") = True Then
                saveitem()
                Session("sNewRelease") = False
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Data has been save !');document.getElementById('Buttonx').click()", True)
            Else
                updatedata()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Update sucessfull !');document.getElementById('Buttonx').click()", True)

            End If

        Else
            If Session("sNewRelease") = True Then
                saveitem()
                Session("sNewRelease") = False
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Data has been save !');document.getElementById('Buttonx').click()", True)
            Else

                saveitem()

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Data has been save !');document.getElementById('Buttonx').click()", True)

            End If
        End If

        grid()
        oCnct.Close()
        txtNote.Text = ""
        Session("sNewRelease") = True
    End Sub
    Sub saveitem()
        If txtNote.Text <> "" Then


            oSQLReader.Close()
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If

            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "INSERT INTO iPx_general_verDTL( appid,date,verid, moduleid, note, active) "
            sSQL = sSQL & "VALUES ( '" & ddlApp.SelectedValue & "','" & Date.Today & "','" & ddlver.SelectedValue & "', '" & ddlmodule.Text & "', '" & txtNote.Text & "', 'Y') "
            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()

            oCnct.Close()
        End If
    End Sub

    Sub updatedata()
        oSQLReader.Close()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If


        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim chk As String
        If chkActive.Checked = True Then
            chk = "Y"
        Else
            chk = "N"
        End If
        sSQL = "UPDATE  iPx_general_verDTL SET date='" & Date.Today & "',  note = '" & txtNote.Text & "' ,active='" & chk & "' where moduleid='" & ddlmodule.SelectedValue & "' and verid ='" & ddlver.SelectedValue & "' and id='" & Session("sIdRelease") & "'"


        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        dropdownVerActive()
    End Sub

    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" & e.Row.RowIndex)
            e.Row.ToolTip = "Click to select this row."
        End If
    End Sub
    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles GridView1.SelectedIndexChanged

        For Each row As GridViewRow In GridView1.Rows

            If row.RowIndex = GridView1.SelectedIndex Then
                row.BackColor = ColorTranslator.FromHtml("#A1DCF2")
                row.ToolTip = String.Empty
            Else
                row.BackColor = ColorTranslator.FromHtml("#FFFFFF")
                row.ToolTip = "Click to select this row."
            End If
        Next

    End Sub
    Public Sub grid()
        oCnct.Open()

        sSQL = "select iPx_general_verDTL.id, iPx_general_verGRP.app as appid,iPx_general_ver.version as verid,iPx_general_ver.description  ,iPx_general_verMDL.description as moduleid,iPx_general_verDTL.note,iPx_general_verDTL.active from iPx_general_verDTL "
        sSQL = sSQL & "inner join iPx_general_verGRP on iPx_general_verDTL.appid=iPx_general_verGRP.id "
        sSQL = sSQL & "inner join iPx_general_ver on iPx_general_verDTL.verid=iPx_general_ver.id "
        sSQL = sSQL & "inner join iPx_general_verMDL on iPx_general_verDTL.moduleid=iPx_general_verMDL.id "

        sSQL = sSQL & " where iPx_general_verDTL.appid='" & ddlApp.SelectedValue & "' order by iPx_general_ver.id desc "

        Dim cmd As SqlDataAdapter = New SqlDataAdapter(sSQL, oCnct)
        Dim dt As DataTable = New DataTable()
        cmd.Fill(dt)

        GridView1.DataSource = dt
        GridView1.DataBind()

        oCnct.Close()

    End Sub
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "getcode" Then
            Session("sNewRelease") = False
            Dim cod As String
            cod = e.CommandArgument.ToString
            Session("sIdRelease") = e.CommandArgument.ToString

            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            txtVer.Text = ""
            dropdownVer()
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "select * FROM iPx_general_verDTL where id='" & cod & "' "
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader
            txtNote.Text = ""
            If oSQLReader.Read Then
                txtNote.Text = oSQLReader.Item("note")
                ddlver.SelectedValue = oSQLReader.Item("verid")
                ddlmodule.SelectedValue = oSQLReader.Item("moduleid")
                If oSQLReader.Item("active") = "Y" Then
                    chkActive.Checked = True
                Else
                    chkActive.Checked = False
                End If
            End If

            oCnct.Close()
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('" & cod & "');document.getElementById('Buttonx').click()", True)
            'ElseIf e.CommandName = "gettrash" Then
            '    Dim cod As String
            '    cod = e.CommandArgument.ToString
            '    If oCnct.State = ConnectionState.Closed Then
            '        oCnct.Open()
            '    End If
            '    oSQLCmd = New SqlCommand(sSQL, oCnct)

            '    sSQL = "UPDATE  iPx_general_verDTL SET active='" & "N" & "' where id='" & cod & "'"


            '    oSQLCmd.CommandText = sSQL
            '    oSQLCmd.ExecuteNonQuery()

            '    oCnct.Close()
            '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Data has been delete !');document.getElementById('Buttonx').click()", True)
            '    grid()
        End If
    End Sub
    Protected Sub btnaddver_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddver.Click
        pnlVersion.Visible = False
        pnlVersionadd.Visible = True
        If ddlver.Text <> "" Then
            txtVer.Text = ddlver.SelectedItem.Text
        End If

    End Sub

    Protected Sub btnsaveVer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsaveVer.Click
        oCnct.Close()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select * FROM iPx_general_ver where appid='" & ddlApp.SelectedValue & "' and  version='" & txtVer.Text & "' and description = '" & txtDesc.Text & "' and active='Y' "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            pnlVersion.Visible = True
            pnlVersionadd.Visible = False
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Version Already Exist !');document.getElementById('Buttonx').click()", True)
            dropdownVer()
        Else
            savever()
        End If

        oCnct.Close()
    End Sub
    Sub savever()
        If txtVer.Text <> "" Then
            oSQLReader.Close()
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If

            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "INSERT INTO  iPx_general_ver ( appid, version, description, active) "
            sSQL = sSQL & "VALUES ( '" & ddlApp.SelectedValue & "', '" & txtVer.Text & "', '" & txtDesc.Text & "', 'Y') "
            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()

            oCnct.Close()
            pnlVersion.Visible = True
            pnlVersionadd.Visible = False
            dropdownVer()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Data has been save !');document.getElementById('Buttonx').click()", True)
        Else
            txtModule.Focus()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please fill data !');document.getElementById('Buttonx').click()", True)

        End If
        txtVer.Text = ""
    End Sub

    Sub shownote()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select * FROM iPx_general_verDTL where appid='" & ddlApp.SelectedValue & "' and  verid='" & ddlver.SelectedValue & "' and  moduleid='" & ddlmodule.Text & "' "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        txtNote.Text = ""
        If oSQLReader.Read Then
            txtNote.Text = oSQLReader.Item("note")
            If oSQLReader.Item("active") = "Y" Then
                chkActive.Checked = True
            Else
                chkActive.Checked = False
            End If
        End If

        oCnct.Close()
    End Sub

    Protected Sub btnCxld_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCxld.Click
        Response.Redirect("releasenoteversion.aspx")
    End Sub

    Protected Sub ddlmodule_SelectedIndexChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlmodule.SelectedIndexChanged
        txtNote.Text = ""
        Session("sNewRelease") = True

    End Sub

    Protected Sub brnAddModule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles brnAddModule.Click
        pnlModule.Visible = False
        pnlAddModule.Visible = True

    End Sub

    Protected Sub btnSaveModule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveModule.Click
        oCnct.Close()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select * FROM   iPx_general_verMDL where description='" & txtModule.Text & "'  "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            pnlModule.Visible = True
            pnlAddModule.Visible = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Version Already Exist !');document.getElementById('Buttonx').click()", True)

        Else

            saveModule()
        End If

        oCnct.Close()
    End Sub

    Sub saveModule()
        If txtModule.Text <> "" Then


            oSQLReader.Close()
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If

            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "INSERT INTO  iPx_general_verMDL (appid, description, active) "
            sSQL = sSQL & "VALUES ('" & ddlApp.SelectedValue & "', '" & txtModule.Text & "', 'Y') "
            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()

            oCnct.Close()
            pnlModule.Visible = True
            pnlAddModule.Visible = False
            dropdownMDL()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Data has been save !');document.getElementById('Buttonx').click()", True)
        Else
            txtModule.Focus()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please fill data !');document.getElementById('Buttonx').click()", True)

        End If
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        pnlVersion.Visible = True
        pnlVersionadd.Visible = False
        pnlModule.Visible = True
        pnlAddModule.Visible = False
        txtNote.Text = ""
        Session("sNewRelease") = True
    End Sub
End Class
