Option Explicit On
Option Strict On
Option Compare Binary

Imports System
Imports System.Globalization

Imports LegacyBL.Globalization


' Win32TimeZone
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
' class that uses Win32TimeZone.

' This should have been part of Microsoft.Win32, so that is
' where I located it.
Namespace Win32


    <Serializable()> Public Class Win32TimeZone
        Inherits SimpleTimeZone


        Private _index As Int32
        Private _displayName As String


        Public Sub New(
          ByVal index As Int32,
          ByVal displayName As String,
          ByVal standardOffset As TimeSpan,
          ByVal standardName As String,
          ByVal standardAbbreviation As String
        )

            MyBase.New(
              standardOffset,
              standardName,
              standardAbbreviation
              )

            _index = index
            _displayName = displayName

        End Sub

        Public Sub New(
          ByVal index As Int32,
          ByVal displayName As String,
          ByVal standardOffset As TimeSpan,
          ByVal standardName As String,
          ByVal standardAbbreviation As String,
          ByVal daylightDelta As TimeSpan,
          ByVal daylightName As String,
          ByVal daylightAbbreviation As String,
          ByVal daylightTimeChangeStart As DaylightTimeChange,
          ByVal daylightTimeChangeEnd As DaylightTimeChange
        )

            MyBase.New(
              standardOffset,
              standardName,
              standardAbbreviation,
              daylightDelta,
              daylightName,
              daylightAbbreviation,
              daylightTimeChangeStart,
              daylightTimeChangeEnd
              )

            _index = index
            _displayName = displayName

        End Sub


        Public ReadOnly Property Index() As Int32
            Get
                Return _index
            End Get
        End Property


        Public ReadOnly Property DisplayName() As String
            Get
                Return _displayName
            End Get
        End Property


        Public Overrides Function ToString() As String
            Return _displayName
        End Function


    End Class


End Namespace

