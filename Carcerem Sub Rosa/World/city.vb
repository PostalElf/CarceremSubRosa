Public Class city
    Implements problemReporter
    Public Sub New()
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
    Friend Function briefReport() As String
        Return name & ", " & continent.ToString
    End Function
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
    Friend Function getDistanceTo(destination As city) As Integer
        Dim rawdata As List(Of String()) = csvFileget("data/distances.csv")
        For Each line In rawdata
            If line(0) = name AndAlso line(1) = destination.name Then Return CInt(line(2))
        Next
        Return -1
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
        If _citysites.Contains(citysite) = False Then Return New problem(Me, problemType.NotFound)
        If citysite.holding Is Nothing = False Then Return New problem(Me, problemType.Occupied)
        If holding.citysite Is Nothing = False Then Return New problem(Me, problemType.Occupied)
        If TypeOf holding Is laundry Then Return New problem(Me, problemType.NotSuitable)
        If TypeOf holding Is safehouse AndAlso getHoldings("safehouse").Count > 0 Then Return New problem(Me, problemType.ExceedCapacity)
        If TypeOf holding Is tacsupport AndAlso getHoldings("tacsupport").Count > 0 Then Return New problem(Me, problemType.ExceedCapacity)

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
    Private Property _importProducts As New List(Of product)
    Friend ReadOnly Property importProducts As List(Of product)
        Get
            Return _importProducts
        End Get
    End Property
    Friend Function addImportProduct(product As product) As problem
        If _importProducts.Contains(product) Then Return New problem(Me, problemType.Duplicate)
        If _demand - _importProducts.Count < 1 Then Return New problem(Me, problemType.ExceedCapacity)

        product.importer = Me
        addModifiers(product.cityModifiers)
        _importProducts.Add(product)
        Return Nothing
    End Function
    Private Property _demand As Integer
    Friend ReadOnly Property demand As Integer
        Get
            Return _demand
        End Get
    End Property
    Friend Sub addDemand(value As Integer)
        _demand += value
    End Sub
    Friend ReadOnly Property income As Integer
        Get
            Dim total As Integer = 0
            For Each product In _importProducts
                total += product.income
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

    Private Property _squads As New List(Of squad)
    Friend ReadOnly Property squads As List(Of squad)
        Get
            Return _squads
        End Get
    End Property
    Friend Function addSquad(squad As squad) As problem
        If _squads.Contains(squad) Then Return New problem(Me, problemType.Duplicate)

        _squads.Add(squad)
        Return Nothing
    End Function
    Friend Function removeSquad(squad As squad) As problem
        If _squads.Contains(squad) = False Then Return New problem(Me, problemType.NotFound)

        _squads.Remove(squad)
        Return Nothing
    End Function
End Class