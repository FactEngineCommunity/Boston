Namespace FEQL

    Partial Public Class Processor

        ''' <summary>
        ''' Returns the SQL for the Derivation
        ''' </summary>
        ''' <param name="asDerivationText">The Derivation Text of the FactType</param>
        ''' <param name="arFactType">The FactType for the Derivation Text</param>
        ''' <returns></returns>
        Public Function processDerivationText(ByVal asDerivationText As String,
                                              ByRef arFactType As FBM.FactType) As String

            Try

                Me.Parsetree = Me.Parser.Parse(asDerivationText)

                Dim lrDerivationClause As New FEQL.DERIVATIONCLAUSE


                If Me.Parsetree.Errors.Count > 0 Then
                    Throw New Exception("Error in Derivation Text for Fact Type, '" & arFactType.Id & "'.")
                End If

                Call Me.GetParseTreeTokensReflection(lrDerivationClause, Me.Parsetree.Nodes(0))

                'PSEUDOCODE
                'Get the FactTypes for the DerivationSubClauses
                'Analyse/Get the type of Derivation
                'Return the SQL for the Derivation

#Region "Get Fact Types For Each DerivationSubClause"

                For Each lrDerivationSubClause In lrDerivationClause.DERIVATIONSUBCLAUSE

                    If lrDerivationSubClause.isFactTypeOnly Then
                        'Get the FactType for the DerivationSubClause
                        Dim larModelObject As New List(Of FBM.ModelObject)
                        Dim larRole As New List(Of FBM.Role)
                        Dim lrFactTypeReading As New FBM.FactTypeReading
                        Dim lrFactType As New FBM.FactType("DummyFactType")

                        Dim lrSentence As New Language.Sentence("DummySentence")

                        Dim liInd = 0
                        For Each lsModelElementName In lrDerivationSubClause.MODELELEMENTNAME
                            Dim lrModelObject = Me.Model.GetModelObjectByName(lsModelElementName)
                            larModelObject.Add(lrModelObject)

                            Dim lrRole = New FBM.Role(lrFactType, lrModelObject)
                            larRole.Add(lrRole)

                            Dim lrPredicatePart = New Language.PredicatePart()
                            Dim lsPredicatePartText = ""
                            If liInd < lrDerivationSubClause.MODELELEMENTNAME.Count - 1 Then
                                For Each lsPredicatePart In lrDerivationSubClause.PREDICATECLAUSE(liInd).PREDICATE
                                    lsPredicatePartText &= lsPredicatePart & " "
                                Next
                                lrPredicatePart.PredicatePartText = Trim(lsPredicatePartText)
                            Else
                                lrPredicatePart.PredicatePartText = ""
                            End If
                            lrSentence.PredicatePart.Add(lrPredicatePart)

                            liInd += 1
                        Next

                        lrFactTypeReading = New FBM.FactTypeReading(lrFactType, larRole, lrSentence)

                        lrDerivationSubClause.FBMFactType = Me.Model.getFactTypeByModelObjectsFactTypeReading(larModelObject, lrFactTypeReading)

                    End If

                Next

#End Region

                Select Case Me.getDerivationType(lrDerivationClause)
                    Case Is = FactEngine.pcenumFEQLDerivationType.TransitiveRingConstraintJoin

                        Return Me.getTransitiveRingConstraintJoinSQL(lrDerivationClause, arFactType)

                End Select

                Return ""

            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

        End Function

        Private Function getTransitiveRingConstraintJoinSQL(ByRef arDerivationClause As FEQL.DERIVATIONCLAUSE,
                                                            ByRef arFactType As FBM.FactType)

            ''(
            ''With RECURSIVE my_tree As (
            ''   Select Case PPA.Protein_Id, PPA.Protein_Id1, PDA.Disease_Id As Disease_Id
            ''   From ProteinProteinAssociation PPA,
            ''        ProteinDiseaseAssociation PDA
            ''   Where PPA.Protein_Id1 = PDA.Protein_Id

            ''                    UNION ALL

            ''   Select Case c.Protein_Id, c.Protein_Id1, p.Disease_Id
            ''   From ProteinProteinAssociation c
            ''     Join my_tree p           
            ''       On p.Protein_Id = c.Protein_Id1
            '') 
            ''Select Case Protein_Id, Disease_Id
            ''From my_tree
            '') PPDA

            Dim lsSQL As String = ""

            lsSQL = "(" & vbCrLf
            lsSQL &= "With RECURSIVE my_tree As (" & vbCrLf
            lsSQL &= "SELECT "

            Dim liInd = 0
            For Each lrColumn In arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.getCorrespondingRDSTable.Column
                If liInd > 0 Then lsSQL &= ","
                lsSQL &= lrColumn.Table.Name & "." & lrColumn.Name
                liInd += 1
            Next
            lsSQL &= ","
            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(1).FBMFactType.getCorrespondingRDSTable.Name & "."
            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(1).FBMFactType.getCorrespondingRDSTable.Column(1).Name & vbCrLf

            lsSQL &= "FROM "

            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.Id & "," & vbCrLf
            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(1).FBMFactType.Id & vbCrLf

            lsSQL &= "WHERE " & arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.Id & "."
            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.getCorrespondingRDSTable.Column(1).Name & " = "
            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(1).FBMFactType.Id & "."
            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(1).FBMFactType.getCorrespondingRDSTable.Column(0).Name & vbCrLf

            lsSQL &= "UNION ALL "

            lsSQL &= "SELECT "

            liInd = 0
            For Each lrColumn In arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.getCorrespondingRDSTable.Column
                If liInd > 0 Then lsSQL &= ","
                lsSQL &= lrColumn.Table.Name & "." & lrColumn.Name
                liInd += 1
            Next
            lsSQL &= ", "
            lsSQL &= "p."
            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(1).FBMFactType.getCorrespondingRDSTable.Column(1).Name & vbCrLf

            lsSQL &= "FROM " & arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.Id & vbCrLf

            lsSQL &= "JOIN my_tree p" & vbCrLf
            lsSQL &= "ON p." & arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.getCorrespondingRDSTable.Column(0).Name & " = "

            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.Id & "."
            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.getCorrespondingRDSTable.Column(1).Name & vbCrLf

            lsSQL &= ")" & vbCrLf
            lsSQL &= "SELECT " '&  Protein_Id, Disease_Id

            liInd = 0
            For Each lrColumn In arFactType.getCorrespondingRDSTable.Column
                If liInd > 0 Then lsSQL &= ","
                lsSQL &= lrColumn.Name
                liInd += 1
            Next

            lsSQL &= vbCrLf & "FROM my_tree" & vbCrLf
            lsSQL &= ") " & arFactType.Name

            Return lsSQL


        End Function



        ''' <summary>
        ''' Returns the type of Derivation for the given DerivationClause. E.g. TransitiveRingConstraintJoin
        ''' </summary>
        ''' <param name="arDerivationClause"></param>
        ''' <returns></returns>
        Public Function getDerivationType(ByRef arDerivationClause As FEQL.DERIVATIONCLAUSE) As FactEngine.pcenumFEQLDerivationType

            If arDerivationClause.DERIVATIONSUBCLAUSE.Count = 2 And
               arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.hasTransitiveRingConstraint Then

                Return FactEngine.pcenumFEQLDerivationType.TransitiveRingConstraintJoin

            End If

            Return FactEngine.pcenumFEQLDerivationType.None

        End Function

    End Class

End Namespace

