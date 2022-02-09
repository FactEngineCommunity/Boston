Imports System.ComponentModel
Imports System.Xml.Serialization
Imports MindFusion.Diagramming

Namespace FBM
    <Serializable()> _
    Public Class RingConstraint
        Inherits FBM.RoleConstraintInstance

        <CategoryAttribute("Constraint Type"), _
        Browsable(True), _
        [ReadOnly](False), _
        BindableAttribute(True), _
        DefaultValueAttribute(""), _
        DesignOnly(False), _
        DescriptionAttribute("The type of Ring Constraint")> _
        Public Overrides Property RingConstraintType() As pcenumRingConstraintType
            Get
                Return Me._RingConstraintType
            End Get
            Set(ByVal value As pcenumRingConstraintType)
                Me._RingConstraintType = value
                Me.RoleConstraint.RingConstraintType = Me._RingConstraintType
            End Set
        End Property

        Sub New()

            MyBase.ConceptType = pcenumConceptType.RoleConstraint

        End Sub

        Public Overloads Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing)

            '------------------------------------------------
            'Set the values in the underlying Model.EntityType
            '------------------------------------------------
            If Me.RoleConstraint.Name = Me.Name Then
                '------------------------------------------------------------
                'Nothing to do. Name of the RoleConstraint has not been changed.
                '------------------------------------------------------------
            Else
                Me.RoleConstraint.SetName(Me.Name)
            End If

            If IsSomething(aoChangedPropertyItem) Then
                Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name
                    Case Is = "ShortDescription"
                        Me.RoleConstraint.ShortDescription = Me.ShortDescription
                    Case Is = "LongDescription"
                        Me.RoleConstraint.LongDescription = Me.LongDescription
                    Case Is = "IsDeontic"
                        Me.RoleConstraint.IsDeontic = Me.IsDeontic
                        If Me.RoleConstraint.RoleConstraintType = pcenumRoleConstraintType.RingConstraint Then
                            If Me.IsDeontic Then
                                Select Case Me.RingConstraintType
                                    Case Is = pcenumRingConstraintType.Acyclic
                                        Me.RingConstraintType = pcenumRingConstraintType.DeonticAcyclic
                                    Case Is = pcenumRingConstraintType.AcyclicIntransitive
                                        Me.RingConstraintType = pcenumRingConstraintType.DeonticAcyclicIntransitive
                                    Case Is = pcenumRingConstraintType.Antisymmetric
                                        Me.RingConstraintType = pcenumRingConstraintType.DeonticAntisymmetric
                                    Case Is = pcenumRingConstraintType.Asymmetric
                                        Me.RingConstraintType = pcenumRingConstraintType.DeonticAssymmetric
                                    Case Is = pcenumRingConstraintType.AsymmetricIntransitive
                                        Me.RingConstraintType = pcenumRingConstraintType.DeonticAssymmetricIntransitive
                                    Case Is = pcenumRingConstraintType.Intransitive
                                        Me.RingConstraintType = pcenumRingConstraintType.DeonticIntransitive
                                    Case Is = pcenumRingConstraintType.Irreflexive
                                        Me.RingConstraintType = pcenumRingConstraintType.DeonticIrreflexive
                                    Case Is = pcenumRingConstraintType.PurelyReflexive
                                        Me.RingConstraintType = pcenumRingConstraintType.DeonticPurelyReflexive
                                    Case Is = pcenumRingConstraintType.Symmetric
                                        Me.RingConstraintType = pcenumRingConstraintType.DeonticSymmetric
                                    Case Is = pcenumRingConstraintType.SymmetricIntransitive
                                        Me.RingConstraintType = pcenumRingConstraintType.DeonticSymmetricIntransitive
                                    Case Is = pcenumRingConstraintType.SymmetricIrreflexive
                                        Me.RingConstraintType = pcenumRingConstraintType.DeonticSymmetricIrreflexive
                                End Select
                            Else
                                Select Case Me.RingConstraintType
                                    Case Is = pcenumRingConstraintType.DeonticAcyclic
                                        Me.RingConstraintType = pcenumRingConstraintType.Acyclic
                                    Case Is = pcenumRingConstraintType.DeonticAcyclicIntransitive
                                        Me.RingConstraintType = pcenumRingConstraintType.AcyclicIntransitive
                                    Case Is = pcenumRingConstraintType.DeonticAntisymmetric
                                        Me.RingConstraintType = pcenumRingConstraintType.Antisymmetric
                                    Case Is = pcenumRingConstraintType.DeonticAssymmetric
                                        Me.RingConstraintType = pcenumRingConstraintType.Asymmetric
                                    Case Is = pcenumRingConstraintType.DeonticAssymmetricIntransitive
                                        Me.RingConstraintType = pcenumRingConstraintType.AsymmetricIntransitive
                                    Case Is = pcenumRingConstraintType.DeonticIntransitive
                                        Me.RingConstraintType = pcenumRingConstraintType.Intransitive
                                    Case Is = pcenumRingConstraintType.DeonticIrreflexive
                                        Me.RingConstraintType = pcenumRingConstraintType.Irreflexive
                                    Case Is = pcenumRingConstraintType.DeonticPurelyReflexive
                                        Me.RingConstraintType = pcenumRingConstraintType.PurelyReflexive
                                    Case Is = pcenumRingConstraintType.DeonticSymmetric
                                        Me.RingConstraintType = pcenumRingConstraintType.Symmetric
                                    Case Is = pcenumRingConstraintType.DeonticSymmetricIntransitive
                                        Me.RingConstraintType = pcenumRingConstraintType.SymmetricIntransitive
                                    Case Is = pcenumRingConstraintType.DeonticSymmetricIrreflexive
                                        Me.RingConstraintType = pcenumRingConstraintType.SymmetricIrreflexive
                                End Select
                            End If
                        End If
                    Case Is = "RingConstraintType"
                        Select Case Me.RingConstraintType
                            Case Is = pcenumRingConstraintType.AcyclicIntransitive
                                Me.Shape.Image = My.Resources.ORMShapes.acyclic_intransitive
                            Case Is = pcenumRingConstraintType.Acyclic
                                Me.Shape.Image = My.Resources.ORMShapes.acyclic
                            Case Is = pcenumRingConstraintType.Antisymmetric
                                Me.Shape.Image = My.Resources.ORMShapes.Antisymmetric
                            Case Is = pcenumRingConstraintType.Asymmetric
                                Me.Shape.Image = My.Resources.ORMShapes.Asymmetric
                            Case Is = pcenumRingConstraintType.AsymmetricIntransitive
                                Me.Shape.Image = My.Resources.ORMShapes.asymmetric_intransitive
                            Case Is = pcenumRingConstraintType.DeonticAcyclic
                                Me.Shape.Image = My.Resources.ORMShapes.deontic_acyclic
                                Me.IsDeontic = True
                            Case Is = pcenumRingConstraintType.DeonticAcyclicIntransitive
                                Me.Shape.Image = My.Resources.ORMShapes.deontic_acyclic_intransitive
                                Me.IsDeontic = True
                            Case Is = pcenumRingConstraintType.DeonticAntisymmetric
                                Me.Shape.Image = My.Resources.ORMShapes.deontic_antisymmetric
                                Me.IsDeontic = True
                            Case Is = pcenumRingConstraintType.DeonticAssymmetric
                                Me.Shape.Image = My.Resources.ORMShapes.deontic_asymmetric
                                Me.IsDeontic = True
                            Case Is = pcenumRingConstraintType.DeonticAssymmetricIntransitive
                                Me.Shape.Image = My.Resources.ORMShapes.deontic_asymmetric_intransitive
                                Me.IsDeontic = True
                            Case Is = pcenumRingConstraintType.DeonticIntransitive
                                Me.Shape.Image = My.Resources.ORMShapes.deontic_intransitive
                                Me.IsDeontic = True
                            Case Is = pcenumRingConstraintType.DeonticIrreflexive
                                Me.Shape.Image = My.Resources.ORMShapes.deontic_irreflexive
                                Me.IsDeontic = True
                            Case Is = pcenumRingConstraintType.DeonticPurelyReflexive
                                Me.Shape.Image = My.Resources.ORMShapes.deontic_purely_reflexive
                                Me.IsDeontic = True
                            Case Is = pcenumRingConstraintType.DeonticSymmetric
                                Me.Shape.Image = My.Resources.ORMShapes.deontic_symmetric
                                Me.IsDeontic = True
                            Case Is = pcenumRingConstraintType.DeonticSymmetricIntransitive
                                Me.Shape.Image = My.Resources.ORMShapes.deontic_symmetric_intransitive
                                Me.IsDeontic = True
                            Case Is = pcenumRingConstraintType.DeonticSymmetricIrreflexive
                                Me.Shape.Image = My.Resources.ORMShapes.deontic_symmetric_irreflexive
                                Me.IsDeontic = True
                            Case Is = pcenumRingConstraintType.Intransitive
                                Me.Shape.Image = My.Resources.ORMShapes.deontic_intransitive
                            Case Is = pcenumRingConstraintType.Irreflexive
                                Me.Shape.Image = My.Resources.ORMShapes.irreflexive
                            Case Is = pcenumRingConstraintType.PurelyReflexive
                                Me.Shape.Image = My.Resources.ORMShapes.purely_reflexive
                            Case Is = pcenumRingConstraintType.Symmetric
                                Me.Shape.Image = My.Resources.ORMShapes.symmetric
                            Case Is = pcenumRingConstraintType.SymmetricIntransitive
                                Me.Shape.Image = My.Resources.ORMShapes.symmetric_intransitive
                            Case Is = pcenumRingConstraintType.SymmetricIrreflexive
                                Me.Shape.Image = My.Resources.ORMShapes.symmetric_irreflexive
                            Case Is = pcenumRingConstraintType.SymmetricTransitive
                                Me.Shape.Image = My.Resources.ORMShapes.symmetric_transitive
                        End Select
                End Select
            End If

            Call Me.Page.Invalidate()
            Call Me.Page.Diagram.Invalidate()

        End Sub

        Public Overloads Sub SetAppropriateColour()

            If IsSomething(Me.Shape) Then
                If Me.Shape.Selected Then
                    Me.Shape.Pen.Color = Color.Blue
                Else
                    If Me.RoleConstraint.HasModelError Then
                        Me.Shape.Transparent = False
                        Me.Shape.Visible = True
                        Me.Shape.Pen.Color = Color.Red
                    Else
                        Me.Shape.Transparent = True
                        Me.Shape.Pen.Color = Color.White
                    End If
                End If
            End If

        End Sub

    End Class

End Namespace