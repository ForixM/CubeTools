rmdir /s /q "UI/bin/Debug/net6.0/Assets"
Xcopy "InitLoader/Assets" "UI/bin/Debug/net6.0/Assets/default" /E/H/C/I

del "UI/bin/Debug/net6.0/7z.dll"
Xcopy "InitLoader/7z.dll" "UI/bin/Debug/net6.0" /C

del "UI/bin/Debug/net6.0/Config.json"
Xcopy "InitLoader/Config.json" "UI/bin/Debug/net6.0" /C