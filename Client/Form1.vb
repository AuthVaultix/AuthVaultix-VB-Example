Imports System.Windows.Forms
Public Class Form1

    Public Shared Client As New AuthVaultix.AuthVaultixClient(
        "test_app",
        "5d36476ca4",
        "7b9729387300a04a9a128f2dbe8a9b24659047ab7933ab312dfdca3d5397fb59",
        "1.0"
    )
    ' ================= Form1_Load =================
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Drag.MakeDraggable(Me)
        If Not Client.Init() Then
            MessageBox.Show(Client.RisponceCollection)
            Environment.Exit(0)
        End If
    End Sub

    ' ================= LOGIN =================
    Private Sub LoginBtn_Click(sender As Object, e As EventArgs) Handles LoginBtn.Click
        If Client.Login(userFild.Text, pasFild.Text) Then
            Dim m As New Form2()
            m.Show()
            Me.Hide()
        Else
            MessageBox.Show(Client.RisponceCollection)
        End If
    End Sub
    ' ================= REGISTER =================
    Private Sub RegisterBtn_Click(sender As Object, e As EventArgs) Handles RegisterBtn.Click
        If Client.Register(userFild.Text, pasFild.Text, keyFild.Text, emailFild.Text) Then
            MessageBox.Show("Register successful ✅")
        Else
            MessageBox.Show(Client.RisponceCollection)
        End If
    End Sub
    ' ================= LICENSE LOGIN =================
    Private Sub LicenceBitn_Click(sender As Object, e As EventArgs) Handles LicenceBitn.Click
        If Client.LicenseLogin(keyFild.Text) Then
            MessageBox.Show("License login successful ✅")
        Else
            MessageBox.Show(Client.RisponceCollection)
        End If
    End Sub
    ' ================= UPGRADE =================
    Private Sub upgradeBtn_Click(sender As Object, e As EventArgs) Handles upgradeBtn.Click
        If Client.Upgrade(userFild.Text, keyFild.Text) Then
            MessageBox.Show("Upgrade successful ✅")
        Else
            MessageBox.Show(Client.RisponceCollection)
        End If
    End Sub
    ' ================= FORGOT PASSWORD =================
    Private Sub forgotBtn_Click(sender As Object, e As EventArgs) Handles forgotBtn.Click
        If Client.ForgotPassword(userFild.Text, keyFild.Text) Then
            MessageBox.Show("Reset email sent ✅")
        Else
            MessageBox.Show(Client.RisponceCollection)
        End If
    End Sub

    Private Sub closeBtn_Click(sender As Object, e As EventArgs) Handles closeBtn.Click
        Environment.Exit(0)
    End Sub

    Private Sub minBtn_Click(sender As Object, e As EventArgs) Handles minBtn.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub
End Class