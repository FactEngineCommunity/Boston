Namespace TestManagement
    Public Class AllocatedTestCase
        Public Property ProjectName As String
        Public Property TestPlanName As String
        Public Property TestSetName As String
        Public Property TestCycleName As String
        Public Property TestCaseCode As String
        Public Property TestCaseName As String
        Public Property ProjectId As String
        Public Property TestPlanIdentifier As String
        Public Property TestSetIdentifier As String
        Public Property TestCycleIdentifier As String
        Public Property TestCaseIdentifier As String

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByVal asProjectName As String,
                       ByVal asTestPlanName As String,
                       ByVal asTestSetName As String,
                       ByVal asTestCycleName As String,
                       ByVal asTestCaseName As String,
                       ByVal asProjectId As String,
                       ByVal asTestPlanIdentifier As String,
                       ByVal asTestSetIdentifier As String,
                       ByVal asTestCycleIdentifier As String,
                       ByVal asTestCaseIdentifer As String)

            Me.ProjectName = asProjectName
            Me.TestPlanName = asTestPlanName
            Me.TestSetName = asTestSetName
            Me.TestCycleName = asTestCycleName
            Me.TestCaseName = asTestCaseName
            Me.ProjectId = asProjectId
            Me.TestPlanIdentifier = asTestPlanIdentifier
            Me.TestSetIdentifier = asTestSetIdentifier
            Me.TestCycleIdentifier = asTestCycleIdentifier
            Me.TestCaseIdentifier = asTestCaseIdentifer

        End Sub

    End Class

End Namespace

