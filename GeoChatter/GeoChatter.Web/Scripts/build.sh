#!/bin/bash

if ! command -v npm &> /dev/null
then
    echo "npm could not be found. It is required to install and build."
    read -p "Press enter to close the program..."
    exit
fi

echo ""
echo "Installing packages..."
echo ""
npm install --verbose

echo ""
echo "Running npm build..."
echo ""
npm run build --verbose

echo ""
echo "Compiling source files with tsc..."
echo ""
tsc --build --verbose

while true ; do 
    case "$1" in
        --no-prompt)
    echo ""
            echo "Exiting..."
            echo ""
            sleep 3s
            exit 0
            ;;
        *)
            read -p "Press enter to close the program..."
            echo ""
            exit 0
            ;;
    esac
done