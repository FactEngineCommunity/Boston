SELECT 
    MSysObjects.TABLE_NAME, 
    MSysObjects.TABLE_TYPE, 
FROM 
    MSysObjects 
WHERE 
    ((MSysObjects.Flags=0) AND (MSysObjects.Type=1)) 
ORDER BY 
    MSysObjects.Name 
