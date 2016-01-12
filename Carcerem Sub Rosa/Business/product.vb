Public Class product
    Public Sub New()
    End Sub
    Public Sub New(aName As String, Optional aCityConsequences As List(Of String) = Nothing)
        name = aName
        If aCityConsequences Is Nothing = False Then cityConsequences.AddRange(aCityConsequences)
    End Sub
    Public Sub New(blueprint As product)
        name = blueprint.name
        If blueprint.cityConsequences.Count > 0 Then _cityConsequences.AddRange(blueprint.cityConsequences)
    End Sub
    Friend Shared Function fileget(targetName As String) As product
        For Each line In csvFileget("data/products.csv")
            If line(0) = targetName Then
                Dim product As New product
                With product
                    Dim n As New rollingCounter(0)
                    .name = line(n.Tick)

                    While n.Tick < line.Length AndAlso n.Last <> ""
                        ._cityConsequences.Add(line(n.Last))
                    End While
                End With
                Return product
            End If
        Next
        Return Nothing
    End Function
    Friend Sub consoleReport(indent As Integer, Optional prefix As String = "")
        Dim ind As String = vbSpace(indent) & prefix
        Console.Write(ind & name)
        If manufacturer Is Nothing = False AndAlso importer Is Nothing = False Then
            Console.Write(": " & manufacturer.city.name & " -> " & importer.name)
            Console.Write(" for $" & income)
        End If
        Console.WriteLine()
    End Sub
    Public Overrides Function ToString() As String
        Return name
    End Function

    Friend Property name As String
    Private Property _cityConsequences As New List(Of String)
    Friend ReadOnly Property cityConsequences As List(Of String)
        Get
            Return _cityConsequences
        End Get
    End Property

    Friend Property manufacturer As factory
    Friend Property importer As city
    Friend ReadOnly Property income As Integer
        Get
            If importer Is Nothing OrElse manufacturer Is Nothing Then Return 0
            Dim solDifference As Integer = importer.standardOfLiving - manufacturer.city.standardOfLiving
            Dim base As Integer = 0
            Select Case solDifference
                Case 0 : base = 100
                Case 1 : base = 300
                Case 2 : base = 500
                Case 3 : base = 700
                Case Else : base = 100
            End Select
            If importer.continent <> manufacturer.continent Then base -= 100
            Return base
        End Get
    End Property
End Class
