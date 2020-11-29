Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxFOLink
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    'Dim sCnctPMS As String = ConfigurationManager.ConnectionStrings("iPxCNCTPMS").ToString
    'Dim oCnctPMS As SqlConnection = New SqlConnection(sCnctPMS)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL As String
    Dim cIpx As New iPxClass
    Sub UserAcces()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='34'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as AddOp "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("AddOp").ToString = "Y" Then
                lbAddFO.Enabled = True
            Else
                lbAddFO.Enabled = False
            End If
        Else
            lbAddFO.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub ListFOLink()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.businessid, a.Description, a.FoLink, b.businessname, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='35'and x.active='Y' and x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "') as EditOp "
        sSQL += "from iPxAcct_FOlink as a "
        sSQL += "INNER JOIN iPx_profile_client AS b ON a.FoLink = b.businessid "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "'"
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & ""
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.Description asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvFO.DataSource = dt
                    gvFO.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvFO.DataSource = dt
                    gvFO.DataBind()
                    gvFO.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub saveFOLink()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "INSERT INTO iPxAcct_FOlink (businessid, FoLink, Description) "
        sSQL = sSQL & "VALUES ('" & Session("sBusinessID") & "','" & dlFO.SelectedValue & "','" & Replace(tbDescription.Text, "'", "''") & "')"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('saved successfully!');", True)
        ListFOLink()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
    End Sub
    Sub updateFOLink()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcct_FOlink SET businessid='" & Session("sBusinessID") & "', Description='" & Replace(tbDescription.Text, "'", "''") & "'"
        sSQL = sSQL & " WHERE FOLink ='" & dlFO.SelectedValue & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Data has been update !');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
        ListFOLink()
    End Sub
    Sub editFOLink()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, b.businessname, PMSProfil.businessid AS businesPMS, clientPMS.businessname AS businessnamePMS, a.Description "
        sSQL += "from iPxAcct_FOlink as a "
        sSQL += "INNER JOIN iPx_profile_client as b ON b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS = a.businessid "
        sSQL += "INNER JOIN iPxPMS_cfg_companyprofile as PMSProfil ON a.FoLink = PMSProfil.businessid "
        sSQL += "INNER JOIN iPxSeries.dbo.iPx_profile_client as clientPMS ON PMSProfil.businessid = clientPMS.businessid "
        sSQL += "where a.businessid = '" & Session("sBusinessID") & "' AND "
        sSQL += "PMSProfil.businessid ='" & Session("sBusinessIDPMS") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Session("BusinessIDPMS") = oSQLReader.Item("businesPMS").ToString
            'tbFo.Text = oSQLReader.Item("businessnamePMS").ToString
            tbDescription.Text = oSQLReader.Item("Description").ToString
            oCnct.Close()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        Else
            oCnct.Close()
        End If
    End Sub
    Sub FOLink()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "Select iPx_profile_user.*,iPx_profile_client.*, iPx_profile_client_usergeneric.password From iPx_profile_user "
        sSQL += "INNER JOIN iPx_profile_client ON iPx_profile_client.userid = iPx_profile_user.id "
        sSQL += "INNER JOIN iPx_profile_client_usergeneric ON iPx_profile_client_usergeneric.genericid = iPx_profile_user.id "
        sSQL += "Where iPx_profile_user.usercode = '" & tbUserPMS.Text & "' and businesstype<>'26' and iPx_profile_user.password='" & tbPassPMS.Text & "' and iPx_profile_client_usergeneric.password='" & tbGeneric.Text & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlFO.DataSource = dt
                dlFO.DataTextField = "businessname"
                dlFO.DataValueField = "businessid"
                dlFO.DataBind()
                dlFO.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "Option") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            Session("sQueryTicket") = ""
            ListFOLink()
        End If
        UserAcces()
    End Sub

    Protected Sub lbAddFO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddFO.Click
        lbSave.Text = "<i class='fa fa-save'></i> Save"
        lbSearchFo.Enabled = True
        'tbFo.Text = ""
        tbDescription.Text = ""
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
    End Sub

    Protected Sub lbSearchFo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSearchFo.Click
        tbUserPMS.Text = ""
        tbPassPMS.Text = ""
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
    End Sub

    Protected Sub lbCloseFo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCloseFo.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddTrans", "hideModalAddTrans()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
    End Sub

    Protected Sub lbLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLogin.Click
        If tbPassPMS.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please enter Your Password !');document.getElementById('Buttonx').click()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddTrans", "hideModalAddTrans()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
        ElseIf tbGeneric.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please enter Your generic id !');document.getElementById('Buttonx').click()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddTrans", "hideModalAddTrans()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
        Else
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If

            sSQL = "Select iPx_profile_user.*,iPx_profile_client.*,(iPx_profile_client_usergeneric.password) as passGen From iPx_profile_user "
            sSQL += "INNER JOIN iPx_profile_client ON iPx_profile_client.userid = iPx_profile_user.id "
            sSQL += "INNER JOIN iPx_profile_client_usergeneric ON iPx_profile_client_usergeneric.genericid = iPx_profile_user.id "
            sSQL += "Where iPx_profile_user.usercode = '" & tbUserPMS.Text & "' and businesstype<>'26'"
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            oSQLReader = oSQLCmd.ExecuteReader

            If oSQLReader.Read Then
                If Trim(oSQLReader.Item("password")) = Trim(tbPassPMS.Text) Then
                    If Trim(oSQLReader.Item("passGen")) = Trim(tbGeneric.Text) Then
                        oSQLReader.Close()

                        FOLink()
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddTrans", "hideModalAddTrans()", True)
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
                    Else
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry Your Generic Id Is Wrong !');document.getElementById('Buttonx').click()", True)
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddTrans", "hideModalAddTrans()", True)
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry Your Password Is Wrong !');document.getElementById('Buttonx').click()", True)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddTrans", "hideModalAddTrans()", True)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
                End If
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry User Is Not Registered Yet !');document.getElementById('Buttonx').click()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddTrans", "hideModalAddTrans()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
            End If
            oCnct.Close()
        End If
    End Sub

    Protected Sub lbSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSave.Click
        If dlFO.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter FO Link!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        ElseIf tbDescription.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Description!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        Else
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT FoLink FROM iPxAcct_FOlink WHERE businessid ='" & Session("sBusinessID") & "' and FoLink = '" & dlFO.SelectedValue & "' "
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            If oSQLReader.Read Then
                oSQLReader.Close()
                If lbSearchFo.Enabled = True Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('FO Link duplicate!');", True)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
                Else
                    updateFOLink()
                End If
            Else
                oSQLReader.Close()
                saveFOLink()
            End If
        End If
    End Sub

    Protected Sub gvFO_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvFO.RowCommand
        If e.CommandName = "getDelete" Then
            'FOLink()
            'dlFO.SelectedValue = e.CommandArgument
            'dlFO.Enabled = False
            'lbSearchFo.Enabled = False
            'lbSave.Text = "<i class='fa fa-save'></i> Update"
            'editFOLink()
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "UPDATE iPxAcct_FOlink SET isActive='N'"
            sSQL = sSQL & " WHERE FOLink ='" & e.CommandArgument & "'"

            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()

            oCnct.Close()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Data has been delete !');", True)
            ListFOLink()
        End If
    End Sub

    Protected Sub lbQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQuery.Click
        tbQBusinessName.Text = ""
        tbQDescription.Text = ""
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub lbAbortQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortQuery.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
    End Sub

    Protected Sub lblQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblQuery.Click
        Session("sQueryTicket") = ""
        If tbQBusinessName.Text <> "" Then
            Session("sCondition") = Session("sCondition") & " and (businessnamePMS like '%" & Replace(tbQBusinessName.Text, "'", "''") & "%') "
        End If
        If tbQDescription.Text <> "" Then
            Session("sCondition") = Session("sCondition") & " and (a.Description like '%" & Replace(tbQDescription.Text, "'", "''") & "%') "
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        ListFOLink()
    End Sub
End Class
