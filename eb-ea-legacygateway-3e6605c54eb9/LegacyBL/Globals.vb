Public Class Globals

    Public Shared Function FromGMT(ByVal sTimeZoneName As String, ByVal GMTDate As Date, Optional ByVal winTimeZones As Win32.Win32TimeZone() = Nothing) As Date
        Return GetTimeZone(sTimeZoneName, winTimeZones).ToLocalTime(GMTDate)
    End Function

    Public Shared Function ToGMT(ByVal sTimeZoneName As String, ByVal sTimeZoneDate As Date, Optional ByVal winTimeZones As Win32.Win32TimeZone() = Nothing) As Date
        Return GetTimeZone(sTimeZoneName, winTimeZones).ToUniversalTime(sTimeZoneDate)
    End Function

    Public Shared Function GetTimeZone(ByVal sTimeZoneName As String, Optional ByRef winTimeZones As Win32.Win32TimeZone() = Nothing) As Win32.Win32TimeZone
        If IsNothing(winTimeZones) Then
            winTimeZones = Win32.TimeZones.GetTimeZones
        End If

        For Each winTimeZone As Win32.Win32TimeZone In winTimeZones
            If Trim(LCase(sTimeZoneName)) = Trim(LCase(winTimeZone.DisplayName)) Then
                Return winTimeZone
                Exit Function
            End If
        Next
        'We can only get here if we did not find the passed in timezone
        Throw New Exception("Invalid time zone specified: " & sTimeZoneName)
    End Function

    Public Shared Function GetTimeZoneName(ByVal nTimeZoneID As Integer) As String

        Dim timezones As String()() = GetTimeZoneSelectValues()

        Dim sTimezone As String = " " 'previous code returns a space if timezone id is not found (previously was in case else below)
        For Each tzi As String() In timezones
            If CInt(tzi(0)) = nTimeZoneID Then
                sTimezone = tzi(1)
                Exit For
            End If
        Next

        Return sTimezone
    End Function

    Public Shared Function GetTimeZoneSelectValues() As String()()
        Dim x As Integer = 0
        Dim values(94)() As String
        values(x) = New System.String() {"0", "(GMT-12:00) International Date Line West", "Dateline Time"} : x += 1
        values(x) = New System.String() {"1", "(GMT-11:00) Midway Island, Samoa", "Samoa Time"} : x += 1
        values(x) = New System.String() {"2", "(GMT-10:00) Hawaii", "Hawaiian Time"} : x += 1
        values(x) = New System.String() {"3", "(GMT-09:00) Alaska", "Alaskan Time"} : x += 1
        values(x) = New System.String() {"4", "(GMT-08:00) Pacific Time (US & Canada)", "Pacific Time"} : x += 1
        values(x) = New System.String() {"74", "(GMT-08:00) Tijuana, Baja California", "Pacific Time (Mexico)"} : x += 1
        values(x) = New System.String() {"6", "(GMT-07:00) Arizona", "Arizona Time"} : x += 1
        values(x) = New System.String() {"75", "(GMT-07:00) Chihuahua, La Paz, Mazatlan - New", "Mountain Time (Mexico)"} : x += 1
        values(x) = New System.String() {"76", "(GMT-07:00) Chihuahua, La Paz, Mazatlan - Old", "Mexico Time 2"} : x += 1
        values(x) = New System.String() {"5", "(GMT-07:00) Mountain Time (US & Canada)", "Mountain Time"} : x += 1
        values(x) = New System.String() {"10", "(GMT-06:00) Central America", "Central America Time"} : x += 1
        values(x) = New System.String() {"7", "(GMT-06:00) Central Time (US & Canada)", "Central Time"} : x += 1
        values(x) = New System.String() {"77", "(GMT-06:00) Guadalajara, Mexico City, Monterrey - New", "Central Time (Mexico)"} : x += 1
        values(x) = New System.String() {"9", "(GMT-06:00) Guadalajara, Mexico City, Monterrey - Old", "Mexico Time"} : x += 1
        values(x) = New System.String() {"8", "(GMT-06:00) Saskatchewan", "Canada Central Time"} : x += 1
        values(x) = New System.String() {"13", "(GMT-05:00) Bogota, Lima, Quito", "SA Pacific Time"} : x += 1
        values(x) = New System.String() {"11", "(GMT-05:00) Eastern Time (US & Canada)", "Eastern Time"} : x += 1
        values(x) = New System.String() {"12", "(GMT-05:00) Indiana (East)", "US Eastern Time"} : x += 1
        values(x) = New System.String() {"86", "(GMT-04:30) Caracas", "Venezuela Time"} : x += 1
        values(x) = New System.String() {"87", "(GMT-04:00) Georgetown, La Paz, San Juan", "SA Western Time"} : x += 1
        values(x) = New System.String() {"90", "(GMT-04:00) Asuncion", "Paraguay Time"} : x += 1
        values(x) = New System.String() {"14", "(GMT-04:00) Atlantic Time (Canada)", "Atlantic Time"} : x += 1
        values(x) = New System.String() {"78", "(GMT-04:00) Manaus", "Central Brazilian Time"} : x += 1
        values(x) = New System.String() {"16", "(GMT-04:00) Santiago", "Pacific SA Time"} : x += 1
        values(x) = New System.String() {"17", "(GMT-03:30) Newfoundland", "Newfoundland Time"} : x += 1
        values(x) = New System.String() {"18", "(GMT-03:00) Brasilia", "E. South America Time"} : x += 1
        values(x) = New System.String() {"19", "(GMT-03:00) Buenos Aires", "Argentina Time"} : x += 1
        values(x) = New System.String() {"20", "(GMT-03:00) Greenland", "Greenland Time"} : x += 1
        values(x) = New System.String() {"79", "(GMT-03:00) Montevideo", "Montevideo Time"} : x += 1
        values(x) = New System.String() {"91", "(GMT-03:00) Cayenne", "SA Eastern Time"} : x += 1
        values(x) = New System.String() {"21", "(GMT-02:00) Mid-Atlantic", "Mid-Atlantic Time"} : x += 1
        values(x) = New System.String() {"22", "(GMT-01:00) Azores", "Azores Time"} : x += 1
        values(x) = New System.String() {"23", "(GMT-01:00) Cape Verde Is.", "Cape Verde Time"} : x += 1
        values(x) = New System.String() {"25", "(GMT) Casablanca", "Morocco Time"} : x += 1
        values(x) = New System.String() {"92", "(GMT) Coordinated Universal Time", "Coordinated Universal Time"} : x += 1
        values(x) = New System.String() {"24", "(GMT) Greenwich Mean Time : Dublin, Edinburgh, Lisbon, London", "GMT Time"} : x += 1
        values(x) = New System.String() {"88", "(GMT) Monrovia, Reykjavik", "Greenwich Time"} : x += 1
        values(x) = New System.String() {"29", "(GMT+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna", "W. Europe Time"} : x += 1
        values(x) = New System.String() {"26", "(GMT+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague", "Central Europe Time"} : x += 1
        values(x) = New System.String() {"28", "(GMT+01:00) Brussels, Copenhagen, Madrid, Paris", "Romance Time"} : x += 1
        values(x) = New System.String() {"27", "(GMT+01:00) Sarajevo, Skopje, Warsaw, Zagreb", "Central European Time"} : x += 1
        values(x) = New System.String() {"30", "(GMT+01:00) West Central Africa", "W. Central Africa Time"} : x += 1
        values(x) = New System.String() {"80", "(GMT+02:00) Amman", "Jordan Time"} : x += 1
        values(x) = New System.String() {"31", "(GMT+02:00) Athens, Bucharest, Istanbul", "GTB Time"} : x += 1
        values(x) = New System.String() {"81", "(GMT+02:00) Beirut", "Middle East Time"} : x += 1
        values(x) = New System.String() {"32", "(GMT+02:00) Cairo", "Egypt Time"} : x += 1
        values(x) = New System.String() {"36", "(GMT+02:00) Harare, Pretoria", "South Africa Time"} : x += 1
        values(x) = New System.String() {"33", "(GMT+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius", "FLE Time"} : x += 1
        values(x) = New System.String() {"35", "(GMT+02:00) Jerusalem", "Jerusalem Time"} : x += 1
        values(x) = New System.String() {"34", "(GMT+02:00) Minsk", "E. Europe Time"} : x += 1
        values(x) = New System.String() {"82", "(GMT+02:00) Windhoek", "Namibia Time"} : x += 1
        values(x) = New System.String() {"40", "(GMT+03:00) Baghdad", "Arabic Time"} : x += 1
        values(x) = New System.String() {"38", "(GMT+03:00) Kuwait, Riyadh", "Arab Time"} : x += 1
        values(x) = New System.String() {"37", "(GMT+03:00) Moscow, St. Petersburg, Volgograd", "Russian Time"} : x += 1
        values(x) = New System.String() {"39", "(GMT+03:00) Nairobi", "E. Africa Time"} : x += 1
        values(x) = New System.String() {"41", "(GMT+03:30) Tehran", "Iran Time"} : x += 1
        values(x) = New System.String() {"83", "(GMT+04:00) Tbilisi", "Georgian Time"} : x += 1
        values(x) = New System.String() {"42", "(GMT+04:00) Abu Dhabi, Muscat", "Arabian Time"} : x += 1
        values(x) = New System.String() {"43", "(GMT+04:00) Baku", "Azerbaijan Time"} : x += 1
        values(x) = New System.String() {"85", "(GMT+04:00) Caucasus Standard Time", "Caucasus Time"} : x += 1
        values(x) = New System.String() {"95", "(GMT+04:00) Port Louis", "Mauritius Time"} : x += 1
        values(x) = New System.String() {"84", "(GMT+04:00) Yerevan", "Armenian Time"} : x += 1
        values(x) = New System.String() {"44", "(GMT+04:30) Kabul", "Afghanistan Time"} : x += 1
        values(x) = New System.String() {"45", "(GMT+05:00) Ekaterinburg", "Ekaterinburg Time"} : x += 1
        values(x) = New System.String() {"46", "(GMT+05:00) Islamabad, Karachi", "Pakistan Time"} : x += 1
        values(x) = New System.String() {"89", "(GMT+05:00) Tashkent", "West Asia Time"} : x += 1
        values(x) = New System.String() {"47", "(GMT+05:30) Chennai, Kolkata, Mumbai, New Delhi", "India Time"} : x += 1
        values(x) = New System.String() {"50", "(GMT+05:30) Sri Jayawardenepura", "Sri Lanka Time"} : x += 1
        values(x) = New System.String() {"48", "(GMT+05:45) Kathmandu", "Nepal Time"} : x += 1
        values(x) = New System.String() {"51", "(GMT+06:00) Novosibirsk", "N. Central Asia Time"} : x += 1
        values(x) = New System.String() {"49", "(GMT+06:00) Astana, Dhaka", "Central Asia Time"} : x += 1
        values(x) = New System.String() {"52", "(GMT+06:30) Yangon (Rangoon)", "Myanmar Time"} : x += 1
        values(x) = New System.String() {"53", "(GMT+07:00) Bangkok, Hanoi, Jakarta", "SE Asia Time"} : x += 1
        values(x) = New System.String() {"54", "(GMT+07:00) Krasnoyarsk", "North Asia Time"} : x += 1
        values(x) = New System.String() {"55", "(GMT+08:00) Beijing, Chongqing, Hong Kong, Urumqi", "China Time"} : x += 1
        values(x) = New System.String() {"59", "(GMT+08:00) Irkutsk", "North Asia East Time"} : x += 1
        values(x) = New System.String() {"93", "(GMT+08:00) Ulaanbaatar", "Ulaanbaatar Time"} : x += 1
        values(x) = New System.String() {"56", "(GMT+08:00) Kuala Lumpur, Singapore", "Malay Peninsula Time"} : x += 1
        values(x) = New System.String() {"58", "(GMT+08:00) Perth", "W. Australia Time"} : x += 1
        values(x) = New System.String() {"57", "(GMT+08:00) Taipei", "Taipei Time"} : x += 1
        values(x) = New System.String() {"61", "(GMT+09:00) Osaka, Sapporo, Tokyo", "Tokyo Time"} : x += 1
        values(x) = New System.String() {"60", "(GMT+09:00) Seoul", "Korea Time"} : x += 1
        values(x) = New System.String() {"62", "(GMT+09:00) Yakutsk", "Yakutsk Time"} : x += 1
        values(x) = New System.String() {"64", "(GMT+09:30) Adelaide", "Cen. Australia Time"} : x += 1
        values(x) = New System.String() {"63", "(GMT+09:30) Darwin", "AUS Central Time"} : x += 1
        values(x) = New System.String() {"66", "(GMT+10:00) Brisbane", "E. Australia Time"} : x += 1
        values(x) = New System.String() {"65", "(GMT+10:00) Canberra, Melbourne, Sydney", "AUS Eastern Time"} : x += 1
        values(x) = New System.String() {"69", "(GMT+10:00) Guam, Port Moresby", "West Pacific Time"} : x += 1
        values(x) = New System.String() {"67", "(GMT+10:00) Hobart", "Tasmania Time"} : x += 1
        values(x) = New System.String() {"68", "(GMT+10:00) Vladivostok", "Vladivostok Time"} : x += 1
        values(x) = New System.String() {"70", "(GMT+11:00) Magadan, Solomon Is., New Caledonia", "Central Pacific Time"} : x += 1
        values(x) = New System.String() {"72", "(GMT+12:00) Auckland, Wellington", "New Zealand Time"} : x += 1
        values(x) = New System.String() {"71", "(GMT+12:00) Fiji, Marshall Is.", "Fiji Time"} : x += 1
        values(x) = New System.String() {"94", "(GMT+12:00) Petropavlovsk-Kamchatsky", "Kamchatka Time"} : x += 1
        values(x) = New System.String() {"73", "(GMT+13:00) Nuku'alofa", "Tonga Time"} : x += 1

        Return values
    End Function

    Public Shared Function GetShortTimeZoneName(ByVal nTimeZoneID As Integer) As String

        Dim timezones As String()() = GetTimeZoneSelectValues()

        Dim sTimezone As String = " " 'previous code returns a space if timezone id is not found (previously was in case else below)
        For Each tzi As String() In timezones
            If CInt(tzi(0)) = nTimeZoneID Then
                sTimezone = tzi(2)
                Exit For
            End If
        Next

        Return sTimezone
    End Function
End Class
