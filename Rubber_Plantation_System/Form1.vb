Imports MySql.Data.MySqlClient
Public Class Form1
    Dim connectionString As String = "server=localhost; user=root; password=admin; database=rubber_plantation                                               ;"

    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        ' Get values from textboxes
        Dim username As String = Guna2TextBox1.Text
        Dim password As String = Guna2TextBox2.Text

        ' Check if username and password are provided
        If username = "" Or password = "" Then
            MessageBox.Show("Please enter Username and Password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Create MySQL connection
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()
                Dim query As String = "SELECT * FROM Admin WHERE Username = @username AND Password = @password"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@username", username)
                    cmd.Parameters.AddWithValue("@password", password)

                    Dim reader As MySqlDataReader = cmd.ExecuteReader()

                    If reader.HasRows Then
                        MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        'Open the Next form (e.g., Plantation Management Form)
                        Dim plantationForm As New PlantationForm()
                        plantationForm.Show()
                        Me.Hide()
                    Else
                        MessageBox.Show("Invalid Username or Password!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If

                    reader.Close()
                End Using
            Catch ex As Exception
                MessageBox.Show("Database Connection Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub Guna2Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Guna2Panel1.Paint

    End Sub
End Class