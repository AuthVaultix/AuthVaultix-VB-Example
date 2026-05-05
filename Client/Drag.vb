Imports System
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Public Class Drag

    Public Const WM_NCLBUTTONDOWN As Integer = &HA1
    Public Const HT_CAPTION As Integer = &H2

    <DllImport("user32.dll")>
    Public Shared Function ReleaseCapture() As Boolean
    End Function

    <DllImport("user32.dll")>
    Public Shared Function SendMessage(hWnd As IntPtr, Msg As Integer, wParam As Integer, lParam As Integer) As Integer
    End Function

    Public Shared Sub MakeDraggable(frm As Form)
        AddHandler frm.MouseDown,
            Sub(sender As Object, e As MouseEventArgs)
                If e.Button = MouseButtons.Left Then
                    ReleaseCapture()
                    SendMessage(frm.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0)
                End If
            End Sub
    End Sub

End Class