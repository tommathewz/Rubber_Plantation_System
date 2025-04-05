Imports MySql.Data.MySqlClient

Public Class product_mgnt

    Dim connectionString As String = "server=localhost; user=root; password=admin; database=rubber_plantation;"

    Private Sub product_mgnt_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadPlantationNames()
    End Sub

    ' Load Plantation Names
    Private Sub LoadPlantationNames()
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()
                Dim query As String = "SELECT PlantationName FROM Plantation"
                Using cmd As New MySqlCommand(query, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Guna2ComboBox1.Items.Add(reader("PlantationName").ToString())
                        End While
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error loading plantation names: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    ' Save to Production Table
    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        Dim plantationName As String = Guna2ComboBox1.Text
        Dim noOfCont As String = Guna2TextBox2.Text
        Dim amtLatex As String = Guna2TextBox3.Text
        Dim noProcessedRs As String = Guna2TextBox4.Text

        ' Validation
        If plantationName = "" Or noOfCont = "" Or amtLatex = "" Or noProcessedRs = "" Then
            MessageBox.Show("Please fill all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not IsNumeric(noOfCont) Or Not IsNumeric(amtLatex) Or Not IsNumeric(noProcessedRs) Then
            MessageBox.Show("Please enter valid numeric values.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Insert Data
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()
                Dim query As String = "INSERT INTO production (Plantation_name, no_of_cont, amt_latex, no_processed_rs) VALUES (@pname, @cont, @latex, @processed)"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@pname", plantationName)
                    cmd.Parameters.AddWithValue("@cont", Convert.ToInt32(noOfCont))
                    cmd.Parameters.AddWithValue("@latex", Convert.ToInt32(amtLatex))
                    cmd.Parameters.AddWithValue("@processed", Convert.ToInt32(noProcessedRs))
                    cmd.ExecuteNonQuery()
                End Using
                MessageBox.Show("Production data saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ClearFields()
            Catch ex As Exception
                MessageBox.Show("Database Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    ' Show Production Data
    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()
                Dim query As String = "SELECT * FROM production"
                Using cmd As New MySqlCommand(query, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        Dim data As String = "Production Records:" & vbCrLf & "----------------------------" & vbCrLf
                        While reader.Read()
                            data &= "ID: " & reader("production_id").ToString() & vbCrLf
                            data &= "Plantation: " & reader("Plantation_name").ToString() & vbCrLf
                            data &= "No. of Containers: " & reader("no_of_cont").ToString() & vbCrLf
                            data &= "Amount of Latex: " & reader("amt_latex").ToString() & vbCrLf
                            data &= "Processed Amount (Rs): " & reader("no_processed_rs").ToString() & vbCrLf
                            data &= "----------------------------" & vbCrLf
                        End While

                        ' Use scrollable MessageBox via new form
                        Dim scrollForm As New Form
                        scrollForm.Text = "Production Data"
                        scrollForm.Size = New Size(500, 400)

                        Dim txtBox As New TextBox
                        txtBox.Multiline = True
                        txtBox.ScrollBars = ScrollBars.Vertical
                        txtBox.Dock = DockStyle.Fill
                        txtBox.ReadOnly = True
                        txtBox.Text = data

                        scrollForm.Controls.Add(txtBox)
                        scrollForm.ShowDialog()
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error fetching data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    ' Clear Input Fields
    Private Sub ClearFields()
        Guna2ComboBox1.SelectedIndex = -1
        Guna2TextBox2.Clear()
        Guna2TextBox3.Clear()
        Guna2TextBox4.Clear()
    End Sub

    Private Sub Guna2GradientButton4_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton4.Click
        Dim sales As New sales_mgnt()
        sales.Show()
        Me.Hide()
    End Sub

    Private Sub Guna2GradientButton3_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton3.Click
        Dim tapp As New tapsche()
        tapp.Show()
        Me.Hide()
    End Sub
End Class
