   new Shape( _
       new ElementTemplate() _
       { _
            new LineTemplate(0, 0, 100, 0), _ 
            new LineTemplate(100, 0, 100, 100), _ 
            new LineTemplate(100, 100, 0, 100), _ 
            new LineTemplate(0, 100, 0, 0) _ 
       }, _ 
       new ElementTemplate() _
       { _
       }, _ 
       new ElementTemplate() _
       { _
            new LineTemplate(0, 0, 100, 0), _ 
            new LineTemplate(100, 0, 100, 100), _ 
            new LineTemplate(100, 100, 0, 100), _ 
            new LineTemplate(0, 100, 0, 0) _ 
       }, _ 
 nothing, FillMode.Winding,   "test" ) 
