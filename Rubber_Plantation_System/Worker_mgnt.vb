Imports MySql.Data.MySqlClient

Public Class Worker_mgnt
    ' MySQL Connection String
    Dim connectionString As String = "server=localhost; user=root; password=admin; database=rubber_plantation;"

    ' Save Worker Data
    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        ' Get values from input fields
        Dim workerName As String = Guna2TextBox1.Text
        Dim workerContactNo As String = Guna2TextBox2.Text
        Dim workerSalary As String = Guna2TextBox3.Text
        Dim workerRole As String = Guna2ComboBox1.Text
        Dim workerJoiningDate As String = Guna2TextBox4.Text

        ' Validation: Check if fields are empty
        If workerName = "" Or workerContactNo = "" Or workerSalary = "" Or workerRole = "" Or workerJoiningDate = "" Then
            MessageBox.Show("Please fill all fields before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Database Connection & Insert Query
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()
                Dim query As String = "INSERT INTO Worker (WorkerName, WorkerContactNo, WorkerSalary, WorkerRole, WorkerJoiningDate) VALUES (@workerName, @workerContactNo, @workerSalary, @workerRole, @workerJoiningDate)"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@workerName", workerName)
                    cmd.Parameters.AddWithValue("@workerContactNo", workerContactNo)
                    cmd.Parameters.AddWithValue("@workerSalary", workerSalary)
                    cmd.Parameters.AddWithValue("@workerRole", workerRole)
                    cmd.Parameters.AddWithValue("@workerJoiningDate", workerJoiningDate)
                    cmd.ExecuteNonQuery()
                End Using
                MessageBox.Show("Worker record saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ClearFields() ' Clear input fields after saving
            Catch ex As Exception
                MessageBox.Show("Database Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    ' Show Existing Worker Data in MessageBox
    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()
                Dim query As String = "SELECT * FROM Worker"
                Using cmd As New MySqlCommand(query, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        Dim workerData As String = "Existing Worker Records:" & vbCrLf & "---------------------------------" & vbCrLf

                        While reader.Read()
                            workerData &= "ID: " & reader("WorkerID").ToString() & vbCrLf
                            workerData &= "Name: " & reader("WorkerName").ToString() & vbCrLf
                            workerData &= "Contact: " & reader("WorkerContactNo").ToString() & vbCrLf
                            workerData &= "Salary: " & reader("WorkerSalary").ToString() & vbCrLf
                            workerData &= "Role: " & reader("WorkerRole").ToString() & vbCrLf
                            workerData &= "Joining Date: " & reader("WorkerJoiningDate").ToString() & vbCrLf
                            workerData &= "---------------------------------" & vbCrLf
                        End While

                        MessageBox.Show(workerData, "Worker Records", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error fetching worker data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
        Dim plantationForm As New PlantationForm()
        plantationForm.Show()
        Me.Hide()
    End Sub

    Private Sub Guna2GradientButton4_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton4.Click
        Dim tapp As New tapsche()
        tapp.Show()
        Me.Hide()
    End Sub
End Class
