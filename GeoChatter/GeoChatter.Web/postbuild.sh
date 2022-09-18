echo ""
echo "Building scripts..."
echo ""
cd Scripts
sh ./build.sh --no-prompt
cd ..
echo ""
echo "Building styles..."
echo ""
cd Styles
sh ./build.sh --no-prompt
cd ..