param($brcInstallDirectory)

# Auto-detect game's install location
if(-not $brcInstallDirectory) {
    $brcInstallDirectory=$(dotnet msbuild -t:LogBRCInstallDirectory -nologo).trim()
}

$assemblyDirectory = "$brcInstallDirectory\Bomb Rush Cyberfunk_Data\Managed"
$publicizedAssemblyDirectory = "$brcInstallDirectory\Bomb Rush Cyberfunk_Data\Managed\publicized_assemblies"

# Install publicizer
if (-not $(get-command assembly-publicizer)) {
    dotnet tool install -g BepInEx.AssemblyPublicizer.Cli
}

# Publicize assemblies
assembly-publicizer -o "$publicizedAssemblyDirectory\Assembly-CSharp_publicized.dll" "$assemblyDirectory\Assembly-CSharp.dll"
