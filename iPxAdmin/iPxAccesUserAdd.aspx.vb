Imports System.Data
Imports System.Data.SqlClient

Partial Class iPxAdmin_iPxAccesUserAdd
    Inherits System.Web.UI.Page
    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Public oCnct As SqlConnection = New SqlConnection(sCnct)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL, activeStatus As String

    Sub ListUserAccesGroup()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select grp from iPxAcct_profile_client_useraccess_mdl group by grp "
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvGrp.DataSource = dt
                    gvGrp.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvGrp.DataSource = dt
                    gvGrp.DataBind()
                    gvGrp.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub

    Sub ListUserAccesModule()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*,(SELECT 'Y' FROM iPxAcct_profile_client_useraccess where "
        sSQL += "businessid='" & Session("sBusinessID") & "' and usercode='" & Session("sCode") & "' and "
        sSQL += "moduleid=a.id and active='Y') as access FROM iPxAcct_profile_client_useraccess_mdl as a where a.active='Y' and grp ='" & Session("sGrpAcces") & "' "
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

    Sub ListFunction()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim s As String = Len(Session("sModuleFunc"))
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * ,(SELECT 'Y' FROM iPxAcct_profile_client_useraccess_dtl where "
        sSQL += "businessid='" & Session("sBusinessID") & "' and usercode='' and moduleid=a.moduleid and funtionid=a.id and active='Y') as access "
        sSQL += "FROM iPxAcct_profile_client_useraccess_fn as a where moduleid = '' and active='Y' "
        sSQL += " order by moduleid,id asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvFunctAcc.DataSource = dt
                    gvFunctAcc.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvFunctAcc.DataSource = dt
                    gvFunctAcc.DataBind()
                    gvFunctAcc.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListUserAccesFunctionAdmin()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * ,(SELECT 'Y' FROM iPxAcct_profile_client_useraccess_dtl where "
        sSQL += "businessid='" & Session("sBusinessID") & "' and usercode='" & Session("sCode") & "' and moduleid=a.moduleid and funtionid=a.id and active='Y') as access "
        sSQL += "FROM iPxAcct_profile_client_useraccess_fn as a where moduleid ='" & Session("sModAcces") & "' and a.active='Y' "
        sSQL += " order by moduleid,id asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvFunctAcc.DataSource = dt
                    gvFunctAcc.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvFunctAcc.DataSource = dt
                    gvFunctAcc.DataBind()
                    'gvFunctAcc.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListUserAccesFunction()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        'Dim s As String = Len(Session("sModuleFunc"))
        'Dim r As String = Mid(Session("sModuleFunc"), 1, s - 1)
        Dim r As String = Session("sModAcces")
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * ,(SELECT 'Y' FROM iPxAcct_profile_client_useraccess_dtl where "
        sSQL += "businessid='" & Session("sBusinessID") & "' and usercode='" & Session("sCode") & "' and moduleid=a.moduleid and funtionid=a.id and active='Y') as access "
        sSQL += "FROM iPxAcct_profile_client_useraccess_fn as a where moduleid in (" & r & ") and a.active='Y' "
        sSQL += " order by moduleid,id asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvFunctAcc.DataSource = dt
                    gvFunctAcc.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvFunctAcc.DataSource = dt
                    gvFunctAcc.DataBind()
                    gvFunctAcc.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub

    Sub cek_username()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT username FROM  iPx_profile_client_userid WHERE businessid='" & Session("sBusinessID") & "' and username = '" & tbusercode.Text & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Username already exist!');", True)
            tbusercode.Focus()
            tbusercode.Text = ""
        End If
        oCnct.Close()
    End Sub
    Sub getData()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_client_userid where businessid='" & Session("sBusinessID") & "' and usercode='" & Session("sCode").ToString.Trim & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        tbusercode.ReadOnly = True
        While oSQLReader.Read
            Dim lock As String
            tbusercode.Text = oSQLReader.Item("usercode").ToString.Trim
            tbfullname.Text = oSQLReader.Item("username").ToString.Trim
            dlGroup.SelectedValue = oSQLReader.Item("usergroup").ToString.Trim
            'lock = oSQLReader.Item("islocked")
            'If lock = "Y" Then
            '    chkStatus.Checked = True
            'Else
            '    chkStatus.Checked = False
            'End If

        End While

        oCnct.Close()
    End Sub
    Sub savedata()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim status As String
        'If chkStatus.Checked Then
        '    status = "Y"
        'Else
        '    status = "N"
        'End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO  iPx_profile_client_userid (businessid, usergroup, userid, usercode, password, username, neverexpired, registereddate, expiredafter, islocked, userimage) "
        sSQL = sSQL & "VALUES ('" & Session("sBusinessID") & "', '" & dlGroup.SelectedValue & "','" & Session("iUserID") & "','" & tbusercode.Text & "','" & tbpass.Text & "','" & tbfullname.Text & "','Y','" & Date.Today & "', '', '" & status & "', '') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

    End Sub
    Sub getAccess()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*,(SELECT 'Y' FROM iPxAcct_profile_client_useraccess where "
        sSQL += "businessid='" & Session("sBusinessID") & "' and usercode='" & Session("sCode").ToString.Trim & "' and "
        sSQL += "moduleid=a.id and active='Y') as access FROM iPxAcct_profile_client_useraccess_mdl as a "
        sSQL += "where a.active='Y' and grp ='" & Session("sGrpAcces") & "' order by id asc"
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
                    'gvModuleAcc.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub saveUserID()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim register As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        sSQL = "INSERT INTO iPx_profile_client_userid(businessid,usergroup,userid,usercode,password,username,neverexpired,registereddate,expiredafter,islocked,userimage) "
        sSQL = sSQL & "VALUES ('" & Session("sBusinessID") & "','" & dlGroup.SelectedValue & "','" & Session("iUserID") & "','" & tbusercode.Text.Trim & "','" & tbpass.Text.Trim & "','" & tbfullname.Text.Trim & "','Y','" & register & "','1900-01-01','N','')"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub saveModuleAcc()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "INSERT INTO iPxAcct_profile_client_useraccess (businessid, usercode, moduleid, active) "
        sSQL = sSQL & "VALUES ('" & Session("sBusinessID") & "','" & tbusercode.Text.Trim & "','" & Session("sModuleID") & "','Y')"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub deleteModuleAcc()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcct_profile_client_useraccess SET active='" & activeStatus & "' where businessid='" & Session("sBusinessID") & "' and usercode='" & tbusercode.Text.Trim & "' and moduleid='" & Session("sModuleID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

    End Sub
    Sub updatedata()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        If PnlPassword.Visible = True Then 'reset pass
            sSQL = "UPDATE iPx_profile_client_userid SET usergroup='" & dlGroup.SelectedValue & "', password='" & tbpass.Text & "', username= '" & tbfullname.Text & "' where businessid='" & Session("sBusinessID") & "' and usercode='" & tbusercode.Text.Trim & "'"
        End If
        If PnlPassword.Visible = False Then 'not reset pass
            sSQL = "UPDATE iPx_profile_client_userid SET usergroup='" & dlGroup.SelectedValue & "', username= '" & tbfullname.Text & "' where businessid='" & Session("sBusinessID") & "' and usercode='" & tbusercode.Text.Trim & "'"
        End If
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

    End Sub
    Sub cekAcces()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT id FROM iPxAcct_profile_client_useraccess WHERE businessid='" & Session("sBusinessID") & "' and usercode='" & tbusercode.Text.Trim & "' and moduleid='" & Session("sModuleID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            'Update
            oCnct.Close()
            deleteModuleAcc()
        Else
            'Save
            oCnct.Close()
            saveModuleAcc()
        End If
    End Sub
    Sub cekFunction()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT id FROM iPxAcct_profile_client_useraccess_fn WHERE moduleid='" & Session("sModuleID") & "' and id='" & Session("sFunctionID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            'Update
            oCnct.Close()
            cekAccesFunction()
        Else
            'Save
            oCnct.Close()
        End If
    End Sub
    Sub cekAccesFunction()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT id FROM iPxAcct_profile_client_useraccess_dtl WHERE businessid='" & Session("sBusinessID") & "' and usercode='" & tbusercode.Text.Trim & "' and moduleid='" & Session("sModuleID") & "' and funtionid='" & Session("sFunctionID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            'Update
            oCnct.Close()
            updateFunctionAcc()
        Else
            'Save
            oCnct.Close()
            saveFunctionAcc()
        End If
    End Sub
    Sub saveFunctionAcc()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "INSERT INTO iPxAcct_profile_client_useraccess_dtl (businessid,usercode,moduleid,funtionid,active) "
        sSQL = sSQL & "VALUES ('" & Session("sBusinessID") & "','" & tbusercode.Text.Trim & "','" & Session("sModuleID") & "','" & Session("sFunctionID") & "','Y')"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub updateFunctionAcc()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcct_profile_client_useraccess_dtl SET active='" & activeStatus & "' "
        sSQL += "where businessid='" & Session("sBusinessID") & "' and usercode='" & tbusercode.Text.Trim & "' "
        sSQL += "and moduleid='" & Session("sModuleID") & "' "
        If Session("sFunctionID") <> "&nbsp;" Then
            sSQL += " and funtionid='" & Session("sFunctionID") & "'"
        End If

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

    End Sub
    Sub deleteFunctionModuleAcc()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcct_profile_client_useraccess_dtl SET active='" & activeStatus & "' "
        sSQL += "where businessid='" & Session("sBusinessID") & "' and usercode='" & tbusercode.Text.Trim & "' "
        sSQL += "and moduleid='" & Session("sModuleID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

    End Sub
    Sub UserGroup()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_client_usergroup"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlGroup.DataSource = dt
                dlGroup.DataTextField = "Description"
                dlGroup.DataValueField = "usergroup"
                dlGroup.DataBind()
                dlGroup.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub listUserAcces()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT b.description,a.*, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='35'and x.active='Y' and x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "') as EditOp "
        sSQL += "FROM iPx_profile_client_userid as a "
        sSQL += "INNER JOIN iPx_profile_client_usergroup as b ON b.usergroup=a.usergroup "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & "  "
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.userid asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvUserAcces.DataSource = dt
                    gvUserAcces.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvUserAcces.DataSource = dt
                    gvUserAcces.DataBind()
                    gvUserAcces.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub SaveCopyAccesModule()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO iPxAcct_profile_client_useraccess (businessid, usercode, moduleid, active) "
        sSQL += "SELECT businessid, usercode, moduleid, active FROM iPxAcct_profile_client_useraccess WHERE businessid='" & Session("sBusinessID") & "' and usercode ='" & Session("sAccesSelect") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub SaveCopyAccesFunction()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO iPxAcct_profile_client_useraccess_dtl (businessid,usercode,moduleid,funtionid,active)  "
        sSQL += "SELECT businessid,usercode,moduleid,funtionid,active FROM iPxAcct_profile_client_useraccess_dtl WHERE businessid='" & Session("sBusinessID") & "' and usercode ='" & Session("sAccesSelect") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cph As ContentPlaceHolder = DirectCast(Me.Master.FindControl("ContentPlaceHolder4"), ContentPlaceHolder)
        Dim label As Label = DirectCast(cph.FindControl("lblMasterTitle"), Label)
        label.Text = "User Access"

        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Page.IsPostBack Then
            lbFunction.Text = "<span class='glyphicon glyphicon-list-alt'></span> Function"
            If Session("sNewCode") = True Then
                UserGroup()
                tbusercode.Text = ""
                Session("sCode") = ""
                tbfullname.Text = ""
                ListUserAccesGroup()
                Session("sGrpAcces") = "Account Payable"
                ListUserAccesModule()
                ListFunction()
                'gvFunctAcc.Enabled = False
                tbusercode.Enabled = True
            Else
                tbusercode.Text = Session("sCode")
                ListUserAccesGroup()
                Session("sGrpAcces") = "Account Payable"
                UserGroup()
                getData()
                getAccess()
                Session("sModAcces") = "14"
                'ListUserAccesModule()
                'gvFunctAcc.Enabled = False
                PnlPassword.Visible = False
                tbusercode.Enabled = False
                btnchange.Visible = True
                If tbusercode.Enabled = False And Session("sUserCode") = tbusercode.Text And Session("sUserCode") = "ADMIN" Then
                    'gvModuleAcc.Enabled = False
                    ListUserAccesFunctionAdmin()
                Else
                    ListFunction()
                End If
            End If
        End If
    End Sub

    'Protected Sub Linkbutton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Linkbutton1.Click
    '    If oCnct.State = ConnectionState.Closed Then
    '        oCnct.Open()
    '    End If
    '    oSQLCmd = New SqlCommand(sSQL, oCnct)
    '    sSQL = "SELECT username FROM  iPx_profile_client_userid WHERE businessid='" & Session("sBusinessID") & "' and username = '" & Session("sCode").ToString.Trim & "'"
    '    oSQLCmd.CommandText = sSQL
    '    oSQLReader = oSQLCmd.ExecuteReader

    '    If oSQLReader.Read Then 'UPDATE
    '        oSQLReader.Close()
    '        If tbusercode.Text = "" Then
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please fill Username!');", True)
    '            'ElseIf (chkConfig.Checked = False) And (chkEvent.Checked = False) And (chkMember.Checked = False) And (chkRenew.Checked = False) And (chkReport.Checked) And (chkTrans.Checked = False) Then
    '            '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please check one or more accesses!');", True)
    '        Else
    '            Session("sMessage") = "Data has been update !| ||"
    '            Session("sWarningID") = "0"
    '            Session("sUrlOKONLY") = "viewUserAcc.aspx"
    '            Session("sUrlYES") = "http://www.thepyxis.net"
    '            Session("sUrlNO") = "http://www.thepyxis.net"
    '            Response.Redirect("warningmsg.aspx")
    '        End If

    '    Else
    '        oSQLReader.Close()
    '        If tbusercode.Text = "" Then 'SAVE
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please fill Username!');", True)
    '        ElseIf tbpass.Text = "" Then
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please fill Password!');", True)
    '        Else
    '            savedata()
    '            Session("sMessage") = "Data has been saved !| ||"
    '            Session("sWarningID") = "0"
    '            Session("sUrlOKONLY") = "viewUserAcc.aspx"
    '            Session("sUrlYES") = "http://www.thepyxis.net"
    '            Session("sUrlNO") = "http://www.thepyxis.net"
    '            Response.Redirect("warningmsg.aspx")
    '        End If

    '    End If
    '    oCnct.Close()

    'End Sub



    Protected Sub btnview_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnview.Click
        Response.Redirect("iPxAccesUser.aspx")
    End Sub

    Protected Sub btnCxld_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCxld.Click
        Response.Redirect("iPxAccesUser.aspx")
    End Sub

    Protected Sub txtusercode_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tbusercode.TextChanged
        cek_username()
    End Sub

    Protected Sub btnchange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnchange.Click
        If btnchange.Text = "Reset Password" Then
            PnlPassword.Visible = True
            btnchange.Text = "Cancel"
        Else
            PnlPassword.Visible = False
            btnchange.Text = "Reset Password"
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If PnlPassword.Visible = True Then
            If tbpass.Text = "" Or Len(tbpass.Text) < 6 Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please enter your password at least 6 characters !');document.getElementById('Buttonx').click()", True)
            ElseIf tbconf_pass.Text = "" Or Len(tbpass.Text) < 6 Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('please enter your password confirm at least 6 characters !');document.getElementById('Buttonx').click()", True)
            ElseIf tbconf_pass.Text <> tbpass.Text Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Passwords do not match!');document.getElementById('Buttonx').click()", True)
            End If
        End If
        If tbusercode.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('please enter your username !');document.getElementById('Buttonx').click()", True)
        Else

            If Session("sNewCode") = False Then
                'Update
                updatedata()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('user acces setting successfully !');document.getElementById('Buttonx').click()", True)

            ElseIf Session("sNewCode") = True Then
                'Save
                saveUserID()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('module acces setting successfully !');document.getElementById('Buttonx').click()", True)
                getData()
                PnlPassword.Visible = False
            End If
            Session("sStatusMod") = "SaveModule"
            Session("sStatus") = "Save"
        End If
    End Sub

    Protected Sub lbFunction_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFunction.Click
        If lbFunction.Text = "<span class='glyphicon glyphicon-list-alt'></span> Function" Then
            gvModuleAcc.Enabled = False
            Session("sModuleFunc") = ""
            Dim cb2 As CheckBox = gvModuleAcc.HeaderRow.FindControl("chkAll")
            Dim cbterpilih As Boolean = False
            Dim cb As CheckBox = Nothing
            Dim n As Integer = 0
            Dim m As Integer = 0
            Do Until n = gvModuleAcc.Rows.Count
                cb = gvModuleAcc.Rows.Item(n).FindControl("chk")
                If cb IsNot Nothing AndAlso cb.Checked Then
                    cbterpilih = True
                    'insert
                    Session("sModuleFunc") += (gvModuleAcc.Rows(n).Cells(1).Text) + ","
                End If
                n += 1
            Loop
            ListUserAccesFunction()
            gvFunctAcc.Enabled = True
            lbFunction.Text = "<span class='glyphicon glyphicon-list-alt'></span> Module"
            If tbusercode.Enabled = False And Session("sUserCode") = tbusercode.Text And Session("sUserCode") = "ADMIN" Then
                gvFunctAcc.Enabled = False
            End If
        ElseIf lbFunction.Text = "<span class='glyphicon glyphicon-list-alt'></span> Module" Then
            gvModuleAcc.Enabled = True
            gvFunctAcc.Enabled = False
            lbFunction.Text = "<span class='glyphicon glyphicon-list-alt'></span> Function"
        End If
    End Sub

    Protected Sub gvGrp_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGrp.RowCommand
        If e.CommandName = "getSelect" Then
            'If Session("sStatus") = "unSave" Then
            '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Access not saved yet, Sure you moved group ?');document.getElementById('Buttonx').click()", True)
            'Else
            Session("sGrpAcces") = e.CommandArgument
            'Session("sStatus") = "unSave"
            ListUserAccesModule()
            'End If
        End If
    End Sub

    Protected Sub gvModuleAcc_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvModuleAcc.RowCommand
        If e.CommandName = "getSelect" Then
            'If Session("sStatusMod") = "unSaveModule" Then
            '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Access not saved yet, Sure you moved group ?');document.getElementById('Buttonx').click()", True)
            'Else
            Session("sModAcces") = e.CommandArgument
            '    Session("sStatusMod") = "unSaveModule"
            ListUserAccesFunction()
            'End If
        End If
    End Sub

    Protected Sub btnCopyAcces_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopyAcces.Click
        listUserAcces()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCOA", "showModalCOA()", True)
    End Sub

    Protected Sub lbCloseCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCloseCopy.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCOA", "hideModalCOA()", True)
    End Sub

    Protected Sub lbAddAcces_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddAcces.Click
        If Session("sNewCode") = False Then
            'Update
            'updatedata()
            If gvModuleAcc.Enabled = True Or gvFunctAcc.Enabled = True Then
                Dim cb2 As CheckBox = gvModuleAcc.HeaderRow.FindControl("chkAll")
                Dim cbterpilih As Boolean = False
                Dim cb As CheckBox = Nothing
                Dim n As Integer = 0
                Dim m As Integer = 0
                Do Until n = gvModuleAcc.Rows.Count
                    cb = gvModuleAcc.Rows.Item(n).FindControl("chk")
                    If cb IsNot Nothing AndAlso cb.Checked Then
                        cbterpilih = True
                        activeStatus = "Y"
                        'insert & update
                        Session("sModuleID") = (gvModuleAcc.Rows(n).Cells(1).Text)
                        cekAcces()
                        'function
                        Dim cbF As CheckBox = gvFunctAcc.HeaderRow.FindControl("chkAll")
                        Dim cbterpilihF As Boolean = False
                        Dim cbFd As CheckBox = Nothing
                        Dim nF As Integer = 0
                        Do Until nF = gvFunctAcc.Rows.Count
                            cbF = gvFunctAcc.Rows.Item(nF).FindControl("chk")
                            If cbF IsNot Nothing AndAlso cb.Checked Then
                                cbterpilihF = True
                                'insert & update
                                m = m + 1
                            End If
                            nF += 1
                        Loop
                        If m > 0 Then
                            nF = 0
                            Do Until nF = gvFunctAcc.Rows.Count
                                cbFd = gvFunctAcc.Rows.Item(nF).FindControl("chk")
                                If cbFd IsNot Nothing AndAlso cbFd.Checked Then
                                    cbterpilihF = True
                                    activeStatus = "Y"
                                    'insert & update
                                    Session("sFunctionID") = (gvFunctAcc.Rows(nF).Cells(1).Text)
                                    cekFunction()
                                Else
                                    Session("sFunctionID") = (gvFunctAcc.Rows(nF).Cells(1).Text)
                                    activeStatus = "N"
                                    updateFunctionAcc()
                                End If
                                nF += 1
                            Loop
                        Else
                            deleteFunctionModuleAcc()
                        End If
                    Else
                        Session("sModuleID") = (gvModuleAcc.Rows(n).Cells(1).Text)
                        activeStatus = "N"
                        deleteModuleAcc()
                        deleteFunctionModuleAcc()
                    End If
                    n += 1
                Loop
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('user acces setting successfully !');document.getElementById('Buttonx').click()", True)

        ElseIf Session("sNewCode") = True Then
            'Save
            'saveUserID()
            Dim cb2 As CheckBox = gvModuleAcc.HeaderRow.FindControl("chkAll")
            Dim cbterpilih As Boolean = False
            Dim cb As CheckBox = Nothing
            Dim n As Integer = 0
            Dim m As Integer = 0
            Do Until n = gvModuleAcc.Rows.Count
                cb = gvModuleAcc.Rows.Item(n).FindControl("chk")
                If cb IsNot Nothing AndAlso cb.Checked Then
                    cbterpilih = True
                    'insert
                    Session("sModuleID") = (gvModuleAcc.Rows(n).Cells(1).Text)
                    saveModuleAcc()
                    Dim cbF As CheckBox = gvFunctAcc.HeaderRow.FindControl("chkAll")
                    Dim cbterpilihF As Boolean = False
                    Dim cbFd As CheckBox = Nothing
                    Dim nF As Integer = 0
                    Do Until nF = gvFunctAcc.Rows.Count
                        cbF = gvFunctAcc.Rows.Item(nF).FindControl("chk")
                        If cbF IsNot Nothing AndAlso cb.Checked Then
                            cbterpilihF = True
                            'insert & update
                            m = m + 1
                        End If
                        nF += 1
                    Loop
                    If m > 0 Then
                        nF = 0
                        Do Until nF = gvFunctAcc.Rows.Count
                            cbFd = gvFunctAcc.Rows.Item(nF).FindControl("chk")
                            If cbFd IsNot Nothing AndAlso cbFd.Checked Then
                                cbterpilihF = True
                                activeStatus = "Y"
                                'insert & update
                                Session("sFunctionID") = (gvFunctAcc.Rows(nF).Cells(1).Text)
                                cekFunction()
                            Else
                                Session("sFunctionID") = (gvFunctAcc.Rows(nF).Cells(1).Text)
                                activeStatus = "N"
                                updateFunctionAcc()
                            End If
                            nF += 1
                        Loop
                    Else

                    End If
                End If
                n += 1
            Loop
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('module acces setting successfully !');document.getElementById('Buttonx').click()", True)
            getData()
        End If
    End Sub

    Protected Sub gvUserAcces_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvUserAcces.RowCommand
        If e.CommandName = "getSelect" Then
            Session("sAccesSelect") = e.CommandArgument
            If PnlPassword.Visible = True Then
                If tbpass.Text = "" And Len(tbpass.Text) < 6 Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please enter your password at least 6 characters !');document.getElementById('Buttonx').click()", True)
                ElseIf tbconf_pass.Text = "" And Len(tbpass.Text) < 6 Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('please enter your password confirm at least 6 characters !');document.getElementById('Buttonx').click()", True)
                ElseIf tbconf_pass.Text <> tbpass.Text Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Passwords do not match!');document.getElementById('Buttonx').click()", True)

                End If
            End If
            If tbusercode.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('please enter your username !');document.getElementById('Buttonx').click()", True)
            ElseIf tbfullname.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('please enter your fullname !');document.getElementById('Buttonx').click()", True)
            Else
                SaveCopyAccesModule()
                SaveCopyAccesFunction()
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCOA", "hideModalCOA()", True)
        End If
    End Sub
End Class