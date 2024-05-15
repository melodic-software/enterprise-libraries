# Function to remove directories with a specific name.
function Remove-SpecialDirectories {
    param (
        [string]$RootPath,
        [string]$DirName
    )

    # Get all directories matching the name, including hidden ones.
    $Directories = Get-ChildItem -Path $RootPath -Directory -Recurse -Force | Where-Object { $_.Name -eq $DirName }

    foreach ($Dir in $Directories) {
        Write-Output "Deleting directory: $($Dir.FullName)"
        # Remove the directory and its contents
        Remove-Item -Path $Dir.FullName -Recurse -Force -ErrorAction SilentlyContinue
    }
}

# Starting path for the search (current directory).
$StartPath = Get-Location

# Delete .vs, bin, and obj directories.
Remove-SpecialDirectories -RootPath $StartPath -DirName ".vs"
Remove-SpecialDirectories -RootPath $StartPath -DirName "bin"
Remove-SpecialDirectories -RootPath $StartPath -DirName "obj"