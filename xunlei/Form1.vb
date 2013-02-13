Imports System.Text.RegularExpressions
Imports System.Runtime.InteropServices
Imports System.Threading

Public Class Form1
    Private Download As DownloadManager
    Private taskID As Integer
    Private msgBoxTitle = "Xunlei Download"
    Private Delegate Sub progressChanged(ByVal percentage As Double)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        l_status.Text = "Ready"
        l_percentage.Text = "0.0%"
        l_size.Text = "0KB/0KB"

        Download = New DownloadManager()
        AddHandler Download.percentageChanged, AddressOf progressChangedCallback
        AddHandler Download.recievedSizeChanged, AddressOf recievedSizeChangedCallback
        AddHandler Download.statusChanged, AddressOf statusChangedCallback
        AddHandler Download.statusConnect, AddressOf statusConnectCallback
        AddHandler Download.statusSuccess, AddressOf statusSuccessCallback
        AddHandler Download.statusPause, AddressOf statusPauseCallback
        AddHandler Download.statusCancel, AddressOf statusCancelCallback
        AddHandler Download.statusResume, AddressOf statusResumeCallback
    End Sub

    Private Sub btn_folder_Click(sender As Object, e As EventArgs) Handles btn_folder.Click
        dialog_file.FileName = DownloadManager.GetFileNameFromURL(t_url.Text)
        If (dialog_file.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            Me.t_save_directory.Text = dialog_file.FileName
        End If
    End Sub

    Private Sub btn_download_Click(sender As Object, e As EventArgs) Handles btn_download.Click
        taskID = Download.add(t_save_directory.Text, t_url.Text)
    End Sub

    Private Sub btn_resume_Click(sender As Object, e As EventArgs)
        Download.resumeTask(taskID)
    End Sub

    Public Sub progressChangedCallback(ByVal taskID As Integer, ByVal percentage As Double)
        If Me.InvokeRequired Then
            Me.Invoke(New DownloadManager.percentageChangedDelegate(AddressOf progressChangedCallback), {taskID, percentage})
        Else
            ProgressBar1.Value = percentage * 100
            l_percentage.Text = Format(percentage * 100, "0.0") & "%"
        End If
    End Sub

    Public Sub statusSuccessCallback(ByVal taskID As Integer)
        If Me.InvokeRequired Then
            Me.Invoke(New DownloadManager.statusSuccessDelegate(AddressOf statusSuccessCallback), {taskID})
        Else
            btn_download.Enabled = True
            btn_pause.Enabled = False
            btn_cancel.Enabled = False
            ProgressBar1.Value = 100
            l_percentage.Text = "100%"
            MsgBox("Download finished!", MsgBoxStyle.OkOnly, msgBoxTitle)
            ProgressBar1.Value = 0
            l_percentage.Text = "0.0%"
            l_status.Text = "Ready"
            l_size.Text = "0KB/0KB"
        End If
    End Sub

    Public Sub statusPauseCallback(ByVal taskID As Integer)
        If Me.InvokeRequired Then
            Me.Invoke(New DownloadManager.statusPauseDelegate(AddressOf statusPauseCallback), {taskID})
        Else
            RemoveHandler btn_download.Click, AddressOf btn_download_Click
            AddHandler btn_download.Click, AddressOf btn_resume_Click
            btn_download.Enabled = True
            btn_pause.Enabled = False
        End If
    End Sub

    Public Sub statusResumeCallback(ByVal taskID As Integer)
        If Me.InvokeRequired Then
            Me.Invoke(New DownloadManager.statusResumeDelegate(AddressOf statusResumeCallback), {taskID})
        Else
            RemoveHandler btn_download.Click, AddressOf btn_resume_Click
            AddHandler btn_download.Click, AddressOf btn_download_Click
            btn_download.Enabled = False
            btn_pause.Enabled = True
        End If
    End Sub

    Public Sub statusCancelCallback(ByVal taskID As Integer)
        If Me.InvokeRequired Then
            Me.Invoke(New DownloadManager.statusCancelDelegate(AddressOf statusCancelCallback), {taskID})
        Else
            btn_download.Enabled = True
            btn_pause.Enabled = False
            btn_cancel.Enabled = False
            ProgressBar1.Value = 0
            l_percentage.Text = "0.0%"
            l_status.Text = "Ready"
            l_size.Text = "0KB/0KB"
        End If
    End Sub

    Public Sub statusConnectCallback(ByVal taskID As Integer)
        If Me.InvokeRequired Then
            Me.Invoke(New DownloadManager.statusConnectDelegate(AddressOf statusConnectCallback), {taskID})
        Else
            btn_download.Enabled = False
            btn_pause.Enabled = True
            btn_cancel.Enabled = True
        End If
    End Sub

    Public Sub statusChangedCallback(ByVal taskID As Integer, ByVal status As String)
        If Me.InvokeRequired Then
            Me.Invoke(New DownloadManager.statusChangedDelegate(AddressOf statusChangedCallback), {taskID, status})
        Else
            l_status.Text = StrConv(status, VbStrConv.ProperCase)
        End If
    End Sub

    Public Sub recievedSizeChangedCallback(ByVal taskID As Integer, ByVal recievedSize As ULong, ByVal fileSize As ULong)
        If Me.InvokeRequired Then
            Me.Invoke(New DownloadManager.recievedSizeChangedDelegate(AddressOf recievedSizeChangedCallback), {taskID, recievedSize, fileSize})
        Else
            l_size.Text = DownloadManager.FormatFileSize(recievedSize) & "/" & DownloadManager.FormatFileSize(fileSize)
        End If
    End Sub

    Private Sub btn_exit_Click(sender As Object, e As EventArgs) Handles btn_exit.Click
        End
    End Sub

    Private Sub btn_pause_Click(sender As Object, e As EventArgs) Handles btn_pause.Click
        Download.pause(taskID)
    End Sub

    Private Sub btn_cancel_Click(sender As Object, e As EventArgs) Handles btn_cancel.Click
        Download.cancel(taskID)
    End Sub
End Class
