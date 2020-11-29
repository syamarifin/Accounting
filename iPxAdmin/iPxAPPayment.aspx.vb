Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxAPPayment
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, a, statusinvoice, month, year, RecID, MonthImport, YearImport, coaDebit, coaCredit As String
    Dim oCnct1 As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd1 As SqlCommand
    Dim oSQLReader1 As SqlDataReader
    Dim sSQL1 As String
    Dim oCnct2 As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd2 As SqlCommand
    Dim oSQLReader2 As SqlDataReader
    Dim sSQL2 As String
    Dim cIpx As New iPxClass
    Sub UserAcces()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='10'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as AddRec "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("AddRec").ToString = "Y" Then
                lbAddRec.Enabled = True
            Else
                lbAddRec.Enabled = False
            End If
        Else
            lbAddRec.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub showdata_dropdownCustStatus()
        dlQStatus.Items.Insert(0, "")
        dlQStatus.Items.Insert(1, "New")
        dlQStatus.Items.Insert(2, "Delete")
    End Sub
    Sub country()
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_geog_country order by country asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlQCountry.DataSource = dt
                dlQCountry.DataTextField = "country"
                dlQCountry.DataValueField = "countryid"
                dlQCountry.DataBind()
                dlQCountry.Items.Insert(0, "")
            End Using
        End Using
        'dlQCountry.SelectedValue = cIpx.getDefaultParameter(Session("sBusinessID"), "01")
    End Sub
    Sub province()
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_geog_province where countryid ='" & dlQCountry.SelectedValue.Trim & "' order by description asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlQProvince.DataSource = dt
                dlQProvince.DataTextField = "description"
                dlQProvince.DataValueField = "provid"
                dlQProvince.DataBind()
                dlQProvince.Items.Insert(0, "")
            End Using
        End Using
        'dlQCity.SelectedValue = cIpx.getDefaultParameter(Session("sBusinessID"), "03")
    End Sub
    Sub city()
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_geog_city where countryid ='" & dlQCountry.SelectedValue.Trim & "' and provid = '" & dlQProvince.SelectedValue.Trim & "' order by city asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlQCity.DataSource = dt
                dlQCity.DataTextField = "city"
                dlQCity.DataValueField = "cityid"
                dlQCity.DataBind()
                dlQCity.Items.Insert(0, "")
            End Using
        End Using
        'dlQCity.SelectedValue = cIpx.getDefaultParameter(Session("sBusinessID"), "04")
    End Sub
    Sub Paidby()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select * from iPxAcctAP_Cfg_Paidby WHERE businessid='" & Session("sBusinessID") & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlPaid.DataSource = dt
                dlPaid.DataTextField = "Description"
                dlPaid.DataValueField = "PaidBy"
                dlPaid.DataBind()
                dlPaid.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub ListReceipt()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        month = Left(tbDate.Text, 2)
        year = Right(tbDate.Text, 4)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.*,(SELECT sum(amountdr) as total FROM iPxAcctAP_Transaction where iPxAcctAP_Transaction.businessid ='" & Session("sBusinessID") & "' AND iPxAcctAP_Transaction.TransID = a.PaymentID) as Amount, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid='" & Session("sBusinessID") & "' and x.usercode='" & Session("sUserCode") & "' and x.funtionid='11' and x.active='Y') as editRec, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid='" & Session("sBusinessID") & "' and x.usercode='" & Session("sUserCode") & "' and x.funtionid='12' and x.active='Y') as deleteRec, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid='" & Session("sBusinessID") & "' and x.usercode='" & Session("sUserCode") & "' and x.funtionid='13' and x.active='Y') as postRec "
        sSQL += "from (SELECT iPxAcctAP_Payment.PaymentID, iPxAcctAP_Payment.PaymentDate, iPxAcctAP_Cfg_Vendor.CoyName, iPxAcctAP_Payment.ReffNo, "
        sSQL += "iPxAcctAP_Cfg_Paidby.Description, iPxAcctAP_Payment.Notes, iPx_profile_user.fullname,iPxAcctAP_Payment.Status "
        sSQL += "FROM iPxAcctAP_Payment "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_Paidby ON iPxAcctAP_Payment.PaidBy = iPxAcctAP_Cfg_Paidby.PaidBy AND iPxAcctAP_Payment.businessid = iPxAcctAP_Cfg_Paidby.businessid "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_Vendor ON iPxAcctAP_Payment.VendorID COLLATE Latin1_General_CI_AS = iPxAcctAP_Cfg_Vendor.VendorID AND "
        sSQL += "iPxAcctAP_Payment.businessid COLLATE Latin1_General_CI_AS = iPxAcctAP_Cfg_Vendor.businessid "
        sSQL += "INNER JOIN iPx_profile_user ON iPxAcctAP_Payment.RegBy = iPx_profile_user.id "
        sSQL += "where iPxAcctAP_Payment.businessid ='" & Session("sBusinessID") & "' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL += " AND month(iPxAcctAP_Payment.PaymentDate)='" & month & "' AND year(iPxAcctAP_Payment.PaymentDate)='" & year & "' AND iPxAcctAP_Payment.Status<>'X'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " ) a order by a.PaymentID desc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvARRec.DataSource = dt
                    gvARRec.DataBind()
                    Dim totalAmount As Decimal
                    If dt.Compute("Sum(Amount)", "").ToString() <> "" Then
                        totalAmount = dt.Compute("Sum(Amount)", "").ToString()
                    Else
                        totalAmount = 0
                    End If
                    gvARRec.FooterRow.Cells(4).Text = "Total"
                    gvARRec.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right
                    gvARRec.FooterRow.Cells(5).Text = totalAmount.ToString("N2")
                    gvARRec.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvARRec.DataSource = dt
                    gvARRec.DataBind()
                    gvARRec.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listCustomer()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAP_Cfg_Vendor.businessid, iPxAcctAP_Cfg_Vendor.VendorID, iPxAcctAP_Cfg_VendorGrp.Description AS ARGroup, "
        sSQL += "iPxAcctAP_Cfg_Vendor.CoyName, iPxAcctAP_Cfg_Vendor.Address, iPxAcctAP_Cfg_Vendor.BilllingAddress, "
        sSQL += "iPx_profile_geog_country.country, iPx_profile_geog_province.description AS Province, iPx_profile_geog_city.city, iPxAcctAP_Cfg_Vendor.Phone, "
        sSQL += "iPxAcctAP_Cfg_Vendor.Email, (select (sum(amountcr)-sum(amountdr)) from iPxAcctAP_Transaction as c where c.VendorID=iPxAcctAP_Cfg_Vendor.VendorID and c.PVno<>'' and c.isActive='Y') as amountTrans FROM iPxAcctAP_Cfg_Vendor "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_VendorGrp ON iPxAcctAP_Cfg_Vendor.businessid = iPxAcctAP_Cfg_VendorGrp.businessid AND iPxAcctAP_Cfg_Vendor.apGroup = iPxAcctAP_Cfg_VendorGrp.apGroup "
        sSQL += "INNER JOIN iPx_profile_geog_country ON iPxAcctAP_Cfg_Vendor.CountryId = iPx_profile_geog_country.countryid "
        sSQL += "INNER JOIN iPx_profile_geog_province ON iPx_profile_geog_country.countryid = iPx_profile_geog_province.countryid AND "
        sSQL += "iPxAcctAP_Cfg_Vendor.provid COLLATE SQL_Latin1_General_CP1_CI_AS = iPx_profile_geog_province.provid "
        sSQL += "INNER JOIN iPx_profile_geog_city ON iPx_profile_geog_province.countryid = iPx_profile_geog_city.countryid AND "
        sSQL += "iPx_profile_geog_province.provid = iPx_profile_geog_city.provid AND iPxAcctAP_Cfg_Vendor.CityID = iPx_profile_geog_city.cityid "
        sSQL += "where iPxAcctAP_Cfg_Vendor.businessid ='" & Session("sBusinessID") & "' AND iPxAcctAP_Cfg_Vendor.VendorID = (select VendorID from iPxAcctAP_PV "
        sSQL += "where iPxAcctAP_PV.VendorID = iPxAcctAP_Cfg_Vendor.VendorID and iPxAcctAP_PV.Status = 'A' and iPxAcctAP_PV.businessid ='" & Session("sBusinessID") & "' group by VendorID) "
        sSQL += "and (select (sum(amountcr)-sum(amountdr)) from iPxAcctAp_Transaction as c where c.VendorID=iPxAcctAP_Cfg_Vendor.VendorID and c.PVno<>'' and c.isActive='Y')>0 "
        sSQL += "AND iPxAcctAP_Cfg_Vendor.IsActive='" & "Y" & "' "
        If Session("sQueryCus") = "" Then
            Session("sQueryCus") = Session("sConditionCus")
            If Session("sQueryCus") <> "" Or Session("sConditionCus") <> "" Then
                sSQL = sSQL & Session("sQueryCus")
                Session("sConditionCus") = ""
            Else
                sSQL = sSQL & ""
            End If
        Else
            sSQL = sSQL & Session("sQueryCus")
            Session("sConditionCus") = ""
        End If
        sSQL += " order by iPxAcctAP_Cfg_Vendor.VendorID asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCustAR.DataSource = dt
                    gvCustAR.DataBind()
                    Dim totalAmount As Decimal
                    If dt.Compute("Sum(amountTrans)", "").ToString() <> "" Then
                        totalAmount = dt.Compute("Sum(amountTrans)", "").ToString()
                    Else
                        totalAmount = 0
                    End If
                    gvCustAR.FooterRow.Cells(6).Text = "Total"
                    gvCustAR.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Right
                    gvCustAR.FooterRow.Cells(7).Text = totalAmount.ToString("N2")
                    gvCustAR.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCustAR.DataSource = dt
                    gvCustAR.DataBind()
                    gvCustAR.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListInvoiceEdit()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAP_PV.*, iPxAcctAP_Cfg_Vendor.CoyName, iPx_profile_user.fullname, "
        sSQL += "(select sum(amountdr) from iPxAcctAP_Transaction "
        sSQL += "where VendorID='" & Session("sReceiptInv") & "' AND transactiontype ='PY' and iPxAcctAP_Transaction.PVno = iPxAcctAP_PV.PVNo) as Debit "
        sSQL += ",(select sum(amountcr) from iPxAcctAP_Transaction "
        sSQL += "where VendorID='" & Session("sReceiptInv") & "' AND transactiontype <>'PY' and iPxAcctAP_Transaction.PVno = iPxAcctAP_PV.PVNo) as Credit "
        sSQL += ",(select (sum(amountcr)-sum(amountdr)) from iPxAcctAP_Transaction where PVno = iPxAcctAP_PV.PVNo) as totalInvoice FROM iPxAcctAP_PV "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_Vendor ON iPxAcctAP_PV.businessid = iPxAcctAP_Cfg_Vendor.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND "
        sSQL += "iPxAcctAP_PV.VendorID COLLATE Latin1_General_CI_AS = iPxAcctAP_Cfg_Vendor.VendorID "
        sSQL += "INNER JOIN iPx_profile_user ON iPxAcctAP_PV.RegBy = iPx_profile_user.id "
        sSQL += "where iPxAcctAP_PV.businessid ='" & Session("sBusinessID") & "' and iPxAcctAP_PV.VendorID ='" & Session("sReceiptInv") & "' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL += " AND iPxAcctAP_PV.Status<>'X'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by iPxAcctAP_PV.DueDate asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvARInv.DataSource = dt
                    gvARInv.DataBind()
                    Dim totalDebit, totalCredit, total As Decimal
                    'Dim totalDebit As Decimal = dt.Compute("Sum(Debit)", "").ToString()
                    If dt.Compute("Sum(Debit)", "").ToString() = "" Then
                        totalDebit = 0
                    Else
                        totalDebit = dt.Compute("Sum(Debit)", "").ToString()
                    End If
                    If dt.Compute("Sum(Credit)", "").ToString() = "" Then
                        totalCredit = 0
                    Else
                        totalCredit = dt.Compute("Sum(Credit)", "").ToString()
                    End If
                    If dt.Compute("Sum(totalInvoice)", "").ToString() = "" Then
                        total = 0
                    Else
                        total = dt.Compute("Sum(totalInvoice)", "").ToString()
                    End If
                    gvARInv.FooterRow.Cells(6).Text = "Total"
                    gvARInv.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Left
                    gvARInv.FooterRow.Cells(7).Text = totalDebit.ToString("N2")
                    gvARInv.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                    gvARInv.FooterRow.Cells(8).Text = totalCredit.ToString("N2")
                    gvARInv.FooterRow.Cells(8).HorizontalAlign = HorizontalAlign.Right
                    gvARInv.FooterRow.Cells(9).Text = total.ToString("N2")
                    gvARInv.FooterRow.Cells(9).HorizontalAlign = HorizontalAlign.Right
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvARInv.DataSource = dt
                    gvARInv.DataBind()
                    gvARInv.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listDetail()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select iPxAcctAP_Transaction.TransID, iPxAcctAP_Transaction.TransDate,iPxAcctAP_Cfg_Vendor.CoyName,iPxAcctAP_Transaction.PVno,iPxAcctAP_Transaction.amountdr from iPxAcctAP_Transaction "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_vendor on iPxAcctAP_Cfg_Vendor.VendorID=iPxAcctAP_Transaction.VendorID "
        sSQL += "where iPxAcctAP_Transaction.businessid ='" & Session("sBusinessID") & "' AND iPxAcctAP_Transaction.transactiontype='PY' and iPxAcctAP_Transaction.TransID ='" & Session("sEditRece") & "' "
        sSQL += " order by iPxAcctAP_Transaction.PVno asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDetail.DataSource = dt
                    gvDetail.DataBind()
                    Dim total As Decimal
                    If dt.Compute("Sum(amountdr)", "").ToString() = "" Then
                        total = 0
                    Else
                        total = dt.Compute("Sum(amountdr)", "").ToString()
                    End If
                    gvDetail.FooterRow.Cells(3).Text = "Total"
                    gvDetail.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Left
                    gvDetail.FooterRow.Cells(4).Text = total.ToString("N2")
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
    Sub editReceipt()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAP_Payment "
        sSQL += "where businessid ='" & Session("sBusinessID") & "' "
        sSQL += "AND PaymentID = '" & Session("sEditRece") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Session("sReceiptInv") = oSQLReader.Item("VendorID").ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub DeleteReceipt()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAP_Payment SET status='X' "
        sSQL = sSQL & "WHERE PaymentID ='" & Session("sSelectCustBathD") & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAP_Transaction SET isActive='N'"
        sSQL = sSQL & "WHERE TransID ='" & Session("sSelectCustBathD") & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub InvoiceNo()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.PVno from iPxAcctAP_Transaction as a "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' "
        sSQL += "AND a.TransID = '" & Session("sSelectCustBathD") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Dim invDel As String = oSQLReader.Item("PVno").ToString
            oCnct.Close()
            findInvPaid(invDel)
        Else
            oCnct.Close()
        End If
    End Sub
    Public Function findInvPaid(ByVal invNo As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.PVno from iPxAcctAP_Transaction as a "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' "
        sSQL += "AND a.PVno = '" & invNo & "' and TransID like 'PY%' and isActive='Y'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            oCnct.Close()
            statusinvoice = "P"
            updateInvoice(invNo)
        Else
            oCnct.Close()
            statusinvoice = "A"
            updateInvoice(invNo)
        End If
    End Function
    Public Function updateInvoice(ByVal invNo As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAP_PV SET Status='" & statusinvoice & "'"
        sSQL = sSQL & " WHERE PVNo ='" & invNo & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Function
    Sub saveReason()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim regDate As Date = Date.Now()
        sSQL = "INSERT INTO iPxAcct_ReasonDelete(businessid,idAcct,dateReason,RegBy,reason,isActive)"
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & Session("sSelectCustBathD") & "','" & regDate & "'"
        sSQL += ",'" & Session("iUserID") & "','" & Replace(tbReason.Text, "'", "''") & "','Y') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

    End Sub
    Public Function Posting(ByVal businessid As String, ByVal TransID As String) As Boolean

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAP_Payment where businessid ='" & businessid & "' and PaymentID='" & TransID & "' "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        While oSQLReader.Read
            'oSQLReader.Close()
            Dim GLid As String = cIpx.GetCounterMBR("GL", "GL")
            saveGLHeader(GLid, businessid, TransID, oSQLReader.Item("PaymentDate"), oSQLReader.Item("Notes"))
            GLDetail(businessid, TransID, GLid)
        End While
        oCnct.Close()

    End Function

    Public Function saveGLHeader(ByVal GLid As String, ByVal businessid As String, ByVal TransID As String, ByVal GLDate As String, ByVal GLDesc As String) As Boolean
        If oCnct1.State = ConnectionState.Closed Then
            oCnct1.Open()
        End If
        oSQLCmd1 = New SqlCommand(sSQL1, oCnct1)

        sSQL1 = "INSERT INTO iPxAcctGL_JVhdr(businessid, TransID, TransDate, Status, JVgroup, ReffNo, Description) "
        sSQL1 += "VALUES ('" & businessid & "','" & GLid & "','" & GLDate & "', "
        sSQL1 += "'O','G4','" & TransID & "','" & GLDesc & "') "
        oSQLCmd1.CommandText = sSQL1
        oSQLCmd1.ExecuteNonQuery()

        oCnct1.Close()
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting header success !');document.getElementById('Buttonx').click()", True)
    End Function
    Public Function GLDetail(ByVal businessid As String, ByVal TransID As String, ByVal GLid As String) As Boolean

        If oCnct2.State = ConnectionState.Closed Then
            oCnct2.Open()
        End If
        oSQLCmd2 = New SqlCommand(sSQL2, oCnct2)
        sSQL2 = "SELECT a.RecID, b.VendorID, b.CoyName, (b.CoaLink) as coaCredit, d.Description, (d.Coa) as coaDebit, a.amountdr "
        sSQL2 += "FROM iPxAcctAP_Transaction as a "
        sSQL2 += "INNER JOIN iPxAcctAP_Cfg_Vendor as b ON a.businessid = b.businessid AND a.VendorID = b.VendorID "
        sSQL2 += "INNER JOIN iPxAcctAP_Payment as c ON c.businessid=a.businessid and c.PaymentID=a.TransID "
        sSQL2 += "INNER JOIN iPxAcctAP_Cfg_Paidby as d ON d.businessid=c.businessid and d.PaidBy=c.PaidBy "
        sSQL2 += "where a.businessid ='" & businessid & "' and a.TransID='" & TransID & "' and a.isActive = 'Y' order by a.RecID asc "
        oSQLCmd2.CommandText = sSQL2
        oSQLReader2 = oSQLCmd2.ExecuteReader
        While oSQLReader2.Read
            saveGLDetailCredit(businessid, GLid, oSQLReader2.Item("coaCredit"), oSQLReader2.Item("CoyName"), TransID, oSQLReader2.Item("amountdr"))
            saveGLDetailDebit(businessid, GLid, oSQLReader2.Item("coaDebit"), oSQLReader2.Item("CoyName"), TransID, oSQLReader2.Item("amountdr"))
        End While
        oCnct2.Close()

    End Function

    Sub idGLDtl()
        Dim oCnct4 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLReader4 As SqlDataReader
        Dim oSQLCmd4 As SqlCommand
        Dim sSQL4 As String
        If oCnct4.State = ConnectionState.Closed Then
            oCnct4.Open()
        End If
        oSQLCmd4 = New SqlCommand(sSQL4, oCnct4)
        sSQL4 = "SELECT MAX(RecID) as RecID FROM iPxAcctGL_JVdtl where TransID = '" & Session("sTransIDGL") & "'"
        oSQLCmd4.CommandText = sSQL4
        oSQLReader4 = oSQLCmd4.ExecuteReader

        oSQLReader4.Read()
        If oSQLReader4.HasRows Then
            RecID = Val(oSQLReader4.Item("RecID").ToString) + 1

        Else
            RecID = "1"
        End If
        oCnct4.Close()
    End Sub
    Public Function saveGLDetailDebit(ByVal businessid As String, ByVal TransID As String, ByVal Coa As String, ByVal Desc As String, ByVal Reff As String, ByVal Amount As String) As Boolean
        Dim oCnct3 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd3 As SqlCommand
        Dim sSQL3 As String
        Session("sTransIDGL") = TransID
        idGLDtl()
        If oCnct3.State = ConnectionState.Closed Then
            oCnct3.Open()
        End If
        oSQLCmd3 = New SqlCommand(sSQL3, oCnct3)
        sSQL3 = "INSERT INTO iPxAcctGL_JVdtl(RecID, businessid, TransID, Coa, Description, Reff, Debit, Credit, isActive) "
        sSQL3 += "VALUES ('" & RecID & "','" & businessid & "','" & TransID & "','" & Coa & "', "
        sSQL3 += "'" & Desc & "','" & Reff & "','0','" & Amount & "', "
        sSQL3 += "'Y') "
        oSQLCmd3.CommandText = sSQL3
        oSQLCmd3.ExecuteNonQuery()

        oCnct3.Close()
    End Function
    Public Function saveGLDetailCredit(ByVal businessid As String, ByVal TransID As String, ByVal Coa As String, ByVal Desc As String, ByVal Reff As String, ByVal Amount As String) As Boolean
        Dim oCnct3 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd3 As SqlCommand
        Dim sSQL3 As String
        Session("sTransIDGL") = TransID
        idGLDtl()
        If oCnct3.State = ConnectionState.Closed Then
            oCnct3.Open()
        End If
        oSQLCmd3 = New SqlCommand(sSQL3, oCnct3)
        sSQL3 = "INSERT INTO iPxAcctGL_JVdtl(RecID, businessid, TransID, Coa, Description, Reff, Debit, Credit, isActive) "
        sSQL3 += "VALUES ('" & RecID & "','" & businessid & "','" & TransID & "','" & Coa & "', "
        sSQL3 += "'" & Desc & "','" & Reff & "','" & Amount & "','0', "
        sSQL3 += "'Y') "
        oSQLCmd3.CommandText = sSQL3
        oSQLCmd3.ExecuteNonQuery()

        oCnct3.Close()
    End Function
    Sub EditPosting()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAP_Payment SET Status='P'"
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and PaymentID='" & Session("sReceiptID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('the data was successfully posted !!');", True)
        ListReceipt()
    End Sub
    Sub cekCloseGrid()
        Dim oCnct4 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLReader4 As SqlDataReader
        Dim oSQLCmd4 As SqlCommand
        Dim sSQL4 As String
        If oCnct4.State = ConnectionState.Closed Then
            oCnct4.Open()
        End If
        oSQLCmd4 = New SqlCommand(sSQL4, oCnct4)
        sSQL4 = "SELECT * FROM iPxAcctGL_JVhdr where businessid='" & Session("sBusinessID") & "' "
        sSQL4 += "and ReffNo like 'PL " & MonthImport & "-" & YearImport & "' and status<>'D'"
        oSQLCmd4.CommandText = sSQL4
        oSQLReader4 = oSQLCmd4.ExecuteReader

        oSQLReader4.Read()
        If oSQLReader4.HasRows Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting process failed, this periode is closed !');document.getElementById('Buttonx').click()", True)
        Else
            Posting(Session("sBusinessID"), Session("sReceiptID"))
            EditPosting()
            Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
            cIpx.saveLog(Session("sBusinessID"), Session("sReceiptID"), TransDateLog, "AP PAY", "Posting", "Post AP Payment " & Session("sReceiptID"), "", Session("sUserCode"))
        End If
        oCnct4.Close()
    End Sub
    Sub Docket()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select * from iPxAcctDocket_Report WHERE businessid in ('','" & Session("sBusinessID") & "') and Grp='PY'"
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Me.IsPostBack Then
            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "AR Receipt") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            Session("sQueryTicket") = ""
            Session("sQueryCus") = ""
            If Session("sDateWorkAPPAY") = "" Then
                tbDate.Text = Format(Now, "MM-yyyy")
            Else
                tbDate.Text = Session("sDateWorkAPPAY")
            End If
            ListReceipt()
        Else
            ListReceipt()
        End If
        UserAcces()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthGL", "$(document).ready(function() {MonthGL()});", True)
    End Sub
    Protected Sub TransdateWork(ByVal sender As Object, ByVal e As EventArgs)
        Session("sDateWorkARRec") = tbDate.Text
        Session("sQueryTicket") = ""
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvARRec.PageIndex = e.NewPageIndex
        Me.ListReceipt()
        gvCustAR.PageIndex = e.NewPageIndex
        Me.listCustomer()
    End Sub

    Protected Sub gvARRec_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvARRec.PageIndexChanging
        gvARRec.PageIndex = e.NewPageIndex
        ListReceipt()
        pnCustGroup.Visible = False
        pnARRec.Visible = True
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvARRec.PageIndex = e.NewPageIndex
        Me.ListReceipt()
        gvCustAR.PageIndex = e.NewPageIndex
        Me.listCustomer()
    End Sub
    Protected Sub lbAddRec_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddRec.Click
        pnCustGroup.Visible = True
        pnARRec.Visible = False
        listCustomer()
        Me.lbAddRec.Enabled = False
        lbView.Visible = True
    End Sub

    Protected Sub gvCustAR_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCustAR.PageIndexChanging
        gvCustAR.PageIndex = e.NewPageIndex
        listCustomer()
        pnCustGroup.Visible = True
        pnARRec.Visible = False
    End Sub

    Protected Sub gvCustAR_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCustAR.RowCommand
        If e.CommandName = "getSelect" Then
            Session("sReceiptInv") = e.CommandArgument
            Session("sEditRece") = ""
            Response.Redirect("iPxAPInputPayment.aspx")
        End If
    End Sub

    Protected Sub gvARRec_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvARRec.RowCommand
        If e.CommandName = "getEdit" Then
            Session("sEditRece") = e.CommandArgument
            Response.Redirect("iPxAPInputPayment.aspx")
        ElseIf e.CommandName = "getDetail" Then
            Session("sEditRece") = e.CommandArgument
            pnListDetail.Visible = True
            pnListDep.Visible = False
            listDetail()
            lbTitle.Text = "Detail Payment"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        ElseIf e.CommandName = "getDeposito" Then
            Session("sEditRece") = e.CommandArgument
            pnListDep.Visible = True
            pnListDetail.Visible = False
            editReceipt()
            ListInvoiceEdit()
            lbTitle.Text = "Saldo Balance"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        ElseIf e.CommandName = "getPrint" Then
            'Dim cIpx As New iPxClass
            'Session("sReport") = "ARRecept"
            Dim a As String = e.CommandArgument
            Session("sTransID") = e.CommandArgument
            'Session("sMapPath") = "~/iPxReportFile/dckAR_Receipt.rpt"
            'Response.Redirect("rptviewer.aspx")
            Docket()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDocket", "showModalDocket()", True)
        ElseIf e.CommandName = "getDelete" Then
            Session("sSelectCustBathD") = e.CommandArgument
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
        ElseIf e.CommandName = "getPosting" Then
            Session("sReceiptID") = e.CommandArgument
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT a.TransID, b.PaymentDate, "
            sSQL += "(SELECT COUNT(b.CoaLink) FROM iPxAcctAP_Transaction as a "
            sSQL += "INNER JOIN iPxAcctAP_Cfg_Vendor as b ON a.businessid = b.businessid AND a.VendorID = b.VendorID "
            sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.TransID='" & Session("sReceiptID") & "' and a.isActive = 'Y' and b.CoaLink='') as coaCredit, "
            sSQL += "(SELECT COUNT(c.Coa) FROM iPxAcctAP_Payment as a INNER JOIN iPxAcctAP_Cfg_Paidby as c "
            sSQL += " ON a.businessid COLLATE SQL_Latin1_General_CP1_CI_AS = c.businessid "
            sSQL += "AND a.PaidBy = c.PaidBy  where a.businessid ='" & Session("sBusinessID") & "' and a.PaymentID='" & Session("sReceiptID") & "' and a.isActive = 'Y' and c.Coa='') as coaDebit "
            sSQL += "FROM iPxAcctAP_Transaction as a "
            sSQL += "INNER JOIN iPxAcctAP_Payment as b ON b.businessid=a.businessid and b.PaymentID=a.TransID "
            sSQL += "WHERE a.TransID = '" & Session("sReceiptID") & "' and a.businessid='" & Session("sBusinessID") & "' group by a.TransID, b.PaymentDate"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            While oSQLReader.Read
                coaCredit = oSQLReader.Item("coaCredit").ToString
                coaDebit = oSQLReader.Item("coaDebit").ToString
                MonthImport = Strings.Mid(Format(oSQLReader.Item("PaymentDate"), "dd-MM-yyy").ToString, 4, 2)
                YearImport = Strings.Right(Format(oSQLReader.Item("PaymentDate"), "dd-MM-yyy").ToString, 4)
            End While
            oSQLReader.Close()
            If MonthImport <> "" And coaCredit = "0" And coaDebit = "0" Then
                cekCloseGrid()
            ElseIf MonthImport = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('posting process failed !');document.getElementById('Buttonx').click()", True)
            ElseIf coaCredit <> "0" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting process failed, COA Credit is empty !');document.getElementById('Buttonx').click()", True)
            ElseIf coaDebit <> "0" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting process failed, COA Debit is empty !');document.getElementById('Buttonx').click()", True)
            End If
        End If
    End Sub

    Protected Sub lbQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQuery.Click
        If lbView.Visible = False Then
            pnRecept.Visible = True
            pnCust.Visible = False
            Paidby()
            tbQCus.Text = ""
            tbQFrom.Text = ""
            tbQRecDate.Text = ""
            tbQRecID.Text = ""
            tbQReff.Text = ""
            tbQUntil.Text = ""
            dlQStatus.Items.Clear()
            showdata_dropdownCustStatus()
            Session("sQueryTicket") = ""
        Else
            Session("sQueryCus") = ""
            pnRecept.Visible = False
            pnCust.Visible = True
            country()
            province()
            city()
            tbQCoyName.Text = ""
            tbQEmail.Text = ""
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub lbAbortQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortQuery.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
    End Sub

    Protected Sub lblQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblQuery.Click
        If lbView.Visible = False Then
            If dlQStatus.SelectedIndex = 0 Then
                If tbQRecDate.Text = "" And tbQFrom.Text = "" And tbQUntil.Text = "" Then
                    month = Left(tbDate.Text, 2)
                    year = Right(tbDate.Text, 4)
                    Session("sCondition") = Session("sCondition") & " AND month(iPxAcctAP_Payment.PaymentDate)='" & month & "' AND year(iPxAcctAP_Payment.PaymentDate)='" & year & "' AND iPxAcctAP_Payment.Status<>'X' "
                ElseIf tbQRecDate.Text <> "" Or tbQFrom.Text <> "" Or tbQUntil.Text <> "" Then
                    Session("sCondition") = Session("sCondition") & " AND iPxAcctAP_Payment.Status<>'X' "
                    tbDate.Text = Format(Now, "MM-yyyy")
                    Session("sDateWorkAPPAY") = tbDate.Text
                End If
            ElseIf dlQStatus.SelectedIndex = 1 Then
                Session("sCondition") = Session("sCondition") & " and iPxAcctAP_Payment.Status = 'N' "
            ElseIf dlQStatus.SelectedIndex = 2 Then
                Session("sCondition") = Session("sCondition") & " and iPxAcctAP_Payment.Status = 'X'"
            End If
            If tbQRecID.Text.Trim <> "" Then
                Session("sCondition") = Session("sCondition") & " and (iPxAcctAP_Payment.PaymentID like '%" & Replace(tbQRecID.Text.Trim, "'", "''") & "%') "
            End If
            If tbQCus.Text.Trim <> "" Then
                Session("sCondition") = Session("sCondition") & " and (iPxAcctAP_Cfg_Vendor.CoyName like '%" & Replace(tbQCus.Text.Trim, "'", "''") & "%') "
            End If
            If tbQReff.Text.Trim <> "" Then
                Session("sCondition") = Session("sCondition") & " and (iPxAcctAP_Payment.ReffNo like '%" & Replace(tbQReff.Text.Trim, "'", "''") & "%') "
            End If
            If dlPaid.Text.Trim <> "" Then
                Session("sCondition") = Session("sCondition") & " and (iPxAcctAP_Payment.PaidBy = '" & dlPaid.SelectedValue & "') "
            End If
            If tbQRecDate.Text <> "" Then
                Dim RecDate As Date
                RecDate = Date.ParseExact(tbQRecDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                Session("sCondition") = Session("sCondition") & " and (iPxAcctAP_Payment.PaymentDate = '" & RecDate & "') "
            End If
            If tbQFrom.Text.Trim <> "" And tbQUntil.Text.Trim <> "" Then
                Dim PerFrom As Date = Date.ParseExact(tbQFrom.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                Dim PerUntl As Date = Date.ParseExact(tbQUntil.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                Session("sCondition") = Session("sCondition") & " AND (iPxAcctAP_Payment.PaymentDate >= '" & PerFrom & "') AND (iPxAcctAP_Payment.PaymentDate <= '" & PerUntl & "') "
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
            ListReceipt()
        Else
            If tbQCoyName.Text.Trim <> "" Then
                Session("sConditionCus") = Session("sConditionCus") & " and (iPxAcctAP_Cfg_Vendor.CoyName like '%" & Replace(tbQCoyName.Text, "'", "''") & "%') "
            End If
            If dlQCountry.Text.Trim <> "" Then
                Session("sConditionCus") = Session("sConditionCus") & " and (iPxAcctAP_Cfg_Vendor.CountryId = '" & dlQCountry.SelectedValue & "') "
            End If
            If dlQProvince.Text.Trim <> "" Then
                Session("sConditionCus") = Session("sConditionCus") & " and (iPxAcctAP_Cfg_Vendor.provid = '" & dlQProvince.SelectedValue & "') "
            End If
            If dlQCity.Text.Trim <> "" Then
                Session("sConditionCus") = Session("sConditionCus") & " and (iPxAcctAP_Cfg_Vendor.CityID= '" & dlQCity.SelectedValue & "') "
            End If
            If tbQEmail.Text.Trim <> "" Then
                Session("sConditionCus") = Session("sConditionCus") & " and (iPxAcctAP_Cfg_Vendor.Email like '%" & Replace(tbQEmail.Text, "'", "''") & "%') "
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
            listCustomer()
        End If
    End Sub

    Protected Sub lbView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbView.Click
        pnCustGroup.Visible = False
        pnARRec.Visible = True
        ListReceipt()
        Me.lbAddRec.Enabled = True
        Me.lbView.Visible = False
    End Sub

    Protected Sub dlQCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlQCountry.SelectedIndexChanged
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        province()
        city()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub dlQProvince_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlQProvince.SelectedIndexChanged
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        city()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub lbAbortDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDelete.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
    End Sub

    Protected Sub lbDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbDelete.Click
        If tbReason.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter your reason first !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
        Else
            'saveReason()
            Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
            cIpx.saveLog(Session("sBusinessID"), Session("sSelectCustBathD"), TransDateLog, "AP PAY", "Delete", Replace(tbReason.Text, "'", "''"), "", Session("sUserCode"))

            DeleteReceipt()
            InvoiceNo()
            ListReceipt()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
        End If
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Protected Sub lbAbortDocket_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDocket.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDocket", "hideModalDocket()", True)
    End Sub

    Protected Sub gvDocket_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDocket.RowCommand
        If e.CommandName = "getDocket" Then
            Dim cIpx As New iPxClass
            Session("sReport") = "APPayment"
            Session("sMapPath") = "~/iPxReportFile/" + e.CommandArgument
            Response.Redirect("rptviewer.aspx")
        End If
    End Sub
End Class
