Classes in this folder are for use within the frmDiagramSTD form.

The classes under the ClassesLANGUAGEORM/ORMSTD folder are used for direct attachment to the ORM Model.

CMML modification is done in the same way as for the RDS model, in that the classes under the ClassesLANGUAGEORM/ORMCMML folder are used to modify the CMML/Core tables.
I.e. The 'tables' (ORM relations) within the Core/CMML explicity for State Transition Diagrams are treated as if they are part of the model.
I.e. In as much as RDS relations (the relational model) is part of the model, so to are state transitions...they are part of the CMML (Common MetaModelling Language).

Narritive
=========
At first may be hard to fathom because STDs are not as easy to conceptualise as the RDS as being part of the model, but when pictured
as a natural extension of any ValueType that has a set of ValueConstraints...that have transitions...then it becomes evident that STDs are very much part of such a model.

The 'Core' model contains the set of STD relations for this reason.