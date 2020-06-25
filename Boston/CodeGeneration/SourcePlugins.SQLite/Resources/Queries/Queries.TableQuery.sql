SELECT 
    name, 
    type 
FROM 
    SQLITE_MASTER 
WHERE 
    (type = 'table' OR type = 'view') 
AND 
    name <> 'sqlite_sequence' 
ORDER BY 
    name
