git submodule add https://github.com/GeoChatter/GeoChatter.GeoGuessr.git

git submodule update --init
git submodule update --init --recursive

git submodule sync
git submodule update --remote
read -p "Press any key to exit..."