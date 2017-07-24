Option Explicit On
Option Strict On
Option Compare Binary

Imports System
Imports System.Collections
Imports System.Globalization

Imports Microsoft.Win32

'Imports MichaelBrumm.Globalization
Imports LegacyBL.Globalization

' Win32 TimeZones
' by Michael R. Brumm
'
' For updates and more information, visit:
' http://www.michaelbrumm.com/simpletimezone.html
'
' or contact me@michaelbrumm.com
'
' Please do not modify this code and re-release it. If you
' require changes to this class, please derive your own class
' from SimpleTimeZone, and add (or override) the methods and
' properties on your own derived class. You never know when 
' your code might need to be version compatible with another
' class that uses Win32 TimeZones.

' This should have been part of Microsoft.Win32, so that is
' where I located it.
Namespace Win32


    Public NotInheritable Class TimeZones


        Private Const VALUE_INDEX As String = "Index"
        Private Const VALUE_DISPLAY_NAME As String = "Display"
        Private Const VALUE_STANDARD_NAME As String = "Std"
        Private Const VALUE_DAYLIGHT_NAME As String = "Dlt"
        Private Const VALUE_ZONE_INFO As String = "TZI"

        Private Const LENGTH_ZONE_INFO As Int32 = 44
        Private Const LENGTH_DWORD As Int32 = 4
        Private Const LENGTH_WORD As Int32 = 2
        Private Const LENGTH_SYSTEMTIME As Int32 = 16



        Private Shared REG_KEYS_TIME_ZONES As String() = {
          "SOFTWARE\Microsoft\Windows\CurrentVersion\Time Zones",
          "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones"
        }


        Private Shared nameRegKeyTimeZones As String


        Private Class TZREGReader


            Public Bias As Int32
            Public StandardBias As Int32
            Public DaylightBias As Int32
            Public StandardDate As SYSTEMTIMEReader
            Public DaylightDate As SYSTEMTIMEReader


            Public Sub New(ByVal bytes As Byte())

                Dim index As Int32
                index = 0

                Bias = BitConverter.ToInt32(bytes, index)
                index = index + LENGTH_DWORD

                StandardBias = BitConverter.ToInt32(bytes, index)
                index = index + LENGTH_DWORD

                DaylightBias = BitConverter.ToInt32(bytes, index)
                index = index + LENGTH_DWORD

                StandardDate = New SYSTEMTIMEReader(bytes, index)
                index = index + LENGTH_SYSTEMTIME

                DaylightDate = New SYSTEMTIMEReader(bytes, index)

            End Sub


        End Class


        Private Class SYSTEMTIMEReader


            Public Year As Int16
            Public Month As Int16
            Public DayOfWeek As Int16
            Public Day As Int16
            Public Hour As Int16
            Public Minute As Int16
            Public Second As Int16
            Public Milliseconds As Int16


            Public Sub New(ByVal bytes As Byte(), ByVal index As Int32)

                Year = BitConverter.ToInt16(bytes, index)
                index = index + LENGTH_WORD

                Month = BitConverter.ToInt16(bytes, index)
                index = index + LENGTH_WORD

                DayOfWeek = BitConverter.ToInt16(bytes, index)
                index = index + LENGTH_WORD

                Day = BitConverter.ToInt16(bytes, index)
                index = index + LENGTH_WORD

                Hour = BitConverter.ToInt16(bytes, index)
                index = index + LENGTH_WORD

                Minute = BitConverter.ToInt16(bytes, index)
                index = index + LENGTH_WORD

                Second = BitConverter.ToInt16(bytes, index)
                index = index + LENGTH_WORD

                Milliseconds = BitConverter.ToInt16(bytes, index)

            End Sub


        End Class


        Shared Sub New()

            With Registry.LocalMachine

                Dim currentNameRegKey As String
                For Each currentNameRegKey In REG_KEYS_TIME_ZONES

                    If (Not (.OpenSubKey(currentNameRegKey) Is Nothing)) Then
                        nameRegKeyTimeZones = currentNameRegKey
                        Exit Sub
                    End If

                Next

            End With

        End Sub


        Private Shared Function GetAbbreviation(
          ByVal name As String
          ) As String

            Dim abbreviation As String = ""

            Dim nameChars As Char()
            nameChars = name.ToCharArray

            Dim currentChar As Char
            For Each currentChar In nameChars
                If (Char.IsUpper(currentChar)) Then
                    abbreviation = abbreviation & currentChar
                End If
            Next

            Return abbreviation

        End Function


        Private Shared Function LoadTimeZone(
          ByVal regKeyTimeZone As RegistryKey
          ) As Win32TimeZone

            Dim timeZoneIndex As Int32
            Dim displayName As String
            Dim standardName As String
            Dim daylightName As String
            Dim timeZoneData As Byte()

            With regKeyTimeZone
                timeZoneIndex = DirectCast(.GetValue(VALUE_INDEX), Int32)
                displayName = DirectCast(.GetValue(VALUE_DISPLAY_NAME), String)
                standardName = DirectCast(.GetValue(VALUE_STANDARD_NAME), String)
                daylightName = DirectCast(.GetValue(VALUE_DAYLIGHT_NAME), String)
                timeZoneData = DirectCast(.GetValue(VALUE_ZONE_INFO), Byte())
            End With

            If (timeZoneData.Length <> LENGTH_ZONE_INFO) Then
                Return Nothing
            End If

            Dim timeZoneInfo As New TZREGReader(timeZoneData)

            Dim standardOffset As New TimeSpan(
              0,
              -(timeZoneInfo.Bias + timeZoneInfo.StandardBias),
              0
              )

            Dim daylightDelta As New TimeSpan(
              0,
              -(timeZoneInfo.DaylightBias),
              0
              )

            If (
              (daylightDelta.Ticks = 0) Or
              (timeZoneInfo.StandardDate.Month = 0) Or
              (timeZoneInfo.DaylightDate.Month = 0)
              ) Then
                Return New Win32TimeZone(
                  timeZoneIndex,
                  displayName,
                  standardOffset,
                  standardName,
                  GetAbbreviation(standardName)
                  )
            End If

            If (
              (timeZoneInfo.StandardDate.Year <> 0) Or
              (timeZoneInfo.DaylightDate.Year <> 0)
              ) Then
                Return Nothing
            End If

            Dim daylightSavingsStart As DaylightTimeChange
            Dim daylightSavingsEnd As DaylightTimeChange

            With timeZoneInfo.DaylightDate
                daylightSavingsStart = New DaylightTimeChange(
                  .Month,
                  CType(.DayOfWeek, DayOfWeek),
                  (.Day - 1),
                  New TimeSpan(0, .Hour, .Minute, .Second, .Milliseconds)
                )
            End With

            With timeZoneInfo.StandardDate
                daylightSavingsEnd = New DaylightTimeChange(
                  .Month,
                  CType(.DayOfWeek, DayOfWeek),
                  (.Day - 1),
                  New TimeSpan(0, .Hour, .Minute, .Second, .Milliseconds)
                )
            End With

            Return New Win32TimeZone(
              timeZoneIndex,
              displayName,
              standardOffset,
              standardName,
              GetAbbreviation(standardName),
              daylightDelta,
              daylightName,
              GetAbbreviation(daylightName),
              daylightSavingsStart,
              daylightSavingsEnd
              )

        End Function


        Public Shared Function GetTimeZone(ByVal index As Int32) As Win32TimeZone

            If (nameRegKeyTimeZones Is Nothing) Then
                Return Nothing
            End If

            Dim regKeyTimeZones As RegistryKey
            Try
                regKeyTimeZones = Registry.LocalMachine.OpenSubKey(nameRegKeyTimeZones)
            Catch
            End Try

            If (regKeyTimeZones Is Nothing) Then
                Return Nothing
            End If

            Dim result As Win32TimeZone

            Dim currentNameSubKey As String
            Dim namesSubKeys As String()
            namesSubKeys = regKeyTimeZones.GetSubKeyNames()

            Dim currentSubKey As RegistryKey

            Dim timeZoneIndex As Int32

            For Each currentNameSubKey In namesSubKeys

                Try
                    currentSubKey = regKeyTimeZones.OpenSubKey(currentNameSubKey)
                Catch
                    currentSubKey = Nothing
                End Try

                If (Not (currentSubKey Is Nothing)) Then

                    Try

                        timeZoneIndex = DirectCast(currentSubKey.GetValue(VALUE_INDEX), Int32)

                        If (timeZoneIndex = index) Then
                            result = LoadTimeZone(currentSubKey)
                            currentSubKey.Close()
                            Exit For
                        End If

                    Catch
                    End Try

                    currentSubKey.Close()

                End If

            Next

            regKeyTimeZones.Close()

            Return result

        End Function


        Private Shared _timeZones As Win32TimeZone()

        Public Shared Function GetTimeZones() As Win32TimeZone()

            If (nameRegKeyTimeZones Is Nothing) Then
                Return New Win32TimeZone() {}
            End If

            If _timeZones Is Nothing Then


                Dim regKeyTimeZones As RegistryKey
                Try
                    regKeyTimeZones = Registry.LocalMachine.OpenSubKey(nameRegKeyTimeZones)
                Catch
                End Try

                If (regKeyTimeZones Is Nothing) Then
                    Return New Win32TimeZone() {}
                End If

                Dim results As New ArrayList

                Dim currentNameSubKey As String
                Dim namesSubKeys As String()
                namesSubKeys = regKeyTimeZones.GetSubKeyNames()

                Dim currentSubKey As RegistryKey

                Dim currentTimeZone As Win32TimeZone

                For Each currentNameSubKey In namesSubKeys

                    Try
                        currentSubKey = regKeyTimeZones.OpenSubKey(currentNameSubKey)
                    Catch
                        currentSubKey = Nothing
                    End Try

                    If (Not (currentSubKey Is Nothing)) Then

                        Try

                            Dim indexProperty As Object = currentSubKey.GetValue(VALUE_INDEX)
                            ' Timezones not addressed by the registry script will not have an Index property
                            ' avoid throwing exceptions by checking.
                            If Not indexProperty Is Nothing Then
                                currentTimeZone = LoadTimeZone(currentSubKey)

                                If (Not (currentTimeZone Is Nothing)) Then
                                    results.Add(currentTimeZone)
                                End If
                            End If
                        Catch

                        Finally
                            currentSubKey.Close()
                        End Try

                    End If

                Next

                regKeyTimeZones.Close()

                _timeZones = DirectCast(results.ToArray(GetType(Win32TimeZone)), Win32TimeZone())
            End If


            Return _timeZones

        End Function


    End Class

End Namespace

