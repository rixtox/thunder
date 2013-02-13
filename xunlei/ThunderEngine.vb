Imports System.Text
Imports System.Runtime.InteropServices

Public Class ThunderEngine
#Region " Import DLL Functions "
    <DllImport(XL_DLL, CharSet:=CharSet.Auto, SetLastError:=True, BestFitMapping:=False, CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Function XLInitDownloadEngine() As Boolean
    End Function
    <DllImport(XL_DLL, CharSet:=CharSet.Auto, SetLastError:=True, BestFitMapping:=False, CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Function XLURLDownloadToFile(ByVal pszFileName As String, ByVal pszUrl As String, ByVal pszRefUrl As String, ByRef lTaskId As UInt32) As UInt32
    End Function
    <DllImport(XL_DLL, CharSet:=CharSet.Auto, SetLastError:=True, BestFitMapping:=False, CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Function XLQueryTaskInfo(lTaskId As UInt32, ByRef plStatus As UInt32, ByRef pullFileSize As UInt64, ByRef pullRecvSize As UInt64) As UInt32
    End Function
    <DllImport(XL_DLL, CharSet:=CharSet.Auto, SetLastError:=True, BestFitMapping:=False, CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Function XLPauseTask(ByVal lTaskId As UInt32, ByRef lNewTaskId As Long) As UInt32
    End Function
    <DllImport(XL_DLL, CharSet:=CharSet.Auto, SetLastError:=True, BestFitMapping:=False, CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Function XLContinueTask(ByVal lTaskId As UInt32) As UInt32
    End Function
    <DllImport(XL_DLL, CharSet:=CharSet.Auto, SetLastError:=True, BestFitMapping:=False, CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Function XLContinueTaskFromTdFile(ByVal pszTdFileFullPath As String, ByRef lTaskId As Long) As UInt32
    End Function
    <DllImport(XL_DLL, CharSet:=CharSet.Auto, SetLastError:=True, BestFitMapping:=False, CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Sub XLStopTask(ByVal lTaskId As UInt32)
    End Sub
    <DllImport(XL_DLL, CharSet:=CharSet.Auto, SetLastError:=True, BestFitMapping:=False, CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Function XLUninitDownloadEngine() As Boolean
    End Function
    <DllImport(XL_DLL, CharSet:=CharSet.Auto, SetLastError:=True, BestFitMapping:=False, CallingConvention:=CallingConvention.StdCall)> _
    Private Shared Function XLGetErrorMsg(ByVal dwErrorId As UInteger, ByVal pszBuffer As String, ByRef dwSize As UInteger) As UInt32
    End Function
#End Region

#Region " Define Constants "
    Private Const XL_DLL = "lib\XLDownload.dll"
    Private Const XL_SUCCESS = &H0
    Private Const XL_ERROR_FAIL = &H10000000
    Private Const XL_ERROR_UNINITAILIZE = XL_ERROR_FAIL + 1
    Private Const XL_ERROR_UNSPORTED_PROTOCOL = XL_ERROR_FAIL + 2
    Private Const XL_ERROR_INIT_TASK_TRAY_ICON_FAIL = XL_ERROR_FAIL + 3
    Private Const XL_ERROR_ADD_TASK_TRAY_ICON_FAIL = XL_ERROR_FAIL + 4
    Private Const XL_ERROR_POINTER_IS_NULL = XL_ERROR_FAIL + 5
    Private Const XL_ERROR_STRING_IS_EMPTY = XL_ERROR_FAIL + 6
    Private Const XL_ERROR_PATH_DONT_INCLUDE_FILENAME = XL_ERROR_FAIL + 7
    Private Const XL_ERROR_CREATE_DIRECTORY_FAIL = XL_ERROR_FAIL + 8
    Private Const XL_ERROR_MEMORY_ISNT_ENOUGH = XL_ERROR_FAIL + 9
    Private Const XL_ERROR_INVALID_ARG = XL_ERROR_FAIL + 10
    Private Const XL_ERROR_TASK_DONT_EXIST = XL_ERROR_FAIL + 11
    Private Const XL_ERROR_FILE_NAME_INVALID = XL_ERROR_FAIL + 12
    Private Const XL_ERROR_NOTIMPL = XL_ERROR_FAIL + 13
    Private Const XL_ERROR_TASKNUM_EXCEED_MAXNUM = XL_ERROR_FAIL + 14
    Private Const XL_ERROR_INVALID_TASK_TYPE = XL_ERROR_FAIL + 15
    Private Const XL_ERROR_FILE_ALREADY_EXIST = XL_ERROR_FAIL + 16
    Private Const XL_ERROR_FILE_DONT_EXIST = XL_ERROR_FAIL + 17
    Private Const XL_ERROR_READ_CFG_FILE_FAIL = XL_ERROR_FAIL + 18
    Private Const XL_ERROR_WRITE_CFG_FILE_FAIL = XL_ERROR_FAIL + 19
    Private Const XL_ERROR_CANNOT_CONTINUE_TASK = XL_ERROR_FAIL + 20
    Private Const XL_ERROR_CANNOT_PAUSE_TASK = XL_ERROR_FAIL + 21
    Private Const XL_ERROR_BUFFER_TOO_SMALL = XL_ERROR_FAIL + 22
    Private Const XL_ERROR_INIT_THREAD_EXIT_TOO_EARLY = XL_ERROR_FAIL + 23
    Private Const XL_ERROR_TP_CRASH = XL_ERROR_FAIL + 24
    Private Const XL_ERROR_TASK_INVALID = XL_ERROR_FAIL + 25

    Private Const XL_STATUS_CONNECT = 0
    Private Const XL_STATUS_DOWNLOAD = 2
    Private Const XL_STATUS_PAUSE = 10
    Private Const XL_STATUS_SUCCESS = 11
    Private Const XL_STATUS_FAIL = 12
#End Region

#Region " Define Variables "
    Private lastError As Long = &H0
#End Region

#Region " Define Structures "
    Public Structure TaskInfo
        Dim id As UInt32
        Dim status As String
        Dim fileSize As UInt64
        Dim recievedSize As UInt64
    End Structure

    Public Structure ErrorInfo
        Dim id As Long
        Dim message As String
    End Structure
#End Region

#Region " Private Functions "
    Protected Overrides Sub Finalize()
        XLUninitDownloadEngine()
        MyBase.Finalize()
    End Sub

    Public Sub New()
        Dim initDL As Boolean = XLInitDownloadEngine()
        Try
        Catch ex As Exception When Not initDL
            Debug.Print("Failed to load Xunlei.")
            Exit Sub
        End Try
    End Sub
#End Region

#Region " Public Functions "
    Public Function getLastError() As ErrorInfo
        Dim retval As ErrorInfo
        retval.id = Me.lastError
        Dim errorMessage = Space(100)
        Dim dwSize = Len(errorMessage)
        Dim result = XLGetErrorMsg(Me.lastError, errorMessage, dwSize)
        If (result = XL_ERROR_INVALID_ARG) Then
            errorMessage = "Unknown error."
        ElseIf (result = XL_ERROR_BUFFER_TOO_SMALL) Then
            errorMessage = Space(dwSize)
            XLGetErrorMsg(Me.lastError, errorMessage, dwSize)
        End If
        retval.message = errorMessage
        Return retval
    End Function

    Public Function addTask(ByVal fileName As String, ByVal URL As String, ByVal refURL As String) As Integer
        Dim taskID As UInt32
        Dim result As Long = XLURLDownloadToFile(fileName, URL, refURL, taskID)
        If Not result = XL_SUCCESS Then
            Me.lastError = result
            Return -1
        End If
        Return taskID
    End Function

    Public Function pauseTask(ByVal taskID As UInt32, ByRef newID As UInt32) As Boolean
        Dim result As Long = XLPauseTask(taskID, newID)
        If Not result = XL_SUCCESS Then
            Me.lastError = result
            Return False
        End If
        Return True
    End Function

    Public Function continueTask(ByVal taskID As Int32) As Boolean
        Dim result As UInt32 = XLContinueTask(taskID)
        If Not result = XL_SUCCESS Then
            Me.lastError = result
            Return False
        End If
        Return True
    End Function

    Public Sub stopTask(ByVal taskID As UInt32)
        XLStopTask(taskID)
    End Sub

    Public Function queryTask(ByVal taskID As UInt32) As TaskInfo
        Dim retval As TaskInfo = New TaskInfo
        retval.id = taskID
        Dim status As UInteger
        Dim result As UInt32 = XLQueryTaskInfo(retval.id, status, retval.fileSize, retval.recievedSize)
        If Not result = XL_SUCCESS Then
            Me.lastError = result
            Throw New Exception(getLastError().message)
        End If
        Select Case status
            Case XL_STATUS_CONNECT
                retval.status = "connect"
            Case XL_STATUS_DOWNLOAD
                retval.status = "download"
            Case XL_STATUS_PAUSE
                retval.status = "pause"
            Case XL_STATUS_SUCCESS
                retval.status = "success"
            Case XL_STATUS_FAIL
                retval.status = "fail"
        End Select
        Return retval
    End Function
#End Region

End Class
