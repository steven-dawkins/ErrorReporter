param (
    [string] $nuspec =  $(throw "nuspec is a required parameter"),
    [string] $baseVersion = $(throw "baseVersion is a required parameter"),
    [string] $branchName = $(throw "branchName is a required parameter"),
    [string] $releaseBranchName = $(throw "releaseBranchName is a required parameter")
)

function Edit-XmlNodes {
    param (
        [xml] $doc = $(throw "doc is a required parameter"),
        [string] $xpath = $(throw "xpath is a required parameter"),
        [string] $value = $(throw "value is a required parameter"),
        [bool] $condition = $true
    )    
    if ($condition -eq $true) {
        $nodes = $doc.SelectNodes($xpath)
         
        foreach ($node in $nodes) {
            if ($node -ne $null) {
                if ($node.NodeType -eq "Element") {
                    $node.InnerXml = $value
                }
                else {
                    $node.Value = $value
                }
            }
        }
    }
}

$safeBranchName = $branchName.replace("#","")
$safeBranchName = $safeBranchName.replace("/","-")
$safeBranchName = $safeBranchName -replace '-?[0-9]{4}-?[0-9]{2}-?[0-9]{2}$', '' # Conventionally we put the date at the end of branch name, remove it

$version = If ($branchName -eq $releaseBranchName) 
    { 
        $baseVersion 
    } 
    Else 
    {
        $baseVersion + "-" + $safeBranchName.subString(0, [System.Math]::Min(19, $safeBranchName.Length))
    }
    
$xml = [xml](Get-Content $nuspec)

Write-Host "Updating version in " + $nuspec + " to " + $version

Edit-XmlNodes $xml -xpath "/package/metadata/version" -value $version

$xml.Save((resolve-path $nuspec))