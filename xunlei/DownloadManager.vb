Imports System.Threading
Imports System.IO
Imports System.Text.RegularExpressions

Public Class DownloadManager

#Region " Define Variables "
    Private taskList As List(Of TaskInfo)
    Private DownloadEngine As ThunderEngine
    Private loopTimer As Timer
#End Region

#Region " Define Delegates and Events "
    Public Delegate Sub percentageChangedDelegate(ByVal taskID As Integer, ByVal percentage As Double)
    Public Delegate Sub idChangedDelegate(ByVal taskID As Integer, ByVal taskID As Integer)
    Public Delegate Sub fileSizeChangedDelegate(ByVal taskID As Integer, ByVal fileSize As ULong)
    Public Delegate Sub recievedSizeChangedDelegate(ByVal taskID As Integer, ByVal receivedSize As ULong, ByVal fileSize As ULong)
    Public Delegate Sub statusChangedDelegate(ByVal taskID As Integer, ByVal status As String)
    Public Delegate Sub statusConnectDelegate(ByVal taskID As Integer)
    Public Delegate Sub statusDownloadDelegate(ByVal taskID As Integer)
    Public Delegate Sub statusPauseDelegate(ByVal taskID As Integer)
    Public Delegate Sub statusSuccessDelegate(ByVal taskID As Integer)
    Public Delegate Sub statusFailDelegate(ByVal taskID As Integer)
    Public Delegate Sub statusCancelDelegate(ByVal taskID As Integer)
    Public Delegate Sub statusResumeDelegate(ByVal taskID As Integer)

    Public Event percentageChanged As percentageChangedDelegate
    Public Event idChanged As idChangedDelegate
    Public Event fileSizeChanged As fileSizeChangedDelegate
    Public Event recievedSizeChanged As recievedSizeChangedDelegate
    Public Event statusChanged As statusChangedDelegate
    Public Event statusConnect As statusConnectDelegate
    Public Event statusDownload As statusDownloadDelegate
    Public Event statusPause As statusPauseDelegate
    Public Event statusSuccess As statusSuccessDelegate
    Public Event statusFail As statusFailDelegate
    Public Event statusCancel As statusCancelDelegate
    Public Event statusResume As statusResumeDelegate
#End Region

#Region " Define Structures "
    Public Structure TaskInfo
        Public filePath As String
        Public fileURL As String
        Public refURL As String
        Public id As Integer
        Public percentage As Double
        Public status As String
        Public fileSize As ULong
        Public recievedSize As ULong
    End Structure
#End Region

#Region " Private Functions "
    Public Sub New()
        Me.DownloadEngine = New ThunderEngine()
        Me.taskList = New List(Of TaskInfo)
        Me.loopTimer = New Timer(New TimerCallback(AddressOf mainLoop), Nothing, 0, 500)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Function renameCoincidentFile(ByVal filePath As String) As String
        If File.Exists(filePath) Then
            Dim fileName = Path.GetFileNameWithoutExtension(filePath)
            Dim fileExtension = Path.GetExtension(filePath)
            Dim match As Match = Regex.Match(fileName, "\((\d+)\)$")
            If match.Success Then
                Dim index As Integer = match.Groups(1).Value + 1
                fileName = Regex.Replace(fileName, "\((\d+)\)$", "(" & index & ")")
            Else
                fileName += "(1)"
            End If
            Return renameCoincidentFile(getDirectoryPath(filePath) & fileName & fileExtension)
        Else
            Return filePath
        End If
    End Function

    Private Sub mainLoop()
        If Me.taskList.Count > 0 Then
            On Error Resume Next
            For i = 0 To Me.taskList.Count - 1
                updateTaskInfo(i)
                Dim task As TaskInfo = Me.taskList(i)
                Select Case task.status
                    Case "download"
                        Dim progress As Double = task.recievedSize / task.fileSize
                        If progress > task.percentage Then
                            task.percentage = progress
                            RaiseEvent percentageChanged(i, progress)
                        End If
                End Select
            Next
        End If
    End Sub

    Private Sub handleError()
        Dim errorInfo = Me.DownloadEngine.getLastError()
        Debug.Print("Error: " & errorInfo.id & ", " & errorInfo.message)
        'MsgBox("Error: " & errorInfo.id & vbNewLine & errorInfo.message, MsgBoxStyle.OkOnly)
    End Sub

    Private Sub updateTaskInfo(ByVal taskID As Integer)
        Dim task As TaskInfo = Me.taskList(taskID)
        Dim taskIndex As Integer = task.id
        Try
            Dim newTask = Me.DownloadEngine.queryTask(taskIndex)
            Debug.Print(newTask.status)
            If Not task.id = newTask.id Then
                task.id = newTask.id
                RaiseEvent idChanged(taskID, newTask.id)
            End If
            If Not task.fileSize = newTask.fileSize Then
                task.fileSize = newTask.fileSize
                RaiseEvent fileSizeChanged(taskID, newTask.fileSize)
            End If
            If Not task.recievedSize = newTask.recievedSize Then
                task.recievedSize = newTask.recievedSize
                RaiseEvent recievedSizeChanged(taskID, newTask.recievedSize, newTask.fileSize)
            End If
            If Not task.Status = newTask.status Then
                task.Status = newTask.status
                RaiseEvent statusChanged(taskID, newTask.status)
                Select Case newTask.status
                    Case "connect"
                        RaiseEvent statusConnect(taskID)
                    Case "download"
                        RaiseEvent statusDownload(taskID)
                    Case "pause"
                        RaiseEvent statusPause(taskID)
                    Case "success"
                        taskList.RemoveAt(taskID)
                        DownloadEngine.stopTask(taskIndex)
                        RaiseEvent statusSuccess(taskID)
                    Case "fail"
                        RaiseEvent statusFail(taskID)
                End Select
            End If

            Me.taskList(taskID) = task
        Catch ex As Exception
            handleError()
        End Try
    End Sub
