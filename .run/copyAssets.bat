rmdir /s /q "UI/bin/Debug/net6.0/Assets"
Xcopy "InitLoader/Assets" "UI/bin/Debug/net6.0/Assets" /E/H/C/I

del "UI/bin/Debug/net6.0/7z.dll"
copy InitLoader\7z.dll UI\bin\Debug\net6.0

del "UI/bin/Debug/net6.0/Config.json"
copy InitLoader\Config.json UI\bin\Debug\net6.0