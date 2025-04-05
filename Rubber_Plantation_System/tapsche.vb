Imports MySql.Data.MySqlClient

Public Class tapsche
    Dim connectionString As String = "server=localhost; user=root; password=admin; database=rubber_plantation;"

    Private Sub tapsche_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadPlantationCombo()
        LoadTapperCombo()
    End Sub

    ' Load PlantationID & PlantationName
    Private Sub LoadPlantationCombo()
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()
                Dim query As String = "SELECT PlantationID, PlantationName FROM Plantation"
                Using cmd As New MySqlCommand(query, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim item As String = reader("PlantationID").ToString() & " - " & reader("PlantationName").ToString()
                            Guna2ComboBox2.Items.Add(item)
                        End While
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error loading plantation data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    ' Load WorkerID & WorkerName where role is 'Tapper'
    Private Sub LoadTapperCombo()
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()
                Dim query As String = "SELECT WorkerID, WorkerName FROM Worker WHERE WorkerRole = 'Tapper'"
                Using cmd As New MySqlCommand(query, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim item As String = reader("WorkerID").ToString() & " - " & reader("WorkerName").ToString()
                            Guna2ComboBox1.Items.Add(item)
                        End While
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error loading tapper data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    ' 🔘 SAVE Button Click
    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        If Guna2ComboBox1.SelectedIndex = -1 OrElse Guna2ComboBox2.SelectedIndex = -1 OrElse Guna2TextBox2.Text = "" Then
            MessageBox.Show("Please select both Tapper and Plantation, and enter Tapping Date.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Extract ID from ComboBoxes
        Dim plantationID As Integer = CInt(Guna2ComboBox2.Text.Split("-"c)(0).Trim())
        Dim tapperID As Integer = CInt(Guna2ComboBox1.Text.Split("-"c)(0).Trim())
        Dim tappingDate As String = Guna2TextBox2.Text.Trim()

        ' Validate Date Format & Month
        Dim parts() As String = tappingDate.Split("-"c)
        If parts.Length <> 3 OrElse Not IsNumeric(parts(1)) Then
            MessageBox.Show("Date must be in DD-MM-YYYY format.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim monthNum As Integer = CInt(parts(1))
        If monthNum >= 1 AndAlso monthNum <= 5 Then
            MessageBox.Show("Tapping is not possible this month.", "Invalid Month", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Insert into Tapping Table
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()
                Dim query As String = "INSERT INTO Tapping (PlantationID, TapperID, TappingDate) VALUES (@pid, @tid, @date)"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@pid", plantationID)
                    cmd.Parameters.AddWithValue("@tid", tapperID)
                    cmd.Parameters.AddWithValue("@date", tappingDate)
                    cmd.ExecuteNonQuery()
                End Using
                MessageBox.Show("Tapping data saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Guna2TextBox2.Clear()
            Catch ex As Exception
                MessageBox.Show("Error saving tapping data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    ' 📋 SHOW Button Click
    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        Dim output As String = "Tapping Records:" & vbCrLf & "----------------------------" & vbCrLf

        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()
                Dim query As String = "SELECT T.TappingID, T.TappingDate, P.PlantationName, W.WorkerName 
                                       FROM Tapping T 
                                       JOIN Plantation P ON T.PlantationID = P.PlantationID 
                                       JOIN Worker W ON T.TapperID = W.WorkerID"
                Using cmd As New MySqlCommand(query, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            output &= "Tapping ID: " & reader("TappingID").ToString() & vbCrLf
                            output &= "Date: " & reader("TappingDate").ToString() & vbCrLf
                            output &= "Plantation: " & reader("PlantationName").ToString() & vbCrLf
                            output &= "Tapper: " & reader("WorkerName").ToString() & vbCrLf
                            output &= "----------------------------" & vbCrLf
                        End While
                    End Using
                End Using
                Dim scrollForm As New Form
                Dim txt As New TextBox With {
                    .Multiline = True,
                    .ScrollBars = ScrollBars.Vertical,
                    .Dock = DockStyle.Fill,
                    .ReadOnly = True,
                    .Text = output
                }
                scrollForm.Text = "Tapping Records"
                scrollForm.Size = New Size(500, 400)
                scrollForm.Controls.Add(txt)
                scrollForm.ShowDialog()
            Catch ex As Exception
                MessageBox.Show("Error loading tapping data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub Guna2GradientButton4_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton4.Click
        Dim pro As New product_mgnt()
        pro.Show()
        Me.Hide()
    End Sub

    Private Sub Guna2GradientButton3_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton3.Click
        Dim wrkr As New Worker_mgnt()
        wrkr.Show()
        Me.Hide()
    End Sub
End Class
