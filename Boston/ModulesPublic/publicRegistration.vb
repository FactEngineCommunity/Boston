Imports System.Security.Cryptography
Imports System.Text
Imports System.Reflection

Module publicRegistration

    Public Function CheckRegistration(ByVal asApplicationKey As String, ByVal asRegistrationKey As String, ByRef arRegistrationResult As tRegistrationResult) As Boolean

        Try

            Dim lsProductIdentifier As String
            Dim lsSoftwareType As String
            Dim lsSubscriptionType As String
            Dim lsDate As String
            Dim lbResult As Boolean = False


            'asApplicationKey,lsSoftwareType,lsSubscriptionType,lsDate

            'Student
            lsProductIdentifier = FormatProductIdentifier(asApplicationKey, "Student", "None", "None")
            If asRegistrationKey = FormatLicenseKey(GetMd5Sum(lsProductIdentifier)) Then
                arRegistrationResult.IsRegistered = True
                arRegistrationResult.SoftwareType = "Student"
                arRegistrationResult.SubscriptionType = "None"
                lbResult = True
                GoTo ReturnResult
            End If

            'Professional - No Subscription
            lsProductIdentifier = FormatProductIdentifier(asApplicationKey, "Professional", "None", "None")
            If asRegistrationKey = FormatLicenseKey(GetMd5Sum(lsProductIdentifier)) Then
                arRegistrationResult.IsRegistered = True
                arRegistrationResult.SoftwareType = "Professional"
                arRegistrationResult.SubscriptionType = "None"
                lbResult = True
                GoTo ReturnResult
            End If

            'Professional - Active Subscription
            For liInd = 1 To 12

                Dim lsTrialDate As String = "01/" & Today.AddMonths(liInd).Month.ToString("D2") & "/" & Today.AddMonths(liInd).Year.ToString

                lsProductIdentifier = FormatProductIdentifier(asApplicationKey, "Professional", "Subscription", lsTrialDate)
                If asRegistrationKey = FormatLicenseKey(GetMd5Sum(lsProductIdentifier)) Then
                    arRegistrationResult.IsRegistered = True
                    arRegistrationResult.SoftwareType = "Professional"
                    arRegistrationResult.SubscriptionType = "Subscription"
                    arRegistrationResult.RegisteredToDate = lsTrialDate
                    lbResult = True
                    GoTo ReturnResult
                End If
            Next

            'Professional - Active Trial
            For liInd = 1 To 30

                Dim lsTrialDate As String = "01/" & Today.AddDays(liInd).Month.ToString & "/" & Today.AddDays(liInd).Year.ToString

                lsProductIdentifier = FormatProductIdentifier(asApplicationKey, "Professional", "Trial", lsTrialDate)
                If asRegistrationKey = FormatLicenseKey(GetMd5Sum(lsProductIdentifier)) Then
                    arRegistrationResult.IsRegistered = True
                    arRegistrationResult.SoftwareType = "Professional"
                    arRegistrationResult.SubscriptionType = "Subscription"
                    arRegistrationResult.RegisteredToDate = lsTrialDate
                    lbResult = True
                    GoTo ReturnResult
                End If
            Next

ReturnResult:
            Return lbResult

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try

    End Function

    Private Function FormatProductIdentifier(ByVal asApplicationKey As String,
                                             ByVal asSoftwareType As String,
                                             ByVal asSubscriptionType As String,
                                             ByVal asDate As String) As String

        Try
            Dim lsProductIdentifier As String

            lsProductIdentifier = Trim(asApplicationKey)
            lsProductIdentifier &= "-"
            lsProductIdentifier &= Trim(asSoftwareType)
            lsProductIdentifier &= "-"
            lsProductIdentifier &= Trim(asSubscriptionType)
            lsProductIdentifier &= "-"
            lsProductIdentifier &= Trim(asDate)

            Return lsProductIdentifier

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Function


    ''' <summary>
    ''' Generates a unique Application Key for the application.
    ''' From: https://www.businessandprocess.com/2012/07/generating-serial-numbers-and-keys-in-c-and-vb-net/
    ''' </summary>
    ''' <returns></returns>
    Public Function GenerateApplicationKey() As String
        Dim serialGuid As Guid = Guid.NewGuid()
        Dim uniqueSerial As String = serialGuid.ToString("N")
        Dim uniqueSerialLength As String = uniqueSerial.Substring(0, 28).ToUpper()

        Dim serialArray As Char() = uniqueSerialLength.ToCharArray()
        Dim finalSerialNumber As String = ""

        Dim j As Integer = 0
        For i As Integer = 0 To 27
            For j = i To 4 + (i - 1)
                finalSerialNumber += serialArray(j)
            Next
            If j = 28 Then
                Exit For
            Else
                i = (j) - 1
                finalSerialNumber += "-"
            End If
        Next


        Return finalSerialNumber
    End Function

    ''' <summary>
    ''' From: https://bytutorial.com/blogs/aspnet/how-to-create-a-simple-license-key-generator-in-csharp
    ''' </summary>
    ''' <param name="productIdentifier"></param>
    ''' <returns></returns>
    Public Function GetMd5Sum(ByVal productIdentifier As String) As String

        Dim enc As System.Text.Encoder = System.Text.Encoding.Unicode.GetEncoder()
        Dim unicodeText As Byte() = New Byte(productIdentifier.Length * 2 - 1) {}
        enc.GetBytes(productIdentifier.ToCharArray(), 0, productIdentifier.Length, unicodeText, 0, True)
        Dim md5 As MD5 = New MD5CryptoServiceProvider()
        Dim result As Byte() = md5.ComputeHash(unicodeText)
        Dim sb As StringBuilder = New StringBuilder()

        For i As Integer = 0 To result.Length - 1
            sb.Append(result(i).ToString("X2"))
        Next

        Return sb.ToString()
    End Function

    ''' <summary>
    ''' From: https://bytutorial.com/blogs/aspnet/how-to-create-a-simple-license-key-generator-in-csharp
    ''' </summary>
    ''' <param name="productIdentifier"></param>
    ''' <returns></returns>
    Public Function FormatLicenseKey(ByVal productIdentifier As String) As String

        productIdentifier = productIdentifier.Substring(0, 28).ToUpper()
        Dim serialArray As Char() = productIdentifier.ToCharArray()
        Dim licenseKey As StringBuilder = New StringBuilder()
        Dim j As Integer = 0

        For i As Integer = 0 To 28 - 1

            For j = i To 4 + i - 1
                licenseKey.Append(serialArray(j))
            Next

            If j = 28 Then
                Exit For
            Else
                i = (j) - 1
                licenseKey.Append("-")
            End If
        Next

        Return licenseKey.ToString()

    End Function

End Module
