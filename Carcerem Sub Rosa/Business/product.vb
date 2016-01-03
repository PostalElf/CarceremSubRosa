﻿Public Class product
    Public Sub New()
    End Sub
    Public Sub New(aName As String, aIndustry As industry, Optional aCityModifiers As List(Of modifier) = Nothing)
        name = aName
        _industry = aIndustry
        If aCityModifiers Is Nothing = False Then cityModifiers.AddRange(aCityModifiers)
    End Sub
    Public Sub New(blueprint As product)
        name = blueprint.name
        _industry = blueprint.industry
        If blueprint.cityModifiers.Count > 0 Then cityModifiers.AddRange(blueprint.cityModifiers)
    End Sub
    Friend Shared Function fileget(targetName As String) As product
        For Each line In csvFileget("data/products.csv")
            If line(0) = targetName Then
                Dim product As New product
                With product
                    Dim n As New rollingCounter(0)
                    .name = line(n.Tick)
                    ._industry = enumArrays.getEnumFromString(line(n.Tick), enumArrays.industryArray)

                    While n.Tick < line.Length AndAlso n.Last <> ""
                        Dim modifier As New modifier(.name, ._cityModifiers, line(n.Last))
                        ._cityModifiers.Add(modifier)
                    End While
                End With
                Return product
            End If
        Next
        Return Nothing
    End Function
    Friend Sub consoleReport(indent As Integer, Optional prefix As String = "")
        Dim ind As String = vbSpace(indent) & prefix
        Console.Write(ind & name & " (" & industry.ToString & ")")
        If manufacturer Is Nothing = False AndAlso importer Is Nothing = False Then
            Console.Write(": " & manufacturer.city.name & " -> " & importer.name)
            Console.Write(" for $" & income)
        End If
        Console.WriteLine()
    End Sub
    Public Overrides Function ToString() As String
        Return name & " (" & industry.ToString & ")"
    End Function

    Friend Property name As String
    Private Property _industry As industry
    Friend ReadOnly Property industry As industry
        Get
            Return _industry
        End Get
    End Property
    Private Property _cityModifiers As New List(Of modifier)
    Friend ReadOnly Property cityModifiers As List(Of modifier)
        Get
            Return _cityModifiers
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
