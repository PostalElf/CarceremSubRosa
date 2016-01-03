Public Class factory
    Inherits holding
    Public Sub New()
    End Sub
    Public Sub New(aName As String, aCitysite As citysite, aShellcompany As shellcompany)
        If aName = "" Then name = holding.getRandomCodename Else name = aName
        citysite = aCitysite
        shellcompany = aShellcompany
        shellcompany.addHolding(Me)
        city.addHolding(Me, citysite)
    End Sub
    Friend Overrides Sub consoleReport(indent As Integer, Optional prefix As String = "")
        Dim ind As String = vbSpace(indent) & prefix
        Dim indd As String = vbSpace(indent + 1) & prefix
        Dim inddd As String = vbSpace(indent + 2) & prefix
        Console.WriteLine(ind & """" & name & """ Factory")
        Console.WriteLine(indd & fakeTab("Shell Company: ", 16) & shellcompany.name)
        Console.WriteLine(indd & fakeTab("City: ", 16) & city.name & ", " & city.parseContinent(continent))
        Console.WriteLine(indd & fakeTab("Visibility: ", 16) & withSign(visibility))
        Console.WriteLine(indd & fakeTab("Upkeep: ", 16) & withReverseSign(upkeep, "$"))
        Console.WriteLine(indd & fakeTab("Income: ", 16) & withSign(income, "$"))
        If exportProducts.Count > 0 Then
            Console.WriteLine(indd & "Products:")
            For Each product In exportProducts
                product.consoleReport(indent + 2, "└ ")
            Next
        End If
    End Sub
    Public Overrides Function ToString() As String
        Return name & " Factory"
    End Function

    Friend ReadOnly Property player As player
        Get
            Return shellcompany.player
        End Get
    End Property

    Friend ReadOnly Property productCapacity As Integer
        Get
            Select Case city.standardOfLiving
                Case standardOfLiving.Low : Return 1
                Case standardOfLiving.Mid : Return 2
                Case standardOfLiving.High : Return 3
                Case Else : Return -1
            End Select
        End Get
    End Property
    Private Property _exportProducts As New List(Of product)
    Friend ReadOnly Property exportProducts As List(Of product)
        Get
            Return _exportProducts
        End Get
    End Property
    Friend Function addExportProduct(product As product) As problem
        If player.checkBlueprint(product.name) = False Then Return New problem(Me, problemType.NotFound)
        If productCapacity - _exportProducts.Count < 1 Then Return New problem(Me, problemType.ExceedCapacity)

        product.manufacturer = Me
        _exportProducts.Add(product)
        Return Nothing
    End Function
    Friend ReadOnly Property income As Integer
        Get
            Dim total As Integer = 0
            For Each product In exportProducts
                total += product.income
            Next
            Return total
        End Get
    End Property

    Friend Overrides ReadOnly Property visibility As Integer
        Get
            Return 1 + productVisibility
        End Get
    End Property
    Private ReadOnly Property productVisibility As Integer
        Get
            Dim total As Integer = 0
            Dim countedProducts As New List(Of String)
            For Each product In _exportProducts
                If countedProducts.Contains(product.name) = False Then
                    countedProducts.Add(product.name)
                    total += 1
                End If
            Next
            Return total
        End Get
    End Property
    Friend Overrides ReadOnly Property upkeep As Integer
        Get
            Return 100
        End Get
    End Property
End Class
