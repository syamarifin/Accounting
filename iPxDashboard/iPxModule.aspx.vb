Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxDashboard_iPxModule
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, active As String
    Dim cIpx As New iPxClass
    Sub listModule()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_profile_client_useraccess_mdl as a where active='Y' "
        sSQL += " order by id asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvModuleAcc.DataSource = dt
                    gvModuleAcc.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvModuleAcc.DataSource = dt
                    gvModuleAcc.DataBind()
                    gvModuleAcc.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listFunction()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.*, (b.description) as moduleDesc from iPxAcct_profile_client_useraccess_fn as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_mdl as b ON b.id=a.moduleid "
        sSQL += "where moduleid='" & Session("moduleid") & "' and a.active='Y'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvFuncAcc.DataSource = dt
                    gvFuncAcc.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvFuncAcc.DataSource = dt
                    gvFuncAcc.DataBind()
                    gvFuncAcc.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub tampilModuleText()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_profile_client_useraccess_mdl where id='" & Session("moduleid") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbModuleDesc.Text = oSQLReader.Item("description").ToString
        Else

        End If
        oCnct.Close()
    End Sub
    Sub saveFunction()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        If cbActiveFuct.Checked = True Then
            active = "Y"
        ElseIf cbActiveFuct.Checked = False Then
            active = "N"
        End If
        sSQL = "INSERT INTO iPxAcct_profile_client_useraccess_fn(moduleid,description,active) "
        sSQL += "VALUES ('" & Session("moduleid") & "','" & Replace(tbFunction.Text, "'", "''") & "','" & active & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub EditFunctino()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.*, (b.description) as moduleDesc from iPxAcct_profile_client_useraccess_fn as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_mdl as b ON b.id=a.moduleid "
        sSQL += "where moduleid='" & Session("moduleid") & "' and a.id='" & Session("idFunct") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbModuleDesc.Text = oSQLReader.Item("moduleDesc").ToString
            tbFunction.Text = oSQLReader.Item("description").ToString
            active = oSQLReader.Item("active").ToString
            If active = "Y" Then
                cbActiveFuct.Checked = True
            ElseIf active = "N" Then
                cbActiveFuct.Checked = False
            End If
        Else

        End If
        oCnct.Close()
    End Sub
    Sub updateFunction()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        If cbActiveFuct.Checked = True Then
            active = "Y"
        ElseIf cbActiveFuct.Checked = False Then
            active = "N"
        End If
        sSQL = "UPDATE iPxAcct_profile_client_useraccess_fn SET description='" & Replace(tbFunction.Text, "'", "''") & "',active='" & active & "' "
        sSQL += "WHERE id ='" & Session("idFunct") & "' "

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub saveModule()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        If cbActiveModule.Checked = True Then
            active = "Y"
        ElseIf cbActiveModule.Checked = False Then
            active = "N"
        End If
        sSQL = "INSERT INTO iPxAcct_profile_client_useraccess_mdl(description,active) "
        sSQL += "VALUES ('" & Replace(tbModule.Text, "'", "''") & "','" & active & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub EditModule()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select * from iPxAcct_profile_client_useraccess_mdl "
        sSQL += "where id='" & Session("moduleEdit") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbModule.Text = oSQLReader.Item("description").ToString
            active = oSQLReader.Item("active").ToString
            If active = "Y" Then
                cbActiveModule.Checked = True
            ElseIf active = "N" Then
                cbActiveModule.Checked = False
            End If
        Else

        End If
        oCnct.Close()
    End Sub
    Sub updateModule()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        If cbActiveModule.Checked = True Then
            active = "Y"
        ElseIf cbActiveModule.Checked = False Then
            active = "N"
        End If
        sSQL = "UPDATE iPxAcct_profile_client_useraccess_mdl SET description='" & Replace(tbModule.Text, "'", "''") & "',active='" & active & "' "
        sSQL += "WHERE id ='" & Session("moduleEdit") & "' "

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Session("moduleid") = ""
            listModule()
            listFunction()
            lbAddFunct.Enabled = False
        End If
    End Sub

    Protected Sub gvModuleAcc_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvModuleAcc.RowCommand
        If e.CommandName = "getDetail" Then
            Session("moduleid") = e.CommandArgument
            listFunction()
            lbAddFunct.Enabled = True
        ElseIf e.CommandName = "getEdit" Then
            Session("moduleEdit") = e.CommandArgument
            EditModule()
            lbSaveModule.Text = "<i class='fa fa-save'></i> Update"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddModule", "showModalAddModule()", True)
        End If
    End Sub

    Protected Sub lbAddFunct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddFunct.Click
        tampilModuleText()
        tbFunction.Text = ""
        cbActiveFuct.Checked = True
        lbSaveFunct.Text = "<i class='fa fa-save'></i> Save"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddFunct", "showModalAddFunct()", True)
    End Sub

    Protected Sub lbAbortAddFunct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortAddFunct.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddFunct", "hideModalAddFunct()", True)
    End Sub

    Protected Sub gvFuncAcc_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvFuncAcc.RowCommand
        If e.CommandName = "getEdit" Then
            Session("idFunct") = e.CommandArgument
            EditFunctino()
            lbSaveFunct.Text = "<i class='fa fa-save'></i> Update"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddFunct", "showModalAddFunct()", True)
        End If
    End Sub

    Protected Sub lbSaveFunct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSaveFunct.Click
        If tbFunction.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Function first !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddFunct", "hideModalAddFunct()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddFunct", "showModalAddFunct()", True)
        Else
            If lbSaveFunct.Text = "<i class='fa fa-save'></i> Save" Then
                saveFunction()
                listFunction()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddFunct", "hideModalAddFunct()", True)
            ElseIf lbSaveFunct.Text = "<i class='fa fa-save'></i> Update" Then
                updateFunction()
                listFunction()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddFunct", "hideModalAddFunct()", True)
            End If
        End If
    End Sub

    Protected Sub lbAddModule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddModule.Click
        tbModule.Text = ""
        cbActiveModule.Checked = True
        lbSaveModule.Text = "<i class='fa fa-save'></i> Save"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddModule", "showModalAddModule()", True)
    End Sub

    Protected Sub lbSaveModule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSaveModule.Click
        If tbModule.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter module first !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddModule", "hideModalAddModule()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddModule", "showModalAddModule()", True)
        Else
            If lbSaveModule.Text = "<i class='fa fa-save'></i> Save" Then
                saveModule()
                listModule()
                listFunction()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddModule", "hideModalAddModule()", True)
            ElseIf lbSaveModule.Text = "<i class='fa fa-save'></i> Update" Then
                updateModule()
                listModule()
                listFunction()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddModule", "hideModalAddModule()", True)
            End If
        End If
    End Sub

    Protected Sub lbAbortModule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortModule.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddModule", "hideModalAddModule()", True)
    End Sub
End Class
