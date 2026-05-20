<#
.SYNOPSIS
Creates a git commit with the provided message on the current branch.

.PARAMETER Message
The commit message describing the changes.

.EXAMPLE
.\create-commit.ps1 -Message "Add new feature X"
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$Message
)

$ErrorActionPreference = "Stop"

try {
    # Verify git is available
    $gitVersion = git --version 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Git is not installed or not in PATH."
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
        Write-Error "Cannot create commit on main branch. Please check out a feature branch."
        exit 1
    }

    # Verify there are changes to commit
    $gitStatus = git status --porcelain
    if ([string]::IsNullOrWhiteSpace($gitStatus)) {
        Write-Error "No changes to commit. Stage some changes first."
        exit 1
    }

    # Create the commit
    Write-Host "Creating commit on branch '$currentBranch'..."
    git commit -m $Message

    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to create commit."
        exit 1
    }

    # Get commit hash and summary
    $commitHash = git rev-parse --short HEAD
    $commitSummary = git log -1 --format=%s

    Write-Host "✓ Commit created successfully!"
    Write-Host "Commit: $commitHash - $commitSummary"
    Write-Host "Branch: $currentBranch"
    Write-Host ""
    Write-Host "Next step: Push to origin with 'git push' or use the create-pr skill"

    Write-Output $commitHash
}
catch {
    Write-Error "Error creating commit: $_"
    exit 1
}
