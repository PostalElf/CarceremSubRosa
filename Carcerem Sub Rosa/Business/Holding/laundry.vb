﻿Public Class laundry
    Inherits holding
    Public Sub New()
    End Sub
    Public Sub New(aName As String, aShellcompany As shellcompany)
        If aName = "" Then name = holding.getRandomCodename Else name = aName
        shellcompany = aShellcompany
        shellcompany.addHolding(Me)
        _laundryRate = laundryRate.Low
    End Sub
    Friend Overrides Sub consoleReport(indent As Integer, Optional prefix As String = "")
        Dim ind As String = vbSpace(indent) & prefix
        Dim indd As String = vbSpace(indent + 1) & prefix
        Console.WriteLine(ind & """" & name & """ Laundry")
        Console.WriteLine(indd & fakeTab("Shell Company: ", 16) & shellcompany.name)
        Console.WriteLine(indd & fakeTab("City: ", 16) & city.name & ", " & city.parseContinent(continent))
        Console.WriteLine(indd & fakeTab("Visibility: ", 16) & withSign(visibility))
        Console.WriteLine(indd & fakeTab("Upkeep: ", 16) & withReverseSign(upkeep, "$"))
        Console.WriteLine(indd & fakeTab("Laundry Rate: ", 16) & laundryRate.ToString & " ($" & laundryRate & "/week)")
    End Sub
    Public Overrides Function ToString() As String
        Return name & " Laundry"
    End Function

    Friend Overrides ReadOnly Property visibility As Integer
        Get
            Select Case laundryRate
                Case laundryRate.Low : Return 1
                Case laundryRate.Mid : Return 2
                Case laundryRate.High : Return 4
                Case Else : Return -1
            End Select
        End Get
    End Property
    Friend Overrides ReadOnly Property upkeep As Integer
        Get
            Return 100
        End Get
    End Property
    Private Property _laundryRate As laundryRate
    Friend ReadOnly Property laundryRate As laundryRate
        Get
            Return _laundryRate
        End Get
    End Property
End Class

Public Enum laundryRate
    Low = 100
    Mid = 300
    High = 500
End Enum