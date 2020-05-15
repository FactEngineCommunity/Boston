
All of the token collection of a ParseTree can be done using System.Reflection and fairly easily.


        MsgBox(GetType(ClientServer.User).GetField("FirstName").FieldType.ToString)

        Call GetType(ClientServer.User).GetField("FirstName").SetValue(Me.zrUser, "Hi there")
        MsgBox(zrUser.FirstName)

1. Create the Classes that you need for each token type
2. Create instance of Objects of the Classes created in 1 (above).
3. Use the same/similar code as found in tVAQLProcessor.vb, but using the reflection code above.

i.e. Copy the relevent code from tVAQLProcessor.vb to this folder and do the above 3 steps.