#End Region

#Region " Public Functions "
    Public Shared Function GetFileNameFromURL(ByVal URL As String) As String
        Try
            Return URL.Substring(URL.LastIndexOf("/") + 1)
        Catch ex As Exception
            Return URL
        End Try
    End Function

    Public Shared Function FormatFileSize(ByVal fileSizeBytes As Long) As String
        Dim sizeTypes() As String = {"b", "Kb", "Mb", "Gb"}
        Dim len As Decimal = fileSizeBytes
        Dim sizeType As Integer = 0
        Do While len > 1024
            len = Decimal.Round(len / 1024, 2)
            sizeType += 1
            If sizeType >= sizeTypes.Length - 1 Then Exit Do
        Loop
        Dim retval As String = len.ToString & " " & sizeTypes(sizeType)
        Return retval
    End Function

    Public Shared Function getDirectoryPath(ByVal filePath As String) As String
        Return Left(filePath, filePath.LastIndexOf(Path.GetFileName(filePath)))
    End Function

    Public Function add(ByRef filePath As String, ByVal fileURL As String, Optional ByVal refURL As String = "") As Integer
        If Directory.Exists(filePath) Then
            filePath = Regex.Replace(filePath, "[\\\/]+$", "\") & GetFileNameFromURL(fileURL)
        End If
        filePath = renameCoincidentFile(filePath)
        Dim result = DownloadEngine.addTask(filePath, fileURL, refURL)
        If result < 0 Then
            handleError()
            Return -1
        End If
        Dim newTask = New TaskInfo()
        newTask.filePath = filePath
        newTask.fileURL = fileURL
        newTask.refURL = refURL
        newTask.id = result
        Me.taskList.Add(newTask)
        Return Me.taskList.IndexOf(newTask)
    End Function

    Public Sub pause(ByVal taskID As Integer)
        Dim newID As Integer
        Dim result = DownloadEngine.pauseTask(Me.taskList(taskID).id, newID)
        If Not result Then
            handleError()
        End If
        Dim newTask As TaskInfo = Me.taskList(taskID)
        newTask.id = newID
        Me.taskList(taskID) = newTask
    End Sub

    Public Sub resumeTask(ByVal taskID)
        Dim result = DownloadEngine.continueTask(Me.taskList(taskID).id)
        If Not result Then
            handleError()
        End If
        RaiseEvent statusResume(taskID)
    End Sub

    Public Sub cancel(ByVal taskID As Integer)
        Dim taskIndex As Integer = Me.taskList(taskID).id
        Dim filePath As String = Me.taskList(taskID).filePath
        taskList.RemoveAt(taskID)
        DownloadEngine.stopTask(taskIndex)
        File.Delete(filePath & ".td")
        File.Delete(filePath & ".td.cfg")
        RaiseEvent statusCancel(taskID)
    End Sub
#End Region

End Class
