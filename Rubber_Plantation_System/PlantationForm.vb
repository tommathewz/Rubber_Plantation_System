Imports MySql.Data.MySqlClient

Public Class PlantationForm
    ' MySQL Connection String
    Dim connectionString As String = "server=localhost; user=root; password=admin; database=rubber_plantation;"

    ' Save Plantation Data
    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        ' Get values from input fields
        Dim plantationName As String = Guna2TextBox1.Text
        Dim acre As String = Guna2TextBox2.Text
        Dim plantType As String = Guna2TextBox3.Text
        Dim status As String = Guna2ComboBox1.Text
        Dim plantYear As String = Guna2TextBox4.Text

        ' Validation: Check if fields are empty
        If plantationName = "" Or acre = "" Or plantType = "" Or status = "" Or plantYear = "" Then
            MessageBox.Show("Please fill all fields before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Validate PlantYear (must be a number and within a valid range)
        If Not IsNumeric(plantYear) OrElse Convert.ToInt32(plantYear) < 1900 OrElse Convert.ToInt32(plantYear) > Year(Now) Then
            MessageBox.Show("Enter a valid Plantation Year (between 1900 and current year).", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Database Connection & Insert Query
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()
                Dim query As String = "INSERT INTO Plantation (PlantationName, Acre, PlantType, Status, PlantYear) VALUES (@plantationName, @acre, @plantType, @status, @plantYear)"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@plantationName", plantationName)
                    cmd.Parameters.AddWithValue("@acre", acre)
                    cmd.Parameters.AddWithValue("@plantType", plantType)
                    cmd.Parameters.AddWithValue("@status", status)
                    cmd.Parameters.AddWithValue("@plantYear", Convert.ToInt32(plantYear))
                    cmd.ExecuteNonQuery()
                End Using
                MessageBox.Show("Plantation record saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ClearFields() ' Clear input fields after saving
            Catch ex As Exception
                MessageBox.Show("Database Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    ' Show Existing Plantation Data in MessageBox
    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()
                Dim query As String = "SELECT * FROM Plantation"
                Using cmd As New MySqlCommand(query, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        Dim plantationData As String = "Existing Plantation Records:" & vbCrLf & "---------------------------------" & vbCrLf

                        While reader.Read()
                            plantationData &= "ID: " & reader("PlantationID").ToString() & vbCrLf
                            plantationData &= "Name: " & reader("PlantationName").ToString() & vbCrLf
                            plantationData &= "Acre: " & reader("Acre").ToString() & vbCrLf
                            plantationData &= "Plant Type: " & reader("PlantType").ToString() & vbCrLf
                            plantationData &= "Status: " & reader("Status").ToString() & vbCrLf
                            plantationData &= "Plant Year: " & reader("PlantYear").ToString() & vbCrLf
                            plantationData &= "---------------------------------" & vbCrLf
                        End While

                        MessageBox.Show(plantationData, "Plantation Records", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error fetching plantation data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    ' Function to Clear Input Fields
    Private Sub ClearFields()
        Guna2TextBox1.Clear()
        Guna2TextBox2.Clear()
        Guna2TextBox3.Clear()
        Guna2ComboBox1.SelectedIndex = -1 ' Reset ComboBox selection
        Guna2TextBox4.Clear()
    End Sub

    Private Sub Guna2GradientButton3_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton3.Click
        Dim workerform As New Worker_mgnt()
        workerform.Show()
        Me.Hide()
    End Sub
End Class
