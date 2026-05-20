<#
.SYNOPSIS
Creates a draft pull request on GitHub using the provided title and description.

.PARAMETER Title
The title for the pull request.

.PARAMETER Description
The description/body for the pull request.

.EXAMPLE
.\create-draft-pr.ps1 -Title "Add new feature" -Description "This PR adds..."
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$Title,

    [Parameter(Mandatory = $true)]
    [string]$Description
)

$ErrorActionPreference = "Stop"

try {
    # Verify gh CLI is installed and accessible
    $ghVersion = gh --version 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Error "GitHub CLI (gh) is not installed or not in PATH. Please install it first."
        exit 1
    }

    # Verify we're authenticated
    $authStatus = gh auth status 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Not authenticated with GitHub CLI. Run 'gh auth login' first."
        exit 1
    }

    # Get current branch name
    $currentBranch = git rev-parse --abbrev-ref HEAD
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to get current branch name."
        exit 1
    }

    # Verify we're not on main branch
    if ($currentBranch -eq "main") {
        Write-Error "Cannot create PR from main branch. Please check out a feature branch."
        exit 1
    }

    # Create draft PR
    Write-Host "Creating draft PR on branch '$currentBranch'..."
    $prUrl = gh pr create --draft --title $Title --body $Description 2>&1

    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to create draft PR: $prUrl"
        exit 1
    }

    # Output the PR URL
    Write-Host "✓ Draft PR created successfully!"
    Write-Host "PR URL: $prUrl"
    Write-Output $prUrl
}
catch {
    Write-Error "Error creating draft PR: $_"
    exit 1
}
