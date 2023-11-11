# Zip up your dll, icon, readme, and manifest
$dll = $( dotnet msbuild -t:LogTargetPath -nologo ).trim()
Compress-Archive -Force -DestinationPath ./thunderstore-upload.zip -Path manifest.json,$dll,README.md,icon.png