Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports MySql.Data.MySqlClient

Public Class sales_mgnt
    Dim connectionString As String = "server=localhost; user=root; password=admin; database=rubber_plantation;"

    Private Sub Guna2GradientButton5_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton5.Click
        ' Get values
        Dim saleDate As String = Guna2TextBox1.Text
        Dim qtyLatex As Integer
        Dim qtyPrs As Integer
        Dim priceLatex As Decimal
        Dim pricePrs As Decimal

        ' Validation
        If Not Integer.TryParse(Guna2TextBox3.Text, qtyLatex) OrElse
           Not Integer.TryParse(Guna2TextBox5.Text, qtyPrs) OrElse
           Not Decimal.TryParse(Guna2TextBox2.Text, priceLatex) OrElse
           Not Decimal.TryParse(Guna2TextBox4.Text, pricePrs) OrElse
           String.IsNullOrWhiteSpace(saleDate) Then
            MessageBox.Show("Please enter valid values for all fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Save to DB
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()
                Dim query As String = "INSERT INTO sales (qty_of_latex, qty_prs, price_latex, price_prs, sale_date) " &
                                      "VALUES (@qty_of_latex, @qty_prs, @price_latex, @price_prs, @sale_date)"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@qty_of_latex", qtyLatex)
                    cmd.Parameters.AddWithValue("@qty_prs", qtyPrs)
                    cmd.Parameters.AddWithValue("@price_latex", priceLatex)
                    cmd.Parameters.AddWithValue("@price_prs", pricePrs)
                    cmd.Parameters.AddWithValue("@sale_date", saleDate)
                    cmd.ExecuteNonQuery()
                End Using
                MessageBox.Show("Sales record saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ClearFields()
            Catch ex As Exception
                MessageBox.Show("Error saving data: " & ex.Message, "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub Guna2GradientButton6_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton6.Click
        ' Calculate total
        Dim qtyLatex As Integer
        Dim qtyPrs As Integer
        Dim priceLatex As Decimal
        Dim pricePrs As Decimal

        If Not Integer.TryParse(Guna2TextBox3.Text, qtyLatex) OrElse
           Not Integer.TryParse(Guna2TextBox5.Text, qtyPrs) OrElse
           Not Decimal.TryParse(Guna2TextBox2.Text, priceLatex) OrElse
           Not Decimal.TryParse(Guna2TextBox4.Text, pricePrs) Then
            MessageBox.Show("Please enter valid numbers to calculate total.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim totalAmt As Decimal = (qtyLatex * priceLatex) + (qtyPrs * pricePrs)
        MessageBox.Show("Total Amount: ₹" & totalAmt.ToString("F2"), "Total Amount", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        ' Show data
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()
                Dim query As String = "SELECT * FROM sales"
                Using cmd As New MySqlCommand(query, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        Dim allData As String = "Sales Records:" & vbCrLf & "--------------------------" & vbCrLf
                        While reader.Read()
                            allData &= "Sale ID: " & reader("sale_id") & vbCrLf
                            allData &= "Date: " & reader("sale_date") & vbCrLf
                            allData &= "Qty Latex: " & reader("qty_of_latex") & vbCrLf
                            allData &= "Qty PRS: " & reader("qty_prs") & vbCrLf
                            allData &= "Price Latex: ₹" & reader("price_latex") & vbCrLf
                            allData &= "Price PRS: ₹" & reader("price_prs") & vbCrLf
                            allData &= "Total: ₹" & reader("totalamt") & vbCrLf
                            allData &= "--------------------------" & vbCrLf
                        End While
                        MessageBox.Show(allData, "All Sales", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error fetching data: " & ex.Message, "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub ClearFields()
        Guna2TextBox1.Clear()
        Guna2TextBox2.Clear()
        Guna2TextBox3.Clear()
        Guna2TextBox4.Clear()
        Guna2TextBox5.Clear()
    End Sub

    Private Sub Guna2GradientButton4_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton4.Click
        Application.Exit()
    End Sub

    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        Dim connectionString As String = "server=localhost; user=root; password=admin; database=rubber_plantation;"
        Dim doc As New Document(PageSize.A4, 10, 10, 20, 20)
        Dim filePath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RubberDataDump.pdf")

        Try
            PdfWriter.GetInstance(doc, New FileStream(filePath, FileMode.Create))
            doc.Open()

            Dim tables As String() = {"plantation", "production", "sales", "tapping", "worker"}

            ' Set title font properly
            Dim titleFont As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14)
            Dim headerFont As Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)
            Dim dataFont As Font = FontFactory.GetFont(FontFactory.HELVETICA, 10)

            Using conn As New MySqlConnection(connectionString)
                conn.Open()
                For Each tableName As String In tables
                    ' Add table title
                    doc.Add(New Paragraph($"{tableName.ToUpper()} TABLE", titleFont))
                    doc.Add(New Paragraph(" "))

                    Dim query As String = $"SELECT * FROM {tableName}"
                    Dim cmd As New MySqlCommand(query, conn)
                    Dim reader As MySqlDataReader = cmd.ExecuteReader()

                    Dim pdfTable As New PdfPTable(reader.FieldCount)
                    pdfTable.WidthPercentage = 100

                    ' Add headers
                    For i As Integer = 0 To reader.FieldCount - 1
                        Dim cell As New PdfPCell(New Phrase(reader.GetName(i), headerFont))
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY
                        pdfTable.AddCell(cell)
                    Next

                    ' Add rows
                    While reader.Read()
                        For i As Integer = 0 To reader.FieldCount - 1
                            pdfTable.AddCell(New Phrase(reader(i).ToString(), dataFont))
                        Next
                    End While
                    reader.Close()

                    doc.Add(pdfTable)
                    doc.Add(New Paragraph(Environment.NewLine))
                Next
                conn.Close()
            End Using

            doc.Close()
            MessageBox.Show("PDF generated successfully at Desktop!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Error generating PDF: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Guna2GradientButton3_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton3.Click
        Dim pro As New product_mgnt()
        pro.Show()
        Me.Hide()
    End Sub
End Class
