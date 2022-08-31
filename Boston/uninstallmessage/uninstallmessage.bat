set message=To fully remove Boston from your PC, remove the following directory: C:\ProgramData\FactEngine (ProgramData is a hidden folder supplied by Windows). The folder contains your database, log files, etc.
set message2=WARNING: To keep your database if reinstalling Boston, do not delete this folder.
reg delete "HKEY_CURRENT_USER\SOFTWARE\Boston" /f
start "" cmd /c "echo %message%& echo %message2%& echo(&pause"