#!/usr/bin/env bash
# Determine OS
OS_TYPE="$(uname)"

# 1. Check if .NET 10 is already present
if command -v dotnet >/dev/null 2>&1; then
    if dotnet --list-sdks | grep -q "^10\."; then
        echo "SUCCESS: .NET 10 SDK is already installed."
        exit 0
    fi
fi

echo "SDK missing. Commencing automated install..."

# 2. Automated, non-interactive installation
if [ "$OS_TYPE" = "Darwin" ]; then
    # macOS: Use Homebrew if available, fallback to official script
    if command -v brew >/dev/null 2>&1; then
        brew install --cask dotnet-sdk
    else
        curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 10.0
    fi
elif [ "$OS_TYPE" = "Linux" ]; then
    # Linux: Fetch and pipe directly to the non-interactive dot.net script
    curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 10.0
else
    echo "ERROR: Unsupported OS environment for automated script execution."
    exit 1
fi

# 3. Export environment paths for the current session context
export DOTNET_ROOT="$HOME/.dotnet"
export PATH="$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools"

echo "SUCCESS: .NET 10 SDK successfully configured."
