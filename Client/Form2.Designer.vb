<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
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
        Me.components = New System.ComponentModel.Container()
        Me.minBtn = New System.Windows.Forms.Button()
        Me.CheackBlacklistBtn = New System.Windows.Forms.Button()
        Me.Message = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Sender = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.closeBtn = New System.Windows.Forms.Button()
        Me.sendMsgBtn = New System.Windows.Forms.Button()
        Me.chatMsgField = New System.Windows.Forms.TextBox()
        Me.chatroomGrid = New System.Windows.Forms.DataGridView()
        Me.Time = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.banBtn = New System.Windows.Forms.Button()
        Me.label10 = New System.Windows.Forms.Label()
        Me.fileExtensionField = New System.Windows.Forms.TextBox()
        Me.label9 = New System.Windows.Forms.Label()
        Me.filePathField = New System.Windows.Forms.TextBox()
        Me.downloadFileBtn = New System.Windows.Forms.Button()
        Me.label6 = New System.Windows.Forms.Label()
        Me.label5 = New System.Windows.Forms.Label()
        Me.label3 = New System.Windows.Forms.Label()
        Me.label4 = New System.Windows.Forms.Label()
        Me.varDataField = New System.Windows.Forms.TextBox()
        Me.varField = New System.Windows.Forms.TextBox()
        Me.fetchUserVarBtn = New System.Windows.Forms.Button()
        Me.timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.setUserVarBtn = New System.Windows.Forms.Button()
        Me.globalVariableField = New System.Windows.Forms.TextBox()
        Me.fetchGlobalVariableBtn = New System.Windows.Forms.Button()
        Me.checkSessionBtn = New System.Windows.Forms.Button()
        Me.sendLogDataBtn = New System.Windows.Forms.Button()
        Me.logDataField = New System.Windows.Forms.TextBox()
        Me.onlineUsersField = New System.Windows.Forms.ListBox()
        Me.userDataField = New System.Windows.Forms.ListBox()
        CType(Me.chatroomGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'minBtn
        '
        Me.minBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.minBtn.ForeColor = System.Drawing.Color.White
        Me.minBtn.Location = New System.Drawing.Point(696, 7)
        Me.minBtn.Name = "minBtn"
        Me.minBtn.Size = New System.Drawing.Size(43, 23)
        Me.minBtn.TabIndex = 165
        Me.minBtn.Text = "-"
        Me.minBtn.UseVisualStyleBackColor = True
        '
        'CheackBlacklistBtn
        '
        Me.CheackBlacklistBtn.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(242, Byte), Integer))
        Me.CheackBlacklistBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(22, Byte), Integer), CType(CType(29, Byte), Integer), CType(CType(39, Byte), Integer))
        Me.CheackBlacklistBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheackBlacklistBtn.ForeColor = System.Drawing.Color.White
        Me.CheackBlacklistBtn.Location = New System.Drawing.Point(8, 498)
        Me.CheackBlacklistBtn.Name = "CheackBlacklistBtn"
        Me.CheackBlacklistBtn.Size = New System.Drawing.Size(323, 30)
        Me.CheackBlacklistBtn.TabIndex = 163
        Me.CheackBlacklistBtn.Text = "Cheack Blacklist ok"
        Me.CheackBlacklistBtn.UseVisualStyleBackColor = False
        '
        'Message
        '
        Me.Message.HeaderText = "Message"
        Me.Message.Name = "Message"
        Me.Message.ReadOnly = True
        Me.Message.Width = 200
        '
        'Sender
        '
        Me.Sender.HeaderText = "Sender"
        Me.Sender.MinimumWidth = 6
        Me.Sender.Name = "Sender"
        Me.Sender.ReadOnly = True
        '
        'closeBtn
        '
        Me.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.closeBtn.ForeColor = System.Drawing.Color.White
        Me.closeBtn.Location = New System.Drawing.Point(745, 7)
        Me.closeBtn.Name = "closeBtn"
        Me.closeBtn.Size = New System.Drawing.Size(43, 23)
        Me.closeBtn.TabIndex = 164
        Me.closeBtn.Text = "X"
        Me.closeBtn.UseVisualStyleBackColor = True
        '
        'sendMsgBtn
        '
        Me.sendMsgBtn.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(242, Byte), Integer))
        Me.sendMsgBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(22, Byte), Integer), CType(CType(29, Byte), Integer), CType(CType(39, Byte), Integer))
        Me.sendMsgBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.sendMsgBtn.ForeColor = System.Drawing.Color.White
        Me.sendMsgBtn.Location = New System.Drawing.Point(692, 497)
        Me.sendMsgBtn.Name = "sendMsgBtn"
        Me.sendMsgBtn.Size = New System.Drawing.Size(94, 36)
        Me.sendMsgBtn.TabIndex = 161
        Me.sendMsgBtn.Text = "Send"
        Me.sendMsgBtn.UseVisualStyleBackColor = False
        '
        'chatMsgField
        '
        Me.chatMsgField.BackColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.chatMsgField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.chatMsgField.Location = New System.Drawing.Point(334, 507)
        Me.chatMsgField.Name = "chatMsgField"
        Me.chatMsgField.Size = New System.Drawing.Size(352, 20)
        Me.chatMsgField.TabIndex = 160
        '
        'chatroomGrid
        '
        Me.chatroomGrid.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.chatroomGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.chatroomGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Sender, Me.Message, Me.Time})
        Me.chatroomGrid.GridColor = System.Drawing.Color.DodgerBlue
        Me.chatroomGrid.Location = New System.Drawing.Point(334, 39)
        Me.chatroomGrid.Name = "chatroomGrid"
        Me.chatroomGrid.ReadOnly = True
        Me.chatroomGrid.Size = New System.Drawing.Size(452, 454)
        Me.chatroomGrid.TabIndex = 162
        '
        'Time
        '
        Me.Time.HeaderText = "Time"
        Me.Time.Name = "Time"
        Me.Time.ReadOnly = True
        '
        'banBtn
        '
        Me.banBtn.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(242, Byte), Integer))
        Me.banBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(22, Byte), Integer), CType(CType(29, Byte), Integer), CType(CType(39, Byte), Integer))
        Me.banBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.banBtn.ForeColor = System.Drawing.Color.White
        Me.banBtn.Location = New System.Drawing.Point(8, 426)
        Me.banBtn.Name = "banBtn"
        Me.banBtn.Size = New System.Drawing.Size(323, 30)
        Me.banBtn.TabIndex = 159
        Me.banBtn.Text = "Ban Account ok"
        Me.banBtn.UseVisualStyleBackColor = False
        '
        'label10
        '
        Me.label10.AutoSize = True
        Me.label10.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label10.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.label10.Location = New System.Drawing.Point(3, 189)
        Me.label10.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.label10.Name = "label10"
        Me.label10.Size = New System.Drawing.Size(117, 15)
        Me.label10.TabIndex = 158
        Me.label10.Text = "File Name/Extension"
        '
        'fileExtensionField
        '
        Me.fileExtensionField.BackColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.fileExtensionField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.fileExtensionField.ForeColor = System.Drawing.Color.White
        Me.fileExtensionField.Location = New System.Drawing.Point(6, 207)
        Me.fileExtensionField.Name = "fileExtensionField"
        Me.fileExtensionField.Size = New System.Drawing.Size(323, 20)
        Me.fileExtensionField.TabIndex = 157
        '
        'label9
        '
        Me.label9.AutoSize = True
        Me.label9.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label9.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.label9.Location = New System.Drawing.Point(3, 145)
        Me.label9.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.label9.Name = "label9"
        Me.label9.Size = New System.Drawing.Size(53, 15)
        Me.label9.TabIndex = 156
        Me.label9.Text = "File Path"
        '
        'filePathField
        '
        Me.filePathField.BackColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.filePathField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.filePathField.ForeColor = System.Drawing.Color.White
        Me.filePathField.Location = New System.Drawing.Point(6, 163)
        Me.filePathField.Name = "filePathField"
        Me.filePathField.Size = New System.Drawing.Size(323, 20)
        Me.filePathField.TabIndex = 155
        '
        'downloadFileBtn
        '
        Me.downloadFileBtn.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(242, Byte), Integer))
        Me.downloadFileBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(22, Byte), Integer), CType(CType(29, Byte), Integer), CType(CType(39, Byte), Integer))
        Me.downloadFileBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.downloadFileBtn.ForeColor = System.Drawing.Color.White
        Me.downloadFileBtn.Location = New System.Drawing.Point(6, 233)
        Me.downloadFileBtn.Name = "downloadFileBtn"
        Me.downloadFileBtn.Size = New System.Drawing.Size(323, 30)
        Me.downloadFileBtn.TabIndex = 154
        Me.downloadFileBtn.Text = "Download File ok"
        Me.downloadFileBtn.UseVisualStyleBackColor = False
        '
        'label6
        '
        Me.label6.AutoSize = True
        Me.label6.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label6.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.label6.Location = New System.Drawing.Point(3, 266)
        Me.label6.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(130, 15)
        Me.label6.TabIndex = 153
        Me.label6.Text = "Global Variable Name:"
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label5.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.label5.Location = New System.Drawing.Point(4, 346)
        Me.label5.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(119, 15)
        Me.label5.TabIndex = 152
        Me.label5.Text = "Data To Send In Log:"
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label3.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.label3.Location = New System.Drawing.Point(3, 63)
        Me.label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(210, 15)
        Me.label3.TabIndex = 151
        Me.label3.Text = "User Variable Data: (For Setting Only)"
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.label4.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.label4.Location = New System.Drawing.Point(3, 19)
        Me.label4.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(119, 15)
        Me.label4.TabIndex = 150
        Me.label4.Text = "User Variable Name:"
        '
        'varDataField
        '
        Me.varDataField.BackColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.varDataField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.varDataField.ForeColor = System.Drawing.Color.White
        Me.varDataField.Location = New System.Drawing.Point(6, 81)
        Me.varDataField.Name = "varDataField"
        Me.varDataField.Size = New System.Drawing.Size(323, 20)
        Me.varDataField.TabIndex = 149
        '
        'varField
        '
        Me.varField.BackColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.varField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.varField.ForeColor = System.Drawing.Color.White
        Me.varField.Location = New System.Drawing.Point(6, 37)
        Me.varField.Name = "varField"
        Me.varField.Size = New System.Drawing.Size(323, 20)
        Me.varField.TabIndex = 148
        '
        'fetchUserVarBtn
        '
        Me.fetchUserVarBtn.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(242, Byte), Integer))
        Me.fetchUserVarBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(22, Byte), Integer), CType(CType(29, Byte), Integer), CType(CType(39, Byte), Integer))
        Me.fetchUserVarBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.fetchUserVarBtn.ForeColor = System.Drawing.Color.White
        Me.fetchUserVarBtn.Location = New System.Drawing.Point(176, 107)
        Me.fetchUserVarBtn.Name = "fetchUserVarBtn"
        Me.fetchUserVarBtn.Size = New System.Drawing.Size(155, 30)
        Me.fetchUserVarBtn.TabIndex = 147
        Me.fetchUserVarBtn.Text = "Fetch User Variable ok"
        Me.fetchUserVarBtn.UseVisualStyleBackColor = False
        '
        'timer1
        '
        Me.timer1.Interval = 1
        '
        'setUserVarBtn
        '
        Me.setUserVarBtn.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(242, Byte), Integer))
        Me.setUserVarBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(22, Byte), Integer), CType(CType(29, Byte), Integer), CType(CType(39, Byte), Integer))
        Me.setUserVarBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.setUserVarBtn.ForeColor = System.Drawing.Color.White
        Me.setUserVarBtn.Location = New System.Drawing.Point(6, 107)
        Me.setUserVarBtn.Name = "setUserVarBtn"
        Me.setUserVarBtn.Size = New System.Drawing.Size(155, 30)
        Me.setUserVarBtn.TabIndex = 146
        Me.setUserVarBtn.Text = "Set User Variable ok"
        Me.setUserVarBtn.UseVisualStyleBackColor = False
        '
        'globalVariableField
        '
        Me.globalVariableField.BackColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.globalVariableField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.globalVariableField.ForeColor = System.Drawing.Color.White
        Me.globalVariableField.Location = New System.Drawing.Point(6, 284)
        Me.globalVariableField.Name = "globalVariableField"
        Me.globalVariableField.Size = New System.Drawing.Size(323, 20)
        Me.globalVariableField.TabIndex = 145
        '
        'fetchGlobalVariableBtn
        '
        Me.fetchGlobalVariableBtn.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(242, Byte), Integer))
        Me.fetchGlobalVariableBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(22, Byte), Integer), CType(CType(29, Byte), Integer), CType(CType(39, Byte), Integer))
        Me.fetchGlobalVariableBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.fetchGlobalVariableBtn.ForeColor = System.Drawing.Color.White
        Me.fetchGlobalVariableBtn.Location = New System.Drawing.Point(6, 310)
        Me.fetchGlobalVariableBtn.Name = "fetchGlobalVariableBtn"
        Me.fetchGlobalVariableBtn.Size = New System.Drawing.Size(323, 30)
        Me.fetchGlobalVariableBtn.TabIndex = 144
        Me.fetchGlobalVariableBtn.Text = "Fetch Global Variable ok"
        Me.fetchGlobalVariableBtn.UseVisualStyleBackColor = False
        '
        'checkSessionBtn
        '
        Me.checkSessionBtn.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(242, Byte), Integer))
        Me.checkSessionBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(22, Byte), Integer), CType(CType(29, Byte), Integer), CType(CType(39, Byte), Integer))
        Me.checkSessionBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.checkSessionBtn.ForeColor = System.Drawing.Color.White
        Me.checkSessionBtn.Location = New System.Drawing.Point(8, 462)
        Me.checkSessionBtn.Name = "checkSessionBtn"
        Me.checkSessionBtn.Size = New System.Drawing.Size(323, 30)
        Me.checkSessionBtn.TabIndex = 143
        Me.checkSessionBtn.Text = "Check Session  ok"
        Me.checkSessionBtn.UseVisualStyleBackColor = False
        '
        'sendLogDataBtn
        '
        Me.sendLogDataBtn.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(242, Byte), Integer))
        Me.sendLogDataBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(22, Byte), Integer), CType(CType(29, Byte), Integer), CType(CType(39, Byte), Integer))
        Me.sendLogDataBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.sendLogDataBtn.ForeColor = System.Drawing.Color.White
        Me.sendLogDataBtn.Location = New System.Drawing.Point(7, 390)
        Me.sendLogDataBtn.Name = "sendLogDataBtn"
        Me.sendLogDataBtn.Size = New System.Drawing.Size(323, 30)
        Me.sendLogDataBtn.TabIndex = 142
        Me.sendLogDataBtn.Text = "Send Log ok"
        Me.sendLogDataBtn.UseVisualStyleBackColor = False
        '
        'logDataField
        '
        Me.logDataField.BackColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.logDataField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.logDataField.ForeColor = System.Drawing.Color.White
        Me.logDataField.Location = New System.Drawing.Point(7, 364)
        Me.logDataField.Name = "logDataField"
        Me.logDataField.Size = New System.Drawing.Size(323, 20)
        Me.logDataField.TabIndex = 141
        '
        'onlineUsersField
        '
        Me.onlineUsersField.BackColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.onlineUsersField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.onlineUsersField.ForeColor = System.Drawing.Color.White
        Me.onlineUsersField.FormattingEnabled = True
        Me.onlineUsersField.Items.AddRange(New Object() {"Online Users:", ""})
        Me.onlineUsersField.Location = New System.Drawing.Point(446, 536)
        Me.onlineUsersField.Name = "onlineUsersField"
        Me.onlineUsersField.Size = New System.Drawing.Size(336, 106)
        Me.onlineUsersField.TabIndex = 140
        '
        'userDataField
        '
        Me.userDataField.BackColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.userDataField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.userDataField.ForeColor = System.Drawing.Color.White
        Me.userDataField.FormattingEnabled = True
        Me.userDataField.Location = New System.Drawing.Point(7, 536)
        Me.userDataField.Name = "userDataField"
        Me.userDataField.Size = New System.Drawing.Size(433, 106)
        Me.userDataField.TabIndex = 139
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(61, Byte), Integer), CType(CType(79, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(790, 648)
        Me.Controls.Add(Me.minBtn)
        Me.Controls.Add(Me.CheackBlacklistBtn)
        Me.Controls.Add(Me.closeBtn)
        Me.Controls.Add(Me.sendMsgBtn)
        Me.Controls.Add(Me.chatMsgField)
        Me.Controls.Add(Me.chatroomGrid)
        Me.Controls.Add(Me.banBtn)
        Me.Controls.Add(Me.label10)
        Me.Controls.Add(Me.fileExtensionField)
        Me.Controls.Add(Me.label9)
        Me.Controls.Add(Me.filePathField)
        Me.Controls.Add(Me.downloadFileBtn)
        Me.Controls.Add(Me.label6)
        Me.Controls.Add(Me.label5)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.varDataField)
        Me.Controls.Add(Me.varField)
        Me.Controls.Add(Me.fetchUserVarBtn)
        Me.Controls.Add(Me.setUserVarBtn)
        Me.Controls.Add(Me.globalVariableField)
        Me.Controls.Add(Me.fetchGlobalVariableBtn)
        Me.Controls.Add(Me.checkSessionBtn)
        Me.Controls.Add(Me.sendLogDataBtn)
        Me.Controls.Add(Me.logDataField)
        Me.Controls.Add(Me.onlineUsersField)
        Me.Controls.Add(Me.userDataField)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Form2"
        Me.Text = "Form2"
        CType(Me.chatroomGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents minBtn As Button
    Private WithEvents CheackBlacklistBtn As Button
    Private WithEvents Message As DataGridViewTextBoxColumn
    Private WithEvents Sender As DataGridViewTextBoxColumn
    Private WithEvents closeBtn As Button
    Private WithEvents sendMsgBtn As Button
    Private WithEvents chatMsgField As TextBox
    Private WithEvents chatroomGrid As DataGridView
    Private WithEvents Time As DataGridViewTextBoxColumn
    Private WithEvents banBtn As Button
    Private WithEvents label10 As Label
    Private WithEvents fileExtensionField As TextBox
    Private WithEvents label9 As Label
    Private WithEvents filePathField As TextBox
    Private WithEvents downloadFileBtn As Button
    Private WithEvents label6 As Label
    Private WithEvents label5 As Label
    Private WithEvents label3 As Label
    Private WithEvents label4 As Label
    Private WithEvents varDataField As TextBox
    Private WithEvents varField As TextBox
    Private WithEvents fetchUserVarBtn As Button
    Private WithEvents timer1 As Timer
    Private WithEvents setUserVarBtn As Button
    Private WithEvents globalVariableField As TextBox
    Private WithEvents fetchGlobalVariableBtn As Button
    Private WithEvents checkSessionBtn As Button
    Private WithEvents sendLogDataBtn As Button
    Private WithEvents logDataField As TextBox
    Private WithEvents onlineUsersField As ListBox
    Private WithEvents userDataField As ListBox
End Class
