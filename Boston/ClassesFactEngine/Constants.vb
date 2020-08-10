Namespace FactEngine
    Public Module Constants

        Public Enum pcenumWhichClauseType
            None = 0
            WhichPredicateNodePropertyIdentification = 1 '      E.g. WHICH has (emailAddress:'hamish.bentley@qut.com.au)
            IsPredicateNodePropertyIdentification '    1 also   E.g. IS in (Semester:'1')  'NB Possibly not required, because IS will be in the Predicate
            PredicateWHICHModelElement = 2 '                    E.g. occupies WHICH Room
            AndThatPredicateThatModelElement = 3 '              E.g. AND THAT Faculty has THAT School
            AndThatModelElementPredicateThatModelElement = 4 '  E.g. AND THAT School is in THAT Faculty
            UnkownPredicateWhichModelElement = 5 '              E.g. has WHICH RoomName
            AndPredicateWhichModelElement = 6 '                 E.g. AND has WHICH RoomName
            AndThatModelElementPredicateWhichModelElement = 7 '     E.g. AND THAT Faculty has FacultyName
            AndThatModelElementPredicateModelElement = 8 'E.g. AND THAT Faculty has FacultyName (as per 7 above)
            AndWhichPredicateNodePropertyIdentification = 9 '   E.g. AND WHICH is in (Faculty:IT')
            WhichPredicateThatModelElement = 10 '               E.g. WHICH involves THAT Lecturer
            ThatPredicateWhichModelElement = 11 '               E.g. THAT involves WHICH Room
            WithClause = 12 '                                   E.g. WITH WHAT Rating (as in "WHICH Person likes WHICH City WITH WHAT Rating"), can also be WITH (Rating:'10') for instance.
            ISNOTClause = 13 '                                  E.g. Person 1 IS NOT Person 2

            '?? below
            'AndThatModelElementPredicateWhichModelElement '    E.g. AND THAT Faculty has WHICH School. Currently unused.Checked.
            'PredicateAModelElement '                           E.g. has A RoomName. Currently unused.Checked.
        End Enum

        Public Enum pcenumFEQLStatementType
            None
            DESCRIBEStatement
            ENUMERATEStatement
            WHICHSELECTStatement
        End Enum


    End Module

End Namespace
