Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data.SqlClient
Imports CrystalDecisions.Shared
Partial Class iPxPMS_rptviewer
    Inherits System.Web.UI.Page

    Public reportdocument As New ReportDocument()
    Dim strCn As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim cn As SqlConnection = New SqlConnection(strCn)
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        Else
            'Dim reportdocument As New ReportDocument()


            '----------------------------
            Dim builder As New SqlConnectionStringBuilder(strCn)
            Dim dbName As String = builder.InitialCatalog
            Dim dbDataSource As String = builder.DataSource
            Dim userId As String = builder.UserID
            Dim pass As String = builder.Password
            reportdocument.Load(Server.MapPath(Session("sMapPath")))
            reportdocument.SetDatabaseLogon(userId, pass, dbDataSource, dbName)
            'reportdocument.SetDatabaseLogon(ConfigurationManager.AppSettings("UID"), ConfigurationManager.AppSettings("PWD"), ConfigurationManager.AppSettings("SVR"), ConfigurationManager.AppSettings("DB"))

            reportdocument.SetParameterValue("sBusinessID", Session("sBusinessID"))

            If Session("sReport") = "SalesSummary" Then
                reportdocument.SetParameterValue("sFOLink", Session("sFoLink"))
                reportdocument.SetParameterValue("ForDate", Session("sDate"))
            ElseIf Session("sReport") = "SalesSummaryDtl" Then
                reportdocument.SetParameterValue("sFOLink", Session("sFoLink"))
                reportdocument.SetParameterValue("ForDate", Session("sDate"))
                reportdocument.SetParameterValue("Code", Session("sCode"))
            ElseIf Session("sReport") = "ARTrans" Then
                reportdocument.SetParameterValue("sFOLink", Session("sFoLink"))
                reportdocument.SetParameterValue("TransactionID", Session("sTransID"))
            ElseIf Session("sReport") = "ARInvoice" Then
                reportdocument.SetParameterValue("sFOLink", Session("sFoLink"))
                reportdocument.SetParameterValue("InvoiceNo", Session("sTransID"))
            ElseIf Session("sReport") = "ARRecept" Then
                reportdocument.SetParameterValue("sFOLink", "")
                reportdocument.SetParameterValue("ReceiptNo", Session("sTransID"))
            ElseIf Session("sReport") = "GLJurnal" Then
                reportdocument.SetParameterValue("sFOLink", "")
                reportdocument.SetParameterValue("TransactionID", Session("sTransID"))
            ElseIf Session("sReport") = "GLSummary" Then
                reportdocument.SetParameterValue("sFOLink", "")
                reportdocument.SetParameterValue("FiscalPeriod", Session("sPeriod"))
            ElseIf Session("sReport") = "GLTrialBalance" Then
                reportdocument.SetParameterValue("sFOLink", "")
                reportdocument.SetParameterValue("FiscalPeriod", Session("sPeriod"))
            ElseIf Session("sReport") = "GLBudget1" Then
                reportdocument.SetParameterValue("FiscalPeriod", Session("sPeriod"))
            ElseIf Session("sReport") = "GLBudget2" Then
                reportdocument.SetParameterValue("FiscalPeriod", Session("sPeriod"))
            ElseIf Session("sReport") = "GLTrialBalanceDtl" Then
                reportdocument.SetParameterValue("FiscalPeriod", Session("sPeriod"))
                reportdocument.SetParameterValue("Coa", Session("sCoaDtl"))
            ElseIf Session("sReport") = "ARAging" Then
                reportdocument.SetParameterValue("sUserId", Session("sUserCode"))
                reportdocument.SetParameterValue("ForDate", Session("sForDate"))
            ElseIf Session("sReport") = "ARSummary" Then
                reportdocument.SetParameterValue("sUserId", Session("sUserCode"))
                reportdocument.SetParameterValue("ForDate", Session("sForDate"))
            ElseIf Session("sReport") = "ARSummaryDtl" Then
                reportdocument.SetParameterValue("FiscalPeriod", Session("sForDate"))
                reportdocument.SetParameterValue("CustomerCode", Session("sCustSummaryCC"))
            ElseIf Session("sReport") = "GLSummaryBalance" Then
                reportdocument.SetParameterValue("sFOLink", "")
                reportdocument.SetParameterValue("FiscalPeriod", Session("sPeriod"))
            ElseIf Session("sReport") = "APTrans" Then
                'reportdocument.SetParameterValue("sFOLink", Session("sFoLink"))
                reportdocument.SetParameterValue("TransactionID", Session("sTransID"))
            ElseIf Session("sReport") = "APPayment" Then
                'reportdocument.SetParameterValue("sFOLink", "")
                reportdocument.SetParameterValue("TransactionID", Session("sTransID"))
            ElseIf Session("sReport") = "APVoucher" Then
                'reportdocument.SetParameterValue("sFOLink", Session("sFoLink"))
                reportdocument.SetParameterValue("TransactionID", Session("sTransID"))
            End If
            CrystalReportViewer1.ReportSource = reportdocument
            'CrystalReportViewer1.PrintMode = CrystalDecisions.Web.PrintMode.Pdf
            'reportdocument.PrintToPrinter(1, True, 0, 0)
            Dim myConnectionInfo As New ConnectionInfo()

            myConnectionInfo.UserID = userId
            myConnectionInfo.Password = pass

            setDBLOGONforREPORT(myConnectionInfo)
        End If
    End Sub
    Private Sub setDBLOGONforREPORT(ByVal myconnectioninfo As ConnectionInfo)
        Dim mytableloginfos As New TableLogOnInfos()
        mytableloginfos = CrystalReportViewer1.LogOnInfo
        For Each myTableLogOnInfo As TableLogOnInfo In mytableloginfos
            myTableLogOnInfo.ConnectionInfo = myconnectioninfo
        Next

    End Sub

    Protected Sub lbAbort_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbort.Click
        If Session("sReport") = "SalesSummary" Then
            Response.Redirect("iPxSalesSummary.aspx")
        ElseIf Session("sReport") = "SalesSummaryDtl" Then
            Response.Redirect("iPxSalesSummary.aspx")
        ElseIf Session("sReport") = "ARTrans" Then
            Response.Redirect("iPxARTransaction.aspx")
        ElseIf Session("sReport") = "ARInvoice" Then
            Response.Redirect("iPxARInvoice.aspx")
        ElseIf Session("sReport") = "ARRecept" Then
            Response.Redirect("iPxARReceipt.aspx")
        ElseIf Session("sReport") = "GLJurnal" Then
            Response.Redirect("iPxGLTransaction.aspx")
        ElseIf Session("sReport") = "GLSummary" Then
            Response.Redirect("iPxGLSummary.aspx")
        ElseIf Session("sReport") = "GLTrialBalance" Then
            Response.Redirect("iPxGLTrialBalance.aspx")
        ElseIf Session("sReport") = "GLSummaryBalance" Then
            Response.Redirect("iPxGLSummaryBalanceSheet.aspx")
        ElseIf Session("sReport") = "GLTrialBalanceDtl" Then
            Response.Redirect("iPxGLTrialBalance.aspx")
        ElseIf Session("sReport") = "ARAging" Then
            Response.Redirect("iPxARAging.aspx")
        ElseIf Session("sReport") = "ARSummary" Then
            Response.Redirect("iPxARSummary.aspx")
        ElseIf Session("sReport") = "ARSummaryDtl" Then
            Response.Redirect("iPxARSummary.aspx")
        ElseIf Session("sReport") = "GLBudget1" Or Session("sReport") = "GLBudget2" Then
            Response.Redirect("iPxGLBudget.aspx")
        ElseIf Session("sReport") = "APTrans" Then
            Response.Redirect("iPxAPTransaction.aspx")
        ElseIf Session("sReport") = "APPayment" Then
            Response.Redirect("iPxAPPayment.aspx")
        ElseIf Session("sReport") = "APVoucher" Then
            Response.Redirect("iPxAPVoucher.aspx")
        ElseIf Session("sReport") = "GLCOA" Then
            Response.Redirect("iPxCoa.aspx")
        End If
    End Sub
End Class

