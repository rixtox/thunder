<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.t_url = New System.Windows.Forms.TextBox()
        Me.t_save_directory = New System.Windows.Forms.TextBox()
        Me.btn_folder = New System.Windows.Forms.Button()
        Me.btn_download = New System.Windows.Forms.Button()
        Me.dialog_folder = New System.Windows.Forms.FolderBrowserDialog()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.l_status = New System.Windows.Forms.ToolStripStatusLabel()
        Me.l_percentage = New System.Windows.Forms.ToolStripStatusLabel()
        Me.l_size = New System.Windows.Forms.ToolStripStatusLabel()
        Me.btn_pause = New System.Windows.Forms.Button()
        Me.btn_cancel = New System.Windows.Forms.Button()
        Me.btn_exit = New System.Windows.Forms.Button()
        Me.dialog_file = New System.Windows.Forms.SaveFileDialog()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        't_url
        '
        Me.t_url.Location = New System.Drawing.Point(12, 12)
        Me.t_url.Name = "t_url"
        Me.t_url.Size = New System.Drawing.Size(594, 21)
        Me.t_url.TabIndex = 0
        Me.t_url.Text = "http://farm9.staticflickr.com/8091/8434293249_69236a67a6_k.jpg"
        '
        't_save_directory
        '
        Me.t_save_directory.Location = New System.Drawing.Point(12, 39)
        Me.t_save_directory.Name = "t_save_directory"
        Me.t_save_directory.Size = New System.Drawing.Size(513, 21)
        Me.t_save_directory.TabIndex = 1
        '
        'btn_folder
        '
        Me.btn_folder.Location = New System.Drawing.Point(531, 38)
        Me.btn_folder.Name = "btn_folder"
        Me.btn_folder.Size = New System.Drawing.Size(75, 23)
        Me.btn_folder.TabIndex = 3
        Me.btn_folder.Text = "Folder"
        Me.btn_folder.UseVisualStyleBackColor = True
        '
        'btn_download
        '
        Me.btn_download.Location = New System.Drawing.Point(12, 96)
        Me.btn_download.Name = "btn_download"
        Me.btn_download.Size = New System.Drawing.Size(75, 23)
        Me.btn_download.TabIndex = 4
        Me.btn_download.Text = "Download"
        Me.btn_download.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 67)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(594, 23)
        Me.ProgressBar1.TabIndex = 5
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.l_status, Me.l_percentage, Me.l_size})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 126)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(620, 22)
        Me.StatusStrip1.SizingGrip = False
        Me.StatusStrip1.TabIndex = 8
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'l_status
        '
        Me.l_status.Name = "l_status"
        Me.l_status.Size = New System.Drawing.Size(49, 17)
        Me.l_status.Text = "l_status"
        '
        'l_percentage
        '
        Me.l_percentage.Name = "l_percentage"
        Me.l_percentage.Size = New System.Drawing.Size(81, 17)
        Me.l_percentage.Text = "l_percentage"
        '
        'l_size
        '
        Me.l_size.Name = "l_size"
        Me.l_size.Size = New System.Drawing.Size(37, 17)
        Me.l_size.Text = "l_size"
        '
        'btn_pause
        '
        Me.btn_pause.Enabled = False
        Me.btn_pause.Location = New System.Drawing.Point(93, 96)
        Me.btn_pause.Name = "btn_pause"
        Me.btn_pause.Size = New System.Drawing.Size(75, 23)
        Me.btn_pause.TabIndex = 9
        Me.btn_pause.Text = "Pause"
        Me.btn_pause.UseVisualStyleBackColor = True
        '
        'btn_cancel
        '
        Me.btn_cancel.Enabled = False
        Me.btn_cancel.Location = New System.Drawing.Point(174, 96)
        Me.btn_cancel.Name = "btn_cancel"
        Me.btn_cancel.Size = New System.Drawing.Size(75, 23)
        Me.btn_cancel.TabIndex = 10
        Me.btn_cancel.Text = "Cancel"
        Me.btn_cancel.UseVisualStyleBackColor = True
        '
        'btn_exit
        '
        Me.btn_exit.Location = New System.Drawing.Point(255, 96)
        Me.btn_exit.Name = "btn_exit"
        Me.btn_exit.Size = New System.Drawing.Size(75, 23)
        Me.btn_exit.TabIndex = 11
        Me.btn_exit.Text = "Exit"
        Me.btn_exit.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(620, 148)
        Me.Controls.Add(Me.btn_exit)
        Me.Controls.Add(Me.btn_cancel)
        Me.Controls.Add(Me.btn_pause)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.btn_download)
        Me.Controls.Add(Me.btn_folder)
        Me.Controls.Add(Me.t_save_directory)
        Me.Controls.Add(Me.t_url)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "Thunder Download"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents t_url As System.Windows.Forms.TextBox
    Friend WithEvents t_save_directory As System.Windows.Forms.TextBox
    Friend WithEvents btn_folder As System.Windows.Forms.Button
    Friend WithEvents btn_download As System.Windows.Forms.Button
    Friend WithEvents dialog_folder As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents l_status As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents l_percentage As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents l_size As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents btn_pause As System.Windows.Forms.Button
    Friend WithEvents btn_cancel As System.Windows.Forms.Button
    Friend WithEvents btn_exit As System.Windows.Forms.Button
    Friend WithEvents dialog_file As System.Windows.Forms.SaveFileDialog

End Class
