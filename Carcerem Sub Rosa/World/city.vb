Public Class city
    Implements problemReporter
    Public Sub New()
        For Each industry In enumArrays.industryArray
            _importProducts.Add(industry, New List(Of product))
            _demand.Add(industry, 0)
        Next
    End Sub
    Friend Shared Function buildCity(line As String()) As city
        Dim city As New city
        With city
            .name = line(0)
            .country = line(1)
            .continent = enumArrays.getEnumFromString(line(2), enumArrays.continentArray)
            ._coords = getCoordsFromString(line(3))
            ._standardOfLiving = CInt(line(4))
        End With
        Return city
    End Function
    Friend Sub consoleReport(indent As Integer)
        Dim ind As String = vbSpace(indent)
        Dim indd As String = vbSpace(indent + 1)

        Console.WriteLine(ind & name & ", " & country)
        Console.WriteLine(indd & "Continent: " & parseContinent(continent))
        Console.WriteLine(indd & "Coordinates: " & _coords.ToString)
        Console.WriteLine(indd & "Wealth: " & standardOfLiving.ToString)
    End Sub
    Public Overrides Function ToString() As String
        Return name & ", " & parseContinent(continent)
    End Function

    Friend Property world As world
    Friend Property name As String Implements problemReporter.name
    Friend Property country As String
    Friend Shared Function parseContinent(continent As continent) As String
        Select Case continent
            Case Carcerem_Sub_Rosa.continent.AmericaN : Return "North America"
            Case Carcerem_Sub_Rosa.continent.AmericaS : Return "South America"
            Case Carcerem_Sub_Rosa.continent.AsiaSE : Return "South East Asia"
            Case Carcerem_Sub_Rosa.continent.MiddleEast : Return "Middle East"
            Case Else : Return continent.ToString
        End Select
    End Function
    Friend Property continent As continent
    Private Property _coords As New xy
    Private Shared Function getCoordsFromString(str As String) As xy
        Dim xy As New xy
        With xy
            Dim raw As String() = str.Split(",")
            For n = 0 To 1
                raw(n) = raw(n).Replace("°", "")
                raw(n) = raw(n).Trim()

                Dim rawsub As String() = raw(n).Split(" ")
                Dim value As Integer = CInt(CDec(rawsub(0)) * 100)
                Dim direction As String = rawsub(1)

                Select Case direction
                    Case "N" : .y = value
                    Case "S" : .y = value * -1
                    Case "E" : .x = value
                    Case "W" : .x = value * -1
                End Select
            Next
        End With
        Return xy
    End Function

    Private Property _standardOfLiving As standardOfLiving
    Friend ReadOnly Property standardOfLiving As standardOfLiving
        Get
            Return _standardOfLiving
        End Get
    End Property
    Private Property _citysites As New List(Of citysite)
    Friend Function addCitysite(citysite As citysite) As problem
        If _citysites.Contains(citysite) Then Return New problem(Me, problemType.Duplicate)

        citysite.city = Me
        _citysites.Add(citysite)
        Return Nothing
    End Function
    Friend Function addHolding(holding As holding, citysite As citysite) As problem
        If _citysites.Contains(citysite) = False Then Return New problem(Me, problemType.CitysiteNotFound)
        If citysite.holding Is Nothing = False Then Return New problem(Me, problemType.CitysiteOccupied)
        If holding.citysite Is Nothing = False Then Return New problem(Me, problemType.CitysiteOccupied)
        If TypeOf holding Is laundry Then Return New problem(Me, problemType.CitysiteNotSuitable)
        If TypeOf holding Is safehouse AndAlso getHoldings("safehouse").Count > 0 Then Return New problem(Me, problemType.ExceedCapacity)
        If TypeOf holding Is tacsupport AndAlso getHoldings("tacsupport").Count >= 3 Then Return New problem(Me, problemType.ExceedCapacity)

        citysite.holding = holding
        holding.citysite = citysite
        Return Nothing
    End Function
    Private Function getHoldings(type As String) As List(Of holding)
        Dim total As New List(Of holding)
        For Each citysite In _citysites
            If citysite.holding Is Nothing = False Then
                Dim holding As holding = citysite.holding
                Select Case type
                    Case "factory" : If TypeOf holding Is factory Then total.Add(holding)
                    Case "laundry" : If TypeOf holding Is laundry Then total.Add(holding)
                    Case "researchlab" : If TypeOf holding Is researchlab Then total.Add(holding)
                    Case "safehouse" : If TypeOf holding Is safehouse Then total.Add(holding)
                    Case "tacsupport" : If TypeOf holding Is tacsupport Then total.Add(holding)
                End Select
            End If
        Next
        Return total
    End Function

    Private Property _shellcompanies As New List(Of shellcompany)
    Friend ReadOnly Property shellcompanies As List(Of shellcompany)
        Get
            Return _shellcompanies
        End Get
    End Property
    Friend Function addShellcompany(shellcompany As shellcompany) As problem
        If _shellcompanies.Contains(shellcompany) Then Return New problem(Me, problemType.Duplicate)

        shellcompany.city = Me
        _shellcompanies.Add(shellcompany)
        Return Nothing
    End Function
    Private Property _importProducts As New Dictionary(Of industry, List(Of product))
    Friend ReadOnly Property importProducts As Dictionary(Of industry, List(Of product))
        Get
            Return _importProducts
        End Get
    End Property
    Friend Function addImportProduct(product As product) As problem
        Dim industry As industry = product.industry
        If _importProducts(industry).Contains(product) Then Return New problem(Me, problemType.Duplicate)
        If _demand(industry) - _importProducts(industry).Count < 1 Then Return New problem(Me, problemType.ExceedCapacity)

        product.importer = Me
        addModifiers(product.cityModifiers)
        _importProducts(industry).Add(product)
        Return Nothing
    End Function
    Private Property _demand As New Dictionary(Of industry, Integer)
    Friend ReadOnly Property demand As Dictionary(Of industry, Integer)
        Get
            Return _demand
        End Get
    End Property
    Friend Sub addDemand(industry As industry, value As Integer)
        _demand(industry) += value
    End Sub
    Friend ReadOnly Property income As Integer
        Get
            Dim total As Integer = 0
            For Each industry In enumArrays.industryArray
                For Each product In _importProducts(industry)
                    total += product.income
                Next
            Next
            Return total
        End Get
    End Property

    Private Property _modifiers As New List(Of modifier)
    Friend ReadOnly Property modifiers As List(Of modifier)
        Get
            Return _modifiers
        End Get
    End Property
    Friend Function addModifier(modifier As modifier) As problem
        If _modifiers.Contains(modifier) Then Return New problem(Me, problemType.Duplicate)

        modifier.parent = _modifiers
        Select Case modifier.typeName

        End Select
        _modifiers.Add(modifier)
        Return Nothing
    End Function
    Friend Function addModifiers(modlist As List(Of modifier)) As problem
        Dim total As problem = Nothing
        For Each modifier In modlist
            total = addModifier(modifier)
            If total Is Nothing = False Then Return total
        Next
        Return Nothing
    End Function
End Class