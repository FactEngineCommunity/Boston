Imports YamlDotNet.Serialization
Imports YamlDotNet.Serialization.NamingConventions

Namespace UMS

    Public Class UniquenessConstraint

        ''' <summary>
        ''' The unique Name of the Constraint within the Model.
        ''' UQ_Person_LoginName 
        ''' </summary>
        Public Property Name As String

        ''' <summary>
        ''' For the Properties/Attributes/Columns of the Constraint.
        ''' ALTER TABLE Booking
        ''' ADD CONSTRAINT UQ_Booking_PersonFilm UNIQUE (Person_Id, Film_Id, DateTime);        
        ''' </summary>
        Public Property Properties As New List(Of String)

    End Class

End Namespace