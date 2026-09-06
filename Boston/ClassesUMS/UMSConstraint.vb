Imports YamlDotNet.Serialization

Namespace UMS

    ''' <summary>
    ''' A class to store Fact-Based Modelling Constraint details in Constrolled Natural Language.
    '''   NB Is for constraints not otherwise covered by the Primary Key and Uniqueness Constraints within UMS.Type etc.
    ''' </summary>    
    Public Class FBMConstraint
        Inherits UMS.ModelElement

        ''' <summary>
        ''' Verbalisation of the Fact-Based Modelling Constraint.
        ''' E.g. The following is a Join Subset Constraint:
        '''
        ''' If some Booking has some Seat then
        ''' that Booking is for some Session that is at some Cinema that contains some Row that contains that Seat;
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Verbalisation As String

    End Class

End Namespace
