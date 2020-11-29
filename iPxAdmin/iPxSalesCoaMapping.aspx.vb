Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxSalesCoaMapping
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
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
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='31'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as SaveConf "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("SaveConf").ToString = "Y" Then
                btnSaveCredit.Enabled = True
                btnSavegrid.Enabled = True
                lbSetCoaRev.Enabled = True
                lbSetCoaSet.Enabled = True
            Else
                btnSaveCredit.Enabled = False
                btnSavegrid.Enabled = False
                lbSetCoaRev.Enabled = False
                lbSetCoaSet.Enabled = False
            End If
        Else
            btnSaveCredit.Enabled = False
            btnSavegrid.Enabled = False
            lbSetCoaRev.Enabled = False
            lbSetCoaSet.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub FOLink()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select b.businessid, b.businessname "
        sSQL += "from iPxAcct_FOlink as a "
        sSQL += "INNER JOIN iPx_profile_client as b ON a.FoLink = b.businessid WHERE a.businessid='" & Session("sBusinessID") & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlFOLink.DataSource = dt
                dlFOLink.DataTextField = "businessname"
                dlFOLink.DataValueField = "businessid"
                dlFOLink.DataBind()
            End Using
        End Using
    End Sub
    Sub listPosCodeCoa()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT '1' as numb, a.*,b.description FROM iPxAcctSales_MapPosCode as a "
        sSQL += "INNER JOIN iPxPMS_cfg_pos as b ON b.poscode=a.PosCode and b.businessid=a.pmsID "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and b.revenuegroup='RO' "
        sSQL += "UNION "
        sSQL += "SELECT '2' as numb, a.*,b.description FROM iPxAcctSales_MapPosCode as a "
        sSQL += "INNER JOIN iPxPMS_cfg_pos as b ON b.poscode=a.PosCode and b.businessid=a.pmsID "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and b.revenuegroup='FB' "
        sSQL += "UNION "
        sSQL += "SELECT '3' as numb, a.*,b.description FROM iPxAcctSales_MapPosCode as a "
        sSQL += "INNER JOIN iPxPMS_cfg_pos as b ON b.poscode=a.PosCode and b.businessid=a.pmsID "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and b.revenuegroup<>'RO' and b.revenuegroup<>'FB' and b.revenuegroup<>'SY' "
        sSQL += "UNION "
        sSQL += "SELECT '4' as numb, a.*,b.description FROM iPxAcctSales_MapPosCode as a "
        sSQL += "INNER JOIN iPxPMS_cfg_pos as b ON b.poscode=a.PosCode and b.businessid=a.pmsID "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and b.revenuegroup='SY' "
        sSQL += "order by numb, b.description asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvPosDebit.DataSource = dt
                    gvPosDebit.DataBind()
                    btnSavegrid.Visible = True
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvPosDebit.DataSource = dt
                    gvPosDebit.DataBind()
                    gvPosDebit.Rows(0).Visible = False
                    btnSavegrid.Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listMapPay()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT '1' as numb,pos.poscode, pos.description, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='GL' and a.PosCode=pos.poscode) as GLCoa, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='CS' and a.PosCode=pos.poscode) as CSCoa, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='CR' and a.PosCode=pos.poscode) as CRCoa, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='CL' and a.PosCode=pos.poscode) as CLCoa, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='WB' and a.PosCode=pos.poscode) as WBCoa "
        sSQL += "FROM iPxPMS_cfg_pos as pos  INNER JOIN iPxAcctSales_MapPayment as Map ON map.pmsID=pos.businessid where map.businessid='" & Session("sBusinessID") & "' and pos.businessid ='" & dlFOLink.SelectedValue & "' and pos.revenuegroup='RO' "
        sSQL += "UNION "
        sSQL += "SELECT '2' as numb,pos.poscode, pos.description, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='GL' and a.PosCode=pos.poscode) as GLCoa, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='CS' and a.PosCode=pos.poscode) as CSCoa, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='CR' and a.PosCode=pos.poscode) as CRCoa, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='CL' and a.PosCode=pos.poscode) as CLCoa, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='WB' and a.PosCode=pos.poscode) as WBCoa "
        sSQL += "FROM iPxPMS_cfg_pos as pos  INNER JOIN iPxAcctSales_MapPayment as Map ON map.pmsID=pos.businessid where map.businessid='" & Session("sBusinessID") & "' and pos.businessid ='" & dlFOLink.SelectedValue & "' and pos.revenuegroup='FB' "
        sSQL += "UNION "
        sSQL += "SELECT '3' as numb,pos.poscode, pos.description, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='GL' and a.PosCode=pos.poscode) as GLCoa, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='CS' and a.PosCode=pos.poscode) as CSCoa, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='CR' and a.PosCode=pos.poscode) as CRCoa, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='CL' and a.PosCode=pos.poscode) as CLCoa, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='WB' and a.PosCode=pos.poscode) as WBCoa "
        sSQL += "FROM iPxPMS_cfg_pos as pos  INNER JOIN iPxAcctSales_MapPayment as Map ON map.pmsID=pos.businessid where map.businessid='" & Session("sBusinessID") & "' and pos.businessid ='" & dlFOLink.SelectedValue & "' and pos.revenuegroup<>'RO' and pos.revenuegroup<>'FB' and pos.revenuegroup<>'SY' "
        sSQL += "UNION "
        sSQL += "SELECT '4' as numb,pos.poscode, pos.description, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='GL' and a.PosCode=pos.poscode) as GLCoa, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='CS' and a.PosCode=pos.poscode) as CSCoa, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='CR' and a.PosCode=pos.poscode) as CRCoa, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='CL' and a.PosCode=pos.poscode) as CLCoa, "
        sSQL += "(select  a.Coa from iPxAcctSales_MapPayment as a "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and a.pmsID='" & dlFOLink.SelectedValue & "' and a.PaymentType='WB' and a.PosCode=pos.poscode) as WBCoa "
        sSQL += "FROM iPxPMS_cfg_pos as pos INNER JOIN iPxAcctSales_MapPayment as Map ON map.pmsID=pos.businessid where map.businessid='" & Session("sBusinessID") & "' and pos.businessid ='" & dlFOLink.SelectedValue & "' and pos.revenuegroup='SY' "
        sSQL += "ORDER BY numb, pos.description asc "
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvPosCredit.DataSource = dt
                    gvPosCredit.DataBind()
                    'btnSavegrid.Visible = True
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvPosCredit.DataSource = dt
                    gvPosCredit.DataBind()
                    gvPosCredit.Rows(0).Visible = False
                    'btnSavegrid.Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub cekMappingCOA()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT PosCode FROM iPxAcctSales_MapPosCode WHERE businessid='" & Session("sBusinessID") & "' and RevGrp='01' and pmsID = '" & dlFOLink.SelectedValue & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            oSQLReader.Close()
            listPosCodeCoa()
        Else
            oSQLReader.Close()
            saveGrpBudget(Session("sBusinessID"), dlFOLink.SelectedValue)
            listPosCodeCoa()
        End If
        oCnct.Close()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT PosCode FROM iPxAcctSales_MapPayment WHERE businessid='" & Session("sBusinessID") & "' and pmsID = '" & dlFOLink.SelectedValue & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            oSQLReader.Close()
            listMapPay()
        Else
            oSQLReader.Close()
            savePayType(Session("sBusinessID"), dlFOLink.SelectedValue)
            listMapPay()
        End If
        oCnct.Close()
    End Sub
    Public Function savePayType(ByVal businessid As String, ByVal businessidfo As String) As Boolean

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT paymenttype FROM iPx_profile_paymenttype order by [order]"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        While oSQLReader.Read
            'oSQLReader.Close()
            saveMapPayCode(businessid, businessidfo, oSQLReader.Item("paymenttype"))
        End While
        oCnct.Close()
    End Function
    Public Function saveMapPayCode(ByVal businessid As String, ByVal businessidfo As String, ByVal PayType As String) As Boolean
        Dim sCnct2 As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct2 As SqlConnection = New SqlConnection(sCnct2)
        Dim oSQLCmd2 As SqlCommand
        Dim oSQLReader2 As SqlDataReader
        If oCnct2.State = ConnectionState.Closed Then
            oCnct2.Open()
        End If
        oSQLCmd2 = New SqlCommand(sSQL, oCnct2)
        sSQL = "SELECT poscode FROM iPxPMS_cfg_pos where businessid ='" & businessidfo & "' "
        oSQLCmd2.CommandText = sSQL
        oSQLReader2 = oSQLCmd2.ExecuteReader
        While oSQLReader2.Read
            'oSQLReader.Close()
            saveMapPay(businessid, businessidfo, oSQLReader2.Item("poscode"), PayType)
        End While
        oCnct2.Close()
    End Function
    Public Function saveMapPay(ByVal businessid As String, ByVal businessidfo As String, ByVal poscode As String, ByVal PayType As String) As Boolean
        Dim sCnct1 As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct1 As SqlConnection = New SqlConnection(sCnct1)
        Dim oSQLCmd1 As SqlCommand
        If oCnct1.State = ConnectionState.Closed Then
            oCnct1.Open()
        End If
        oSQLCmd1 = New SqlCommand(sSQL, oCnct1)
        sSQL = "INSERT INTO iPxAcctSales_MapPayment(businessid,PosCode,PaymentType,Coa,pmsID) "
        sSQL = sSQL & "VALUES ('" & businessid & "','" & poscode & "','" & PayType & "','','" & businessidfo & "') "
        oSQLCmd1.CommandText = sSQL
        oSQLCmd1.ExecuteNonQuery()
        oCnct1.Close()
    End Function
    Public Function saveGrpBudget(ByVal businessid As String, ByVal businessidfo As String) As Boolean

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT poscode FROM iPxPMS_cfg_pos where businessid ='" & businessidfo & "' "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        While oSQLReader.Read
            'oSQLReader.Close()
            saveBudget(businessid, businessidfo, oSQLReader.Item("poscode"))
        End While
        oCnct.Close()
    End Function
    Public Function saveBudget(ByVal businessid As String, ByVal businessidfo As String, ByVal poscode As String) As Boolean
        Dim sCnct1 As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct1 As SqlConnection = New SqlConnection(sCnct1)
        Dim oSQLCmd1 As SqlCommand
        If oCnct1.State = ConnectionState.Closed Then
            oCnct1.Open()
        End If
        oSQLCmd1 = New SqlCommand(sSQL, oCnct1)
        sSQL = "INSERT INTO iPxAcctSales_MapPosCode(businessid,PosCode,RevGrp,RevenueCoa,TaxCoa,ServiceCoa,pmsID) "
        sSQL = sSQL & "VALUES ('" & businessid & "','" & poscode & "','01','0','0','0'"
        sSQL = sSQL & ",'" & businessidfo & "') "
        oSQLCmd1.CommandText = sSQL
        oSQLCmd1.ExecuteNonQuery()
        oCnct1.Close()
    End Function
    Public Function saveGrid(ByVal businessid As String, ByVal businessfo As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        For Each row As GridViewRow In gvPosDebit.Rows
            Dim hdCode As HiddenField = row.FindControl("hdCode")
            Dim hdGroup As HiddenField = row.FindControl("hdGroup")
            Dim txtRevCoa As TextBox = row.FindControl("txtRevCoa")
            Dim txtTaxCoa As TextBox = row.FindControl("txtTaxCoa")
            Dim txtService As TextBox = row.FindControl("txtService")

            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "UPDATE iPxAcctSales_MapPosCode  SET RevenueCoa ='" & txtRevCoa.Text & "', TaxCoa ='" & txtTaxCoa.Text & "',ServiceCoa='" & txtService.Text & "' "
            sSQL += "where businessid ='" & businessid & "' and pmsID='" & businessfo & "' and RevGrp ='" & hdGroup.Value.Trim & "' and PosCode ='" & hdCode.Value.Trim & "' "
            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()

        Next
        oCnct.Close()
    End Function
    Public Function saveGridCredit(ByVal businessid As String, ByVal businessfo As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        For Each row As GridViewRow In gvPosCredit.Rows
            Dim hdCode As HiddenField = row.FindControl("hdCode")
            Dim hdCodeDesc As HiddenField = row.FindControl("hdCodeDesc")
            Dim txtGLCoa As TextBox = row.FindControl("txtGLCoa")
            Dim txtCSCoa As TextBox = row.FindControl("txtCSCoa")
            Dim txtCRCoa As TextBox = row.FindControl("txtCRCoa")
            Dim txtCLCoa As TextBox = row.FindControl("txtCLCoa")
            Dim txtWBCoa As TextBox = row.FindControl("txtWBCoa")
            If txtGLCoa.Text <> "" Then
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "SELECT * FROM iPxAcct_Coa AS a "
                sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.Coa = '" & Replace(txtGLCoa.Text, "'", "''") & "'"
                sSQL += " and a.isactive = 'Y' order by a.Description asc"
                oSQLCmd.CommandText = sSQL
                oSQLReader = oSQLCmd.ExecuteReader

                If oSQLReader.Read Then
                    oSQLReader.Close()
                    oSQLCmd = New SqlCommand(sSQL, oCnct)
                    sSQL = "UPDATE iPxAcctSales_MapPayment  SET Coa ='" & txtGLCoa.Text & "' "
                    sSQL += "where businessid ='" & businessid & "' and pmsID='" & businessfo & "' and PaymentType ='GL' and PosCode ='" & hdCode.Value.Trim & "' "
                    oSQLCmd.CommandText = sSQL
                    oSQLCmd.ExecuteNonQuery()
                Else
                    oSQLReader.Close()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('sorry, the coa POST TO GUEST ACCOUNT (GL) and " & Trim(LCase(hdCodeDesc.Value)) & " you entered is not registered!');", True)
                End If
            Else
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "UPDATE iPxAcctSales_MapPayment  SET Coa ='" & txtGLCoa.Text & "' "
                sSQL += "where businessid ='" & businessid & "' and pmsID='" & businessfo & "' and PaymentType ='GL' and PosCode ='" & hdCode.Value.Trim & "' "
                oSQLCmd.CommandText = sSQL
                oSQLCmd.ExecuteNonQuery()
            End If
            If txtCSCoa.Text <> "" Then
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "SELECT * FROM iPxAcct_Coa AS a "
                sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.Coa = '" & Replace(txtCSCoa.Text, "'", "''") & "'"
                sSQL += " and a.isactive = 'Y' order by a.Description asc"
                oSQLCmd.CommandText = sSQL
                oSQLReader = oSQLCmd.ExecuteReader

                If oSQLReader.Read Then
                    oSQLReader.Close()
                    oSQLCmd = New SqlCommand(sSQL, oCnct)
                    sSQL = "UPDATE iPxAcctSales_MapPayment  SET Coa ='" & txtCSCoa.Text & "' "
                    sSQL += "where businessid ='" & businessid & "' and pmsID='" & businessfo & "' and PaymentType ='CS' and PosCode ='" & hdCode.Value.Trim & "' "
                    oSQLCmd.CommandText = sSQL
                    oSQLCmd.ExecuteNonQuery()
                Else
                    oSQLReader.Close()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('sorry, the coa CASH and " & Trim(LCase(hdCodeDesc.Value)) & " you entered is not registered!');", True)
                End If
            Else
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "UPDATE iPxAcctSales_MapPayment  SET Coa ='" & txtCSCoa.Text & "' "
                sSQL += "where businessid ='" & businessid & "' and pmsID='" & businessfo & "' and PaymentType ='CS' and PosCode ='" & hdCode.Value.Trim & "' "
                oSQLCmd.CommandText = sSQL
                oSQLCmd.ExecuteNonQuery()
            End If
            If txtCRCoa.Text <> "" Then
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "SELECT * FROM iPxAcct_Coa AS a "
                sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.Coa = '" & Replace(txtCRCoa.Text, "'", "''") & "'"
                sSQL += " and a.isactive = 'Y' order by a.Description asc"
                oSQLCmd.CommandText = sSQL
                oSQLReader = oSQLCmd.ExecuteReader

                If oSQLReader.Read Then
                    oSQLReader.Close()
                    oSQLCmd = New SqlCommand(sSQL, oCnct)
                    sSQL = "UPDATE iPxAcctSales_MapPayment  SET Coa ='" & txtCRCoa.Text & "' "
                    sSQL += "where businessid ='" & businessid & "' and pmsID='" & businessfo & "' and PaymentType ='CR' and PosCode ='" & hdCode.Value.Trim & "' "
                    oSQLCmd.CommandText = sSQL
                    oSQLCmd.ExecuteNonQuery()
                Else
                    oSQLReader.Close()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('sorry, the coa CARD and " & Trim(LCase(hdCodeDesc.Value)) & " you entered is not registered!');", True)
                End If
            Else
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "UPDATE iPxAcctSales_MapPayment  SET Coa ='" & txtCRCoa.Text & "' "
                sSQL += "where businessid ='" & businessid & "' and pmsID='" & businessfo & "' and PaymentType ='CR' and PosCode ='" & hdCode.Value.Trim & "' "
                oSQLCmd.CommandText = sSQL
                oSQLCmd.ExecuteNonQuery()
            End If
            If txtCLCoa.Text <> "" Then
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "SELECT * FROM iPxAcct_Coa AS a "
                sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.Coa = '" & Replace(txtCLCoa.Text, "'", "''") & "'"
                sSQL += " and a.isactive = 'Y' order by a.Description asc"
                oSQLCmd.CommandText = sSQL
                oSQLReader = oSQLCmd.ExecuteReader

                If oSQLReader.Read Then
                    oSQLReader.Close()
                    oSQLCmd = New SqlCommand(sSQL, oCnct)
                    sSQL = "UPDATE iPxAcctSales_MapPayment  SET Coa ='" & txtCLCoa.Text & "' "
                    sSQL += "where businessid ='" & businessid & "' and pmsID='" & businessfo & "' and PaymentType ='CL' and PosCode ='" & hdCode.Value.Trim & "' "
                    oSQLCmd.CommandText = sSQL
                    oSQLCmd.ExecuteNonQuery()
                Else
                    oSQLReader.Close()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('sorry, the coa CITY LEDGER and " & Trim(LCase(hdCodeDesc.Value)) & " you entered is not registered!');", True)
                End If
            Else
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "UPDATE iPxAcctSales_MapPayment  SET Coa ='" & txtCLCoa.Text & "' "
                sSQL += "where businessid ='" & businessid & "' and pmsID='" & businessfo & "' and PaymentType ='CL' and PosCode ='" & hdCode.Value.Trim & "' "
                oSQLCmd.CommandText = sSQL
                oSQLCmd.ExecuteNonQuery()
            End If
            If txtWBCoa.Text <> "" Then
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "SELECT * FROM iPxAcct_Coa AS a "
                sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.Coa = '" & Replace(txtWBCoa.Text, "'", "''") & "'"
                sSQL += " and a.isactive = 'Y' order by a.Description asc"
                oSQLCmd.CommandText = sSQL
                oSQLReader = oSQLCmd.ExecuteReader

                If oSQLReader.Read Then
                    oSQLReader.Close()
                    oSQLCmd = New SqlCommand(sSQL, oCnct)
                    sSQL = "UPDATE iPxAcctSales_MapPayment  SET Coa ='" & txtWBCoa.Text & "' "
                    sSQL += "where businessid ='" & businessid & "' and pmsID='" & businessfo & "' and PaymentType ='WB' and PosCode ='" & hdCode.Value.Trim & "' "
                    oSQLCmd.CommandText = sSQL
                    oSQLCmd.ExecuteNonQuery()
                Else
                    oSQLReader.Close()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('sorry, the coa WEB BOOKING and " & Trim(LCase(hdCodeDesc.Value)) & " you entered is not registered!');", True)
                End If
            Else
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "UPDATE iPxAcctSales_MapPayment  SET Coa ='" & txtWBCoa.Text & "' "
                sSQL += "where businessid ='" & businessid & "' and pmsID='" & businessfo & "' and PaymentType ='WB' and PosCode ='" & hdCode.Value.Trim & "' "
                oSQLCmd.CommandText = sSQL
                oSQLCmd.ExecuteNonQuery()
            End If
        Next
        oCnct.Close()
    End Function
    Sub ListCOA()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.businessid, a.Coa, a.description, a.type, a.d_c, a.grpLevel, a.levelid, (b.Description) AS Devision, (c.Description) AS Departement, "
        sSQL += "(d.Description) AS SubDepartement, a.Status, a.notes, a.isactive "
        sSQL += "FROM iPxAcct_Coa AS a "
        sSQL += "INNER JOIN iPxAcct_CoaDivision AS b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.Devision = b.Division "
        sSQL += "INNER JOIN iPxAcct_CoaDepartement AS c ON a.businessid = c.businessid COLLATE Latin1_General_CI_AS AND b.Division = c.Division "
        sSQL += "AND a.Departement = c.Departement "
        sSQL += "INNER JOIN iPxAcct_CoaSubDepartement AS d ON a.businessid = d.businessid COLLATE Latin1_General_CI_AS "
        sSQL += "AND a.SubDepartement = d.SubDept AND c.Division = d.Division AND c.Departement = d.Departement "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.Coa like '" & Replace(tbFindCoaList.Text, "'", "''") & "%'"
        sSQL += " and a.isactive = 'Y'"
        sSQL += " order by a.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCoa.DataSource = dt
                    gvCoa.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCoa.DataSource = dt
                    gvCoa.DataBind()
                    gvCoa.Rows(0).Visible = False
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
            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "Sales Configuration") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            FOLink()
            cekMappingCOA()
            listMapPay()
            UserAcces()
        End If
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvCoa.PageIndex = e.NewPageIndex
        Me.ListCOA()
    End Sub

    Protected Sub gvCoa_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCoa.PageIndexChanging
        gvCoa.PageIndex = e.NewPageIndex
        Me.ListCOA()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvCoa.PageIndex = e.NewPageIndex
        Me.ListCOA()
    End Sub
    Protected Sub dlFOLink_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlFOLink.SelectedIndexChanged
        cekMappingCOA()
    End Sub

    Protected Sub btnSavegrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSavegrid.Click
        saveGrid(Session("sBusinessID"), dlFOLink.SelectedValue)
        listPosCodeCoa()
    End Sub

    Protected Sub gvPosCredit_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPosCredit.RowCommand
        If e.CommandName = "getCOAGL" Then
            Session("sPos") = e.CommandArgument
            Session("sPay") = "GL"
            For Each row As GridViewRow In gvPosCredit.Rows
                Dim hdCode As HiddenField = row.FindControl("hdCode")
                If hdCode.Value = Session("sPos") Then
                    Dim txtGLCoa As TextBox = row.FindControl("txtGLCoa")
                    tbFindCoaList.Text = txtGLCoa.Text
                End If
            Next
            ListCOA()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreditActive", "CreditActive()", True)
        ElseIf e.CommandName = "getCOACS" Then
            Session("sPos") = e.CommandArgument
            Session("sPay") = "CS"
            For Each row As GridViewRow In gvPosCredit.Rows
                Dim hdCode As HiddenField = row.FindControl("hdCode")
                If hdCode.Value = Session("sPos") Then
                    Dim txtCSCoa As TextBox = row.FindControl("txtCSCoa")
                    tbFindCoaList.Text = txtCSCoa.Text
                End If
            Next
            ListCOA()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreditActive", "CreditActive()", True)
        ElseIf e.CommandName = "getCOACR" Then
            Session("sPos") = e.CommandArgument
            Session("sPay") = "CR"
            For Each row As GridViewRow In gvPosCredit.Rows
                Dim hdCode As HiddenField = row.FindControl("hdCode")
                If hdCode.Value = Session("sPos") Then
                    Dim txtCRCoa As TextBox = row.FindControl("txtCRCoa")
                    tbFindCoaList.Text = txtCRCoa.Text
                End If
            Next
            ListCOA()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreditActive", "CreditActive()", True)
        ElseIf e.CommandName = "getCOACL" Then
            Session("sPos") = e.CommandArgument
            Session("sPay") = "CL"
            For Each row As GridViewRow In gvPosCredit.Rows
                Dim hdCode As HiddenField = row.FindControl("hdCode")
                If hdCode.Value = Session("sPos") Then
                    Dim txtCLCoa As TextBox = row.FindControl("txtCLCoa")
                    tbFindCoaList.Text = txtCLCoa.Text
                End If
            Next
            ListCOA()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreditActive", "CreditActive()", True)
        ElseIf e.CommandName = "getCOAWB" Then
            Session("sPos") = e.CommandArgument
            Session("sPay") = "WB"
            For Each row As GridViewRow In gvPosCredit.Rows
                Dim hdCode As HiddenField = row.FindControl("hdCode")
                If hdCode.Value = Session("sPos") Then
                    Dim txtWBCoa As TextBox = row.FindControl("txtWBCoa")
                    tbFindCoaList.Text = txtWBCoa.Text
                End If
            Next
            ListCOA()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreditActive", "CreditActive()", True)
        End If
        listMapPay()
        listPosCodeCoa()
    End Sub

    Protected Sub lbAbortListCOA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortListCOA.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
        If Session("sPay") = "SettingRevenue" Or Session("sPay") = "SettingTax" Or Session("sPay") = "SettingService" Or Session("sPay") = "SettingGL" Or Session("sPay") = "SettingCS" Or Session("sPay") = "SettingCR" Or Session("sPay") = "SettingCL" Or Session("sPay") = "SettingWB" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalSettingCOA", "showModalSettingCOA()", True)
        ElseIf Session("sPay") <> "Service" And Session("sPay") <> "Tax" And Session("sPay") <> "Rev" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreditActive", "CreditActive()", True)
        End If
        listMapPay()
        listPosCodeCoa()
    End Sub

    Protected Sub gvCoa_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCoa.RowCommand
        If e.CommandName = "getSelect" Then
            Session("sCoa") = e.CommandArgument
            If Session("sPay") = "SettingRevenue" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
                txtSetRevenue.Text = Session("sCoa")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalSettingCOA", "showModalSettingCOA()", True)
                listMapPay()
                listPosCodeCoa()
            ElseIf Session("sPay") = "SettingTax" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
                txtSetTax.Text = Session("sCoa")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalSettingCOA", "showModalSettingCOA()", True)
                listMapPay()
                listPosCodeCoa()
            ElseIf Session("sPay") = "SettingService" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
                txtSetService.Text = Session("sCoa")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalSettingCOA", "showModalSettingCOA()", True)
                listMapPay()
                listPosCodeCoa()
            ElseIf Session("sPay") = "SettingGL" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreditActive", "CreditActive()", True)
                txtSetGL.Text = Session("sCoa")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalSettingCOA", "showModalSettingCOA()", True)
                listMapPay()
                listPosCodeCoa()
            ElseIf Session("sPay") = "SettingCS" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreditActive", "CreditActive()", True)
                txtSetCS.Text = Session("sCoa")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalSettingCOA", "showModalSettingCOA()", True)
                listMapPay()
                listPosCodeCoa()
            ElseIf Session("sPay") = "SettingCR" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreditActive", "CreditActive()", True)
                txtSetCR.Text = Session("sCoa")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalSettingCOA", "showModalSettingCOA()", True)
                listMapPay()
                listPosCodeCoa()
            ElseIf Session("sPay") = "SettingCL" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreditActive", "CreditActive()", True)
                txtSetCL.Text = Session("sCoa")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalSettingCOA", "showModalSettingCOA()", True)
                listMapPay()
                listPosCodeCoa()
            ElseIf Session("sPay") = "SettingWB" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreditActive", "CreditActive()", True)
                txtSetWB.Text = Session("sCoa")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalSettingCOA", "showModalSettingCOA()", True)
                listMapPay()
                listPosCodeCoa()
            ElseIf Session("sPay") = "Service" Or Session("sPay") = "Tax" Or Session("sPay") = "Rev" Then
                
                For Each row As GridViewRow In gvPosDebit.Rows
                    Dim hdCode As HiddenField = row.FindControl("hdCode")
                    'Dim txtRevCoa As TextBox = row.FindControl("txtRevCoa")
                    'Dim txtTaxCoa As TextBox = row.FindControl("txtTaxCoa")
                    If hdCode.Value = Session("sPos") Then
                        If Session("sPay") = "Rev" Then
                            Dim txtRevCoa As TextBox = row.FindControl("txtRevCoa")
                            txtRevCoa.Text = Session("sCoa")
                        ElseIf Session("sPay") = "Tax" Then
                            Dim txtTaxCoa As TextBox = row.FindControl("txtTaxCoa")
                            txtTaxCoa.Text = Session("sCoa")
                        ElseIf Session("sPay") = "Service" Then
                            Dim txtService As TextBox = row.FindControl("txtService")
                            txtService.Text = Session("sCoa")
                        End If
                    End If
                Next
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
            Else
                For Each row As GridViewRow In gvPosCredit.Rows
                    Dim hdCode As HiddenField = row.FindControl("hdCode")
                    'Dim txtRevCoa As TextBox = row.FindControl("txtRevCoa")
                    'Dim txtTaxCoa As TextBox = row.FindControl("txtTaxCoa")
                    If hdCode.Value = Session("sPos") Then
                        If Session("sPay") = "GL" Then
                            Dim txtGLCoa As TextBox = row.FindControl("txtGLCoa")
                            txtGLCoa.Text = Session("sCoa")
                        ElseIf Session("sPay") = "CS" Then
                            Dim txtCSCoa As TextBox = row.FindControl("txtCSCoa")
                            txtCSCoa.Text = Session("sCoa")
                        ElseIf Session("sPay") = "CR" Then
                            Dim txtCRCoa As TextBox = row.FindControl("txtCRCoa")
                            txtCRCoa.Text = Session("sCoa")
                        ElseIf Session("sPay") = "CL" Then
                            Dim txtCLCoa As TextBox = row.FindControl("txtCLCoa")
                            txtCLCoa.Text = Session("sCoa")
                        ElseIf Session("sPay") = "WB" Then
                            Dim txtWBCoa As TextBox = row.FindControl("txtWBCoa")
                            txtWBCoa.Text = Session("sCoa")
                        End If
                    End If
                Next
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreditActive", "CreditActive()", True)
            End If
        End If
    End Sub

    Protected Sub btnSaveCredit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveCredit.Click
        saveGridCredit(Session("sBusinessID"), dlFOLink.SelectedValue)
        listMapPay()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreditActive", "CreditActive()", True)
    End Sub

    Protected Sub gvPosDebit_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPosDebit.RowCommand
        If e.CommandName = "getCOARev" Then
            Session("sPos") = e.CommandArgument
            Session("sPay") = "Rev"
            For Each row As GridViewRow In gvPosDebit.Rows
                Dim hdCode As HiddenField = row.FindControl("hdCode")
                If hdCode.Value = Session("sPos") Then
                    Dim txtRevCoa As TextBox = row.FindControl("txtRevCoa")
                    tbFindCoaList.Text = txtRevCoa.Text
                End If
            Next
            ListCOA()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
        ElseIf e.CommandName = "getCOATax" Then
            Session("sPos") = e.CommandArgument
            Session("sPay") = "Tax"
            For Each row As GridViewRow In gvPosDebit.Rows
                Dim hdCode As HiddenField = row.FindControl("hdCode")
                If hdCode.Value = Session("sPos") Then
                    Dim txtTaxCoa As TextBox = row.FindControl("txtTaxCoa")
                    tbFindCoaList.Text = txtTaxCoa.Text
                End If
            Next
            ListCOA()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
        ElseIf e.CommandName = "getCOAService" Then
            Session("sPos") = e.CommandArgument
            Session("sPay") = "Service"
            For Each row As GridViewRow In gvPosDebit.Rows
                Dim hdCode As HiddenField = row.FindControl("hdCode")
                If hdCode.Value = Session("sPos") Then
                    Dim txtService As TextBox = row.FindControl("txtService")
                    tbFindCoaList.Text = txtService.Text
                End If
            Next
            ListCOA()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
        End If
        listMapPay()
        listPosCodeCoa()
    End Sub

    Protected Sub lbFindListCoa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFindListCoa.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
        If Session("sPay") <> "Service" And Session("sPay") <> "Tax" And Session("sPay") <> "Rev" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreditActive", "CreditActive()", True)
        End If
        ListCOA()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
    End Sub
    Private tmpCategoryName As String = ""
    Private tmpHeaderName As String = ""
    Dim group As Integer = 0
    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim drv As DataRowView = CType(e.Row.DataItem, DataRowView)
                If tmpCategoryName <> drv("numb").ToString() Then
                tmpCategoryName = drv("numb").ToString()
                If drv("numb").ToString() = "1" Then
                    tmpHeaderName = "ROOM REVENUE"
                ElseIf drv("numb").ToString() = "2" Then
                    tmpHeaderName = "FOOD AND BEVERAGE"
                ElseIf drv("numb").ToString() = "4" Then
                    tmpHeaderName = "SYSTEM"
                ElseIf drv("numb").ToString() = "3" Then
                    tmpHeaderName = "OTHER"
                End If
                Dim tbl As Table = TryCast(e.Row.Parent, Table)

                If tbl IsNot Nothing Then
                    Dim row As GridViewRow = New GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal)
                    Dim cell As TableCell = New TableCell()
                    cell.ColumnSpan = Me.gvPosDebit.Columns.Count
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
    End Sub

    Private tmpCategoryNameSet As String = ""
    Private tmpHeaderNameSet As String = ""
    Dim groupSet As Integer = 0
    Protected Sub OnRowDataBoundSet(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim drv As DataRowView = CType(e.Row.DataItem, DataRowView)
            If tmpCategoryNameSet <> drv("numb").ToString() Then
                tmpCategoryNameSet = drv("numb").ToString()
                If drv("numb").ToString() = "1" Then
                    tmpHeaderNameSet = "ROOM REVENUE"
                ElseIf drv("numb").ToString() = "2" Then
                    tmpHeaderNameSet = "FOOD AND BEVERAGE"
                ElseIf drv("numb").ToString() = "4" Then
                    tmpHeaderNameSet = "SYSTEM"
                ElseIf drv("numb").ToString() = "3" Then
                    tmpHeaderNameSet = "OTHER"
                End If
                Dim tbl As Table = TryCast(e.Row.Parent, Table)

                If tbl IsNot Nothing Then
                    Dim row As GridViewRow = New GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal)
                    Dim cell As TableCell = New TableCell()
                    cell.ColumnSpan = Me.gvPosCredit.Columns.Count
                    cell.Width = Unit.Percentage(100)
                    cell.Style.Add("Font-weight", "bold")
                    cell.Style.Add("background-color", "#fffff")
                    cell.Style.Add("color", "black")
                    cell.Style.Add("text-transform", "uppercase")
                    Dim span As HtmlGenericControl = New HtmlGenericControl("span")

                    span.InnerHtml = tmpHeaderNameSet
                    cell.Controls.Add(span)
                    row.Cells.Add(cell)
                    tbl.Rows.AddAt(tbl.Rows.Count - 1, row)
                    groupSet += 1
                End If
            End If
        End If
    End Sub

    Protected Sub lbSetCoaRev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSetCoaRev.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalSettingCOA", "showModalSettingCOA()", True)
        listMapPay()
        listPosCodeCoa()
        txtSetRevenue.Text = ""
        txtSetTax.Text = ""
        txtSetService.Text = ""
        pnRevenue.Visible = True
        pnSetlement.Visible = False
    End Sub

    Protected Sub lbSetCoaSet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSetCoaSet.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalSettingCOA", "showModalSettingCOA()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreditActive", "CreditActive()", True)
        listMapPay()
        listPosCodeCoa()
        cbGL.Checked = False
        cbCS.Checked = False
        cbCR.Checked = False
        cbCL.Checked = False
        cbWB.Checked = False
        txtSetGL.Text = ""
        txtSetCS.Text = ""
        txtSetCR.Text = ""
        txtSetCL.Text = ""
        txtSetWB.Text = ""
        pnRevenue.Visible = False
        pnSetlement.Visible = True
    End Sub

    Protected Sub lbAbortSettingCoa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortSettingCoa.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalSettingCOA", "hideModalSettingCOA()", True)
        listMapPay()
        listPosCodeCoa()
        If pnSetlement.Visible = True Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreditActive", "CreditActive()", True)
        End If
    End Sub

    Protected Sub lbSetRevenue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSetRevenue.Click
        Session("sPay") = "SettingRevenue"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalSettingCOA", "hideModalSettingCOA()", True)
        tbFindCoaList.Text = txtSetRevenue.Text
        ListCOA()
        listMapPay()
        listPosCodeCoa()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
    End Sub

    Protected Sub lbSetTax_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSetTax.Click
        Session("sPay") = "SettingTax"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalSettingCOA", "hideModalSettingCOA()", True)
        tbFindCoaList.Text = txtSetTax.Text
        ListCOA()
        listMapPay()
        listPosCodeCoa()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
    End Sub

    Protected Sub lbSetService_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSetService.Click
        Session("sPay") = "SettingService"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalSettingCOA", "hideModalSettingCOA()", True)
        tbFindCoaList.Text = txtSetService.Text
        ListCOA()
        listMapPay()
        listPosCodeCoa()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
    End Sub

    Protected Sub lbSetGL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSetGL.Click
        Session("sPay") = "SettingGL"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalSettingCOA", "hideModalSettingCOA()", True)
        tbFindCoaList.Text = txtSetGL.Text
        ListCOA()
        listMapPay()
        listPosCodeCoa()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
    End Sub

    Protected Sub lbSetCS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSetCS.Click
        Session("sPay") = "SettingCS"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalSettingCOA", "hideModalSettingCOA()", True)
        tbFindCoaList.Text = txtSetCS.Text
        ListCOA()
        listMapPay()
        listPosCodeCoa()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
    End Sub

    Protected Sub lbSetCR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSetCR.Click
        Session("sPay") = "SettingCR"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalSettingCOA", "hideModalSettingCOA()", True)
        tbFindCoaList.Text = txtSetCR.Text
        ListCOA()
        listMapPay()
        listPosCodeCoa()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
    End Sub

    Protected Sub lbSetCL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSetCL.Click
        Session("sPay") = "SettingCL"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalSettingCOA", "hideModalSettingCOA()", True)
        tbFindCoaList.Text = txtSetCL.Text
        ListCOA()
        listMapPay()
        listPosCodeCoa()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
    End Sub

    Protected Sub lbSetWB_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSetWB.Click
        Session("sPay") = "SettingWB"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalSettingCOA", "hideModalSettingCOA()", True)
        tbFindCoaList.Text = txtSetWB.Text
        ListCOA()
        listMapPay()
        listPosCodeCoa()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
    End Sub

    Protected Sub lbSaveSetting_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSaveSetting.Click
        If pnRevenue.Visible = True Then
            If cbRevenue.Checked = True Then
                If oCnct.State = ConnectionState.Closed Then
                    oCnct.Open()
                End If
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "UPDATE iPxAcctSales_MapPosCode  SET "
                sSQL += "RevenueCoa ='" & txtSetRevenue.Text & "' "
                sSQL += "where businessid ='" & Session("sBusinessID") & "' and pmsID='" & dlFOLink.SelectedValue & "' "
                oSQLCmd.CommandText = sSQL
                oSQLCmd.ExecuteNonQuery()
                oCnct.Close()
            End If
            If cbTax.Checked = True Then
                If oCnct.State = ConnectionState.Closed Then
                    oCnct.Open()
                End If
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "UPDATE iPxAcctSales_MapPosCode  SET "
                sSQL += "TaxCoa ='" & txtSetTax.Text & "' "
                sSQL += "where businessid ='" & Session("sBusinessID") & "' and pmsID='" & dlFOLink.SelectedValue & "' "
                oSQLCmd.CommandText = sSQL
                oSQLCmd.ExecuteNonQuery()
                oCnct.Close()
            End If
            If cbService.Checked = True Then
                If oCnct.State = ConnectionState.Closed Then
                    oCnct.Open()
                End If
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "UPDATE iPxAcctSales_MapPosCode  SET "
                sSQL += "ServiceCoa='" & txtSetService.Text & "' "
                sSQL += "where businessid ='" & Session("sBusinessID") & "' and pmsID='" & dlFOLink.SelectedValue & "' "
                oSQLCmd.CommandText = sSQL
                oSQLCmd.ExecuteNonQuery()
                oCnct.Close()
            End If
            If cbRevenue.Checked = False And cbTax.Checked = False And cbService.Checked = False Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select at least 1 checkbox !');", True)
            End If
        ElseIf pnSetlement.Visible = True Then
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            If cbGL.Checked = True Then
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "UPDATE iPxAcctSales_MapPayment  SET Coa ='" & txtSetGL.Text & "' "
                sSQL += "where businessid ='" & Session("sBusinessID") & "' and pmsID='" & dlFOLink.SelectedValue & "' and PaymentType ='GL' "
                oSQLCmd.CommandText = sSQL
                oSQLCmd.ExecuteNonQuery()
            End If
            If cbCS.Checked = True Then
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "UPDATE iPxAcctSales_MapPayment  SET Coa ='" & txtSetCS.Text & "' "
                sSQL += "where businessid ='" & Session("sBusinessID") & "' and pmsID='" & dlFOLink.SelectedValue & "' and PaymentType ='CS' "
                oSQLCmd.CommandText = sSQL
                oSQLCmd.ExecuteNonQuery()
            End If
            If cbCR.Checked = True Then
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "UPDATE iPxAcctSales_MapPayment  SET Coa ='" & txtSetCR.Text & "' "
                sSQL += "where businessid ='" & Session("sBusinessID") & "' and pmsID='" & dlFOLink.SelectedValue & "' and PaymentType ='CR' "
                oSQLCmd.CommandText = sSQL
                oSQLCmd.ExecuteNonQuery()
            End If
            If cbCL.Checked = True Then
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "UPDATE iPxAcctSales_MapPayment  SET Coa ='" & txtSetCL.Text & "' "
                sSQL += "where businessid ='" & Session("sBusinessID") & "' and pmsID='" & dlFOLink.SelectedValue & "' and PaymentType ='CL' "
                oSQLCmd.CommandText = sSQL
                oSQLCmd.ExecuteNonQuery()
            End If
            If cbWB.Checked = True Then
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "UPDATE iPxAcctSales_MapPayment  SET Coa ='" & txtSetWB.Text & "' "
                sSQL += "where businessid ='" & Session("sBusinessID") & "' and pmsID='" & dlFOLink.SelectedValue & "' and PaymentType ='WB' "
                oSQLCmd.CommandText = sSQL
                oSQLCmd.ExecuteNonQuery()
            End If
            oCnct.Close()
            If cbGL.Checked = False And cbCS.Checked = False And cbCR.Checked = False And cbCL.Checked = False And cbWB.Checked = False Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select at least 1 checkbox !');", True)
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreditActive", "CreditActive()", True)
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalSettingCOA", "hideModalSettingCOA()", True)
        listMapPay()
        listPosCodeCoa()
    End Sub
End Class
