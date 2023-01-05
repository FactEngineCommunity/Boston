Imports Boston.Parser.Syntax.Strings

Namespace Parser.Syntax

    Friend Class Constants

        Public Const TAG_BEGIN_DEFAULT As String = "<<!"
        Public Const TAG_END_DEFAULT As String = "!>>"

        Public Shared TAG_BEGIN As String = "<<!"
        Public Shared TAG_END As String = "!>>"

        Public Const VARIABLE_ATTRIBUTE_VALUE As String = "value"
        Public Const VARIABLE_ATTRIBUTE_OWNER As String = "owner" 'Boston specific. Not part of orginal Metadrone.        
        Public Const VARIABLE_ATTRIBUTE_ID As String = "id" 'Boston specific. Not part of orginal Metadrone.        
        Public Const VARIABLE_ATTRIBUTE_NAME As String = "name" 'Boston specific. Not part of orginal Metadrone.        
        Public Const VARIABLE_ATTRIBUTE_REFERENCINGTABLENAME As String = "referencingtablename" 'Boston specific. Not part of orginal Metadrone.
        Public Const VARIABLE_ATTRIBUTE_REFERENCINGCOLUMNNAME As String = "referencingcolumnname" 'Boston specific. Not part of orginal Metadrone.
        Public Const VARIABLE_ATTRIBUTE_REFERENCEDTABLENAME As String = "referencedtablename" 'Boston specific. Not part of orginal Metadrone.
        Public Const VARIABLE_ATTRIBUTE_REFERENCEDCOLUMNNAME As String = "referencedcolumnname" 'Boston specific. Not part of orginal Metadrone.
        Public Const VARIABLE_ATTRIBUTE_SCHEMAROWVAL As String = "schemarowval" 'Boston specific. Used to load Columns for Indexes.        
        Public Const VARIABLE_ATTRIBUTE_DATATYPE As String = "datatype"
        Public Const VARIABLE_ATTRIBUTE_ISIDENTITY As String = "isidentity"
        Public Const VARIABLE_ATTRIBUTE_ISPRIMARYKEY As String = "isprimarykey"
        Public Const VARIABLE_ATTRIBUTE_ISFOREIGNKEY As String = "isforeignkey"
        Public Const VARIABLE_ATTRIBUTE_NULLABLE As String = "nullable"
        Public Const VARIABLE_ATTRIBUTE_PGSEDGENAME As String = "pgsedgename" 'Boston specific. Not part of original Metadrone.
        Public Const VARIABLE_ATTRIBUTE_ISPGSRELATION As String = "ispgsrelation" 'Boston specific. Not part of original Metadrone.
        Public Const VARIABLE_ATTRIBUTE_ISOBJECTIFIED As String = "isobjectified" 'Boston specific. Not part of original Metadrone.                        
        Public Const VARIABLE_ATTRIBUTE_PRIMARYSUPERTYPENAME As String = "primarysupertypename" 'Boston specific. Not part of original Metadrone.
        Public Const VARIABLE_ATTRIBUTE_RELATIONS As String = "relations" 'Boston specific. Not part of original Metadrone.
        Public Const VARIABLE_ATTRIBUTE_INCOMINGRELATIONS As String = "incomingrelations" 'Boston specific. Not part of original Metadrone.
        Public Const VARIABLE_ATTRIBUTE_RELATION As String = "relation" 'Boston specific. Not part of original Metadrone.
        Public Const VARIABLE_ATTRIBUTE_INCOMINGRELATION As String = "incomingrelation" 'Boston specific. Not part of original Metadrone.
        Public Const VARIABLE_ATTRIBUTE_INDEX As String = "index" 'Boston specific. Not part of original Metadrone.
        Public Const VARIABLE_ATTRIBUTE_ALLOWZEROLENGTH As String = "allowzerolength" 'Boston specific. Not part of original Metadrone.
        Public Const VARIABLE_ATTRIBUTE_SHORTDESCRIPTION As String = "shortdescription" 'Boston specific. Not part of original Metadrone. The ShortDescription of the JoinedORMObject of the ActiveRole of the Column.
        Public Const VARIABLE_ATTRIBUTE_PREDICATE As String = "predicate" 'Boston specific. Not part of original Metadrone.
        Public Const VARIABLE_ATTRIBUTE_CHECKVALUE As String = "checkvalue" 'Boston specific. Not part of original Metadrone.
        Public Const VARIABLE_ATTRIBUTE_HASCHECKVALUE As String = "hascheckvalue" 'Boston specific. Not part of original Metadrone.
        Public Const VARIABLE_ATTRIBUTE_LENGTH As String = "length"
        Public Const VARIABLE_ATTRIBUTE_PRECISION As String = "precision"
        Public Const VARIABLE_ATTRIBUTE_SCALE As String = "scale"
        Public Const VARIABLE_ATTRIBUTE_LISTCOUNT As String = "listcount"
        Public Const VARIABLE_ATTRIBUTE_LISTPOS As String = "listpos"
        Public Const VARIABLE_ATTRIBUTE_COLUMN As String = "column" 'At this stage, used for the set of Columns in an Index.
        Public Const VARIABLE_ATTRIBUTE_COLUMNCOUNT As String = "columncount"
        Public Const VARIABLE_ATTRIBUTE_FBMFACTTYPENAME As String = "fbmfacttypename"
        Public Const VARIABLE_ATTRIBUTE_REFERENCINGROLENAME As String = "referencingrolename"
        Public Const VARIABLE_ATTRIBUTE_REFERENCEDROLENAME As String = "referencedrolename"
        Public Const VARIABLE_ATTRIBUTE_ISLINKFACTTYPE As String = "islinkfacttype"
        Public Const VARIABLE_ATTRIBUTE_PKCOLUMNCOUNT As String = "pkcolumncount"
        Public Const VARIABLE_ATTRIBUTE_FKCOLUMNCOUNT As String = "fkcolumncount"
        Public Const VARIABLE_ATTRIBUTE_IDCOLUMNCOUNT As String = "idcolumncount"
        Public Const VARIABLE_ATTRIBUTE_PARAMCOUNT As String = "paramcount"
        Public Const VARIABLE_ATTRIBUTE_INPARAMCOUNT As String = "inparamcount"
        Public Const VARIABLE_ATTRIBUTE_OUTPARAMCOUNT As String = "outparamcount"
        Public Const VARIABLE_ATTRIBUTE_INOUTPARAMCOUNT As String = "inoutparamcount"
        Public Const VARIABLE_ATTRIBUTE_MODE As String = "mode"
        Public Const VARIABLE_ATTRIBUTE_ISINMODE As String = "isinmode"
        Public Const VARIABLE_ATTRIBUTE_ISOUTMODE As String = "isoutmode"
        Public Const VARIABLE_ATTRIBUTE_ISINOUTMODE As String = "isinoutmode"

        Public Const VARIABLE_METHOD_REPLACE As String = "replace"
        Public Const VARIABLE_METHOD_INDEXOFCOL As String = "indexofcol"

        Public Const RESERVED_COMMENT_LINE As String = "//"
        'Public Const ACTION_COMMENT_BEGIN As String = "/*" 'Don't use these just now
        'Public Const ACTION_COMMENT_END As String = "*/"   'Don't use these just now

        Public Const GLOBALS_SOURCES As String = "sources"

        Public Const ACTION_ROOT As String = "ROOT"
        Public Const ACTION_HEADER As String = "header"

        Public Const DEPRECATED_HEADER_PATH As String = "path"
        Public Const HEADER_IS As String = "is"

        Public Const ACTION_CALL As String = "call"
        Public Const ACTION_FOR As String = "for"
        Public Const ACTION_IN As String = "in"
        Public Const ACTION_IF As String = "if"
        Public Const ACTION_ELSE As String = "else"
        Public Const ACTION_ELSEIF As String = "elseif"
        Public Const ACTION_END As String = "end"
        Public Const ACTION_SET As String = "set"
        Public Const ACTION_EXIT As String = "exit"
        Public Const ACTION_WHEN As String = "when"

        Public Const SYS_RETURN As String = "return"
        Public Const SYS_OUT As String = "out"
        Public Const SYS_OUTLN As String = "outln"
        Public Const SYS_COUT As String = "cout"
        Public Const SYS_COUTLN As String = "coutln"
        Public Const SYS_CLCON As String = "clcon"
        Public Const SYS_MAKEDIR As String = "makedir"
        Public Const SYS_FILECOPY As String = "filecopy"
        Public Const SYS_COMMAND As String = "command"
        Public Const SYS_RUNVB As String = "runvb"
        Public Const SYS_RUNCS As String = "runcs"
        Public Const SYS_RUNSCRIPT As String = "runscript"
        Public Const SYS_CNUM As String = "cnum"
        Public Const SYS_MATHS_SIN As String = "sin"
        Public Const SYS_MATHS_COS As String = "cos"
        Public Const SYS_MATHS_TAN As String = "tan"
        Public Const SYS_MATHS_SQRT As String = "sqrt"

        Public Const RESERVED_TRUE As String = "true"
        Public Const RESERVED_FALSE As String = "false"
        Public Const RESERVED_AND As String = "and"
        Public Const RESERVED_OR As String = "or"
        Public Const RESERVED_NOT As String = "not"
        Public Const RESERVED_NOTEQUALTO As String = "<>"
        Public Const RESERVED_LESSTHAN As String = "<"
        Public Const RESERVED_GREATERTHAN As String = ">"
        Public Shared RESERVED_OPERATORS() As String = {"`", "~", "!", "#", "$", "%", "^", "&", "*", "(", ")", "-", "+", "=", ",", _
                                                        ".", "/", "?", ";", ":", "'", "[", "{", "}", "]", "|", "\", "<", ">"}
        Public Const RESERVED_SEPERATOR As String = ";"
        Public Const RESERVED_PREPROC As String = "#"
        Public Const RESERVED_PREPROC_IGNORECASE As String = "ignorecase"
        Public Const RESERVED_PREPROC_IGNORECASE_ON As String = "on"
        Public Const RESERVED_PREPROC_IGNORECASE_OFF As String = "off"
        Public Const RESERVED_PREPROC_SAFEBEGIN As String = "safebegin"
        Public Const RESERVED_PREPROC_SAFEEND As String = "safeend"

        Public Const OBJECT_TABLE As String = "table"
        Public Const OBJECT_VIEW As String = "view"
        Public Const OBJECT_RELATIONS As String = "relations" 'Boston specific. Not part of original Metadrone. Used to get all Colum Relations for a Table
        Public Const OBJECT_INCOMINGRELATIONS As String = "incomingrelations" 'Boston specific. Not part of original Metadrone. Used to get all Colum Relations for a Table
        Public Const OBJECT_RELATION As String = "relation" 'Boston specific. Not part of original Metadrone. Used to get individual RDS.Relations for a Table
        Public Const OBJECT_INCOMINGRELATION As String = "incomingrelation" 'Boston specific. Not part of original Metadrone. Used to get individual RDS.Relations for a Table
        Public Const OBJECT_INDEX As String = "index" 'Boston specific. Not part of original Metadrone. Used to get individual Indexes for a Table
        Public Const OBJECT_STRING As String = "string" 'Boston specific. Not part of original Metadrone. Used to get individual RDS.Relations for a Table
        Public Const OBJECT_COLUMN As String = "column"
        Public Const OBJECT_PKCOLUMN As String = "pkcolumn"
        Public Const OBJECT_FKCOLUMN As String = "fkcolumn"
        Public Const OBJECT_IDCOLUMN As String = "idcolumn"
        Public Const OBJECT_FILE As String = "file"
        Public Const OBJECT_PROCEDURE As String = "procedure"
        Public Const OBJECT_FUNCTION As String = "function"
        Public Const OBJECT_PARAMETER As String = "param"
        Public Const OBJECT_INPARAMETER As String = "inparam"
        Public Const OBJECT_OUTPARAMETER As String = "outparam"
        Public Const OBJECT_INOUTPARAMETER As String = "inoutparam"

        Public Const FUNC_IGNORE As String = "addignore"
        Public Const FUNC_USEONLY As String = "adduseonly"
        Public Const FUNC_REPLACEALL As String = "addreplaceall"



        'Documentation
        Friend Shared DOCO_GLOBALS_SOURCES As String = "Select a source for this loop."
        Friend Shared DOCO_ACTION_SINGLE As String = "Code generation results in a single output file."
        Friend Shared DOCO_ACTION_HEADER_FOR As String = "Code generation results in multiple output files as determined by a for loop." & System.Environment.NewLine & _
                                                         "Eg: for table tblvar in conn" & System.Environment.NewLine & _
                                                         "or: for column col in tblvar"
        Friend Shared DOCO_ACTION_CALL As String = "Call a template." & System.Environment.NewLine & _
                                                   "Eg: call template(arg, list)"
        Friend Shared DOCO_ACTION_FOR As String = "Begin a for loop, proceeded by a loop type and variable declaration." & System.Environment.NewLine & _
                                                  "Eg: for table tblvar in conn" & System.Environment.NewLine & _
                                                  "or: for column col in tblvar"
        Friend Shared DOCO_ACTION_IF As String = "Begin an if condition."
        Friend Shared DOCO_ACTION_ELSE As String = "Else an if."
        Friend Shared DOCO_ACTION_ELSEIF As String = "ElseIf an if."
        Friend Shared DOCO_ACTION_END As String = "Enclose a for/if block."
        Friend Shared DOCO_ACTION_SET As String = "Declare or set a variable."
        Friend Shared DOCO_ACTION_EXIT As String = "Conditionally exit immediate block." & System.Environment.NewLine & _
                                                   "Eg: exit when tblvar.listpos > 10"

        Friend Shared DOCO_SYS_RETURN As String = "Return from this point." & System.Environment.NewLine & _
                                                  "Eg: return(""c:\output\path.ext"")" & System.Environment.NewLine & _
                                                  "or: return() to return with no output"
        Friend Shared DOCO_SYS_OUT As String = "Output a string." & System.Environment.NewLine & _
                                               "Eg: out(""text value"")"
        Friend Shared DOCO_SYS_OUTLN As String = "Output a string followed by new line." & System.Environment.NewLine & _
                                                 "Eg: outln(""text value"")"
        Friend Shared DOCO_SYS_COUT As String = "Output to the console." & System.Environment.NewLine & _
                                                "Eg: out(""text value"")"
        Friend Shared DOCO_SYS_COUTLN As String = "Output to the console followed by new line." & System.Environment.NewLine & _
                                                  "Eg: outln(""text value"")"
        Friend Shared DOCO_SYS_CLCON As String = "Clear the console." & System.Environment.NewLine & _
                                                 "Eg: clcon()"
        Friend Shared DOCO_SYS_MAKEDIR As String = "Create a directory." & System.Environment.NewLine & _
                                                   "Directories are not created in preview mode." & System.Environment.NewLine & _
                                                   "Eg: makedir(""create\directory"")" & System.Environment.NewLine & _
                                                   "or" & System.Environment.NewLine & _
                                                   "makedir(""c:\create\rooted\directory"")"
        Friend Shared DOCO_SYS_FILECOPY As String = "Copy a file." & System.Environment.NewLine & _
                                                    "Files are not copied in preview mode." & System.Environment.NewLine & _
                                                    "Eg: filecopy(""source.txt"", ""..\dest.txt"")" & System.Environment.NewLine & _
                                                    "or" & System.Environment.NewLine & _
                                                    "filecopy(""c:\source.txt"", ""c:\dest.txt"")"
        Friend Shared DOCO_SYS_COMMAND As String = "Execute a command line operation." & System.Environment.NewLine & _
                                                   "Not executed in preview mode." & System.Environment.NewLine & _
                                                   "Eg: command(""cmd"", ""/c delete c:\dev\file.txt"")" & System.Environment.NewLine & _
                                                   "or" & System.Environment.NewLine & _
                                                   "command(""c:\dev\postbuild.bat"", """")"
        Friend Shared DOCO_SYS_RUNCS As String = "Execute visual C sharp code." & System.Environment.NewLine & _
                                                 "Eg: runcs(""namespace.class.method(args,etc)"")" & System.Environment.NewLine & _
                                                 "    runcs(""CS.MyClass.DoThis(123, """"myargval"""")"")"
        Friend Shared DOCO_SYS_RUNVB As String = "Execute visual basic.net code." & System.Environment.NewLine & _
                                                 "Eg: runvb(""namespace.class.method(args,etc)"")" & System.Environment.NewLine & _
                                                 "    runvb(""VB.MyClass.DoThis(123, """"myargval"""")"")"
        Friend Shared DOCO_SYS_RUNSCRIPT As String = "Execute script against connection source." & System.Environment.NewLine & _
                                                     "Eg: runscript(sources.source1, ""script.sql"")"
        Friend Shared DOCO_SYS_CNUM As String = "Convert expression to numeric." & System.Environment.NewLine & _
                                                "Eg: out(cnum(""12"") + 3)"
        Friend Shared DOCO_SYS_MATHS_SIN As String = "Sin maths function."
        Friend Shared DOCO_SYS_MATHS_COS As String = "Cos maths function."
        Friend Shared DOCO_SYS_MATHS_TAN As String = "Tan maths function."
        Friend Shared DOCO_SYS_MATHS_SQRT As String = "Square root maths function."


        Friend Shared DOCO_RESERVED_COMMENT_LINE As String = "Comment."
        Friend Shared DOCO_RESERVED_DEFINE_GLOBAL As String = "Use a global."


        Friend Shared DOCO_OBJECT_TABLE As String = "Loop through tables in database connection."
        Friend Shared DOCO_OBJECT_VIEW As String = "Loop through views in database connection."
        Friend Shared DOCO_OBJECT_PROCEDURE As String = "Loop through stored procedures in database connection."
        Friend Shared DOCO_OBJECT_FUNCTION As String = "Loop through functions in database connection."
        Friend Shared DOCO_OBJECT_FILE As String = "Loop through files in directory."
        Friend Shared DOCO_OBJECT_RELATION As String = "Loop through Relations specific to the Table/Column." 'Boston specific. Not in original Metadrone.
        Friend Shared DOCO_OBJECT_INCOMINGRELATION As String = "Loop through Incoming Relations specific to the Table/Column." 'Boston specific. Not in original Metadrone.
        Friend Shared DOCO_OBJECT_INDEX As String = "Loop through Indexes specific to the Table." 'Boston specific. Not in original Metadrone.
        Friend Shared DOCO_OBJECT_STRING As String = "Loop through Strings specific to a variable." 'Boston specific. Not in original Metadrone.
        Friend Shared DOCO_OBJECT_COLUMN As String = "Loop through columns in the parent loop's current table/routine iteration."
        Friend Shared DOCO_OBJECT_PKCOLUMN As String = "Loop through primary key columns in the parent loop's current table/routine iteration."
        Friend Shared DOCO_OBJECT_FKCOLUMN As String = "Loop through foreign key columns in the parent loop's current table/routine iteration."
        Friend Shared DOCO_OBJECT_IDCOLUMN As String = "Loop through identity columns in the parent loop's current table/routine iteration."
        Friend Shared DOCO_OBJECT_PARAMETER As String = "Loop through parameters in the parent loop's current routine iteration."
        Friend Shared DOCO_OBJECT_INPARAMETER As String = "Loop through input parameters in the parent loop's current routine iteration."
        Friend Shared DOCO_OBJECT_OUTPARAMETER As String = "Loop through output parameters in the parent loop's current routine iteration."
        Friend Shared DOCO_OBJECT_INOUTPARAMETER As String = "Loop through input/output parameters in the parent loop's current routine iteration."


        Friend Shared DOCO_VARIABLE_ATTRIBUTE_OWNER As String = "Owner Table of an Index." 'Boston specific. Not part of original Metadrone        
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_ID As String = "Id of variable." 'Boston specific. Not part of original Metadrone        
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_NAME As String = "Name of variable." 'Boston specific. Not part of original Metadrone        
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_REFERENCINGTABLENAME As String = "The name of the Table to which the Relation belongs, of variable." 'Boston specific. Not part of original Metadrone
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_REFERENCINGCOLUMNNAME As String = "The name of the Column referencing another Table/Column by the Relation, of variable." 'Boston specific. Not part of original Metadrone
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_REFERENCEDTABLENAME As String = "The name of the Table referenced by the Relation, of variable." 'Boston specific. Not part of original Metadrone
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_REFERENCEDCOLUMNNAME As String = "The name of the Column referenced by the Relation, of variable." 'Boston specific. Not part of original Metadrone
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_VALUE As String = "Value of variable."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_DATATYPE As String = "Provider's type definition of variable."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_ISIDENTITY As String = "If is an identity column."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_ISPRIMARYKEY As String = "If is a primary key column/index."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_ISFOREIGNKEY As String = "If is a foreign key column."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_NULLABLE As String = "If is nullable field."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_DEFAULTVALUE As String = "Default value for field."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_PGSEDGENAME As String = "The name of the Edge if the Table is a Property Graph Schema Relation/Edge." 'Boston specific. Not part of original Metadrone.
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_ISPGSRELATION As String = "True if the Table is a Property Graph Schema Relation/Edge." 'Boston specific. Not part of original Metadrone.
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_ISOBJECTIFIED As String = "True if the Fact Type for the Table is an Objectified Fact Type." 'Boston specific. Not part of original Metadrone.        
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_PRIMARYSUPERTYPENAME As String = "The primary supertype name of the Table." 'Boston specific. Not part of original Metadrone.
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_RELATIONS As String = "Set of Relations relating to the Table." 'Boston specific. Not part of original Metadrone.
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_INCOMINGRELATIONS As String = "Set of Incoming Relations relating to the Table." 'Boston specific. Not part of original Metadrone.
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_RELATION As String = "Set of Relations relating to the Column." 'Boston specific. Not part of original Metadrone.
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_INCOMINGRELATION As String = "Set of Incoming Relations relating to the Column." 'Boston specific. Not part of original Metadrone.
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_INDEX As String = "Set of Relations relating to the Column." 'Boston specific. Not part of original Metadrone.
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_ALLOWZEROLENGTH As String = "Field allows zero length." 'Boston specific. Not part of original Metadrone.
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_SHORTDESCRIPTION As String = "Short Description of the Model Element for the field." 'Boston specific. Not part of original Metadrone.
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_PREDICATE As String = "A predicate for the Field." 'Boston specific. Not part of original Metadrone.
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_CHECKVALUE As String = "A list of check values for the Field." 'Boston specific. Not part of original Metadrone.
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_HASCHECKVALUE As String = "True if the count of check values for the Field > 0." 'Boston specific. Not part of original Metadrone.
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_LENGTH As String = "Length of field."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_PRECISION As String = "Precision of field."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_SCALE As String = "Scale of field."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_COLUMN As String = "Set of Columns relating to an Index." 'At this stage, just for Indexes.  'Boston specific. Not part of original Metadrone.
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_COLUMNCOUNT As String = "Number of columns in collection this variable refers to."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_FBMFACTTYPENAME As String = "The Fact Type responsible for the Relation."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_REFERENCINGROLENAME As String = "The Role Name of the Referencing Model Element for the Relation."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_REFERENCEDROLENAME As String = "The Role Name for the Referenced Model Element for the Relation."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_ISLINKFACTTYPE As String = "True if the relation is for a Link Fact Type."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_PKCOLUMNCOUNT As String = "Number of primary key columns in collection this variable refers to."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_FKCOLUMNCOUNT As String = "Number of foreign key columns in collection this variable refers to."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_IDCOLUMNCOUNT As String = "Number of identity columns in collection this variable refers to."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_PARAMCOUNT As String = "Number of parameters in collection this variable refers to."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_INPARAMCOUNT As String = "Number of input parameters in collection this variable refers to."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_OUTPARAMCOUNT As String = "Number of output parameters in collection this variable refers to."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_INOUTPARAMCOUNT As String = "Number of in-out parameters in collection this variable refers to."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_LISTCOUNT As String = "Number of items in collection this variable refers to."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_LISTPOS As String = "Index of list position this variable is in collection (starts at 1)."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_MODE As String = "Parameter input/output mode."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_ISINMODE As String = "If parameter mode in IN."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_ISOUTMODE As String = "If parameter mode is OUT."
        Friend Shared DOCO_VARIABLE_ATTRIBUTE_ISINOUTMODE As String = "If parameter mode is INOUT."
        Friend Shared DOCO_VARIABLE_METHOD_REPLACE As String = "Replaces all occurrences of a specified string in this variable's value with another specified string." & System.Environment.NewLine & _
                                                               "Eg: column.replace(""-"",""_"")"

        Friend Shared DOCO_VARIABLE_METHOD_INDEXOFCOL As String = "Returns index of column within a table (starts at 1), returning 0 (zero) if not found." & System.Environment.NewLine & _
                                                                  "Will throw an exception if attempting on a column variable." & System.Environment.NewLine & _
                                                                  "Eg: table.indexofcol(""name"")"
        Friend Shared DOCO_FUNC_IGNORE As String = "Add an iteration ignore for the variable ignoring anthing that matches the string expression." & System.Environment.NewLine & _
                                                   "Eg: tblvar.addignore(""Addresses"")" & System.Environment.NewLine & _
                                                   "or: colvar.addignore( ""FirstName"")"
        Friend Shared DOCO_FUNC_USEONLY As String = "Add an iteration restriction for the variable using only what matches the string expression." & System.Environment.NewLine & _
                                                    "Eg: tblvar.adduseonly(""Addresses"")" & System.Environment.NewLine & _
                                                    "or: colvar.adduseonly( ""FirstName"")"
        Friend Shared DOCO_FUNC_REPLACEALL As String = "Add a replace for all occurrences of a specified string." & System.Environment.NewLine & _
                                                       "Eg: tblvar.addreplaceall(""-"",""_"")" & System.Environment.NewLine & _
                                                       "or: colvar.addreplaceall(""-"",""_"")"




        Friend Shared Function IsReservedWord(ByVal value As String) As Boolean
            If String.IsNullOrEmpty(value) Then Return False

            If StrEq(value, ACTION_HEADER) Then Return True
            If StrEq(value, HEADER_IS) Then Return True

            If StrEq(value, ACTION_CALL) Then Return True
            If StrEq(value, ACTION_FOR) Then Return True
            If StrEq(value, ACTION_IN) Then Return True
            If StrEq(value, ACTION_IF) Then Return True
            If StrEq(value, ACTION_ELSE) Then Return True
            If StrEq(value, ACTION_ELSEIF) Then Return True
            If StrEq(value, ACTION_END) Then Return True
            If StrEq(value, ACTION_SET) Then Return True
            If StrEq(value, ACTION_EXIT) Then Return True
            If StrEq(value, ACTION_WHEN) Then Return True

            If StrEq(value, SYS_RETURN) Then Return True
            If StrEq(value, SYS_OUT) Then Return True
            If StrEq(value, SYS_OUTLN) Then Return True
            If StrEq(value, SYS_COUT) Then Return True
            If StrEq(value, SYS_COUTLN) Then Return True
            If StrEq(value, SYS_CLCON) Then Return True
            If StrEq(value, SYS_MAKEDIR) Then Return True
            If StrEq(value, SYS_FILECOPY) Then Return True
            If StrEq(value, SYS_COMMAND) Then Return True
            If StrEq(value, SYS_RUNCS) Then Return True
            If StrEq(value, SYS_RUNVB) Then Return True
            If StrEq(value, SYS_RUNSCRIPT) Then Return True
            If StrEq(value, SYS_CNUM) Then Return True
            If StrEq(value, SYS_MATHS_SIN) Then Return True
            If StrEq(value, SYS_MATHS_COS) Then Return True
            If StrEq(value, SYS_MATHS_TAN) Then Return True
            If StrEq(value, SYS_MATHS_SQRT) Then Return True

            If StrEq(value, RESERVED_TRUE) Then Return True
            If StrEq(value, RESERVED_FALSE) Then Return True
            If StrEq(value, RESERVED_AND) Then Return True
            If StrEq(value, RESERVED_OR) Then Return True
            If StrEq(value, RESERVED_NOT) Then Return True
            If StrEq(value, RESERVED_NOTEQUALTO) Then Return True
            If StrEq(value, RESERVED_LESSTHAN) Then Return True
            If StrEq(value, RESERVED_GREATERTHAN) Then Return True
            If StrEq(value, RESERVED_SEPERATOR) Then Return True
            If StrEq(value, RESERVED_PREPROC_IGNORECASE) Then Return True
            If StrEq(value, RESERVED_PREPROC_SAFEBEGIN) Then Return True
            If StrEq(value, RESERVED_PREPROC_SAFEEND) Then Return True

            If StrEq(value, OBJECT_RELATION) Then Return True 'Boston specific. Not part of the original Metadrone.
            If StrEq(value, OBJECT_INCOMINGRELATION) Then Return True 'Boston specific. Not part of the original Metadrone.
            If StrEq(value, OBJECT_INDEX) Then Return True 'Boston specific. Not part of the original Metadrone.
            If StrEq(value, OBJECT_STRING) Then Return True 'Boston specific. Not part of the original Metadrone.
            If StrEq(value, OBJECT_TABLE) Then Return True
            If StrEq(value, OBJECT_VIEW) Then Return True
            If StrEq(value, OBJECT_COLUMN) Then Return True
            If StrEq(value, OBJECT_IDCOLUMN) Then Return True
            If StrEq(value, OBJECT_PKCOLUMN) Then Return True
            If StrEq(value, OBJECT_FKCOLUMN) Then Return True
            If StrEq(value, OBJECT_FILE) Then Return True
            If StrEq(value, OBJECT_PROCEDURE) Then Return True
            If StrEq(value, OBJECT_FUNCTION) Then Return True
            If StrEq(value, OBJECT_PARAMETER) Then Return True
            If StrEq(value, OBJECT_INPARAMETER) Then Return True
            If StrEq(value, OBJECT_OUTPARAMETER) Then Return True
            If StrEq(value, OBJECT_INOUTPARAMETER) Then Return True

            If StrEq(value, GLOBALS_SOURCES) Then Return True

            If IsNumeric(value) Then Return True

            Return False
        End Function

        Friend Shared Function IsSystemAttribute(ByVal value As String) As Boolean
            If String.IsNullOrEmpty(value) Then Return False

            If StrEq(value, VARIABLE_ATTRIBUTE_OWNER) Then Return True 'Boston specific. Not part of original Metadrone.            
            If StrEq(value, VARIABLE_ATTRIBUTE_NAME) Then Return True 'Boston specific. Not part of original Metadrone.            
            If StrEq(value, VARIABLE_ATTRIBUTE_REFERENCINGTABLENAME) Then Return True 'Boston specific. Not part of original Metadrone.
            If StrEq(value, VARIABLE_ATTRIBUTE_REFERENCINGCOLUMNNAME) Then Return True 'Boston specific. Not part of original Metadrone.
            If StrEq(value, VARIABLE_ATTRIBUTE_REFERENCEDTABLENAME) Then Return True 'Boston specific. Not part of original Metadrone.
            If StrEq(value, VARIABLE_ATTRIBUTE_REFERENCEDCOLUMNNAME) Then Return True 'Boston specific. Not part of original Metadrone.
            If StrEq(value, VARIABLE_ATTRIBUTE_VALUE) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_DATATYPE) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_ISIDENTITY) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_ISPRIMARYKEY) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_ISFOREIGNKEY) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_NULLABLE) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_PGSEDGENAME) Then Return True 'Boston specific. Not part of original Metadrone.
            If StrEq(value, VARIABLE_ATTRIBUTE_ISPGSRELATION) Then Return True 'Boston specific. Not part of original Metadrone.
            If StrEq(value, VARIABLE_ATTRIBUTE_ISOBJECTIFIED) Then Return True 'Boston specific. Not part of original Metadrone.            
            If StrEq(value, VARIABLE_ATTRIBUTE_PRIMARYSUPERTYPENAME) Then Return True 'Boston specific. Not part of original Metadrone.
            If StrEq(value, VARIABLE_ATTRIBUTE_RELATIONS) Then Return True 'Boston specific. Not part of original Metadrone.
            If StrEq(value, VARIABLE_ATTRIBUTE_INCOMINGRELATIONS) Then Return True 'Boston specific. Not part of original Metadrone.
            If StrEq(value, VARIABLE_ATTRIBUTE_RELATION) Then Return True 'Boston specific. Not part of original Metadrone.            
            If StrEq(value, VARIABLE_ATTRIBUTE_INCOMINGRELATION) Then Return True 'Boston specific. Not part of original Metadrone.            
            If StrEq(value, VARIABLE_ATTRIBUTE_INDEX) Then Return True 'Boston specific. Not part of original Metadrone.
            If StrEq(value, VARIABLE_ATTRIBUTE_ALLOWZEROLENGTH) Then Return True 'Boston specific. Not part of original Metadrone.
            If StrEq(value, VARIABLE_ATTRIBUTE_SHORTDESCRIPTION) Then Return True 'Boston specific. Not part of original Metadrone. ShortDescription of the JoinedORMObject for the ActiveRole of the Column for a field.
            If StrEq(value, VARIABLE_ATTRIBUTE_PREDICATE) Then Return True 'Boston specific. Not part of original Metadrone.
            If StrEq(value, VARIABLE_ATTRIBUTE_CHECKVALUE) Then Return True 'Boston specific. Not part of original Metadrone.
            If StrEq(value, VARIABLE_ATTRIBUTE_HASCHECKVALUE) Then Return True 'Boston specific. Not part of original Metadrone.
            If StrEq(value, VARIABLE_ATTRIBUTE_LENGTH) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_PRECISION) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_SCALE) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_LISTCOUNT) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_LISTPOS) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_COLUMN) Then Return True 'Boston specific. Not part of original Metadrone. Used just for Indexes at this stage.
            If StrEq(value, VARIABLE_ATTRIBUTE_COLUMNCOUNT) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_FBMFACTTYPENAME) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_REFERENCINGROLENAME) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_REFERENCEDROLENAME) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_ISLINKFACTTYPE) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_PKCOLUMNCOUNT) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_FKCOLUMNCOUNT) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_IDCOLUMNCOUNT) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_PARAMCOUNT) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_INPARAMCOUNT) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_OUTPARAMCOUNT) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_INOUTPARAMCOUNT) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_MODE) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_ISINMODE) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_ISOUTMODE) Then Return True
            If StrEq(value, VARIABLE_ATTRIBUTE_ISINOUTMODE) Then Return True

            Return False
        End Function

        Friend Shared Function ContainsOperator(ByVal value As String) As Boolean
            For i As Integer = 0 To RESERVED_OPERATORS.Count - 1
                If value.IndexOfAny(RESERVED_OPERATORS(i).ToCharArray) > -1 Then
                    Return True
                End If
            Next
        End Function

        Friend Shared Function ContainsOperator_ExceptPeriod(ByVal value As String) As Boolean
            For i As Integer = 0 To RESERVED_OPERATORS.Count - 1
                If RESERVED_OPERATORS(i).Equals(".") Then Continue For
                If value.IndexOfAny(RESERVED_OPERATORS(i).ToCharArray) > -1 Then Return True
            Next
        End Function

        Friend Shared Function IsOperator(ByVal value As String) As Boolean
            For i As Integer = 0 To RESERVED_OPERATORS.Count - 1
                If value.Equals(RESERVED_OPERATORS(i)) Then Return True
            Next
        End Function

    End Class

End Namespace